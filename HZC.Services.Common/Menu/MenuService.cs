using HZC.Core;
using HZC.Database;
using System.Collections.Generic;
using System.Linq;

namespace HZC.Common.Services
{
    public class MenuService : BaseService<MenuEntity>
    {
        public List<MenuDto> FetchDto()
        {
            return db.FetchBySql<MenuDto>("SELECT Id,Name,ParentId,Icon,Url,Sort,Roles FROM Base_Menu WHERE IsDel=0").ToList();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Result Save(MenuEntity entity, IAppUser user)
        {
            if (entity.Id == 0)
            {
                return Create(entity, user, () =>
                {
                    MenuUtil.Clear();
                });
            }
            else
            {
                return Update(entity, user, () =>
                {
                    MenuUtil.Clear();
                });
            }
        }

        #region ��д��֤
        protected override string ValidateCreate(MenuEntity entity, IAppUser user)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return "�˵����Ʋ���Ϊ��";
            }

            if (string.IsNullOrWhiteSpace(entity.Icon))
            {
                return "�˵�ͼ�겻��Ϊ��";
            }

            return string.Empty;
        }

        protected override string ValidateDelete(MenuEntity entity, IAppUser user)
        {
            var count = db.GetCount<MenuEntity>(MySearchUtil.New()
                .AndEqual("IsDel", false)
                .AndEqual("ParentId", entity.Id));
            if (count > 0)
            {
                return "�¼��˵���Ϊ�գ���ֹɾ��";
            }

            return string.Empty;
        }

        protected override string ValidateUpdate(MenuEntity entity, IAppUser user)
        {
            return ValidateCreate(entity, user);
        } 
        #endregion
    }
}
