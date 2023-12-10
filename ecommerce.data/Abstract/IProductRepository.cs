using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.data.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        List<Product> GetPopularProducts();
        List<Product> GetProductByGenderCategory(string categoryName, string genderId);
        Product GetProductById(int id);
        List<Product> GetAllProductsWithImage();
        void ProductCreate(Product product, int[] categoryIds, List<SizeType> sizeTypes);
        void ProductUpdate(Product product, int[] categoryIds, int[] sizeTypeIds); //, List<SizeType> sizeTypes
        List<Product> SearchList(string q);
    }
}