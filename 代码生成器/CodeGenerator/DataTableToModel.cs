using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator
{
    public class DataTableToModel
    {
        public static string Get(DbTable table, string namespaceName)
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
            sb.Append("using HZC.Core;" + Environment.NewLine);
            sb.Append("using HZC.Database;" + Environment.NewLine);
            sb.Append("using System;" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("namespace " + namespaceName + ".Services" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            
            sb.Append("    [MyDataTable(\"" + table.name + "\")]" + Environment.NewLine);
            sb.Append("    public partial class " + className + "Entity : BaseEntity" + Environment.NewLine);
            sb.Append("    {" + Environment.NewLine);
            string[] ignoreColumns = new string[] { "Id", "CreateAt", "CreateBy", "Creator", "UpdateAt", "UpdateBy", "Updator" };

            for (int i = 0; i < table.columns.Count; i++)
            {
                var dr = table.columns[i];
                if (!ignoreColumns.Contains(dr.name))
                {
                    sb.Append("        /// <summary>" + Environment.NewLine);
                    sb.Append("        /// " + (string.IsNullOrWhiteSpace(dr.description) ? dr.name : dr.description) + Environment.NewLine);
                    sb.Append("        /// </summary>" + Environment.NewLine);
                    sb.Append("        public " + DataColumnConvertor.ToCSharpType(dr) + " " + dr.name + " { get; set; }" + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                }
            }
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
