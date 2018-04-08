using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

public partial class Company_ShopManager_RecommendGoodsPic2 : AdminPageBase
{
    public string pic1 = string.Empty;
    public string pic2 = string.Empty;
    public string pic3 = string.Empty;
    public string pic4 = string.Empty;
    public string pic5 = string.Empty;
    public string url1 = string.Empty;
    public string url2 = string.Empty;
    public string url3 = string.Empty;
    public string url4 = string.Empty;
    public string url5 = string.Empty;
    int comPid = 0;
    public int id1 = 0;
    public int id2 = 0;
    public int id3 = 0;
    public int id4 = 0;
    public int id5 = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["KeyID"];
        if (obj != null)
        {
            comPid = Convert.ToInt32(obj.ToString());
        }
        if (!IsPostBack)
        {
            if (comPid != 0)
            {
                List<Hi.Model.BD_ShopImageList> l = new Hi.BLL.BD_ShopImageList().GetList("", "isnull(dr,0)=0 and compId=" + comPid + " and type=2", "");
                if (l.Count > 0)
                {
                    int i = 0;
                    foreach (Hi.Model.BD_ShopImageList item in l)
                    {
                        i++;
                        switch (i)
                        {
                            case 1: this.imgAvatar1.Src = "Goodsimages/" + item.ImageUrl;
                                pic1 = item.ImageUrl;
                                url1 = item.GoodsUrl.ToString();
                                id1 = item.ID;
                                break;
                            case 2: this.imgAvatar2.Src = "Goodsimages/" + item.ImageUrl;
                                pic2 = item.ImageUrl;
                                url2 = item.GoodsUrl.ToString();
                                id2 = item.ID;
                                break;
                            case 3: this.imgAvatar3.Src = "Goodsimages/" + item.ImageUrl;
                                pic3 = item.ImageUrl;
                                url3 = item.GoodsUrl.ToString();
                                id3 = item.ID;
                                break;
                            case 4: this.imgAvatar4.Src = "Goodsimages/" + item.ImageUrl;
                                pic4 = item.ImageUrl;
                                url4 = item.GoodsUrl.ToString();
                                id4 = item.ID;
                                break;
                            case 5: this.imgAvatar5.Src = "Goodsimages/" + item.ImageUrl;
                                pic5 = item.ImageUrl;
                                url5 = item.GoodsUrl.ToString();
                                id5 = item.ID;
                                break;
                        }
                    }
                }
            }
            else
            {
               // JScript.AlertMsgOne(this, "厂商Id有误", JScript.IconOption.错误);
                JScript.AlertMethod(this, "厂商Id有误", JScript.IconOption.错误, "function(){  window.location.href='../../admin/systems/CompList.aspx'; }");
                return;
            }
        }
    }
    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (comPid == 0)
        {
            JScript.AlertMethod(this, "厂商Id有误", JScript.IconOption.错误, "function(){  window.location.href='../../admin/systems/CompList.aspx'; }");
            return;
        }
        SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        try
        {
            new Hi.BLL.BD_ShopImageList().Delete(comPid, Tran);
            object url = Request["txtUrl"];
            object hrImg = Request["hrImgPath"];
            if (url != null)
            {
                for (int i = 0; i < url.ToString().Split(',').Length; i++)
                {
                    Hi.Model.BD_ShopImageList model = new Hi.Model.BD_ShopImageList();
                    model.CompID = comPid;
                    model.Type = 2;
                    model.ImageUrl = hrImg.ToString().Split(',')[i];
                   string goodsUrl = url.ToString().Split(',')[i].Trim();
                    if (goodsUrl == "请输入商品网址")
                    {
                        model.ImageName = "";
                        model.GoodsUrl = "";
                    }
                    else
                    {
                        model.GoodsUrl = url.ToString().Split(',')[i];
                        string id = goodsUrl.Substring(goodsUrl.IndexOf('=') + 1, goodsUrl.LastIndexOf('&') - goodsUrl.IndexOf('=') - 1);
                        string str = goodsUrl.Substring(goodsUrl.IndexOf('?') + 1, goodsUrl.IndexOf('=') - goodsUrl.IndexOf('?') - 1).Trim();//判断是goodsid还是GoodsinfoId
                        if (str.ToLowerInvariant() == "goodsid")
                        {
                            Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(id.ToString()), Tran);
                            if (model2 != null)
                            {
                                model.ImageName = model2.GoodsName;
                            }
                            else
                            {
                                model.ImageName = "";
                            }
                        }
                        else if (str.ToLowerInvariant() == "goodsinfoid")
                        {
                            Hi.Model.BD_GoodsInfo model3 = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id.ToString()), Tran);
                            if (model3 != null)
                            {
                                Hi.Model.BD_Goods model4 = new Hi.BLL.BD_Goods().GetModel(model3.GoodsID, Tran);
                                if (model4 != null)
                                {
                                    model.ImageName = model4.GoodsName;
                                }
                            }
                            else
                            {
                                model.ImageName = "";
                            }
                        }
                    }
                    model.ImageTitle = "";
                    model.GoodsID = 0;// Convert.ToInt32(goodsId.ToString().Split(',')[i]);
                    model.CreateDate = DateTime.Now;
                    model.CreateUserID = this.UserID;
                    model.ts = DateTime.Now;
                    model.modifyuser = this.UserID;
                    model.dr = 0;
                    new Hi.BLL.BD_ShopImageList().Add(model, Tran);
                }
            }
            Tran.Commit();
            Response.Redirect("RecommendGoodsPic2.aspx?KeyID=" + comPid);
        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMsgOne(this, "编辑失败,出错原因" + ex.Message, JScript.IconOption.错误);
            return;
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }

    }
}