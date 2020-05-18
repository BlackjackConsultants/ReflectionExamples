using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionExamples2.Model {
    public class Phone : INode {
        private string number;

        public string Number {
            get { 
                return number; 
            }
            set {
                number = value;
            }
        }
        public string Type { get; set; }
        public int Id { get; set; }
        public INode Entity { get; set; }
    }
}
