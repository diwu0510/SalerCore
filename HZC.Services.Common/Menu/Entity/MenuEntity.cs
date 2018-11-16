using HZC.Core;

namespace HZC.Common.Services
{
    [MyDataTable("Base_Menu")]
    public class MenuEntity : BaseEntity
    {
        /// <summary>
        /// �˵���ʾ������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �˵�ͼ��
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// �˵�����
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// �ϼ��˵�
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// ��Щ��ɫ�ɼ�
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// �Ƿ�ɾ��
        /// </summary>
        public bool IsDel { get; set; } = false;
    }
}
