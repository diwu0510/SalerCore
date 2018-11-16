using HZC.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Zodo.Saler.Services;
using Zodo.Saler.Website.Extensions;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize(Roles = "admin, 销售总监")]
    public class SalerReportController : BaseController
    {
        private readonly ReportService reportService = new ReportService();

        #region 月汇总
        /// <summary>
        /// 月汇总饼图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [Route("Report/Saler/Month")]
        public IActionResult SalerMonth(int? year, int deptId = 0)
        {
            InitUI(year, deptId);
            return View();
        }

        /// <summary>
        /// 月汇总趋势图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [Route("Report/Saler/MonthLine")]
        public IActionResult SalerMonthLine(int? year, int deptId = 0)
        {
            InitUI(year, deptId);
            return View();
        }

        /// <summary>
        /// 获取月汇总数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public JsonResult GetSalerMonthData(int year, int deptId)
        {
            var data = reportService.SalerMonthSummary(year, deptId);
            ViewBag.Depts = DeptUtil.All().ToSelectList("Id", "Name", deptId.ToString());
            return Json(data);
        }
        #endregion

        #region 季度汇总
        /// <summary>
        /// 季度汇总饼图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [Route("Report/Saler/Quarter")]
        public IActionResult SalerQuarter(int? year, int deptId = 0)
        {
            InitUI(year, deptId);
            return View();
        }

        /// <summary>
        /// 季度汇总趋势图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [Route("Report/Saler/QuarterLine")]
        public IActionResult SalerQuarterLine(int? year, int deptId = 0)
        {
            InitUI(year, deptId);
            return View();
        }

        /// <summary>
        /// 获取季度汇总源数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public JsonResult GetSalerQuarterData(int year, int deptId)
        {
            var data = reportService.SalerQuarterSummary(year, deptId);
            return Json(data);
        }
        #endregion

        #region 年汇总
        /// <summary>
        /// 年汇总饼图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [Route("Report/Saler/Year")]
        public IActionResult SalerYear(int? year, int deptId = 0)
        {
            InitUI(year, deptId);
            return View();
        }

        /// <summary>
        /// 年汇总趋势图
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [Route("Report/Saler/YearLine")]
        public IActionResult SalerYearLine(int deptId = 0)
        {
            InitUI(0, deptId);
            return View();
        }

        /// <summary>
        /// 获取年汇总源数据
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public JsonResult GetSalerYearData(int deptId)
        {
            var data = reportService.SalerYearSummary(deptId);
            return Json(data);
        }
        #endregion

        /// <summary>
        /// 月详情
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public IActionResult Details(int salerId, int year, int month)
        {
            var service = new MonthReportService();
            var entity = service.LoadDto(salerId, year, month);
            return View(entity);
        }

        /// <summary>
        /// 记录列表
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public IActionResult Items(int salerId, int year, int? quarter)
        {
            var service = new MonthReportService();
            var param = new MonthReportSearchParam
            {
                SalerId = salerId,
                Year = year
            };
            if (quarter.HasValue)
            {
                param.Quarter = quarter;  
            }
            var list = service.Get(param);
            return View(list);
        }

        #region 辅助方法
        /// <summary>
        /// 初始化界面UI
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptId"></param>
        private void InitUI(int? year, int deptId)
        {
            var currentYear = DateTime.Today.Year;
            var years = new List<SelectListItem>();
            for (int i = 0; i < 5; i++)
            {
                var temp = currentYear - i;
                years.Add(new SelectListItem
                {
                    Text = temp.ToString() + "年",
                    Value = temp.ToString(),
                    Selected = (year.HasValue ? year.Value == temp : currentYear == temp)
                });
            }
            ViewBag.Years = years;

            var month = DateTime.Today.Month;
            List<SelectListItem> months = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new SelectListItem { Text = $"{i}月", Value = i.ToString(), Selected = month == i });
            }
            ViewBag.Months = months;

            ViewBag.Groups = new List<SelectListItem>
            {
                new SelectListItem { Text = "效益", Value = "xy" },
                new SelectListItem { Text = "业绩", Value = "yj" },
                new SelectListItem { Text = "毛利", Value = "ml" },
                new SelectListItem { Text = "账面毛利", Value = "zmml" },
                new SelectListItem { Text = "成本", Value = "hj" }
            };

            ViewBag.Depts = DeptUtil.All().ToSelectList("Id", "Name", deptId.ToString());
        }
        #endregion
    }
}