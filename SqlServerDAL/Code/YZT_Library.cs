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
    /// 数据访问类 YZT_Library
    /// </summary>
    public partial class YZT_Library
    {
        public YZT_Library()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Library model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_Library](");
            strSql.Append("[CompID],[DisID],[LibraryNO],[LibraryDate],[IState],[Salesman],[HtID],[PaymentDays],[MoneyDate],[GiveMode],[ArriveDate],[AddrID],[Principal],[Phone],[Address],[IsOBill],[BillNo],[IsBill],[Rise],[Content],[OBank],[OAccount],[TRNumber],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@LibraryNO,@LibraryDate,@IState,@Salesman,@HtID,@PaymentDays,@MoneyDate,@GiveMode,@ArriveDate,@AddrID,@Principal,@Phone,@Address,@IsOBill,@BillNo,@IsBill,@Rise,@Content,@OBank,@OAccount,@TRNumber,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@LibraryNO", SqlDbType.VarChar,50),
                    new SqlParameter("@LibraryDate", SqlDbType.DateTime),
                    new SqlParameter("@IState", SqlDbType.Int),
                    new SqlParameter("@Salesman", SqlDbType.VarChar,50),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@PaymentDays", SqlDbType.Int),
                    new SqlParameter("@MoneyDate", SqlDbType.DateTime),
                    new SqlParameter("@GiveMode", SqlDbType.VarChar,50),
                    new SqlParameter("@ArriveDate", SqlDbType.DateTime),
                    new SqlParameter("@AddrID", SqlDbType.Int),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,300),
                    new SqlParameter("@IsOBill", SqlDbType.Int),
                    new SqlParameter("@BillNo", SqlDbType.VarChar,30),
                    new SqlParameter("@IsBill", SqlDbType.Int),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
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

            if (model.LibraryNO != null)
                parameters[2].Value = model.LibraryNO;
            else
                parameters[2].Value = DBNull.Value;


            if (model.LibraryDate != DateTime.MinValue)
                parameters[3].Value = model.LibraryDate;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.IState;

            if (model.Salesman != null)
                parameters[5].Value = model.Salesman;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.HtID;
            parameters[7].Value = model.PaymentDays;

            if (model.MoneyDate != DateTime.MinValue)
                parameters[8].Value = model.MoneyDate;
            else
                parameters[8].Value = DBNull.Value;


            if (model.GiveMode != null)
                parameters[9].Value = model.GiveMode;
            else
                parameters[9].Value = DBNull.Value;


            if (model.ArriveDate != DateTime.MinValue)
                parameters[10].Value = model.ArriveDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.AddrID;

            if (model.Principal != null)
                parameters[12].Value = model.Principal;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[13].Value = model.Phone;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Address != null)
                parameters[14].Value = model.Address;
            else
                parameters[14].Value = DBNull.Value;

            parameters[15].Value = model.IsOBill;

            if (model.BillNo != null)
                parameters[16].Value = model.BillNo;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.IsBill;

            if (model.Rise != null)
                parameters[18].Value = model.Rise;
            else
                parameters[18].Value = DBNull.Value;


            if (model.Content != null)
                parameters[19].Value = model.Content;
            else
                parameters[19].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[20].Value = model.OBank;
            else
                parameters[20].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[21].Value = model.OAccount;
            else
                parameters[21].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[22].Value = model.TRNumber;
            else
                parameters[22].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[23].Value = model.Remark;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[24].Value = model.vdef1;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[25].Value = model.vdef2;
            else
                parameters[25].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[26].Value = model.vdef3;
            else
                parameters[26].Value = DBNull.Value;

            parameters[27].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[28].Value = model.CreateDate;
            else
                parameters[28].Value = DBNull.Value;

            parameters[29].Value = model.ts;
            parameters[30].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Library model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_Library] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[LibraryNO]=@LibraryNO,");
            strSql.Append("[LibraryDate]=@LibraryDate,");
            strSql.Append("[IState]=@IState,");
            strSql.Append("[Salesman]=@Salesman,");
            strSql.Append("[HtID]=@HtID,");
            strSql.Append("[PaymentDays]=@PaymentDays,");
            strSql.Append("[MoneyDate]=@MoneyDate,");
            strSql.Append("[GiveMode]=@GiveMode,");
            strSql.Append("[ArriveDate]=@ArriveDate,");
            strSql.Append("[AddrID]=@AddrID,");
            strSql.Append("[Principal]=@Principal,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[IsOBill]=@IsOBill,");
            strSql.Append("[BillNo]=@BillNo,");
            strSql.Append("[IsBill]=@IsBill,");
            strSql.Append("[Rise]=@Rise,");
            strSql.Append("[Content]=@Content,");
            strSql.Append("[OBank]=@OBank,");
            strSql.Append("[OAccount]=@OAccount,");
            strSql.Append("[TRNumber]=@TRNumber,");
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
                    new SqlParameter("@LibraryNO", SqlDbType.VarChar,50),
                    new SqlParameter("@LibraryDate", SqlDbType.DateTime),
                    new SqlParameter("@IState", SqlDbType.Int),
                    new SqlParameter("@Salesman", SqlDbType.VarChar,50),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@PaymentDays", SqlDbType.Int),
                    new SqlParameter("@MoneyDate", SqlDbType.DateTime),
                    new SqlParameter("@GiveMode", SqlDbType.VarChar,50),
                    new SqlParameter("@ArriveDate", SqlDbType.DateTime),
                    new SqlParameter("@AddrID", SqlDbType.Int),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,300),
                    new SqlParameter("@IsOBill", SqlDbType.Int),
                    new SqlParameter("@BillNo", SqlDbType.VarChar,30),
                    new SqlParameter("@IsBill", SqlDbType.Int),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
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

            if (model.LibraryNO != null)
                parameters[3].Value = model.LibraryNO;
            else
                parameters[3].Value = DBNull.Value;


            if (model.LibraryDate != DateTime.MinValue)
                parameters[4].Value = model.LibraryDate;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.IState;

            if (model.Salesman != null)
                parameters[6].Value = model.Salesman;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.HtID;
            parameters[8].Value = model.PaymentDays;

            if (model.MoneyDate != DateTime.MinValue)
                parameters[9].Value = model.MoneyDate;
            else
                parameters[9].Value = DBNull.Value;


            if (model.GiveMode != null)
                parameters[10].Value = model.GiveMode;
            else
                parameters[10].Value = DBNull.Value;


            if (model.ArriveDate != DateTime.MinValue)
                parameters[11].Value = model.ArriveDate;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.AddrID;

            if (model.Principal != null)
                parameters[13].Value = model.Principal;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[14].Value = model.Phone;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Address != null)
                parameters[15].Value = model.Address;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.IsOBill;

            if (model.BillNo != null)
                parameters[17].Value = model.BillNo;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.IsBill;

            if (model.Rise != null)
                parameters[19].Value = model.Rise;
            else
                parameters[19].Value = DBNull.Value;


            if (model.Content != null)
                parameters[20].Value = model.Content;
            else
                parameters[20].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[21].Value = model.OBank;
            else
                parameters[21].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[22].Value = model.OAccount;
            else
                parameters[22].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[23].Value = model.TRNumber;
            else
                parameters[23].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[24].Value = model.Remark;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[25].Value = model.vdef1;
            else
                parameters[25].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[26].Value = model.vdef2;
            else
                parameters[26].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[27].Value = model.vdef3;
            else
                parameters[27].Value = DBNull.Value;

            parameters[28].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[29].Value = model.CreateDate;
            else
                parameters[29].Value = DBNull.Value;

            parameters[30].Value = model.ts;
            parameters[31].Value = model.dr;
            parameters[32].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_Library] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[YZT_Library]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [YZT_Library]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.YZT_Library GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [YZT_Library] ");
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
        public IList<Hi.Model.YZT_Library> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [YZT_Library]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.YZT_Library> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.YZT_Library> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[YZT_Library]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.YZT_Library GetModel(DataRow r)
        {
            Hi.Model.YZT_Library model = new Hi.Model.YZT_Library();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.LibraryNO = SqlHelper.GetString(r["LibraryNO"]);
            model.LibraryDate = SqlHelper.GetDateTime(r["LibraryDate"]);
            model.IState = SqlHelper.GetInt(r["IState"]);
            model.Salesman = SqlHelper.GetString(r["Salesman"]);
            model.HtID = SqlHelper.GetInt(r["HtID"]);
            model.PaymentDays = SqlHelper.GetInt(r["PaymentDays"]);
            model.MoneyDate = SqlHelper.GetDateTime(r["MoneyDate"]);
            model.GiveMode = SqlHelper.GetString(r["GiveMode"]);
            model.ArriveDate = SqlHelper.GetDateTime(r["ArriveDate"]);
            model.AddrID = SqlHelper.GetInt(r["AddrID"]);
            model.Principal = SqlHelper.GetString(r["Principal"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.Address = SqlHelper.GetString(r["Address"]);
            model.IsOBill = SqlHelper.GetInt(r["IsOBill"]);
            model.BillNo = SqlHelper.GetString(r["BillNo"]);
            model.IsBill = SqlHelper.GetInt(r["IsBill"]);
            model.Rise = SqlHelper.GetString(r["Rise"]);
            model.Content = SqlHelper.GetString(r["Content"]);
            model.OBank = SqlHelper.GetString(r["OBank"]);
            model.OAccount = SqlHelper.GetString(r["OAccount"]);
            model.TRNumber = SqlHelper.GetString(r["TRNumber"]);
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
        private IList<Hi.Model.YZT_Library> GetList(DataSet ds)
        {
            List<Hi.Model.YZT_Library> l = new List<Hi.Model.YZT_Library>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                l.Add(GetModel(r));
            }
            return l;
        }
    }
}
