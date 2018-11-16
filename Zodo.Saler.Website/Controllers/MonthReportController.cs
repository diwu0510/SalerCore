using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HZC.Core;
using Zodo.Saler.Services;
using Zodo.Saler.Website.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize(Roles = "admin")]
    public class MonthReportController : BaseController
    {
        private readonly MonthReportService service = new MonthReportService();

        public MonthReportController()
        { }

        #region 列表页
        public ActionResult Index()
        {
            MonthReportSearchParam param = new MonthReportSearchParam();
            param.Year = DateTime.Today.Year;
            param.Month = DateTime.Today.Month;
            InitUI();
            return View(param);
        }

        public JsonResult Get(MonthReportSearchParam param, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var list = service.Get(param);
                return Json(ResultUtil.List<MonthReportDto>(list));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return Json(ResultUtil.Fail());
            }
        }
        #endregion

        #region 创建
        public IActionResult Create()
        {
            var today = DateTime.Today;
            var entity = new MonthReportCreateDto
            {
                Year = today.Year,
                Month = today.Month,
                SalerId = 0
            };

            InitUI();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(IFormCollection collection)
        {
            MonthReportCreateDto entity = new MonthReportCreateDto();
            TryUpdateModelAsync(entity);

            var result = service.Create(entity, AppUser);
            return Json(result);
        }
        #endregion

        #region 批量创建
        public IActionResult MultiCreate()
        {
            var today = DateTime.Today;
            var entity = new MonthReportMultiCreateDto
            {
                Year = today.Year,
                Month = today.Month
            };
            InitUI();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult MultiCreate(IFormCollection collection)
        {
            MonthReportMultiCreateDto entity = new MonthReportMultiCreateDto();
            TryUpdateModelAsync(entity);

            var result = service.Create(entity, AppUser);
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
            var result =  service.Delete(entity, AppUser);
            // 如果有缓存，注意在这里要清空缓存

            return Json(result);
        }
        #endregion

        #region 获取部门下的所有销售
        public JsonResult GetDeptSalers(int id)
        {
            var salerService = new SalerService();
            var salers = salerService.GetByDept(id);
            return Json(salers.Select(s => new
            {
                Id = s.Id,
                Name = s.Name
            }));
        }
        #endregion

        #region 辅助方法
        private void InitUI()
        {
            var year = DateTime.Today.Year;

            List<SelectListItem> years = new List<SelectListItem>();
            for (int i = year - 3; i <= year + 1; i++)
            {
                years.Add(new SelectListItem { Text = i.ToString() + "年", Value = i.ToString() });
            }
            ViewBag.Years = years;

            List<SelectListItem> months = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new SelectListItem { Text = i.ToString() + "月", Value = i.ToString() });
            }
            ViewBag.Months = months;

            ViewBag.Depts = DeptUtil.All().ToSelectList("Id", "Name");
        }
        #endregion
    }
}
