namespace WoasFormsApp.Data
{
    abstract public class TemplateField
    {
        public required int Id { get; set; }

        public required Template Template { get; set; }
        
        public required string Title { get; set; }
        public string? Description { get; set; }

        public required bool ShowInAnalytics { get; set; } = true;

        public required int Position { get; set; }
    }

    public class FieldTextSingleLine : TemplateField { }
    public class FieldTextMultiLine : TemplateField { }
    public class FieldInt : TemplateField { }
    public class FieldMultiSelect : TemplateField { }

}