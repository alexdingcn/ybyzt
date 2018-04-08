using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;

namespace Hi.BLL
{
    public partial class BD_RebateDetail
    {
        public int Add(Hi.Model.BD_RebateDetail model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }

        public bool Update(Hi.Model.BD_RebateDetail model, SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }

        public List<Hi.Model.BD_RebateDetail> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_RebateDetail>;
        }

        /// <summary>
        /// 获取用户以及用户明细表数据
        /// </summary>
        public DataTable GetList(int pageSize, int pageIndex, string fldSort, bool sort, string fldName, string TbName, string strCondition, out int pageCount, out int count, bool IsDistinct = false, string CustomFldName="")
        {
            return dal.GetList(pageSize, pageIndex, fldSort, sort, fldName, TbName, strCondition, out pageCount, out count, IsDistinct, CustomFldName);
        }

    }
}
