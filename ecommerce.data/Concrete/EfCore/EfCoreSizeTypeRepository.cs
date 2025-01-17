using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreSizeTypeRepository : EfCoreRepository<SizeType>, ISizeTypeRepository
    {
        public EfCoreSizeTypeRepository(ECommerceContext context):base(context)
        {
            
        }
        private ECommerceContext ECommerceContext{
            get{return context as ECommerceContext;}
        }
        public List<SizeType> GetSizeTypesByParentCategory(int parentCategoryId)
        {
           
                var sizeTypes = ECommerceContext.SizeTypes.Where(s=>s.ParentCategoryId==parentCategoryId).ToList();
                if(sizeTypes==null){
                    throw new Exception("sizeTypes null");
                }else{

                return sizeTypes;
                }
            
        }

        public List<int> GetSizeTypesByProductId(int id)
        {
           
                var sizeTypes = ECommerceContext
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