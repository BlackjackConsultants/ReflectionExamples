using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	            if (i>0) 
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
    }
}
