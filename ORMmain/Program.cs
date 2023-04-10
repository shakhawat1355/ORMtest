
using ORMmain;
using ORMtest.Entities;

Course c1 = new Course();

string s = JsonSerializer.Convert(c1);

//Console.WriteLine(s);