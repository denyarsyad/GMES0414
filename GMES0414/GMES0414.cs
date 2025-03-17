using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JPlatform.Client.JERPBaseForm6;
using JPlatform.Client.Library6.interFace;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Diagnostics;
using JPlatform.Client.Controls6;
using DevExpress.XtraGrid;
using JPlatform.Client.CSIGMESBaseform6;
using System.Net;
using System.Reflection;
using System.IO;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using CSI.MES.P.DAO;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Net.Sockets;
using System.Management;
using DevExpress.Export;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing.Drawing2D;
using System.Net.Mail;
using System.Globalization;
using DevExpress.XtraReports.UI;

namespace CSI.MES.P
{
    public partial class GMES0414 : CSIGMESBaseform6
    {
        public GMES0414()
        {
            InitializeComponent();
        }

        DataTable dtItem = new DataTable();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            NewButton = false;
            DeleteButton = true;
            PreviewButton = true;
            PrintButton = false;
            AddButton = false;
            DeleteRowButton = false;
            SaveButton = false;

            dtEFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            dtETo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            fnGetCbo("GET_STATUS");
            txtInterval.Text = "5";
            chkAutoRf.CheckState = CheckState.Checked;
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
            InitControls(grdMain);

            dtYear.EditValue = DateTime.Now.ToString("yyyy");
            dtYear.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;
            dtYear.Properties.DisplayFormat.FormatString = "yyyy";
            dtYear.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtYear.Properties.EditFormat.FormatString = "yyyy";
            dtYear.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtYear.Properties.Mask.EditMask = "yyyy";
            dtYear.Properties.ShowToday = false;
            dtYear.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;

            #region [DESIGN]
            lblDate.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblTo.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblStatus.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblMinutes.Font = new Font("Calibri", 12, FontStyle.Bold);

            dtEFrom.Font = new Font("Calibri", 12, FontStyle.Bold);
            dtETo.Font = new Font("Calibri", 12, FontStyle.Bold);
            cboStatus.Font = new Font("Calibri", 12, FontStyle.Bold);
            txtInterval.Font = new Font("Calibri", 12, FontStyle.Bold);
            chkAutoRf.Font = new Font("Calibri", 12, FontStyle.Bold);
            dtYear.Font = new Font("Calibri", 12, FontStyle.Bold);
            btnGenerate.Font = new Font("Calibri", 12, FontStyle.Bold);

            gvwMain.OptionsView.ShowGroupPanel = false;
            gvwMain.ColumnPanelRowHeight = 40;

            for (int i = 0; i < gvwMain.Columns.Count; i++)
            {
                gvwMain.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gvwMain.Columns[i].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            }

            pctRed.BackColor = ColorTranslator.FromHtml("#F47174");
            pctRed.Image = new Bitmap(1, 1);
            pctYellow.BackColor = ColorTranslator.FromHtml("#FFFDD0");
            pctYellow.Image = new Bitmap(1, 1);
            pctGrey.BackColor = ColorTranslator.FromHtml("#D1CFC8");
            pctGrey.Image = new Bitmap(1, 1);
            pctGreen.BackColor = ColorTranslator.FromHtml("#EAFFDE");
            pctGreen.Image = new Bitmap(1, 1);

            lblRed.Font = new Font("Calibri", 10, FontStyle.Regular);
            lblRed.Text = "Cancelled"; 
            lblYellow.Font = new Font("Calibri", 10, FontStyle.Regular);
            lblYellow.Text = "Registered/Belum Input GA";
            lblGrey.Font = new Font("Calibri", 10, FontStyle.Regular);
            lblGrey.Text = "Waktu Keberangkatan Sudah Kadaluwarsa";
            lblGreen.Font = new Font("Calibri", 10, FontStyle.Regular);
            lblGreen.Text = "Finished/GA Sudah Input";
            #endregion

            fnGetCboPop("GET_CBO_DEPT");

        }

        public override void QueryClick()
        {
            base.QueryClick();

            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                InitControls(grdMain);
                fnSearch("GET_DATA", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
            }
            else
            {
                string cekYear = dtYear.EditValue.ToString().Length > 4 ? dtYear.DateTime.ToString("yyyy") : dtYear.EditValue.ToString();
                InitControls(grdCarStock);
                fnSearchCarStock("GET_CAR_STOCK", cekYear, "", "");
            }
        }

        public override void SaveClick()
        {
            base.SaveClick();

            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Save Data", IconType.Warning) == DialogResult.Yes)
                {
                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        gvwMain.PostEditor();
                        gvwMain.UpdateCurrentRow();

                        int cntSucced = 0;
                        int cntError = 0;
                        int cntError2 = 0;

                        DataTable dt = grdMain.DataSource as DataTable;
                        if (dt != null)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                string ck = row.RowState.ToString();
                                if (row.RowState == DataRowState.Modified)
                                {
                                    string rentalNo = row["RENT_ID"].ToString();
                                    string driverId = row["DRIVER_ID"].ToString();
                                    string detailCar = row["DETAIL_CAR"].ToString();
                                    string seq = row["SEQ"].ToString();
                                    string status = row["STATUS"].ToString();
                                    string startDate = row["START_DATE"].ToString();
                                    DateTime departure = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                                    if (departure >= DateTime.Now)
                                    {
                                        if (rentalNo != "" && driverId != "" && detailCar != "" && status != "C")
                                        {
                                            if (status == "R")
                                            {
                                                fnUpdate(rentalNo, driverId, detailCar, seq);
                                            }
                                            else
                                            {
                                                fnEdit(rentalNo, driverId, detailCar, seq);
                                            }
                                            cntSucced++;
                                        }
                                        else if (status == "C")
                                        {
                                            cntError++;
                                        }
                                        else
                                        {
                                            cntError++;
                                        }
                                    }
                                    else
                                    {
                                        cntError2++;
                                    }
                                }
                            }

                            dt.AcceptChanges();
                        }

                        //QueryClick();
                        InitControls(grdMain);
                        dtEFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                        dtETo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                        fnSearch("GET_DATA", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());

                        if (cntSucced > 0 && cntError == 0)
                        {
                            MessageBoxW("Update Succeed: " + cntSucced);
                        }
                        else if (cntError2 > 0)
                        {
                            MessageBoxW("Departure has expired: " + cntError2);
                        }
                        else
                        {
                            MessageBoxW("Update Succeed: " + cntSucced + " & Unsucceed: " + cntError);
                        }
                        cntSucced = 0;
                        cntError = 0;
                        cntError2 = 0;
                    }
                    else
                    {
                        DataTable dt = grdCarStock.DataSource as DataTable;
                        int cntSucced = 0;
                        int cntError = 0;
                        if (dt != null)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                string ck = row.RowState.ToString();
                                if (row.RowState == DataRowState.Modified)
                                {
                                    string date = row["CAL_DATE"].ToString();
                                    string carStock = row["CAR_STOCK"].ToString();
                                    string remark = row["REMARK"].ToString();

                                    if (date != "" && carStock != "")
                                    {
                                        fnSaveCarStock("UPDATE_CAR_STOCK", date, carStock, remark);
                                        cntSucced++;
                                    }
                                    else
                                    {
                                        cntError++;
                                    }
                                }
                            }

                            dt.AcceptChanges();
                            QueryClick();
                            MessageBoxW("Update Succeed: " + cntSucced);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("SaveClick " + ex.Message);
            }
        }

        public override void PreviewClick()
        {
            base.PreviewClick();

            try
            {
                int row = gvwMain.FocusedRowHandle;
                string col = gvwMain.Columns["RENT_ID"].FieldName;
                string cekId = gvwMain.GetRowCellValue(row, col).ToString();

                fnPreview("GET_PREVIEW", cekId);

                QueryClick();
            }
            catch (Exception ex)
            {
                MessageBoxW("PreviewClick " + ex.Message);
            }
        }

        public override void DeleteClick()
        {
            base.DeleteClick();
            
            try
            {
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                fnSearchCancel("GET_FOR_CANCEL", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
            }
            catch (Exception ex)
            {
                MessageBoxW("gvwMain_RowClick " + ex.Message);
            }

        }

        private void fnGetCbo(string paramType)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;
                dtData = cProc.SetParamData(dtData, paramType, "", "", "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        cboStatus.Properties.DataSource = dtData;
                        cboStatus.Properties.DisplayMember = "NAME";
                        cboStatus.Properties.ValueMember = "CODE";

                        DataRow[] defRow = dtData.Select("CODE = 'A'");
                        cboStatus.EditValue = defRow.Length > 0 ? cboStatus.EditValue = defRow[0]["CODE"] : cboStatus.EditValue = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetCbo " + ex.Message);
            }
        }

        private void fnGetCboPop(string paramType)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;
                dtData = cProc.SetParamData(dtData, paramType, "", "", "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtItem = rs.ResultDataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetCbo " + ex.Message);
            }
        }

        private void fnSearch(string paramType, string paramFrom, string paramTo, string paramStatus)
        {
            try
            {
                //REFRESH GRID
                while (gvwMain.RowCount > 0)
                {
                    gvwMain.DeleteRow(0);
                }

                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;
                //DataTable dtAdmin = null;

                dtData = cProc.SetParamData(dtData, paramType, paramFrom, paramTo, paramStatus);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        grdMain.DataSource = dtData;
                        PreviewButton = true;
                        DeleteButton = true;
                        //SetData(grdMain, dtData);
                        dtData.AcceptChanges();

                        DataTable dtDriver = null;
                        DataTable dtCar = null;
                        dtDriver = cProc.SetParamData(dtDriver, "GET_DRIVER", "", "");
                        dtCar = cProc.SetParamData(dtCar, "GET_CAR", "", "");
                        ResultSet rsDrvr = CommonCallQuery(dtDriver, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);
                        ResultSet rsCar = CommonCallQuery(dtCar, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);
                        if (rsDrvr != null && rsDrvr.ResultDataSet.Tables.Count > 0)
                        {
                            dtDriver = rsDrvr.ResultDataSet.Tables[0];
                            if (dtDriver.Rows.Count > 0)
                            {
                                repDriver.DataSource = dtDriver;
                                repDriver.DisplayMember = "NAME";
                                repDriver.ValueMember = "CODE";
                            }
                        }

                        if (rsCar != null && rsCar.ResultDataSet.Tables.Count > 0)
                        {
                            dtCar = rsCar.ResultDataSet.Tables[0];
                            if (dtCar.Rows.Count > 0)
                            {
                                repCar.DataSource = dtCar;
                                repCar.DisplayMember = "NAME";
                                repCar.ValueMember = "CODE";
                            }
                        }


                        fnDesign();

                        gvwMain.Columns["DRIVER_ID"].OptionsColumn.AllowEdit = true;
                        gvwMain.Columns["DETAIL_CAR"].OptionsColumn.AllowEdit = true;
                        gvwMain.Columns["SEQ"].OptionsColumn.AllowEdit = true;

                        #region [OLD]
                        ////VALIDASI ADMIN
                        //dtAdmin = cProc.SetParamData(dtAdmin, "GET_ADMIN");
                        //ResultSet rSet = CommonCallQuery(dtAdmin, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);
                        //dtAdmin = rSet.ResultDataSet.Tables[0];
                        //if (dtAdmin.Rows.Count > 0)
                        //{
                        //    foreach (DataRow rw in dtAdmin.Rows)
                        //    {
                        //        string adm = rw[0].ToString();
                        //        if (SessionInfo.UserID.ToUpper().Contains(adm.ToUpper()))
                        //        {
                        //            gvwMain.Columns["DRIVER_ID"].OptionsColumn.AllowEdit = true;
                        //            gvwMain.Columns["DETAIL_CAR"].OptionsColumn.AllowEdit = true;
                        //            //PreviewButton = true;
                        //            return;
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                    else
                    {
                        PreviewButton = false;
                        DeleteButton = false;
                        //fnDesign();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxW("fnSearch " + ex.Message);
            }
        }

        private void fnSearchCarStock(string paramType, string paramFrom, string paramTo, string paramStatus)
        {
            try
            {
                //REFRESH GRID
                while (gvwCarStock.RowCount > 0)
                {
                    gvwCarStock.DeleteRow(0);
                }

                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, paramFrom, paramTo, paramStatus);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        grdCarStock.DataSource = dtData;
                        PreviewButton = false;
                        DeleteButton = false;
                        dtData.AcceptChanges();

                        fnDesignCarStock();

                        gvwCarStock.Columns["CAL_DATE"].OptionsColumn.AllowEdit = false;
                        gvwCarStock.Columns["CAL_DAY"].OptionsColumn.AllowEdit = false;
                        gvwCarStock.Columns["CAR_STOCK"].OptionsColumn.AllowEdit = true;
                        gvwCarStock.Columns["REMARK"].OptionsColumn.AllowEdit = true;
                    }
                    else
                    {
                        PreviewButton = false;
                        DeleteButton = false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxW("fnSearch " + ex.Message);
            }
        }

        private void fnUpdate(string RENT_ID, string DRIVER_ID, string DETAIL_CAR, string SEQ)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  "UPDATE", //ACTION
                                                  RENT_ID, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  DRIVER_ID, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  DETAIL_CAR, //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  SEQ,
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "F", //CONFIRM/FINISH
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  SessionInfo.UserID,   //UPDATER
                                                  DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                  Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Save Succeed");
                    //QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnUpdate " + ex.Message);
            }
        }

        private void fnEdit(string RENT_ID, string DRIVER_ID, string DETAIL_CAR, string SEQ)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  "EDIT", //ACTION
                                                  RENT_ID, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  DRIVER_ID, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  DETAIL_CAR, //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  SEQ,
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "F", //CONFIRM/FINISH
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  SessionInfo.UserID,   //UPDATER
                                                  DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                  Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Save Succeed");
                    //QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnUpdate " + ex.Message);
            }
        }

        private void fnPreview(string paramType, string paramId)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, paramId);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        rptSIOK rpt = new rptSIOK();
                        rpt.BindData(dtData);

                        ReportPrintTool prntTool = new ReportPrintTool(rpt);
                        prntTool.ShowPreview();

                        Form prvForm = prntTool.PreviewForm;
                        if (prvForm != null)
                        {
                            prvForm.WindowState = FormWindowState.Maximized;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxW("fnPreview " + ex.Message);
            }
        }

        private void fnDesign()
        {
            try
            {
                gvwMain.ColumnPanelRowHeight = 40;
                gvwMain.RowHeight = 30;
                gvwMain.OptionsView.ShowFooter = true;
                gvwMain.Appearance.FooterPanel.Font = new Font("Calibri", 12, FontStyle.Bold);
                gvwMain.Columns["HOUR_DURATION"].Summary.Clear();
                gvwMain.Columns["HOUR_DURATION"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "HOUR_DURATION", "Total: {0:N0}");

                for (int i = 0; i < gvwMain.Columns.Count; i++)
                {
                    gvwMain.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwMain.Columns[i].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
                    gvwMain.Columns[i].OptionsColumn.AllowEdit = false;
                    gvwMain.Columns[i].AppearanceCell.Font = new Font("Calibri", 12, FontStyle.Regular);
                    gvwMain.Columns[i].Width = gvwMain.Columns[i].GetBestWidth();

                    if (i == 0 || i == 1 || i == 2 || i == 8 || i == 14 || i == 15 || i == 17)
                    {
                        gvwMain.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    }
                    else if (i == 10 || i == 16)
                    {
                        gvwMain.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnDesign " + ex.Message);
            }
        }

        private void fnDesignCarStock()
        {
            try
            {

                gvwCarStock.ColumnPanelRowHeight = 40;
                gvwCarStock.RowHeight = 30;
                gvwCarStock.OptionsView.ShowFooter = true;
                gvwCarStock.Appearance.FooterPanel.Font = new Font("Calibri", 12, FontStyle.Bold);
                //gvwCarStock.Columns["HOUR_DURATION"].Summary.Clear();
                //gvwCarStock.Columns["HOUR_DURATION"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "HOUR_DURATION", "Total: {0:N0}");

                for (int i = 0; i < gvwCarStock.Columns.Count; i++)
                {
                    gvwCarStock.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwCarStock.Columns[i].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
                    gvwCarStock.Columns[i].OptionsColumn.AllowEdit = false;
                    gvwCarStock.Columns[i].AppearanceCell.Font = new Font("Calibri", 12, FontStyle.Regular);
                    gvwCarStock.Columns[i].Width = gvwMain.Columns[i].GetBestWidth();
                }

                gvwCarStock.Columns["CAL_DATE"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gvwCarStock.Columns["HOLIDAY_YN"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gvwCarStock.Columns["CAR_STOCK"].Width = 1000;
                gvwCarStock.Columns["REMARK"].Width = 2500;

                gvwCarStock.OptionsView.ShowGroupPanel = false;
            }
            catch (Exception ex)
            {
                MessageBoxW("fnDesign " + ex.Message);
            }
        }

        private void fnDesignCancel()
        {
            try
            {
                gvwCancel.ColumnPanelRowHeight = 40;
                gvwCancel.RowHeight = 30;
                gvwCancel.OptionsView.ShowFooter = true;
                gvwCancel.Appearance.FooterPanel.Font = new Font("Calibri", 12, FontStyle.Bold);
                gvwCancel.OptionsView.ShowGroupPanel = false;
                gvwCancel.Columns["MEMO"].Summary.Clear();
                gvwCancel.Columns["MEMO"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "MEMO", "Total: {0:N0}");

                for (int i = 0; i < gvwCancel.Columns.Count; i++)
                {
                    gvwCancel.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwCancel.Columns[i].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
                    gvwCancel.Columns[i].OptionsColumn.AllowEdit = false;
                    gvwCancel.Columns[i].AppearanceCell.Font = new Font("Calibri", 12, FontStyle.Regular);
                    //gvwCancel.Columns[i].Width = gvwMain.Columns[i].GetBestWidth();

                    if (i == 7)
                    {
                        gvwCancel.Columns[i].Width = 900;
                    }
                    else if (i == 0 || i == 5 || i == 6)
                    {
                        gvwCancel.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnDesign " + ex.Message);
            }
        }

        private void dtEFrom_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtETo.EditValue != null && dtEFrom.EditValue != null && cboStatus.EditValue != null)
                {
                    if (dtEFrom.DateTime > dtETo.DateTime)
                    {
                        dtETo.DateTime = dtEFrom.DateTime;
                    }

                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("dtEFrom_EditValueChanged " + ex.Message);
            }
        }

        private void dtETo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtETo.EditValue != null && dtEFrom.EditValue != null && cboStatus.EditValue != null)
                {
                    if (dtEFrom.DateTime > dtETo.DateTime)
                    {
                        dtEFrom.DateTime = dtETo.DateTime;
                    }

                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("dtETo_EditValueChanged " + ex.Message);
            }
        }

        private void cboStatus_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                QueryClick();
            }
            catch (Exception ex)
            {
                MessageBoxW("cboStatus_EditValueChanged " + ex.Message);
            }
        }

        private void gvwMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView vw = sender as GridView;
                string driverIdValue = vw.GetRowCellValue(e.RowHandle, "DRIVER_ID").ToString();
                string carValue = vw.GetRowCellValue(e.RowHandle, "DETAIL_CAR").ToString();
                string sts = vw.GetRowCellValue(e.RowHandle, "STATUS").ToString();
                string seq = vw.GetRowCellValue(e.RowHandle, "SEQ").ToString();
                string startDate = vw.GetRowCellValue(e.RowHandle, "START_DATE").ToString();
                DateTime departure = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                if (e.Column.FieldName.Contains("DRIVER_ID"))
                {
                    if (driverIdValue.Length > 0 && sts == "F")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                    else if (sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                }

                if (e.Column.FieldName.Contains("DETAIL_CAR"))
                {
                    if (carValue.Length > 0 && sts == "F")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                    else if (sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                }

                if (e.Column.FieldName.Contains("STATUS"))
                {
                    if (sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                    else
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                }

                if (e.Column.FieldName.Contains("SEQ"))
                {
                    if (seq.Length > 0 && sts == "F")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                    else if (sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                }

                if (departure < DateTime.Now && sts == "R")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#D1CFC8");
                }

                if (sts == "C")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#F47174");
                    e.Appearance.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("gvwMain_RowCellStyle " + ex.Message);
            }
        }

        private void gvwMain_RowClick(object sender, RowClickEventArgs e)
        {
            //try
            //{
            //    if (e.Clicks > 1)
            //    {
            //        splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            //        fnSearchCancel("GET_ALL", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBoxW("gvwMain_RowClick " + ex.Message);
            //}
        }

        private void fnSearchCancel(string paramType, string paramFrom, string paramTo, string paramStatus)
        {
            try
            {
                //REFRESH GRID
                while (gvwCancel.RowCount > 0)
                {
                    gvwCancel.DeleteRow(0);
                }

                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, paramFrom, paramTo, paramStatus);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        grdCancel.DataSource = dtData;
                        dtData.AcceptChanges();

                        fnDesignCancel();

                        gvwCancel.Columns["MEMO"].OptionsColumn.AllowEdit = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSearchCancel " + ex.Message);
            }
        }

        private void gvwMain_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            //int row = gvwMain.FocusedRowHandle;
            //string col = gvwMain.Columns["STATUS"].FieldName;
            //string status = gvwMain.GetRowCellValue(row, col).ToString();

            //if (status != "C")
            //{
            //    SaveButton = true;
            //}
        }

        private string getIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBoxW("getIpAddress " + ex.Message);
                return null;
            }
        }

        private string GetMacAddress()
        {
            try
            {
                var macAddr =
                    (
                        from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == OperationalStatus.Up
                        select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();
                return macAddr;
            }
            catch (Exception ex)
            {
                MessageBoxW("GetMacAddress " + ex.Message);
                return null;
            }
        }

        private void chkAutoRf_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //string interval = txtInterval.Text != "0" ? txtInterval.Text : "1";
                //txtInterval.Text = interval;
                if (chkAutoRf.CheckState == CheckState.Checked)
                {
                    tmrRefresh.Interval = Convert.ToInt16(txtInterval.Text) * 60 * 1000;
                    tmrRefresh.Enabled = true;
                    tmrRefresh.Start();
                }
                else
                {
                    tmrRefresh.Enabled = false;
                    tmrRefresh.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("chkAutoRf_CheckedChanged " + ex.Message);
            }
        }

        private void txtInterval_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                #region [OLD]
                //int interval;
                //if (int.TryParse(txtInterval.Text, out interval) && interval > 0)
                //{
                //    tmrRefresh.Interval = Convert.ToInt16(txtInterval.Text) * 60 * 1000;
                //    tmrRefresh.Enabled = true;
                //    tmrRefresh.Start();
                //}
                //else
                //{
                //    txtInterval.Text = "1";
                //    tmrRefresh.Interval = 1 * 60 * 1000;
                //    tmrRefresh.Enabled = true;
                //    tmrRefresh.Start();
                //}
                #endregion

                string interval = txtInterval.Text != "0" ? txtInterval.Text : "0";

                if (interval != "0")
                {
                    chkAutoRf.CheckState = CheckState.Checked;
                }
                else
                {
                    chkAutoRf.CheckState = CheckState.Unchecked;
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("txtInterval_EditValueChanged " + ex.Message);
            }
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            QueryClick();
        }

        private void btnAddDriver_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                btnAddDriver.Image =  Properties.Resources.addDriverPress;
            }
            catch (Exception ex)
            {
                MessageBoxW("btnAddDriver_MouseDown " + ex.Message);
            }
        }

        private void btnAddDriver_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                btnAddDriver.Image = Properties.Resources.addDriver;
            }
            catch (Exception ex)
            {
                MessageBoxW("btnAddDriver_MouseDown " + ex.Message);
            }
        }

        private void btnAddCar_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                btnAddCar.Image = Properties.Resources.addCarPress;
            }
            catch (Exception ex)
            {
                MessageBoxW("btnAddDriver_MouseDown " + ex.Message);
            }
        }

        private void btnAddCar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                btnAddCar.Image = Properties.Resources.addCar;
            }
            catch (Exception ex)
            {
                MessageBoxW("btnAddDriver_MouseDown " + ex.Message);
            }
        }

        private void btnAddDriver_Click(object sender, EventArgs e)
        {
            try
            {
                fnGetDataDriver("GET_DATA_DRIVER");
            }
            catch (Exception ex)
            {
                MessageBoxW("btnAddDriver_Click " + ex.Message);
            }
        }

        private void fnGetDataDriver(string paramType)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];

                    if (dtData.Rows.Count > 0)
                    {
                        AddDriver popDriver = new AddDriver(dtData, this);
                        popDriver.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetDataDriver " + ex.Message);
            }
        }

        public void fnSaveDriver(string paramType, string name, string contact)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  paramType, //ACTION
                                                  name, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  contact, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  "", //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //CONFIRM/FINISH
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  SessionInfo.UserID,   //UPDATER
                                                  DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                  Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress(), //UPDATE_PC
                                                  "",
                                                  "",
                                                  ""
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Save Succeed");
                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSaveDriver " + ex.Message);
            }
        }

        public void fnUpdateDriver(string paramType, string id, string name, string contact)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  paramType, //ACTION
                                                  id, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  name, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  contact, //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //CONFIRM/FINISH
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  SessionInfo.UserID,   //UPDATER
                                                  DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                  Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Save Succeed");
                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSaveDriver " + ex.Message);
            }
        }

        public void fnDeleteDriver(string paramType, string id)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  paramType, //ACTION
                                                  id, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  "", //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  "", //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //CONFIRM/FINISH
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //UPDATER
                                                  "", //UPDATE_DT
                                                  ""  //UPDATE_PC
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Delete Succeed");
                    MessageBox.Show("Delete Succeed");
                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSaveDriver " + ex.Message);
            }
        }

        public DataTable fnRefreshDataDriver(string paramType)
        {
            DataTable dtData = new DataTable();
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414();
                dtData = null;
                dtData = cProc.SetParamData(dtData, paramType, "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                }
                else
                {
                    dtData.Clear(); 
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetDataDriver " + ex.Message);
            }

            return dtData;
        }

        private void fnGetDataCar(string paramType)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];

                    if (dtData.Rows.Count > 0)
                    {
                        AddCar popCar = new AddCar(dtData, this);
                        popCar.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetDataDriver " + ex.Message);
            }
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            try
            {
                fnGetDataCar("GET_DATA_CAR");
            }
            catch (Exception ex)
            {
                MessageBoxW("btnAddCar_Click " + ex.Message);
            }
        }

        public void fnSaveCar(string paramType, string name, string serialNo, string color)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  paramType, //ACTION
                                                  name, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  serialNo, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  color, //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //CONFIRM/FINISH
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  SessionInfo.UserID,   //UPDATER
                                                  DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                  Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress(), //UPDATE_PC
                                                  "",
                                                  "",
                                                  ""
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Save Succeed");
                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSaveDriver " + ex.Message);
            }
        }

        public void fnUpdateCar(string paramType, string id, string name, string serialNo, string color)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  paramType, //ACTION
                                                  id, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  name, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  serialNo, //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  color, //PLANT_CD ==> PINJEM
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //CONFIRM/FINISH
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  SessionInfo.UserID,   //UPDATER
                                                  DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                  Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Save Succeed");
                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSaveDriver " + ex.Message);
            }
        }

        public void fnDeleteCar(string paramType, string id)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  paramType, //ACTION
                                                  id, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  "", //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  "", //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //CONFIRM/FINISH
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //UPDATER
                                                  "", //UPDATE_DT
                                                  ""  //UPDATE_PC
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Delete Succeed");
                    MessageBox.Show("Delete Succeed");
                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSaveDriver " + ex.Message);
            }
        }

        private void repDriver_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                GridView vw = gvwMain;
                if (vw.FocusedRowHandle >= 0)
                {
                    object val = (sender as DevExpress.XtraEditors.BaseEdit).EditValue;

                    string contact = fnGetContact("GET_CONTACT", val as string);
                    vw.SetRowCellValue(vw.FocusedRowHandle, "CONTACT", contact);

                    int row = gvwMain.FocusedRowHandle;
                    string col = gvwMain.Columns["STATUS"].FieldName;
                    string status = gvwMain.GetRowCellValue(row, col).ToString();

                    if (status != "C")
                    {
                        SaveButton = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("repDriver_EditValueChanged " + ex.Message);
            }
        }

        private string fnGetContact(string paramType, string id)
        {
            string contact = "";
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, id);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];

                    if (dtData.Rows.Count > 0)
                    {
                        contact = dtData.Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetContact " + ex.Message);
            }
            return contact;
        }

        private void repCar_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                GridView vw = gvwMain;
                if (vw.FocusedRowHandle >= 0)
                {
                    object val = (sender as DevExpress.XtraEditors.BaseEdit).EditValue;

                    string serialNo = fnGetContact("GET_SERIAL_NO", val as string);
                    string color = fnGetContact("GET_COLOR", val as string);
                    vw.SetRowCellValue(vw.FocusedRowHandle, "SERIAL_NO", serialNo);
                    vw.SetRowCellValue(vw.FocusedRowHandle, "COLOR", color);

                    int row = gvwMain.FocusedRowHandle;
                    string col = gvwMain.Columns["STATUS"].FieldName;
                    string status = gvwMain.GetRowCellValue(row, col).ToString();

                    if (status != "C")
                    {
                        SaveButton = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("repCar_EditValueChanged " + ex.Message);
            }
        }

        private void repDriver_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
        {
            //try
            //{
            //    LookUpEdit edit = sender as LookUpEdit;
            //    if (edit != null && e.Value != null)
            //    {
            //        // Coba cari berdasarkan ValueMember (CODE)
            //        DataRowView row = edit.Properties.GetDataSourceRowByKeyValue(e.Value) as DataRowView;

            //        if (row != null)
            //        {
            //            e.DisplayText = row["NAME"].ToString(); // Jika cocok, tampilkan NAME
            //        }
            //        else
            //        {
            //            e.DisplayText = e.Value.ToString(); // Jika tidak cocok, tampilkan CODE
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBoxW("repDriver_CustomDisplayText " + ex.Message);
            //}
        }

        private void btnExportExcel_MouseDown(object sender, MouseEventArgs e)
        {
            btnExportExcel.Image = Properties.Resources.ms_excel_press;
        }

        private void btnExportExcel_MouseUp(object sender, MouseEventArgs e)
        {
            btnExportExcel.Image = Properties.Resources.ms_excel;
        }

        //private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        //{
        //    btnCancel.Image = Properties.Resources.cancelledClick;
        //}

        //private void btnCancel_MouseUp(object sender, MouseEventArgs e)
        //{
        //    btnCancel.Image = Properties.Resources.cancelled;
        //}

        //private void exportToExcel(GridView gvw)
        //{
        //    try
        //    {
        //        Excel.Application excelApp = new Excel.Application();
        //        excelApp.Visible = true;
        //        Excel.Workbook workBook = excelApp.Workbooks.Add();
        //        Excel.Worksheet workSheet = (Excel.Worksheet)workBook.Sheets[1];

        //        //Header Row
        //        for (int col = 0; col < gvwMain.Columns.Count; col++)
        //        {
        //            workSheet.Cells[1, col + 1] = gvwMain.Columns[col].FieldName;
        //        }

        //        //Data Row
        //        for (int row = 0; row < gvwMain.RowCount; row++)
        //        {
        //            for (int col = 0; col < gvwMain.Columns.Count; col++)
        //            {
        //                object cellValue = gvwMain.GetRowCellValue(row, gvwMain.Columns[col]);
        //                workSheet.Cells[row + 2, col + 1] = cellValue != null ? cellValue.ToString() : "";
        //            }
        //        }

        //        //Save the Excel file
        //        SaveFileDialog saveFileDialog = new SaveFileDialog();
        //        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
        //        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            workBook.SaveAs(saveFileDialog.FileName);
        //            workBook.Close();
        //            excelApp.Quit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBoxW("exportToExcel " + ex.Message);
        //    }
        //}

        private void exportToExcel(GridView gvw)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;
                Excel.Workbook workBook = excelApp.Workbooks.Add();
                Excel.Worksheet workSheet = (Excel.Worksheet)workBook.Sheets[1];

                // Header Row
                for (int col = 0; col < gvw.Columns.Count; col++)
                {
                    workSheet.Cells[1, col + 1] = gvw.Columns[col].Caption;

                    // Format semua kolom sebagai teks
                    ((Excel.Range)workSheet.Columns[col + 1]).NumberFormat = "@";
                }

                //Data Row
                for (int row = 0; row < gvwMain.RowCount; row++)
                {
                    for (int col = 0; col < gvwMain.Columns.Count; col++)
                    {
                        GridColumn column = gvwMain.Columns[col];
                        object cellValue = gvwMain.GetRowCellValue(row, column);

                        // Jika kolom adalah kolom driver (combobox)
                        if (column.FieldName == "DRIVER_ID")
                        {
                            // Coba dapatkan teks yang ditampilkan di combobox
                            cellValue = gvwMain.GetDisplayTextByColumnValue(column, cellValue);
                        }
                        else if (column.FieldName == "DETAIL_CAR")
                        {
                            cellValue = gvwMain.GetDisplayTextByColumnValue(column, cellValue);
                        }

                        workSheet.Cells[row + 2, col + 1] = cellValue != null ? cellValue.ToString() : "";
                    }
                }

                // Save the Excel file
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    workBook.SaveAs(saveFileDialog.FileName);
                    workBook.Close();
                    excelApp.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("exportToExcel " + ex.Message);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                pbProgressShow();

                if (gvwMain.RowCount > 0)
                {
                    exportToExcel(gvwMain);
                }
                else
                {
                    MessageBoxW("Please click search");
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("btnExportExcel_Click " + ex.Message);
            }
            finally
            {
                pbProgressHide();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Cancel Data", IconType.Warning) == DialogResult.Yes)
                {
                    int cntSucced = 0;
                    int cntError = 0;

                    DataTable dt = grdCancel.DataSource as DataTable;
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string ck = row.RowState.ToString();
                            if (row.RowState == DataRowState.Modified)
                            {
                                string rentalNo = row["REG_ID"].ToString();
                                string memo = row["MEMO"].ToString();
                                string startDate = row["STRT"].ToString();
                                DateTime departure = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                                if (rentalNo != "" && DateTime.Now < departure)
                                {
                                    fnCancel("SET_CANCEL", rentalNo, memo);
                                    cntSucced++;
                                }
                                else
                                {
                                    cntError++;
                                }
                            }
                        }

                        dt.AcceptChanges();
                    }

                    QueryClick();
                    fnSearchCancel("GET_FOR_CANCEL", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
                    if (cntSucced > 0 && cntError == 0)
                    {
                        MessageBoxW("Canceled Succeed: " + cntSucced);
                    }
                    else
                    {
                        MessageBoxW("Canceled Succeed: " + cntSucced + " & Unsucceed: " + cntError);
                    }
                    cntSucced = 0;
                    cntError = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("btnCancel_Click " + ex.Message);
            }
        }

        private void fnCancel(string paramType, string paramId, string paramMemo)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                  paramType, //ACTION
                                                  paramId, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                  paramMemo, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                  "", //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "", //ACTIVITIY_CD
                                                  "",
                                                  "", //PLACE_DESC
                                                  "",
                                                  "",
                                                  "", //USE_DESC
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "C", //STATUS
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  "",
                                                  SessionInfo.UserID,   //UPDATER
                                                  DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                  Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                  );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Canceled Succeed");
                    QueryClick();
                    //fnSearchCancel("GET_ALL", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnCancel " + ex.Message);
            }
        }

        private void gvwCancel_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView vw = sender as GridView;
                string memo = vw.GetRowCellValue(e.RowHandle, "MEMO").ToString();
                string startDate = vw.GetRowCellValue(e.RowHandle, "STRT").ToString();
                DateTime departure = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                if (memo != "")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#F47174");
                    e.Appearance.ForeColor = Color.White;
                }
                else if (departure < DateTime.Now && memo == "")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#D1CFC8");
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("gvwCancel_RowCellStyle " + ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string cekYear = dtYear.EditValue.ToString().Length > 4 ? dtYear.DateTime.ToString("yyyy") : dtYear.EditValue.ToString();
                generateCalendar("GENERATE_DATE", cekYear);
            }
            catch (Exception ex)
            {
                MessageBoxW("btnGenerate_Click " + ex.Message);
            }
        }

        private void generateCalendar(string paramType, string paramYear)
        {
            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Generate Data", IconType.Warning) == DialogResult.Yes)
                {
                    SP_GMES0414 cProc = new SP_GMES0414("S");
                    DataTable dtData = null;

                    dtData = cProc.SetParamDataInsert(dtData,
                                                      paramType, //ACTION
                                                      paramYear, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                      "", //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                      "", //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                      "", //PLANT_CD ==> PINJEM
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "", //ACTIVITIY_CD
                                                      "",
                                                      "", //PLACE_DESC
                                                      "",
                                                      "",
                                                      "", //USE_DESC
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "", //CONFIRM/FINISH
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      SessionInfo.UserID,   //UPDATER
                                                      DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                      Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                      );

                    if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                    {
                        //MessageBoxW("Save Succeed");
                        QueryClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("generateCalendar " + ex.Message);
            }
        }

        private void gvwCarStock_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView vw = sender as GridView;
                string holidayYn = vw.GetRowCellValue(e.RowHandle, "HOLIDAY_YN").ToString();
                if (holidayYn.Contains("Y"))
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#F29D9F");
                    e.Appearance.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("gvwCarStock_RowCellStyle " + ex.Message);
            }
        }

        private void repCarStock_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                GridView vw = gvwCarStock;
                if (vw.FocusedRowHandle >= 0)
                {
                    int row = vw.FocusedRowHandle;
                    string stock = vw.EditingValue != null ? vw.EditingValue.ToString() : "";

                    if (stock != "")
                    {
                        SaveButton = true;
                    }
                    else
                    {
                        SaveButton = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("repCarStock_EditValueChanged " + ex.Message);
            }
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPageIndex != 0)
                {
                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("xtraTabControl1_Click " + ex.Message);
            }
        }

        private void dtYear_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                QueryClick();
            }
            catch (Exception ex)
            {
                MessageBoxW("dtYear_EditValueChanged " + ex.Message);
            }
        }

        private void fnSaveCarStock(string paramType, string paramDate, string paramCarStock, string paramRemark)
        {
            try
            {
                SP_GMES0414 cProc = new SP_GMES0414("S");
                DataTable dtData = null;

                dtData = cProc.SetParamDataInsert(dtData,
                                                    paramType, //ACTION
                                                    paramDate, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                    paramCarStock, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                    paramRemark, //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "", //ACTIVITIY_CD
                                                    "",
                                                    "", //PLACE_DESC
                                                    "",
                                                    "",
                                                    "", //USE_DESC
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "", //CONFIRM/FINISH
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "",
                                                    "", "", "",
                                                    //SessionInfo.UserID,   //UPDATER
                                                    //DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                    //Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress(), //UPDATE_PC
                                                    "",
                                                    "",
                                                    ""
                                                    );

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    //MessageBoxW("Save Succeed");
                    //QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("" + ex.Message);
            }
        }
    }
}
