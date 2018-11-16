using HZC.Common.Services;
using HZC.Core;
using HZC.Database;
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Zodo.Saler.Services
{
    public partial class DeptSalerService : BaseService<DeptSalerEntity>
    {
        #region 重写实体验证
        protected override string ValidateCreate(DeptSalerEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateUpdate(DeptSalerEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateDelete(DeptSalerEntity t, IAppUser user)
        {
            return string.Empty;
        }
        #endregion
    }
}

