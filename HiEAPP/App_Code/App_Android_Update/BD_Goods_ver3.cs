using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.UI.WebControls;
using DBUtility;
using Hi.Model;
using LitJson;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;

/// <summary>
///BD_Goods_ver3 的摘要说明
/// </summary>
public class BD_Goods_ver3
{
	public BD_Goods_ver3()
	{

    }
    #region//经销商根据SKUID列表获取对应的sku列表
    public ResultSKUList SearchGoodsInfoList(string JSon, string version)
    {
        #region//从JSon中取出参数
        string UserID = string.Empty;
        string ResellerID = string.Empty;
        string SKUID = string.Empty;
        string strsql = string.Empty;
        string areaid = string.Empty;

        JsonData JInfo = JsonMapper.ToObject(JSon);
        if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["SKUID"].ToString() != "")
        {
            UserID = JInfo["UserID"].ToString();
            ResellerID = JInfo["ResellerID"].ToString();
            SKUID = JInfo["SKUID"].ToString();
        }
        else
        {
            return new ResultSKUList() { Result = "F", Description = "传入参数异常" };
        }

        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(ResellerID.ToInt());
        if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
            return new ResultSKUList() { Result = "F", Description = "未找到经销商" };

        Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
        if (!new Common().IsLegitUser(int.Parse(UserID), out one,0, int.Parse(ResellerID == "" ? "0" : ResellerID)))
            return new ResultSKUList() { Result = "F", Description = "登录信息异常" };


        int IsInve = Common.rdoOrderAudit("商品是否启用库存", dis.CompID).ToInt(0);//判断此核心企业是否启用库存

        #endregion

        string[] SKUList = SKUID.Replace(" ", "").Split(',');
        Hi.Model.BD_GoodsInfo goodsinfo = new Hi.Model.BD_GoodsInfo();//goodsinfo的实体
        Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
        List<class_ver3.SKU> sku_list = new List<class_ver3.SKU>();//返回的sku实体list
        Hi.Model.BD_Goods product = new Hi.Model.BD_Goods();//商品的实体
        Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
        for (int i = 0; i < SKUList.Length; i++)
        {
            //通过skuid获得goodsinfo的实体
            goodsinfo = bll_goodsinfo.GetModel(SKUList[i].ToInt());
            if (goodsinfo == null || goodsinfo.dr == 1 || !goodsinfo.IsEnabled)
                return new ResultSKUList() { Result = "F", Description = "SKU信息异常" };
            //通过goodsid获取商品实体
            product = bll_goods.GetModel(goodsinfo.GoodsID);
            if(product == null||product.dr == 1|| product.IsEnabled == 0)
                return new ResultSKUList() { Result = "F", Description = "SKU信息异常" };
            //判断这个实体是否属于此经销商的核心企业
            if (goodsinfo.CompID != dis.CompID)
                return new ResultSKUList() { Result = "F", Description = "SKU信息异常" };
            //判断商品的不可售区域
            strsql = "select areaID from BD_GoodsAreas where GoodsID  = "+goodsinfo.GoodsID+" and DisID = "+dis.ID+" and isnull(dr,0) = 0 and CompID = "+goodsinfo.CompID+" ";
            areaid = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if(areaid != "")
                return new ResultSKUList() { Result = "F", Description = "SKU信息异常" };
            //赋值
            class_ver3.SKU SKU = new class_ver3.SKU();//返回的Sku实体
            SKU.ProductID = goodsinfo.GoodsID.ToString();
            SKU.SKUID = goodsinfo.ID.ToString();
            SKU.BarCode = goodsinfo.BarCode;
            SKU.ValueInfo = goodsinfo.ValueInfo;
            
            SKU.SalePrice = goodsinfo.SalePrice.ToString("0.00");
            SKU.SKUName = product.GoodsName + goodsinfo.ValueInfo;
            int ProID = 0; //暂时未用到 促销ID
            SKU.IsPro = "0"; //默认不是促销价
            decimal price = Common.GetProPrice(goodsinfo.GoodsID.ToString(), goodsinfo.ID.ToString(),
                goodsinfo.CompID.ToString(), out ProID);
            //if (price == 0)
            //{
            //    if (dis.ID.ToString() != "")
            //    {
            //        List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
            //            " GoodsInfoID='" + goodsinfo.ID + "' and ISNULL(dr,0)=0 and compid='" + goodsinfo.CompID +
            //            "' and DisID = " + dis.ID  + " and isnull(IsEnabled,0)=1", "");
            //        SKU.TinkerPrice = goodsPrice.Count != 0
            //            ? goodsPrice[0].TinkerPrice.ToString("0.00")
            //            : goodsinfo.TinkerPrice.ToString("0.00");
            //    }
            //    else
            //    {
            //        SKU.TinkerPrice = goodsinfo.TinkerPrice.ToString("0.00");
            //    }
            //}
            if(price !=0)
            {
                SKU.IsPro = "1"; //是促销价
                SKU.ProInfo =GetProInfo(ProID, goodsinfo.ID);
                //SKU.TinkerPrice = price.ToString("0.00");
            }
            SKU.TinkerPrice = BLL.Common.GetGoodsPrice(dis.CompID, dis.ID, goodsinfo.ID).ToString("0.00");
            SKU.InStock = goodsinfo.IsOffline.ToString();
            if (IsInve != 0)//不启用库存的时候库存返回-1
            {
                SKU.Inventory = "-1";
            }
            else//启用库存库存返回真实库存
            {
                SKU.Inventory = ClsSystem.gnvl(goodsinfo.Inventory,"0");
            }
            SKU.ProductCode = ClsSystem.gnvl(product.GoodsCode, "");
            SKU.ProductName = product.GoodsName;

            List<class_ver3.ProductAttValueID> ProductAttValueIDList = new List<class_ver3.ProductAttValueID>();

            List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", " GoodsID='" + goodsinfo.GoodsID + "' and ISNULL(dr,0)=0", "");
            if (attrList == null)
                return new ResultSKUList() { Result = "F", Description = "未找到商品属性名字" };
            //List<Hi.Model.BD_GoodsAttrs> attrValList = new Hi.BLL.BD_GoodsAttrs().GetList("*", " CompID =" + dis.CompID + "", "");
            foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
            {
                //strID += "," + attribute.AttrsID;
                string[] args = new[] { goodsinfo.ValueInfo };
                string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strs[1] == attribute.AttrsInfoName)
                    {
                        class_ver3.ProductAttValueID productAttValueID = new class_ver3.ProductAttValueID()
                        {
                            ProductAttributeValueID = attribute.ID.ToString()//
                        };
                        ProductAttValueIDList.Add(productAttValueID);
                    }
                }
            }
            SKU.ProductAttValueIDList = ProductAttValueIDList;
            sku_list.Add(SKU);
        }

        return new ResultSKUList() { Result = "T",Description = "获取成功",SKUList = sku_list};
    }
    #endregion


    #region//核心企业根据SKUID列表获取对应的
    public ResultSKUList SearchCompGoodsInfoList(string JSon, string version)
    {
        #region//从JSon中取出参数
        string UserID = string.Empty;
        string CompID = string.Empty;
        string SKUID = string.Empty;
        string strsql = string.Empty;
        string areaid = string.Empty;

        JsonData JInfo = JsonMapper.ToObject(JSon);
        if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["SKUID"].ToString() != "")
        {
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompanyID"].ToString();
            SKUID = JInfo["SKUID"].ToString();
        }
        else
        {
            return new ResultSKUList() { Result = "F", Description = "传入参数异常" };
        }

        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID.ToInt());
        if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
            return new ResultSKUList() { Result = "F", Description = "未找到经销商" };

        Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
        if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
            return new ResultSKUList() { Result = "F", Description = "登录信息异常" };


        int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存

        #endregion

        string[] SKUList = SKUID.Replace(" ", "").Split(',');
        Hi.Model.BD_GoodsInfo goodsinfo = new Hi.Model.BD_GoodsInfo();//goodsinfo的实体
        Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
        List<class_ver3.SKU> sku_list = new List<class_ver3.SKU>();//返回的sku实体list
        Hi.Model.BD_Goods product = new Hi.Model.BD_Goods();//商品的实体
        Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
        for (int i = 0; i < SKUList.Length; i++)
        {
            //通过skuid获得goodsinfo的实体
            goodsinfo = bll_goodsinfo.GetModel(SKUList[i].ToInt());
            if(goodsinfo ==null || goodsinfo.dr == 1 || !goodsinfo.IsEnabled)
                return new ResultSKUList() { Result = "F", Description = "SKU信息异常" };
            
            //通过goodsid获取商品实体
            product = bll_goods.GetModel(goodsinfo.GoodsID);
            if (product == null || product.dr == 1 || product.IsEnabled == 0)
                return new ResultSKUList() { Result = "F", Description = "SKU信息异常" };
            //判断这个实体是否属于此经销商的核心企业
            if (goodsinfo.CompID != comp.ID)
                return new ResultSKUList() { Result = "F", Description = "SKU信息异常" };
            //判断商品的不可售区域
            //strsql = "select areaID from BD_GoodsAreas where GoodsID  = " + goodsinfo.GoodsID + " and DisID = " + dis.ID + " and isnull(dr,0) = 0 and CompID = " + goodsinfo.CompID + " ";
            //areaid = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            //if (areaid != "")
            //    return new ResultSKUList() { Result = "F", Description = "SKU信息异常" };
            //赋值
            class_ver3.SKU SKU = new class_ver3.SKU();//返回的Sku实体
            SKU.ProductID = goodsinfo.GoodsID.ToString();
            SKU.SKUID = goodsinfo.ID.ToString();
            SKU.BarCode = goodsinfo.BarCode;
            SKU.ValueInfo = goodsinfo.ValueInfo;
            SKU.SalePrice = goodsinfo.SalePrice.ToString("0.00");
            SKU.SKUName = product.GoodsName + goodsinfo.ValueInfo;
            int ProID = 0; //暂时未用到 促销ID
            SKU.IsPro = "0"; //默认不是促销价
            decimal price = Common.GetProPrice(goodsinfo.GoodsID.ToString(), goodsinfo.ID.ToString(),
                goodsinfo.CompID.ToString(), out ProID);
            //if (price == 0)
            //{
                //if (dis.ID.ToString() != "")
                //{
                //    List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                //        " GoodsInfoID='" + goodsinfo.ID + "' and ISNULL(dr,0)=0 and compid='" + goodsinfo.CompID +
                //        "' and DisID = " + dis.ID + " and isnull(IsEnabled,0)=1", "");
                //    SKU.TinkerPrice = goodsPrice.Count != 0
                //        ? goodsPrice[0].TinkerPrice.ToString("0.00")
                //        : goodsinfo.TinkerPrice.ToString("0.00");
                //}
                //else
                //{
                    SKU.TinkerPrice = goodsinfo.TinkerPrice.ToString("0.00");
                //}
            //}
            if(price !=0)
            {
                SKU.IsPro = "1"; //是促销价
                SKU.ProInfo = GetProInfo(ProID, goodsinfo.ID);
                //SKU.TinkerPrice = price.ToString("0.00");
            }
            SKU.TinkerPrice = BLL.Common.GetGoodsPrice(comp.ID,0,goodsinfo.ID).ToString("0.00");
            SKU.InStock = product.IsOffline.ToString();
            if (IsInve != 0)//不启用库存的时候库存返回-1
            {
                SKU.Inventory = "-1";
            }
            else//启用库存库存返回真实库存
            {
                SKU.Inventory = ClsSystem.gnvl(goodsinfo.Inventory, "0");
            }
            SKU.ProductCode = ClsSystem.gnvl(product.GoodsCode, "");
            SKU.ProductName = product.GoodsName;

            List<class_ver3.ProductAttValueID> ProductAttValueIDList = new List<class_ver3.ProductAttValueID>();

            List<Hi.Model.BD_GoodsAttrsInfo> attrList = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", " GoodsID='" + goodsinfo.GoodsID + "' and ISNULL(dr,0)=0", "");
            if (attrList == null)
                return new ResultSKUList() { Result = "F", Description = "未找到商品属性名字" };
            //List<Hi.Model.BD_GoodsAttrs> attrValList = new Hi.BLL.BD_GoodsAttrs().GetList("*", " CompID =" + dis.CompID + "", "");
            foreach (Hi.Model.BD_GoodsAttrsInfo attribute in attrList)
            {
                //strID += "," + attribute.AttrsID;
                string[] args = new[] { goodsinfo.ValueInfo };
                string[] items = args[0].Split(new char[] { '；' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string[] strs = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strs[1] == attribute.AttrsInfoName)
                    {
                        class_ver3.ProductAttValueID productAttValueID = new class_ver3.ProductAttValueID()
                        {
                            ProductAttributeValueID = attribute.ID.ToString()//
                        };
                        ProductAttValueIDList.Add(productAttValueID);
                    }
                }
            }
            SKU.ProductAttValueIDList = ProductAttValueIDList;
            sku_list.Add(SKU);
        }

        return new ResultSKUList() { Result = "T", Description = "获取成功", SKUList = sku_list };
    }
    #endregion




    #region//根据SKUID列表获取对应的sku列表的返回值类型
    public class ResultSKUList
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<class_ver3.SKU> SKUList { get; set; }
    }
    #endregion

    /// <summary>
    /// 促销信息
    /// </summary>
    /// <param name="ProID">促销ID</param>
    /// <param name="GoodInfoID">GoodsInfoID</param>
    /// <returns></returns>
    public class_ver3.PromotionInfo GetProInfo(int ProID, int GoodInfoID)
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

            return new class_ver3.PromotionInfo()
            {
                ProID = ProID.ToString(),
                ProTitle = pro.ProTitle,
                ProInfos = proInfos,

                Type = pro.Type.ToString(),
                ProType = pro.ProType.ToString(),
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
    #region//商品修改库存及上下架
    public ResultProductsEdit CompanyProductsEdit(string JSon, string version)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string ProductID = string.Empty;
        string isint = string.Empty;
        string SKUID = string.Empty;
        string Shelves =string.Empty;
        string Inventory = string.Empty;
        int int_inv = 0;
        int num_out = 0;
        decimal dem_inv = 0;
        //Hi.Model.BD_GoodsInfo goodsinfo_shelves= null;
        Hi.Model.BD_GoodsInfo goodsinfo = null;
        Hi.Model.BD_Goods goods = null;
        Hi.BLL.BD_Goods goods_bll = new Hi.BLL.BD_Goods();
        Hi.BLL.BD_GoodsInfo goodsinfo_bll = new Hi.BLL.BD_GoodsInfo();
        SqlTransaction mytran = DBUtility.SqlHelper.CreateStoreTranSaction();
        try
        {
            #region//解析JSon并取出JSon中的值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["ProductID"].ToString() != "" && JInfo["SKUList"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompanyID"].ToString();
                ProductID = JInfo["ProductID"].ToString();
            }
            else
            {
                mytran.Rollback();
                return new ResultProductsEdit() { Result = "F", Description = "传入参数错误" };
            }
            #endregion
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
            {
                mytran.Rollback();
                return new ResultProductsEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(int.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
            {
                mytran.Rollback();
                return new ResultProductsEdit() { Result = "F", Description = "未找到核心企业" };
            }
            int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存
            isint = Common.rdoOrderAudit("订单下单数量是否取整", comp.ID);//判断此核心企业发货数量是否取整，0表示取整，0.00表示有两位小数

            #region //循环skulist，根据传入的数据修改商品的上下架状态或库存
            foreach (JsonData SKU in JInfo["SKUList"])
            {
                //判断skulist中的值是否存在问题
                if (SKU["SKUID"].ToString() != "" && SKU["Shelves"].ToString() != "" && SKU["Inventory"].ToString() != "")
                {
                    SKUID = SKU["SKUID"].ToString();
                    Shelves = SKU["Shelves"].ToString();
                    Inventory = SKU["Inventory"].ToString();
                }
                else
                {
                    mytran.Rollback();
                    return new ResultProductsEdit() { Result = "F", Description = "传入参数错误" };
                }
                //根据skuid取出bd_goodsinfo表的实体
                goodsinfo = goodsinfo_bll.GetModel(Convert.ToInt32(SKUID));
                if (goodsinfo == null || goodsinfo.dr == 1 || !goodsinfo.IsEnabled)
                {
                    mytran.Rollback();
                    return new ResultProductsEdit() { Result = "F", Description = "商品信息错误" };
                }
                //如果不启用库存的时候，skulist中的Inventory必须为-1，表示不修改库存
                if (IsInve != 0 && Inventory != "-1")
                {
                    mytran.Rollback();
                    return new ResultProductsEdit() { Result = "F", Description = "该核心企业没开启库存，不能修改库存" };
                }
                //如果启用库存并且inventory不为-1，表示修改了库存
                else if (Inventory != "-1")
                {
                    //如果核心企业必须取整的话，修改库存的时候，库存必须为整数
                    if (isint == "0")
                    {
                        dem_inv = decimal.Parse(Inventory);
                        int_inv = (int)dem_inv;//取整数部分
                        if (!int.TryParse(Inventory, out num_out))
                        {
                            //判断小数位上的数字是不是都是0
                            if (decimal.Parse(Inventory) != int_inv)
                            {
                                mytran.Rollback();
                                return new ResultProductsEdit() { Result = "F", Description = "库存应为整数" };
                            }
                        }
                    }
                    goodsinfo.Inventory = decimal.Parse(Inventory);//修改此skuid商品的库存
                }
                //如果是商品上架的话，需要判断这个product是否上架，没上架的需要把此product上线
                if (Shelves == "0")
                {
                    goodsinfo.IsOffline = 1;
                    //根据goodsinfo中的goodsid取出对应的bd_goods表的实体
                    goods = goods_bll.GetModel(goodsinfo.GoodsID);
                    if (goods == null || goods.dr == 1 || goods.IsEnabled == 0)
                    {
                        return new ResultProductsEdit() { Result = "F", Description = "商品信息错误" };
                    }
                    //如果product没上线的话需要将product上线
                    if (goods.IsOffline == 0)
                    {
                        goods.IsOffline = 1;
                        goods.ts = DateTime.Now;
                        goods.modifyuser = Convert.ToInt32(UserID);
                        goods.CompID = comp.ID;
                        if (!goods_bll.Update(goods, mytran))
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "更新失败" };
                        }
                    }
                }
                else if (Shelves == "1")//如果商品下架的话，需要判断product的所有sku是否下架，全下架的时候，需要把product下架
                {
                    goodsinfo.IsOffline = 0;
                    List<Hi.Model.BD_GoodsInfo> ll = new Hi.BLL.BD_GoodsInfo().GetList("", " goodsId=" + goodsinfo.GoodsID + " and isnull(IsOffline,1)=1 and isnull(dr,0)=0 and isnull(isenabled,0) = 1 and compid=" + comp.ID + "", "", mytran);
                    if (ll.Count == 0)
                    {
                        //根据goodsinfo中的goodsid取出对应的bd_goods表的实体
                        goods = goods_bll.GetModel(goodsinfo.GoodsID);
                        if (goods == null || goods.dr == 1 || goods.IsEnabled == 0)
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "商品信息错误" };
                        }
                        goods.IsOffline = 0;
                        goods.ts = DateTime.Now;
                        goods.modifyuser = Convert.ToInt32(UserID);
                        goods.CompID = comp.ID;
                        if (!goods_bll.Update(goods, mytran))
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "更新失败" };
                        }
                    }


                }

                goodsinfo.ts = DateTime.Now;
                goodsinfo.modifyuser = Convert.ToInt32(UserID);
                goodsinfo.CompID = comp.ID;
                //更新bd_goodsinfo表
                if (!goodsinfo_bll.Update(goodsinfo, mytran))
                {
                    mytran.Rollback();
                    return new ResultProductsEdit() { Result = "F", Description = "更新失败" };
                }

            }
            #endregion
        }

        catch (Exception ex)
        {
            mytran.Rollback();
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CompanyProductsEdit:" + JSon);
            return new ResultProductsEdit() { Result = "F", Description = "参数异常" };
        }
        finally
        {
            if (mytran != null)
            {
                if (mytran.Connection != null)
                {
                    mytran.Connection.Close();
                }
            }
        }
        mytran.Commit();
        return new ResultProductsEdit(){Result = "T",Description = "返回成功"};
    }
    #endregion

    #region//获取经销商对应的核心企业的配置信息
    public ResultConfig GetConfiguration(string JSon,string version)
    {
        string UserID = string.Empty;
        string DisID = string.Empty;
        string isint = string.Empty;
        string CompID = string.Empty;
        try
        {
            #region//解析JSon并取出JSon中的值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString()!="")
            {
                UserID = JInfo["UserID"].ToString();
                DisID = JInfo["ResellerID"].ToString();
                CompID = JInfo["CompanyID"].ToString();
            }
            else
            {
                return new ResultConfig() { Result = "F",Description = "传入参数错误"};
            }
            #endregion
            if (DisID != "")//传入的是经销商ID的话
            {
                //判断登录信息是否正确
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(UserID), out one,0, int.Parse(DisID == "" ? "0" : DisID)))
                    return new ResultConfig() { Result = "F", Description = "登录信息异常" };
                //判断经销商信息是否异常
                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(DisID));
                if (dis == null || dis.dr == 1 || dis.IsEnabled == 0 || dis.AuditState == 0)
                    return new ResultConfig() { Result = "F", Description = "经销商信息异常" };
                //CompID = dis.CompID.ToString();
            }
            else
            {
                //传入的是核心企业ID判断此核心企业的登录信息是否正确
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                    return new ResultConfig() { Result = "F", Description = "登录信息异常" };
            }
            //判断经销商对应的核心企业是否异常
                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new ResultConfig() { Result = "F", Description = "核心企业信息异常" };

                int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存
                isint = Common.rdoOrderAudit("订单下单数量是否取整", comp.ID);//判断此核心企业发货数量是否取整，0表示取整，0.00表示有两位小数
                int isrebate = Common.rdoOrderAudit("订单支付返利是否启用", comp.ID).ToInt(0);//判断核心企业是否启用返利
            //判断是否维护收款账号
                List<Hi.Model.PAY_PaymentBank> paybank_list = new Hi.BLL.PAY_PaymentBank().GetList("","CompID = "+comp.ID+" and isnull(Isno,0) = 1 and isnull(dr,0) =0 ","");
            //判断是否维护支付宝账号
                List<Hi.Model.Pay_PayWxandAli> alipay_list = new Hi.BLL.Pay_PayWxandAli().GetList("","CompID = "+comp.ID+" and isnull(ali_isno,0) = 1","");

            //返回参数
                ResultConfig result = new ResultConfig();
                result.Result = "T";
                result.Description = "返回成功";
                if (IsInve == 0)
                {
                    result.ProductInventory = "1";
                }
                else
                {
                    result.ProductInventory = "0";
                }
                if (isrebate == 1)
                {
                    result.ProductRebate = "1";
                }
                else
                {
                    result.ProductRebate = "0";
                }
                if (isint == "0")
                {
                    result.ProductNum = "0";
                }
                else
                {
                    result.ProductNum = "1";
                }
                if (paybank_list == null || paybank_list.Count == 0)
                {
                    result.IsFastPay = "0";
                }
                else
                {
                    result.IsFastPay = "1";
                }
                if (alipay_list == null || alipay_list.Count == 0)
                {
                    result.IsAliPay = "0";
                }
                else
                {
                    result.IsAliPay = "0";
                }
                return result;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerConfiguration:" + JSon);
            return new ResultConfig() { Result = "F", Description = "参数异常" };
        }
    }
    #endregion


    //核心企业根据经销商名称/编码，商品名称/编码获取获取商品列表
    /*
     * 有三种情况1.核心企业查询所有商品compid传值，resellerid不传值
     * 2.核心企业修改订单时查询商品compid传值,resellerid传值
     * 3.经销商查询商品compid不传值，resellerid不传值
     */
    public ResultCompanyProductSearch CompanyProductSearch(string JSon)
    {
        string CompID = string.Empty;
        string disID = string.Empty;
        string CriticalProductID = string.Empty;
        string gettype = string.Empty;
        string type = string.Empty;
        string search = string.Empty;
        JsonData Filter = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["CriticalProductID"].ToString() == "" ||
                JInfo["GetType"].ToString() == "" || JInfo["Type"].ToString() == "" || JInfo["CompID"].ToString() == "")
            {
                return new ResultCompanyProductSearch() { Result = "F", Description = "参数异常" };
            }
            else
            {
                CompID = JInfo["CompID"].ToString();
                CriticalProductID = JInfo["CriticalProductID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                gettype = JInfo["GetType"].ToString();
                type = JInfo["Type"].ToString();
                search = JInfo["Condition"].ToString();
                Filter = JInfo["Filter"];
            }
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            Hi.Model.BD_Distributor dis;
            Hi.Model.BD_Company comp;
            Hi.BLL.BD_Distributor bll_dis = new Hi.BLL.BD_Distributor();
            Hi.BLL.BD_GoodsCategory bll_category = new Hi.BLL.BD_GoodsCategory();
            Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
            List<Hi.Model.BD_Distributor> list_dis;
            //返回的商品列表
            List<class_ver3.ProductSimple> list_resultproduct = new List<class_ver3.ProductSimple>();

            //判断核心企业是否异常
            comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultCompanyProductSearch() { Result = "F", Description = "核心企业信息异常" };
            if (disID != "")//如果传入的经销商id也需要判断经销商信息是否正确
            {
                //判断经销商是否异常
                dis = new Hi.BLL.BD_Distributor().GetModel(Int32.Parse(disID));
                if (dis == null || dis.dr == 1 || dis.AuditState == 0 || dis.IsEnabled == 0)
                    return new ResultCompanyProductSearch() { Result = "F", Description = "经销商信息异常" };
            }

            
            #endregion
            StringBuilder tablename = new StringBuilder();
            string strWhere = "where 1=1 ";


            if (Filter["IsSales"].ToString() == "1")//表示促销商品
            {
                List<Hi.Model.BD_Promotion> promotionList = new Hi.BLL.BD_Promotion().GetList("",
                       " compID=" + CompID.ToInt() + " and ProStartTime<='" + DateTime.Now + "' and ProEndTime >='" +
                       DateTime.Now + "' and IsEnabled=1", "");
                List<Hi.Model.BD_PromotionDetail> detailList = new List<BD_PromotionDetail>();
                if (promotionList != null && promotionList.Count > 0)
                {
                    detailList = new Hi.BLL.BD_PromotionDetail().GetList("",
                        " ProID in(" + string.Join(",", promotionList.Select(p => p.ID)) + ")", "");
                }
                //if (promotionList.Count == 0)
                //    return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "今天无促销" };
                //if (detailList.Count == 0)
                //    return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "今天无促销" };
                var ienum = detailList.Select(p => p.GoodsID);
                if (ienum.Count() > 0)
                    strWhere += " and a.ID in ( " + string.Join(",", ienum) + ")";
                else
                    return new ResultCompanyProductSearch() { Result = "T", Description = "没有更多数据", ProductSimpleList = null };
            }
            string productstatus = Filter["ProductStatus"].ToString();
            switch (productstatus)//商品上下架属性
            {
                case "1":
                    {
                        strWhere += " and ISNULL(a.IsOffline,0) =1";
                    }
                    break;
                case "2":
                    {
                        strWhere += " and ISNULL(a.IsOffline,0)=0 ";
                    }
                    break;
            }
            if (Filter["MaxPrice"].ToString() != "")//商品最高价格
            {
                strWhere += " and d.TinkerPrice<=" + decimal.Parse(Filter["MaxPrice"].ToString()) + "";
            }
            if (Filter["MinPrice"].ToString() != "")//商品最低价格
            {
                strWhere += " and d.TinkerPrice >= " + decimal.Parse(Filter["MinPrice"].ToString()) + "";
            }
            if (Filter["Classif"].ToString() != "-1")//商品分类
            {
                List<Hi.Model.BD_GoodsCategory> list_category_all = new List<Hi.Model.BD_GoodsCategory>();
                //判断分类是否存在
                Hi.Model.BD_GoodsCategory category = bll_category.GetModel(Int32.Parse(Filter["Classif"].ToString()));
                if (category == null || category.dr == 1 || category.IsEnabled == 0)
                {
                    return new ResultCompanyProductSearch() { Result = "F", Description = "商品分类异常" };
                }

                //判断此分类是不是末级分类
                List<Hi.Model.BD_GoodsCategory> list_category = bll_category.GetList("ID", "ParentId = " + Filter["Classif"].ToString() + " and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1", "");
                if (list_category != null && list_category.Count > 0)//不是末级的话找出末级
                {
                    foreach (Hi.Model.BD_GoodsCategory goodscategory in list_category)
                    {
                        List<Hi.Model.BD_GoodsCategory> list_goodscategory = bll_category.GetList("ID", "ParentId = " + goodscategory.ID + " and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1", "");
                        if (list_goodscategory == null || list_goodscategory.Count == 0)
                        {
                            list_category_all.Add(goodscategory);
                        }
                        else
                        {
                            //while(list_goodscategory!=null&&list_goodscategory.Count>0)
                            //{
                            //    foreach(Hi.Model.BD_GoodsCategory goodscategory2 in list_goodscategory)
                            //    {
                            //        List<Hi.Model.BD_GoodsCategory> list_goodscategory2 =  bll_category.GetList("ID","ParentId = "+goodscategory2.ID+" and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1","");
                            //        if(list_goodscategory2 == null|| list_goodscategory2.Count ==0)
                            //        {
                            //            list_category_all.Add(goodscategory);
                            //        }
                            //        else
                            //        {
                            //            list_goodscategory = list_goodscategory2;
                            //        }
                            //    }
                            //}
                            //因为只有三级所以第三极直接插进去就行了（不用判断有没下级）
                            list_category_all.AddRange(list_goodscategory);
                        }

                    }
                    if (list_category_all != null)
                    {
                        strWhere += " and a.CategoryID in ( -1 ";
                        strWhere = list_category_all.Aggregate(strWhere, (current, goods) => current + ("," + goods.ID)) +
                                   ")";

                    }
                }
                //是末级的只要商品类别等于此类别就行了
                else
                {
                    strWhere += " and a.CategoryID = " + Filter["Classif"].ToString() + "";
                }
                #region//测试取没有分支的末级节点(思想：先去最开始的分支一直取到全部取完，然后依次往后取最近的分支，取完，直到最后)
                //    while(list_category!=null&&list_category.Count>0)
                //    {
                //        int j = 0;
                //        foreach(Hi.Model.BD_GoodsCategory goodscategory in list_category)
                //        {
                //             List<Hi.Model.BD_GoodsCategory> list_goodscategory =  bll_category.GetList("ID","ParentId = "+goodscategory.ID+" and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1","ID");
                //            if(list_goodscategory == null|| list_goodscategory.Count ==0)
                //            {
                //                list_category_all.Add(goodscategory);
                //                j++;
                //            }
                //            else
                //            {
                //                list_category = list_goodscategory;
                //                break;
                //            }
                //            if(j==list_category.Count)
                //            {
                //                if(goodscategory.ParentId==Int32.Parse( Filter["Classif"].ToString()))
                //                {
                //                    list_category =null;
                //                    break;
                //                }
                //                Hi.Model.BD_GoodsCategory model_cate = bll_category.GetModel(goodscategory.ParentId);
                //                if(model_cate.ParentId ==Int32.Parse( Filter["Classif"].ToString()))
                //                {
                //                    list_category =null;
                //                    break;
                //                }
                //                list_category  =   bll_category.GetList("ID","ParentId = "+model_cate.ParentId+" and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1 and ID>"+model_cate.ID+"","ID");
                //                while((list_category ==null||list_category.Count==0 )&&model_cate.ParentId!=Int32.Parse( Filter["Classif"].ToString()))
                //                {
                //                    model_cate= bll_category.GetModel(model_cate.ParentId);
                //                    list_category  =   bll_category.GetList("ID","ParentId = "+model_cate.ParentId+" and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1 and ID>"+model_cate.ID+"","ID");
                //                }
                //            }
                //        }

                //}

                #endregion
            }
            //先结合订单取出销量，再将此表进行分页
            tablename.Append("select isnull( sum(isnull(c.GoodsNum ,0)), 0 ) as salenum,a.ID  as productid,d.TinkerPrice as saleprice,a.CreateDate as createdate  ");
            tablename.Append("  from  BD_Goods a  left join BD_GoodsInfo b on a.ID = b.GoodsID left join DIS_OrderDetail c on b.ID = c.GoodsinfoID ");
            tablename.Append(" left join BD_GoodsInfo d on a.viewinfoid = d.id ");
            if (disID != "")
            {
                tablename.Append(" inner join (select cd.GoodsID from YZT_ContractDetail cd left join YZT_Contract con on cd.ContID=con.ID where compid ='" + CompID + "' and disid ='" + disID + "' ");
                tablename.Append(" union select cm.GoodsID from YZT_CMerchants cm left join YZT_FirstCamp fc on cm.ID =fc.CMID left join ");
                tablename.Append(" YZT_ContractDetail cd on cd.FCID=fc.ID where cm.dr=0 and fc.State=2 and fc.compid ='" + CompID + "' and fc.disid ='" + disID + "') conandfc  on  b.ID = conandfc.GoodsID ");
            }
            tablename.Append(strWhere);
            tablename.Append(" and ISNULL(a.dr,0)=0 and ISNULL(a.IsEnabled,0)=1 and a.CompID=" + CompID + " ");
            tablename.Append(" group by a.ID,a.CreateDate,d.TinkerPrice");
           
            
            string orderby = "";
            string sort = "";
            //根据传入的type判断排序方式,跟顺序
            switch (type)
            {
                case "0"://表示按照销量排序
                    {
                        orderby = "salenum";
                        sort = "0";
                    }
                    break;
                case "1"://表示按创建时间排序
                    {
                        orderby = "createdate";
                        sort = "0";
                    }
                    break;
                case "2"://表示按价格从高到低
                    {
                        orderby = "saleprice";
                        sort = "0";
                    }
                    break;
                case "3"://表示按价格从低到高
                    {
                        orderby = "saleprice";
                        sort = "1";

                    }
                    break;

            }
            string strsql = new Common().PageSqlString(CriticalProductID, "productid", "(" + tablename.ToString() + ")x", orderby,
                sort, "", gettype, "10");
            //执行sql，取出满足条件的数据
            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];

            #region //返回参数
            if (dsList != null)
            {
                if (dsList.Rows.Count == 0)
                {
                    return new ResultCompanyProductSearch() { Result = "T", Description = "没有更多数据", ProductSimpleList = null };
                }
          
                Hi.Model.BD_Goods goodsmodel = null;
                //取出此核心企业或经销商所有被收藏的商品ID（用于后面判断商品是否被收藏）
                List<Hi.Model.BD_DisCollect> Colist = new List<Hi.Model.BD_DisCollect>();
                if (disID != "")
                {
                    Colist = Common.GetDataSource<BD_DisCollect>("",
                       " and disID='" + disID + "' and compID=" + Int32.Parse(CompID) + " and IsEnabled =1 and ISNULL(dr,0)=0");
                }
                else
                {
                    Colist = Common.GetDataSource<BD_DisCollect>("",
                       "  and compID=" + Int32.Parse(CompID) + " and IsEnabled =1 and ISNULL(dr,0)=0");
                }
                foreach (DataRow row in dsList.Rows)
                {
                    class_ver3.ProductSimple productsimple = new class_ver3.ProductSimple();
                    //根据dt中的商品ID找到对应的商品实例
                    goodsmodel = bll_goods.GetModel(Int32.Parse(ClsSystem.gnvl(row["productid"], "") == "" ? "0" : ClsSystem.gnvl(row["productid"], "")));
                    if (goodsmodel == null || goodsmodel.dr == 1 || goodsmodel.IsEnabled == 0)
                        return new ResultCompanyProductSearch() { Result = "F", Description = "商品信息异常" };
                    productsimple.ProductID = goodsmodel.ID.ToString();
                    productsimple.ProductCode = goodsmodel.GoodsCode;
                    productsimple.ProductName = goodsmodel.GoodsName;
                    //如果传入经销商ID的话，需要取经销商价格
                    if (disID != "")
                    {
                        productsimple.SalePrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID), Convert.ToInt32(disID), goodsmodel.ViewInfoID).ToString("0.00");

                    }
                    else
                    {
                        productsimple.SalePrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID), 0, goodsmodel.ViewInfoID).ToString("0.00");
                    }
                    //判断商品有没收藏
                    List<Hi.Model.BD_DisCollect> alist = Colist != null && Colist.Count > 0
    ? Colist.Where(p => p.GoodsID.ToString() == productsimple.ProductID).ToList()
    : null;
                    productsimple.IsCollect = alist != null && alist.Count > 0 ? "1" : "0";
                    productsimple.IsSale = goodsmodel.IsSale.ToString();
                    productsimple.Title = goodsmodel.Title;
                    //productsimple.Details = goodsmodel.Details;
                    productsimple.Unit = goodsmodel.Unit;
                    productsimple.InStock = goodsmodel.IsOffline.ToString();
                    productsimple.SaleNum = decimal.Parse(ClsSystem.gnvl(row["salenum"], 0)).ToString("0.00");

                    List<class_ver3.Pic> Pic = new List<class_ver3.Pic>();
                    #region //获取图片

                    if (goodsmodel.Pic != "" && goodsmodel.Pic != "X")
                    {
                        class_ver3.Pic pic = new class_ver3.Pic();
                        pic.ProductID = goodsmodel.ID.ToString();
                        pic.IsDeafult = "1";
                        pic.PicUrl = Common.GetPicURL(goodsmodel.Pic, "resize400", int.Parse(CompID));
                        Pic.Add(pic);
                    }

                    Pic.AddRange(GetPicList(goodsmodel.ID.ToString()));

                    #endregion

                    productsimple.ProductPicUrlList = Pic;

                    list_resultproduct.Add(productsimple);
                }
                return new ResultCompanyProductSearch()
                {
                    Result = "T",
                    Description = "返回成功",
                    CompanyID = comp.ID.ToString(),
                    CompanyName = comp.CompName,
                    ProductSimpleList = list_resultproduct
                };
            }
            else
            {
                return new ResultCompanyProductSearch() { Result = "F", Description = "找不到该商品" };
            }
            #endregion
            
            return new ResultCompanyProductSearch() { Result = "T", Description = "返回成功", ProductSimpleList = list_resultproduct };
            
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CompanyProductSearch:" + JSon);
            return new ResultCompanyProductSearch() { Result = "F", Description = "参数异常" };
        }
    }



    //核心企业根据经销商名称/编码，商品名称/编码获取获取商品列表
    /*
     * 有三种情况1.核心企业查询所有商品compid传值，resellerid不传值
     * 2.核心企业修改订单时查询商品compid传值,resellerid传值
     * 3.经销商查询商品compid不传值，resellerid不传值
     */
    public ResultCompanyProductSearch ResellerProductSearch(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string disID = string.Empty;
        string CriticalProductID = string.Empty;
        string gettype = string.Empty;
        string type = string.Empty;
        string search = string.Empty;
        JsonData Filter = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString() == "" || JInfo["CriticalProductID"].ToString() == "" ||
                JInfo["GetType"].ToString() == "" || JInfo["Type"].ToString() == "" || JInfo["CompID"].ToString() == "" || JInfo["ResellerID"].ToString() == "")
            {
                return new ResultCompanyProductSearch() { Result = "F", Description = "参数异常" };
            }
            else
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                CriticalProductID = JInfo["CriticalProductID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                gettype = JInfo["GetType"].ToString();
                type = JInfo["Type"].ToString();
                search = JInfo["Condition"].ToString();
                Filter = JInfo["Filter"];
            }
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            Hi.Model.BD_Distributor dis;
            Hi.Model.BD_Company comp;
            Hi.BLL.BD_Distributor bll_dis = new Hi.BLL.BD_Distributor();
            Hi.BLL.SYS_GType bll_category = new Hi.BLL.SYS_GType();
            Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
            List<Hi.Model.BD_Distributor> list_dis;
            //返回的商品列表
            List<class_ver3.ProductSimple> list_resultproduct = new List<class_ver3.ProductSimple>();
            //如果经销商id是空，则表示核心企业登录
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, 0, Int32.Parse(disID == "" ? "0" : disID)))
            {
                return new ResultCompanyProductSearch() { Result = "F", Description = "登录信息异常" };
            }
            //判断经销商是否存在异常
            dis = new Hi.BLL.BD_Distributor().GetModel(Int32.Parse(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new ResultCompanyProductSearch() { Result = "F", Description = "经销商信息异常" };
            //CompID = dis.CompID.ToString();
            //判断经销商对应的核心企业是否异常
            comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultCompanyProductSearch() { Result = "F", Description = "核心企业信息异常" };
            #endregion
            StringBuilder tablename = new StringBuilder();
            string strWhere = "where 1=1 ";

            if (search != "")
            {
                strWhere += " and a.GoodsName like '%" + search + "%' ";
            }


                if (Filter["IsSales"].ToString() == "1")//表示促销商品
                {
                    List<Hi.Model.BD_Promotion> promotionList = new Hi.BLL.BD_Promotion().GetList("",
                           " compID=" + CompID.ToInt() + " and ProStartTime<='" + DateTime.Now + "' and ProEndTime >='" +
                           DateTime.Now + "' and IsEnabled=1", "");
                    List<Hi.Model.BD_PromotionDetail> detailList = new List<BD_PromotionDetail>();
                    if (promotionList != null && promotionList.Count > 0)
                    {
                        detailList = new Hi.BLL.BD_PromotionDetail().GetList("",
                            " ProID in(" + string.Join(",", promotionList.Select(p => p.ID)) + ")", "");
                    }
                    //if (promotionList.Count == 0)
                    //    return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "今天无促销" };
                    //if (detailList.Count == 0)
                    //    return new BD_GoodsCategory.ResultProductList() { Result = "F", Description = "今天无促销" };
                    var ienum = detailList.Select(p => p.GoodsID);
                    if (ienum.Count() > 0)
                        strWhere += " and a.ID in ( " + string.Join(",", ienum) + ")";
                    else
                        return new ResultCompanyProductSearch() { Result = "T", Description = "没有更多数据", ProductSimpleList = null };
                }
                string productstatus = Filter["ProductStatus"].ToString();
                switch (productstatus)//商品上下架属性
                {
                    case "1":
                        {
                            strWhere += " and ISNULL(a.IsOffline,0) =1";
                        }
                        break;
                    case "2":
                        {
                            strWhere += " and ISNULL(a.IsOffline,0)=0 ";
                        }
                        break;
                }
                if (Filter["MaxPrice"].ToString() != "")//商品最高价格
                {
                    strWhere += " and d.TinkerPrice<=" + decimal.Parse(Filter["MaxPrice"].ToString()) + "";
                }
                if (Filter["MinPrice"].ToString() != "")//商品最低价格
                {
                    strWhere += " and d.TinkerPrice >= " + decimal.Parse(Filter["MinPrice"].ToString()) + "";
                }
                if (Filter["Classif"].ToString() != "-1")//商品分类
                {
                    List<Hi.Model.SYS_GType> list_category_all = new List<Hi.Model.SYS_GType>();
                    //判断分类是否存在
                    Hi.Model.SYS_GType category = bll_category.GetModel(Int32.Parse(Filter["Classif"].ToString()));
                    if (category == null || category.dr == true || category.IsEnabled == false)
                    {
                        return new ResultCompanyProductSearch() { Result = "F", Description = "商品分类异常" };
                    }

                    //判断此分类是不是末级分类
                    List<Hi.Model.SYS_GType> list_category = bll_category.GetList("ID", "ParentId = " + Filter["Classif"].ToString() + " and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1", "");
                    if (list_category != null && list_category.Count > 0)//不是末级的话找出末级
                    {
                        foreach (Hi.Model.SYS_GType goodscategory in list_category)
                        {
                            List<Hi.Model.SYS_GType> list_goodscategory = bll_category.GetList("ID", "ParentId = " + goodscategory.ID + " and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1", "");
                            if (list_goodscategory == null || list_goodscategory.Count == 0)
                            {
                                list_category_all.Add(goodscategory);
                            }
                            else
                            {
                                //while(list_goodscategory!=null&&list_goodscategory.Count>0)
                                //{
                                //    foreach(Hi.Model.BD_GoodsCategory goodscategory2 in list_goodscategory)
                                //    {
                                //        List<Hi.Model.BD_GoodsCategory> list_goodscategory2 =  bll_category.GetList("ID","ParentId = "+goodscategory2.ID+" and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1","");
                                //        if(list_goodscategory2 == null|| list_goodscategory2.Count ==0)
                                //        {
                                //            list_category_all.Add(goodscategory);
                                //        }
                                //        else
                                //        {
                                //            list_goodscategory = list_goodscategory2;
                                //        }
                                //    }
                                //}
                                //因为只有三级所以第三极直接插进去就行了（不用判断有没下级）
                                list_category_all.AddRange(list_goodscategory);
                            }

                        }
                        if (list_category_all != null)
                        {
                            strWhere += " and a.CategoryID in ( -1 ";
                            strWhere = list_category_all.Aggregate(strWhere, (current, goods) => current + ("," + goods.ID)) +
                                       ")";

                        }
                    }
                    //是末级的只要商品类别等于此类别就行了
                    else
                    {
                        strWhere += " and a.CategoryID = " + Filter["Classif"].ToString() + "";
                    }
                    #region//测试取没有分支的末级节点(思想：先去最开始的分支一直取到全部取完，然后依次往后取最近的分支，取完，直到最后)
                    //    while(list_category!=null&&list_category.Count>0)
                    //    {
                    //        int j = 0;
                    //        foreach(Hi.Model.BD_GoodsCategory goodscategory in list_category)
                    //        {
                    //             List<Hi.Model.BD_GoodsCategory> list_goodscategory =  bll_category.GetList("ID","ParentId = "+goodscategory.ID+" and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1","ID");
                    //            if(list_goodscategory == null|| list_goodscategory.Count ==0)
                    //            {
                    //                list_category_all.Add(goodscategory);
                    //                j++;
                    //            }
                    //            else
                    //            {
                    //                list_category = list_goodscategory;
                    //                break;
                    //            }
                    //            if(j==list_category.Count)
                    //            {
                    //                if(goodscategory.ParentId==Int32.Parse( Filter["Classif"].ToString()))
                    //                {
                    //                    list_category =null;
                    //                    break;
                    //                }
                    //                Hi.Model.BD_GoodsCategory model_cate = bll_category.GetModel(goodscategory.ParentId);
                    //                if(model_cate.ParentId ==Int32.Parse( Filter["Classif"].ToString()))
                    //                {
                    //                    list_category =null;
                    //                    break;
                    //                }
                    //                list_category  =   bll_category.GetList("ID","ParentId = "+model_cate.ParentId+" and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1 and ID>"+model_cate.ID+"","ID");
                    //                while((list_category ==null||list_category.Count==0 )&&model_cate.ParentId!=Int32.Parse( Filter["Classif"].ToString()))
                    //                {
                    //                    model_cate= bll_category.GetModel(model_cate.ParentId);
                    //                    list_category  =   bll_category.GetList("ID","ParentId = "+model_cate.ParentId+" and ISNULL(dr,0)=0 and ISNULL(IsEnabled,0)=1 and ID>"+model_cate.ID+"","ID");
                    //                }
                    //            }
                    //        }

                    //}

                    #endregion
                }
                //先结合订单取出销量，再将此表进行分页
                tablename.Append("select isnull( sum(isnull(c.GoodsNum ,0)), 0 ) as salenum,a.ID  as productid,d.TinkerPrice as saleprice,a.CreateDate as createdate  ");
                tablename.Append("  from  BD_Goods a  left join BD_GoodsInfo b on a.ID = b.GoodsID left join DIS_OrderDetail c on b.ID = c.GoodsinfoID ");
                tablename.Append(" left join BD_GoodsInfo d on a.viewinfoid = d.id inner join ");
                tablename.Append(" (select cd.GoodsID from YZT_ContractDetail cd left join YZT_Contract con on cd.ContID=con.ID where compid ='"+CompID+"' and disid ='"+disID+"' ");
                tablename.Append(" union select cm.GoodsID from YZT_CMerchants cm left join YZT_FirstCamp fc on cm.ID =fc.CMID left join ");
                tablename.Append(" YZT_ContractDetail cd on cd.FCID=fc.ID where cm.dr=0 and fc.State=2 and fc.compid ='" + CompID + "' and fc.disid ='" + disID + "') conandfc  on  b.ID = conandfc.GoodsID ");
                tablename.Append(strWhere);
                tablename.Append(" and ISNULL(a.dr,0)=0 and ISNULL(a.IsEnabled,0)=1 and a.CompID=" + CompID + " ");
                tablename.Append(" group by a.ID,a.CreateDate,d.TinkerPrice");
            
            string orderby = "";
            string sort = "";
            //根据传入的type判断排序方式,跟顺序
            switch (type)
            {
                case "0"://表示按照销量排序
                    {
                        orderby = "salenum";
                        sort = "0";
                    }
                    break;
                case "1"://表示按创建时间排序
                    {
                        orderby = "createdate";
                        sort = "0";
                    }
                    break;
                case "2"://表示按价格从高到低
                    {
                        orderby = "saleprice";
                        sort = "0";
                    }
                    break;
                case "3"://表示按价格从低到高
                    {
                        orderby = "saleprice";
                        sort = "1";

                    }
                    break;

            }
            string strsql = new Common().PageSqlString(CriticalProductID, "productid", "(" + tablename.ToString() + ")x", orderby,
                sort, "", gettype, "10");
            //执行sql，取出满足条件的数据
            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];

            #region //返回参数
            if (dsList != null)
            {
                if (dsList.Rows.Count == 0)
                    return new ResultCompanyProductSearch() { Result = "T", Description = "没有更多数据", ProductSimpleList = null };
                Hi.Model.BD_Goods goodsmodel = null;
                //取出此核心企业或经销商所有被收藏的商品ID（用于后面判断商品是否被收藏）
                List<Hi.Model.BD_DisCollect> Colist = new List<Hi.Model.BD_DisCollect>();
                if (disID != "")
                {
                    Colist = Common.GetDataSource<BD_DisCollect>("",
                       " and disID='" + disID + "' and compID=" + Int32.Parse(CompID) + " and IsEnabled =1 and ISNULL(dr,0)=0");
                }
                else
                {
                    Colist = Common.GetDataSource<BD_DisCollect>("",
                       "  and compID=" + Int32.Parse(CompID) + " and IsEnabled =1 and ISNULL(dr,0)=0");
                }
                foreach (DataRow row in dsList.Rows)
                {
                    class_ver3.ProductSimple productsimple = new class_ver3.ProductSimple();
                    //根据dt中的商品ID找到对应的商品实例
                    goodsmodel = bll_goods.GetModel(Int32.Parse(ClsSystem.gnvl(row["productid"], "") == "" ? "0" : ClsSystem.gnvl(row["productid"], "")));
                    if (goodsmodel == null || goodsmodel.dr == 1 || goodsmodel.IsEnabled == 0)
                        return new ResultCompanyProductSearch() { Result = "F", Description = "商品信息异常" };
                    productsimple.ProductID = goodsmodel.ID.ToString();
                    productsimple.ProductCode = goodsmodel.GoodsCode;
                    productsimple.ProductName = goodsmodel.GoodsName;
                    //如果传入经销商ID的话，需要取经销商价格
                    if (disID != "")
                    {
                        productsimple.SalePrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID), Convert.ToInt32(disID), goodsmodel.ViewInfoID).ToString("0.00");

                    }
                    else
                    {
                        productsimple.SalePrice = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID), 0, goodsmodel.ViewInfoID).ToString("0.00");
                    }
                    //判断商品有没收藏
                    List<Hi.Model.BD_DisCollect> alist = Colist != null && Colist.Count > 0
    ? Colist.Where(p => p.GoodsID.ToString() == productsimple.ProductID).ToList()
    : null;
                    productsimple.IsCollect = alist != null && alist.Count > 0 ? "1" : "0";
                    productsimple.IsSale = goodsmodel.IsSale.ToString();
                    productsimple.Title = goodsmodel.Title;
                    //productsimple.Details = goodsmodel.Details;
                    productsimple.Unit = goodsmodel.Unit;
                    productsimple.InStock = goodsmodel.IsOffline.ToString();
                    productsimple.SaleNum = decimal.Parse(ClsSystem.gnvl(row["salenum"], 0)).ToString("0.00");

                    List<class_ver3.Pic> Pic = new List<class_ver3.Pic>();
                    #region //获取图片

                    if (goodsmodel.Pic != "" && goodsmodel.Pic != "X")
                    {
                        class_ver3.Pic pic = new class_ver3.Pic();
                        pic.ProductID = goodsmodel.ID.ToString();
                        pic.IsDeafult = "1";
                        pic.PicUrl = Common.GetPicURL(goodsmodel.Pic, "resize400", goodsmodel.CompID);
                        Pic.Add(pic);
                    }

                    Pic.AddRange(GetPicList(goodsmodel.ID.ToString()));

                    #endregion

                    productsimple.ProductPicUrlList = Pic;

                    list_resultproduct.Add(productsimple);
                }
                return new ResultCompanyProductSearch() { Result = "T", Description = "返回成功", ProductSimpleList = list_resultproduct };
            }
            else
            {
                return new ResultCompanyProductSearch() { Result = "F", Description = "找不到该商品" };
            }
            #endregion

            return new ResultCompanyProductSearch() { Result = "T", Description = "返回成功", ProductSimpleList = list_resultproduct };

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CompanyProductSearch:" + JSon);
            return new ResultCompanyProductSearch() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 图片
    /// </summary>
    /// <param name="goodsID"></param>
    /// <returns></returns>
    public List<class_ver3.Pic> GetPicList(string goodsID)
    {
        List<class_ver3.Pic> picList = new List<class_ver3.Pic>();
        List<Hi.Model.BD_ImageList> imgList = new Hi.BLL.BD_ImageList().GetList("", " dr=0 and GoodsID='" + goodsID + "'", "");
        if (imgList != null && imgList.Count > 0)
        {
            foreach (var img in imgList)
            {
                class_ver3.Pic pic = new class_ver3.Pic();
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
    /// 核心企业获取商品计量单位
    /// </summary>
    /// <param name="goodsID"></param>
    /// <returns></returns>
    public ResultUnit GetUnit(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        try
        {
#region//JSon取值

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "")
                return new ResultUnit() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new ResultUnit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
           Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultUnit() { Result = "F", Description = "核心企业信息异常" };
            
#endregion
            //获取计量单位
            List<Hi.Model.BD_DefDoc_B> list_unit = new Hi.BLL.BD_DefDoc_B().GetList("ID,AtVal",
                "Compid=" + CompID + " and AtName='计量单位' and isnull(dr,0)=0", "");
            //获取计量单位
            List<Unit> list_result = new List<Unit>();
            foreach (Hi.Model.BD_DefDoc_B model in list_unit)
            {
                Unit unit = new Unit();
                unit.UnitID = model.ID.ToString();
                unit.UnitName = model.AtVal;
                list_result.Add(unit);
            }
            //获取属性模板
            List<Hi.Model.BD_Template> list_template_model = new Hi.BLL.BD_Template().GetList("ID,TemplateName",
                "CompID = "+comp.ID+" and isnull(isenabled,0)=1 and isnull(dr,0)=0 ","");
            List<GoodsTemplate> list_template = new List<GoodsTemplate>();
                foreach (Hi.Model.BD_Template model in list_template_model)
                {
                    GoodsTemplate template = new GoodsTemplate();
                    template.TemplateID = model.ID.ToString();
                    template.TemplateName = model.TemplateName;
                    list_template.Add(template);
                }
            //获取商品标签
                List<Hi.Model.SYS_SysName> list_span = new Hi.BLL.SYS_SysName().GetList("Value", "CompID=" + CompID + " and Name = '商品标签管理' and isnull(dr,0)=0", "");
                List<GoodsSpan> list_goodsspan = new List<GoodsSpan>();
                if (list_span != null && list_span.Count > 0)
                {
                    Hi.Model.SYS_SysName sysname = list_span[0];
                    string[] value = sysname.Value.Split(',');
                    for (int i = 0; i < value.Length; i++)
                    {
                        GoodsSpan span = new GoodsSpan();
                        span.SpanValue = value[i];
                        list_goodsspan.Add(span);
                    }
                }
            return new ResultUnit() { Result= "T",Description="获取成功",UnitList=list_result,GoodsTemplateList= list_template,GoodsSpanList=list_goodsspan};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetUnit:" + JSon);
            return new ResultUnit() { Result = "F", Description = "获取失败" };
        }

    }

    /// <summary>
    /// 核心企业根据规格属性模板获取模板属性
    /// </summary>
    /// <param name="goodsID"></param>
    /// <returns></returns>
    public TemplateValueResult GetTemplateValue(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string TemplateID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["TemplateID"].ToString().Trim() == "")
                return new TemplateValueResult() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            TemplateID = JInfo["TemplateID"].ToString();
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new TemplateValueResult() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new TemplateValueResult() { Result = "F", Description = "核心企业信息异常" };
            //判断属性模板是否正确
            Hi.Model.BD_Template template = new Hi.BLL.BD_Template().GetModel(Int32.Parse(TemplateID));
            if (template.CompID != comp.ID || template.IsEnabled != 1)
                return new TemplateValueResult() { Result="F",Description="属性模板异常"};
            if (template.dr == 1)
                return new TemplateValueResult() { Result = "F", Description = "此属性模板已被删除" };
            #endregion
            //获取属性模板值信息
            string sql = @"select COUNT(*) from BD_Template as a, BD_TemplateAttribute as b,BD_TemplateAttrValue as c
where a.ID=b.TemplateID and c.TemplateAttrID=b.ID and a.CompID=" + CompID + " and c.CompID=" + CompID + " and b.CompID=" + CompID + " and a.ID=" + template.ID;
            string count = ClsSystem.gnvl(SqlAccess.ExecuteScalar(sql,SqlHelper.LocalSqlServer),"0");
            DataTable dt = new DataTable();
            if (count == "0")
                dt = GetIDByname("DISTINCT ID,AttributeName,STUFF((SELECT ','+AttrValue   FROM (select a.ID, AttributeName,AttrValue from BD_TemplateAttribute as a left join BD_Attribute as b on a.AttributeID=b.ID left join BD_AttributeValues as c on c.AttributeID=b.ID  where ISNULL(a.dr,0)=0 and  ISNULL(b.dr,0)=0 and  ISNULL(c.dr,0)=0  and a.CompID=" + CompID + " and b.CompID=" + CompID + " and c.CompID=" + CompID + " and a.TemplateID=" + template.ID + ") as B WHERE b.AttributeName = A.AttributeName FOR XML PATH('')),1,1,'')AS AttrValue", "(select a.ID,AttributeName,AttrValue from BD_TemplateAttribute as a  left join BD_Attribute as b on a.AttributeID=b.ID left join BD_AttributeValues as c on c.AttributeID=b.ID where ISNULL(a.dr,0)=0 and  ISNULL(b.dr,0)=0 and  ISNULL(c.dr,0)=0 and a.CompID=" + CompID + " and b.CompID=" + CompID + " and c.CompID=" + CompID + " and a.TemplateID=" + template.ID + ") AS A", "1=1 order by a.ID ");
            else
                dt = GetIDByname("DISTINCT ID,AttributeName,STUFF((SELECT ','+AttrValue   FROM (select a.ID, AttributeName,AttrValue from BD_TemplateAttribute as a left join BD_Attribute as b on a.AttributeID=b.ID left join BD_AttributeValues as c on c.AttributeID=b.ID   left join BD_TemplateAttrValue as d on d.TemplateAttrID=a.ID and d.AttributeValueID=c.id where ISNULL(a.dr,0)=0 and  ISNULL(b.dr,0)=0 and  ISNULL(c.dr,0)=0 and ISNULL(d.dr,0)=0 and d.CompID=" + CompID + "  and a.CompID=" + CompID + " and b.CompID=" + CompID + " and c.CompID=" + CompID + " and a.TemplateID=" + template.ID + ") as B WHERE b.AttributeName = A.AttributeName FOR XML PATH('')),1,1,'')AS AttrValue", "(select a.ID,AttributeName,AttrValue from BD_TemplateAttribute as a  left join BD_Attribute as b on a.AttributeID=b.ID left join BD_AttributeValues as c on c.AttributeID=b.ID where ISNULL(a.dr,0)=0 and  ISNULL(b.dr,0)=0 and  ISNULL(c.dr,0)=0 and a.CompID=" + CompID + " and b.CompID=" + CompID + " and c.CompID=" + CompID + " and a.TemplateID=" + template.ID + ") AS A", "1=1 order by a.ID ");
            List<TemplateValue> list_temvalue = new List<TemplateValue>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TemplateValue temvalue = new TemplateValue();
                temvalue.AttrID = ClsSystem.gnvl(dt.Rows[i]["ID"], "");
                temvalue.AttrName = ClsSystem.gnvl(dt.Rows[i]["AttributeName"], "");
                temvalue.AttrValue = ClsSystem.gnvl(dt.Rows[i]["AttrValue"], "");
                list_temvalue.Add(temvalue);
            }
            return new TemplateValueResult() { Result= "T",Description = "获取成功",TemplateValueList = list_temvalue};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetTemplateValue:" + JSon);
            return new TemplateValueResult() { Result = "F", Description = "获取失败" };
        }
    }


    /// <summary>
    /// 根据name找到对应ID
    /// </summary>
    /// <param name="clounms">列名</param>
    /// <param name="table">表名</param>
    /// <param name="wheres">条件</param> 
    /// <returns>Id值</returns>
    public DataTable GetIDByname(string clounms, string table, string wheres)
    {
        DataTable dt = null;
        try
        {
            Hi.BLL.PAY_PrePayment prebll = new Hi.BLL.PAY_PrePayment();
            dt = prebll.GetDate(clounms, table, wheres);//"id","BD_Goods",""
            return dt;
        }
        catch
        {
            dt = null;
        }
        return dt;

    }

    /// <summary>
    /// 核心企业修改商品信息
    /// </summary>
    /// <param name="goodsID"></param>
    /// <returns></returns>
    public ResultProductsEdit EditGoods(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["Product"].ToString().Trim() == "")
                return new ResultProductsEdit() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            JsonData product = JInfo["Product"];
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new ResultProductsEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultProductsEdit() { Result = "F", Description = "核心企业信息异常" };
            int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存
            #endregion
            Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
            Hi.Model.BD_Goods good = bll_goods.GetModel(Int32.Parse(product["ProductID"].ToString()));
            //判断商品是否存在异常
            if (good == null)
                return new ResultProductsEdit() { Result ="F",Description ="商品异常"};
            //判断时间戳
            if (good.ts.ToString() != product["ts"].ToString())
                return new ResultProductsEdit() { Result = "F", Description = "商品已被他人操作请稍后再试" };
            string productname = Common.NoHTML(product["ProductName"].ToString().Trim());
            if(productname=="")
                 return new ResultProductsEdit() { Result = "F", Description = "请输入商品名称" };
            //判断商品分类是否正确
            Hi.BLL.BD_GoodsCategory bll_goodscategory = new Hi.BLL.BD_GoodsCategory();
            Hi.Model.BD_GoodsCategory goodscategory = bll_goodscategory.GetModel(Int32.Parse(product["ClassifyID"].ToString()));
            if (goodscategory == null || goodscategory.CompID != comp.ID || goodscategory.IsEnabled != 1)
                return new ResultProductsEdit() { Result = "F", Description = "商品分类异常" };
            if (goodscategory.dr == 1)
                return new ResultProductsEdit() { Result = "F", Description = "商品分类已被删除" };
            //判断商品分类是否是末级分类
            List<Hi.Model.BD_GoodsCategory> list_goodscategory = bll_goodscategory.GetList("id",
                "ParentId = " + product["ClassifyID"] + " and isnull(dr,0)=0 and isnull(isenabled,0) =1", "");
            if (list_goodscategory != null && list_goodscategory.Count > 0)
                return new ResultProductsEdit() { Result = "F", Description = "此分类不是末级分类，请选择末级分类" };
            //判断计量单位是否存在
            //List<Hi.Model.BD_DefDoc_B> list_unit = new Hi.BLL.BD_DefDoc_B().GetList("id",
            //    "CompID= " + comp.ID + " and AtName = '计量单位' and isnull(dr,0)=0 and AtVal = '" + product["Unit"] + "'", "");
            //if (list_unit == null || list_unit.Count <= 0)
            //    return new ResultProductsEdit() { Result = "F", Description = "计量单位错误" };
            //选择商品规格属性模板时需要判断规格模板是否正确
            if (product["TemplateID"] != null && product["TemplateID"].ToString().Trim() != "")
            {
                Hi.Model.BD_Template template = new Hi.BLL.BD_Template().GetModel(Int32.Parse(product["TemplateID"].ToString()));
                if (template == null || template.CompID != comp.ID || template.IsEnabled != 1)
                    return new ResultProductsEdit() { Result = "F", Description = "规格模板错误" };
                if (template.dr == 1)
                    return new ResultProductsEdit() { Result = "F", Description = "此规格模板已被删除" };
            }
            //修改bd_goods表
            good.ts = DateTime.Now;
            good.modifyuser = one.ID;
            good.GoodsName =productname;
            good.CategoryID = goodscategory.ID;
            good.Unit = product["Unit"].ToString();
            good.SalePrice = decimal.Parse(product["SalePrice"].ToString());
            good.IsIndex = product["SortIndex"].ToString() == "" ? 0 : int.Parse(product["SortIndex"].ToString());
            if (product["IsRecommend"].ToString() == "1")
                good.IsRecommended = 2;
            else if (product["IsShop"].ToString() == "1")
                good.IsRecommended = 1;
            else
                good.IsRecommended = 0;
            good.Title = Common.NoHTML(ClsSystem.gnvl(product["Title"], ""));
            good.Details = Common.NoHTML(ClsSystem.gnvl(product["Details"], ""));
            //判断是否启用多规格属性
            if (product["ProductAttributeList"] == null || string.IsNullOrEmpty(product["ProductAttributeList"].ToString().Trim()))
            {
                good.IsAttribute = 0;
            }
            else
            {
                good.IsAttribute = 1;
            }
            good.TemplateId = product["TemplateID"].ToString().Trim() == "" ? 0 : Int32.Parse(product["TemplateID"].ToString().Trim());
            #region //更新商品标签表
            List<Hi.Model.BD_GoodsLabels> list_goodslabel = new List<Hi.Model.BD_GoodsLabels>();
            //选择商品标签时需要判断商品标签是否正确
            if (product["GoodsSpanList"] != null && string.IsNullOrEmpty(product["GoodsSpanList"].ToString().Trim()))
            {

                List<Hi.Model.SYS_SysName> list_span = new Hi.BLL.SYS_SysName().GetList("Value", "CompID=" + CompID + " and Name = '商品标签管理' and isnull(dr,0)=0", "");
                if (list_span == null || list_span.Count <= 0)
                    return new ResultProductsEdit() { Result = "F", Description = "商品标签错误" };
                string[] span_s = list_span[0].Value.Split(',');
                //将选中的商品标签取出
                JsonData spanlist = product["GoodsSpanList"];
                Hi.Model.BD_GoodsLabels goodlable = null;
                //循环判断所选的每一个标签是否正确
                foreach (JsonData span in spanlist)
                {
                    if (span_s.ToList().IndexOf(span["GoodsSpanValue"].ToString()) < 0)
                    {
                        return new ResultProductsEdit() { Result = "F", Description = "商品标签错误" };
                    }
                    //正确需要在bd——goodslables表中插入一条数据
                    else
                    {
                        goodlable = new Hi.Model.BD_GoodsLabels();
                        goodlable.CompID = comp.ID;
                        goodlable.GoodsID = good.ID;
                        goodlable.ts = DateTime.Now;
                        goodlable.LabelName = span["GoodsSpanValue"].ToString();
                        goodlable.modifyuser = one.ID;
                        list_goodslabel.Add(goodlable);
                    }
                }
            }
            #endregion
            #region//更新sku信息
            JsonData skulist = product["SKUList"];
            List<Hi.Model.BD_GoodsInfo> list_goodsinfo = new Hi.BLL.BD_GoodsInfo().GetList("",
                "GoodsID = " + product["ProductID"] + " and isnull(isenabled,0)=1 and isnull(dr,0)=0", "");
            List<Hi.Model.BD_GoodsInfo> list_info_update = new List<Hi.Model.BD_GoodsInfo>();//需要更新的goodsinfo数据
            List<Hi.Model.BD_GoodsInfo> list_info_add = new List<Hi.Model.BD_GoodsInfo>();//需要新增的goodsinfo数据
            //循环skulist分别对每个sku数据进行操作
            Hi.Model.BD_GoodsInfo infomodel = null;
            //string maxcode = "";
            int code = 0;
            int isoffine = 0;//整个商品的上下架信息
            List<Hi.Model.SYS_SysCode> list_syscode = new Hi.BLL.SYS_SysCode().GetList("", "isnull(dr,0)=0 and compId=" + CompID + "and codeName='P" + CompID + "'", "");
            if (list_syscode != null && list_syscode.Count > 0)
            {
                code = Int32.Parse(list_syscode[0].CodeValue);
            }
            foreach (JsonData goodsinfo in skulist)
            {
                //如果是修改的sku信息取出，数据库中对应的此条信息
                if (goodsinfo["SKUID"].ToString().Trim() != "")
                {
                    infomodel = list_goodsinfo.Find(p => p.ID == Int32.Parse(goodsinfo["SKUID"].ToString().Trim()));
                    list_goodsinfo.Remove(infomodel);
                    //修改的数据，国际条码跟上下架属性直接用传过来的
                    infomodel.BarCode = Common.NoHTML(goodsinfo["BarCode"].ToString());
                    //修改的数据调整后价格就是传过来的调整后价格
                    infomodel.TinkerPrice = decimal.Parse(ClsSystem.gnvl(goodsinfo["TinkerPrice"], "0"));
                }
                //如果是新增的sku信息，则需要在数据库中新增一条数据
                else
                {
                    infomodel = new Hi.Model.BD_GoodsInfo();
                    infomodel.CompID = comp.ID;
                    infomodel.GoodsID = Int32.Parse(product["ProductID"].ToString());
                    infomodel.CreateDate = DateTime.Now;
                    infomodel.CreateUserID = one.ID;
                    //新增的数据国际条码如果存入值是空的话就自动生成
                    if (ClsSystem.gnvl(goodsinfo["BarCode"], "") == "")
                    {
                        code++;
                        //maxcode = GoodsCode("", CompID);
                        infomodel.BarCode = "P" + CompID + code.ToString().PadLeft(6, '0');
                    }
                    else
                        infomodel.BarCode = Common.NoHTML(goodsinfo["BarCode"].ToString());
                    //新增的数据的调整后价格就是基础价格
                    infomodel.TinkerPrice = decimal.Parse(goodsinfo["SalePrice"].ToString());
                    //设置valueinfo与value1，2，3
                    infomodel.ValueInfo = Common.NoHTML(ClsSystem.gnvl(goodsinfo["ValueInfo"], ""));
                    if (goodsinfo["ProductAttValueIDList"].ToString() != "")
                    {
                        int i = 0;
                        foreach (JsonData ProductAttValue in goodsinfo["ProductAttValueIDList"])
                        {
                            i++;
                            //根据i值将属性值分别插入到value1,value2,value3中
                            switch (i)
                            {
                                case 1:
                                    infomodel.Value1 = Common.NoHTML(ProductAttValue["ProductAttributeValueID"].ToString());
                                    break;
                                case 2:
                                    infomodel.Value2 = Common.NoHTML(ProductAttValue["ProductAttributeValueID"].ToString());
                                    break;
                                case 3:
                                    infomodel.Value3 = Common.NoHTML(ProductAttValue["ProductAttributeValueID"].ToString());
                                    break;
                            }
                        }
                    }
                    
                }
                infomodel.IsOffline = int.Parse(goodsinfo["InStock"].ToString());
                //有一个商品sku上架，则商品上架
                if (infomodel.IsOffline == 1)
                    isoffine = 1;
                infomodel.SalePrice = decimal.Parse(goodsinfo["SalePrice"].ToString());
                infomodel.ts = DateTime.Now;
                infomodel.modifyuser = one.ID;
                infomodel.dr = 0;
                infomodel.IsEnabled = true;
                if (IsInve == 0 && (goodsinfo["Inventory"].ToString() == "" || decimal.Parse(goodsinfo["Inventory"].ToString()) < 0))
                    return new ResultProductsEdit() { Result = "F", Description = "已开启库存，请输入商品库存" };
                if (goodsinfo["Inventory"].ToString() == "")
                {
                    infomodel.Inventory = 0;
                }
                //inventory传-1的话，就是没开启库存情况下修改sku信息，不用改库存
                else if (decimal.Parse(goodsinfo["Inventory"].ToString()) < 0)
                {
                }
                else
                {
                    infomodel.Inventory = decimal.Parse(goodsinfo["Inventory"].ToString());
                }
                //infomodel.ValueInfo = ClsSystem.gnvl(goodsinfo["ValueInfo"], "");
                //导入属性值，直接根据valueinfo截取
                //if (infomodel.ValueInfo != "")
                //{
                //    //先将最后一个冒号截掉,再将剩下的字符串根据；号拆分成字符串数组
                //    string[] value = infomodel.ValueInfo.Substring(0, infomodel.ValueInfo.Length - 2).Split('；');
                //    //循环字符串数组
                //    for (int i = 0; i < value.Length; i++)
                //    {
                //        if (value[i].IndexOf(":") < 0)
                //            continue;
                //        //根据i值将属性值分别插入到value1,value2,value3中
                //        switch (i)
                //        {
                //            case 1:
                //                infomodel.Value1 = value[i].Substring(0, value[i].IndexOf(":") - 1);
                //                break;
                //            case 2:
                //                infomodel.Value2 = value[i].Substring(0, value[i].IndexOf(":") - 1);
                //                break;
                //            case 3:
                //                infomodel.Value3 = value[i].Substring(0, value[i].IndexOf(":") - 1);
                //                break;
                //        }


                //    }
                //}
                if (goodsinfo["SKUID"].ToString().Trim() != "")
                    list_info_update.Add(infomodel);
                else
                    list_info_add.Add(infomodel);
            }
            #endregion

            //更新数据顺便更新goodsattr表跟syscode表
            SqlConnection conn = new SqlConnection(SqlHelper.LocalSqlServer);
            conn.Open();
            SqlTransaction mytran = conn.BeginTransaction();
            try
            {
                //删除原先的goodslables表
                List<Hi.Model.BD_GoodsLabels> list_goodlabels = new Hi.BLL.BD_GoodsLabels().GetList("", "goodsid = " + good.ID + " and compID = " + comp.ID + " and isnull(dr,0) =0 ", "");
                int isdeletelable = new Hi.BLL.BD_GoodsLabels().Delete(good.ID, comp.ID, mytran);
                if (list_goodlabels == null || list_goodlabels.Count <= 0)
                    isdeletelable = 1;
                //将新的goodslables表的数据插入数据库
                if (list_goodslabel != null && list_goodscategory.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsLabels goodlable in list_goodslabel)
                    {
                        if (new Hi.BLL.BD_GoodsLabels().Add(goodlable, mytran) <= 0)
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                        }
                    }
                }
                //删除，更新和添加goodsinfo表信息
                //删除
                Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
                if (list_goodsinfo != null && list_goodsinfo.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsInfo goodinfo_del in list_goodsinfo)
                    {
                        goodinfo_del.dr = 1;
                        goodinfo_del.ts = DateTime.Now;
                        goodinfo_del.modifyuser = one.ID;
                        if (!bll_goodsinfo.Update(goodinfo_del, mytran))
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                        }
                    }
                }
                //更新
                if (list_info_update != null && list_info_update.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsInfo goodinfo_up in list_info_update)
                    {
                        if (!bll_goodsinfo.Update(goodinfo_up, mytran))
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                        }
                    }
                }
                //新增
                if (list_info_add != null && list_info_add.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsInfo goodinfo_add in list_info_add)
                    {
                        if (bll_goodsinfo.Add(goodinfo_add, mytran) <= 0)
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                        }
                    }
                }

                //更新goods表
                good.IsOffline = isoffine;
                bool isgoodsture = bll_goods.Update(good, mytran);
                if (!isgoodsture || isdeletelable <= 0)
                {
                    mytran.Rollback();
                    return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                }
                //删除原有的BD_GoodsAttrs和BD_GoodsAttrsInfo表数据
                Hi.BLL.BD_GoodsAttrs bll_goodsattr = new Hi.BLL.BD_GoodsAttrs();
                List<Hi.Model.BD_GoodsAttrs> list_goodsattrs = bll_goodsattr.GetList("", "isnull(dr,0)=0 and compid=" + CompID + " and goodsid=" + good.ID, "");
                if (list_goodsattrs != null && list_goodsattrs.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsAttrs item in list_goodsattrs)
                    {
                        item.dr = 1;
                        item.ts = DateTime.Now;
                        item.modifyuser = one.ID;
                        if (!bll_goodsattr.Update(item, mytran))
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                        }
                    }
                }
                Hi.BLL.BD_GoodsAttrsInfo bll_attrinfo = new Hi.BLL.BD_GoodsAttrsInfo();
                List<Hi.Model.BD_GoodsAttrsInfo> list_attrinfo = bll_attrinfo.GetList("", "isnull(dr,0)=0 and compid=" + CompID + " and goodsid=" + good.ID, "");
                if (list_attrinfo != null && list_attrinfo.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsAttrsInfo item2 in list_attrinfo)
                    {
                        item2.dr = 1;
                        item2.ts = DateTime.Now;
                        item2.modifyuser = one.ID;
                        if (!bll_attrinfo.Update(item2, mytran))
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                        }
                    }
                }
                //将新的数据插入BD_GoodsAttrs和BD_GoodsAttrsInfo表
                if (product["ProductAttributeList"] != null && !string.IsNullOrEmpty(product["ProductAttributeList"].ToString().Trim()))
                {
                    JsonData attrlist = product["ProductAttributeList"];
                    foreach (JsonData attr in attrlist)
                    {
                        Hi.Model.BD_GoodsAttrs attrmodel = new Hi.Model.BD_GoodsAttrs();
                        attrmodel.CompID = comp.ID;
                        attrmodel.GoodsID = good.ID;
                        attrmodel.AttrsName = Common.NoHTML(attr["ProductAttributeName"].ToString());
                        attrmodel.ts = DateTime.Now;
                        attrmodel.modifyuser = one.ID;
                        attrmodel.dr = 0;
                        int attrid = 0;
                        if ((attrid = bll_goodsattr.Add(attrmodel, mytran)) > 0)//插入属性值表
                        {
                            if (attr["ProductAttValueList"] != null && attr["ProductAttValueList"].ToString().Trim() != "")
                            {
                                JsonData attrvaluelist = attr["ProductAttValueList"];
                                foreach (JsonData attrvalue in attrvaluelist)
                                {
                                    Hi.Model.BD_GoodsAttrsInfo attrinfomodel = new Hi.Model.BD_GoodsAttrsInfo();
                                    attrinfomodel.AttrsID = attrid;
                                    attrinfomodel.CompID = comp.ID;
                                    attrinfomodel.GoodsID = good.ID;
                                    attrinfomodel.modifyuser = one.ID;
                                    attrinfomodel.ts = DateTime.Now;
                                    attrinfomodel.AttrsInfoName = Common.NoHTML(attrvalue["ProductAttValueName"].ToString());
                                    attrinfomodel.dr = 0;
                                    if (bll_attrinfo.Add(attrinfomodel, mytran) <= 0)
                                    {
                                        mytran.Rollback();
                                        return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                                    }
                                }
                            }
                        }
                        else
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                        }
                    }
                }
                //更新sys_syscode表
                Hi.BLL.SYS_SysCode bll_syscode = new Hi.BLL.SYS_SysCode();
                if (list_syscode != null && list_syscode.Count > 0)
                {
                    list_syscode[0].CodeValue = code.ToString();
                    list_syscode[0].ts = DateTime.Now;
                    list_syscode[0].modifyuser = one.ID;
                    if (!bll_syscode.Update(list_syscode[0], mytran))
                    {
                        mytran.Rollback();
                        return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                    }
                }
                else
                {
                    Hi.Model.SYS_SysCode syscodemodel = new Hi.Model.SYS_SysCode();
                    syscodemodel.CompID = comp.ID;
                    syscodemodel.CodeName = "P" + comp.ID;
                    syscodemodel.CodeValue = code.ToString();
                    syscodemodel.ts = DateTime.Now;
                    syscodemodel.dr = 0;
                    syscodemodel.modifyuser = one.ID;
                    if (bll_syscode.Add(syscodemodel, mytran) <= 0)
                    {
                        mytran.Rollback();
                        return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
                    }
                }
                mytran.Commit();
            }
            catch
            {
                mytran.Rollback();
                return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
            }
            finally
            {
                conn.Close();
                mytran.Dispose();
            }
            //将生成的商品的第一条sku信息，放入bd_goods表中
            Common com = new Common();
           // good.ViewInfos = com.GoodsType(good.ID.ToString(), CompID);
            List<Hi.Model.BD_GoodsInfo> list_info = new Hi.BLL.BD_GoodsInfo().GetList("",
                " GoodsID=" + good.ID + "  and CompID=" + CompID + " and IsEnabled=1 and isnull(dr,0)=0", "");
            if (list_info != null && list_info.Count > 0)
                good.ViewInfoID = list_info[0].ID;
            bll_goods.Update(good);
            return new ResultProductsEdit() { Result = "T", Description = "修改成功" };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditGoods:" + JSon);
            return new ResultProductsEdit() { Result = "F", Description = "修改失败" };
        }
 
    }


    /// <summary>
    /// 核心企业新增商品
    /// </summary>
    /// <param name="goodsID"></param>
    /// <returns></returns>
    public ResultProductsEdit AddGoods(string JSon)

    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["Product"].ToString().Trim() == "")
                return new ResultProductsEdit() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            JsonData product = JInfo["Product"];
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new ResultProductsEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultProductsEdit() { Result = "F", Description = "核心企业信息异常" };
            int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存
            #endregion
            Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
            //判断商品名称是否输入
            string productname = Common.NoHTML(product["ProductName"].ToString().Trim());
            if (productname == "")
                return new ResultProductsEdit() { Result = "F", Description = "请输入商品名称" };
            //判断商品分类是否正确
            Hi.BLL.BD_GoodsCategory bll_goodscategory = new Hi.BLL.BD_GoodsCategory();
            Hi.Model.BD_GoodsCategory goodscategory = bll_goodscategory.GetModel(Int32.Parse(product["ClassifyID"].ToString()));
            if (goodscategory == null || goodscategory.CompID != comp.ID || goodscategory.IsEnabled != 1)
                return new ResultProductsEdit() { Result = "F", Description = "商品分类异常" };
            if (goodscategory.dr == 1)
                return new ResultProductsEdit() { Result = "F", Description = "商品分类已被删除" };
            //判断商品分类是否是末级分类
            List<Hi.Model.BD_GoodsCategory> list_goodscategory = bll_goodscategory.GetList("id",
                "ParentId = " + product["ClassifyID"] + " and isnull(dr,0)=0 and isnull(isenabled,0) =1", "");
            if (list_goodscategory != null && list_goodscategory.Count > 0)
                return new ResultProductsEdit() { Result = "F", Description = "此分类不是末级分类，请选择末级分类" };
            //判断计量单位是否存在
            List<Hi.Model.BD_DefDoc_B> list_unit = new Hi.BLL.BD_DefDoc_B().GetList("id",
                "CompID= " + comp.ID + " and AtName = '计量单位' and isnull(dr,0)=0 and AtVal = '" + product["Unit"] + "'", "");
            if (list_unit == null || list_unit.Count <= 0)
                return new ResultProductsEdit() { Result = "F", Description = "计量单位错误" };
            //选择商品规格属性模板时需要判断规格模板是否正确
            if (product["TemplateID"] != null && product["TemplateID"].ToString().Trim() != "")
            {
                Hi.Model.BD_Template template = new Hi.BLL.BD_Template().GetModel(Int32.Parse(product["TemplateID"].ToString()));
                if (template == null || template.CompID != comp.ID || template.IsEnabled != 1)
                    return new ResultProductsEdit() { Result = "F", Description = "规格模板错误" };
                if (template.dr == 1)
                    return new ResultProductsEdit() { Result = "F", Description = "此规格模板已被删除" };
            }
            //新建bd_goods对象
            Hi.Model.BD_Goods good = new Hi.Model.BD_Goods();
            good.CompID = comp.ID;
            good.GoodsCode = "";

            good.ts = DateTime.Now;
            good.modifyuser = one.ID;
            good.GoodsName = productname;
            good.CategoryID = goodscategory.ID;
            good.Unit = product["Unit"].ToString();
            good.SalePrice = decimal.Parse(product["SalePrice"].ToString());
            good.IsIndex = product["SortIndex"].ToString() == "" ? 0 : int.Parse(product["SortIndex"].ToString());
            good.IsSale = 0;
            good.Pic = "";
            good.Pic2 = "";
            good.Pic3 = "";
            if (product["IsRecommend"].ToString() == "1")
                good.IsRecommended = 2;
            else if (product["IsShop"].ToString() == "1")
                good.IsRecommended = 1;
            else
                good.IsRecommended = 0;
            good.Title = Common.NoHTML(ClsSystem.gnvl(product["Title"], ""));
            good.Details = Common.NoHTML(ClsSystem.gnvl(product["Details"], ""));
            //判断是否启用多规格属性
            if (product["ProductAttributeList"] == null || string.IsNullOrEmpty(product["ProductAttributeList"].ToString().Trim()))
            {
                good.IsAttribute = 0;
            }
            else
            {
                good.IsAttribute = 1;
            }
            good.TemplateId = product["TemplateID"].ToString().Trim() == "" ? 0 : Int32.Parse(product["TemplateID"].ToString().Trim());
            good.memo = "";
            good.IsEnabled = 1;
            good.CreateUserID = one.ID;
            good.CreateDate = DateTime.Now;
            good.dr = 0;
            good.IsFirstShow = false;
            good.Sortindex = 0;
            good.IsLS = 0;
            #region //更新商品标签表
            List<Hi.Model.BD_GoodsLabels> list_goodslabel = new List<Hi.Model.BD_GoodsLabels>();
            //选择商品标签时需要判断商品标签是否正确
            if (product["GoodsSpanList"] != null && string.IsNullOrEmpty(product["GoodsSpanList"].ToString().Trim()))
            {

                List<Hi.Model.SYS_SysName> list_span = new Hi.BLL.SYS_SysName().GetList("Value", "CompID=" + CompID + " and Name = '商品标签管理' and isnull(dr,0)=0", "");
                if (list_span == null || list_span.Count <= 0)
                    return new ResultProductsEdit() { Result = "F", Description = "商品标签错误" };
                string[] span_s = list_span[0].Value.Split(',');
                //将选中的商品标签取出
                JsonData spanlist = product["GoodsSpanList"];
                Hi.Model.BD_GoodsLabels goodlable = null;
                //循环判断所选的每一个标签是否正确
                foreach (JsonData span in spanlist)
                {
                    if (span_s.ToList().IndexOf(span["GoodsSpanValue"].ToString()) < 0)
                    {
                        return new ResultProductsEdit() { Result = "F", Description = "商品标签错误" };
                    }
                    //正确需要在bd——goodslables表中插入一条数据
                    else
                    {
                        goodlable = new Hi.Model.BD_GoodsLabels();
                        goodlable.CompID = comp.ID;
                        //goodlable.GoodsID = good.ID;
                        goodlable.ts = DateTime.Now;
                        goodlable.LabelName = span["GoodsSpanValue"].ToString();
                        goodlable.modifyuser = one.ID;
                        list_goodslabel.Add(goodlable);
                    }
                }
            }
            #endregion
            #region//sku的数据
            JsonData skulist = product["SKUList"];
            List<Hi.Model.BD_GoodsInfo> list_info_add = new List<Hi.Model.BD_GoodsInfo>();//需要新增的goodsinfo数据
            //循环skulist分别对每个sku数据进行操作
            Hi.Model.BD_GoodsInfo infomodel = null;
            //string maxcode = "";
            int code = 0;
            int isoffine = 0;//整个商品的上下架信息
            List<Hi.Model.SYS_SysCode> list_syscode = new Hi.BLL.SYS_SysCode().GetList("", "isnull(dr,0)=0 and compId=" + CompID + "and codeName='P" + CompID + "'", "");
            if (list_syscode != null && list_syscode.Count > 0)
            {
                code = Int32.Parse(list_syscode[0].CodeValue);
            }
            foreach (JsonData goodsinfo in skulist)
            {
                infomodel = new Hi.Model.BD_GoodsInfo();
                infomodel.CompID = comp.ID;
                //infomodel.GoodsID = Int32.Parse(product["ProductID"].ToString());
                infomodel.CreateDate = DateTime.Now;
                infomodel.CreateUserID = one.ID;
                //新增的数据国际条码如果存入值是空的话就自动生成
                if (ClsSystem.gnvl(goodsinfo["BarCode"], "") == "")
                {
                    code++;
                    //maxcode = GoodsCode("", CompID);
                    infomodel.BarCode = "P" + CompID + code.ToString().PadLeft(6, '0');
                }
                else
                    infomodel.BarCode = Common.NoHTML(goodsinfo["BarCode"].ToString());
                //新增的数据的调整后价格就是基础价格
                infomodel.TinkerPrice = decimal.Parse(goodsinfo["SalePrice"].ToString());
                //设置valueinfo与value1，2，3
                infomodel.ValueInfo = Common.NoHTML(ClsSystem.gnvl(goodsinfo["ValueInfo"], ""));
                if (goodsinfo["ProductAttValueIDList"].ToString() != "")
                {
                    int i = 0;
                    foreach (JsonData ProductAttValue in goodsinfo["ProductAttValueIDList"])
                    {
                        i++;
                        //根据i值将属性值分别插入到value1,value2,value3中
                        switch (i)
                        {
                            case 1:
                                infomodel.Value1 = Common.NoHTML(ProductAttValue["ProductAttributeValueID"].ToString());
                                break;
                            case 2:
                                infomodel.Value2 = Common.NoHTML(ProductAttValue["ProductAttributeValueID"].ToString());
                                break;
                            case 3:
                                infomodel.Value3 = Common.NoHTML(ProductAttValue["ProductAttributeValueID"].ToString());
                                break;
                        }
                    }
                }

                infomodel.IsOffline = int.Parse(goodsinfo["InStock"].ToString());
                //有一个商品sku上架，则商品上架
                if (infomodel.IsOffline == 1)
                    isoffine = 1;
                infomodel.SalePrice = decimal.Parse(goodsinfo["SalePrice"].ToString());
                infomodel.ts = DateTime.Now;
                infomodel.modifyuser = one.ID;
                infomodel.IsEnabled = true;
                infomodel.dr = 0;
                if (IsInve == 0 && goodsinfo["Inventory"].ToString() == "")
                    return new ResultProductsEdit() { Result = "F", Description = "已开启库存，请输入商品库存" };
                if (goodsinfo["Inventory"].ToString() == "")
                {
                    infomodel.Inventory = 0;
                }
                else
                {
                    infomodel.Inventory = decimal.Parse(goodsinfo["Inventory"].ToString());
                }
                list_info_add.Add(infomodel);
                    
            }
            #endregion
            #region//开启事务更新数据库
            SqlConnection conn = new SqlConnection(SqlHelper.LocalSqlServer);
            conn.Open();
            SqlTransaction mytran = conn.BeginTransaction();
            int goodid = 0;
            try
            {
                //新增bd_GOODS数据
                goodid = bll_goods.Add(good, mytran);
                if (goodid <= 0)
                {
                    mytran.Rollback();
                    return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
                }
                //将新的goodslables表的数据插入数据库
                if (list_goodslabel != null && list_goodscategory.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsLabels goodlable in list_goodslabel)
                    {
                        goodlable.GoodsID = goodid;
                        if (new Hi.BLL.BD_GoodsLabels().Add(goodlable, mytran) <= 0)
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
                        }
                    }
                }
                //新增bd_goodsinfo
                if (list_info_add != null && list_info_add.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsInfo goodinfo_add in list_info_add)
                    {
                        goodinfo_add.GoodsID = goodid;
                        if (new Hi.BLL.BD_GoodsInfo().Add(goodinfo_add,mytran)<=0)
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
                        }
                    }
                }
                //将新的数据插入BD_GoodsAttrs和BD_GoodsAttrsInfo表
                if (product["ProductAttributeList"] != null && !string.IsNullOrEmpty(product["ProductAttributeList"].ToString().Trim()))
                {
                    JsonData attrlist = product["ProductAttributeList"];
                    foreach (JsonData attr in attrlist)
                    {
                        Hi.Model.BD_GoodsAttrs attrmodel = new Hi.Model.BD_GoodsAttrs();
                        attrmodel.CompID = comp.ID;
                        attrmodel.GoodsID =goodid;
                        attrmodel.AttrsName = Common.NoHTML(attr["ProductAttributeName"].ToString());
                        attrmodel.ts = DateTime.Now;
                        attrmodel.modifyuser = one.ID;
                        attrmodel.dr = 0;
                        int attrid = 0;
                        if ((attrid = new Hi.BLL.BD_GoodsAttrs().Add(attrmodel, mytran)) > 0)//插入属性值表
                        {
                            if (attr["ProductAttValueList"] != null && attr["ProductAttValueList"].ToString().Trim() != "")
                            {
                                JsonData attrvaluelist = attr["ProductAttValueList"];
                                foreach (JsonData attrvalue in attrvaluelist)
                                {
                                    Hi.Model.BD_GoodsAttrsInfo attrinfomodel = new Hi.Model.BD_GoodsAttrsInfo();
                                    attrinfomodel.AttrsID = attrid;
                                    attrinfomodel.CompID = comp.ID;
                                    attrinfomodel.GoodsID = goodid;
                                    attrinfomodel.modifyuser = one.ID;
                                    attrinfomodel.ts = DateTime.Now;
                                    attrinfomodel.AttrsInfoName = Common.NoHTML(attrvalue["ProductAttValueName"].ToString());
                                    attrinfomodel.dr = 0;
                                    if (new Hi.BLL.BD_GoodsAttrsInfo().Add(attrinfomodel, mytran) <= 0)
                                    {
                                        mytran.Rollback();
                                        return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
                                    }
                                }
                            }
                        }
                        else
                        {
                            mytran.Rollback();
                            return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
                        }
                    }
                }
                //更新或新增sys_syscode表
                Hi.BLL.SYS_SysCode bll_syscode = new Hi.BLL.SYS_SysCode();
                //之前有数据就修改
                if (list_syscode != null && list_syscode.Count > 0)
                {
                    list_syscode[0].CodeValue = code.ToString();
                    list_syscode[0].ts = DateTime.Now;
                    list_syscode[0].modifyuser = one.ID;
                    if (!bll_syscode.Update(list_syscode[0], mytran))
                    {
                        mytran.Rollback();
                        return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
                    }
                }
                else//无数据新增
                {
                    Hi.Model.SYS_SysCode syscodemodel = new Hi.Model.SYS_SysCode();
                    syscodemodel.CompID = comp.ID;
                    syscodemodel.CodeName = "P" + comp.ID;
                    syscodemodel.CodeValue = code.ToString();
                    syscodemodel.ts = DateTime.Now;
                    syscodemodel.dr = 0;
                    syscodemodel.modifyuser = one.ID;
                    if (bll_syscode.Add(syscodemodel, mytran) <= 0)
                    {
                        mytran.Rollback();
                        return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
                    }
                }
                mytran.Commit();
            }
            catch
            {
                mytran.Rollback();
                return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
            }
            finally
            {
                conn.Close();
                mytran.Dispose();
            }
            //将生成的商品的第一条sku信息，放入bd_goods表中
            Common com = new Common();
            good.ViewInfos = com.GoodsType(goodid.ToString(), CompID);
            List<Hi.Model.BD_GoodsInfo> list_info = new Hi.BLL.BD_GoodsInfo().GetList("id",
                " GoodsID=" + goodid + " and isnull(ValueInfo,'')='" + com.sAttr + "' and CompID=" + CompID + " and IsEnabled=1 and isnull(dr,0)=0", "");
            if (list_info != null && list_info.Count > 0)
                good.ViewInfoID = list_info[0].ID;
            bll_goods.Update(good);
            return new ResultProductsEdit() { Result = "T", Description = "保存成功" };
            #endregion
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "AddGoods:" + JSon);
            return new ResultProductsEdit() { Result = "F", Description = "保存失败" };
        }
    }
    /// <summary>
    /// 核心企业判断商品的属性与属性值是否可以删除
    /// </summary>
    /// <param name="goodsID"></param>
    /// <returns></returns>
    public ResultProductsEdit IsAttrDel(String JSon)
    {
        string CompID = string.Empty;
        string UserID = string.Empty;
        string Type = string.Empty;
        string Value = string.Empty;
        string GoodID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["ProductID"].ToString().Trim() == "" ||
                JInfo["Type"].ToString().Trim() == "" || JInfo["Value"].ToString().Trim() == "")
                return new ResultProductsEdit() { Result ="F",Description ="参数异常"};
            CompID = JInfo["CompID"].ToString();
            UserID = JInfo["UserID"].ToString();
            Type = JInfo["Type"].ToString();
            Value = JInfo["Value"].ToString();
            GoodID = JInfo["ProductID"].ToString();
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new ResultProductsEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultProductsEdit() { Result = "F", Description = "核心企业信息异常" };
            #endregion
            //根据商品ID，取出改商品对应的所有SKU信息
            List<Hi.Model.BD_GoodsInfo> list_goodsInfo = new Hi.BLL.BD_GoodsInfo().GetList("","isnull(dr,0)=0 and compid=" + CompID + " and goodsid=" + GoodID+"","");
            string[] valueinfo_all = null;
            string[] attr_value = null;
            DataTable dt_Order = null;
            Hi.BLL.DIS_Order bll_Order = new Hi.BLL.DIS_Order();
            if (list_goodsInfo != null && list_goodsInfo.Count > 0)
            {
                //循环所有的Sku信息，找出对应的属性值或属性的商品
                foreach (Hi.Model.BD_GoodsInfo goodsInfo in list_goodsInfo)
                {
                    //根据；号，将valueinfo拆分每个属性
                    valueinfo_all = goodsInfo.ValueInfo.Split('；');
                    //循环每个属性
                    foreach (string valueinfo_one in valueinfo_all)
                    {
                        //将属性值与属性拆分开
                        attr_value = valueinfo_one.Split(':');
                        //type为0表示删除属性，为1表示删除属性值
                        if (Type == "0")
                        {
                            //attr_value的第0行存的是属性
                            if (attr_value[0] == Value)//如果此SKU的属性跟需要删除的属性一样，需要判断订单中是否存在改SKU的商品
                            {
                                //判断订单中是否存在改SKU的商品
                                dt_Order = bll_Order.GetList("b.GoodsinfoID",
                                    "DIS_Order as a left join DIS_OrderDetail as b on a.ID=b.OrderID",
                                    "isnull(a.dr,0)=0 and isnull(b.dr,0)=0  and a.CompID=" + CompID + " and b.goodsinfoid=" + goodsInfo.ID, "");
                                if (dt_Order != null && dt_Order.Rows.Count > 0)
                                    return new ResultProductsEdit() { Result = "F", Description = "已有订单存在该属性商品，不能直接删除" };
                            }
                        }
                        else if (Type == "1")//type为1表示删除属性值
                        {
                            //attr_value的第1行存的是属性值
                            if (attr_value[1] == Value)//如果此SKU的属性值跟需要删除的属性值一样，需要判断订单中是否存在改SKU的商品
                            {
                                //判断订单中是否存在改SKU的商品
                                dt_Order = bll_Order.GetList("b.GoodsinfoID",
                                    "DIS_Order as a left join DIS_OrderDetail as b on a.ID=b.OrderID",
                                    "isnull(a.dr,0)=0 and isnull(b.dr,0)=0  and a.CompID=" + CompID + " and b.goodsinfoid=" + goodsInfo.ID, "");
                                if (dt_Order != null && dt_Order.Rows.Count > 0)
                                    return new ResultProductsEdit() { Result = "F", Description = "已有订单存在该属性值商品，不能直接删除" };
                                

                            }
                        }
                        else
                            return new ResultProductsEdit() { Result ="F",Description = "参数异常"};
                    }
                }
            }
            return new ResultProductsEdit() { Result = "F",Description ="删除成功"};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditGoods:" + JSon);
            return new ResultProductsEdit() { Result = "F", Description = "获取失败" };
        }
    }



    //获取经销商对应的核心企业的配置信息的返回参数
    public class ResultConfig
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string ProductNum { get; set; }
        public string ProductRebate { get; set; }
        public string ProductInventory { get; set; }
        public String IsFastPay { get; set; }//是否维护快捷支付账号
        public String IsAliPay { get; set; }//是否维护支付宝账号
    }

    //核心企业获取计量单位的返回
    public class ResultUnit
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<Unit> UnitList { get; set; }
        public List<GoodsTemplate> GoodsTemplateList { get; set; }
        public List<GoodsSpan> GoodsSpanList { get; set; }
    }

    public class Unit
    {
        public string UnitName { get; set; }
        public string UnitID { get; set; }
    }

    public class GoodsTemplate
    {
        public String TemplateID { get; set; }
        public String TemplateName { get; set; }
    }

    public class TemplateValue
    {
        public String AttrID { get; set; }
        public String AttrName { get; set; }
        public String AttrValue { get; set; }
    }
    public class GoodsSpan
    {
        public String SpanValue { get; set; }
    }

    //核心企业根据规格属性模板获取模板属性返回参数
    public class TemplateValueResult
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<TemplateValue> TemplateValueList { get; set; }
    }
    //修改商品上下架跟商品库存的返回参数
    public class ResultProductsEdit
    {
        public string Result { get; set; }
        public string Description { get; set; }
    }

    //核心企业根据经销商名称/编码，商品名称/编码获取获取商品列表的返回参数
    public class ResultCompanyProductSearch
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public List<class_ver3.ProductSimple> ProductSimpleList {get;set;}
    }
    ////修改商品上下架跟库存时传入的sku实体
    //public class SKU_Edit
    //{
    //    public string SKUID { get;set;};
    //}
}