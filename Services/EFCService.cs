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
        public static async Task<IEnumerable<T>> GetListAsync<T>(this DbSet<T> sets, Func<T, bool> expression = null) where T : ISuper
        => await (expression is null ? sets.ToListAsync() : sets.Where(expression).AsQueryable<T>().ToListAsync());
        public static async Task<T> GetOneAsync<T>(this DbSet<T> sets, Guid id) where T : ISuper
        => await sets.Where(i => i.Id == id).FirstOrDefaultAsync();
        private static async Task<Tuple<bool, string>> TransactionAsync(this IDbContextTransaction trans, bool success, string message)
        {
            await (success ? trans.CommitAsync() : trans.RollbackAsync());
            return new Tuple<bool, string>(success, message);
        }
        public static async Task<Tuple<bool, string>> CreateManyAsync<T>(this AppDBContext db, DbSet<T> sets, IEnumerable<T> entities) where T : ISuper
        {
            var trans = await db.Database.BeginTransactionAsync();
            try
            {
                sets.AddRange(entities);
                await db.SaveChangesAsync();
                List<Guid> success = new();
                foreach (var i in entities)
                {
                    if ((await sets.Where(j => j.Id == i.Id).FirstOrDefaultAsync()) != null) success.Add(i.Id);
                }
                // if (success.Count != entities.Count()) return await trans.TransactionAsync(false, "新增失败，数据胡滚操作成功！");
                // else return await trans.TransactionAsync(true, "新增成功");
                var check = success.Count != entities.Count();
                return await trans.TransactionAsync(check, $"新增{(check ? "" : "失败，数据回滚操作")}成功！");
            }
            catch (Exception)
            {
                return await trans.TransactionAsync(false, "新增失败，数据胡滚操作成功！");
            }
        }
        private static async Task<Tuple<bool, string>> EditAsync<T>(this AppDBContext db, DbSet<T> sets, EditType type = EditType.Add, T entity = null, Guid? id = null) where T : ISuper
        {
            var title = string.Empty;
            switch(type)
            {
                case EditType.Add: sets.Add(entity); title = "新增"; break;
                case EditType.Modify: sets.Update(entity); title = "更新"; break;
                case EditType.Remove: sets.Remove(entity); title = "删除"; break;
            }
            try
            {
                switch(type)
                {
                    case EditType.Add: sets.Add(entity); break;
                    case EditType.Modify: sets.Update(entity); break;
                    case EditType.Remove: sets.Remove(entity); break;
                }
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
        public static async Task<Tuple<bool, string>> CreateOneAsync<T>(this AppDBContext db, DbSet<T> sets, T entity) where T : ISuper
        => await db.EditAsync(sets, entity: entity);
        public static async Task<Tuple<bool, string>> ModifyAsync<T>(this AppDBContext db, DbSet<T> sets, Guid id, T entity) where T : ISuper
        => await db.EditAsync(sets, EditType.Modify, entity, id);
        public static async Task<Tuple<bool, string>> DeleteOneAsync<T>(this AppDBContext db, DbSet<T> sets, Guid id) where T : ISuper
        => await db.EditAsync(sets, EditType.Remove, id: id);
    }
}