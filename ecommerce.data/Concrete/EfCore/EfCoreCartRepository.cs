using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreRepository<Cart,ECommerceContext>, ICartRepository
    {
        public void Create(Cart entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Cart entity)
        {
            throw new NotImplementedException();
        }

        public List<Cart> GetAll()
        {
            throw new NotImplementedException();
        }

        public Cart GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Cart entity)
        {
            throw new NotImplementedException();
        }
    }
}