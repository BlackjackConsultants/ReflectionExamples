using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionExamples2.Model;

namespace ReflectionExamples {
	[TestClass]
	public class FieldInfoExamples {
		[TestMethod]
		public void GetValueExample() {
			Contact contact = new Contact();
			contact.FileAs = "Jorge Perez";
			Type myType = typeof(Contact);
			FieldInfo myFieldInfo = myType.GetField("fileAs", BindingFlags.NonPublic | BindingFlags.Instance);

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
	}
}
