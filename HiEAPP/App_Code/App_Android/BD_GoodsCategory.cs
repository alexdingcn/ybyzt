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

public class BD_GoodsCategory
{
    public BD_GoodsCategory()
    {
        //大部分的开发，是和PC端一起边设计边开发代码，所以大多数为垃圾代码。
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
            if (!new Common().IsLegitUser(int.Parse(userID), out user, int.Parse(companyID), int.Parse(disID == "" ? "0" : disID)))
                return new ResultGoodsCategory() { Result = "F", Description = "参数异常" };

            //Hi.Model.BD_Company comp = new Hi.Model.BD_Company();
            //if (companyID.Trim() == "0")//经销商分类：compID传0
            //{
            //    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID.Trim()));
            //    if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
            //        return new ResultGoodsCategory() { Result = "F", Description = "经销商信息异常" };
            //    comp = new Hi.BLL.BD_Company().GetModel(compUser.CompID);
            //}
            //else //企业分类
            //{
            //    comp = new Hi.BLL.BD_Company().GetModel(int.Parse(companyID.Trim()));
            //}
            //if (comp == null || comp.dr == 1 || comp.IsEnabled == 0)
            //    return new ResultGoodsCategory() { Result = "T", Description = "企业异常" };

            string strWhere = string.Empty;
            //strWhere = " CompID='" + comp.ID + "' and ISNULL(dr,0)=0 and IsEnabled = 1 ";
            strWhere = " ISNULL(dr,0)=0 and IsEnabled = 1";
            List<Hi.Model.SYS_GType> list = new Hi.BLL.SYS_GType().GetList("", strWhere, "");
            List<ProductClassify> pList = new List<ProductClassify>();
            ProductClassify classifies = new ProductClassify();
            classifies.ClassifyID = "-1";
            classifies.ClassifyName = "全部分类";
            classifies.ParentID = "0";
            classifies.SortIndex = "0";
            pList.Add(classifies);
            if (list.Count != 0)
            {
                foreach (var bdGoodsCategory in list)
                {
                    ProductClassify pClassifies = new ProductClassify();
                    pClassifies.ClassifyID = bdGoodsCategory.ID.ToString();
                    pClassifies.ClassifyName = bdGoodsCategory.TypeName;
                    pClassifies.ParentID = bdGoodsCategory.ParentId.ToString();
                    pClassifies.SortIndex = bdGoodsCategory.SortIndex;
                    pList.Add(pClassifies);
                }
            }



            return new ResultGoodsCategory()
            {
                Result = "T",
                Description = "获取成功",
                //CompanyID = comp.ID.ToString(),
                //CompanyName = comp.CompName,
                ProductClassifyList = pList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerProductClassifyList ：" + JSon);
            return new ResultGoodsCategory() { Result = "F", Description = "异常" };
        }
    }

    /// <summary>
    /// 获取某一分类产品信息,兼容了无属性的值，分类传-1
    /// 2016 4 19 旧版本
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultProductList GetProductList(string JSon)
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
            //JInfo["ClassifyID"].ToString() != "" &&
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
                return new ResultProductList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,0,int.Parse(disID == "" ? "0" : disID)))
                return new ResultProductList() { Result = "F", Description = "登录信息异常" };

            #endregion

            #region 模拟分页

            string strsql = string.Empty; //搜索sql
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new ResultProductList() { Result = "F", Description = "未找到经销商" };
            const string tabName = " [dbo].[BD_Goods]"; //表名
            sortType = sortType == "2" ? "SalePrice" : "ID"; //价格排序
            string strWhere = string.Empty;

            switch (categoryID)
            {
                case "-2"://促销列表
                    {
                        List<Hi.Model.BD_Promotion> promotionList = new Hi.BLL.BD_Promotion().GetList("",
                            " compID='" + dis.CompID + "' and ProStartTime<='" + DateTime.Now + "' and ProEndTime >='" +
                            DateTime.Now + "' and IsEnabled=1", "");
                        List<Hi.Model.BD_PromotionDetail> detailList = new List<BD_PromotionDetail>();
                        if (promotionList != null && promotionList.Count > 0)
                        {
                            detailList = new Hi.BLL.BD_PromotionDetail().GetList("", " ProID in(" + string.Join(",", promotionList.Select(p => p.ID)) + ")", "");
                        }
                        if (promotionList == null)
                            return new ResultProductList() { Result = "F", Description = "今天无促销" };
                        if (detailList == null)
                            return new ResultProductList() { Result = "F", Description = "今天无促销" };
                        var ienum = detailList.Select(p => p.GoodsID);
                        strWhere += " and ID in ( " + string.Join(",", ienum) + ")";
                    }
                    break;
                case "-3"://收藏列表
                    {
                        List<Hi.Model.BD_DisCollect> collects = new Hi.BLL.BD_DisCollect().GetList("", "disID='" + disID + "' and dr=0", "");
                        if (collects != null)
                        {
                            strWhere += " and ID not in ( -1 ";
                            strWhere = collects.Aggregate(strWhere, (current, goods) => current + ("," + goods.GoodsID)) + ")";
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
            strWhere += " and ISNULL(dr,0)=0 and IsEnabled = 1 and IsOffLine=1 and compid=" + dis.CompID;

            strsql = new Common().PageSqlString(criticalProductID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);

            #endregion

            List<Hi.Model.BD_DisCollect> Colist = Common.GetDataSource<BD_DisCollect>("*", " and disID='" + disID + "' and compID='" + dis.CompID + "' and IsEnabled =1");
            //List<Hi.Model.BD_GoodsInfo> infoList = Common.GetDataSource<BD_GoodsInfo>("*", " and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0 and IsEnabled=1");
            List<Hi.Model.BD_GoodsInfo> infoList = new Hi.BLL.BD_GoodsInfo().GetList("", " CompID='" + dis.CompID + "' and ISNULL(dr,0)=0 and IsEnabled=1 and IsOffLine=1", "");

            #region 赋值

            int CategoryID = 0;

            List<Product> ProductList = new List<Product>();
            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if (dsList != null)
            {
                if (dsList.Rows.Count == 0)
                    return new ResultProductList() { Result = "T", Description = "没有更多数据" };
                foreach (DataRow row in dsList.Rows)
                {
                    string SKUName = string.Empty;
                    Product product = new Product();
                    product.ProductID = row["ID"].ToString(); //商品ID
                    product.ProductName = row["GoodsName"].ToString();
                    SKUName += product.ProductName;
                    decimal salePrice = decimal.Parse(row["SalePrice"].ToString());
                    //if (detailList != null && detailList.Count > 0)
                    //{
                    //    var plist = detailList.Where(p => p.GoodsID.ToString() == product.ProductID).ToList();
                    //    if (plist.Count > 0)
                    //        product.IsPro = "1";
                    //}
                    product.SalePrice = salePrice.ToString("0.00");
                    product.IsSale = row["IsSale"].ToString();

                    List<Hi.Model.BD_DisCollect> alist = Colist != null && Colist.Count > 0 ? Colist.Where(p => p.GoodsID.ToString() == product.ProductID).ToList() : null;
                    product.IsCollect = alist != null && alist.Count > 0 ? "1" : "0";
                    product.Title = row["Title"].ToString();
                    //product.Details = row["Details"].ToString();
                    product.Title = row["Title"].ToString();
                    product.Unit = row["Unit"].ToString();
                    product.ClassifyID = row["CategoryID"].ToString();
                    CategoryID = int.Parse(row["CategoryID"].ToString()); //类别ID

                    List<Pic> Pic = new List<Pic>();

                    #region List<Pic> Pic

                    if (row["Pic"].ToString() != "" && row["Pic"].ToString() != "X")
                    {
                        Pic pic = new Pic();
                        pic.ProductID = row["ID"].ToString();
                        pic.IsDeafult = "1";
                        pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                     row["Pic"].ToString();
                        Pic.Add(pic);
                    }

                    #endregion

                    product.ProductPicUrlList = Pic;

                    List<SKU> SKUList = new List<SKU>();
                    string strID = "0";

                    #region 通过 商品ID和属性值ID关联表，找到属性值

                    string strgoodsAttr = "0";
                    List<Hi.Model.BD_GoodsInfo> goodsInfo = infoList != null && infoList.Count > 0 ? infoList.Where(p => p.GoodsID.ToString() == row["ID"].ToString()).ToList() : null;
                    foreach (Hi.Model.BD_GoodsInfo goods in goodsInfo)
                    {
                        if (!Common.IsOffline(goods.ID))
                            continue;

                        SKU SKU = new SKU();
                        //SKUID是GoodsInfoID,SKUName是GoodsName+各种属性值
                        SKU.SKUID = goods.ID.ToString();
                        SKU.ProductID = goods.GoodsID.ToString();
                        SKU.BarCode = goods.BarCode;
                        //SKUName = GoodsName + ValueInfo
                        SKU.SKUName = SKUName + " " + goods.ValueInfo;

                        SKU.ValueInfo = goods.ValueInfo;
                        SKU.SalePrice = goods.SalePrice.ToString("0.00");

                        int ProID = 0; //暂时未用到 促销ID
                        SKU.IsPro = "0"; //默认不是促销价
                        decimal price = Common.GetProPrice(goods.GoodsID.ToString(), goods.ID.ToString(),
                            goods.CompID.ToString(), out ProID);
                        if (price == 0)
                        {
                            List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                                " GoodsInfoID='" + goods.ID + "' and ISNULL(dr,0)=0 and compid='" + goods.CompID +
                                "' and IsEnabled=1", "");
                            SKU.TinkerPrice = goodsPrice.Count != 0
                                ? goodsPrice[0].TinkerPrice.ToString("0.00")
                                : goods.TinkerPrice.ToString("0.00");
                        }
                        else
                        {
                            SKU.IsPro = "1"; //是促销价
                            Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
                            if (pro != null)
                            {
                                string info = string.Empty;

                                List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail()
                                    .GetList("", " ProID=" + pro.ID + " and GoodInfoID ='" + SKU.SKUID + "' and dr=0", "");
                                if (dList != null && dList.Count > 0)
                                {
                                    if (pro.Type == 0 && pro.ProType == 1)
                                    {
                                        info = "赠品";
                                    }
                                    else if (pro.Type == 0 && pro.ProType == 2)
                                    {
                                        info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                    }
                                    else if (pro.Type == 1 && pro.ProType == 3)
                                    {
                                        info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" + dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                                    }
                                    else if (pro.Type == 1 && pro.ProType == 4)
                                    {
                                        info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                    }
                                }

                                SKU.ProInfo = new PromotionInfo()
                                {
                                    ProID = ProID.ToString(),
                                    ProTitle = pro.ProTitle,
                                    ProInfos = info,

                                    Tpye = pro.Type.ToString(),
                                    ProTpye = pro.ProType.ToString(),
                                    Discount = pro.Discount.ToString("0.00"),

                                    ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                    ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                                };
                            }

                            SKU.TinkerPrice = price.ToString("0.00");
                        }
                        List<ProductAttValueID> ProductAttValueIDList = new List<ProductAttValueID>();

                        List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", " GoodsID='" + goods.GoodsID + "' and ISNULL(dr,0)=0", "");
                        if (attrList == null)
                            return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                        List<Hi.Model.BD_AttributeValues> attrValList = new Hi.BLL.BD_AttributeValues().GetList("*", " CompID ='" + dis.CompID + "' and isenabled = 1", "");
                        foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
                        {
                            strID += "," + attribute.ID;
                            Hi.Model.BD_AttributeValues goodsAttr = attrValList != null && attrValList.Count > 0 ? attrValList.Where(p => p.ID == attribute.ID).ToList()[0] : null;
                            if (goodsAttr == null)
                                return new ResultProductList() { Result = "F", Description = "商品属性异常" };

                            strgoodsAttr += "," + goodsAttr.AttributeID;

                            string[] args = new[] { goods.ValueInfo };
                            string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string item in items)
                            {
                                string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                                if (strs[1] == goodsAttr.AttrValue)
                                {
                                    ProductAttValueID productAttValueID = new ProductAttValueID();
                                    productAttValueID.ProductAttributeValueID = attribute.ID.ToString();
                                    ProductAttValueIDList.Add(productAttValueID);
                                }
                            }
                        }

                        SKU.ProductAttValueIDList = ProductAttValueIDList;
                        SKUList.Add(SKU);
                    }

                    #endregion

                    product.SKUList = SKUList;

                    List<ProductAttribute> ProductAttributeList = new List<ProductAttribute>();

                    #region 通过商品类别ID和属性ID关联表，找到属性ID

                    List<Hi.Model.BD_CategoryAttribute> val = new Hi.BLL.BD_CategoryAttribute().GetList("", " ID in (" + strgoodsAttr + ") and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0", "");
                    if (val == null)
                        return new ResultProductList() { Result = "F", Description = "未找到商品属性" };

                    foreach (Hi.Model.BD_CategoryAttribute goodsAttr in val)
                    {
                        ProductAttribute proAttr = new ProductAttribute();

                        proAttr.ProductID = row["ID"].ToString();
                        proAttr.ProductAttributeID = goodsAttr.AttributeID.ToString(); //属性ID
                        Hi.Model.BD_Attribute attr = new Hi.BLL.BD_Attribute().GetModel(goodsAttr.AttributeID);
                        proAttr.ProductAttributeName = attr.AttributeName; //属性名称

                        List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();
                        List<Hi.Model.BD_AttributeValues> attrList = new Hi.BLL.BD_AttributeValues().GetList("",
                            " AttributeID='" + goodsAttr.ID + "' and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0" +
                            " and ID in (" + strID + ")", "ID"); //todo:商品属性表修改咨询商品结构
                        if (attrList == null)
                            return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                        foreach (Hi.Model.BD_AttributeValues attribute in attrList)
                        {
                            ProductAttValue productAttValue = new ProductAttValue();
                            productAttValue.ProductID = row["ID"].ToString();
                            productAttValue.ProductAttributeID = goodsAttr.AttributeID.ToString();
                            productAttValue.ProductAttValueID = attribute.ID.ToString();
                            productAttValue.ProductAttValueName = attribute.AttrValue;

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

            return new ResultProductList()
            {
                Result = "T",
                Description = "获取成功",
                ClassifyID = categoryID,
                ProductList = ProductList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetProductList : " + JSon);
            return new ResultProductList() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultGoodsList SearchGoods(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string goodsID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["GoodsID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                goodsID = JInfo["GoodsID"].ToString();
            }
            else
            {
                return new ResultGoodsList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultGoodsList() { Result = "F", Description = "用户异常" };
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new ResultGoodsList() { Result = "F", Description = "未找到经销商" };

            #endregion

            #region 模拟分页

            string strWhere = " and ID = '" + goodsID + "' "; //查询条件
            const string tabName = " [dbo].[BD_Goods]"; //表名
            List<Common.GoodsID> list = Common.DisEnAreaGoodsID(disID, dis.CompID.ToString()); //商品可售区域判断
            if (list != null)
            {
                strWhere += " and ID not in ( -1 ";
                strWhere = list.Aggregate(strWhere, (current, goods) => current + ("," + goods.goodsID)) + ")";
            }
            strWhere += " and ISNULL(dr,0)=0 and IsEnabled = 1  and IsOffLine=1 and compid=" + dis.CompID;

            string strsql = new Common().PageSqlString("-1", "ID", tabName, "ID", "0", strWhere, "0", "10");

            #endregion

            #region 赋值

            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if (dsList == null || dsList.Rows.Count == 0)
                return new ResultGoodsList() { Result = "F", Description = "没有更多数据" };
            DataRow row = dsList.Rows[0];

            string SKUName = string.Empty;
            Product product = new Product();
            product.ProductID = row["ID"].ToString();
            product.ProductName = row["GoodsName"].ToString();
            SKUName += product.ProductName;
            //list中的商品价格需要根据bd_goods表中的viewinfoid（商品对应的第一个goodsinfoid）取出此规格属性的价格
            decimal salePrice = BLL.Common.GetGoodsPrice(dis.CompID,Convert.ToInt32(disID),Convert.ToInt32(ClsSystem.gnvl(row["ViewInfoID"],"0")));
            //decimal salePrice = decimal.Parse(row["SalePrice"].ToString());
            product.SalePrice = salePrice.ToString("0.00");
            product.IsSale = row["IsSale"].ToString();
            List<Hi.Model.BD_DisCollect> alist = new Hi.BLL.BD_DisCollect().GetList("",
                " disID='" + disID + "' and compID='" + dis.CompID + "' and goodsID='" + product.ProductID +
                "' and IsEnabled =1", "");
            product.IsCollect = alist.Count > 0 ? "1" : "0";
            product.Title = row["Title"].ToString();
            //product.Details = row["Details"].ToString();
            product.Title = row["Title"].ToString();
            product.Unit = row["Unit"].ToString();

            List<Pic> Pic = new List<Pic>();

            #region List<Pic> Pic

            if (row["Pic"].ToString() != "" && row["Pic"].ToString() != "X")
            {
                Pic pic = new Pic();
                pic.ProductID = row["ID"].ToString();
                pic.IsDeafult = "1";
                pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                             row["Pic"];
                Pic.Add(pic);
            }

            #endregion

            product.ProductPicUrlList = Pic;

            List<SKU> SKUList = new List<SKU>();
            string strID = "0";

            #region 通过 商品ID和属性值ID关联表，找到属性值

            string strgoodsAttr = "0";
            List<Hi.Model.BD_GoodsInfo> goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("",
                " GoodsID='" + row["ID"].ToString() + "' and CompID='" + dis.CompID +
                "' and ISNULL(dr,0)=0 and IsEnabled=1 and isoffline=1", "");
            foreach (Hi.Model.BD_GoodsInfo goods in goodsInfo)
            {
                SKU SKU = new SKU();
                //SKUID是GoodsInfoID,SKUName是GoodsName+各种属性值
                SKU.SKUID = goods.ID.ToString();
                SKU.ProductID = goods.GoodsID.ToString();
                SKU.BarCode = goods.BarCode;
                //SKUName = GoodsName + ValueInfo
                SKU.SKUName = SKUName + " " + goods.ValueInfo;

                SKU.ValueInfo = goods.ValueInfo;
                SKU.SalePrice = goods.SalePrice.ToString();

                int ProID = 0; //暂时未用到 促销ID
                decimal price = Common.GetProPrice(goods.GoodsID.ToString(), goods.ID.ToString(),
                    goods.CompID.ToString(), out ProID);
                //if (price == 0)
                //{
                //    List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                //        " GoodsInfoID='" + goods.ID + "' and ISNULL(dr,0)=0 and compid='" + goods.CompID +
                //        "' and IsEnabled=1", "");
                //    SKU.TinkerPrice = goodsPrice.Count != 0
                //        ? goodsPrice[0].TinkerPrice.ToString()
                //        : goods.TinkerPrice.ToString();
                //}
                //else
                if(price !=0)
                {
                    SKU.IsPro = "1"; //是促销价
                    Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
                    if (pro != null)
                    {
                        string info = string.Empty;

                        List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail()
                            .GetList("", " ProID=" + pro.ID + " and GoodInfoID ='" + SKU.SKUID + "' and dr=0", "");
                        if (dList != null && dList.Count > 0)
                        {
                            if (pro.Type == 0 && pro.ProType == 1)
                            {
                                info = "赠品";
                            }
                            else if (pro.Type == 0 && pro.ProType == 2)
                            {
                                info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                            }
                            else if (pro.Type == 1 && pro.ProType == 3)
                            {
                                info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" + dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                            }
                            else if (pro.Type == 1 && pro.ProType == 4)
                            {
                                info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                            }
                        }

                        SKU.ProInfo = new PromotionInfo()
                        {
                            ProID = ProID.ToString(),
                            ProTitle = pro.ProTitle,
                            ProInfos = info,

                            Tpye = pro.Type.ToString(),
                            ProTpye = pro.ProType.ToString(),
                            Discount = pro.Discount.ToString("0.00"),

                            ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                            ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                        };
                    }
                    //SKU.TinkerPrice = price.ToString();
                }
                SKU.TinkerPrice = BLL.Common.GetGoodsPrice(dis.CompID,Convert.ToInt32(disID),goods.ID).ToString("0.00");
                List<ProductAttValueID> ProductAttValueIDList = new List<ProductAttValueID>();
                List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("",
                    " GoodsID='" + goods.GoodsID + "' and ISNULL(dr,0)=0", "");
                if (attrList == null)
                    return new ResultGoodsList() { Result = "F", Description = "未找到商品属性名字" };
                foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
                {
                    strID += "," + attribute.ID;
                    Hi.Model.BD_AttributeValues goodsAttr =
                        new Hi.BLL.BD_AttributeValues().GetModel(attribute.ID);
                    if (goodsAttr == null)
                        return new ResultGoodsList() { Result = "F", Description = "商品属性异常" };

                    strgoodsAttr += "," + goodsAttr.AttributeID;

                    string[] args = new[] { goods.ValueInfo };
                    string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in items)
                    {
                        string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        //foreach (string str in strs)
                        //{
                        if (strs[1] == goodsAttr.AttrValue)
                        {
                            ProductAttValueID productAttValueID = new ProductAttValueID();
                            productAttValueID.ProductAttributeValueID = attribute.ID.ToString();
                            ProductAttValueIDList.Add(productAttValueID);
                        }
                        //}
                    }
                }

                SKU.ProductAttValueIDList = ProductAttValueIDList;
                SKUList.Add(SKU);
            }

            #endregion

            product.SKUList = SKUList;

            List<ProductAttribute> ProductAttributeList = new List<ProductAttribute>();

            #region 通过商品类别ID和属性ID关联表，找到属性ID

            List<Hi.Model.BD_CategoryAttribute> val = new Hi.BLL.BD_CategoryAttribute().GetList("",
                " ID in (" + strgoodsAttr + ") and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0", "");
            if (val == null)
                return new ResultGoodsList() { Result = "F", Description = "未找到商品属性" };

            foreach (Hi.Model.BD_CategoryAttribute goodsAttr in val)
            {
                ProductAttribute proAttr = new ProductAttribute();

                proAttr.ProductID = row["ID"].ToString();
                proAttr.ProductAttributeID = goodsAttr.AttributeID.ToString(); //属性ID
                Hi.Model.BD_Attribute attr = new Hi.BLL.BD_Attribute().GetModel(goodsAttr.AttributeID);
                proAttr.ProductAttributeName = attr.AttributeName; //属性名称

                List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();
                List<Hi.Model.BD_AttributeValues> attrList = new Hi.BLL.BD_AttributeValues().GetList("",
                    " AttributeID='" + goodsAttr.ID + "' and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0" +
                    " and ID in (" + strID + ")", "ID"); //todo:商品属性表修改咨询商品结构
                if (attrList == null)
                    return new ResultGoodsList() { Result = "F", Description = "未找到商品属性名字" };
                foreach (Hi.Model.BD_AttributeValues attribute in attrList)
                {
                    ProductAttValue productAttValue = new ProductAttValue();
                    productAttValue.ProductID = row["ID"].ToString();
                    productAttValue.ProductAttributeID = goodsAttr.AttributeID.ToString();
                    productAttValue.ProductAttValueID = attribute.ID.ToString();
                    productAttValue.ProductAttValueName = attribute.AttrValue;

                    ProductAttValueList.Add(productAttValue);
                }
                proAttr.ProductAttValueList = ProductAttValueList;
                ProductAttributeList.Add(proAttr);
            }

            #endregion

            product.ProductAttributeList = ProductAttributeList;

            #endregion

            return new ResultGoodsList()
            {
                Result = "T",
                Description = "获取成功",
                Product = product
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.StackTrace + ":" + ex.Message, "SearchGoods : " + JSon);
            return new ResultGoodsList() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 通过GoodsID搜索商品
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultProductList SearchGoodsList(string JSon,string version)
    {
        try
        {
            #region JSon取值
            string disID = string.Empty;
            string goodsID = string.Empty;
            string compID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (version.ToLower() == "android" || version.ToLower() == "ios" || float.Parse(version) < 5)//版本1跟之前版本没有传入核心企业id
            {
                if (JInfo.Count > 0 && JInfo["ResellerID"].ToString() != "" &&
                    JInfo["GoodsID"].ToString() != "")
                {
                    disID = JInfo["ResellerID"].ToString();
                    goodsID = JInfo["GoodsID"].ToString();
                }
                else
                {
                    return new ResultProductList() { Result = "F", Description = "参数异常" };
                }
            }
            else if (float.Parse(version) >= 5)
            {
                if (JInfo.Count > 0 && (JInfo["ResellerID"].ToString() != "" || JInfo["CompanyID"].ToString() != "") &&
    JInfo["GoodsID"].ToString() != "")
                {
                    disID = JInfo["ResellerID"].ToString();
                    goodsID = JInfo["GoodsID"].ToString();
                    compID = JInfo["CompanyID"].ToString();
                }
                else
                {
                    return new ResultProductList() { Result = "F", Description = "参数异常" };
                }
            }

            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(int.Parse(compID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0)
            {
                return new ResultProductList() { Result = "F", Description = "未找到核心企业" };
            }

            int IsInve = Common.rdoOrderAudit("商品是否启用库存", compID.ToInt()).ToInt(0);//判断此核心企业是否启用库存

            #endregion

            #region 模拟分页

            string strWhere = " and ID in (" + goodsID + ") "; //查询条件
            const string tabName = " [dbo].[BD_Goods]"; //表名
            //if (disID != "")//disid不为""时查询经销商，需要判断商品的可销售区域
            //{
            //    List<Common.GoodsID> list = Common.DisEnAreaGoodsID(disID, compID); //商品可售区域判断
            //    if (list != null)
            //    {
            //        strWhere += " and ID not in ( -1 ";
            //        strWhere = list.Aggregate(strWhere, (current, goods) => current + ("," + goods.goodsID)) + ")";
            //    }
            //}
            if (version.ToLower() != "android" && version.ToLower() != "ios"&& version != "0")//除了第一版之后的都需要取出上架跟下架的商品
            {
                strWhere += " and ISNULL(dr,0)=0 and IsEnabled = 1   and compid=" + compID;
            }
            else//第一版只需要取出上架的商品
            {
                strWhere += " and ISNULL(dr,0)=0 and IsEnabled = 1 and IsOffLine=1  and compid=" + compID;
            }

            string strsql = new Common().PageSqlString("-1", "ID", tabName, "ID", "0", strWhere, "0", "10");

            #endregion

            #region 赋值

            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if (dsList == null || dsList.Rows.Count == 0)
                return new ResultProductList() { Result = "F", Description = "没有更多数据" };

            List<Product> productList = new List<Product>();
            List<SKU> listorderby = null;
            foreach (DataRow row in dsList.Rows)
            {
                string SKUName = string.Empty;
                Product product = new Product();
                product.ProductID = row["ID"].ToString();
                string url = ConfigurationManager.AppSettings["url"].ToString();
                url = url + row["ID"].ToString();
                product.url = url;
                product.ProductName = row["GoodsName"].ToString();
                product.InStock = ClsSystem.gnvl(row["IsOffline"], "0");//是否上下架
                //判断是否店铺显示，店铺推荐（店铺推荐了，必须店铺显示，相反店铺没显示，就不能店铺推荐）
                string IsRecommended = ClsSystem.gnvl(row["IsRecommended"], "0");
                if (IsRecommended == "0")
                {
                    product.IsShop = "0";
                    product.IsRecommend = "0";
                }
                else if (IsRecommended == "2")
                {
                    product.IsShop = "1";
                    product.IsRecommend = "1";
                }
                else
                {
                    product.IsShop = "1";
                    product.IsRecommend = "0";
                }
                //获取排序号
                product.SortIndex = ClsSystem.gnvl(row["IsIndex"], "");
                //获取分类编码跟分类名称
                product.ClassifyID = row["CategoryID"].ToString();
                Hi.Model.BD_GoodsCategory category = new Hi.BLL.BD_GoodsCategory().GetModel(Int32.Parse(row["CategoryID"].ToString()));
                if (category != null)
                    product.ClassifyName = category.CategoryName;
                else
                    product.ClassifyName = "";
               
                //获取商品标签
                List<Hi.Model.BD_GoodsLabels> list_goodslables = new Hi.BLL.BD_GoodsLabels().GetList("LabelName", "GoodsID=" + row["ID"] + " and isnull(dr,0)=0", "");
                List<GoodsSpan> list_goodsspan = new List<GoodsSpan>();
                foreach (Hi.Model.BD_GoodsLabels span in list_goodslables)
                {
                    GoodsSpan goodsspan = new GoodsSpan();
                    goodsspan.GoodsSpanValue = span.LabelName;
                    list_goodsspan.Add(goodsspan);
                }
                product.GoodsSpanList = list_goodsspan;
                //获取属性模板ID
                product.TemplateID = ClsSystem.gnvl(row["TemplateId"], "");
                product.ts = row["ts"].ToString();
 

                SKUName += product.ProductName;
                //list中的商品价格应该根据bd_goods表中的viewinfoid（该商品对应的第一个goodsinfoid）的此规格属性的价格
                decimal salePrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(compID),disID==""?0:Convert.ToInt32(disID),Convert.ToInt32(ClsSystem.gnvl(row["ViewInfoID"],"0")));
                //decimal salePrice = decimal.Parse(row["SalePrice"].ToString());
                product.SalePrice = salePrice.ToString("0.00");
                product.IsSale = row["IsSale"].ToString();
                if (disID != "")//查询经销商的话，才要查询此商品是否收藏
                {
                    List<Hi.Model.BD_DisCollect> alist = new Hi.BLL.BD_DisCollect().GetList("",
                        " disID=" + disID + " and compID=" + compID + " and goodsID=" + product.ProductID +
                        " and IsEnabled =1", "");
                    product.IsCollect = alist.Count > 0 ? "1" : "0";
                }
                product.Title =ClsSystem.gnvl(row["Title"],"");
                product.Details = ClsSystem.gnvl(row["Details"].ToString().Replace("\"/Kindeditor/", "\"https://one.yibanjf.com/Kindeditor/"), "");
                //product.Title = row["Title"].ToString();
                product.Unit = row["Unit"].ToString();


                #region List<Pic> Pic

                List<Pic> Pic = new List<Pic>();
                List<Hi.Model.BD_ImageList> imgList = new Hi.BLL.BD_ImageList().GetList("", " dr=0 and GoodsID='" + goodsID + "'", "");
                if (imgList != null && imgList.Count > 0)
                {
                    foreach (var img in imgList)
                    {
                        Pic pic = new Pic();
                        pic.ProductID = row["ID"].ToString();
                        pic.IsDeafult = "0";
                        pic.PicUrl = Common.GetPicURL(img.Pic, "resize400", int.Parse(compID));
                        Pic.Add(pic);
                    }
                }

             
                //if (row["Pic2"].ToString() != "" && row["Pic2"].ToString() != "X")
                //{
                //    Pic pic = new Pic();
                //    pic.ProductID = row["ID"].ToString();
                //    pic.IsDeafult = "0";
                //    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                //                 row["Pic2"];
                //    Pic.Add(pic);
                //}
                ////图片三
                //if (row["Pic3"].ToString() != "" && row["Pic3"].ToString() != "X")
                //{
                //    Pic pic = new Pic();
                //    pic.ProductID = row["ID"].ToString();
                //    pic.IsDeafult = "0";
                //    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                //                 row["Pic3"];
                //    Pic.Add(pic);
                //}

                #endregion

                product.ProductPicUrlList = Pic;

                List<SKU> SKUList = new List<SKU>();
                string strID = "0";

                #region 通过 商品ID和属性值ID关联表，找到属性值

                string strgoodsAttr = "0";
                List<Hi.Model.BD_GoodsInfo> goodsInfo = new List<Hi.Model.BD_GoodsInfo>();
                if (version.ToLower() != "android" && version.ToLower() != "ios" && version != "0")//除了第一版之后的版本都需要取出上架跟下架的商品的SKU
                {
                    goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("",
    " GoodsID=" + row["ID"].ToString() + " and CompID=" + compID +
    " and IsEnabled=1 and isnull(dr,0) =0 ", "");
                }
                else
                {
                    goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("",
                        " GoodsID=" + row["ID"].ToString() + " and CompID=" + compID +
                        " and IsEnabled=1 and isoffline=1 and isnull(dr,0) =0 ", "");
                }
                string maxprice = "";//商品最高价
                string minprice = "";//商品最低价
                decimal suminventory = 0;//商品所有SKU的库存总和
                foreach (Hi.Model.BD_GoodsInfo goods in goodsInfo)
                {
                    SKU SKU = new SKU();
                    //SKUID是GoodsInfoID,SKUName是GoodsName+各种属性值
                    SKU.SKUID = goods.ID.ToString();
                    SKU.ProductID = goods.GoodsID.ToString();
                    SKU.BarCode = goods.BarCode;
                    //SKUName = GoodsName + ValueInfo
                    //版本五之前的skuname是商品名称，属性值拼接成sku名称，版本五及以后版本skuname只需要给商品名称
                    if (version.ToLower() != "android" && version.ToLower() != "ios" && float.Parse(version) >= 5)
                    {
                        SKU.SKUName = SKUName;
                    }
                    else
                    {
                        SKU.SKUName = SKUName + " " + goods.ValueInfo;
                    }
                    SKU.InStock = ClsSystem.gnvl(goods.IsOffline, "0");
                    if (disID != "" && !Common.getGoodInfoID(compID, disID, goods.ID.ToString()))
                        SKU.InStock = "0";
                    SKU.ValueInfo = goods.ValueInfo;
                    SKU.SalePrice = goods.SalePrice.ToString("0.00");
                    SKU.Delete = goods.dr.ToString();
                    //取销量
                    string salenum = Common.GetSaleNum(goods.ID);
                    if (salenum == "")
                        return new ResultProductList() { Result ="F",Description = "参数异常"};
                    SKU.SaleNum = salenum;
                    //获取特定属性商品的库存
                    if (IsInve != 0 && version.ToLower() != "android" && version.ToLower() != "ios" )
                    {
                        SKU.Inventory = "-1";
                        //SKU.Inventory = goods.Inventory.ToString();
                       
                    }
                    else
                    {
                        SKU.Inventory = goods.Inventory.ToString();
                        suminventory += goods.Inventory;
                    }
                    if (disID != "")
                    {
                        int ProID = 0; //暂时未用到 促销ID            
                        SKU.IsPro = "0"; //默认不是促销价
                        decimal price = Common.GetProPrice(goods.GoodsID.ToString(), goods.ID.ToString(),
                            goods.CompID.ToString(), out ProID);
                        //if (price == 0)
                        //{
                        //    List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                        //        " GoodsInfoID=" + goods.ID + " and DisID= " + disID + " and  ISNULL(dr,0)=0 and compid=" + goods.CompID +
                        //        " and IsEnabled=1", "");
                        //    SKU.TinkerPrice = goodsPrice.Count != 0
                        //        ? goodsPrice[0].TinkerPrice.ToString()
                        //        : goods.TinkerPrice.ToString();
                        //}
                        if(price!=0)
                        {
                            SKU.IsPro = "1"; //是促销价
                            Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
                            if (pro != null)
                            {
                                string info = string.Empty;

                                List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail()
                                    .GetList("", " ProID=" + pro.ID + " and GoodInfoID =" + SKU.SKUID + " and dr=0",
                                        "");
                                if (dList != null && dList.Count > 0)
                                {
                                    if (pro.Type == 0 && pro.ProType == 1)
                                    {
                                        info = "赠品";
                                    }
                                    else if (pro.Type == 0 && pro.ProType == 2)
                                    {
                                        info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                    }
                                    else if (pro.Type == 1 && pro.ProType == 3)
                                    {
                                        info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" +
                                               dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                                    }
                                    else if (pro.Type == 1 && pro.ProType == 4)
                                    {
                                        info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                    }
                                }

                                SKU.ProInfo = new PromotionInfo()
                                {
                                    ProID = ProID.ToString(),
                                    ProTitle = pro.ProTitle,
                                    ProInfos = info,

                                    Tpye = pro.Type.ToString(),
                                    ProTpye = pro.ProType.ToString(),
                                    Discount = pro.Discount.ToString("0.00"),

                                    ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                    ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                                };
                            }
                            //SKU.TinkerPrice = price.ToString();
                        }
                        SKU.TinkerPrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(compID), Convert.ToInt32(disID), goods.ID).ToString("0.00");
                    }
                    else
                    {
                        SKU.TinkerPrice = goods.TinkerPrice.ToString("0.00");
                    }
                    List<ProductAttValueID> ProductAttValueIDList = new List<ProductAttValueID>();

                    //之前代码，由于表BD_AttributeValues不用了，代码就存在问题
                    //List<Hi.Model.BD_GoodsAttrValues> attrList = new Hi.BLL.BD_GoodsAttrValues().GetList("",
                    //    " GoodsID='" + goods.GoodsID + "' and ISNULL(dr,0)=0", "");
                    //if (attrList == null)
                    //    return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                    //foreach (Hi.Model.BD_GoodsAttrValues attribute in attrList)
                    //{
                    //    strID += "," + attribute.ValuesID;
                    //    Hi.Model.BD_AttributeValues goodsAttr =
                    //        new Hi.BLL.BD_AttributeValues().GetModel(attribute.ValuesID);
                    //    if (goodsAttr == null)
                    //        return new ResultProductList() { Result = "F", Description = "商品属性异常" };

                    //    strgoodsAttr += "," + goodsAttr.AttributeID;

                    //    string[] args = new[] {goods.ValueInfo};
                    //    string[] items = args[0].Split(new char[] {'；'}, StringSplitOptions.RemoveEmptyEntries);
                    //    foreach (string item in items)
                    //    {
                    //        string[] strs = item.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                    //        //foreach (string str in strs)
                    //        //{
                    //        if (strs[1] == goodsAttr.AttrValue)
                    //        {
                    //            ProductAttValueID productAttValueID = new ProductAttValueID();
                    //            productAttValueID.ProductAttributeID = attribute.ValuesID.ToString();
                    //            ProductAttValueIDList.Add(productAttValueID);
                    //        }
                    //        //}
                    //    }
                    //}

                    //新写的代码，查表BD_GoodsAttrsInfo取出属性值id
                    List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", " GoodsID=" + goods.GoodsID + " and ISNULL(dr,0)=0", "");
                    foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
                    {
                        strID += "," + attribute.AttrsID;
                        string[] args = new[] { goods.ValueInfo };
                        string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in items)
                        {
                            string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                            if (strs[1] == attribute.AttrsInfoName)
                            {
                                ProductAttValueID productAttValueID = new ProductAttValueID()
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
                //对skulist根据TinkerPrice进行排序
                SKUList.OrderBy(x => x.TinkerPrice);
                minprice = SKUList[0].TinkerPrice;
                maxprice = SKUList[SKUList.Count - 1].TinkerPrice;
                product.MaxPrice = maxprice;
                product.MinPrice = minprice;
                //商品所有SKU的总库存,不开启库存管理传-1
                if (IsInve != 0)//未开启库存管理
                {
                    product.Inventory = "-1";
                }
                else
                {
                    product.Inventory = suminventory.ToString();
                }
                List<ProductAttribute> ProductAttributeList = new List<ProductAttribute>();

                #region 通过商品类别ID和属性ID关联表，找到属性ID

                //List<Hi.Model.BD_CategoryAttribute> val = new Hi.BLL.BD_CategoryAttribute().GetList("",
                //    " ID in (" + strgoodsAttr + ") and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0", "");
                //if (val == null)
                //    return new ResultProductList() { Result = "F", Description = "未找到商品属性" };

                //foreach (Hi.Model.BD_CategoryAttribute goodsAttr in val)
                //{
                //    ProductAttribute proAttr = new ProductAttribute();

                //    proAttr.ProductID = row["ID"].ToString();
                //    proAttr.ProductAttributeID = goodsAttr.AttributeID.ToString(); //属性ID
                //    Hi.Model.BD_Attribute attr = new Hi.BLL.BD_Attribute().GetModel(goodsAttr.AttributeID);
                //    proAttr.ProductAttributeName = attr.AttributeName; //属性名称

                //    List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();
                //    List<Hi.Model.BD_AttributeValues> attrList = new Hi.BLL.BD_AttributeValues().GetList("",
                //        " AttributeID='" + goodsAttr.ID + "' and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0" +
                //        " and ID in (" + strID + ")", "ID"); //todo:商品属性表修改咨询商品结构
                //    if (attrList == null)
                //        return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                //    foreach (Hi.Model.BD_AttributeValues attribute in attrList)
                //    {
                //        ProductAttValue productAttValue = new ProductAttValue();
                //        productAttValue.ProductID = row["ID"].ToString().ToString();
                //        productAttValue.ProductAttributeID = goodsAttr.AttributeID.ToString();
                //        productAttValue.ProductAttValueID = attribute.ID.ToString();
                //        productAttValue.ProductAttValueName = attribute.AttrValue;

                //        ProductAttValueList.Add(productAttValue);
                //    }
                //    proAttr.ProductAttValueList = ProductAttValueList;
                //    ProductAttributeList.Add(proAttr);
                //}
                //ProductAttribute productattr = new ProductAttribute();
                strsql = "select ID,AttrsName from BD_GoodsAttrs where goodsid = " + row["ID"].ToString() + " and isnull(dr,0) =0 and compid = " + compID + "";
                DataTable dt_attr = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                for (int i = 0; i < dt_attr.Rows.Count; i++)
                {
                    ProductAttribute productattr = new ProductAttribute();
                    productattr.ProductID = row["ID"].ToString();
                    productattr.ProductAttributeID = ClsSystem.gnvl(dt_attr.Rows[i][0], "");
                    productattr.ProductAttributeName = ClsSystem.gnvl(dt_attr.Rows[i][1], "");
                    List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();

                    strsql = "select ID,AttrsInfoName from BD_GoodsAttrsInfo where attrsid = " + dt_attr.Rows[i][0] + " and goodsid = " + row["ID"].ToString() + " ";
                    strsql += " and isnull(dr,0) = 0 and compid = " + compID + "";
                    DataTable dt_attrinfo = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);

                    for (int j = 0; j < dt_attrinfo.Rows.Count; j++)
                    {
                        ProductAttValue productattvalue = new ProductAttValue();
                        productattvalue.ProductID = row["ID"].ToString();
                        productattvalue.ProductAttributeID = ClsSystem.gnvl(dt_attr.Rows[i][0], "");
                        productattvalue.ProductAttValueID = ClsSystem.gnvl(dt_attrinfo.Rows[j][0], "");
                        productattvalue.ProductAttValueName = ClsSystem.gnvl(dt_attrinfo.Rows[j][1], "");
                        ProductAttValueList.Add(productattvalue);

                    }
                    productattr.ProductAttValueList = ProductAttValueList;
                    ProductAttributeList.Add(productattr);
                }

                #endregion

                product.ProductAttributeList = ProductAttributeList;

                productList.Add(product);
            }

            #endregion

            return new ResultProductList()
            {
                Result = "T",
                Description = "获取成功",
                ProductList = productList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.StackTrace + ":" + ex.Message, "SearchGoodsList : " + JSon);
            return new ResultProductList() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 核心企业通过商品名称搜索商品
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultProductList SearchProductList(string JSon,string version)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string productName = string.Empty;
            string criticalProductID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;
            string compID = string.Empty;
            string strWhere = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (version.ToLower() == "android" || version.ToLower() == "ios" || float.Parse(version) < 5)//版本1跟之前版本没有传入核心企业id
            {

                if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                    JInfo["CriticalProductID"].ToString() != "" && JInfo["ProductName"].ToString() != "" &&
                    JInfo["GetType"].ToString() != "" && JInfo["Rows"].ToString() != "" &&
                    JInfo["SortType"].ToString() != "" && JInfo["Sort"].ToString() != "")
                {
                    userID = JInfo["UserID"].ToString();
                    disID = JInfo["ResellerID"].ToString();
                    productName = JInfo["ProductName"].ToString();
                    criticalProductID = JInfo["CriticalProductID"].ToString();
                    getType = JInfo["GetType"].ToString();
                    rows = JInfo["Rows"].ToString();
                    sortType = JInfo["SortType"].ToString();
                    sort = JInfo["Sort"].ToString();                
                }
                else
                {
                    return new ResultProductList() { Result = "F", Description = "参数异常" };
                }
            }
            else if (float.Parse(version) >= 5)//版本3及以上的
            {
                if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && (JInfo["ResellerID"].ToString() != "" || JInfo["CompID"].ToString() != "") &&
    JInfo["CriticalProductID"].ToString() != "" && JInfo["ProductName"].ToString() != "" &&
    JInfo["GetType"].ToString() != "" && JInfo["Rows"].ToString() != "" &&
    JInfo["SortType"].ToString() != "" && JInfo["Sort"].ToString() != "")
                {
                    userID = JInfo["UserID"].ToString();
                    disID = JInfo["ResellerID"].ToString();
                    productName = JInfo["ProductName"].ToString();
                    criticalProductID = JInfo["CriticalProductID"].ToString();
                    getType = JInfo["GetType"].ToString();
                    rows = JInfo["Rows"].ToString();
                    sortType = JInfo["SortType"].ToString();
                    sort = JInfo["Sort"].ToString();
                    compID = JInfo["CompanyID"].ToString();
                }
                else
                {
                    return new ResultProductList() { Result = "F", Description = "参数异常" };
                }
            }
            if (disID == "")//disid是""的，就是查询核心企业的，判断核心企业信息是否正确
            {
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(userID), out one, int.Parse(compID == "" ? "0" : compID)))
                    return new ResultProductList() { Result = "F", Description = "登录信息异常" };
                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(int.Parse(compID));
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0)
                {
                    return new ResultProductList() { Result = "F", Description = "未找到核心企业" };
                }
            }
            else//查询经销商，判断经销商信息是否正确
            {
                Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(userID), out user,0, int.Parse(disID == "" ? "0" : disID)))
                    return new ResultProductList() { Result = "F", Description = "登录信息异常" };
                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
                if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                    return new ResultProductList() { Result = "F", Description = "未找到经销商" };
                //compID = dis.CompID.ToString();
            }

            #endregion

            #region 模拟分页

             strWhere = " and GoodsName like '%" + productName + "%' ";//查询条件
            const string tabName = " [dbo].[BD_Goods]"; //表名
            sortType = sortType == "2" ? "SalePrice" : "ID"; //价格排序
            //如果是查询经销商的需要判断可销售区域
            if (disID != "")
            {
                List<Common.GoodsID> list = Common.DisEnAreaGoodsID(disID, compID);//商品可售区域判断
                if (list != null)
                {
                    strWhere += " and ID not in ( -1 ";
                    strWhere = list.Aggregate(strWhere, (current, goods) => current + ("," + goods.goodsID)) + ")";
                }
            }
            strWhere += " and ISNULL(dr,0)=0 and IsEnabled = 1  and IsOffLine=1 and compid=" + compID;

            string strsql = new Common().PageSqlString(criticalProductID, "ID", tabName, sortType, sort, strWhere, getType, rows);

            #endregion

            #region 赋值

            List<Product> ProductList = new List<Product>();
            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if (dsList != null)
            {
                if (dsList.Rows.Count == 0)
                    return new ResultProductList() { Result = "F", Description = "没有更多数据" };
                foreach (DataRow row in dsList.Rows)
                {
                    string SKUName = string.Empty;
                    Product product = new Product();
                    product.ProductID = row["ID"].ToString(); //商品ID
                    product.ProductName = row["GoodsName"].ToString();
                    SKUName += product.ProductName;
                    //list的商品价格需要根据bd_goods表中的viewinfoid（第一个skuid）取出价格
                    
                    decimal salePrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(compID),disID==""?0:Convert.ToInt32(disID),Convert.ToInt32(ClsSystem.gnvl(row["ViewInfoID"],"0")));
                    //decimal salePrice = decimal.Parse(row["SalePrice"].ToString());
                    product.SalePrice = salePrice.ToString("0.00");
                    product.IsSale = row["IsSale"].ToString();
                    List<Hi.Model.BD_DisCollect> alist = new Hi.BLL.BD_DisCollect().GetList("",
                        " disID='" + disID + "' and compID='" + compID + "' and goodsID='" + product.ProductID +
                        "' and IsEnabled =1", "");
                    product.IsCollect = alist.Count > 0 ? "1" : "0";
                    product.Title = row["Title"].ToString();
                    //product.Details = row["Details"].ToString();
                    //product.Title = row["Title"].ToString();
                    product.Unit = row["Unit"].ToString();

                    List<Pic> Pic = new List<Pic>();

                    #region List<Pic> Pic

                    if (row["Pic"].ToString() != "" && row["Pic"].ToString() != "X")
                    {
                        Pic pic = new Pic();
                        pic.ProductID = row["ID"].ToString();
                        pic.IsDeafult = "1";
                        pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                     row["Pic"].ToString();
                        Pic.Add(pic);
                    }

                    #endregion

                    product.ProductPicUrlList = Pic;

                    List<SKU> SKUList = new List<SKU>();
                    string strID = "0";

                    #region 通过 商品ID和属性值ID关联表，找到属性值

                    string strgoodsAttr = "0";
                    List<Hi.Model.BD_GoodsInfo> goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("",
                        " GoodsID='" + row["ID"].ToString() + "' and CompID='" + compID +
                        "' and ISNULL(dr,0)=0 and IsEnabled=1 and isoffline=1", "");
                    foreach (Hi.Model.BD_GoodsInfo goods in goodsInfo)
                    {
                        SKU SKU = new SKU();
                        //SKUID是GoodsInfoID,SKUName是GoodsName+各种属性值
                        SKU.SKUID = goods.ID.ToString();
                        SKU.ProductID = goods.GoodsID.ToString();
                        SKU.BarCode = goods.BarCode;
                        //SKUName = GoodsName + ValueInfo
                        SKU.SKUName = SKUName + " " + goods.ValueInfo;

                        SKU.ValueInfo = goods.ValueInfo;
                        SKU.SalePrice = goods.SalePrice.ToString();
                        if (disID != "")
                        {
                            int ProID = 0; //暂时未用到 促销ID
                            decimal price = Common.GetProPrice(goods.GoodsID.ToString(), goods.ID.ToString(),
                                goods.CompID.ToString(), out ProID);
                            //if (price == 0)
                            //{
                            //    List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                            //        " GoodsInfoID=" + goods.ID + " and  DisID = " + disID + " and ISNULL(dr,0)=0 and compid=" + goods.CompID +
                            //        " and IsEnabled=1", "");
                            //    SKU.TinkerPrice = goodsPrice.Count != 0
                            //        ? goodsPrice[0].TinkerPrice.ToString()
                            //        : goods.TinkerPrice.ToString();
                            //}
                            //else
                            if(price !=0)
                            {
                                SKU.IsPro = "1"; //是促销价
                                Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
                                if (pro != null)
                                {
                                    string info = string.Empty;

                                    List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail()
                                        .GetList("", " ProID=" + pro.ID + " and GoodInfoID ='" + SKU.SKUID + "' and dr=0", "");
                                    if (dList != null && dList.Count > 0)
                                    {
                                        if (pro.Type == 0 && pro.ProType == 1)
                                        {
                                            info = "赠品";
                                        }
                                        else if (pro.Type == 0 && pro.ProType == 2)
                                        {
                                            info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                        }
                                        else if (pro.Type == 1 && pro.ProType == 3)
                                        {
                                            info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" + dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                                        }
                                        else if (pro.Type == 1 && pro.ProType == 4)
                                        {
                                            info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                        }
                                    }

                                    SKU.ProInfo = new PromotionInfo()
                                    {
                                        ProID = ProID.ToString(),
                                        ProTitle = pro.ProTitle,
                                        ProInfos = info,

                                        Tpye = pro.Type.ToString(),
                                        ProTpye = pro.ProType.ToString(),
                                        Discount = pro.Discount.ToString("0.00"),

                                        ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                        ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                                    };
                                }

                                //SKU.TinkerPrice = price.ToString();
                            }
                            SKU.TinkerPrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(compID),Convert.ToInt32(disID),goods.ID).ToString("0.00");
                        }
                        List<ProductAttValueID> ProductAttValueIDList = new List<ProductAttValueID>();
                        List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("",
                            " GoodsID='" + goods.GoodsID + "' and ISNULL(dr,0)=0", "");
                        if (attrList == null)
                            return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                        foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
                        {
                            strID += "," + attribute.ID;
                            Hi.Model.BD_GoodsAttrsInfo goodsAttr =
                                new Hi.BLL.BD_GoodsAttrsInfo().GetModel(attribute.ID);
                            if (goodsAttr == null)
                                return new ResultProductList() { Result = "F", Description = "商品属性异常" };

                            strgoodsAttr += "," + goodsAttr.AttrsID;

                            string[] args = new[] { goods.ValueInfo };
                            string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string item in items)
                            {
                                string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                //foreach (string str in strs)
                                //{
                                if (strs[1] == goodsAttr.AttrsInfoName)
                                {
                                    ProductAttValueID productAttValueID = new ProductAttValueID();
                                    productAttValueID.ProductAttributeValueID = attribute.ID.ToString();
                                    ProductAttValueIDList.Add(productAttValueID);
                                }
                                //}
                            }
                        }

                        SKU.ProductAttValueIDList = ProductAttValueIDList;
                        SKUList.Add(SKU);
                    }

                    #endregion

                    product.SKUList = SKUList;

                    List<ProductAttribute> ProductAttributeList = new List<ProductAttribute>();

                    #region 通过商品类别ID和属性ID关联表，找到属性ID

                    List<Hi.Model.BD_CategoryAttribute> val = new Hi.BLL.BD_CategoryAttribute().GetList("",
                        " ID in (" + strgoodsAttr + ") and CompID='" + compID + "' and ISNULL(dr,0)=0", "");
                    if (val == null)
                        return new ResultProductList() { Result = "F", Description = "未找到商品属性" };

                    foreach (Hi.Model.BD_CategoryAttribute goodsAttr in val)
                    {
                        ProductAttribute proAttr = new ProductAttribute();

                        proAttr.ProductID = row["ID"].ToString();
                        proAttr.ProductAttributeID = goodsAttr.AttributeID.ToString(); //属性ID
                        Hi.Model.BD_Attribute attr = new Hi.BLL.BD_Attribute().GetModel(goodsAttr.AttributeID);
                        proAttr.ProductAttributeName = attr.AttributeName; //属性名称

                        List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();
                        List<Hi.Model.BD_AttributeValues> attrList = new Hi.BLL.BD_AttributeValues().GetList("",
                            " AttributeID='" + goodsAttr.ID + "' and CompID='" + compID + "' and ISNULL(dr,0)=0" +
                            " and ID in (" + strID + ")", "ID"); //todo:商品属性表修改咨询商品结构
                        if (attrList == null)
                            return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                        foreach (Hi.Model.BD_AttributeValues attribute in attrList)
                        {
                            ProductAttValue productAttValue = new ProductAttValue();
                            productAttValue.ProductID = row["ID"].ToString();
                            productAttValue.ProductAttributeID = goodsAttr.AttributeID.ToString();
                            productAttValue.ProductAttValueID = attribute.ID.ToString();
                            productAttValue.ProductAttValueName = attribute.AttrValue;

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

            return new ResultProductList()
            {
                Result = "T",
                Description = "获取成功",
                ProductList = ProductList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.StackTrace + ":" + ex.Message, "SearchProductList : " + JSon);
            return new ResultProductList() { Result = "F", Description = "参数异常" };
        }
    }


    /// <summary>
    /// 经销商通过GoodsID搜索商品
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultProductList ResellerSearchGoodsList(string JSon, string version)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string goodsID = string.Empty;
            string compID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
JInfo["GoodsID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                goodsID = JInfo["GoodsID"].ToString();
                compID = JInfo["CompanyID"].ToString();
            }
            else
            {
                return new ResultProductList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultProductList() { Result = "F", Description = "登录信息异常" };
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new ResultProductList() { Result = "F", Description = "未找到经销商" };
            //compID = dis.CompID.ToString();



            int IsInve = Common.rdoOrderAudit("商品是否启用库存", compID.ToInt()).ToInt(0);//判断此核心企业是否启用库存

            #endregion

            #region 模拟分页

            string strWhere = " and ID in (" + goodsID + ") "; //查询条件
            const string tabName = " [dbo].[BD_Goods]"; //表名
            if (disID != "")//disid不为""时查询经销商，需要判断商品的可销售区域
            {
                List<Common.GoodsID> list = Common.DisEnAreaGoodsID(disID, compID); //商品可售区域判断
                if (list != null)
                {
                    strWhere += " and ID not in ( -1 ";
                    strWhere = list.Aggregate(strWhere, (current, goods) => current + ("," + goods.goodsID)) + ")";
                }
            }
            if (version.ToLower() != "android" && version.ToLower() != "ios" && version != "0")//除了第一版之后的都需要取出上架跟下架的商品
            {
                strWhere += " and ISNULL(dr,0)=0 and IsEnabled = 1   and compid=" + compID;
            }
            else//第一版只需要取出上架的商品
            {
                strWhere += " and ISNULL(dr,0)=0 and IsEnabled = 1 and IsOffLine=1  and compid=" + compID;
            }

            string strsql = new Common().PageSqlString("-1", "ID", tabName, "ID", "0", strWhere, "0", "10");

            #endregion

            #region 赋值

            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if (dsList == null || dsList.Rows.Count == 0)
                return new ResultProductList() { Result = "F", Description = "没有更多数据" };

            List<Product> productList = new List<Product>();
            List<SKU> listorderby = null;
            foreach (DataRow row in dsList.Rows)
            {
                string SKUName = string.Empty;
                Product product = new Product();
                product.ProductID = row["ID"].ToString();
                string url = ConfigurationManager.AppSettings["url"].ToString();
                url = url + row["ID"].ToString();
                product.url = url;
                product.ProductName = row["GoodsName"].ToString();
                product.InStock = ClsSystem.gnvl(row["IsOffline"], "0");//是否上下架
                //判断是否店铺显示，店铺推荐（店铺推荐了，必须店铺显示，相反店铺没显示，就不能店铺推荐）
                string IsRecommended = ClsSystem.gnvl(row["IsRecommended"], "0");
                if (IsRecommended == "0")
                {
                    product.IsShop = "0";
                    product.IsRecommend = "0";
                }
                else if (IsRecommended == "2")
                {
                    product.IsShop = "1";
                    product.IsRecommend = "1";
                }
                else
                {
                    product.IsShop = "1";
                    product.IsRecommend = "0";
                }
                //获取排序号
                product.SortIndex = ClsSystem.gnvl(row["IsIndex"], "");
                //获取分类编码跟分类名称
                product.ClassifyID = row["CategoryID"].ToString();
                Hi.Model.BD_GoodsCategory category = new Hi.BLL.BD_GoodsCategory().GetModel(Int32.Parse(row["CategoryID"].ToString()));
                if (category!=null)
                    product.ClassifyName = category.CategoryName;

                //获取商品标签
                List<Hi.Model.BD_GoodsLabels> list_goodslables = new Hi.BLL.BD_GoodsLabels().GetList("LabelName", "GoodsID=" + row["ID"] + " and isnull(dr,0)=0", "");
                List<GoodsSpan> list_goodsspan = new List<GoodsSpan>();
                foreach (Hi.Model.BD_GoodsLabels span in list_goodslables)
                {
                    GoodsSpan goodsspan = new GoodsSpan();
                    goodsspan.GoodsSpanValue = span.LabelName;
                    list_goodsspan.Add(goodsspan);
                }
                product.GoodsSpanList = list_goodsspan;
                //获取属性模板ID
                product.TemplateID = ClsSystem.gnvl(row["TemplateId"], "");
                product.ts = row["ts"].ToString();


                SKUName += product.ProductName;
                //list中的商品价格应该根据bd_goods表中的viewinfoid（该商品对应的第一个goodsinfoid）的此规格属性的价格
                decimal salePrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(compID), disID == "" ? 0 : Convert.ToInt32(disID), Convert.ToInt32(ClsSystem.gnvl(row["ViewInfoID"], "0")));
                //decimal salePrice = decimal.Parse(row["SalePrice"].ToString());
                product.SalePrice = salePrice.ToString("0.00");
                product.IsSale = row["IsSale"].ToString();

                List<Hi.Model.BD_DisCollect> alist = new Hi.BLL.BD_DisCollect().GetList("",
     " disID=" + disID + " and compID=" + compID + " and goodsID=" + product.ProductID +
     " and IsEnabled =1", "");
                product.IsCollect = alist.Count > 0 ? "1" : "0";

                product.Title = ClsSystem.gnvl(row["Title"], "");

                //product.Details = ClsSystem.gnvl(row["Details"], "");
                product.Details = ClsSystem.gnvl(row["Details"].ToString().Replace("\"/Kindeditor/", "\"https://one.yibanjf.com/Kindeditor/"), "");
                //product.Title = row["Title"].ToString();
                product.Unit = row["Unit"].ToString();

                List<Pic> Pic = new List<Pic>();

                #region List<Pic> Pic

                if (row["Pic"].ToString() != "" && row["Pic"].ToString() != "X")
                {
                    Pic pic = new Pic();
                    pic.ProductID = row["ID"].ToString();
                    pic.IsDeafult = "1";
                    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                 row["Pic"];
                    Pic.Add(pic);

                }
                //Pic.AddRange(GetPicList(row["ID"].ToString()));
                BD_Goods bd_goods = new BD_Goods();
                Pic.AddRange(bd_goods.GetPicList(row["ID"].ToString()));
                //图片二
                //if (row["Pic2"].ToString() != "" && row["Pic2"].ToString() != "X")
                //{
                //    Pic pic = new Pic();
                //    pic.ProductID = row["ID"].ToString();
                //    pic.IsDeafult = "0";
                //    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                //                 row["Pic2"];
                //    Pic.Add(pic);
                //}
                ////图片三
                //if (row["Pic3"].ToString() != "" && row["Pic3"].ToString() != "X")
                //{
                //    Pic pic = new Pic();
                //    pic.ProductID = row["ID"].ToString();
                //    pic.IsDeafult = "0";
                //    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                //                 row["Pic3"];
                //    Pic.Add(pic);
                //}

                #endregion

                product.ProductPicUrlList = Pic;

                List<SKU> SKUList = new List<SKU>();
                string strID = "0";

                #region 通过 商品ID和属性值ID关联表，找到属性值

                string strgoodsAttr = "0";
                List<Hi.Model.BD_GoodsInfo> goodsInfo = new List<Hi.Model.BD_GoodsInfo>();
                if (version.ToLower() != "android" && version.ToLower() != "ios" && version != "0")//除了第一版之后的版本都需要取出上架跟下架的商品的SKU
                {
                    goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("",
    " GoodsID=" + row["ID"].ToString() + " and CompID=" + compID +
    " and IsEnabled=1 and isnull(dr,0) =0 ", "");
                }
                else
                {
                    goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("",
                        " GoodsID=" + row["ID"].ToString() + " and CompID=" + compID +
                        " and IsEnabled=1 and isoffline=1 and isnull(dr,0) =0 ", "");
                }
                string maxprice = "";//商品最高价
                string minprice = "";//商品最低价
                decimal suminventory = 0;//商品所有SKU的库存总和
                foreach (Hi.Model.BD_GoodsInfo goods in goodsInfo)
                {
                    SKU SKU = new SKU();
                    //SKUID是GoodsInfoID,SKUName是GoodsName+各种属性值
                    SKU.SKUID = goods.ID.ToString();
                    SKU.ProductID = goods.GoodsID.ToString();
                    SKU.BarCode = goods.BarCode;
                    //SKUName = GoodsName + ValueInfo
                    //版本五之前的skuname是商品名称，属性值拼接成sku名称，版本五及以后版本skuname只需要给商品名称
                    if (version.ToLower() != "android" && version.ToLower() != "ios" && float.Parse(version) >= 5)
                    {
                        SKU.SKUName = SKUName;
                    }
                    else
                    {
                        SKU.SKUName = SKUName + " " + goods.ValueInfo;
                    }
                    SKU.InStock = ClsSystem.gnvl(goods.IsOffline, "0");
                    if (!Common.getGoodInfoID(compID, disID, goods.ID.ToString()))
                        SKU.InStock = "0";
                    SKU.ValueInfo = goods.ValueInfo;
                    SKU.SalePrice = goods.SalePrice.ToString("0.00");
                    SKU.Delete = goods.dr.ToString();
                    //取销量
                    string salenum = Common.GetSaleNum(goods.ID);
                    if (salenum == "")
                        return new ResultProductList() { Result = "F", Description = "参数异常" };
                    SKU.SaleNum = salenum;
                    //获取特定属性商品的库存
                    if (IsInve != 0 && version.ToLower() != "android" && version.ToLower() != "ios")
                    {
                        SKU.Inventory = "-1";
                        //SKU.Inventory = goods.Inventory.ToString();

                    }
                    else
                    {
                        SKU.Inventory = goods.Inventory.ToString();
                        suminventory += goods.Inventory;
                    }
                    if (disID != "")
                    {
                        int ProID = 0; //暂时未用到 促销ID            
                        SKU.IsPro = "0"; //默认不是促销价
                        decimal price = Common.GetProPrice(goods.GoodsID.ToString(), goods.ID.ToString(),
                            goods.CompID.ToString(), out ProID);
                        //if (price == 0)
                        //{
                        //    List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                        //        " GoodsInfoID=" + goods.ID + " and DisID= " + disID + " and  ISNULL(dr,0)=0 and compid=" + goods.CompID +
                        //        " and IsEnabled=1", "");
                        //    SKU.TinkerPrice = goodsPrice.Count != 0
                        //        ? goodsPrice[0].TinkerPrice.ToString()
                        //        : goods.TinkerPrice.ToString();
                        //}
                        if (price != 0)
                        {
                            SKU.IsPro = "1"; //是促销价
                            Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
                            if (pro != null)
                            {
                                string info = string.Empty;

                                List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail()
                                    .GetList("", " ProID=" + pro.ID + " and GoodInfoID =" + SKU.SKUID + " and dr=0",
                                        "");
                                if (dList != null && dList.Count > 0)
                                {
                                    if (pro.Type == 0 && pro.ProType == 1)
                                    {
                                        info = "赠品";
                                    }
                                    else if (pro.Type == 0 && pro.ProType == 2)
                                    {
                                        info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                    }
                                    else if (pro.Type == 1 && pro.ProType == 3)
                                    {
                                        info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" +
                                               dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                                    }
                                    else if (pro.Type == 1 && pro.ProType == 4)
                                    {
                                        info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                    }
                                }

                                SKU.ProInfo = new PromotionInfo()
                                {
                                    ProID = ProID.ToString(),
                                    ProTitle = pro.ProTitle,
                                    ProInfos = info,

                                    Tpye = pro.Type.ToString(),
                                    ProTpye = pro.ProType.ToString(),
                                    Discount = pro.Discount.ToString("0.00"),

                                    ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                    ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                                };
                            }
                            //SKU.TinkerPrice = price.ToString();
                        }
                        SKU.TinkerPrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(compID), Convert.ToInt32(disID), goods.ID).ToString("0.00");
                    }
                    else
                    {
                        SKU.TinkerPrice = goods.TinkerPrice.ToString("0.00");
                    }
                    List<ProductAttValueID> ProductAttValueIDList = new List<ProductAttValueID>();

                    //之前代码，由于表BD_AttributeValues不用了，代码就存在问题
                    //List<Hi.Model.BD_GoodsAttrValues> attrList = new Hi.BLL.BD_GoodsAttrValues().GetList("",
                    //    " GoodsID='" + goods.GoodsID + "' and ISNULL(dr,0)=0", "");
                    //if (attrList == null)
                    //    return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                    //foreach (Hi.Model.BD_GoodsAttrValues attribute in attrList)
                    //{
                    //    strID += "," + attribute.ValuesID;
                    //    Hi.Model.BD_AttributeValues goodsAttr =
                    //        new Hi.BLL.BD_AttributeValues().GetModel(attribute.ValuesID);
                    //    if (goodsAttr == null)
                    //        return new ResultProductList() { Result = "F", Description = "商品属性异常" };

                    //    strgoodsAttr += "," + goodsAttr.AttributeID;

                    //    string[] args = new[] {goods.ValueInfo};
                    //    string[] items = args[0].Split(new char[] {'；'}, StringSplitOptions.RemoveEmptyEntries);
                    //    foreach (string item in items)
                    //    {
                    //        string[] strs = item.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                    //        //foreach (string str in strs)
                    //        //{
                    //        if (strs[1] == goodsAttr.AttrValue)
                    //        {
                    //            ProductAttValueID productAttValueID = new ProductAttValueID();
                    //            productAttValueID.ProductAttributeID = attribute.ValuesID.ToString();
                    //            ProductAttValueIDList.Add(productAttValueID);
                    //        }
                    //        //}
                    //    }
                    //}

                    //新写的代码，查表BD_GoodsAttrsInfo取出属性值id
                    List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", " GoodsID=" + goods.GoodsID + " and ISNULL(dr,0)=0", "");
                    foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
                    {
                        strID += "," + attribute.AttrsID;
                        string[] args = new[] { goods.ValueInfo };
                        string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in items)
                        {
                            string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                            if (strs[1] == attribute.AttrsInfoName)
                            {
                                ProductAttValueID productAttValueID = new ProductAttValueID()
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
                //对skulist根据TinkerPrice进行排序
                SKUList.OrderBy(x => x.TinkerPrice);
                minprice = SKUList[0].TinkerPrice;
                maxprice = SKUList[SKUList.Count - 1].TinkerPrice;
                product.MaxPrice = maxprice;
                product.MinPrice = minprice;
                //商品所有SKU的总库存,不开启库存管理传-1
                if (IsInve != 0)//未开启库存管理
                {
                    product.Inventory = "-1";
                }
                else
                {
                    product.Inventory = suminventory.ToString();
                }
                List<ProductAttribute> ProductAttributeList = new List<ProductAttribute>();

                #region 通过商品类别ID和属性ID关联表，找到属性ID

                //List<Hi.Model.BD_CategoryAttribute> val = new Hi.BLL.BD_CategoryAttribute().GetList("",
                //    " ID in (" + strgoodsAttr + ") and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0", "");
                //if (val == null)
                //    return new ResultProductList() { Result = "F", Description = "未找到商品属性" };

                //foreach (Hi.Model.BD_CategoryAttribute goodsAttr in val)
                //{
                //    ProductAttribute proAttr = new ProductAttribute();

                //    proAttr.ProductID = row["ID"].ToString();
                //    proAttr.ProductAttributeID = goodsAttr.AttributeID.ToString(); //属性ID
                //    Hi.Model.BD_Attribute attr = new Hi.BLL.BD_Attribute().GetModel(goodsAttr.AttributeID);
                //    proAttr.ProductAttributeName = attr.AttributeName; //属性名称

                //    List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();
                //    List<Hi.Model.BD_AttributeValues> attrList = new Hi.BLL.BD_AttributeValues().GetList("",
                //        " AttributeID='" + goodsAttr.ID + "' and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0" +
                //        " and ID in (" + strID + ")", "ID"); //todo:商品属性表修改咨询商品结构
                //    if (attrList == null)
                //        return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                //    foreach (Hi.Model.BD_AttributeValues attribute in attrList)
                //    {
                //        ProductAttValue productAttValue = new ProductAttValue();
                //        productAttValue.ProductID = row["ID"].ToString().ToString();
                //        productAttValue.ProductAttributeID = goodsAttr.AttributeID.ToString();
                //        productAttValue.ProductAttValueID = attribute.ID.ToString();
                //        productAttValue.ProductAttValueName = attribute.AttrValue;

                //        ProductAttValueList.Add(productAttValue);
                //    }
                //    proAttr.ProductAttValueList = ProductAttValueList;
                //    ProductAttributeList.Add(proAttr);
                //}
                //ProductAttribute productattr = new ProductAttribute();
                strsql = "select ID,AttrsName from BD_GoodsAttrs where goodsid = " + row["ID"].ToString() + " and isnull(dr,0) =0 and compid = " + compID + "";
                DataTable dt_attr = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                for (int i = 0; i < dt_attr.Rows.Count; i++)
                {
                    ProductAttribute productattr = new ProductAttribute();
                    productattr.ProductID = row["ID"].ToString();
                    productattr.ProductAttributeID = ClsSystem.gnvl(dt_attr.Rows[i][0], "");
                    productattr.ProductAttributeName = ClsSystem.gnvl(dt_attr.Rows[i][1], "");
                    List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();

                    strsql = "select ID,AttrsInfoName from BD_GoodsAttrsInfo where attrsid = " + dt_attr.Rows[i][0] + " and goodsid = " + row["ID"].ToString() + " ";
                    strsql += " and isnull(dr,0) = 0 and compid = " + compID + "";
                    DataTable dt_attrinfo = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);

                    for (int j = 0; j < dt_attrinfo.Rows.Count; j++)
                    {
                        ProductAttValue productattvalue = new ProductAttValue();
                        productattvalue.ProductID = row["ID"].ToString();
                        productattvalue.ProductAttributeID = ClsSystem.gnvl(dt_attr.Rows[i][0], "");
                        productattvalue.ProductAttValueID = ClsSystem.gnvl(dt_attrinfo.Rows[j][0], "");
                        productattvalue.ProductAttValueName = ClsSystem.gnvl(dt_attrinfo.Rows[j][1], "");
                        ProductAttValueList.Add(productattvalue);

                    }
                    productattr.ProductAttValueList = ProductAttValueList;
                    ProductAttributeList.Add(productattr);
                }

                #endregion

                product.ProductAttributeList = ProductAttributeList;

                productList.Add(product);
            }

            #endregion

            return new ResultProductList()
            {
                Result = "T",
                Description = "获取成功",
                ProductList = productList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.StackTrace + ":" + ex.Message, "SearchGoodsList : " + JSon);
            return new ResultProductList() { Result = "F", Description = "参数异常" };
        }
    }


    /// <summary>
    /// 商品收藏与取消
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultCollect SetCollect(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string goodsID = string.Empty;
            string type = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["GoodsID"].ToString() != "" && JInfo["Type"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                goodsID = JInfo["GoodsID"].ToString();
                type = JInfo["Type"].ToString(); //1收藏 0取消
            }
            else
            {
                return new ResultCollect() { Result = "F", Description = "参数异常" };
            }

            #endregion

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultCollect() { Result = "F", Description = "登录信息异常" };

            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(disID));
            if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                return new ResultCollect() { Result = "F", Description = "经销商异常" };

            List<Hi.Model.BD_DisCollect> list = new Hi.BLL.BD_DisCollect().GetList("",
                " disID='" + disID + "' and goodsID='" + goodsID + "'", "");

            switch (type.Trim())
            {
                case "0":
                    if (list.Count > 0)
                    {
                        return new Hi.BLL.BD_DisCollect().delete(Convert.ToInt32(disID), int.Parse(goodsID)) ?
                            new ResultCollect() { Result = "T", Description = "取消成功", GoodsID = goodsID } :
                            new ResultCollect() { Result = "F", Description = "处理失败", GoodsID = goodsID };
                    }
                    else
                    {
                        return new ResultCollect() { Result = "F", Description = "该商品未收藏", GoodsID = goodsID };
                    }
                    break;
                case "1":
                    {
                        if (list.Count > 0)
                            new ResultCollect() { Result = "T", Description = "已经收藏", GoodsID = goodsID };
                        Hi.Model.BD_DisCollect collect = new Hi.Model.BD_DisCollect();
                        collect.CompID = dis.CompID;
                        collect.DisID = dis.ID;
                        collect.DisUserID = user.ID;
                        collect.GoodsID = int.Parse(goodsID);
                        collect.IsEnabled = 1;
                        collect.CreateDate = DateTime.Now;
                        collect.CreateUserID = user.ID;
                        collect.ts = DateTime.Now;
                        collect.modifyuser = user.ID;
                        if (list.Count == 0)
                        {
                            return new Hi.BLL.BD_DisCollect().Add(collect) > 0 ?
                                new ResultCollect() { Result = "T", Description = "收藏成功", GoodsID = goodsID } :
                                new ResultCollect() { Result = "F", Description = "处理失败", GoodsID = goodsID };
                        }
                        else
                        {
                            return new ResultCollect() { Result = "F", Description = "该商品已收藏", GoodsID = goodsID };
                        }
                    }
                    break;
                default:
                    return new ResultCollect() { Result = "F", Description = "操作状态异常", GoodsID = goodsID };
            }

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.StackTrace + ":" + ex.Message, "SetCollet : " + JSon);
            return new ResultCollect() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 收藏列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultProductList GetCollectList(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string criticalProductID = string.Empty; //当前列表最临界点产品ID:初始-1
            string sortType = "CreateDate"; //排序
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sort = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["CriticalProductID"].ToString() != "" && JInfo["GetType"].ToString() != "" &&
                JInfo["Rows"].ToString() != "" && JInfo["Sort"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                criticalProductID = JInfo["CriticalProductID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sort = JInfo["Sort"].ToString();
            }
            else
            {
                return new ResultProductList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultProductList() { Result = "F", Description = "登录信息异常" };

            #endregion

            #region 模拟分页

            string strsql = string.Empty; //搜索sql
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new ResultProductList() { Result = "F", Description = "未找到经销商" };

            Hi.Model.SYS_CompUser compUser = new SYS_CompUser();
            List<Hi.Model.SYS_CompUser> copUserList = new Hi.BLL.SYS_CompUser().GetList("", " DisID=" + disID + " and UserID=" + userID + " and IsAudit=2 and IsEnabled=1", "");
            if (copUserList == null || copUserList.Count <= 0)
                return new ResultProductList() { Result = "F", Description = "登录信息异常" };

            compUser = copUserList[0];

            const string tabName = " [dbo].[BD_Goods]"; //表名

            string strWhere = string.Empty;

            //商品可售区域判断
            List<Common.GoodsID> list = Common.DisEnAreaGoodsID(disID, compUser.CompID.ToString());
            if (list != null)
            {
                strWhere += " and ID not in ( -1 ";
                strWhere = list.Aggregate(strWhere, (current, goods) => current + ("," + goods.goodsID)) + ") ";
            }
            strWhere += " and id in (select GoodsID from BD_DisCollect where DisID=" + dis.ID + " and dr=0 and IsEnabled = 1) ";
            strWhere += " and ISNULL(dr,0)=0 and IsEnabled = 1";

            strsql = new Common().PageSqlString(criticalProductID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);

            #endregion

            List<Product> ProductList = new List<Product>();
            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if (dsList != null)
            {
                if (dsList.Rows.Count == 0)
                    return new ResultProductList() { Result = "T", Description = "没有更多数据" };
                foreach (DataRow row in dsList.Rows)
                {
                    string SKUName = string.Empty;
                    Product product = new Product();
                    product.ProductID = row["ID"].ToString(); //商品ID
                    product.ProductName = row["GoodsName"].ToString();
                    SKUName += product.ProductName;
                    //list的商品价格需要根据bd_goods表中viewinfoid对应的goodsinfoid的商品的价格
                    decimal salePrice = BLL.Common.GetGoodsPrice(compUser.CompID,Convert.ToInt32(disID),Convert.ToInt32(ClsSystem.gnvl(row["ViewInfoID"],"0")));
                    product.SalePrice = salePrice.ToString("0.00");
                    //decimal salePrice = decimal.Parse(row["SalePrice"].ToString());
                    //product.SalePrice = salePrice.ToString("0.00");
                    product.IsSale = row["IsSale"].ToString();
                    List<Hi.Model.BD_DisCollect> alist = new Hi.BLL.BD_DisCollect().GetList("",
                        " disID='" + disID + "' and compID='" + compUser.CompID + "' and goodsID='" + product.ProductID +
                        "' and IsEnabled =1", "");
                    product.IsCollect = alist.Count > 0 ? "1" : "0";
                    product.Title = row["Title"].ToString();
                    product.Title = row["Title"].ToString();
                    product.Unit = row["Unit"].ToString();
                    //product.Details = row["Details"].ToString().Trim() != "" ? row["Details"].ToString().Trim() : row["memo"].ToString().Trim();
                    List<Pic> Pic = new List<Pic>();

                    #region List<Pic> Pic

                    if (row["Pic"].ToString() != "" && row["Pic"].ToString() != "X")
                    {
                        Pic pic = new Pic();
                        pic.ProductID = row["ID"].ToString();
                        pic.IsDeafult = "1";
                        pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                     row["Pic"];
                        Pic.Add(pic);
                    }

                    #endregion

                    product.ProductPicUrlList = Pic;

                    List<SKU> SKUList = new List<SKU>();
                    string strID = "0";

                    #region 通过 商品ID和属性值ID关联表，找到属性值

                    string strgoodsAttr = "0";
                    List<Hi.Model.BD_GoodsInfo> goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("",
                        " GoodsID='" + row["ID"].ToString() + "' and CompID='" + compUser.CompID +
                        "' and ISNULL(dr,0)=0 and IsEnabled=1 and isoffline=1", "");
                    foreach (Hi.Model.BD_GoodsInfo goods in goodsInfo)
                    {
                        SKU SKU = new SKU();
                        //SKUID是GoodsInfoID,SKUName是GoodsName+各种属性值
                        SKU.SKUID = goods.ID.ToString();
                        SKU.ProductID = goods.GoodsID.ToString();
                        SKU.BarCode = goods.BarCode;
                        //SKUName = GoodsName + ValueInfo
                        SKU.SKUName = SKUName + " " + goods.ValueInfo;

                        SKU.ValueInfo = goods.ValueInfo;
                        SKU.SalePrice = goods.SalePrice.ToString("0.00");

                        int ProID = 0; //暂时未用到 促销ID
                        decimal price = Common.GetProPrice(goods.GoodsID.ToString(), goods.ID.ToString(),
                            goods.CompID.ToString(), out ProID);
                        //if (price == 0)
                        //{
                        //    List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                        //        " GoodsInfoID='" + goods.ID + "' and ISNULL(dr,0)=0 and compid='" + goods.CompID +
                        //        "' and IsEnabled=1", "");
                        //    SKU.TinkerPrice = goodsPrice.Count != 0
                        //        ? goodsPrice[0].TinkerPrice.ToString("0.00")
                        //        : goods.TinkerPrice.ToString("0.00");
                        //}
                        //else
                        if(price !=0)
                        {
                            SKU.IsPro = "1"; //是促销价
                            Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
                            if (pro != null)
                            {
                                string info = string.Empty;

                                List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail()
                                    .GetList("", " ProID=" + pro.ID + " and GoodInfoID ='" + SKU.SKUID + "' and dr=0", "");
                                if (dList != null && dList.Count > 0)
                                {
                                    if (pro.Type == 0 && pro.ProType == 1)
                                    {
                                        info = "赠品";
                                    }
                                    else if (pro.Type == 0 && pro.ProType == 2)
                                    {
                                        info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                    }
                                    else if (pro.Type == 1 && pro.ProType == 3)
                                    {
                                        info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" +
                                               dList[0].GoodsPrice.ToString("0.00") +
                                               dList[0].GoodsUnit;
                                    }
                                    else if (pro.Type == 1 && pro.ProType == 4)
                                    {
                                        info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                    }
                                }

                                SKU.ProInfo = new PromotionInfo()
                                {
                                    ProID = ProID.ToString(),
                                    ProTitle = pro.ProTitle,
                                    ProInfos = info,

                                    Tpye = pro.Type.ToString(),
                                    ProTpye = pro.ProType.ToString(),
                                    Discount = pro.Discount.ToString(),

                                    ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                    ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                                };
                            }
                            //SKU.TinkerPrice = price.ToString("0.00");
                        }
                        SKU.TinkerPrice = BLL.Common.GetGoodsPrice(compUser.CompID,Convert.ToInt32(disID),goods.ID).ToString("0.00");
                        List<ProductAttValueID> ProductAttValueIDList = new List<ProductAttValueID>();
                        ////List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("",
                        ////    " GoodsID='" + goods.GoodsID + "' and ISNULL(dr,0)=0", "");
                        ////if (attrList == null)
                        ////    return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                        ////foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
                        ////{
                        ////    strID += "," + attribute.ID;
                        ////    Hi.Model.BD_AttributeValues goodsAttr =
                        ////        new Hi.BLL.BD_AttributeValues().GetModel(attribute.ID);
                        ////    if (goodsAttr == null)
                        ////        return new ResultProductList() { Result = "F", Description = "商品属性异常" };

                        ////    strgoodsAttr += "," + goodsAttr.AttributeID.ToString();

                        ////    string[] args = new[] { goods.ValueInfo };
                        ////    string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                        ////    foreach (string item in items)
                        ////    {
                        ////        string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        ////        //foreach (string str in strs)
                        ////        //{
                        ////        if (strs[1] == goodsAttr.AttrValue)
                        ////        {
                        ////            ProductAttValueID productAttValueID = new ProductAttValueID();
                        ////            productAttValueID.ProductAttributeValueID = attribute.ID.ToString();
                        ////            ProductAttValueIDList.Add(productAttValueID);
                        ////        }
                        ////        //}
                        ////    }
                        ////}

                        SKU.ProductAttValueIDList = ProductAttValueIDList;
                        SKUList.Add(SKU);
                    }

                    #endregion

                    product.SKUList = SKUList;

                    List<ProductAttribute> ProductAttributeList = new List<ProductAttribute>();

                    #region 通过商品类别ID和属性ID关联表，找到属性ID

                    List<Hi.Model.BD_CategoryAttribute> val = new Hi.BLL.BD_CategoryAttribute().GetList("",
                        " ID in (" + strgoodsAttr + ") and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0", "");
                    if (val == null)
                        return new ResultProductList() { Result = "F", Description = "未找到商品属性" };

                    foreach (Hi.Model.BD_CategoryAttribute goodsAttr in val)
                    {
                        ProductAttribute proAttr = new ProductAttribute();

                        proAttr.ProductID = row["ID"].ToString();
                        proAttr.ProductAttributeID = goodsAttr.AttributeID.ToString(); //属性ID
                        Hi.Model.BD_Attribute attr = new Hi.BLL.BD_Attribute().GetModel(goodsAttr.AttributeID);
                        proAttr.ProductAttributeName = attr.AttributeName; //属性名称

                        List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();
                        List<Hi.Model.BD_AttributeValues> attrList = new Hi.BLL.BD_AttributeValues().GetList("",
                            " AttributeID='" + goodsAttr.ID + "' and CompID='" + compUser.CompID + "' and ISNULL(dr,0)=0" +
                            " and ID in (" + strID + ")", "ID"); //todo:商品属性表修改咨询商品结构
                        if (attrList == null)
                            return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                        foreach (Hi.Model.BD_AttributeValues attribute in attrList)
                        {
                            ProductAttValue productAttValue = new ProductAttValue();
                            productAttValue.ProductID = row["ID"].ToString();
                            productAttValue.ProductAttributeID = goodsAttr.AttributeID.ToString();
                            productAttValue.ProductAttValueID = attribute.ID.ToString();
                            productAttValue.ProductAttValueName = attribute.AttrValue;

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
            return new ResultProductList()
            {
                Result = "T",
                Description = "获取成功",
                ProductList = ProductList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCollectList：" + JSon);
            return new ResultProductList() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 获取订单优惠列表
    /// </summary>
    /// <param name="CompId"></param>
    /// <returns></returns>
    public ResultOrderProList GetOrderPro(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                compID = JInfo["CompID"].ToString();
            }
            else
            {
                return new ResultOrderProList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user, int.Parse(compID == "" ? "0" : compID)))
                return new ResultOrderProList() { Result = "F", Description = "登录信息异常" };

            #endregion

            List<ResultOrderPro> list = new List<ResultOrderPro>();
            list = Common.ReturnOrderProList(compID);

            return new ResultOrderProList()
            {
                Result = "T",
                Description = "",
                OrderPro = list
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetOrderPro：" + JSon);
            return new ResultOrderProList() { Result = "F", Description = "参数异常" };
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
        public String ClassifyName { get; set; }
        public string SalePrice { get; set; }
        public string IsSale { get; set; }
        public string IsCollect { get; set; }
        public string Title { get; set; }
        public String Details { get; set; }
        public string Unit { get; set; }
        public string InStock { get; set; }
        public List<Pic> ProductPicUrlList { get; set; }
        public List<ProductAttribute> ProductAttributeList { get; set; }
        public List<SKU> SKUList { get; set; }
        public string MaxPrice { get; set; }
        public string MinPrice { get; set; }
        public string Inventory { get; set; }
        public String IsShop { get; set; }//是否店铺显示（0：表示不显示，1：表示显示）
        public String IsRecommend { get; set; }//是否店铺推荐（0：表示不是，1：表示是）
        public String SortIndex { get; set; }//排序号
        public List<GoodsSpan> GoodsSpanList { get; set; }//商品标签
        public String TemplateID { get; set; }//属性模板ID
        public String ts { get; set; }
        public String url { get; set; }//商品介绍url
    }
    public class GoodsSpan
    {
        public String GoodsSpanValue { get; set; }
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
        public string Inventory {get;set;}//库存
        public string InStock { get; set; }//是否上下架
        public string ProductCode { get; set; }//商品编码
        public string ProductName { get; set; }//商品名称
        public string Delete { get; set; }
        public String SaleNum { get; set; }//销量
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
        public string ProductAttributeValueID { get; set; }
    }

    #endregion
}