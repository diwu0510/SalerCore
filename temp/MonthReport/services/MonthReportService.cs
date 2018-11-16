using HZC.Common.Services;
using HZC.Core;
using HZC.Database;
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Zodo.Saler.Services
{
    public partial class MonthReportService : BaseService<MonthReportEntity>
    {
        #region 重写实体验证
        protected override string ValidateCreate(MonthReportEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateUpdate(MonthReportEntity t, IAppUser user)
        {
            return string.Empty;
        }

        protected override string ValidateDelete(MonthReportEntity t, IAppUser user)
        {
            return string.Empty;
        }
        #endregion
    }
}

