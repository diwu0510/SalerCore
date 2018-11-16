using HZC.Core;

namespace Zodo.Saler.Services
{
    [MyDataTable("Base_DeptSaler")]
    public partial class DeptSalerEntity : BaseEntity
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// 销售ID
        /// </summary>
        public int SalerId { get; set; }
    }
}
