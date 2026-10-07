using Volo.Abp.Settings;

namespace Model.Efatura.Settings;

public class EfaturaSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EfaturaSettings.MySetting1));
    }
}
