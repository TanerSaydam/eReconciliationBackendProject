using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserReletionshipDal : IEntityRepository<UserReletionship>
    {
        List<UserReletionshipDto> GetListDto(int adminUserId);
        UserReletionshipDto GetById(int userUserId);
    }
}
