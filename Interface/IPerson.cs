﻿namespace Task.Interface;

public interface IPerson
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ThirdName { get; set; }
    public string PhoneNumber { get; set; }
    public string PassportSeries { get; set; }
    public string PassportNumber { get; set; }
}
