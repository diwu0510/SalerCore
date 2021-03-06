using HZC.Core;
using HZC.Database.Container;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace HZC.Database
{
    public class MyDbUtil
    {
        private string _connectionString;

        #region 构造方法
        public MyDbUtil(string connStr)
        {
            _connectionString = connStr;
        }

        public MyDbUtil() : this(ConfigurationManager.AppSettings["DefaultConnectionString"])
        { }

        public static MyDbUtil New()
        {
            return new MyDbUtil();
        }
        #endregion

        #region 获取一个SqlConnection
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        #endregion

        #region 获取查询的sql语句
        public string GetQuerySql(string cols, string table, string where, string orderby)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(cols);
            sb.Append(" FROM ");
            sb.Append(table);
            sb.Append(" WHERE ");
            sb.Append(where);
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                sb.Append(" ORDER BY ");
                sb.Append(orderby);
            }
            return sb.ToString();
        }

        public string GetPagingQuerySql(string cols, string tables, string condition, string orderby, int index, int size)
        {
            if (index == 1)
            {
                string sql = string.Format(
                    @"SELECT TOP {4} {0} FROM {1} WHERE {2} ORDER BY {3};SELECT @RecordCount=COUNT(0) FROM {1} WHERE {2}",
                    cols,
                    tables,
                    condition,
                    orderby,
                    size
                );

                return sql;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("FROM ").Append(tables);

                if (!string.IsNullOrWhiteSpace(condition))
                {
                    sb.Append(" WHERE ").Append(condition);
                }

                if (string.IsNullOrWhiteSpace(orderby))
                {
                    throw new Exception("分页列表必须指定orderby字段");
                }

                string sql = string.Format(
                    @"  WITH PAGEDDATA AS
					    (
						    SELECT TOP 100 PERCENT {0}, ROW_NUMBER() OVER (ORDER BY {1}) AS FLUENTDATA_ROWNUMBER
						    {2}
					    )
					    SELECT *
					    FROM PAGEDDATA
					    WHERE FLUENTDATA_ROWNUMBER BETWEEN {3} AND {4};
                        SELECT @RecordCount=COUNT(0) FROM {5} WHERE {6}",
                    cols,
                    orderby,
                    sb,
                    (index - 1) * size + 1,
                    index * size,
                    tables,
                    condition
                );
                return sql;
            }
        }
        #endregion

        #region 获取所有数据
        public IEnumerable<T> FetchBySql<T>(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql, param);
            }
        }

        public IEnumerable<T> Fetch<T>(MySearchUtil util, string table = "", string cols = "*")
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                table = GetTableName(typeof(T));
            }

            string where = util.ConditionClaus;
            string orderby = util.OrderByClaus;
            DynamicParameters param = util.Parameters;

            string sql = GetQuerySql(cols, table, where, orderby);
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql, param);
            }
        }

        public IEnumerable<dynamic> FetchBySql(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query(sql, param);
            }
        }

        public IEnumerable<dynamic> Fetch(MySearchUtil util, string table, string cols = "*")
        {
            string where = util.ConditionClaus;
            string orderby = util.OrderByClaus;
            DynamicParameters param = util.Parameters;

            string sql = GetQuerySql(cols, table, where, orderby);
            using (var conn = GetConnection())
            {
                return conn.Query(sql, param);
            }
        }
        #endregion

        #region 分页获取数据
        public PageList<T> Query<T>(MySearchUtil util, int pageIndex, int pageSize, string table = "", string cols = "*")
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                table = GetTableName(typeof(T));
            }

            string where = util.ConditionClaus;
            string orderby = util.OrderByClaus;
            DynamicParameters param = util.PageListParameters;

            string sql = GetPagingQuerySql(cols, table, where, orderby, pageIndex, pageSize);
            using (var conn = GetConnection())
            {
                var list = conn.Query<T>(sql, param);
                var total = param.Get<int>("RecordCount");
                return new PageList<T>
                {
                    Body = list,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    RecordCount = total
                };
            }
        }

        public PageList<dynamic> Query(MySearchUtil util, int pageIndex, int pageSize, string table, string cols = "*")
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                throw new ArgumentNullException("表名不能为空");
            }

            string where = util.ConditionClaus;
            string orderby = util.OrderByClaus;
            DynamicParameters param = util.PageListParameters;

            string sql = GetPagingQuerySql(cols, table, where, orderby, pageIndex, pageSize);
            using (var conn = GetConnection())
            {
                var list = conn.Query(sql, param);
                var total = param.Get<int>("RecordCount");
                return new PageList<dynamic>
                {
                    Body = list,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    RecordCount = total
                };
            }
        }
        #endregion

        #region 加载实体
        /// <summary>
        /// 加载一个动态类型实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic LoadBySql(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query(sql, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 使用sql语句加载实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadBySql<T>(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 通过id加载实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public T Load<T>(int id, string cols = "*", string tableName = "")
        {
            using (var conn = GetConnection())
            {
                tableName = string.IsNullOrWhiteSpace(tableName) ? GetTableName(typeof(T)) : tableName;
                string sql = "SELECT " + cols + " FROM [" + tableName + "] WHERE Id=@id";
                return conn.Query<T>(sql, new { id = id }).SingleOrDefault();
            }
        }

        /// <summary>
        /// 指定条件加载实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="util"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public T Load<T>(MySearchUtil util, string cols = "*", string tableName = "")
        {
            using (var conn = GetConnection())
            {
                tableName = string.IsNullOrWhiteSpace(tableName) ? GetTableName(typeof(T)) : tableName;
                string sql = "SELECT TOP 1 " + cols + " FROM [" + tableName + "] WHERE " +
                    util.ConditionClaus + (string.IsNullOrWhiteSpace(util.OrderByClaus) ? "" : " ORDER BY " + util.OrderByClaus);
                return conn.Query<T>(sql, util.Parameters).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>  
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, P1>(string sql, Func<T, P1, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T, P1, T>(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <typeparam name="P2"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, P1, P2>(string sql, Func<T, P1, P2, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T, P1, P2, T>(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <typeparam name="P2"></typeparam>
        /// <typeparam name="P3"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, P1, P2, P3>(string sql, Func<T, P1, P2, P3, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T, P1, P2, P3, T>(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <typeparam name="P2"></typeparam>
        /// <typeparam name="P3"></typeparam>
        /// <typeparam name="P4"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, P1, P2, P3, P4>(string sql, Func<T, P1, P2, P3, P4, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T, P1, P2, P3, P4, T>(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <typeparam name="P2"></typeparam>
        /// <typeparam name="P3"></typeparam>
        /// <typeparam name="P4"></typeparam>
        /// <typeparam name="P5"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, P1, P2, P3, P4, P5>(string sql, Func<T, P1, P2, P3, P4, P5, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T, P1, P2, P3, P4, P5, T>(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }


        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadWith<T, T1>(string sql, Func<T, List<T1>, T> func, Object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1s = multiReader.Read<T1>().ToList();
                    func(entity, sub1s);
                    return entity;
                }
            }
        }

        public T LoadWith<T, T1, T2>(string sql, Func<T, List<T1>, List<T2>, T> func, Object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1s = multiReader.Read<T1>().ToList();
                    var sub2s = multiReader.Read<T2>().ToList();
                    func(entity, sub1s, sub2s);
                    return entity;
                }
            }
        }

        public T LoadWith<T, T1, T2, T3>(string sql, Func<T, List<T1>, List<T2>, List<T3>, T> func, Object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1s = multiReader.Read<T1>().ToList();
                    var sub2s = multiReader.Read<T2>().ToList();
                    var sub3s = multiReader.Read<T3>().ToList();
                    func(entity, sub1s, sub2s, sub3s);
                    return entity;
                }
            }
        }

        public T LoadWith<T, T1, T2, T3, T4>(string sql, Func<T, List<T1>, List<T2>, List<T3>, List<T4>, T> func, Object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1s = multiReader.Read<T1>().ToList();
                    var sub2s = multiReader.Read<T2>().ToList();
                    var sub3s = multiReader.Read<T3>().ToList();
                    var sub4s = multiReader.Read<T4>().ToList();
                    func(entity, sub1s, sub2s, sub3s, sub4s);
                    return entity;
                }
            }
        }

        public T LoadWith<T, T1, T2, T3, T4, T5>(string sql, Func<T, List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, T> func, Object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1s = multiReader.Read<T1>().ToList();
                    var sub2s = multiReader.Read<T2>().ToList();
                    var sub3s = multiReader.Read<T3>().ToList();
                    var sub4s = multiReader.Read<T4>().ToList();
                    var sub5s = multiReader.Read<T5>().ToList();
                    func(entity, sub1s, sub2s, sub3s, sub4s, sub5s);
                    return entity;
                }
            }
        }
        #endregion

        #region 获取数量
        public int GetCount<T>(MySearchUtil util, string tableName = "")
        {
            string condition = "";
            DynamicParameters param = null;
            
            condition = util.ConditionClaus;
            param = util.Parameters;

            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = GetTableName(typeof(T));
            }
            using (var conn = GetConnection())
            {
                string sql = "SELECT COUNT(0) FROM [" + tableName + "] WHERE " + condition;
                return conn.ExecuteScalar<int>(sql, param);
            }
        }

        public int GetCount(MySearchUtil util, string tableName)
        {
            string condition = "";
            DynamicParameters param = null;

            condition = util.ConditionClaus;
            param = util.Parameters;
            
            using (var conn = GetConnection())
            {
                string sql = "SELECT COUNT(0) FROM [" + tableName + "] WHERE " + condition;
                return conn.ExecuteScalar<int>(sql, param);
            }
        }
        #endregion

        #region 添加实体
        public int Create<T>(T t)
        {
            using (var conn = GetConnection())
            {
                string sql = MyContainer.Get(typeof(T)).InsertSql;
                return conn.Execute(sql, t);
            }
        }

        public int Create<T>(List<T> ts)
        {
            if (ts.Count == 0) return 0;
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = MyContainer.Get(typeof(T)).InsertSql;
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var rows = conn.Execute(sql, ts, trans, 30, CommandType.Text);
                        trans.Commit();
                        return rows;
                    }
                    catch (DataException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region 修改实体
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">要更新的实体</param>
        /// <returns></returns>
        public int Update<T>(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("要修改的实体为空");
            }

            using (var conn = GetConnection())
            {
                string sql = MyContainer.Get(typeof(T)).UpdateSql;
                return conn.Execute(sql, t);
            }
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts">要更新的实体列表</param>
        /// <returns></returns>
        public int Update<T>(List<T> ts)
        {
            using (var conn = GetConnection())
            {
                string sql = MyContainer.Get(typeof(T)).UpdateSql;
                return conn.Execute(sql, ts);
            }
        }

        /// <summary>
        /// 更新实体的指定列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">要更新的实体</param>
        /// <param name="include">要更新的列</param>
        /// <returns></returns>
        public int UpdateInclude<T>(T t, string[] include)
        {
            if (t == null)
            {
                throw new ArgumentNullException("要修改的实体为空");
            }

            if (include.Length == 0)
            {
                throw new ArgumentException("必须指定要更新的列");
            }

            using (var conn = GetConnection())
            {
                var sql = MyContainer.Get(typeof(T)).GetUpdateSql(include);
                return conn.Execute(sql, t);
            }
        }

        /// <summary>
        /// 批量更新实体的指定列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts">要更新的实体列表</param>
        /// <param name="include">要更新的列</param>
        /// <returns></returns>
        public int UpdateInclude<T>(List<T> ts, string[] include)
        {
            if (include.Length == 0)
            {
                throw new ArgumentException("必须指定要更新的列");
            }

            using (var conn = GetConnection())
            {
                var sql = MyContainer.Get(typeof(T)).GetUpdateSql(include);
                return conn.Execute(sql, ts);
            }
        }

        /// <summary>
        /// 更新实体，指定不需要更新的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">要更新的实体</param>
        /// <param name="exclude">不需要更新的列</param>
        /// <returns></returns>
        public int UpdateExclude<T>(T t, string[] exclude)
        {
            if (t == null)
            {
                throw new ArgumentNullException("要修改的实体为空");
            }

            if (exclude.Length == 0)
            {
                throw new ArgumentException("必须指定要更新的列");
            }

            using (var conn = GetConnection())
            {
                var sql = MyContainer.Get(typeof(T)).GetUpdateSql(null, exclude, "default");
                return conn.Execute(sql, t);
            }
        }

        /// <summary>
        /// 更新实体列表，指定不需要更新的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts">要更新的实体列表</param>
        /// <param name="exclude">不需要更新的列</param>
        /// <returns></returns>
        public int UpdateExclude<T>(List<T> ts, string[] exclude)
        {
            if (exclude.Length == 0)
            {
                throw new ArgumentException("必须指定要更新的列");
            }

            using (var conn = GetConnection())
            {
                var sql = MyContainer.Get(typeof(T)).GetUpdateSql(null, exclude, "default");
                return conn.Execute(sql, ts);
            }
        }

        /// <summary>
        /// 指定更新的列和值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="kvs">要更新的列和值</param>
        /// <param name="msu">要更新的数据条件</param>
        /// <returns></returns>
        public int Set<T>(KeyValuePairs kvs, MySearchUtil msu)
        {
            DynamicParameters param = new DynamicParameters();
            List<string> props = new List<string>();

            foreach (var kv in kvs)
            {
                props.Add(kv.Key);
                param.Add(kv.Key, kv.Value);
            }

            var sql = MyContainer.Get(typeof(T)).GetUpdateSql(props.ToArray(), null, msu.ConditionClaus);
            using (var conn = GetConnection())
            {
                return conn.Execute(sql, param);
            }
        }
        #endregion

        #region 删除实体
        public int Delete<T>(int id, string tableName = "")
        {
            using (var conn = GetConnection())
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = GetTableName(typeof(T));
                }
                return conn.Execute(string.Format(@"DELETE [{0}] WHERE Id=@id", tableName), new { id = id });
            }
        }

        public int Delete<T>(int[] ids, string tableName = "")
        {
            using (var conn = GetConnection())
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = GetTableName(typeof(T));
                }

                return conn.Execute(string.Format(@"DELETE [{0}] WHERE Id in @ids", tableName), new { ids = ids });
            }
        }

        public int Delete<T>(MySearchUtil mcu, string tableName = "")
        {
            using (var conn = GetConnection())
            {
                if (mcu == null)
                {
                    mcu = new MySearchUtil();
                }

                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = GetTableName(typeof(T));
                }

                return conn.Execute(string.Format(@"DELETE [{0}] WHERE {1}", tableName, mcu.ConditionClaus), mcu.Parameters);
            }
        }

        public int Remove<T>(int id, IAppUser user = null, string tableName = "")
        {
            using (var conn = GetConnection())
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = GetTableName(typeof(T));
                }
                if (user != null)
                {
                    return conn.Execute(string.Format(@"UPDATE [{0}] SET IsDel=1,UpdateAt=GETDATE(),Updator=@UserName WHERE Id=@id", tableName), new { id = id, UserName = user.Name });
                }
                else
                {
                    return conn.Execute(string.Format(@"UPDATE [{0}] SET IsDel=1 WHERE Id=@id", tableName), new { id = id });
                }
            }
        }

        public int Remove<T>(int[] ids, IAppUser user = null, string tableName = "")
        {
            using (var conn = GetConnection())
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = GetTableName(typeof(T));
                }
                if (user == null)
                {
                    return conn.Execute(string.Format(@"UPDATE [{0}] SET IsDel=1 WHERE Id in @ids", tableName), new { ids = ids });
                }
                else
                {
                    return conn.Execute(string.Format(@"UPDATE [{0}] SET IsDel=1,UpdateAt=GETDATE(),Updator=@UserName WHERE Id in @ids", tableName), new { ids = ids, UserName = user.Name });
                }
            }
        }

        public int Remove<T>(MySearchUtil mcu, string tableName = "")
        {
            using (var conn = GetConnection())
            {
                if (mcu == null)
                {
                    mcu = new MySearchUtil();
                }

                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = GetTableName(typeof(T));
                }

                return conn.Execute(string.Format(@"UPDATE [{0}] SET IsDel=1 WHERE ", tableName, mcu.ConditionClaus), mcu.Parameters);
            }
        }
        #endregion

        #region 执行sql操作
        /// <summary>
        /// 执行sql语句，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, Object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Execute(sql, param);
            }
        }

        /// <summary>
        /// 执行sql并获取第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ExecuteScalar<T>(sql, param);
            }
        }

        /// <summary>
        /// 执行存储过程，返回受影响的行数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(string procName, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Execute(procName, param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 执行存储过程，返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteProc<T>(string procName, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ExecuteScalar<T>(procName, param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 以事务的方式执行一组sql语句
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExecuteTran(string[] sqls)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var s in sqls)
                        {
                            conn.Execute(s, null, tran);
                        }
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一组带参数的sql语句
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExecuteTran(KeyValuePairs sqls)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var s in sqls)
                        {
                            conn.Execute(s.Key, s.Value, tran);
                        }
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 执行sql，返回多个列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>> MultiQuery<T1, T2>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var conn = GetConnection())
            {
                using (var multi = conn.QueryMultiple(sql, param, commandType: commandType))
                {
                    var list1 = multi.Read<T1>();
                    var list2 = multi.Read<T2>();

                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(list1, list2);
                }
            }
        }

        /// <summary>
        /// 执行sql，返回多个列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> MultiQuery<T1, T2, T3>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var conn = GetConnection())
            {
                using (var multi = conn.QueryMultiple(sql, param, commandType: commandType))
                {
                    var list1 = multi.Read<T1>();
                    var list2 = multi.Read<T2>();
                    var list3 = multi.Read<T3>();

                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(list1, list2, list3);
                }
            }
        }

        /// <summary>
        /// 执行sql，返回多个列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> MultiQuery<T1, T2, T3, T4>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var conn = GetConnection())
            {
                using (var multi = conn.QueryMultiple(sql, param, commandType: commandType))
                {
                    var list1 = multi.Read<T1>();
                    var list2 = multi.Read<T2>();
                    var list3 = multi.Read<T3>();
                    var list4 = multi.Read<T4>();

                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>(list1, list2, list3, list4);
                }
            }
        }

        /// <summary>
        /// 执行sql，返回多个列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>> MultiQuery<T1, T2, T3, T4, T5>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var conn = GetConnection())
            {
                using (var multi = conn.QueryMultiple(sql, param, commandType: commandType))
                {
                    var list1 = multi.Read<T1>();
                    var list2 = multi.Read<T2>();
                    var list3 = multi.Read<T3>();
                    var list4 = multi.Read<T4>();
                    var list5 = multi.Read<T5>();

                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>(list1, list2, list3, list4, list5);
                }
            }
        }
        #endregion

        #region 私有方法
        private string GetTableName(Type type)
        {
            var entityInfo = MyContainer.Get(type);
            return entityInfo.TableName;
        }
        #endregion

        #region 获取创建和修改的SQL语句
        public string GetInsertSql<T>()
        {
            return MyContainer.Get(typeof(T))?.InsertSql;
        }

        public string GetUpdateSql<T>()
        {
            return MyContainer.Get(typeof(T))?.UpdateSql;
        } 
        #endregion
    }

    public class KeyValuePairs : List<KeyValuePair<string, object>>
    {
        public static KeyValuePairs New()
        {
            return new KeyValuePairs();
        }

        public KeyValuePairs Add(string column, object value)
        {
            this.Add(new KeyValuePair<string, object>(column, value));
            return this;
        }
    }
}
