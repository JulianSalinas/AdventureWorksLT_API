using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksLT_API.Models
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string CompanyName { get; set; }

        public string EmailAddress { get; set; }
        
        public string Phone { get; set; }
    }
}
