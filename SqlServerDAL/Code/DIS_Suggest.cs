//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/7/7 11:26:45
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
    /// 数据访问类 DIS_Suggest
    /// </summary>
    public class DIS_Suggest
    {
        public DIS_Suggest()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_Suggest model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_Suggest](");
            strSql.Append("[Title],[DisUserID],[CompID],[CompUserID],[Remark],[CompRemark],[CreateDate],[ReplyDate],[Stype],[ReceiptNo],[Suggest],[ts],[modifyuser],[DisID])");
            strSql.Append(" values (");
            strSql.Append("@Title,@DisUserID,@CompID,@CompUserID,@Remark,@CompRemark,@CreateDate,@ReplyDate,@Stype,@ReceiptNo,@Suggest,@ts,@modifyuser,@DisID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@Title", SqlDbType.VarChar,500),
                    new SqlParameter("@DisUserID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@CompUserID", SqlDbType.Int),
                    new SqlParameter("@Remark", SqlDbType.Text),
                    new SqlParameter("@CompRemark", SqlDbType.Text),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ReplyDate", SqlDbType.DateTime),
                    new SqlParameter("@Stype", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@Suggest", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int)
            };

            if (model.Title != null)
                parameters[0].Value = model.Title;
            else
                parameters[0].Value = DBNull.Value;

            parameters[1].Value = model.DisUserID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.CompUserID;

            if (model.Remark != null)
                parameters[4].Value = model.Remark;
            else
                parameters[4].Value = DBNull.Value;


            if (model.CompRemark != null)
                parameters[5].Value = model.CompRemark;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.CreateDate;

            if (model.ReplyDate != DateTime.MinValue)
                parameters[7].Value = model.ReplyDate;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.Stype;

            if (model.ReceiptNo != null)
                parameters[9].Value = model.ReceiptNo;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.Suggest;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.modifyuser;
            parameters[13].Value = model.DisID;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_Suggest model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Suggest] set ");
            strSql.Append("[Title]=@Title,");
            strSql.Append("[DisUserID]=@DisUserID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[CompUserID]=@CompUserID,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[CompRemark]=@CompRemark,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ReplyDate]=@ReplyDate,");
            strSql.Append("[Stype]=@Stype,");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[Suggest]=@Suggest,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[IsAnswer]=@IsAnswer");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@Title", SqlDbType.VarChar,500),
                    new SqlParameter("@DisUserID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@CompUserID", SqlDbType.Int),
                    new SqlParameter("@Remark", SqlDbType.Text),
                    new SqlParameter("@CompRemark", SqlDbType.Text),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ReplyDate", SqlDbType.DateTime),
                    new SqlParameter("@Stype", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@Suggest", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@IsAnswer", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;

            if (model.Title != null)
                parameters[1].Value = model.Title;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.DisUserID;
            parameters[3].Value = model.CompID;
            parameters[4].Value = model.CompUserID;

            if (model.Remark != null)
                parameters[5].Value = model.Remark;
            else
                parameters[5].Value = DBNull.Value;


            if (model.CompRemark != null)
                parameters[6].Value = model.CompRemark;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CreateDate;

            if (model.ReplyDate != DateTime.MinValue)
                parameters[8].Value = model.ReplyDate;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.Stype;

            if (model.ReceiptNo != null)
                parameters[10].Value = model.ReceiptNo;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.Suggest;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;
            parameters[15].Value = model.DisID;
            parameters[16].Value = model.IsAnswer;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_Suggest] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_Suggest]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_Suggest]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_Suggest GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_Suggest] ");
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
        public IList<Hi.Model.DIS_Suggest> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_Suggest]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_Suggest> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_Suggest> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_Suggest]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_Suggest GetModel(DataRow r)
        {
            Hi.Model.DIS_Suggest model = new Hi.Model.DIS_Suggest();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.Title = SqlHelper.GetString(r["Title"]);
            model.DisUserID = SqlHelper.GetInt(r["DisUserID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.CompUserID = SqlHelper.GetInt(r["CompUserID"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.CompRemark = SqlHelper.GetString(r["CompRemark"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ReplyDate = SqlHelper.GetDateTime(r["ReplyDate"]);
            model.Stype = SqlHelper.GetInt(r["Stype"]);
            model.ReceiptNo = SqlHelper.GetString(r["ReceiptNo"]);
            model.Suggest = SqlHelper.GetInt(r["Suggest"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.IsAnswer = SqlHelper.GetInt(r["IsAnswer"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.DIS_Suggest> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_Suggest>(ds.Tables[0]);
        }
    }
}
