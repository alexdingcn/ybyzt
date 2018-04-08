

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Distributor_MenuEdit : DisPageBase
{
    //public Hi.Model.SYS_Users user = null;
    Hi.BLL.SYS_SysFun sysFunBLL = new Hi.BLL.SYS_SysFun();
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        if (!IsPostBack)
        {
            Bind();
        }
    }

    //绑定一级权限
    public void Bind()
    {
        IList<Hi.Model.SYS_SysFun> sysFuns = sysFunBLL.GetList(""," ParentCode = '' and Type=2","SortIndex");
        this.RepeaterPMeau.DataSource = sysFuns;
        this.RepeaterPMeau.DataBind();

        if (Request.QueryString["RoleId"] != null)
        {
            int RoleId = Convert.ToInt32(Request.QueryString["RoleId"]);
            Hi.Model.SYS_Role Model = new Hi.BLL.SYS_Role().GetModel(RoleId);
            if (Model != null)
            {
                lblRoleName.Text = Model.RoleName;
            }
        }
    }
    /// <summary>
    /// 绑定二级权限
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RepeaterPMeau_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //找到分类Repeater关联的数据项 
            Hi.Model.SYS_SysFun rowv = (Hi.Model.SYS_SysFun)e.Item.DataItem;
            //提取分类ID 
            string NodeId = rowv.FunCode;
            if (NodeId.ToString() != "")
            {
                Repeater rp = (Repeater)e.Item.FindControl("RepeaterItemMeau");
                Hi.BLL.SYS_SysFun SysFunBLL = new Hi.BLL.SYS_SysFun();
                List<Hi.Model.SYS_SysFun> list = SysFunBLL.GetSysFunByParentNodeId(NodeId) as List<Hi.Model.SYS_SysFun>;
                if (list.Count > 0)
                {
                    rp.DataSource = list;
                    rp.DataBind();
                }
            }
        }
    }
    /// <summary>
    /// 绑定三级权限
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RepeaterMeau_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            //找到分类Repeater关联的数据项 
            Hi.Model.SYS_SysFun rowv = (Hi.Model.SYS_SysFun)e.Item.DataItem;
            //提取分类ID 
            string NodeId = rowv.FunCode;
            if (NodeId.ToString() != "")
            {
                //Repeater rp = (Repeater)e.Item.FindControl("RepeaterItem");
                //Hi.BLL.SYS_SysFun SysFunBLL = new Hi.BLL.SYS_SysFun();
                //List<Hi.Model.SYS_SysFun> list = SysFunBLL.GetSysFunByParentNodeId(NodeId) as List<Hi.Model.SYS_SysFun>;
                //if (list.Count > 0)
                //{
                //    rp.DataSource = list;
                //    rp.DataBind();
                //}
            }
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["RoleId"] != null)//删除原有模块
        {
            int RoleId = Convert.ToInt32(Request.QueryString["RoleId"]);
            

            string MyRole = this.HF_MyRole.Value.Trim();
            try
            {
                if (MyRole != "")
                {
                    SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, "delete from SYS_RoleSysFun where RoleID=" + RoleId);
                    int count = 0;//统计
                    string[] arr = MyRole.Split(',');
                    if (arr.Length > 0)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (arr[i] != "")
                            {
                                Hi.Model.SYS_RoleSysFun RSModel = new Hi.Model.SYS_RoleSysFun();
                                RSModel.CreateDate = DateTime.Now;
                                RSModel.CreateUserID = this.UserID;
                                RSModel.modifyuser = this.UserID;
                                RSModel.RoleID = RoleId;
                                RSModel.CompID = this.CompID;
                                RSModel.DisID = this.DisID;
                                RSModel.FunCode = arr[i].ToString();
                                RSModel.IsEnabled = 1;
                                RSModel.ts = DateTime.Now;

                                count = count + new Hi.BLL.SYS_RoleSysFun().Add(RSModel);
                            }
                        }
                        if (Request["type"] == "2")
                        {
                            Response.Redirect("RoleInfo.aspx?KeyID=" + Common.DesEncrypt(RoleId.ToString(), Common.EncryptKey));
                        }
                        else
                        {
                            Response.Redirect("RoleList.aspx");
                        }
                    }
                }
                else
                {
                    if (Request["type"] == "2")
                    {
                        JScript.AlertMethod(this, "最少保留一种权限！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleInfo.aspx?KeyID=" + Common.DesEncrypt(RoleId.ToString(), Common.EncryptKey)) + "'); }");
                    }
                    else
                    {
                        JScript.AlertMethod(this, "最少保留一种权限！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleList.aspx") + "'); }");
                    }
                }
            }
            catch
            {
                if (Request["type"] == "1")
                {
                    JScript.AlertMethod(this, "权限分配失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleList.aspx") + "'); }");
                }
                if (Request["type"] == "2")
                {
                    JScript.AlertMethod(this, "权限分配失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleInfo.aspx?KeyID=" + Common.DesEncrypt(RoleId.ToString(), Common.EncryptKey)) + "'); }");
                }
            }
        }
    }
    /// <summary>
    /// 通过id获得父级id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public string GetParentIdById(string Id)
    {
        string Result = "";
        if (!string.IsNullOrEmpty(Id))
        {
            Hi.Model.SYS_SysFun Model = new Hi.BLL.SYS_SysFun().GetModel(Convert.ToInt32(Id));
            if (Model != null)
            {
                Result = Model.FunCode;
            }
        }
        return Result;
    }
    /// <summary>
    /// 初始加载是否绑定
    /// </summary>
    /// <param name="NodeID"></param>
    /// <returns></returns>
    public string BindRoleSysFun(string NodeID)
    {
        string Role = string.Empty;
        if (Request.QueryString["RoleId"] != null)
        {
            int RoleId = Convert.ToInt32(Request.QueryString["RoleId"]);
            List<Hi.Model.SYS_RoleSysFun> List = new Hi.BLL.SYS_RoleSysFun().GetList(null, " Roleid=" + RoleId, null);
            foreach (Hi.Model.SYS_RoleSysFun Model in List)
            {
                if (NodeID.Trim().Equals(Model.FunCode.ToString().Trim()))
                {
                    Role = "checked=\"checked\"";
                    break;
                }
            }
        }
        return Role;
    }
}