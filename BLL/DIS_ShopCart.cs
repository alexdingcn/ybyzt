using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Hi.BLL
{
     /// <summary>
    /// 业务逻辑类 DIS_ShopCart
    /// </summary>
    public partial class DIS_ShopCart
    {
        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        /// <param name="strWhat">字段</param>
        /// <param name="TbName">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderby">排序字段</param>
        /// <returns></returns>
        public DataTable GetList(string strWhat, string TbName, string strWhere, string strOrderby)
        {
            return dal.GetList(strWhat, TbName, strWhere, strOrderby);
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        /// <param name="strWhat">字段</param>
        /// <param name="TbName">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderby">排序字段</param>
        /// <returns></returns>
        public DataTable GetListCart(string strWhat, string TbName, string strWhere, string strOrderby)
        {
            return dal.GetListCart(strWhat, TbName, strWhere, strOrderby);
        }

        /// <summary>
        /// 购物车与商品的连接查询
        /// </summary>
        /// <returns></returns>
        public DataTable GetGoodsCart(string strWhere, string strOrderby)
        {
            return dal.GetGoodsCart(strWhere, strOrderby);
        }

        /// <summary>
        /// 更新经销商购物车
        /// </summary>
        /// <param name="ID">登录用户ID</param>
        /// <param name="CompID">登录用户企业ID</param>
        /// <param name="DisID">登录用户经销商ID</param>
        /// <param name="goodsinfoid">商品ID</param>
        /// <param name="ProID">促销ID</param>
        /// <param name="Num">商品数量</param>
        /// <param name="Price">商品基本价格</param>
        /// <param name="TPrice">商品最终销售价格</param>
        /// /// <param name="CartType">是否添加商品 1、添加到购物车，2、减商品数量 3、加商品数量  4、删除购物车商品</param>
        /// <returns>0、更新失败  1、更新成功</returns>
        public int Cart(int ID,int CompID,int DisID, string goodsinfoid, string ProID, string Num, string Price, string TPrice, string CartType)
        {
            bool falg = false;
            //经销商购物车实体
            Hi.Model.DIS_ShopCart cartModel = new Hi.Model.DIS_ShopCart();
            try
            {
                //判断查询商品详细信息
                System.Data.DataTable goodsinfoDt = new Hi.BLL.DIS_ShopCart().GetList("ID,GoodsID,ValueInfo,SalePrice,TinkerPrice", "BD_GoodsInfo", " ID=" + goodsinfoid + " and isEnabled=1 and IsOffline=1 and CompID=" + CompID, "");
                //判断商品可用
                if (goodsinfoDt != null && goodsinfoDt.Rows.Count > 0)
                {
                    //修改商品
                    if (CartType == "2" || CartType == "3")
                    {
                        //判断该商品在购物车是否想存在
                        string arestr = " GoodsInfoID=" + goodsinfoid + " and DisID=" + DisID + " and CompID=" + CompID;
                        List<Hi.Model.DIS_ShopCart> lmodel = dal.GetModel("top 1 *", arestr);
                        if (lmodel != null && lmodel.Count > 0)
                        {
                            falg = true;
                            foreach (var item in lmodel)
                            {
                                item.Price = Convert.ToDecimal(Price);
                                item.AuditAmount = Convert.ToDecimal(TPrice);
                                item.sumAmount = Convert.ToDecimal(TPrice) * Convert.ToDecimal(Num);
                                cartModel = item;
                            }
                        }
                    }
                    else if (CartType == "1")
                    {
                        //新增商品
                        string arestr = " GoodsInfoID=" + goodsinfoid + " and DisID=" + DisID + " and CompID=" + CompID;
                        List<Hi.Model.DIS_ShopCart> lmodel = dal.GetModel("top 1 *", arestr);

                        if (lmodel != null && lmodel.Count > 0)
                        {
                            falg = true;
                            foreach (var item in lmodel)
                            {
                                cartModel = item;
                                Num = (cartModel.GoodsNum + Convert.ToDecimal(Num)).ToString();
                            }
                        }
                        else
                        {
                            cartModel.CreateUserID = ID;
                            cartModel.CreateDate = DateTime.Now;

                            cartModel.CompID = CompID;
                            cartModel.DisID = DisID;
                            cartModel.GoodsinfoID = Convert.ToInt32(goodsinfoid);
                            cartModel.GoodsID = Convert.ToInt32(goodsinfoDt.Rows[0]["GoodsID"]);
                            cartModel.GoodsInfos = goodsinfoDt.Rows[0]["ValueInfo"].ToString();
                        }
                    }

                    cartModel.ts = DateTime.Now;
                    cartModel.modifyuser = ID;
                    cartModel.GoodsNum = Convert.ToDecimal(Num);
                    cartModel.AuditAmount = Convert.ToDecimal(TPrice);
                    cartModel.Price = Convert.ToDecimal(Price);
                    //单个商品小计
                    cartModel.sumAmount = Convert.ToDecimal(Num) * Convert.ToDecimal(TPrice);

                    if (ProID != "")
                    {
                        //判断促销是否可用
                        DataTable prodt = new Hi.BLL.DIS_ShopCart().GetList("Type,ProType,DisCount", "BD_Promotion", " ID=" + ProID + " and isEnabled=1 and dr=0 and CompID=" + CompID, "");
                        if (prodt != null && prodt.Rows.Count > 0)
                        {
                            cartModel.ProID = Convert.ToInt32(ProID.ToString());
                            cartModel.ProType = Convert.ToInt32(prodt.Rows[0]["ProType"].ToString());
                            cartModel.DisCount = Convert.ToDecimal(prodt.Rows[0]["DisCount"].ToString());

                            //促销为商品促销中的满送，查询促销明细表
                            if (Convert.ToInt32(prodt.Rows[0]["Type"]) == 1 && Convert.ToInt32(prodt.Rows[0]["ProType"]) == 3)
                            {
                                //查询促销明细表
                                System.Data.DataTable prodDt = new Hi.BLL.DIS_ShopCart().GetList("GoodsPrice,SendGoodsInfoID", "BD_PromotionDetail", " ProID=" + ProID + "and GoodInfoID=" + goodsinfoid + " and dr=0 and CompID=" + CompID, "");
                                if (prodDt != null && prodDt.Rows.Count > 0)
                                {
                                    int sendNum = Convert.ToInt32(Convert.ToDecimal(Num)) / Convert.ToInt32(prodt.Rows[0]["DisCount"]);
                                    cartModel.ProNum = sendNum * Convert.ToInt32(prodDt.Rows[0]["GoodsPrice"]);
                                }
                            }
                        }
                    }
                    else
                    {
                        string sql = string.Format(@"select top 1 pro.ID,pro.CompID,pro.ProType,pro.Type,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and prod.GoodInfoID={2} and (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE()) and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", goodsinfoDt.Rows[0]["GoodsID"].ToString(), CompID, goodsinfoid);
                        DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];
                        DateTime now = DateTime.Now;

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                if (Convert.ToDateTime(item["ProStartTime"]) <= now && Convert.ToDateTime(item["ProEndTime"]).AddDays(1) > now)
                                {
                                    cartModel.ProID = Convert.ToInt32(item["ID"].ToString());
                                    cartModel.ProType = Convert.ToInt32(item["ProType"].ToString());
                                    cartModel.DisCount = Convert.ToDecimal(item["DisCount"].ToString());
                                    //促销为商品促销中的满送，查询促销明细表
                                    if (Convert.ToInt32(item["Type"].ToString()) == 1 && Convert.ToInt32(item["ProType"].ToString()) == 3)
                                    {
                                        int sendNum = Convert.ToInt32(Convert.ToDecimal(Num)) / Convert.ToInt32(item["DisCount"]);
                                        cartModel.ProNum = sendNum * Convert.ToInt32(item["GoodsPrice"]);
                                    }
                                }
                            }
                        }
                    }
                    if (falg)
                        return new Hi.BLL.DIS_ShopCart().Update(cartModel) == true ? 1 : 0;
                    return new Hi.BLL.DIS_ShopCart().Add(cartModel);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="wheresrt"></param>
        /// <returns></returns>
        public bool CartEmpty(string wheresrt)
        {
            return dal.CartEmpty(wheresrt);
        }

        /// <summary>
        /// 返回购物车商品总数量,总价
        /// </summary>
        /// <returns></returns>
        public DataTable SumCartNum(string CompID, string DisID)
        {
            return dal.SumCartNum(CompID, DisID);
        }

        /// <summary>
        /// 返回单个商品小计
        /// </summary>
        /// <param name="CompID"></param>
        /// <param name="DisID"></param>
        /// <param name="GoodsinfoID"></param>
        /// <returns></returns>
        public decimal GetAuditAmount(string CompID, string DisID, string GoodsinfoID)
        {
            return dal.GetAuditAmount(CompID, DisID, GoodsinfoID);
        }
    }
}
