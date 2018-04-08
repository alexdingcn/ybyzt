using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

/// <summary>
/// 查看订单发货记录
/// </summary>
public partial class Company_Order_OrderOutInfo : CompPageBase
{
    Hi.BLL.DIS_OrderOut OrderOutBll = new Hi.BLL.DIS_OrderOut();

    public int OrderId = 0;
    public int OrderOutId = 0;
    public int DisId = 0;
    public int types = 0;
    public string ProID = "0";
    public string ProPrice = "";
    public string ProIDD = "0";
    public string ProType = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request["orderId"] != null)
            {
                OrderId = Convert.ToInt32(Common.DesDecrypt(Request["orderId"].ToString(), Common.EncryptKey));
            }
            Bind();
            ExpressBind();
        }
    }

    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        if (KeyID != 0)
        {
            if (!Common.PageCompOperable("OrderOut", KeyID, CompID))
            {
                Response.Redirect("../../NoOperable.aspx");
                return;
            }

            Hi.Model.DIS_OrderOut OrderOutModel = OrderOutBll.GetModel(KeyID);

            if (OrderOutModel != null)
            {
                OrderId = OrderOutModel.OrderID;
                ViewState["OrderId"] = OrderId;
                OrderOutId = OrderOutModel.ID;
                this.lblReceiptNo.InnerText = OrderOutModel.ReceiptNo;
                this.lblOrderNo.InnerText = OrderInfoType.getOrder(OrderOutModel.OrderID, "ReceiptNo");
                DisId = OrderOutModel.DisID;

                ProID = OrderInfoType.getOrderExt(OrderOutModel.OrderID, "ProID");
                ProPrice = OrderInfoType.getOrderExt(OrderOutModel.OrderID, "ProAmount");
                ProIDD = OrderInfoType.getOrderExt(OrderOutModel.OrderID, "ProDID");
                ProType = OrderInfoType.getOrderExt(OrderOutModel.OrderID, "Protype");

                this.lblDisName.InnerText = Common.GetDis(OrderOutModel.DisID, "DisName");
                this.hidDisId.Value = OrderOutModel.DisID.ToString();

                this.lblSendDate.InnerText = OrderOutModel.SendDate == DateTime.MinValue ? "" : OrderOutModel.SendDate.ToString("yyyy-MM-dd");
                //this.lblExpress.InnerText = OrderOutModel.Express;
                //this.lblExpressNo.InnerText = OrderOutModel.ExpressNo;
                //this.lblExpressPerson.InnerText = OrderOutModel.ExpressPerson;
                //this.lblExpressTel.InnerText = OrderOutModel.ExpressTel;
                //this.lblExpressBao.InnerText = OrderOutModel.ExpressBao.ToString();
                //this.lblPostFee.InnerText = OrderOutModel.PostFee.ToString("N");
                //this.lblActionUser.InnerText = OrderOutModel.ActionUser;
                this.lblRemark.InnerText = OrderOutModel.Remark;

                //签收信息
                this.lblIsSign.InnerText = OrderOutModel.IsSign == 0 ? "未签收" : "已签收";
                this.lblSignUser.InnerText = OrderOutModel.SignUser;
                this.hidSignUserId.Value = OrderOutModel.SignUserId.ToString();
                this.lblSignDate.InnerText = OrderOutModel.SignDate == DateTime.MinValue ? "" : OrderOutModel.SignDate.ToString("yyyy-MM-dd");
                this.lblSignRemark.InnerText = OrderOutModel.SignRemark;

                BindOrderDetail(OrderOutModel.OrderID, OrderOutModel.DisID);

                if (this.Erptype != 0)
                {
                    //非U8、U9等用户  可以对订单进行操作
                    this.li_Express.Visible = true;
                }
            }
        }
        else
        {
            Response.Write("数据错误!");
            Response.End();
        }
    }


    /// <summary>
    /// 绑定订单明细
    /// </summary>
    public void BindOrderDetail(int OrderId, int DisID)
    {
        SelectGoods.Clear();
        SelectGoods.OrderDetail(OrderId, DisID, this.CompID);
        DataTable dt = Session["GoodsInfo"] as DataTable;
        if (dt != null)
        {
            this.rpDtl.DataSource = dt;
            this.rpDtl.DataBind();
        }
        else
        {
            this.rpDtl.DataSource = "";
            this.rpDtl.DataBind();
        }
        SelectGoods.Clear(DisID, this.CompID);
    }


    /// <summary>
    /// 物流信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExpress_Click(object sender, EventArgs e)
    {
        //Bind();
        Response.Redirect("OrderOutInfo.aspx?KeyID=" + Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) + "&go=2");
    }
    /// <summary>
    /// 删除物流
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDel_Click(object sender, EventArgs e)
    {
        List<Hi.Model.DIS_Logistics> l = new Hi.BLL.DIS_Logistics().GetList("", "isnull(dr,0)=0 and orderId=" + ViewState["OrderId"].ToString(), "");
        if (l.Count > 0)
        {
            Hi.Model.DIS_OrderOut model2 = new Hi.BLL.DIS_OrderOut().GetModel(Convert.ToInt32(KeyID));
            SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            try
            {
                foreach (Hi.Model.DIS_Logistics item in l)
                {
                    Hi.Model.DIS_Logistics model = new Hi.BLL.DIS_Logistics().GetModel(item.Id);
                    model.dr = 1;
                    model.ts = DateTime.Now;
                    model.modifyuser = this.UserID;

                    //model2.ExpressNo = "";
                    model2.ts = DateTime.Now;
                    model2.modifyuser = this.UserID;
                    //model2.Express = "";
                    bool bol = new Hi.BLL.DIS_Logistics().Update(model, Tran);
                    new Hi.BLL.DIS_OrderOut().Update(model2, Tran);
                }
                Tran.Commit();
                this.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script> location.replace(location.href);</script>");
                return;
            }
            catch (Exception ex)
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                    {
                        Tran.Rollback();
                    }
                }
                JScript.ShowParentRefresh(this.Page, "删除物流失败");
                JScript.AlertMsgParentRefresh(this.Page, "删除物流失败", JScript.IconOption.哭脸);
            }
        }
    }

    /// <summary>
    /// 物流信息绑定
    /// </summary>
    public void ExpressBind()
    {
        List<Hi.Model.DIS_Order> lll = new Hi.BLL.DIS_Order().GetList("", "isnull(dr,0)=0 and ostate>=5 and id=" + OrderId, "");
        if (lll.Count == 0)
        {
            this.delLogistics.Visible = true;//显示
            this.editLogistics.Visible = true;//显示
        }
        else
        {
            this.li_Express.Visible = false;
            this.delLogistics.Visible = false;//隐藏
            this.editLogistics.Visible = false;//隐藏
        }
        List<Hi.Model.DIS_Logistics> l = new Hi.BLL.DIS_Logistics().GetList("", "isnull(dr,0)=0 and orderId=" + OrderId + " and orderoutId=" + KeyID, "");
        if (l.Count == 0)
        {
            this.li_Express.Visible = true;
            this.delLogistics.Visible = false;
            this.editLogistics.Visible = false;//隐藏
            this.divInfomation.InnerHtml = "暂无物流信息";
        }
        else
        {
            this.li_Express.Visible = false;
            this.delLogistics.Visible = false;
            this.editLogistics.Visible = true;//隐藏

            List<Hi.Model.DIS_Logistics> ll = new Hi.BLL.DIS_Logistics().GetList("", "isnull(dr,0)=0 and orderId=" + OrderId + " and orderOutId=" + KeyID, "");
            if (ll.Count > 0)
            {
                foreach (Hi.Model.DIS_Logistics item in ll)
                {
                    Hi.Model.DIS_Logistics model = new Hi.BLL.DIS_Logistics().GetModel(item.Id);
                    this.lblCompName.InnerText = model.ComPName;
                    this.lblLogisticsNo.InnerText = model.LogisticsNo;
                    types = model.Type;
                    if (model.Type == 1)
                    {
                        this.lblLogisticsInfo.InnerHtml = "以下跟踪信息由<a href=\"http://www.aikuaidi.cn/\" style=\"color: Blue\" target=\"_blank\">爱快递提供</a>，如有疑问请到物流公司官网查询</label>";
                        this.lblwul.Text = "编辑物流";
                        // string ApiKey = "ebb4e2088d483b00";////请把XXXXXX修改成您在快递100网站申请的APIKey
                        string ApiKey = "4088ed72ed034b61b4b5adf05870aeba";
                        string typeCom = model.ComPName;
                        typeCom = Information.TypeCom(typeCom);
                        string nu = model.LogisticsNo;
                        // string apiurl = "http://api.kuaidi100.com/api?id=" + ApiKey + "&com=" + typeCom + "&nu=" + nu + "&show=0&muti=1&order=asc";
                        string apiurl = "http://www.aikuaidi.cn/rest/?key=" + ApiKey + "&order=" + nu + "&id=" + typeCom + "&ord=asc&show=json";
                        WebRequest request = WebRequest.Create(@apiurl);
                        WebResponse response = request.GetResponse();
                        Stream stream = response.GetResponseStream();
                        Encoding encode = Encoding.UTF8;
                        StreamReader reader = new StreamReader(stream, encode);
                        string detail = reader.ReadToEnd();
                        Logistics logistics = JsonConvert.DeserializeObject<Logistics>(detail);
                        //if (logistics.message == "ok")
                        //{
                        if (logistics.errCode == "0")
                        {
                            List<Information> information = logistics.data;
                            this.rptLogistics.DataSource = information;
                            this.rptLogistics.DataBind();
                            model.Context = JsonConvert.SerializeObject(information);
                            new Hi.BLL.DIS_Logistics().Update(model);
                        }

                        //  }
                    }
                    else
                    {
                        this.lblLogisticsInfo.InnerHtml = "请点击<a href=\"JavaScript:;\" style=\"color: Blue\" class=\"aLogisticsEdit\" >编辑物流信息</a>进行新增或者修改物流信息";
                        this.lblwul.Text = "编辑物流信息";
                        JavaScriptSerializer Serializer = new JavaScriptSerializer();
                        List<Information> objs = Serializer.Deserialize<List<Information>>(model.Context);
                        this.rptLogistics.DataSource = objs;
                        this.rptLogistics.DataBind();
                    }
                }
            }
        }
    }

}