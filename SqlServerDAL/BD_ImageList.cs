using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;


namespace Hi.SQLServerDAL
{
    public partial class BD_ImageList
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Adds(Hi.Model.BD_ImageList model)
        {
            string strSql = string.Format("insert into [BD_ImageList]([GoodsID],[CompID],[Pic],[Pic2],[Pic3],[IsIndex],[CreateUserID],[CreateDate],[ts],[modifyuser],[dr])values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',0);select @@Identity", model.GoodsID, model.CompID, model.Pic, model.Pic2, model.Pic3, model.IsIndex, model.CreateUserID, model.CreateDate, model.ts, model.modifyuser);
            return strSql.ToString();
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_ImageList model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_ImageList](");
            strSql.Append("[CompID],[GoodsID],[Pic],[Pic2],[Pic3],[IsIndex],[CreateUserID],[CreateDate],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsID,@Pic,@Pic2,@Pic3,@IsIndex,@CreateUserID,@CreateDate,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@Pic", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic2", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic3", SqlDbType.VarChar,128),
                    new SqlParameter("@IsIndex", SqlDbType.SmallInt),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsID;

            if (model.Pic != null)
                parameters[2].Value = model.Pic;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Pic2 != null)
                parameters[3].Value = model.Pic2;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Pic3 != null)
                parameters[4].Value = model.Pic3;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.IsIndex;
            parameters[6].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[7].Value = model.CreateDate;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[8].Value = model.ts;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.dr;
            parameters[10].Value = model.modifyuser;
            // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

        }
        /// <summary>
        /// 删除某商品全部标签
        /// </summary>
        /// <returns></returns>
        public int Delete(int goodsId, int compId, SqlTransaction Tran)
        {
            string sql = "delete BD_ImageList where goodsId=@goodsId  and compId=@compId";
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
    }
}
