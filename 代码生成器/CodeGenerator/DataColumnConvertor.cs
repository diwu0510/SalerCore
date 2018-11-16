using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DataColumnConvertor
    {
        // 转换为C#变量类型
        public static string ToCSharpType(DbTableColumn dr)
        {
            bool isNullable = dr.isNullAble == "1" ? true : false;

            switch (dr.type)
            {
                case "int":
                    return isNullable ? "int?" : "int";
                case "bigint":
                    return isNullable ? "long?" : "long";
                case "decimal":
                    return isNullable ? "decimal?" : "decimal";
                case "varchar":
                    return "string";
                case "char":
                    return "string";
                case "nvarchar":
                    return "string";
                case "datetime":
                    return isNullable ? "DateTime?" : "DateTime";
                case "bit":
                    return isNullable ? "bool?" : "bool";
                default:
                    throw new Exception("Type not known:" + dr.type);
            }
        }

        public static string ToCSharpType(string type, bool isNullable = false)
        {
            switch (type)
            {
                case "int":
                    return isNullable ? "int?" : "int";
                case "bigint":
                    return isNullable ? "long?" : "long";
                case "decimal":
                    return isNullable ? "decimal?" : "decimal";
                case "varchar":
                    return "string";
                case "char":
                    return "string";
                case "nvarchar":
                    return "string";
                case "datetime":
                    return isNullable ? "DateTime?" : "DateTime";
                case "bit":
                    return isNullable ? "bool?" : "bool";
                default:
                    throw new Exception("Type not known:" + type);
            }
        }

        // 转换为SqlDbType
        public static string ToSqlDbType(DbTableColumn dr)
        {
            switch (dr.type)
            {
                case "tinyint":
                    return " SqlDbType.TinyInt";
                case "int":
                    return " SqlDbType.Int";
                case "bigint":
                    return " SqlDbType.BigInt";
                case "char":
                    return " SqlDbType.Char";
                case "varchar":
                    return " SqlDbType.VarChar";
                case "nvarchar":
                    return " SqlDbType.NVarChar";
                case "datetime":
                    return " SqlDbType.DateTime";
                case "bit":
                    return " SqlDbType.Bit";
                case "decimal":
                    return " SqlDbType.Decimal";
                default:
                    throw new Exception("Type not known " + dr.type);
            }
        }

        public static string ToProcType(DbTableColumn dr)
        {
            switch (dr.type)
            {
                case "varchar":
                    return "varchar(" + ((dr.size == 2147483647 || dr.size == -1) ? "max" : dr.size.ToString()) + ")";
                case "nvarchar":
                    return "nvarchar(" + ((dr.size == 2147483647 || dr.size == -1) ? "max" : dr.size.ToString()) + ")";
                default:
                    return dr.type;
            }
        }
    }
}
