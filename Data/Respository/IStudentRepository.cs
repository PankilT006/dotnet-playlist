using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data.Respository;

namespace WebApi.Data.Repository
{
    public interface IStudentRepository: ICollegeRepository<Students>
    {

    }
}
