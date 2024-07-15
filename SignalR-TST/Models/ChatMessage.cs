﻿namespace SignalR_TST.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
