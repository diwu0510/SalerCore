using HZC.Core;
using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    [MyDataTable("Base_MonthReport")]
    public partial class MonthReportEntity : BaseEntity
    {
        /// <summary>
        /// 销售ID
        /// </summary>
        public int SalerId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 半年，1上半年|2下半年
        /// </summary>
        public int HalfYear { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public int Quarter { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 飞机
        /// </summary>
        public decimal FJ { get; set; } = 0;

        /// <summary>
        /// 火车
        /// </summary>
        public decimal HC { get; set; } = 0;

        /// <summary>
        /// 巴士
        /// </summary>
        public decimal BS { get; set; } = 0;

        /// <summary>
        /// 自驾车
        /// </summary>
        public decimal ZJC { get; set; } = 0;

        /// <summary>
        /// 的士费
        /// </summary>
        public decimal DSF { get; set; } = 0;

        /// <summary>
        /// 业务招待费
        /// </summary>
        public decimal YWZDF { get; set; } = 0;

        /// <summary>
        /// 住宿费
        /// </summary>
        public decimal ZSF { get; set; } = 0;

        /// <summary>
        /// 住宿节约奖励
        /// </summary>
        public decimal ZSJYJL { get; set; } = 0;

        /// <summary>
        /// 交通津贴
        /// </summary>
        public decimal JTJT { get; set; } = 0;

        /// <summary>
        /// 餐费津贴
        /// </summary>
        public decimal CFJT { get; set; } = 0;

        /// <summary>
        /// 通讯费
        /// </summary>
        public decimal TXF { get; set; } = 0;

        /// <summary>
        /// 其他费用
        /// </summary>
        public decimal QTFY { get; set; } = 0;

        /// <summary>
        /// 薪资
        /// </summary>
        public decimal XZ { get; set; } = 0;

        /// <summary>
        /// 合计
        /// </summary>
        [MyDataField(Ignore = true)]
        public double HJ { get; set; }

        /// <summary>
        /// 业绩
        /// </summary>
        public decimal YJ { get; set; } = 0;

        /// <summary>
        /// 服务费
        /// </summary>
        public decimal FWF { get; set; } = 0;

        /// <summary>
        /// 账面毛利
        /// </summary>
        public decimal ZMML { get; set; } = 0;

        /// <summary>
        /// 毛利
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public decimal ML { get; set; } = 0;

        /// <summary>
        /// 效益
        /// </summary>
        [MyDataField(Ignore = true)]
        public decimal XY { get; set; }

        /// <summary>
        /// 上一个月的业绩
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public decimal LastMonthXY { get; set; }

        /// <summary>
        /// 上一年度当月的效益
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public decimal LastYearXY { get; set; }

        /// <summary>
        /// IsDel
        /// </summary>
        public bool IsDel { get; set; } = false;

    }
}
