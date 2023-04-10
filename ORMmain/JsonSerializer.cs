using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORMmain
{
    public class JsonSerializer
    {

        public static string Convert(object obj)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            Serializer(obj, jsonBuilder);
            return jsonBuilder.ToString();
        }

        public static void Serializer(object obj, StringBuilder jsonBuilder)
        {
            Type objType = obj.GetType();

           


            jsonBuilder.Append("{");

            bool InitialElemenet = true;
            PropertyInfo[] properties = objType.GetProperties();




            foreach (var property in properties)

            {
                Console.WriteLine(property);

                //var propertyValue = property.GetValue(obj);

                //

                //if (propertyValue == null)
                //{
                //    continue;
                //}

                //if (!InitialElemenet) jsonBuilder.Append(",");


                //InitialElemenet = false;

                //jsonBuilder.Append("\"");
                //jsonBuilder.Append(property.Name);
                //jsonBuilder.Append("\":");

                //if (propertyValue is IList list) ListSerializer(list, jsonBuilder);

                //else if (propertyValue is string)
                //{
                //    jsonBuilder.Append("\"");
                //    jsonBuilder.Append(propertyValue.ToString());
                //    jsonBuilder.Append("\"");
                //}

                //else if (propertyValue.GetType().IsValueType)
                //{
                //    jsonBuilder.Append("\"");
                //    jsonBuilder.Append(propertyValue.ToString());
                //    jsonBuilder.Append("\"");
                //}


                //else
                //{
                //    Serializer(propertyValue, jsonBuilder);
                //}
            }

            jsonBuilder.Append("}");
        }

        public static void ListSerializer(IList list, StringBuilder jsonBuilder)
        {
            jsonBuilder.Append("[");

            bool InitialElemenet = true;
            foreach (var item in list)
            {
                if (!InitialElemenet)
                {
                    jsonBuilder.Append(",");
                }
                InitialElemenet = false;

                if (item is string)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(item.ToString());
                    jsonBuilder.Append("\"");
                }

                else if (item.GetType().IsValueType)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(item.ToString());
                    jsonBuilder.Append("\"");
                }

                else
                {
                    Serializer(item, jsonBuilder);
                }
            }

            jsonBuilder.Append("]");
        }



    

}
}
