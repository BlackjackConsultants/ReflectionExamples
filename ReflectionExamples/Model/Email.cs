namespace ReflectionExamples2.Model{
    public class Email{
        public string EmailAddress { get; set; }

        [FetchOption(FetchOptions.Domain)]
        public Domain Domain { get; set; }
    }
}