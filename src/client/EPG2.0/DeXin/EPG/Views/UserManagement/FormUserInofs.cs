using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.UserManagement
{
	public class FormUserInofs : FormToolBase
	{
		private IContainer components = null;

		private DGV dgvUsreInfos;

		private DataGridViewCheckBoxColumn CK;

		private DataGridViewTextBoxColumn DGVUsreID;

		private DataGridViewTextBoxColumn DGVUserName;

		private DataGridViewTextBoxColumn DGVpassword;

		private DataGridViewTextBoxColumn DGVSex;

		private DataGridViewComboBoxColumn DGVGroupID;

		private DataGridViewTextBoxColumn DGVPhoneNumber;

		private DataGridViewTextBoxColumn DGVCreator;

		private DataGridViewTextBoxColumn DGVCreateTime;

		private DataGridViewCheckBoxColumn DGVStatus;

		private UserInfoManager userManger = new UserInfoManager();

		public IList<UserInfo> users = new List<UserInfo>();

		private IList<string> Keys = new List<string>();

		private LogBLL logBll = new LogBLL();

		private ToolStripButton tsbSetInitialPasswork = null;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.UserManagement.FormUserInofs));
			this.dgvUsreInfos = new Com.Cn.Tool.UserControls.DGV();
			this.CK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.DGVUsreID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DGVUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DGVpassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DGVSex = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DGVGroupID = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.DGVPhoneNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DGVCreator = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DGVCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DGVStatus = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgvUsreInfos).BeginInit();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.dgvUsreInfos);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			this.dgvUsreInfos.AllowUserToAddRows = false;
			this.dgvUsreInfos.AllowUserToDeleteRows = false;
			this.dgvUsreInfos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvUsreInfos.Columns.AddRange(this.CK, this.DGVUsreID, this.DGVUserName, this.DGVpassword, this.DGVSex, this.DGVGroupID, this.DGVPhoneNumber, this.DGVCreator, this.DGVCreateTime, this.DGVStatus);
			resources.ApplyResources(this.dgvUsreInfos, "dgvUsreInfos");
			this.dgvUsreInfos.Name = "dgvUsreInfos";
			this.dgvUsreInfos.RowTemplate.Height = 23;
			this.dgvUsreInfos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvUsreInfos.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvUsreInfos_CellMouseClick);
			this.dgvUsreInfos.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvUsreInfos_CellMouseDoubleClick);
			this.dgvUsreInfos.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(dgvUsreInfos_DataError);
			resources.ApplyResources(this.CK, "CK");
			this.CK.Name = "CK";
			this.CK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.CK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			resources.ApplyResources(this.DGVUsreID, "DGVUsreID");
			this.DGVUsreID.Name = "DGVUsreID";
			resources.ApplyResources(this.DGVUserName, "DGVUserName");
			this.DGVUserName.Name = "DGVUserName";
			resources.ApplyResources(this.DGVpassword, "DGVpassword");
			this.DGVpassword.Name = "DGVpassword";
			resources.ApplyResources(this.DGVSex, "DGVSex");
			this.DGVSex.Name = "DGVSex";
			this.DGVGroupID.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			resources.ApplyResources(this.DGVGroupID, "DGVGroupID");
			this.DGVGroupID.Name = "DGVGroupID";
			this.DGVGroupID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.DGVGroupID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			resources.ApplyResources(this.DGVPhoneNumber, "DGVPhoneNumber");
			this.DGVPhoneNumber.Name = "DGVPhoneNumber";
			resources.ApplyResources(this.DGVCreator, "DGVCreator");
			this.DGVCreator.Name = "DGVCreator";
			resources.ApplyResources(this.DGVCreateTime, "DGVCreateTime");
			this.DGVCreateTime.Name = "DGVCreateTime";
			resources.ApplyResources(this.DGVStatus, "DGVStatus");
			this.DGVStatus.Name = "DGVStatus";
			this.DGVStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.DGVStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormUserInofs";
			base.pnlBase.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgvUsreInfos).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormUserInofs()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncUSERINFO;
		}

		protected override void FormLoad()
		{
			SetMenuVisible(false, true, true, false, false, false, false, true, false);
			CreateOtherToolStripButton();
			InitData();
		}

		private void CreateOtherToolStripButton()
		{
			tsbSetInitialPasswork = new ToolStripButton();
			tsbSetInitialPasswork.Name = "ResetPWD";
			tsbSetInitialPasswork.Text = ResourceManagement.GetLocationString("ResetPassword");
			tsbSetInitialPasswork.ToolTipText = ResourceManagement.GetLocationString("ResetPassword");
			tsbSetInitialPasswork.Click += tsbSetInitialPasswork_Click;
			toolStrip1.Items.Insert(3, tsbSetInitialPasswork);
		}

		private void tsbSetInitialPasswork_Click(object sender, EventArgs e)
		{
			DataGridViewRow currentRow = dgvUsreInfos.CurrentRow;
			if (currentRow != null)
			{
				FormResetPassword formResetPassword = new FormResetPassword(currentRow.Cells[DGVUsreID.Name].Value.ToString(), PageEnum.Edit);
				formResetPassword.WindowState = FormWindowState.Normal;
				formResetPassword.StartPosition = FormStartPosition.CenterParent;
				if (formResetPassword.ShowDialog() == DialogResult.OK)
				{
					GetAllUserInfos();
					BindUserInfo(users);
				}
			}
		}

		private void dgvUsreInfos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			base.FocusRowIndex = ((e.RowIndex >= 0) ? e.RowIndex : 0);
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				DataGridViewRow dataGridViewRow = dgvUsreInfos.Rows[e.RowIndex];
				string userID = Tool.GetString(dataGridViewRow.Cells[DGVUsreID.Name].Value);
				FormUserInfoDetail formUserInfoDetail = new FormUserInfoDetail(userID, PageEnum.Show);
				formUserInfoDetail.Owner = this;
				formUserInfoDetail.StartPosition = FormStartPosition.CenterParent;
				formUserInfoDetail.ShowDialog();
			}
		}

		private void dgvUsreInfos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			dgvUsreInfos.EndEdit();
			if (e.ColumnIndex != 0 || e.RowIndex < 0 || dgvUsreInfos.Rows.Count == 0)
			{
				return;
			}
			DataGridViewRow dataGridViewRow = dgvUsreInfos.Rows[e.RowIndex];
			DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[CK.Name];
			string item;
			if (Tool.GetBoolean(dataGridViewCell.Value))
			{
				dataGridViewCell.Value = false;
				item = Tool.GetString(dataGridViewRow.Cells[DGVUsreID.Name].Value);
				if (Keys.Contains(item))
				{
					Keys.Remove(item);
				}
				return;
			}
			dataGridViewCell.Value = true;
			item = Tool.GetString(dataGridViewRow.Cells[DGVUsreID.Name].Value);
			bool boolean = Tool.GetBoolean(dataGridViewRow.Cells[DGVStatus.Name].Value);
			if (!Keys.Contains(item) && boolean)
			{
				Keys.Add(item);
			}
		}

		private void dgvUsreInfos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
		}

		protected override void FuncQuery()
		{
			try
			{
				base.FocusRowIndex = 0;
				BindUserInfo(users);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected override void FuncAdd()
		{
			base.FocusRowIndex = 0;
			FormUserInfoDetail formUserInfoDetail = new FormUserInfoDetail(PageEnum.Add);
			formUserInfoDetail.Owner = this;
			formUserInfoDetail.StartPosition = FormStartPosition.CenterParent;
			formUserInfoDetail.ShowDialog();
			GetAllUserInfos();
		}

		protected override void FuncEdit()
		{
			DataGridViewRow currentRow = dgvUsreInfos.CurrentRow;
			if (currentRow != null)
			{
				base.FocusRowIndex = currentRow.Index;
				string userID = Tool.GetString(currentRow.Cells[DGVUsreID.Name].Value);
				FormUserInfoDetail formUserInfoDetail = new FormUserInfoDetail(userID, PageEnum.Edit);
				formUserInfoDetail.Owner = this;
				formUserInfoDetail.StartPosition = FormStartPosition.CenterParent;
				formUserInfoDetail.ShowDialog();
			}
		}

		protected override bool FuncDelete()
		{
			base.FocusRowIndex = 0;
			if (Keys.Count == 0)
			{
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SelectOneOrMoreRows]);
				return false;
			}
			if (!Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.OKCancel))
			{
				return false;
			}
			string[] array = Keys.ToArray();
			string text = "";
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				text = text + text2 + " ";
			}
			string message = "";
			try
			{
				if (userManger.DeleteStatusUserInfo(LoginUser.Ins.UserIns.UserID, array, out message))
				{
					logBll.AddLog(EnumFunctionName.SysFuncUSERINFO, EnumOSName.Forbid, text);
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			Tool.ShowMessage(message);
			return true;
		}

		protected void InitData()
		{
			BindUserGroup();
			dgvUsreInfos.AllowUserToAddRows = false;
			Tool.SetDGVReadOnly(dgvUsreInfos, true, "");
			string[] columnNames = new string[2] { DGVpassword.Name, CK.Name };
			Tool.SetDGVNotSortMode(dgvUsreInfos);
			Tool.SetDGVVisible(dgvUsreInfos, true, columnNames);
			GetAllUserInfos();
			tsbDelete.Text = ResourceManagement.GetLocationString("Forbid");
		}

		private void BindUserGroup()
		{
			GroupInfoBLL groupInfoBLL = new GroupInfoBLL();
			try
			{
				IList<GroupInfo> allData = groupInfoBLL.GetAllData();
				DGVGroupID.DataSource = allData;
				DGVGroupID.ValueMember = "GroupID";
				DGVGroupID.DisplayMember = "GroupName";
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void GetAllUserInfos()
		{
			try
			{
				Keys.Clear();
				users = userManger.SearchUserInfosByCondiction("", "", "", "", 0, 0);
				dgvUsreInfos.Rows.Clear();
				foreach (UserInfo user in users)
				{
					int rowIndex = dgvUsreInfos.Rows.Add(new DataGridViewRow());
					BindDataToDGV(rowIndex, user);
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void BindUserInfo(IList<UserInfo> userinfos)
		{
			dgvUsreInfos.Rows.Clear();
			Keys.Clear();
			foreach (UserInfo user in users)
			{
				int num = dgvUsreInfos.Rows.Add(new DataGridViewRow());
				BindDataToDGV(num, user);
				dgvUsreInfos.Rows[num].Selected = num == base.FocusRowIndex;
				if (num == base.FocusRowIndex)
				{
					dgvUsreInfos.CurrentCell = dgvUsreInfos.Rows[num].Cells[1];
				}
			}
		}

		private void BindDataToDGV(int rowIndex, UserInfo temp)
		{
			DataGridViewRow dataGridViewRow = dgvUsreInfos.Rows[rowIndex];
			dataGridViewRow.Cells[DGVUsreID.Name].Value = Tool.GetString(temp.UserID);
			dataGridViewRow.Cells[DGVUserName.Name].Value = Tool.GetString(temp.UserName);
			dataGridViewRow.Cells[DGVpassword.Name].Value = Tool.GetString(temp.Password);
			dataGridViewRow.Cells[DGVGroupID.Name].Value = Tool.GetInt32(temp.GroupID);
			dataGridViewRow.Cells[DGVSex.Name].Value = (temp.Sex ? ResourceManagement.GetLocationString("SexMale") : ResourceManagement.GetLocationString("SexFemale"));
			dataGridViewRow.Cells[DGVPhoneNumber.Name].Value = Tool.GetString(temp.PhoneNumber);
			dataGridViewRow.Cells[DGVCreator.Name].Value = Tool.GetString(temp.Creator);
			dataGridViewRow.Cells[DGVCreateTime.Name].Value = temp.CreateTime;
			dataGridViewRow.Cells[DGVStatus.Name].Value = Tool.GetBoolean(temp.Status);
		}

		private void ShowQueryForm()
		{
		}

		private void dgvUsreInfos_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			MessageBox.Show(e.ColumnIndex + " Row " + e.RowIndex);
		}
	}
}
