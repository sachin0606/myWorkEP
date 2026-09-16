using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using Com.Cn.Tool.UserControls;
using DeXin.EPG.BLL;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.TableDescriptor.DescriptorDemos
{
	public class Context_DescriptorDemo : DescriptorDemoBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableContent_Descriptor;

		private Label lblContent_nibble_level_1;

		private Label lblContent_nibble_level_2;

		private Label lblUser_nibble_1;

		private Label lblUser_nibble_2;

		private DescriptorCombobox cmbContent_nibble_level_1;

		private DescriptorCombobox cmbContent_nibble_level_2;

		private DescriptorTextBox txtUser_nibble1;

		private DescriptorTextBox txtUser_nibble2;

		private DGV DGV;

		private Button btnAdd;

		private Button btnModify;

		private DataGridViewTextBoxColumn colContent_nibble_level_1;

		private DataGridViewTextBoxColumn colContent_nibble_level_2;

		private DataGridViewTextBoxColumn colUser_bibble_1;

		private DataGridViewTextBoxColumn colUser_bibble_2;

		private DescriptorManagement pageManager = new DescriptorManagement();

		private IDictionary<int, BaseKey> Dirs = new Dictionary<int, BaseKey>();

		private int count = 0;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.DescriptorDemos.Context_DescriptorDemo));
			this.tableContent_Descriptor = new System.Windows.Forms.TableLayoutPanel();
			this.lblContent_nibble_level_1 = new System.Windows.Forms.Label();
			this.lblContent_nibble_level_2 = new System.Windows.Forms.Label();
			this.lblUser_nibble_1 = new System.Windows.Forms.Label();
			this.lblUser_nibble_2 = new System.Windows.Forms.Label();
			this.cmbContent_nibble_level_1 = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.cmbContent_nibble_level_2 = new DeXin.EPG.Tools.EPGUserControl.DescriptorCombobox();
			this.txtUser_nibble1 = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.txtUser_nibble2 = new DeXin.EPG.Tools.EPGUserControl.DescriptorTextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnModify = new System.Windows.Forms.Button();
			this.DGV = new Com.Cn.Tool.UserControls.DGV();
			this.colContent_nibble_level_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colContent_nibble_level_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colUser_bibble_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colUser_bibble_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			base.grbContext.SuspendLayout();
			this.tableContent_Descriptor.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).BeginInit();
			base.SuspendLayout();
			base.grbContext.Controls.Add(this.DGV);
			base.grbContext.Controls.Add(this.tableContent_Descriptor);
			resources.ApplyResources(base.grbContext, "grbContext");
			resources.ApplyResources(this.tableContent_Descriptor, "tableContent_Descriptor");
			this.tableContent_Descriptor.Controls.Add(this.lblContent_nibble_level_1, 0, 0);
			this.tableContent_Descriptor.Controls.Add(this.lblContent_nibble_level_2, 0, 1);
			this.tableContent_Descriptor.Controls.Add(this.lblUser_nibble_1, 0, 2);
			this.tableContent_Descriptor.Controls.Add(this.lblUser_nibble_2, 0, 3);
			this.tableContent_Descriptor.Controls.Add(this.cmbContent_nibble_level_1, 1, 0);
			this.tableContent_Descriptor.Controls.Add(this.cmbContent_nibble_level_2, 1, 1);
			this.tableContent_Descriptor.Controls.Add(this.txtUser_nibble1, 1, 2);
			this.tableContent_Descriptor.Controls.Add(this.txtUser_nibble2, 1, 3);
			this.tableContent_Descriptor.Controls.Add(this.btnAdd, 2, 0);
			this.tableContent_Descriptor.Controls.Add(this.btnModify, 2, 1);
			this.tableContent_Descriptor.Name = "tableContent_Descriptor";
			resources.ApplyResources(this.lblContent_nibble_level_1, "lblContent_nibble_level_1");
			this.lblContent_nibble_level_1.Name = "lblContent_nibble_level_1";
			resources.ApplyResources(this.lblContent_nibble_level_2, "lblContent_nibble_level_2");
			this.lblContent_nibble_level_2.Name = "lblContent_nibble_level_2";
			resources.ApplyResources(this.lblUser_nibble_1, "lblUser_nibble_1");
			this.lblUser_nibble_1.Name = "lblUser_nibble_1";
			resources.ApplyResources(this.lblUser_nibble_2, "lblUser_nibble_2");
			this.lblUser_nibble_2.Name = "lblUser_nibble_2";
			this.cmbContent_nibble_level_1.FormattingEnabled = true;
			resources.ApplyResources(this.cmbContent_nibble_level_1, "cmbContent_nibble_level_1");
			this.cmbContent_nibble_level_1.MaxValue = 14L;
			this.cmbContent_nibble_level_1.MinValue = 1L;
			this.cmbContent_nibble_level_1.Name = "cmbContent_nibble_level_1";
			this.cmbContent_nibble_level_1.Value = "";
			this.cmbContent_nibble_level_1.SelectedIndexChanged += new System.EventHandler(cmbContent_nibble_level_1_SelectedIndexChanged);
			this.cmbContent_nibble_level_2.FormattingEnabled = true;
			resources.ApplyResources(this.cmbContent_nibble_level_2, "cmbContent_nibble_level_2");
			this.cmbContent_nibble_level_2.MaxValue = 14L;
			this.cmbContent_nibble_level_2.MinValue = 0L;
			this.cmbContent_nibble_level_2.Name = "cmbContent_nibble_level_2";
			this.cmbContent_nibble_level_2.Value = "";
			resources.ApplyResources(this.txtUser_nibble1, "txtUser_nibble1");
			this.txtUser_nibble1.MaxValue = 15L;
			this.txtUser_nibble1.MinValue = 0L;
			this.txtUser_nibble1.Name = "txtUser_nibble1";
			this.txtUser_nibble1.Value = new byte[0];
			resources.ApplyResources(this.txtUser_nibble2, "txtUser_nibble2");
			this.txtUser_nibble2.MaxValue = 15L;
			this.txtUser_nibble2.MinValue = 0L;
			this.txtUser_nibble2.Name = "txtUser_nibble2";
			this.txtUser_nibble2.Value = new byte[0];
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
			resources.ApplyResources(this.btnModify, "btnModify");
			this.btnModify.Name = "btnModify";
			this.btnModify.UseVisualStyleBackColor = true;
			this.btnModify.Click += new System.EventHandler(btnModify_Click);
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(this.colContent_nibble_level_1, this.colContent_nibble_level_2, this.colUser_bibble_1, this.colUser_bibble_2);
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.Name = "DGV";
			this.DGV.ReadOnly = true;
			this.DGV.RowTemplate.Height = 23;
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DGV_CellMouseClick);
			resources.ApplyResources(this.colContent_nibble_level_1, "colContent_nibble_level_1");
			this.colContent_nibble_level_1.Name = "colContent_nibble_level_1";
			this.colContent_nibble_level_1.ReadOnly = true;
			this.colContent_nibble_level_1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			resources.ApplyResources(this.colContent_nibble_level_2, "colContent_nibble_level_2");
			this.colContent_nibble_level_2.Name = "colContent_nibble_level_2";
			this.colContent_nibble_level_2.ReadOnly = true;
			this.colContent_nibble_level_2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			resources.ApplyResources(this.colUser_bibble_1, "colUser_bibble_1");
			this.colUser_bibble_1.Name = "colUser_bibble_1";
			this.colUser_bibble_1.ReadOnly = true;
			resources.ApplyResources(this.colUser_bibble_2, "colUser_bibble_2");
			this.colUser_bibble_2.Name = "colUser_bibble_2";
			this.colUser_bibble_2.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Context_DescriptorDemo";
			base.grbContext.ResumeLayout(false);
			this.tableContent_Descriptor.ResumeLayout(false);
			this.tableContent_Descriptor.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.DGV).EndInit();
			base.ResumeLayout(false);
		}

		public Context_DescriptorDemo()
		{
			InitializeComponent();
		}

		public Context_DescriptorDemo(DescriptorBase descriptor, PageEnum pageFlag)
			: base(descriptor, pageFlag)
		{
			InitializeComponent();
			InitControl();
		}

		protected override void InitControl()
		{
			IList<BaseKey> contentNibbleLevelByParentID = pageManager.GetContentNibbleLevelByParentID(0);
			if (base.PageFlag == PageEnum.Add)
			{
				Tool.BindingComboBox(contentNibbleLevelByParentID, cmbContent_nibble_level_1);
				contentNibbleLevelByParentID = pageManager.GetContentNibbleLevelByParentID(1);
				Tool.BindingComboBox(contentNibbleLevelByParentID, cmbContent_nibble_level_2);
			}
			else
			{
				Tool.BindingComboBox(contentNibbleLevelByParentID.Where((BaseKey p) => p.BKStatus).ToList(), cmbContent_nibble_level_1);
				contentNibbleLevelByParentID = (from p in pageManager.GetContentNibbleLevelByParentID(1)
					where p.BKStatus
					select p).ToList();
				Tool.BindingComboBox(contentNibbleLevelByParentID, cmbContent_nibble_level_2);
			}
			CommTool.SetTextBoxInputNumber16(txtUser_nibble1);
			CommTool.SetTextBoxInputNumber16(txtUser_nibble2);
			Tool.SetDGVReadOnly((DataGridView)DGV, true, (string[])null);
		}

		protected override void BindingData()
		{
			if (base.PageFlag != PageEnum.Add && base.PageFlag != PageEnum.Empty)
			{
				Dirs.Clear();
				BaseKey baseKey = new BaseKey();
				byte[] array = new byte[0];
				string text = "";
				for (int i = 0; i < base.ET.DataLength; i += 2)
				{
					array = Tool.ResolveByte(base.ET.DescriptionData, i, 2);
					text = Tool.BytesToString(array);
					baseKey = new BaseKey();
					baseKey.BKKey = "0" + text.Substring(0, 1);
					baseKey.BKValue = "0" + text.Substring(1, 1);
					baseKey.Text = text.Substring(2);
					Dirs.Add(count, baseKey);
					count++;
				}
				BindDGV();
			}
			if (base.PageFlag == PageEnum.Show)
			{
				cmbContent_nibble_level_1.Enabled = false;
				cmbContent_nibble_level_2.Enabled = false;
				txtUser_nibble1.ReadOnly = true;
				txtUser_nibble2.ReadOnly = true;
				Button button = btnAdd;
				bool enabled = (btnModify.Enabled = false);
				button.Enabled = enabled;
			}
		}

		public override bool IsValidated(ErrorProvider error)
		{
			if (Dirs.Keys.Count == 0)
			{
				MessageBox.Show("no data");
				return false;
			}
			base.ET.DataLength = Dirs.Keys.Count * 2;
			BaseKey baseKey = null;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int key in Dirs.Keys)
			{
				baseKey = Dirs[key];
				stringBuilder.Append(baseKey.BKKey.Substring(1));
				stringBuilder.Append(baseKey.BKValue.Substring(1));
				stringBuilder.Append(baseKey.Text);
			}
			base.ET.DescriptionData = Tool.StringToBytes(stringBuilder.ToString());
			return true;
		}

		private void cmbContent_nibble_level_1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string value = cmbContent_nibble_level_1.Value;
			if (value.Contains('.') || value.Length == 0)
			{
				return;
			}
			try
			{
				int pid = Tool.ToNumber32(value);
				IList<BaseKey> contentNibbleLevelByParentID = pageManager.GetContentNibbleLevelByParentID(pid);
				if (base.PageFlag == PageEnum.Add)
				{
					Tool.BindingComboBox(contentNibbleLevelByParentID, cmbContent_nibble_level_2);
					return;
				}
				Tool.BindingComboBox(contentNibbleLevelByParentID.Where((BaseKey p) => p.BKStatus).ToList(), cmbContent_nibble_level_2);
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (CheckData())
			{
				count++;
				string value = cmbContent_nibble_level_1.Value;
				string value2 = cmbContent_nibble_level_2.Value;
				BaseKey baseKey = new BaseKey(value, value2, true);
				baseKey.Text = txtUser_nibble1.Text + txtUser_nibble2.Text;
				Dirs.Add(count, baseKey);
				BindDGV();
			}
		}

		private void btnModify_Click(object sender, EventArgs e)
		{
			if (DGV.CurrentRow != null && CheckData())
			{
				string value = cmbContent_nibble_level_1.Value;
				string value2 = cmbContent_nibble_level_2.Value;
				int key = Convert.ToInt32(DGV.CurrentRow.Tag);
				BaseKey baseKey = Dirs[key];
				baseKey.BKKey = value;
				baseKey.BKValue = value2;
				baseKey.Text = txtUser_nibble1.Text + txtUser_nibble2.Text;
				BindDGV();
			}
		}

		private bool CheckData()
		{
			if (cmbContent_nibble_level_1.Value.Length == 0)
			{
				Tool.ShowErrorProviderMessage(cmbContent_nibble_level_1, LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				return false;
			}
			Tool.ShowErrorProviderMessage(cmbContent_nibble_level_1, "");
			if (cmbContent_nibble_level_2.Value.Length == 0)
			{
				Tool.ShowErrorProviderMessage(cmbContent_nibble_level_2, LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				return false;
			}
			Tool.ShowErrorProviderMessage(cmbContent_nibble_level_2, "");
			if (txtUser_nibble1.Text.Length == 0)
			{
				Tool.ShowErrorProviderMessage(txtUser_nibble1, LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				return false;
			}
			Tool.ShowErrorProviderMessage(txtUser_nibble1, "");
			if (txtUser_nibble2.Text.Length == 0)
			{
				Tool.ShowErrorProviderMessage(txtUser_nibble2, LocationControl.Ins.LocationItems[LacationEnum.IsNotNull]);
				return false;
			}
			Tool.ShowErrorProviderMessage(txtUser_nibble2, "");
			return true;
		}

		private void BindDGV()
		{
			DGV.Rows.Clear();
			BaseKey baseKey = null;
			int num = 0;
			foreach (int key in Dirs.Keys)
			{
				baseKey = Dirs[key];
				num = DGV.Rows.Add(new DataGridViewRow());
				DGV.Rows[num].Tag = key;
				DGV.Rows[num].Cells[colContent_nibble_level_1.Name].Value = baseKey.BKKey;
				DGV.Rows[num].Cells[colContent_nibble_level_2.Name].Value = baseKey.BKValue;
				DGV.Rows[num].Cells[colUser_bibble_1.Name].Value = baseKey.Text.ToArray()[0];
				DGV.Rows[num].Cells[colUser_bibble_2.Name].Value = baseKey.Text.ToArray()[1];
			}
			cmbContent_nibble_level_1.SelectedIndex = 0;
			cmbContent_nibble_level_2.Value = "00";
			DescriptorTextBox descriptorTextBox = txtUser_nibble1;
			string text = (txtUser_nibble2.Text = "");
			descriptorTextBox.Text = text;
		}

		private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				int key = Convert.ToInt32(DGV.CurrentRow.Tag);
				BaseKey baseKey = Dirs[key];
				cmbContent_nibble_level_1.Value = baseKey.BKKey;
				cmbContent_nibble_level_2.Value = baseKey.BKValue;
				txtUser_nibble1.Text = baseKey.Text.ToArray()[0].ToString();
				txtUser_nibble2.Text = baseKey.Text.ToArray()[1].ToString();
			}
		}
	}
}
