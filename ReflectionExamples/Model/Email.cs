namespace ReflectionExamples2.Model{
    public class Email : INode {
        public string EmailAddress { get; set; }

        [FetchOption(FetchOptions.Domain)]
        public Domain Domain { get; set; }
        public INode Entity { get; set; }
    }
}