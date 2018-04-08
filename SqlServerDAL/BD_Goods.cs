using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_Goods
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Adds(Hi.Model.BD_Goods model)
        {
            string sql = string.Empty;
            StringBuilder strSql = new StringBuilder();
            if (model.OfflineStateDate.ToString() == "0001/1/1 0:00:00")
            {
                strSql.Append("insert into [BD_Goods](");
                strSql.Append("[CompID],[GoodsName],[GoodsCode],[CategoryID],[Unit],[SalePrice],[IsOffline],[IsIndex],[IsSale],[IsRecommended],[Pic],[Pic2],[Title],[Details],[memo],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[Pic3],[registeredCertificate],[validity])");
                strSql.Append("values (");
                strSql.Append("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','[21]','[22]')");
                strSql.Append(";select @@Identity");
                sql = string.Format(strSql.ToString(), model.CompID, model.GoodsName, model.GoodsCode, model.CategoryID, model.Unit, model.SalePrice, model.IsOffline, model.IsIndex, model.IsSale, model.IsRecommended, model.Pic, model.Pic2, model.Title, model.Details, model.memo, model.IsEnabled, model.CreateUserID, model.CreateDate, model.ts, model.modifyuser, model.Pic3,model.registeredCertificate,model.validity);
            }
            else
            {
                strSql.Append("insert into [BD_Goods](");
                strSql.Append("[CompID],[GoodsName],[GoodsCode],[CategoryID],[Unit],[SalePrice],[IsOffline],[IsIndex],[IsSale],[IsRecommended],[OfflineStateDate],[OfflineEndDate],[Pic],[Pic2],[Title],[Details],[memo],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[Pic3],[registeredCertificate],[validity])");
                strSql.Append("values (");
                strSql.Append("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','[23]','[24]')");
                strSql.Append(";select @@Identity");
                sql = string.Format(strSql.ToString(), model.CompID, model.GoodsName, model.GoodsCode, model.CategoryID, model.Unit, model.SalePrice, model.IsOffline, model.IsIndex, model.IsSale, model.IsRecommended, model.OfflineStateDate, model.OfflineEndDate, model.Pic, model.Pic2, model.Title, model.Details, model.memo, model.IsEnabled, model.CreateUserID, model.CreateDate, model.ts, model.modifyuser, model.Pic3,model.registeredCertificate,model.validity);
            }
            return sql;//  SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 是否上下架插入
        /// </summary>
        /// <returns></returns>
        public int Addss(Hi.Model.BD_Goods model)
        {
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, Adds(model)));
        }

        /// <summary>
        /// 商品属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="compid"></param>
        /// <returns></returns>
        public DataTable GoodsAttr(string id, string compid)
        {
            string sql = string.Format(@"select c.ID,AttributeName  from BD_GoodsAttrValues as a
                                         left join BD_AttributeValues as b on a.ValuesID=b.ID
                                            left join BD_CategoryAttribute as d on b.AttributeID =d.id
                                          left join BD_Attribute as c on c.ID=d.AttributeID
                                         where GoodsID={0}  and ISNULL(a.dr,0)=0
                                          and ISNULL(b.dr,0)=0   and ISNULL(d.dr,0)=0
                                          and ISNULL(c.dr,0)=0 and b.CompID={1} and c.CompID={1} and d.CompID={1}
                                          group by AttributeName,c.ID", id, compid);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        }

        /// <summary>
        /// 商品属性、属性值
        /// </summary>
        /// <param name="Id">商品Id</param>
        /// <param name="compid">企业Id</param>
        /// <returns></returns>
        public DataTable GoodsAttributeValue(string Id, string compid)
        {
            string sql = string.Format(@"select c.AttributeName,c.ID as attrId,a.GoodsID,b.AttrValue,b.ID as valuesId from BD_GoodsAttrValues as a 
                                        left join BD_AttributeValues as b on a.ValuesID=b.ID
                                           left join BD_CategoryAttribute as d on b.AttributeID=d.ID
                                        left join BD_Attribute as c on c.ID=d.AttributeID
                                        where GoodsID={0} and b.CompID={1} and c.CompID={1}  and d.CompID={1} and ISNULL(a.dr,0)=0
                                        and ISNULL(b.dr,0)=0 and ISNULL(c.dr,0)=0 and ISNULL(d.dr,0)=0", Id, compid);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        }

        /// <summary>
        /// 全部上架
        /// </summary>
        /// <returns></returns>
        public int Updates()
        {
            string sql = "update BD_Goods set isoffline=0";
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, sql);
        }

        public IList<Hi.Model.BD_Goods> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby, Tran));
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Goods]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Goods model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Goods] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[GoodsCode]=@GoodsCode,");
            strSql.Append("[CategoryID]=@CategoryID,");
            strSql.Append("[Unit]=@Unit,");
            strSql.Append("[SalePrice]=@SalePrice,");
            strSql.Append("[IsOffline]=@IsOffline,");
            strSql.Append("[IsIndex]=@IsIndex,");
            strSql.Append("[IsSale]=@IsSale,");
            strSql.Append("[IsRecommended]=@IsRecommended,");
            strSql.Append("[OfflineStateDate]=@OfflineStateDate,");
            strSql.Append("[OfflineEndDate]=@OfflineEndDate,");
            strSql.Append("[Pic]=@Pic,");
            strSql.Append("[Pic2]=@Pic2,");
            strSql.Append("[Pic3]=@Pic3,");
            strSql.Append("[Title]=@Title,");
            strSql.Append("[Details]=@Details,");
            strSql.Append("[IsAttribute]=@IsAttribute,");
            strSql.Append("[TemplateId]=@TemplateId,");
            strSql.Append("[Value1]=@Value1,");
            strSql.Append("[Value2]=@Value2,");
            strSql.Append("[Value3]=@Value3,");
            strSql.Append("[Value4]=@Value4,");
            strSql.Append("[Value5]=@Value5,");
            strSql.Append("[memo]=@memo,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[ViewInfos]=@ViewInfos,");
            strSql.Append("[ViewInfoID]=@ViewInfoID,");
            strSql.Append("[IsFirstShow]=@IsFirstShow,");
            strSql.Append("[Sortindex]=@Sortindex,");
            strSql.Append("[NewPic]=@NewPic,");
            strSql.Append("[ShowName]=@ShowName,");
            strSql.Append("[registeredCertificate]=@registeredCertificate,");
            strSql.Append("[validity]=@validity");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,128),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,32),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@Unit", SqlDbType.VarChar,10),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@IsOffline", SqlDbType.Int),
                    new SqlParameter("@IsIndex", SqlDbType.Int),
                    new SqlParameter("@IsSale", SqlDbType.Int),
                    new SqlParameter("@IsRecommended", SqlDbType.Int),
                    new SqlParameter("@OfflineStateDate", SqlDbType.DateTime),
                    new SqlParameter("@OfflineEndDate", SqlDbType.DateTime),
                    new SqlParameter("@Pic", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic2", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic3", SqlDbType.VarChar,128),
                    new SqlParameter("@Title", SqlDbType.VarChar,250),
                    new SqlParameter("@Details", SqlDbType.Text),
                    new SqlParameter("@IsAttribute", SqlDbType.Int),
                    new SqlParameter("@TemplateId", SqlDbType.Int),
                    new SqlParameter("@Value1", SqlDbType.VarChar,200),
                    new SqlParameter("@Value2", SqlDbType.VarChar,200),
                    new SqlParameter("@Value3", SqlDbType.VarChar,200),
                    new SqlParameter("@Value4", SqlDbType.VarChar,200),
                    new SqlParameter("@Value5", SqlDbType.VarChar,200),
                    new SqlParameter("@memo", SqlDbType.Text),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@ViewInfos", SqlDbType.VarChar,2000),
                    new SqlParameter("@ViewInfoID", SqlDbType.Int),
                    new SqlParameter("@IsFirstShow", SqlDbType.Bit),
                    new SqlParameter("@Sortindex", SqlDbType.Int),
                    new SqlParameter("@NewPic", SqlDbType.NVarChar,128),
                    new SqlParameter("@ShowName", SqlDbType.NVarChar,100),
                    new SqlParameter("@registeredCertificate", SqlDbType.NVarChar,50),
                    new SqlParameter("@validity", SqlDbType.DateTime)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsName;
            parameters[3].Value = model.GoodsCode;
            parameters[4].Value = model.CategoryID;

            if (model.Unit != null)
                parameters[5].Value = model.Unit;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.SalePrice;
            parameters[7].Value = model.IsOffline;
            parameters[8].Value = model.IsIndex;
            parameters[9].Value = model.IsSale;
            parameters[10].Value = model.IsRecommended;

            if (model.OfflineStateDate != DateTime.MinValue)
                parameters[11].Value = model.OfflineStateDate;
            else
                parameters[11].Value = DBNull.Value;


            if (model.OfflineEndDate != DateTime.MinValue)
                parameters[12].Value = model.OfflineEndDate;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Pic != null)
                parameters[13].Value = model.Pic;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Pic2 != null)
                parameters[14].Value = model.Pic2;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Pic3 != null)
                parameters[15].Value = model.Pic3;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Title != null)
                parameters[16].Value = model.Title;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Details != null)
                parameters[17].Value = model.Details;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.IsAttribute;
            parameters[19].Value = model.TemplateId;

            if (model.Value1 != null)
                parameters[20].Value = model.Value1;
            else
                parameters[20].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[21].Value = model.Value2;
            else
                parameters[21].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[22].Value = model.Value3;
            else
                parameters[22].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[23].Value = model.Value4;
            else
                parameters[23].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[24].Value = model.Value5;
            else
                parameters[24].Value = DBNull.Value;


            if (model.memo != null)
                parameters[25].Value = model.memo;
            else
                parameters[25].Value = DBNull.Value;

            parameters[26].Value = model.IsEnabled;
            parameters[27].Value = model.CreateUserID;
            parameters[28].Value = model.CreateDate;
            parameters[29].Value = model.ts;
            parameters[30].Value = model.dr;
            parameters[31].Value = model.modifyuser;

            if (model.ViewInfos != null)
                parameters[32].Value = model.ViewInfos;
            else
                parameters[32].Value = DBNull.Value;

            parameters[33].Value = model.ViewInfoID;
            parameters[34].Value = model.IsFirstShow;
            parameters[35].Value = model.Sortindex;

            if (model.NewPic != null)
                parameters[36].Value = model.NewPic;
            else
                parameters[36].Value = DBNull.Value;


            if (model.ShowName != null)
                parameters[37].Value = model.ShowName;
            else
                parameters[37].Value = DBNull.Value;

            if (model.registeredCertificate != null)
                parameters[38].Value = model.registeredCertificate;
            else
                parameters[38].Value = DBNull.Value;

            parameters[39].Value = model.validity;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Goods model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Goods](");
            strSql.Append("[CompID],[GoodsName],[GoodsCode],[CategoryID],[Unit],[SalePrice],[IsOffline],[IsIndex],[IsSale],[IsRecommended],[OfflineStateDate],[OfflineEndDate],[Pic],[Pic2],[Pic3],[Title],[Details],[IsAttribute],[TemplateId],[Value1],[Value2],[Value3],[Value4],[Value5],[memo],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[ViewInfos],[ViewInfoID],[NewPic],[ShowName],[IsFirstShow],[Sortindex],[registeredCertificate],[validity])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsName,@GoodsCode,@CategoryID,@Unit,@SalePrice,@IsOffline,@IsIndex,@IsSale,@IsRecommended,@OfflineStateDate,@OfflineEndDate,@Pic,@Pic2,@Pic3,@Title,@Details,@IsAttribute,@TemplateId,@Value1,@Value2,@Value3,@Value4,@Value5,@memo,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser,@ViewInfos,@ViewInfoID,@NewPic,@ShowName,@IsFirstShow,@Sortindex,@registeredCertificate,@validity)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,128),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,32),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@Unit", SqlDbType.VarChar,10),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@IsOffline", SqlDbType.Int),
                    new SqlParameter("@IsIndex", SqlDbType.Int),
                    new SqlParameter("@IsSale", SqlDbType.Int),
                    new SqlParameter("@IsRecommended", SqlDbType.Int),
                    new SqlParameter("@OfflineStateDate", SqlDbType.DateTime),
                    new SqlParameter("@OfflineEndDate", SqlDbType.DateTime),
                    new SqlParameter("@Pic", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic2", SqlDbType.VarChar,128),
                    new SqlParameter("@Pic3", SqlDbType.VarChar,128),
                    new SqlParameter("@Title", SqlDbType.VarChar,250),
                    new SqlParameter("@Details", SqlDbType.Text),
                    new SqlParameter("@IsAttribute", SqlDbType.Int),
                    new SqlParameter("@TemplateId", SqlDbType.Int),
                    new SqlParameter("@Value1", SqlDbType.VarChar,200),
                    new SqlParameter("@Value2", SqlDbType.VarChar,200),
                    new SqlParameter("@Value3", SqlDbType.VarChar,200),
                    new SqlParameter("@Value4", SqlDbType.VarChar,200),
                    new SqlParameter("@Value5", SqlDbType.VarChar,200),
                    new SqlParameter("@memo", SqlDbType.Text),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@ViewInfos", SqlDbType.VarChar,2000),
                    new SqlParameter("@ViewInfoID", SqlDbType.Int),
                    new SqlParameter("@NewPic", SqlDbType.NVarChar,128),
                    new SqlParameter("@ShowName", SqlDbType.NVarChar,100),
                    new SqlParameter("@IsFirstShow", SqlDbType.Bit),
                    new SqlParameter("@Sortindex",SqlDbType.Int),
                    new SqlParameter("@registeredCertificate",SqlDbType.VarChar,50),
                    new SqlParameter("@validity",SqlDbType.DateTime)

            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsName;
            parameters[2].Value = model.GoodsCode;
            parameters[3].Value = model.CategoryID;

            if (model.Unit != null)
                parameters[4].Value = model.Unit;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.SalePrice;
            parameters[6].Value = model.IsOffline;
            parameters[7].Value = model.IsIndex;
            parameters[8].Value = model.IsSale;
            parameters[9].Value = model.IsRecommended;

            if (model.OfflineStateDate != DateTime.MinValue)
                parameters[10].Value = model.OfflineStateDate;
            else
                parameters[10].Value = DBNull.Value;


            if (model.OfflineEndDate != DateTime.MinValue)
                parameters[11].Value = model.OfflineEndDate;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Pic != null)
                parameters[12].Value = model.Pic;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Pic2 != null)
                parameters[13].Value = model.Pic2;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Pic3 != null)
                parameters[14].Value = model.Pic3;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Title != null)
                parameters[15].Value = model.Title;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Details != null)
                parameters[16].Value = model.Details;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.IsAttribute;
            parameters[18].Value = model.TemplateId;

            if (model.Value1 != null)
                parameters[19].Value = model.Value1;
            else
                parameters[19].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[20].Value = model.Value2;
            else
                parameters[20].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[21].Value = model.Value3;
            else
                parameters[21].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[22].Value = model.Value4;
            else
                parameters[22].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[23].Value = model.Value5;
            else
                parameters[23].Value = DBNull.Value;


            if (model.memo != null)
                parameters[24].Value = model.memo;
            else
                parameters[24].Value = DBNull.Value;

            parameters[25].Value = model.IsEnabled;
            parameters[26].Value = model.CreateUserID;
            parameters[27].Value = model.CreateDate;
            parameters[28].Value = model.ts;
            parameters[29].Value = model.modifyuser;

            if (model.ViewInfos != null)
                parameters[30].Value = model.ViewInfos;
            else
                parameters[30].Value = DBNull.Value;

            parameters[31].Value = model.ViewInfoID;



            if (model.NewPic != null)
                parameters[32].Value = model.NewPic;
            else
                parameters[32].Value = DBNull.Value;

            if (model.ShowName != null)
                parameters[33].Value = model.ShowName;
            else
                parameters[33].Value = DBNull.Value;

            parameters[34].Value = model.IsFirstShow;
            parameters[35].Value = model.Sortindex;


            if (model.registeredCertificate != null)
                parameters[36].Value = model.registeredCertificate;
            else
                parameters[36].Value = DBNull.Value;

            parameters[37].Value = model.validity;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

            //return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Goods GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Goods] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            DataSet ds = SqlHelper.Query(strSql.ToString(), Tran, parameters);// SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
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
        /// 分页获取泛型数据列表
        /// </summary>
        public DataTable GetList(int pageSize, int pageIndex, string fldSort, bool sort, string fldName, string TbName, string strCondition, out int pageCount, out int count, string CustomFldName = "", bool IsDist = true)
        {
            if (string.IsNullOrEmpty(TbName))
            {
                TbName = "[BD_Goods]";
            }
            if (string.IsNullOrEmpty(fldName))
            {
                fldName = " * ";
            }
            string strSql;
            DataSet ds = SqlHelper.PageList2(SqlHelper.LocalSqlServer, TbName, fldName, pageSize, pageIndex, fldSort, sort, strCondition, "BD_Goods.ID", IsDist, out pageCount, out count, out strSql, CustomFldName);
            return ds.Tables[0];
        }


        public DataTable GetPic(string strwhere)
        {
            string sql = "select g.pic from BD_Goodsinfo info left join BD_Goods g on info.GoodsID=g.ID where " + strwhere;
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];
        }
    }
}
