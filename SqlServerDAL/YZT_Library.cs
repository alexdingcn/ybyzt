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

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Library model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_Library](");
            strSql.Append("[CompID],[DisID],[LibraryNO],[LibraryDate],[IState],[Salesman],[HtID],[PaymentDays],[MoneyDate],[GiveMode],[ArriveDate],[AddrID],[Principal],[Phone],[Address],[IsOBill],[BillNo],[IsBill],[Rise],[Content],[OBank],[OAccount],[TRNumber],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser],[hospital])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@LibraryNO,@LibraryDate,@IState,@Salesman,@HtID,@PaymentDays,@MoneyDate,@GiveMode,@ArriveDate,@AddrID,@Principal,@Phone,@Address,@IsOBill,@BillNo,@IsBill,@Rise,@Content,@OBank,@OAccount,@TRNumber,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser,@hospital)");
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
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@hospital", SqlDbType.VarChar,50)
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

            if (model.hospital != null)
                parameters[31].Value = model.hospital;
            else
                parameters[31].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Library model, SqlTransaction Tran)
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
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[hospital]=@hospital");
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
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@hospital", SqlDbType.VarChar,50)
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

            if (model.hospital != null)
                parameters[33].Value = model.hospital;
            else
                parameters[33].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 获取代理商出库列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageCount"></param>
        /// <param name="Counts"></param>
        /// <param name="pagCount">是否已知总行数</param>
        /// <returns></returns>
        public DataTable getDataTable(int pageSize, int pageIndex, string strWhere, out int pageCount, out int Counts,int pagCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select l.ID,l.LibraryNO,l.LibraryDate,h.HospitalName,u.TrueName,l.IState ,l.hospital   ");
            strSql.Append(" from YZT_Library l left join SYS_Hospital h on l.HtID=h.ID   ");
            strSql.Append("left join SYS_Users u on  l.CreateUserID=u.ID  ");
            strSql.AppendFormat("where 1=1 {0} ", strWhere);
            if (pagCount <= 0)
            {
                DataTable pageCountTable = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
                Counts = pageCountTable.Rows.Count;
            }
            else
            {
                Counts = pagCount;
            }
          
            if (Counts % pageSize == 0)
                pageCount = Counts / pageSize;
            else
                pageCount = (Counts / pageSize) + 1;

            StringBuilder sb = new StringBuilder();
            DataTable Table = new DataTable();
                sb.AppendFormat("select * from (select ROW_NUMBER() over( order by  ID desc) rwo,* from ");
                sb.AppendFormat(" ({0}) tab )b", strSql.ToString());
                sb.AppendFormat(" where b.rwo between ({0}-1)*{1}+1 and {2}*{3}", pageIndex, pageSize, pageIndex, pageSize);
                Table = SqlHelper.Query(SqlHelper.LocalSqlServer, sb.ToString()).Tables[0];
                return Table;

        }


        /// <summary>
        /// 获取医院下拉
        /// </summary>
        /// <param name="DisID"></param>
        /// <returns></returns>
        public DataTable getHtDrop(string DisID)
        {
            string sql = string.Format("select hospital from YZT_Library  where hospital<>''  and dr=0 and IState =1 and disid={0}  group by hospital ", DisID);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        }


        #endregion


    }
}
