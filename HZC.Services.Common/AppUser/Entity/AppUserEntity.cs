using HZC.Core;

namespace HZC.Common.Services
{
    [MyDataTable("Base_User")]
    public class AppUserEntity : BaseEntity
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public string Pw { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public bool IsDel { get; set; } = false;
    }
}
