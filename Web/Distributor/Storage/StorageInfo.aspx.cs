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

        if (!IsPostBack)
        {
            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());
            Bings();
        }

    }

    public void Bings()
    {
        string StorageID = Request.QueryString["KeyID"];
        this.StorageID.Value = StorageID;
        if (!string.IsNullOrWhiteSpace(StorageID)) {

            Hi.Model.YZT_Storage storageModel = new Hi.BLL.YZT_Storage().GetModel(Convert.ToInt32(StorageID));
            if (storageModel != null) {
                this.ddrComp.Value = storageModel.CompID.ToString();
                txtStorageDate.Value = storageModel.StorageDate.ToString("yyyy-MM-dd");
                lblStorageType1.Value = storageModel.StorageType.ToString();
                this.StorageNO.Value = storageModel.StorageNO;
                OrderNote.Value = storageModel.Remark;
                if (storageModel.IState == 1)
                {
                    btnUpdate.Visible = false;
                    auditBtn.Visible = false;
                }
            }

            List<Hi.Model.YZT_StorageDetail> storageDetailList = new Hi.BLL.YZT_StorageDetail().GetList("", " dr=0 and StorageID=" + StorageID + "", "");
            Rep_StorageDetail.DataSource = storageDetailList;
            Rep_StorageDetail.DataBind();
        }
    }



}