//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/4/11 10:25:44
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
    /// 数据访问类 Pay_PaymentSettings
    /// </summary>
    public class Pay_PaymentSettings
    {
        public Pay_PaymentSettings()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.Pay_PaymentSettings model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [Pay_PaymentSettings](");
            strSql.Append("[CompID],[pay_sxfsq],[pay_zffs],[pay_kjzfbl],[pay_kjzfstart],[pay_kjzfend],[pay_ylzfbl],[pay_ylzfstart],[pay_ylzfend],[pay_b2cwyzfbl],[pay_b2bwyzf],[Pay_mfcs],[createUser],[createDate],[Start],[remark],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5],[vdef6],[vdef7],[vdef8],[vdef9],[vdef10],[vdef11],[vdef12],[vdef13],[vdef14],[vdef15],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@pay_sxfsq,@pay_zffs,@pay_kjzfbl,@pay_kjzfstart,@pay_kjzfend,@pay_ylzfbl,@pay_ylzfstart,@pay_ylzfend,@pay_b2cwyzfbl,@pay_b2bwyzf,@Pay_mfcs,@createUser,@createDate,@Start,@remark,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5,@vdef6,@vdef7,@vdef8,@vdef9,@vdef10,@vdef11,@vdef12,@vdef13,@vdef14,@vdef15,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.BigInt),
                    new SqlParameter("@pay_sxfsq", SqlDbType.Int),
                    new SqlParameter("@pay_zffs", SqlDbType.Int),
                    new SqlParameter("@pay_kjzfbl", SqlDbType.Decimal),
                    new SqlParameter("@pay_kjzfstart", SqlDbType.Decimal),
                    new SqlParameter("@pay_kjzfend", SqlDbType.Decimal),
                    new SqlParameter("@pay_ylzfbl", SqlDbType.Decimal),
                    new SqlParameter("@pay_ylzfstart", SqlDbType.Decimal),
                    new SqlParameter("@pay_ylzfend", SqlDbType.Decimal),
                    new SqlParameter("@pay_b2cwyzfbl", SqlDbType.Decimal),
                    new SqlParameter("@pay_b2bwyzf", SqlDbType.Decimal),
                    new SqlParameter("@Pay_mfcs", SqlDbType.Int),
                    new SqlParameter("@createUser", SqlDbType.Int),
                    new SqlParameter("@createDate", SqlDbType.DateTime),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef6", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef7", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef8", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef9", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef10", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef11", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef12", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef13", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef14", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef15", SqlDbType.NVarChar,128),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.pay_sxfsq;
            parameters[2].Value = model.pay_zffs;
            parameters[3].Value = model.pay_kjzfbl;
            parameters[4].Value = model.pay_kjzfstart;
            parameters[5].Value = model.pay_kjzfend;
            parameters[6].Value = model.pay_ylzfbl;
            parameters[7].Value = model.pay_ylzfstart;
            parameters[8].Value = model.pay_ylzfend;
            parameters[9].Value = model.pay_b2cwyzfbl;
            parameters[10].Value = model.pay_b2bwyzf;
            parameters[11].Value = model.Pay_mfcs;
            parameters[12].Value = model.createUser;

            if (model.createDate != DateTime.MinValue)
                parameters[13].Value = model.createDate;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.Start;

            if (model.remark != null)
                parameters[15].Value = model.remark;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[16].Value = model.vdef1;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[17].Value = model.vdef2;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[18].Value = model.vdef3;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[19].Value = model.vdef4;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[20].Value = model.vdef5;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef6 != null)
                parameters[21].Value = model.vdef6;
            else
                parameters[21].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[22].Value = model.vdef7;
            else
                parameters[22].Value = DBNull.Value;


            if (model.vdef8 != null)
                parameters[23].Value = model.vdef8;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef9 != null)
                parameters[24].Value = model.vdef9;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef10 != null)
                parameters[25].Value = model.vdef10;
            else
                parameters[25].Value = DBNull.Value;


            if (model.vdef11 != null)
                parameters[26].Value = model.vdef11;
            else
                parameters[26].Value = DBNull.Value;


            if (model.vdef12 != null)
                parameters[27].Value = model.vdef12;
            else
                parameters[27].Value = DBNull.Value;


            if (model.vdef13 != null)
                parameters[28].Value = model.vdef13;
            else
                parameters[28].Value = DBNull.Value;


            if (model.vdef14 != null)
                parameters[29].Value = model.vdef14;
            else
                parameters[29].Value = DBNull.Value;


            if (model.vdef15 != null)
                parameters[30].Value = model.vdef15;
            else
                parameters[30].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[31].Value = model.ts;
            else
                parameters[31].Value = DBNull.Value;

            parameters[32].Value = model.dr;
            parameters[33].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.Pay_PaymentSettings model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [Pay_PaymentSettings] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[pay_sxfsq]=@pay_sxfsq,");
            strSql.Append("[pay_zffs]=@pay_zffs,");
            strSql.Append("[pay_kjzfbl]=@pay_kjzfbl,");
            strSql.Append("[pay_kjzfstart]=@pay_kjzfstart,");
            strSql.Append("[pay_kjzfend]=@pay_kjzfend,");
            strSql.Append("[pay_ylzfbl]=@pay_ylzfbl,");
            strSql.Append("[pay_ylzfstart]=@pay_ylzfstart,");
            strSql.Append("[pay_ylzfend]=@pay_ylzfend,");
            strSql.Append("[pay_b2cwyzfbl]=@pay_b2cwyzfbl,");
            strSql.Append("[pay_b2bwyzf]=@pay_b2bwyzf,");
            strSql.Append("[Pay_mfcs]=@Pay_mfcs,");
            strSql.Append("[createUser]=@createUser,");
            strSql.Append("[createDate]=@createDate,");
            strSql.Append("[Start]=@Start,");
            strSql.Append("[remark]=@remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5,");
            strSql.Append("[vdef6]=@vdef6,");
            strSql.Append("[vdef7]=@vdef7,");
            strSql.Append("[vdef8]=@vdef8,");
            strSql.Append("[vdef9]=@vdef9,");
            strSql.Append("[vdef10]=@vdef10,");
            strSql.Append("[vdef11]=@vdef11,");
            strSql.Append("[vdef12]=@vdef12,");
            strSql.Append("[vdef13]=@vdef13,");
            strSql.Append("[vdef14]=@vdef14,");
            strSql.Append("[vdef15]=@vdef15,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.BigInt),
                    new SqlParameter("@pay_sxfsq", SqlDbType.Int),
                    new SqlParameter("@pay_zffs", SqlDbType.Int),
                    new SqlParameter("@pay_kjzfbl", SqlDbType.Decimal),
                    new SqlParameter("@pay_kjzfstart", SqlDbType.Decimal),
                    new SqlParameter("@pay_kjzfend", SqlDbType.Decimal),
                    new SqlParameter("@pay_ylzfbl", SqlDbType.Decimal),
                    new SqlParameter("@pay_ylzfstart", SqlDbType.Decimal),
                    new SqlParameter("@pay_ylzfend", SqlDbType.Decimal),
                    new SqlParameter("@pay_b2cwyzfbl", SqlDbType.Decimal),
                    new SqlParameter("@pay_b2bwyzf", SqlDbType.Decimal),
                    new SqlParameter("@Pay_mfcs", SqlDbType.Int),
                    new SqlParameter("@createUser", SqlDbType.Int),
                    new SqlParameter("@createDate", SqlDbType.DateTime),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef6", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef7", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef8", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef9", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef10", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef11", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef12", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef13", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef14", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef15", SqlDbType.NVarChar,128),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.pay_sxfsq;
            parameters[3].Value = model.pay_zffs;
            parameters[4].Value = model.pay_kjzfbl;
            parameters[5].Value = model.pay_kjzfstart;
            parameters[6].Value = model.pay_kjzfend;
            parameters[7].Value = model.pay_ylzfbl;
            parameters[8].Value = model.pay_ylzfstart;
            parameters[9].Value = model.pay_ylzfend;
            parameters[10].Value = model.pay_b2cwyzfbl;
            parameters[11].Value = model.pay_b2bwyzf;
            parameters[12].Value = model.Pay_mfcs;
            parameters[13].Value = model.createUser;

            if (model.createDate != DateTime.MinValue)
                parameters[14].Value = model.createDate;
            else
                parameters[14].Value = DBNull.Value;

            parameters[15].Value = model.Start;

            if (model.remark != null)
                parameters[16].Value = model.remark;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[17].Value = model.vdef1;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[18].Value = model.vdef2;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[19].Value = model.vdef3;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[20].Value = model.vdef4;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[21].Value = model.vdef5;
            else
                parameters[21].Value = DBNull.Value;


            if (model.vdef6 != null)
                parameters[22].Value = model.vdef6;
            else
                parameters[22].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[23].Value = model.vdef7;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef8 != null)
                parameters[24].Value = model.vdef8;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef9 != null)
                parameters[25].Value = model.vdef9;
            else
                parameters[25].Value = DBNull.Value;


            if (model.vdef10 != null)
                parameters[26].Value = model.vdef10;
            else
                parameters[26].Value = DBNull.Value;


            if (model.vdef11 != null)
                parameters[27].Value = model.vdef11;
            else
                parameters[27].Value = DBNull.Value;


            if (model.vdef12 != null)
                parameters[28].Value = model.vdef12;
            else
                parameters[28].Value = DBNull.Value;


            if (model.vdef13 != null)
                parameters[29].Value = model.vdef13;
            else
                parameters[29].Value = DBNull.Value;


            if (model.vdef14 != null)
                parameters[30].Value = model.vdef14;
            else
                parameters[30].Value = DBNull.Value;


            if (model.vdef15 != null)
                parameters[31].Value = model.vdef15;
            else
                parameters[31].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[32].Value = model.ts;
            else
                parameters[32].Value = DBNull.Value;

            parameters[33].Value = model.dr;
            parameters[34].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [Pay_PaymentSettings] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[Pay_PaymentSettings]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [Pay_PaymentSettings]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.Pay_PaymentSettings GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [Pay_PaymentSettings] ");
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
        public IList<Hi.Model.Pay_PaymentSettings> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [Pay_PaymentSettings]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.Pay_PaymentSettings> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.Pay_PaymentSettings> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[Pay_PaymentSettings]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.Pay_PaymentSettings GetModel(DataRow r)
        {
            Hi.Model.Pay_PaymentSettings model = new Hi.Model.Pay_PaymentSettings();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.pay_sxfsq = SqlHelper.GetInt(r["pay_sxfsq"]);
            model.pay_zffs = SqlHelper.GetInt(r["pay_zffs"]);
            model.pay_kjzfbl = SqlHelper.GetDecimal(r["pay_kjzfbl"]);
            model.pay_kjzfstart = SqlHelper.GetDecimal(r["pay_kjzfstart"]);
            model.pay_kjzfend = SqlHelper.GetDecimal(r["pay_kjzfend"]);
            model.pay_ylzfbl = SqlHelper.GetDecimal(r["pay_ylzfbl"]);
            model.pay_ylzfstart = SqlHelper.GetDecimal(r["pay_ylzfstart"]);
            model.pay_ylzfend = SqlHelper.GetDecimal(r["pay_ylzfend"]);
            model.pay_b2cwyzfbl = SqlHelper.GetDecimal(r["pay_b2cwyzfbl"]);
            model.pay_b2bwyzf = SqlHelper.GetDecimal(r["pay_b2bwyzf"]);
            model.Pay_mfcs = SqlHelper.GetInt(r["Pay_mfcs"]);
            model.createUser = SqlHelper.GetInt(r["createUser"]);
            model.createDate = SqlHelper.GetDateTime(r["createDate"]);
            model.Start = SqlHelper.GetInt(r["Start"]);
            model.remark = SqlHelper.GetString(r["remark"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.vdef4 = SqlHelper.GetString(r["vdef4"]);
            model.vdef5 = SqlHelper.GetString(r["vdef5"]);
            model.vdef6 = SqlHelper.GetString(r["vdef6"]);
            model.vdef7 = SqlHelper.GetString(r["vdef7"]);
            model.vdef8 = SqlHelper.GetString(r["vdef8"]);
            model.vdef9 = SqlHelper.GetString(r["vdef9"]);
            model.vdef10 = SqlHelper.GetString(r["vdef10"]);
            model.vdef11 = SqlHelper.GetString(r["vdef11"]);
            model.vdef12 = SqlHelper.GetString(r["vdef12"]);
            model.vdef13 = SqlHelper.GetString(r["vdef13"]);
            model.vdef14 = SqlHelper.GetString(r["vdef14"]);
            model.vdef15 = SqlHelper.GetString(r["vdef15"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.Pay_PaymentSettings> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.Pay_PaymentSettings>(ds.Tables[0]);
        }
    }
}
