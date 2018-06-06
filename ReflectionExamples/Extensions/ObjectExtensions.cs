using System;
using System.Collections.Generic;
using System.Linq;
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
        public static int GetHashCodeValue(this Object obj){
            var hashString = "";
            foreach (var prop in obj.GetType().GetProperties()){
                var value = prop.GetValue(obj, null);
                if (value != null){
                    hashString += value + "|";
                }
            }
            return hashString.GetHashCode();
        }

        public static StringBuilder GetObjectPath(this Object obj, string[] keys, string parent, StringBuilder path) {
            if (path == null) {
                path = new StringBuilder();
            }
            var nameParts = obj.GetType().FullName.Split('.');
            var typeName = nameParts[nameParts.Length - 1];
            var entityProperty = obj.GetType().GetProperty(parent);
            var pathPart = new StringBuilder();
            pathPart.Append("/").Append(typeName).Append("[");
            foreach (var key in keys) {
                var propValue = obj.GetType().GetProperty(key).GetValue(obj);
                pathPart.Append(key).Append("=").Append(propValue);
            }
            pathPart.Append("]");
            if (entityProperty == null) {
                return pathPart.Append(path);
            } else {
                return GetObjectPath(obj, keys, parent, pathPart.Append(path));
            }
        }
    }
}
