using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ORMmain
{
    public class Temp
    {
        public static string GenerateInsertSql<T>(T obj, string tableName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"INSERT INTO {tableName} (");

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            List<string> columns = new List<string>();
            List<string> values = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // If the property is a List<T>, recursively generate INSERT statements for each item in the list
                    dynamic list = property.GetValue(obj);
                    foreach (dynamic item in list)
                    {
                        string nestedTableName = $"{tableName}_{property.Name}";
                        sb.Append(GenerateInsertSql(item, nestedTableName));
                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    // If the property is another class, recursively generate an INSERT statement for the nested object
                    dynamic nestedObj = property.GetValue(obj);
                    string nestedTableName = $"{tableName}_{property.Name}";
                    sb.Append(GenerateInsertSql(nestedObj, nestedTableName));
                }
                else
                {
                    // For all other properties, add the column name and value to the INSERT statement
                    string columnName = property.Name;
                    object columnValue = property.GetValue(obj);

                    columns.Add(columnName);
                    values.Add($"'{columnValue}'");
                }
            }

            sb.Append(string.Join(",", columns));
            sb.Append(") VALUES (");
            sb.Append(string.Join(",", values));
            sb.Append(");");

            return sb.ToString();
        }



        public static List<string> ExtractSqlStatements(string input)
        {
            // Define a regular expression to match only proper SQL statements
            Regex sqlRegex = new Regex(@"\b(INSERT INTO|UPDATE|DELETE FROM|SELECT|CREATE|ALTER|DROP)\b", RegexOptions.IgnoreCase);

            // Split the input string into individual statements using semicolons as the delimiter
            string[] statements = input.Split(';');

            // Loop through each statement and only keep those that match the SQL regex
            List<string> sqlStatements = new List<string>();
            foreach (string statement in statements)
            {
                if (sqlRegex.IsMatch(statement))
                {
                    sqlStatements.Add(statement.Trim());
                }
            }

            // Return the list of valid SQL statements
            return sqlStatements;
        }


        public static List<string> GetSqlStatements(string sqlString)
        {
            List<string> sqlStatements = new List<string>();
            int index = 0;
            int startIndex = sqlString.IndexOf("INSERT", index);
            int endIndex = sqlString.IndexOf(";", startIndex);
            while (startIndex >= 0 && endIndex >= 0)
            {
                string sqlStatement = sqlString.Substring(startIndex, endIndex - startIndex + 1);
                sqlStatements.Add(sqlStatement);
                index = endIndex + 1;
                startIndex = sqlString.IndexOf("INSERT", index);
                endIndex = sqlString.IndexOf(";", startIndex);
            }
            return sqlStatements;
        }


        public static List<string> GetSqlStatements2(string input)
        {
            List<string> sqlStatements = new List<string>();
            int startIndex = 0;

            while (true)
            {
                int insertIndex = input.IndexOf("INSERT", startIndex);
                int semicolonIndex = input.IndexOf(";", startIndex);

                if (insertIndex == -1 || semicolonIndex == -1)
                {
                    break;
                }

                if (insertIndex < semicolonIndex)
                {
                    string sqlStatement = input.Substring(insertIndex, semicolonIndex - insertIndex + 1);

                    if (sqlStatement.StartsWith("INSERT"))
                    {
                        sqlStatements.Add(sqlStatement);
                    }

                    startIndex = semicolonIndex + 1;
                }
                else
                {
                    startIndex = insertIndex + 1;
                }
            }

            return sqlStatements;
        }



    }
}
