namespace WoasFormsApp.Services
{
    public class sfUserDataView
    {
        public required string SalesForceContactID { get; init; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string About { get; set; } = "";
        public DateOnly? BirthDay { get; set; } 
    }

    public interface ISFContactManager
    {
        public Task<sfUserDataView?> CreateCurrentUserSFContact();
        public Task<sfUserDataView?> GetCurrentUserSFContact();
        public Task<bool> UpdateCurrentUserSFContact(sfUserDataView sfUserDataView);
    }
}
