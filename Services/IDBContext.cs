using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFC4RESTAPI.Models.Super;

namespace EFC4RESTAPI.Services
{
    public interface IDBContext
    {
        // Post IEnumerable<T>
        Task<Tuple<bool, string>> AddManyAsync<T>(IEnumerable<T> entities) where T : ISuper;
        // POST 
        Task<Tuple<bool, string>> AddOneAsync<T>(T entity) where T : ISuper;
        // GET
        Task<IEnumerable<T>> GetListAsync<T>() where T : ISuper;
        // GET{id}
        Task<T> GetOneAsync<T>(Guid id) where T : ISuper;
        // Put IEnumerable<Tuple<Guid,T>>
        Task<Tuple<bool, string>> ModifyManyAsync<T>(IEnumerable<Tuple<Guid, T>> ids_entities) where T : ISuper;
        // Put{id}
        Task<Tuple<bool, string>> ModifyOneAsync<T>(Guid id, T entity) where T : ISuper;
        // Delete
        Task<Tuple<bool, string>> RemoveManyAsync<T>(IEnumerable<Guid> ids) where T : ISuper;
        // Delete{id}
        Task<Tuple<bool, string>> RemoveOneAsync<T>(Guid id) where T : ISuper;
    }
}