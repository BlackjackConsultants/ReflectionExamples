using System.Collections;
using System.Collections.Generic;

namespace ReflectionExamples2.Model {
	public class Contact : INode {
        public string FileAs { get; set; }

		[FetchOption(FetchOptions.Address)]
	    public Address Address { get; set; }

		[FetchOption(FetchOptions.Phone)]
        public Phone Phone { get; set; }

	    public IList<Email> Emails { get; set; }

	    public string FirstName { get; set; }
	    public string LastName { get; set; }
        public INode Entity {
            get;
            set ;
        }

        public virtual string GetFileAs(){
	        return FirstName + " " + LastName;
	    }
    }
}
