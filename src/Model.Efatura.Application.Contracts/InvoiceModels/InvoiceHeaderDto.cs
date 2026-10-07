using System.Collections.Generic;

namespace Model.Efatura.InvoiceModels
{
    public class InvoiceHeaderDto
    {
        public string? UUID { get; set; }
        public string CompanyCode { get; set; }
        public int? SystemDocNum { get; set; }
        public string? Invoice_ID { get; set; }
        public bool? Sent { get; set; }
        public string? IssueDate { get; set; }
        public string? IssueTime { get; set; }
        public string? ProfileID { get; set; }
        public string? InvoiceTypeCode { get; set; }
        public string? XSLT_Adi { get; set; }
        public string? DocumentCurrencyCode { get; set; }
        public decimal? LineExtensionAmount { get; set; }
        public decimal? AllowanceTotalAmount { get; set; }
        public decimal? TaxInclusiveAmount { get; set; }
        public decimal? PayableAmount { get; set; }
        public decimal? CalculationRate { get; set; }
        public string? OrderReferenceId { get; set; }
        public string? OrderReferenceDate { get; set; }
        //public int IsCashRegister { get; set; }
        public List<NoteModelDto> Notes { get; set; }
    }
}
