using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace TsSenders
{
	public class FormSender : Form
	{
		private IContainer components;

		private GroupBox gbButtons;

		private GroupBox gbInfo;

		private TableLayoutPanel tableLayoutPanel2;

		private TableLayoutPanel tableLayoutPanel1;

		private Button btnSend;

		private Button btnStop;

		private Label lblfilepath;

		private TextBox txtPath;

		private Button btnselectfile;

		private Label label2;

		private TextBox txtRate;

		private Label label1;

		private TableLayoutPanel tableLayoutPanel3;

		private RadioButton rb_sendmodel_plyer;

		private RadioButton rb_sendmodel_ip;

		private Label label3;

		private CheckBox cbo_isloop;

		private Label label4;

		private TextBox txtip;

		private TextBox txtport;

		private Label label5;

		private ComboBox cmblocalip;

		private decimal rate;

		private string path = "";

		private IPAddress ip;

		private int port;

		private bool loop;

		private SenderModels sendermodel
		{
			get
			{
				if (!rb_sendmodel_ip.Checked)
				{
					return SenderModels.Plyer_Box;
				}
				return SenderModels.IP;
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
			this.gbButtons = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnSend = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.gbInfo = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbo_isloop = new System.Windows.Forms.CheckBox();
			this.txtip = new System.Windows.Forms.TextBox();
			this.txtport = new System.Windows.Forms.TextBox();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.btnselectfile = new System.Windows.Forms.Button();
			this.lblfilepath = new System.Windows.Forms.Label();
			this.txtRate = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cmblocalip = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.rb_sendmodel_plyer = new System.Windows.Forms.RadioButton();
			this.rb_sendmodel_ip = new System.Windows.Forms.RadioButton();
			this.gbButtons.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.gbInfo.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			base.SuspendLayout();
			this.gbButtons.Controls.Add(this.tableLayoutPanel2);
			this.gbButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.gbButtons.Location = new System.Drawing.Point(0, 240);
			this.gbButtons.Name = "gbButtons";
			this.gbButtons.Size = new System.Drawing.Size(471, 62);
			this.gbButtons.TabIndex = 0;
			this.gbButtons.TabStop = false;
			this.gbButtons.Text = "operate";
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel2.Controls.Add(this.btnSend, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.btnStop, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(465, 42);
			this.tableLayoutPanel2.TabIndex = 0;
			this.btnSend.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnSend.Location = new System.Drawing.Point(144, 9);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 23);
			this.btnSend.TabIndex = 0;
			this.btnSend.Text = "send";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(btnSend_Click);
			this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnStop.Enabled = false;
			this.btnStop.Location = new System.Drawing.Point(245, 9);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 1;
			this.btnStop.Text = "stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(btnStop_Click);
			this.gbInfo.Controls.Add(this.tableLayoutPanel1);
			this.gbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbInfo.Location = new System.Drawing.Point(0, 0);
			this.gbInfo.Name = "gbInfo";
			this.gbInfo.Size = new System.Drawing.Size(471, 240);
			this.gbInfo.TabIndex = 0;
			this.gbInfo.TabStop = false;
			this.gbInfo.Text = "setting";
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.cbo_isloop, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.txtip, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtport, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.txtPath, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnselectfile, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblfilepath, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtRate, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.cmblocalip, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 8;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(465, 220);
			this.tableLayoutPanel1.TabIndex = 0;
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(51, 122);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(17, 12);
			this.label3.TabIndex = 8;
			this.label3.Text = "IP";
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(39, 149);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 12);
			this.label4.TabIndex = 9;
			this.label4.Text = "port";
			this.cbo_isloop.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cbo_isloop.AutoSize = true;
			this.cbo_isloop.Location = new System.Drawing.Point(74, 172);
			this.cbo_isloop.Name = "cbo_isloop";
			this.cbo_isloop.Size = new System.Drawing.Size(48, 16);
			this.cbo_isloop.TabIndex = 7;
			this.cbo_isloop.Text = "loop";
			this.cbo_isloop.UseVisualStyleBackColor = true;
			this.txtip.Location = new System.Drawing.Point(74, 118);
			this.txtip.MaxLength = 15;
			this.txtip.Name = "txtip";
			this.txtip.Size = new System.Drawing.Size(145, 21);
			this.txtip.TabIndex = 10;
			this.txtip.Text = "224.2.2.2";
			this.txtport.Location = new System.Drawing.Point(74, 145);
			this.txtport.MaxLength = 5;
			this.txtport.Name = "txtport";
			this.txtport.Size = new System.Drawing.Size(145, 21);
			this.txtport.TabIndex = 11;
			this.txtport.Text = "1001";
			this.txtPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtPath.Location = new System.Drawing.Point(74, 4);
			this.txtPath.Name = "txtPath";
			this.txtPath.ReadOnly = true;
			this.txtPath.Size = new System.Drawing.Size(310, 21);
			this.txtPath.TabIndex = 1;
			this.btnselectfile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnselectfile.Location = new System.Drawing.Point(390, 3);
			this.btnselectfile.Name = "btnselectfile";
			this.btnselectfile.Size = new System.Drawing.Size(40, 23);
			this.btnselectfile.TabIndex = 2;
			this.btnselectfile.Text = "...";
			this.btnselectfile.UseVisualStyleBackColor = true;
			this.btnselectfile.Click += new System.EventHandler(btnselectfile_Click);
			this.lblfilepath.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblfilepath.AutoSize = true;
			this.lblfilepath.Location = new System.Drawing.Point(9, 8);
			this.lblfilepath.Name = "lblfilepath";
			this.lblfilepath.Size = new System.Drawing.Size(59, 12);
			this.lblfilepath.TabIndex = 0;
			this.lblfilepath.Text = "file path";
			this.txtRate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtRate.Location = new System.Drawing.Point(74, 32);
			this.txtRate.MaxLength = 6;
			this.txtRate.Name = "txtRate";
			this.txtRate.Size = new System.Drawing.Size(145, 21);
			this.txtRate.TabIndex = 4;
			this.txtRate.Text = "1";
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "rate(MB)";
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(33, 96);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(35, 12);
			this.label5.TabIndex = 12;
			this.label5.Text = "local";
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 66);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 12);
			this.label1.TabIndex = 5;
			this.label1.Text = "send model";
			this.cmblocalip.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cmblocalip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmblocalip.FormattingEnabled = true;
			this.cmblocalip.Location = new System.Drawing.Point(74, 92);
			this.cmblocalip.Name = "cmblocalip";
			this.cmblocalip.Size = new System.Drawing.Size(145, 20);
			this.cmblocalip.TabIndex = 13;
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel3.Controls.Add(this.rb_sendmodel_plyer, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.rb_sendmodel_ip, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(74, 59);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(310, 27);
			this.tableLayoutPanel3.TabIndex = 6;
			this.rb_sendmodel_plyer.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.rb_sendmodel_plyer.AutoSize = true;
			this.rb_sendmodel_plyer.Location = new System.Drawing.Point(148, 5);
			this.rb_sendmodel_plyer.Name = "rb_sendmodel_plyer";
			this.rb_sendmodel_plyer.Size = new System.Drawing.Size(59, 16);
			this.rb_sendmodel_plyer.TabIndex = 0;
			this.rb_sendmodel_plyer.Text = "player";
			this.rb_sendmodel_plyer.UseVisualStyleBackColor = true;
			this.rb_sendmodel_plyer.CheckedChanged += new System.EventHandler(rb_sendmodel_CheckedChanged);
			this.rb_sendmodel_ip.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.rb_sendmodel_ip.AutoSize = true;
			this.rb_sendmodel_ip.Checked = true;
			this.rb_sendmodel_ip.Location = new System.Drawing.Point(3, 5);
			this.rb_sendmodel_ip.Name = "rb_sendmodel_ip";
			this.rb_sendmodel_ip.Size = new System.Drawing.Size(35, 16);
			this.rb_sendmodel_ip.TabIndex = 0;
			this.rb_sendmodel_ip.TabStop = true;
			this.rb_sendmodel_ip.Text = "IP";
			this.rb_sendmodel_ip.UseVisualStyleBackColor = true;
			this.rb_sendmodel_ip.CheckedChanged += new System.EventHandler(rb_sendmodel_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(471, 302);
			base.Controls.Add(this.gbInfo);
			base.Controls.Add(this.gbButtons);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Name = "FormSender";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sender";
			base.Load += new System.EventHandler(FormSender_Load);
			this.gbButtons.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.gbInfo.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			base.ResumeLayout(false);
		}

		public FormSender()
		{
			InitializeComponent();
			if (base.Owner != null)
			{
				base.Icon = base.Owner.Icon;
			}
			SenderHelper.Ins.SenderStpoedHandller += delegate
			{
				checkstate(false);
			};
		}

		private void FormSender_Load(object sender, EventArgs e)
		{
			List<string> list = new List<string>();
			IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
			foreach (IPAddress iPAddress in hostAddresses)
			{
				if (!iPAddress.IsIPv6LinkLocal && !iPAddress.IsIPv6Multicast && !iPAddress.IsIPv6SiteLocal && !iPAddress.IsIPv6Teredo)
				{
					list.Add(iPAddress.ToString());
				}
			}
			cmblocalip.DataSource = list;
		}

		private void check()
		{
			try
			{
				rate = Convert.ToDecimal(txtRate.Text);
			}
			catch (Exception)
			{
				txtRate.Text = "";
				txtRate.Focus();
				rate = 0m;
			}
			if (rate <= 0m)
			{
				txtRate.Focus();
				throw new Exception("rate is error!");
			}
			path = txtPath.Text;
			if (!File.Exists(path))
			{
				txtPath.Focus();
				throw new FileNotFoundException();
			}
			if (sendermodel == SenderModels.IP)
			{
				ip = IPAddress.Parse(txtip.Text);
				port = Convert.ToInt32(txtport.Text);
				if (port < 1 || port > 65535)
				{
					throw new Exception("port is error!");
				}
			}
			loop = cbo_isloop.Checked;
		}

		private void checkstate(bool state)
		{
			if (base.InvokeRequired)
			{
				Invoke((Action)delegate
				{
					checkstates(state);
				});
			}
			else
			{
				checkstates(state);
			}
		}

		private void checkstates(bool state)
		{
			btnStop.Enabled = state;
			Button button = btnSend;
			bool enabled = (btnselectfile.Enabled = !state);
			button.Enabled = enabled;
			if (btnSend.Enabled)
			{
				txtRate.ReadOnly = false;
				CheckBox checkBox = cbo_isloop;
				RadioButton radioButton = rb_sendmodel_plyer;
				bool flag2 = (rb_sendmodel_ip.Enabled = true);
				bool enabled2 = (radioButton.Enabled = flag2);
				checkBox.Enabled = enabled2;
				TextBox textBox = txtip;
				bool readOnly = (txtport.ReadOnly = rb_sendmodel_plyer.Checked);
				textBox.ReadOnly = readOnly;
				cmblocalip.Enabled = !rb_sendmodel_plyer.Checked;
			}
			else
			{
				txtRate.ReadOnly = true;
				CheckBox checkBox2 = cbo_isloop;
				RadioButton radioButton2 = rb_sendmodel_plyer;
				bool flag6 = (rb_sendmodel_ip.Enabled = false);
				bool enabled3 = (radioButton2.Enabled = flag6);
				checkBox2.Enabled = enabled3;
				TextBox textBox2 = txtip;
				bool readOnly2 = (txtport.ReadOnly = true);
				textBox2.ReadOnly = readOnly2;
				cmblocalip.Enabled = false;
			}
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			try
			{
				check();
				checkstate(true);
				if (sendermodel == SenderModels.IP)
				{
					UdpClient client = new UdpClient();
					if (cmblocalip.SelectedItem != null)
					{
						client = new UdpClient(new IPEndPoint(IPAddress.Parse(cmblocalip.SelectedItem.ToString()), 0));
					}
					SenderHelper.Ins.Init(loop, SenderModels.IP);
					SenderHelper.Ins.Send(path, rate, client, new IPEndPoint(ip, port));
				}
				else if (sendermodel == SenderModels.Plyer_Box)
				{
					SenderHelper.CheckPlayer(rate);
					SenderHelper.Ins.Init(loop, SenderModels.Plyer_Box);
					SenderHelper.Ins.Send(path, rate);
				}
			}
			catch (Exception ex)
			{
				SenderHelper.Ins.Dispose();
				if (ex != null)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			SenderHelper.Ins.Dispose();
		}

		private void rb_sendmodel_CheckedChanged(object sender, EventArgs e)
		{
			checkstate(!btnSend.Enabled);
		}

		private void btnselectfile_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				txtPath.Text = openFileDialog.FileName;
			}
		}
	}
}
