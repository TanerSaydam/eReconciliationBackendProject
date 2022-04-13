using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserReletionShipManager : IUserReletionShipService
    {
        private readonly IUserReletionshipDal _userReletionshipDal;

        public UserReletionShipManager(IUserReletionshipDal userReletionshipDal)
        {
            _userReletionshipDal = userReletionshipDal;
        }

        public void Add(UserReletionship userReletionship)
        {
            _userReletionshipDal.Add(userReletionship);
        }

        public void Delete(UserReletionship userReletionship)
        {
            _userReletionshipDal.Delete(userReletionship);
        }

        public List<UserReletionship> GetList(int userId)
        {
            return _userReletionshipDal.GetList(p=> p.UserUserId == userId);
        }

        public IDataResult<UserReletionshipDto> GetById(int userUserId)
        {
            return new SuccesDataResult<UserReletionshipDto>(_userReletionshipDal.GetById(userUserId));
        }

        public IDataResult<List<UserReletionshipDto>> GetListDto(int adminUserId)
        {
            return new SuccesDataResult<List<UserReletionshipDto>>(_userReletionshipDal.GetListDto(adminUserId));
        }

        public void Update(UserReletionship userReletionship)
        {
            _userReletionshipDal.Update(userReletionship);
        }
    }
}
