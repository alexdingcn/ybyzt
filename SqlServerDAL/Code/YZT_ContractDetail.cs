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
    /// 数据访问类 YZT_ContractDetail
    /// </summary>
    public partial class YZT_ContractDetail
    {
        public YZT_ContractDetail()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_ContractDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_ContractDetail](");
            strSql.Append("[ContID],[GoodsID],[GoodsCode],[GoodsName],[ValueInfo],[HtID],[SalePrice],[discount],[TinkerPrice],[target],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser],[FCID],[AreaID])");
            strSql.Append(" values (");
            strSql.Append("@ContID,@GoodsID,@GoodsCode,@GoodsName,@ValueInfo,@HtID,@SalePrice,@discount,@TinkerPrice,@target,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser,@FCID,@AreaID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@ContID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@discount", SqlDbType.Decimal),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@target", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@FCID", SqlDbType.Int),
                    new SqlParameter("@AreaID", SqlDbType.Int)
            };
            parameters[0].Value = model.ContID;
            parameters[1].Value = model.GoodsID;

            if (model.GoodsCode != null)
                parameters[2].Value = model.GoodsCode;
            else
                parameters[2].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[3].Value = model.GoodsName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[4].Value = model.ValueInfo;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.HtID;
            parameters[6].Value = model.SalePrice;
            parameters[7].Value = model.discount;
            parameters[8].Value = model.TinkerPrice;
            parameters[9].Value = model.target;

            if (model.Remark != null)
                parameters[10].Value = model.Remark;
            else
                parameters[10].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[11].Value = model.vdef1;
            else
                parameters[11].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[12].Value = model.vdef2;
            else
                parameters[12].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[13].Value = model.vdef3;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[15].Value = model.CreateDate;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.ts;
            parameters[17].Value = model.modifyuser;
            parameters[18].Value = model.FCID;
            parameters[19].Value = model.AreaID;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_ContractDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_ContractDetail] set ");
            strSql.Append("[ContID]=@ContID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsCode]=@GoodsCode,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[ValueInfo]=@ValueInfo,");
            strSql.Append("[HtID]=@HtID,");
            strSql.Append("[SalePrice]=@SalePrice,");
            strSql.Append("[discount]=@discount,");
            strSql.Append("[TinkerPrice]=@TinkerPrice,");
            strSql.Append("[target]=@target,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[AreaID]=@AreaID");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@ContID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@discount", SqlDbType.Decimal),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@target", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@AreaID", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ContID;
            parameters[2].Value = model.GoodsID;

            if (model.GoodsCode != null)
                parameters[3].Value = model.GoodsCode;
            else
                parameters[3].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[4].Value = model.GoodsName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[5].Value = model.ValueInfo;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.HtID;
            parameters[7].Value = model.SalePrice;
            parameters[8].Value = model.discount;
            parameters[9].Value = model.TinkerPrice;
            parameters[10].Value = model.target;

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
            parameters[18].Value = model.dr;
            parameters[19].Value = model.modifyuser;
            parameters[20].Value = model.AreaID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_ContractDetail] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[YZT_ContractDetail]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [YZT_ContractDetail]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.YZT_ContractDetail GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [YZT_ContractDetail] ");
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
        public IList<Hi.Model.YZT_ContractDetail> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [YZT_ContractDetail]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.YZT_ContractDetail> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.YZT_ContractDetail> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[YZT_ContractDetail]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.YZT_ContractDetail GetModel(DataRow r)
        {
            Hi.Model.YZT_ContractDetail model = new Hi.Model.YZT_ContractDetail();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.ContID = SqlHelper.GetInt(r["ContID"]);
            model.GoodsID = SqlHelper.GetInt(r["GoodsID"]);
            model.GoodsCode = SqlHelper.GetString(r["GoodsCode"]);
            model.GoodsName = SqlHelper.GetString(r["GoodsName"]);
            model.ValueInfo = SqlHelper.GetString(r["ValueInfo"]);
            model.HtID = SqlHelper.GetInt(r["HtID"]);
            model.SalePrice = SqlHelper.GetDecimal(r["SalePrice"]);
            model.discount = SqlHelper.GetDecimal(r["discount"]);
            model.TinkerPrice = SqlHelper.GetDecimal(r["TinkerPrice"]);
            model.target = SqlHelper.GetDecimal(r["target"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.FCID = SqlHelper.GetInt(r["FCID"]);
            model.AreaID = SqlHelper.GetInt(r["AreaID"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.YZT_ContractDetail> GetList(DataSet ds)
        {
            List<Hi.Model.YZT_ContractDetail> l = new List<Hi.Model.YZT_ContractDetail>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                l.Add(GetModel(r));
            }
            return l;
        }
    }
}
