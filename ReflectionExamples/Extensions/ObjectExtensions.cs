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
    }
}
