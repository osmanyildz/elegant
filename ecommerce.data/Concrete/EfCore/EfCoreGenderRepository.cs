using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreGenderRepository:EfCoreRepository<Gender,ECommerceContext>,IGenderRepository
    {
        
    }
}