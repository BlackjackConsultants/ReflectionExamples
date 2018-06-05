using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples2.Model;
using ReflectionExamples2.Services;

namespace ReflectionExamples {
	[TestClass]
    public class AttributeExamples {
        private static HashSet<Type> stopper = new HashSet<Type>();

		[TestMethod]
		public void GetValueExample() {
            var contact = new Contact();
            var methods = contact.GetType().GetMethods();
            foreach (var methodInfo in methods) {
                foreach (var attrib in methodInfo.GetCustomAttributes(true)) {
                    System.Diagnostics.Debug.WriteLine("methodInfo: " + methodInfo.Name + ", attribute: " + attrib.GetType().ToString());
                }
            }
		}

        [TestMethod]
        public void GetSpecificCustomAttributeFromClass() {
            Individual individual = new Individual() { FileAs = "Mr. Jorge Perez", FirstName = "jorge", LastName = "Perez", Address = new Address() { ZipCode = "33333" } };
            foreach (var prop in individual.GetType().GetProperties()) {
                System.Diagnostics.Debug.WriteLine("Attribues of {0}", prop.Name);
                var fetchOption = GetFetchOptionAttributes(prop);
                if (fetchOption != FetchOptions.AsBigInteger(FetchOptions.None)){
                    System.Diagnostics.Debug.WriteLine("Attribute name: {0}", fetchOption);
                }
            }
	    }

        [TestMethod]
        public void GetSpecificCustomAttributeFromObjectGraph() {
            ReflectionService reflectionService = new ReflectionService();
            // retrive fetch options
            var fetchOptions = reflectionService.GetFetchOptionsForObjectGraph(typeof(Individual));
            // check that all properties of the object graph are tested
            AssertFetchOptionsAreSet(typeof(Individual), fetchOptions);
            // make sure that Domain is not set cuz is not part of this graph
            AssertFetchOptionsAreNotSet(typeof(Individual), FetchOptions.AsBigInteger("1"));
        }


        [TestMethod]
        public void TestStopper() {
            ReflectionService reflectionService = new ReflectionService();
            // retrive fetch options
            var fetchOptions = reflectionService.GetFetchOptionsForObjectGraph(typeof(A));
            // check that all properties of the object graph are in the return value (fetchOptions)
            Assert.IsTrue((fetchOptions & FetchOptions.AsBigInteger("4")) != BigInteger.Zero);
            Assert.IsTrue((fetchOptions & FetchOptions.AsBigInteger("2")) != BigInteger.Zero);
            Assert.IsTrue(true, "If execution gets to this point then no stackoverflow occured");
            System.Diagnostics.Debug.WriteLine(fetchOptions);
        }

        [TestMethod]
        public void GetSpecificCustomAttributeFromObjectGraphTestBackReference() {
            ReflectionService reflectionService = new ReflectionService();
            // retrive fetch options
            var fetchOptions = reflectionService.GetFetchOptionsForObjectGraph(typeof(A));
            // check that all properties of the object graph are in the return value (fetchOptions)
            Assert.IsTrue((fetchOptions & FetchOptions.AsBigInteger("10")) != BigInteger.Zero);
            Assert.IsTrue((fetchOptions & FetchOptions.AsBigInteger("08")) != BigInteger.Zero);
            Assert.IsTrue(true, "If execution gets to this point then no stackoverflow occured");
            System.Diagnostics.Debug.WriteLine(fetchOptions);
        }

	    /// <summary>
        /// pass the type and the fetchoptions created to see if for all types with a fetchoption, the fetchoption has been set.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fetchOptions"></param>
	    private void AssertFetchOptionsAreSet(Type type, BigInteger fetchOptions){
            System.Diagnostics.Debug.WriteLine(string.Format("Entering AssertTypeGraph for type {0}", type.Name));
            // this type has not been discovered, get the fetchoptions for this type
            foreach (var prop in type.GetProperties()) {
                // get property attribute
                var fetchOption = GetFetchOptionAttributes(prop);
                if (fetchOption != FetchOptions.AsBigInteger(FetchOptions.None)) {
                    // attribute exists
                    System.Diagnostics.Debug.WriteLine("type {2}, property {0} has fetchoption {1}", prop.Name, FetchOptions.AsString(fetchOption), type.Name);
                    // found a reference property with fetchoption attribute
                    Assert.IsTrue((fetchOptions & fetchOption) != BigInteger.Zero);
                    // get fetch options from this type
                    AssertFetchOptionsAreSet(prop.PropertyType, fetchOptions);
                }
            }
            // add to cache
            stopper.Add(type);
            ////return fetchOptions;
	    }

        private void AssertFetchOptionsAreNotSet(Type type, BigInteger fetchOptions) {
            System.Diagnostics.Debug.WriteLine(string.Format("Entering AssertTypeGraph for type {0}", type.Name));
            // this type has not been discovered, get the fetchoptions for this type
            foreach (var prop in type.GetProperties()) {
                // get property attribute
                var fetchOption = GetFetchOptionAttributes(prop);
                if (fetchOption != FetchOptions.AsBigInteger(FetchOptions.None)) {
                    // attribute exists
                    System.Diagnostics.Debug.WriteLine("type {2}, property {0} has fetchoption {1}", prop.Name, FetchOptions.AsString(fetchOption), type.Name);
                    // found a reference property with fetchoption attribute
                    Assert.IsFalse((fetchOptions & fetchOption) != BigInteger.Zero);
                    // get fetch options from this type
                    AssertFetchOptionsAreNotSet(prop.PropertyType, fetchOptions);
                }
            }
            // add to cache
            stopper.Add(type);
            ////return fetchOptions;
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
