namespace Model.Efatura.InvoiceModels
{
    public class CustomerDto
    {
        public Customer_IdentificationDto[] customerIdentificationsOther { get; set; }
        public string? IdentificationID { get; set; }
        public string? PartyName { get; set; }
        public string? TaxSchemeName { get; set; }
        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public string? CitySubdivisionName { get; set; }
        public string? StreetName { get; set; }
        public string? ElectronicMail { get; set; }
        public string? WebsiteURI { get; set; }
        public string? Telefax { get; set; }
        public string? Telephone { get; set; }
        public string? Person_FirstName { get; set; }
        public string? Person_FamilyName { get; set; }
    }
}
