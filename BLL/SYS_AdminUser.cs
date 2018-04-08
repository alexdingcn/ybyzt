using System;
using System.Collections.Generic;
using System.Text;

namespace Hi.BLL
{
    public partial class SYS_AdminUser
    {
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.SYS_AdminUser GetModelByName(string LoginId)
        {
            return dal.GetModelByName(LoginId);

        }
        /// <summary>
        /// 修改密码
        /// <param name="Id">用户ID</param>
        /// <param name="NewPassWord">MD5加密过的新的密码</param>
        /// </summary>
        public bool UpdatePassWord(string NewPassWord, string Id)
        {
            return dal.UpdatePassWord(NewPassWord, Id);

        }
    }
}
