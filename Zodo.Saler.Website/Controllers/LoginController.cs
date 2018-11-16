using HZC.Common.Services;
using HZC.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zodo.Saler.Website.Extensions;
using Zodo.Saler.Website.Models;

namespace Zodo.Saler.Website.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 返回验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult VerifyCode()
        {
            string code = "";
            MemoryStream ms = new VerifyCodeUtil().Create(out code);
            HttpContext.Session.SetString("LoginValidateCode", code);
            Response.Body.Dispose();
            return File(ms.ToArray(), @"image/png");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<JsonResult> Login(LoginViewModel account)
        {
            var service = new AppUserService();

            var model = new LoginViewModel();
            await TryUpdateModelAsync(model);

            if (!ModelState.IsValid)
            {
                List<string> errorMsg = new List<string>();
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors.ToList();
                    foreach (var error in errors)
                    {
                        errorMsg.Add(error.ErrorMessage);
                    }
                }
                return Json(ResultUtil.AuthFail(string.Join(";", errorMsg)));
            }

            if (account.VerifyCode.ToLower() != HttpContext.Session.GetString("LoginValidateCode").ToLower())
            {
                return Json(ResultUtil.AuthFail("验证码错误"));
            }

            var user = service.Login(model.Name.Trim(), model.Pw.Trim());
            if (user.Code != 200)
            {
                return Json(ResultUtil.AuthFail("用户不存在"));
            }
            else if (AESEncryptor.Decrypt(user.Body.Pw) != model.Pw)
            {
                return Json(ResultUtil.AuthFail("帐号或密码错误"));
            }
            else
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Body.Name));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Body.Role));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Body.Id.ToString()));

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Today.AddDays(7),
                    RedirectUri = "/Login"
                });

                HttpContext.Session.Set<AppUser>("User", new AppUser { Id = user.Body.Id, Name = user.Body.Name, Role = user.Body.Role });
                return Json(new Result { Code = 200, Message = "" });
            }
        }
    }
}