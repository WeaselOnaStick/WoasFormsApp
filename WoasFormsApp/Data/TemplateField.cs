namespace WoasFormsApp.Data
{
    public class TemplateField
    {
        public int Id { get; set; }

        public Template Template { get; set; }

        // Hidden becomes true when this field is deleted during template editing
        // Hidden fields are not shown when filling a form
        // Hidden becomes false during template editing when there becomes N fields of this type and this is Nth field of its type
        public bool Hidden { get; set; } = false;

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public TemplateFieldType Type { get; set; }

        public int Position { get; set; }
        
        public bool ShowInAnalytics { get; set; } = true;
    }

    public class TemplateFieldType
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}