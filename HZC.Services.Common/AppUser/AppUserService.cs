using HZC.Core;
using HZC.Database;

namespace HZC.Common.Services
{
    public class AppUserService : BaseService<AppUserEntity>
    {
        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name">用户账号</param>
        /// <param name="pw">密码</param>
        /// <returns></returns>
        public Result<AppUserEntity> Login(string name, string pw)
        {
            if (string.IsNullOrWhiteSpace(name.Trim()) || string.IsNullOrWhiteSpace(pw.Trim()))
            {
                return ResultUtil.Fail<AppUserEntity>(null, "用户名或密码不能为空");
            }
            var entity = db.Load<AppUserEntity>(MySearchUtil.New()
                .AndEqual("IsDel", false)
                .AndEqual("Name", name));
            if (entity == null || AESEncryptor.Decrypt(entity.Pw) != pw)
            {
                return ResultUtil.Fail<AppUserEntity>(null, "用户名密码错误");
            }
            else
            {
                return ResultUtil.Success<AppUserEntity>(entity);
            }
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="oldPw">原始密码</param>
        /// <param name="newPw">新密码</param>
        /// <returns></returns>
        public Result ChangePw(int id, string oldPw, string newPw)
        {
            var account = Load(id);
            if (account == null || account.IsDel)
            {
                return ResultUtil.Fail("请求的数据不存在");
            }
            if (AESEncryptor.Decrypt(account.Pw) != oldPw)
            {
                return ResultUtil.Fail("原始密码错误");
            }
            else
            {
                string sql = "UPDATE [Base_User] SET Pw=@Pw WHERE Id=@Id";
                var row = db.Execute(sql, new { Id = id, Pw = AESEncryptor.Encrypt(newPw) });

                return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
            }
        }
        #endregion

        #region 重置密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result ResetPw(int id)
        {
            var entity = Load(id);
            if (entity == null || entity.IsDel)
            {
                return ResultUtil.Fail("用户不存在或已被删除");
            }
            string sql = "UPDATE [Base_User] SET Pw=@Pw WHERE Id=@Id";
            var row = db.Execute(sql, new { Id = id, Pw = AESEncryptor.Encrypt("123456") });
            return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
        }
        #endregion

        #region 重写验证
        protected override string ValidateCreate(AppUserEntity entity, IAppUser user)
        {
            if (entity.Name.ToLower() == "admin")
            {
                return "用户名不能为admin";
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return "用户名不能为空";
            }

            if (string.IsNullOrWhiteSpace(entity.Role))
            {
                return "用户角色不能为空";
            }

            var count = db.GetCount(MySearchUtil.New()
                .AndEqual("Name", entity.Name)
                .AndEqual("IsDel", false), "Base_User");

            if (count > 0)
            {
                return "当前用户名已经存在";
            }

            return string.Empty;
        }

        protected override string ValidateUpdate(AppUserEntity entity, IAppUser user)
        {
            if (entity.Name.ToLower() == "admin")
            {
                return "用户名不能为admin";
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return "用户名不能为空";
            }

            if (string.IsNullOrWhiteSpace(entity.Role))
            {
                return "用户角色不能为空";
            }

            var count = db.GetCount(MySearchUtil.New()
                .AndEqual("Name", entity.Name)
                .AndEqual("IsDel", false)
                .AndNotEqual("Id", entity.Id), "Base_User");

            if (count > 0)
            {
                return "当前用户名已经存在";
            }

            return string.Empty;
        } 
        #endregion
    }
}
