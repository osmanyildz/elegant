using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreRepository<TEntity> : IRepository<TEntity>
    where TEntity: class
    {
        protected readonly DbContext context;
        public EfCoreRepository(DbContext ctx)
        {
            context = ctx;
        }
        public void Create(TEntity entity)
        {
            
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            
        }
        public void Delete(TEntity entity)
        {
           
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            
        }

        public List<TEntity> GetAll()
        {
           
               return context.Set<TEntity>().ToList();
            
        }

        public TEntity GetById(int id)
        {
            
                return context.Set<TEntity>().Find(id);
            
        }

        public void Update(TEntity entity)
        {
            
                context.Entry(entity).State = EntityState.Modified; //changetracking ile takip edilen entity'nin değişiklik durumunu kontrol edip entry ile entity'e atıyoruz
                context.SaveChanges();
            
        }
    }
}