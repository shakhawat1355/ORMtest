using final;
using final.Entities;

f_DbContext context = new f_DbContext();

Course c1 = new Course { CourseName = "java", Fees = 9000 };

context.Add(c1);

context.SaveChanges();