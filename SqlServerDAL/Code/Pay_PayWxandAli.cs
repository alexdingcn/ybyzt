//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/9/18 10:27:08
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
    /// 数据访问类 Pay_PayWxandAli
    /// </summary>
    public partial class Pay_PayWxandAli
    {
        public Pay_PayWxandAli()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.Pay_PayWxandAli model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [Pay_PayWxandAli](");
            strSql.Append("[CompID],[wx_appid],[wx_mchid],[wx_key],[wx_appsechet],[wx_Isno],[wx_vdef2],[wx_vdef3],[ali_partner],[ali_key],[ali_seller_email],[ali_isno],[ali_vdef2],[ali_vdef3],[ali_RSAkey])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@wx_appid,@wx_mchid,@wx_key,@wx_appsechet,@wx_Isno,@wx_vdef2,@wx_vdef3,@ali_partner,@ali_key,@ali_seller_email,@ali_isno,@ali_vdef2,@ali_vdef3,@ali_RSAkey)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.BigInt),
                    new SqlParameter("@wx_appid", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_mchid", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_key", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_appsechet", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_Isno", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_vdef2", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_vdef3", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_partner", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_key", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_seller_email", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_isno", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_vdef2", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_vdef3", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_RSAkey", SqlDbType.NVarChar,300)
            };
            parameters[0].Value = model.CompID;

            if (model.wx_appid != null)
                parameters[1].Value = model.wx_appid;
            else
                parameters[1].Value = DBNull.Value;


            if (model.wx_mchid != null)
                parameters[2].Value = model.wx_mchid;
            else
                parameters[2].Value = DBNull.Value;


            if (model.wx_key != null)
                parameters[3].Value = model.wx_key;
            else
                parameters[3].Value = DBNull.Value;


            if (model.wx_appsechet != null)
                parameters[4].Value = model.wx_appsechet;
            else
                parameters[4].Value = DBNull.Value;


            if (model.wx_Isno != null)
                parameters[5].Value = model.wx_Isno;
            else
                parameters[5].Value = DBNull.Value;


            if (model.wx_vdef2 != null)
                parameters[6].Value = model.wx_vdef2;
            else
                parameters[6].Value = DBNull.Value;


            if (model.wx_vdef3 != null)
                parameters[7].Value = model.wx_vdef3;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ali_partner != null)
                parameters[8].Value = model.ali_partner;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ali_key != null)
                parameters[9].Value = model.ali_key;
            else
                parameters[9].Value = DBNull.Value;


            if (model.ali_seller_email != null)
                parameters[10].Value = model.ali_seller_email;
            else
                parameters[10].Value = DBNull.Value;


            if (model.ali_isno != null)
                parameters[11].Value = model.ali_isno;
            else
                parameters[11].Value = DBNull.Value;


            if (model.ali_vdef2 != null)
                parameters[12].Value = model.ali_vdef2;
            else
                parameters[12].Value = DBNull.Value;


            if (model.ali_vdef3 != null)
                parameters[13].Value = model.ali_vdef3;
            else
                parameters[13].Value = DBNull.Value;


            if (model.ali_RSAkey != null)
                parameters[14].Value = model.ali_RSAkey;
            else
                parameters[14].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.Pay_PayWxandAli model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [Pay_PayWxandAli] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[wx_appid]=@wx_appid,");
            strSql.Append("[wx_mchid]=@wx_mchid,");
            strSql.Append("[wx_key]=@wx_key,");
            strSql.Append("[wx_appsechet]=@wx_appsechet,");
            strSql.Append("[wx_Isno]=@wx_Isno,");
            strSql.Append("[wx_vdef2]=@wx_vdef2,");
            strSql.Append("[wx_vdef3]=@wx_vdef3,");
            strSql.Append("[ali_partner]=@ali_partner,");
            strSql.Append("[ali_key]=@ali_key,");
            strSql.Append("[ali_seller_email]=@ali_seller_email,");
            strSql.Append("[ali_isno]=@ali_isno,");
            strSql.Append("[ali_vdef2]=@ali_vdef2,");
            strSql.Append("[ali_vdef3]=@ali_vdef3,");
            strSql.Append("[ali_RSAkey]=@ali_RSAkey");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.BigInt),
                    new SqlParameter("@wx_appid", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_mchid", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_key", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_appsechet", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_Isno", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_vdef2", SqlDbType.NVarChar,50),
                    new SqlParameter("@wx_vdef3", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_partner", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_key", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_seller_email", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_isno", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_vdef2", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_vdef3", SqlDbType.NVarChar,50),
                    new SqlParameter("@ali_RSAkey", SqlDbType.NVarChar,300)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.wx_appid != null)
                parameters[2].Value = model.wx_appid;
            else
                parameters[2].Value = DBNull.Value;


            if (model.wx_mchid != null)
                parameters[3].Value = model.wx_mchid;
            else
                parameters[3].Value = DBNull.Value;


            if (model.wx_key != null)
                parameters[4].Value = model.wx_key;
            else
                parameters[4].Value = DBNull.Value;


            if (model.wx_appsechet != null)
                parameters[5].Value = model.wx_appsechet;
            else
                parameters[5].Value = DBNull.Value;


            if (model.wx_Isno != null)
                parameters[6].Value = model.wx_Isno;
            else
                parameters[6].Value = DBNull.Value;


            if (model.wx_vdef2 != null)
                parameters[7].Value = model.wx_vdef2;
            else
                parameters[7].Value = DBNull.Value;


            if (model.wx_vdef3 != null)
                parameters[8].Value = model.wx_vdef3;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ali_partner != null)
                parameters[9].Value = model.ali_partner;
            else
                parameters[9].Value = DBNull.Value;


            if (model.ali_key != null)
                parameters[10].Value = model.ali_key;
            else
                parameters[10].Value = DBNull.Value;


            if (model.ali_seller_email != null)
                parameters[11].Value = model.ali_seller_email;
            else
                parameters[11].Value = DBNull.Value;


            if (model.ali_isno != null)
                parameters[12].Value = model.ali_isno;
            else
                parameters[12].Value = DBNull.Value;


            if (model.ali_vdef2 != null)
                parameters[13].Value = model.ali_vdef2;
            else
                parameters[13].Value = DBNull.Value;


            if (model.ali_vdef3 != null)
                parameters[14].Value = model.ali_vdef3;
            else
                parameters[14].Value = DBNull.Value;


            if (model.ali_RSAkey != null)
                parameters[15].Value = model.ali_RSAkey;
            else
                parameters[15].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [Pay_PayWxandAli] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[Pay_PayWxandAli]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [Pay_PayWxandAli]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.Pay_PayWxandAli GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [Pay_PayWxandAli] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
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
        public IList<Hi.Model.Pay_PayWxandAli> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [Pay_PayWxandAli]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.Pay_PayWxandAli> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.Pay_PayWxandAli> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[Pay_PayWxandAli]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.Pay_PayWxandAli GetModel(DataRow r)
        {
            Hi.Model.Pay_PayWxandAli model = new Hi.Model.Pay_PayWxandAli();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.wx_appid = SqlHelper.GetString(r["wx_appid"]);
            model.wx_mchid = SqlHelper.GetString(r["wx_mchid"]);
            model.wx_key = SqlHelper.GetString(r["wx_key"]);
            model.wx_appsechet = SqlHelper.GetString(r["wx_appsechet"]);
            model.wx_Isno = SqlHelper.GetString(r["wx_Isno"]);
            model.wx_vdef2 = SqlHelper.GetString(r["wx_vdef2"]);
            model.wx_vdef3 = SqlHelper.GetString(r["wx_vdef3"]);
            model.ali_partner = SqlHelper.GetString(r["ali_partner"]);
            model.ali_key = SqlHelper.GetString(r["ali_key"]);
            model.ali_seller_email = SqlHelper.GetString(r["ali_seller_email"]);
            model.ali_isno = SqlHelper.GetString(r["ali_isno"]);
            model.ali_vdef2 = SqlHelper.GetString(r["ali_vdef2"]);
            model.ali_vdef3 = SqlHelper.GetString(r["ali_vdef3"]);
            model.ali_RSAkey = SqlHelper.GetString(r["ali_RSAkey"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.Pay_PayWxandAli> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.Pay_PayWxandAli>(ds.Tables[0]);
        }
    }
}
