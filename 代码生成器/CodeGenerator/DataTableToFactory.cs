using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DataTableToFactory
    {
        public static string Get(DbTable table, string ns)
        {
            StringBuilder sb = new StringBuilder();
            int total = table.columns.Count;

            // 命名空间
            sb.Append("using System;" + Environment.NewLine);
            sb.Append("using System.Collections.Generic;" + Environment.NewLine);
            sb.Append("using System.Configuration;" + Environment.NewLine);
            sb.Append("using System.Data;" + Environment.NewLine);
            sb.Append("using System.Data.SqlClient;" + Environment.NewLine);
            sb.Append("using " + ns + ".Core;" + Environment.NewLine);

            // 开始定义类
            sb.Append("namespace " + ns + ".Services" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("    public partial class " + table.name + "Factory : BaseFactory<" + table.name + ">" + Environment.NewLine);
            sb.Append("    {" + Environment.NewLine);
            sb.Append("        #region 构造函数" + Environment.NewLine);
            sb.Append("        public " + table.name + "Factory() : base() { }" + Environment.NewLine);
            sb.Append("        public " + table.name + "Factory(string connSettingKeyName) : base(connSettingKeyName) { }" + Environment.NewLine);
            sb.Append("        #endregion" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("        #region 增改数据" + Environment.NewLine);
            sb.Append("        public override int Create(" + table.name + " model, int operatorId)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            try" + Environment.NewLine);
            sb.Append("            {" + Environment.NewLine);
            sb.Append("                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings[_connectionString]))" + Environment.NewLine);
            sb.Append("                {" + Environment.NewLine);
            sb.Append("                    SqlParameter[] param = {" + Environment.NewLine);

            for (int i = 0; i < total; i++)
            {
                var dr = table.columns[i];
                if (dr.name != "IsDel" && dr.name != "CreateAt" && dr.name != "UpdateAt" && dr.name != "UpdateBy")
                {
                    sb.Append("                        new SqlParameter(\"@" + dr.name + "\", " + DataColumnConvertor.ToSqlDbType(dr) + ")," + Environment.NewLine);
                }
            }

            sb.Append("                    };" + Environment.NewLine);

            sb.Append(Environment.NewLine);

            sb.Append("                    param[0].Direction = ParameterDirection.Output;" + Environment.NewLine);
            var j = 0;
            for (int i = 0; i < total; i++)
            {
                var dr = table.columns[i];
                if (dr.name != "IsDel" && dr.name != "CreateAt" && dr.name != "UpdateAt" && dr.name != "UpdateBy" && dr.name != table.name + "Id")
                {
                    if (dr.name == "CreateBy")
                    {
                        sb.Append("                    param[" + (j + 1) + "].Value = operatorId;" + Environment.NewLine);
                    }
                    else
                    {
                        sb.Append("                    param[" + (j + 1) + "].Value = model." + dr.name + ";" + Environment.NewLine);
                    }
                    j++;
                }
            }
            sb.Append("                    int obj = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, \"" + table.name + "_Add\", param);" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("                    return obj == 0 ? 0 : Convert.ToInt32(param[0].Value);" + Environment.NewLine);
            sb.Append("                }" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append("            catch(Exception ex)" + Environment.NewLine);
            sb.Append("            {" + Environment.NewLine);
            sb.Append("                DbExceptionHelper.DoException(\"" + table.name + "Factory.Create\", ex);" + Environment.NewLine);
            sb.Append("                return 0;" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("        public override int Update(" + table.name + " model, int operatorId)" + Environment.NewLine);
            sb.Append("        {" + Environment.NewLine);
            sb.Append("            try" + Environment.NewLine);
            sb.Append("            {" + Environment.NewLine);
            sb.Append("                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings[_connectionString]))" + Environment.NewLine);
            sb.Append("                {" + Environment.NewLine);
            sb.Append("                    SqlParameter[] param = {" + Environment.NewLine);


            for (int i = 0; i < total; i++)
            {
                var dr = table.columns[i];
                if (dr.name != "IsDel" && dr.name != "CreateAt" && dr.name != "UpdateAt" && dr.name != "CreateBy")
                {
                    sb.Append("                        new SqlParameter(\"@" + dr.name + "\", " + DataColumnConvertor.ToSqlDbType(dr) + ")," + Environment.NewLine);
                }
            }

            sb.Append("                    };" + Environment.NewLine);

            sb.Append(Environment.NewLine);
            j = 0;
            for (int i = 0; i < total; i++)
            {
                var dr = table.columns[i];
                if (dr.name != "IsDel" && dr.name != "CreateAt" && dr.name != "UpdateAt" && dr.name != "CreateBy")
                {
                    if (dr.name == "UpdateBy")
                    {
                        sb.Append("                    param[" + j + "].Value = operatorId;" + Environment.NewLine);
                    }
                    else
                    {
                        sb.Append("                    param[" + j + "].Value = model." + dr.name + ";" + Environment.NewLine);
                    }
                    j++;
                }
            }
            sb.Append(Environment.NewLine);
            sb.Append("                    return SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, \"" + table.name + "_Update\", param);" + Environment.NewLine);
            sb.Append("                }" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append("            catch(Exception ex)" + Environment.NewLine);
            sb.Append("            {" + Environment.NewLine);
            sb.Append("                DbExceptionHelper.DoException(\"" + table.name + "Factory.Update\", ex);" + Environment.NewLine);
            sb.Append("                return 0;" + Environment.NewLine);
            sb.Append("            }" + Environment.NewLine);
            sb.Append("        }" + Environment.NewLine);
            sb.Append("        #endregion" + Environment.NewLine);
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
