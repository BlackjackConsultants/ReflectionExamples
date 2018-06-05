namespace ReflectionExamples2.Model {
	public class B {
		[FetchOption(FetchOptions.Address)]
		public A Type { get; set; }

        [FetchOption(FetchOptions.Domain)]
        public C BaseClass { get; set; }
	}
}