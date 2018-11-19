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
        /// 员工编号
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// 销售姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 工资
        /// </summary>
        public int Salary { get; set; }

        /// <summary>
        /// 所在部门名称
        /// </summary>
        public string DeptName { get; set; }
    }
}
