using HZC.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using Zodo.Saler.Services;
using Zodo.Saler.Website.Extensions;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize(Roles = "admin, 销售总监")]
    public class ReportController : BaseController
    {
        private readonly MonthReportService service = new MonthReportService();
        private readonly ReportService reportService = new ReportService();

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

        #region 报表详情
        public IActionResult Details(int id)
        {
            var entity = service.LoadDto(id);
            return View(entity);
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SyncData(int id)
        {
            var result = service.UpdateLastData(id);
            return Json(result);
        }

        #region 导出
        public IActionResult Export(int year, int month, int? deptId)
        {
            if (year < 2000 || year > DateTime.Today.Year)
            {
                return Content("年度参数必须大于等于2000且小于当前年份");
            }

            if (month < 1 || month > 12)
            {
                return Content("月度参数必须大于等于1，小于等于12");
            }

            var param = new MonthReportSearchParam();
            param.Year = year;
            param.Month = month;
            param.DeptId = deptId;

            var data = service.Get(param);
            try
            {
                var list = service.Get(param);

                string folderName = DateTime.Today.ToString("yyyyMM");
                string fileName = $"{year}年{month}月销售月报.xlsx";
                string baseFolderName = $"{Directory.GetCurrentDirectory()}//wwwroot//report";
                if (!Directory.Exists(baseFolderName))
                {
                    Directory.CreateDirectory(baseFolderName);
                }
                string savePath = $"{baseFolderName}//{fileName}";

                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                }

                using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(savePath)))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("sheet1");
                    workSheet.Cells.Style.Font.Name = "microsoft yahei";
                    workSheet.Cells.Style.Font.Size = 9;
                    workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    workSheet.Cells[1, 1, 1, 28].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, 1, 1, 28].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, 1, 1, 28].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, 1, 1, 28].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, 1, 1, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    workSheet.Cells[2, 1, 2, 28].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[2, 1, 2, 28].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[2, 1, 2, 28].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[2, 1, 2, 28].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[2, 1, 2, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    workSheet.Cells[1, 1, 1, 28].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[1, 1, 1, 28].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    workSheet.Cells[1, 1, 1, 28].Style.Font.Bold = true;
                    workSheet.Cells[1, 1, 1, 28].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[2, 1, 2, 28].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[2, 1, 2, 28].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    workSheet.Cells[2, 1, 2, 28].Style.Font.Bold = true;
                    workSheet.Cells[2, 1, 2, 28].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[1, 1].Value = "部门";
                    workSheet.Cells[1, 2].Value = "业务员";
                    workSheet.Cells[1, 3].Value = "职位";
                    workSheet.Cells[1, 4].Value = "年";
                    workSheet.Cells[1, 5].Value = "月";

                    workSheet.Cells[1, 1, 2, 1].Merge = true;
                    workSheet.Cells[1, 2, 2, 2].Merge = true;
                    workSheet.Cells[1, 3, 2, 3].Merge = true;
                    workSheet.Cells[1, 4, 2, 4].Merge = true;
                    workSheet.Cells[1, 5, 2, 5].Merge = true;

                    workSheet.Cells[1, 6].Value = "所乘交通工具";
                    workSheet.Cells[1, 6, 1, 10].Merge = true;

                    workSheet.Cells[2, 6].Value = "飞机";
                    workSheet.Cells[2, 7].Value = "火车";
                    workSheet.Cells[2, 8].Value = "巴士";
                    workSheet.Cells[2, 9].Value = "自驾车";
                    workSheet.Cells[2, 10].Value = "的士费";

                    workSheet.Cells[1, 11].Value = "业务招待费";
                    workSheet.Cells[1, 12].Value = "住宿费";
                    workSheet.Cells[1, 13].Value = "住宿节约奖励";

                    workSheet.Cells[1, 11, 2, 11].Merge = true;
                    workSheet.Cells[1, 11, 2, 12].Merge = true;
                    workSheet.Cells[1, 11, 2, 13].Merge = true;

                    workSheet.Cells[1, 14].Value = "出差津贴";
                    workSheet.Cells[1, 14, 1, 15].Merge = true;

                    workSheet.Cells[2, 14].Value = "交通津贴";
                    workSheet.Cells[2, 15].Value = "餐费津贴";

                    workSheet.Cells[1, 16].Value = "通讯费";
                    workSheet.Cells[1, 17].Value = "其他费用";
                    workSheet.Cells[1, 18].Value = "薪资";
                    workSheet.Cells[1, 19].Value = "合计";
                    workSheet.Cells[1, 20].Value = "业绩";
                    workSheet.Cells[1, 21].Value = "服务费";
                    workSheet.Cells[1, 22].Value = "账面毛利";
                    workSheet.Cells[1, 23].Value = "毛利";
                    workSheet.Cells[1, 24].Value = "毛利率";
                    workSheet.Cells[1, 25].Value = "销售费用占业绩";
                    workSheet.Cells[1, 26].Value = "效益";
                    workSheet.Cells[1, 27].Value = "上个月效益";
                    workSheet.Cells[1, 28].Value = "去年同月效益";

                    workSheet.Cells[1, 16, 2, 16].Merge = true;
                    workSheet.Cells[1, 17, 2, 17].Merge = true;
                    workSheet.Cells[1, 18, 2, 18].Merge = true;
                    workSheet.Cells[1, 19, 2, 19].Merge = true;
                    workSheet.Cells[1, 20, 2, 20].Merge = true;
                    workSheet.Cells[1, 21, 2, 21].Merge = true;
                    workSheet.Cells[1, 22, 2, 22].Merge = true;
                    workSheet.Cells[1, 23, 2, 23].Merge = true;
                    workSheet.Cells[1, 24, 2, 24].Merge = true;
                    workSheet.Cells[1, 25, 2, 25].Merge = true;
                    workSheet.Cells[1, 26, 2, 26].Merge = true;
                    workSheet.Cells[1, 27, 2, 27].Merge = true;
                    workSheet.Cells[1, 28, 2, 28].Merge = true;

                    workSheet.Row(1).Height = 16;

                    var rowIndex = 3;

                    foreach (var report in list)
                    {
                        workSheet.Cells[rowIndex, 1].Value = report.DeptName;
                        workSheet.Cells[rowIndex, 2].Value = report.SalerName;
                        workSheet.Cells[rowIndex, 3].Value = report.Job;
                        workSheet.Cells[rowIndex, 4].Value = report.Year;
                        workSheet.Cells[rowIndex, 5].Value = report.Month;

                        workSheet.Cells[rowIndex, 6].Value = report.FJ;
                        workSheet.Cells[rowIndex, 7].Value = report.HC;
                        workSheet.Cells[rowIndex, 8].Value = report.BS;
                        workSheet.Cells[rowIndex, 9].Value = report.ZJC;
                        workSheet.Cells[rowIndex, 10].Value = report.DSF;
                        workSheet.Cells[rowIndex, 11].Value = report.YWZDF;
                        workSheet.Cells[rowIndex, 12].Value = report.ZSF;
                        workSheet.Cells[rowIndex, 13].Value = report.ZSJYJL;
                        workSheet.Cells[rowIndex, 14].Value = report.JTJT;
                        workSheet.Cells[rowIndex, 15].Value = report.CFJT;
                        workSheet.Cells[rowIndex, 16].Value = report.TXF;
                        workSheet.Cells[rowIndex, 17].Value = report.QTFY;
                        workSheet.Cells[rowIndex, 18].Value = report.XZ;
                        workSheet.Cells[rowIndex, 19].Value = report.HJ;
                        workSheet.Cells[rowIndex, 20].Value = report.YJ;
                        workSheet.Cells[rowIndex, 21].Value = report.FWF;
                        workSheet.Cells[rowIndex, 22].Value = report.ZMML;
                        workSheet.Cells[rowIndex, 23].Value = report.ML;
                        workSheet.Cells[rowIndex, 24].Value = report.MLL;
                        workSheet.Cells[rowIndex, 25].Value = report.XSFYZYJ;
                        workSheet.Cells[rowIndex, 26].Value = report.XY;
                        workSheet.Cells[rowIndex, 27].Value = report.LastMonthXY;
                        workSheet.Cells[rowIndex, 28].Value = report.LastYearXY;

                        workSheet.Cells[rowIndex, 1, rowIndex, 28].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[rowIndex, 1, rowIndex, 28].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[rowIndex, 1, rowIndex, 28].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[rowIndex, 1, rowIndex, 28].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[rowIndex, 1, rowIndex, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        workSheet.Cells[rowIndex, 1, rowIndex, 28].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        workSheet.Row(rowIndex).Height = 20;
                        rowIndex++;
                    }

                    package.Save();
                    return Redirect($"~/report/{HttpUtility.UrlEncode(fileName)}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return Json(ResultUtil.Fail());
            }
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