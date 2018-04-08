//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/11/23 18:18:43
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
    /// 数据访问类 BF_ZD_PAYBANK
    /// </summary>
    public class BF_ZD_PAYBANK
    {
        public BF_ZD_PAYBANK()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Hi.Model.BF_ZD_PAYBANK model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BF_ZD_PAYBANK](");
            strSql.Append("[FQHHO2])");
            strSql.Append(" values (");
            strSql.Append("@FQHHO2)");
            SqlParameter[] parameters = {
                    new SqlParameter("@FQHHO2", SqlDbType.VarChar,14)
            };
            parameters[0].Value = model.FQHHO2;

            if (SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0)
                   return model.FQHHO2;
            else
                return null;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BF_ZD_PAYBANK model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BF_ZD_PAYBANK] set ");
            strSql.Append("[ZHUANT]=@ZHUANT,");
            strSql.Append("[JIGULB]=@JIGULB,");
            strSql.Append("[HANBDM]=@HANBDM,");
            strSql.Append("[C2ZCHH]=@C2ZCHH,");
            strSql.Append("[BHSJCY]=@BHSJCY,");
            strSql.Append("[C2RHDM]=@C2RHDM,");
            strSql.Append("[FBHHO2]=@FBHHO2,");
            strSql.Append("[QSHHO2]=@QSHHO2,");
            strSql.Append("[JIEDDM]=@JIEDDM,");
            strSql.Append("[FKHMC1]=@FKHMC1,");
            strSql.Append("[CXUMC1]=@CXUMC1,");
            strSql.Append("[YLIUBZ]=@YLIUBZ,");
            strSql.Append("[SUSDDM]=@SUSDDM,");
            strSql.Append("[DIZHI1]=@DIZHI1,");
            strSql.Append("[YOUZBM]=@YOUZBM,");
            strSql.Append("[TFDESC]=@TFDESC,");
            strSql.Append("[BYZDBE]=@BYZDBE,");
            strSql.Append("[SXIORQ]=@SXIORQ,");
            strSql.Append("[SHIXRQ]=@SHIXRQ,");
            strSql.Append("[BEIZXX]=@BEIZXX,");
            strSql.Append("[BEIY40]=@BEIY40,");
            strSql.Append("[BYBZ01]=@BYBZ01,");
            strSql.Append("[BYBZ02]=@BYBZ02,");
            strSql.Append("[WEIHRQ]=@WEIHRQ,");
            strSql.Append("[WEIHSJ]=@WEIHSJ,");
            strSql.Append("[WEIHGY]=@WEIHGY,");
            strSql.Append("[ROWIDD]=@ROWIDD,");
            strSql.Append("[SHJNCH]=@SHJNCH,");
            strSql.Append("[JILUZT]=@JILUZT,");
            strSql.Append("[SPEC16]=@SPEC16,");
            strSql.Append("[SPEC32]=@SPEC32");
            strSql.Append(" where [FQHHO2]=@FQHHO2");
            SqlParameter[] parameters = {
                    new SqlParameter("@FQHHO2", SqlDbType.VarChar,14),
                    new SqlParameter("@ZHUANT", SqlDbType.VarChar,8),
                    new SqlParameter("@JIGULB", SqlDbType.VarChar,8),
                    new SqlParameter("@HANBDM", SqlDbType.VarChar,8),
                    new SqlParameter("@C2ZCHH", SqlDbType.VarChar,14),
                    new SqlParameter("@BHSJCY", SqlDbType.VarChar,130),
                    new SqlParameter("@C2RHDM", SqlDbType.VarChar,14),
                    new SqlParameter("@FBHHO2", SqlDbType.VarChar,14),
                    new SqlParameter("@QSHHO2", SqlDbType.VarChar,14),
                    new SqlParameter("@JIEDDM", SqlDbType.VarChar,8),
                    new SqlParameter("@FKHMC1", SqlDbType.VarChar,160),
                    new SqlParameter("@CXUMC1", SqlDbType.VarChar,80),
                    new SqlParameter("@YLIUBZ", SqlDbType.VarChar,80),
                    new SqlParameter("@SUSDDM", SqlDbType.VarChar,8),
                    new SqlParameter("@DIZHI1", SqlDbType.VarChar,160),
                    new SqlParameter("@YOUZBM", SqlDbType.VarChar,8),
                    new SqlParameter("@TFDESC", SqlDbType.VarChar,100),
                    new SqlParameter("@BYZDBE", SqlDbType.VarChar,120),
                    new SqlParameter("@SXIORQ", SqlDbType.VarChar,8),
                    new SqlParameter("@SHIXRQ", SqlDbType.VarChar,8),
                    new SqlParameter("@BEIZXX", SqlDbType.VarChar,128),
                    new SqlParameter("@BEIY40", SqlDbType.VarChar,40),
                    new SqlParameter("@BYBZ01", SqlDbType.VarChar,8),
                    new SqlParameter("@BYBZ02", SqlDbType.VarChar,8),
                    new SqlParameter("@WEIHRQ", SqlDbType.VarChar,8),
                    new SqlParameter("@WEIHSJ", SqlDbType.VarChar,16),
                    new SqlParameter("@WEIHGY", SqlDbType.VarChar,8),
                    new SqlParameter("@ROWIDD", SqlDbType.VarChar,24),
                    new SqlParameter("@SHJNCH", SqlDbType.VarChar,24),
                    new SqlParameter("@JILUZT", SqlDbType.VarChar,8),
                    new SqlParameter("@SPEC16", SqlDbType.VarChar,16),
                    new SqlParameter("@SPEC32", SqlDbType.VarChar,32)
            };
            parameters[0].Value = model.FQHHO2;
            parameters[1].Value = model.ZHUANT;
            parameters[2].Value = model.JIGULB;
            parameters[3].Value = model.HANBDM;
            parameters[4].Value = model.C2ZCHH;
            parameters[5].Value = model.BHSJCY;
            parameters[6].Value = model.C2RHDM;
            parameters[7].Value = model.FBHHO2;
            parameters[8].Value = model.QSHHO2;
            parameters[9].Value = model.JIEDDM;
            parameters[10].Value = model.FKHMC1;
            parameters[11].Value = model.CXUMC1;
            parameters[12].Value = model.YLIUBZ;
            parameters[13].Value = model.SUSDDM;
            parameters[14].Value = model.DIZHI1;
            parameters[15].Value = model.YOUZBM;
            parameters[16].Value = model.TFDESC;
            parameters[17].Value = model.BYZDBE;
            parameters[18].Value = model.SXIORQ;
            parameters[19].Value = model.SHIXRQ;
            parameters[20].Value = model.BEIZXX;
            parameters[21].Value = model.BEIY40;
            parameters[22].Value = model.BYBZ01;
            parameters[23].Value = model.BYBZ02;
            parameters[24].Value = model.WEIHRQ;
            parameters[25].Value = model.WEIHSJ;
            parameters[26].Value = model.WEIHGY;
            parameters[27].Value = model.ROWIDD;
            parameters[28].Value = model.SHJNCH;
            parameters[29].Value = model.JILUZT;
            parameters[30].Value = model.SPEC16;
            parameters[31].Value = model.SPEC32;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string FQHHO2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BF_ZD_PAYBANK] ");
            strSql.Append(" where [FQHHO2]=@FQHHO2");
            SqlParameter[] parameters = {
                    new SqlParameter("@FQHHO2", SqlDbType.VarChar,14)};
            parameters[0].Value = FQHHO2;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string FQHHO2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BF_ZD_PAYBANK]");
            strSql.Append(" where [FQHHO2]= @FQHHO2");
            SqlParameter[] parameters = {
                    new SqlParameter("@FQHHO2", SqlDbType.VarChar,14)};
            parameters[0].Value = FQHHO2;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BF_ZD_PAYBANK GetModel(string FQHHO2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BF_ZD_PAYBANK] ");
            strSql.Append(" where [FQHHO2]=@FQHHO2");
            SqlParameter[] parameters = {
                    new SqlParameter("@FQHHO2", SqlDbType.VarChar,14)};
            parameters[0].Value = FQHHO2;
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
        public IList<Hi.Model.BF_ZD_PAYBANK> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BF_ZD_PAYBANK]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BF_ZD_PAYBANK> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BF_ZD_PAYBANK> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count,string fldName)
        {
            string strSql;
            DataSet ds=null;
            if(string.IsNullOrEmpty(fldName))
             ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BF_ZD_PAYBANK]", null, pageSize, pageIndex, fldSort, sort, strCondition, "FQHHO2", false, out pageCount, out count, out strSql);
           else
             ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BF_ZD_PAYBANK]", fldName, pageSize, pageIndex, fldSort, sort, strCondition, "FQHHO2", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BF_ZD_PAYBANK GetModel(DataRow r)
        {
            Hi.Model.BF_ZD_PAYBANK model = new Hi.Model.BF_ZD_PAYBANK();
            model.FQHHO2 = SqlHelper.GetString(r["FQHHO2"]);
            model.ZHUANT = SqlHelper.GetString(r["ZHUANT"]);
            model.JIGULB = SqlHelper.GetString(r["JIGULB"]);
            model.HANBDM = SqlHelper.GetString(r["HANBDM"]);
            model.C2ZCHH = SqlHelper.GetString(r["C2ZCHH"]);
            model.BHSJCY = SqlHelper.GetString(r["BHSJCY"]);
            model.C2RHDM = SqlHelper.GetString(r["C2RHDM"]);
            model.FBHHO2 = SqlHelper.GetString(r["FBHHO2"]);
            model.QSHHO2 = SqlHelper.GetString(r["QSHHO2"]);
            model.JIEDDM = SqlHelper.GetString(r["JIEDDM"]);
            model.FKHMC1 = SqlHelper.GetString(r["FKHMC1"]);
            model.CXUMC1 = SqlHelper.GetString(r["CXUMC1"]);
            model.YLIUBZ = SqlHelper.GetString(r["YLIUBZ"]);
            model.SUSDDM = SqlHelper.GetString(r["SUSDDM"]);
            model.DIZHI1 = SqlHelper.GetString(r["DIZHI1"]);
            model.YOUZBM = SqlHelper.GetString(r["YOUZBM"]);
            model.TFDESC = SqlHelper.GetString(r["TFDESC"]);
            model.BYZDBE = SqlHelper.GetString(r["BYZDBE"]);
            model.SXIORQ = SqlHelper.GetString(r["SXIORQ"]);
            model.SHIXRQ = SqlHelper.GetString(r["SHIXRQ"]);
            model.BEIZXX = SqlHelper.GetString(r["BEIZXX"]);
            model.BEIY40 = SqlHelper.GetString(r["BEIY40"]);
            model.BYBZ01 = SqlHelper.GetString(r["BYBZ01"]);
            model.BYBZ02 = SqlHelper.GetString(r["BYBZ02"]);
            model.WEIHRQ = SqlHelper.GetString(r["WEIHRQ"]);
            model.WEIHSJ = SqlHelper.GetString(r["WEIHSJ"]);
            model.WEIHGY = SqlHelper.GetString(r["WEIHGY"]);
            model.ROWIDD = SqlHelper.GetString(r["ROWIDD"]);
            model.SHJNCH = SqlHelper.GetString(r["SHJNCH"]);
            model.JILUZT = SqlHelper.GetString(r["JILUZT"]);
            model.SPEC16 = SqlHelper.GetString(r["SPEC16"]);
            model.SPEC32 = SqlHelper.GetString(r["SPEC32"]);
            return model;
        }


        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BF_ZD_PAYBANK GetModel1(DataRow r)
        {
            Hi.Model.BF_ZD_PAYBANK model = new Hi.Model.BF_ZD_PAYBANK();
            model.FQHHO2 = SqlHelper.GetString(r["FQHHO2"]);
            model.FKHMC1 = SqlHelper.GetString(r["FKHMC1"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BF_ZD_PAYBANK> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BF_ZD_PAYBANK>(ds.Tables[0]);
        }
    }
}
