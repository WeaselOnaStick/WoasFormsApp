using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WoasFormsApp.Data
{
    public class TemplateComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public WoasFormsAppUser User { get; set; }

        public Template? Template { get; set; }

        public string Text { get; set; } = "";

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}