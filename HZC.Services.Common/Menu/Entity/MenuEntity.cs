using HZC.Core;

namespace HZC.Common.Services
{
    [MyDataTable("Base_Menu")]
    public class MenuEntity : BaseEntity
    {
        /// <summary>
        /// 菜单显示的文字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单导航
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 上级菜单
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 哪些角色可见
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; } = false;
    }
}
