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
    public partial class AddCar : Form
    {
        GMES0414 _frm;

        public AddCar(DataTable dtData, GMES0414 frm)
        {
            InitializeComponent();

            _frm = frm;

            gvwAddCar.Columns["ID"].Visible = false;
            btnAdd.Image = Properties.Resources.Plus;

            try
            {
                if (dtData.Rows.Count > 0)
                {
                    grdAddCar.DataSource = dtData;
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
                gvwAddCar.ColumnPanelRowHeight = 40;
                gvwAddCar.RowHeight = 30;
                gvwAddCar.OptionsView.ShowGroupPanel = false;
                for (int i = 0; i < gvwAddCar.Columns.Count; i++)
                {
                    gvwAddCar.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwAddCar.Columns[i].AppearanceHeader.Font = new Font("Calibri", 14, FontStyle.Bold);
                    gvwAddCar.Columns[i].AppearanceCell.Font = new Font("Calibri", 12, FontStyle.Regular);
                    gvwAddCar.Columns[i].Width = gvwAddCar.Columns[i].GetBestWidth();
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
                gvwAddCar.AddNewRow();
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
                DataTable dt = grdAddCar.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string cek = row.RowState.ToString();
                        if (row.RowState == DataRowState.Added)
                        {
                            string carName = row["NAME"].ToString();
                            string serialNo = row["SERIAL_NO"].ToString();
                            string color = row["COLOR"].ToString();

                            if (carName != "")
                            {
                                _frm.fnSaveCar("ADD_CAR", carName, serialNo, color);
                            }
                        }
                        else if (row.RowState == DataRowState.Modified)
                        {
                            string id = row["ID"].ToString();
                            string carName = row["NAME"].ToString();
                            string serialNo = row["SERIAL_NO"].ToString();
                            string color = row["COLOR"].ToString();

                            if (id != "")
                            {
                                _frm.fnUpdateCar("UPDATE_CAR", id, carName, serialNo, color);
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
                int row = gvwAddCar.FocusedRowHandle;
                string col = gvwAddCar.Columns["ID"].FieldName;
                string id = gvwAddCar.GetRowCellValue(row, col).ToString();

                _frm.fnDeleteCar("DELETE_CAR", id);

                DataTable data = _frm.fnRefreshDataDriver("GET_DATA_CAR");
                grdAddCar.DataSource = data;

            }
            catch (Exception ex)
            {
                MessageBox.Show("btnMin_Click " + ex.Message);
            }
        }


    }
}
