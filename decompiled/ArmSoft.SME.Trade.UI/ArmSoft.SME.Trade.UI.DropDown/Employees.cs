using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

[DropDown(10003)]
public class Employees : DropDownView<EmployeeDataProvider.DataRow, EmployeeDataProvider.Param>
{
	public bool IsOnlySalesConsultant { get; set; }

	public override string Caption
	{
		get
		{
			if (IsOnlySalesConsultant)
			{
				return ArmSoft.SME.Trade.Common.Resources.SalesConsultants;
			}
			return ArmSoft.SME.Common.Resources.Employees;
		}
	}

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Employee).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Employee).AllowEdit;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public Employees(bool isOnlySalesConsultant = false)
	{
		DataProvider = new EmployeeDataProvider();
		FillColumnsFromDataProvider();
		IsOnlySalesConsultant = isOnlySalesConsultant;
	}

	public override short ValueMemberLength()
	{
		return 12;
	}

	public override string ValueMember(int index)
	{
		return Data[index].EmployeeCode;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	public override bool AddNew(ref string value, ref string comment, Window owner, double top, double left)
	{
		return AddOrEdit(ref value, ref comment, isNew: true, owner, top, left);
	}

	public override bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow)
	{
		return AddOrEdit(ref value, ref comment, isNew: false, owner, top, left);
	}

	private bool AddOrEdit(ref string value, ref string comment, bool isNew, Window owner, double top, double left)
	{
		bool result = false;
		EmployeeDocumentWindow employeeDocumentWindow = (isNew ? ((EmployeeDocumentWindow)Settings.DirectoryService.CreatePOWindow(DirectoryTypeSE.Employee)) : ((EmployeeDocumentWindow)Settings.DirectoryService.CreatePOWindow(Settings.DirectoryService.Load(DirectoryTypeSE.Employee, value))));
		employeeDocumentWindow.Owner = owner;
		employeeDocumentWindow.Top = top;
		employeeDocumentWindow.Left = left;
		if (employeeDocumentWindow.ShowDialog(isNew ? WindowViewMode.NewCreate : WindowViewMode.Edit).Value)
		{
			DataLoadedAtLeastOnce = false;
			result = true;
			value = employeeDocumentWindow.PO.Code;
			comment = employeeDocumentWindow.PO.Name;
		}
		return result;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.IsOnlySalesConsultant = IsOnlySalesConsultant;
	}

	public override bool Validate(string value, ref string comment, ref string realValue)
	{
		EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(value);
		if (item == null)
		{
			return false;
		}
		comment = item.Caption;
		realValue = item.EmployeeCode;
		return true;
	}
}
