using HZC.Common.Services;
using HZC.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zodo.Saler.Services;
using Zodo.Saler.Website.Extensions;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize(Roles = "admin")]
    public class SalerController : BaseController
    {
        private readonly SalerService service = new SalerService();
        private readonly DeptService deptService = new DeptService();

        public SalerController()
        { }

        #region 列表页
        public ActionResult Index()
        {
            InitUI();
            return View();
        }

        public JsonResult Get(SalerSearchParam param, int pageIndex = 1, int pageSize = 20)
        {
            var list = service.Query(param.ToSearchUtil(), pageIndex, pageSize);
            return Json(ResultUtil.PageList<SalerEntity>(list));
        }
        #endregion

        #region 创建
        public IActionResult Create()
        {
            InitUI();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(IFormCollection collection)
        {
            SalerEntity entity = new SalerEntity();
            TryUpdateModelAsync(entity);

            var result = service.Create(entity, AppUser, () =>
            {
                SalerUtil.Clear();
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
            SalerEntity entity = new SalerEntity();
            TryUpdateModelAsync(entity);
            var result = service.Update(entity, AppUser, () =>
            {
                SalerUtil.Clear();
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
            var result =  service.Remove(entity, AppUser, () =>
            {
                SalerUtil.Clear();
            });
            // 如果有缓存，注意在这里要清空缓存

            return Json(result);
        }
        #endregion

        #region 辅助方法
        private void InitUI()
        {
            var jobs = DataItemUtil.GetValues("Jobs").ToSelectList();
            ViewBag.Jobs = jobs;

            ViewBag.Depts = DeptUtil.All().ToSelectList("Id", "Name");
        }
        #endregion
    }
}
