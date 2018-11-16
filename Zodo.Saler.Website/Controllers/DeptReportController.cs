using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Zodo.Saler.Services;
using Zodo.Saler.Website.Extensions;

namespace Zodo.Saler.Website.Controllers
{
    public class DeptReportController : Controller
    {
        private readonly ReportService reportService = new ReportService();

        #region 月汇总
        [Route("Report/Dept/Month")]
        public IActionResult DeptMonth(int? year)
        {
            InitUI(year);
            return View();
        }

        [Route("Report/Dept/MonthLine")]
        public IActionResult DeptMonthLine(int? year)
        {
            InitUI(year);
            return View();
        }

        [Route("Report/Dept/MonthBar")]
        public IActionResult DeptMonthBar(int? year)
        {
            InitUI(year);
            return View();
        }

        public JsonResult GetDeptMonthData(int year)
        {
            var data = reportService.DeptMonthSummary(year);
            return Json(data);
        }
        #endregion

        #region 季度汇总
        [Route("Report/Dept/Quarter")]
        public IActionResult DeptQuarter(int? year)
        {
            InitUI(year);
            return View();
        }

        [Route("Report/Dept/QuarterLine")]
        public IActionResult DeptQuarterLine(int? year)
        {
            InitUI(year);
            return View();
        }

        public JsonResult GetDeptQuarterData(int year)
        {
            var data = reportService.DeptQuarterSummary(year);
            return Json(data);
        }
        #endregion

        #region 年汇总
        [Route("Report/Dept/Year")]
        public IActionResult DeptYear(int? year)
        {
            InitUI(year);
            return View();
        }

        [Route("Report/Dept/YearLine")]
        public IActionResult DeptYearLine()
        {
            InitUI(0);
            return View();
        }

        public JsonResult GetDeptYearData()
        {
            var data = reportService.DeptYearSummary();
            return Json(data);
        }
        #endregion
        
        public IActionResult Details(string type, int year, int dateUnit, int deptId)
        {
            IEnumerable<SummaryViewModel> data = new List<SummaryViewModel>();
            switch (type.ToLower().Trim())
            {
                case "month":
                    data = reportService.SalerMonthSummary(year, deptId).Where(r => r.DateUnit == dateUnit);
                    break;
                case "quarter":
                    data = reportService.SalerQuarterSummary(year, deptId).Where(r => r.DateUnit == dateUnit);
                    break;
                case "year":
                    data = reportService.SalerYearSummary(deptId).Where(r => r.DateUnit == dateUnit);
                    break;
                default:
                    break;
            }
            return View(data);
        }

        #region 辅助方法
        private void InitUI(int? year)
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

            ViewBag.Depts = DeptUtil.All().ToSelectList("Id", "Name");
        }
        #endregion
    }
}