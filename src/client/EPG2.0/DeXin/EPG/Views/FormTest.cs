using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.TSFileHelper;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views
{
	public class FormTest : FormToolBase
	{
		private IContainer components = null;

		private OpenFileDialog openFileDialog1;

		private Button button1;

		public FormTest()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			string fileName = openFileDialog1.FileName;
			TSFileExpert tSFileExpert = new TSFileExpert();
			NitInfo nit = new NitInfo();
			Dictionary<int, byte[]> dictionary = tSFileExpert.ConvertToSections(fileName, 16, 0, 0);
			if (dictionary.Count > 0)
			{
				byte[] value = dictionary.First().Value;
				nit.NetworkId = value[3] | value[4];
				nit.VersionNumber = -1;
				foreach (int item in dictionary.Keys.OrderBy((int x) => x))
				{
					byte[] array = dictionary[item];
					int num = ((array[8] & 0xF) << 8) | array[9];
					byte b2;
					for (int num2 = 10; num2 < num + 10; num2 += b2)
					{
						byte b = array[num2++];
						b2 = array[num2++];
						if (b == 64)
						{
							byte[] array2 = new byte[b2];
							Array.Copy(array, num2, array2, 0, b2);
							nit.NetworkName = Encoding.Default.GetString(array2);
							break;
						}
					}
					if (!string.IsNullOrEmpty(nit.NetworkName))
					{
						break;
					}
				}
				int num3 = 1;
				do
				{
					Dictionary<int, byte[]> dictionary2 = tSFileExpert.ConvertToSections(fileName, 18, num3, 0);
					if (dictionary2.Count > 0)
					{
						int num4 = 14;
						byte[] value2 = dictionary2.Last().Value;
						while (num4 + 11 < value2.Length)
						{
							num4 += 10;
							int num = ((value2[num4] & 0xF) << 8) | value2[num4 + 1];
							num4 += 2;
							while (num > 0)
							{
								byte b = value2[num4++];
								byte b2 = value2[num4++];
								if (b == 77)
								{
									byte[] array3 = new byte[3];
									Array.Copy(value2, num4, array3, 0, 3);
									nit.LanguageCode = Encoding.ASCII.GetString(array3);
									break;
								}
								num -= b2 + 1;
							}
							if (!string.IsNullOrEmpty(nit.LanguageCode))
							{
								break;
							}
						}
						num3++;
						continue;
					}
					nit.LanguageCode = "eng";
					break;
				}
				while (string.IsNullOrEmpty(nit.LanguageCode));
				NITInfoManager nITInfoManager = new NITInfoManager();
				IList<NitInfo> nITInfoByNetworkID = nITInfoManager.GetNITInfoByNetworkID(nit.NetworkId);
				if (nITInfoByNetworkID != null && nITInfoByNetworkID.Count > 0)
				{
					nit.Identity = nITInfoByNetworkID.First().Identity;
					nit.VersionNumber = nITInfoByNetworkID.First().VersionNumber;
				}
				nITInfoManager.SaveNITInfo(nit);
				nit = nITInfoManager.GetNITInfoByNetworkID(nit.NetworkId).Single();
			}
			if (nit.NetworkId == 0)
			{
				return;
			}
			TsInfo tsInfo = new TsInfo();
			Dictionary<int, byte[]> dictionary3 = tSFileExpert.ConvertToSections(fileName, 17, 0, 66);
			if (dictionary3.Count <= 0)
			{
				return;
			}
			foreach (int item2 in dictionary3.Keys.OrderBy((int x) => x))
			{
				byte[] array4 = dictionary3[item2];
				if (tsInfo.TsId == 0)
				{
					tsInfo.TsId = ((array4[3] & 0xF) << 8) | array4[4];
					tsInfo.TsName = string.Format("TS[0x{0}]", tsInfo.TsId.ToString("X2"));
					tsInfo.OriginalNetworkId = nit.NetworkId;
					tsInfo.NitTableId = nit.Identity;
					tsInfo.PatVersionNumber = -1;
					tsInfo.SdtVersionNumber = -1;
					tsInfo.CatVersionNumber = -1;
					TSInfoManager tSInfoManager = new TSInfoManager();
					IList<TsInfo> tSInfoByTSId = tSInfoManager.GetTSInfoByTSId(tsInfo.TsId);
					if (tSInfoByTSId == null || tSInfoByTSId.Count == 0)
					{
						tSInfoManager.SaveTSInfo(tsInfo);
						tsInfo = tSInfoManager.GetTSInfoByTSId(tsInfo.TsId).First((TsInfo x) => x.NitTableId == nit.Identity);
					}
					else
					{
						tsInfo = tSInfoByTSId.First((TsInfo x) => x.NitTableId == nit.Identity);
					}
				}
				int num4 = 11;
				while (num4 + 3 + 4 < array4.Length)
				{
					ProgramInfo programInfo = new ProgramInfo();
					programInfo.TsTableId = tsInfo.Identity;
					ProgramInfo programInfo2 = programInfo;
					ProgramInfoManager programInfoManager = new ProgramInfoManager();
					programInfo2.ServiceId = (array4[num4++] << 8) | array4[num4++];
					programInfo2.PrgSdtEitscflag = (array4[num4] & 2) << 1;
					programInfo2.PrgSdtEitpfflag = array4[num4] & 1;
					num4++;
					programInfo2.PrgSdtRunningstate = (array4[num4] & 0xE0) >> 5;
					programInfo2.PrgSdtFreecamod = (array4[num4] & 0x10) >> 4;
					int num = ((array4[num4++] & 0xF) << 8) | array4[num4++];
					while (num > 0)
					{
						byte b = array4[num4++];
						byte b2 = array4[num4++];
						if (b == 72)
						{
							programInfo2.ServiceType = array4[num4++];
							int num5 = array4[num4++];
							byte[] array5 = new byte[num5];
							Array.Copy(array4, num4, array5, 0, num5);
							num4 += num5;
							programInfo2.ProviderName = Encoding.Default.GetString(array5);
							int num6 = array4[num4++];
							byte[] array6 = new byte[num6];
							Array.Copy(array4, num4, array6, 0, num6);
							num4 += num6;
							programInfo2.ProgramName = Encoding.Default.GetString(array6);
							num4 += num - (b2 + 2);
							break;
						}
						num -= b2 + 1;
					}
					ProgramInfo byTSIdentityAndServiceId = programInfoManager.GetByTSIdentityAndServiceId(tsInfo.Identity, programInfo2.ServiceId);
					if (byTSIdentityAndServiceId != null)
					{
						programInfo2.Identity = byTSIdentityAndServiceId.Identity;
					}
					programInfoManager.Save(programInfo2);
				}
			}
		}

		private void FormTest_Load(object sender, EventArgs e)
		{
			EPGSettings.Instatce.DBConnectionString = EPGSettings.Instatce.DBConnectionString;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.FormTest));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.button1 = new System.Windows.Forms.Button();
			base.pnlBase.SuspendLayout();
			base.SuspendLayout();
			base.pnlBase.Controls.Add(this.button1);
			resources.ApplyResources(base.pnlBase, "pnlBase");
			this.openFileDialog1.FileName = "openFileDialog1";
			resources.ApplyResources(this.button1, "button1");
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(button1_Click);
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormTest";
			base.Load += new System.EventHandler(FormTest_Load);
			base.pnlBase.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
