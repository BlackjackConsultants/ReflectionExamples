using System.Collections;
using System.Collections.Generic;

namespace ReflectionExamples2.Model {
    public class Contact : INode {
        public Contact() {
            this.Emails = new List<Email>();
        }
        private string _fileAs;

        public string FileAs {
            get { return _fileAs; }
            set {
                _fileAs = value;
            }
        }

        [FetchOption(FetchOptions.Address)]
        public Address Address { get; set; }

        [FetchOption(FetchOptions.Phone)]
        public Phone Phone { get; set; }

        public IList<Email> Emails { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public INode Entity {
            get;
            set;
        }

        public virtual string GetFileAs() {
            return FirstName + " " + LastName;
        }
    }
}
