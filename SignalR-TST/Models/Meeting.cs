namespace SignalR_TST.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string OrganizerUserId { get; set; }
        public string ParticipantUserId { get; set; }
        public string MeetingUrl { get; set; }
        public DateTime ScheduledTime { get; set; }

    }
}
