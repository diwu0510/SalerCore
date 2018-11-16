using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HZC.Common.Services;
using HZC.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zodo.Saler.Website.Extensions;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize(Roles = "admin")]
    public class AppUserController : BaseController
    {
        private readonly AppUserService service = new AppUserService();

        #region 首页
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Get(AppUserSearchParam param)
        {
            var list = service.Fetch(param.ToSearchUtil());
            return Json(ResultUtil.List<AppUserEntity>(list));
        }
        #endregion

        #region 创建
        public IActionResult Create()
        {
            InitUI();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public JsonResult Create(IFormCollection collection)
        {
            var entity = new AppUserEntity();
            TryUpdateModelAsync(entity);
            entity.Pw = AESEncryptor.Encrypt("123456");
            var result = service.Create(entity, AppUser);
            return Json(result);
        }
        #endregion

        #region 编辑
        public IActionResult Edit(int id)
        {
            var entity = service.Load(id);
            if (entity == null)
            {
                return Empty();
            }
            InitUI();
            return View(entity);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public JsonResult Edit(int id, IFormCollection collection)
        {
            var entity = new AppUserEntity();
            TryUpdateModelAsync(entity);

            var result = service.Update(entity, AppUser);
            return Json(result);
        }
        #endregion

        #region 删除
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public JsonResult Delete(int id)
        {
            var entity = service.Load(id);
            if (entity == null)
            {
                return Json(ResultUtil.AuthFail("请求的数据不存在"));
            }
            var result = service.Remove(entity, AppUser);
            return Json(result);
        }
        #endregion

        #region 重置密码
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public JsonResult ResetPw(int id)
        {
            var result = service.ResetPw(id);
            return Json(result);
        }
        #endregion

        #region 私有方法
        private void InitUI()
        {
            var roles = DataItemUtil.GetValues("Roles").ToSelectList();
            ViewBag.Roles = roles;
        } 
        #endregion
    }
}