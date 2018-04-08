using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Hi.BLL
{
    public partial class SYS_CompUser
    {
        public int Add(Hi.Model.SYS_CompUser model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }

        /// <summary>
        /// 返回用户账户
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetComUser(string strWhere, string StrWhat = "")
        {
            return dal.GetComUser(strWhere,StrWhat);
        }

        public bool Update(Hi.Model.SYS_CompUser model, SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }

             /// <summary>
        /// 获取用户以及用户明细表数据
        /// </summary>
        public DataTable GetList(int pageSize, int pageIndex, string fldSort, bool sort, string fldName, string TbName, string strCondition, out int pageCount, out int count, bool IsDistinct = false)
        {
            return dal.GetList(pageSize, pageIndex, fldSort, sort, fldName, TbName, strCondition, out pageCount, out count, IsDistinct);
        }
    }
}
