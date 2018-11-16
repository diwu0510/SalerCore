using System;
using System.Collections.Generic;
using System.Text;

namespace Zodo.Saler.Services
{
    /// <summary>
    /// 月度报告批量创建DTO
    /// </summary>
    public class MonthReportMultiCreateDto
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
    }
}
