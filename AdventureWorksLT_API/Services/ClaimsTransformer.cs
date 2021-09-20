using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace AdventureWorksLT_API.Services
{
    public class ClaimsTransformer : IClaimsTransformation
    {
		public ClaimsTransformer(IUserRoleManager userRoleManager)
		{
			UserRoleManager = userRoleManager;
		}

		private IUserRoleManager UserRoleManager { get; }

		public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
		{
            if (principal.Identity is WindowsIdentity identity)
                await AddRolesToIdentity(identity);

            return principal;
		}

		private async Task AddRolesToIdentity(WindowsIdentity identity)
		{
			foreach (var role in UserRoleManager.GetRoles())
			{
				identity.AddClaim(new Claim(identity.RoleClaimType, role));
			}

			await Task.CompletedTask;
		}
	}
}
