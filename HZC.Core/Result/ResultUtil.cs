using System.Collections.Generic;

namespace HZC.Core
{
    public class ResultUtil
    {
        #region �����ɹ�
        public static Result Success(string msg = "")
        {
            return new Result { Code = 200, Message = msg };
        }

        public static Result<T> Success<T>(T body, string msg = "")
        {
            return new Result<T>(200, body, msg);
        }
        #endregion

        #region ����ʧ��
        public static Result Fail(string msg = "����ʧ��")
        {
            return new Result { Code = 201, Message = msg };
        }

        public static Result<T> Fail<T>(T body, string msg = "����ʧ��")
        {
            return new Result<T>(201, body, msg);
        }
        #endregion

        #region ��֤ʧ��
        public static Result AuthFail(string msg = "��֤ʧ��")
        {
            return new Result { Code = 204, Message = msg };
        }

        public static Result<T> AuthFail<T>(T body, string msg = "��֤ʧ��")
        {
            return new Result<T>(204, body, msg);
        }
        #endregion

        #region �б�
        public static ListResult<T> List<T>(IEnumerable<T> body, int code = 200, string message = "")
        {
            return new ListResult<T>
            {
                Code = code,
                Message = message,
                Body = body
            };
        }

        public static PageListResult<T> PageList<T>(PageList<T> body, int code = 200, string message = "")
        {
            return new PageListResult<T>
            {
                Code = code,
                Message = message,
                Body = body.Body,
                PageIndex = body.PageIndex,
                PageSize = body.PageSize,
                RecordCount = body.RecordCount
            };
        }
        #endregion
    }
}
