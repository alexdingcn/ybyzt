using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_GoodsTypeInfo : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        if (KeyID != 0)
        {
            Hi.Model.SYS_GoodsType goodstype = new Hi.BLL.SYS_GoodsType().GetModel(KeyID);

            lblname.InnerText = goodstype.GoodsTypeName;
            lblsort.InnerText = goodstype.SortIndex;
            lblstate.InnerText = goodstype.IsEnabled == 1 ? "启用" : "禁用";
        }
        else
        {
            Response.Write("商品分类不存在。");
            Response.End();
        }
    }

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.SYS_GoodsType goodstype = new Hi.BLL.SYS_GoodsType().GetModel(KeyID);
        if (goodstype != null)
        {
            goodstype.dr = 1;
            goodstype.ts = DateTime.Now;
            goodstype.modifyuser = UserID;
            if (new Hi.BLL.SYS_GoodsType().Update(goodstype))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='GoodsTypeList.aspx'; }");
                Response.Redirect("GoodsTypeList.aspx");
            }
        }
    }
}