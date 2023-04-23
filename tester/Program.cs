
using System.Data.SqlClient;
using tester;

string connectionString = "Server=DESKTOP-8VMMQPN\\SQLEXPRESS;Database=bookzone;Trusted_Connection=True;Encrypt=False";
AdoTest db = new AdoTest(connectionString);

string commandText = "INSERT INTO Authors(Name, Address, ImgDir) VALUES (@Value1, @Value2, @Value3)";
SqlParameter[] parameters = new SqlParameter[]
{
    new SqlParameter("@Value1", "Hello"),
    new SqlParameter("@Value2", "World"),
        new SqlParameter("@Value3", "World")
};

db.ExecuteNonQuery(commandText, parameters);
