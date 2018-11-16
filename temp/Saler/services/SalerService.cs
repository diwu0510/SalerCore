using HZC.Common.Services;
using HZC.Core;
using HZC.Database;
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Zodo.Saler.Services
{
    public partial class SalerService : BaseService<SalerEntity>
    {
        #region 重写实体验证
        protected override string ValidateCreate(SalerEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateUpdate(SalerEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateDelete(SalerEntity t, IAppUser user)
        {
            return string.Empty;
        }
        #endregion
    }
}

