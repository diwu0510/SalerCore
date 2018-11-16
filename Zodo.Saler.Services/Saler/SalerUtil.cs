using System.Collections.Generic;
using System.Linq;

namespace Zodo.Saler.Services
{
    public class SalerUtil
    {
        private static List<SalerEntity> _salers;

        private static void Init()
        {
            var service = new SalerService();
            var salers = service.Fetch(new SalerSearchParam().ToSearchUtil());

            _salers = salers.ToList();
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public static List<SalerEntity> All()
        {
            if (_salers == null)
            {
                Init();
            }
            return _salers;
        }

        /// <summary>
        /// 清理部门缓存
        /// </summary>
        public static void Clear()
        {
            _salers = null;
        }
    }
}
