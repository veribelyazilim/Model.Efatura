using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Model.Efatura.InvoiceModels
{
    public interface IInvoiceModelService : IApplicationService
    {
        Task<PagedResultDto<InvoiceModelDto>> GetEfaturaList(InvoiceModelCo filter);
    }
}
