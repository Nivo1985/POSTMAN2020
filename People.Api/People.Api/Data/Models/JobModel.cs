using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace People.Api.Data.Models
{
    public class JobModel
    {
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }

        public PeriodModel Period { get; set; }

        public int PersonId { get; set; }
    }
}
