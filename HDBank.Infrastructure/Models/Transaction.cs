using System.ComponentModel.DataAnnotations;

namespace HDBank.Infrastructure.Models
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Amount { get; set; }
        public string Description { get; set; }

        public string SenderId { get; set; }
        public AppUser Sender { get; set; }

        public string ReceiverId { get; set; }
        public AppUser Receiver { get; set; }

    }
}