using Microsoft.Extensions.Localization;
using Model.Efatura.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Model.Efatura;

[Dependency(ReplaceServices = true)]
public class EfaturaBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<EfaturaResource> _localizer;

    public EfaturaBrandingProvider(IStringLocalizer<EfaturaResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
