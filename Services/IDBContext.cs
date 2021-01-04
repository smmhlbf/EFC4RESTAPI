using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFC4RESTAPI.Models.Super;
using Microsoft.EntityFrameworkCore;

namespace EFC4RESTAPI.Services
{
    public interface IDBContext
    {
        // GET
        Task<IEnumerable<T>> GetListAsync<T>(DbSet<T> sets, Func<T, bool> expression = null) where T : ISuper;
        // GET{id}
        Task<T> GetOneAsync<T>(DbSet<T> sets, Guid id) where T : ISuper;
        // POST IEnumerable<T>
        Task<Tuple<bool, string>> CreateManyAsync<T>(DbSet<T> sets, IEnumerable<T> entities) where T : ISuper;
        // POST T
        Task<Tuple<bool, string>> CreateOneAsync<T>(DbSet<T> sets, T entity) where T : ISuper;
        // PUT{id} T
        Task<Tuple<bool, string>> ModifyAsync<T>(DbSet<T> sets, Guid id, T entity) where T : ISuper;
        // DELETE IEnumerable<T>
        Task<Tuple<bool, string>> DeleteManyAsync<T>(DbSet<T> sets, IEnumerable<int> ids) where T : ISuper;
        // DELETE{id}
        Task<Tuple<bool, string>> DeleteOneAsync<T>(DbSet<T> sets, Guid id) where T : ISuper;
    }
}