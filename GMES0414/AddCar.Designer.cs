namespace CSI.MES.P
{
    partial class AddCar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grdAddCar = new DevExpress.XtraGrid.GridControl();
            this.gvwAddCar = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SERIAL_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMin = new DevExpress.XtraEditors.PictureEdit();
            this.btnAdd = new DevExpress.XtraEditors.PictureEdit();
            this.btnSave = new DevExpress.XtraEditors.PictureEdit();
            this.COLOR = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdAddCar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAddCar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnMin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grdAddCar
            // 
            this.grdAddCar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAddCar.Location = new System.Drawing.Point(0, 41);
            this.grdAddCar.MainView = this.gvwAddCar;
            this.grdAddCar.Name = "grdAddCar";
            this.grdAddCar.Size = new System.Drawing.Size(347, 229);
            this.grdAddCar.TabIndex = 0;
            this.grdAddCar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvwAddCar});
            // 
            // gvwAddCar
            // 
            this.gvwAddCar.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.NAME,
            this.SERIAL_NO,
            this.COLOR,
            this.ID});
            this.gvwAddCar.GridControl = this.grdAddCar;
            this.gvwAddCar.Name = "gvwAddCar";
            // 
            // NAME
            // 
            this.NAME.Caption = "Car Name";
            this.NAME.FieldName = "NAME";
            this.NAME.Name = "NAME";
            this.NAME.Visible = true;
            this.NAME.VisibleIndex = 0;
            // 
            // SERIAL_NO
            // 
            this.SERIAL_NO.Caption = "Serial No";
            this.SERIAL_NO.FieldName = "SERIAL_NO";
            this.SERIAL_NO.Name = "SERIAL_NO";
            this.SERIAL_NO.Visible = true;
            this.SERIAL_NO.VisibleIndex = 1;
            // 
            // ID
            // 
            this.ID.Caption = "ID";
            this.ID.FieldName = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = true;
            this.ID.VisibleIndex = 3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tableLayoutPanel1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(347, 41);
            this.panelControl1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.btnMin, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAdd, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(226, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(119, 37);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnMin
            // 
            this.btnMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMin.EditValue = global::CSI.MES.P.Properties.Resources.Minus;
            this.btnMin.Location = new System.Drawing.Point(80, 0);
            this.btnMin.Margin = new System.Windows.Forms.Padding(0);
            this.btnMin.Name = "btnMin";
            this.btnMin.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnMin.Properties.Appearance.Options.UseBackColor = true;
            this.btnMin.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.btnMin.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.btnMin.Size = new System.Drawing.Size(39, 37);
            this.btnMin.TabIndex = 0;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            this.btnMin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnMin_MouseDown);
            this.btnMin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMin_MouseUp);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.EditValue = global::CSI.MES.P.Properties.Resources.Plus;
            this.btnAdd.Location = new System.Drawing.Point(40, 0);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.Properties.Appearance.Options.UseBackColor = true;
            this.btnAdd.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.btnAdd.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.btnAdd.Size = new System.Drawing.Size(35, 37);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAdd_MouseDown);
            this.btnAdd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAdd_MouseUp);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.EditValue = global::CSI.MES.P.Properties.Resources.ceklis;
            this.btnSave.Location = new System.Drawing.Point(0, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Properties.Appearance.Options.UseBackColor = true;
            this.btnSave.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.btnSave.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.btnSave.Size = new System.Drawing.Size(35, 37);
            this.btnSave.TabIndex = 2;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSave_MouseDown);
            this.btnSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSave_MouseUp);
            // 
            // COLOR
            // 
            this.COLOR.Caption = "Color";
            this.COLOR.FieldName = "COLOR";
            this.COLOR.Name = "COLOR";
            this.COLOR.Visible = true;
            this.COLOR.VisibleIndex = 2;
            // 
            // AddCar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 270);
            this.Controls.Add(this.grdAddCar);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Car";
            ((System.ComponentModel.ISupportInitialize)(this.grdAddCar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvwAddCar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnMin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdAddCar;
        private DevExpress.XtraGrid.Views.Grid.GridView gvwAddCar;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn NAME;
        private DevExpress.XtraGrid.Columns.GridColumn SERIAL_NO;
        private DevExpress.XtraEditors.PictureEdit btnMin;
        private DevExpress.XtraEditors.PictureEdit btnAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PictureEdit btnSave;
        private DevExpress.XtraGrid.Columns.GridColumn ID;
        private DevExpress.XtraGrid.Columns.GridColumn COLOR;
    }
}