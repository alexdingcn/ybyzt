using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using System.Data;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Text;

public partial class Company_CMerchants_FirstCampInfo : CompPageBase
{
    public string DisArea = string.Empty;//代理商区域数据源
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        bind();
    }

    public string Where()
    {
        string strWhere = string.Empty;

        if (this.txtComName.Value.Trim() != "")
        {
            strWhere += " and dis.DisName like '%" + this.txtComName.Value.Trim() + "%'";
        }
        if (this.txtContractNO.Value.Trim() != "")
        {
            strWhere += " and ht.HospitalName like '%" + this.txtContractNO.Value.Trim() + "%'";
        }
        return strWhere;
    }

     /// <summary>
    /// 招商信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.YZT_CMerchants cmModel = new Hi.BLL.YZT_CMerchants().GetModel(this.KeyID);

            this.txtCMCode.Value = cmModel.CMCode;
            this.txtCMName.Value = cmModel.CMName;
            this.txtGoodsCode.Value = Common.GetGoodsName(cmModel.GoodsID.ToString(), "GoodsCode");
            this.txtGoodsName.Value = Common.GetGoodsName(cmModel.GoodsID.ToString(), "GoodsName");
            this.txtRemark.Value = cmModel.Remark;
            this.txtValueInfo.Value = Common.GetGoodsInfo(cmModel.GoodsID.ToString());
            this.txtInvalidDate.Value = cmModel.InvalidDate == DateTime.MinValue ? "" : cmModel.InvalidDate.ToString("yyyy-MM-dd");
            this.txtForceDate.Value = cmModel.ForceDate == DateTime.MinValue ? "" : cmModel.ForceDate.ToString("yyyy-MM-dd");
            this.txtCategoryID.Value = Common.GetCategoryName(Common.GetGoodsName(cmModel.GoodsID.ToString(), "CategoryID"));
            this.ddrtype.Value = cmModel.Type.ToString();

            if (cmModel.ProvideData.IndexOf("1") > -1)
            {
                this.chk1.Checked = true;
                this.lblchk1.Visible = true;
            }
            if (cmModel.ProvideData.IndexOf("2") > -1)
            {
                this.lblchk2.Visible = true;
                this.chk2.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("3") > -1)
            {
                this.lblchk3.Visible = true;
                this.chk3.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("4") > -1)
            {
                this.lblchk4.Visible = true;
                this.chk4.Checked = true;
            }

            string strwhere = "";
            if (ViewState["strwhere"] != null) {
                strwhere = ViewState["strwhere"].ToString();
            }

            string sql = @"select fc.*,dis.DisName,dis.Principal,dis.Phone,ht.HospitalCode,ht.HospitalName from YZT_FirstCamp fc left join BD_Distributor dis on fc.DisID=dis.ID left join SYS_Hospital ht on fc.HtID=ht.ID where fc.dr=0 and dis.DisName<>'' and fc.CMID=" + this.KeyID + strwhere + " order by State";
            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                this.rptFc.DataSource = dt;
                this.rptFc.DataBind();
            }
            else {
                this.rptFc.DataSource = null;
                this.rptFc.DataBind();
            }


            //绑定区域
            StringBuilder sbare = new StringBuilder();
            List<Hi.Model.BD_DisArea> are = new Hi.BLL.BD_DisArea().GetList("top 12 * ", "isnull(dr,0)=0 and  ParentId=0  and CompanyID=" + this.CompID, " SortIndex");
            if (are.Count > 0)
            {
                sbare.Append("[");
                int num = 0;
                foreach (var model in are)
                {
                    num++;
                    sbare.Append("{code:'" + model.ID + "',value: '" + model.ID + "',label: '" + model.AreaName + "'");
                    List<Hi.Model.BD_DisArea> aret1 = new Hi.BLL.BD_DisArea().GetList("Areacode,ID,AreaName", "isnull(dr,0)=0  and ParentId=" + model.ID, "");
                    if (aret1.Count > 0)
                    {
                        sbare.Append(",children: [");
                        int num2 = 0;
                        foreach (var model2 in aret1)
                        {
                            num2++;
                            sbare.Append("{code:'" + model2.ID + "',value: '" + model2.ID + "',label: '" + model2.AreaName + "'");
                            List<Hi.Model.BD_DisArea> are3 = new Hi.BLL.BD_DisArea().GetList("Areacode,ID,AreaName", "isnull(dr,0)=0  and ParentId=" + model2.ID, "");
                            if (are3.Count > 0)
                            {
                                sbare.Append(",children: [");
                                int num3 = 0;
                                foreach (var item3 in are3)
                                {
                                    num3++;
                                    if (num3 == are3.Count)
                                        sbare.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.AreaName + "'}");
                                    else
                                        sbare.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.AreaName + "'},");

                                }
                                sbare.Append("]");
                            }

                            if (num2 == aret1.Count)
                                sbare.Append("}");
                            else
                                sbare.Append("},");
                        }
                        sbare.Append("]");

                    }
                    if (num == are.Count)
                        sbare.Append("}");
                    else
                        sbare.Append("},");
                }
                sbare.Append("]");
                DisArea = sbare.ToString();
            }
        }
    }

    /// <summary>
    /// 授权书
    /// </summary>
    /// <param name="fcID"></param>
    /// <returns></returns>
    public string Queryannex(string fcID)
    {
        List<Hi.Model.YZT_Annex> annlist = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + fcID + " and type=2 and fileAlias='2' ", "");
        string str = "";
        if (annlist != null && annlist.Count > 0) {
            str = "<a style=\"cursor: pointer;\" class=\"bt\" title=\"" + annlist[0].fileName + "\" href=\"../../UploadFile/" + annlist[0].fileName + "\" target=\"_blank\">" + Common.MySubstring(annlist[0].fileName,20,"...") + "</a>";
        }
        return str;
    }

    [WebMethod]
    public static string QrFC(string cmID, string compID, string htid)
    {
        Common.ResultMessage Msg = new Common.ResultMessage();
        string sql = "select * from YZT_FirstCamp fc where isnull(fc.dr,0)=0 and fc.State=2 and fc.CMID=" + cmID + " and HtID=" + htid + " and fc.CompID=" + compID;

        DataTable htDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (htDt != null && htDt.Rows.Count <= 0)
        {
            Msg.result = true;
        }
        else {
            Msg.code = "此医院已存在通过的代理商，是否确定处理！";
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }
}