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
    /// ����ʵ�������
    /// </summary>
    public class MyContainer
    {
        /// <summary>
        /// ʵ�弰ʵ����Ϣ���ֵ�
        /// </summary>
        private static ConcurrentDictionary<string, MyEntityInfo> _dict = new ConcurrentDictionary<string, MyEntityInfo>();

        #region ��������
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
