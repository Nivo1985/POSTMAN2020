using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace People.Api.Data.Entities
{
    public class Period
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PeriodId { get; set; }

        [Required]
        public DateTime Begin { get; set; }
        public DateTime? End { get; set; }
    }
}
