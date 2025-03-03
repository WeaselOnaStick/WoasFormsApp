namespace WoasFormsApp.Services
{
    public class sfUserDataView
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string About { get; set; } = "";
        public DateTime BirthDay { get; set; } 
    }

    public interface ISFContactManager
    {
        public Task<sfUserDataView?> GetCurrentUserSFContact();
    }
}
