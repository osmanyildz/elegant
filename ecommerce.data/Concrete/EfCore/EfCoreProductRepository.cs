using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Quic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreRepository<Product>, IProductRepository
    {
        public EfCoreProductRepository(ECommerceContext context): base(context)
        {
            
        }
        private ECommerceContext ECommerceContext{
            get {return context as ECommerceContext;}
        }
        public List<Product> GetProductByGenderCategory(string categoryName, string genderId){
           
                var ps = new List<Product>();
                var products = ECommerceContext.Products.AsQueryable();
                if(!(string.IsNullOrEmpty(categoryName))){
                products = products
                .Include(p=>p.ProductCategories)
                .ThenInclude(pc=>pc.Category)
                .Where(p=>p.ProductCategories.Any(a=>(a.Category.Url==categoryName) && (a.Product.GenderId.ToString()==genderId))); //.Where(a=>a.Gender.Name==genderName)
                }
                foreach (var item in products.ToList())
                {
                    item.ImageUrls=GetImageUrlByProductId(item.Id);
                    ps.Add(item);
                }
                return ps.ToList();
            
        }

       public Product GetProductById(int id)
{
        var product = ECommerceContext.Products
            .Include(p => p.ProductSizeTypes)
            .ThenInclude(pst => pst.SizeType)
            .Include(a=>a.ProductCategories)
            .ThenInclude(k=>k.Category)
            .FirstOrDefault(p => p.Id == id);

        if (product != null)
        {
            product.ImageUrls = GetImageUrlByProductId(id);
        }
        return product;
    
}

        private List<Image> GetImageUrlByProductId(int id){
            
                var url = ECommerceContext.Images.Where(i=>i.ProductId==id).ToList();
                return url;
            
        }
        public List<Product> GetPopularProducts(){
           
                var products = ECommerceContext.Products.AsQueryable();
                products = products
                .Where(p=>p.IsPopular)
                .Include(i=>i.ImageUrls);
                return products.ToList();
            
        }

        public List<Product> GetAllProductsWithImage()
        {
            
                var products = ECommerceContext.Products.AsQueryable();
                products = products
                .Include(i=>i.ProductCategories)
                .ThenInclude(p=>p.Category)
                .Include(b=>b.ImageUrls)
                .Include(ps=>ps.ProductSizeTypes)
                .ThenInclude(pst=>pst.SizeType)
                .Where(p=>p.ProductSizeTypes.Any(pst=>pst.ProductId==p.Id));
                
                return products.ToList();
            
        }
        public void ProductCreate(Product product,int[] categoryIds, List<SizeType> sizeTypes){
            //etki edeceği tablolar: Products, Images, ProductCategory, ProductSizeType(parentCategoryId'ye göre size'ları ekleyeceğiz)
           
            
                ECommerceContext.Products.Add(product);
                
                ECommerceContext.SaveChanges();

                // var prdCat = new ProductCategory(){ProductId=product.Id,CategoryId=categoryIds[0]};
                foreach (var item in categoryIds)
                {
                ECommerceContext.ProductCategories.Add(new ProductCategory(){ProductId=product.Id, CategoryId=item});
                
                }
                foreach (var item in sizeTypes)
                {
                ECommerceContext.ProductSizeTypes.Add(new ProductSizeType(){ProductId=product.Id,SizeTypeId=item.Id});
                }
                ECommerceContext.SaveChanges();

            
        }
     
        public void ProductUpdate(Product product, int[] categoryIds,int[] sizeTypeIds) 
        {
            
                var entity = ECommerceContext
                .Products
                .Include(i=>i.ProductCategories)
                .Include(p=>p.ProductSizeTypes)
                .FirstOrDefault(i=>product.Id==i.Id);


                DeleteImageUrls(entity.Id);

                if(product!=null && entity!=null){
                    entity.Name=product.Name;
                    entity.Price=product.Price;
                    entity.Description=product.Description;
                    entity.ImageUrls=product.ImageUrls;
                    entity.IsPopular=product.IsPopular;
                    entity.Url=product.Url;
                    entity.ParentCategoryId=product.ParentCategoryId;
                    entity.GenderId=product.GenderId;
                    
                    entity.ProductCategories = categoryIds.Select(i=>new ProductCategory(){
                        ProductId=product.Id,
                        CategoryId=i
                    }).ToList();
                    entity.ProductSizeTypes = sizeTypeIds.Select(i=> new ProductSizeType(){
                        ProductId=product.Id,
                        SizeTypeId=i
                    }).ToList();
                    
                    ECommerceContext.SaveChanges();
                }

            
        }
        private void DeleteImageUrls(int id){

                var urls = ECommerceContext.Images.Where(i=>i.ProductId==id).ToList();
                ECommerceContext.Images.RemoveRange(urls);
                ECommerceContext.SaveChanges();
            
        }

        public List<Product> SearchList(string q)
        {
           
                var products = ECommerceContext.Products.AsQueryable();
                products = products.Where(p=>p.Name.Contains(q)||p.Description.Contains(q)).Include(i=>i.ImageUrls).Include(a=>a.ProductCategories).ThenInclude(o=>o.Category).Include(s=>s.ProductSizeTypes).ThenInclude(l=>l.SizeType);
                return products.ToList();
            
        }
    }
    }













