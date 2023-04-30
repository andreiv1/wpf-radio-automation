using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class Schedule : BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TemplateId { get; set; }
        public Template Template { get; set; }  
    }
}
