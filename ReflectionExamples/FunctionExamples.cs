using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples2.Model;
using ReflectionExamples2.Extensions;

namespace ReflectionExamples2 {

	[TestClass]
    public class FunctionExamples {
        [TestMethod]
        public void CallingAMethodByReflection() {
            // create instance of class Calculator
            Contact inst = (Contact)Activator.CreateInstance(typeof(Contact));
            inst.FirstName = "jorge";
            inst.LastName = "perez";
            var fileAs = inst.GetType().GetTypeInfo().GetDeclaredMethod("GetFileAs").Invoke(inst, null);
            Assert.AreEqual(fileAs, "jorge perez");
        }

        /// <summary>
        /// this method tests how the subclass method is called.
        /// </summary>
        [TestMethod]
        public void CallingASubClassMethodByReflection() {
            // create instance of class Calculator
            Individual inst = (Individual)Activator.CreateInstance(typeof(Individual));
            inst.FirstName = "jorge";
            inst.LastName = "perez";
            var fileAs = inst.GetType().GetTypeInfo().GetDeclaredMethod("GetFileAs").Invoke(inst, null);
            Assert.AreEqual(fileAs, "jorge, perez");
        }

        /// <summary>
        /// this method tests how the subclass method is called.
        /// </summary>
        [TestMethod]
        public void MoveItemWithId0ToFirst() {
            // prepare data
            Individual tempInd = null;
            List<Individual> indList = new List<Individual>();
            indList.Add(new Individual() {LastName = "Perez2", FirstName = "Jorge", Id = 2});
            indList.Add(new Individual() {LastName = "Perez1", FirstName = "Jorge", Id = 1});
            indList.Add(new Individual() {LastName = "Perez0", FirstName = "Jorge", Id = 0});
            indList.Add(new Individual() {LastName = "Perez3", FirstName = "Jorge", Id = 3 });
            // remove item from its location
            for (int i = indList.Count - 1; i >= 0; i--){
                var currentInd = indList[i];
                if (currentInd.Id == 0){
                    tempInd = currentInd;
                    indList.RemoveAt(i);
                }
            }
            // insert at position 0
            indList.Insert(0, tempInd);
            Assert.AreEqual(indList.Count, 4);
            Assert.IsTrue(indList[0].Id == 0);
        }
    }
}
