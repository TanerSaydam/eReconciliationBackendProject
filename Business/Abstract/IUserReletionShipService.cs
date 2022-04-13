using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserReletionShipService
    {
        void Add(UserReletionship userReletionship);
        void Update(UserReletionship userReletionship);
        void Delete(UserReletionship userReletionship);
        IDataResult<List<UserReletionshipDto>> GetListDto(int adminUserId);
        IDataResult<UserReletionshipDto> GetById(int userUserId);
        List<UserReletionship> GetList(int userId);
    }
}
