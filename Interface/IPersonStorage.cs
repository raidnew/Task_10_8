namespace Task.Interface;

public interface IPersonStorage
{
    Action? OnPersonsLoad { get; set; }
    Action<IPerson>? OnPersonsSaved { get; set; }
    Action<IPerson>? OnPersonsAdded { get; set; }
    int GetCountPersons();
    IPerson? GetPersonByID(int ID);
    IPerson GetPersonByIndex(int index);
    List<IPerson> GetPersons(int startindex, int count);
    void Save();
    void SavePerson(IPerson person);
}