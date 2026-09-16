using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool.Model;
using DeXin.EPG.Resource;

namespace DeXin.EPG.Tools.EPGUserControl
{
	public class DynamicConditionSearchNew : UserControl
	{
		private IContainer components = null;

		private Panel pnl;

		private Button btnAddCondition;

		private Button btnMoveCondition;

		private TableLayoutPanel tableLayoutPanel1;

		private ComboBox cmbField;

		private ComboBox cmbExpression;

		private TextBox txtValue;

		private RadioButton radOR;

		private RadioButton radAND;

		private Panel pnlRad;

		private int i = 0;

		private int j = 55;

		private Dictionary<string, string> _basekeys = new Dictionary<string, string>();

		private string _Conditions = "";

		private string ConditionsTemp;

		public Dictionary<string, string> basekeys
		{
			get
			{
				if (_basekeys == null)
				{
					return new Dictionary<string, string>();
				}
				return _basekeys;
			}
			set
			{
				_basekeys = value;
			}
		}

		[Description("生成的条件字符串")]
		[Category("自定义属性")]
		public string Conditions
		{
			get
			{
				_Conditions = ConditionsTemp;
				return _Conditions;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Tools.EPGUserControl.DynamicConditionSearchNew));
			this.pnl = new System.Windows.Forms.Panel();
			this.btnAddCondition = new System.Windows.Forms.Button();
			this.btnMoveCondition = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(this.pnl, "pnl");
			this.tableLayoutPanel1.SetColumnSpan(this.pnl, 5);
			this.pnl.Name = "pnl";
			resources.ApplyResources(this.btnAddCondition, "btnAddCondition");
			this.btnAddCondition.Name = "btnAddCondition";
			this.btnAddCondition.UseVisualStyleBackColor = true;
			this.btnAddCondition.Click += new System.EventHandler(btnAddCondition_Click);
			resources.ApplyResources(this.btnMoveCondition, "btnMoveCondition");
			this.btnMoveCondition.Name = "btnMoveCondition";
			this.btnMoveCondition.UseVisualStyleBackColor = true;
			this.btnMoveCondition.Click += new System.EventHandler(btnMoveCondition_Click);
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.pnl, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnMoveCondition, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnAddCondition, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.tableLayoutPanel1);
			base.Name = "SMSDynamicConditionSearchNew";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}

		public DynamicConditionSearchNew()
		{
			InitializeComponent();
		}

		private void CreateRadButton(ref int i)
		{
			int num = 20;
			int num2 = j * i;
			radOR = new RadioButton();
			radOR.AutoSize = true;
			radOR.Location = new Point(44, 3);
			radOR.Name = "radOR" + i;
			radOR.Size = new Size(35, 16);
			radOR.TabIndex = 5;
			radOR.TabStop = true;
			radOR.Text = ResourceManagement.GetLocationString("OR");
			radOR.UseVisualStyleBackColor = true;
			radOR.Checked = true;
			radAND = new RadioButton();
			radAND.AutoSize = true;
			radAND.Location = new Point(3, 3);
			radAND.Name = "radAND" + i;
			radAND.Size = new Size(35, 16);
			radAND.TabIndex = 5;
			radAND.TabStop = true;
			radAND.Text = ResourceManagement.GetLocationString("AND");
			radAND.UseVisualStyleBackColor = true;
			pnlRad = new Panel();
			pnlRad.Controls.Add(radAND);
			pnlRad.Controls.Add(radOR);
			pnlRad.Location = new Point(num, num2);
			pnlRad.Name = "pnlRad" + i;
			pnlRad.Size = new Size(84, 22);
			pnlRad.TabIndex = 6;
			pnl.Controls.Add(pnlRad);
			if (i == 0)
			{
				radOR.Checked = false;
				radAND.Checked = true;
				pnlRad.Visible = false;
			}
		}

		private void CreateComboBox(ref int i)
		{
			cmbField = new ComboBox();
			int num = 20;
			int num2 = 30 + j * i;
			cmbField.FormattingEnabled = true;
			cmbField.Location = new Point(num, num2);
			cmbField.Name = "cmbField" + i;
			cmbField.Size = new Size(109, 20);
			cmbField.TabIndex = 2;
			cmbField.DropDownStyle = ComboBoxStyle.DropDownList;
			pnl.Controls.Add(cmbField);
			cmbField.DataSource = CollectionToDataTable(basekeys);
			cmbField.DisplayMember = "BKValue";
			cmbField.ValueMember = "BKKey";
		}

		private void CreateExpressionCombo(ref int i)
		{
			int num = 150;
			int num2 = 30 + j * i;
			cmbExpression = new ComboBox();
			cmbExpression.FormattingEnabled = true;
			cmbExpression.Location = new Point(num, num2);
			cmbExpression.Name = "cmbExpression" + i;
			cmbExpression.Size = new Size(120, 20);
			cmbExpression.TabIndex = 3;
			cmbExpression.DropDownStyle = ComboBoxStyle.DropDownList;
			pnl.Controls.Add(cmbExpression);
			IList<BaseKey> list = new List<BaseKey>();
			list = new List<BaseKey>();
			list.Add(new BaseKey("LIKE", ResourceManagement.GetLocationString("Contain"), true));
			list.Add(new BaseKey("NOT LIKE", ResourceManagement.GetLocationString("NotContain"), true));
			list.Add(new BaseKey("=", ResourceManagement.GetLocationString("Equal"), true));
			list.Add(new BaseKey("!=", ResourceManagement.GetLocationString("NotEqual"), true));
			list.Add(new BaseKey(">", ResourceManagement.GetLocationString("Bigger"), true));
			list.Add(new BaseKey("<", ResourceManagement.GetLocationString("Smaller"), true));
			list.Add(new BaseKey(">=", ResourceManagement.GetLocationString("BiggerOrEqual"), true));
			list.Add(new BaseKey("<=", ResourceManagement.GetLocationString("SmallerOrEqual"), true));
			cmbExpression.DataSource = list;
			cmbExpression.DisplayMember = "BKValue";
			cmbExpression.ValueMember = "BKKey";
		}

		private void CreateTextBox(ref int i)
		{
			int num = 285;
			int num2 = 30 + j * i;
			txtValue = new TextBox();
			txtValue.Location = new Point(num, num2);
			txtValue.Name = "txtValue" + i;
			txtValue.Size = new Size(120, 21);
			txtValue.TabIndex = 4;
			pnl.Controls.Add(txtValue);
		}

		private void btnAddCondition_Click(object sender, EventArgs e)
		{
			if (i < basekeys.Keys.Count)
			{
				CreateRadButton(ref i);
				CreateComboBox(ref i);
				CreateExpressionCombo(ref i);
				CreateTextBox(ref i);
				i++;
			}
			else
			{
				MessageBox.Show(ResourceManagement.GetLocationString("CanNotAddCondtion"));
			}
		}

		private void btnMoveCondition_Click(object sender, EventArgs e)
		{
			if (i > 0)
			{
				i--;
				pnl.Controls.RemoveByKey("cmbField" + i);
				pnl.Controls.RemoveByKey("cmbExpression" + i);
				pnl.Controls.RemoveByKey("pnlRad" + i);
				pnl.Controls.RemoveByKey("txtValue" + i);
			}
			else
			{
				MessageBox.Show(ResourceManagement.GetLocationString("CanNotRemoveCondtion"));
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			PrepareCondition();
		}

		public void PrepareCondition()
		{
			ConditionsTemp = "";
			if (!pnl.HasChildren)
			{
				return;
			}
			ControlCollection controls = pnl.Controls;
			for (int i = 0; i < this.i; i++)
			{
				string text = "";
				ComboBox comboBox = (ComboBox)controls.Find("cmbField" + i, false).FirstOrDefault();
				ComboBox comboBox2 = (ComboBox)controls.Find("cmbExpression" + i, false).FirstOrDefault();
				Panel panel = (Panel)controls.Find("pnlRad" + i, true).FirstOrDefault();
				foreach (Control control in panel.Controls)
				{
					if (control.GetType().ToString() == "System.Windows.Forms.RadioButton" && ((RadioButton)control).Name.Contains("radAND"))
					{
						text = ((!((RadioButton)control).Checked) ? " OR " : " AND ");
					}
				}
				TextBox textBox = (TextBox)controls.Find("txtValue" + i, false).FirstOrDefault();
				string text2 = comboBox.SelectedValue.ToString();
				string text3 = comboBox2.SelectedValue.ToString();
				string text4 = textBox.Text;
				if (text3.Equals("LIKE"))
				{
					string conditionsTemp = ConditionsTemp;
					ConditionsTemp = conditionsTemp + text + " CHARINDEX('" + text4 + "'," + text2 + ")>0 ";
				}
				else if (text3.Equals("NOT LIKE"))
				{
					string conditionsTemp = ConditionsTemp;
					ConditionsTemp = conditionsTemp + text + " CHARINDEX('" + text4 + "'," + text2 + ")<0 ";
				}
				else
				{
					string conditionsTemp = ConditionsTemp;
					ConditionsTemp = conditionsTemp + text + " " + text2 + " " + text3 + " '" + text4 + "' ";
				}
			}
		}

		private DataTable CollectionToDataTable(IList<BaseKey> basekeys)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("BKKey");
			dataTable.Columns.Add("BKValue");
			DataRow dataRow = null;
			foreach (BaseKey basekey in basekeys)
			{
				dataRow = dataTable.NewRow();
				dataRow["BKKey"] = basekey.BKKey;
				dataRow["BKValue"] = basekey.BKValue;
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}

		private DataTable CollectionToDataTable(Dictionary<string, string> basekeys)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("BKKey");
			dataTable.Columns.Add("BKValue");
			DataRow dataRow = null;
			foreach (KeyValuePair<string, string> basekey in basekeys)
			{
				dataRow = dataTable.NewRow();
				dataRow["BKKey"] = basekey.Key;
				dataRow["BKValue"] = basekey.Value;
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}
	}
}
