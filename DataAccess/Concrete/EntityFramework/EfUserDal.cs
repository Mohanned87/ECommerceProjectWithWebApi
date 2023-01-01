using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfBaseRepository<User, ECommerceProjectWithWebAPIContext>, IUserDal
    {
    }
}
