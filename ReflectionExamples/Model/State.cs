namespace ReflectionExamples2.Model{
    public class State{
        public string Name { get; set; }

        [FetchOption(FetchOptions.Country)]
        public Country Country { get; set; }
    }
}