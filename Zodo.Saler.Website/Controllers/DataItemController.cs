using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HZC.Core;
using Zodo.Saler.Website.Extensions;
using HZC.Common.Services;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize(Roles = "admin")]
    public class DataItemController : BaseController
    {
        private readonly DataItemService service = new DataItemService();

        public DataItemController()
        { }

        #region 列表页
        public ActionResult Index()
        {
            InitUI();
            return View();
        }

        public JsonResult Get(DataItemSearchParam param, int pageIndex = 1, int pageSize = 20)
        {
            var list = service.Fetch(param.ToSearchUtil());
            return Json(ResultUtil.Success<IEnumerable<DataItemEntity>>(list));
        }
        #endregion

        #region 创建
        public ActionResult Create()
        {
            InitUI();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(IFormCollection collection)
        {
            DataItemEntity entity = new DataItemEntity();
            TryUpdateModelAsync(entity);

            var result = service.Create(entity, AppUser, () =>
            {
                DataItemUtil.Clear();
            });
            return Json(result);
        }
        #endregion

        #region 修改
        public IActionResult Edit(int id)
        {
            var entity = service.Load(id);
            if (entity == null)
            {
                return Empty();
            }
            else
            {
                InitUI();
                return View(entity);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(int id, IFormCollection collection)
        {
            DataItemEntity entity = new DataItemEntity();
            TryUpdateModelAsync(entity);

            var result = service.Update(entity, AppUser, () =>
            {
                DataItemUtil.Clear();
            });
            return Json(result);
        }
        #endregion

        #region 删除
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            var entity = service.Load(id);
            if (entity == null)
            {
                return Json(ResultUtil.AuthFail("请求的数据不存在"));
            }

            var result = service.Remove(entity, AppUser, () =>
            {
                DataItemUtil.Clear();
            });
            return Json(result);
        }
        #endregion

        #region 辅助方法
        private void InitUI()
        {
        }
        #endregion
    }
}
