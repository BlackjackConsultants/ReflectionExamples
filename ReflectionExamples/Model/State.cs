namespace ReflectionExamples2.Model{
    public class State : INode {
        public string Name { get; set; }

        [FetchOption(FetchOptions.Country)]
        public Country Country { get; set; }
        public INode Entity { get; set; }
    }
}