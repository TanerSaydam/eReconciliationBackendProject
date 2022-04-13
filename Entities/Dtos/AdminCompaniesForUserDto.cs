using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AdminCompaniesForUserDto : Company , IDto 
    {
        public bool IsTrue { get; set; }
    }
}
