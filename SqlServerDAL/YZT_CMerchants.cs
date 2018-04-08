using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 YZT_CMerchants 
    /// </summary>
    public partial class YZT_CMerchants
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_CMerchants model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_CMerchants](");
            strSql.Append("[IsEnabled],[CMCode],[CMName],[GoodsID],[GoodsCode],[GoodsName],[CategoryID],[ValueInfo],[ForceDate],[InvalidDate],[Type],[Specify],[ProvideData],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser],[CompID])");
            strSql.Append(" values (");
            strSql.Append("@IsEnabled,@CMCode,@CMName,@GoodsID,@GoodsCode,@GoodsName,@CategoryID,@ValueInfo,@ForceDate,@InvalidDate,@Type,@Specify,@ProvideData,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser,@CompID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CMCode", SqlDbType.VarChar,50),
                    new SqlParameter("@CMName", SqlDbType.VarChar,100),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@ForceDate", SqlDbType.DateTime),
                    new SqlParameter("@InvalidDate", SqlDbType.DateTime),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@Specify", SqlDbType.VarChar,500),
                    new SqlParameter("@ProvideData", SqlDbType.VarChar,500),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int)
            };
            parameters[0].Value = model.IsEnabled;

            if (model.CMCode != null)
                parameters[1].Value = model.CMCode;
            else
                parameters[1].Value = DBNull.Value;


            if (model.CMName != null)
                parameters[2].Value = model.CMName;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.GoodsID;

            if (model.GoodsCode != null)
                parameters[4].Value = model.GoodsCode;
            else
                parameters[4].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[5].Value = model.GoodsName;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.CategoryID;

            if (model.ValueInfo != null)
                parameters[7].Value = model.ValueInfo;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ForceDate != DateTime.MinValue)
                parameters[8].Value = model.ForceDate;
            else
                parameters[8].Value = DBNull.Value;


            if (model.InvalidDate != DateTime.MinValue)
                parameters[9].Value = model.InvalidDate;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.Type;

            if (model.Specify != null)
                parameters[11].Value = model.Specify;
            else
                parameters[11].Value = DBNull.Value;


            if (model.ProvideData != null)
                parameters[12].Value = model.ProvideData;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[13].Value = model.Remark;
            else
                parameters[13].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[14].Value = model.vdef1;
            else
                parameters[14].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[15].Value = model.vdef2;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[16].Value = model.vdef3;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[18].Value = model.CreateDate;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.ts;
            parameters[20].Value = model.modifyuser;
            parameters[21].Value = model.CompID;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_CMerchants model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_CMerchants] set ");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CMCode]=@CMCode,");
            strSql.Append("[CMName]=@CMName,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsCode]=@GoodsCode,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[CategoryID]=@CategoryID,");
            strSql.Append("[ValueInfo]=@ValueInfo,");
            strSql.Append("[ForceDate]=@ForceDate,");
            strSql.Append("[InvalidDate]=@InvalidDate,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[Specify]=@Specify,");
            strSql.Append("[ProvideData]=@ProvideData,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[CompID]=@CompID");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CMCode", SqlDbType.VarChar,50),
                    new SqlParameter("@CMName", SqlDbType.VarChar,100),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@ForceDate", SqlDbType.DateTime),
                    new SqlParameter("@InvalidDate", SqlDbType.DateTime),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@Specify", SqlDbType.VarChar,500),
                    new SqlParameter("@ProvideData", SqlDbType.VarChar,500),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.IsEnabled;

            if (model.CMCode != null)
                parameters[2].Value = model.CMCode;
            else
                parameters[2].Value = DBNull.Value;


            if (model.CMName != null)
                parameters[3].Value = model.CMName;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.GoodsID;

            if (model.GoodsCode != null)
                parameters[5].Value = model.GoodsCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[6].Value = model.GoodsName;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CategoryID;

            if (model.ValueInfo != null)
                parameters[8].Value = model.ValueInfo;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ForceDate != DateTime.MinValue)
                parameters[9].Value = model.ForceDate;
            else
                parameters[9].Value = DBNull.Value;


            if (model.InvalidDate != DateTime.MinValue)
                parameters[10].Value = model.InvalidDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.Type;

            if (model.Specify != null)
                parameters[12].Value = model.Specify;
            else
                parameters[12].Value = DBNull.Value;


            if (model.ProvideData != null)
                parameters[13].Value = model.ProvideData;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[14].Value = model.Remark;
            else
                parameters[14].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[15].Value = model.vdef1;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[16].Value = model.vdef2;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[17].Value = model.vdef3;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[19].Value = model.CreateDate;
            else
                parameters[19].Value = DBNull.Value;

            parameters[20].Value = model.ts;
            parameters[21].Value = model.dr;
            parameters[22].Value = model.modifyuser;
            parameters[23].Value = model.CompID;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }


        /// <summary>
        /// 获取  该代理商的  所有有效  招商单
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="Disid"></param>
        /// <param name="pageCount"></param>
        /// <param name="Counts"></param>
        /// <returns></returns>
        public DataTable getDataTable(int pageSize, int pageIndex, string Disid,string strwhere, out int pageCount, out int Counts)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.*,h.HospitalName from YZT_CMerchants c, YZT_FirstCamp f ,SYS_Hospital h    ");
            strSql.AppendFormat("where  c.id=f.CMID and f.DisID={0}  and f.State=2 ",Disid);
            strSql.AppendFormat(" and c.dr=0 and f.dr=0 and h.id=f.HtID  ", Disid);
            if(!string.IsNullOrEmpty(strwhere))
                strSql.AppendFormat(" {0}  ", strwhere);

            DataTable pageCountTable = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
            Counts = pageCountTable.Rows.Count;
            if (Counts % pageSize == 0)
                pageCount = Counts / pageSize;
            else
                pageCount = (Counts / pageSize) + 1;

            StringBuilder sb = new StringBuilder();
            DataTable Table = new DataTable();
            if (Counts > 0)
            {
                sb.AppendFormat("select * from (select ROW_NUMBER() over( order by  ID) rwo,* from ");
                sb.AppendFormat(" ({0}) tab )b", strSql.ToString());
                sb.AppendFormat(" where b.rwo between ({0}-1)*{1}+1 and {2}*{3}", pageIndex, pageSize, pageIndex, pageSize);
                Table = SqlHelper.Query(SqlHelper.LocalSqlServer, sb.ToString()).Tables[0];
                return Table;
            }
            return pageCountTable;
        }




    }
}
