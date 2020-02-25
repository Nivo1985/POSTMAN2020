using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using People.Api.Data.Entities;

namespace People.Api.Data
{
    public static class InitialData
    {
        public static void Seed(this PeopleContext dbContext)
        {
            if (!dbContext.People.Any())
            {
                dbContext.People.Add(new Person()
                {
                    FirstName = "Karol",
                    LastName = "Rogowski",
                    DateOfBirth = new DateTime(1985,12,8),
                    DateOfDeath = null,
                    Jobs = new List<Job>()
                    {
                        new Job()
                        {
                            Title = "Senior Web Developer",
                            Company = "DevCore",
                            Description = "A Web Developer is responsible for the coding, design and layout of a website according to a company's specifications. As the role takes into consideration user experience and function, a certain level of both graphic design and computer programming is necessary.",
                            Period = new Period()
                            {
                                Begin = new DateTime(2009, 3,1),
                                End = new DateTime(2014,3,31)
                            }
                        },
                        new Job()
                        {
                            Title = "Developer",
                            Company = "Transition Technologies",
                            Period = new Period()
                            {
                                Begin = new DateTime(2014,4,1),
                                End = new DateTime(2017,3,31)
                            }
                        },
                        new Job()
                        {
                            Title = "Tech Lead",
                            Company = "SoftwareHut",
                            Period = new Period()
                            {
                                Begin = new DateTime(2017,4,1),
                                End = null
                            }
                        },
                        new Job()
                        {
                            Title = "Video Author",
                            Company = "VideoPoint",
                            Period = new Period()
                            {
                                Begin = new DateTime(2019,12,19),
                                End = null
                            }
                        }
                    }
                });

                dbContext.People.Add(new Person()
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    DateOfBirth = new DateTime(1901,11,10),
                    DateOfDeath = new DateTime(1969, 4, 1)
                });
            }

            dbContext.SaveChanges();
        }
    }
}
