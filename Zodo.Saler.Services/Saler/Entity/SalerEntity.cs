using HZC.Core;

namespace Zodo.Saler.Services
{
    [MyDataTable("Base_Saler")]
    public partial class SalerEntity : BaseEntity
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

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

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
    }
}
