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

        [TestMethod]
        public void GenerateObjectPath() {
            var individual = CreateIndividual();
            var path = individual.Phone.GetObjectPath(new string[1] { "Id" }, "Entity", null);
        }

        private Phone CreatePhone(Individual parent) {
            var phone = new Phone             {
                Entity = parent,
                Id = 1,
                Number = "305.333.3333"
            };
            return phone;
        }

        private Individual CreateIndividual() {
            var individual = new Individual();
            individual.Id = 1;
            individual.FirstName = "jorge";
            individual.LastName = "perez";
            individual.FileAs = "jp";
            individual.Emails = CreateEmails(individual);
            individual.Address = CreateAddress(individual);
            individual.Id = 1;
            individual.Phone = CreatePhone(individual);
            return individual;
        }

        private Phone CreatePhone() {
            return new Phone {
                Id = 1,
                Number = "305.555.3333"
            };
        }

        private IList<Email> CreateEmails(Contact parent) {
            var emails = new List<Email>();
            var email1 = new Email();
            email1.Entity = parent;
            email1.Domain = CreateDomain(email1);
            email1.EmailAddress = "jogito.suarez@yahoo.com";
            emails.Add(email1);
            return emails;
            
        }

        private Domain CreateDomain(Email email) {
            return new Domain { Entity = email, Name = "ALL" };
        }

        private Address CreateAddress(Contact parent) {
            var address = new Address();
            return new Address
            {
                Entity = parent,
                State = CreateState(address),
                ZipCode = "33333"
            };
        }

        private State CreateState(Address parent) {
            var state = new State();
            state.Name = "FL";
            state.Entity = parent;
            state.Country = CreateCountry(state);
            return state;
        }

        private Country CreateCountry(State parent) {
            return new Country { Entity = parent, Name = "USA" };
        }
    }
}
