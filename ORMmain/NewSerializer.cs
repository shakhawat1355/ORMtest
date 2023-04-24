using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORMmain
{
    public class NewSerializer
    {
        public static Dictionary<string, object> Convert(object obj)
        {
            var result = new Dictionary<string, object>();
            Serialize(obj, result);
            return result;
        }

        private static void Serialize(object obj, Dictionary<string, object> dict)
        {
            Type objType = obj.GetType();
            PropertyInfo[] properties = objType.GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(obj);

                if (propertyValue == null)
                {
                    continue;
                }

                if (propertyValue is IList list)
                {
                    var listResult = new List<object>();

                    foreach (var item in list)
                    {
                        if (item == null)
                        {
                            continue;
                        }
                        if (item is string || item.GetType().IsValueType)
                        {
                            listResult.Add(item);
                        }
                        else
                        {
                            var nestedDict = new Dictionary<string, object>();
                            Serialize(item, nestedDict);
                            listResult.Add(nestedDict);
                        }
                    }
                    dict[property.Name] = listResult;
                }
                else if (propertyValue is string || propertyValue.GetType().IsValueType)
                {
                    dict[property.Name] = propertyValue;
                }
                else
                {
                    var nestedDict = new Dictionary<string, object>();
                    Serialize(propertyValue, nestedDict);
                    dict[property.Name] = nestedDict;
                }
            }
        }



        static void PrintNestedDict(Dictionary<string, object> dict, int indent = 0)
        {
            foreach (var pair in dict)
            {
                var key = pair.Key;
                var value = pair.Value;

                // Print the key with appropriate indentation
                Console.Write(new string(' ', indent));
                Console.Write(key + ": ");

                // If the value is a nested dictionary, recursively print it
                if (value is Dictionary<string, object> nestedDict)
                {
                    Console.WriteLine();
                    PrintNestedDict(nestedDict, indent + 2);
                }
                // Otherwise, print the value
                else
                {
                    Console.WriteLine(value);
                }
            }
        }


    }
}
