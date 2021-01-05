using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC4RESTAPI.Models.Super;
using EFC4RESTAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFC4RESTAPI.Controllers.Super
{
    public class SuperController<X, Y, Z> : ControllerBase where X : ISuper where Y : ISuper where Z : IUngetSuper
    {
        private readonly IDBContext _dbcontext;
        public SuperController(IDBContext dBContext) => _dbcontext = dBContext;
        [HttpGet("many")]
        public async Task<IEnumerable<Y>> GetListAsync() => (await _dbcontext.GetListAsync<X>()).Select(i => i as Y);
        [HttpGet("single/{id}")]
        public async Task<Y> GetOneAsync(Guid id) => (await _dbcontext.GetOneAsync<X>(id)) as Y;
        [HttpPost("many")]
        public async Task<ActionResult> CreateManyAsync(IEnumerable<Z> entities)
        {
            List<X> xs = new();
            foreach(var e in entities) { xs.Add((X)e.AsModel()); }
            var check = await _dbcontext.AddManyAsync(xs);
            return new ContentResult { Content = check.Item2 };
        } 
        [HttpPost("single")]
        public async Task<ActionResult> CreateOneAsync(Z entity)
        {
            X x = (X)entity.AsModel();
            await _dbcontext.AddOneAsync(x);
            return CreatedAtAction(nameof(GetOneAsync), new { id = x.Id }, x as Y);
        }
        [HttpPut("many")]
        public async Task<ActionResult> ModifyManyAsync(IEnumerable<Tuple<Guid, Z>> ids_entities)
        {
            List<Tuple<Guid, X>> xs = new();
            foreach(var ie in ids_entities)
            {
                xs.Add(new Tuple<Guid, X>(ie.Item1, ((X)ie.Item2.AsModel()) with { Id = ie.Item1 }));
            }
            var check = await _dbcontext.ModifyManyAsync(xs);
            return new ContentResult { Content = check.Item2 };
        }
        [HttpPut("single/{id}")]
        public async Task<ActionResult> ModifyOneAsync(Guid id, Z entity)
        {
            X x = ((X)entity.AsModel()) with { Id = id };
            var check = await _dbcontext.ModifyOneAsync(id, x);
            return CreatedAtAction(nameof(GetOneAsync), new { id = x.Id}, x as Y);
        }
        [HttpDelete("many")]
        public async Task<ActionResult> RemoveManyAsync(IEnumerable<Guid> ids)
        => new ContentResult { Content = (await _dbcontext.RemoveManyAsync<X>(ids)).Item2 };
        [HttpDelete("single/{id}")]
        public async Task<ActionResult> RemoveOneAsync(Guid id)
        => new ContentResult { Content = (await _dbcontext.RemoveOneAsync<X>(id)).Item2 };
    }
}