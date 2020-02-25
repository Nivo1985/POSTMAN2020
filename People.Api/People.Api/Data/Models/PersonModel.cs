using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace People.Api.Data.Models
{
    public class PersonModel
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public ICollection<JobModel> Jobs { get; set; } = new List<JobModel>();

        public int NumberOfJobs { get; set; }
    }
}
