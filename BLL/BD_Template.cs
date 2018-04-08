using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Hi.BLL
{
    public partial class BD_Template
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Template model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Template GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Template model, SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
    }
}
