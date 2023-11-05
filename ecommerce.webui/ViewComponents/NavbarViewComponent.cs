using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.webui.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.webui.ViewComponents
{
    public class NavbarViewComponent:ViewComponent
    {
        ICategoryRepository _categoryRepository;
        public NavbarViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IViewComponentResult Invoke(){
            var model = new CategoryListModel(){
                // Categories=_categoryRepository.GetAll()
                ManCategories=_categoryRepository.GetCategoriesByGenderId(1),
                WomanCategories=_categoryRepository.GetCategoriesByGenderId(2)
            };
            return View(model);
        }
    }
}