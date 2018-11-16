using HZC.Core;

namespace HZC.Common.Services
{
    [MyDataTable("Base_DataItem")]
    public class DataItemEntity : BaseEntity
    {
        /// <summary>
        /// 数据字典的键
        /// </summary>
        public string K { get; set; }

        /// <summary>
        /// 数据字典的值
        /// </summary>
        public string V { get; set; }

        /// <summary>
        /// 字典项目说明
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public string Cate { get; set; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool Editable { get; set; } = true;

        /// <summary>
        /// 是否已经删除
        /// </summary>
        public bool IsDel { get; set; } = false;
    }
}
