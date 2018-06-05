using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ReflectionExamples2.Model;

namespace ReflectionExamples2.Services {
    public class ReflectionService {

        private static Dictionary<Type, BigInteger> cachedFetchOptions = new Dictionary<Type, BigInteger>();
        private static HashSet<Type> stopper = new HashSet<Type>();
        private static Dictionary<Type, IList<Type>> backReferences = new Dictionary<Type, IList<Type>>();

        /// <summary>
        /// Returns all the fetch options of an object graph.
        /// </summary>
        private BigInteger GetFetchOptionsForObjectGraph(Type type, BigInteger fetchOptions){
            System.Diagnostics.Debug.WriteLine(string.Format("Entering GetFetchOptionsForObjectGraph for type {0}", type.Name));
            BigInteger retFetchOption = fetchOptions;
            if (cachedFetchOptions.ContainsKey(type)){
                // the type has already been cached. 
                retFetchOption = retFetchOption | cachedFetchOptions[type];
                return retFetchOption;
            }
            if (!stopper.Contains(type)){
                // this type has not been discovered, get the fetchoptions for this type
                foreach (var prop in type.GetProperties()){
                    var fetchOption = GetFetchOptionAttributes(prop);
                    if (fetchOption != FetchOptions.AsBigInteger(FetchOptions.None)){
                        System.Diagnostics.Debug.WriteLine("type {2}, property {0} has fetchoption {1}", prop.Name, FetchOptions.AsString(fetchOption), type.Name);
                        // found a reference property with fetchoption attribute
                        if ((retFetchOption & fetchOption) == BigInteger.Zero) 
                            retFetchOption = retFetchOption | fetchOption;
                        // stop circular ref
                        stopper.Add(type);
                        // get fetch options from this type
                        retFetchOption = GetFetchOptionsForObjectGraph(prop.PropertyType, retFetchOption);
                    }
                }
                // if the type is a base, then get derived types.
                if (type.IsAbstract){
                    // get derive types
                    IList<Type> derivedTypes;
                    if (backReferences.ContainsKey(type))
                        derivedTypes = backReferences[type];
                    else{
                        derivedTypes = type.Assembly.GetTypes().Where(t => t.IsSubclassOf(type)).ToList();
                        backReferences.Add(type, derivedTypes);
                    }
                    // get the fetch options for each derive type
                    foreach (var derivedType in derivedTypes) {
                        // backreferences have not been discovered
                        retFetchOption = GetFetchOptionsForObjectGraph(derivedType, retFetchOption);
                    }
                }
                // add to cache
                cachedFetchOptions.Add(type, retFetchOption);
                return retFetchOption;
            }
            // this type has not been discovered, get the fetchoptions for this type
            return retFetchOption;
        }

        /// <summary>
        /// Returns all the fetch options of an object graph.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public BigInteger GetFetchOptionsForObjectGraph(Type type){
            // clear stopper
            stopper.Clear();
            // return the fetchoptions of the type and its children.
            return GetFetchOptionsForObjectGraph(type, FetchOptions.AsBigInteger(FetchOptions.None));
        }

        public BigInteger GetFetchOptionAttributes(PropertyInfo property) {
            if (property == null)
                throw new ArgumentNullException("property");
            var attributes = property.GetCustomAttributes<FetchOptionAttribute>(true);
            if (attributes.Any()) {
                // this property has fetchoption attribute
                var enumarator = attributes.GetEnumerator();
                enumarator.MoveNext();
                return FetchOptions.AsBigInteger(enumarator.Current.FetchOption);
            }
            return FetchOptions.AsBigInteger(FetchOptions.None);
        }
    }
}
