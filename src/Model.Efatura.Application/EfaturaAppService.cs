using Model.Efatura.Localization;
using Volo.Abp.Application.Services;

namespace Model.Efatura;

/* Inherit your application services from this class.
 */
public abstract class EfaturaAppService : ApplicationService
{
    protected EfaturaAppService()
    {
        LocalizationResource = typeof(EfaturaResource);
    }
}
