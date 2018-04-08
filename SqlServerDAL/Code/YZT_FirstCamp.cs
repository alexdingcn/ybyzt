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
    /// 数据访问类 YZT_FirstCamp
    /// </summary>
    public partial class YZT_FirstCamp
    {
        public YZT_FirstCamp()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_FirstCamp model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_FirstCamp](");
            strSql.Append("[CMID],[CompID],[DisID],[HtID],[State],[ForceDate],[InvalidDate],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser],[ApplyRemark],[AreaID])");
            strSql.Append(" values (");
            strSql.Append("@CMID,@CompID,@DisID,@HtID,@State,@ForceDate,@InvalidDate,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser,@ApplyRemark,@AreaID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CMID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@ForceDate", SqlDbType.DateTime),
                    new SqlParameter("@InvalidDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@ApplyRemark", SqlDbType.VarChar),
                    new SqlParameter("@AreaID", SqlDbType.Int)
            };
            parameters[0].Value = model.CMID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.HtID;
            parameters[4].Value = model.State;

            if (model.ForceDate != DateTime.MinValue)
                parameters[5].Value = model.ForceDate;
            else
                parameters[5].Value = DBNull.Value;


            if (model.InvalidDate != DateTime.MinValue)
                parameters[6].Value = model.InvalidDate;
            else
                parameters[6].Value = DBNull.Value;


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
            parameters[15].Value = model.Applyremark;
            parameters[16].Value = model.AreaID;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_FirstCamp model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_FirstCamp] set ");
            strSql.Append("[CMID]=@CMID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[HtID]=@HtID,");
            strSql.Append("[State]=@State,");
            strSql.Append("[ForceDate]=@ForceDate,");
            strSql.Append("[InvalidDate]=@InvalidDate,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[ApplyRemark]=@Applyremark,");
            strSql.Append("[AreaID]=@AreaID");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CMID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@ForceDate", SqlDbType.DateTime),
                    new SqlParameter("@InvalidDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@ApplyRemark", SqlDbType.VarChar),
                    new SqlParameter("@AreaID", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CMID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.DisID;
            parameters[4].Value = model.HtID;
            parameters[5].Value = model.State;

            if (model.ForceDate != DateTime.MinValue)
                parameters[6].Value = model.ForceDate;
            else
                parameters[6].Value = DBNull.Value;


            if (model.InvalidDate != DateTime.MinValue)
                parameters[7].Value = model.InvalidDate;
            else
                parameters[7].Value = DBNull.Value;


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
            parameters[17].Value = model.Applyremark;
            parameters[18].Value = model.AreaID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_FirstCamp] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[YZT_FirstCamp]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [YZT_FirstCamp]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.YZT_FirstCamp GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [YZT_FirstCamp] ");
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
        public IList<Hi.Model.YZT_FirstCamp> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [YZT_FirstCamp]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.YZT_FirstCamp> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.YZT_FirstCamp> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[YZT_FirstCamp]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.YZT_FirstCamp GetModel(DataRow r)
        {
            Hi.Model.YZT_FirstCamp model = new Hi.Model.YZT_FirstCamp();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CMID = SqlHelper.GetInt(r["CMID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.HtID = SqlHelper.GetInt(r["HtID"]);
            model.State = SqlHelper.GetInt(r["State"]);
            model.ForceDate = SqlHelper.GetDateTime(r["ForceDate"]);
            model.InvalidDate = SqlHelper.GetDateTime(r["InvalidDate"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.Applyremark = SqlHelper.GetString(r["ApplyRemark"]);
            model.AreaID = SqlHelper.GetInt(r["AreaID"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.YZT_FirstCamp> GetList(DataSet ds)
        {
            List<Hi.Model.YZT_FirstCamp> l = new List<Hi.Model.YZT_FirstCamp>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                l.Add(GetModel(r));
            }
            return l;
        }
    }
}
