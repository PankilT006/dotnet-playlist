using System;
using System.IO.Compression;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data.Respository;

public class CollegeRepository<T> : ICollegeRepository<T> where T : class
{
    private readonly CollegeDBContext _context;
    private readonly DbSet<T> _dbSet;
    public CollegeRepository(CollegeDBContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public async Task<T> CreateAsync(T dbRecord)
    {
        _dbSet.Add(dbRecord);
        await _context.SaveChangesAsync();
        return dbRecord;
    }

    public async Task<bool> DeleteAsync(T students)
    {
        // Console.WriteLine($"Attempting to delete record:{students.Id}");

        if (students == null)
            return false;

        _dbSet.Remove(students);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public Task<List<T>> GetAllAsync()
    {
        return _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T> UpdateAsync(T dbRecord)
    {
        _dbSet.Update(dbRecord);
        await _context.SaveChangesAsync();
        return dbRecord;
    }


}
