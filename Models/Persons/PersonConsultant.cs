namespace Task.Models;

public class PersonConsultant : PersonBase
{

    virtual public int ID { get { return _id; } set {} }
    virtual public string FirstName { get { return _firstName; } set {} }
    virtual public string LastName { get { return _lastName; } set {} }
    virtual public string ThirdName { get { return _thirdName; } set {} }
    virtual public string PhoneNumber { 
        get { return _phoneNumber; } 
        set 
        { 
            if(value.Length > 0) _phoneNumber = value; 
        } 
    }
    virtual public string PassportSeries { get { return "**********"; } set {} }
    virtual public string PassportNumber { get { return "**********"; } set {} }

    public PersonConsultant() : base() { }

    public PersonConsultant(int id) : base(id) { }

    public PersonConsultant(int id, string firstName, string lastName, string thirdName) : base(id, firstName, lastName, thirdName) { }

    public PersonConsultant(int id, string firstName, string lastName, string thirdName, string phoneNumber, string passportSeries, string passportNumber) :
        base(id, firstName, lastName, thirdName, phoneNumber, passportSeries, passportNumber) { }

    ~PersonConsultant() { }
}
