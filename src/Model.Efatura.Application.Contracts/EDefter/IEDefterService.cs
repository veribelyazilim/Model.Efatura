using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Model.Efatura.EDefter
{
    public interface IEDefterService : IApplicationService
    {
        Task<EDefterResultDto> GetEDefterList(EDefterCo filter);
    }
}
