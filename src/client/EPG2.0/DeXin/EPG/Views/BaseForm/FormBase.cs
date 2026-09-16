using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.BLL;
using DeXin.EPG.BLL.ApplicationManagement;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Tools;
using DeXin.EPG.Tools.EPGUserControl;

namespace DeXin.EPG.Views.BaseForm
{
	public class FormBase : Form
	{
		private int _FocusRowIndex = 0;

		private PermitGroupManagementBLL permitManager = new PermitGroupManagementBLL();

		protected EnumEditModeFlag editModeFlag = EnumEditModeFlag.X;

		protected EnumFunctionName _FunctionName = EnumFunctionName.SysFuncAllFunctions;

		protected string msg = string.Empty;

		private DateTime _dtNow = DateTime.Now;

		private PermitGroup _permitGroup = null;

		private IContainer components = null;

		protected int FocusRowIndex
		{
			get
			{
				return _FocusRowIndex;
			}
			set
			{
				_FocusRowIndex = value;
			}
		}

		public EnumFunctionName FunctionName
		{
			get
			{
				if (_FunctionName == EnumFunctionName.SysFuncAllFunctions)
				{
					FormBase formBase = base.Owner as FormBase;
					if (formBase != null)
					{
						_FunctionName = formBase.FunctionName;
					}
				}
				return _FunctionName;
			}
		}

		protected DateTime dtNow
		{
			get
			{
				return _dtNow;
			}
			set
			{
				_dtNow = value;
			}
		}

		public PermitGroup permitGroup
		{
			get
			{
				if (_permitGroup == null)
				{
					if (LoginUser.Ins.UserIns != null)
					{
						_permitGroup = permitManager.GetDataByGroupIDAndFunctionID(Tool.GetInt32(LoginUser.Ins.UserIns.GroupID), ((int)FunctionName).ToString()).FirstOrDefault();
					}
					if (_permitGroup == null)
					{
						_permitGroup = new PermitGroup();
						PermitGroup obj = _permitGroup;
						PermitGroup obj2 = _permitGroup;
						PermitGroup obj3 = _permitGroup;
						PermitGroup obj4 = _permitGroup;
						bool flag = (_permitGroup.CanPrint = false);
						flag = (obj4.CanEdit = flag);
						flag = (obj3.CanDelete = flag);
						flag = (obj2.CanQuery = flag);
						obj.CanAdd = flag;
					}
				}
				return _permitGroup;
			}
			set
			{
				_permitGroup = value;
			}
		}

		public FormBase()
		{
			InitializeComponent();
		}

		private void FormBase_Shown(object sender, EventArgs e)
		{
			try
			{
				FormShown();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		private void FormBase_Load(object sender, EventArgs e)
		{
			try
			{
				FormLoad();
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
		}

		protected bool Permit(enumPermission oper)
		{
			bool result = false;
			if (permitGroup != null)
			{
				switch (oper)
				{
				case enumPermission.CanAdd:
					result = permitGroup.CanAdd;
					break;
				case enumPermission.CanDelete:
					result = permitGroup.CanDelete;
					break;
				case enumPermission.CanEdit:
					result = permitGroup.CanEdit;
					break;
				case enumPermission.CanPrint:
					result = permitGroup.CanPrint;
					break;
				case enumPermission.CanQuery:
					result = permitGroup.CanQuery;
					break;
				}
			}
			return result;
		}

		protected bool Permit(enumPermission oper, bool isShowMessage)
		{
			bool flag = false;
			try
			{
				flag = Permit(oper);
				if (!flag && isShowMessage)
				{
					Tool.ShowMessage(ResourceManagement.GetLocationString("NoPermit"));
				}
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			return flag;
		}

		protected virtual void FormLoad()
		{
		}

		protected virtual void FormShown()
		{
		}

		protected bool ValidateInput(Control ContainerControl)
		{
			ValidateChildren(ValidationConstraints.TabStop);
			foreach (Control control in ContainerControl.Controls)
			{
				if (control.Controls.Count > 0)
				{
					if (!ValidateInput(control))
					{
						return false;
					}
				}
				else if (control is TextBoxBase && control.Enabled && !((TextBoxBase)control).ReadOnly && control is ValidatTextBox && !((ValidatTextBox)control).IsValidated)
				{
					return false;
				}
			}
			return true;
		}

		protected bool ValidateInput()
		{
			return ValidateInput(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.BaseForm.FormBase));
			base.SuspendLayout();
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "FormBase";
			base.Load += new System.EventHandler(FormBase_Load);
			base.Shown += new System.EventHandler(FormBase_Shown);
			base.ResumeLayout(false);
		}
	}
}
