using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DataTableToController
    {
        public static string Get(DbTable table, string ns, string areaName)
        {
            StringBuilder sb = new StringBuilder();
            int total = table.columns.Count;

            string className = "";
            string[] subNames = table.name.Split('_');
            if (subNames.Length > 0)
            {
                className = subNames[subNames.Length - 1];
            }
            else
            {
                className = table.name;
            }

            // 命名空间
            sb.Append("using System;" + Environment.NewLine);
            sb.Append("using System.Collections.Generic;" + Environment.NewLine);
            sb.Append("using System.Linq;" + Environment.NewLine);
            sb.Append("using Microsoft.AspNetCore.Authorization;" + Environment.NewLine);
            sb.Append("using Microsoft.AspNetCore.Http;" + Environment.NewLine);
            sb.Append("using Microsoft.AspNetCore.Mvc;" + Environment.NewLine);
            sb.Append("using HZC.Core;" + Environment.NewLine);
            sb.Append("using " + ns + ".Services;" + Environment.NewLine);
            sb.Append("using " + ns + ".Website.Extensions;" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            if (string.IsNullOrWhiteSpace(areaName))
            {
                sb.Append("namespace " + ns + ".Website.Controllers" + Environment.NewLine);
            }
            else
            {
                sb.Append("namespace " + ns + ".WebSite.Areas." + areaName + ".Controllers" + Environment.NewLine);
            }
            sb.Append("{" + Environment.NewLine);
            sb.Append("    [Authorize]" + Environment.NewLine);
            sb.Append("    public class " + className +  "Controller : BaseController" + Environment.NewLine);
            sb.Append("    {" + Environment.NewLine);
            sb.Append("        private readonly " + className +  "Service service = new " + className +  "Service();" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("        public " + className +  "Controller()" + Environment.NewLine);
            sb.Append("        { }" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("        #region 列表页" + Environment.NewLine);
            sb.Append("        public ActionResult Index()" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            InitUI();" + Environment.NewLine);
            sb.Append("            return View();" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("        public JsonResult Get(" + className + "SearchParam param, int pageIndex = 1, int pageSize = 20)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            var list = service.Query(param.ToSearchUtil(), pageIndex, pageSize);" + Environment.NewLine);
            sb.Append("            return Json(ResultUtil.PageList<" + className + "Entity>(list));" + Environment.NewLine);

            sb.Append(Environment.NewLine);

            sb.Append("            // var list = service.Fetch(param.ToSearchUtil());" + Environment.NewLine);
            sb.Append("            // return Json(ResultUtil.Success<IEnumerable<" + className + "Entity>>(list));" + Environment.NewLine);

            sb.Append("        }" + Environment.NewLine);
            sb.Append("        #endregion" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("        #region 创建" + Environment.NewLine);
            sb.Append("        public IActionResult Create()" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            InitUI();" + Environment.NewLine);
            sb.Append("            return View();" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("        [HttpPost]" + Environment.NewLine);
            sb.Append("        [ValidateAntiForgeryToken]" + Environment.NewLine);
            sb.Append("        public JsonResult Create(IFormCollection collection)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            " + className + "Entity entity = new " + className + "Entity();" + Environment.NewLine);
            sb.Append("            TryUpdateModelAsync(entity);" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("            var result = service.Create(entity, AppUser);" + Environment.NewLine);
            sb.Append("            // 如果有缓存，注意在这里要清空缓存" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("            return Json(result);" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append("        #endregion" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("        #region 修改" + Environment.NewLine);
            sb.Append("        public IActionResult Edit(int id)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            var entity = service.Load(id);" + Environment.NewLine);
            sb.Append("            if (entity == null)" + Environment.NewLine);
            sb.Append("            {" + Environment.NewLine);
            sb.Append("                return Empty();" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append("            else" + Environment.NewLine);
            sb.Append("            {" + Environment.NewLine);
            sb.Append("                InitUI();" + Environment.NewLine);
            sb.Append("                return View(entity);" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("        [HttpPost]" + Environment.NewLine);
            sb.Append("        [ValidateAntiForgeryToken]" + Environment.NewLine);
            sb.Append("        public JsonResult Edit(int id, IFormCollection collection)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            " + className + "Entity entity = new " + className + "Entity();" + Environment.NewLine);
            sb.Append("            TryUpdateModelAsync(entity);" + Environment.NewLine);
            sb.Append("            var result = service.Update(entity, AppUser);" + Environment.NewLine);
            sb.Append("            // 如果有缓存，注意在这里要清空缓存" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("            return Json(result);" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append("        #endregion" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            
            sb.Append("        #region 删除" + Environment.NewLine);
            sb.Append("        [HttpPost]" + Environment.NewLine);
            sb.Append("        [ValidateAntiForgeryToken]" + Environment.NewLine);
            sb.Append("        public JsonResult Delete(int id)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            var entity = service.Load(id);" + Environment.NewLine);
            sb.Append("            if (entity == null)" + Environment.NewLine);
            sb.Append("            {" + Environment.NewLine);
            sb.Append("                return Json(ResultUtil.AuthFail(\"请求的数据不存在\"));" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append("            var result =  service.Remove(entity, AppUser);" + Environment.NewLine);
            sb.Append("            // 如果有缓存，注意在这里要清空缓存" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("            return Json(result);" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append("        #endregion" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("        #region 辅助方法" + Environment.NewLine);
            sb.Append("        private void InitUI()" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append("        #endregion" + Environment.NewLine);
            sb.Append("    }" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);

            return sb.ToString();
        }

        public static void SaveToFile(DbTable table, string ns, string areaName)
        {
            string content = Get(table, ns, areaName);
        }
    }
}
