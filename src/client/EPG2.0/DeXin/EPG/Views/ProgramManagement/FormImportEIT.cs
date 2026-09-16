using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.EventFileHelper;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;
using Dexin.Util.Common.Text;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormImportEIT : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel2;

		private TextBox txtEITFilePath;

		private FlowLayoutPanel flowLayoutPanel1;

		private Button btSelectPath;

		private Button btSearchEIT;

		private FolderBrowserDialog fbdEITFilePath;

		private Label lblSearchProcess;

		private Label lblImportProcess;

		private TreeView tvImportPrg;

		private ContextMenuStrip cmsSelectEITFile;

		private ToolStripMenuItem tsmSelectAll;

		private ToolStripMenuItem tsmDeselectAll;

		private FlowLayoutPanel flowLayoutPanel2;

		private ComboBox cbxDateFormat;

		private CheckBox ckbUseDateFormat;

		private ToolStripMenuItem tsmCollapseAll;

		private ToolStripMenuItem tsmExpandAll;

		private CheckBox ckbIncludeExcel;

		private ComboBox cbxUseRegion;

		private FlowLayoutPanel flowLayoutPanel3;

		private CheckBox ckbUseLan;

		private TextBox txtLan;

		public FormImportEIT()
		{
			InitializeComponent();
			tvImportPrg.AfterCheck += delegate(object sendera, TreeViewEventArgs ea)
			{
				Tool.SetNodeChecked(ea.Node, true);
			};
		}

		private void FormImportEIT_Load(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncEITImport;
			base.PageFlag = PageEnum.Add;
			List<BaseKey> list = new List<BaseKey>();
			foreach (EnumDateFormat value in Enum.GetValues(typeof(EnumDateFormat)))
			{
				list.Add(new BaseKey
				{
					BKKey = value.ToString(),
					BKValue = ResourceManagement.GetLocationString(value.ToString())
				});
			}
			Tool.BindingComboBox(list, cbxDateFormat);
			cbxUseRegion.DisplayMember = "val";
			cbxUseRegion.ValueMember = "key";
			List<object> list2 = new List<object>();
			foreach (EnumUseRegions value2 in Enum.GetValues(typeof(EnumUseRegions)))
			{
				if (value2 == EnumUseRegions.Other)
				{
					list2.Add(new
					{
						key = value2.ToString(),
						val = ResourceManagement.GetLocationString(value2.ToString())
					});
				}
				else
				{
					list2.Add(new
					{
						key = value2.ToString(),
						val = value2.ToString()
					});
				}
			}
			cbxUseRegion.DataSource = list2;
			cbxUseRegion.SelectedValue = EPGSettings.Instatce.UseRegion.ToString();
		}

		private void btSelectPath_Click(object sender, EventArgs e)
		{
			if (fbdEITFilePath.ShowDialog() == DialogResult.OK)
			{
				txtEITFilePath.Text = fbdEITFilePath.SelectedPath;
			}
		}

		private void btSearchEIT_Click(object sender, EventArgs e)
		{
			string selectPath = txtEITFilePath.Text.Trim();
			if (string.IsNullOrEmpty(selectPath) || !Directory.Exists(selectPath))
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("SelectFilePath"));
				return;
			}
			if (new DirectoryInfo(selectPath).Parent == null)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("CannotSelectRootDirectory"));
				return;
			}
			tvImportPrg.Nodes.Clear();
			Thread thread = new Thread((ThreadStart)delegate
			{
				IList<ProgramInfo> hasEITFileProgram = new ProgramInfoManager().GetHasEITFileProgram();
				foreach (ProgramInfo item in hasEITFileProgram)
				{
					try
					{
						if (!string.IsNullOrWhiteSpace(item.EITFileName))
						{
							string text = ResourceManagement.GetLocationString("Program") + "[{0}] " + ResourceManagement.GetLocationString("Event") + "[{1}]";
							EITFileSearchProcess_Changed(string.Format(ResourceManagement.GetLocationString("FindingEvent") + " " + text, item.ProgramName, item.EITFileName));
							string[] files = Directory.GetFiles(selectPath, item.EITFileName, SearchOption.AllDirectories);
							if (files.Length == 0)
							{
								EITFileSearchProcess_Changed(string.Format(ResourceManagement.GetLocationString("Program") + "[{0}]", item.ProgramName, item.EITFileName));
							}
							else if (files.Length == 1)
							{
								EITFileSearchProcess_Changed(string.Format(ResourceManagement.GetLocationString("AddingEventFileForProgram") + " " + text, item.ProgramName, item.EITFileName));
								item.EITFileName = files[0];
								AddEITFileItemToUI(item);
							}
							else
							{
								string message = string.Format(ResourceManagement.GetLocationString("CannotProcessEventFilesFound") + " " + text, item.ProgramName, item.EITFileName);
								EITFileSearchProcess_Changed(message);
								Tool.WriteTXTLog(message);
							}
						}
					}
					catch (Exception ex)
					{
						Tool.WriteTXTLog(string.Format("Error:{0}. ts id[{1}] service id[{2}] service name[{3}] event file{4}", ex.Message, item.TS.TsId.ToString("X4"), item.ServiceId.ToString("X4"), item.ProgramName, item.EITFileName));
					}
				}
				if (tvImportPrg.Nodes.Count > 0)
				{
					EITFileSearchProcess_Changed(string.Format(ResourceManagement.GetLocationString("FindEventAndSelectProgram")));
				}
				else
				{
					EITFileSearchProcess_Changed(string.Format(ResourceManagement.GetLocationString("CannotFindEvents")));
				}
			});
			thread.Name = "EIT File Search";
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			thread.Start();
		}

		protected override bool FunSubmit()
		{
			IList<TreeNode> checkedNodes = Tool.GetCheckedNodes(tvImportPrg.Nodes, true);
			if (checkedNodes.Count < 1)
			{
				Tool.ShowMessage(ResourceManagement.GetLocationString("SelectOneOrMoreProgram"));
				return false;
			}
			EnumDateFormat? dateFormat = null;
			if (ckbUseDateFormat.Checked)
			{
				dateFormat = (EnumDateFormat)(object)Tool.GetEnum(typeof(EnumDateFormat), Tool.GetValue(cbxDateFormat));
			}
			base.Enabled = false;
			Thread thread = new Thread((ThreadStart)delegate
			{
				try
				{
					bool isAllImportSucc = true;
					int affectedNums = 0;
					List<ProgramInfo> selectPrgList = (from p in Tool.GetCheckedNodes(tvImportPrg.Nodes, true)
						select p.Tag as ProgramInfo into p1
						where p1 != null
						select p1).ToList();
					new EITInfoManager().SaveImportEITAndLog(selectPrgList, EITFileImprotProcess_Changed, dateFormat, ref isAllImportSucc, ref affectedNums, ckbUseLan.Checked ? txtLan.Text.Trim() : "");
					string empty = string.Empty;
					empty = ((!isAllImportSucc || affectedNums <= 0) ? ResourceManagement.GetLocationString("ImportFinishedAndSomeIgnored") : ResourceManagement.GetLocationString("ImportFinishedAndAllSucceed"));
					empty = string.Format("{0}\n{1}", empty, string.Format(ResourceManagement.GetLocationString("ImportCount"), affectedNums));
					if (File.Exists(Tool.GetLogPath(DateTime.Now)))
					{
						if (MessageBox.Show(string.Format("{0}\n{1}", empty, ResourceManagement.GetLocationString("IsOpenLog")), "EPG", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							Process.Start("notepad.exe", Tool.GetLogPath(DateTime.Now));
						}
					}
					else
					{
						Tool.ShowMessage(empty);
					}
					if (File.Exists(Tool.GetLogPath(DateTime.Now)))
					{
						if (MessageBox.Show(string.Format("Open the log folder?"), "EPG", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							Process.Start("Explorer.exe", TextHelper.GetLogFolder());
						}
					}
					else
					{
						Tool.ShowMessage(empty);
					}
					base.DialogResult = DialogResult.OK;
				}
				catch (Exception ex)
				{
					Exception ex2 = ex;
					TextHelper.WriteTXTLog(ex2.Message);
					Invoke((Action)delegate
					{
						MessageBox.Show(ex2.Message);
						base.Enabled = true;
					});
				}
			});
			thread.Name = "EIT File Import";
			thread.IsBackground = true;
			thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			Thread thread2 = thread;
			thread2.Start();
			return true;
		}

		private void AddEITFileItemToUI(object arg)
		{
			tvImportPrg.Invoke((TreeViewAddNodeHandeller)delegate(TreeView tree, object addArg)
			{
				if (addArg is ProgramInfo)
				{
					ProgramInfo programInfo = addArg as ProgramInfo;
					foreach (TreeNode node3 in tree.Nodes)
					{
						NitInfo nitInfo = node3.Tag as NitInfo;
						if (nitInfo.Identity == programInfo.TS.NitTableId)
						{
							foreach (TreeNode node4 in node3.Nodes)
							{
								TsInfo tsInfo = node4.Tag as TsInfo;
								if (tsInfo.Identity == programInfo.TsTableId)
								{
									node4.Nodes.Add(new TreeNode
									{
										Text = string.Format("[0x{0}]{1}", programInfo.ServiceId.ToString("X4"), programInfo.BKValue),
										Tag = programInfo,
										Checked = true
									});
									tvImportPrg.ExpandAll();
									return;
								}
							}
							TreeNode node = new TreeNode
							{
								Text = string.Format("[0x{0}]{1}", programInfo.TS.TsId.ToString("X4"), programInfo.TS.BKValue),
								Tag = programInfo.TS,
								Checked = true
							};
							node3.Nodes.Add(node);
							AddEITFileItemToUI(addArg);
							return;
						}
					}
					TreeNode node2 = new TreeNode
					{
						Text = string.Format("[0x{0}]{1}", programInfo.TS.NIT.NetworkId.ToString("X4"), programInfo.TS.NIT.BKValue),
						Tag = programInfo.TS.NIT,
						Checked = true
					};
					tvImportPrg.Nodes.Add(node2);
					AddEITFileItemToUI(addArg);
				}
			}, tvImportPrg, arg);
		}

		private void EITFileImprotProcess_Changed(string message)
		{
			lblImportProcess.Invoke((EITFileImprotProcessHandeller)delegate(Label lable, string msg)
			{
				lable.Text = msg;
			}, lblImportProcess, message);
		}

		private void EITFileSearchProcess_Changed(string message)
		{
			lblSearchProcess.Invoke((EITFileImprotProcessHandeller)delegate(Label lable, string msg)
			{
				lable.Text = msg;
			}, lblSearchProcess, message);
		}

		private void tsmSelectAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in tvImportPrg.Nodes)
			{
				node.Checked = true;
			}
		}

		private void tsmDeselectAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in tvImportPrg.Nodes)
			{
				node.Checked = false;
			}
		}

		private void ckbUseDateFormat_CheckedChanged(object sender, EventArgs e)
		{
			cbxDateFormat.Enabled = ckbUseDateFormat.Checked;
		}

		private void tsmCollapseAll_Click(object sender, EventArgs e)
		{
			tvImportPrg.CollapseAll();
		}

		private void tsmExpandAll_Click(object sender, EventArgs e)
		{
			tvImportPrg.ExpandAll();
		}

		private void ckbIncludeExcel_CheckedChanged(object sender, EventArgs e)
		{
			cbxUseRegion.Enabled = ckbIncludeExcel.Checked;
		}

		private void cbxUseRegion_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ckbIncludeExcel.Checked && !EPGSettings.Instatce.UseRegion.ToString().Equals(cbxUseRegion.SelectedValue))
			{
				EPGSettings.Instatce.UseRegion = (EnumUseRegions)(object)Tool.GetEnum(typeof(EnumUseRegions), Tool.GetValue(cbxUseRegion));
				EPGSettings.Instatce.Save();
			}
		}

		private void ckbUseLan_CheckedChanged(object sender, EventArgs e)
		{
			txtLan.Enabled = ckbUseLan.Checked;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormImportEIT));
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.txtEITFilePath = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btSelectPath = new System.Windows.Forms.Button();
			this.btSearchEIT = new System.Windows.Forms.Button();
			this.lblSearchProcess = new System.Windows.Forms.Label();
			this.lblImportProcess = new System.Windows.Forms.Label();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.ckbUseLan = new System.Windows.Forms.CheckBox();
			this.txtLan = new System.Windows.Forms.TextBox();
			this.tvImportPrg = new System.Windows.Forms.TreeView();
			this.cmsSelectEITFile = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmExpandAll = new System.Windows.Forms.ToolStripMenuItem();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.ckbUseDateFormat = new System.Windows.Forms.CheckBox();
			this.cbxDateFormat = new System.Windows.Forms.ComboBox();
			this.ckbIncludeExcel = new System.Windows.Forms.CheckBox();
			this.cbxUseRegion = new System.Windows.Forms.ComboBox();
			this.fbdEITFilePath = new System.Windows.Forms.FolderBrowserDialog();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.cmsSelectEITFile.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.txtEITFilePath, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblSearchProcess, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblImportProcess, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel3, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.tvImportPrg, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 0, 1);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.txtEITFilePath, "txtEITFilePath");
			this.txtEITFilePath.Name = "txtEITFilePath";
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this.btSelectPath);
			this.flowLayoutPanel1.Controls.Add(this.btSearchEIT);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.btSelectPath, "btSelectPath");
			this.btSelectPath.Name = "btSelectPath";
			this.btSelectPath.UseVisualStyleBackColor = true;
			this.btSelectPath.Click += new System.EventHandler(btSelectPath_Click);
			resources.ApplyResources(this.btSearchEIT, "btSearchEIT");
			this.btSearchEIT.Name = "btSearchEIT";
			this.btSearchEIT.UseVisualStyleBackColor = true;
			this.btSearchEIT.Click += new System.EventHandler(btSearchEIT_Click);
			resources.ApplyResources(this.lblSearchProcess, "lblSearchProcess");
			this.tableLayoutPanel2.SetColumnSpan(this.lblSearchProcess, 2);
			this.lblSearchProcess.Name = "lblSearchProcess";
			resources.ApplyResources(this.lblImportProcess, "lblImportProcess");
			this.tableLayoutPanel2.SetColumnSpan(this.lblImportProcess, 2);
			this.lblImportProcess.Name = "lblImportProcess";
			resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
			this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel3, 2);
			this.flowLayoutPanel3.Controls.Add(this.ckbIncludeExcel);
			this.flowLayoutPanel3.Controls.Add(this.cbxUseRegion);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			resources.ApplyResources(this.ckbUseLan, "ckbUseLan");
			this.ckbUseLan.Name = "ckbUseLan";
			this.ckbUseLan.UseVisualStyleBackColor = true;
			this.ckbUseLan.CheckedChanged += new System.EventHandler(ckbUseLan_CheckedChanged);
			resources.ApplyResources(this.txtLan, "txtLan");
			this.txtLan.Name = "txtLan";
			this.tvImportPrg.CheckBoxes = true;
			this.tableLayoutPanel2.SetColumnSpan(this.tvImportPrg, 2);
			this.tvImportPrg.ContextMenuStrip = this.cmsSelectEITFile;
			resources.ApplyResources(this.tvImportPrg, "tvImportPrg");
			this.tvImportPrg.Name = "tvImportPrg";
			this.cmsSelectEITFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.tsmSelectAll, this.tsmDeselectAll, this.tsmCollapseAll, this.tsmExpandAll });
			this.cmsSelectEITFile.Name = "cmsSelectEITFile";
			resources.ApplyResources(this.cmsSelectEITFile, "cmsSelectEITFile");
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
			resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
			this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel2, 2);
			this.flowLayoutPanel2.Controls.Add(this.ckbUseDateFormat);
			this.flowLayoutPanel2.Controls.Add(this.cbxDateFormat);
			this.flowLayoutPanel2.Controls.Add(this.ckbUseLan);
			this.flowLayoutPanel2.Controls.Add(this.txtLan);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			resources.ApplyResources(this.ckbUseDateFormat, "ckbUseDateFormat");
			this.ckbUseDateFormat.Name = "ckbUseDateFormat";
			this.ckbUseDateFormat.UseVisualStyleBackColor = true;
			this.ckbUseDateFormat.CheckedChanged += new System.EventHandler(ckbUseDateFormat_CheckedChanged);
			resources.ApplyResources(this.cbxDateFormat, "cbxDateFormat");
			this.cbxDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxDateFormat.FormattingEnabled = true;
			this.cbxDateFormat.Items.AddRange(new object[3]
			{
				resources.GetString("cbxDateFormat.Items"),
				resources.GetString("cbxDateFormat.Items1"),
				resources.GetString("cbxDateFormat.Items2")
			});
			this.cbxDateFormat.Name = "cbxDateFormat";
			resources.ApplyResources(this.ckbIncludeExcel, "ckbIncludeExcel");
			this.ckbIncludeExcel.Name = "ckbIncludeExcel";
			this.ckbIncludeExcel.UseVisualStyleBackColor = true;
			this.ckbIncludeExcel.CheckedChanged += new System.EventHandler(ckbIncludeExcel_CheckedChanged);
			resources.ApplyResources(this.cbxUseRegion, "cbxUseRegion");
			this.cbxUseRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxUseRegion.FormattingEnabled = true;
			this.cbxUseRegion.Name = "cbxUseRegion";
			this.cbxUseRegion.SelectedIndexChanged += new System.EventHandler(cbxUseRegion_SelectedIndexChanged);
			resources.ApplyResources(this.fbdEITFilePath, "fbdEITFilePath");
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormImportEIT";
			base.Load += new System.EventHandler(FormImportEIT_Load);
			base.Controls.SetChildIndex(base.grbText, 0);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.cmsSelectEITFile.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
