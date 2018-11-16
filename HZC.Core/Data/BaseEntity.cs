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
        /// ��������
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        [JsonConverter(typeof(DateTimeFormatConverter))]
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// ������ID
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public int CreateBy { get; set; }

        /// <summary>
        /// �������ʺ�
        /// </summary>
        [MyDataField(UpdateIgnore = true)]
        public string Creator { get; set; }
        
        /// <summary>
        /// ��������
        /// </summary>
        [JsonConverter(typeof(DateTimeFormatConverter))]
        public DateTime UpdateAt { get; set; }
        
        /// <summary>
        /// ������ID
        /// </summary>
        public int UpdateBy { get; set; }

        /// <summary>
        /// �������ʺ�
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
