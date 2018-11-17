using HZC.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Zodo.Saler.Services;
using Zodo.Saler.Website.Extensions;
using Zodo.Saler.Website.Models;

namespace Zodo.Saler.Website.Controllers
{
    [Authorize(Roles = "admin, 销售管理部")]
    public class SaleManageController : BaseController
    {
        private readonly MonthReportService service = new MonthReportService();
        private IHostingEnvironment _hostingEnvironment;

        public SaleManageController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

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
                var list = service.GetSaleManageList(param);
                return Json(ResultUtil.List<MonthReportSaleManageViewDto>(list));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return Json(ResultUtil.Fail());
            }
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

            var salerService = new SalerService();
            var saler = salerService.Load(entity.SalerId);
            if (saler == null)
            {
                return Content("无效数据");
            }

            var dept = DeptUtil.All().Where(d => d.Id == entity.DeptId).SingleOrDefault();
            if (dept == null)
            {
                return Content("无效数据");
            }

            ViewBag.Title = string.Format("[{0}-{1}][{2}]{3}", entity.Year, entity.Month, dept.Name, saler.Name);

            return View(entity);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public JsonResult Edit(int id, IFormCollection collection)
        {
            var entity = new MonthReportEntity();
            TryUpdateModelAsync(entity);

            var result = service.SaleManageSubmit(entity, AppUser);
            return Json(result);
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

        #region 下载模板
        public IActionResult Template(int year, int month)
        {
            try
            {
                MonthReportSearchParam param = new MonthReportSearchParam();
                param.Year = year;
                param.Month = month;
                var list = service.GetSaleManageList(param);

                string folderName = DateTime.Today.ToString("yyyyMM");
                string fileName = $"销售管理部-{year}{month}.xlsx";
                string baseFolderName = $"{Directory.GetCurrentDirectory()}//wwwroot//template";
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
                    workSheet.Column(12).Width = 13;
                    workSheet.Column(14).Width = 14;

                    workSheet.Cells[1, 1, 1, 18].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, 1, 1, 18].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, 1, 1, 18].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, 1, 1, 18].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, 1, 1, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    int rowIndex = 1;
                    workSheet.Cells[rowIndex, 1, rowIndex, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[rowIndex, 1, rowIndex, 18].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                    workSheet.Cells[rowIndex, 1].Value = "编号";
                    workSheet.Cells[rowIndex, 2].Value = "年";
                    workSheet.Cells[rowIndex, 3].Value = "月";
                    workSheet.Cells[rowIndex, 4].Value = "部门";
                    workSheet.Cells[rowIndex, 5].Value = "业务员";
                    workSheet.Cells[rowIndex, 6].Value = "职位";
                    workSheet.Cells[rowIndex, 7].Value = "飞机";
                    workSheet.Cells[rowIndex, 8].Value = "火车";
                    workSheet.Cells[rowIndex, 9].Value = "巴士";
                    workSheet.Cells[rowIndex, 10].Value = "自驾车";
                    workSheet.Cells[rowIndex, 11].Value = "的士费";
                    workSheet.Cells[rowIndex, 12].Value = "业务招待费";
                    workSheet.Cells[rowIndex, 13].Value = "住宿费";
                    workSheet.Cells[rowIndex, 14].Value = "住宿节约奖励";
                    workSheet.Cells[rowIndex, 15].Value = "交通津贴";
                    workSheet.Cells[rowIndex, 16].Value = "餐费津贴";
                    workSheet.Cells[rowIndex, 17].Value = "通讯费";
                    workSheet.Cells[rowIndex, 18].Value = "其他费用";

                    workSheet.Row(rowIndex).Height = 28;

                    rowIndex++;

                    foreach (var report in list)
                    {
                        workSheet.Cells[rowIndex, 1].Value = report.Id;
                        workSheet.Cells[rowIndex, 2].Value = report.Year;
                        workSheet.Cells[rowIndex, 3].Value = report.Month;
                        workSheet.Cells[rowIndex, 4].Value = report.DeptName.ToString();
                        workSheet.Cells[rowIndex, 5].Value = report.SalerName.ToString();
                        workSheet.Cells[rowIndex, 6].Value = report.Job;
                        workSheet.Cells[rowIndex, 7].Value = report.FJ;
                        workSheet.Cells[rowIndex, 8].Value = report.HC;
                        workSheet.Cells[rowIndex, 9].Value = report.BS;
                        workSheet.Cells[rowIndex, 10].Value = report.ZJC;
                        workSheet.Cells[rowIndex, 11].Value = report.DSF;
                        workSheet.Cells[rowIndex, 12].Value = report.YWZDF;
                        workSheet.Cells[rowIndex, 13].Value = report.ZSF;
                        workSheet.Cells[rowIndex, 14].Value = report.ZSJYJL;
                        workSheet.Cells[rowIndex, 15].Value = report.JTJT;
                        workSheet.Cells[rowIndex, 16].Value = report.CFJT;
                        workSheet.Cells[rowIndex, 17].Value = report.TXF;
                        workSheet.Cells[rowIndex, 18].Value = report.QTFY;

                        workSheet.Cells[rowIndex, 1, rowIndex, 18].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[rowIndex, 1, rowIndex, 18].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[rowIndex, 1, rowIndex, 18].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[rowIndex, 1, rowIndex, 18].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[rowIndex, 1, rowIndex, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                        workSheet.Row(rowIndex).Height = 24;
                        rowIndex++;
                    }

                    package.Save();
                    return Redirect($"~/template/{HttpUtility.UrlEncode(fileName)}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return Json(ResultUtil.Fail());
            }
        }
        #endregion

        #region 导入Excel
        public IActionResult Import(int year, int month)
        {
            ViewBag.ReturnUrl = $"/SaleManage/Index?year={year}&month={month}";
            ViewBag.Year = year;
            ViewBag.Month = month;
            return View();
        }

        [HttpPost]
        public IActionResult Import(int year, int month, IFormFile excelFile)
        {
            ViewBag.ReturnUrl = $"/SaleManage/Index?year={year}&month={month}";
            ViewBag.ImportUrl = $"/SaleManage/Import?year={year}&month={month}";

            // 基础验证
            if (excelFile == null || Path.GetExtension(excelFile.FileName) != ".xlsx")
            {
                return Content("不受支持的文件");
            }

            // 获取原始数据
            MonthReportSearchParam param = new MonthReportSearchParam();
            param.Month = month;
            param.Year = year;
            MonthReportService service = new MonthReportService();
            var source = service.GetSaleManageList(param);
            if (source.Count() == 0)
            {
                return Content("当前日期原数据为空");
            }

            // 解析Excel中的记录
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string folderName = Path.Combine(sWebRootFolder, "reports");
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            FileInfo file = new FileInfo(Path.Combine(folderName, excelFile.FileName));
            try
            {
                using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
                {
                    excelFile.CopyTo(fs);
                    fs.Flush();
                }
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    if (ColCount != 18)
                    {
                        return Content("不合法的数据列");
                    }

                    List<ImportResultViewModel> result = new List<ImportResultViewModel>();

                    // 获取转换后的数据列表
                    List<MonthReportEntity> content = new List<MonthReportEntity>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        Convert(worksheet, row);
                    }

                    // 内部方法，单条处理数据
                    void Convert(ExcelWorksheet sheet, int idx)
                    {
                        try
                        {
                            var entity = new MonthReportEntity();

                            int id = 0;
                            decimal fj = 0;
                            decimal hc = 0;
                            decimal bs = 0;
                            decimal zjc = 0;
                            decimal dsf = 0;
                            decimal ywzdf = 0;
                            decimal zsf = 0;
                            decimal zsjyjl = 0;
                            decimal jtjt = 0;
                            decimal cfjt = 0;
                            decimal txf = 0;
                            decimal qtfy = 0;

                            int y = 0;
                            int m = 0;

                            int.TryParse(sheet.Cells[idx, 2].Value.ToString(), out y);
                            int.TryParse(sheet.Cells[idx, 3].Value.ToString(), out m);

                            int.TryParse(sheet.Cells[idx, 1].Value.ToString(), out id);
                            decimal.TryParse(sheet.Cells[idx, 7].Value.ToString(), out fj);
                            decimal.TryParse(sheet.Cells[idx, 8].Value.ToString(), out hc);
                            decimal.TryParse(sheet.Cells[idx, 9].Value.ToString(), out bs);
                            decimal.TryParse(sheet.Cells[idx, 10].Value.ToString(), out zjc);
                            decimal.TryParse(sheet.Cells[idx, 11].Value.ToString(), out dsf);
                            decimal.TryParse(sheet.Cells[idx, 12].Value.ToString(), out ywzdf);
                            decimal.TryParse(sheet.Cells[idx, 13].Value.ToString(), out zsf);
                            decimal.TryParse(sheet.Cells[idx, 14].Value.ToString(), out zsjyjl);
                            decimal.TryParse(sheet.Cells[idx, 15].Value.ToString(), out jtjt);
                            decimal.TryParse(sheet.Cells[idx, 16].Value.ToString(), out cfjt);
                            decimal.TryParse(sheet.Cells[idx, 17].Value.ToString(), out txf);
                            decimal.TryParse(sheet.Cells[idx, 18].Value.ToString(), out qtfy);


                            entity.Id = id;
                            entity.FJ = fj;
                            entity.HC = hc;
                            entity.BS = bs;
                            entity.ZJC = zjc;
                            entity.DSF = dsf;
                            entity.YWZDF = ywzdf;
                            entity.ZSF = zsf;
                            entity.ZSJYJL = zsjyjl;
                            entity.JTJT = jtjt;
                            entity.CFJT = cfjt;
                            entity.TXF = txf;
                            entity.QTFY = qtfy;
                            entity.Year = y;
                            entity.Month = m;

                            var deptName = sheet.Cells[idx, 4].Value.ToString();
                            var salerName = sheet.Cells[idx, 5].Value.ToString();

                            var r = new ImportResultViewModel
                            {
                                Id = id,
                                DeptName = deptName,
                                SalerName = salerName,
                                Result = "清洗失败",
                                Remark = "源数据中不存在此记录"
                            };

                            var s = source.Where(mr => mr.Id == id).SingleOrDefault();
                            if (s == null)
                            {
                                r.Result = "数据清洗失败";
                                r.Remark = "元数据中不存在此记录";
                            }
                            else if (s.Year != year || s.Month != month)
                            {
                                r.Result = "数据清洗失败";
                                r.Remark = "年度和月度与源数据不符";
                            }
                            else if (s.SalerName.Trim() != salerName.Trim() || s.DeptName.Trim() != deptName.Trim())
                            {
                                r.Result = "数据清洗失败";
                                r.Remark = "部门和业务员不匹配";
                            }
                            else
                            {
                                var submitResult = service.SaleManageSubmit(entity, AppUser);
                                if (submitResult.Code == 200)
                                {
                                    r.Result = "更新成功";
                                    r.Remark = "";
                                }
                                else
                                {
                                    r.Result = "数据更新失败";
                                    r.Remark = submitResult.Message;
                                }
                            }
                            result.Add(r);
                        }
                        catch (Exception ex)
                        {
                            result.Add(new ImportResultViewModel
                            {
                                Id = 0,
                                DeptName = "",
                                SalerName = "",
                                Result = "系统错误",
                                Remark = $"第{idx}条记录发生错误：{ex.Message}"
                            });
                        }
                    }

                    return View("Result", result);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        } 
        #endregion
    }
}