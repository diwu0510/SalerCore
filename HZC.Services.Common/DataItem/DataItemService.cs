using HZC.Core;
using HZC.Database;

namespace HZC.Common.Services
{
    public class DataItemService : BaseService<DataItemEntity>
    {
        /// <summary>
        /// 设置数据字典，如果键已存在则更新该记录，键不存在则新建记录
        /// </summary>
        /// <param name="k">键</param>
        /// <param name="v">值</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        public Result Set(string k, string v, IAppUser user)
        {
            var entity = db.Load<DataItemEntity>(MySearchUtil.New()
                .AndEqual("IsDel", false)
                .AndEqual("K", k.Trim()));

            if (entity == null)
            {
                entity = new DataItemEntity
                {
                    K = k.Trim(),
                    V = v.Trim()
                };
                return Create(entity, user, ()=>
                {
                    DataItemUtil.Clear();
                });
            }
            else
            {
                entity.V = v;
                return Update(entity, user);
            }
        }

        #region 重写实体验证
        protected override string ValidateCreate(DataItemEntity entity, IAppUser user)
        {
            if (user.Name != "admin")
            {
                return "仅管理员可创建字典项";
            }

            if (string.IsNullOrWhiteSpace(entity.K))
            {
                return "字典键不能为空";
            }

            if (string.IsNullOrWhiteSpace(entity.V))
            {
                return "字典值不能为空";
            }

            var count = db.GetCount<DataItemEntity>(MySearchUtil.New()
                .AndEqual("K", entity.K.Trim())
                .AndEqual("IsDel", false));
            if (count > 0)
            {
                return "该键已存在";
            }
            return base.ValidateCreate(entity, user);
        }

        protected override string ValidateDelete(DataItemEntity entity, IAppUser user)
        {
            if (!entity.Editable && user.Name != "admin")
            {
                return "该项目不允许编辑";
            }

            if (string.IsNullOrWhiteSpace(entity.K))
            {
                return "字典键不能为空";
            }

            if (string.IsNullOrWhiteSpace(entity.V))
            {
                return "字典值不能为空";
            }

            var count = db.GetCount<DataItemEntity>(MySearchUtil.New()
                .AndEqual("K", entity.K.Trim())
                .AndEqual("IsDel", false)
                .AndNotEqual("Id", entity.Id));
            if (count > 0)
            {
                return "该键已存在";
            }

            return string.Empty;
        } 
        #endregion
    }
}
