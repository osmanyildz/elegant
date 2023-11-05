using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.data.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void CategoryCreate(Category category, int[] genderIds);
        List<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void UpdateCategory(Category category);
        List<Category> GetCategoriesByGenderId(int id);
    }
}