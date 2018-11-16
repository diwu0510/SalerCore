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
        /// 保存
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

        #region 重写验证
        protected override string ValidateCreate(MenuEntity entity, IAppUser user)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return "菜单名称不能为空";
            }

            if (string.IsNullOrWhiteSpace(entity.Icon))
            {
                return "菜单图标不能为空";
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
                return "下级菜单不为空，禁止删除";
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
