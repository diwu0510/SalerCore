using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DataTableToSearchParam
    {
        public static string Get(DbTable table, string namespaceName, List<string> searchParams = null)
        {
            StringBuilder sb = new StringBuilder();
            List<string> exclude = new List<string> { "IsDel", "CreateAt", "CreateBy", "Creator", "UpdateAt", "UpdateBy", "Updator" };

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
            sb.Append("using HZC.Common.Services;" + Environment.NewLine);
            sb.Append("using HZC.Database;" + Environment.NewLine);
            sb.Append("using System;" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("namespace " + namespaceName + ".Services" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            
            sb.Append("    public partial class " + className + "SearchParam : ISearchParam" + Environment.NewLine);
            sb.Append("    {" + Environment.NewLine);
            sb.Append("        public string Key { get; set; }" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            if (searchParams != null && searchParams.Count > 0)
            {
                foreach (var param in searchParams)
                {
                    if (param.EndsWith("At"))
                    {
                        sb.Append("        public DateTime? " + param + "Start { get; set; }" + Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        sb.Append("        public DateTime? " + param + "End { get; set; }" + Environment.NewLine);
                    }
                    else if (param.StartsWith("Is"))
                    {
                        sb.Append("        public bool? " + param + " { get; set; }" + Environment.NewLine);
                    }
                    else if (param.EndsWith("Id"))
                    {
                        sb.Append("        public int? " + param + " { get; set; }" + Environment.NewLine);
                    }
                    else
                    {
                        sb.Append("        public string " + param + " { get; set; }" + Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            sb.Append("        public MySearchUtil ToSearchUtil()" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            MySearchUtil util = MySearchUtil.New().OrderByDesc(\"Id\");" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("            if (!string.IsNullOrWhiteSpace(Key))" + Environment.NewLine);
            sb.Append("            {" + Environment.NewLine);
            sb.Append("                // util.AndContains(new string[] { \"Title\", \"Name\" }, Key.Trim());" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            if (searchParams != null && searchParams.Count > 0)
            {
                foreach (var param in searchParams)
                {
                    if (param.EndsWith("At"))
                    {
                        sb.Append("            if (" + param + "Start.HasValue)" + Environment.NewLine);
                        sb.Append("            {" + Environment.NewLine);
                        sb.Append("                util.AndGreaterThanEqual(\"" + param + "\", " + param + ".Value);" + Environment.NewLine);
                        sb.Append("            }" + Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        sb.Append("            if (" + param + "Start.HasValue)" + Environment.NewLine);
                        sb.Append("            {" + Environment.NewLine);
                        sb.Append("                util.AndGreaterThanEqual(\"" + param + "\", " + param + ".Value);" + Environment.NewLine);
                        sb.Append("            }" + Environment.NewLine);
                    }
                    else if (param.StartsWith("Is"))
                    {
                        sb.Append("            if (" + param + ".HasValue)" + Environment.NewLine);
                        sb.Append("            {" + Environment.NewLine);
                        sb.Append("                util.AndEqual(\"" + param + "\", " + param + ".Value);" + Environment.NewLine);
                        sb.Append("            }" + Environment.NewLine);
                    }
                    else if (param.EndsWith("Id"))
                    {
                        sb.Append("            if (" + param + ".HasValue)" + Environment.NewLine);
                        sb.Append("            {" + Environment.NewLine);
                        sb.Append("                util.AndEqual(\"" + param + "\", " + param + ".Value);" + Environment.NewLine);
                        sb.Append("            }" + Environment.NewLine);
                    }
                    else
                    {
                        sb.Append("            if (!string.IsNullOrWhiteSpace(" + param + "))" + Environment.NewLine);
                        sb.Append("            {" + Environment.NewLine);
                        sb.Append("                util.AndContains(\"" + param + "\", " + param + ".Trim());" + Environment.NewLine);
                        sb.Append("            }" + Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            sb.Append("            return util;" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("        protected string GetFullColumnName(string columnName)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            return \"" + table.name + ".\" + columnName;" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append("    }" + Environment.NewLine);

            sb.Append("}" + Environment.NewLine);
            return sb.ToString();
        }

        public static void SaveToFile(DbTable table, string ns)
        {
            string content = Get(table, ns);
        }
    }
}
