using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Admin_SetGoodsShow : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            DataBindShow();
        }
    }

    public void DataBindShow()
    {
        if (KeyID > 0)
        {
            Hi.Model.BD_Goods good = new Hi.BLL.BD_Goods().GetModel(KeyID);
            //List<Hi.Model.BD_Goods> ListGoods = new Hi.BLL.BD_Goods().GetList("IsFirstShow,Sortindex,NewPic,GoodsName,ShowName,ID", " dr=0 and id=" + KeyID + "  ", "");
            if (good!=null)
            {
                if (good.IsFirstShow)
                {
                    rdShowYes.Checked = true;
                    rdShowNo.Checked = false;
                }
                if (!string.IsNullOrWhiteSpace(good.NewPic))
                {
                    ImgNewPic.Style.Remove("display");
                    ImgNewPic.Src = ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + good.NewPic;
                    HdNewPicName.Value = good.NewPic;
                }
                lblGoodsName.InnerText = good.GoodsName;
                txt_SortIndex.Value = good.Sortindex.ToString();
                txt_ShowName.Value = good.ShowName;
            }
            else
            {
                if (Request.UrlReferrer != null)
                {
                    JScript.AlertMsgMo(this, "商品数据有误", "function (){ window.parent.layer.close(window.parent.LayserIndex); }");
                    return;
                }
                else
                {
                    Response.Write("商品数据有误。");
                    Response.End();
                }
            }
        }
        else
        {
            if (Request.UrlReferrer != null)
            {
                JScript.AlertMsgMo(this, "商品数据有误", "function (){ window.parent.layer.close(window.parent.LayserIndex); }");
                return;
            }
            else
            {
                Response.Write("商品数据有误。");
                Response.End();
            }
        }
    }

    protected void btnSubMit_Click(object sender, EventArgs e)
    {
        Hi.Model.BD_Goods good = new Hi.BLL.BD_Goods().GetModel(KeyID);
        //List<Hi.Model.BD_Goods> ListGoods = new Hi.BLL.BD_Goods().GetList("*", " dr=0 and id=" + KeyID + "  ", "");
        if (good != null)
        {
            good.Sortindex = txt_SortIndex.Value.ToInt(0);
            good.IsFirstShow = rdShowYes.Checked;
            good.NewPic = Common.NoHTML(HdNewPicName.Value);
            good.ShowName = Common.NoHTML(txt_ShowName.Value.Trim());
            new Hi.BLL.BD_Goods().Update(good);
            ClientScript.RegisterStartupScript(this.GetType(), "Msg", "<script> if(window.parent.Reload){  window.parent.Reload(); }else { window.parent.layer.close(window.parent.LayserIndex)};  </script>");
        }
    }
}