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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ecommerce.webui.Identity;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore.Metadata;
//

namespace ecommerce.webui.Controllers
{
  [Authorize(Roles = "Admin")]
  public class AdminController : Controller
  {
    private IProductRepository _productRepository;
    private ICategoryRepository _categoryRepository;
    private ISizeTypeRepository _sizeTypeRepository;
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<User> _userManager;
    private IOrderRepository _orderRepository;
    public AdminController(IProductRepository productRepository, ICategoryRepository categoryRepository, ISizeTypeRepository sizeTypeRepository, RoleManager<IdentityRole> roleManager, UserManager<User> userManager,IOrderRepository orderRepository)
    {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _sizeTypeRepository = sizeTypeRepository;
      _roleManager = roleManager;
      _userManager = userManager;
      _orderRepository=orderRepository;
    }

    public async Task<IActionResult> UserDelete(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user != null)
      {
        await _userManager.DeleteAsync(user);
      }
      return RedirectToAction("UserList");
    }
    [HttpGet]
    public async Task<IActionResult> UserEdit(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user != null)
      {
        var selectedRoles = await _userManager.GetRolesAsync(user);
        var roles = _roleManager.Roles.Select(i => i.Name);
        ViewBag.AllRoles = roles;
        return View(new UserDetailModel()
        {
          UserId = user.Id,
          FirstName = user.FirstName,
          LastName = user.LastName,
          Email = user.Email,
          EmailConfirmed = user.EmailConfirmed,
          SelectedRoles = selectedRoles
        });
      }
      return RedirectToAction("UserList");
    }
    [HttpPost]
    public async Task<IActionResult> UserEdit(UserDetailModel model, string[]? SelectedRoles)
    {

      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user != null)
        {
          user.FirstName = model.FirstName;
          user.LastName = model.LastName;
          user.Email = model.Email;
          user.EmailConfirmed = model.EmailConfirmed;

          var result = await _userManager.UpdateAsync(user);

          if (result.Succeeded)
          {
            var userRoles = await _userManager.GetRolesAsync(user);
            SelectedRoles = SelectedRoles ?? new string[] { };
            await _userManager.AddToRolesAsync(user, SelectedRoles.Except(userRoles).ToArray<string>());
            await _userManager.RemoveFromRolesAsync(user, userRoles.Except(SelectedRoles).ToArray<string>());
            return Redirect("/admin/userlist");
          }
        }
        return Redirect("/admin/userlist");
      }
      var roles = _roleManager.Roles.Select(i => i.Name);
      ViewBag.AllRoles = roles;
      return View(model);
    }
    public IActionResult UserList()
    {

      return View(_userManager.Users);
    }

    public async Task<IActionResult> RoleDelete(string roleId)
    {
      if (roleId != null)
      {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role != null)
        {
          var result = await _roleManager.DeleteAsync(role);
        }
      }
      return RedirectToAction("RoleList");
    }

    [HttpGet]
    public async Task<IActionResult> RoleEdit(string id)
    {
      System.Console.WriteLine(id + "----***********---------*******-----");
      var role = await _roleManager.FindByIdAsync(id);

      var members = new List<User>();
      var nonmembers = new List<User>();

      foreach (var user in _userManager.Users.ToList())
      {
        var list = await _userManager.IsInRoleAsync(user, role.Name)
                        ? members : nonmembers;

        list.Add(user);
      }
      var model = new RoleDetails()
      {
        Role = role,
        Members = members,
        NonMembers = nonmembers
      };
      return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> RoleEdit(RoleEditModel model)
    {

      foreach (var id in model.IdsToAdd ?? new string[] { })
      {
        var auser = await _userManager.FindByIdAsync(id);
        if (auser != null)
        {
          var result = await _userManager.AddToRoleAsync(auser, model.RoleName);
          if (!result.Succeeded)
          {
            foreach (var item in result.Errors)
            {
              ModelState.AddModelError("", item.Description);
            }
          }
        }
      }
      foreach (var userId in model.IdsToDelete ?? new string[] { })
      {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
          var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
          if (!result.Succeeded)
          {
            foreach (var item in result.Errors)
            {
              ModelState.AddModelError("", item.Description);
            }
          }
        }
      }

      return Redirect("/admin/role/" + model.RoleId);
    }
    public IActionResult RoleList()
    {
      return View(_roleManager.Roles);
    }
    [HttpGet]
    public IActionResult RoleCreate()
    {
      return View();
    }
    [HttpPost]
    public async Task<IActionResult> RoleCreate(RoleModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
        if (result.Succeeded)
        {
          return RedirectToAction("RoleList");
        }
        else
        {
          foreach (var error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
        }
      }
      return View();
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
    public IActionResult CategoryCreate(CategoryModel model, int[] genderIds)
    {
      var category = new Category();
      category.Name = model.Name;
      category.ParentCategoryId = model.parentCategoryId;
      category.Url = model.Name.Replace(" ", "-").ToLower();
      foreach (var item in genderIds)
      {
        System.Console.WriteLine(item);
      }
      _categoryRepository.CategoryCreate(category, genderIds);
      return RedirectToAction("CategoryList");
    }
    public IActionResult CategoryList()
    {
      var model = new List<CategoryViewModel>();
      var categories = _categoryRepository.GetAllCategories();
      // var subCategoryList = _categoryRepository.GetAllSubCategories();
      // var scList = new List<SubCategoryViewModel>();
      // foreach (var item in subCategoryList)
      // {
      //   var subCatModel = new SubCategoryViewModel(){
      //   Id=item.Id,
      //   Name=item.subCategoryName,
      //   ParentCategoryName=item.Category.Name,
      //   GenderCategories=item.Category.GenderCategories
      // };
      // scList.Add(subCatModel);
      // }
      foreach (var item in categories)
      {
        model.Add(new CategoryViewModel()
        {
          Name = item.Name,
          CategoryId = item.Id,
          genderCategories = item.GenderCategories,
          parentCategories = item.ParentCategory,

        });
      }
      return View(model);
    }
    public IActionResult CategoryDelete(int id)
    {
      var category = _categoryRepository.GetById(id);
      if (category != null)
      {
        _categoryRepository.Delete(category);
      }
      return RedirectToAction("CategoryList");
    }
    [HttpGet]
    public IActionResult CategoryEdit(int id)
    {
      //parentCategoryId'yi burada kontrol ederek istersek 
      var category = _categoryRepository.GetCategoryById(id);
      var model = new CategoryEditModel()
      {
        CategoryId = category.Id,
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
    public IActionResult CategoryEdit(CategoryEditModel model)
    {
      var category = new Category()
      {
        Name = model.Name,
        Id = model.CategoryId,
        ParentCategoryId = model.ParentCategoryId,
      };
      if (model.genderIds == null)
      {
        TempData["alert"] = "En az bir adet cinsiyet giriniz";
        return View("_alert-message");
      }

      category.GenderCategories = new List<GenderCategory>();

      foreach (var item in model.genderIds)
      {

        category.GenderCategories.Add(new GenderCategory()
        {
          GenderId = (int)item,
          CategoryId = model.CategoryId
        });
      }
      _categoryRepository.UpdateCategory(category);

      return RedirectToAction("CategoryList");
    }
    [HttpGet]
    public IActionResult SubCategoryCreate(){
      var categories = _categoryRepository.GetAllCategories();
      var modelList = new List<Category>();
      // foreach(var item in categories){
      //     var cat = new Category(){
      //       Name=item.Name,
      //       Id=item.Id,
      //     };
      // }
      
      var model = new SubCategoryModel(){
        Categories=categories
      };
      return View(model);
    }
  [HttpPost]
  public IActionResult SubCategoryCreate(string Name, int categoryId){
    var category = _categoryRepository.GetCategoryById(categoryId);
    var entity = new SubCategory(){
      subCategoryName=Name,
      CategoryId=category.Id
    };
    _categoryRepository.CreateSubCategory(entity);
    return RedirectToAction("CategoryList");
  }
    public IActionResult OrderList(){
      var orders = _orderRepository.GetAllOrders();
      var modelList = new List<OrderListModel>(); 
      foreach (var item in orders)
      {
        var model = new OrderListModel(){
          Id=item.Id,
          OrderDate=item.OrderDate.ToString(),
          orderState=item.OrderState,
          CustomerId=item.UserId,
          CustomerNameSurname=item.FirstName+" "+item.LastName,
          OrderNumber=item.OrderNumber
        };
        modelList.Add(model);
      }
      return View(modelList);
    }
    // [HttpGet]
    public IActionResult OrderDetails(int id){
       var order = _orderRepository.GetOrderItemsByOrderId(id);
      // System.Console.WriteLine(order.Id);

       var model = new OrderAdminViewModel(){
          OrderId=order.Id,
          FirstName=order.FirstName,
          LastName=order.LastName,
          Address=order.Address,
          Phone=order.Phone,
          Note=order.Note,
          OrderState=(int)order.OrderState,
          Date=order.OrderDate,
          Email=order.Email
       };

      // System.Console.WriteLine(order.Id);          
      // System.Console.WriteLine(order.FirstName);          
      // System.Console.WriteLine(order.LastName);          
      // System.Console.WriteLine(order.Address);          
      // System.Console.WriteLine(order.Phone);             
      // System.Console.WriteLine(order.Note);          
      // System.Console.WriteLine(order.OrderState);   
      // System.Console.WriteLine(order.OrderDate);          
      //  foreach (var item in order.OrderItems)
      //  {
      //   // System.Console.WriteLine(item.Id);
      //   // System.Console.WriteLine(item.Price);
      //   // System.Console.WriteLine(item.Quantity);
      //   // System.Console.WriteLine(item.Product.ImageUrls.ElementAt(0).ImageUrl);
      //   var m = new OrderItemDetailModel(){
      //     Id=item.Id,
      //     Price=item.Price,
      //     Quantity=item.Quantity,
      //     ImageUrl=item.Product.ImageUrls.ElementAt(0).ImageUrl.ToString(),
      //     CancelledState=0
      //   };
      //     System.Console.WriteLine(m.Id);
      //     System.Console.WriteLine(m.Price);
      //     System.Console.WriteLine(m.Quantity);
      //     System.Console.WriteLine(m.ImageUrl);
      //     System.Console.WriteLine(m.CancelledState);

      //   model.orderItemDetailModels.Add(m);
        
      //  }
  var m = new List<OrderItemDetailModel>();
      foreach (var item in order.OrderItems)
      {
        // System.Console.WriteLine(item.Id);
        // System.Console.WriteLine(item.OrderId);
        // System.Console.WriteLine(item.Quantity);
        // System.Console.WriteLine(item.ProductId);
        // System.Console.WriteLine(item.Price);
        // System.Console.WriteLine(item.Product.ImageUrls.ElementAt(0).ImageUrl);
        m.Add(new OrderItemDetailModel(){
          Id=item.Id,
          Name=item.Product.Name,
          Quantity=item.Quantity,
          Price=item.Price,
          ImageUrl=item.Product.ImageUrls.ElementAt(0).ImageUrl,
          CancelledState=0,
          SizeType=item.SizeType
        });
      }
      model.orderItemDetailModels=m;
      return View(model);
    }
    [HttpPost]
    public IActionResult OrderStateChange(){
      return View();
    }
  }
}






