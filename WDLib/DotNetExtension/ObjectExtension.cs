/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox//.DotNetExtension
{
    public static class ObjectExtension
    {
        private class ObjectWithPath
        {
            public object TheObject { get; protected set; }
            public string Path { get; protected set; }

            public ObjectWithPath(object theObject, string path)
            {
                this.TheObject = theObject;
                this.Path = path;
            }

        }

        public static bool EqualsAny<T>(this T what, params T[] items)
        {
            return items.Contains(what);
        }

        public static List<string> FindValues(this object theObject, long value)
        {
            List<string> results = new List<string>();

            Stack<ObjectWithPath> items = new Stack<ObjectWithPath>();
            List<object> visitedItems = new List<object>(); //to stop infinite recursion
            items.Push(new  ObjectWithPath(theObject, ""));

            while(items.Count > 0)
            {
                ObjectWithPath owp = items.Pop();
                object obj = owp.TheObject;
                string path = owp.Path;
                visitedItems.Add(obj);

                var fields = obj.GetType().GetFields();
                foreach (var field in fields)
                {
                    object objVal = field.GetValue(obj);
                    examineField(value,
                                results,
                                items, visitedItems,
                                obj, path,
                                objVal, field.Name);
                }

                var properties = obj.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.GetMethod.GetParameters().Length == 0) //don't examine indexors
                    {
                        try
                        {
                            object objVal = property.GetValue(obj);
                            examineField(value,
                                        results,
                                        items, visitedItems,
                                        obj, path,
                                        objVal, property.Name);
                        }
                        catch //(Exception ex)
                        {
                            
                        }
                    }
                }
            }

            return results;
        }

        private static void examineField(long value, 
                                        List<string> results, 
                                        Stack<ObjectWithPath> items, 
                                        List<object> visitedItems, 
                                        object obj, 
                                        string path, 
                                        object feildValue, 
                                        string fieldName)
        {
            if (feildValue == null)
            {
                return;
            }

            long? f = feildValue as long?;
            if ((f != null) && (value == f))
            {
                results.Add(string.Format("{0} => {1} = {2}", path, fieldName, f));
            }

            if (feildValue.GetType().IsClass && (!visitedItems.Contains(feildValue)))
            {
                if (!isSkippedType(feildValue.GetType()))
                {
                    items.Push(new ObjectWithPath(feildValue, path + @"\" + fieldName));
                }
            }
        }

        private static Type[] skippedTypes = new Type[] {
            typeof(Type), 
            typeof(MethodInfo),
            typeof(string),    
            };

        private static bool isSkippedType(Type type)
        {
            return skippedTypes.Contains(type);
        }
    }
}
