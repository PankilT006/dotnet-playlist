using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Data.Respository;
using WebApi.Migrations;

namespace WebApi.Data.Repository;

public interface IDepartmentRepository : ICollegeRepository<Department>
{
   
}
