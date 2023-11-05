using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreSizeTypeRepository : EfCoreRepository<SizeType, ECommerceContext>, ISizeTypeRepository
    {
        public List<SizeType> GetSizeTypesByParentCategory(int parentCategoryId)
        {
            using (var context = new ECommerceContext())
            {
                var sizeTypes = context.SizeTypes.Where(s=>s.ParentCategoryId==parentCategoryId).ToList();
                if(sizeTypes==null){
                    throw new Exception("sizeTypes null");
                }else{

                return sizeTypes;
                }
            }
        }

        public List<int> GetSizeTypesByProductId(int id)
        {
            using (var context = new ECommerceContext())
            {
                var sizeTypes = context
                .SizeTypes
                .Include(i=>i.ProductSizeTypes)
                .ThenInclude(p=>p.Product)
                .Where(i=>i.ProductSizeTypes.Any(p=>p.ProductId==id))
                .Select(i=>i.Id)
                .ToList();
                return sizeTypes;
            }
        }
    }
}