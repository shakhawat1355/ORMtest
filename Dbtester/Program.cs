using Dbtester;

t1_dbContext context = new t1_dbContext();



Test1 c1 = new Test1 { Title = "java", Fees = 9000 };

context.Add(c1);

context.SaveChanges();