namespace Model.Efatura.InvoiceModels
{
    public class LineTaxDto
    {
        public string? Tax_Code { get; set; }
        public string? Tax_Name { get; set; }
        public decimal? Tax_Base { get; set; }
        public decimal? Tax_Perc { get; set; }
        public decimal? Tax_Amnt { get; set; }
        public string? Tax_Exem { get; set; }
        public int? Tax_Exem_Code { get; set; }
    }
}
