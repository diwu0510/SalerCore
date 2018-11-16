using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DataTableToListView
    {
        public static string Get(DbTable table, List<string> searchCols, List<string> listCols, string ns, string areaName)
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

            sb.Append("@{" + Environment.NewLine);
            sb.Append("    ViewData[\"Title\"] = \"" + className + "管理\";" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("<div class=\"container-fluid full-height\">" + Environment.NewLine);
            sb.Append("    <div id=\"container-header\">" + Environment.NewLine);
            sb.Append("        <form id=\"form\" onsubmit=\"return false;\">" + Environment.NewLine);
            sb.Append("            <div class=\"toolbar row\">" + Environment.NewLine);
            sb.Append("                <div class=\"col-6\">" + Environment.NewLine);
            sb.Append("                    <a id=\"btn-create\""+ Environment.NewLine);
            sb.Append("                       href=\"javascript:;\"" + Environment.NewLine);
            sb.Append("                       data-action=\"" + (!string.IsNullOrWhiteSpace(areaName) ? "/" + areaName : "") + "/" + className + "/Create\"" + Environment.NewLine);
            sb.Append("                       data-type=\"pop\"" + Environment.NewLine);
            sb.Append("                       data-options='{\"title\": \"创建\", \"width\": 720, \"height\": 400, \"before\": null}'" + Environment.NewLine);
            sb.Append("                       class=\"btn btn-green btn-customer\"><i class=\"fa fa-plus-square\"></i> 添加</a>" + Environment.NewLine);

            sb.Append("                    <a id=\"btn-edit\"" + Environment.NewLine);
            sb.Append("                       href=\"javascript:;\"" + Environment.NewLine);
            sb.Append("                       data-action=\"" + (!string.IsNullOrWhiteSpace(areaName) ? "/" + areaName : "") + "/" + className + "/Edit\"" + Environment.NewLine);
            sb.Append("                       data-type=\"pop\"" + Environment.NewLine);
            sb.Append("                       data-grid=\"grid\"" + Environment.NewLine);
            sb.Append("                       data-options='{\"title\": \"修改\", \"width\": 720, \"height\": 400, \"before\": null}'" + Environment.NewLine);
            sb.Append("                       class=\"btn btn-blue btn-customer\"><i class=\"fa fa-pencil-square\"></i> 修改</a>" + Environment.NewLine);

            sb.Append("                    <a id=\"btn-delete\"" + Environment.NewLine);
            sb.Append("                       href=\"javascript:;\"" + Environment.NewLine);
            sb.Append("                       data-action=\"" + (!string.IsNullOrWhiteSpace(areaName) ? "/" + areaName : "") + "/" + className + "/Delete\"" + Environment.NewLine);
            sb.Append("                       data-type=\"ajax\"" + Environment.NewLine);
            sb.Append("                       data-grid=\"grid\"" + Environment.NewLine);
            sb.Append("                       data-options='{\"msg\": \"确认操作？\", \"before\": null}'" + Environment.NewLine);
            sb.Append("                       class=\"btn btn-red btn-customer\"><i class=\"fa fa-trash-o\"></i> 删除</a>" + Environment.NewLine);
            
            sb.Append("                    <a id=\"btn-refresh\" href=\"javascript:;\" class=\"btn btn-default\" onclick=\"window.grid.reload();\"><i class=\"fa fa-refresh\"></i> 刷新</a>" + Environment.NewLine);
            sb.Append("                </div>" + Environment.NewLine);
            sb.Append("                <div class=\"col-6 content-right\">" + Environment.NewLine);
            sb.Append("                    @Html.AntiForgeryToken()" + Environment.NewLine);
            sb.Append("                    <input name=\"Key\" type=\"search\" value=\"\" placeholder=\"关键字\" />" + Environment.NewLine);
            sb.Append("                    <input type=\"button\" class=\"btn btn-blue\" value=\"搜索\" onclick=\"window.grid.reload()\">" + Environment.NewLine);
            sb.Append("                    <a href=\"javascript:;\" class=\"btn btn-default search-toggle-button\"><i class=\"fa fa-filter\"></i> 高级筛选</a>" + Environment.NewLine);
            sb.Append("                </div>" + Environment.NewLine);
            sb.Append("            </div>" + Environment.NewLine);

            sb.Append("            <div id=\"search-box\" class=\"search-box\">" + Environment.NewLine);
            sb.Append("                <div class=\"caret-h\"></div>" + Environment.NewLine);
            sb.Append("                <div class=\"search-box-body form-box\">" + Environment.NewLine);
            sb.Append("                    <div class=\"row\">" + Environment.NewLine);

            for (int i = 0; i < total; i++)
            {
                var dr = table.columns[i];
                if (searchCols.Contains(dr.name))
                {
                    sb.Append("                        <div class=\"col-4 form-box-cell\">" + Environment.NewLine);
                    sb.Append("                            <div class=\"label\">" + dr.description + "</div>" + Environment.NewLine);
                    if (dr.name.EndsWith("Id"))
                    {
                        sb.Append("                            <div class=\"control\">" + Environment.NewLine);
                        sb.Append("                                <select name=\"" + dr.name + "\" asp-items=\"ViewBag." + dr.name.Replace("Id", "s") + "\">" + Environment.NewLine);
                        sb.Append("                                    <option value=\"\">" + (string.IsNullOrWhiteSpace(dr.description) ? dr.name : dr.description) + "</option>" + Environment.NewLine);
                        sb.Append("                                </select>" + Environment.NewLine);
                        sb.Append("                            </div>" + Environment.NewLine);
                    }
                    else
                    {
                        if (dr.type == "datetime")
                        {
                            sb.Append("                            <div class=\"control\">" + Environment.NewLine);
                            sb.Append("                                <div class=\"col-6\">" + Environment.NewLine);
                            sb.Append("                                    <input type=\"date\" name=\"" + dr.name + "Start\" />" + Environment.NewLine);
                            sb.Append("                                </div>" + Environment.NewLine);
                            sb.Append("                                <div class=\"col-6\">" + Environment.NewLine);
                            sb.Append("                                    <input type=\"date\" name=\"" + dr.name + "End\" />" + Environment.NewLine);
                            sb.Append("                                </div>" + Environment.NewLine);
                            sb.Append("                            </div>" + Environment.NewLine);
                        }
                        else
                        {
                            sb.Append("                            <div class=\"control\"><input type=\"text\" name=\"" + dr.name + "\" /></div>" + Environment.NewLine);
                        }
                    }
                    sb.Append("                        </div>" + Environment.NewLine);
                }
            }
            
            sb.Append("                    </div>" + Environment.NewLine);
            sb.Append("                </div>" + Environment.NewLine);
            sb.Append("            </div>" + Environment.NewLine);
            sb.Append("        </form>" + Environment.NewLine);
            sb.Append("    </div>" + Environment.NewLine);
            sb.Append("    <div id=\"list\" class=\"grid-box\"></div>" + Environment.NewLine);
            sb.Append("</div>" + Environment.NewLine);

            sb.Append(Environment.NewLine);

            sb.Append("@section scripts" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("    <script src=\"~/lib/myui/myGridNoFixed.v2.js\"></script>" + Environment.NewLine);
            sb.Append("    <script>" + Environment.NewLine);
            sb.Append("        var grid = $('#list').MyGrid({" + Environment.NewLine);
            sb.Append("            api: '" + (!string.IsNullOrWhiteSpace(areaName) ? "/" + areaName : "") + "/" + className + "/Get'," + Environment.NewLine);
            sb.Append("            columns: [" + Environment.NewLine);
            sb.Append("                { title: '编号', type: 'indexNum', width: 40 }," + Environment.NewLine);

            for (int j = 0; j < total; j++)
            {
                var dr = table.columns[j];
                if (listCols.Contains(dr.name))
                {
                    sb.Append("                { title: '" + dr.description + "', field: '" + (dr.name.First().ToString().ToLower() + dr.name.Substring(1, dr.name.Length - 1)) + "', width: 120 }," + Environment.NewLine);
                }
            }

            sb.Append("            ]," + Environment.NewLine);
            sb.Append("            keyColumn: 'id'," + Environment.NewLine);
            sb.Append("            height: function() {" + Environment.NewLine);
            sb.Append("                var h = $(window).height() - $('#container-header').height() - 40;" + Environment.NewLine);
            sb.Append("                return h;" + Environment.NewLine);
            sb.Append("            }," + Environment.NewLine);
            sb.Append("            pageSize: 20," + Environment.NewLine);
            sb.Append("            dataConvertFn: function(source) { return source; }," + Environment.NewLine);
            sb.Append("            renderCompleteFn: null," + Environment.NewLine);
            sb.Append("            filterFn: function() { return $('#form').serialize(); }," + Environment.NewLine);
            sb.Append("            ajaxErrorFn: null," + Environment.NewLine);
            sb.Append("            ajaxBeforeSendFn: null," + Environment.NewLine);
            sb.Append("            click: function(item) { }," + Environment.NewLine);
            sb.Append("            dblClick: function(item) { $('#btn-edit').trigger('click'); }," + Environment.NewLine);
            sb.Append("            multi: false," + Environment.NewLine);
            sb.Append("            pager: true," + Environment.NewLine);
            sb.Append("            auto: true" + Environment.NewLine);
            sb.Append("        });" + Environment.NewLine);
            sb.Append("    </script>" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}
