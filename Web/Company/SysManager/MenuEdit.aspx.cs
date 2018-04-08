using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class MenuEdit : CompPageBase
{
    Hi.BLL.SYS_SysFun sysFunBLL = new Hi.BLL.SYS_SysFun();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    //绑定一级权限
    public void Bind()
    {
        IList<Hi.Model.SYS_SysFun> sysFuns = sysFunBLL.GetList("", " ParentCode = '' and Type=1", "SortIndex");
        this.RepeaterPMeau.DataSource = sysFuns;
        this.RepeaterPMeau.DataBind();

        if (Request.QueryString["RoleId"] != null)
        {
            int RoleId = Convert.ToInt32(Request.QueryString["RoleId"]);
            Hi.Model.SYS_Role Model = new Hi.BLL.SYS_Role().GetModel(RoleId);
            if (Model != null)
            {
                lblRoleName.Text = Model.RoleName;
                //企业管理员
                if (Model.RoleName.Equals("企业管理员"))
                {
                    b1.InnerText = "企业最高级权限，所有权限都有效";
                }
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
            SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, "delete from SYS_RoleSysFun where RoleID=" + RoleId);

            string MyRole = this.HF_MyRole.Value.Trim();
            try
            {
                if (MyRole != "")
                {
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
                                RSModel.CreateUserID = UserID;
                                RSModel.modifyuser = UserID;
                                RSModel.RoleID = RoleId;
                                RSModel.CompID = CompID;
                                RSModel.FunCode = arr[i].ToString();
                                RSModel.IsEnabled = 1;
                                RSModel.ts = DateTime.Now;

                                count = count + new Hi.BLL.SYS_RoleSysFun().Add(RSModel);
                            }
                        }
                        //if (arr.Length == count)
                        //{
                        if (Request["nextstep"] + "" == "1")
                        {
                            if (Request["type"] == "2")
                            {
                                Response.Redirect("RoleInfo.aspx?nextstep=1&KeyID=" + RoleId);
                            }
                            else
                            {
                                Response.Redirect("RoleList.aspx?nextstep=1");
                            }
                        }
                        else
                        {
                            if (Request["type"] == "2")
                            {
                                Response.Redirect("RoleInfo.aspx?KeyID=" + RoleId);
                            }
                            else
                            {
                                Response.Redirect("RoleList.aspx");
                            }
                        }     
                        //}
                        //else
                        //{
                        //    JScript.AlertAndRedirect("权限分配不完整！", "RoleList.aspx");
                        //}
                    }
                }
                else
                {
                    if (Request["nextstep"] + "" == "1")
                    {
                        if (Request["type"] == "2")
                        {
                            JScript.AlertMethod(this, "数据有误！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleInfo.aspx?nextstep=1&KeyID=" + RoleId) + "'); }");
                        }
                        else
                        {
                            JScript.AlertMethod(this, "数据有误！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleList.aspx?nextstep=1") + "'); }");
                        }
                    }
                    else
                    {
                        if (Request["type"] == "2")
                        {
                            JScript.AlertMethod(this, "数据有误！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleInfo.aspx?KeyID=" + RoleId) + "'); }");
                        }
                        else
                        {
                            JScript.AlertMethod(this, "数据有误！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleList.aspx") + "'); }");
                        }
                    }
                }
            }
            catch
            {
                if (Request["nextstep"] + "" == "1")
                {
                    if (Request["type"] == "1")
                    {
                        JScript.AlertMethod(this, "权限分配失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleList.aspx?nextstep=1") + "'); }");
                    }
                    if (Request["type"] == "2")
                    {
                        JScript.AlertMethod(this, "数据有误！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleInfo.aspx?nextstep=1&KeyID=" + RoleId) + "'); }");
                    }
                }
                else
                {
                    if (Request["type"] == "1")
                    {
                        JScript.AlertMethod(this, "权限分配失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleList.aspx") + "'); }");
                    }
                    if (Request["type"] == "2")
                    {
                        JScript.AlertMethod(this, "权限分配失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleInfo.aspx?KeyID=" + RoleId) + "'); }");
                    }
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
        //我要维护 -- 修改登录密码
        if (NodeID == "1512" || NodeID == "15")
        {
            Role = "checked=\"checked\"";
        }
        if (NodeID == "2513" || NodeID == "25")
        {
            Role = "checked=\"checked\"";
        }

        //企业管理员
        if (lblRoleName.Text.Trim().Equals("企业管理员"))
        {
            Role = "checked=\"checked\"  disabled=\"disabled\" ";
        }
        return Role;
    }
}