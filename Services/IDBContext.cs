using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFC4RESTAPI.Models.Super;

namespace EFC4RESTAPI.Services
{
    public interface IDBContext
    {
        // GET
        Task<IEnumerable<T>> GetListAsync<T>(Func<T, bool> expression = null) where T : ISuper;
        // GET{id}
        Task<T> GetOneAsync<T>(int id) where T : ISuper;
        // POST IEnumerable<T>
        Task<Tuple<bool, string>> CreateManyAsync<T>(IEnumerable<T> entities) where T : ISuper;
        // POST T
        Task<Tuple<bool, string>> CreateOneAsync<T>(T entity) where T : ISuper;
        // PUT{id} T
        Task<Tuple<bool, string>> ModifyAsync<T>(int id, T entity) where T : ISuper;
        // DELETE IEnumerable<T>
        Task<Tuple<bool, string>> DeleteManyAsync<T>(IEnumerable<int> ids) where T : ISuper;
        // DELETE{id}
        Task<Tuple<bool, string>> DeleteOneAsync<T>(int id) where T : ISuper;
    }
}