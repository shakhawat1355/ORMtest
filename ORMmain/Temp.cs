using ORMtest.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

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





        public static Dictionary<string, List<Tuple<string, object>>> ExtractCourseData(object course)
        {
            Dictionary<string, List<Tuple<string, object>>> dict = new Dictionary<string, List<Tuple<string, object>>>();
            ExtractObjectData(course, dict);
            return dict;
        }

        private static void ExtractObjectData(object obj, Dictionary<string, List<Tuple<string, object>>> dict)
        {
            Type objType = obj.GetType();
            PropertyInfo[] props = objType.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(obj);
                if (propValue == null) continue;

                Type propType = propValue.GetType();

                if (propType.IsClass && propType != typeof(string))
                {
                    ExtractObjectData(propValue, dict);
                }
                else if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var list = (IEnumerable)propValue;
                    foreach (var item in list)
                    {
                        ExtractObjectData(item, dict);
                    }
                }
                else
                {
                    string propName = prop.Name;
                    string className = objType.Name;

                    if (!dict.ContainsKey(className))
                    {
                        dict[className] = new List<Tuple<string, object>>();
                    }

                    dict[className].Add(new Tuple<string, object>(propName, propValue));
                }


            }
        }





    }




    public static class AdoNetHelper
    {
        private static readonly string connectionString = "your_connection_string_here";

        public static void InsertObjectRecursively(object obj, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.Transaction = transaction;

                        // Get the properties of the object
                        PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        // Build the INSERT statement for this object
                        List<string> columnNames = new List<string>();
                        List<string> values = new List<string>();
                        foreach (PropertyInfo property in properties)
                        {
                            // Skip any properties that are not primitive types or strings
                            if (!IsPrimitiveOrString(property.PropertyType)) continue;

                            columnNames.Add(property.Name);
                            values.Add(GetSqlValue(property.GetValue(obj)));
                        }

                        string sql = $"INSERT INTO {tableName} ({string.Join(",", columnNames)}) VALUES ({string.Join(",", values)})";
                        command.CommandText = sql;
                        command.ExecuteNonQuery();

                        // Recursively insert any complex properties
                        foreach (PropertyInfo property in properties)
                        {
                            // Skip any properties that are not complex types
                            if (IsPrimitiveOrString(property.PropertyType) || IsEnumerable(property.PropertyType)) continue;

                            object value = property.GetValue(obj);
                            if (value != null)
                            {
                                InsertObjectRecursively(value, property.Name);
                            }
                        }

                        // Recursively insert any items in enumerable properties
                        foreach (PropertyInfo property in properties)
                        {
                            // Skip any properties that are not enumerable types
                            if (!IsEnumerable(property.PropertyType)) continue;

                            IEnumerable collection = property.GetValue(obj) as IEnumerable;
                            if (collection != null)
                            {
                                foreach (object item in collection)
                                {
                                    InsertObjectRecursively(item, property.Name);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private static bool IsPrimitiveOrString(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }

        private static bool IsEnumerable(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static string GetSqlValue(object value)
        {
            if (value == null) return "NULL";
            if (value.GetType() == typeof(DateTime)) return $"'{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff")}'";
            if (value.GetType() == typeof(bool)) return ((bool)value) ? "1" : "0";
            if (value.GetType() == typeof(string)) return $"'{value.ToString().Replace("'", "''")}'";
            if (value.GetType() == typeof(decimal)) return ((decimal)value).ToString("0.#########");
            return value.ToString();
        }
    }









}


