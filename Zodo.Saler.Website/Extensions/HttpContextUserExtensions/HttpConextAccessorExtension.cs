using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Zodo.Saler.Website.Extensions
{
    public static class HttpConextAccessorExtension
    {
        /// <summary>
        /// 注入IHttpContextAccessor
        /// </summary>
        /// <param name="servcies"></param>
        public static void AddHttpAccessor(this IServiceCollection servcies)
        {
            servcies.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// 将请求过程中将HttpContextAccessor压入到HttpContextUtil
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHttpContextContextUtil(this IApplicationBuilder app)
        {
            var accessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContextUtil.Init(accessor);
            return app;
        }
    }
}
