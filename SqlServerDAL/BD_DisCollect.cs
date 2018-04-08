using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data;

namespace Hi.SQLServerDAL
{
    public partial class BD_DisCollect
    {
        public string GetGoodsIDs(int DisID)
        {
            string sqlstr = "select GoodsID from BD_DisCollect where DisID=" + DisID + " and dr=0";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);
            string disid = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    disid += ds.Tables[0].Rows[i][0] + ",";
                }
            }
            return disid.TrimEnd(',');
        }

        public bool delete(int disid, int goodsid)
        {
            string sqlstr = string.Format("delete BD_DisCollect where disid={0} and goodsid={1}", disid, goodsid);
            if (SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, sqlstr) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
