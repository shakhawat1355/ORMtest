using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ORMmain
{
    public class Insert
    {
        private readonly SqlConnection _connection = new SqlConnection("Server=DESKTOP-86V0L02;Database=assignment_4;Trusted_Connection=True;");
        //public InsertIntoDatabase(SqlConnection connection)
        //{
        //    _connection = connection;
        //}
        public void InsertObjectIntoDb(object obj, string foreign_key, string refId)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            Type type = obj.GetType();

            string tableName = string.Empty;

            if (type.IsClass)
            {
                tableName = type.Name;
            }

            string FkColumn = $"{tableName.ToLower()}id";

            PropertyInfo[] properties = type.GetProperties();

            string? fkId = properties.FirstOrDefault(x => x.Name.ToLower() == "id")?.GetValue(obj).ToString();

            if (foreign_key != null && refId != null)
            {
                dict.Add(foreign_key, refId);
            }

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(obj);
                if (value == null) continue;
                if (value is string || value.GetType().IsValueType)
                {
                    dict.Add(property.Name, value);
                }
                else if (value is IList list)
                {
                    foreach (var item in list)
                    {
                        InsertObjectIntoDb(item, FkColumn, fkId);
                    }
                }
                else
                {
                    InsertObjectIntoDb(value, FkColumn, fkId);
                }
            }
            //Console.WriteLine("Current State of DICT:");
            //foreach(var item in dict)
            //{
            //    Console.WriteLine(item.Key +" "+ item.Value);
            //}
            GenerateInsertSql(tableName, dict);
        }

        public void GenerateInsertSql(string tableName, Dictionary<string, object> columnValues)
        {
            var columns = string.Join(", ", columnValues.Keys);

            var parameters = string.Join(", ", columnValues.Select(x => "@" + x.Key));

            //SqlConnection connection = new SqlConnection();
            //connection.ConnectionString = "Server=DESKTOP-86V0L02;Database=assignment_4;Trusted_Connection=True;";

            var query = new StringBuilder();

            query.Append($"INSERT INTO {tableName} ({columns}) VALUES ({parameters})");

            Console.WriteLine(query);

            using (SqlCommand cmd = new SqlCommand(query.ToString(), _connection))
            {
                foreach (var x in columnValues)
                {
                    cmd.Parameters.AddWithValue("@" + x.Key, x.Value);
                }
                try
                {
                    _connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (_connection.State != ConnectionState.Closed)
                    {
                        _connection.Close();
                    }
                }

            }

        }

    }
}
