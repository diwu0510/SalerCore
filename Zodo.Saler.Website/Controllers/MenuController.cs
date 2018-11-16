using HZC.Common.Services;
using HZC.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zodo.Saler.Website.Extensions;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize(Roles = "admin")]
    public class MenuController : BaseController
    {
        private MenuService service = new MenuService();

        #region 首页
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Get()
        {
            var menus = MenuUtil.All();
            return Json(ResultUtil.Success<List<MenuDto>>(menus));
        }
        #endregion

        #region 保存
        public IActionResult Edit(int? id)
        {
            MenuEntity entity;
            if (!id.HasValue)
            {
                entity = new MenuEntity();
            }
            else
            {
                entity = service.Load((int)id);
                if (entity == null)
                {
                    return new EmptyResult();
                }
            }
            InitUI();

            if (string.IsNullOrWhiteSpace(entity.Roles))
            {
                ViewBag.Roles = DataItemUtil.GetValues("Roles").ToSelectList();
            }
            else
            {
                string[] entityRoles = entity.Roles.Split(',');
                var allRoles = DataItemUtil.GetValues("Roles");
                List<SelectListItem> items = new List<SelectListItem>();
                foreach (var r in allRoles)
                {
                    if (entityRoles.Contains(r))
                    {
                        items.Add(new SelectListItem { Text = r, Value = r, Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = r, Value = r, Selected = false });
                    }
                }
                ViewBag.Roles = items;
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(IFormCollection collection)
        {
            try
            {
                MenuEntity entity = new MenuEntity();
                TryUpdateModelAsync(entity);
                var result = service.Save(entity, AppUser);
                if (result.Code == 200)
                {
                    MenuUtil.Clear();
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ResultUtil.Fail(ex));
            }
        }
        #endregion

        #region 删除
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var entity = service.Load(id);
                var result = service.Delete(entity, AppUser);
                if (result.Code == 200)
                {
                    MenuUtil.Clear();
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ResultUtil.Fail(ex));
            }
        }
        #endregion

        #region 私有方法
        private void InitUI()
        {
            var menus = MenuUtil.All();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var d in menus)
            {
                listItems.Add(new SelectListItem { Text = showName(d.Name, d.Level), Value = d.Id.ToString() });
            }
            ViewBag.Parents = listItems;

            string showName(string txt, int level)
            {
                string str = "";
                if (level > 1)
                {
                    for (int i = 0; i < level; i++)
                    {
                        str += HttpUtility.HtmlDecode("&nbsp;&nbsp;");
                    }
                    str += "|- " + txt;
                }
                else
                {
                    str = txt;
                }

                return str;
            }
        }
        #endregion
    }
}