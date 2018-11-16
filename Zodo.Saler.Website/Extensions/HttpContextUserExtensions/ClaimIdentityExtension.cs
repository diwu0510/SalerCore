using System;
using System.Security.Claims;

namespace Zodo.Saler.Website.Extensions
{
    public static class ClaimIdentityExtension
    {
        /// <summary>
        /// 获取指定类型的用户信息
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static string FindFirstValue(this ClaimsPrincipal principal, string claimType)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            Claim claim = principal.FindFirst(claimType);
            if (claim != null)
            {
                return claim.Value;
            }

            ClaimsPrincipal claims = new ClaimsPrincipal();

            return string.Empty;
        }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static int FindId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            var idStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            int id = 0;
            Int32.TryParse(idStr, out id);
            return id;
        }
    }
}
