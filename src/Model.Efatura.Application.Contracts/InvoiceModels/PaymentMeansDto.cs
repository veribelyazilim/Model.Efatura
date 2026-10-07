using System;

namespace Model.Efatura.InvoiceModels
{
    public class PaymentMeansDto
    {
        public string PaymentMeansCode { get; set; }      // Ödeme şekli kodu (GİB kod listesi, örn: "42" havale, "48" kredi kartı vb.)
        public string InstructionNote { get; set; }        // Açıklama notu
        public string PaymentChannelCode { get; set; }      // Ödeme kanalı kodu
        public DateTime PaymentDueDate { get; set; }        // Son ödeme tarihi
        public string PayeeFinancialAccount { get; set; }   // IBAN
        public string PayeeFinancialCurrencyCode { get; set; } // Hesap para birimi (TRY, USD, ...)
    }
}
