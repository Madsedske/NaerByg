namespace API.Models
{
    public class ApiSyncLog
    {
        public int ApiSyncLogId { get; set; }
        public int ChainId { get; set; }
        public string DataObject { get; set; }
        public DateTime SyncedAt { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }

}
