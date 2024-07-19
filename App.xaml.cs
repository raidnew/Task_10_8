using System.Configuration;
using System.Data;
using System.Windows;
using Task.Interface;
using Task.Models;
using Task.ViewModel;

namespace Task;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    //private Dictionary<class, Window> Windows;

    private WndLogin _wndLogin;
    private WndPersonsList _wndPersonsList;
    private IPersonStorage _personStorage;
    public App()
    {
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        ShowLoginWindow();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _personStorage?.Save();
    }

    private void ShowLoginWindow()
    {
        (_wndLogin ?? (_wndLogin = new WndLogin())).Show();
        LoginVM.OnLogin += OnLogin;
    }

    private void OnLogin(Role role)
    {
        _personStorage = new ExtendedJsonPersonStorage<ExtendedPersonInStorage>();
        CurrentRole.Current = role;
        ShowPersonList();
        LoginVM.OnLogin -= OnLogin;
        _wndLogin.Close();
    }

    private void ShowPersonList()
    {
        (_wndPersonsList ?? (_wndPersonsList = new WndPersonsList())).Show();
        (_wndPersonsList.DataContext as PersonsListVM).SetModel(_personStorage);
    }


}