using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using ecommerce.webui.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ecommerce.webui.Controllers
{
    public class ProductController:Controller
    {
        
        private IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public IActionResult Search(string q){
            var products = _productRepository.SearchList(q);
            var productModel = new List<ProductViewModel>();
           
            foreach (var item in products)
            {
            var imgUrls = new List<Image>();
                var productCat = new List<ProductCategory>();
            var sizes = new List<ProductSizeType>();
                foreach (var img in item.ImageUrls)
                {
                    var url = new Image(){
                        ImageUrl=img.ImageUrl,
                        Id = img.Id,
                        ProductId=img.ProductId
                    };
                    imgUrls.Add(url);
                } 
                foreach (var i in item.ProductCategories)
                {
                    var pCat = new ProductCategory(){
                        Category=i.Category
                    };
                    productCat.Add(pCat);
                }
                foreach (var it in item.ProductSizeTypes)
                {
                    var size = new ProductSizeType(){
                        SizeType=it.SizeType
                    };
                    sizes.Add(size);
                }
                 var viewModel = new ProductViewModel(){
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price=item.Price,
                    IsPopular = item.IsPopular,
                    Url = item.Url,
                    GenderId=item.GenderId,
                    ParentCategoryId=item.ParentCategoryId,
                    ProductSizeTypes=sizes,
                    Categories=productCat,
                    ImageUrls=imgUrls
                };
                productModel.Add(viewModel);
            }
            return View(productModel);
        }
        public IActionResult List(string category,string genderId){
          
            var products = _productRepository.GetProductByGenderCategory(category,genderId);
            var pvModel = new List<ProductViewModel>();
            foreach (var item in products)
            {
            pvModel.Add(new ProductViewModel(){
                Id=item.Id,
                Name=item.Name,
                Description=item.Description,
                ImageUrls=item.ImageUrls,
                IsPopular=item.IsPopular,
                GenderId=item.GenderId,
                Price=item.Price,
                Url=item.Url
            });
            }
           
            return View(pvModel);
        }
        public IActionResult Details(int id){
            var product = _productRepository.GetProductById(id);
           
            var model = new ProductViewModel(){
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description=product.Description,
                IsPopular = product.IsPopular,
                ImageUrls=product.ImageUrls,
                GenderId = product.GenderId,
                Url = product.Url,
                ProductSizeTypes=product.ProductSizeTypes
            };

           
            return View(model);
        }

        public IActionResult PopularProducts(){
            var model = new List<ProductViewModel>();
            foreach(var item in _productRepository.GetPopularProducts()){
                model.Add( new ProductViewModel{
                    Id=item.Id,
                    Name=item.Name,
                    Description=item.Description,
                    ImageUrls=item.ImageUrls,
                    Price=item.Price,
                    IsPopular=item.IsPopular,
                    GenderId=item.GenderId
                });    
            }
    
            return View(model);
        }
    }
}