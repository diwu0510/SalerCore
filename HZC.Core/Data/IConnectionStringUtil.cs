namespace HZC.Core
{
    public interface IConnectionStringUtil
    {
        /// <summary>
        /// 获取默认的数据库连接字符串
        /// </summary>
        /// <returns></returns>
        string GetDefaultConnectionString();

        /// <summary>
        /// 获取AppSetting.json中ConnectionStrings下指定名称的数据库连接字符串
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        string GetConnectionString(string sectionName);
    }
}
