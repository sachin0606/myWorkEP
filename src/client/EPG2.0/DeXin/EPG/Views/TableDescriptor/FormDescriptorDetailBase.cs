using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using Com.Cn.Tool.Model;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Tools;
using DeXin.EPG.Views.BaseForm;
using DeXin.EPG.Views.TableDescriptor.DescriptorDemos;
using EPG.Views.TableDescriptor.DescriptorDemos;

namespace DeXin.EPG.Views.TableDescriptor
{
	public class FormDescriptorDetailBase : FormDetailBase
	{
		private DescriptorManagement desManager = new DescriptorManagement();

		private DescriptorTypeManager desTypeManager = new DescriptorTypeManager();

		private LogBLL logManager = new LogBLL();

		protected DescriptorBase _ET = null;

		private UCDescriptorBase DesPanel = null;

		private IContainer components = null;

		private GroupBox grbType;

		private Label lblLocation;

		private ComboBox cmbLocation;

		private Label lblDesType;

		private ComboBox cmbDesType;

		protected GroupBox grbData;

		private TableLayoutPanel tableLayoutPanel2;

		public DescriptorBase ET
		{
			get
			{
				return _ET;
			}
			set
			{
				_ET = value;
			}
		}

		public FormDescriptorDetailBase(long id, long tableId, EnumPossibleLoaction location, PageEnum pageFlag)
		{
			InitializeComponent();
			base.PageFlag = pageFlag;
			if (base.PageFlag == PageEnum.Edit || base.PageFlag == PageEnum.Show || location == EnumPossibleLoaction.TOT)
			{
				ET = desManager.GetDescriptorByID(location, id);
			}
			if (ET == null)
			{
				ET = new DescriptorSigleBase(location, tableId);
				pageFlag = PageEnum.Add;
			}
			InitControl();
			cmbLocation.Enabled = false;
		}

		public FormDescriptorDetailBase(long id, long batID, long tsId, PageEnum pageFlag)
		{
			InitializeComponent();
			_FunctionName = EnumFunctionName.SysFuncDescriptorType;
			base.PageFlag = pageFlag;
			if (base.PageFlag == PageEnum.Edit || base.PageFlag == PageEnum.Show)
			{
				ET = desManager.GetDescriptorByID(EnumPossibleLoaction.BAT_TS, id);
			}
			if (ET == null)
			{
				ET = new DescriptorSigleBase(EnumPossibleLoaction.BAT_TS, tsId, batID);
				pageFlag = PageEnum.Add;
			}
			InitControl();
			cmbLocation.Enabled = false;
		}

		protected override void FormLoad()
		{
			BindingData();
			switch (base.PageFlag)
			{
			case PageEnum.Empty:
			case PageEnum.Show:
			case PageEnum.Edit:
				cmbDesType.Enabled = false;
				break;
			case PageEnum.Add:
				break;
			}
		}

		protected override bool FunSubmit()
		{
			if (DesPanel == null)
			{
				return false;
			}
			if (!ValidateInput())
			{
				return false;
			}
			if (!DesPanel.IsValidated(error))
			{
				return false;
			}
			bool flag = false;
			switch (base.PageFlag)
			{
			case PageEnum.Add:
				if (DesPanel is DescriptorDemoBase)
				{
					DescriptorDemoBase descriptorDemoBase = DesPanel as DescriptorDemoBase;
					DescriptorType descriptorType = cmbDesType.SelectedItem as DescriptorType;
					descriptorDemoBase.ET.DesTypeTag = descriptorType.DesTag;
					descriptorDemoBase.ET.DesTypeID = descriptorType.Id;
				}
				flag = desManager.AddDescriptor(DesPanel.Descriotor, out msg);
				break;
			case PageEnum.Edit:
				if (DesPanel is DescriptorDemoBase)
				{
					DescriptorDemoBase descriptorDemoBase = DesPanel as DescriptorDemoBase;
					descriptorDemoBase.ET.DescriptionData = descriptorDemoBase.ET.DescriptionData;
					descriptorDemoBase.ET.DataLength = descriptorDemoBase.ET.DataLength;
				}
				flag = desManager.ModifyDescriptor(DesPanel.Descriotor, out msg);
				break;
			}
			Tool.ShowMessage(msg);
			if (flag)
			{
				base.DialogResult = DialogResult.OK;
			}
			return flag;
		}

		protected override void FunCancel()
		{
			Close();
		}

		private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				string value = Tool.GetValue(cmbLocation);
				if (value.Contains('.'))
				{
					return;
				}
				if (base.PageFlag == PageEnum.Add)
				{
					Tool.BindingComboBox((from p in BaseData.Ins.GetDescriptorTypesByLocation(ET.Location, false)
						orderby p.BKValue
						select p).ToList(), cmbDesType);
				}
				else
				{
					Tool.BindingComboBox((from p in BaseData.Ins.GetDescriptorTypesByLocation(ET.Location, true)
						orderby p.BKValue
						select p).ToList(), cmbDesType);
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void cmbDesType_SelectedIndexChanged(object sender, EventArgs e)
		{
			DescriptorType descriptorType = cmbDesType.SelectedItem as DescriptorType;
			if (descriptorType == null)
			{
				return;
			}
			if (descriptorType.DesType == 1)
			{
				DesPanel = new DefaultDemo(ET, base.PageFlag);
			}
			else
			{
				if (ET != null && ET is DescriptorSigleBase)
				{
					DescriptorSigleBase descriptorSigleBase = ET as DescriptorSigleBase;
					if (descriptorSigleBase != null && descriptorSigleBase.DesTypeTag >= 0 && descriptorSigleBase.DesTypeTag != descriptorType.DesTag)
					{
						return;
					}
				}
				switch (descriptorType.DesTag)
				{
				case 64:
				case 71:
				case 93:
					DesPanel = new TextBoxDemo(ET, base.PageFlag);
					break;
				case 65:
					DesPanel = new Service_List_DescriptorDemo(ET, base.PageFlag);
					break;
				case 67:
					DesPanel = new Satellite_Delivery_System_Descriptor_Demo(ET, base.PageFlag);
					break;
				case 68:
					DesPanel = new Cable_Delivery_System_Descriptor_Demo(ET, base.PageFlag);
					break;
				case 72:
					DesPanel = new Service_DescriptorDemo(ET, base.PageFlag);
					break;
				case 90:
					DesPanel = new Terrestrial_delivery_system_descriptorDemo(ET, base.PageFlag);
					break;
				case 9:
					DesPanel = new CA_DescriptorDemo(ET, base.PageFlag);
					break;
				case 84:
					DesPanel = new Context_DescriptorDemo(ET, base.PageFlag);
					break;
				case 88:
					DesPanel = new Local_Time_Offset_Descriptor_Demo(ET, base.PageFlag);
					break;
				case 77:
					DesPanel = new Short_Event_Descriptor_Demo(ET, base.PageFlag);
					break;
				case 131:
					DesPanel = new Logical_Channel_Descriptor_Demo(ET, base.PageFlag);
					break;
				case 74:
					DesPanel = new Linkage_DescriptorDemo(ET, base.PageFlag);
					break;
				case 81:
					DesPanel = new Mosaic_DescriptorDemo(ET, base.PageFlag);
					break;
				case 98:
					DesPanel = new FrequencylistdescriptorDemo(ET, base.PageFlag);
					break;
				case 97:
					DesPanel = new Short_Smoothing_Buffer_DescriptorDemo(ET, base.PageFlag);
					break;
				case 82:
					DesPanel = new Stream_Identifier_DescriptorDemo(ET, base.PageFlag);
					break;
				case 102:
					DesPanel = new Data_Broadcast_Id_DescriptorDemo(ET, base.PageFlag);
					break;
				case 85:
					DesPanel = new Parental_Rating_Demo(ET, base.PageFlag);
					break;
				case 78:
					DesPanel = new ExtendedEventDescriptorDemo(ET, base.PageFlag);
					break;
				default:
					DesPanel = new DefaultDemo(ET, base.PageFlag);
					break;
				}
			}
			DesPanel.Dock = DockStyle.Fill;
			grbData.Controls.Clear();
			grbData.Controls.Add(DesPanel);
		}

		private void InitControl()
		{
			Tool.BindingComboBox(BaseData.Ins.GetPosibleLocations(), cmbLocation);
		}

		private void BindingData()
		{
			cmbLocation.SelectedValue = ET.Location.ToString();
			if (base.PageFlag != PageEnum.Add)
			{
				cmbDesType.SelectedValue = ET.DesTypeID.ToString();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.TableDescriptor.FormDescriptorDetailBase));
			this.grbType = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblLocation = new System.Windows.Forms.Label();
			this.cmbLocation = new System.Windows.Forms.ComboBox();
			this.lblDesType = new System.Windows.Forms.Label();
			this.cmbDesType = new System.Windows.Forms.ComboBox();
			this.grbData = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)base.error).BeginInit();
			base.grbText.SuspendLayout();
			this.grbType.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			base.grbText.Controls.Add(this.grbData);
			base.grbText.Controls.Add(this.grbType);
			resources.ApplyResources(base.grbText, "grbText");
			this.grbType.Controls.Add(this.tableLayoutPanel2);
			resources.ApplyResources(this.grbType, "grbType");
			this.grbType.Name = "grbType";
			this.grbType.TabStop = false;
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.lblLocation, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.cmbLocation, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblDesType, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.cmbDesType, 3, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			resources.ApplyResources(this.lblLocation, "lblLocation");
			this.lblLocation.Name = "lblLocation";
			resources.ApplyResources(this.cmbLocation, "cmbLocation");
			this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLocation.FormattingEnabled = true;
			this.cmbLocation.Name = "cmbLocation";
			this.cmbLocation.SelectedIndexChanged += new System.EventHandler(cmbLocation_SelectedIndexChanged);
			resources.ApplyResources(this.lblDesType, "lblDesType");
			this.lblDesType.Name = "lblDesType";
			resources.ApplyResources(this.cmbDesType, "cmbDesType");
			this.cmbDesType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDesType.FormattingEnabled = true;
			this.cmbDesType.Name = "cmbDesType";
			this.cmbDesType.SelectedIndexChanged += new System.EventHandler(cmbDesType_SelectedIndexChanged);
			resources.ApplyResources(this.grbData, "grbData");
			this.grbData.Name = "grbData";
			this.grbData.TabStop = false;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormDescriptorDetailBase";
			((System.ComponentModel.ISupportInitialize)base.error).EndInit();
			base.grbText.ResumeLayout(false);
			this.grbType.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
