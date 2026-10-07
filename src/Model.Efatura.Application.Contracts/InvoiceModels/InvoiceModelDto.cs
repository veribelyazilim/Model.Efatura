using System.Collections.Generic;

namespace Model.Efatura.InvoiceModels
{
    public class InvoiceModelDto
    {
        public InvoiceHeaderDto? invoiceheader { get; set; }
        public SupplierDto? supplier { get; set; }
        public SupplierAgentPartyDto? supplierAgent { get; set; }
        public CustomerDto? customer { get; set; }
        public CustomerAgentPartyDto? customerAgent { get; set; }
        public List<DespatchDto> despatchs { get; set; }
        public List<InvoiceLineDto> invoiceLines { get; set; }
        public List<BillingReferenceDto> billingReferences { get; set; }
        public BuyerCustomerPartyDto BuyerCustomerParty { get; set; }
        public List<PaymentMeansDto> PaymentMeans { get; set; }
    }
}
