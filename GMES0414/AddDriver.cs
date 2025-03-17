using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSI.MES.P
{
    public partial class AddDriver : Form
    {
        GMES0414 _frm;

        public AddDriver(DataTable dtData, GMES0414 frm)
        {
            InitializeComponent();

            _frm = frm;

            gvwAddDriver.Columns["ID"].Visible = false;

            try
            {
                if (dtData.Rows.Count > 0)
                {
                    grdAddDriver.DataSource = dtData;
                    dtData.AcceptChanges();

                    fnDesign();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddDriver " + ex.Message);
            }
        }

        private void fnDesign()
        {
            try
            {
                gvwAddDriver.ColumnPanelRowHeight = 40;
                gvwAddDriver.RowHeight = 30;
                gvwAddDriver.OptionsView.ShowGroupPanel = false;
                for (int i = 0; i < gvwAddDriver.Columns.Count; i++)
                {
                    gvwAddDriver.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwAddDriver.Columns[i].AppearanceHeader.Font = new Font("Calibri", 14, FontStyle.Bold);
                    gvwAddDriver.Columns[i].AppearanceCell.Font = new Font("Calibri", 12, FontStyle.Regular);
                    gvwAddDriver.Columns[i].Width = gvwAddDriver.Columns[i].GetBestWidth();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("fnDesign " + ex.Message);
            }
        }

        private void btnAdd_MouseDown(object sender, MouseEventArgs e)
        {
            btnAdd.Image = Properties.Resources.PlusPress;
        }

        private void btnAdd_MouseUp(object sender, MouseEventArgs e)
        {
            btnAdd.Image = Properties.Resources.Plus;
        }

        private void btnMin_MouseDown(object sender, MouseEventArgs e)
        {
            btnMin.Image = Properties.Resources.MinusPress;
        }

        private void btnMin_MouseUp(object sender, MouseEventArgs e)
        {
            btnMin.Image = Properties.Resources.Minus;
        }

        private void btnSave_MouseDown(object sender, MouseEventArgs e)
        {
            btnSave.Image = Properties.Resources.ceklisHover;
        }

        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            btnSave.Image = Properties.Resources.ceklis;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                gvwAddDriver.AddNewRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnAdd_Click " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = grdAddDriver.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string cek = row.RowState.ToString();
                        if (row.RowState == DataRowState.Added)
                        {
                            string drvName = row["NAME"].ToString();
                            string contact = row["CONTACT"].ToString();

                            if (drvName != "")
                            {
                                _frm.fnSaveDriver("ADD_DRIVER", drvName, contact);
                            }
                        }
                        else if (row.RowState == DataRowState.Modified)
                        {
                            string id = row["ID"].ToString();
                            string drvName = row["NAME"].ToString();
                            string contact = row["CONTACT"].ToString();

                            if (id != "")
                            {
                                _frm.fnUpdateDriver("UPDATE_DRIVER", id, drvName, contact);
                            }
                        }
                    }

                    MessageBox.Show("Succeed");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("btnSave_Click " + ex.Message);
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            try
            {
                int row = gvwAddDriver.FocusedRowHandle;
                string col = gvwAddDriver.Columns["ID"].FieldName;
                string id = gvwAddDriver.GetRowCellValue(row, col).ToString();

                _frm.fnDeleteDriver("DELETE_DRIVER", id);

                DataTable data = _frm.fnRefreshDataDriver("GET_DATA_DRIVER");
                grdAddDriver.DataSource = data;

            }
            catch (Exception ex)
            {
                MessageBox.Show("btnMin_Click " + ex.Message);
            }
        }


    }
}
