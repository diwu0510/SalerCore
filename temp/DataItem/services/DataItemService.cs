using HZC.Core;
using HZC.Database;
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Zodo.Saler.Services
{
    public partial class DataItemService : BaseService<DataItemEntity>
    {
        #region 重写实体验证
        protected override string ValidateCreate(DataItemEntity t)
        {
            return string.Empty;
        }

        protected override string ValidateUpdate(DataItemEntity t)
        {
            return string.Empty;
        }

        protected override string ValidateDelete(DataItemEntity t)
        {
            return string.Empty;
        }
        #endregion
    }
}

