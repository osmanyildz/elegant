using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;

namespace ecommerce.data.Concrete.EfCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceContext _context;
        public UnitOfWork(ECommerceContext context)
        {
            _context = context;
        }
        private EfCoreCartRepository _cartRepository;
        private EfCoreCategoryRepository _categoryRepository;
        private EfCoreGenderRepository _genderRepository;
        private EfCoreOrderRepository _orderRepository;
        private EfCoreProductRepository _productRepository;
        private EfCoreSizeTypeRepository _sizeTypeRepository;
        public IProductRepository Products => throw new NotImplementedException();

        public ICartRepository Carts => _cartRepository = _cartRepository ?? new EfCoreCartRepository(_context);
        public ICategoryRepository Categories => _categoryRepository ?? new EfCoreCategoryRepository(_context);

        public IGenderRepository Genders => _genderRepository ?? new EfCoreGenderRepository(_context);

        public IOrderRepository Orders => _orderRepository ?? new EfCoreOrderRepository(_context);

        public ISizeTypeRepository SizeTypes => _sizeTypeRepository ?? new EfCoreSizeTypeRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}