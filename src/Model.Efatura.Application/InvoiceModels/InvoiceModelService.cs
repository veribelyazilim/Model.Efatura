using Microsoft.AspNetCore.Authorization;
using Model.Efatura.AuxiliaryManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Model.Efatura.InvoiceModels
{
    [Authorize]
    public class InvoiceModelService : ApplicationService, IInvoiceModelService
    {
        private readonly IAuxiliaryManager _sqlManager;
        public InvoiceModelService(IAuxiliaryManager sqlManager)
        {
            _sqlManager = sqlManager;
        }

        public async Task<PagedResultDto<InvoiceModelDto>> GetEfaturaList(InvoiceModelCo filter)
        {
            if (CurrentUser.UserName != "TestUser" && CurrentUser.UserName != "admin" && CurrentUser.UserName != "PConUser")
            {
                throw new UserFriendlyException("Bu servisi kullanmaya yetkili değilsiniz.");
            }

            try
            {
                var list = await _sqlManager.GetEfaturaList(filter);

                var invList = PrepareModels(list);

                return new PagedResultDto<InvoiceModelDto>()
                {
                    Items = invList,
                    TotalCount = list.TotalCount
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        private List<InvoiceModelDto> PrepareModels(ProcReturnCombinedDto procDto)
        {
            InvoiceModelDto currentModel = null;
            var invoiceList = new List<InvoiceModelDto>();

            SupplierDto comp = new();
            string currentUuid = "";

            int lineNum = 0;

            try
            {
                comp.supplierParty = procDto.ProcCustomer;

                foreach (var line in procDto.ProcLines)
                {
                    string uuid = line.U_efatura_uuid;

                    if (uuid != currentUuid)
                    {
                        currentUuid = uuid;
                        lineNum = 0;

                        List<DespatchDto> despatchs = new();

                        currentModel = new InvoiceModelDto
                        {
                            invoiceheader = new InvoiceHeaderDto
                            {
                                UUID = uuid,
                                CompanyCode = line.CompanyCode,
                                Invoice_ID = line.NumAtCard,
                                SystemDocNum = line.DocNum,
                                IssueDate = line.DocDate,
                                IssueTime = line.DocTime,
                                ProfileID = line.ProfileID,
                                InvoiceTypeCode = line.DocObjectCode,
                                LineExtensionAmount = line.LineExtensionAmount,
                                AllowanceTotalAmount = line.AllowanceTotalAmount,
                                TaxInclusiveAmount = line.DocTotal,
                                PayableAmount = line.DocTotal,
                                CalculationRate = line.DocRate,
                                Sent = line.U_efatura_sent == 1,
                                DocumentCurrencyCode = line.DocumentCurrencyCode
                            }
                        };

                        string comm = line.Comments;
                        if (!string.IsNullOrEmpty(comm))
                        {
                            List<NoteModelDto> comments = new();
                            comments.Add(new NoteModelDto { Note = comm });

                            currentModel.invoiceheader.Notes = comments;
                        }

                        currentModel.supplier = comp;

                        string cardName = line.Unvan;

                        currentModel.customer = new CustomerDto
                        {
                            IdentificationID = line.IdentificationID,
                            PartyName = cardName,
                            TaxSchemeName = line.AdditionalID,
                            CountryName = line.CountryName,
                            CitySubdivisionName = line.BillToCounty,
                            StreetName = line.BillToStreet,
                            ElectronicMail = line.EmailAddress,
                            WebsiteURI = line.Website,
                            Telefax = line.Fax,
                            Telephone = line.Phone1,
                            CityName = line.BillToState
                        };

                        if (line.VergiKimlikNo != null)
                        {
                            var (first, last) = SplitCardName(cardName);
                            currentModel.customer.Person_FirstName = first;
                            currentModel.customer.Person_FamilyName = last;

                            //string[] nameParts = cardName.Split(' ');
                            //if (nameParts.Length > 1)
                            //{
                            //    currentModel.customer.Person_FirstName = string.Join(" ", nameParts.Take(nameParts.Length - 1));
                            //    currentModel.customer.Person_FamilyName = nameParts.Last();
                            //}
                        }

                        currentModel.despatchs = despatchs;

                        List<InvoiceLineDto> lineList = new();

                        currentModel.invoiceLines = lineList;

                        invoiceList.Add(currentModel);

                        List<BillingReferenceDto> billingReferences = new();

                        currentModel.billingReferences = billingReferences;

                        //uuid != currentUuid END
                    }

                    InvoiceLineDto iLine = new InvoiceLineDto
                    {
                        ID = lineNum,
                        Item_Name = line.Item_Name,
                        Quantity_Amount = line.Quantity_Amount,
                        Price_Amount = line.PriceAmount,
                        Allowance_Percent = line.Allowance_Percent,
                        Allowance_Amount = line.AllowanceAmount,
                        Price_Total = line.Price_Total,
                        Quantity_Unit_User = line.Quantity_Unit_User,
                        SellersItemIdentification = line.MalzemeKodu,
                        ManufacturersItemIdentification = line.UreticiMalzemeKodu
                    };

                    lineNum++;

                    List<LineTaxDto> rowTaxList = new List<LineTaxDto>();

                    //Kdv'nin her şartta eklenmesi istendi. Bu nedenle > 0 şartını ve else kısmını commentledim.
                    //İstisna kodu varsa zaten sorun yok. İstasna kodu yoksa hatalı faturadır, Gib'ten dönsün, kontrol edelim diyorlar.
                    //if (line.KdvTutari > 0)
                    {
                        LineTaxDto tax = new();

                        tax.Tax_Code = "0015";
                        tax.Tax_Name = "KDV";
                        tax.Tax_Base = line.KdvMatrahi;
                        tax.Tax_Perc = line.KdvOrani;
                        tax.Tax_Amnt = line.KdvTutari;

                        if (tax.Tax_Amnt == 0)
                        {
                            if (!string.IsNullOrEmpty(line.IstisnaSebebiKod))
                            {
                                tax.Tax_Exem = line.IstisnaSebebi;
                                tax.Tax_Exem_Code = int.TryParse(line.IstisnaSebebiKod, out var exemCode)
                                ? exemCode
                                : (int?)null;
                                tax.Tax_Amnt = 0;
                                tax.Tax_Perc = 0;

                                currentModel.invoiceheader.InvoiceTypeCode = "ISTISNA";
                            }
                        }
                        //else
                        //{
                        //    if (line.Tax_Amnt <= 0)
                        //    {
                        //        tax.Tax_Exem = "KDV - İstisna Olmayan Diğer";
                        //        tax.Tax_Exem_Code = 351;
                        //        tax.Tax_Perc = 0;
                        //    }
                        //}
                        
                        rowTaxList.Add(tax);
                    }
                    
                    if (line.OtvTutari > 0)
                    {
                        rowTaxList.Add(new LineTaxDto
                        {
                            Tax_Code = "0074",
                            Tax_Name = "OTV",
                            Tax_Base = line.OtvMatrahi,
                            Tax_Perc = line.OtvOrani,
                            Tax_Amnt = line.OtvTutari
                        });
                    }

                    /*
                    LineTaxDto tax = new LineTaxDto
                    {
                        Tax_Code = line.Tax_Code,
                        Tax_Name = line.Tax_Name,
                        Tax_Base = line.Tax_Base,
                        Tax_Perc = line.Tax_Perc,
                        Tax_Amnt = line.Tax_Amnt
                    };
                    */
                    

                    
                    iLine.lineTaxes = rowTaxList;

                    if (!string.IsNullOrEmpty(line.DespatchDocumentID))
                    {
                        DespatchDto despatch = new();
                        despatch.DespatchDocumentID = $"{line.DespatchDocumentID:D13}";

                        if (!currentModel.despatchs.Any(d => d.DespatchDocumentID == despatch.DespatchDocumentID))
                        {
                            despatch.DespatchDocumentIssueDate = line.DespatchDocumentIssueDate;
                            currentModel.despatchs.Add(despatch);
                        }
                    }

                    /*
                    string despatchFolioPref = line.DespatchFolioPref;

                    if (!string.IsNullOrEmpty(despatchFolioPref))
                    {
                        int despatchFolioNum = line.DespatchFolioNum;

                        DespatchDto despatch = new();

                        despatch.DespatchDocumentID = $"{despatchFolioPref}{despatchFolioNum:D13}";

                        if (!currentModel.despatchs.Any(d => d.DespatchDocumentID == despatch.DespatchDocumentID))
                        {
                            despatch.DespatchDocumentIssueDate = line.DespatchDocumentIssueDate;
                            currentModel.despatchs.Add(despatch);
                        }
                    }
                    */

                    string refInvId = line.RefInvId;

                    if (!string.IsNullOrEmpty(refInvId))
                    {
                        // Zaten mevcutsa ekleme
                        bool exists = currentModel.billingReferences.Any(b => b.ID == refInvId);
                        if (!exists)
                        {
                            string refIssueDate = line.RefIssueDate;
                            if (!string.IsNullOrEmpty(refIssueDate))
                            {
                                BillingReferenceDto refInv = new BillingReferenceDto
                                {
                                    ID = refInvId,
                                    IssueDate = refIssueDate,
                                    DocumentTypeCode = "IADE"
                                };
                                currentModel.billingReferences.Add(refInv);
                            }
                        }
                    }

                    currentModel.invoiceLines.Add(iLine);

                    //KAMU
                    if (line.Senaryo == "KAMU")
                    {
                        currentModel.invoiceheader.ProfileID = "KAMU";
                        BuyerCustomerPartyDto bcd = new()
                        {
                            IdentificationID = line.IdentificationID,
                            PartyName = line.Unvan,
                            CountryName = line.CountryName,
                            CityName = line.BillToState
                        };

                        currentModel.BuyerCustomerParty = bcd;

                        string iban = line.GondericiFirmaIban.Length >= 26 ? line.GondericiFirmaIban[^26..] : line.GondericiFirmaIban;

                        List<PaymentMeansDto> paymentMeans = new();
                        paymentMeans.Add(new PaymentMeansDto
                        {
                            PaymentMeansCode = "42",
                            InstructionNote = line.GondericiFirmaIban,
                            PaymentChannelCode = "",
                            PaymentDueDate = DateTime.ParseExact(
                                line.DocDate,
                                "MM/dd/yyyy HH:mm:ss",
                                CultureInfo.InvariantCulture),
                            PayeeFinancialAccount = iban, //İBAN
                            PayeeFinancialCurrencyCode = line.DocumentCurrencyCode
                        });

                        currentModel.PaymentMeans = paymentMeans;
                    }

                }

                return invoiceList;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public static (string FirstName, string LastName) SplitCardName(string cardName)
        {
            if (string.IsNullOrWhiteSpace(cardName))
            {
                // Boş/null veri: burada kendi fallback stratejinizi tanımlayın
                return ("N/A", "N/A");
            }

            // Baş/son boşlukları temizle, birden fazla boşluğu teke indir
            var parts = cardName
                .Trim()
                .Split((char[])null, StringSplitOptions.RemoveEmptyEntries); // \s+ mantığıyla böler

            string firstName;
            string lastName;

            if (parts.Length == 1)
            {
                // Tek kelime: hem ad hem soyad olarak kullan
                firstName = parts[0];
                lastName = parts[0];
            }
            else
            {
                lastName = parts[^1];
                firstName = string.Join(" ", parts.Take(parts.Length - 1));
            }

            // Minimum 2 karakter kuralını garanti altına al
            firstName = EnsureMinLength(firstName);
            lastName = EnsureMinLength(lastName);

            return (firstName, lastName);
        }

        private static string EnsureMinLength(string value, int minLength = 2)
        {
            if (string.IsNullOrEmpty(value))
                return new string('X', minLength); // ya da başka bir placeholder

            if (value.Length >= minLength)
                return value;

            // Kısa olan değeri tekrarlayarak uzat (ör. "x" -> "xx", "A" -> "AA")
            var sb = new System.Text.StringBuilder(value);
            while (sb.Length < minLength)
                sb.Append(value);

            return sb.ToString();
        }
    }
}
