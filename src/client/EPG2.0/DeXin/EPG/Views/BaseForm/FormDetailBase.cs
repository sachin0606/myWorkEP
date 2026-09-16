using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.Model;

namespace DeXin.EPG.Views.BaseForm
{
	public class FormDetailBase : FormBase
	{
		private IContainer components = null;

		private ToolStripButton tsbCancel;

		private ToolStripButton tsbSubmit;

		protected ToolStrip topTSMenu;

		protected ErrorProvider error;

		private Button btnSubmit;

		private Button btnCancel;

		private GroupBox grbButton;

		private TableLayoutPanel tableLayoutPanel1;

		protected GroupBox grbText;

		private PageEnum _PageFlag = PageEnum.Empty;

		protected PageEnum PageFlag
		{
			get
			{
				return _PageFlag;
			}
			set
			{
				_PageFlag = value;
			}
		}

		protected string ContentText
		{
			get
			{
				return grbText.Text;
			}
			set
			{
				grbText.Text = value;
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.BaseForm.FormDetailBase));
			this.topTSMenu = new System.Windows.Forms.ToolStrip();
			this.tsbSubmit = new System.Windows.Forms.ToolStripButton();
			this.tsbCancel = new System.Windows.Forms.ToolStripButton();
			this.error = new System.Windows.Forms.ErrorProvider(this.components);
			this.btnSubmit = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grbButton = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.grbText = new System.Windows.Forms.GroupBox();
			this.topTSMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.error).BeginInit();
			this.grbButton.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.topTSMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsbSubmit, this.tsbCancel });
			resources.ApplyResources(this.topTSMenu, "topTSMenu");
			this.topTSMenu.Name = "topTSMenu";
			this.tsbSubmit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			resources.ApplyResources(this.tsbSubmit, "tsbSubmit");
			this.tsbSubmit.Name = "tsbSubmit";
			this.tsbSubmit.Click += new System.EventHandler(tsbSubmit_Click);
			this.tsbCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			resources.ApplyResources(this.tsbCancel, "tsbCancel");
			this.tsbCancel.Name = "tsbCancel";
			this.tsbCancel.Click += new System.EventHandler(tsbCancel_Click);
			this.error.ContainerControl = this;
			resources.ApplyResources(this.btnSubmit, "btnSubmit");
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Click += new System.EventHandler(tsbSubmit_Click);
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(tsbCancel_Click);
			this.grbButton.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(this.grbButton, "grbButton");
			this.grbButton.Name = "grbButton";
			this.grbButton.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnSubmit, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.grbText, "grbText");
			this.grbText.Name = "grbText";
			this.grbText.TabStop = false;
			base.AcceptButton = this.btnSubmit;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.grbText);
			base.Controls.Add(this.grbButton);
			base.Controls.Add(this.topTSMenu);
			base.Name = "FormDetailBase";
			base.ShowInTaskbar = false;
			this.topTSMenu.ResumeLayout(false);
			this.topTSMenu.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.error).EndInit();
			this.grbButton.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormDetailBase()
		{
			InitializeComponent();
			grbText.Text = "";
		}

		private void FormDetailBase_Load(object sender, EventArgs e)
		{
		}

		private void tsbSubmit_Click(object sender, EventArgs e)
		{
			try
			{
				if (FunSubmit())
				{
					PageFlag = PageEnum.Show;
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void tsbCancel_Click(object sender, EventArgs e)
		{
			try
			{
				FunCancel();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected virtual bool FunSubmit()
		{
			return true;
		}

		protected virtual void FunCancel()
		{
		}

		protected override void FormShown()
		{
			SysFunction sysFunction = new SysFunction(base.FunctionName);
			ContentText = sysFunction.LocationFunctionName;
			switch (PageFlag)
			{
			case PageEnum.Add:
			case PageEnum.Edit:
			{
				ToolStripButton toolStripButton = tsbSubmit;
				bool enabled = (tsbCancel.Enabled = true);
				toolStripButton.Enabled = enabled;
				break;
			}
			default:
				topTSMenu.Enabled = false;
				grbButton.Enabled = false;
				break;
			}
		}
	}
}
