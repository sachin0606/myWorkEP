using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormTS : FormDetailBase
	{
		private NitInfo nit;

		private TsInfo ts;

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblTSName;

		private Label lblTSId;

		private Label lblVersion;

		private Label lblNIT;

		private Label lblOrgNIT;

		private TextBox txtNIT;

		private NumTextBox txtOrgNIT;

		private NumTextBox txtTSId;

		private NumTextBox txtPATVersion;

		private ValidatTextBox txtTSName;

		private Label label1;

		private Label label2;

		private NumTextBox txtCATVersion;

		private NumTextBox txtSDTVersion;

		private GroupBox gbxAdvance;

		private TableLayoutPanel tableLayoutPanel2;

		private Button btAdvance;

		private CheckBox ckVerPat;

		private CheckBox ckVerCat;

		private CheckBox ckVerSdt;

		public FormTS(NitInfo NIT)
		{
			InitializeComponent();
			nit = NIT;
			base.PageFlag = PageEnum.Add;
		}

		public FormTS(TsInfo TS)
		{
			InitializeComponent();
			ts = TS;
			base.PageFlag = PageEnum.Edit;
		}

		private void FormTS_Load(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncTSInfo;
			if (nit != null)
			{
				txtNIT.Text = nit.ToString();
				txtOrgNIT.Text = nit.NetworkId.ToString("X4");
				ts = new TsInfo();
				ts.NitTableId = nit.Identity;
			}
			else if (ts != null)
			{
				txtNIT.Text = ts.NIT.ToString();
				txtOrgNIT.Text = ts.OriginalNetworkId.ToString("X4");
				txtTSId.Text = ts.TsId.ToString("X4");
				txtTSName.Text = ts.TsName;
				if (ts.PatVersionNumber < 0)
				{
					txtPATVersion.Text = "00";
					ckVerPat.Checked = true;
				}
				else
				{
					txtPATVersion.Text = ts.PatVersionNumber.ToString("X2");
					ckVerPat.Checked = false;
				}
				if (ts.CatVersionNumber < 0)
				{
					txtCATVersion.Text = "00";
					ckVerCat.Checked = true;
				}
				else
				{
					txtCATVersion.Text = ts.CatVersionNumber.ToString("X2");
					ckVerCat.Checked = false;
				}
				if (ts.SdtVersionNumber < 0)
				{
					txtSDTVersion.Text = "00";
					ckVerSdt.Checked = true;
				}
				else
				{
					txtSDTVersion.Text = ts.SdtVersionNumber.ToString("X2");
					ckVerSdt.Checked = false;
				}
			}
			btAdvance_Click(sender, e);
		}

		protected override bool FunSubmit()
		{
			if (!ValidateInput())
			{
				return false;
			}
			TSInfoManager tSInfoManager = new TSInfoManager();
			ts.OriginalNetworkId = txtOrgNIT.ValueOfInt32;
			ts.TsId = txtTSId.ValueOfInt32;
			ts.TsName = txtTSName.Text;
			if (ckVerPat.Checked)
			{
				ts.PatVersionNumber = -1;
			}
			else
			{
				ts.PatVersionNumber = txtPATVersion.ValueOfInt32;
			}
			if (ckVerCat.Checked)
			{
				ts.CatVersionNumber = -1;
			}
			else
			{
				ts.CatVersionNumber = txtCATVersion.ValueOfInt32;
			}
			if (ckVerSdt.Checked)
			{
				ts.SdtVersionNumber = -1;
			}
			else
			{
				ts.SdtVersionNumber = txtSDTVersion.ValueOfInt32;
			}
			try
			{
				if (tSInfoManager.SaveTSInfo(ts) == 1)
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveSucceed]);
					base.DialogResult = DialogResult.OK;
					return true;
				}
				Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.SaveFailure]);
				return false;
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex.Message);
				txtTSId.Focus();
				txtTSId.SelectAll();
				return false;
			}
		}

		protected override void FunCancel()
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void btAdvance_Click(object sender, EventArgs e)
		{
			if (gbxAdvance.Visible)
			{
				gbxAdvance.Hide();
				base.Height -= gbxAdvance.Height;
				btAdvance.Text = ResourceManagement.GetLocationString("ShowAdancedProperty");
			}
			else
			{
				base.Height += gbxAdvance.Height;
				gbxAdvance.Show();
				btAdvance.Text = ResourceManagement.GetLocationString("HideAdancedProperty");
			}
		}

		private void ckVerPat_CheckedChanged(object sender, EventArgs e)
		{
			txtPATVersion.Enabled = !ckVerPat.Checked;
			if (ckVerPat.Checked)
			{
				txtPATVersion.Text = "00";
			}
			else if (ts != null && ts.PatVersionNumber > 0)
			{
				txtPATVersion.Text = ts.PatVersionNumber.ToString("X2");
			}
		}

		private void ckVerCat_CheckedChanged(object sender, EventArgs e)
		{
			txtCATVersion.Enabled = !ckVerCat.Checked;
			if (ckVerCat.Checked)
			{
				txtCATVersion.Text = "00";
			}
			else if (ts != null && ts.CatVersionNumber > 0)
			{
				txtCATVersion.Text = ts.CatVersionNumber.ToString("X2");
			}
		}

		private void ckVerSdt_CheckedChanged(object sender, EventArgs e)
		{
			txtSDTVersion.Enabled = !ckVerSdt.Checked;
			if (ckVerSdt.Checked)
			{
				txtSDTVersion.Text = "00";
			}
			else if (ts != null && ts.SdtVersionNumber > 0)
			{
				txtSDTVersion.Text = ts.SdtVersionNumber.ToString("X2");
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormTS));
			this.lblTSName = new System.Windows.Forms.Label();
			this.lblTSId = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblNIT = new System.Windows.Forms.Label();
			this.lblOrgNIT = new System.Windows.Forms.Label();
			this.txtNIT = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtOrgNIT = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtTSId = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtTSName = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.gbxAdvance = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.txtPATVersion = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCATVersion = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtSDTVersion = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.ckVerPat = new System.Windows.Forms.CheckBox();
			this.ckVerCat = new System.Windows.Forms.CheckBox();
			this.ckVerSdt = new System.Windows.Forms.CheckBox();
			this.btAdvance = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.gbxAdvance.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.error, "error");
			resources.ApplyResources(base.grbText, "grbText");
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			base.error.SetError(base.grbText, resources.GetString("grbText.Error"));
			base.error.SetIconAlignment(base.grbText, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("grbText.IconAlignment"));
			base.error.SetIconPadding(base.grbText, (int)resources.GetObject("grbText.IconPadding"));
			resources.ApplyResources(this.lblTSName, "lblTSName");
			base.error.SetError(this.lblTSName, resources.GetString("lblTSName.Error"));
			base.error.SetIconAlignment(this.lblTSName, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblTSName.IconAlignment"));
			base.error.SetIconPadding(this.lblTSName, (int)resources.GetObject("lblTSName.IconPadding"));
			this.lblTSName.Name = "lblTSName";
			resources.ApplyResources(this.lblTSId, "lblTSId");
			base.error.SetError(this.lblTSId, resources.GetString("lblTSId.Error"));
			base.error.SetIconAlignment(this.lblTSId, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblTSId.IconAlignment"));
			base.error.SetIconPadding(this.lblTSId, (int)resources.GetObject("lblTSId.IconPadding"));
			this.lblTSId.Name = "lblTSId";
			resources.ApplyResources(this.lblVersion, "lblVersion");
			base.error.SetError(this.lblVersion, resources.GetString("lblVersion.Error"));
			base.error.SetIconAlignment(this.lblVersion, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblVersion.IconAlignment"));
			base.error.SetIconPadding(this.lblVersion, (int)resources.GetObject("lblVersion.IconPadding"));
			this.lblVersion.Name = "lblVersion";
			resources.ApplyResources(this.lblNIT, "lblNIT");
			base.error.SetError(this.lblNIT, resources.GetString("lblNIT.Error"));
			base.error.SetIconAlignment(this.lblNIT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblNIT.IconAlignment"));
			base.error.SetIconPadding(this.lblNIT, (int)resources.GetObject("lblNIT.IconPadding"));
			this.lblNIT.Name = "lblNIT";
			resources.ApplyResources(this.lblOrgNIT, "lblOrgNIT");
			base.error.SetError(this.lblOrgNIT, resources.GetString("lblOrgNIT.Error"));
			base.error.SetIconAlignment(this.lblOrgNIT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("lblOrgNIT.IconAlignment"));
			base.error.SetIconPadding(this.lblOrgNIT, (int)resources.GetObject("lblOrgNIT.IconPadding"));
			this.lblOrgNIT.Name = "lblOrgNIT";
			resources.ApplyResources(this.txtNIT, "txtNIT");
			base.error.SetError(this.txtNIT, resources.GetString("txtNIT.Error"));
			base.error.SetIconAlignment(this.txtNIT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtNIT.IconAlignment"));
			base.error.SetIconPadding(this.txtNIT, (int)resources.GetObject("txtNIT.IconPadding"));
			this.txtNIT.Name = "txtNIT";
			this.txtNIT.ReadOnly = true;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblTSName, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblTSId, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblNIT, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblOrgNIT, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtNIT, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtOrgNIT, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtTSId, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtTSName, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.gbxAdvance, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.btAdvance, 0, 4);
			base.error.SetError(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.Error"));
			base.error.SetIconAlignment(this.tableLayoutPanel1, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("tableLayoutPanel1.IconAlignment"));
			base.error.SetIconPadding(this.tableLayoutPanel1, (int)resources.GetObject("tableLayoutPanel1.IconPadding"));
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.txtOrgNIT, "txtOrgNIT");
			base.error.SetError(this.txtOrgNIT, resources.GetString("txtOrgNIT.Error"));
			this.txtOrgNIT.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtOrgNIT, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtOrgNIT.IconAlignment"));
			base.error.SetIconPadding(this.txtOrgNIT, (int)resources.GetObject("txtOrgNIT.IconPadding"));
			this.txtOrgNIT.IsValidated = true;
			this.txtOrgNIT.MaxValue = 65535;
			this.txtOrgNIT.Name = "txtOrgNIT";
			this.txtOrgNIT.NotNull = true;
			this.txtOrgNIT.SetMaxLength = 2;
			this.txtOrgNIT.Value = new byte[0];
			resources.ApplyResources(this.txtTSId, "txtTSId");
			base.error.SetError(this.txtTSId, resources.GetString("txtTSId.Error"));
			this.txtTSId.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtTSId, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtTSId.IconAlignment"));
			base.error.SetIconPadding(this.txtTSId, (int)resources.GetObject("txtTSId.IconPadding"));
			this.txtTSId.IsValidated = true;
			this.txtTSId.MaxValue = 65535;
			this.txtTSId.Name = "txtTSId";
			this.txtTSId.NotNull = true;
			this.txtTSId.SetMaxLength = 2;
			this.txtTSId.Value = new byte[0];
			resources.ApplyResources(this.txtTSName, "txtTSName");
			base.error.SetError(this.txtTSName, resources.GetString("txtTSName.Error"));
			base.error.SetIconAlignment(this.txtTSName, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtTSName.IconAlignment"));
			base.error.SetIconPadding(this.txtTSName, (int)resources.GetObject("txtTSName.IconPadding"));
			this.txtTSName.IsValidated = true;
			this.txtTSName.Name = "txtTSName";
			this.txtTSName.NotNull = true;
			resources.ApplyResources(this.gbxAdvance, "gbxAdvance");
			this.tableLayoutPanel1.SetColumnSpan(this.gbxAdvance, 2);
			this.gbxAdvance.Controls.Add(this.tableLayoutPanel2);
			base.error.SetError(this.gbxAdvance, resources.GetString("gbxAdvance.Error"));
			base.error.SetIconAlignment(this.gbxAdvance, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("gbxAdvance.IconAlignment"));
			base.error.SetIconPadding(this.gbxAdvance, (int)resources.GetObject("gbxAdvance.IconPadding"));
			this.gbxAdvance.Name = "gbxAdvance";
			this.gbxAdvance.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.lblVersion, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.txtPATVersion, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.txtCATVersion, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.txtSDTVersion, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.ckVerPat, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.ckVerCat, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.ckVerSdt, 2, 2);
			base.error.SetError(this.tableLayoutPanel2, resources.GetString("tableLayoutPanel2.Error"));
			base.error.SetIconAlignment(this.tableLayoutPanel2, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("tableLayoutPanel2.IconAlignment"));
			base.error.SetIconPadding(this.tableLayoutPanel2, (int)resources.GetObject("tableLayoutPanel2.IconPadding"));
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.txtPATVersion, "txtPATVersion");
			base.error.SetError(this.txtPATVersion, resources.GetString("txtPATVersion.Error"));
			this.txtPATVersion.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtPATVersion, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtPATVersion.IconAlignment"));
			base.error.SetIconPadding(this.txtPATVersion, (int)resources.GetObject("txtPATVersion.IconPadding"));
			this.txtPATVersion.IsValidated = true;
			this.txtPATVersion.MaxValue = 31;
			this.txtPATVersion.Name = "txtPATVersion";
			this.txtPATVersion.NotNull = true;
			this.txtPATVersion.SetMaxLength = 1;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox = this.txtPATVersion;
			byte[] value = new byte[1];
			numTextBox.Value = value;
			resources.ApplyResources(this.label1, "label1");
			base.error.SetError(this.label1, resources.GetString("label1.Error"));
			base.error.SetIconAlignment(this.label1, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("label1.IconAlignment"));
			base.error.SetIconPadding(this.label1, (int)resources.GetObject("label1.IconPadding"));
			this.label1.Name = "label1";
			resources.ApplyResources(this.txtCATVersion, "txtCATVersion");
			base.error.SetError(this.txtCATVersion, resources.GetString("txtCATVersion.Error"));
			this.txtCATVersion.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtCATVersion, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtCATVersion.IconAlignment"));
			base.error.SetIconPadding(this.txtCATVersion, (int)resources.GetObject("txtCATVersion.IconPadding"));
			this.txtCATVersion.IsValidated = true;
			this.txtCATVersion.MaxValue = 31;
			this.txtCATVersion.Name = "txtCATVersion";
			this.txtCATVersion.NotNull = true;
			this.txtCATVersion.SetMaxLength = 1;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox2 = this.txtCATVersion;
			value = new byte[1];
			numTextBox2.Value = value;
			resources.ApplyResources(this.label2, "label2");
			base.error.SetError(this.label2, resources.GetString("label2.Error"));
			base.error.SetIconAlignment(this.label2, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("label2.IconAlignment"));
			base.error.SetIconPadding(this.label2, (int)resources.GetObject("label2.IconPadding"));
			this.label2.Name = "label2";
			resources.ApplyResources(this.txtSDTVersion, "txtSDTVersion");
			base.error.SetError(this.txtSDTVersion, resources.GetString("txtSDTVersion.Error"));
			this.txtSDTVersion.ForeColor = System.Drawing.Color.Black;
			base.error.SetIconAlignment(this.txtSDTVersion, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("txtSDTVersion.IconAlignment"));
			base.error.SetIconPadding(this.txtSDTVersion, (int)resources.GetObject("txtSDTVersion.IconPadding"));
			this.txtSDTVersion.IsValidated = true;
			this.txtSDTVersion.MaxValue = 31;
			this.txtSDTVersion.Name = "txtSDTVersion";
			this.txtSDTVersion.NotNull = true;
			this.txtSDTVersion.SetMaxLength = 1;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox3 = this.txtSDTVersion;
			value = new byte[1];
			numTextBox3.Value = value;
			resources.ApplyResources(this.ckVerPat, "ckVerPat");
			this.ckVerPat.Checked = true;
			this.ckVerPat.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckVerPat, resources.GetString("ckVerPat.Error"));
			base.error.SetIconAlignment(this.ckVerPat, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckVerPat.IconAlignment"));
			base.error.SetIconPadding(this.ckVerPat, (int)resources.GetObject("ckVerPat.IconPadding"));
			this.ckVerPat.Name = "ckVerPat";
			this.ckVerPat.UseVisualStyleBackColor = true;
			this.ckVerPat.CheckedChanged += new System.EventHandler(ckVerPat_CheckedChanged);
			resources.ApplyResources(this.ckVerCat, "ckVerCat");
			this.ckVerCat.Checked = true;
			this.ckVerCat.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckVerCat, resources.GetString("ckVerCat.Error"));
			base.error.SetIconAlignment(this.ckVerCat, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckVerCat.IconAlignment"));
			base.error.SetIconPadding(this.ckVerCat, (int)resources.GetObject("ckVerCat.IconPadding"));
			this.ckVerCat.Name = "ckVerCat";
			this.ckVerCat.UseVisualStyleBackColor = true;
			this.ckVerCat.CheckedChanged += new System.EventHandler(ckVerCat_CheckedChanged);
			resources.ApplyResources(this.ckVerSdt, "ckVerSdt");
			this.ckVerSdt.Checked = true;
			this.ckVerSdt.CheckState = System.Windows.Forms.CheckState.Checked;
			base.error.SetError(this.ckVerSdt, resources.GetString("ckVerSdt.Error"));
			base.error.SetIconAlignment(this.ckVerSdt, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("ckVerSdt.IconAlignment"));
			base.error.SetIconPadding(this.ckVerSdt, (int)resources.GetObject("ckVerSdt.IconPadding"));
			this.ckVerSdt.Name = "ckVerSdt";
			this.ckVerSdt.UseVisualStyleBackColor = true;
			this.ckVerSdt.CheckedChanged += new System.EventHandler(ckVerSdt_CheckedChanged);
			resources.ApplyResources(this.btAdvance, "btAdvance");
			base.error.SetError(this.btAdvance, resources.GetString("btAdvance.Error"));
			base.error.SetIconAlignment(this.btAdvance, (System.Windows.Forms.ErrorIconAlignment)resources.GetObject("btAdvance.IconAlignment"));
			base.error.SetIconPadding(this.btAdvance, (int)resources.GetObject("btAdvance.IconPadding"));
			this.btAdvance.Name = "btAdvance";
			this.btAdvance.UseVisualStyleBackColor = true;
			this.btAdvance.Click += new System.EventHandler(btAdvance_Click);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormTS";
			base.Load += new System.EventHandler(FormTS_Load);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			base.grbText.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.gbxAdvance.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
