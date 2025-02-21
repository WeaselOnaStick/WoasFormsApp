using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WoasFormsApp.Data
{
    public class Template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public WoasFormsAppUser? Owner { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

        public string Title { get; set; } = "";
        public string? Description { get; set; } = "";

        public string? CoverImageUrl { get; set; } = "";

        public int TopicId { get; set; } = 0;
        public TemplateTopic Topic { get; set; }

        public ICollection<TemplateTag> Tags {  get; set; } = new HashSet<TemplateTag>();

        public ICollection<WoasFormsAppUser> UsersWhoLiked { get; set; } = new HashSet<WoasFormsAppUser>();

        public ICollection<TemplateComment> Comments { get; set; } = new HashSet<TemplateComment>();

        // if NOT Public refer to trusted list
        public bool Public { get; set; } = true;
        public ICollection<WoasFormsAppUser> AllowedUsers { get; set; } = new HashSet<WoasFormsAppUser>();

        public ICollection<Response> Responses { get; set; } = new HashSet<Response>();

        public required List<TemplateField> Fields { get; set; } = new List<TemplateField>();
    }
}