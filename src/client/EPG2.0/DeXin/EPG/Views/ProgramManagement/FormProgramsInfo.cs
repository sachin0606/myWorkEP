using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;
using EPG.Tools;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormProgramsInfo : FormBase
	{
		private LogBLL loger = new LogBLL();

		private IContainer components = null;

		private TreeView treeView1;

		private ContextMenuStrip cmsNIT;

		private ToolStripMenuItem tsmAddTS;

		private ToolStripMenuItem tsmEditNIT;

		private ToolStripMenuItem tsmDelNIT;

		private ContextMenuStrip cmsTS;

		private ToolStripMenuItem tsmAddPrg;

		private ToolStripMenuItem tsmEditTS;

		private ToolStripMenuItem tsmDelTS;

		private ContextMenuStrip cmsProgram;

		private ToolStripMenuItem tsmEditPrg;

		private ToolStripMenuItem tsmDelPrg;

		private ContextMenuStrip cmsEPG;

		private ToolStripMenuItem tsmAddNIT;

		private ContextMenuStrip cmsForm;

		private ToolStripMenuItem tsmRefresh;

		private ToolStripMenuItem tsmClose;

		private ToolStripMenuItem tsmExport;

		public FormProgramsInfo()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncNETWORK;
		}

		private void ProgramsInfo_Load(object sender, EventArgs e)
		{
			TreeNode treeNode = new TreeNode("EPG");
			treeNode.ContextMenuStrip = cmsEPG;
			LoadeTreeNodes(treeNode);
			treeView1.Nodes.Clear();
			treeView1.Nodes.Add(treeNode);
			treeNode.Expand();
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			treeView1.SelectedNode = e.Node;
		}

		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			LoadeTreeNodes(e.Node);
		}

		private void LoadeTreeNodes(TreeNode reLoadeNode)
		{
			reLoadeNode.Nodes.Clear();
			if (reLoadeNode.Tag == null)
			{
				IList<NitInfo> list = (from x in new NITInfoManager().GetAllNITInfos()
					orderby x.NetworkId
					select x).ToList();
				{
					foreach (NitInfo item in list)
					{
						TreeNode treeNode = new TreeNode();
						treeNode.ContextMenuStrip = cmsNIT;
						treeNode.Text = string.Format("[0x{0}] {1}", item.NetworkId.ToString("X4"), item.BKValue);
						treeNode.Tag = item;
						treeNode.Nodes.Add("");
						reLoadeNode.Nodes.Add(treeNode);
					}
					return;
				}
			}
			if (reLoadeNode.Tag is NitInfo)
			{
				NitInfo nITInfoByNetworkIdentity = new NITInfoManager().GetNITInfoByNetworkIdentity(((NitInfo)reLoadeNode.Tag).Identity);
				reLoadeNode.Text = string.Format("[0x{0}] {1}", nITInfoByNetworkIdentity.NetworkId.ToString("X4"), nITInfoByNetworkIdentity.BKValue);
				reLoadeNode.Tag = nITInfoByNetworkIdentity;
				IList<TsInfo> list2 = (from x in new TSInfoManager().GetTSInfoByNetWorkIdentity(nITInfoByNetworkIdentity.Identity)
					orderby x.TsId
					select x).ToList();
				{
					foreach (TsInfo item2 in list2)
					{
						TreeNode treeNode = new TreeNode();
						treeNode.ContextMenuStrip = cmsTS;
						treeNode.Text = string.Format("[0x{0}] {1}", item2.TsId.ToString("X4"), item2.BKValue);
						treeNode.Tag = item2;
						treeNode.Nodes.Add("");
						reLoadeNode.Nodes.Add(treeNode);
					}
					return;
				}
			}
			if (reLoadeNode.Tag is TsInfo)
			{
				TsInfo tSInfoByTSIdentity = new TSInfoManager().GetTSInfoByTSIdentity(((TsInfo)reLoadeNode.Tag).Identity);
				reLoadeNode.Text = string.Format("[0x{0}] {1}", tSInfoByTSIdentity.TsId.ToString("X4"), tSInfoByTSIdentity.BKValue);
				reLoadeNode.Tag = tSInfoByTSIdentity;
				IList<ProgramInfo> list3 = (from x in new ProgramInfoManager().GetByTSIdentity(tSInfoByTSIdentity.Identity)
					orderby x.ServiceId
					select x).ToList();
				{
					foreach (ProgramInfo item3 in list3)
					{
						TreeNode treeNode = new TreeNode();
						treeNode.ContextMenuStrip = cmsProgram;
						treeNode.Text = string.Format("[0x{0}] {1}", item3.ServiceId.ToString("X4"), item3.BKValue);
						treeNode.Tag = item3;
						reLoadeNode.Nodes.Add(treeNode);
					}
					return;
				}
			}
			if (reLoadeNode.Tag is ProgramInfo)
			{
				ProgramInfo byIdentity = new ProgramInfoManager().GetByIdentity(((ProgramInfo)reLoadeNode.Tag).Identity);
				reLoadeNode.Text = string.Format("[0x{0}] {1}", byIdentity.ServiceId.ToString("X4"), byIdentity.BKValue);
				reLoadeNode.Tag = byIdentity;
			}
		}

		private void tsmRefresh_Click(object sender, EventArgs e)
		{
			ProgramsInfo_Load(sender, e);
		}

		private void tsmExport_Click(object sender, EventArgs e)
		{
			List<string> fileLines = (from x in new ProgramInfoManager().GetChannelConfiguration()
				select x.ToString()).ToList();
			CSVExporter.Export("ProgramInfo", fileLines);
		}

		private void tsmClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void tsmAddNIT_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncNETWORK;
			if (Permit(enumPermission.CanAdd, true))
			{
				RegistInfo registC = EPGRegistor.GetRegistC();
				if (registC.TrialVersion && new NITInfoManager().GetAllNITInfos().Count >= 1)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("VersionLimited"));
				}
				else if (new FormNIT().ShowDialog() == DialogResult.OK)
				{
					LoadeTreeNodes(treeView1.SelectedNode);
					loger.AddLog(_FunctionName, EnumOSName.Add, "");
				}
			}
		}

		private void tsmEditNIT_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncNETWORK;
			if (Permit(enumPermission.CanEdit, true))
			{
				NitInfo nitInfo = treeView1.SelectedNode.Tag as NitInfo;
				if (new FormNIT(nitInfo).ShowDialog() == DialogResult.OK)
				{
					LoadeTreeNodes(treeView1.SelectedNode);
					loger.AddLog(_FunctionName, EnumOSName.Edit, "key:" + nitInfo.Identity);
				}
			}
		}

		private void tsmDelNIT_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncNETWORK;
			if (!Permit(enumPermission.CanDelete, true))
			{
				return;
			}
			NitInfo nitInfo = treeView1.SelectedNode.Tag as NitInfo;
			if (Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.OKCancel))
			{
				if (new NITInfoManager().DeleteNITInfo(nitInfo.Identity) == 1)
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.DeleteSucceed]);
					LoadeTreeNodes(treeView1.SelectedNode.Parent);
					loger.AddLog(_FunctionName, EnumOSName.Delete, nitInfo.ToString() + ">" + nitInfo.Identity);
				}
				else
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.DeleteFailure]);
				}
			}
		}

		private void tsmAddTS_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncTSInfo;
			if (Permit(enumPermission.CanAdd, true))
			{
				TreeNode selectedNode = treeView1.SelectedNode;
				NitInfo nitInfo = selectedNode.Tag as NitInfo;
				RegistInfo registC = EPGRegistor.GetRegistC();
				if (registC.TrialVersion && new TSInfoManager().GetTSInfoByNetWorkIdentity(nitInfo.Identity).Count >= 2)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("VersionLimited"));
				}
				else if (new FormTS(nitInfo).ShowDialog() == DialogResult.OK)
				{
					LoadeTreeNodes(treeView1.SelectedNode);
					loger.AddLog(_FunctionName, EnumOSName.Add, "");
				}
			}
		}

		private void tsmEditTS_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncTSInfo;
			if (Permit(enumPermission.CanEdit, true))
			{
				TsInfo tsInfo = treeView1.SelectedNode.Tag as TsInfo;
				if (new FormTS(tsInfo).ShowDialog() == DialogResult.OK)
				{
					LoadeTreeNodes(treeView1.SelectedNode);
					loger.AddLog(_FunctionName, EnumOSName.Edit, "key:" + tsInfo.Identity);
				}
			}
		}

		private void tsmDelTS_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncTSInfo;
			if (!Permit(enumPermission.CanDelete, true))
			{
				return;
			}
			TsInfo tsInfo = treeView1.SelectedNode.Tag as TsInfo;
			if (Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.OKCancel))
			{
				if (new TSInfoManager().DeleteTSInfo(tsInfo.Identity) == 1)
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.DeleteSucceed]);
					LoadeTreeNodes(treeView1.SelectedNode.Parent);
					loger.AddLog(_FunctionName, EnumOSName.Delete, tsInfo.ToString() + ">" + tsInfo.Identity);
				}
				else
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.DeleteFailure]);
				}
			}
		}

		private void tsmAddPrg_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncProgramInfo;
			if (Permit(enumPermission.CanAdd, true))
			{
				TsInfo tsInfo = treeView1.SelectedNode.Tag as TsInfo;
				RegistInfo registC = EPGRegistor.GetRegistC();
				if (registC.TrialVersion && new ProgramInfoManager().GetByTSIdentity(tsInfo.Identity).Count >= 5)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("VersionLimited"));
				}
				else if (new FormProgram(tsInfo).ShowDialog() == DialogResult.OK)
				{
					LoadeTreeNodes(treeView1.SelectedNode);
					loger.AddLog(_FunctionName, EnumOSName.Add, "");
				}
			}
		}

		private void tsmEditPrg_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncProgramInfo;
			if (Permit(enumPermission.CanEdit, true))
			{
				ProgramInfo programInfo = treeView1.SelectedNode.Tag as ProgramInfo;
				if (new FormProgram(programInfo).ShowDialog() == DialogResult.OK)
				{
					LoadeTreeNodes(treeView1.SelectedNode);
					loger.AddLog(_FunctionName, EnumOSName.Edit, "key:" + programInfo.Identity);
				}
			}
		}

		private void tsmDelPrg_Click(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncProgramInfo;
			if (!Permit(enumPermission.CanDelete, true))
			{
				return;
			}
			ProgramInfo programInfo = treeView1.SelectedNode.Tag as ProgramInfo;
			if (Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.OKCancel))
			{
				if (new ProgramInfoManager().DeleteProgram(programInfo.Identity) == 1)
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.DeleteSucceed]);
					LoadeTreeNodes(treeView1.SelectedNode.Parent);
					loger.AddLog(_FunctionName, EnumOSName.Delete, programInfo.ToString() + ">" + programInfo.Identity);
				}
				else
				{
					Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.DeleteFailure]);
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormProgramsInfo));
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.cmsForm = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmExport = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmClose = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsNIT = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmAddTS = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEditNIT = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDelNIT = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsTS = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmAddPrg = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmEditTS = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDelTS = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsProgram = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmEditPrg = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDelPrg = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsEPG = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmAddNIT = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsForm.SuspendLayout();
			this.cmsNIT.SuspendLayout();
			this.cmsTS.SuspendLayout();
			this.cmsProgram.SuspendLayout();
			this.cmsEPG.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(this.treeView1, "treeView1");
			this.treeView1.ContextMenuStrip = this.cmsForm;
			this.treeView1.Name = "treeView1";
			this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(treeView1_BeforeExpand);
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
			resources.ApplyResources(this.cmsForm, "cmsForm");
			this.cmsForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmRefresh, this.tsmExport, this.tsmClose });
			this.cmsForm.Name = "cmsNIT";
			resources.ApplyResources(this.tsmRefresh, "tsmRefresh");
			this.tsmRefresh.Name = "tsmRefresh";
			this.tsmRefresh.Click += new System.EventHandler(tsmRefresh_Click);
			resources.ApplyResources(this.tsmExport, "tsmExport");
			this.tsmExport.Name = "tsmExport";
			this.tsmExport.Click += new System.EventHandler(tsmExport_Click);
			resources.ApplyResources(this.tsmClose, "tsmClose");
			this.tsmClose.Name = "tsmClose";
			this.tsmClose.Click += new System.EventHandler(tsmClose_Click);
			resources.ApplyResources(this.cmsNIT, "cmsNIT");
			this.cmsNIT.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmAddTS, this.tsmEditNIT, this.tsmDelNIT });
			this.cmsNIT.Name = "cmsNIT";
			resources.ApplyResources(this.tsmAddTS, "tsmAddTS");
			this.tsmAddTS.Name = "tsmAddTS";
			this.tsmAddTS.Click += new System.EventHandler(tsmAddTS_Click);
			resources.ApplyResources(this.tsmEditNIT, "tsmEditNIT");
			this.tsmEditNIT.Name = "tsmEditNIT";
			this.tsmEditNIT.Click += new System.EventHandler(tsmEditNIT_Click);
			resources.ApplyResources(this.tsmDelNIT, "tsmDelNIT");
			this.tsmDelNIT.Name = "tsmDelNIT";
			this.tsmDelNIT.Click += new System.EventHandler(tsmDelNIT_Click);
			resources.ApplyResources(this.cmsTS, "cmsTS");
			this.cmsTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmAddPrg, this.tsmEditTS, this.tsmDelTS });
			this.cmsTS.Name = "cmsNIT";
			resources.ApplyResources(this.tsmAddPrg, "tsmAddPrg");
			this.tsmAddPrg.Name = "tsmAddPrg";
			this.tsmAddPrg.Click += new System.EventHandler(tsmAddPrg_Click);
			resources.ApplyResources(this.tsmEditTS, "tsmEditTS");
			this.tsmEditTS.Name = "tsmEditTS";
			this.tsmEditTS.Click += new System.EventHandler(tsmEditTS_Click);
			resources.ApplyResources(this.tsmDelTS, "tsmDelTS");
			this.tsmDelTS.Name = "tsmDelTS";
			this.tsmDelTS.Click += new System.EventHandler(tsmDelTS_Click);
			resources.ApplyResources(this.cmsProgram, "cmsProgram");
			this.cmsProgram.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmEditPrg, this.tsmDelPrg });
			this.cmsProgram.Name = "cmsNIT";
			resources.ApplyResources(this.tsmEditPrg, "tsmEditPrg");
			this.tsmEditPrg.Name = "tsmEditPrg";
			this.tsmEditPrg.Click += new System.EventHandler(tsmEditPrg_Click);
			resources.ApplyResources(this.tsmDelPrg, "tsmDelPrg");
			this.tsmDelPrg.Name = "tsmDelPrg";
			this.tsmDelPrg.Click += new System.EventHandler(tsmDelPrg_Click);
			resources.ApplyResources(this.cmsEPG, "cmsEPG");
			this.cmsEPG.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.tsmAddNIT });
			this.cmsEPG.Name = "cmsNIT";
			resources.ApplyResources(this.tsmAddNIT, "tsmAddNIT");
			this.tsmAddNIT.Name = "tsmAddNIT";
			this.tsmAddNIT.Click += new System.EventHandler(tsmAddNIT_Click);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.treeView1);
			base.Name = "FormProgramsInfo";
			base.Load += new System.EventHandler(ProgramsInfo_Load);
			this.cmsForm.ResumeLayout(false);
			this.cmsNIT.ResumeLayout(false);
			this.cmsTS.ResumeLayout(false);
			this.cmsProgram.ResumeLayout(false);
			this.cmsEPG.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
