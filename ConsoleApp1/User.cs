using System;
using System.Runtime.Intrinsics.X86;

namespace Example;
public interface IUser
{  
    string Identity {get; set; }
    string UserName { get; set; }
    string Password { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    bool IsActive { get; set; }
}
public interface IPersonal : IUser
{
    string SSN { get; set; }
    decimal Salary { get; set; }
    void CalculateSalary(short workingHours);
}

public interface IStudent : IUser
{
    ushort StudentNumber {get; set; }
    int Absenteeism { get; set; }
    byte Marks { get; set; }
}
public interface IJobber:IUser
{
    string LicenseNumber {get; set; } 
    string WorkArea { get; set; }
}
public abstract class User:IUser
{
    public string Identity {get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public User() { }
    public User(string userName, string password,bool isActive, string identity) {
        UserName = userName;
        Password = password;
        IsActive = isActive;
        Identity = identity;
    }

}

public class Personal : User, IPersonal
{
    public string SSN { get; set; }
    public decimal Salary { get; set; }
    public Personal() { }
    public Personal(string userName, string password, bool isActive, string ssn, decimal salary, string identity) : base(userName, password, isActive,identity)
    {
        SSN = ssn;
        Salary = salary;
    }
    public void CalculateSalary(short workingHours)
    {
        Console.WriteLine(@$"Calculated salary: {Salary * workingHours} ₺");
    }
}

public class Student:User,IStudent {
    public ushort StudentNumber {get; set; }
    public int Absenteeism { get; set; }
    public byte Marks { get; set; }
    public Student() { }
    public Student(string userName, string password, bool isActive, int absenteeism, byte marks,string identity, ushort studentNumber) : base(userName, password, isActive,identity)
    {
        StudentNumber = studentNumber;
        Absenteeism = absenteeism;
        Marks = marks;
    }
}
public class Jobber:User,IJobber
{
    public string LicenseNumber {get; set; }
    public string WorkArea { get; set; }
    public Jobber() { }
    public Jobber(string userName, string password, bool isActive, string workArea, string identity, string licenseNumber) : base(userName, password, isActive,identity)
    {
        WorkArea = workArea;
        LicenseNumber = licenseNumber;
    }
}
public enum UserTypeEnum
{
    Personal,
    Student,
    Jobber
}
public static class UserFactory
{
    public static IUser GetInstance(UserTypeEnum userType)
    {
        if(userType==UserTypeEnum.Personal)
            return new Personal();
        if (userType == UserTypeEnum.Student)
            return new Student();
        return new Jobber();
    }
}
