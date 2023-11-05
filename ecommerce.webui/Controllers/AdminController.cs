using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using ecommerce.webui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;


namespace ecommerce.webui.Controllers
{
  public class AdminController : Controller
  {
    IProductRepository _productRepository;
    ICategoryRepository _categoryRepository;
    ISizeTypeRepository _sizeTypeRepository;
    public AdminController(IProductRepository productRepository, ICategoryRepository categoryRepository, ISizeTypeRepository sizeTypeRepository)
    {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _sizeTypeRepository = sizeTypeRepository;
    }
    public IActionResult ProductEdit()
    {
      return View();
    }
    public IActionResult ProductList()
    {
      var products = _productRepository.GetAllProductsWithImage();
      var pvModel = new List<ProductViewModel>();

      foreach (var item in products)
      {
        pvModel.Add(new ProductViewModel()
        {
          Id = item.Id,
          Name = item.Name,
          Description = item.Description,
          IsPopular = item.IsPopular,
          ImageUrls = item.ImageUrls,
          Price = item.Price,
          GenderId = item.GenderId,
          Url = item.Url,
          Categories = item.ProductCategories,
          ProductSizeTypes = item.ProductSizeTypes
        });
      }
      return View(pvModel);
    }


    [HttpGet]
    public IActionResult ProductCreate()
    {
      var c = _categoryRepository.GetAll();
      ViewBag.categories = c;
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductModel model, IFormFile file1, IFormFile file2, IFormFile file3, int[] categoryIds)
    {

      var product = new Product();
      System.Console.WriteLine(model.IsPopular);
      if (model.IsPopular == 1)
      {
        product.IsPopular = true;
      }
      else
      {
        product.IsPopular = false;
      }
      product.Name = model.Name;
      product.Description = model.Description;
      product.ParentCategoryId = model.ParentCategoryId;
      product.Url = model.Name.Replace(" ", "-").ToLower();
      product.GenderId = model.GenderId;
      product.Price = model.Price;

      if (file1 != null && file2 != null && file3 != null)
      {
        var extension1 = Path.GetExtension(file1.FileName);
        var extension2 = Path.GetExtension(file2.FileName);
        var extension3 = Path.GetExtension(file3.FileName);
        var randomName1 = string.Format($"{Guid.NewGuid()}{extension1}");
        var randomName2 = string.Format($"{Guid.NewGuid()}{extension2}");
        var randomName3 = string.Format($"{Guid.NewGuid()}{extension3}");
        var imgPath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName1);
        var imgPath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName2);
        var imgPath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName3);

        product.ImageUrls = new List<Image>(){
          new Image(){ImageUrl=randomName1},
          new Image(){ImageUrl=randomName2},
          new Image(){ImageUrl=randomName3},
        };
        using (var stream = new FileStream(imgPath1, FileMode.Create))
        {
          await file1.CopyToAsync(stream);
        }

        using (var stream = new FileStream(imgPath2, FileMode.Create))
        {
          await file2.CopyToAsync(stream);
        }

        using (var stream = new FileStream(imgPath3, FileMode.Create))
        {
          await file3.CopyToAsync(stream);
        }

      }
      else
      {
        throw new Exception("En az bir tane file eksik. Tüm resim dosyalarını doldurun");
      }

      var sizeTypes = _sizeTypeRepository.GetSizeTypesByParentCategory(model.ParentCategoryId);
      if (sizeTypes == null)
      {
        System.Console.WriteLine("sizeTypes null");
      }
      _productRepository.ProductCreate(product, categoryIds, sizeTypes);
      return RedirectToAction("ProductList");
    }

    public IActionResult ProductDelete(int id)
    {

      var p = _productRepository.GetProductById(id);
      _productRepository.Delete(_productRepository.GetProductById(id));
      return RedirectToAction("ProductList");
    }
    [HttpGet]
    public IActionResult ProductEdit(int id)
    {
      var product = _productRepository.GetProductById(id);

      if (product == null)
      {
        System.Console.WriteLine("Product null");
        var c = _categoryRepository.GetAll();
        ViewBag.categories = c;
        return View();
      }
      else
      {
        var model = new ProductEditModel()
        {
          ProductId = product.Id,
          Name = product.Name,
          Description = product.Description,
          ImageUrls = product.ImageUrls,
          Price = product.Price,
          IsPopular = product.IsPopular,
          Url = product.Url,
          GenderId = product.GenderId,
          ParentCategoryId = product.ParentCategoryId,
        };

        model.SelectedCategories = new List<int>();
        foreach (var item in product.ProductCategories)
        {
          model.SelectedCategories.Add(item.CategoryId);
        }

        var c = _categoryRepository.GetAll();
        ViewBag.categories = c;


        ViewBag.SelectedSizeTypes = _sizeTypeRepository.GetSizeTypesByProductId(id);

        ViewBag.SizeTypes = _sizeTypeRepository.GetSizeTypesByParentCategory(model.ParentCategoryId);

        return View(model);
      }
    }
    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductEditModel productEditModel, string im1, string im2, string im3, IFormFile? file1, IFormFile? file2, IFormFile? file3, int[] categoryIds, int[] sizeTypeIds)
    {

      var product = new Product();
      product.Id = productEditModel.ProductId;
      product.Name = productEditModel.Name;
      product.IsPopular = productEditModel.IsPopular;
      product.Url = productEditModel.Name.Replace(" ", "-").ToLower();
      product.GenderId = productEditModel.GenderId;
      product.ParentCategoryId = productEditModel.ParentCategoryId;
      product.Price = productEditModel.Price;
      product.Description = productEditModel.Description;

      // product.ImageUrls=new List<Image>(){
      //   new Image(){ImageUrl=im1},
      //   new Image(){ImageUrl=im2},
      //   new Image(){ImageUrl=im3}
      // };
      var pImageUrls = new List<Image>(){
    new Image(){ImageUrl=im1},
    new Image(){ImageUrl=im2},
    new Image(){ImageUrl=im3},
  };

      if (file1 != null)
      {
        var extension1 = Path.GetExtension(file1.FileName);
        var randomName1 = string.Format($"{Guid.NewGuid()}{extension1}");
        var imgPath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName1);
        // product.ImageUrls.RemoveAt(0);
        pImageUrls.RemoveAt(0);
        pImageUrls.Add(new Image() { ImageUrl = randomName1 });
        // product.ImageUrls.Add(new Image(){ImageUrl=randomName1});
        using (var stream = new FileStream(imgPath1, FileMode.Create))
        {
          await file1.CopyToAsync(stream);
        }
      }
      if (file2 != null)
      {
        var extension2 = Path.GetExtension(file2.FileName);
        var randomName2 = string.Format($"{Guid.NewGuid()}{extension2}");
        var imgPath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName2);
        // product.ImageUrls.RemoveAt(1);
        pImageUrls.RemoveAt(1);
        pImageUrls.Add(new Image() { ImageUrl = randomName2 });
        // product.ImageUrls.Add(new Image(){ImageUrl=randomName2});
        using (var stream = new FileStream(imgPath2, FileMode.Create))
        {
          await file2.CopyToAsync(stream);
        }
      }
      if (file3 != null)
      {
        var extension3 = Path.GetExtension(file3.FileName);
        var randomName3 = string.Format($"{Guid.NewGuid()}{extension3}");
        var imgPath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName3);
        // product.ImageUrls.RemoveAt(2);
        pImageUrls.RemoveAt(2);
        pImageUrls.Add(new Image() { ImageUrl = randomName3 });
        // product.ImageUrls.Add(new Image(){ImageUrl=randomName3});
        using (var stream = new FileStream(imgPath3, FileMode.Create))
        {
          await file3.CopyToAsync(stream);
        }
      }
      product.ImageUrls = null;
      product.ImageUrls = pImageUrls;
      _productRepository.ProductUpdate(product, categoryIds, sizeTypeIds);
      return RedirectToAction("ProductList");
    }
    [HttpGet]
    public IActionResult CategoryCreate()
    {
      return View();
    }
    [HttpPost]
    public IActionResult CategoryCreate(CategoryModel model,int[] genderIds){
      var category = new Category();
      category.Name=model.Name;
      category.ParentCategoryId=model.parentCategoryId;
      category.Url = model.Name.Replace(" ", "-").ToLower();
      foreach (var item in genderIds)
      {
        System.Console.WriteLine(item);
      }
      _categoryRepository.CategoryCreate(category,genderIds);
      return RedirectToAction("CategoryList");
    }
    public IActionResult CategoryList(){
      var model = new List<CategoryViewModel>();
      var categories = _categoryRepository.GetAllCategories();
      foreach (var item in categories)
      {
        model.Add(new CategoryViewModel(){
          Name=item.Name,
          CategoryId=item.Id,
          genderCategories=item.GenderCategories,
          parentCategories=item.ParentCategory
        });
      }
      return View(model);
    }
    public IActionResult CategoryDelete(int id){
      var category = _categoryRepository.GetById(id); 
      if(category!=null){
      _categoryRepository.Delete(category);
      }
      return RedirectToAction("CategoryList");
    }
    [HttpGet]
    public IActionResult CategoryEdit(int id){
      //parentCategoryId'yi burada kontrol ederek istersek 
      var category = _categoryRepository.GetCategoryById(id);
      var model = new CategoryEditModel(){
        CategoryId=category.Id,
        Name = category.Name,
        ParentCategoryId = category.ParentCategoryId
      };
      model.genderIds = new List<int?>();
      foreach (var item in category.GenderCategories)
      {
        model.genderIds.Add(item.Gender.Id);
      }
      return View(model);
    }
    [HttpPost]
    public IActionResult CategoryEdit(CategoryEditModel model){
      var category = new Category(){
        Name=model.Name,
        Id=model.CategoryId,
        ParentCategoryId=model.ParentCategoryId,
      };
      if(model.genderIds == null){
          TempData["alert"] = "En az bir adet cinsiyet giriniz";
          return View("_alert-message");
      }

      category.GenderCategories = new List<GenderCategory>();

      foreach (var item in model.genderIds)
      {

        category.GenderCategories.Add(new GenderCategory(){
          GenderId=(int)item,
          CategoryId=model.CategoryId
        });
      }
      _categoryRepository.UpdateCategory(category);
     
        return RedirectToAction("CategoryList");
    }
  }
}






