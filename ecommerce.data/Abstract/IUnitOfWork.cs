using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.data.Abstract
{
    public interface IUnitOfWork:IDisposable
    {
        IProductRepository Products{get;}
        ICartRepository Carts{get;}
        ICategoryRepository Categories{get;}
        IGenderRepository Genders{get;}
        IOrderRepository Orders{get;}
        ISizeTypeRepository SizeTypes{get;}
        void Save();
    }
}