using System.Collections.Generic;

namespace Model.Efatura.InvoiceModels
{
    public class InvoiceLineDto
    {
        public int? ID { get; set; }
        public string? Item_Name { get; set; }
        public string? SellersItemIdentification { get; set; }
        public string? ManufacturersItemIdentification { get; set; }
        public decimal? Quantity_Amount { get; set; }
        public decimal? Price_Amount { get; set; }
        public string? Quantity_Unit_User { get; set; }
        public decimal? Allowance_Percent { get; set; }
        public decimal? Allowance_Amount { get; set; }
        public decimal? Price_Total { get; set; }
        public List<LineTaxDto> lineTaxes { get; set; }
        public List<ItemInstanceDto> itemInstances { get; set; }
    }
}
