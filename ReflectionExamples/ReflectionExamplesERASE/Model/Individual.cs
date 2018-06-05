using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionExamples.Attributes;

namespace ReflectionExamples.Model {
	public class Individual : Contact{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Example]
        public string GetFileAs() {
            return FirstName + ", " + LastName;
        }
	}
}
