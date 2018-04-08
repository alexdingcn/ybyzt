using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Aop.Api.Domain;
using DBUtility;
using Hi.Model;
using LitJson;

/// <summary>
///BD_Goods 的摘要说明
/// </summary>
public class BD_Goods_Update
{
    public BD_Goods_Update()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 获取某一分类产品信息,兼容了无属性的值，分类传-1
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public BD_GoodsCategory.ResultProductList GetProductsList(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string categoryID = string.Empty; //分类ID
            string criticalProductID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["CriticalProductID"].ToString() != "" &&
                JInfo["GetType"].ToString() != "" && JInfo["Rows"].ToString() != "" &&
                JInfo["SortType"].ToString() != "" && JInfo["Sort"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                categoryID = JInfo["ClassifyID"].ToString();
                criticalProductID = JInfo["CriticalProductID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
            }
            else
            {
                return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,0, int.Parse(disID == "" ? "0" : disID)))
                return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "参数异常" };

            #endregion

            #region 模拟分页

            string strsql = string.Empty; //搜索sql
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "未找到经销商" };
            const string tabName = " [dbo].[BD_Goods]"; //表名
            sortType = sortType == "2" ? "SalePrice" : "ID"; //价格排序
            string strWhere = string.Empty;

            switch (categoryID)
            {
                case "-2": //促销列表
                    {
                        List<Hi.Model.BD_Promotion> promotionList = new Hi.BLL.BD_Promotion().GetList("",
                            " compID='" + dis.CompID + "' and ProStartTime<='" + DateTime.Now + "' and ProEndTime >='" +
                            DateTime.Now + "' and IsEnabled=1", "");
                        List<Hi.Model.BD_PromotionDetail> detailList = new List<BD_PromotionDetail>();
                        if (promotionList != null && promotionList.Count > 0)
                        {
                            detailList = new Hi.BLL.BD_PromotionDetail().GetList("",
                                " ProID in(" + string.Join(",", promotionList.Select(p => p.ID)) + ")", "");
                        }
                        if (promotionList == null)
                            return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "今天无促销" };
                        if (detailList == null)
                            return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "今天无促销" };
                        var ienum = detailList.Select(p => p.GoodsID);
                        if (ienum.Count() > 0)
                            strWhere += " and ID in ( " + string.Join(",", ienum) + ")";
                    }
                    break;
                case "-3": //收藏列表
                    {
                        List<Hi.Model.BD_DisCollect> collects = new Hi.BLL.BD_DisCollect().GetList("",
                            "disID='" + disID + "' and dr=0", "");
                        if (collects != null)
                        {
                            strWhere += " and ID not in ( -1 ";
                            strWhere = collects.Aggregate(strWhere, (current, goods) => current + ("," + goods.GoodsID)) +
                                       ")";
                        }
                    }
                    break;
                default:
                    if (categoryID != "-1") //全部
                    {
                        strWhere += " and CategoryID in (" + Common.AllCategory(int.Parse(categoryID)) + ")";
                    }
                    break;
            }

            //商品可售区域判断
            List<Common.GoodsID> list = Common.DisEnAreaGoodsID(disID, dis.CompID.ToString());
            if (list != null)
            {
                strWhere += " and ID not in ( -1 ";
                strWhere = list.Aggregate(strWhere, (current, goods) => current + ("," + goods.goodsID)) + ")";
            }
            strWhere += " and ISNULL(dr,0)=0 and isoffline=1 and IsEnabled = 1 and compid=" + dis.CompID;

            strsql = new Common().PageSqlString(criticalProductID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);

            #endregion

            List<Hi.Model.BD_DisCollect> Colist = Common.GetDataSource<BD_DisCollect>("",
                " and disID='" + disID + "' and compID='" + dis.CompID + "' and IsEnabled =1");
            List<Hi.Model.BD_GoodsInfo> infoAllList = new Hi.BLL.BD_GoodsInfo().GetList("",
                " CompID='" + dis.CompID + "' and ISNULL(dr,0)=0 and IsEnabled=1 and isoffline=1", "");

            #region 赋值

            int CategoryID = 0;

            List<BD_GoodsCategory.Product> ProductList = new List<BD_GoodsCategory.Product>();
            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if (dsList != null)
            {
                if (dsList.Rows.Count == 0)
                    return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "未找到数据" };
                foreach (DataRow row in dsList.Rows)
                {
                    string SKUName = string.Empty;
                    BD_GoodsCategory.Product product = new BD_GoodsCategory.Product();
                    product.ProductID = row["ID"].ToString(); //商品ID
                    product.ProductName = row["GoodsName"].ToString();
                    SKUName += product.ProductName;
                    product.SalePrice = decimal.Parse(row["SalePrice"].ToString()).ToString("0.00");
                    product.IsSale = row["IsSale"].ToString();

                    List<Hi.Model.BD_DisCollect> alist = Colist != null && Colist.Count > 0
                        ? Colist.Where(p => p.GoodsID.ToString() == product.ProductID).ToList()
                        : null;
                    product.IsCollect = alist != null && alist.Count > 0 ? "1" : "0";
                    product.Title = row["Title"].ToString();
                    //product.Details = row["Details"].ToString();
                    product.Title = row["Title"].ToString();
                    product.Unit = row["Unit"].ToString();
                    product.ClassifyID = row["CategoryID"].ToString();
                    CategoryID = int.Parse(row["CategoryID"].ToString()); //类别ID

                    List<BD_GoodsCategory.Pic> Pic = new List<BD_GoodsCategory.Pic>();

                    #region List<Pic> Pic

                    if (row["Pic"].ToString() != "" && row["Pic"].ToString() != "X")
                    {
                        BD_GoodsCategory.Pic pic = new BD_GoodsCategory.Pic();
                        pic.ProductID = row["ID"].ToString();
                        pic.IsDeafult = "1";
                        pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                     row["Pic"].ToString();
                        Pic.Add(pic);
                    }

                    Pic.AddRange(GetPicList(row["ID"].ToString()));

                    #endregion

                    product.ProductPicUrlList = Pic;

                    List<BD_GoodsCategory.SKU> SKUList = new List<BD_GoodsCategory.SKU>();
                    string strID = "0";

                    #region 通过 商品ID和属性值ID关联表，找到属性值

                    List<Hi.Model.BD_GoodsInfo> goodsInfoList = infoAllList != null && infoAllList.Count > 0
                        ? infoAllList.Where(p => p.GoodsID.ToString() == row["ID"].ToString()).ToList()
                        : null;
                    foreach (Hi.Model.BD_GoodsInfo goodsInfo in goodsInfoList)
                    {
                        if (!Common.IsOffline(goodsInfo.ID)) continue;

                        BD_GoodsCategory.SKU SKU = new BD_GoodsCategory.SKU();
                        //SKUID是GoodsInfoID,SKUName是GoodsName+各种属性值
                        SKU.SKUID = goodsInfo.ID.ToString();
                        SKU.ProductID = goodsInfo.GoodsID.ToString();
                        SKU.BarCode = goodsInfo.BarCode;
                        //SKUName = GoodsName + ValueInfo
                        SKU.SKUName = SKUName + " " + goodsInfo.ValueInfo;

                        SKU.ValueInfo = goodsInfo.ValueInfo;
                        SKU.SalePrice = goodsInfo.SalePrice.ToString("0.00");

                        int ProID = 0; //暂时未用到 促销ID
                        SKU.IsPro = "0"; //默认不是促销价
                        decimal price = Common.GetProPrice(goodsInfo.GoodsID.ToString(), goodsInfo.ID.ToString(),
                            goodsInfo.CompID.ToString(), out ProID);
                        if (price == 0)
                        {
                            List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                                " GoodsInfoID='" + goodsInfo.ID + "' and ISNULL(dr,0)=0 and compid='" + goodsInfo.CompID +
                                "' and IsEnabled=1", "");
                            SKU.TinkerPrice = goodsPrice.Count != 0
                                ? goodsPrice[0].TinkerPrice.ToString("0.00")
                                : goodsInfo.TinkerPrice.ToString("0.00");
                        }
                        else
                        {
                            SKU.IsPro = "1"; //是促销价
                            SKU.ProInfo = GetProInfo(ProID, goodsInfo.ID);
                            SKU.TinkerPrice = price.ToString("0.00");
                        }


                        List<BD_GoodsCategory.ProductAttValueID> ProductAttValueIDList = new List<BD_GoodsCategory.ProductAttValueID>();

                        List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", " GoodsID='" + goodsInfo.GoodsID + "' and ISNULL(dr,0)=0", "");
                        if (attrList == null)
                            return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                        List<Hi.Model.BD_GoodsAttrs> attrValList = new Hi.BLL.BD_GoodsAttrs().GetList("*", " CompID ='" + dis.CompID + "'", "");
                        foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
                        {
                            strID += "," + attribute.AttrsID;
                            string[] args = new[] { goodsInfo.ValueInfo };
                            string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string item in items)
                            {
                                string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                                if (strs[1] == attribute.AttrsInfoName)
                                {
                                    BD_GoodsCategory.ProductAttValueID productAttValueID = new BD_GoodsCategory.ProductAttValueID()
                                    {
                                        ProductAttributeValueID = attribute.ID.ToString()//
                                    };
                                    ProductAttValueIDList.Add(productAttValueID);
                                }
                            }
                        }

                        SKU.ProductAttValueIDList = ProductAttValueIDList;
                        SKUList.Add(SKU);
                    }

                    #endregion

                    product.SKUList = SKUList;

                    List<BD_GoodsCategory.ProductAttribute> ProductAttributeList = new List<BD_GoodsCategory.ProductAttribute>();

                    #region 通过商品类别ID和属性ID关联表，找到属性ID

                    List<Hi.Model.BD_GoodsAttrs> val = new Hi.BLL.BD_GoodsAttrs().GetList("", " ID in (" + strID + ") and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0", "");
                    if (val == null)
                        return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "未找到商品属性" };

                    foreach (Hi.Model.BD_GoodsAttrs goodsAttr in val)
                    {
                        BD_GoodsCategory.ProductAttribute proAttr = new BD_GoodsCategory.ProductAttribute();

                        proAttr.ProductID = row["ID"].ToString();
                        proAttr.ProductAttributeID = goodsAttr.ID.ToString(); //属性ID
                        proAttr.ProductAttributeName = goodsAttr.AttrsName; //属性名称

                        List<BD_GoodsCategory.ProductAttValue> ProductAttValueList = new List<BD_GoodsCategory.ProductAttValue>();

                        List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("",
                            " AttrsID='" + goodsAttr.ID + "' and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0" +
                            " and AttrsID in (" + strID + ")", "ID"); //todo:商品属性表修改咨询商品结构
                        if (attrList == null)
                            return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                        foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
                        {
                            BD_GoodsCategory.ProductAttValue productAttValue = new BD_GoodsCategory.ProductAttValue();
                            productAttValue.ProductID = row["ID"].ToString();
                            productAttValue.ProductAttributeID = goodsAttr.ID.ToString();
                            productAttValue.ProductAttValueID = attribute.ID.ToString();
                            productAttValue.ProductAttValueName = attribute.AttrsInfoName;

                            ProductAttValueList.Add(productAttValue);
                        }
                        proAttr.ProductAttValueList = ProductAttValueList;
                        ProductAttributeList.Add(proAttr);
                    }

                    #endregion

                    product.ProductAttributeList = ProductAttributeList;

                    ProductList.Add(product);
                }
            }

            #endregion

            return new BD_GoodsCategory.ResultProductList()
            {
                Result = "T",
                Description = "获取成功",
                ClassifyID = categoryID,
                ProductList = ProductList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerProductList ：" + JSon);
            return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "异常" };
        }
    }

    /// <summary>
    /// 图片
    /// </summary>
    /// <param name="goodsID"></param>
    /// <returns></returns>
    public List<BD_GoodsCategory.Pic> GetPicList(string goodsID)
    {
        List<BD_GoodsCategory.Pic> picList = new List<BD_GoodsCategory.Pic>();
        List<Hi.Model.BD_ImageList> imgList = new Hi.BLL.BD_ImageList().GetList("", " dr=0 and GoodsID='" + goodsID + "'", "");
        if (imgList != null && imgList.Count > 0)
        {
            foreach (var img in imgList)
            {
                BD_GoodsCategory.Pic pic = new BD_GoodsCategory.Pic();
                pic.ProductID = goodsID;
                pic.IsDeafult = "0";
                pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" + img.Pic2;
                //pic.PicUrl = ConfigurationManager.AppSettings["AppCompImg"].ToString().Trim() + img.Pic3;
                picList.Add(pic);

            }
        }
        return picList;
    }

    /// <summary>
    /// 促销信息
    /// </summary>
    /// <param name="ProID">促销ID</param>
    /// <param name="GoodInfoID">GoodsInfoID</param>
    /// <returns></returns>
    public BD_GoodsCategory.PromotionInfo GetProInfo(int ProID, int GoodInfoID)
    {
        Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
        if (pro != null)
        {
            string proInfos = string.Empty;

            List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail()
                .GetList("", " ProID=" + pro.ID + " and GoodInfoID ='" + GoodInfoID + "' and dr=0",
                    "");
            if (dList != null && dList.Count > 0)
            {
                if (pro.Type == 0 && pro.ProType == 1)
                {
                    proInfos = "赠品";
                }
                else if (pro.Type == 0 && pro.ProType == 2)
                {
                    proInfos = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                }
                else if (pro.Type == 1 && pro.ProType == 3)
                {
                    proInfos = "商品数量满" + pro.Discount.ToString("0.00") + "赠" +
                               dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                }
                else if (pro.Type == 1 && pro.ProType == 4)
                {
                    proInfos = "商品打折" + pro.Discount.ToString("0.00") + "%";
                }
            }

            return new BD_GoodsCategory.PromotionInfo()
            {
                ProID = ProID.ToString(),
                ProTitle = pro.ProTitle,
                ProInfos = proInfos,

                Tpye = pro.Type.ToString(),
                ProTpye = pro.ProType.ToString(),
                Discount = pro.Discount.ToString("0.00"),

                ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
            };
        }
        else
        {
            return null;
        }
    }
}