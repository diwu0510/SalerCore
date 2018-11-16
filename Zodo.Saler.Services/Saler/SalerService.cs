using HZC.Common.Services;
using HZC.Core;
using System.Collections.Generic;

namespace Zodo.Saler.Services
{
    public partial class SalerService : BaseService<SalerEntity>
    {
        #region 获取指定部门的所有销售
        /// <summary>
        /// 获取指定部门的所有销售
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public IEnumerable<SalerEntity> GetByDept(int? deptId)
        {
            SalerSearchParam param = new SalerSearchParam();
            param.DeptId = deptId.Value;
            return Fetch(param.ToSearchUtil());
        } 
        #endregion

        #region 重写实体验证
        protected override string ValidateCreate(SalerEntity t, IAppUser user)
        {
            if (string.IsNullOrWhiteSpace(t.Name))
            {
                return "业务员姓名不能为空";
            }

            if (t.DeptId == 0)
            {
                return "必须指定业务员所在部门";
            }

            if (string.IsNullOrWhiteSpace(t.Job))
            {
                return "业务员职位不能为空";
            }

            return string.Empty;
        }

        protected override string ValidateUpdate(SalerEntity t, IAppUser user)
        {
            return ValidateCreate(t, user);
        }

        protected override string ValidateDelete(SalerEntity t, IAppUser user)
        {
            return string.Empty;
        }
        #endregion
    }
}

