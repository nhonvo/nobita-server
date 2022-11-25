using HDBank.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HDBank.Infrastructure.Data
{
    public class HDBankDbContext : IdentityDbContext<AppUser>
    {
        public HDBankDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}