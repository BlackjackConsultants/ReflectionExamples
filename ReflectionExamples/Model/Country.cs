namespace ReflectionExamples2.Model {
    public class Country : INode {
        public INode Entity { get ; set ; }

        public string Name { get; set; }
    }
}
