using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples.Attributes;
using ReflectionExamples.Model;

namespace ReflectionExamples {
    [TestClass]
    public class AttributeExamples {
        [TestMethod]
        public void GettingMethodAttributes() {
            var passed = false;
            var individual = new Individual();
            var methods = individual.GetType().GetMethods();
            foreach (var methodInfo in methods) {
                foreach (var attrib in methodInfo.GetCustomAttributes(true)) {
                    System.Diagnostics.Debug.WriteLine("methodInfo: " + methodInfo.Name + ", attribute: " + attrib.GetType().ToString());
                    if (attrib is ReflectionExamples.Attributes.ExampleAttribute) {
                        passed = true;
                    }
                }
            }
            Assert.IsTrue(passed);
        }

        [TestMethod]
        public void IsDefinedExamples() {
            var passed = false;
            var individual = new Individual();
            var methods = individual.GetType().GetMethods();
            foreach (var methodInfo in methods) {
                foreach (var attrib in methodInfo.GetCustomAttributes(true)) {
                    System.Diagnostics.Debug.WriteLine("methodInfo: " + methodInfo.Name + ", attribute: " + attrib.GetType().ToString());
                    if (attrib is ExampleAttribute) {
                        passed = methodInfo.IsDefined(typeof(ExampleAttribute), true);
                        System.Diagnostics.Debug.WriteLine(passed);
                    }
                }
            }
            Assert.IsTrue(passed);
        }
    }
}
