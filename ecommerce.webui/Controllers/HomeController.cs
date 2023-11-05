using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using ecommerce.webui.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.webui.Controllers
{
    public class HomeController:Controller
    {
        private IProductRepository _productRepository;
        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public IActionResult Index(){
          var pvModel = new List<ProductViewModel>();

            // var model = new ProductListViewModel(){
            //   Products = _productRepository.GetPopularProducts()
            // };
            var products = _productRepository.GetPopularProducts();
            foreach (var item in products)
            {
              pvModel.Add(new ProductViewModel(){
                Id=item.Id,
                Name=item.Name,
                Description=item.Description,
                ImageUrls=item.ImageUrls,
                GenderId=item.GenderId,
                Url=item.Url,
                IsPopular=item.IsPopular,
                Price=item.Price
              });
            }
          return View(pvModel); 
        }
    }
}