using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 YZT_CMerchants 
    /// </summary>
    public partial class YZT_CMerchants
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_CMerchants model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_CMerchants model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }


        /// <summary>
        /// 获取  该代理商的  所有有效  招商单
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="Disid"></param>
        /// <param name="pageCount"></param>
        /// <param name="Counts"></param>
        /// <returns></returns>
        public DataTable getDataTable(int pageSize, int pageIndex, string Disid,string strwhere, out int pageCount, out int Counts)
        {
            return dal.getDataTable(pageSize, pageIndex, Disid, strwhere, out pageCount,out Counts);
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }


    }
}
