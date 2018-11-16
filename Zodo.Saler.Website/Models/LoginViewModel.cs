namespace Zodo.Saler.Website.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pw { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
    }
}
