using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionExamples.Attributes {
    public class ExampleAttribute : Attribute {
        private string stringVal;

        public ExampleAttribute() {
            stringVal = "This is the default string.";
        }

        public string StringValue {
            get { return stringVal; }
            set { stringVal = value; }
        }
    }
}
