using Model.Efatura.InvoiceModels;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Model.Efatura.InvoiceModelHt
{
    public interface IInvoiceModelHtService : IApplicationService
    {
        Task<PagedResultDto<InvoiceModelDto>> GetEfaturaList(InvoiceModelCo filter);
        Task<PagedResultDto<InvoiceModelDto>> GetEfaturaListTest(InvoiceModelCo filter);
    }
}
