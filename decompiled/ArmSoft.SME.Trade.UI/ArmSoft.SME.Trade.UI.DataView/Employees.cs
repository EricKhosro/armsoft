using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10026)]
public class Employees : TradeDirectoryDataView<EmployeesWithLogDataProvider.DataRow, EmployeesWithLogDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly Employees _dataView;

		private readonly DropDownControl _employee;

		private readonly CheckEditExt _extended;

		public DataViewDialog(Employees dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_employee = AddDropDownControl(container, "EmployeeCode", ArmSoft.SME.Common.Resources.Employee, new ArmSoft.SME.Trade.UI.DropDown.Employees());
			_employee.Code = (useParamValues ? _dataView.EmployeeCode : string.Empty);
			_extended = AddBooleanControl("ShowExtended", ArmSoft.SME.Common.Resources.Extended);
			_extended.IsChecked = useParamValues && _dataView.Extended;
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.EmployeeCode = _employee.Code;
			_dataView.Extended = _extended.IsChecked.Value;
		}
	}

	private class PrintEmplIDCardWindow(List<EmployeeCacheRow> selectedContracts) : DialogWindow
	{
		private readonly List<EmployeeCacheRow> _selectedEmployees = selectedContracts;

		private DropDownControl _template;

		private DropDownControl _employee;

		private string _employeeCode = string.Empty;

		internal void Invoke()
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.PrintIDCard;
			Name = "DlgPrintIDCard";
			HelpId = "Sales_Lists_Employees_PrintIDCard";
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			LayoutGroup container = AddHorizontalLayoutGroup();
			if (_selectedEmployees.Count <= 1)
			{
				_employee = AddDropDownControl(container, "EmployeeCode", ArmSoft.SME.Common.Resources.Employee, new ArmSoft.SME.Trade.UI.DropDown.Employees(), isRequired: true, allowAdditionButtons: false, storeValue: false, isShowComment: true, 12);
				AddStoreValuesButton(container);
				container = null;
				if (_selectedEmployees.Count == 1)
				{
					_employeeCode = _selectedEmployees[0].EmployeeCode;
					_employee.Code = _employeeCode;
				}
			}
			else
			{
				AddStoreValuesButton(container);
			}
			_template = AddDropDownControl(container, "Template", ArmSoft.SME.Common.Resources.PrintTemplate, new Template(short.Parse(PrintTemplateDocumentTypeSE.IDCard.Value)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			SetFocusOnActivation = _template;
			if (!ShowDialog().Value)
			{
				return;
			}
			if (_selectedEmployees.Count <= 1 && _employeeCode != _employee.Code)
			{
				EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(_employee.Code);
				if (item != null)
				{
					_selectedEmployees.Clear();
					_selectedEmployees.Add(item);
				}
			}
			PrintIDCard();
		}

		private void PrintIDCard()
		{
			EmployeeIDCard printableElement = new EmployeeIDCard
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.PrintIDCard,
				EmployeeList = _selectedEmployees,
				Prefix = MCRInfo.GetInstance().Prefix,
				Suffix = MCRInfo.GetInstance().Suffix
			};
			Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(printableElement, PrintMode.PrintPreview, _template.Code, null, 1, includePrintInfo: false);
		}
	}

	public override short DirectoryType => DirectoryTypeSE.Employee.Value;

	protected override bool IsPOWindow => true;

	public override bool AllowEditCodeOfDirectories => Settings.ParametersService.AllowEditCodeOfDirectories();

	public override string HelpId => "Sales_Lists_Employees";

	public override bool DialogSupported => true;

	public string EmployeeCode
	{
		get
		{
			return (string)Parameters["EmployeeCode"].Value;
		}
		set
		{
			Parameters["EmployeeCode"].Value = value;
		}
	}

	public bool Extended
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowExtended"].Value);
		}
		set
		{
			Parameters["ShowExtended"].Value = value;
		}
	}

	public Employees()
	{
		DataProvider = new EmployeesWithLogDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("EmployeeCode", ArmSoft.SME.Common.Resources.Employee, string.Empty);
		AddParameter("ShowExtended", ArmSoft.SME.Common.Resources.Extended, false);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("EmployeeID", value: false);
		dvp.Set_ColumnVisible("UserName", Extended);
		dvp.Set_ColumnVisible("ChangeDate", Extended);
		dvp.Set_ColumnVisible("CreationDate", Extended);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("EmployeeCode");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintEmplIDCard", ArmSoft.SME.Trade.Common.Resources.PrintIDCard, PrintEmplIDCard);
			if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.AllDocumentsJournal).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowEmployeeDocuments", ArmSoft.SME.Common.Resources.Documents, ShowEmployeeDocuments, "journal_16.svg");
			}
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.EmployeeCode = EmployeeCode;
		ParametersProvider.ShowExtended = Extended;
	}

	private void ShowEmployeeDocuments()
	{
		EmployeesWithLogDataProvider.DataRow dataRow = (EmployeesWithLogDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			AllDocumentsJournalDataView allDocumentsJournalDataView = new AllDocumentsJournalDataView();
			allDocumentsJournalDataView.EmplCode = dataRow.EmployeeCode;
			allDocumentsJournalDataView.StartDate = Settings.HostingEnvironment.DateBegin;
			allDocumentsJournalDataView.EndDate = Settings.HostingEnvironment.DateEnd;
			allDocumentsJournalDataView.Show(useParamValues: true);
		}
	}

	private void PrintEmplIDCard()
	{
		List<EmployeesWithLogDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<EmployeesWithLogDataProvider.DataRow>();
		List<EmployeeCacheRow> list = new List<EmployeeCacheRow>();
		foreach (EmployeesWithLogDataProvider.DataRow item2 in selectedOrFocusedRows)
		{
			EmployeeCacheRow item = new EmployeeCacheRow
			{
				Id = item2.EmployeeID,
				EmployeeCode = item2.EmployeeCode,
				Caption = item2.Caption,
				Address = item2.Address,
				Phone = item2.Phone,
				Passport = item2.Passport,
				EmployeeCard = item2.EmployeeCard,
				IsSalesConsultant = item2.IsSalesConsultant
			};
			list.Add(item);
		}
		new PrintEmplIDCardWindow(list).Invoke();
	}
}
