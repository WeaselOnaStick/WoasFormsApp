namespace WoasFormsApp.Data
{
    public class TemplateComment
    {
        public int Id{ get; set; }
        
        public required WoasFormsAppUser User { get; set; }

        public required Template Template { get; set; }

        public required string Text { get; set; } = "";

        public required DateTime PostedAt { get; set; }
    }
}