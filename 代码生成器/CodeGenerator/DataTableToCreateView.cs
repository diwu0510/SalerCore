using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DataTableToCreateView
    {
        public static string Get(DbTable table, string ns, string an)
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
            sb.Append("@model " + ns + ".Services." + className + "Entity" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("@{" + Environment.NewLine);
            sb.Append("    ViewData[\"Title\"] = \"" + className + "创建\";" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("<form asp-action=\"Create\">" + Environment.NewLine);    
            sb.Append("    <h1 class=\"form-box-title\">@ViewData[\"Title\"]</h1>" + Environment.NewLine);
            sb.Append("    <hr />" + Environment.NewLine);
            sb.Append("    <div class=\"form-box-fixed-container\">" + Environment.NewLine);
            sb.Append("        <div class=\"form-box\">" + Environment.NewLine);
            sb.Append("            <div class=\"row\">" + Environment.NewLine);

            List<string> cols = new List<string> { "Id", "IsDel", "CreateBy", "CreateAt", "Creator", "UpdateAt", "UpdateBy", "Updator" };
            for (int i = 0; i < total; i++)
            {
                var dr = table.columns[i];
                if (!cols.Contains(dr.name))
                {
                    sb.Append("                <div class=\"col-6\">" + Environment.NewLine);
                    sb.Append("                    <div class=\"label\">" + (string.IsNullOrWhiteSpace(dr.description) ? dr.name : dr.description) + "</div>" + Environment.NewLine);
                    sb.Append("                    <div class=\"control\">" + Environment.NewLine);
                    if (dr.name.EndsWith("Id"))
                    {
                        sb.Append("                        <select asp-for=\"" + dr.name + "\" asp-items=\"ViewBag." + dr.name.Replace("Id", "s") + "\" isvalid=\"yes\" checkexpession=\"NotNull\">" + Environment.NewLine);
                        sb.Append("                            <option value=\"\">请选择</option>" + Environment.NewLine);
                        sb.Append("                        </select>" + Environment.NewLine);
                    }
                    else
                    {
                        if (dr.type == "int" || dr.type == "bigint" || dr.type == "double" || dr.type == "decimal")
                        {
                            sb.Append("                        <input type=\"number\" asp-for=\"" + dr.name + "\" isvalid=\"yes\" checkexpession=\"Number\" />" + Environment.NewLine);
                        }
                        else if (dr.type == "datetime")
                        {
                            sb.Append("                        <input type=\"date\" asp-for=\"" + dr.name + "\" isvalid=\"yes\" checkexpession=\"Date\" />" + Environment.NewLine);
                        }
                        else
                        {
                            sb.Append("                        <input type=\"text\" asp-for=\"" + dr.name + "\" isvalid=\"yes\" checkexpession=\"NotNull\" />" + Environment.NewLine);
                        }
                    }
                    sb.Append("                    </div>" + Environment.NewLine);
                    sb.Append("                </div>" + Environment.NewLine);
                }
            }

            sb.Append("            </div>" + Environment.NewLine);
            sb.Append("        </div>" + Environment.NewLine);
            sb.Append("    </div>" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("    <div class=\"form-box-fixed-foot row\">" + Environment.NewLine);
            sb.Append("        <div class=\"col-6\">" + Environment.NewLine);
            sb.Append("            <label>&nbsp;</label>" + Environment.NewLine);
            sb.Append("        </div>" + Environment.NewLine);
            sb.Append("        <div class=\"col-6 content-right\">" + Environment.NewLine);
            sb.Append("            <input type=\"submit\" value=\"提交\" class=\"btn btn-blue\" />" + Environment.NewLine);
            sb.Append("            <input id=\"btn-closeSelf\" type=\"button\" value=\"关闭\" class=\"btn btn-red btn-close\" />" + Environment.NewLine);
            sb.Append("        </div>" + Environment.NewLine);
            sb.Append("    </div>" + Environment.NewLine);
            sb.Append("</form>" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("@section scripts {" + Environment.NewLine);
            sb.Append("    <script src=\"~/lib/validator/validator.js\"></script>" + Environment.NewLine);
            sb.Append("    <script>" + Environment.NewLine);
            sb.Append("        myUI.initForm({pageType: \"pop\"});" + Environment.NewLine);
            sb.Append("    </script>" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);

            return sb.ToString();
        }

        public static void SaveToFile(DbTable table, string ns, string an)
        {
            string content = Get(table, ns, an);
        }
    }
}
