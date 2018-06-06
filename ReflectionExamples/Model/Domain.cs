namespace ReflectionExamples2.Model{
    public class Domain : INode {
        public string Name { get; set; }
        public INode Entity { get; set; }
    }
}