using Business.Abstract;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BaBsRecoinciliationManager : IBaBsRecoinciliationService
    {
        private readonly IBaBsRecoinciliationDal _baBsRecoinciliationDal;

        public BaBsRecoinciliationManager(IBaBsRecoinciliationDal baBsRecoinciliationDal)
        {
            _baBsRecoinciliationDal = baBsRecoinciliationDal;
        }
    }
}
