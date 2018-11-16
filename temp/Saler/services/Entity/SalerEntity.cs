using HZC.Core;
using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    [MyDataTable("Base_Saler")]
    public partial class SalerEntity : BaseEntity
    {
        /// <summary>
        /// 销售姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 工资
        /// </summary>
        public int Salary { get; set; }

    }
}
