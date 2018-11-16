using HZC.Core;
using HZC.Database;
using System;

namespace Zodo.Saler.Services
{
    [MyDataTable("Base_DataItem")]
    public partial class DataItemEntity : BaseEntity
    {
        /// <summary>
        /// 键
        /// </summary>
        public string K { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string V { get; set; }

    }
}
