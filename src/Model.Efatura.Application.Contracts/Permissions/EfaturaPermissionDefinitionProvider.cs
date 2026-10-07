using Model.Efatura.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Model.Efatura.Permissions;

public class EfaturaPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EfaturaPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(EfaturaPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EfaturaResource>(name);
    }
}
