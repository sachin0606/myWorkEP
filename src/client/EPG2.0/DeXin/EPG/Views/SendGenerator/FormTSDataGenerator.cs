using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.SendGenerator
{
	public class FormTSDataGenerator : FormToolBase
	{
		private IContainer components = null;

		private SplitContainer splitContainer1;

		private TableLayoutPanel tableLayoutPanel2;

		private Label label1;

		private TreeView tvController;

		private TableLayoutPanel tableLayoutPanel3;

		private Label label2;

		private TreeView tvResult;

		private ContextMenuStrip cmsSendPrg;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private ToolStripMenuItem tsmCollapseAll;

		private ToolStripMenuItem tsmExpandAll;

		public FormTSDataGenerator()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncSendGenerator;
			tsbAdd.Text = ResourceManagement.GetLocationString("Send");
			tsbEdit.Text = ResourceManagement.GetLocationString("UpdateSendingData");
			SetMenuVisible(false, true, true, false, false, false, false, true, false);
			tvController.AfterCheck += delegate(object sendera, TreeViewEventArgs ea)
			{
				Tool.SetNodeChecked(ea.Node, true);
			};
			tvResult.BeforeCheck += delegate(object senderb, TreeViewCancelEventArgs eb)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("OperateInSendingArea"));
				eb.Cancel = true;
			};
		}

		private void FormTSDataGenerator_Load(object sender, EventArgs e)
		{
			IList<long> keyList = (from p in new PATManager().GetAll()
				select p.ProgramTableId).ToList();
			TreeNode treeNode = new TreeNode("EPG");
			LoadeTreeNodes(treeNode, keyList, true);
			tvController.Nodes.Clear();
			tvController.Nodes.Add(treeNode);
			TreeNode treeNode2 = new TreeNode("EPG");
			LoadeTreeNodes(treeNode2, keyList, false);
			tvResult.Nodes.Clear();
			tvResult.Nodes.Add(treeNode2);
		}

		private void LoadeTreeNodes(TreeNode parentNode, IList<long> keyList, bool showAllNode)
		{
			parentNode.Nodes.Clear();
			if (parentNode.Tag == null)
			{
				IList<NitInfo> allNITInfos = new NITInfoManager().GetAllNITInfos();
				{
					foreach (NitInfo item in allNITInfos)
					{
						TreeNode treeNode = new TreeNode();
						treeNode.Text = string.Format("[0x{0}]{1}", item.NetworkId.ToString("X4"), item.NetworkName);
						treeNode.Tag = item;
						LoadeTreeNodes(treeNode, keyList, showAllNode);
						if (treeNode.Nodes.Count > 0)
						{
							parentNode.Nodes.Add(treeNode);
						}
						if (Tool.GetCheckedNodes(treeNode.Nodes, true).Count > 0)
						{
							parentNode.Expand();
						}
					}
					return;
				}
			}
			if (parentNode.Tag is NitInfo)
			{
				NitInfo nitInfo = parentNode.Tag as NitInfo;
				IList<TsInfo> tSInfoByNetWorkIdentity = new TSInfoManager().GetTSInfoByNetWorkIdentity(nitInfo.Identity);
				{
					foreach (TsInfo item2 in tSInfoByNetWorkIdentity)
					{
						TreeNode treeNode = new TreeNode();
						treeNode.Text = string.Format("[0x{0}]{1}", item2.TsId.ToString("X4"), item2.TsName);
						treeNode.Tag = item2;
						LoadeTreeNodes(treeNode, keyList, showAllNode);
						if (treeNode.Nodes.Count > 0)
						{
							parentNode.Nodes.Add(treeNode);
						}
						if (Tool.GetCheckedNodes(treeNode.Nodes, true).Count > 0)
						{
							parentNode.Expand();
						}
					}
					return;
				}
			}
			if (parentNode.Tag is TsInfo)
			{
				TsInfo tsInfo = parentNode.Tag as TsInfo;
				IList<ProgramInfo> byTSIdentity = new ProgramInfoManager().GetByTSIdentity(tsInfo.Identity);
				{
					foreach (ProgramInfo item3 in byTSIdentity)
					{
						TreeNode treeNode = new TreeNode();
						if (keyList.Contains(item3.Identity))
						{
							treeNode.Checked = true;
							treeNode.BackColor = Color.Aqua;
						}
						else
						{
							if (!showAllNode)
							{
								continue;
							}
							treeNode.Checked = false;
						}
						treeNode.Text = string.Format("[0x{0}]{1}", item3.ServiceId.ToString("X4"), item3.ProgramName);
						treeNode.Tag = item3;
						parentNode.Nodes.Add(treeNode);
						if (treeNode.Checked)
						{
							parentNode.Expand();
						}
					}
					return;
				}
			}
			if (!(parentNode.Tag is ProgramInfo))
			{
			}
		}

		protected override void FuncAdd()
		{
			List<TreeNode> list = Tool.GetCheckedNodes(tvController.Nodes, true).ToList();
			List<long> list2 = new List<long>();
			foreach (TreeNode item in list)
			{
				if (item.Tag is ProgramInfo)
				{
					list2.Add(((ProgramInfo)item.Tag).Identity);
				}
			}
			new PATManager().Save(list2, LoginUser.Ins.UserIns.UserID);
			Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.OperateSucceed]);
			FormTSDataGenerator_Load(null, null);
		}

		protected override void FuncEdit()
		{
			int num = new SendNoticesManager().SetAllNoticeOn();
			Tool.ShowMessage(ResourceManagement.GetLocationString("NoticeSendedCount") + num);
			FormTSDataGenerator_Load(null, null);
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in tvController.Nodes)
			{
				node.Checked = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in tvController.Nodes)
			{
				node.Checked = false;
			}
		}

		private void tsmCollapseAll_Click(object sender, EventArgs e)
		{
			tvController.CollapseAll();
		}

		private void tsmExpandAll_Click(object sender, EventArgs e)
		{
			tvController.ExpandAll();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.SendGenerator.FormTSDataGenerator));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.tvController = new System.Windows.Forms.TreeView();
			this.cmsSendPrg = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmExpandAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.tvResult = new System.Windows.Forms.TreeView();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.cmsSendPrg.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.splitContainer1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
			this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel3);
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.tvController, 0, 1);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.tvController.CheckBoxes = true;
			this.tvController.ContextMenuStrip = this.cmsSendPrg;
			resources.ApplyResources(this.tvController, "tvController");
			this.tvController.Name = "tvController";
			this.cmsSendPrg.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.tsmSelectAll, this.tsmDeselectAll, this.tsmCollapseAll, this.tsmExpandAll });
			this.cmsSendPrg.Name = "cmsSendPrg";
			resources.ApplyResources(this.cmsSendPrg, "cmsSendPrg");
			this.tsmSelectAll.Name = "tsmSelectAll";
			resources.ApplyResources(this.tsmSelectAll, "tsmSelectAll");
			this.tsmSelectAll.Click += new System.EventHandler(tsmSelectAll_Click);
			this.tsmDeselectAll.Name = "tsmDeselectAll";
			resources.ApplyResources(this.tsmDeselectAll, "tsmDeselectAll");
			this.tsmDeselectAll.Click += new System.EventHandler(tsmDeselectAll_Click);
			this.tsmCollapseAll.Name = "tsmCollapseAll";
			resources.ApplyResources(this.tsmCollapseAll, "tsmCollapseAll");
			this.tsmCollapseAll.Click += new System.EventHandler(tsmCollapseAll_Click);
			this.tsmExpandAll.Name = "tsmExpandAll";
			resources.ApplyResources(this.tsmExpandAll, "tsmExpandAll");
			this.tsmExpandAll.Click += new System.EventHandler(tsmExpandAll_Click);
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tvResult, 0, 1);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			resources.ApplyResources(this.tvResult, "tvResult");
			this.tvResult.Name = "tvResult";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormTSDataGenerator";
			base.Load += new System.EventHandler(FormTSDataGenerator_Load);
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.cmsSendPrg.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
