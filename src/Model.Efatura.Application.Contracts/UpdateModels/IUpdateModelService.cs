using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Model.Efatura.UpdateModels
{
    public interface IUpdateModelService : IApplicationService
    {
        public Task UpdateInvoiceStatus(UpdateModelCo filter);
        public Task UpdateInvoiceStatusTest(UpdateModelCo filter);
    }
}
