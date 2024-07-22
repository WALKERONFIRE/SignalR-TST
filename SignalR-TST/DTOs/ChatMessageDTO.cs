namespace SignalR_TST.DTOs
{
    public class ChatMessageDTO
    {
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
