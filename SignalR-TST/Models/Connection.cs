using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR_TST.Models
{
    public class Connection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ConId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
