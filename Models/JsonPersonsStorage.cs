using System.Text.Json;
using Task.Interface;
using Task.Common;
using System.Collections.ObjectModel;

namespace Task.Models;

public class JsonPersonsStorage<T> : IPersonStorage
{
    public Action? OnPersonsLoad { get; set; }
    public Action<IPerson>? OnPersonsSaved { get; set; }
    public Action<IPerson>? OnPersonsAdded { get; set; }

    protected int _lastPersonId = 0;
    protected ObservableCollection<IPerson> _personsList;
    private FileStorage _fileStorage;

    public bool IsReady { get; private set; }

    public JsonPersonsStorage()
    {
        IsReady = false;
        _personsList = new ObservableCollection<IPerson>();
        _fileStorage = new FileStorage("jsonstorage.json");
        _fileStorage.OnFileLoaded += OnDataLoad;
        _fileStorage.LoadFile();
    }

    public void SavePerson(IPerson person)
    {
        if (person.ID >= 0)
            ModifyPerson(person);
        else
            if (RoleAccess.AddAvailable())
                CreateNewPerson(person);
    }

    public int GetCountPersons()
    {
        return _personsList.Count;
    }

    public IPerson? GetPersonByID(int ID)
    {
        return _personsList.Single(person => person.ID == ID);
    }

    public IPerson GetPersonByIndex(int index)
    {
        return CreatePerson(_personsList[index]);
    }
    public List<IPerson> GetPersons(int startindex = 0, int count = -1)
    {
        List<IPerson> persons = new List<IPerson>();
        int personsCount = GetCountPersons();
        startindex = Math.Max(startindex, 0);

        if (count <= 0) 
            count = personsCount;

        for (int  i = startindex; i < personsCount || i < startindex + count; i++)
            persons.Add(CreatePerson(_personsList[i]));

        return persons;
    }

    public void Save()
    {
        List<string> serializedPersons = new List<string>();
        foreach (IPerson person in _personsList)
        {
            serializedPersons.Add(Serialize((T)person));
        }
        _fileStorage.SaveFile(serializedPersons);
    }
    virtual protected void ModifyPerson(IPerson person)
    {
        PersonInStorage foundPerson = GetPersonByID(person.ID) as PersonInStorage;
        if (foundPerson == null) return;
        foundPerson.Clone(person);
        OnPersonsSaved?.Invoke(person);
    }

    virtual protected void CreateNewPerson(IPerson personData)
    {
        PersonInStorage newPerson = new PersonInStorage(_lastPersonId++,
            personData.FirstName,
            personData.LastName,
            personData.ThirdName,
            personData.PhoneNumber,
            personData.PassportSeries,
            personData.PassportNumber);
        _personsList.Add(newPerson);
        OnPersonsAdded?.Invoke(newPerson);
    }

    virtual protected IPerson CreatePerson(IPerson person)
    {
        IPerson retPerson = null;
        switch (CurrentRole.Current)
        {
            case Role.Manager:
                retPerson = new PersonManager(person.ID, person.FirstName, person.LastName, person.ThirdName, person.PhoneNumber, person.PassportSeries, person.PassportNumber);
                break;
            case Role.Consultant:
            default:
                retPerson = new PersonConsultant(person.ID, person.FirstName, person.LastName, person.ThirdName, person.PhoneNumber, person.PassportSeries, person.PassportNumber);
                break;
        }
        return retPerson;
    }

    virtual protected string Serialize(T person)
    {
        return JsonSerializer.Serialize(person);
    }

    virtual protected T? Deserialize(string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    private void InitLastID()
    {
        if (_personsList != null && _personsList.Count > 0)
            _lastPersonId = _personsList.Last().ID + 1;
    }

    private void OnDataLoad(List<string> serializedPersons)
    {
        foreach (string jsonPerson in serializedPersons)
        {
            IPerson person = (IPerson)Deserialize(jsonPerson);
            if(person != null) _personsList.Add(person);
        }
        InitLastID();
        IsReady = true;
        OnPersonsLoad?.Invoke();
    }

}