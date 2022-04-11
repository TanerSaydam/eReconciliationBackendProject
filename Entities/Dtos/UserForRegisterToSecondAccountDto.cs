using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserForRegisterToSecondAccountDto : UserForRegister , IDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
    }
}
