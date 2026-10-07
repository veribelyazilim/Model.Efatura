namespace Model.Efatura.HtLogs
{
    public class HtLogDto
    {
        public string UUID { get; set; }
        public string DocumentId { get; set; }
        public string Status { get; set; }
        public bool Sent { get; set; }
        public int Error { get; set; } = 0;
    }
}
