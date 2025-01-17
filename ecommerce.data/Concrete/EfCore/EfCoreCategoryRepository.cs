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
    public class EfCoreCategoryRepository : EfCoreRepository<Category>, ICategoryRepository
    {
        public EfCoreCategoryRepository(ECommerceContext context):base(context)
        {
            
        }
        private ECommerceContext ECommerceContext{
            get{return context as ECommerceContext;}
        }
        public void CategoryCreate(Category category, int[] genderIds)
        {
         
                ECommerceContext.Categories.Add(category);
                ECommerceContext.SaveChanges();
                foreach (var item in genderIds)
                {
                ECommerceContext.GenderCategories.Add(new GenderCategory(){GenderId=item,CategoryId=category.Id});
                }
                ECommerceContext.SaveChanges();

            
        }
        public List<Category> GetAllCategories(){
           
                var categories = ECommerceContext.Categories.AsQueryable();
                categories=categories.Include(i=>i.GenderCategories)
                .ThenInclude(a=>a.Gender)
                .Include(k=>k.ParentCategory);
                return categories.ToList();
            
        }

        public Category GetCategoryById(int id)
        {
           
                var category = ECommerceContext.Categories
                .Include(i=>i.GenderCategories)
                .ThenInclude(p=>p.Gender)
                .FirstOrDefault(a=>a.Id==id);
                return category;
            
        }

        public void UpdateCategory(Category category)
        {
            
                var entity = ECommerceContext.Categories.Include(i=>i.GenderCategories).FirstOrDefault(c=>c.Id==category.Id);
                if(entity != null && category != null){
                entity.Name=category.Name;
                entity.ParentCategoryId=category.ParentCategoryId; 
                entity.GenderCategories=category.GenderCategories;
                ECommerceContext.Entry(entity).State=EntityState.Modified;
                ECommerceContext.SaveChanges();
                }
            
        }
        public List<Category> GetCategoriesByGenderId(int id){
           
                var categoryList = ECommerceContext.Categories.Include(i=>i.GenderCategories).ThenInclude(p=>p.Gender).Where(a=>a.GenderCategories.Any(o=>o.GenderId==id)).ToList();
                return categoryList;
            
        }

        public void CreateSubCategory(SubCategory subCategory)
        {
            
                ECommerceContext.SubCategories.Add(subCategory);
                ECommerceContext.SaveChanges();
            
        }

        public List<SubCategory> GetAllSubCategories()
        {
            
                var subCategoryList = ECommerceContext.SubCategories.Include(s=>s.Category).ThenInclude(c=>c.GenderCategories).ToList();
                return subCategoryList;
            
        }
    }
}


