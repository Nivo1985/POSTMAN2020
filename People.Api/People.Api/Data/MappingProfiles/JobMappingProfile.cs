using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using People.Api.Data.Entities;
using People.Api.Data.Models;

namespace People.Api.Data.MappingProfiles
{
    public class JobMappingProfile: Profile
    {
        public JobMappingProfile()
        {
            CreateMap<Job, JobModel>().ReverseMap();
        }
    }
}
