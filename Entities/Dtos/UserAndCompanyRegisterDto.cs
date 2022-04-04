using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserAndCompanyRegisterDto : IDto
    {    
        public UserForRegister UserForRegister { get; set; }
        public Company Company { get; set; }
    }
}
