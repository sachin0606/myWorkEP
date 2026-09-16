using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TsSenders
{
	public class Form2 : Form
	{
		private bool isStop;

		private int package_size = 188;

		private decimal default_total_rate = 100m;

		private decimal total_size;

		private decimal total_rate;

		private decimal active_rate;

		private UdpClient udp = new UdpClient(new IPEndPoint(IPAddress.Parse("192.168.19.1"), 0));

		private IPEndPoint remote_endpoin = new IPEndPoint(IPAddress.Parse("224.2.2.2"), 1002);

		private Random random = new Random();

		private int send_count;

		private DateTime send_start_time = DateTime.Now;

		private IContainer components;

		private Button btn_active;

		private Label label1;

		private Button btn_supply;

		private Label label2;

		private Button btn_stop;

		private Label label3;

		private TextBox txt_total_rate;

		public Form2()
		{
			InitializeComponent();
		}

		private void btn_active_Click(object sender, EventArgs e)
		{
			total_size = 0m;
			send_count = 0;
			default_total_rate = Convert.ToDecimal(txt_total_rate.Text);
			isStop = false;
			List<byte[]> packages = new List<byte[]>();
			for (int i = 0; i < 100; i++)
			{
				byte[] array = new byte[package_size];
				for (int j = 0; j < package_size; j++)
				{
					array[j] = byte.MaxValue;
				}
				packages.Add(array);
			}
			Thread thread = new Thread((ThreadStart)delegate
			{
				int num = 0;
				decimal num2 = 0m;
				DateTime now = DateTime.Now;
				while (!isStop && !base.Disposing && !base.IsDisposed)
				{
					if (num == packages.Count)
					{
						num = 0;
					}
					if (num % 16777215 == 0)
					{
						num2 = 0m;
						now = DateTime.Now;
						total_size = 0m;
						send_start_time = now;
					}
					send(packages[num++]);
					num2 += (decimal)package_size;
					double totalMilliseconds = (DateTime.Now - now).TotalMilliseconds;
					try
					{
						active_rate = num2 / (decimal)totalMilliseconds;
						label1.Invoke((Action)delegate
						{
							label1.Text = active_rate.ToString();
						});
					}
					catch
					{
					}
					int millisecondsTimeout = random.Next(1, 500);
					Thread.Sleep(millisecondsTimeout);
				}
			});
			thread.Priority = ThreadPriority.Highest;
			thread.Start();
		}

		private void btn_supply_Click(object sender, EventArgs e)
		{
			byte[] empty_package = new byte[package_size];
			for (int i = 0; i < empty_package.Length; i++)
			{
				empty_package[i] = byte.MaxValue;
			}
			new Thread((ThreadStart)delegate
			{
				while (!isStop && !base.Disposing && !base.IsDisposed)
				{
					double totalMilliseconds = (DateTime.Now - send_start_time).TotalMilliseconds;
					try
					{
						total_rate = total_size / (decimal)totalMilliseconds;
						decimal num = default_total_rate - total_rate;
						Console.WriteLine("total_rate:{0:0.0000}   active_rate:{1:0.0000}", total_rate, active_rate);
						if (num > 0m)
						{
							decimal num2 = num * 1024m;
							int num3 = (int)(num2 / (decimal)package_size + 1m);
							if (num3 > 100)
							{
								num3 = 100;
							}
							for (int j = 0; j < num3; j++)
							{
								send(empty_package);
							}
							totalMilliseconds = (DateTime.Now - send_start_time).TotalMilliseconds;
							total_rate = total_size / (decimal)totalMilliseconds;
						}
						label2.Invoke((Action)delegate
						{
							label2.Text = total_rate.ToString();
						});
					}
					catch
					{
					}
					Thread.Sleep(1);
				}
			}).Start();
		}

		private void send(byte[] package)
		{
			total_size += (decimal)package_size;
			udp.Send(package, package_size, remote_endpoin);
		}

		private void btn_stop_Click(object sender, EventArgs e)
		{
			byte[] bytes = new byte[2] { 183, 209 };
			string value = Encoding.BigEndianUnicode.GetString(bytes);
			Console.WriteLine(value);
			isStop = true;
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
			this.btn_active = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btn_supply = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.btn_stop = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txt_total_rate = new System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.btn_active.Location = new System.Drawing.Point(76, 121);
			this.btn_active.Name = "btn_active";
			this.btn_active.Size = new System.Drawing.Size(117, 23);
			this.btn_active.TabIndex = 0;
			this.btn_active.Text = "有效数据发送";
			this.btn_active.UseVisualStyleBackColor = true;
			this.btn_active.Click += new System.EventHandler(btn_active_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(31, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			this.btn_supply.Location = new System.Drawing.Point(76, 150);
			this.btn_supply.Name = "btn_supply";
			this.btn_supply.Size = new System.Drawing.Size(117, 23);
			this.btn_supply.TabIndex = 2;
			this.btn_supply.Text = "补充";
			this.btn_supply.UseVisualStyleBackColor = true;
			this.btn_supply.Click += new System.EventHandler(btn_supply_Click);
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(31, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "label2";
			this.btn_stop.Location = new System.Drawing.Point(76, 179);
			this.btn_stop.Name = "btn_stop";
			this.btn_stop.Size = new System.Drawing.Size(117, 23);
			this.btn_stop.TabIndex = 4;
			this.btn_stop.Text = "停止";
			this.btn_stop.UseVisualStyleBackColor = true;
			this.btn_stop.Click += new System.EventHandler(btn_stop_Click);
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(31, 84);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "set rate(KB)";
			this.txt_total_rate.Location = new System.Drawing.Point(114, 81);
			this.txt_total_rate.MaxLength = 5;
			this.txt_total_rate.Name = "txt_total_rate";
			this.txt_total_rate.Size = new System.Drawing.Size(62, 21);
			this.txt_total_rate.TabIndex = 6;
			this.txt_total_rate.Text = "10";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(294, 205);
			base.Controls.Add(this.txt_total_rate);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.btn_stop);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.btn_supply);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btn_active);
			base.Name = "Form2";
			this.Text = "Form2";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
