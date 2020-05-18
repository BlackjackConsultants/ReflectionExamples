using System;
using ReflectionExamples.Attributes;
using ReflectionExamples2.Extensions;

namespace ReflectionExamples2.Model {
	public class Individual : Contact{
        public Individual() {
            this.Phone = new Phone();
            this.Address = new Address();
            this.Emails = new System.Collections.Generic.List<Email>();
        }
	    public int Id { get; set; }

	    [Example]
        public override string GetFileAs() {
            return FirstName + ", " + LastName;
        }
    }
}
