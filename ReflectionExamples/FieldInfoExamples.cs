using System;
using System.Collections;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples2.Model;

namespace ReflectionExamples {
	[TestClass]
	public class FieldInfoExamples {
		[TestMethod]
		public void GetPrivateFieldValueExample() {
			Contact contact = new Contact();
			contact.FileAs = "Jorge Perez";
			Type myType = typeof(Contact);
			FieldInfo myFieldInfo = myType.GetField("_fileAs", BindingFlags.NonPublic | BindingFlags.Instance);

			// Display the string before applying SetValue to the field.
			var value = myFieldInfo.GetValue(contact);
			Assert.AreEqual("Jorge Perez", value);
		}

		[TestMethod]
		public void SetValueExample() {
			Contact contact = new Contact();
			contact.FileAs = "Jorge Perez";
			Type myType = typeof(Contact);
			FieldInfo myFieldInfo = myType.GetField("fileAs", BindingFlags.NonPublic | BindingFlags.Instance);

			// Display the string before applying SetValue to the field.
			myFieldInfo.SetValue(contact, "Alex Rodriguez");
			Assert.AreEqual("Alex Rodriguez", contact.FileAs);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void SetInvalidTypeValueExample() {
			Contact contact = new Contact();
			Type myType = typeof(Contact);
			FieldInfo myFieldInfo = myType.GetField("fileAs", BindingFlags.NonPublic | BindingFlags.Instance);

			// Display the string before applying SetValue to the field.
			myFieldInfo.SetValue(contact, 33);
			Assert.AreEqual("Alex Rodriguez", contact.FileAs);
		}

		[TestMethod]
		public void GettingTypeExample() {
			int radius = 3;
			string typeString = (radius * radius * Math.PI).GetType().ToString();
			Assert.AreEqual("System.Double",typeString);
		}


		[TestMethod]
		public void GetIndexPropertyValue() {
			object theValue = null;
			var contact = new Contact();
			contact.FirstName = "jorge";
			contact.Emails.Add(new Email() { Id = 1, EmailAddress = "jorgepires@gmail.com" });

			PropertyInfo propInfoList = contact.GetType().GetProperty("Emails"); //.GetValue(c.Emails);
			IList emails = propInfoList.GetValue(contact, null) as IList;
			Assert.IsNotNull(emails);
            PropertyInfo emailAddressPropInfo = propInfoList.PropertyType.GetGenericArguments()[0].GetProperty("EmailAddress");

            foreach (var email in emails) {
                theValue = emailAddressPropInfo.GetValue(email, null);
				Assert.AreEqual(theValue, "jorgepires@gmail.com");
                emailAddressPropInfo.SetValue(email, "jorgepires@yahoo.com", null);  // <-- set to an appropriate value
				Assert.AreEqual(((Email)email).EmailAddress, "jorgepires@yahoo.com");
			}

		}
	}
}
