namespace Model.Efatura.EDefter
{
    /// <summary>
    /// dbo.EDEFTER prosedürünün çıktı satırı. Özellik adları prosedür kolonlarıyla
    /// (büyük/küçük harf farkı hariç) birebir aynıdır, Dapper doğrudan eşler.
    /// </summary>
    public class EDefterLineDto
    {
        public string EnteredBy { get; set; } = default!;
        public string EnteredDate { get; set; } = default!;
        public int EntryNumber { get; set; }
        public string? EntryComment { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public int EntryNumberCount { get; set; }
        public int? BatchId { get; set; }
        public int? LineNumber { get; set; }
        public int LineNumberCounter { get; set; }
        public string AccountMainId { get; set; } = default!;
        public string? AccountMainDescription { get; set; }
        public string? AccountSubDescription { get; set; }
        public string AccountSubId { get; set; } = default!;
        public decimal Amount { get; set; }
        public string DebitCreditCode { get; set; } = default!;
        public string PostingDate { get; set; } = default!;
        public string? DocumentType { get; set; }
        public string? DocumentTypeDescription { get; set; }
        public string? DocumentNumber { get; set; }
        public string? DocumentDate { get; set; }
        public int DocumentReference { get; set; }
        public string? PaymentMethod { get; set; }
        public string DetailComment { get; set; } = default!;
    }
}
