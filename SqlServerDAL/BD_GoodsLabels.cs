using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data.SqlClient;
using System.Data;

namespace Hi.SQLServerDAL
{
    public partial class BD_GoodsLabels
    {
        /// <summary>
        /// 删除某商品全部标签
        /// </summary>
        /// <returns></returns>
        public int Delete(int goodsId, int compId, SqlTransaction Tran)
        {
            string sql = "delete BD_GoodsLabels where goodsId=@goodsId  and compId=@compId";
            SqlParameter[] parameters = {
                    new SqlParameter("@goodsId", SqlDbType.BigInt),
                          new SqlParameter("@compId", SqlDbType.Int)
            };
            parameters[0].Value = goodsId;
            parameters[1].Value = compId;
            if (Tran != null)
            {
                return SqlHelper.ExecuteSql(sql.ToString(), Tran, parameters);
            }
            else
            {
                return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, sql);
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsLabels model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_GoodsLabels](");
            strSql.Append("[CompID],[GoodsID],[LabelName],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsID,@LabelName,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@LabelName", SqlDbType.NVarChar,200),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsID;

            if (model.LabelName != null)
                parameters[2].Value = model.LabelName;
            else
                parameters[2].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[3].Value = model.ts;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.dr;
            parameters[5].Value = model.modifyuser;
            // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));


        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Adds(Hi.Model.BD_GoodsLabels model)
        {
            string strSql = string.Format("insert into [BD_GoodsLabels]([GoodsID],[CompID],[LabelName],[ts],[dr],[modifyuser])values('{0}','{1}','{2}','{3}','{4}','{5}');select @@Identity", model.GoodsID, model.CompID, model.LabelName, model.ts, model.dr, model.modifyuser);
            return strSql.ToString();
        }

    }
}
