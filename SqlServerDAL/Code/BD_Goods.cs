//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/11/30 10:32:07
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
    /// 数据访问类 BD_Goods
    /// </summary>
    public partial class BD_Goods
    {
        public BD_Goods()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Goods model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Goods](");
            strSql.Append("[CompID],[GoodsName],[GoodsCode],[CategoryID],[Unit],[SalePrice],[IsOffline],[IsIndex],[IsSale],[IsRecommended],[OfflineStateDate],[OfflineEndDate],[Pic],[Pic2],[Pic3],[Title],[HideInfo1],[HideInfo2],[Details],[IsAttribute],[TemplateId],[Value1],[Value2],[Value3],[Value4],[Value5],[memo],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[ViewInfos],[ViewInfoID],[IsFirstShow],[NewPic],[ShowName],[IsLS],[LSPrice],[registeredCertificate],[validity])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsName,@GoodsCode,@CategoryID,@Unit,@SalePrice,@IsOffline,@IsIndex,@IsSale,@IsRecommended,@OfflineStateDate,@OfflineEndDate,@Pic,@Pic2,@Pic3,@Title,@HideInfo1,@HideInfo2,@Details,@IsAttribute,@TemplateId,@Value1,@Value2,@Value3,@Value4,@Value5,@memo,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser,@ViewInfos,@ViewInfoID,@IsFirstShow,@NewPic,@ShowName,@IsLS,@LSPrice,@registeredCertificate,@validity)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,128),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,32),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@Unit", SqlDbType.VarChar,30),
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
                    new SqlParameter("@HideInfo1", SqlDbType.VarChar,128),
                    new SqlParameter("@HideInfo2", SqlDbType.VarChar,128),
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
                    new SqlParameter("@ViewInfos", SqlDbType.VarChar,5000),
                    new SqlParameter("@ViewInfoID", SqlDbType.Int),
                    new SqlParameter("@IsFirstShow", SqlDbType.Bit),
                    new SqlParameter("@NewPic", SqlDbType.NVarChar,128),
                    new SqlParameter("@ShowName", SqlDbType.NVarChar,100),
                    new SqlParameter("@IsLS", SqlDbType.Int),
                    new SqlParameter("@LSPrice", SqlDbType.Decimal),
                    new SqlParameter("@registeredCertificate", SqlDbType.VarChar,50),
                    new SqlParameter("@validity", SqlDbType.DateTime)
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


            if (model.HideInfo1 != null)
                parameters[16].Value = model.HideInfo1;
            else
                parameters[16].Value = DBNull.Value;


            if (model.HideInfo2 != null)
                parameters[17].Value = model.HideInfo2;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Details != null)
                parameters[18].Value = model.Details;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.IsAttribute;
            parameters[20].Value = model.TemplateId;

            if (model.Value1 != null)
                parameters[21].Value = model.Value1;
            else
                parameters[21].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[22].Value = model.Value2;
            else
                parameters[22].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[23].Value = model.Value3;
            else
                parameters[23].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[24].Value = model.Value4;
            else
                parameters[24].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[25].Value = model.Value5;
            else
                parameters[25].Value = DBNull.Value;


            if (model.memo != null)
                parameters[26].Value = model.memo;
            else
                parameters[26].Value = DBNull.Value;

            parameters[27].Value = model.IsEnabled;
            parameters[28].Value = model.CreateUserID;
            parameters[29].Value = model.CreateDate;
            parameters[30].Value = model.ts;
            parameters[31].Value = model.modifyuser;

            if (model.ViewInfos != null)
                parameters[32].Value = model.ViewInfos;
            else
                parameters[32].Value = DBNull.Value;

            parameters[33].Value = model.ViewInfoID;
            parameters[34].Value = model.IsFirstShow;

            if (model.NewPic != null)
                parameters[35].Value = model.NewPic;
            else
                parameters[35].Value = DBNull.Value;


            if (model.ShowName != null)
                parameters[36].Value = model.ShowName;
            else
                parameters[36].Value = DBNull.Value;

            parameters[37].Value = model.IsLS;
            parameters[38].Value = model.LSPrice;

            if (model.registeredCertificate != null)
                parameters[39].Value = model.registeredCertificate;
            else
                parameters[39].Value = DBNull.Value;

            parameters[40].Value = model.validity;


            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Goods model)
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
            strSql.Append("[HideInfo1]=@HideInfo1,");
            strSql.Append("[HideInfo2]=@HideInfo2,");
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
            strSql.Append("[IsLS]=@IsLS,");
            strSql.Append("[LSPrice]=@LSPrice,");
            strSql.Append("[registeredCertificate]=@registeredCertificate,");
            strSql.Append("[validity]=@validity");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,128),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,32),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@Unit", SqlDbType.VarChar,30),
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
                    new SqlParameter("@HideInfo1", SqlDbType.VarChar,128),
                    new SqlParameter("@HideInfo2", SqlDbType.VarChar,128),
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
                    new SqlParameter("@ViewInfos", SqlDbType.VarChar,5000),
                    new SqlParameter("@ViewInfoID", SqlDbType.Int),
                    new SqlParameter("@IsFirstShow", SqlDbType.Bit),
                    new SqlParameter("@Sortindex", SqlDbType.Int),
                    new SqlParameter("@NewPic", SqlDbType.NVarChar,128),
                    new SqlParameter("@ShowName", SqlDbType.NVarChar,100),
                    new SqlParameter("@IsLS", SqlDbType.Int),
                    new SqlParameter("@LSPrice", SqlDbType.Decimal),
                    new SqlParameter("@registeredCertificate", SqlDbType.VarChar,50),
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


            if (model.HideInfo1 != null)
                parameters[17].Value = model.HideInfo1;
            else
                parameters[17].Value = DBNull.Value;


            if (model.HideInfo2 != null)
                parameters[18].Value = model.HideInfo2;
            else
                parameters[18].Value = DBNull.Value;


            if (model.Details != null)
                parameters[19].Value = model.Details;
            else
                parameters[19].Value = DBNull.Value;

            parameters[20].Value = model.IsAttribute;
            parameters[21].Value = model.TemplateId;

            if (model.Value1 != null)
                parameters[22].Value = model.Value1;
            else
                parameters[22].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[23].Value = model.Value2;
            else
                parameters[23].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[24].Value = model.Value3;
            else
                parameters[24].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[25].Value = model.Value4;
            else
                parameters[25].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[26].Value = model.Value5;
            else
                parameters[26].Value = DBNull.Value;


            if (model.memo != null)
                parameters[27].Value = model.memo;
            else
                parameters[27].Value = DBNull.Value;

            parameters[28].Value = model.IsEnabled;
            parameters[29].Value = model.CreateUserID;
            parameters[30].Value = model.CreateDate;
            parameters[31].Value = model.ts;
            parameters[32].Value = model.dr;
            parameters[33].Value = model.modifyuser;

            if (model.ViewInfos != null)
                parameters[34].Value = model.ViewInfos;
            else
                parameters[34].Value = DBNull.Value;

            parameters[35].Value = model.ViewInfoID;
            parameters[36].Value = model.IsFirstShow;
            parameters[37].Value = model.Sortindex;

            if (model.NewPic != null)
                parameters[38].Value = model.NewPic;
            else
                parameters[38].Value = DBNull.Value;


            if (model.ShowName != null)
                parameters[39].Value = model.ShowName;
            else
                parameters[39].Value = DBNull.Value;

            parameters[40].Value = model.IsLS;
            parameters[41].Value = model.LSPrice;

            if (model.registeredCertificate != null)
                parameters[42].Value = model.registeredCertificate;
            else
                parameters[42].Value = DBNull.Value;

            parameters[43].Value = model.validity;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_Goods] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_Goods]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_Goods]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Goods GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Goods] ");
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
        public IList<Hi.Model.BD_Goods> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Goods]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_Goods> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_Goods> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count, string FildName = null)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_Goods]", FildName, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_Goods GetModel(DataRow r)
        {
            Hi.Model.BD_Goods model = new Hi.Model.BD_Goods();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.GoodsName = SqlHelper.GetString(r["GoodsName"]);
            model.GoodsCode = SqlHelper.GetString(r["GoodsCode"]);
            model.CategoryID = SqlHelper.GetInt(r["CategoryID"]);
            model.Unit = SqlHelper.GetString(r["Unit"]);
            model.SalePrice = SqlHelper.GetDecimal(r["SalePrice"]);
            model.IsOffline = SqlHelper.GetInt(r["IsOffline"]);
            model.IsIndex = SqlHelper.GetInt(r["IsIndex"]);
            model.IsSale = SqlHelper.GetInt(r["IsSale"]);
            model.IsRecommended = SqlHelper.GetInt(r["IsRecommended"]);
            model.OfflineStateDate = SqlHelper.GetDateTime(r["OfflineStateDate"]);
            model.OfflineEndDate = SqlHelper.GetDateTime(r["OfflineEndDate"]);
            model.Pic = SqlHelper.GetString(r["Pic"]);
            model.Pic2 = SqlHelper.GetString(r["Pic2"]);
            model.Pic3 = SqlHelper.GetString(r["Pic3"]);
            model.Title = SqlHelper.GetString(r["Title"]);
            model.HideInfo1 = SqlHelper.GetString(r["HideInfo1"]);
            model.HideInfo2 = SqlHelper.GetString(r["HideInfo2"]);
            model.Details = SqlHelper.GetString(r["Details"]);
            model.IsAttribute = SqlHelper.GetInt(r["IsAttribute"]);
            model.TemplateId = SqlHelper.GetInt(r["TemplateId"]);
            model.Value1 = SqlHelper.GetString(r["Value1"]);
            model.Value2 = SqlHelper.GetString(r["Value2"]);
            model.Value3 = SqlHelper.GetString(r["Value3"]);
            model.Value4 = SqlHelper.GetString(r["Value4"]);
            model.Value5 = SqlHelper.GetString(r["Value5"]);
            model.memo = SqlHelper.GetString(r["memo"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.ViewInfos = SqlHelper.GetString(r["ViewInfos"]);
            model.ViewInfoID = SqlHelper.GetInt(r["ViewInfoID"]);
            model.IsFirstShow = SqlHelper.GetBool(r["IsFirstShow"]);
            model.Sortindex = SqlHelper.GetInt(r["Sortindex"]);
            model.NewPic = SqlHelper.GetString(r["NewPic"]);
            model.ShowName = SqlHelper.GetString(r["ShowName"]);
            model.IsLS = SqlHelper.GetInt(r["IsLS"]);
            model.LSPrice = SqlHelper.GetDecimal(r["LSPrice"]);
            model.registeredCertificate= SqlHelper.GetString(r["registeredCertificate"]);
            model.validity = SqlHelper.GetDateTime(r["validity"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_Goods> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_Goods>(ds.Tables[0]);
        }
    }
}
