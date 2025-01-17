using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.webui.EmailServices;
using ecommerce.webui.Identity;
using ecommerce.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ecommerce.webui.Controllers
{
    public class AccountController : Controller
    {
        private ICartRepository _cartRepository;
        private UserManager<User> _userManager; //User olaylarını 
        private SignInManager<User> _signInManager; // cookie olaylarını yönetecek
        private IEmailSender _emailSender;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartRepository cartRepository) 
        {
            _cartRepository=cartRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult AccessDenied(){
            
            return View();
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce hesap oluşturulmamış");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen email hesabınıza gelen link ile üyeliğinizi onaylayınız.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }
            ModelState.AddModelError("", "Girilen kullanıcı adı veya parola yanlış");

            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model, int term)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (term != 1)
            {
                System.Console.WriteLine("term 1 değil");
                ModelState.AddModelError("", "Koşulları Kabul Etmeniz Gerekmektedir.");
                View(model); 
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.FirstName +""+ model.LastName,
                Email = model.Email,
                //Password'u user manager aracılığıyla kullanacağız
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            System.Console.WriteLine("******************** Buraya Geldi *********************************");
           
            if (result.Succeeded)
            {
                //generate token bilgisi oluşturacağız kullanıcı hesabı onaylanması için
                //email ile oluşturulan token gönderilir.
                  await _userManager.AddToRoleAsync(user,"User"); //kullanıcı kayıt olurken burada rol atamasını direkt olarak yapabiliriz.
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id, //userId ve token action metodunun parametreleri
                    token = code
                });

                //email gönderme
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız", $"Lütfen email hesabınızı onaylamak için <a href='http://localhost:5220{url}'>tıklayınız</a>");
             
                return RedirectToAction("Login", "Account");
            }
             foreach (var item in result.Errors)
            {
                System.Console.WriteLine(item.Description);
            }
            ModelState.AddModelError("", "Bilinmeyen bir hata oldu, lütfen tekrar deneyiniz.");
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                CreateMessage("Geçersiz Token", "danger");
                TempData["message"] = "Geçersiz token";
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _cartRepository.CreateFirstCart(userId);
                    CreateMessage("Hesabınız onaylanmıştır", "success");
                    return View();
                }
            }
            CreateMessage("Hesabınız onaylanmadı", "warning");
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email){
            if(string.IsNullOrEmpty(Email)){
                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if(user==null){
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
             var url = Url.Action("ResetPassword", "Account", new
                {
                    userId = user.Id, //userId ve token action metodunun parametreleri
                    token = code
                });

                //email gönderme
                await _emailSender.SendEmailAsync(Email, "Parolayı sıfırla", $"Lütfen parolanızı yenilemek için <a href='http://localhost:5220{url}'>tıklayınız</a>");
            return View();
            
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token){ //QueryString'ten bunları aldık
        if(userId==null || token==null){
            return RedirectToAction("Index","Home");
        }
        var model = new ResetPasswordModel(){
            Token=token
        };
            return View();
        }
        [HttpPost] 
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model){
            if(!ModelState.IsValid){
                return View();
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user==null){
                return RedirectToAction("Index","Home");
            }
            var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);
            if(result.Succeeded){
                return RedirectToAction("Login","Account");
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        private void CreateMessage(string message, string alerttype)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }
}

