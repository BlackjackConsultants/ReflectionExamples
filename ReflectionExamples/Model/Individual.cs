using System;
using ReflectionExamples.Attributes;
using ReflectionExamples2.Extensions;

namespace ReflectionExamples2.Model {
	public class Individual : Contact{
        public string FirstName { get; set; }
        public string LastName { get; set; }
	    public int Id { get; set; }

	    [Example]
        public override string GetFileAs() {
            return FirstName + ", " + LastName;
        }
    }
}
