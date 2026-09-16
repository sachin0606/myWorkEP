using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.TSFileHelper;
using DeXin.EPG.Model;
using DeXin.EPG.Resource.ResourcesSpecialFiles;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;

namespace DeXin.EPG.Views.ProgramManagement
{
	public class FormImportTSFile : FormDetailBase
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel2;

		private GroupBox groupBox1;

		private GroupBox groupBox2;

		private TableLayoutPanel tableLayoutPanel3;

		private CheckBox ckNIT;

		private CheckBox ckTS;

		private CheckBox ckProgram;

		private CheckBox ckMedia;

		private CheckBox ckBAT;

		private FlowLayoutPanel flowLayoutPanel1;

		private TextBox txtFileName;

		private Button btSelectFile;

		private OpenFileDialog opdSourceFile;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.ProgramManagement.FormImportTSFile));
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.btSelectFile = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.ckNIT = new System.Windows.Forms.CheckBox();
			this.ckTS = new System.Windows.Forms.CheckBox();
			this.ckProgram = new System.Windows.Forms.CheckBox();
			this.ckMedia = new System.Windows.Forms.CheckBox();
			this.ckBAT = new System.Windows.Forms.CheckBox();
			this.opdSourceFile = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(base.grbText, "grbText");
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.groupBox2.Controls.Add(this.flowLayoutPanel1);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.flowLayoutPanel1.Controls.Add(this.txtFileName);
			this.flowLayoutPanel1.Controls.Add(this.btSelectFile);
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			resources.ApplyResources(this.txtFileName, "txtFileName");
			this.txtFileName.Name = "txtFileName";
			resources.ApplyResources(this.btSelectFile, "btSelectFile");
			this.btSelectFile.Name = "btSelectFile";
			this.btSelectFile.UseVisualStyleBackColor = true;
			this.btSelectFile.Click += new System.EventHandler(btSelectFile_Click);
			this.groupBox1.Controls.Add(this.tableLayoutPanel3);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.ckNIT, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.ckTS, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.ckProgram, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.ckMedia, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.ckBAT, 1, 1);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			resources.ApplyResources(this.ckNIT, "ckNIT");
			this.ckNIT.Checked = true;
			this.ckNIT.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckNIT.Name = "ckNIT";
			this.ckNIT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckTS, "ckTS");
			this.ckTS.Checked = true;
			this.ckTS.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckTS.Name = "ckTS";
			this.ckTS.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckProgram, "ckProgram");
			this.ckProgram.Checked = true;
			this.ckProgram.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckProgram.Name = "ckProgram";
			this.ckProgram.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckMedia, "ckMedia");
			this.ckMedia.Name = "ckMedia";
			this.ckMedia.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.ckBAT, "ckBAT");
			this.ckBAT.Name = "ckBAT";
			this.ckBAT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.opdSourceFile, "opdSourceFile");
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormImportTSFile";
			base.Load += new System.EventHandler(FormImportTSFile_Load);
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FormImportTSFile()
		{
			InitializeComponent();
		}

		private void FormImportTSFile_Load(object sender, EventArgs e)
		{
			_FunctionName = EnumFunctionName.SysFuncProgramInfo;
			base.PageFlag = PageEnum.Add;
			base.ContentText = ResourceSpecial.ImportTSFile;
		}

		private void btSelectFile_Click(object sender, EventArgs e)
		{
			if (opdSourceFile.ShowDialog() == DialogResult.OK)
			{
				txtFileName.Text = opdSourceFile.FileName;
			}
		}

		protected override bool FunSubmit()
		{
			string text = txtFileName.Text;
			if (!string.IsNullOrEmpty(text) && File.Exists(text))
			{
				try
				{
					base.Enabled = false;
					TSFileExpert tSFileExpert = new TSFileExpert();
					NitInfo nit = new NitInfo
					{
						NetworkId = -1
					};
					Dictionary<int, byte[]> dictionary = tSFileExpert.ConvertToSections(text, 16, 0, 0);
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
							for (int num2 = 10; num2 <= num + 10 + 4; num2 += b2)
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
							Dictionary<int, byte[]> dictionary2 = tSFileExpert.ConvertToSections(text, 18, num3, 0);
							if (dictionary2.Count > 0)
							{
								int num4 = 14;
								byte[] value2 = dictionary2.Last().Value;
								while (num4 + 11 <= value2.Length)
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
					if (nit.NetworkId == -1)
					{
						return false;
					}
					TsInfo tsInfo = new TsInfo();
					tsInfo.TsId = -1;
					TsInfo tsInfo2 = tsInfo;
					Dictionary<int, byte[]> dictionary3 = tSFileExpert.ConvertToSections(text, 17, 0, 66);
					if (dictionary3.Count > 0)
					{
						foreach (int item2 in dictionary3.Keys.OrderBy((int x) => x))
						{
							byte[] array4 = dictionary3[item2];
							if (tsInfo2.TsId == -1)
							{
								tsInfo2.TsId = ((array4[3] & 0xF) << 8) | array4[4];
								tsInfo2.TsName = string.Format("TS[0x{0}]", tsInfo2.TsId.ToString("X2"));
								tsInfo2.OriginalNetworkId = nit.NetworkId;
								tsInfo2.NitTableId = nit.Identity;
								tsInfo2.PatVersionNumber = -1;
								tsInfo2.SdtVersionNumber = -1;
								tsInfo2.CatVersionNumber = -1;
							}
							TSInfoManager tSInfoManager = new TSInfoManager();
							TsInfo tsInfo3 = tSInfoManager.GetTSInfoByTSId(tsInfo2.TsId).FirstOrDefault((TsInfo x) => x.NitTableId == nit.Identity);
							if (tsInfo3 == null)
							{
								tSInfoManager.SaveTSInfo(tsInfo2);
								tsInfo2 = tSInfoManager.GetTSInfoByTSId(tsInfo2.TsId).First((TsInfo x) => x.NitTableId == nit.Identity);
							}
							else
							{
								tsInfo2 = tsInfo3;
							}
							int num4 = 11;
							while (num4 + 3 + 4 <= array4.Length)
							{
								ProgramInfo programInfo = new ProgramInfo();
								programInfo.TsTableId = tsInfo2.Identity;
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
								ProgramInfo byTSIdentityAndServiceId = programInfoManager.GetByTSIdentityAndServiceId(tsInfo2.Identity, programInfo2.ServiceId);
								if (byTSIdentityAndServiceId != null)
								{
									programInfo2.Identity = byTSIdentityAndServiceId.Identity;
								}
								programInfoManager.Save(programInfo2);
							}
						}
					}
					Dictionary<int, byte[]> dictionary4 = tSFileExpert.ConvertToSections(text, 0, 0, 0);
					foreach (int item3 in dictionary4.Keys.OrderBy((int x) => x))
					{
						byte[] array4 = dictionary4[item3];
						int num4 = 8;
						while (num4 + 4 + 4 <= array4.Length)
						{
							int num7 = (array4[num4++] << 8) | array4[num4++];
							if (num7 == 0)
							{
								num4 += 2;
								continue;
							}
							int num8 = ((array4[num4++] & 0x1F) << 8) | array4[num4++];
							ProgramInfoManager programInfoManager = new ProgramInfoManager();
							ProgramInfo byTSIdentityAndServiceId = programInfoManager.GetByTSIdentityAndServiceId(tsInfo2.Identity, num7);
							if (byTSIdentityAndServiceId == null)
							{
								continue;
							}
							byTSIdentityAndServiceId.PmtId = num8;
							if (ckMedia.Checked)
							{
								Dictionary<int, byte[]> dictionary5 = tSFileExpert.ConvertToSections(text, num8, 0, 0);
								foreach (int item4 in dictionary5.Keys.OrderBy((int x) => x))
								{
									byte[] array7 = dictionary5[item4];
									int num9 = 8;
									if (byTSIdentityAndServiceId.PCRPID == 0)
									{
										byTSIdentityAndServiceId.PCRPID = ((array7[num9] & 0x1F) << 8) | array7[num9 + 1];
									}
									num9 += 2;
									int num = ((array7[num9++] & 0xF) << 8) | array7[num9++];
									num9 += num;
									MultimediaManager multimediaManager = new MultimediaManager();
									int num10;
									for (; num9 + 4 + 5 <= array7.Length; num9 += num10)
									{
										Multimedia media = new Multimedia();
										media.ProgramTableId = byTSIdentityAndServiceId.Identity;
										media.Program = byTSIdentityAndServiceId;
										int streamTypeId = array7[num9++];
										StreamTypeInfo byStreamTypeId = new StreamTypeInfoManager().GetByStreamTypeId(streamTypeId);
										if (byStreamTypeId != null)
										{
											media.StreamTypeTableId = byStreamTypeId.Identity;
											media.PesPid = ((array7[num9++] & 0x1F) << 8) | array7[num9++];
											Multimedia multimedia = multimediaManager.GetByProgramIdentity(byTSIdentityAndServiceId.Identity).FirstOrDefault((Multimedia x) => x.PesPid == media.PesPid);
											if (multimedia != null)
											{
												media.Id = multimedia.Id;
											}
											multimediaManager.Save(media);
										}
										num10 = ((array7[num9++] & 0xF) << 8) | array7[num9++];
									}
								}
							}
							programInfoManager.Save(byTSIdentityAndServiceId);
						}
					}
					if (ckBAT.Checked)
					{
						Dictionary<int, Dictionary<int, byte[]>> dictionary6 = tSFileExpert.ParseToBATSections(text);
						foreach (int item5 in dictionary6.Keys.OrderBy((int x) => x))
						{
							Dictionary<int, byte[]> dictionary7 = dictionary6[item5];
							BATInfo bATInfo = new BATInfo();
							bATInfo.BATId = -1;
							bATInfo.BATVersionNumber = -1;
							BATInfo bATInfo2 = bATInfo;
							List<ProgramInfo> list = new List<ProgramInfo>();
							BATToProgram bATToProgram = new BATToProgram();
							foreach (int item6 in dictionary7.Keys.OrderBy((int x) => x))
							{
								byte[] array = dictionary7[item6];
								int num4 = 3;
								if (bATInfo2.BATId == -1)
								{
									bATInfo2.BATId = (array[num4] << 8) | array[num4 + 1];
								}
								num4 = 8;
								int num11 = ((array[num4++] & 0xF) << 8) | array[num4++];
								if (string.IsNullOrEmpty(bATInfo2.BATName))
								{
									while (num11 > 0)
									{
										byte b = array[num4++];
										byte b2 = array[num4++];
										if (b == 71)
										{
											byte[] array6 = new byte[b2];
											Array.Copy(array, num4, array6, 0, b2);
											num4 += b2;
											bATInfo2.BATName = Encoding.Default.GetString(array6);
											break;
										}
										num11 -= b2 + 2;
									}
								}
								num4 = 10 + num11;
								int num12 = ((array[num4++] & 0xF) << 8) | array[num4++];
								while (num12 > 0)
								{
									int tSID = (array[num4++] << 8) | array[num4++];
									int networkID = (array[num4++] << 8) | array[num4++];
									if (bATInfo2.NitTableId == 0)
									{
										bATInfo2.NIT = nit;
										bATInfo2.NitTableId = nit.Identity;
									}
									int num13 = ((array[num4++] & 0xF) << 8) | array[num4++];
									int num14 = num13;
									ProgramInfoManager programInfoManager = new ProgramInfoManager();
									while (num14 > 0)
									{
										byte b = array[num4++];
										byte b2 = array[num4++];
										byte b3 = b2;
										if (b == 65)
										{
											while (b3 > 0)
											{
												int serviceID = (array[num4++] << 8) | array[num4++];
												ProgramInfo programInfo2 = programInfoManager.Get(networkID, tSID, serviceID);
												if (programInfo2 != null)
												{
													list.Add(programInfo2);
												}
												num4++;
												b3 -= 3;
											}
										}
										num14 -= 2 + b2;
									}
									num12 -= 6 + num13;
								}
							}
							BATInfoManager bATInfoManager = new BATInfoManager();
							BATInfo byBATId = bATInfoManager.GetByBATId(bATInfo2.NitTableId, bATInfo2.BATId);
							if (byBATId != null)
							{
								bATInfo2.Identity = byBATId.Identity;
							}
							bATToProgram.BAT = bATInfo2;
							bATToProgram.ProgramList = list;
							new BATInfoManager().Save(bATToProgram);
						}
					}
					if (nit.Identity > 0 && tsInfo2.Identity > 0)
					{
						base.DialogResult = DialogResult.OK;
					}
				}
				catch (Exception ex)
				{
					Tool.ShowMessage(ex);
				}
				finally
				{
					base.Enabled = true;
				}
			}
			return base.FunSubmit();
		}
	}
}
