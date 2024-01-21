using Example;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

var personalUsers = JsonConvert.DeserializeObject<IList<Personal>>(Datas.PersonDataJson);
var studentUsers = JsonConvert.DeserializeObject<IList<Student>>(Datas.StudentDataJson) ;
var jobberUsers = JsonConvert.DeserializeObject<IList<Jobber>>(Datas.JobberDataJson);
IDictionary<string,IList<string>> indexes =new Dictionary<string,IList<string>>();
IDictionary<string, IUser> fastList = new Dictionary<string, IUser>();

fastList.AddToDictionary(personalUsers.Select(user=>(user as IUser)).ToList(),indexes);
fastList.AddToDictionary(studentUsers.Select(user=>(user as IUser)).ToList(),indexes);
fastList.AddToDictionary(jobberUsers.Select(user=>(user as IUser)).ToList(),indexes);

var findAllByFirstName = FindByIndex("Deeyn");
Console.WriteLine(JsonConvert.SerializeObject(findAllByFirstName));
var findAllByLasttName = FindByIndex("Fairbourn");
Console.WriteLine(JsonConvert.SerializeObject(findAllByLasttName));
var findAllByUserName = FindByIndex("dfairbourn0");
Console.WriteLine(JsonConvert.SerializeObject(findAllByUserName));
var findAllByIdentity = FindByIndex("12432423");
Console.WriteLine(JsonConvert.SerializeObject(findAllByIdentity));
var findAllBySSN = FindByIndex("644-64-2957");
Console.WriteLine(JsonConvert.SerializeObject(findAllBySSN));
var findAllByStudentNumber = FindByIndex("345");
Console.WriteLine(JsonConvert.SerializeObject(findAllByStudentNumber));
var findAllByAbsenteeism = FindByIndex("14");
Console.WriteLine(JsonConvert.SerializeObject(findAllByAbsenteeism));
var findAllByMarks = FindByIndex("83");
Console.WriteLine(JsonConvert.SerializeObject(findAllByMarks));
var findAllByLicenseNumber = FindByIndex("1223");
Console.WriteLine(JsonConvert.SerializeObject(findAllByLicenseNumber));
var findAllByWorkArea = FindByIndex("Perugia San Francesco d'Assisi Umbria International Airport");
Console.WriteLine(JsonConvert.SerializeObject(findAllByWorkArea));


Console.ReadKey();

IList<IUser>? FindByIndex(string search)
{
    if (indexes.ContainsKey(search))
    {
        var findedKeys=indexes[search];
        return findedKeys.Select(key => fastList[key]).ToList();
    }
    return null;
}
public static class MicrosoftExtensions
{

    public static void AddToDictionary<TKey,TValue>(
        this IDictionary<TKey, TValue> values, 
        IList<TValue> users,
        IDictionary<TKey,IList<TKey>> indexes
        )
        where TValue: IUser
        where TKey: notnull
    {
        TKey castToKey(object key)
        {
            return (TKey)key;
        };
        void addIndex(object findKeyObject, TKey dataKey)
        {
            TKey findKey= castToKey(findKeyObject);
            if (indexes.ContainsKey(findKey))
            {
                indexes[findKey].Add(dataKey);
            }
            else
            {
                indexes.Add(findKey, new List<TKey>() { dataKey });
            }
        };
        users?.ToList().ForEach(user =>
        {
            TKey key = castToKey(user.UserName);
            values.Add(key, user);
            addIndex(user.FirstName, key);
            addIndex(user.LastName, key);
            addIndex(user.Identity, key);
            addIndex(user.Password, key);
            var personal = user.CastTo<IPersonal>();
            if (personal!=null){
                addIndex(personal.SSN, key);                
            }
            var student = user.CastTo<IStudent>();
            if (student!=null){
                addIndex(student.Absenteeism.ToString(), key);
                addIndex(student.Marks.ToString(), key);
                addIndex(student.StudentNumber.ToString(), key);
            }
            var jobber = user.CastTo<IJobber>();
            if (jobber!=null){
                addIndex(jobber.LicenseNumber, key);
                addIndex(jobber.WorkArea, key);
            }
        });
    }
    public static TObject? CastTo<TObject>(this object value)
        where TObject: class
    {
        if (value is TObject)
        {
            return value as TObject;
        }
        return null;
    }
}