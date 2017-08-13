namespace IdentitySample.Authorization
{
    using IdentitySample.Data.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using System.Threading.Tasks;

    public class AdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Article>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, Article resource)
        {
            //Admin can do anything
            if (context.User != null && context.User.IsInRole(Constants.AdminRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
