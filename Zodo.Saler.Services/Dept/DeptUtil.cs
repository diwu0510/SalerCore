using System.Collections.Generic;
using System.Linq;

namespace Zodo.Saler.Services
{
    public class DeptUtil
    {
        private static List<DeptEntity> _depts;

        private static void Init()
        {
            var service = new DeptService();
            var depts = service.Fetch(new DeptSearchParam().ToSearchUtil());

            _depts = depts.ToList();
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public static List<DeptEntity> All()
        {
            if (_depts == null)
            {
                Init();
            }
            return _depts;
        }

        /// <summary>
        /// 清理部门缓存
        /// </summary>
        public static void Clear()
        {
            _depts = null;
        }
    }
}
