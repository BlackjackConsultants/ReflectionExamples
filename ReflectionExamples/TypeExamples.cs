using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples2.Model;

namespace ReflectionExamples2 {
	[TestClass]
	public class TypeExamples {

		[TestMethod]
		public void GettingTypeExample() {
			int radius = 3;
			string typeString = (radius * radius * Math.PI).GetType().ToString();
			Assert.AreEqual("", "");
		}

		/// <summary>
		/// Use 'is' to check the type of an instance
		/// </summary>
		[TestMethod]
		public void CheckTypeOfAnInstance() {
			Individual individual = new Individual() { FileAs = "Jorge" };
			Organization organization = new Organization() { FileAs = "Microsoft" };
			Assert.IsTrue(individual is Individual);
			Assert.IsTrue(individual is Contact);
		}

		/// <summary>
		/// Use 'is' to check the type of an instance
		/// </summary>
		[TestMethod]
		public void GettingTheTypesAtCompileTimeVsRuntime() {
			
		}

		/// <summary>
		/// Helps show difference betwee gettype() and typeof
		/// </summary>
		[TestMethod]
		public void InstantiatingATypeWithTypeofandGetType() {
			string s = "hello";
			Type t1 = typeof(string);
			Type t2 = s.GetType();
			Assert.AreEqual(t1, t2, "The 2 types are not the same");
		}

        /// <summary>
        /// Helps show difference betwee gettype() and typeof
        /// </summary>
        [TestMethod]
        public void InstantiatingATypeDynamicallyWithString() {
            Type t1 = Type.GetType("System.String");
            Assert.AreEqual(t1.FullName, "System.String");
        }

        /// <summary>
        /// Helps show difference betwee gettype() and typeof
        /// </summary>
        [TestMethod]
        public void GetPropertiesOfAClass() {
            Individual individual = new Individual() { FileAs = "Mr. Jorge Perez", FirstName =  "jorge", LastName = "Perez", Address = new Address() {ZipCode = "33333"}};
            foreach (var prop in individual.GetType().GetProperties()) {
                System.Diagnostics.Debug.WriteLine("{0}={1}", prop.Name, prop.GetValue(individual, null));
            }
        }

        /// <summary>
        /// Helps show difference betwee gettype() and typeof
        /// </summary>
        [TestMethod]
        public void GetPropertyOfAClass() {
            Individual individual = new Individual() { FileAs = "Mr. Jorge Perez", FirstName = "jorge", LastName = "Perez", Address = new Address() { ZipCode = "33333" } };
            foreach (var prop in individual.GetType().GetProperties()) {
                System.Diagnostics.Debug.WriteLine("{0}={1}", prop.Name, prop.GetValue(individual, null));
            }
        }

        [TestMethod]
        public void GetPropertyType() {
            Individual individual = new Individual() { FileAs = "Mr. Jorge Perez", FirstName = "jorge", LastName = "Perez", Address = new Address() { ZipCode = "33333" } };
                    foreach (PropertyInfo pi in individual.GetType().GetProperties())
                        if (typeof(Address).IsAssignableFrom(pi.PropertyType))
                            Console.WriteLine("Address can be assigned to " + pi.Name);
        }

	    [TestMethod]
	    public void GetDerivedTypes(){
            var derivedClasses = typeof(C).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(C))).ToList();
            Assert.AreEqual(derivedClasses.Count(), 2);
	    }

        [TestMethod]
        public void CheckIfPropertyExists() {
            Individual individual = new Individual() { FileAs = "Mr. Jorge Perez", FirstName = "jorge", LastName = "Perez", Address = new Address() { ZipCode = "33333" } };
            var fileAsExists = individual.GetType().GetProperty("FileAs") != null;
            Assert.IsTrue(fileAsExists);
            var middleNameExists = individual.GetType().GetProperty("MiddleName") != null;
            Assert.IsFalse(middleNameExists);
        }

        [TestMethod]
        public void GetTypeName() {
            Individual individual = new Individual() { FileAs = "Mr. Jorge Perez", FirstName = "jorge", LastName = "Perez", Address = new Address() { ZipCode = "33333" } };
            var nameParts = individual.GetType().FullName.Split('.');
            var name = nameParts[nameParts.Length - 1];
            Assert.AreEqual(name, "Individual");
        }

        [TestMethod]
        public void TestIsAbstract(){
            var isAbstract = typeof(C).IsAbstract;
            Assert.IsTrue(isAbstract);
        }
	}
}
