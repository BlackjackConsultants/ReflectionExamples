using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples.Model;

namespace ReflectionExamples {
	[TestClass]
	public class TypeExamples {

		[TestMethod]
		public void GettingTypeExample() {
			int radius = 3;
			string typeString = (radius * radius * Math.PI).GetType().ToString();
			Assert.AreEqual("", "");
		}

		[TestMethod]
		public void CheckObjecIs() {
			Individual individual = new Individual() { FileAs = "Jorge" };
			Organization organization = new Organization() { FileAs = "Microsoft" };
			Assert.IsFalse(individual is Organization);
			Assert.IsTrue(individual is Individual);
			Assert.IsTrue(individual is Contact);
		}
	}
}
