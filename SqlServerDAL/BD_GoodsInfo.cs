using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DBUtility;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    public partial class BD_GoodsInfo
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Adds(Hi.Model.BD_GoodsInfo model)
        {
            string sql = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_GoodsInfo](");
            strSql.Append("[GoodsID],[CompID],[BarCode],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9],[Value10],[ValueInfo],[SalePrice],[TinkerPrice],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[BatchNO],[validDate])");
            strSql.Append("values(");
            strSql.Append("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')");
            strSql.Append(";select @@Identity");
            sql = string.Format(strSql.ToString(), model.GoodsID, model.CompID, model.BarCode, model.Value1, model.Value2, model.Value3, model.Value4, model.Value5, model.Value6, model.Value7, model.Value8, model.Value9, model.Value10, model.ValueInfo, model.SalePrice, model.TinkerPrice, model.IsEnabled, model.CreateUserID, model.CreateDate, model.ts, model.modifyuser,model.Batchno,model.Validdate);
            return sql; //SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
        /// <summary>
        /// 批量插入BD_GoodsInfo表
        /// </summary>
        /// <param name="ll"></param>
        public bool InserGoodsInfo(List<Hi.Model.BD_GoodsInfo> ll)
        {
            DataTable dataTable = GetTableSchema();
            foreach (Hi.Model.BD_GoodsInfo item in ll)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[1] = item.CompID;
                dataRow[2] = item.GoodsID;
                dataRow[3] = item.BarCode;
                dataRow[4] = item.Value1;
                dataRow[5] = item.Value2;
                dataRow[6] = item.Value3;
                dataRow[7] = item.Value4;
                dataRow[8] = item.Value5;
                dataRow[9] = item.Value6;
                dataRow[10] = item.Value7;
                dataRow[11] = item.Value8;
                dataRow[12] = item.Value9;
                dataRow[13] = item.Value10;
                dataRow[14] = item.ValueInfo;
                dataRow[15] = item.SalePrice;
                dataRow[16] = item.TinkerPrice;
                dataRow[17] = item.IsEnabled;
                dataRow[18] = item.CreateUserID;
                dataRow[19] = item.CreateDate;
                dataRow[20] = item.ts;
                dataRow[21] = item.dr;
                dataRow[22] = item.modifyuser;
                dataRow[23] = item.Batchno;
                dataRow[24] = item.Validdate;
                dataTable.Rows.Add(dataRow);
            }
            return SqlHelper.SqlBulkCopyInsert(SqlHelper.LocalSqlServer, dataTable, "BD_GoodsInfo");
        }
        private static DataTable GetTableSchema()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("ID"), new DataColumn("CompID"), new DataColumn("GoodsID"), new DataColumn("BarCode"), new DataColumn("Value1"), new DataColumn("Value2"), new DataColumn("Value3"), new DataColumn("Value4"), new DataColumn("Value5"), new DataColumn("Value6"), new DataColumn("Value7"), new DataColumn("Value8"), new DataColumn("Value9"), new DataColumn("Value10"), new DataColumn("ValueInfo"), new DataColumn("SalePrice"), new DataColumn("TinkerPrice"), new DataColumn("IsEnabled"), new DataColumn("CreateUserID"), new DataColumn("CreateDate"), new DataColumn("ts"), new DataColumn("dr"), new DataColumn("modifyuser"), new DataColumn("BatchNO"), new DataColumn("validDate") });
            return dataTable;
        }
        /// <summary>
        /// 删除GoodInfo
        /// </summary>
        /// <returns></returns>
        public int Deletes(string goodsid, int compid)
        {
            string sql = "delete bd_goodsinfo where compid=" + compid + " and goodsid=" + goodsid;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, sql);
        }

        /// <summary>
        /// 根据GoodsID查找GoodsInfoID
        /// </summary>
        /// <returns></returns>
        public string GetGoodsInfoID(string GoodsID)
        {
            string sql = "select top 1 id from bd_goodsinfo where goodsid= " + GoodsID;
            if (SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0].Rows.Count > 0)
            {
                return SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0].Rows[0][0].ToString();
            }
            return "";
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_GoodsInfo model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_GoodsInfo] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[BarCode]=@BarCode,");
            strSql.Append("[IsOffline]=@IsOffline,");
            strSql.Append("[Value1]=@Value1,");
            strSql.Append("[Value2]=@Value2,");
            strSql.Append("[Value3]=@Value3,");
            strSql.Append("[Value4]=@Value4,");
            strSql.Append("[Value5]=@Value5,");
            strSql.Append("[Value6]=@Value6,");
            strSql.Append("[Value7]=@Value7,");
            strSql.Append("[Value8]=@Value8,");
            strSql.Append("[Value9]=@Value9,");
            strSql.Append("[Value10]=@Value10,");
            strSql.Append("[ValueInfo]=@ValueInfo,");
            strSql.Append("[SalePrice]=@SalePrice,");
            strSql.Append("[TinkerPrice]=@TinkerPrice,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[Inventory]=@Inventory,");
            strSql.Append("[BatchNO]=@BatchNO,");
            strSql.Append("[validDate]=@validDate");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50),
                    new SqlParameter("@IsOffline", SqlDbType.Int),
                    new SqlParameter("@Value1", SqlDbType.VarChar,50),
                    new SqlParameter("@Value2", SqlDbType.VarChar,50),
                    new SqlParameter("@Value3", SqlDbType.VarChar,50),
                    new SqlParameter("@Value4", SqlDbType.VarChar,50),
                    new SqlParameter("@Value5", SqlDbType.VarChar,50),
                    new SqlParameter("@Value6", SqlDbType.VarChar,50),
                    new SqlParameter("@Value7", SqlDbType.VarChar,50),
                    new SqlParameter("@Value8", SqlDbType.VarChar,50),
                    new SqlParameter("@Value9", SqlDbType.VarChar,50),
                    new SqlParameter("@Value10", SqlDbType.VarChar,50),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Inventory", SqlDbType.Decimal),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsID;

            if (model.BarCode != null)
                parameters[3].Value = model.BarCode;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.IsOffline;

            if (model.Value1 != null)
                parameters[5].Value = model.Value1;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[6].Value = model.Value2;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[7].Value = model.Value3;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[8].Value = model.Value4;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[9].Value = model.Value5;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Value6 != null)
                parameters[10].Value = model.Value6;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Value7 != null)
                parameters[11].Value = model.Value7;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Value8 != null)
                parameters[12].Value = model.Value8;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Value9 != null)
                parameters[13].Value = model.Value9;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Value10 != null)
                parameters[14].Value = model.Value10;
            else
                parameters[14].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[15].Value = model.ValueInfo;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.SalePrice;
            parameters[17].Value = model.TinkerPrice;
            parameters[18].Value = model.IsEnabled;
            parameters[19].Value = model.CreateUserID;
            parameters[20].Value = model.CreateDate;
            parameters[21].Value = model.ts;
            parameters[22].Value = model.dr;
            parameters[23].Value = model.modifyuser;
            parameters[24].Value = model.Inventory;

            if (model.Batchno != null)
                parameters[25].Value = model.Batchno;
            else
                parameters[25].Value = DBNull.Value;

            if (model.Validdate != DateTime.MinValue)
                parameters[26].Value = model.Validdate;
            else
                parameters[26].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

            // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsInfo model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_GoodsInfo](");
            strSql.Append("[CompID],[GoodsID],[BarCode],[IsOffline],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9],[Value10],[ValueInfo],[SalePrice],[TinkerPrice],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[Inventory],[BatchNO],[validDate])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsID,@BarCode,@IsOffline,@Value1,@Value2,@Value3,@Value4,@Value5,@Value6,@Value7,@Value8,@Value9,@Value10,@ValueInfo,@SalePrice,@TinkerPrice,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser,@Inventory,@BatchNO,@validDate)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50),
                    new SqlParameter("@IsOffline", SqlDbType.Int),
                    new SqlParameter("@Value1", SqlDbType.VarChar,50),
                    new SqlParameter("@Value2", SqlDbType.VarChar,50),
                    new SqlParameter("@Value3", SqlDbType.VarChar,50),
                    new SqlParameter("@Value4", SqlDbType.VarChar,50),
                    new SqlParameter("@Value5", SqlDbType.VarChar,50),
                    new SqlParameter("@Value6", SqlDbType.VarChar,50),
                    new SqlParameter("@Value7", SqlDbType.VarChar,50),
                    new SqlParameter("@Value8", SqlDbType.VarChar,50),
                    new SqlParameter("@Value9", SqlDbType.VarChar,50),
                    new SqlParameter("@Value10", SqlDbType.VarChar,50),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Inventory", SqlDbType.Decimal),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsID;

            if (model.BarCode != null)
                parameters[2].Value = model.BarCode;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.IsOffline;

            if (model.Value1 != null)
                parameters[4].Value = model.Value1;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[5].Value = model.Value2;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[6].Value = model.Value3;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[7].Value = model.Value4;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[8].Value = model.Value5;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Value6 != null)
                parameters[9].Value = model.Value6;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Value7 != null)
                parameters[10].Value = model.Value7;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Value8 != null)
                parameters[11].Value = model.Value8;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Value9 != null)
                parameters[12].Value = model.Value9;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Value10 != null)
                parameters[13].Value = model.Value10;
            else
                parameters[13].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[14].Value = model.ValueInfo;
            else
                parameters[14].Value = DBNull.Value;

            parameters[15].Value = model.SalePrice;
            parameters[16].Value = model.TinkerPrice;
            parameters[17].Value = model.IsEnabled;
            parameters[18].Value = model.CreateUserID;
            parameters[19].Value = model.CreateDate;
            parameters[20].Value = model.ts;
            parameters[21].Value = model.modifyuser;
            parameters[22].Value = model.Inventory;

            if (model.Batchno != null)
                parameters[23].Value = model.Batchno;
            else
                parameters[23].Value = DBNull.Value;

            if (model.Validdate != DateTime.MinValue)
                parameters[24].Value = model.Validdate;
            else
                parameters[24].Value = DBNull.Value;

            //  return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));


        }
        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_GoodsInfo]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_GoodsInfo> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby, Tran));
        }
        /// <summary>
        /// 获取新的goodsinfo实体
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="goodsId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public DataTable GetGoodsInfoModel(string compId, string goodsId, string categoryId, string infoIdList, SqlTransaction Tran)
        {
            DataTable dt = new DataTable();
            string sql = string.Format(@"select a.* from BD_Goodsinfo as a,BD_AttributeValues as b 
                        where a.Value1=b.AttrValue and ISNULL(b.dr,0)=0 
                        and b.IsEnabled=1 and ISNULL(a.dr,0)=0
                        and a.IsEnabled=1 and a.CompID={0} 
                        and b.CompID={0} and a.GoodsID={1} 
                        and AttributeID={2} {3} order by b.ID", compId, goodsId, categoryId, infoIdList == "" ? "" : "and a.ID in(" + infoIdList + ")");
            if (Tran == null)
            {
                dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            }
            else
            {
                dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql, Tran).Tables[0];
            }
            return dt;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_GoodsInfo GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_GoodsInfo] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            DataSet ds = SqlHelper.Query(strSql.ToString(), Tran, parameters);
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
        /// 获取goods表没删除的商品信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetGoodsModel(string where)
        {
            string sql = string.Format(@"select  b.Unit,a.* ,b.GoodsName,b.memo,b.Pic from BD_GoodsInfo as a ,BD_Goods as b
                        where a.GoodsID=b.ID and b.IsEnabled=1 and ISNULL(a.dr,0)=0
                        and ISNULL(b.dr,0)=0 and a.IsEnabled=1 {0}  order by CreateDate desc", where);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        }

        /// <summary>
        /// 获取goods表没删除的商品信息  (合同用)
        /// </summary>
        /// <returns></returns>
        public DataSet getGoodsModels(string where)
        {
            string sql = string.Format(@"select  b.Unit,a.* ,b.GoodsName,b.memo,b.Pic,0 as FirstCampID ,0 as htid,'' as HospitalName,0 as AreaID,'' as AreaName from BD_GoodsInfo a left join BD_Goods as b  on
            a.GoodsID=b.ID and b.IsEnabled=1 and ISNULL(a.dr,0)=0
            and ISNULL(b.dr,0)=0 and a.IsEnabled=1 and Inventory>=0 
            where Inventory>=0 {0} order by CreateDate desc", where);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        }

        /// <summary>
        /// 获取代理商信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet getGoodsCMerchants(string where)
        {
            string sql = string.Format(@"select f.id as FirstCampID,f.htid
,h.HospitalName,f.AreaID,disa.AreaName,c.GoodsID,f.DisID from  
YZT_CMerchants c left join YZT_FirstCamp f on f.CMID =c.ID and f.State=2
left join SYS_Hospital h on h.id=f.htid  
left join BD_DisArea disa on disa.ID=f.AreaID
where ISNULL(c.dr,0)=0 {0}", where);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        }

        /// <summary>
        /// 根据 infonID  获取商品列表   （代理商 库存用）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet getDisGoodsStock(string where)
        {
            string sql = string.Format(@"select i.id,g.GoodsName,g.GoodsCode,g.Unit,i.ValueInfo,d.validDate,d.SignNum,d.BatchNO,d.Remark 
             from DIS_OrderOut o ,DIS_OrderOutDetail d,BD_Goods g,BD_GoodsInfo i 
             where   o.IsSign=1 and d.GoodsinfoID =i.ID
             and  i.goodsid=g.id and d.OrderOutID=o.ID {0} ", where);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        }

    }
}
