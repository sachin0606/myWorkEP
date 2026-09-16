using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;

namespace DeXin.EPG.Views.BaseForm
{
	public class FormToolBase : FormBase
	{
		private IContainer components = null;

		protected ToolStrip toolStrip1;

		protected ToolStripButton tsbQuery;

		protected ToolStripButton tsbAdd;

		protected ToolStripButton tsbEdit;

		protected ToolStripButton tsbSave;

		protected ToolStripButton tsbDelete;

		protected ToolStripButton tsbCancel;

		protected ToolStripButton tsbPrint;

		protected ToolStripButton tsbClose;

		protected GroupBox pnlBase;

		private bool _IsUseEnable = false;

		protected string ContentText
		{
			get
			{
				return pnlBase.Text;
			}
			set
			{
				pnlBase.Text = value;
			}
		}

		public bool IsUseEnable
		{
			get
			{
				return _IsUseEnable;
			}
			set
			{
				_IsUseEnable = value;
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.BaseForm.FormToolBase));
			this.pnlBase = new System.Windows.Forms.GroupBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbQuery = new System.Windows.Forms.ToolStripButton();
			this.tsbAdd = new System.Windows.Forms.ToolStripButton();
			this.tsbEdit = new System.Windows.Forms.ToolStripButton();
			this.tsbSave = new System.Windows.Forms.ToolStripButton();
			this.tsbDelete = new System.Windows.Forms.ToolStripButton();
			this.tsbCancel = new System.Windows.Forms.ToolStripButton();
			this.tsbPrint = new System.Windows.Forms.ToolStripButton();
			this.tsbClose = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(this.pnlBase, "pnlBase");
			this.pnlBase.Name = "pnlBase";
			this.pnlBase.TabStop = false;
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.tsbQuery, this.tsbAdd, this.tsbEdit, this.tsbSave, this.tsbDelete, this.tsbCancel, this.tsbPrint, this.tsbClose });
			this.toolStrip1.Name = "toolStrip1";
			resources.ApplyResources(this.tsbQuery, "tsbQuery");
			this.tsbQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbQuery.Name = "tsbQuery";
			this.tsbQuery.Click += new System.EventHandler(tsbQuery_Click);
			resources.ApplyResources(this.tsbAdd, "tsbAdd");
			this.tsbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbAdd.Name = "tsbAdd";
			this.tsbAdd.Click += new System.EventHandler(tsbAdd_Click);
			resources.ApplyResources(this.tsbEdit, "tsbEdit");
			this.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbEdit.Name = "tsbEdit";
			this.tsbEdit.Click += new System.EventHandler(tsbEdit_Click);
			resources.ApplyResources(this.tsbSave, "tsbSave");
			this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbSave.Name = "tsbSave";
			this.tsbSave.Click += new System.EventHandler(tsbSave_Click);
			resources.ApplyResources(this.tsbDelete, "tsbDelete");
			this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbDelete.Name = "tsbDelete";
			this.tsbDelete.Click += new System.EventHandler(tsbDelete_Click);
			resources.ApplyResources(this.tsbCancel, "tsbCancel");
			this.tsbCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbCancel.Name = "tsbCancel";
			this.tsbCancel.Click += new System.EventHandler(tsbCancel_Click);
			resources.ApplyResources(this.tsbPrint, "tsbPrint");
			this.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbPrint.Name = "tsbPrint";
			this.tsbPrint.Click += new System.EventHandler(tsbPrint_Click);
			resources.ApplyResources(this.tsbClose, "tsbClose");
			this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbClose.Name = "tsbClose";
			this.tsbClose.Click += new System.EventHandler(tsbClose_Click);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.pnlBase);
			base.Controls.Add(this.toolStrip1);
			base.Name = "FormToolBase";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormToolBase()
		{
			InitializeComponent();
			toolStrip1.Renderer = new ToolTripRenderProfessionStyle();
		}

		protected void SetMenuVisible(bool queryVisible, bool addVisible, bool editVisible, bool delVisible, bool saveVisible, bool printVisible, bool helpVisible, bool closeVisible, bool cancelVisible)
		{
			tsbQuery.Visible = queryVisible;
			tsbAdd.Visible = addVisible;
			tsbEdit.Visible = editVisible;
			tsbDelete.Visible = delVisible;
			tsbCancel.Visible = cancelVisible;
			tsbSave.Visible = saveVisible;
			tsbPrint.Visible = false;
			tsbClose.Visible = closeVisible;
		}

		private void SetMenuEnabled(bool queryEnabled, bool addEnabled, bool editEnabled, bool delEnabled, bool cancelEnabled, bool saveEnabled, bool printEnabled, bool helpEnabled, bool closeEnabled)
		{
			tsbQuery.Enabled = queryEnabled;
			tsbAdd.Enabled = addEnabled;
			tsbEdit.Enabled = editEnabled;
			tsbDelete.Enabled = delEnabled;
			tsbCancel.Enabled = cancelEnabled;
			tsbSave.Enabled = saveEnabled;
			tsbPrint.Enabled = printEnabled;
			tsbClose.Enabled = closeEnabled;
		}

		private void tsbQuery_Click(object sender, EventArgs e)
		{
			editModeFlag = EnumEditModeFlag.Q;
			if (Permit(enumPermission.CanQuery, true))
			{
				FuncQuery();
			}
		}

		protected virtual void FuncQuery()
		{
		}

		private void tsbAdd_Click(object sender, EventArgs e)
		{
			if (Permit(enumPermission.CanAdd, true))
			{
				FuncAdd();
				editModeFlag = EnumEditModeFlag.A;
				if (IsUseEnable)
				{
					InitialToolButtonEnable(editModeFlag);
				}
			}
		}

		protected virtual void FuncAdd()
		{
		}

		private void tsbEdit_Click(object sender, EventArgs e)
		{
			if (Permit(enumPermission.CanEdit, true))
			{
				FuncEdit();
				editModeFlag = EnumEditModeFlag.E;
				if (IsUseEnable)
				{
					InitialToolButtonEnable(editModeFlag);
				}
			}
		}

		protected virtual void FuncEdit()
		{
		}

		private void tsbSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (FuncSave())
				{
					editModeFlag = EnumEditModeFlag.X;
					if (IsUseEnable)
					{
						InitialToolButtonEnable(editModeFlag);
					}
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected virtual bool FuncSave()
		{
			return true;
		}

		private void tsbDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (Permit(enumPermission.CanDelete, true) && FuncDelete())
				{
					editModeFlag = EnumEditModeFlag.D;
					if (IsUseEnable)
					{
						InitialToolButtonEnable(editModeFlag);
					}
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected virtual bool CkeckDelete()
		{
			return true;
		}

		protected virtual bool FuncDelete()
		{
			return true;
		}

		private void tsbCancel_Click(object sender, EventArgs e)
		{
			try
			{
				FuncCancel();
				editModeFlag = EnumEditModeFlag.X;
				if (IsUseEnable)
				{
					InitialToolButtonEnable(editModeFlag);
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected virtual void FuncCancel()
		{
		}

		private void tsbPrint_Click(object sender, EventArgs e)
		{
			try
			{
				if (Permit(enumPermission.CanPrint, true))
				{
					FuncPrint();
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected virtual void FuncPrint()
		{
		}

		private void tsbClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void InitialToolButtonEnable(EnumEditModeFlag editModeFlag)
		{
			switch (editModeFlag)
			{
			case EnumEditModeFlag.A:
				SetMenuEnabled(false, false, false, false, true, true, true, true, false);
				break;
			case EnumEditModeFlag.E:
				SetMenuEnabled(false, false, false, false, true, true, true, true, false);
				break;
			case EnumEditModeFlag.D:
				SetMenuEnabled(false, false, true, true, true, true, false, true, false);
				break;
			case EnumEditModeFlag.Q:
				SetMenuEnabled(false, false, true, true, true, false, false, true, false);
				break;
			case EnumEditModeFlag.S:
				SetMenuEnabled(true, true, false, false, false, false, false, true, true);
				break;
			default:
				SetMenuEnabled(true, true, true, true, true, true, true, true, true);
				break;
			}
		}

		protected bool PermissionUse(EnumEditModeFlag editModeFlag, enumPermission enumPermiss, bool isUseEnable)
		{
			base.editModeFlag = editModeFlag;
			if (isUseEnable)
			{
				InitialToolButtonEnable(editModeFlag);
			}
			if (base.permitGroup == null)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("NoPermit"));
				return false;
			}
			bool flag = false;
			switch (enumPermiss)
			{
			case enumPermission.CanAdd:
				flag = base.permitGroup.CanAdd;
				break;
			case enumPermission.CanEdit:
				flag = base.permitGroup.CanEdit;
				break;
			case enumPermission.CanDelete:
				flag = base.permitGroup.CanDelete;
				break;
			case enumPermission.CanPrint:
				flag = base.permitGroup.CanPrint;
				break;
			case enumPermission.CanQuery:
				flag = base.permitGroup.CanQuery;
				break;
			}
			if (!flag)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("NoPermit"));
				return false;
			}
			return true;
		}

		private void FormToolBase_Load(object sender, EventArgs e)
		{
		}

		protected override void FormShown()
		{
			SysFunction sysFunction = new SysFunction(_FunctionName);
			ContentText = sysFunction.LocationFunctionName;
		}
	}
}
