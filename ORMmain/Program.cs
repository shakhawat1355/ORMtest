
using ORMmain;
using ORMtest.Entities;

Course c1 = new Course();

//string s = JsonSerializer.Convert(c1);

//string s2 = Temp.GenerateInsertSql(c1, "course");

//Console.WriteLine(s);

//List<String> list = new List<String>();

//list = Temp.GetSqlStatements2(s2);

//foreach(String s in list)
//{
//    Console.WriteLine(s);
//}

//Dictionary<string, object> temp = new Dictionary<string, object>();

//temp = NewSerializer.Convert(c1);

AdoNetHelper.InsertObjectRecursively(c1, "Course");

//var temp2 = Temp.ExtractCourseData(c1);

//foreach (var x in temp2)
//{
//    Console.WriteLine(x);
//}