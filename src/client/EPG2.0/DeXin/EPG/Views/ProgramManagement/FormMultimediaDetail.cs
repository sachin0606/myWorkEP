using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormMultimediaDetail : FormDetailBase
	{
		private ProgramInfo CurrPrg;

		private Multimedia CurrMedia;

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblNIT;

		private Label lblOrgNIT;

		private NumTextBox txtPESId;

		private ComboBox cbxPESType;

		public FormMultimediaDetail(ProgramInfo Program)
		{
			InitializeComponent();
			CurrPrg = Program;
			CurrMedia = new Multimedia
			{
				ProgramTableId = Program.Identity,
				Program = Program
			};
			base.PageFlag = PageEnum.Add;
		}

		public FormMultimediaDetail(Multimedia Media, PageEnum DoModel)
		{
			InitializeComponent();
			CurrMedia = Media;
			base.PageFlag = DoModel;
		}

		private void FormMultimediaDetail_Load(object sender, EventArgs e)
		{
			IList<BaseKey> lst = ((IEnumerable<BaseKey>)new StreamTypeInfoManager().GetAll()).ToList();
			Tool.BindingComboBox(lst, cbxPESType);
			if (base.PageFlag == PageEnum.Show || base.PageFlag == PageEnum.Edit)
			{
				txtPESId.Text = CurrMedia.PesPid.ToString("X4");
				cbxPESType.SelectedValue = CurrMedia.StreamTypeTableId.ToString();
				if (base.PageFlag == PageEnum.Show)
				{
					txtPESId.ReadOnly = true;
					cbxPESType.Enabled = false;
				}
			}
			_FunctionName = EnumFunctionName.SysFuncMultimediaManage;
		}

		protected override bool FunSubmit()
		{
			if (!ValidateInput())
			{
				return false;
			}
			CurrMedia.PesPid = txtPESId.ValueOfInt32;
			CurrMedia.StreamTypeTableId = Tool.GetInt64(Tool.GetValue(cbxPESType));
			if (new MultimediaManager().Save(CurrMedia) == 1)
			{
				base.DialogResult = DialogResult.OK;
				return true;
			}
			return false;
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			base.DialogResult = DialogResult.Cancel;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormMultimediaDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblNIT = new System.Windows.Forms.Label();
			this.lblOrgNIT = new System.Windows.Forms.Label();
			this.txtPESId = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.cbxPESType = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.lblNIT, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblOrgNIT, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtPESId, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cbxPESType, 1, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.lblNIT, "lblNIT");
			this.lblNIT.Name = "lblNIT";
			resources.ApplyResources(this.lblOrgNIT, "lblOrgNIT");
			this.lblOrgNIT.Name = "lblOrgNIT";
			resources.ApplyResources(this.txtPESId, "txtPESId");
			this.txtPESId.ForeColor = System.Drawing.Color.Black;
			this.txtPESId.IsValidated = true;
			this.txtPESId.MaxValue = 65535;
			this.txtPESId.Name = "txtPESId";
			this.txtPESId.NotNull = true;
			this.txtPESId.SetMaxLength = 2;
			this.txtPESId.Value = new byte[0];
			this.cbxPESType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPESType.FormattingEnabled = true;
			resources.ApplyResources(this.cbxPESType, "cbxPESType");
			this.cbxPESType.Name = "cbxPESType";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormMultimediaDetail";
			base.Load += new System.EventHandler(FormMultimediaDetail_Load);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			base.grbText.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
