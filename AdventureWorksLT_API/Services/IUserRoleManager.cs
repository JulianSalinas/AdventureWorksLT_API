using System.Collections.Generic;

namespace AdventureWorksLT_API.Services
{
	public interface IUserRoleManager
	{
		IEnumerable<string> GetRoles();
	}
}
