using HZC.Core;
using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    [MyDataTable("Base_DeptSaler")]
    public partial class DeptSalerEntity : BaseEntity
    {
        /// <summary>
        /// DeptId
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// SalerId
        /// </summary>
        public int SalerId { get; set; }

    }
}
