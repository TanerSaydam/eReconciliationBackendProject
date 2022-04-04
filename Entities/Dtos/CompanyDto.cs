using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CompanyDto
    {
        public Company Company { get; set; }
        public int UserId { get; set; }
    }
}
