using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR_TST.Models
{
    public class ChatMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
