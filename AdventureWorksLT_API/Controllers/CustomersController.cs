using System.Collections.Generic;
using AdventureWorksLT_API.Models;
using AdventureWorksLT_DA;
using AdventureWorksLT_DA.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System.Linq;
using AutoMapper;

namespace AdventureWorksLT_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private static readonly string[] RequiredScope = { "AdventureWorksLT_API.Read" };

        private readonly IMapper _mapper;
        private readonly ILogger<CustomersController> _logger;
        private readonly AdventureWorksContext _context;

        public CustomersController(IMapper mapper, ILogger<CustomersController> logger, AdventureWorksContext context)
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get list of customers using pagination
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetCustomersByPage")]
        public IActionResult GetCustomersByPage([FromQuery]PageRequest request)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(RequiredScope);

            var totalRecords = _context.Customer.Count();

            const string defaultOrderBy = "lastName";
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

            var records = _mapper.Map<List<Customer>, List<CustomerDTO>>(customers);

            var response = new PageResponse<CustomerDTO> { 
                TotalRecords = totalRecords,
                PageRecords = records
            };

            return Ok(response);
        }


    }
}
