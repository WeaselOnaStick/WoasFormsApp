using Duende.IdentityModel.Client;

namespace WoasFormsApp.Services
{
    public class JiraTicketView
    {
        public string Id { get; set; } = "";
        public string Status { get; set; } = "UNDEFINED";
        public string Summary { get; set; } = "";
        public DateTime FiledOn { get; set; }
    }

    public class JiraCustomerView
    {
        public string AccountId { get; set; } = "";
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public List<JiraTicketView> Tickets { get; set; } = new List<JiraTicketView>();
    }

    public interface IJiraTicketsService
    {
        public Task<JiraCustomerView?> CreateCustomerFromCurrentUser();
        public Task<JiraCustomerView?> GetCustomerFromCurrentUser();

        public Task<JiraTicketView?> CreateTicket(JiraTicketView model);
    }
}
