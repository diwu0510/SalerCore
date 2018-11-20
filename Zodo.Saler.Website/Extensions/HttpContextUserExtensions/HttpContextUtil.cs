using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zodo.Saler.Website.Extensions
{
    /// <summary>
    /// 通过HttpContextAccessor获取HttpContext
    /// 必须与HttpContextAccessorExtensions一起使用
    /// 必须在startup->ConfigureServices中加入 services.AddHttpAccessor();
    /// 必须在startup->Configure中加入 app.UseHttpContextContextUtil();
    /// </summary>
    public static class HttpContextUtil
    {
        private static IHttpContextAccessor _accessor;

        public static HttpContext Current => _accessor.HttpContext;

        /// <summary>
        /// 仅用于HttpContextAccessorExtension中赋值accessor，禁止直接调用
        /// </summary>
        /// <param name="accessor"></param>
        internal static void Init(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
