namespace SignalR_TST.Models
{
    public class FileMessage
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public byte[] File { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
