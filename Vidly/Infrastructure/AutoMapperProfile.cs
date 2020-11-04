using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.DTO;
using Vidly.Models;

namespace Vidly.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Domain to DTO
            CreateMap<Customer, CustomerDTO>();
            CreateMap<Movie, MovieDTO>();
            CreateMap<MembershipType, MembershipTypeDTO>();

            // DTO to Domain
            CreateMap<CustomerDTO, Customer>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<MovieDTO, Movie>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<MembershipTypeDTO, MembershipType>()
                .ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}