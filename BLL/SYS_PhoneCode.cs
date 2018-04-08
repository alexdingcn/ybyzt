using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Hi.BLL
{
    public partial class SYS_PhoneCode
    {
        /// <summary>
        /// 根据用户与手机获取手机验证码实体
        /// </summary>
        /// <param name="username"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Hi.Model.SYS_PhoneCode GetModel(string username, string phone)
        {
            return dal.GetModel(username, phone);
        }

        public Hi.Model.SYS_PhoneCode GetModel(string phone)
        {
            return dal.GetModel(phone);
        }
        public bool Update(Hi.Model.SYS_PhoneCode model, SqlTransaction Tran) {
            return dal.Update(model, Tran);
        }

        public Hi.Model.SYS_PhoneCode GetModel(string module, string Phone, string PhoneCode)
        {
            return dal.GetModel(module, Phone, PhoneCode);
        }


        public List<Hi.Model.SYS_PhoneCode> GetListModel(string module, string Phone)
        {
            return dal.GetListModel(module, Phone);
        }
    }
}
