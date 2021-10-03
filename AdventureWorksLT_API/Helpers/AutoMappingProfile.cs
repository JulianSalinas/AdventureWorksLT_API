using AdventureWorksLT_API.Models;
using AdventureWorksLT_DA.Entities;
using AutoMapper;

namespace AdventureWorksLT_API.Helpers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Customer, CustomerDTO>();
        }
    }
}