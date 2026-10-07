using Microsoft.AspNetCore.Authorization;
using Model.Efatura.AuxiliaryManager;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Model.Efatura.EDefter
{
    [Authorize]
    public class EDefterService : ApplicationService, IEDefterService
    {
        private readonly IAuxiliaryManager _sqlManager;

        public EDefterService(IAuxiliaryManager sqlManager)
        {
            _sqlManager = sqlManager;
        }

        public async Task<EDefterResultDto> GetEDefterList(EDefterCo filter)
        {
            if (CurrentUser.UserName != "HtUser" && CurrentUser.UserName != "admin")
            {
                throw new UserFriendlyException("Bu servisi kullanmaya yetkili değilsiniz.");
            }

            if (filter.StartDate == null || filter.EndDate == null)
            {
                throw new UserFriendlyException("Başlangıç ve bitiş tarihi zorunludur.");
            }

            if (filter.StartDate.Value.Date > filter.EndDate.Value.Date)
            {
                throw new UserFriendlyException("Başlangıç tarihi bitiş tarihinden büyük olamaz.");
            }

            if (filter.PageSize is <= 0)
            {
                throw new UserFriendlyException("Sayfa boyutu sıfırdan büyük olmalıdır.");
            }

            if (filter.PageNumber is <= 0)
            {
                throw new UserFriendlyException("Sayfa numarası 1 veya daha büyük olmalıdır.");
            }

            if (filter.PageNumber != null && filter.PageSize == null)
            {
                throw new UserFriendlyException("Sayfa numarası için sayfa boyutu da verilmelidir.");
            }

            var result = await _sqlManager.GetEDefterList(filter);

            result.LastLineNumber = filter.PreviousLineNumber + result.TotalCount;
            result.PageCount = filter.PageSize is > 0
                ? (int)((result.TotalCount + (long)filter.PageSize.Value - 1) / filter.PageSize.Value)
                : null;

            return result;
        }
    }
}
