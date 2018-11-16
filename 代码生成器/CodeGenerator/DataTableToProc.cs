using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DataTableToProc
    {
        public static string _namespace = "Migrant";

        public static string Get(DbTable table)
        {
            StringBuilder sb = new StringBuilder();

            List<string> addExclude = new List<string> { "CreateAt", "UpdateAt", "UpdateBy", table.name + "Id", "IsDel" };
            List<DbTableColumn> addColumns = table.columns.Where(c => !addExclude.Contains(c.name)).ToList();

            List<string> editExclude = new List<string> { "CreateAt", "CreateBy", "UpdateAt", "IsDel" };
            List<DbTableColumn> editColumns = table.columns.Where(c => !editExclude.Contains(c.name)).ToList();

            // 创建
            sb.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[" + table.name + "_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)" + Environment.NewLine);
            sb.Append("drop procedure [" + table.name + "_Add]" + Environment.NewLine);
            sb.Append("GO" + Environment.NewLine);
            sb.Append("--------------------------------------------------" + Environment.NewLine);
            sb.Append("--用途：增加一条记录 " + Environment.NewLine);
            sb.Append("--项目名称：" + _namespace + Environment.NewLine);
            sb.Append("--说明：自动生成" + Environment.NewLine);
            sb.Append("--时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine);
            sb.Append("--------------------------------------------------" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("CREATE PROCEDURE [dbo].[" + table.name + "_Add]" + Environment.NewLine);
            sb.Append("    @" + table.name + "Id int output," + Environment.NewLine);
            
            for (int i = 0; i < addColumns.Count; i++)
            {
                var dr = addColumns[i];
                sb.Append("    @" + dr.name + " " + DataColumnConvertor.ToProcType(dr) + (i == (addColumns.Count - 1) ? "" : ",") + Environment.NewLine);
            }

            sb.Append(Environment.NewLine);
            sb.Append("AS" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("INSERT INTO [" + table.name + "] ( ");

            for (int i = 0; i < addColumns.Count; i++)
            {
                var dr = addColumns[i];
                sb.Append("[" + dr.name + "],");
            }

            sb.Append("IsDel,CreateAt,UpdateBy,UpdateAt) " + Environment.NewLine);
            sb.Append("    VALUES ( ");
            for (int i = 0; i < addColumns.Count; i++)
            {
                var dr = addColumns[i];
                sb.Append("@" + dr.name + ",");
            }
            sb.Append("0,GETDATE(),@CreateBy,GETDATE() );" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("SET @" + table.name + "Id = @@IDENTITY;" + Environment.NewLine);
            sb.Append("GO" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            // 修改
            sb.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[" + table.name + "_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)" + Environment.NewLine);
            sb.Append("drop procedure [" + table.name + "_Add]" + Environment.NewLine);
            sb.Append("GO" + Environment.NewLine);
            sb.Append("--------------------------------------------------" + Environment.NewLine);
            sb.Append("--用途：修改一条记录 " + Environment.NewLine);
            sb.Append("--项目名称：" + _namespace + Environment.NewLine);
            sb.Append("--说明：自动生成" + Environment.NewLine);
            sb.Append("--时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine);
            sb.Append("--------------------------------------------------" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("CREATE PROCEDURE [dbo].[" + table.name + "_Update]" + Environment.NewLine);

            for (int i = 0; i < editColumns.Count; i++)
            {
                var dr = editColumns[i];
                sb.Append("    @" + dr.name + " " + DataColumnConvertor.ToProcType(dr) + (i == (editColumns.Count - 1) ? "" : ",") + Environment.NewLine);
            }

            sb.Append(Environment.NewLine);
            sb.Append("AS" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("UPDATE [" + table.name + "] SET ");

            for (int i = 0; i < editColumns.Count; i++)
            {
                var dr = editColumns[i];
                if (dr.name != table.name + "Id")
                {
                    sb.Append("[" + dr.name + "]=@" + dr.name + ",");
                }
            }

            sb.Append("UpdateAt=GETDATE() WHERE " + table.name + "Id = @" + table.name + "Id;" + Environment.NewLine);
            sb.Append("GO" + Environment.NewLine);

            return sb.ToString();
        }
    }
}
