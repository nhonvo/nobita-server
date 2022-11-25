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
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Transaction>()
                .HasOne(t => t.Sender)
                .WithMany(s => s.SendTransactions)
                .HasForeignKey(t => t.SenderId);

            builder.Entity<Transaction>()
                .HasOne(t => t.Receiver)
                .WithMany(s => s.ReceiveTransactions)
                .HasForeignKey(t => t.ReceiverId);
        }
    }
}