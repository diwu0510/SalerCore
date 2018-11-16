using HZC.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zodo.Saler.Services
{
    public class ReportService
    {
        private MyDbUtil db = new MyDbUtil();

        #region 部门汇总
        /// <summary>
        /// 部门月度汇总
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<SummaryViewModel> DeptMonthSummary(int year, bool isContainsDeletedDept = false)
        {
            string sql = "SELECT DeptId AS Id,Month AS DateUnit,SUM(HJ) AS HJ,SUM(ML) AS ML,SUM(ZMML) AS ML,SUM(XY) AS XY FROM Base_MonthReport WHERE Year=@Year AND IsDel=0 GROUP BY DeptId,Month";
            var list = db.FetchBySql<SummaryViewModel>(sql, new { Year = year });

            var depts = DeptUtil.All();

            var result = new List<SummaryViewModel>();

            foreach (var dept in depts.OrderBy(d => d.Sort))
            {
                var datas = list.Where(m => m.Id == dept.Id);
                for (var i = 1; i <= 12; i++)
                {
                    var data = datas.Where(d => d.DateUnit == i).SingleOrDefault();
                    if (data == null)
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = dept.Id,
                            Name = dept.Name,
                            HJ = 0,
                            ZMML = 0,
                            YJ = 0,
                            ML = 0,
                            DateUnit = i,
                            Year = year,
                            XY = 0
                        });
                    }
                    else
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = dept.Id,
                            Name = dept.Name,
                            HJ = data.HJ,
                            ZMML = data.ZMML,
                            YJ = data.YJ,
                            ML = data.ML,
                            DateUnit = i,
                            Year = year,
                            XY = data.XY
                        });

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 部门季度汇总
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<SummaryViewModel> DeptQuarterSummary(int year, bool isContainsDeletedDept = false)
        {
            string sql = "SELECT DeptId AS Id,Quarter AS DateUnit,SUM(HJ) AS HJ,SUM(ML) AS ML,SUM(ZMML) AS ML,SUM(XY) AS XY FROM Base_MonthReport WHERE Year=@Year AND IsDel=0 GROUP BY DeptId,Quarter";
            var list = db.FetchBySql<SummaryViewModel>(sql, new { Year = year });

            var depts = DeptUtil.All();

            var result = new List<SummaryViewModel>();
            foreach (var dept in depts.OrderBy(d => d.Sort))
            {
                var datas = list.Where(m => m.Id == dept.Id);
                for (var i = 1; i <= 4; i++)
                {
                    var data = datas.Where(d => d.DateUnit == i).SingleOrDefault();
                    if (data == null)
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = dept.Id,
                            Name = dept.Name,
                            HJ = 0,
                            ZMML = 0,
                            YJ = 0,
                            ML = 0,
                            DateUnit = i,
                            Year = year,
                            XY = 0
                        });
                    }
                    else
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = dept.Id,
                            Name = dept.Name,
                            HJ = data.HJ,
                            ZMML = data.ZMML,
                            YJ = data.YJ,
                            ML = data.ML,
                            DateUnit = i,
                            Year = year,
                            XY = data.XY
                        });

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 部门年汇总
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<SummaryViewModel> DeptYearSummary(bool isContainsDeletedDept = false)
        {
            List<int> years = new List<int>();
            int year = DateTime.Today.Year;

            string sql = "SELECT DeptId AS Id,[Year] AS DateUnit,SUM(HJ) AS HJ,SUM(ML) AS ML,SUM(ZMML) AS ML,SUM(XY) AS XY FROM Base_MonthReport WHERE ([Year] BETWEEN @Year-4 AND @Year) AND IsDel=0 GROUP BY DeptId,Year";
            var list = db.FetchBySql<SummaryViewModel>(sql, new { Year = year });

            var depts = DeptUtil.All();

            var result = new List<SummaryViewModel>();
            foreach (var dept in depts.OrderBy(d => d.Sort))
            {
                var datas = list.Where(m => m.Id == dept.Id);
                for (var i = year - 4; i <= year; i++)
                {
                    var data = datas.Where(d => d.DateUnit == i).SingleOrDefault();
                    if (data == null)
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = dept.Id,
                            Name = dept.Name,
                            HJ = 0,
                            ZMML = 0,
                            YJ = 0,
                            ML = 0,
                            DateUnit = i,
                            Year = year,
                            XY = 0
                        });
                    }
                    else
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = dept.Id,
                            Name = dept.Name,
                            HJ = data.HJ,
                            ZMML = data.ZMML,
                            YJ = data.YJ,
                            ML = data.ML,
                            DateUnit = i,
                            Year = year,
                            XY = data.XY
                        });
                    }
                }
            }
            return result;
        }
        #endregion

        #region 员工汇总
        /// <summary>
        /// 部门月度汇总
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<SummaryViewModel> SalerMonthSummary(int year, int deptId = 0, bool isContainsDeletedSaler = false)
        {
            string sql = $"SELECT SalerId AS Id,Month AS DateUnit,SUM(HJ) AS HJ,SUM(ML) AS ML,SUM(ZMML) AS ML,SUM(XY) AS XY FROM Base_MonthReport WHERE Year=@Year{(deptId > 0 ? " AND DeptId=@DeptId" : "")} AND IsDel=0 GROUP BY SalerId,Month";
            var list = db.FetchBySql<SummaryViewModel>(sql, new { Year = year, DeptId = deptId });

            var salers = SalerUtil.All();
            if (deptId > 0)
            {
                salers = salers.Where(s => s.DeptId == deptId).ToList();
            }

            var result = new List<SummaryViewModel>();

            foreach (var saler in salers.OrderBy(d => d.Name))
            {
                var datas = list.Where(m => m.Id == saler.Id);
                for (var i = 1; i <= 12; i++)
                {
                    var data = datas.Where(d => d.DateUnit == i).SingleOrDefault();
                    if (data == null)
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = saler.Id,
                            Name = saler.Name,
                            HJ = 0,
                            ZMML = 0,
                            YJ = 0,
                            ML = 0,
                            DateUnit = i,
                            Year = year,
                            XY = 0
                        });
                    }
                    else
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = saler.Id,
                            Name = saler.Name,
                            HJ = data.HJ,
                            ZMML = data.ZMML,
                            YJ = data.YJ,
                            ML = data.ML,
                            DateUnit = i,
                            Year = year,
                            XY = data.XY
                        });

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 部门季度汇总
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<SummaryViewModel> SalerQuarterSummary(int year, int deptId = 0, bool isContainsDeletedSaler = false)
        {
            string sql = $"SELECT SalerId AS Id,Quarter AS DateUnit,SUM(HJ) AS HJ,SUM(ML) AS ML,SUM(ZMML) AS ML,SUM(XY) AS XY FROM Base_MonthReport WHERE Year=@Year{(deptId > 0 ? " AND DeptId = @DeptId" : "")} AND IsDel=0 GROUP BY SalerId,Quarter";
            var list = db.FetchBySql<SummaryViewModel>(sql, new { Year = year, DeptId = deptId });

            var salers = SalerUtil.All();
            if (deptId > 0)
            {
                salers = salers.Where(s => s.DeptId == deptId).ToList();
            }

            var result = new List<SummaryViewModel>();
            foreach (var saler in salers.OrderBy(d => d.Name))
            {
                var datas = list.Where(m => m.Id == saler.Id);
                for (var i = 1; i <= 4; i++)
                {
                    var data = datas.Where(d => d.DateUnit == i).SingleOrDefault();
                    if (data == null)
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = saler.Id,
                            Name = saler.Name,
                            HJ = 0,
                            ZMML = 0,
                            YJ = 0,
                            ML = 0,
                            DateUnit = i,
                            Year = year,
                            XY = 0
                        });
                    }
                    else
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = saler.Id,
                            Name = saler.Name,
                            HJ = data.HJ,
                            ZMML = data.ZMML,
                            YJ = data.YJ,
                            ML = data.ML,
                            DateUnit = i,
                            Year = year,
                            XY = data.XY
                        });

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 部门年汇总
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public List<SummaryViewModel> SalerYearSummary(int deptId = 0, bool isContainsDeletedSaler = false)
        {
            List<int> years = new List<int>();
            int y = DateTime.Today.Year;

            string sql = $"SELECT SalerId AS Id,[Year] AS DateUnit,SUM(HJ) AS HJ,SUM(ML) AS ML,SUM(ZMML) AS ML,SUM(XY) AS XY FROM Base_MonthReport WHERE [Year] BETWEEN @Year-4 AND @Year{(deptId > 0 ? " AND DeptId = @DeptId" : "")} AND IsDel=0 GROUP BY SalerId,Year";
            var list = db.FetchBySql<SummaryViewModel>(sql, new { Year = y, DeptId = deptId });

            var salers = SalerUtil.All();
            if (deptId > 0)
            {
                salers = salers.Where(s => s.DeptId == deptId).ToList();
            }

            var result = new List<SummaryViewModel>();
            foreach (var saler in salers.OrderBy(d => d.Name))
            {
                var datas = list.Where(m => m.Id == saler.Id);
                for (var i = y - 4; i <= y; i++)
                {
                    var data = datas.Where(d => d.DateUnit == i).SingleOrDefault();
                    if (data == null)
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = saler.Id,
                            Name = saler.Name,
                            HJ = 0,
                            ZMML = 0,
                            YJ = 0,
                            ML = 0,
                            DateUnit = i,
                            Year = y,
                            XY = 0
                        });
                    }
                    else
                    {
                        result.Add(new SummaryViewModel
                        {
                            Id = saler.Id,
                            Name = saler.Name,
                            HJ = data.HJ,
                            ZMML = data.ZMML,
                            YJ = data.YJ,
                            ML = data.ML,
                            DateUnit = i,
                            Year = y,
                            XY = data.XY
                        });

                    }
                }
            }
            return result;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
