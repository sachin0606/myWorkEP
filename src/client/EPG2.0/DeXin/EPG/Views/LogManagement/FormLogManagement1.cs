using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;
using DeXin.EPG.Views.QueryForms;

namespace DeXin.EPG.Views.LogManagement
{
	public class FormLogManagement1 : FormToolBase
	{
		public IList<LogInfo> logInfos = new List<LogInfo>();

		public PagingSearch pagingSearch = new PagingSearch();

		private LogBLL logBLL = new LogBLL();

		private IContainer components = null;

		private DataGridView dgvLogInfo;

		private EPGPagination epgPagination1;

		private DataGridViewTextBoxColumn colLogID;

		private DataGridViewTextBoxColumn colUserID;

		private DataGridViewTextBoxColumn colUserName;

		private DataGridViewTextBoxColumn colFunctionID;

		private DataGridViewTextBoxColumn colFunctionName;

		private DataGridViewTextBoxColumn colOSName;

		private DataGridViewTextBoxColumn colDescription;

		private DataGridViewTextBoxColumn colOperTime;

		private DataGridViewTextBoxColumn colIPAddr;

		public FormLogManagement1()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncLogManagement;
		}

		protected override void FormLoad()
		{
			if (!Permit(enumPermission.CanQuery))
			{
				epgPagination1.Enabled = false;
			}
			dgvLogInfo.AutoGenerateColumns = false;
			PrepareQueryByPaging();
			logInfos = new List<LogInfo>();
			epgPagination1.TotleCount = pagingSearch.TotalCount;
			BindDGV();
			SetMenuVisible(true, false, false, false, false, false, false, false, false);
			epgPagination1.Dock = DockStyle.Top;
			dgvLogInfo.Dock = DockStyle.Fill;
		}

		private void BindDGV()
		{
			BindingList<LogInfo> dataSource = new BindingList<LogInfo>(logInfos);
			dgvLogInfo.DataSource = dataSource;
		}

		protected override void FuncQuery()
		{
			_FunctionName = EnumFunctionName.SysFuncLogManagement;
			PrepareQueryByPaging();
			FormQueryLog formQueryLog = new FormQueryLog();
			formQueryLog.pagingSearch = pagingSearch;
			formQueryLog.Owner = this;
			formQueryLog.StartPosition = FormStartPosition.CenterParent;
			formQueryLog.MaximizeBox = false;
			formQueryLog.MinimizeBox = false;
			formQueryLog.WindowState = FormWindowState.Normal;
			formQueryLog.ShowDialog();
			formQueryLog.Close();
			epgPagination1.TotleCount = pagingSearch.TotalCount;
			BindDGV();
		}

		private void epgPagination1_PageIndexChanged(object sender, EventArgs e)
		{
			try
			{
				pagingSearch.PageIndex = epgPagination1.PageIndex;
				logInfos = logBLL.GetLogDataByPaging(pagingSearch).ToList();
				epgPagination1.TotleCount = pagingSearch.TotalCount;
				BindDGV();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void PrepareQueryByPaging()
		{
			pagingSearch.PKName = "LogID";
			pagingSearch.TableName = "LogInfoManagement";
			pagingSearch.Condition = PrepareCondition();
			pagingSearch.OrderBy = " LogID DESC";
			pagingSearch.ColumsNames = "*";
			pagingSearch.PageIndex = epgPagination1.PageIndex;
			pagingSearch.PageSize = epgPagination1.PageSize;
			pagingSearch.TotalCount = epgPagination1.TotleCount;
		}

		private string PrepareCondition()
		{
			return "";
		}

		protected override void FuncPrint()
		{
		}

		private void dgvLogInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.LogManagement.FormLogManagement1));
			this.epgPagination1 = new DeXin.EPG.Tools.EPGUserControl.EPGPagination();
			this.dgvLogInfo = new System.Windows.Forms.DataGridView();
			this.colLogID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colUserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colFunctionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colFunctionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colOSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colOperTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colIPAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvLogInfo).BeginInit();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.dgvLogInfo);
			base.pnlBase.Controls.Add(this.epgPagination1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			resources.ApplyResources(this.epgPagination1, "epgPagination1");
			this.epgPagination1.Name = "epgPagination1";
			this.epgPagination1.PageIndex = 0;
			this.epgPagination1.PageSize = 0;
			this.epgPagination1.TotleCount = 0;
			this.epgPagination1.PageIndexChanged += new System.EventHandler(epgPagination1_PageIndexChanged);
			this.dgvLogInfo.AllowUserToAddRows = false;
			this.dgvLogInfo.AllowUserToDeleteRows = false;
			this.dgvLogInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvLogInfo.Columns.AddRange(this.colLogID, this.colUserID, this.colUserName, this.colFunctionID, this.colFunctionName, this.colOSName, this.colDescription, this.colOperTime, this.colIPAddr);
			resources.ApplyResources(this.dgvLogInfo, "dgvLogInfo");
			this.dgvLogInfo.Name = "dgvLogInfo";
			this.dgvLogInfo.ReadOnly = true;
			this.dgvLogInfo.RowTemplate.Height = 23;
			this.dgvLogInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.colLogID.DataPropertyName = "LogID";
			resources.ApplyResources(this.colLogID, "colLogID");
			this.colLogID.Name = "colLogID";
			this.colLogID.ReadOnly = true;
			this.colUserID.DataPropertyName = "UserID";
			resources.ApplyResources(this.colUserID, "colUserID");
			this.colUserID.Name = "colUserID";
			this.colUserID.ReadOnly = true;
			this.colUserName.DataPropertyName = "UserName";
			resources.ApplyResources(this.colUserName, "colUserName");
			this.colUserName.Name = "colUserName";
			this.colUserName.ReadOnly = true;
			this.colUserName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colFunctionID.DataPropertyName = "FunctionID";
			resources.ApplyResources(this.colFunctionID, "colFunctionID");
			this.colFunctionID.Name = "colFunctionID";
			this.colFunctionID.ReadOnly = true;
			this.colFunctionName.DataPropertyName = "LocationFunctionName";
			resources.ApplyResources(this.colFunctionName, "colFunctionName");
			this.colFunctionName.Name = "colFunctionName";
			this.colFunctionName.ReadOnly = true;
			this.colOSName.DataPropertyName = "OSName";
			resources.ApplyResources(this.colOSName, "colOSName");
			this.colOSName.Name = "colOSName";
			this.colOSName.ReadOnly = true;
			this.colDescription.DataPropertyName = "Description";
			resources.ApplyResources(this.colDescription, "colDescription");
			this.colDescription.Name = "colDescription";
			this.colDescription.ReadOnly = true;
			this.colOperTime.DataPropertyName = "OperateDateTime";
			resources.ApplyResources(this.colOperTime, "colOperTime");
			this.colOperTime.Name = "colOperTime";
			this.colOperTime.ReadOnly = true;
			this.colIPAddr.DataPropertyName = "IPAddress";
			resources.ApplyResources(this.colIPAddr, "colIPAddr");
			this.colIPAddr.Name = "colIPAddr";
			this.colIPAddr.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormLogManagement1";
			base.pnlBase.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvLogInfo).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
