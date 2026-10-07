using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Model.Efatura.HtLogs
{
    public class HtLog : FullAuditedEntity<Guid>
    {
        [StringLength(40)]
        public string UUID { get; set; }
        [StringLength(40)]
        public string DocumentId { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        public bool Sent { get; set; }
        public int Error { get; set; } = 0;
        public string? ErrorMessage { get; set; }
    }
}
