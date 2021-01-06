using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC4RESTAPI.Models.Super;
using EFC4RESTAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFC4RESTAPI.Services
{
    public enum EditType : int { Add = 0, Modify, Remove }
    public static class EFCService
    {
        public static async Task<IEnumerable<T>> GetListAsync<T>(this DbSet<T> sets) where T : ISuper
        => await sets.ToListAsync();
        public static async Task<T> GetOneAsync<T>(this DbSet<T> sets, Guid id) where T : ISuper
        => await sets.Where(i => i.Id == id).FirstOrDefaultAsync();
        #region Private functions
        private static IEnumerable<T> GetList<T>(this DbSet<T> sets, Func<T, bool> expression) where T : ISuper
        => sets.Where(expression);
        private static async Task<T> GetOneNoTrackingAsync<T>(this DbSet<T> sets, Guid id) where T : ISuper
        => await sets.AsNoTracking<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
        private static async Task<Tuple<bool, string>> TransactionAsync(this IDbContextTransaction trans, bool success, string message)
        {
            await (success ? trans.CommitAsync() : trans.RollbackAsync());
            return new Tuple<bool, string>(success, message);
        }
        public static string GetTitle(this EditType type) => type == EditType.Add ? "新增" : (type == EditType.Modify ? "更新" : "删除");
        private static void EditDo<T>(this DbSet<T> sets, EditType type, IEnumerable<T> entities, bool many = false) where T : ISuper
        {
            switch(type)
                {
                    case EditType.Add:  if (many) sets.AddRange(entities); else sets.Add(entities.First()); break;
                    case EditType.Modify: if (many) sets.UpdateRange(entities); else sets.Update(entities.First()); break;
                    case EditType.Remove: if (many) sets.RemoveRange(entities); else sets.Remove(entities.First()); break;
                }
        }
        private static async Task<Tuple<bool, string>> EditManyAsync<T>(this AppDBContext db, DbSet<T> sets, EditType type = EditType.Add, 
        IEnumerable<T> entities = null, IEnumerable<Guid> ids = null, IEnumerable<Tuple<Guid, T>> ids_entities = null) where T : ISuper
        {
            var trans = await db.Database.BeginTransactionAsync();
            var title = type.GetTitle();
            try
            {
                List<T> es = new();
                switch(type)
                {
                    case EditType.Add : es = entities.ToList(); break;
                    case EditType.Modify: 
                        foreach(var ie in ids_entities)
                        {
                            if ((await sets.GetOneNoTrackingAsync(ie.Item1)) != null) es.Add(ie.Item2);
                        }
                        break;
                    case EditType.Remove: es = sets.GetList<T>(i => ids.Contains(i.Id)).ToList(); break;
                }
                sets.EditDo<T>(type, es, true);
                await db.SaveChangesAsync();
                List<Guid> success = new();
                foreach (var i in es)
                {
                    switch(type)
                    {
                        case EditType.Add: if ((await sets.Where(j => j == i).FirstOrDefaultAsync()) != null) success.Add(i.Id); break;
                        case EditType.Modify: if ((await sets.Where(j => j.Id == i.Id).FirstOrDefaultAsync()) == i) success.Add(i.Id); break;
                        case EditType.Remove: if ((await sets.Where(j => j.Id == i.Id).FirstOrDefaultAsync()) == null) success.Add(i.Id); break;
                    }
                }
                var check = success.Count == (type == EditType.Add ? entities.Count() : (type == EditType.Modify ? ids_entities.Count() : ids.Count()));
                return await trans.TransactionAsync(check, $"{title}{(check ? "" : "失败，数据回滚操作")}成功！");
            }
            catch (Exception) { return await trans.TransactionAsync(false, $"{title}失败，数据回滚操作成功！"); }
            // catch (Exception e) { return await trans.TransactionAsync(false, e.Message.ToString()); }
        }
        private static async Task<Tuple<bool, string>> EditOneAsync<T>(this AppDBContext db, DbSet<T> sets, EditType type = EditType.Add,
                                                                        T entity = null, Guid? id = null) where T : ISuper
        {
            var title = type.GetTitle();
            try
            {
                sets.EditDo<T>(type, new List<T> { type != EditType.Remove ? entity : await sets.GetOneAsync<T>(id.Value) });
                await db.SaveChangesAsync();
                bool check = false;
                switch(type)
                {
                    case EditType.Add: check = await sets.Where(i => i == entity).FirstOrDefaultAsync() != null; break;
                    case EditType.Modify: check = await sets.Where(i => i.Id == id).FirstOrDefaultAsync() == entity; break;
                    case EditType.Remove: check = await sets.Where(i => i.Id == id).FirstOrDefaultAsync() == null; break;
                }
                return new Tuple<bool, string>(check, $"{title}{(check ? "成功" : "失败")}");
            }
            catch (Exception) { return new Tuple<bool, string>(false, $"{title}失败！");}
        }
        #endregion
        public static async Task<Tuple<bool, string>> AddOneAsync<T>(this AppDBContext db, DbSet<T> sets, T entity) where T : ISuper
        => await db.EditOneAsync(sets, entity: entity);
        public static async Task<Tuple<bool, string>> ModifyOneAsync<T>(this AppDBContext db, DbSet<T> sets, Guid id, T entity) where T : ISuper
        => await db.EditOneAsync(sets, EditType.Modify, entity, id);
        public static async Task<Tuple<bool, string>> RemoveOneAsync<T>(this AppDBContext db, DbSet<T> sets, Guid id) where T : ISuper
        => await db.EditOneAsync(sets, EditType.Remove, id: id);
        public static async Task<Tuple<bool, string>> AddManyAsync<T>(this AppDBContext db, DbSet<T> sets, IEnumerable<T> entities) where T : ISuper
        => await db.EditManyAsync(sets, EditType.Add, entities);
        public static async Task<Tuple<bool, string>> ModifyManyAsync<T>(this AppDBContext db, DbSet<T> sets, IEnumerable<Tuple<Guid, T>> ids_entities) where T : ISuper
        => await db.EditManyAsync(sets, EditType.Modify, ids_entities : ids_entities);
        public static async Task<Tuple<bool, string>> RemoveManyAsync<T>(this AppDBContext db, DbSet<T> sets, IEnumerable<Guid> ids) where T : ISuper
        => await db.EditManyAsync(sets, EditType.Remove, ids : ids);
    }
}