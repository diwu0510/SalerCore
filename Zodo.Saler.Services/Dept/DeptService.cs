using HZC.Core;
using HZC.Database;
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using HZC.Common.Services;

namespace Zodo.Saler.Services
{
    public partial class DeptService : BaseService<DeptEntity>
    {
        #region 重写实体验证
        protected override string ValidateCreate(DeptEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateUpdate(DeptEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateDelete(DeptEntity t, IAppUser user)
        {
            return string.Empty;
        }
        #endregion
    }
}

