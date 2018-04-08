using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SysManager_AttrManager : CompPageBase
{
    Hi.BLL.BD_DefDoc AttrNameBLL = new Hi.BLL.BD_DefDoc();
    Hi.BLL.BD_DefDoc_B AttrValBLL = new Hi.BLL.BD_DefDoc_B();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();

            this.val.Visible = false;
            this.Sorh.Visible = false;
            // this.table.Visible = false;
        }
    }

    /// <summary>
    /// 加载字典名
    /// </summary>
    protected void Bind()
    {
        string strWhere = string.Empty;
        List<Hi.Model.BD_DefDoc> l = AttrNameBLL.GetList("", " CompID='" + this.CompID + "' ", "Id Asc");

        this.DgAttrName.DataSource = l;
        this.DgAttrName.DataBind();
    }

    /// <summary>
    /// 加载字典项
    /// </summary>
    /// <param name="AttrNameId"></param>
    /// <param name="newIndex"></param>
    protected void AttrValBind(string AttrNameId, int newIndex)
    {
        string strWhere = string.Empty;
        string Sort = string.Empty;
        this.lblMsg.Visible = false;

        if (AttrNameId != "")
        {
            strWhere = " DefID='" + AttrNameId + "'";
        }
        if (newIndex < 0)
        {
            Sort = "SortIndex desc";
        }
        else
        {
            Sort = "SortIndex asc";
        }

        List<Hi.Model.BD_DefDoc_B> l = AttrValBLL.GetList("", strWhere, Sort);

        this.dgAttrVal.DataSource = l;
        this.dgAttrVal.DataBind();


    }

    /// <summary>
    /// 字典项选中事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dgAttrVal_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            string AttrValId = e.Item.Cells[0].Text; //获取鼠标点击的字典项Id
            switch (e.CommandName)
            {
                case "Modify":
                    this.dgAttrVal.SelectedIndex = e.Item.ItemIndex;
                    updateAttrVal(AttrValId);
                    break;
                default:
                    break;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// 字典名选中事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DgAttrName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string attrNameId = this.DgAttrName.Items[this.DgAttrName.SelectedIndex].Cells[0].Text;  //获取鼠标点击的字典名Id
        ViewState["attrNameId"] = attrNameId;
        this.lblAttrValMsg.Text = "";
        AttrValBind(attrNameId, 1);
        Clear();
    }

    /// <summary>
    /// 修改字典项信息
    /// </summary>
    /// <param name="AttrValId"></param>
    protected void updateAttrVal(string AttrValId)
    {
        if (AttrValId != "")
        {
            Hi.Model.BD_DefDoc_B attrValModel = AttrValBLL.GetModel(Convert.ToInt32(AttrValId));
            if (attrValModel != null)
            {
                ViewState["attrValId"] = AttrValId;

                this.txtAtVal.Text = attrValModel.AtVal;
                this.txtSortIndex.Text = attrValModel.SortIndex.ToString();

                //项目扩展
                this.btnUp.Visible = false;
                this.btnDown.Visible = false;
                this.btnAddNew.Visible = false;

                this.btnSave.Visible = true;
                this.btnDelete.Visible = true;

                //this.table.Visible = true;
                this.val.Visible = true;
                this.Sorh.Visible = true;
                this.txtAtVal.Visible = true;
                this.txtSortIndex.Visible = true;


                this.lblMsg.Visible = false;
                this.lblAttrValMsg.Text = "";
            }
            else
            {
                JScript.AlertMsgOne(this, "数据不存在！", JScript.IconOption.错误,2500);
            }
        }
    }

    #region 页面事件

    private void Clear()
    {
        this.btnAddNew.Visible = true;
        this.btnUp.Visible = true;
        this.btnDown.Visible = true;

        this.btnDelete.Visible = false;
        this.btnSave.Visible = false;

        //this.table.Visible = true;
        this.val.Visible = true;
        this.Sorh.Visible = true;

        this.txtAtVal.Visible = true;
        this.txtAtVal.Text = "";
        this.txtSortIndex.Visible = true;
        this.txtSortIndex.Text = "";

        this.lblAttrValMsg.Visible = false;
        ViewState["attrValId"] = "";

    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        string attrNameId = ViewState["attrNameId"].ToString();
        string attrValId = this.dgAttrVal.SelectedItem.Cells[0].Text;

        if (attrValId != "")
        {
            Hi.Model.BD_DefDoc_B attrValModel = AttrValBLL.GetModel(Convert.ToInt16(attrValId));

            if (attrValModel != null)
            {
                bool falg = AttrValBLL.Delete(Convert.ToInt16(attrValId));
                if (falg)
                {
                    JScript.AlertMsgOne(this, "数据删除成功！", JScript.IconOption.错误, 2500);
                    AttrValBind(attrNameId, 1);
                    Clear();

                    //日志
                    //Hi.Model.A_AdminUser UModel = Session["AdminUser"] as Hi.Model.A_AdminUser;
                    //Utils.EditLog("用户" + this.UserName + "删除属性值成功", ID, "属性名称模块", "SysManager/AttrManager.aspx", 1);
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "数据不存在！", JScript.IconOption.错误, 2500);
            }
        }
    }

    /// <summary>
    /// 向上排序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUp_Click(object sender, System.Web.UI.ImageClickEventArgs e) 
    {
        string attrNameId = ViewState["attrNameId"].ToString();

        if (attrNameId != "")
        {
            AttrValBind(attrNameId, 1);
        }
    }

    /// <summary>
    /// 向下排序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDown_Click(object sender, System.Web.UI.ImageClickEventArgs e) 
    {
        string attrNameId = ViewState["attrNameId"].ToString();

        if (attrNameId != "")
        {
            AttrValBind(attrNameId, -1);
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        string attrVal = string.Empty;   //属性值
        string SortIndex = string.Empty;   //排序
        string str = string.Empty;


        string attrNameId = ViewState["attrNameId"].ToString();  //属性名称Id

        #region 获取页面的值
        if (this.txtAtVal.Text != "")
        {
            attrVal = this.txtAtVal.Text.Trim();
        }
        if (this.txtSortIndex.Text != "")
        {
            SortIndex = this.txtSortIndex.Text.Trim();
        }
        #endregion

        #region  判断
        if (attrVal == "")
        {
            str += "- 属性值不能为空。\\r\\n";
        }
        //if (SortIndex == "")
        //{
        //    str += "- 排序不能为空。\\r\\n";
        //}
        #endregion

        if (str != "")
        {
            JScript.AlertMsgOne(this, str, JScript.IconOption.错误, 2500);
            return;
        }

        Hi.Model.BD_DefDoc AttrNameModel = AttrNameBLL.GetModel(Convert.ToInt16(attrNameId));
        if (AttrNameModel != null)
        {
            Hi.Model.BD_DefDoc_B attrValModel = new Hi.Model.BD_DefDoc_B();
            attrValModel.DefID = Convert.ToInt16(attrNameId);
            attrValModel.CompID = this.CompID;
            attrValModel.AtName = AttrNameModel.AtName;
            attrValModel.AtVal = attrVal;
            attrValModel.SortIndex = SortIndex;
            attrValModel.ts = DateTime.Now;
            attrValModel.modifyuser = this.UserID;

            int count = AttrValBLL.Add(attrValModel);
            if (count > 0)
            {
                JScript.AlertMsgOne(this, "数据新增成功！", JScript.IconOption.笑脸);
                AttrValBind(attrNameId, 1);
                Clear();

                //日志
                //Hi.Model.A_AdminUser UModel = Session["AdminUser"] as Hi.Model.A_AdminUser;
                //Utils.EditLog("用户" + this.UserName + "新增属性值成功", ID, "属性名称模块", "SysManager/AttrManager.aspx", 1);
            }
            else
            {
                JScript.AlertMsgOne(this, "数据新增失败", JScript.IconOption.错误, 2500);
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "属性名称不存在", JScript.IconOption.错误, 2500);
        }

    }

    /// <summary>
    /// 保存按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        string attrVal = string.Empty;   //属性值
        string SortIndex = string.Empty;   //排序
        string str = string.Empty;

        string attrNameId = ViewState["attrNameId"].ToString();  //属性名称Id
        string attrValId = ViewState["attrValId"].ToString();    //属性值Id

        #region 获取页面的值
        if (this.txtAtVal.Text != "")
        {
            attrVal = this.txtAtVal.Text.Trim();
        }
        if (this.txtSortIndex.Text != "")
        {
            SortIndex = this.txtSortIndex.Text.Trim();
        }
        #endregion

        #region  判断
        if (attrVal == "")
        {
            str += "- 属性值不能为空。\\r\\n";
        }
        //if (SortIndex == "")
        //{
        //    str += "- 排序不能为空。\\r\\n";
        //}
        #endregion

        if (str != "")
        {
            JScript.AlertMsgOne(this, str, JScript.IconOption.错误, 2500);
            return;
        }

        Hi.Model.BD_DefDoc_B attrValModel = new Hi.Model.BD_DefDoc_B();
        Hi.Model.BD_DefDoc AttrNameModel = AttrNameBLL.GetModel(Convert.ToInt16(attrNameId));
        if (AttrNameModel != null)
        {
            attrValModel.ID = Convert.ToInt16(attrValId);
            attrValModel.DefID = Convert.ToInt16(attrNameId);
            attrValModel.AtName = AttrNameModel.AtName;
            attrValModel.AtVal = attrVal;
            attrValModel.SortIndex = SortIndex;

            bool falg = AttrValBLL.Update(attrValModel);
            if (falg)
            {
                JScript.AlertMsgOne(this, "数据修改成功！", JScript.IconOption.笑脸);
                AttrValBind(attrNameId, 1);
                Clear();

                //日志
                // Hi.Model.A_AdminUser UModel = Session["AdminUser"] as Hi.Model.A_AdminUser;
                //Utils.EditLog("用户" + this.UserName + "修改属性值成功", ID, "属性名称模块", "SysManager/AttrManager.aspx", 1);
            }
            else
            {
                JScript.AlertMsgOne(this, "数据修改失败", JScript.IconOption.错误, 2500);
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "属性名称不存在", JScript.IconOption.错误, 2500);
        }

    }

    #endregion
}