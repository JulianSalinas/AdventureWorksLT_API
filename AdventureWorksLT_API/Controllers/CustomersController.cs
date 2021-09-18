using AdventureWorksLT_API.Models;
using AdventureWorksLT_DA;
using AdventureWorksLT_DA.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System.Linq;

namespace AdventureWorksLT_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        static readonly string[] requiredScope = new string[] { "AdventureWorksLT_API.Read" };

        private readonly ILogger<CustomersController> _logger;
        private readonly AdventureWorksContext _context;

        public CustomersController(ILogger<CustomersController> logger, AdventureWorksContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get list of customers using pagination
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "AdventureWorks.Admin")]
        [HttpGet("GetCustomersByPage")]
        public IActionResult GetCustomersByPage([FromQuery]PageRequest request)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(requiredScope);

            var totalRecords = _context.Customer.Count();

            var defaultOrderBy = "lastName";
            var orderBy = string.IsNullOrWhiteSpace(request.OrderBy) ? defaultOrderBy : request.OrderBy;
            var orderByDir = request.OrderByDir ?? "asc";

            var source = _context.Customer;

            var query = orderByDir == "asc"
                ? source.OrderByDynamic(c => $"c.{orderBy}")
                : source.OrderByDescendingDynamic(c => $"c.{orderBy}");

            var customers = query
                .Skip((request.PageNumber + 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var response = new PageResponse<Customer> { 
                TotalRecords = totalRecords,
                PageRecords = customers
            };

            return Ok(response);
        }

    }
}
