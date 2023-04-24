
using ORMmain;
using ORMtest.Entities;

Course c1 = new Course();

//string s = JsonSerializer.Convert(c1);

string s2 = Temp.GenerateInsertSql(c1, "course");

//Console.WriteLine(s2);

List<String> list = new List<String>();

list = Temp.GetSqlStatements2(s2);

foreach(String s in list)
{
    Console.WriteLine(s);
}