//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/23 13:26:21
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
    /// 数据访问类 BD_Ereceipt
    /// </summary>
    public class BD_Ereceipt
    {
        public BD_Ereceipt()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Ereceipt model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Ereceipt](");
            strSql.Append("[CompID],[ereceipt_rtbill],[ereceipt_kd],[ereceipt_nm],[ereceipt_std],[ereceipt_grd],[ereceipt_unit],[ereceipt_num],[ereceipt_price],[ereceipt_value],[ereceipt_brd],[ereceipt_chkbill],[ereceipt_duedate],[ereceipt_gdsdic],[ereceipt_hder],[ereceipt_gds],[ereceipt_whnm],[ereceipt_whno],[ereceipt_batchno],[ereceipt_sgndt],[ereceipt_mfters],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@ereceipt_rtbill,@ereceipt_kd,@ereceipt_nm,@ereceipt_std,@ereceipt_grd,@ereceipt_unit,@ereceipt_num,@ereceipt_price,@ereceipt_value,@ereceipt_brd,@ereceipt_chkbill,@ereceipt_duedate,@ereceipt_gdsdic,@ereceipt_hder,@ereceipt_gds,@ereceipt_whnm,@ereceipt_whno,@ereceipt_batchno,@ereceipt_sgndt,@ereceipt_mfters,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ereceipt_rtbill", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_kd", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_nm", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_std", SqlDbType.NVarChar,50),
                    new SqlParameter("@ereceipt_grd", SqlDbType.NVarChar,50),
                    new SqlParameter("@ereceipt_unit", SqlDbType.NVarChar,50),
                    new SqlParameter("@ereceipt_num", SqlDbType.Decimal),
                    new SqlParameter("@ereceipt_price", SqlDbType.Decimal),
                    new SqlParameter("@ereceipt_value", SqlDbType.Decimal),
                    new SqlParameter("@ereceipt_brd", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_chkbill", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_duedate", SqlDbType.DateTime),
                    new SqlParameter("@ereceipt_gdsdic", SqlDbType.NVarChar,800),
                    new SqlParameter("@ereceipt_hder", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_gds", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_whnm", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_whno", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_batchno", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_sgndt", SqlDbType.DateTime),
                    new SqlParameter("@ereceipt_mfters", SqlDbType.NVarChar,200),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.ereceipt_rtbill;
            parameters[2].Value = model.ereceipt_kd;
            parameters[3].Value = model.ereceipt_nm;
            parameters[4].Value = model.ereceipt_std;

            if (model.ereceipt_grd != null)
                parameters[5].Value = model.ereceipt_grd;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.ereceipt_unit;
            parameters[7].Value = model.ereceipt_num;
            parameters[8].Value = model.ereceipt_price;
            parameters[9].Value = model.ereceipt_value;

            if (model.ereceipt_brd != null)
                parameters[10].Value = model.ereceipt_brd;
            else
                parameters[10].Value = DBNull.Value;


            if (model.ereceipt_chkbill != null)
                parameters[11].Value = model.ereceipt_chkbill;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.ereceipt_duedate;

            if (model.ereceipt_gdsdic != null)
                parameters[13].Value = model.ereceipt_gdsdic;
            else
                parameters[13].Value = DBNull.Value;


            if (model.ereceipt_hder != null)
                parameters[14].Value = model.ereceipt_hder;
            else
                parameters[14].Value = DBNull.Value;


            if (model.ereceipt_gds != null)
                parameters[15].Value = model.ereceipt_gds;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.ereceipt_whnm;
            parameters[17].Value = model.ereceipt_whno;
            parameters[18].Value = model.ereceipt_batchno;

            if (model.ereceipt_sgndt != DateTime.MinValue)
                parameters[19].Value = model.ereceipt_sgndt;
            else
                parameters[19].Value = DBNull.Value;

            parameters[20].Value = model.ereceipt_mfters;

            if (model.ts != DateTime.MinValue)
                parameters[21].Value = model.ts;
            else
                parameters[21].Value = DBNull.Value;

            parameters[22].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Ereceipt model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Ereceipt] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[ereceipt_rtbill]=@ereceipt_rtbill,");
            strSql.Append("[ereceipt_kd]=@ereceipt_kd,");
            strSql.Append("[ereceipt_nm]=@ereceipt_nm,");
            strSql.Append("[ereceipt_std]=@ereceipt_std,");
            strSql.Append("[ereceipt_grd]=@ereceipt_grd,");
            strSql.Append("[ereceipt_unit]=@ereceipt_unit,");
            strSql.Append("[ereceipt_num]=@ereceipt_num,");
            strSql.Append("[ereceipt_price]=@ereceipt_price,");
            strSql.Append("[ereceipt_value]=@ereceipt_value,");
            strSql.Append("[ereceipt_brd]=@ereceipt_brd,");
            strSql.Append("[ereceipt_chkbill]=@ereceipt_chkbill,");
            strSql.Append("[ereceipt_duedate]=@ereceipt_duedate,");
            strSql.Append("[ereceipt_gdsdic]=@ereceipt_gdsdic,");
            strSql.Append("[ereceipt_hder]=@ereceipt_hder,");
            strSql.Append("[ereceipt_gds]=@ereceipt_gds,");
            strSql.Append("[ereceipt_whnm]=@ereceipt_whnm,");
            strSql.Append("[ereceipt_whno]=@ereceipt_whno,");
            strSql.Append("[ereceipt_batchno]=@ereceipt_batchno,");
            strSql.Append("[ereceipt_sgndt]=@ereceipt_sgndt,");
            strSql.Append("[ereceipt_mfters]=@ereceipt_mfters,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ereceipt_rtbill", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_kd", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_nm", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_std", SqlDbType.NVarChar,50),
                    new SqlParameter("@ereceipt_grd", SqlDbType.NVarChar,50),
                    new SqlParameter("@ereceipt_unit", SqlDbType.NVarChar,50),
                    new SqlParameter("@ereceipt_num", SqlDbType.Decimal),
                    new SqlParameter("@ereceipt_price", SqlDbType.Decimal),
                    new SqlParameter("@ereceipt_value", SqlDbType.Decimal),
                    new SqlParameter("@ereceipt_brd", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_chkbill", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_duedate", SqlDbType.DateTime),
                    new SqlParameter("@ereceipt_gdsdic", SqlDbType.NVarChar,800),
                    new SqlParameter("@ereceipt_hder", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_gds", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_whnm", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_whno", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_batchno", SqlDbType.NVarChar,200),
                    new SqlParameter("@ereceipt_sgndt", SqlDbType.DateTime),
                    new SqlParameter("@ereceipt_mfters", SqlDbType.NVarChar,200),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.ereceipt_rtbill;
            parameters[3].Value = model.ereceipt_kd;
            parameters[4].Value = model.ereceipt_nm;
            parameters[5].Value = model.ereceipt_std;

            if (model.ereceipt_grd != null)
                parameters[6].Value = model.ereceipt_grd;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.ereceipt_unit;
            parameters[8].Value = model.ereceipt_num;
            parameters[9].Value = model.ereceipt_price;
            parameters[10].Value = model.ereceipt_value;

            if (model.ereceipt_brd != null)
                parameters[11].Value = model.ereceipt_brd;
            else
                parameters[11].Value = DBNull.Value;


            if (model.ereceipt_chkbill != null)
                parameters[12].Value = model.ereceipt_chkbill;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.ereceipt_duedate;

            if (model.ereceipt_gdsdic != null)
                parameters[14].Value = model.ereceipt_gdsdic;
            else
                parameters[14].Value = DBNull.Value;


            if (model.ereceipt_hder != null)
                parameters[15].Value = model.ereceipt_hder;
            else
                parameters[15].Value = DBNull.Value;


            if (model.ereceipt_gds != null)
                parameters[16].Value = model.ereceipt_gds;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.ereceipt_whnm;
            parameters[18].Value = model.ereceipt_whno;
            parameters[19].Value = model.ereceipt_batchno;

            if (model.ereceipt_sgndt != DateTime.MinValue)
                parameters[20].Value = model.ereceipt_sgndt;
            else
                parameters[20].Value = DBNull.Value;

            parameters[21].Value = model.ereceipt_mfters;

            if (model.ts != DateTime.MinValue)
                parameters[22].Value = model.ts;
            else
                parameters[22].Value = DBNull.Value;

            parameters[23].Value = model.dr;
            parameters[24].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_Ereceipt] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_Ereceipt]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_Ereceipt]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Ereceipt GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Ereceipt] ");
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
        public IList<Hi.Model.BD_Ereceipt> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Ereceipt]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_Ereceipt> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_Ereceipt> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_Ereceipt]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_Ereceipt GetModel(DataRow r)
        {
            Hi.Model.BD_Ereceipt model = new Hi.Model.BD_Ereceipt();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.ereceipt_rtbill = SqlHelper.GetString(r["ereceipt_rtbill"]);
            model.ereceipt_kd = SqlHelper.GetString(r["ereceipt_kd"]);
            model.ereceipt_nm = SqlHelper.GetString(r["ereceipt_nm"]);
            model.ereceipt_std = SqlHelper.GetString(r["ereceipt_std"]);
            model.ereceipt_grd = SqlHelper.GetString(r["ereceipt_grd"]);
            model.ereceipt_unit = SqlHelper.GetString(r["ereceipt_unit"]);
            model.ereceipt_num = SqlHelper.GetDecimal(r["ereceipt_num"]);
            model.ereceipt_price = SqlHelper.GetDecimal(r["ereceipt_price"]);
            model.ereceipt_value = SqlHelper.GetDecimal(r["ereceipt_value"]);
            model.ereceipt_brd = SqlHelper.GetString(r["ereceipt_brd"]);
            model.ereceipt_chkbill = SqlHelper.GetString(r["ereceipt_chkbill"]);
            model.ereceipt_duedate = SqlHelper.GetDateTime(r["ereceipt_duedate"]);
            model.ereceipt_gdsdic = SqlHelper.GetString(r["ereceipt_gdsdic"]);
            model.ereceipt_hder = SqlHelper.GetString(r["ereceipt_hder"]);
            model.ereceipt_gds = SqlHelper.GetString(r["ereceipt_gds"]);
            model.ereceipt_whnm = SqlHelper.GetString(r["ereceipt_whnm"]);
            model.ereceipt_whno = SqlHelper.GetString(r["ereceipt_whno"]);
            model.ereceipt_batchno = SqlHelper.GetString(r["ereceipt_batchno"]);
            model.ereceipt_sgndt = SqlHelper.GetDateTime(r["ereceipt_sgndt"]);
            model.ereceipt_mfters = SqlHelper.GetString(r["ereceipt_mfters"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_Ereceipt> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_Ereceipt>(ds.Tables[0]);
        }
    }
}
