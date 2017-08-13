namespace IdentitySample.Authorization
{
    using IdentitySample.Data.Entities;
    using IdentitySample.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class UserModAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Article>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserModAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, Article resource)
        {
            if(context.User == null)
            {
                return Task.CompletedTask;
            }

            // user mod cannot delete, activate or deactivate any articles
            if(requirement.Name == OperationRequirements.DeleteOperationName ||
                requirement.Name == OperationRequirements.ActivateOperationName ||
                requirement.Name == OperationRequirements.DeactivateOperationName)
            {
                return Task.CompletedTask;
            }

            // user mod can only edit his articles
            if (resource.CreateBy == _userManager.GetUserName(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
