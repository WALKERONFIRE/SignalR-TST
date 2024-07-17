using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR_TST.Models
{
    public class Meeting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrganizerUserId { get; set; }
        public string ParticipantUserId { get; set; }
        public string MeetingUrl { get; set; }
        public DateTime ScheduledTime { get; set; }

    }
}
