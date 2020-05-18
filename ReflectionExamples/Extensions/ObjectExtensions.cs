using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionExamples2.Extensions {
    public static class ObjectExtensions {
        /// <summary>
        /// dynamically casts an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetObjectAs<T>(this Object obj) where T : class {
            return Convert.ChangeType(obj, typeof(T)) as T;
        }

        /// <summary>
        /// returns the hashcode of an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetHashCodeValue(this Object obj) {
            var hashString = "";
            foreach (var prop in obj.GetType().GetProperties()) {
                var value = prop.GetValue(obj, null);
                if (value != null) {
                    hashString += value + "|";
                }
            }
            return hashString.GetHashCode();
        }

        /// <summary>
        /// /bookstore/book[price>35]/title
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="keys"></param>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static StringBuilder GetObjectPath(this Object obj, List<KeyValuePair<string, string>> keys, string parent) {
            var nameParts = obj.GetType().FullName.Split('.');
            var typeName = nameParts[nameParts.Length - 1];
            var pathPart = new StringBuilder();
            pathPart.Append("/").Append(typeName).Append("[");
            var objKeys = keys.Where(x => x.Key.ToLower() == typeName.ToLower());
            int i = 0;
            foreach (var key in objKeys) {
                var propValue = obj.GetType().GetProperty(key.Value).GetValue(obj);
                if (i > 0)
                    pathPart.Append("&");
                pathPart.Append(key.Value).Append("=").Append(propValue);
                i++;
            }
            pathPart.Append("]");
            var parentInstance = obj.GetType().GetProperty(parent).GetValue(obj);
            if (parentInstance == null) {
                return pathPart;
            } else {
                var parentPathPart = GetObjectPath(parentInstance, keys, parent);
                return parentPathPart.Append(pathPart);
            }
        }

        /// <summary>
        /// updates an an object from another. only sets the values that are set in the source object and are not null.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        /// <param name="ignoreList"></param>
        public static void UpdateFrom(this Object obj, Object source, IList<string> ignoreTypeList, IList<string> ignorePropertyList) {
            if (source != null) {
                System.Diagnostics.Debug.WriteLine("Updating type " + source.GetType().FullName + ".");
                foreach (var prop in source.GetType().GetProperties()) {
                    var value = prop.GetValue(source);
                    if (value != null) {
                        if (value.GetType().IsValueType || value.GetType().FullName == "System.String") {
                            // a value type
                            if (!IsReadOnly(prop)) {
                                if (!IsIgnored(ignoreTypeList, ignorePropertyList, prop)) {
                                    // is not ignored
                                    System.Diagnostics.Debug.WriteLine("Setting value property " + prop.Name + " / value: " + value);
                                    obj.GetType().GetProperty(prop.Name).SetValue(obj, value);
                                } else {
                                    // property is ignored
                                    System.Diagnostics.Debug.WriteLine("value property " + prop.Name + " / with value: " + value + " has been ignored.");
                                }
                            } else {
                                System.Diagnostics.Debug.WriteLine("Setting value property " + prop.Name + " BUT IS READONLY ");
                            }
                        } else {
                            // a reference type
                            if (!IsIgnored(ignoreTypeList, ignorePropertyList, prop)) {
                                obj.GetType().GetProperty(prop.Name).GetValue(obj).UpdateFrom(source.GetType().GetProperty(prop.Name).GetValue(source), ignoreTypeList, ignorePropertyList);
                            }
                        }
                    }
                }
            } else {
                System.Diagnostics.Debug.WriteLine("Updating type " + obj.GetType().FullName + ". BUT SOURCE IS NULL");
            }
        }

        /// <summary>
        /// returns true if item should be ignored.
        /// </summary>
        /// <param name="ignoreList"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static bool IsIgnored(IList<string> ignoreTypeList, IList<string> ignorePropertyList, PropertyInfo prop) {
            bool retVal = false;
            if (ignoreTypeList != null) {
                // check type ignore list
                var ignoredItem = ignoreTypeList.Where(i => prop.PropertyType.FullName.StartsWith(i)).FirstOrDefault();
                retVal = ignoredItem != null;
            }
            if (ignorePropertyList != null && !retVal) {
                // check property ignore list since not in ignore type list
                var ignoredItem = ignorePropertyList.Where(i => prop.Name.StartsWith(i)).FirstOrDefault();
                retVal = ignoredItem != null;
            }
            return retVal;
        }

        /// <summary>
        /// returns true if is read only
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static bool IsReadOnly(PropertyInfo prop) {
            ReadOnlyAttribute attrib = Attribute.GetCustomAttribute(prop, typeof(ReadOnlyAttribute)) as ReadOnlyAttribute;
            bool readOnly = !prop.CanWrite || (attrib != null && attrib.IsReadOnly);
            return readOnly;
        }
    }
}
