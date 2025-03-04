namespace WoasFormsApp.Services
{
    public class sfUserDataView
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string About { get; set; } = "";
        public DateOnly? BirthDay { get; set; } 
    }

    public interface ISFContactManager
    {
        public Task<sfUserDataView?> CreateCurrentUserSFContact();
        public Task<sfUserDataView?> GetCurrentUserSFContact();
        public Task UpdateCurrentUserSFContact(sfUserDataView sfUserDataView);
    }
}
