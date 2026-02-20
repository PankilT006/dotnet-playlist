using Microsoft.EntityFrameworkCore;
using WebApi.Data.Respository;

namespace WebApi.Data.Repository
{
    public class DepartmentRepository : CollegeRepository<Department>,IDepartmentRepository
    {
        private readonly CollegeDBContext _context;

        public DepartmentRepository(CollegeDBContext context) : base(context)
        {
            _context = context;
        }

       
    }
}
