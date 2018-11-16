using HZC.Database;
using System.Collections.Generic;
using System.Linq;

namespace HZC.Common.Services
{
    public class DataItemUtil
    {
        private static List<DataItemDto> _items;

        /// <summary>
        /// 获取所有字典项
        /// </summary>
        /// <returns></returns>
        public static List<DataItemDto> All()
        {
            if (_items == null)
            {
                var service = new DataItemService();
                var list = service.Fetch(MySearchUtil.New()
                    .AndEqual("IsDel", false));
                _items = list.Select(d => new DataItemDto
                {
                    K = d.K,
                    V = d.V
                }).ToList();
            }
            return _items;
        }

        /// <summary>
        /// 获取指定key的字典项
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static DataItemDto Get(string k)
        {
            return All().Where(d => d.K == k.Trim()).SingleOrDefault();
        }

        /// <summary>
        /// 获取字典中指定Key的值，如果key不存在，返回NULL
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static string GetString(string k)
        {
            var di = Get(k);
            if (di == null)
            {
                return null;
            }
            else
            {
                return di.V;
            }
        }

        /// <summary>
        /// 获取字典中指定key的值使用符号'|'分割后的数组
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static string[] GetValues(string k)
        {
            var value = Get(k);
            if (value == null)
            {
                return new string[] { };
            }
            else
            {
                var str = value.V;
                return str.Split('|');
            }
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        public static void Clear()
        {
            _items = null;
        }
    }
}
