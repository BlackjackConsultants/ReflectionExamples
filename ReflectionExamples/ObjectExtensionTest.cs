using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples2.Model;
using ReflectionExamples2.Extensions;

namespace ReflectionExamples2 {

	[TestClass]
    public class ObjectExtensionTest {

        [TestMethod]
        public void TestHashHasValue() {
            Individual individual = new Individual();
            individual.FirstName = "jorge";
            individual.LastName = "perez";
            var hash = individual.GetHashCodeValue();
            Assert.AreNotEqual(hash, 0);
        }

        [TestMethod]
        public void TestHashHasSameValue() {
            Individual individual = new Individual();
            individual.FirstName = "jorge";
            individual.LastName = "perez";
            Individual individual2 = new Individual();
            individual2.FirstName = "jorge";
            individual2.LastName = "perez";
            var hash = individual.GetHashCodeValue();
            var hash2 = individual2.GetHashCodeValue();
            Assert.AreEqual(hash, hash2);
        }
    }
}
