using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionExamples.Model {
	public class Contact {
		private string fileAs;
		public string FileAs {
			get { return this.fileAs; }
			set {
				this.fileAs = value;
			}
		}
	}
}
