

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using DBUtility;

public partial class Distributor_DealerLeft : System.Web.UI.UserControl
{
    public int master = 0;
    public string ShowID
    {
        get { return Showid.Value.Trim(); }
        set { Showid.Value = value; }
    }
    public int CompID = 0;
    public string price = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        //登录信息
        LoginModel logUser = Session["UserModel"] as LoginModel;
        price = new Hi.BLL.PAY_PrePayment().sums(logUser.DisID, logUser.CompID).ToString("0.00");
        CompID = logUser.CompID;
        
        Bind();
    }

    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        if (Session["UserModel"] is LoginModel)
        {
            #region 权限 

            LoginModel model = Session["UserModel"] as LoginModel;
            if (model != null)
            {
                
                int[] roleList =
                {
                     20,2011,                                      
                     21,2110, 2113,                                      
                     22,2210, 2211, 2212, 2214, 2215, 2217,           
                     23,2312,                                
                     24,2410, 2411, 2412,                                
                     25,2511, 2512, 2513, 2514, 2515, 2516, 2517,                        
                     26,2610, 2611, 2612, 2613, 2614, 2615, 2616, 2617,           
                     27,2710, 2711,                                                      
                     28,2810, 2811, 2812, 2813, 2814, 2815, 2816, 2817, 2818, 2819
                };


                //招商
                Dis2610.Visible = true;
                Dis2612.Visible = true;
                Dis2615.Visible = true;
                Dis2617.Visible = true;

                //暂时都放开
                //判断登录代理商是否存在加盟的厂商
                if (!LoginModel.ExistRoleComp())
                {
                    Dis25.Attributes["href"] = ResolveUrl("DeliveryList.aspx");
                    Dis2511.Visible = true;
                    Dis2512.Visible = true;
                    Dis2513.Visible = true;
                    Dis2514.Visible = true;
                    Dis2515.Visible = true;
                    Dis2516.Visible = true;
                    Dis2517.Visible = true;
                    return;
                }

                #region 老式

                if (IsDisAdmin(int.Parse(Common.DesDecrypt(model.CUID, Common.EncryptKey))))
                {
                    master = 1;
                   
                    Dis20.Attributes["href"] = ResolveUrl("OrderList.aspx");
                    Dis21.Attributes["href"] = ResolveUrl("GoodsList.aspx");
                    Dis22.Attributes["href"] = ResolveUrl("pay/orderPayList.aspx");
                    //Dis23.Attributes["href"] = ResolveUrl("ReturnOrderList.aspx");
                    Dis24.Attributes["href"] = ResolveUrl("Rep/RepOrderList.aspx");
                    Dis25.Attributes["href"] = ResolveUrl("DeliveryList.aspx");
                    //招商
                    Dis26.Attributes["href"] = ResolveUrl("CMerchants/CMerchantsList.aspx");
                    //合同
                    Dis27.Attributes["href"] = ResolveUrl("Contract/ContractList.aspx");
                    //库存
                    Dis28.Attributes["href"] = ResolveUrl("Storage/GoodsStorageList.aspx");

                    Dis2110.Visible = true;
                    Dis2113.Visible = true;

                    Dis2210.Visible = true;
                    Dis2211.Visible = true;
                    //Dis2212.Visible = true;
                    //Dis2214.Visible = true;
                    //Dis2215.Visible = true;
                    //Dis2217.Visible = true;

                    //Dis2310.Visible = true;
                    //Dis2311.Visible = true;
                    //Dis2312.Visible = true;

                    //Dis2410.Visible = true;
                    //Dis2411.Visible = true;
                    //Dis2412.Visible = true;
                    
                    Dis2511.Visible = true;
                    Dis2512.Visible = true;
                    Dis2513.Visible = true;
                    Dis2514.Visible = true;
                    Dis2515.Visible = true;
                    Dis2516.Visible = true;
                    Dis2517.Visible = true;

                    //招商
                    Dis2610.Visible = true;
                    Dis2612.Visible = true;
                    Dis2615.Visible = true;
                    Dis2617.Visible = true;

                    //库存
                    Dis2810.Visible = true;
                    Dis2812.Visible = true;
                    Dis2811.Visible = true;
                    Dis2814.Visible = true;
                    Dis2817.Visible = true;
                    
                }
                else
                {

                    string str = @"select a.RoleID RoleID ,b.FunCode FunCode,c.FunName FunName from [SYS_CompUser] a left JOIN dbo.SYS_RoleUser d ON a.UserID=d.UserID LEFT JOIN [SYS_RoleSysFun] b on d.RoleID = b.RoleID left join [SYS_SysFun] c on b.FunCode = c.FunCode where a.isAudit=2 and a.isEnabled =1 and a.dr= 0 and d.UserID = " + model.UserID + " and d.RoleID IN (SELECT RoleID FROM dbo.SYS_RoleUser  WHERE UserID=" + model.UserID+" AND IsEnabled=1)   and b.isEnabled = 1 and b.dr = 0 and c.type = 2 and c.FunCode in (" + string.Join(",", roleList) +") and c.isEnabled =1 and c.dr = 0 group by a.RoleID,b.FunCode,c.FunName";
                    DataTable dtRole = SqlHelper.Query(SqlHelper.LocalSqlServer, str).Tables[0];
                    if (dtRole != null)
                    {
                        if (dtRole.Rows.Count > 0)
                        {
                            foreach (DataRow item in dtRole.Rows)
                            {
                                string code = item["FunCode"].ToString();
                                switch (code)
                                {
                                    case "20":
                                        Dis20.Attributes["href"] = ResolveUrl("OrderList.aspx");
                                        break;
                                    case "21":
                                        Dis21.Attributes["href"] = ResolveUrl("GoodsList.aspx");
                                        break;
                                    case "22":
                                        Dis22.Attributes["href"] = ResolveUrl("pay/orderPayList.aspx");
                                        break;
                                    //case "23":
                                    //    Dis23.Attributes["href"] = ResolveUrl("ReturnOrderList.aspx");
                                    //    break;
                                    case "24":
                                        Dis24.Attributes["href"] = ResolveUrl("Rep/RepOrderList.aspx");
                                        break;
                                    case "25":
                                        Dis25.Attributes["href"] = ResolveUrl("DeliveryList.aspx");
                                        break;

                                    //招商
                                    case "26":
                                        Dis26.Attributes["href"] = ResolveUrl("CMerchants/CMerchantsList.aspx");
                                        break;
                                    //合同
                                    case "27":
                                        Dis27.Attributes["href"] = ResolveUrl("Contract/ContractList.aspx");
                                        break;
                                    //库存
                                    case "28":
                                        Dis28.Attributes["href"] = ResolveUrl("Storage/GoodsStorageList.aspx");
                                        break;


                                    case "2011":
                                       
                                        break;
                                    case "2110":
                                        Dis2110.Visible = true;
                                        break;
                                    case "2113":
                                        Dis2113.Visible = true;
                                        break;
                                    case "2210":
                                        Dis2210.Visible = true;
                                        break;
                                    case "2211":
                                        Dis2211.Visible = true;
                                        break;
                                    //case "2212":
                                    //    Dis2212.Visible = true;
                                    //    break;
                                    //case "2214":
                                    //    Dis2214.Visible = true;
                                    //    break;
                                    //case "2215":
                                    //    Dis2215.Visible = true;
                                    //    break;
                                    
                                    //case "2310":
                                    //    Dis2310.Visible = true;
                                    //    break;
                                    //case "2311":
                                    //    Dis2311.Visible = true;
                                    //    break;
                                    //case "2312":
                                    //    Dis2312.Visible = true;
                                    //    break;
                                    //case "2410":
                                    //    Dis2410.Visible = true;
                                    //    break;
                                    //case "2411":
                                    //    Dis2411.Visible = true;
                                    //    break;
                                    //case "2412":
                                    //    Dis2412.Visible = true;
                                    //    break;

                                    case "2511":
                                        Dis2511.Visible = true;
                                        break;
                                    case "2512":
                                        Dis2512.Visible = true;
                                        break;
                                    case "2513":
                                        Dis2513.Visible = true;
                                        break;
                                    case "2514":
                                        Dis2514.Visible = true;
                                        break;
                                    case "2515":
                                        Dis2515.Visible = true;
                                        break;
                                    case "2516":
                                        Dis2516.Visible = true;
                                        break;
                                    case "2517":
                                        Dis2517.Visible = true;
                                        break;

                                    //招商
                                    case "2610":
                                        Dis2610.Visible = true;
                                        break;
                                    case "2612":
                                        Dis2612.Visible = true;
                                        break;
                                    case "2615":
                                        Dis2615.Visible = true;
                                        break;
                                    case "2617":
                                        Dis2617.Visible = true;
                                        break;

                                    //库存
                                    case "2810":
                                        Dis2810.Visible = true;
                                        Dis2812.Visible = true;
                                        break;
                                    case "2811":
                                        Dis2811.Visible = true;
                                        break;
                                    case "2814":
                                        Dis2814.Visible = true;
                                        break;
                                    case "2817":
                                        Dis2817.Visible = true;
                                        break;

                                }
                            }
                        }
                    }
                }

                #endregion
            }
            #endregion

          
        }
    }

    //判断是否为管理员登录
    public bool IsDisAdmin(int ID)
    {
        string sql = "select id from SYS_CompUser where isnull(dr,0)=0 and Utype=5 and ID=" + ID;
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            return true;
        else
            return false;
    }
}