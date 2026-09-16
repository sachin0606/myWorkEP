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
using DeXin.EPG.Tools.EPGUserControl;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormBATManageDetail : FormDetailBase
	{
		private NitInfo CurrNIT;

		private BATInfo CurrBAT;

		private BATInfoManager batManager = new BATInfoManager();

		private BATToProgramManager batToPrgManager = new BATToProgramManager();

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;

		private TextBox txtNIT;

		private NumTextBox txtBATId;

		private NumTextBox txtVersion;

		private ValidatTextBox txtBATMame;

		private GroupBox groupBox1;

		private TreeView tvPrograms;

		private ContextMenuStrip cmsBATPrgs;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private FlowLayoutPanel flowLayoutPanel1;

		private CheckBox ckVerBAT;

		private TableLayoutPanel tableLayoutPanel2;

		private CheckBox ckEditServiceList;

		private FormBATManageDetail()
		{
			InitializeComponent();
		}

		public FormBATManageDetail(NitInfo NIT)
		{
			InitializeComponent();
			CurrNIT = NIT;
			CurrBAT = new BATInfo();
			CurrBAT.NitTableId = NIT.Identity;
			base.PageFlag = PageEnum.Add;
		}

		public FormBATManageDetail(BATInfo BAT, PageEnum DoModel)
		{
			InitializeComponent();
			CurrNIT = BAT.NIT;
			CurrBAT = BAT;
			base.PageFlag = DoModel;
		}

		private void FormBATManageDetail_Load(object sender, EventArgs e)
		{
			IList<ProgramInfo> prgList = new List<ProgramInfo>();
			if (base.PageFlag == PageEnum.Add)
			{
				txtNIT.Text = CurrNIT.ToString();
			}
			else
			{
				txtNIT.Text = CurrNIT.ToString();
				txtBATId.Text = CurrBAT.BATId.ToString("X4");
				txtBATMame.Text = CurrBAT.TableName;
				if (CurrBAT.BATVersionNumber < 0)
				{
					txtVersion.Text = "00";
					ckVerBAT.Checked = true;
				}
				else
				{
					txtVersion.Text = CurrBAT.BATVersionNumber.ToString("X2");
					ckVerBAT.Checked = false;
				}
				prgList = batToPrgManager.GetByBAT(CurrBAT.Identity).ProgramList;
			}
			tvPrograms.AfterCheck += delegate(object sendera, TreeViewEventArgs ea)
			{
				Tool.SetNodeChecked(ea.Node, true);
			};
			TreeNode treeNode = new TreeNode(CurrNIT.NetworkName);
			treeNode.Tag = CurrNIT;
			TreeNode treeNode2 = treeNode;
			LoadeTreeNodes(treeNode2, prgList);
			tvPrograms.Nodes.Clear();
			tvPrograms.Nodes.Add(treeNode2);
			tvPrograms.ExpandAll();
			switch (base.PageFlag)
			{
			case PageEnum.Add:
				break;
			case PageEnum.Edit:
				break;
			case PageEnum.Show:
				foreach (Control control in tableLayoutPanel1.Controls)
				{
					if (control is TextBoxBase)
					{
						((TextBoxBase)control).ReadOnly = true;
					}
				}
				tvPrograms.Enabled = false;
				break;
			}
		}

		private void LoadeTreeNodes(TreeNode parentNode, IList<ProgramInfo> prgList)
		{
			parentNode.Nodes.Clear();
			if (parentNode.Tag is NitInfo)
			{
				NitInfo nitInfo = parentNode.Tag as NitInfo;
				IList<TsInfo> tSInfoByNetWorkIdentity = new TSInfoManager().GetTSInfoByNetWorkIdentity(nitInfo.Identity);
				{
					foreach (TsInfo item in tSInfoByNetWorkIdentity)
					{
						TreeNode treeNode = new TreeNode();
						treeNode.Text = item.BKValue;
						treeNode.Tag = item;
						parentNode.Nodes.Add(treeNode);
						LoadeTreeNodes(treeNode, prgList);
					}
					return;
				}
			}
			if (parentNode.Tag is TsInfo)
			{
				TsInfo tsInfo = parentNode.Tag as TsInfo;
				IList<ProgramInfo> byTSIdentity = new ProgramInfoManager().GetByTSIdentity(tsInfo.Identity);
				{
					foreach (ProgramInfo item2 in byTSIdentity)
					{
						TreeNode treeNode = new TreeNode();
						if (prgList.Select((ProgramInfo p) => p.Identity).Contains(item2.Identity))
						{
							treeNode.Checked = true;
						}
						else
						{
							treeNode.Checked = false;
						}
						treeNode.Text = item2.BKValue;
						treeNode.Tag = item2;
						parentNode.Nodes.Add(treeNode);
					}
					return;
				}
			}
			if (!(parentNode.Tag is ProgramInfo))
			{
			}
		}

		protected override bool FunSubmit()
		{
			if (!ValidateInput(tableLayoutPanel1))
			{
				return false;
			}
			CurrBAT.BATId = txtBATId.ValueOfInt32;
			CurrBAT.BATName = txtBATMame.Text;
			if (ckVerBAT.Checked)
			{
				CurrBAT.BATVersionNumber = -1;
			}
			else
			{
				CurrBAT.BATVersionNumber = txtVersion.ValueOfInt32;
			}
			BATToProgram bATToProgram = new BATToProgram();
			bATToProgram.BAT = CurrBAT;
			if (ckEditServiceList.Checked)
			{
				bATToProgram.ProgramList = new List<ProgramInfo>();
				IList<TreeNode> checkedNodes = Tool.GetCheckedNodes(tvPrograms.Nodes, true);
				foreach (TreeNode item in checkedNodes)
				{
					if (item.Tag is ProgramInfo)
					{
						bATToProgram.ProgramList.Add(item.Tag as ProgramInfo);
					}
				}
			}
			batManager.Save(bATToProgram, ckEditServiceList.Checked);
			base.DialogResult = DialogResult.OK;
			return true;
		}

		protected override void FunCancel()
		{
			base.FunCancel();
			base.DialogResult = DialogResult.Cancel;
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in tvPrograms.Nodes)
			{
				node.Checked = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in tvPrograms.Nodes)
			{
				node.Checked = false;
			}
		}

		private void ckVerBAT_CheckedChanged(object sender, EventArgs e)
		{
			txtVersion.Enabled = !ckVerBAT.Checked;
			if (ckVerBAT.Checked)
			{
				txtVersion.Text = "00";
			}
			else if (CurrBAT != null && CurrBAT.BATVersionNumber > 0)
			{
				txtVersion.Text = CurrBAT.BATVersionNumber.ToString("X2");
			}
		}

		private void ckEditServiceList_CheckedChanged(object sender, EventArgs e)
		{
			tvPrograms.Enabled = ckEditServiceList.Checked;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormBATManageDetail));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtNIT = new System.Windows.Forms.TextBox();
			this.txtBATId = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.txtBATMame = new DeXin.EPG.Tools.EPGUserControl.ValidatTextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtVersion = new DeXin.EPG.Tools.EPGUserControl.NumTextBox();
			this.ckVerBAT = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.tvPrograms = new System.Windows.Forms.TreeView();
			this.cmsBATPrgs = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.ckEditServiceList = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.cmsBATPrgs.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.groupBox1);
			base.grbText.Controls.Add(this.tableLayoutPanel1);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtNIT, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtBATId, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtBATMame, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			resources.ApplyResources(this.txtNIT, "txtNIT");
			this.txtNIT.Name = "txtNIT";
			this.txtNIT.ReadOnly = true;
			resources.ApplyResources(this.txtBATId, "txtBATId");
			this.txtBATId.ForeColor = System.Drawing.Color.Black;
			this.txtBATId.IsValidated = true;
			this.txtBATId.MaxValue = 65535;
			this.txtBATId.Name = "txtBATId";
			this.txtBATId.NotNull = true;
			this.txtBATId.SetMaxLength = 2;
			this.txtBATId.Value = new byte[0];
			resources.ApplyResources(this.txtBATMame, "txtBATMame");
			this.txtBATMame.IsValidated = true;
			this.txtBATMame.Name = "txtBATMame";
			this.txtBATMame.NotNull = true;
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this.txtVersion);
			this.flowLayoutPanel1.Controls.Add(this.ckVerBAT);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.txtVersion, "txtVersion");
			this.txtVersion.ForeColor = System.Drawing.Color.Black;
			this.txtVersion.IsValidated = true;
			this.txtVersion.MaxValue = 31;
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.NotNull = true;
			this.txtVersion.SetMaxLength = 1;
			DeXin.EPG.Tools.EPGUserControl.NumTextBox numTextBox = this.txtVersion;
			byte[] value = new byte[1];
			numTextBox.Value = value;
			resources.ApplyResources(this.ckVerBAT, "ckVerBAT");
			this.ckVerBAT.Name = "ckVerBAT";
			this.ckVerBAT.UseVisualStyleBackColor = true;
			this.ckVerBAT.CheckedChanged += new System.EventHandler(ckVerBAT_CheckedChanged);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.tvPrograms, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.ckEditServiceList, 0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tvPrograms.CheckBoxes = true;
			this.tvPrograms.ContextMenuStrip = this.cmsBATPrgs;
			resources.ApplyResources(this.tvPrograms, "tvPrograms");
			this.tvPrograms.Name = "tvPrograms";
			this.cmsBATPrgs.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmSelectAll, this.tsmDeselectAll });
			this.cmsBATPrgs.Name = "cmsBATPrgs";
			resources.ApplyResources(this.cmsBATPrgs, "cmsBATPrgs");
			this.tsmSelectAll.Name = "tsmSelectAll";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			resources.ApplyResources(this.ckEditServiceList, "ckEditServiceList");
			this.ckEditServiceList.Name = "ckEditServiceList";
			this.ckEditServiceList.UseVisualStyleBackColor = true;
			this.ckEditServiceList.CheckedChanged += new System.EventHandler(ckEditServiceList_CheckedChanged);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "FormBATManageDetail";
			base.Load += new System.EventHandler(FormBATManageDetail_Load);
			base.Controls.SetChildIndex(base.grbText, 0);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.cmsBATPrgs.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
