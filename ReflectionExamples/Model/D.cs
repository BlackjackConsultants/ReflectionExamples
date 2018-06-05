namespace ReflectionExamples2.Model{
    public class D : C {
        [FetchOption(FetchOptions.State)]
        public F Type { get; set; }
        
    }
}