using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using People.Api.Data.Entities;
using People.Api.Data.Models;

namespace People.Api.Data.MappingProfiles
{
    public class PersonMappingProfile: Profile
    {
        public PersonMappingProfile()
        {
            CreateMap<Person, PersonModel>().ReverseMap();
        }
    }
}
