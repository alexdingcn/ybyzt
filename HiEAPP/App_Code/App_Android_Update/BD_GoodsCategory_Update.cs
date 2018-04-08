using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using DBUtility;
using Hi.Model;
using LitJson;

/// <summary>
///BD_Goods 的摘要说明
/// </summary>
public class BD_GoodsCategory_Update
{
    public BD_GoodsCategory_Update()
	{

    }
    /// <summary>
    /// 获取企业产品分类列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultGoodsCategory GetResellerProductClassifyList(string JSon)
    {
        try
        {
            string userID = string.Empty;
            string disID = string.Empty;
            string companyID = string.Empty; //绑定的企业ID（目前只绑定一家，默认传入0）
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["CompanyID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                companyID = JInfo["CompanyID"].ToString();
            }
            else
            {
                return new ResultGoodsCategory() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,int.Parse(companyID), int.Parse(disID == "" ? "0" : disID)))
                return new ResultGoodsCategory() { Result = "F", Description = "参数异常" };

            Hi.Model.BD_Company comp = new Hi.Model.BD_Company();
            if (companyID.Trim() == "0")//经销商分类：compID传0
            {
                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID.Trim()));
                if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                    return new ResultGoodsCategory() { Result = "F", Description = "经销商信息异常" };
                comp = new Hi.BLL.BD_Company().GetModel(dis.CompID);
            }
            else //企业分类
            {
                comp = new Hi.BLL.BD_Company().GetModel(int.Parse(companyID.Trim()));
            }
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0)
                return new ResultGoodsCategory() { Result = "T", Description = "企业异常" };



            string strsql = "select GoodsTypeID as ClassifyID, CategoryCode as ClassifyCode,CategoryName as ClassifyName,";
            strsql = strsql + "ParentID,SortIndex from BD_GoodsCategory ";
            strsql = strsql + "where CompID='" + comp.ID + "' and ISNULL(dr,0)=0 and IsEnabled = 1 ";
            DataTable dt = SqlAccess.ExecuteSqlDataTable(strsql, ConfigurationSettings.AppSettings["ConnectionString"].ToString());


            List<ProductClassify> pList = new List<ProductClassify>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductClassify pClassifies = new ProductClassify();
                pClassifies.ClassifyID = ClsSystem.gnvl(dt.Rows[i]["ClassifyID"], "");
                pClassifies.ClassifyName = ClsSystem.gnvl(dt.Rows[i]["ClassifyName"], "");
                pClassifies.ParentID = ClsSystem.gnvl(dt.Rows[i]["ParentID"], "");
                pClassifies.SortIndex = ClsSystem.gnvl(dt.Rows[i]["SortIndex"], "");
                pList.Add(pClassifies);

            }

            ProductClassify classifies = new ProductClassify();
            classifies.ClassifyID = "-1";
            classifies.ClassifyName = "全部分类";
            classifies.ParentID = "0";
            classifies.SortIndex = "0";
            pList.Add(classifies);

            return new ResultGoodsCategory()
            {
                Result = "T",
                Description = "获取成功",
                CompanyID = comp.ID.ToString(),
                CompanyName = comp.CompName,
                ProductClassifyList = pList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerProductClassifyList ：" + JSon);
            return new ResultGoodsCategory() { Result = "F", Description = "异常" };
        }
    }
       

     #region 返回

        public class ResultOrderProList
        {
            public string Result { get; set; }
            public string Description { get; set; }
            public List<ResultOrderPro> OrderPro { get; set; }
        }

        public class ResultOrderPro
        {
            public string ProID { get; set; }
            public string ProType { get; set; }
            public string ProStartTime { get; set; }
            public string ProEndTime { get; set; }
            public string OrderPrice { get; set; }
            public string Discount { get; set; }
        }

        public class ResultCollect
        {
            public string Result { get; set; }
            public string Description { get; set; }
            public string GoodsID { get; set; }
        }

        public class ResultGoodsCategory
        {
            public string Result { get; set; }
            public string Description { get; set; }
            public string CompanyID { get; set; }
            public string CompanyName { get; set; }
            public List<ProductClassify> ProductClassifyList { get; set; }
        }

        public class ProductClassify
        {
            public string ClassifyID { get; set; }
            public string ClassifyName { get; set; }
            public string ClassifyCode { get; set; }
            public string ParentID { get; set; }
            public string SortIndex { get; set; }
        }

        public class ResultGoodsList
        {
            public string Result { get; set; }
            public string Description { get; set; }
            public string ClassifyID { get; set; }
            public Product Product { get; set; }
        }

        public class ResultProductList
        {
            public string Result { get; set; }
            public string Description { get; set; }
            public string ClassifyID { get; set; }
            public List<Product> ProductList { get; set; }
        }

        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
            public string ClassifyID { get; set; }
            public string SalePrice { get; set; }
            public string IsSale { get; set; }
            public string IsCollect { get; set; }
            public string Title { get; set; }
            public string Unit { get; set; }
            public List<Pic> ProductPicUrlList { get; set; }
            public List<ProductAttribute> ProductAttributeList { get; set; }
            public List<SKU> SKUList { get; set; }
        }

        public class Pic
        {
            public string ProductID { get; set; }
            public string IsDeafult { get; set; }
            public string PicUrl { get; set; }
        }

        public class ProductAttribute
        {
            public string ProductID { get; set; }
            public string ProductAttributeID { get; set; }
            public string ProductAttributeName { get; set; }
            public List<ProductAttValue> ProductAttValueList { get; set; }
        }

        public class ProductAttValue
        {
            public string ProductID { get; set; }
            public string ProductAttributeID { get; set; }
            public string ProductAttValueID { get; set; }
            public string ProductAttValueName { get; set; }
        }

        public class SKU
        {
            public string ProductID { get; set; }
            public string SKUID { get; set; }
            public string SKUName { get; set; }
            public string BarCode { get; set; }
            public string ValueInfo { get; set; }
            public string SalePrice { get; set; }
            public string IsPro { get; set; }
            public PromotionInfo ProInfo { get; set; }
            public string TinkerPrice { get; set; }
            public List<ProductAttValueID> ProductAttValueIDList { get; set; }
        }

        public class PromotionInfo
        {
            public string ProID { get; set; }
            public string ProTitle { get; set; }
            public string ProInfos { get; set; }
            
            public string Tpye { get; set; }//促销类型   0、特价促销 1、商品促销
            public string ProTpye { get; set; }//促销方式   特价促销（1、赠品  2、优惠 ）  商品促销（3、满送  4、打折）
            public string Discount { get; set; }//打折率   （ProType = 1、2 是0;     3是满件数  4是打折0-100）

            public string ProStartTime { get; set; }//促销开始时间
            public string ProEndTime { get; set; }//促销结束时间
        }

        public class ProductAttValueID
        {
            public string ProductAttributeID { get; set; }
        }

        #endregion
}