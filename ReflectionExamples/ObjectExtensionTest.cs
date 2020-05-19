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
        public void CheckCollision() {
            for (int ii = 0; ii < 100; ii++) {
                Dictionary<int, Individual> dictionary = new Dictionary<int, Individual>();
                for (int i = 0; i < 100000; i++) {
                    var individual = CreateLazyIndividual(i);
                    var hashKey = individual.GetHashCode();
                    if (!dictionary.ContainsKey(hashKey)) {
                        // hash not found in dictionary
                        dictionary.Add(hashKey, individual);
                        // create a second individual to check if it exists
                        var individual2 = CreateLazyIndividual(i);
                        var hashKey2 = individual.GetHashCode();
                        if (!dictionary.ContainsKey(hashKey2))
                            Assert.Fail();
                    } else {
                        // hash not found in dictionary
                        System.Diagnostics.Debug.WriteLine("{0}. The collision occurred at {1}", new[] { ii.ToString(), i.ToString() });
                        break;
                    }
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GenerateObjectPath() {
            var individual = CreateIndividual();
            List<KeyValuePair<string, string>> keys = new List<KeyValuePair<string, string>>();
            keys.Add(new KeyValuePair<string, string>("individual", "Id"));
            keys.Add(new KeyValuePair<string, string>("phone", "Id"));
            keys.Add(new KeyValuePair<string, string>("phone", "Number"));
            keys.Add(new KeyValuePair<string, string>("address", "Id"));
            keys.Add(new KeyValuePair<string, string>("state", "Id"));
            keys.Add(new KeyValuePair<string, string>("country", "Id"));
            var path = individual.Phone.GetObjectPath(keys, "Entity");
            Assert.AreEqual("/Individual[Id=1]/Phone[Id=1&Number=305.333.3333]", path.ToString());
        }

        [TestMethod]
        public void PartialUpdate() {
            // change email list
            Individual individual = CreateIndividual();
            Individual sourceIndividual = new Individual();
            sourceIndividual.Emails.Add(new Email() { EmailAddress = "lucre@gmail.com" });
            individual.UpdateFrom(sourceIndividual, this.getIgnoreTypeList(), this.getIgnorePropertyList());
            Assert.AreEqual(individual.Emails.Count, 2);
            Assert.AreEqual(individual.Emails[1].EmailAddress, sourceIndividual.Emails[0].EmailAddress);
            // change phone reference class
            individual = CreateIndividual();
            sourceIndividual = new Individual();
            sourceIndividual.Phone.Number = "999-999-9999";
            individual.UpdateFrom(sourceIndividual, this.getIgnoreTypeList(), this.getIgnorePropertyList());
            Assert.AreEqual(individual.Phone.Number, sourceIndividual.Phone.Number);
            Assert.AreNotEqual(individual.Phone.Type, sourceIndividual.Phone.Type);
            Assert.AreNotEqual(individual.Phone.Id, sourceIndividual.Phone.Id);
            // change individual reference class
            individual = CreateIndividual();
            sourceIndividual = new Individual();
            sourceIndividual.FirstName = "Juanito";
            individual.UpdateFrom(sourceIndividual, this.getIgnoreTypeList(), this.getIgnorePropertyList());
            Assert.AreEqual(individual.FirstName, "Juanito");

        }

        private IList<string> getIgnoreTypeList() {
            var ignoreItems = new List<string>();
            ignoreItems.Add("System.Collections.Generic.IList");
            return ignoreItems;
        }

        private IList<string> getIgnorePropertyList() {
            var ignoreItems = new List<string>();
            ignoreItems.Add("Id");
            return ignoreItems;
        }

        private Phone CreatePhone(Individual parent) {
            var phone = new Phone
            {
                Entity = parent,
                Id = 1,
                Number = "305.333.3333",
                Type = "home"
            };
            return phone;
        }

        private Individual CreateLazyIndividual(int id) {
            var individual = new Individual();
            individual.FirstName = "jorge";
            individual.LastName = "perez";
            individual.FileAs = "jp";
            individual.Id = id;
            return individual;
        }

        private Individual CreateIndividual() {
            var individual = new Individual();
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
            return new Phone
            {
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
            email1.Id = 999;
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
