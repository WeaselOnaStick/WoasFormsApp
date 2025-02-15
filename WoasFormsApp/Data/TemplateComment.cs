namespace WoasFormsApp.Data
{
    public class TemplateComment
    {
        public int Id { get; set; }
        
        public WoasFormsAppUser User { get; set; }

        public Template? Template { get; set; }

        public string Text { get; set; } = "";

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}