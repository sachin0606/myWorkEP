using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TsSenders
{
	public class s : Form
	{
		private bool flag;

		private IContainer components;

		private Button btnSelete;

		private Label label1;

		private TextBox txtPath;

		private Label label2;

		private TextBox txtRate;

		private Button btnSend;

		private Button btnStop;

		private Label label3;

		private TextBox txtIP;

		private Label label4;

		private TextBox txtPort;

		private CheckBox cboIsCalcRate;

		private Button btnSendWithRate;

		private TextBox txtSendCount;

		public s()
		{
			InitializeComponent();
		}

		private void btnSelete_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				txtPath.Text = openFileDialog.FileName;
			}
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			SenderHelper.Ins.Dispose();
			decimal num = 0m;
			try
			{
				num = Convert.ToDecimal(txtRate.Text);
			}
			catch (Exception)
			{
				txtRate.Text = "";
				txtRate.Focus();
				num = 0m;
			}
			if (num <= 0m)
			{
				txtRate.Focus();
				return;
			}
			string path = txtPath.Text;
			new Thread((ThreadStart)delegate
			{
				List<byte> list = new List<byte>();
				int num2 = 1024;
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					byte[] array = new byte[num2];
					while (fileStream.Length - fileStream.Position >= num2)
					{
						fileStream.Read(array, 0, num2);
						list.AddRange(array);
					}
					num2 = (int)(fileStream.Length - fileStream.Position);
					array = new byte[num2];
					fileStream.Read(array, 0, num2);
					list.AddRange(array);
				}
			}).Start();
		}

		private void Send(string path, decimal rate, IPAddress ip, int port)
		{
			flag = true;
			if (!File.Exists(path))
			{
				new FileNotFoundException();
			}
			int num = 100;
			DateTime now = DateTime.Now;
			int num2 = (int)(rate * 1024m * ((decimal)num / 1000m));
			byte[] array = new byte[0];
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
			}
			IPEndPoint endPoint = new IPEndPoint(ip, port);
			using (UdpClient udpClient = new UdpClient())
			{
				now = DateTime.Now;
				do
				{
					byte[] dgram = new byte[num2];
					int num3 = num2;
					int num4 = 0;
					while (num4 < array.Length && flag)
					{
						num3 = ((num4 + num2 <= array.Length) ? num2 : (array.Length - num4));
						num4 += num3;
						udpClient.Send(dgram, num3, endPoint);
						double totalMilliseconds = (DateTime.Now - now).TotalMilliseconds;
						double num5 = (double)(num4 * 100) / totalMilliseconds;
						Console.WriteLine("current rate:{0}/{1}   {2}/{3}", num5, num2, num4, array.Length);
						if (num5 > (double)num2)
						{
							Thread.Sleep(num);
						}
					}
					now = DateTime.Now;
				}
				while (flag);
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			SenderHelper.Ins.Dispose();
			flag = false;
		}

		private void btnSendWithRate_Click(object sender, EventArgs e)
		{
			decimal rate = Convert.ToDecimal(txtRate.Text);
			bool loop = cboIsCalcRate.Checked;
			string ipString = txtIP.Text;
			string text = txtPort.Text;
			SenderHelper.Ins.Init(loop, SenderModels.IP);
			SenderHelper.Ins.Send(txtPath.Text, rate, new UdpClient(), new IPEndPoint(IPAddress.Parse(ipString), int.Parse(text)));
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
			this.btnSelete = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtRate = new System.Windows.Forms.TextBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtIP = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.cboIsCalcRate = new System.Windows.Forms.CheckBox();
			this.btnSendWithRate = new System.Windows.Forms.Button();
			this.txtSendCount = new System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.btnSelete.Location = new System.Drawing.Point(357, 68);
			this.btnSelete.Name = "btnSelete";
			this.btnSelete.Size = new System.Drawing.Size(31, 23);
			this.btnSelete.TabIndex = 0;
			this.btnSelete.Text = "...";
			this.btnSelete.UseVisualStyleBackColor = true;
			this.btnSelete.Click += new System.EventHandler(btnSelete_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(42, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "Path";
			this.txtPath.Location = new System.Drawing.Point(89, 70);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(262, 21);
			this.txtPath.TabIndex = 2;
			this.txtPath.Text = "F:\\MyGathering\\视频\\mv\\ice_age.ts";
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(42, 110);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "Rate";
			this.txtRate.Location = new System.Drawing.Point(89, 107);
			this.txtRate.Name = "txtRate";
			this.txtRate.Size = new System.Drawing.Size(100, 21);
			this.txtRate.TabIndex = 4;
			this.txtRate.Text = "1";
			this.btnSend.Location = new System.Drawing.Point(195, 105);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 23);
			this.btnSend.TabIndex = 5;
			this.btnSend.Text = "Send";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(btnSend_Click);
			this.btnStop.Location = new System.Drawing.Point(276, 105);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 6;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(btnStop_Click);
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(54, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(17, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "IP";
			this.txtIP.Location = new System.Drawing.Point(89, 38);
			this.txtIP.Name = "txtIP";
			this.txtIP.Size = new System.Drawing.Size(100, 21);
			this.txtIP.TabIndex = 8;
			this.txtIP.Text = "224.2.2.2";
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(195, 41);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 12);
			this.label4.TabIndex = 9;
			this.label4.Text = "Port";
			this.txtPort.Location = new System.Drawing.Point(230, 38);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(121, 21);
			this.txtPort.TabIndex = 10;
			this.txtPort.Text = "5006";
			this.cboIsCalcRate.AutoSize = true;
			this.cboIsCalcRate.Location = new System.Drawing.Point(89, 184);
			this.cboIsCalcRate.Name = "cboIsCalcRate";
			this.cboIsCalcRate.Size = new System.Drawing.Size(72, 16);
			this.cboIsCalcRate.TabIndex = 11;
			this.cboIsCalcRate.Text = "显示码率";
			this.cboIsCalcRate.UseVisualStyleBackColor = true;
			this.btnSendWithRate.Location = new System.Drawing.Point(195, 142);
			this.btnSendWithRate.Name = "btnSendWithRate";
			this.btnSendWithRate.Size = new System.Drawing.Size(94, 23);
			this.btnSendWithRate.TabIndex = 12;
			this.btnSendWithRate.Text = "SendWithRate";
			this.btnSendWithRate.UseVisualStyleBackColor = true;
			this.btnSendWithRate.Click += new System.EventHandler(btnSendWithRate_Click);
			this.txtSendCount.Location = new System.Drawing.Point(89, 144);
			this.txtSendCount.Name = "txtSendCount";
			this.txtSendCount.Size = new System.Drawing.Size(100, 21);
			this.txtSendCount.TabIndex = 13;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(449, 212);
			base.Controls.Add(this.txtSendCount);
			base.Controls.Add(this.btnSendWithRate);
			base.Controls.Add(this.cboIsCalcRate);
			base.Controls.Add(this.txtPort);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.txtIP);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.btnStop);
			base.Controls.Add(this.btnSend);
			base.Controls.Add(this.txtRate);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.txtPath);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnSelete);
			base.Name = "s";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
