using HZC.Common.Services;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Zodo.Saler.Website.Models;

namespace Zodo.Saler.Website.Extensions
{
    [Authorize]
    public class BaseController : Controller
    {
        #region 当前用户AppUser
        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public AppUser AppUser
        {
            get
            {
                var u = HttpContext.Session.Get<AppUser>("AppUser");
                if (u == null)
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        u = new AppUser
                        {
                            Id = HttpContext.User.FindId(),
                            Name = HttpContext.User.Identity.Name,
                            Role = HttpContext.User.FindFirstValue(ClaimTypes.Role)
                        };
                        HttpContext.Session.Set<AppUser>("AppUser", u);
                    }
                    else
                    {
                        return null;
                    }
                }
                return u;
            }
        } 
        #endregion

        #region 日志
        private ILog log;

        /// <summary>
        /// 基于Log4Net的Log
        /// </summary>
        protected ILog Log
        {
            get
            {
                if (log == null)
                {
                    string area = "";
                    if (ControllerContext.RouteData.Values["area"] != null)
                    {
                        area = ControllerContext.RouteData.Values["area"].ToString() + "-";
                    }
                    string controller = ControllerContext.RouteData.Values["controller"].ToString();
                    string action = ControllerContext.RouteData.Values["action"].ToString();

                    log = LogManager.GetLogger(
                        Startup.loggerRepository.Name,
                        area + controller + "-" + action
                    );
                }
                return log;
            }
        }
        #endregion

        #region 通用输出
        /// <summary>
        /// 请求的数据不存在
        /// </summary>
        /// <returns></returns>
        public IActionResult Empty(string message = "")
        {
            var error = new ErrorMessageModel();
            error.Title = "404";
            error.Message = string.IsNullOrWhiteSpace(message) ? "请求的数据不存在" : message;
            return RedirectToAction("Error", "Home", error);
        }

        /// <summary>
        /// 无效访问，请求参数错误等
        /// </summary>
        /// <returns></returns>
        public IActionResult Bad(string message = "")
        {
            var error = new ErrorMessageModel();
            error.Title = "500";
            error.Message = string.IsNullOrWhiteSpace(message) ? "无效请求，请从正规途径访问" : message;
            return RedirectToAction("Error", "Home", error);
        }
        #endregion

        #region 通用返回
        public IActionResult Empty()
        {
            return Content("请求的数据为空");
        }
        #endregion

        #region 获取实体验证错误说明
        /// <summary>
        /// 获取实体验证错误说明列表
        /// </summary>
        /// <returns></returns>
        protected List<string> GetModelErrors()
        {
            List<string> errorMsg = new List<string>();
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors.ToList();
                foreach (var error in errors)
                {
                    errorMsg.Add(error.ErrorMessage);
                }
            }
            return errorMsg;
        } 
        #endregion
    }
}
