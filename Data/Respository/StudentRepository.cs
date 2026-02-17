using Microsoft.EntityFrameworkCore;

namespace WebApi.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _context;

        public StudentRepository(CollegeDBContext context)
        {
            _context = context;
        }

        public async Task<List<Students>> GetStudentsAsync()
        {
            return await _context.Students
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<Students?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> CreateStudentAsync(Students student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> UpdateStudentAsync(Students student)
        {
            var existingStudent = await _context.Students
                                                .FirstOrDefaultAsync(s => s.Id == student.Id);

            if (existingStudent == null)
                return false;

            existingStudent.StudentName = student.StudentName;
            existingStudent.Email = student.Email;
            existingStudent.Address = student.Address;
            existingStudent.MobileNumber = student.MobileNumber;
            existingStudent.Admission = student.Admission;
 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
