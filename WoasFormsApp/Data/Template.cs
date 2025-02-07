namespace WoasFormsApp.Data
{
    public class Template
    {
        public required string Title { get; set; }
        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; }

        public required int TopicId { get; set; } = 0;
        public Topic topic { get; set; }

        public List<string>? UserIdsWhoLiked { get; set; }
        public List<WoasFormsAppUser>? UsersWhoLiked { get; set; }

        // if NOT Public refer to trusted list
        public bool Public { get; set; } = true;

        public List<string> AllowedUserIds { get; set; }
        public List<WoasFormsAppUser> AllowedUsers {  get; set; }

        public List<>
    }
}