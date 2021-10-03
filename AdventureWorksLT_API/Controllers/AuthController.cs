using System.Linq;
using AdventureWorksLT_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdventureWorksLT_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly IUserRoleManager _userRoleManager;

        public AuthController(ILogger<AuthController> logger, IUserRoleManager roleManager)
        {
            _logger = logger;
            _userRoleManager = roleManager;
        }

        /// <summary>
        /// Get list of roles that belong to the authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoles")]
        public IActionResult GetCustomersByPage()
        {
            var userRoles = _userRoleManager.GetRoles().ToList();
            return Ok(userRoles);
        }

    }
}
