using System;

namespace Zodo.Saler.Application
{
    public partial class SalerDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

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
