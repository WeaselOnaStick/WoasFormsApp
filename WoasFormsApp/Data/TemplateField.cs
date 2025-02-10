namespace WoasFormsApp.Data
{
    public class TemplateField
    {
        public required int Id { get; set; }

        public required Template Template { get; set; }
        
        public required string Title { get; set; }
        public string? Description { get; set; }

        public required TemplateFieldType Type { get; set; }

        public required int Position { get; set; }
        
        public required bool ShowInAnalytics { get; set; } = true;
    }

    public class TemplateFieldType
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}