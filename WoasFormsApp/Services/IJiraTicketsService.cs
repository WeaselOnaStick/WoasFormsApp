using Duende.IdentityModel.Client;
using System.ComponentModel.DataAnnotations;

namespace WoasFormsApp.Services
{
    public class JiraTicketView
    {
        public string Id { get; set; } = "";
        public string Status { get; set; } = "UNDEFINED";
        public string Summary { get; set; } = "";
    }

    public class JiraCustomerView
    {
        public string AccountId { get; set; } = "";
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public List<JiraTicketView> Tickets { get; set; } = new List<JiraTicketView>();
    }

    public class NewTicketModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Summary { get; set; } = "";
    }

    public interface IJiraTicketsService
    {
        public Task<JiraCustomerView?> CreateCustomerFromCurrentUser(string? overrideEmail = null);
        public Task<JiraCustomerView?> GetCustomerFromCurrentUser();

        public Task<JiraTicketView?> CreateTicketByCurrentUser(NewTicketModel model);
        public Task<JiraTicketView?> GetTicketById(string ticketId);
    }
}
