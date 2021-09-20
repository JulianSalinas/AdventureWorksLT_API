using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace AdventureWorksLT_API.Services
{
    public class TranslatedUserRoleManager : IUserRoleManager
	{
		public TranslatedUserRoleManager(IHttpContextAccessor httpContextAccessor)
		{
			HttpContext = httpContextAccessor.HttpContext;
		}

		private HttpContext HttpContext { get; }

		public IEnumerable<string> GetRoles()
		{
            if (HttpContext.User?.Identity is not WindowsIdentity identity)
                return Array.Empty<string>();

			string domain = Regex.Replace(identity.Name, "(.*)\\\\.*", "$1", RegexOptions.None);

			return identity.Groups
				.Select(group => group.Translate(typeof(NTAccount)).Value)
				.Where(rol => rol.StartsWith(domain))
				.Select(rol => Path.GetFileNameWithoutExtension(rol))
				.ToList();
		}
	}
}
