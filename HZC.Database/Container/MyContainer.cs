using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HZC.Database.Container
{
    /// <summary>
    /// 数据实体的容器
    /// </summary>
    public class MyContainer
    {
        /// <summary>
        /// 实体及实体信息的字典
        /// </summary>
        private static ConcurrentDictionary<string, MyEntityInfo> _dict = new ConcurrentDictionary<string, MyEntityInfo>();

        #region 公共方法
        public static MyEntityInfo Get(Type type)
        {
            MyEntityInfo result;
            if (!_dict.TryGetValue(type.Name, out result))
            {
                result = new MyEntityInfo(type);
                _dict.TryAdd(type.Name, result);
            }
            return result;
        }
        #endregion
    }
}
