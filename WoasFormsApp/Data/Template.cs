using System.ComponentModel.DataAnnotations.Schema;

namespace WoasFormsApp.Data
{
    public class Template
    {
        public int Id { get; set; }

        public WoasFormsAppUser Owner { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public required string Title { get; set; }
        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; } = "https://placehold.co/300x200";

        public required int TopicId { get; set; } = 0;
        public TemplateTopic Topic { get; set; }

        public ICollection<TemplateTag>? Tags {  get; set; }

        public ICollection<WoasFormsAppUser>? UsersWhoLiked { get; set; }

        public ICollection<TemplateComment>? Comments { get; set; }

        // if NOT Public refer to trusted list
        public bool Public { get; set; } = true;
        public ICollection<WoasFormsAppUser> AllowedUsers {  get; set; }


        public ICollection<Response> Responses { get; set; }

        public ICollection<TemplateField> Fields { get; set; }
    }
}