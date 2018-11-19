using HZC.Common.Services;
using HZC.Core;
using HZC.Database;
using System;
using System.Collections.Generic;

namespace Zodo.Saler.Services
{
    public partial class MonthReportService : BaseService<MonthReportEntity>
    {
        /// <summary>
        /// 销售管理部可编辑的字段
        /// </summary>
        private readonly string[] COLUMNS_SALEMANAGE = new string[]
        {
            "FJ", "HC", "BS", "ZJC", "DSF", "YWZDF", "ZSF", "ZSJYJL", "ZBFY", "CCBT", "TXF", "QTFY"
        };

        /// <summary>
        /// 人事部可编辑的字段
        /// </summary>
        private readonly string[] COLUMNS_PERSONNEL = new string[]
        {
            "XZ"
        };

        /// <summary>
        /// 销售服务部可编辑的字段
        /// </summary>
        private readonly string[] COLUMNS_SALESERVICE = new string[]
        {
            "YJ", "ZMML"
        };

        /// <summary>
        /// 证券事业部可编辑的字段
        /// </summary>
        private readonly string[] COLUMNS_SECURITY = new string[]
        {
            "FWF"
        };

        #region 列表视图数据
        /// <summary>
        /// 管理员视图，可以查看所有列
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<MonthReportDto> Get(MonthReportSearchParam param)
        {
            var list = db.Fetch<MonthReportDto>(param.ToSearchUtil(), "MonthReportView");
            return list;
        }

        /// <summary>
        /// 销售管理部视图
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<MonthReportSaleManageViewDto> GetSaleManageList(MonthReportSearchParam param)
        {
            var list = db.Fetch<MonthReportSaleManageViewDto>(param.ToSearchUtil(), "MonthReportView");
            return list;
        }

        /// <summary>
        /// 销售服务部视图
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<MonthReportSaleServiceViewDto> GetSaleServiceList(MonthReportSearchParam param)
        {
            var list = db.Fetch<MonthReportSaleServiceViewDto>(param.ToSearchUtil(), "MonthReportView");
            return list;
        }

        /// <summary>
        /// 人事部视图
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<MonthReportPersonnelViewDto> GetPersonnelList(MonthReportSearchParam param)
        {
            var list = db.Fetch<MonthReportPersonnelViewDto>(param.ToSearchUtil(), "MonthReportView");
            return list;
        }

        /// <summary>
        /// 证券事务部视图
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<MonthReportSecurityViewDto> GetSecurityList(MonthReportSearchParam param)
        {
            var list = db.Fetch<MonthReportSecurityViewDto>(param.ToSearchUtil(), "MonthReportView");
            return list;
        }
        #endregion

        #region 创建记录
        /// <summary>
        /// 创建单一记录
        /// </summary>
        /// <param name="salerId">销售ID</param>
        /// <param name="deptId">部门ID</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        public Result Create(MonthReportCreateDto entity, IAppUser user)
        {
            var quarter = 1;
            var halfYear = 1;
            if (entity.Month > 0 && entity.Month <= 3)
            {
                quarter = 1;
            }
            else if (entity.Month > 3 && entity.Month <= 6)
            {
                quarter = 2;
            }
            else if (entity.Month > 6 && entity.Month <= 9)
            {
                quarter = 3;
                halfYear = 2;
            }
            else if (entity.Month > 9 && entity.Month <= 12)
            {
                quarter = 4;
                halfYear = 2;
            }
            else
            {
                return ResultUtil.Fail("无效的月度参数");
            }

            var count = db.GetCount<MonthReportEntity>(MySearchUtil.New()
                .AndEqual("Year", entity.Year)
                .AndEqual("Month", entity.Month)
                .AndEqual("SalerId", entity.SalerId));
            if (count > 0)
            {
                return ResultUtil.Fail("该用户记录已存在，添加失败");
            }

            string sql = @"
INSERT INTO Base_MonthReport (SalerId,DeptId,Year,HalfYear,Quarter,Month,FJ,HC,BS,ZJC,DSF,YWZDF,ZSF,ZSJYJL,ZBFY,CCBT,TXF,QTFY,XZ,YJ,FWF,ZMML,IsDel,CreateAt,CreateBy,Creator,UpdateAt,UpdateBy,Updator) 
SELECT Id,DeptId,@Year,@HalfYear,@Quarter,@Month,0,0,0,0,0,0,0,0,0,0,0,0,Salary,0,0,0,0,GETDATE(),@UserId,@UserName,GETDATE(),@UserId,@UserName
FROM Base_Saler WHERE Id=@Id And IsDel=0";

            var row = db.Execute(sql, new { Id = entity.SalerId, Year = entity.Year, HalfYear = halfYear, Quarter = quarter, Month = entity.Month, UserId = user.Id, UserName = user.Name });
            return row > 0 ? ResultUtil.Success() : ResultUtil.Fail("创建失败，此业务员可能已删除");
        }

        /// <summary>
        /// 批量创建空白记录
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        public Result Create(MonthReportMultiCreateDto entity, IAppUser user)
        {
            var quarter = 1;
            var halfYear = 1;
            if (entity.Month > 0 && entity.Month <= 3)
            {
                quarter = 1;
            }
            else if (entity.Month > 3 && entity.Month <= 6)
            {
                quarter = 2;
            }
            else if (entity.Month > 6 && entity.Month <= 9)
            {
                quarter = 3;
                halfYear = 2;
            }
            else if (entity.Month > 9 && entity.Month <= 12)
            {
                quarter = 4;
                halfYear = 2;
            }
            else
            {
                return ResultUtil.Fail("无效的月度参数");
            }

            var count = db.GetCount<MonthReportEntity>(MySearchUtil.New()
                .AndEqual("Month", entity.Month)
                .AndEqual("Year", entity.Year));
            if (count > 0)
            {
                return ResultUtil.Fail("已存在该年度该月记录，禁止批量添加");
            }

            var depts = DeptUtil.All();
            string sql = @"
INSERT INTO Base_MonthReport (SalerId,DeptId,Year,HalfYear,Quarter,Month,FJ,HC,BS,ZJC,DSF,YWZDF,ZSF,ZSJYJL,ZBFY,CCBT,TXF,QTFY,XZ,YJ,FWF,ZMML,IsDel,CreateAt,CreateBy,Creator,UpdateAt,UpdateBy,Updator) 
SELECT Id,DeptId,@Year,@HalfYear,@Quarter,@Month,0,0,0,0,0,0,0,0,0,0,0,0,Salary,0,0,0,0,GETDATE(),@UserId,@UserName,GETDATE(),@UserId,@UserName
FROM Base_Saler WHERE IsDel=0 ORDER BY DeptId,Name";
            var row = db.Execute(sql, new { Year = entity.Year, Month = entity.Month, HalfYear = halfYear, Quarter = quarter, UserId = user.Id, UserName = user.Name });
            return row > 0 ? ResultUtil.Success<int>(row) : ResultUtil.Fail();
        }

        public Result UpdateLastRecord(int year, int month)
        {
            return ResultUtil.Success();
        }
        #endregion

        /// <summary>
        /// 销售管理部提交数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Result SaleManageSubmit(MonthReportEntity entity, IAppUser user)
        {
            string sql = "UPDATE Base_MonthReport SET {0} WHERE Id=@Id";
            List<string> columns = new List<string>();
            foreach (var str in COLUMNS_SALEMANAGE)
            {
                columns.Add(str + "=@" + str);
            }
            var row = db.Execute(string.Format(sql, string.Join(',', columns)), entity);
            return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
        }

        /// <summary>
        /// 人事部提交数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Result PersonnelSubmit(MonthReportEntity entity, IAppUser user, bool isAsync)
        {
            if (!isAsync)
            {
                string sql = "UPDATE Base_MonthReport SET {0} WHERE Id=@Id";
                List<string> columns = new List<string>();
                foreach (var str in COLUMNS_PERSONNEL)
                {
                    columns.Add(str + "=@" + str);
                }
                var row = db.Execute(string.Format(sql, string.Join(',', columns)), entity);
                return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
            }
            else
            {
                string sql = "UPDATE Base_MonthReport SET {0} WHERE Id=@Id";
                List<string> columns = new List<string>();
                foreach (var str in COLUMNS_PERSONNEL)
                {
                    columns.Add(str + "=@" + str);
                }
                KeyValuePairs sqls = new KeyValuePairs();
                sqls.Add(string.Format(sql, string.Join(',', columns)), entity);
                sqls.Add("UPDATE Base_Saler SET Salary=@Salary WHERE Id=@SalerId", new { SalerId = entity.SalerId, Salary = entity.XZ });
                var row = db.ExecuteTran(sqls);
                return row ? ResultUtil.Success() : ResultUtil.Fail();
            }
        }

        /// <summary>
        /// 销售服务部提交数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Result SaleServiceSubmit(MonthReportEntity entity, IAppUser user)
        {
            string sql = "UPDATE Base_MonthReport SET {0} WHERE Id=@Id";
            List<string> columns = new List<string>();
            foreach (var str in COLUMNS_SALESERVICE)
            {
                columns.Add(str + "=@" + str);
            }
            var row = db.Execute(string.Format(sql, string.Join(',', columns)), entity);
            return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
        }

        /// <summary>
        /// 证券事务部提交数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Result SecuritySubmit(MonthReportEntity entity, IAppUser user)
        {
            string sql = "UPDATE Base_MonthReport SET {0} WHERE Id=@Id";
            List<string> columns = new List<string>();
            foreach (var str in COLUMNS_SECURITY)
            {
                columns.Add(str + "=@" + str);
            }
            var row = db.Execute(string.Format(sql, string.Join(',', columns)), entity);
            return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
        }

        /// <summary>
        /// 加载一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MonthReportDto LoadDto(int id)
        {
            var entity = db.LoadBySql<MonthReportDto>("SELECT * FROM MonthReportView WHERE Id=@Id", new { Id = id });
            return entity;
        }

        /// <summary>
        /// 根据业务员、年、月获取实体
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public MonthReportDto LoadDto(int salerId, int year, int month)
        {
            var entity = db.LoadBySql<MonthReportDto>("SELECT * FROM MonthReportView WHERE SalerId=@SalerId AND Year=@Year AND Month=@Month",
                new { SalerId = salerId, Year = year, Month = month });
            return entity;
        }

        /// <summary>
        /// 更新同比环比数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result UpdateLastData(int id)
        {
            var entity = Load(id);
            if (entity == null)
            {
                return ResultUtil.AuthFail("请求的数据不存在");
            }

            int lastMonthYear = entity.Year;
            int lastMonthMonth = entity.Month;

            int lastYearYear = entity.Year - 1;
            int lastYearMonth = entity.Month;

            if (lastMonthMonth == 1)
            {
                lastMonthMonth = 12;
                lastMonthYear = lastMonthYear - 1;
            }
            else
            {
                lastMonthMonth = lastMonthMonth - 1;
            }

            KeyValuePairs kvs = new KeyValuePairs();
            kvs.Add(@"UPDATE Base_MonthReport SET LastMonthXY=(SELECT XY FROM Base_MonthReport WHERE SalerId=@SalerId AND Year=@Year AND Month=@Month) WHERE Id=@Id",
                new { Id = id, Year = lastMonthYear, Month = lastMonthMonth, SalerId = entity.SalerId });
            kvs.Add(@"UPDATE Base_MonthReport SET LastYearXY=(SELECT XY FROM Base_MonthReport WHERE SalerId=@SalerId AND Year=@Year AND Month=@Month) WHERE Id=@Id",
                new { Id = id, Year = lastYearYear, Month = lastYearMonth, SalerId = entity.SalerId });

            var result = db.ExecuteTran(kvs);
            return result ? ResultUtil.Success() : ResultUtil.Fail();
        }

        #region 重写实体验证
        protected override string ValidateCreate(MonthReportEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateUpdate(MonthReportEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateDelete(MonthReportEntity t, IAppUser user)
        {
            return string.Empty;
        }
        #endregion
    }
}

