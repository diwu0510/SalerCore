using Newtonsoft.Json;
using System;

namespace HZC.Core
{
    public class BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [MyDataField(IsPrimaryKey = true)]
        public int Id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        [JsonConverter(typeof(DateTimeFormatConverter))]
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建人帐号
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public string Creator { get; set; }
        
        /// <summary>
        /// 更新日期
        /// </summary>
        [JsonConverter(typeof(DateTimeFormatConverter))]
        public DateTime UpdateAt { get; set; }
        
        /// <summary>
        /// 更新人ID
        /// </summary>
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新人帐号
        /// </summary>
        public string Updator { get; set; }

        public void BeforeCreate(IAppUser user)
        {
            CreateAt = DateTime.Now;
            CreateBy = user.Id;
            Creator = user.Name;
            UpdateAt = DateTime.Now;
            UpdateBy = user.Id;
            Updator = user.Name; 
        }

        public void BeforeUpdate(IAppUser user)
        {
            UpdateAt = DateTime.Now;
            UpdateBy = user.Id;
            Updator = user.Name;
        }
    }
}
