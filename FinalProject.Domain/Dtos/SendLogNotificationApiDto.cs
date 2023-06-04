

namespace FinalProject.Domain.Dtos
{
    public class SendLogNotificationApiDto
    {
        public SeleniumLodDto Log { get; set; }
        public string ConnectionId { get; set; }
        public SendLogNotificationApiDto(SeleniumLodDto log, string connectionId)
        {
            Log = log;
            ConnectionId = connectionId;
        }
    }
}
