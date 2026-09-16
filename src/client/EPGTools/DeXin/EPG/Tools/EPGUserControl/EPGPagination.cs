using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com.Cn.Tool;

namespace DeXin.EPG.Tools.EPGUserControl
{
	public class EPGPagination : UserControl
	{
		private int _PageIndex = 0;

		private int _PageCount = 1;

		private int _PageSize = -1;

		private int _TotleCount = 0;

		private IContainer components = null;

		private Button btnLast;

		private Button btnNext;

		private Button btnPrevious;

		private Button btnFirst;

		private Button btnGo;

		private Label lblTotle;

		private TextBox txtPageIndex;

		private Label label1;

		private FlowLayoutPanel flowLayoutPanel1;

		private Label lblShowTotal;

		private Label lblTolal;

		[Description("当前页码下标")]
		[Category("自定义属性")]
		public int PageIndex
		{
			get
			{
				return _PageIndex;
			}
			set
			{
				_PageIndex = value;
				txtPageIndex.Text = (_PageIndex + 1).ToString();
				ChangeButtonEnable();
			}
		}

		[Category("属性")]
		[Description("总页数")]
		protected int PageCount
		{
			get
			{
				return _PageCount;
			}
			set
			{
				_PageCount = value;
				ChangeButtonEnable();
			}
		}

		[Category("自定义属性")]
		[Description("每页记录数")]
		public int PageSize
		{
			get
			{
				if (_PageSize <= 0)
				{
					_PageSize = 25;
				}
				return _PageSize;
			}
			set
			{
				_PageSize = value;
			}
		}

		[Category("自定义属性")]
		[Description("总记录数")]
		public int TotleCount
		{
			get
			{
				return _TotleCount;
			}
			set
			{
				_TotleCount = value;
				if (PageSize == 0)
				{
					PageCount = 1;
				}
				if (_TotleCount == 0 || _TotleCount == 1)
				{
					if (PageIndex != 0)
					{
						PageIndex = 0;
					}
					PageCount = 1;
				}
				else
				{
					PageCount = ((PageSize == 0) ? 1 : ((_TotleCount - 1) / PageSize + 1));
				}
				lblTotle.Text = PageCount.ToString();
				lblShowTotal.Text = _TotleCount.ToString();
			}
		}

		[Category("自定义事件")]
		[Description("语言改变后触发")]
		public event EventHandler LanguageChange;

		[Category("自定义事件")]
		[Description("页码改变后触发")]
		public event EventHandler PageIndexChanged;

		[Category("自定义事件")]
		[Description("每页行数改变后触发")]
		public event EventHandler PageSizeChanged;

		public EPGPagination()
		{
			InitializeComponent();
		}

		protected void OnLanguageChanged(EventArgs e)
		{
			if (this.LanguageChange != null)
			{
				this.LanguageChange(this, e);
			}
		}

		protected void OnPageIndexChanged(EventArgs e)
		{
			if (this.PageIndexChanged != null)
			{
				this.PageIndexChanged(this, e);
			}
		}

		protected void OnPageSizeChanged(EventArgs e)
		{
			if (this.PageSizeChanged != null)
			{
				this.PageSizeChanged(this, e);
			}
		}

		private void SMSPagination_Load(object sender, EventArgs e)
		{
			PageIndexChanged += SMSPagination_PageIndexChanged;
			LanguageChange += SMSPagination_LanguageChange;
		}

		public void SMSPagination_LanguageChange(object sender, EventArgs e)
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EPGPagination));
			componentResourceManager.ApplyResources(btnFirst, "btnFirst");
			componentResourceManager.ApplyResources(btnLast, "btnLast");
			componentResourceManager.ApplyResources(btnNext, "btnNext");
			componentResourceManager.ApplyResources(btnPrevious, "btnPrevious");
			componentResourceManager.ApplyResources(btnGo, "btnGo");
			componentResourceManager.ApplyResources(lblTolal, "lblTolal");
		}

		private void btn_Click(object sender, EventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				switch (button.Name)
				{
				case "btnFirst":
					PageIndex = 0;
					break;
				case "btnPrevious":
					PageIndex--;
					break;
				case "btnNext":
					PageIndex++;
					break;
				case "btnLast":
					PageIndex = PageCount - 1;
					break;
				case "btnGo":
				{
					int num = Tool.GetInt32(txtPageIndex.Text) - 1;
					PageIndex = ((num >= 0) ? ((num >= PageCount - 1) ? (PageCount - 1) : num) : 0);
					break;
				}
				}
			}
			txtPageIndex.Text = (PageIndex + 1).ToString();
			OnPageIndexChanged(EventArgs.Empty);
		}

		private void SMSPagination_PageIndexChanged(object sender, EventArgs e)
		{
			ChangeButtonEnable();
		}

		private void txtPageIndex_KeyPress(object sender, KeyPressEventArgs e)
		{
			int keyChar = e.KeyChar;
			if (keyChar == 13)
			{
				btn_Click(btnGo, e);
			}
		}

		private void ChangeButtonEnable()
		{
			btnFirst.Enabled = true;
			btnPrevious.Enabled = true;
			btnNext.Enabled = true;
			btnLast.Enabled = true;
			if (PageIndex == 0)
			{
				btnFirst.Enabled = false;
				btnPrevious.Enabled = false;
			}
			if (PageCount == 0 || PageIndex == PageCount - 1)
			{
				btnNext.Enabled = false;
				btnLast.Enabled = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Tools.EPGUserControl.EPGPagination));
			this.btnLast = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.btnFirst = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.lblTotle = new System.Windows.Forms.Label();
			this.txtPageIndex = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblShowTotal = new System.Windows.Forms.Label();
			this.lblTolal = new System.Windows.Forms.Label();
			this.flowLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(this.btnLast, "btnLast");
			this.btnLast.Name = "btnLast";
			this.btnLast.UseVisualStyleBackColor = true;
			this.btnLast.Click += new System.EventHandler(btn_Click);
			resources.ApplyResources(this.btnNext, "btnNext");
			this.btnNext.Name = "btnNext";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(btn_Click);
			resources.ApplyResources(this.btnPrevious, "btnPrevious");
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(btn_Click);
			resources.ApplyResources(this.btnFirst, "btnFirst");
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.UseVisualStyleBackColor = true;
			this.btnFirst.Click += new System.EventHandler(btn_Click);
			resources.ApplyResources(this.btnGo, "btnGo");
			this.btnGo.Name = "btnGo";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(btn_Click);
			resources.ApplyResources(this.lblTotle, "lblTotle");
			this.lblTotle.Name = "lblTotle";
			resources.ApplyResources(this.txtPageIndex, "txtPageIndex");
			this.txtPageIndex.Name = "txtPageIndex";
			this.txtPageIndex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPageIndex_KeyPress);
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.flowLayoutPanel1.Controls.Add(this.btnLast);
			this.flowLayoutPanel1.Controls.Add(this.btnNext);
			this.flowLayoutPanel1.Controls.Add(this.btnPrevious);
			this.flowLayoutPanel1.Controls.Add(this.btnFirst);
			this.flowLayoutPanel1.Controls.Add(this.btnGo);
			this.flowLayoutPanel1.Controls.Add(this.lblTotle);
			this.flowLayoutPanel1.Controls.Add(this.label1);
			this.flowLayoutPanel1.Controls.Add(this.txtPageIndex);
			this.flowLayoutPanel1.Controls.Add(this.lblShowTotal);
			this.flowLayoutPanel1.Controls.Add(this.lblTolal);
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.lblShowTotal, "lblShowTotal");
			this.lblShowTotal.Name = "lblShowTotal";
			resources.ApplyResources(this.lblTolal, "lblTolal");
			this.lblTolal.Name = "lblTolal";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.flowLayoutPanel1);
			base.Name = "EPGPagination";
			base.Load += new System.EventHandler(SMSPagination_Load);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
