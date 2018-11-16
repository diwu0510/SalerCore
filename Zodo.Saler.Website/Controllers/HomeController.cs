using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zodo.Saler.Website.Extensions;
using Zodo.Saler.Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using HZC.Common.Services;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.Role = AppUser.Role;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "/Login");
        }

        #region 树形列表
        public JsonResult Menu()
        {
            var tree = MenuUtil.Tree(AppUser.Role);
            return Json(tree);
        }
        #endregion

        public IActionResult ChangePw()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public JsonResult ChangePw(IFormCollection collection)
        {
            var entity = new ChangePwViewModel();
            TryUpdateModelAsync(entity);

            if (string.IsNullOrWhiteSpace(entity.OldPw) || string.IsNullOrWhiteSpace(entity.NewPw))
            {
                return Json("原密码和新密码不能为空");
            }
            entity.Id = AppUser.Id;
            var service = new AppUserService();
            var result = service.ChangePw(entity.Id, entity.OldPw, entity.NewPw);
            return Json(result);
        }
    }
}
