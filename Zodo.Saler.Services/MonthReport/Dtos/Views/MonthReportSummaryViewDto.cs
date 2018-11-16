using System;
using System.Collections.Generic;
using System.Text;

namespace Zodo.Saler.Services.MonthReport.Dtos.Views
{
    public class MonthReportGroupViewDto
    {
        /// <summary>
        /// 分组对象ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分组对象名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 分组依据的年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 分组依据的日期子项目，可能是月|季度|上半年下半年
        /// </summary>
        public int DateUnit { get; set; }

        /// <summary>
        /// 业绩总和
        /// </summary>
        public decimal YJ { get; set; }

        /// <summary>
        /// 账面毛利总和
        /// </summary>
        public decimal ZMML { get; set; }

        /// <summary>
        /// 毛利总和
        /// </summary>
        public decimal ML { get; set; }

        /// <summary>
        /// 效益总和
        /// </summary>
        public decimal XY { get; set; }

        /// <summary>
        /// 费用合计总和
        /// </summary>
        public decimal ZH { get; set; }

        /// <summary>
        /// 销售费用占业绩
        /// </summary>
        public decimal XSFYZYJ { get; set; }
    }
}
