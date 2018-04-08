//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/12/23 16:31:42
//
// 功能描述: 
//
// 修改标识: 
// 修改描述: 
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Collections;
using System.Collections.Generic;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 YZT_Payment
    /// </summary>
    public partial class YZT_Payment
    {
        public YZT_Payment()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Payment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_Payment](");
            strSql.Append("[CompID],[DisID],[PaymentNO],[PaymentDate],[HtID],[IState],[PaymentAmount],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@PaymentNO,@PaymentDate,@HtID,@IState,@PaymentAmount,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@PaymentNO", SqlDbType.VarChar,50),
                    new SqlParameter("@PaymentDate", SqlDbType.DateTime),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@IState", SqlDbType.Int),
                    new SqlParameter("@PaymentAmount", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;

            if (model.PaymentNO != null)
                parameters[2].Value = model.PaymentNO;
            else
                parameters[2].Value = DBNull.Value;


            if (model.PaymentDate != DateTime.MinValue)
                parameters[3].Value = model.PaymentDate;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.HtID;
            parameters[5].Value = model.IState;
            parameters[6].Value = model.PaymentAmount;

            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[8].Value = model.vdef1;
            else
                parameters[8].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[9].Value = model.vdef2;
            else
                parameters[9].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[10].Value = model.vdef3;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[12].Value = model.CreateDate;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.ts;
            parameters[14].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Payment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_Payment] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[PaymentNO]=@PaymentNO,");
            strSql.Append("[PaymentDate]=@PaymentDate,");
            strSql.Append("[HtID]=@HtID,");
            strSql.Append("[IState]=@IState,");
            strSql.Append("[PaymentAmount]=@PaymentAmount,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@PaymentNO", SqlDbType.VarChar,50),
                    new SqlParameter("@PaymentDate", SqlDbType.DateTime),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@IState", SqlDbType.Int),
                    new SqlParameter("@PaymentAmount", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;

            if (model.PaymentNO != null)
                parameters[3].Value = model.PaymentNO;
            else
                parameters[3].Value = DBNull.Value;


            if (model.PaymentDate != DateTime.MinValue)
                parameters[4].Value = model.PaymentDate;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.HtID;
            parameters[6].Value = model.IState;
            parameters[7].Value = model.PaymentAmount;

            if (model.Remark != null)
                parameters[8].Value = model.Remark;
            else
                parameters[8].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[9].Value = model.vdef1;
            else
                parameters[9].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[10].Value = model.vdef2;
            else
                parameters[10].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[11].Value = model.vdef3;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[13].Value = model.CreateDate;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.ts;
            parameters[15].Value = model.dr;
            parameters[16].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_Payment] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[YZT_Payment]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [YZT_Payment]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.YZT_Payment GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [YZT_Payment] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow r = ds.Tables[0].Rows[0];
                return GetModel(r);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取数据集,建议只在多表联查时使用
        /// </summary>
        public DataSet GetDataSet(string strSql)
        {
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql);
        }

        /// <summary>
        /// 获取泛型数据列表,建议只在多表联查时使用
        /// </summary>
        public IList<Hi.Model.YZT_Payment> GetList(string strSql)
        {
            return GetList(GetDataSet(strSql));
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [YZT_Payment]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.YZT_Payment> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.YZT_Payment> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[YZT_Payment]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.YZT_Payment GetModel(DataRow r)
        {
            Hi.Model.YZT_Payment model = new Hi.Model.YZT_Payment();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.PaymentNO = SqlHelper.GetString(r["PaymentNO"]);
            model.PaymentDate = SqlHelper.GetDateTime(r["PaymentDate"]);
            model.HtID = SqlHelper.GetInt(r["HtID"]);
            model.IState = SqlHelper.GetInt(r["IState"]);
            model.PaymentAmount = SqlHelper.GetDecimal(r["PaymentAmount"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]); 
            model.hospital = SqlHelper.GetString(r["hospital"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.YZT_Payment> GetList(DataSet ds)
        {
            List<Hi.Model.YZT_Payment> l = new List<Hi.Model.YZT_Payment>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                l.Add(GetModel(r));
            }
            return l;
        }
    }
}
