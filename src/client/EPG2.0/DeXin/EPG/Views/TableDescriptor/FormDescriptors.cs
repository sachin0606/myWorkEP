using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.TableDescriptor
{
	public class FormDescriptors : FormToolBase
	{
		private IContainer components = null;

		private SplitContainer sc;

		private TreeView tv;

		private ContextMenuStrip cmsBAT_TS_Descriptor;

		private ToolStripMenuItem addBAT_TS_Descriptor;

		private ContextMenuStrip cmsNIT_Descriptor;

		private ToolStripMenuItem cmsAddNitDes;

		private ContextMenuStrip cmsBAT_Descriptor;

		private ToolStripMenuItem addBAT_Descriptor;

		private ContextMenuStrip cmsPMT_Media_Descriptor;

		private ToolStripMenuItem cmsAdd_PMT_Media_Descriptor;

		private ContextMenuStrip cmsSDT_Descriptor;

		private ToolStripMenuItem addSDT_Descriptor;

		private ContextMenuStrip cmsEIT_Descriptor;

		private ToolStripMenuItem cmsAddEIT_Descriptor;

		private ContextMenuStrip cmsPMT_Descriptor;

		private ToolStripMenuItem addPMT_Descriptor;

		private ContextMenuStrip cmsCAT_Descriptor;

		private ToolStripMenuItem cmsAdd_Descriptor;

		private ContextMenuStrip cmsNIT_TS_Descriptor;

		private ToolStripMenuItem cmsAddNIT_TS_Descriptor;

		private DGV DGV;

		private ContextMenuStrip cmsTV_RightMenu;

		private ToolStripMenuItem cmsRefurbishTreeView;

		private ContextMenuStrip cms_TOT_Descriptor;

		private ToolStripMenuItem cmsAdd_TOT_Descriptor;

		private DataGridViewCheckBoxColumn CK;

		private DataGridViewTextBoxColumn colIDentity;

		private DataGridViewTextBoxColumn colDesTag;

		private DataGridViewTextBoxColumn colDesName;

		private DataGridViewTextBoxColumn colDesLength;

		private DataGridViewTextBoxColumn colValue;

		private NITInfoManager nitManager = new NITInfoManager();

		private TSInfoManager tsManager = new TSInfoManager();

		private DescriptorManagement desManager = new DescriptorManagement();

		private ProgramInfoManager proManager = new ProgramInfoManager();

		private BATInfoManager batManager = new BATInfoManager();

		private MultimediaManager multimediaManager = new MultimediaManager();

		private EITInfoManager eitManager = new EITInfoManager();

		private DescriptorManagement pageManager = new DescriptorManagement();

		private LogBLL logManager = new LogBLL();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.FormDescriptors));
			this.sc = new System.Windows.Forms.SplitContainer();
			this.tv = new System.Windows.Forms.TreeView();
			this.cmsTV_RightMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsRefurbishTreeView = new System.Windows.Forms.ToolStripMenuItem();
			this.DGV = new Com.Cn.Tool.UserControls.DGV();
			this.CK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colIDentity = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDesTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDesName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDesLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cmsBAT_TS_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addBAT_TS_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsNIT_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsAddNitDes = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsBAT_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addBAT_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsPMT_Media_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsAdd_PMT_Media_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsSDT_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addSDT_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsEIT_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsAddEIT_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsPMT_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addPMT_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsCAT_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsAdd_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsNIT_TS_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsAddNIT_TS_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			this.cms_TOT_Descriptor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsAdd_TOT_Descriptor = new System.Windows.Forms.ToolStripMenuItem();
			base.pnlBase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.sc).BeginInit();
			this.sc.Panel1.SuspendLayout();
			this.sc.Panel2.SuspendLayout();
			this.sc.SuspendLayout();
			this.cmsTV_RightMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).BeginInit();
			this.cmsBAT_TS_Descriptor.SuspendLayout();
			this.cmsNIT_Descriptor.SuspendLayout();
			this.cmsBAT_Descriptor.SuspendLayout();
			this.cmsPMT_Media_Descriptor.SuspendLayout();
			this.cmsSDT_Descriptor.SuspendLayout();
			this.cmsEIT_Descriptor.SuspendLayout();
			this.cmsPMT_Descriptor.SuspendLayout();
			this.cmsCAT_Descriptor.SuspendLayout();
			this.cmsNIT_TS_Descriptor.SuspendLayout();
			this.cms_TOT_Descriptor.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.sc);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			resources.ApplyResources(this.sc, "sc");
			this.sc.Name = "sc";
			this.sc.Panel1.Controls.Add(this.tv);
			this.sc.Panel2.Controls.Add(this.DGV);
			this.tv.ContextMenuStrip = this.cmsTV_RightMenu;
			resources.ApplyResources(this.tv, "tv");
			this.tv.Name = "tv";
			this.tv.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(tv_AfterCollapse);
			this.tv.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(tv_BeforeExpand);
			this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(tv_AfterSelect);
			this.tv.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(tv_NodeMouseClick);
			this.cmsTV_RightMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsRefurbishTreeView });
			this.cmsTV_RightMenu.Name = "cmsTV_RightMenu";
			resources.ApplyResources(this.cmsTV_RightMenu, "cmsTV_RightMenu");
			this.cmsRefurbishTreeView.Name = "cmsRefurbishTreeView";
			resources.ApplyResources(this.cmsRefurbishTreeView, "cmsRefurbishTreeView");
			this.cmsRefurbishTreeView.Click += new System.EventHandler(cmsRefurbishTreeView_Click);
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(this.CK, this.colIDentity, this.colDesTag, this.colDesName, this.colDesLength, this.colValue);
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.Name = "DGV";
			this.DGV.RowTemplate.Height = 23;
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DGV_CellMouseClick);
			this.DGV.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DGV_CellMouseDoubleClick);
			resources.ApplyResources(this.CK, "CK");
			this.CK.Name = "CK";
			this.CK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.CK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colIDentity.DataPropertyName = "Identity";
			resources.ApplyResources(this.colIDentity, "colIDentity");
			this.colIDentity.Name = "colIDentity";
			this.colDesTag.DataPropertyName = "DesTypeTag";
			resources.ApplyResources(this.colDesTag, "colDesTag");
			this.colDesTag.Name = "colDesTag";
			this.colDesName.DataPropertyName = "DesTypeName";
			resources.ApplyResources(this.colDesName, "colDesName");
			this.colDesName.Name = "colDesName";
			this.colDesLength.DataPropertyName = "DataLength";
			resources.ApplyResources(this.colDesLength, "colDesLength");
			this.colDesLength.Name = "colDesLength";
			this.colValue.DataPropertyName = "DescriptionData";
			resources.ApplyResources(this.colValue, "colValue");
			this.colValue.Name = "colValue";
			this.cmsBAT_TS_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.addBAT_TS_Descriptor });
			this.cmsBAT_TS_Descriptor.Name = "cmsBAT_TS_Descriptor";
			resources.ApplyResources(this.cmsBAT_TS_Descriptor, "cmsBAT_TS_Descriptor");
			this.cmsBAT_TS_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.addBAT_TS_Descriptor.Name = "addBAT_TS_Descriptor";
			resources.ApplyResources(this.addBAT_TS_Descriptor, "addBAT_TS_Descriptor");
			this.cmsNIT_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsAddNitDes });
			this.cmsNIT_Descriptor.Name = "cmbNit";
			resources.ApplyResources(this.cmsNIT_Descriptor, "cmsNIT_Descriptor");
			this.cmsNIT_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.cmsAddNitDes.Name = "cmsAddNitDes";
			resources.ApplyResources(this.cmsAddNitDes, "cmsAddNitDes");
			this.cmsBAT_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.addBAT_Descriptor });
			this.cmsBAT_Descriptor.Name = "cmsBAT_Descriptor";
			resources.ApplyResources(this.cmsBAT_Descriptor, "cmsBAT_Descriptor");
			this.cmsBAT_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.addBAT_Descriptor.Name = "addBAT_Descriptor";
			resources.ApplyResources(this.addBAT_Descriptor, "addBAT_Descriptor");
			this.cmsPMT_Media_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsAdd_PMT_Media_Descriptor });
			this.cmsPMT_Media_Descriptor.Name = "cmsPMT_Media_Descriptor";
			resources.ApplyResources(this.cmsPMT_Media_Descriptor, "cmsPMT_Media_Descriptor");
			this.cmsPMT_Media_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.cmsAdd_PMT_Media_Descriptor.Name = "cmsAdd_PMT_Media_Descriptor";
			resources.ApplyResources(this.cmsAdd_PMT_Media_Descriptor, "cmsAdd_PMT_Media_Descriptor");
			this.cmsSDT_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.addSDT_Descriptor });
			this.cmsSDT_Descriptor.Name = "cmsSDT_Descriptor";
			resources.ApplyResources(this.cmsSDT_Descriptor, "cmsSDT_Descriptor");
			this.cmsSDT_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.addSDT_Descriptor.Name = "addSDT_Descriptor";
			resources.ApplyResources(this.addSDT_Descriptor, "addSDT_Descriptor");
			this.cmsEIT_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsAddEIT_Descriptor });
			this.cmsEIT_Descriptor.Name = "cmsEIT_Descriptor";
			resources.ApplyResources(this.cmsEIT_Descriptor, "cmsEIT_Descriptor");
			this.cmsEIT_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.cmsAddEIT_Descriptor.Name = "cmsAddEIT_Descriptor";
			resources.ApplyResources(this.cmsAddEIT_Descriptor, "cmsAddEIT_Descriptor");
			this.cmsPMT_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.addPMT_Descriptor });
			this.cmsPMT_Descriptor.Name = "cmsPMT_Descriptor";
			resources.ApplyResources(this.cmsPMT_Descriptor, "cmsPMT_Descriptor");
			this.cmsPMT_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.addPMT_Descriptor.Name = "addPMT_Descriptor";
			resources.ApplyResources(this.addPMT_Descriptor, "addPMT_Descriptor");
			this.cmsCAT_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsAdd_Descriptor });
			this.cmsCAT_Descriptor.Name = "cmsCAT_Descriptor";
			resources.ApplyResources(this.cmsCAT_Descriptor, "cmsCAT_Descriptor");
			this.cmsCAT_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.cmsAdd_Descriptor.Name = "cmsAdd_Descriptor";
			resources.ApplyResources(this.cmsAdd_Descriptor, "cmsAdd_Descriptor");
			this.cmsNIT_TS_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsAddNIT_TS_Descriptor });
			this.cmsNIT_TS_Descriptor.Name = "cmsNIT_TS_Descriptor";
			resources.ApplyResources(this.cmsNIT_TS_Descriptor, "cmsNIT_TS_Descriptor");
			this.cmsNIT_TS_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			this.cmsAddNIT_TS_Descriptor.Name = "cmsAddNIT_TS_Descriptor";
			resources.ApplyResources(this.cmsAddNIT_TS_Descriptor, "cmsAddNIT_TS_Descriptor");
			this.cms_TOT_Descriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.cmsAdd_TOT_Descriptor });
			this.cms_TOT_Descriptor.Name = "cms_TOT_Descriptor";
			resources.ApplyResources(this.cms_TOT_Descriptor, "cms_TOT_Descriptor");
			this.cmsAdd_TOT_Descriptor.Name = "cmsAdd_TOT_Descriptor";
			resources.ApplyResources(this.cmsAdd_TOT_Descriptor, "cmsAdd_TOT_Descriptor");
			this.cmsAdd_TOT_Descriptor.Click += new System.EventHandler(CMSAddDescriptor_Click);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormDescriptors";
			base.Controls.SetChildIndex(base.pnlBase, 0);
			base.pnlBase.ResumeLayout(false);
			this.sc.Panel1.ResumeLayout(false);
			this.sc.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.sc).EndInit();
			this.sc.ResumeLayout(false);
			this.cmsTV_RightMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.DGV).EndInit();
			this.cmsBAT_TS_Descriptor.ResumeLayout(false);
			this.cmsNIT_Descriptor.ResumeLayout(false);
			this.cmsBAT_Descriptor.ResumeLayout(false);
			this.cmsPMT_Media_Descriptor.ResumeLayout(false);
			this.cmsSDT_Descriptor.ResumeLayout(false);
			this.cmsEIT_Descriptor.ResumeLayout(false);
			this.cmsPMT_Descriptor.ResumeLayout(false);
			this.cmsCAT_Descriptor.ResumeLayout(false);
			this.cmsNIT_TS_Descriptor.ResumeLayout(false);
			this.cms_TOT_Descriptor.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormDescriptors()
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncDescriptorType;
		}

		protected override void FormLoad()
		{
			SetMenuVisible(true, false, true, true, false, false, true, true, false);
			LoadTree();
			DGV.InitDGV(CK.Name);
			tsbQuery.Text = ResourceManagement.GetLocationString("Refresh");
		}

		protected override void FuncQuery()
		{
			LoadTree();
			base.FuncQuery();
		}

		protected override void FuncEdit()
		{
			ShowDetail(DGV.CurrentRow, PageEnum.Edit);
			base.FuncEdit();
		}

		protected override bool FuncDelete()
		{
			bool flag = true;
			IList<long> list = new List<long>();
			EnumPossibleLoaction enumPossibleLoaction = EnumPossibleLoaction.BAT;
			foreach (DataGridViewRow item in (IEnumerable)DGV.Rows)
			{
				if (Tool.GetBoolean(item.Cells[CK.Name].Value))
				{
					DescriptorSigleBase descriptorSigleBase = item.Tag as DescriptorSigleBase;
					if (descriptorSigleBase != null)
					{
						enumPossibleLoaction = descriptorSigleBase.Location;
						list.Add(descriptorSigleBase.Identity);
					}
				}
			}
			if (list.Count == 0)
			{
				msg = ResourceManagement.GetLocationString("NoAvailableContentDelete");
				flag = false;
			}
			else if (Tool.ShowMessage(LocationControl.Ins.LocationItems[LacationEnum.ConfirmDelete], MessageBoxButtons.OKCancel))
			{
				flag = pageManager.DeleteDescriptor(enumPossibleLoaction, list, out msg);
				if (flag)
				{
					StringBuilder stringBuilder = new StringBuilder(enumPossibleLoaction.ToString() + ":");
					foreach (long item2 in list)
					{
						stringBuilder.Append(item2 + "-");
					}
					logManager.AddLog(base.FunctionName, EnumOSName.Delete, stringBuilder.ToString());
					RefurbishDGV();
				}
			}
			Tool.ShowMessage(msg);
			return flag;
		}

		private void tv_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			try
			{
				ExpandNode(e);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void tv_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			NodeItem nodeItem = e.Node.Tag as NodeItem;
			if (nodeItem != null)
			{
				e.Node.Nodes.Clear();
				e.Node.Nodes.Add(new TreeNode());
			}
		}

		private void CMSAddDescriptor_Click(object sender, EventArgs e)
		{
			NodeItem nodeItem = tv.SelectedNode.Tag as NodeItem;
			if (nodeItem != null && nodeItem.DesLevel != DescriptorLevel.TABLE && nodeItem.DesLevel != DescriptorLevel.LEVELNONE)
			{
				FormDescriptorDetailBase formDescriptorDetailBase = null;
				switch (nodeItem.Location)
				{
				case EnumPossibleLoaction.BAT_TS:
					formDescriptorDetailBase = new FormDescriptorDetailBase(0L, nodeItem.BATId, nodeItem.BAT_TS_Id, PageEnum.Add);
					break;
				case EnumPossibleLoaction.TOT:
					formDescriptorDetailBase = new FormDescriptorDetailBase(0L, 0L, nodeItem.Location, PageEnum.Add);
					break;
				default:
					formDescriptorDetailBase = new FormDescriptorDetailBase(0L, nodeItem.TableID, nodeItem.Location, PageEnum.Add);
					break;
				}
				ShowDetail(formDescriptorDetailBase);
			}
		}

		private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			DGV.EndEdit();
			if (e.RowIndex < 0 || e.ColumnIndex != 0)
			{
				return;
			}
			try
			{
				DataGridViewRow dataGridViewRow = DGV.Rows[e.RowIndex];
				dataGridViewRow.Cells[CK.Name].Value = !Tool.GetBoolean(dataGridViewRow.Cells[CK.Name].Value);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void DGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			try
			{
				ShowDetail(DGV.Rows[e.RowIndex], PageEnum.Show);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void tv_AfterSelect(object sender, TreeViewEventArgs e)
		{
			try
			{
				RefurbishDGV();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void tv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			tv.SelectedNode = e.Node;
		}

		private void cmsRefurbishTreeView_Click(object sender, EventArgs e)
		{
			LoadTree();
		}

		private void LoadTree()
		{
			tv.Nodes.Clear();
			DGV.Rows.Clear();
			try
			{
				TreeNode treeNode = new TreeNode();
				treeNode.Text = "EPG";
				AddNitNodes(treeNode);
				tv.Nodes.Add(treeNode);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void AddNitNodes(TreeNode root)
		{
			IList<TreeNode> list = new List<TreeNode>();
			TreeNode treeNode = null;
			IList<BaseKey> list2 = ((IEnumerable<BaseKey>)nitManager.GetAllNITInfos()).ToList();
			foreach (BaseKey item in list2)
			{
				treeNode = new TreeNode(string.Format("[0x{0}]{1}", ((NitInfo)item.Node).NetworkId.ToString("X4"), item.BKValue));
				AddTSNodes(treeNode, (NitInfo)item.Node);
				root.Nodes.Add(treeNode);
			}
		}

		private void AddTSNodes(TreeNode networkNode, NitInfo network)
		{
			IList<TreeNode> list = new List<TreeNode>();
			TreeNode treeNode = null;
			IList<BaseKey> list2 = ((IEnumerable<BaseKey>)tsManager.GetTSInfoByNetWorkIdentity(network.Identity)).ToList();
			BaseTable baseTable = null;
			foreach (BaseKey item in list2)
			{
				baseTable = new BaseTable();
				baseTable.network = network;
				baseTable.ts = (TsInfo)item.Node;
				treeNode = new TreeNode(string.Format("[0x{0}]{1}", ((TsInfo)baseTable.ts).TsId.ToString("X4"), item.BKValue));
				AddAllTable(treeNode, baseTable);
				networkNode.Nodes.Add(treeNode);
			}
		}

		private void AddAllTable(TreeNode tsNode, BaseTable tableinfo)
		{
			TreeNode treeNode = null;
			NodeItem nodeItem = new NodeItem();
			NodeItem nodeItem2 = new NodeItem();
			NodeItem nodeItem3 = new NodeItem();
			foreach (EnumTableType value in Enum.GetValues(typeof(EnumTableType)))
			{
				treeNode = new TreeNode(value.ToString());
				treeNode.Nodes.Add(new TreeNode(""));
				switch (value)
				{
				case EnumTableType.NIT:
					nodeItem = new NodeItem();
					nodeItem.Location = EnumPossibleLoaction.NIT;
					nodeItem.TableType = EnumTableType.NIT;
					nodeItem.DesLevel = DescriptorLevel.LEVEL1;
					nodeItem.NetWorkId = tableinfo.network.Identity;
					nodeItem.TSId = tableinfo.ts.Identity;
					nodeItem.TableID = tableinfo.network.Identity;
					treeNode.Tag = nodeItem;
					treeNode.ContextMenuStrip = cmsNIT_Descriptor;
					tsNode.Nodes.Add(treeNode);
					break;
				case EnumTableType.PMT:
					nodeItem = new NodeItem();
					nodeItem.Location = EnumPossibleLoaction.PMT;
					nodeItem.TableType = EnumTableType.PMT;
					nodeItem.DesLevel = DescriptorLevel.TABLE;
					nodeItem.NetWorkId = tableinfo.network.Identity;
					nodeItem.TSId = tableinfo.ts.Identity;
					treeNode.Tag = nodeItem;
					tsNode.Nodes.Add(treeNode);
					break;
				case EnumTableType.BAT:
					nodeItem = new NodeItem();
					nodeItem.Location = EnumPossibleLoaction.BAT;
					nodeItem.TableType = EnumTableType.BAT;
					nodeItem.DesLevel = DescriptorLevel.TABLE;
					nodeItem.NetWorkId = tableinfo.network.Identity;
					nodeItem.TSId = tableinfo.ts.Identity;
					treeNode.Tag = nodeItem;
					tsNode.Nodes.Add(treeNode);
					break;
				case EnumTableType.SDT:
					nodeItem = new NodeItem();
					nodeItem.Location = EnumPossibleLoaction.SDT;
					nodeItem.TableType = EnumTableType.SDT;
					nodeItem.DesLevel = DescriptorLevel.TABLE;
					nodeItem.NetWorkId = tableinfo.network.Identity;
					nodeItem.TSId = tableinfo.ts.Identity;
					treeNode.Tag = nodeItem;
					tsNode.Nodes.Add(treeNode);
					break;
				case EnumTableType.CAT:
					nodeItem = new NodeItem();
					nodeItem.Location = EnumPossibleLoaction.CAT;
					nodeItem.TableType = EnumTableType.CAT;
					nodeItem.DesLevel = DescriptorLevel.LEVEL1;
					nodeItem.NetWorkId = tableinfo.network.Identity;
					nodeItem.TSId = tableinfo.ts.Identity;
					nodeItem.TableID = tableinfo.ts.Identity;
					treeNode.Tag = nodeItem;
					treeNode.ContextMenuStrip = cmsCAT_Descriptor;
					tsNode.Nodes.Add(treeNode);
					break;
				case EnumTableType.EIT:
				{
					bool flag = true;
					nodeItem = new NodeItem();
					nodeItem.Location = EnumPossibleLoaction.EIT;
					nodeItem.TableType = EnumTableType.EIT;
					nodeItem.DesLevel = DescriptorLevel.TABLE;
					nodeItem.NetWorkId = tableinfo.network.Identity;
					nodeItem.TSId = tableinfo.ts.Identity;
					treeNode.Tag = nodeItem;
					tsNode.Nodes.Add(treeNode);
					break;
				}
				case EnumTableType.TOT:
					nodeItem = new NodeItem();
					nodeItem.Location = EnumPossibleLoaction.TOT;
					nodeItem.TableType = EnumTableType.TOT;
					nodeItem.DesLevel = DescriptorLevel.LEVEL1;
					treeNode.Tag = nodeItem;
					treeNode.ContextMenuStrip = cms_TOT_Descriptor;
					tsNode.Nodes.Add(treeNode);
					break;
				}
			}
		}

		private void ExpandNode(TreeViewCancelEventArgs e)
		{
			NodeItem nodeItem = e.Node.Tag as NodeItem;
			if (nodeItem == null)
			{
				return;
			}
			e.Node.Nodes.Clear();
			NodeItem nodeItem2 = null;
			IList<BaseKey> list = null;
			TreeNode treeNode = null;
			switch (nodeItem.TableType)
			{
			case EnumTableType.NIT:
			{
				DescriptorLevel desLevel = nodeItem.DesLevel;
				if (desLevel != DescriptorLevel.LEVEL1)
				{
					break;
				}
				list = ((IEnumerable<BaseKey>)tsManager.GetTSInfoByNetWorkIdentity(nodeItem.NetWorkId)).ToList();
				{
					foreach (BaseKey item in list)
					{
						TsInfo tsInfo = item.Node as TsInfo;
						treeNode = new TreeNode(string.Format("[0x{0}]{1}", Convert.ToInt32(tsInfo.TsId).ToString("X4"), tsInfo.TableName));
						nodeItem2 = NodeItem.CloneNode(nodeItem);
						nodeItem2.DesLevel = DescriptorLevel.LEVEL2;
						nodeItem2.Location = EnumPossibleLoaction.NIT_TS;
						nodeItem2.TableType = EnumTableType.TS;
						nodeItem2.TSId = tsInfo.Identity;
						nodeItem2.TableID = tsInfo.Identity;
						treeNode.Tag = nodeItem2;
						treeNode.ContextMenuStrip = cmsNIT_TS_Descriptor;
						e.Node.Nodes.Add(treeNode);
					}
					break;
				}
			}
			case EnumTableType.PMT:
				switch (nodeItem.DesLevel)
				{
				case DescriptorLevel.TABLE:
					list = ((IEnumerable<BaseKey>)proManager.GetByTSIdentity(nodeItem.TSId)).ToList();
					{
						foreach (BaseKey item2 in list)
						{
							ProgramInfo programInfo = item2.Node as ProgramInfo;
							treeNode = new TreeNode(string.Format("[0x{0}]{1}", programInfo.ServiceId.ToString("X4"), programInfo.ProgramName));
							nodeItem2 = NodeItem.CloneNode(nodeItem);
							nodeItem2.DesLevel = DescriptorLevel.LEVEL1;
							nodeItem2.Location = EnumPossibleLoaction.PMT;
							nodeItem2.TableType = EnumTableType.PMT;
							nodeItem2.ProgramId = programInfo.Identity;
							nodeItem2.TableID = programInfo.Identity;
							treeNode.Tag = nodeItem2;
							treeNode.ContextMenuStrip = cmsPMT_Descriptor;
							treeNode.Nodes.Add(new TreeNode());
							e.Node.Nodes.Add(treeNode);
						}
						break;
					}
				case DescriptorLevel.LEVEL1:
				{
					IList<Multimedia> byProgramIdentity = multimediaManager.GetByProgramIdentity(nodeItem.ProgramId);
					{
						foreach (Multimedia item3 in byProgramIdentity)
						{
							treeNode = new TreeNode(item3.PESType.StreamName);
							nodeItem2 = NodeItem.CloneNode(nodeItem);
							nodeItem2.DesLevel = DescriptorLevel.LEVEL2;
							nodeItem2.Location = EnumPossibleLoaction.ES;
							nodeItem2.TableType = EnumTableType.PMT;
							nodeItem2.MediaId = item3.Id;
							nodeItem2.TableID = item3.Id;
							treeNode.Tag = nodeItem2;
							treeNode.ContextMenuStrip = cmsPMT_Media_Descriptor;
							e.Node.Nodes.Add(treeNode);
						}
						break;
					}
				}
				}
				break;
			case EnumTableType.BAT:
				switch (nodeItem.DesLevel)
				{
				case DescriptorLevel.TABLE:
					list = ((IEnumerable<BaseKey>)batManager.GetByNetworkIdentity(nodeItem.NetWorkId)).ToList();
					{
						foreach (BaseKey item4 in list)
						{
							BATInfo bATInfo = item4.Node as BATInfo;
							treeNode = new TreeNode(bATInfo.TableName);
							nodeItem2 = NodeItem.CloneNode(nodeItem);
							nodeItem2.DesLevel = DescriptorLevel.LEVEL1;
							nodeItem2.Location = EnumPossibleLoaction.BAT;
							nodeItem2.TableType = EnumTableType.BAT;
							nodeItem2.BATId = bATInfo.Identity;
							nodeItem2.TableID = bATInfo.Identity;
							treeNode.Tag = nodeItem2;
							treeNode.ContextMenuStrip = cmsBAT_Descriptor;
							treeNode.Nodes.Add(new TreeNode());
							e.Node.Nodes.Add(treeNode);
						}
						break;
					}
				case DescriptorLevel.LEVEL1:
					list = ((IEnumerable<BaseKey>)tsManager.GetTSInfoByNetWorkIdentity(nodeItem.NetWorkId)).ToList();
					{
						foreach (BaseKey item5 in list)
						{
							TsInfo tsInfo = item5.Node as TsInfo;
							treeNode = new TreeNode(string.Format("[0x{0}]{1}", tsInfo.TsId.ToString("X4"), tsInfo.TableName));
							nodeItem2 = NodeItem.CloneNode(nodeItem);
							nodeItem2.DesLevel = DescriptorLevel.LEVEL2;
							nodeItem2.Location = EnumPossibleLoaction.BAT_TS;
							nodeItem2.TableType = EnumTableType.BAT;
							nodeItem2.BAT_TS_Id = tsInfo.Identity;
							nodeItem2.TableID = tsInfo.Identity;
							treeNode.Tag = nodeItem2;
							treeNode.ContextMenuStrip = cmsBAT_TS_Descriptor;
							e.Node.Nodes.Add(treeNode);
						}
						break;
					}
				}
				break;
			case EnumTableType.SDT:
				if (nodeItem.DesLevel != DescriptorLevel.TABLE)
				{
					break;
				}
				list = ((IEnumerable<BaseKey>)proManager.GetByTSIdentity(nodeItem.TSId)).ToList();
				{
					foreach (BaseKey item6 in list)
					{
						ProgramInfo programInfo = item6.Node as ProgramInfo;
						treeNode = new TreeNode(string.Format("[0x{0}]{1}", programInfo.ServiceId.ToString("X4"), programInfo.ProgramName));
						nodeItem2 = NodeItem.CloneNode(nodeItem);
						nodeItem2.DesLevel = DescriptorLevel.LEVEL1;
						nodeItem2.Location = EnumPossibleLoaction.SDT;
						nodeItem2.TableType = EnumTableType.SDT;
						nodeItem2.ProgramId = programInfo.Identity;
						nodeItem2.TableID = programInfo.Identity;
						treeNode.Tag = nodeItem2;
						treeNode.ContextMenuStrip = cmsSDT_Descriptor;
						e.Node.Nodes.Add(treeNode);
					}
					break;
				}
			case EnumTableType.EIT:
				switch (nodeItem.DesLevel)
				{
				case DescriptorLevel.TABLE:
					list = ((IEnumerable<BaseKey>)proManager.GetByTSIdentity(nodeItem.TSId)).ToList();
					{
						foreach (BaseKey item7 in list)
						{
							ProgramInfo programInfo = item7.Node as ProgramInfo;
							treeNode = new TreeNode(string.Format("[0x{0}]{1}", programInfo.ServiceId.ToString("X4"), programInfo.ProgramName));
							nodeItem2 = NodeItem.CloneNode(nodeItem);
							nodeItem2.DesLevel = DescriptorLevel.LEVELNONE;
							nodeItem2.Location = EnumPossibleLoaction.EIT;
							nodeItem2.TableType = EnumTableType.EIT;
							nodeItem2.ProgramId = programInfo.Identity;
							treeNode.Tag = nodeItem2;
							treeNode.Nodes.Add(new TreeNode());
							e.Node.Nodes.Add(treeNode);
						}
						break;
					}
				case DescriptorLevel.LEVELNONE:
					list = ((IEnumerable<BaseKey>)eitManager.GetByProgramIdentity(nodeItem.ProgramId)).ToList();
					{
						foreach (BaseKey item8 in list)
						{
							EitInfo eitInfo = item8.Node as EitInfo;
							treeNode = new TreeNode(string.Format("[0x{0}]{1}", eitInfo.EventId.ToString("X4"), eitInfo.TableName));
							nodeItem2 = NodeItem.CloneNode(nodeItem);
							nodeItem2.DesLevel = DescriptorLevel.LEVEL1;
							nodeItem2.Location = EnumPossibleLoaction.EIT;
							nodeItem2.TableType = EnumTableType.EIT;
							nodeItem2.EventId = eitInfo.Identity;
							nodeItem2.TableID = eitInfo.Identity;
							treeNode.Tag = nodeItem2;
							treeNode.ContextMenuStrip = cmsEIT_Descriptor;
							e.Node.Nodes.Add(treeNode);
						}
						break;
					}
				}
				break;
			case EnumTableType.PAT:
			case EnumTableType.TS:
				break;
			}
		}

		private void ShowDetail(DataGridViewRow row, PageEnum pFlag)
		{
			try
			{
				if (row != null)
				{
					DescriptorSigleBase descriptorSigleBase = row.Tag as DescriptorSigleBase;
					if (descriptorSigleBase != null)
					{
						FormDescriptorDetailBase formDescriptorDetailBase = null;
						EnumPossibleLoaction location = descriptorSigleBase.Location;
						formDescriptorDetailBase = ((location != EnumPossibleLoaction.BAT_TS) ? new FormDescriptorDetailBase(descriptorSigleBase.Identity, descriptorSigleBase.TableID, descriptorSigleBase.Location, pFlag) : new FormDescriptorDetailBase(descriptorSigleBase.Identity, descriptorSigleBase.BATTableID, descriptorSigleBase.TableID, pFlag));
						ShowDetail(formDescriptorDetailBase);
					}
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void ShowDetail(FormDescriptorDetailBase fdd)
		{
			try
			{
				fdd.Owner = this;
				if (fdd.ShowDialog() == DialogResult.OK)
				{
					RefurbishDGV();
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void RefurbishDGV()
		{
			if (tv.SelectedNode == null)
			{
				return;
			}
			NodeItem nodeItem = tv.SelectedNode.Tag as NodeItem;
			DGV.Rows.Clear();
			if (nodeItem == null || nodeItem.TableID <= 0)
			{
				return;
			}
			IList<DescriptorSigleBase> list = new List<DescriptorSigleBase>();
			switch (nodeItem.Location)
			{
			case EnumPossibleLoaction.BAT_TS:
				list = desManager.GetDescriptors(nodeItem.BATId, nodeItem.BAT_TS_Id);
				break;
			case EnumPossibleLoaction.TOT:
				list = new List<DescriptorSigleBase>();
				break;
			default:
				list = desManager.GetDescriptors(nodeItem.Location, nodeItem.TableID);
				break;
			}
			foreach (DescriptorSigleBase item in list)
			{
				int index = DGV.Rows.Add(new DataGridViewRow());
				DGV.Rows[index].Tag = item;
				DGV.Rows[index].Cells[colIDentity.Name].Value = item.Identity.ToString();
				DGV.Rows[index].Cells[colDesTag.Name].Value = string.Format("0x{0}", Tool.FormatNumber(item.DesTypeTag, FormatString.Ins.X2));
				DGV.Rows[index].Cells[colDesName.Name].Value = item.DesTypeName;
				DGV.Rows[index].Cells[colDesLength.Name].Value = string.Format("0x{1}({0})", item.DataLength, item.DataLength.ToString("X2"));
				DGV.Rows[index].Cells[colValue.Name].Value = Tool.BytesToString(item.DescriptionData);
			}
		}
	}
}
