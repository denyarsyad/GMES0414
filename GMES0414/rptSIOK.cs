using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
using DevExpress.Data.Helpers;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;


namespace CSI.MES.P
{
    public partial class rptSIOK : DevExpress.XtraReports.UI.XtraReport
    {
        public rptSIOK()
        {
            InitializeComponent();

            PaperKind = PaperKind.A5;
            Landscape = false;
        }

        public void BindData(DataTable dtData)
        {
            try
            {
                string userId = dtData.Rows[0]["USER_ID"].ToString() != "" ? dtData.Rows[0]["USER_ID"].ToString() : "-";
                string regDt = dtData.Rows[0]["RENTAL_DATE"].ToString() != "" ? dtData.Rows[0]["RENTAL_DATE"].ToString() : "-";
                string startDt = dtData.Rows[0]["START_DATE"].ToString() != "" ? dtData.Rows[0]["START_DATE"].ToString() : "-";
                string time = dtData.Rows[0]["START_TIME"].ToString() != "" ? dtData.Rows[0]["START_TIME"].ToString() : "-";
                string dest = dtData.Rows[0]["DESTINATION"].ToString() != "" ? dtData.Rows[0]["DESTINATION"].ToString() : "-";
                string purp = dtData.Rows[0]["PURPOSES"].ToString() != "" ? dtData.Rows[0]["PURPOSES"].ToString() : "-";
                string psger = dtData.Rows[0]["PASSANGERS"].ToString() != "" ? dtData.Rows[0]["PASSANGERS"].ToString() : "-";

                tblUserId.Text = userId;
                tblRegDt.Text = regDt;
                tblStartDt.Text = startDt;
                tblTime.Text = time;
                tblDestination.Text = dest;
                chkOfficial.CheckState = purp == "Y" ? System.Windows.Forms.CheckState.Checked : System.Windows.Forms.CheckState.Unchecked;
                chkUnofficial.CheckState = purp == "N" ? System.Windows.Forms.CheckState.Checked : System.Windows.Forms.CheckState.Unchecked;
                tblPassanger.Text = psger;

            }
            catch (Exception ex)
            {

            }
        }
    }
}
