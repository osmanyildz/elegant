using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreRepository<TEntity, TContext> : IRepository<TEntity>
    where TEntity: class
    where TContext: DbContext, new()
    {
        public void Create(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
        }

        public List<TEntity> GetAll()
        {
            using (var context = new TContext())
            {
               return context.Set<TEntity>().ToList();
            }
        }

        public TEntity GetById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Find(id);
            }
        }

        public void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified; //changetracking ile takip edilen entity'nin değişiklik durumunu kontrol edip entry ile entity'e atıyoruz
                context.SaveChanges();
            }
        }
    }
}