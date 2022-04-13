using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserReletionshipDto : IDto
    {
        public int Id { get; set; }
        public int AdminUserId { get; set; }
        public string AdminUserName { get; set; }
        public string AdminMail { get; set; }
        public DateTime AdminAddedAt { get; set; }
        public bool AdminIsActive { get; set; }
        public int UserUserId { get; set; }
        public string UserUserName { get; set; }
        public string UserMail { get; set; }
        public string UserMailValue { get; set; }
        public DateTime UserAddedAt { get; set; }
        public bool UserIsActive { get; set; }        
        public List<Company> Companies { get; set; }
    }
}
