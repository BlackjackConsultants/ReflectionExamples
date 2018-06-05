using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples2.Model;
using ReflectionExamples2.Extensions;

namespace ReflectionExamples2 {

	[TestClass]
    public class CastingExamples {
        [TestMethod]
        public void CastByReflection(){
            object contact = new Contact { FileAs = "Jorge Perez" }; ;
            Contact contact2 = null;
            contact2 = Convert.ChangeType(contact, typeof(Contact)) as Contact;
            Assert.AreEqual(contact2.FileAs, "Jorge Perez");
        }

        [TestMethod]
        public void CastByReflectionUsingGenericExtensionMethod() {
            object contact = new Contact { FileAs = "Jorge Perez" }; ;
            Contact contact2 = null;
            contact2 = contact.GetObjectAs<Contact>();
            Assert.AreEqual(contact2.FileAs, "Jorge Perez");
        }
    }
}
