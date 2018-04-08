using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_Storage_StorageAdd : DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        object obj = Request["action"];
        if (obj != null)
        {
            //选中的商品
            if (obj.ToString() == "goodsInfo")
            {
                string OrderOutDetailID = Request["OrderOutDetailID"] + "";
                Response.Write(disBings(OrderOutDetailID));
                Response.End();
            }
        }

        if (!IsPostBack)
        {
            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());
            Bind();
        }

    }

    public string disBings(string OrderOutDetailID = "")
    {

        StringBuilder strwhere = new StringBuilder();
        if (!Util.IsEmpty(OrderOutDetailID))
        {
            strwhere.AppendFormat(" and d.id in(" + OrderOutDetailID + ")");
        }
        DataTable dt = new Hi.BLL.BD_GoodsInfo().getDisGoodsStock(strwhere.ToString()).Tables[0];
        return ConvertJson.ToJson(dt);
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string StorageID = Request.QueryString["KeyID"];
        this.StorageID.Value = StorageID;
        if (!string.IsNullOrWhiteSpace(StorageID))
        {

            Hi.Model.YZT_Storage storageModel = new Hi.BLL.YZT_Storage().GetModel(Convert.ToInt32(StorageID));
            if (storageModel != null)
            {
                this.ddrComp.Value = storageModel.CompID.ToString();
                txtStorageDate.Value = storageModel.StorageDate.ToString("yyyy-MM-dd");
                lblStorageType1.Value = storageModel.StorageType.ToString() ;
                this.StorageNO.Value = storageModel.StorageNO;
                OrderNote.Value = storageModel.Remark;
            }

            List<Hi.Model.YZT_StorageDetail> storageDetailList = new Hi.BLL.YZT_StorageDetail().GetList("", " dr=0 and StorageID=" + StorageID + "", "");
            Rep_StorageDetail.DataSource = storageDetailList;
            Rep_StorageDetail.DataBind();
            if (storageDetailList.Count > 0) oneTR.Visible = false;
        }


    }

}