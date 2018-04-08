using System;
using System.Collections.Generic;
using System.Text;


namespace Hi.BLL
{
    public partial class BD_DefDoc_B
    {
        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Updates(string user, List<int> l, string compid)
        {
            foreach (int ID in l)
            {
                 dal.Updates(user, ID.ToString(), compid);
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DefDoc_B model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
    }
}
