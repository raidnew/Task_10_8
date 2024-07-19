using System.ComponentModel;
using System.Windows.Input;
using Task.Common;
using Task.Interface;
using Task.Models;

namespace Task.ViewModel;

public class PersonsListVM : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private List<IPerson>? _persons;
    private ICommand? _addPerson;
    private ICommand? _editPerson;
    private IPersonStorage _model;
    private WndEditPerson wndEditPerson;
    private int _selectedIndex;

    public int SelectedIndex
    {
        get { return _selectedIndex; }
        set
        {
            _selectedIndex = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public PersonsListVM() 
    {
        Persons = new List<IPerson>();
    }

    public void SetModel(IPersonStorage model)
    {
        _model = model;
        UpdatePersonsList();
    }

    public List<IPerson>? Persons
    {
        get { return _persons; }
        set { 
            _persons = value;
            OnPropertyChanged("Persons");
        }
    }
    public IPerson SelectedPerson { get; set; }

    public ICommand ClickEditPerson
    {
        get
        {
            return _editPerson ?? (_editPerson = new CommandHandler(() => EditPerson(), () => CanEditPerson));
        }
    }
    public ICommand ClickAddPerson
    {
        get
        {
            return _addPerson ?? (_addPerson = new CommandHandler(() => AddPerson(), () => CanAddPerson));
        }
    }

    private bool CanEditPerson
    {
        get
        {
            return SelectedIndex >= 0 && SelectedIndex <= _persons.Count;
        }
    }

    private bool CanAddPerson
    {
        get
        {
            return RoleAccess.AddAvailable();
        }
    }

    private void UpdatePersonsList()
    {
        Persons = _model.GetPersons(0, -1);
    }

    private void EditPerson()
    {
        ShowPersonEditWnd(_persons[SelectedIndex]);
    }

    private void AddPerson()
    {
        ShowPersonEditWnd(new ExtendedPersonInStorage());
    }

    private void ShowPersonEditWnd(IPerson person)
    {
        wndEditPerson = new WndEditPerson();
        (wndEditPerson.DataContext as EditPersonVM).Person = person;
        (wndEditPerson.DataContext as EditPersonVM).OnPersonHasEdited += OnPersonChange;
        (wndEditPerson.DataContext as EditPersonVM).OnPersonEditCanceled += OnPersonEditCancel;
        wndEditPerson.Show();
    }

    private void OnPersonEditCancel()
    {
        wndEditPerson.Close();
    }

    private void OnPersonChange(IPerson person)
    {
        _model.SavePerson(person);
        UpdatePersonsList();
        wndEditPerson.Close();
    }

    private void OnPropertyChanged(string prop)
    {
        if(PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

}
