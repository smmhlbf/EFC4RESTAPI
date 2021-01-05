using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFC4RESTAPI.Models.Super;
using EFC4RESTAPI.Services;

namespace EFC4RESTAPI.Repositories
{
    public class EFCRepository : IDBContext
    {
        private AppDBContext _db;
        public EFCRepository(AppDBContext db) => _db = db;
        // Post IEnumerable<T>
        public async Task<Tuple<bool, string>> AddManyAsync<T>(IEnumerable<T> entities) where T : ISuper
        => await _db.AddManyAsync<T>(_db.Sets<T>(), entities);
        // Post T
        public async Task<Tuple<bool, string>> AddOneAsync<T>(T entity) where T : ISuper
        => await _db.AddOneAsync<T>(_db.Sets<T>(), entity);
        // Get
        public async Task<IEnumerable<T>> GetListAsync<T>(Func<T, bool> expression = null) where T : ISuper
        => await _db.Sets<T>().GetListAsync(expression);
        // Get{id}
        public async Task<T> GetOneAsync<T>(Guid id) where T : ISuper
        => await _db.Sets<T>().GetOneAsync(id);
        // Put IEnumerable<Tuple<Guid,T>>
        public async Task<Tuple<bool, string>> ModifyManyAsync<T>(IEnumerable<Tuple<Guid, T>> ids_entities) where T : ISuper
        => await _db.ModifyManyAsync<T>(_db.Sets<T>(), ids_entities);
        // Put{id}
        public async Task<Tuple<bool, string>> ModifyOneAsync<T>(Guid id, T entity) where T : ISuper
        => await _db.ModifyOneAsync<T>(_db.Sets<T>(), id, entity);
        // Delete IEnumerable<Guid>
        public async Task<Tuple<bool, string>> RemoveManyAsync<T>(IEnumerable<Guid> ids) where T : ISuper
        => await _db.RemoveManyAsync<T>(_db.Sets<T>(), ids);
        // Delete{id}
        public async Task<Tuple<bool, string>> RemoveOneAsync<T>(Guid id) where T : ISuper
        => await _db.RemoveOneAsync<T>(_db.Sets<T>(), id);
    }
}