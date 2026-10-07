using System;

namespace Model.Efatura.InvoiceModels
{
    public class InvoiceModelCo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? CompanyCode { get; set; }
        public string? UUID { get; set; }
        public bool? Sent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? NoInvoiceNumber { get; set; }
        public bool? IsEfatura { get; set; }
        public string? InvoiceNumber { get; set; }
        //public string VeritabaniAdi { get; set; }
        //public string Cid { get; set; }
    }
}
