using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Com.Cn.Tool;
using DeXin.EPG.BLL;
using DeXin.EPG.Model;
using DeXin.EPG.Resource;
using DeXin.EPG.Views.BaseForm;
using DeXin.EPG.Views.LogManagement;

namespace DeXin.EPG.Views.QueryForms
{
	public class FormQueryLog : QueryBaseFormDynamicConditionNew
	{
		public IList<LogInfo> logInfos = new List<LogInfo>();

		private LogBLL logBLL = new LogBLL();

		private FormLogManagement1 parent = null;

		public PagingSearch pagingSearch = new PagingSearch();

		private IContainer components = null;

		public FormQueryLog()
		{
			InitializeComponent();
		}

		protected override void FormLoad()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("LogID", ResourceManagement.GetLocationString("LogID"));
			dictionary.Add("UserID", ResourceManagement.GetLocationString("UserID"));
			dictionary.Add("UserName", ResourceManagement.GetLocationString("UserName"));
			dictionary.Add("Year(OperateDateTime)", ResourceManagement.GetLocationString("Year"));
			dictionary.Add("Month(OperateDateTime)", ResourceManagement.GetLocationString("Month"));
			dictionary.Add("Day(OperateDateTime)", ResourceManagement.GetLocationString("Day"));
			dictionary.Add("Hour(OperateDateTime)", ResourceManagement.GetLocationString("Hour"));
			dictionary.Add("Minute(OperateDateTime)", ResourceManagement.GetLocationString("Minute"));
			smsDynamicConditionSearchNew.basekeys = dictionary;
		}

		protected override void FunQuery()
		{
			Query();
		}

		protected override void FunCancel()
		{
			logInfos.Clear();
			Close();
		}

		public void Query()
		{
			try
			{
				pagingSearch.PageIndex = 0;
				pagingSearch.Condition = smsDynamicConditionSearchNew.Conditions;
				parent = base.Owner as FormLogManagement1;
				parent.logInfos = logBLL.GetLogDataByPaging(pagingSearch).ToList();
				parent.pagingSearch = pagingSearch;
			}
			catch (Exception ex)
			{
				Tool.ShowMessage(ex);
			}
			finally
			{
				Hide();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeXin.EPG.Views.QueryForms.FormQueryLog));
			base.gbContext.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(base.gbContext, "gbContext");
			resources.ApplyResources(base.btnCancel, "btnCancel");
			resources.ApplyResources(base.btnQuery, "btnQuery");
			resources.ApplyResources(base.smsDynamicConditionSearchNew, "smsDynamicConditionSearchNew");
			resources.ApplyResources(this, "$this");
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FormQueryLog";
			base.gbContext.ResumeLayout(false);
			base.gbContext.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
