using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreRepository<Category, ECommerceContext>, ICategoryRepository
    {
        public void CategoryCreate(Category category, int[] genderIds)
        {
            using (var context = new ECommerceContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
                foreach (var item in genderIds)
                {
                context.GenderCategories.Add(new GenderCategory(){GenderId=item,CategoryId=category.Id});
                }
                context.SaveChanges();

            }
        }
        public List<Category> GetAllCategories(){
            using (var context = new ECommerceContext())
            {
                var categories = context.Categories.AsQueryable();
                categories=categories.Include(i=>i.GenderCategories)
                .ThenInclude(a=>a.Gender)
                .Include(k=>k.ParentCategory);
                return categories.ToList();
            }
        }

        public Category GetCategoryById(int id)
        {
            using (var context = new ECommerceContext())
            {
                var category = context.Categories
                .Include(i=>i.GenderCategories)
                .ThenInclude(p=>p.Gender)
                .FirstOrDefault(a=>a.Id==id);
                return category;
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var context = new ECommerceContext())
            {
                var entity = context.Categories.Include(i=>i.GenderCategories).FirstOrDefault(c=>c.Id==category.Id);
                if(entity != null && category != null){
                entity.Name=category.Name;
                entity.ParentCategoryId=category.ParentCategoryId; 
                entity.GenderCategories=category.GenderCategories;
                context.Entry(entity).State=EntityState.Modified;
                context.SaveChanges();
                }
            }
        }
        public List<Category> GetCategoriesByGenderId(int id){
            using (var context = new ECommerceContext())
            {
                var categoryList = context.Categories.Include(i=>i.GenderCategories).ThenInclude(p=>p.Gender).Where(a=>a.GenderCategories.Any(o=>o.GenderId==id)).ToList();
                return categoryList;
            }
        }

        public void CreateSubCategory(SubCategory subCategory)
        {
            using (var context = new ECommerceContext())
            {
                context.SubCategories.Add(subCategory);
                context.SaveChanges();
            }
        }

        public List<SubCategory> GetAllSubCategories()
        {
            using (var context = new ECommerceContext())
            {
                var subCategoryList = context.SubCategories.Include(s=>s.Category).ThenInclude(c=>c.GenderCategories).ToList();
                return subCategoryList;
            }
        }
    }
}


