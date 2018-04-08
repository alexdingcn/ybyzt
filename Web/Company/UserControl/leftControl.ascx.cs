using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Company_UserControl_leftControl : System.Web.UI.UserControl
{
    public int Erptype = 0;
    public string ShowID
    {
        get { return ShowidLeft.Value.Trim(); }
        set { ShowidLeft.Value = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        databind();
    }

    protected void databind()
    {
        if (Session["UserModel"] is LoginModel)
        {
            #region 权限
            LoginModel model = Session["UserModel"] as LoginModel;
            
            bool isbte = false;
            ////返利
            List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", " CompID=" + model.CompID, "");
            Sysl = Sysl.FindAll(p => p.Name == "订单支付返利是否启用");
            if (Sysl != null && Sysl.Count > 0)
            {
                isbte = Sysl[0].Value.ToString() == "1";
            }
            else
            {
                isbte = false;
            }
            ////是否启用库存
            this.spwhgl.Visible = OrderInfoType.rdoOrderAudit("商品是否启用库存", model.CompID) == "0" ? true : false;

            Erptype = model.Erptype;
            if (model != null)
            {
                //获得用户类型
                Hi.Model.SYS_Users sysUser = new Hi.BLL.SYS_Users().GetModel(model.UserID);
                if (sysUser != null && Common.HasAdminRole(sysUser.ID))
                {
                    //订单
                    //ddgl.Visible = true;
                    ddlb.Visible = true;
                    //ddsh.Visible = true;
                    //ddfh.Visible = true;
                    //ddth.Visible = true;

                    //收款
                    // ddskbl.Visible = true;
                    ddskmx.Visible = true;

                    //zdskbl.Visible = true;
                    //zdskmx.Visible = true;

                    //qbcx.Visible = true;

                    //ddsk.Visible = true;
                    //zdsk.Visible = true;
                    //qyqb.Visible = true;

                    //商品
                    splb.Visible = true;
                    spfl.Visible = true;
                    spsxwh.Visible = true;
                    spggmb.Visible = true;

                    spkc.Visible = true;
                    sprk.Visible = true;
                    spck.Visible = true;
                    sppd.Visible = false;
                    Dd3.Visible = true;

                    //jxsjg.Visible = true;
                    //spbks.Visible = true;
                    spcx.Visible = true;
                    ddcx.Visible = true;

                    //代理商
                    jxslb.Visible = true;
                    jxssh.Visible = true;
                    jxsgly.Visible = true;

                    jxsfl.Visible = true;
                    jxsqy.Visible = true;
                    //dlssyzl.Visible = true;

                    //我的报表
                    ddtj.Visible = true;
                    jxstj.Visible = true;
                    sptj.Visible = true;
                    zltj.Visible = true;
                    spxsmx.Visible = true;
                    
                    //sjfx.Visible = true;
                    //zhfx.Visible = true;
                    //ddfx.Visible = true;
                    //jxsfx.Visible = true;
                    //cpfx.Visible = true;
                    //cpabcfx.Visible = true;
                    //ysfx.Visible = true;

                    //设置
                    skzhgl.Visible = true;
                    gwqxwh.Visible = true;
                    ygzhwh.Visible = true;
                    xgdlmm.Visible = true;
                    xtsz.Visible = true;

                    xxfb.Visible = true;
                    lyhf.Visible = true;
                    qyxx.Visible = true;
                    xsywh.Visible = true;
                    dply.Visible = true;
                    dpxx.Visible = true;
                    dpwh.Visible = true;

                    
                    //商品
                    spxx.Visible = true;
                    spwh.Visible = true;
                    spcxgl.Visible = true;
                    //代理商
                    jxsxx.Visible = true;
                    jsxsz.Visible = true;
                    //设置
                    jbsz.Visible = true;
                    xxwh.Visible = true;
                    wddp.Visible = true;

                    //招商
                    zslb.Visible = true;
                    sylb.Visible = true;
                    dlssy.Visible = true;
                    wdsy.Visible = true;

                    //合同
                    htlb.Visible = true;


                    //我的桌面
                    //wdzm.Visible = true;

                    ddglHref.Attributes["href"] = ResolveUrl("../Order/OrderCreateList.aspx");
                    //ddskHref.Attributes["href"] = ResolveUrl("../Report/CompCollection.aspx");
                    goodsHref.Attributes["href"] = ResolveUrl("../GoodsNew/GoodsList.aspx");
                    disHref.Attributes["href"] = ResolveUrl("../SysManager/DisList.aspx");
                    reportHref.Attributes["href"] = ResolveUrl("../Report/CustSaleRpt.aspx");
                    sysHref.Attributes["href"] = ResolveUrl("../Pay/PayAccountList.aspx");

                    zsHref.Attributes["href"] = ResolveUrl("../CMerchants/CMerchantsList.aspx");
                    htHref.Attributes["href"] = ResolveUrl("../Contract/ContractList.aspx");
                }
                else
                {
                    //string sql = "select rf.FunCode from SYS_RoleSysFun rf join SYS_CompUser u on u.RoleID=rf.RoleID where u.UserID=" + model.UserID;
                    //DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                    List<Hi.Model.SYS_RoleSysFun> list = new Hi.BLL.SYS_RoleSysFun().GetList(" FunCode ", " CompID="+model.CompID+" AND  RoleID IN (SELECT RoleID FROM dbo.SYS_RoleUser WHERE UserID="+model.UserID+" AND IsEnabled=1 AND dr=0) GROUP BY FunCode ", "");
                    for (int i = 0; i < list.Count; i++)
                    {
                        string code = list[i].FunCode.ToString();
                        switch (code)
                        {
                            //订单
                            case "10":
                                ddglHref.Attributes["href"] = ResolveUrl("../Order/OrderCreateList.aspx");
                                break;
                            case "1011":
                                ddlb.Visible = true;
                                break;
                            
                            case "1111":
                                ddskmx.Visible = true;
                                break;
                            //case "1112":
                            //    zdskbl.Visible = true;
                            //    break;
                            //case "1114":
                            //    zdskmx.Visible = true;
                            //    break;
                            //case "1115":
                            //    qbcx.Visible = true;
                            //    break;
                            

                            //商品
                            case "12":
                                goodsHref.Attributes["href"] = ResolveUrl("../GoodsNew/GoodsInfoList.aspx");
                                break;
                            case "1210":
                                splb.Visible = true;
                                break;
                            case "1213":
                                spkc.Visible = true;
                                break;
                            case "1214":
                                spfl.Visible = true;
                                break;
                            //case "1215":
                            //    jxsjg.Visible = true;
                            //    break;
                            case "1216":
                                spsxwh.Visible = true;
                                break;
                            case "1217":
                                spggmb.Visible = true;
                                break;
                            //case "1218":
                            //    spbks.Visible = true;
                            //    break;
                            case "1219":
                                spcx.Visible = true;
                                break;
                            case "1220":
                                spkc.Visible = true;
                                break;
                            case "1221":
                                sprk.Visible = true;
                                break;
                            case "1223":
                                spck.Visible = true;
                                break;
                            case "1225":
                                sppd.Visible = false;
                                break;
                            case "1227":
                                Dd3.Visible = true;
                                break;
                            case "1232":
                                spcx.Visible = true;
                                break;
                            case "1233":
                                ddcx.Visible = true;
                                break;

                            //代理商
                            case "13":
                                disHref.Attributes["href"] = ResolveUrl("../SysManager/DisList.aspx");
                                break;
                            case "1310":
                                jxslb.Visible = true;
                                break;
                            case "1312":
                                jxssh.Visible = true;
                                break;
                            case "1314":
                                jxsgly.Visible = true;
                                break;
                            case "1315":
                                jxsfl.Visible = true;
                                break;
                            case "1316":
                                jxsqy.Visible = true;
                                break;


                            //我的报表
                            case "14":
                                reportHref.Attributes["href"] = ResolveUrl("../Report/CustSaleRpt.aspx");
                                break;
                            case "1410":
                                ddtj.Visible = true;
                                break;
                            case "1411":
                                jxstj.Visible = true;
                                break;
                            case "1412":
                                sptj.Visible = true;
                                break;
                            case "1413":
                                zltj.Visible = true;
                                break;
                            case "1414":
                                spxsmx.Visible = true;
                                break;
                            //case "1416":
                            //    zhfx.Visible = true;
                            //    break;
                            //case "1417":
                            //    ddfx.Visible = true;
                            //    break;
                            //case "1418":
                            //    jxsfx.Visible = true;
                            //    break;
                            //case "1419":
                            //    cpfx.Visible = true;
                            //    break;
                            //case "1420":
                            //    cpabcfx.Visible = true;
                            //    break;
                            //case "1421":
                            //    ysfx.Visible = true;
                            //    break;

                            //设置
                            case "15":
                                sysHref.Attributes["href"] = ResolveUrl("../Pay/PayAccountList.aspx");
                                break;
                            case "1510":
                                skzhgl.Visible = true;
                                break;
                            case "1511":
                                gwqxwh.Visible = true;
                                break;
                            case "1512":
                                ygzhwh.Visible = true;
                                break;
                            case "1513":
                                xtsz.Visible = true;
                                break;

                            case "1514":
                                xxfb.Visible = true;
                                break;
                            case "1515":
                                lyhf.Visible = true;
                                break;
                            case "1516":
                                qyxx.Visible = true;
                                break;
                            case "1517":
                                xsywh.Visible = true;
                                break;
                            case "1518":
                                dpxx.Visible = true;
                                break;     
                            case "1519":
                                dpwh.Visible = true;
                                break;
                            case "1520":
                                dply.Visible = true;
                                break;

                            //招商
                            case "16":
                                zsHref.Attributes["href"] = ResolveUrl("../CMerchants/CMerchantsList.aspx");
                                break;
                            case "1611":
                                zslb.Visible = true;
                                break;
                            case "1614":
                                sylb.Visible = true;
                                break;
                            case "1617":
                                dlssy.Visible = true;
                                break;
                            case "1618":
                                wdsy.Visible = true;
                                break;

                            //合同
                            case "17":
                                htHref.Attributes["href"] = ResolveUrl("../Contract/ContractList.aspx");
                                break;
                            case "1711":
                                htlb.Visible = true;
                                break;

                        }
                        string[] OrderCode = new string[] { "1011", "1012", "1013", "1015" };
                        if (OrderCode.Contains(code))
                        {
                            ddglHref.Attributes["href"] = ResolveUrl("../Order/OrderCreateList.aspx");
                        }
                    }
                    
                    //我的桌面
                    //wdzm.Style.Add("display", "none;");
                    //我要开通
                    //ktwykt.Style.Add("display", "none;"); 


                }
                //我要维护，修改登录密码
                xgdlmm.Visible = true;
                //gmfw.Visible = true;

            }
            #endregion
        }
    }
}