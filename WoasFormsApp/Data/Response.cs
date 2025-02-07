namespace WoasFormsApp.Data
{
    public class Response
    {
        public required int Id { get; set; }

        public required Template Template { get; set; }

        public ICollection<ResponseAnswer> Answers {  get; set; }
    }

    public class ResponseAnswer
    {
        public required int Id { get; set; }

        public required TemplateField Field { get; set; }

    }
}