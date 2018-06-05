using System;

namespace ReflectionExamples2.Model {

    /// <summary>
    /// This attribute specifies the <see cref="FetchOptions"/> needed to fetch the property from the Web Services.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FetchOptionAttribute : Attribute {

        private readonly string fetchOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="FetchOptionAttribute"/> class.
        /// </summary>
        /// <param name="fetchOption">The <see cref="FetchOptions"/> value.</param>
        public FetchOptionAttribute(string fetchOption) {
            if (String.IsNullOrEmpty(fetchOption))
                throw new ArgumentNullException("fetchOption");
            // store option
            this.fetchOption = fetchOption;
        }

        /// <summary>
        /// Gets the <see cref="FetchOptions"/> value.
        /// </summary>
        public string FetchOption {
            get { return fetchOption; }
        }
    }
}
