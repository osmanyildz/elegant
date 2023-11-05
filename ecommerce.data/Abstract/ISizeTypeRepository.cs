using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.data.Abstract
{
    public interface ISizeTypeRepository:IRepository<SizeType>
    {
        List<SizeType> GetSizeTypesByParentCategory(int parentCategoryId);
        List<int> GetSizeTypesByProductId(int id);
    }
}