using HZC.Core;
using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    [MyDataTable("Base_Dept")]
    public partial class DeptEntity : BaseEntity
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

    }
}
