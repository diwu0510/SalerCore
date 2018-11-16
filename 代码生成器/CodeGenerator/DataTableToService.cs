using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DataTableToService
    {
        public static string Get(DbTable table, string ns)
        {
            StringBuilder sb = new StringBuilder();
            List<string> exclude = new List<string> { "IsDel", "CreateAt", "CreateBy", "UpdateAt", "UpdateBy" };

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
            sb.Append("using HZC.Core;" + Environment.NewLine);
            sb.Append("using HZC.Database;" + Environment.NewLine);
            sb.Append("using System;" + Environment.NewLine);
            sb.Append("using System.Data;" + Environment.NewLine);
            sb.Append("using System.Collections.Generic;" + Environment.NewLine);
            sb.Append("using System.Text;" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("namespace " + ns + ".Services" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("    public partial class " + className + "Service : BaseService<" + className + "Entity>" + Environment.NewLine);
            sb.Append("    {" + Environment.NewLine);
            sb.Append("        #region 重写实体验证" + Environment.NewLine);
            sb.Append("        protected override string ValidateCreate(" + className + "Entity t, IAppUser user)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            return string.Empty;" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);

            sb.Append(Environment.NewLine);

            sb.Append("        protected override string ValidateUpdate(" + className + "Entity t, IAppUser user)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            return string.Empty;" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);

            sb.Append(Environment.NewLine);

            sb.Append("        protected override string ValidateDelete(" + className + "Entity t, IAppUser user)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            return string.Empty;" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append("        #endregion" + Environment.NewLine);

            sb.Append("    }" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        public static void SaveToFile(DbTable table, string ns)
        {
            string content = Get(table, ns);
        }
    }
}
