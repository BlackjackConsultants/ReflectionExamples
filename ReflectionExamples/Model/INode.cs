using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionExamples2.Model {
    public interface INode {
        INode Entity { get; set; }
    }
}
