using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WoasFormsApp.Data
{
    public class WoasFormsDbContext(DbContextOptions<WoasFormsDbContext> options) : IdentityDbContext<WoasFormsAppUser>(options)
    {

    }
}
