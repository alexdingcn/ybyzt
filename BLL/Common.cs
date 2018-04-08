using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBUtility;

namespace BLL
{
    public class Common
    {
        /// <summary>
        /// 获取订单设置
        /// </summary>
        /// <param name="Name">设置名称</param>
        /// <param name="CompID">核心企业ID</param>
        /// <returns>0、不审核,1、审核</returns>
        public static string rdoOrderAudit(string Name, int CompID)
        {
            List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", " CompID=" + CompID + " and Name='" + Name + "' and dr=0", "");
            if (sl != null && sl.Count > 0)
            {
                return sl[0].Value;
            }
            return "0";
        }

        /// <summary>
        /// 判断商品库存是否满足
        /// </summary>
        /// <param name="goodsInfoID">商品信息ID</param>
        /// <param name="OrderId">订单ID</param>
        /// <param name="num">下单商品库存数量</param>
        /// <param name="Inventory">商品库存数量</param>
        /// <returns>false：可以下单  true：商品存库不足</returns>
        public static bool GetInevntory(int CompID, int goodsInfoID, int OrderId, decimal num, out string Inventory)
        {
            Inventory = "0";

            int IsInve = Convert.ToInt32(rdoOrderAudit("商品是否启用库存", CompID));

            if (IsInve == 0)
            {
                string Digits = rdoOrderAudit("订单下单数量是否取整", CompID);

                //启用库存
                if (OrderId != 0)
                {
                    //修改订单时，判断商品库存加上订单明细上的该商品数量
                    List<Hi.Model.DIS_OrderDetail> l = new Hi.BLL.DIS_OrderDetail().GetList("", " IsNull(dr,0)=0 and GoodsInfoID=" + goodsInfoID + " and OrderID=" + OrderId, "");

                    if (l != null && l.Count <= 0)
                        Inventory = "0";
                    else
                        Inventory = (Convert.ToDecimal(l[0].GoodsNum.ToString()) + Convert.ToDecimal(l[0].ProNum.ToString())).ToString();
                }

                Hi.Model.BD_GoodsInfo infoModel = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoID);
                Inventory = (Convert.ToDecimal(Inventory) + infoModel.Inventory).ToString();

                Inventory = decimal.Parse(string.Format("{0:N2}", Inventory)).ToString(Digits);

                if (num != 0 && Convert.ToDecimal(Inventory) < num)
                    return true;
                return false;
            }
            else
                return false;
        }

        /// <summary>
        /// 判断商品是否可下单成功
        /// </summary>
        /// <param name="GoodsInfoID">下单商品ID</param>
        /// <param name="msg">提示信息</param>
        /// <returns>false：可以下单  true：不能下单</returns>
        public static bool GetGoodsInfo(int GoodsInfoID, out string msg)
        {
            msg = "";
            Hi.Model.BD_GoodsInfo infoModel = new Hi.BLL.BD_GoodsInfo().GetModel(GoodsInfoID);

            if (infoModel != null)
            {
                if (infoModel.dr == 1)
                {
                    msg = "商品已删除";
                    return true;
                }
                else if (!infoModel.IsEnabled)
                {
                    msg = "商品已禁用";
                    return true;
                }
                else if (infoModel.IsOffline == 0)
                {
                    msg = "商品已下架";
                    return true;
                }
                return false;
            }
            msg = "商品不存在";
            return true;
        }

        /// <summary>
        /// 下单时，返利是否可以用
        /// </summary>
        /// <param name="CompID">核心企业ID</param>
        /// <param name="DisID">经销商ID</param>
        /// <param name="OrderID">订单ID</param>
        /// <param name="Amount">使用的返利金额</param>
        /// <param name="msg">提示信息</param>
        /// <returns>false：可以下单  true：不能下单</returns>
        public static bool GetRebate(int CompID, int DisID, int OrderID, decimal Amount, out string msg)
        {
            msg = "";
            if (Amount == 0)
            {
                return false;
            }
            int Fanli = Convert.ToInt32(rdoOrderAudit("订单支付返利是否启用", CompID));

            if (Fanli == 1)
            {
                //启用的返利
                Hi.BLL.BD_Rebate bate = new Hi.BLL.BD_Rebate();
                decimal rebate = 0;

                if (OrderID != 0)
                    rebate = (bate.GetEditEnableAmount(OrderID, DisID));
                else
                    rebate = (bate.GetRebateEnableAmount(DisID));

                if (Amount > rebate)
                {
                    msg = "返利余额不足。";
                    return true;
                }
                return false;
            }
            else
            {
                if (OrderID != 0)
                {
                    //没有启用返利时，修改订单时，修改该订单是否用过返利
                    decimal rebate = 0;
                    List<Hi.Model.BD_RebateDetail> list = new Hi.BLL.BD_RebateDetail().GetList("", " OrderID = " + OrderID + " and IsNull(dr,0) = 0", "");
                    if (list != null && list.Count > 0)
                    {
                        rebate = list.Sum(item => item.Amount);
                    }

                    if (Amount < rebate)
                    {
                        msg = "经销商返利不足。";
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 是否可用赊销额度
        /// </summary>
        /// <param name="CompID">厂商ID</param>
        /// <param name="DisID">代理商ID</param>
        /// <param name="CreditAmount">赊销额度</param>
        /// <returns>false 不可赊销，true 可赊销</returns>
        public static bool GetCredit(int CompID, int DisID, out decimal CreditAmount)
        {
            CreditAmount = 0;

            List<Hi.Model.SYS_CompUser> usersList = new Hi.BLL.SYS_CompUser().GetList("", "  CompID=" + CompID.ToString() + " and DisID=" + DisID + " and Isnull(IsAudit,0)=2 and Isnull(IsEnabled,0)=1", "");

            if (usersList != null && usersList.Count > 0) {
                if (usersList[0].CreditType != 0)
                {
                    CreditAmount = usersList[0].CreditAmount;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断订单促销
        /// </summary>
        /// <param name="TotalPrice">下单总金额</param>
        /// <returns>返回促销的金额</returns>
        public static decimal GetProPrice(decimal TotalPrice, out string proID, out string ProIDD, out string ProType, int CompId)
        {
            proID = "0";
            ProIDD = "0";
            ProType = "";
            string sql = string.Format(@"select p.ID,p.Type,p.ProType,p.ProStartTime,p.ProEndTime,pd2.OrderPrice, pd2.Discount,pd2.ID IDD from BD_Promotion as p left join BD_PromotionDetail2 as pd2 on p.ID=pd2.ProID where p.[type]=2 and IsEnabled=1 and isnull(p.dr,0)=0 and p.CompID={0}  and (p.ProStartTime<=GETDATE() and DATEADD(D,1,p.ProEndTime)>GETDATE()) order by p.CreateDate desc,pd2.OrderPrice desc", CompId);

            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

            decimal Price = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Price = 0;
                    if (Convert.ToDecimal(item["OrderPrice"].ToString()) <= TotalPrice)
                    {
                        proID = item["ID"].ToString();
                        ProIDD = item["IDD"].ToString();
                        if (item["ProType"].ToString() == "5")
                        {
                            //满减
                            Price = Convert.ToDecimal(item["Discount"].ToString());
                            //ProType,订单满减价格，打折率(protype=5 满减金额，=6满减折扣)
                            ProType = "5," + item["OrderPrice"].ToString() + "," + item["Discount"].ToString();
                        }
                        else if (item["ProType"].ToString() == "6")
                        {
                            //满折
                            decimal Discount = Convert.ToDecimal(item["Discount"].ToString());
                            TotalPrice = TotalPrice * (1 - (Discount / 100));

                            Price = Convert.ToDecimal(TotalPrice);
                            ProType = "6," + item["OrderPrice"].ToString() + "," + item["Discount"].ToString();
                        }
                        return Price;
                    }
                }
                return Price;
            }
            return 0;
        }

        /// <summary>
        /// 获取商品最新的促销
        /// </summary>
        /// <param name="GoodsID">商品ID</param>
        /// <param name="GoodsInfoId">商品信息ID</param>
        /// <param name="CompId">核心企业ID</param>
        /// <param name="Num">商品购买数量</param>
        /// <param name="pty">促销类型（输入参数）</param>
        /// <param name="ppty">促销提示（输入参数）</param>
        /// <param name="proID">促销ID（输入参数）</param>
        /// <returns>返回</returns>
        public static decimal GetProNum(string GoodsID, string GoodsInfoId, int CompId, decimal Num, out string pty, out string ppty, out string proID)
        {
            pty = "0";
            ppty = "";
            proID = "";
            if (GoodsID.ToString() == "" || CompId.ToString() == "")
            {
                return 0;
            }

            string sql = string.Empty;

            if (GoodsInfoId != "")
            {
                sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and prod.GoodInfoID={2} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", GoodsID.ToString(), CompId, GoodsInfoId);
            }
            else
            {
                sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", GoodsID.ToString(), CompId);
            }

            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            DateTime now = DateTime.Now;

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (Convert.ToDateTime(item["ProStartTime"]) <= now && Convert.ToDateTime(item["ProEndTime"]).AddDays(1) > now)
                    {
                        proID = item["ID"].ToString();
                        pty = item["ProType"].ToString();
                        //促销类型,促销方式，商品价格，打折率
                        ppty = item["Type"] + "," + pty + "," + item["GoodsPrice"] + "," + item["DisCount"];

                        /*促销类型，促销方式（特价促销（1、赠品  2、优惠 ）  商品促销（3、满送  4、打折）
                        ），促销价格（1、2、4是商品价格，3是送的商品数量），打折率（ProType = 1、2 是0，3是满件数  4是打折0-100*/
                        //打折率（1、2 是0，3是满件数  4是打折0-100）
                        //赔送商品数量
                        if (Num >= Convert.ToDecimal(item["DisCount"]))
                        {
                            if (item["ProType"].ToString() == "3")
                            {
                                decimal sendNum = Convert.ToInt32(Math.Floor(Num / Convert.ToInt32(item["DisCount"])));
                                return Convert.ToDecimal(item["GoodsPrice"]) * sendNum;
                            }
                            else
                                return 0;
                        }
                        else
                            return 0;
                    }
                }
            }
            return 0;
        }

        #region 获取商品最新价格


        /// <summary>
        /// 获取商品最新价格信息
        /// </summary>
        /// <param name="CompID">核心企业ID</param>
        /// <param name="DisID">经销商ID</param>
        /// <param name="GoodinfoID">商品ID</param>
        /// <returns></returns>
        public static decimal GetGoodsPrice(int CompID, int DisID, int GoodinfoID)
        {
            //获取商品信息
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"select * from dbo.GetGoodsInfoPrice({0},{1},'{2}')", DisID, CompID, GoodinfoID);

            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                //查询商品
                DataRow[] dr = dt.Select(" GoodsInfoID=" + GoodinfoID);
                if (dr.Length > 0)
                    return Convert.ToDecimal(dr[0]["finalPrice"].ToString());
                else
                    return 0;
            }
            return 0;
        }

        /// <summary>
        /// 获取商品最新价格
        /// </summary>
        /// <param name="CompID">核心企业ID</param>
        /// <param name="DisID">经销商ID</param>
        /// <param name="GoodsInfoID">商品ID</param>
        /// <returns></returns>
        public static List<gDprice> GetPrice(int CompID, int DisID, List<int> infoIDl)
        {

            List<gDprice> g = new List<gDprice>();
            gDprice gd = null;
            try
            {
                var disInfoID = string.Empty;

                //判断是否存在商品ID
                if (infoIDl != null && infoIDl.Count > 0)
                {
                    var infoID = string.Join(",", infoIDl);
                    string sql = "select * from dbo.GetGoodsInfoPrice(" + DisID + "," + CompID + ",'" + infoID + "')";

                    DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            gd = new gDprice();
                            gd.goodsInfoId = int.Parse(item["GoodsInfoID"].ToString());
                            //经销商价格
                            gd.disPrice = decimal.Parse(item["disPrice"].ToString());
                            //经销商分类、区域价格
                            gd.typePrice = decimal.Parse(item["typePrice"].ToString());
                            //促销价格
                            gd.bpPrice = decimal.Parse(item["BpPrice"].ToString());
                            //最终价
                            gd.FinalPrice = decimal.Parse(item["finalPrice"].ToString());
                            g.Add(gd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return g;
        }

        /// <summary>
        /// 获取商品最新价格
        /// </summary>
        /// <param name="CompID">核心企业ID</param>
        /// <param name="DisID">经销商ID</param>
        /// <param name="GoodsInfoID">商品ID</param>
        /// <returns></returns>
        public static List<gDprice> GetGoodsPrice(int CompID, int DisID, List<int> infoIDl)
        {
            //返回
            List<gDprice> g = new List<gDprice>();
            gDprice gd = null;
            try
            {
                //没有设置经销商商品价格的商品ID
                var disInfoID = string.Empty;

                //判断是否存在商品ID
                if (infoIDl != null && infoIDl.Count > 0)
                {
                    var infoID = string.Join(",", infoIDl);

                    //经销商品查询
                    Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(DisID);

                    //判断经销商信息是否正确
                    if (disModel == null)
                        return g;

                    //查询经销商品价格
                    List<Hi.Model.BD_GoodsPrice> disPrice = new Hi.BLL.BD_GoodsPrice().GetList("", " isnull(dr,0)=0 and isnull(IsEnabled,0)=1 and CompID=" + CompID + " and DisID=" + DisID + " and GoodsInfoID in (" + infoID + ")", "");

                    if (disPrice != null && disPrice.Count > 0)
                    {
                        //存在经销商品价格
                        foreach (var item in disPrice)
                        {
                            //删除已查询出商品价格的商品ID
                            if (infoIDl.Contains(item.GoodsInfoID))
                                infoIDl.Remove(item.GoodsInfoID);

                            gd = new gDprice();
                            gd.goodsInfoId = item.GoodsInfoID;
                            gd.FinalPrice = item.TinkerPrice;
                            g.Add(gd);
                        }
                    }
                    else
                        disInfoID = infoID;

                    //disInfoID为空时，将 List<int>转为字符串且infoIDl存在参数
                    if (infoIDl.Count > 0)
                        disInfoID = string.Join(",", infoIDl);
                    else
                        return g;

                    //查询经销商分类、区域价格主表
                    List<Hi.Model.BD_DisPrice> ll = new List<Hi.Model.BD_DisPrice>();
                    List<Hi.Model.BD_DisPrice> l = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and isnull(Type,0)!=0 and isnull(IsEnabled,0)=1 and CompID=" + CompID, "");

                    if (l != null && l.Count > 0)
                    {
                        //1、分类  2、区域
                        int priceType = l[0].Type;
                        //查询BD_DisPrice条件
                        StringBuilder str = new StringBuilder();

                        if (priceType == 1)
                        {
                            #region 经销商分类
                            //经销商分类
                            string typeId = string.Empty;
                            string[] typeT = { };

                            //查询经销商分类
                            if (disModel.DisTypeID != 0)
                                typeId = DisTypeId(disModel.DisTypeID, CompID, "");
                            else
                            {
                                //没有经销商分类，查询商品基本价格
                                List<gDprice> ggl = GetgDprice(CompID, disInfoID);
                                if (ggl != null && ggl.Count > 0)
                                {
                                    foreach (var item in ggl)
                                        g.Add(item);
                                }
                                return g;
                            }

                            if (typeId.Length > 0)
                                typeT = typeId.Split(new char[] { ',' });

                            //经销商分类
                            if (typeT.Length - 1 == 1)
                                str.Append("Type=" + priceType + " and (One=" + typeT[0] + " and Two=0 and Three=0)");
                            else if (typeT.Length - 1 == 2)
                                str.Append("Type=" + priceType + " and ((One=" + typeT[0] + " and Two=0 and Three=0) or (One=" + typeT[0] + " and Two=" + typeT[1] + " and Three=0))");
                            else if (typeT.Length - 1 == 3)
                                str.Append("Type=" + priceType + " and ((One=" + typeT[0] + " and Two=0 and Three=0) or (One=" + typeT[0] + " and Two=" + typeT[1] + " and Three=0) or (One=" + typeT[0] + " and Two=" + typeT[1] + " and Three=" + typeT[2] + "))");

                            #endregion
                        }
                        else if (priceType == 2)
                        {
                            #region 经销商区域

                            //经销商区域
                            string areaId = string.Empty;
                            string[] areaA = { };
                            //查询经销商区域
                            if (disModel.AreaID != 0)
                                areaId = DisAreaId(disModel.AreaID, CompID, "");
                            else
                            {
                                //没有经销商区域，查询商品基本价格
                                List<gDprice> ggl = GetgDprice(CompID, disInfoID);
                                if (ggl != null && ggl.Count > 0)
                                {
                                    foreach (var item in ggl)
                                        g.Add(item);
                                }
                                return g;
                            }

                            if (areaId.Length > 0)
                                areaA = areaId.Split(new char[] { ',' });

                            //经销商区域
                            if (areaA.Length - 1 == 1)
                                str.Append("Type=" + priceType + " and One=" + areaA[0] + " and Two=0 and Three=0");
                            else if (areaA.Length - 1 == 2)
                                str.Append("Type=" + priceType + " and ((One=" + areaA[0] + " and Two=0 and Three=0) or (One=" + areaA[0] + " and Two=" + areaA[1] + " and Three=0))");
                            else if (areaA.Length - 1 == 3)
                                str.Append("Type=" + priceType + " and ((One=" + areaA[0] + " and Two=0 and Three=0) or (One=" + areaA[0] + " and Two=" + areaA[1] + " and Three=0) or (One=" + areaA[0] + " and Two=" + areaA[1] + " and Three=" + areaA[2] + "))");

                            #endregion
                        }

                        //查询设置的商品价格
                        ll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and isnull(IsEnabled,0)=1 and " + str.ToString() + " and CompID=" + CompID, "");
                        if (ll != null && ll.Count > 0)
                        {
                            //存在经销商分类、区域的商品价格，取出其价格
                            string pId = string.Empty;
                            foreach (var item in ll)
                                pId += item.ID + ",";

                            if (pId.Length > 0)
                                pId = pId.Substring(0, pId.Length - 1);

                            List<Hi.Model.BD_DisPriceInfo> pinfol = new Hi.BLL.BD_DisPriceInfo().GetList("", " CompID=" + CompID + " and GoodsInfoID in(" + disInfoID + ")", "");

                            if (pinfol != null && pinfol.Count > 0)
                            {
                                foreach (var item in pinfol)
                                {
                                    //删除已查询出商品价格的商品ID
                                    if (infoIDl.Contains(item.GoodsInfoID))
                                        infoIDl.Remove(item.GoodsInfoID);

                                    gd = new gDprice();
                                    gd.goodsInfoId = item.GoodsInfoID;
                                    gd.FinalPrice = item.TinkerPrice;
                                    g.Add(gd);
                                }

                                //disInfoID为空时，将 List<int>转为字符串且infoIDl存在参数
                                if (infoIDl.Count > 0)
                                    disInfoID = string.Join(",", infoIDl);
                                else
                                    return g;

                                //不存在经销商分类、区域的商品价格，查询商品基本价格
                                List<gDprice> ggl = GetgDprice(CompID, disInfoID);
                                if (ggl != null && ggl.Count > 0)
                                {
                                    foreach (var item in ggl)
                                        g.Add(item);
                                }
                            }
                            else
                            {
                                //不存在经销商分类、区域的商品价格，查询商品基本价格
                                List<gDprice> ggl = GetgDprice(CompID, disInfoID);
                                if (ggl != null && ggl.Count > 0)
                                {
                                    foreach (var item in ggl)
                                        g.Add(item);
                                }
                            }
                        }
                        else
                        {
                            //不存在经销商分类、区域的商品价格，查询商品基本价格
                            List<gDprice> ggl = GetgDprice(CompID, disInfoID);
                            if (ggl != null && ggl.Count > 0)
                            {
                                foreach (var item in ggl)
                                    g.Add(item);
                            }
                        }
                    }
                    else
                    {
                        //不存在经销商分类、区域的商品价格，查询商品基本价格
                        List<gDprice> ggl = GetgDprice(CompID, disInfoID);
                        if (ggl != null && ggl.Count > 0)
                        {
                            foreach (var item in ggl)
                                g.Add(item);
                        }
                    }
                }
                else
                    return g;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return g;
        }

        /// <summary>
        /// 查询商品基本价格
        /// </summary>
        /// <param name="CompID">核心企业ID</param>
        /// <param name="disInfoID">商品ID</param>
        /// <returns></returns>
        public static List<gDprice> GetgDprice(int CompID, string disInfoID)
        {
            List<gDprice> g = new List<gDprice>();

            //不存在经销商分类、区域的商品价格，查询商品基本价格
            List<Hi.Model.BD_GoodsInfo> infol = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(IsOffline,0)=1 and isnull(dr,0)=0 and isnull(IsEnabled,0)=1 and CompID=" + CompID + " and ID in(" + disInfoID + ")", "");

            if (infol != null && infol.Count > 0)
            {
                foreach (var item in infol)
                {
                    gDprice gd = new gDprice();
                    gd.goodsInfoId = item.ID;
                    gd.FinalPrice = item.TinkerPrice;
                    g.Add(gd);
                }
            }
            else
                return g;

            return g;
        }

        //区域Id
        public static string aredIdList = string.Empty;
        //分类Id
        public static string typeIdList = string.Empty;

        /// <summary>
        /// 递归得到区域Id
        /// </summary>
        public static string DisAreaId(int id, int compId, string aredId = "")
        {
            aredIdList = aredId;
            List<Hi.Model.BD_DisArea> l = new Hi.BLL.BD_DisArea().GetList("", "ID=" + id + " and isnull(dr,0)=0 and CompanyID=" + compId, "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_DisArea item in l)
                {
                    aredIdList = item.ID + "," + aredIdList;
                    DisAreaId(item.ParentID, compId, aredIdList);
                }
            }
            return aredIdList;
        }

        /// <summary>
        /// 递归得到分类Id
        /// </summary>
        public static string DisTypeId(int id, int compId, string typeId = "")
        {
            typeIdList = typeId;
            List<Hi.Model.BD_DisType> l = new Hi.BLL.BD_DisType().GetList("", "ID=" + id + " and isEnabled=0 and isnull(dr,0)=0 and compid=" + compId, "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_DisType item in l)
                {
                    typeIdList = item.ID + "," + typeIdList;
                    DisTypeId(item.ParentId, compId, typeIdList);
                }
            }
            return typeIdList;
        }

        #endregion


        /// <summary>
        /// 公用分页方法
        /// </summary>
        /// <typeparam name="T">返回实例类型</typeparam>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="CurrentIndex">当前页码</param>
        /// <param name="PageCount">总页数（输出参数）</param>
        /// <param name="DataCount">数据总条数（输出参数）</param>
        /// <param name="Pagestart">分页开始页码数（输出参数）</param>
        /// <param name="PageEnd">分页结束页码数（输出参数</param>
        /// <param name="StrWhart">要查询的字段（默认查询所有）</param>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="OrderName">排序字段</param>
        /// <param name="TableName">要查询的表名（默认为传入的T类型名称）</param>
        /// <returns></returns>
        public static List<T> GetListByPage<T>(int PageSize, int CurrentIndex, out int PageCount, out int DataCount, out int Pagestart, out int PageEnd, string StrWhart = "", string SqlWhere = "", string OrderName = "", string TableName = "")
        {
            return Hi.SQLServerDAL.Common.GetListByPage<T>(PageSize, CurrentIndex, out PageCount, out DataCount, out Pagestart, out PageEnd, StrWhart, SqlWhere, OrderName, TableName);
        }


        public static List<T> GetListByAjaxPage<T>(int PageSize, int CurrentIndex, out int DataCount, string StrWhart = "", string SqlWhere = "", string OrderName = "", string TableName = "")
        {
            return Hi.SQLServerDAL.Common.GetListByAjaxPage<T>(PageSize, CurrentIndex, out DataCount, StrWhart, SqlWhere, OrderName, TableName);
        }

        public static DataTable GetListByAjaxPage(int PageSize, int CurrentIndex, out int DataCount, string TableName, string StrWhart = "", string SqlWhere = "", string OrderName = "")
        {
            return Hi.SQLServerDAL.Common.GetListByAjaxPage(PageSize, CurrentIndex, out DataCount, StrWhart, SqlWhere, OrderName, TableName);
        }

    }

    public class gDprice
    {
        public gDprice()
        {
            goodsID = 0;
            goodsInfoId = 0;
            disPrice = 0;
            typePrice = 0;
            bpPrice = 0;
            FinalPrice = 0;
        }
        public int goodsID;
        public int goodsInfoId;
        public decimal disPrice;
        public decimal typePrice;
        public decimal bpPrice;
        public decimal FinalPrice;
    }

}
