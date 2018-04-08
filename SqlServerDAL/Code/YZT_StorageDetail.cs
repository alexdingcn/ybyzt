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
    /// 数据访问类 YZT_StorageDetail
    /// </summary>
    public partial class YZT_StorageDetail
    {
        public YZT_StorageDetail()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_StorageDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_StorageDetail](");
            strSql.Append("[CompID],[DisID],[StorageID],[GoodsID],[GoodsCode],[GoodsName],[ValueInfo],[Unit],[BatchNO],[validDate],[StorageNum],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser],[OutID],[OutDID])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@StorageID,@GoodsID,@GoodsCode,@GoodsName,@ValueInfo,@Unit,@BatchNO,@validDate,@StorageNum,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser,@OutID,@OutDID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@StorageID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@Unit", SqlDbType.VarChar,30),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@StorageNum", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@OutID", SqlDbType.Int),
                    new SqlParameter("@OutDID", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.StorageID;
            parameters[3].Value = model.GoodsID;

            if (model.GoodsCode != null)
                parameters[4].Value = model.GoodsCode;
            else
                parameters[4].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[5].Value = model.GoodsName;
            else
                parameters[5].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[6].Value = model.ValueInfo;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[7].Value = model.Unit;
            else
                parameters[7].Value = DBNull.Value;


            if (model.BatchNO != null)
                parameters[8].Value = model.BatchNO;
            else
                parameters[8].Value = DBNull.Value;


            if (model.validDate != DateTime.MinValue)
                parameters[9].Value = model.validDate;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.StorageNum;

            if (model.Remark != null)
                parameters[11].Value = model.Remark;
            else
                parameters[11].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[12].Value = model.vdef1;
            else
                parameters[12].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[13].Value = model.vdef2;
            else
                parameters[13].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[14].Value = model.vdef3;
            else
                parameters[14].Value = DBNull.Value;

            parameters[15].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[16].Value = model.CreateDate;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.ts;
            parameters[18].Value = model.modifyuser;
            parameters[19].Value = model.Outid;
            parameters[20].Value = model.Outdid;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_StorageDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_StorageDetail] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[StorageID]=@StorageID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsCode]=@GoodsCode,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[ValueInfo]=@ValueInfo,");
            strSql.Append("[Unit]=@Unit,");
            strSql.Append("[BatchNO]=@BatchNO,");
            strSql.Append("[validDate]=@validDate,");
            strSql.Append("[StorageNum]=@StorageNum,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[OutID]=@OutID,");
            strSql.Append("[OutDID]=@OutDID");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@StorageID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@Unit", SqlDbType.VarChar,30),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@StorageNum", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@OutID", SqlDbType.Int),
                    new SqlParameter("@OutDID", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.StorageID;
            parameters[4].Value = model.GoodsID;

            if (model.GoodsCode != null)
                parameters[5].Value = model.GoodsCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[6].Value = model.GoodsName;
            else
                parameters[6].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[7].Value = model.ValueInfo;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[8].Value = model.Unit;
            else
                parameters[8].Value = DBNull.Value;


            if (model.BatchNO != null)
                parameters[9].Value = model.BatchNO;
            else
                parameters[9].Value = DBNull.Value;


            if (model.validDate != DateTime.MinValue)
                parameters[10].Value = model.validDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.StorageNum;

            if (model.Remark != null)
                parameters[12].Value = model.Remark;
            else
                parameters[12].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[13].Value = model.vdef1;
            else
                parameters[13].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[14].Value = model.vdef2;
            else
                parameters[14].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[15].Value = model.vdef3;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[17].Value = model.CreateDate;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.ts;
            parameters[19].Value = model.dr;
            parameters[20].Value = model.modifyuser;
            parameters[21].Value = model.Outid;
            parameters[22].Value = model.Outdid;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_StorageDetail] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[YZT_StorageDetail]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [YZT_StorageDetail]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.YZT_StorageDetail GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [YZT_StorageDetail] ");
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
        public IList<Hi.Model.YZT_StorageDetail> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [YZT_StorageDetail]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.YZT_StorageDetail> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.YZT_StorageDetail> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[YZT_StorageDetail]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.YZT_StorageDetail GetModel(DataRow r)
        {
            Hi.Model.YZT_StorageDetail model = new Hi.Model.YZT_StorageDetail();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.StorageID = SqlHelper.GetInt(r["StorageID"]);
            model.GoodsID = SqlHelper.GetInt(r["GoodsID"]);
            model.GoodsCode = SqlHelper.GetString(r["GoodsCode"]);
            model.GoodsName = SqlHelper.GetString(r["GoodsName"]);
            model.ValueInfo = SqlHelper.GetString(r["ValueInfo"]);
            model.Unit = SqlHelper.GetString(r["Unit"]);
            model.BatchNO = SqlHelper.GetString(r["BatchNO"]);
            model.validDate = SqlHelper.GetDateTime(r["validDate"]);
            model.StorageNum = SqlHelper.GetDecimal(r["StorageNum"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.YZT_StorageDetail> GetList(DataSet ds)
        {
            List<Hi.Model.YZT_StorageDetail> l = new List<Hi.Model.YZT_StorageDetail>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                l.Add(GetModel(r));
            }
            return l;
        }
    }
}
