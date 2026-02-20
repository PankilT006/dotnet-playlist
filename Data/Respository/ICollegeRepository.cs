using System;
using System.Linq.Expressions;

namespace WebApi.Data.Respository;

public interface ICollegeRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);
    Task<T> CreateAsync(T dbRecord);
    Task<T> UpdateAsync(T dbRecord);
    Task<bool> DeleteAsync(T students);
}

