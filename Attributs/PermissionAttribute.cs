using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NewSky.API.Services.Interface;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class PermissionAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _permissionName;

    public PermissionAttribute(string permissionName)
    {
        _permissionName = permissionName;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
        var user = await userService.GetCurrentUserAsync(includePermissions: true);
        var hasPermission = userService.HasPermission(user, _permissionName);
        if (!hasPermission)
        {
            context.Result = new ForbidResult(); 
            return;
        }

        await next();
    }
}
