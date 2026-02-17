using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Data.Repository
{
    public interface IStudentRepository
    {
        Task<List<Students>> GetStudentsAsync();
        Task<Students?> GetStudentByIdAsync(int id);
        Task<int> CreateStudentAsync(Students student);
        Task<bool> UpdateStudentAsync(Students student);
        Task<bool> DeleteStudentAsync(int id);
    }
}
