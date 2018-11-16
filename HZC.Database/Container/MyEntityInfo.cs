using HZC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HZC.Database.Container
{
    /// <summary>
    /// ʵ����Ϣ
    /// </summary>
    public class MyEntityInfo
    {
        private string _prefix = "@";

        public MyEntityInfo()
        { }

        public MyEntityInfo(string prefix)
        {
            _prefix = prefix;
        }

        #region ����
        /// <summary>
        /// ʵ������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ʵ���Ӧ�����ݱ����
        /// </summary>
        public string TableName { get; set; }

        public string KeyColumn { get; set; }

        public string KeyName { get; set; }

        /// <summary>
        /// �����б�
        /// </summary>
        public List<MyEntityPropertyInfo> Properties { get; set; }

        /// <summary>
        /// ��׼�������
        /// </summary>
        public string InsertSql { get; set; }

        /// <summary>
        /// ��׼�������
        /// </summary>
        public string UpdateSql { get; set; }
        #endregion

        #region ���췽��
        public MyEntityInfo(Type type)
        {
            Name = type.Name;
            Properties = new List<MyEntityPropertyInfo>();

            // ��ȡ����
            var tableAttrs = type.GetCustomAttributes(typeof(MyDataTableAttribute), false);
            if (tableAttrs == null || tableAttrs.Length == 0)
            {
                TableName = type.Name.Replace("Entity", "");
            }
            else
            {
                var attr = (MyDataTableAttribute)tableAttrs[0];
                TableName = attr.Name;
            }

            foreach (var p in type.GetProperties())
            {
                if (!ValidPropertyType(p))
                {
                    continue;
                }

                var colAttrs = p.GetCustomAttributes(typeof(MyDataFieldAttribute), false);
                if (colAttrs == null || colAttrs.Length == 0)
                {
                    Properties.Add(new MyEntityPropertyInfo
                    {
                        Name = p.Name,
                        ColumnName = p.Name
                    });
                }
                else
                {
                    var attr = (MyDataFieldAttribute)colAttrs[0];
                    if (attr.IsPrimaryKey)
                    {
                        KeyColumn = string.IsNullOrWhiteSpace(attr.ColumnName) ? p.Name : attr.ColumnName;
                        KeyName = p.Name;
                    }
                    Properties.Add(new MyEntityPropertyInfo
                    {
                        Name = p.Name,
                        ColumnName = string.IsNullOrWhiteSpace(attr.ColumnName) ? p.Name : attr.ColumnName,
                        InsertIgnore = attr.InsertIgnore,
                        UpdateIgnore = attr.UpdateIgnore,
                        Ignore = attr.Ignore || attr.IsPrimaryKey,
                        IsPrimaryKey = attr.IsPrimaryKey
                    });
                }
            }

            InsertSql = GetInsertSql();
            UpdateSql = GetUpdateSql(where: "default");
        }
        #endregion
        
        #region ˽�з���
        /// <summary>
        /// �ж�ʵ��������Ƿ��ǿ���ת��Ϊ�����е�����
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool ValidPropertyType(PropertyInfo property)
        {
            string[] types = new string[]
            {
                "Byte", "SByte", "Int16", "UInt16", "Int32", "UInt32", "Int64", "UInt64", "Single", "Double", "Boolean",
                "String", "Char", "Guid", "DateTime", "Byte[]", "DateTimeOffset"
            };

            string typeName;
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeName = property.PropertyType.GetGenericArguments()[0].Name;
            }
            else
            {
                typeName = property.PropertyType.Name;
            }
            
            return types.Contains(typeName);
        }

        public string GetInsertSql(string[] include = null, string[] exclude = null)
        {
            var props = Properties.Where(p => !p.Ignore && !p.InsertIgnore);
            if (include != null)
            {
                props = props.Where(p => include.Contains(p.Name));
            }
            if (exclude != null)
            {
                props = props.Where(p => !exclude.Contains(p.Name));
            }

            var ps = props.Select(p => "[" + p.Name + "]");
            var parameters = props.Select(p => _prefix + p.Name);

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [" + TableName + "] (");
            sb.Append(string.Join(",", ps));
            sb.Append(") VALUES (");
            sb.Append(string.Join(",", parameters));
            sb.Append(");");
            return sb.ToString();
        }

        public string GetUpdateSql(string[] include = null, string[] exclude = null, string where = "default")
        {
            var props = Properties.Where(p => !p.Ignore && !p.UpdateIgnore);
            if (include != null)
            {
                props = props.Where(p => include.Contains(p.Name));
            }
            if (exclude != null)
            {
                props = props.Where(p => !exclude.Contains(p.Name));
            }

            if (where == "default")
            {
                where = KeyColumn + "=" + _prefix + KeyName;
            }

            var ps = props.Select(p => "[" + p.Name + "]=" + _prefix + p.Name);

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE [" + TableName + "] SET ")
                .Append(string.Join(",", ps))
                .Append(string.IsNullOrWhiteSpace(where) ? "" : " WHERE " + where);
            return sb.ToString();
        }
        #endregion
    }
}
