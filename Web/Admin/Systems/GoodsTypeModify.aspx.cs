using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_GoodsTypeModify : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Databind();
        }
    }

    public void Databind()
    {
        if (KeyID != 0)
        {
            Hi.Model.SYS_GoodsType goodstype = new Hi.BLL.SYS_GoodsType().GetModel(KeyID);
            try
            {
                txtGoodsName.Value = goodstype.GoodsTypeName;
                txtSort.Value = goodstype.SortIndex;
                int status = goodstype.IsEnabled;
                this.rdoStatus1.Checked = (status != 1);
                this.rdoStatus0.Checked = (status == 1);
            }
            catch (Exception ex)
            {

            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_GoodsType goodstype = null;

        if (string.IsNullOrEmpty(txtGoodsName.Value.Trim()))
        {
            JScript.AlertMsg(this, "商品分类名称不能为空!");
            return;
        }
        if (KeyID != 0)
        {
            goodstype = new Hi.BLL.SYS_GoodsType().GetModel(KeyID);
            goodstype.GoodsTypeName = Common.NoHTML(txtGoodsName.Value.Trim());
            goodstype.SortIndex = Common.NoHTML(txtSort.Value.Trim());
            if (this.rdoStatus1.Checked)
                goodstype.IsEnabled = 0;
            else
                goodstype.IsEnabled = 1;

            goodstype.GoodsTypeCode="0";
            goodstype.ts = DateTime.Now;
            goodstype.modifyuser = UserID;

            if (new Hi.BLL.SYS_GoodsType().Update(goodstype))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='GoodsTypeInfo.aspx?KeyID=" + KeyID + "'; }");
                Response.Redirect("GoodsTypeInfo.aspx?KeyID=" + KeyID);
            }
        }
        else
        {
            goodstype = new Hi.Model.SYS_GoodsType();
            goodstype.GoodsTypeName = Common.NoHTML(txtGoodsName.Value.Trim());

            goodstype.SortIndex = Common.NoHTML(txtSort.Value.Trim());
            if (this.rdoStatus1.Checked)
                goodstype.IsEnabled = 0;
            else
                goodstype.IsEnabled = 1;

            //标准参数
            goodstype.CreateDate = DateTime.Now;
            goodstype.CreateUserID = UserID;
            goodstype.ts = DateTime.Now;
            goodstype.modifyuser = UserID;
            int newuserid = 0;
            newuserid = new Hi.BLL.SYS_GoodsType().Add(goodstype);
            if (newuserid > 0)
            {
                Response.Redirect("GoodsTypeInfo.aspx?KeyID=" + newuserid);
            }
        }
    }

}