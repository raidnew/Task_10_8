using System.ComponentModel;
using System.Windows.Input;
using Task.Common;
using Task.Models;

namespace Task.ViewModel;

public class LoginVM : INotifyPropertyChanged
{
    public static Action<Role>? OnLogin;
    public event PropertyChangedEventHandler? PropertyChanged;

    private ICommand? _clickLogin;
    private string _login = "";
    private string _password = "";

    public string Login
    {
        get { return _login; }
        set
        {
            _login = value;
            OnPropertyChanged("Login");
        }
    }

    public string Password
    {
        get { return _password; }
        set
        {
            _password = value;
            OnPropertyChanged("Password");
        }
    }

    public Role CurrentRole { get; set; }

    public Role[] AllowRoles 
    { 
        get 
        { 
            return new Role[] { Role.Manager, Role.Consultant }; 
        }  
    }

    public ICommand ClickLogin
    {
        get
        {
            return _clickLogin ?? (_clickLogin = new CommandHandler(() => TryLogin(), () => CanTryLogin));
        }
    }

    public bool CanTryLogin
    {
        get
        {
            return Login.Length > 0 && Password.Length > 0;
        }
    }

    public LoginVM() { }

    private void TryLogin()
    {
        OnLogin?.Invoke(CurrentRole);
    }

    private void OnPropertyChanged(string prop)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}
