using Model.Efatura.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Model.Efatura.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EfaturaController : AbpControllerBase
{
    protected EfaturaController()
    {
        LocalizationResource = typeof(EfaturaResource);
    }
}
