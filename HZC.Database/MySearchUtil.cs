using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HZC.Database
{
    public class MySearchUtil
    {
        #region 配置字段
        private string _sqlParameterPrefix = "@";                                   // SqlParameter的前缀
        private string _tableName = "";                                             // 表名
        private int _idx = 0;                                                       // 参数索引
        #endregion

        #region 条件子句和查询参数字段
        private string _conditonClauses = "";                                       // 条件子句
        private DynamicParameters _params = new DynamicParameters();                // Dapper的查询参数
        private List<string> _cols = new List<string>();
        private List<string> _orderby = new List<string>();                         // 排序字段
        #endregion

        #region 构造函数
        public MySearchUtil()
        { }

        public MySearchUtil(string prefix)
        {
            _sqlParameterPrefix = prefix;
        }

        public MySearchUtil(string prefix, string tableName = "")
        {
            _sqlParameterPrefix = prefix;
            _tableName = tableName;
        }
        #endregion

        #region 属性-查询语句和结果
        /// <summary>
        /// 条件子句
        /// </summary>
        public string ConditionClaus
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_conditonClauses))
                {
                    return _conditonClauses;
                }
                else
                {
                    return "1=1";
                }
            }
        }

        /// <summary>
        /// 查询参数
        /// </summary>
        public DynamicParameters Parameters
        {
            get
            {
                return _params;
            }
        }

        /// <summary>
        /// 分页查询参数
        /// </summary>
        public DynamicParameters PageListParameters
        {
            get
            {
                _params.Add("RecordCount", dbType: System.Data.DbType.Int32, direction: ParameterDirection.Output);
                return _params;
            }
        }

        /// <summary>
        /// 排序子句
        /// </summary>
        public string OrderByClaus
        {
            get
            {
                if (_orderby.Count == 0)
                {
                    return "";
                }
                else
                {
                    return string.Join(",", _orderby);
                }
            }
        }

        /// <summary>
        /// 查询的列的子句
        /// </summary>
        public string Columns
        {
            get
            {
                if (_cols.Count == 0)
                {
                    return "";
                }
                return string.Join(",", _cols);
            }
        }
        #endregion

        #region 返回一个实例
        /// <summary>
        /// 创建一个SearchUtil实例
        /// </summary>
        /// <returns></returns>
        public static MySearchUtil New()
        {
            return new MySearchUtil();
        }

        /// <summary>
        /// 创建一个SearchUtil实例
        /// </summary>
        /// <param name="prefix">参数的前缀，sqlserver: "@"，mysql: "$"，oracle: ":"</param>
        /// <returns></returns>
        public static MySearchUtil New(string prefix)
        {
            return new MySearchUtil(prefix);
        }

        /// <summary>
        /// 创建一个SearchUtil实例
        /// </summary>
        /// <param name="prefix">参数的前缀，sqlserver: "@"，mysql: "$"，oracle: ":"</param>
        /// <param name="tableName">数据表名称</param>
        /// <returns></returns>
        public static MySearchUtil New(string prefix, string tableName = "")
        {
            return new MySearchUtil(prefix, tableName);
        }
        #endregion

        #region 连接子句
        /// <summary>
        /// And子句
        /// </summary>
        /// <param name="conditionString">条件</param>
        /// <returns></returns>
        private MySearchUtil AND(string conditionString)
        {
            if (string.IsNullOrWhiteSpace(conditionString))
            {
                return this;
            }

            if (!string.IsNullOrWhiteSpace(_conditonClauses))
            {
                _conditonClauses += " AND ";
            }
            _conditonClauses += conditionString;
            return this;
        }

        /// <summary>
        /// Or子句
        /// </summary>
        /// <param name="conditionString"></param>
        /// <returns></returns>
        private MySearchUtil OR(string conditionString)
        {
            if (string.IsNullOrWhiteSpace(conditionString))
            {
                return this;
            }

            if (!string.IsNullOrWhiteSpace(_conditonClauses))
            {
                _conditonClauses = "(" + _conditonClauses + ") OR ";
            }
            _conditonClauses += conditionString;
            return this;
        }

        /// <summary>
        /// AndOr子句
        /// </summary>
        /// <param name="mcc"></param>
        /// <returns></returns>
        public MySearchUtil AndOr(ConditionClausList mcc)
        {
            if (mcc.Count == 0)
            {
                return this;
            }

            var strs = ResolveConditionClauses(mcc);
            if (!string.IsNullOrWhiteSpace(_conditonClauses))
            {
                _conditonClauses += " AND ";
            }
            _conditonClauses += "(" + string.Join(" OR ", strs) + ")";
            return this;
        }
        #endregion

        #region AND
        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public MySearchUtil AndEqual(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + "=" + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// 不等于
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndNotEqual(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + "<>" + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndGreaterThan(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + ">" + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndGreaterThanEqual(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + ">=" + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndLessThan(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + "<" + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndLessThanEqual(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + "<=" + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndContains(string column, string value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, "%" + value + "%");
            return AND(string.Format(GetColumnName(column, tableName) + " LIKE " + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil AndContains(string[] columns, string value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, "%" + value + "%");

            if (columns.Count() > 0)
            {
                if (!string.IsNullOrWhiteSpace(_conditonClauses))
                {
                    _conditonClauses += " AND ";
                }
                _conditonClauses += "(";
                List<string> claus = new List<string>();
                foreach (var column in columns)
                {
                    claus.Add(GetColumnName(column, tableName) + " LIKE " + _sqlParameterPrefix + paramName);
                }

                _conditonClauses += string.Join(" Or ", claus) + ")";
            }

            return this;
        }

        /// <summary>
        /// 左包含
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndStartWith(string column, string value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value + "%");
            return AND(string.Format(GetColumnName(column, tableName) + " LIKE " + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// 右包含
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndEndWith(string column, string value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, "%" + value);
            return AND(string.Format(GetColumnName(column, tableName) + " LIKE " + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// In子句-数字
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndIn(string column, int[] value, string tableName = "")
        {
            if (value.Count() == 0)
            {
                return this;
            }

            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + " IN " + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// In子句-数字
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndIn(string column, float[] value, string tableName = "")
        {
            if (value.Count() == 0)
            {
                return this;
            }

            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + " IN " + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// In子句-数字
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndIn(string column, decimal[] value, string tableName = "")
        {
            if (value.Count() == 0)
            {
                return this;
            }

            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + " IN " + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// In子句-文本
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndIn(string column, string[] value, string tableName = "")
        {
            if (value.Count() == 0)
            {
                return this;
            }

            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + " IN " + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// In子句-日期
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MySearchUtil AndIn(string column, DateTime[] value, string tableName = "")
        {
            if (value.Count() == 0)
            {
                return this;
            }

            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return AND(string.Format(GetColumnName(column, tableName) + " IN " + _sqlParameterPrefix + paramName));
        }

        /// <summary>
        /// 不为空
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public MySearchUtil AndNotNull(string column, string tableName = "")
        {
            return AND(GetColumnName(column, tableName) + " IS NOT NULL");
        }

        /// <summary>
        /// 不为空且不为空字符串
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public MySearchUtil AndNotNullOrEmpty(string column, string tableName = "")
        {
            return AND(GetColumnName(column, tableName) + " IS NOT NULL AND " + column + " <> ''");
        }

        /// <summary>
        /// 为空
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public MySearchUtil AndNull(string column, string tableName = "")
        {
            return AND(GetColumnName(column, tableName) + " IS NULL");
        }

        /// <summary>
        /// 为空或空字符串
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public MySearchUtil AndNullOrEmpty(string column, string tableName = "")
        {
            return AND(GetColumnName(column, tableName) + " IS NULL AND " + GetColumnName(column, tableName) + " = ''");
        }

        /// <summary>
        /// 没有参数的And子句
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public MySearchUtil AndNoParam(string condition)
        {
            return AND(condition);
        }
        #endregion

        #region OR
        public MySearchUtil OrEqual(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return OR(string.Format(GetColumnName(column, tableName) + "=" + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrGreaterThan(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return OR(string.Format(GetColumnName(column, tableName) + ">" + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrGreaterThanEqual(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return OR(string.Format(GetColumnName(column, tableName) + ">=" + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrLessThan(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return OR(string.Format(GetColumnName(column, tableName) + "<" + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrLessThanEqual(string column, object value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return OR(string.Format(GetColumnName(column, tableName) + "<=" + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrContains(string column, string value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, "%" + value + "%");
            return OR(string.Format(GetColumnName(column, tableName) + " LIKE " + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrStartWith(string column, string value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value + "%");
            return OR(string.Format(GetColumnName(column, tableName) + " LIKE " + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrEndWith(string column, string value, string tableName = "")
        {
            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, "%" + value);
            return OR(string.Format(GetColumnName(column, tableName) + " LIKE " + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrIn(string column, object[] value, string tableName = "")
        {
            if (value.Count() == 0)
            {
                return this;
            }

            var paramName = "p" + _idx++.ToString();
            _params.Add(paramName, value);
            return OR(string.Format(GetColumnName(column, tableName) + " IN " + _sqlParameterPrefix + paramName));
        }

        public MySearchUtil OrNoParam(string condition)
        {
            return OR(condition);
        }
        #endregion

        #region 解析条件子句
        private string ResolveConditonClaus(ConditionClaus claus)
        {
            string op = ResolveOp(claus.Op);
            var paramName = "p" + _idx++.ToString();
            string condition = "";
            if (!string.IsNullOrWhiteSpace(op))
            {
                condition = string.Format("{0} {1} {2}", claus.Column, op, _sqlParameterPrefix + paramName);
                _params.Add(paramName, claus.Value);
            }
            else if (claus.Op == SqlOperator.Contains)
            {
                condition = claus.Column + " LIKE " + _sqlParameterPrefix + paramName;
                _params.Add(paramName, "%" + claus.Value + "%");
            }
            else if (claus.Op == SqlOperator.StartWith)
            {
                condition = claus.Column + " LIKE " + _sqlParameterPrefix + paramName;
                _params.Add(paramName, claus.Value + "%");
            }
            else if (claus.Op == SqlOperator.EndWith)
            {
                condition = claus.Column + " LIKE " + _sqlParameterPrefix + paramName;
                _params.Add(paramName, "%" + claus.Value);
            }
            return condition;
        }

        private List<string> ResolveConditionClauses(ConditionClausList mcc)
        {
            List<string> clauses = new List<string>();
            foreach (var m in mcc)
            {
                clauses.Add(ResolveConditonClaus(m));
            }
            return clauses;
        }

        private string ResolveOp(SqlOperator op)
        {
            switch (op)
            {
                case SqlOperator.Equal:
                    return "=";
                case SqlOperator.NotEqual:
                    return "<>";
                case SqlOperator.GreaterThan:
                    return ">";
                case SqlOperator.GreaterThanEqual:
                    return ">=";
                case SqlOperator.LessThan:
                    return "<";
                case SqlOperator.LessThanEqual:
                    return "<=";
                case SqlOperator.In:
                    return "IN";
                default:
                    return "";
            }
        }
        #endregion

        #region 排序
        public MySearchUtil OrderBy(string column, string tableName = "")
        {
            _orderby.Add(GetColumnName(column, tableName));
            return this;
        }

        public MySearchUtil OrderByDesc(string column, string tableName = "")
        {
            _orderby.Add(GetColumnName(column, tableName) + " DESC");
            return this;
        }

        public MySearchUtil OrderBy(SortClaus sortClaus)
        {
            string orderby = sortClaus.Column;
            if (sortClaus.Direction == SqlOrderByDirection.DESC)
            {
                orderby += " DESC";
            }
            _orderby.Add(sortClaus.Column);
            return this;
        }

        public MySearchUtil OrderBy(SortClausList sortClauses)
        {
            foreach (var claus in sortClauses)
            {
                string ob = claus.Column;
                if (claus.Direction == SqlOrderByDirection.DESC)
                {
                    ob += " DESC";
                }
                _orderby.Add(ob);
            }
            return this;
        }
        #endregion

        #region 查询的列
        public MySearchUtil Select(string cols)
        {
            if (!string.IsNullOrWhiteSpace(cols))
            {
                _cols.Add(cols);
            }
            return this;
        }

        public MySearchUtil Select(string table, string cols)
        {
            SelectClaus sc = new SelectClaus(table, cols);
            return Select(sc);
        }

        public MySearchUtil Select(SelectClaus select)
        {
            _cols.Add(select.Column);
            return this;
        }

        public MySearchUtil Select(SelectClausList selectClausList)
        {
            foreach (var sc in selectClausList)
            {
                _cols.Add(sc.Column);
            }
            return this;
        }
        #endregion

        private string GetColumnName(string column, string tableName = "")
        {
            return string.IsNullOrWhiteSpace(tableName) ? column : "[" + tableName + "].[" + column + "]";
        }
    }

    public class ConditionClausList : List<ConditionClaus>
    {
        public static ConditionClausList New()
        {
            return new ConditionClausList();
        }

        public ConditionClausList Add(string column, SqlOperator op, object value)
        {
            this.Add(new ConditionClaus(column, op, value));
            return this;
        }
    }

    public class ConditionClaus
    {
        public string Column { get; set; }

        public object Value { get; set; }

        public SqlOperator Op;

        public ConditionClaus(string column, SqlOperator op, object val)
        {
            Column = column;
            Op = op;
            Value = val;
        }
    }

    public enum SqlOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanEqual,
        LessThan,
        LessThanEqual,
        Contains,
        StartWith,
        EndWith,
        In
    }

    public class SortClaus
    {
        public string Column { get; set; }

        public SqlOrderByDirection Direction { get; set; }

        public SortClaus(string column, SqlOrderByDirection direction)
        {
            Column = column;
            Direction = direction;
        }

        public SortClaus(string column) : this(column, SqlOrderByDirection.ASC)
        { }
    }

    public class SortClausList : List<SortClaus>
    {
        public static SortClausList New()
        {
            return new SortClausList();
        }

        public SortClausList Add(string column, SqlOrderByDirection direction)
        {
            this.Add(new SortClaus(column, direction));
            return this;
        }

        public SortClausList Add(string column)
        {
            return this.Add(column, SqlOrderByDirection.ASC);
        }
    }

    public class SelectClaus
    {
        public string Table { get; set; }

        public string Column { get; set; }

        public SelectClaus(string table, string column)
        {
            Table = table;
            var columns = column.Split(',');
            if (columns.Length == 1)
            {
                if (string.IsNullOrWhiteSpace(table))
                {
                    Column = column;
                }
                else
                {
                    Column = "[" + table + "]." + column;
                }
            }
            else
            {
                foreach (var s in columns)
                {
                    if (string.IsNullOrWhiteSpace(table))
                    {
                        Column += ",";
                    }
                    else
                    {
                        Column += ",[" + table + "]." + s;
                    }
                }

                if (!string.IsNullOrWhiteSpace(Column))
                {
                    Column = Column.Remove(0, 1);
                }
            }
        }

        public SelectClaus(string table, string[] columns)
        {
            Table = table;
            foreach (var s in columns)
            {
                Column += "[" + table + "]." + s;
            }
        }

        public SelectClaus(string table, List<string> columns) : this(table, columns.ToArray())
        { }

        public SelectClaus(string column) : this("", column)
        { }

        public SelectClaus(string[] columns) : this("", columns)
        { }

        public SelectClaus(List<string> columns) : this("", columns)
        { }
    }

    public class SelectClausList : List<SelectClaus>
    {
        public static SelectClausList New()
        {
            return new SelectClausList();
        }

        public SelectClausList Add(string table, string columns)
        {
            this.Add(new SelectClaus(table, columns));
            return this;
        }

        public SelectClausList Add(string columns)
        {
            this.Add(new SelectClaus(columns));
            return this;
        }

        public SelectClausList Add(string[] columns)
        {
            this.Add(new SelectClaus(columns));
            return this;
        }

        public SelectClausList Add(List<string> columns)
        {
            this.Add(new SelectClaus(columns));
            return this;
        }

        public SelectClausList Add(string table, string[] columns)
        {
            this.Add(new SelectClaus(table, columns));
            return this;
        }

        public SelectClausList Add(string table, List<string> columns)
        {
            this.Add(new SelectClaus(table, columns));
            return this;
        }
    }

    public enum SqlOrderByDirection
    {
        ASC,
        DESC
    }
}
