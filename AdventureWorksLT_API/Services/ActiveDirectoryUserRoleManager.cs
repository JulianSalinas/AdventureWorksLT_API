using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AdventureWorksLT_API.Services
{
    public class ActiveDirectoryUserRoleManager : IUserRoleManager
	{
		const string MemberOfAttribute = "memberOf";
		const string CanonicalName_Regex = @"(?:CN=(?<cn>[(\w|\s)]+)),.*,?OU=(?:Security Groups),?.*";
		const string CanonicalName_GroupName = "cn";

		private HttpContext HttpContext { get; }

		public ActiveDirectoryUserRoleManager(ILogger<ActiveDirectoryUserRoleManager> logger, IHttpContextAccessor httpContextAccessor)
		{
			HttpContext = httpContextAccessor.HttpContext;
		}

        public IEnumerable<string> GetRoles()
		{
			string userName = HttpContext.User?.Identity?.Name;
			string ldapUrl = "127.0.0.1";

			if (string.IsNullOrWhiteSpace(userName))
				return Array.Empty<string>();

			string sanitizedUser = userName.Contains(@"\") ? userName.Substring(userName.IndexOf(@"\") + 1) : userName;

            using var entry = new DirectoryEntry($"LDAP://{ldapUrl}");
            using var searcher = new DirectorySearcher(entry);
            searcher.Filter = $"(sAMAccountName={sanitizedUser})";
            searcher.PropertiesToLoad.Add(MemberOfAttribute);
            var searchResult = searcher.FindOne();

            var memberOf = (searchResult != null && searchResult.Properties.Contains(MemberOfAttribute)) ? searchResult.Properties[MemberOfAttribute] : null;

            List<string> roles = new();

            foreach (var role in memberOf)
            {
                var match = Regex.Match(role.ToString(), CanonicalName_Regex, RegexOptions.IgnoreCase);
                if (match.Success)
                    roles.Add(match.Groups[CanonicalName_GroupName].Value);
            }

            return roles;
		}
	}
}
