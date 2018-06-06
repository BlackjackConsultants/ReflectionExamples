using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionExamples2.Model {
    public class Address : INode {
        public string ZipCode { get; set; }

        [FetchOption(FetchOptions.State)]
        public State State { get; set; }
        public INode Entity { get; set; }
    }
}
