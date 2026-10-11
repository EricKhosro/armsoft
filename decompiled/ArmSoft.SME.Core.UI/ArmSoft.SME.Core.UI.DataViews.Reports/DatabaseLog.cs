using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(7)]
public class DatabaseLog : DataView<DatabaseLogDataProvider.DataRow, DatabaseLogDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly DatabaseLog _dataView;

		private readonly DateEditControl _dateBegin;

		private readonly DateEditControl _dateEnd;

		private readonly DropDownControl _dbName;

		private readonly DropDownControl _userName;

		public DataViewDialog(DatabaseLog dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_dateBegin = AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriodBegin, useLongDate: false, isRequired: false, !useParamValues);
			_dateBegin.EditValue = (useParamValues ? dataView.DateBegin : new DateOnly?(DateTime.Today.ToDateOnly()));
			_dateEnd = AddDateControl(container, "DateEnd", string.Empty, useLongDate: false, isRequired: false, !useParamValues);
			_dateEnd.EditValue = (useParamValues ? dataView.DateEnd : new DateOnly?(DateTime.Today.ToDateOnly()));
			AddSpecialDateButton(container, _dateBegin, _dateEnd);
			_dbName = AddDropDownControl("DatabaseName", ArmSoft.SME.Common.Resources.Database, new ArmSoft.SME.Core.UI.DropDowns.Databases(""), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_dbName.Code = (useParamValues ? dataView.DatabaseName : Settings.LoginService.Database);
			_userName = AddDropDownControl("UserName", ArmSoft.SME.Common.Resources.User, new UsersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_userName.Code = (useParamValues ? dataView.UserName : "");
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.DateBegin = _dateBegin.EditValue;
			_dataView.DateEnd = _dateEnd.EditValue;
			_dataView.DatabaseName = _dbName.Code;
			_dataView.UserName = _userName.Code;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_dateBegin.EditValue, _dateEnd.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	public override string Caption => Resources.UsersLoginHistory;

	public override string HelpId => "AdminTools_Users_LoginlogoutHistory";

	public DateOnly? DateBegin
	{
		get
		{
			return (DateOnly?)Parameters["DateBegin"].Value;
		}
		set
		{
			Parameters["DateBegin"].Value = value;
		}
	}

	public DateOnly? DateEnd
	{
		get
		{
			return (DateOnly?)Parameters["DateEnd"].Value;
		}
		set
		{
			Parameters["DateEnd"].Value = value;
		}
	}

	public string DatabaseName
	{
		get
		{
			return Convert.ToString(Parameters["DatabaseName"].Value);
		}
		set
		{
			Parameters["DatabaseName"].Value = value;
		}
	}

	public string UserName
	{
		get
		{
			return Convert.ToString(Parameters["UserName"].Value);
		}
		set
		{
			Parameters["UserName"].Value = value;
		}
	}

	public override bool DialogSupported => true;

	public DatabaseLog()
	{
		DataProvider = new DatabaseLogDataProvider();
		AddParameter("DateBegin", Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("DateEnd", Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("DatabaseName", Resources.Database, string.Empty);
		AddParameter("UserName", Resources.UserName, string.Empty);
		FillColumnsFromDataProvider();
		Columns["ProcessId"].Format = DecimalExtension.GetNumberFormat(10, 0);
		Columns["Date"].Format = "dd/MM/yy HH:mm:ss";
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.DateBegin = DateBegin.ToDateTime();
		ParametersProvider.DateEnd = DateEnd.ToDateTime();
		ParametersProvider.DatabaseName = DatabaseName;
		ParametersProvider.UserId = Settings.CoreCacheService.User.GetItemByName(UserName)?.CloudUserId ?? (-1);
	}
}
