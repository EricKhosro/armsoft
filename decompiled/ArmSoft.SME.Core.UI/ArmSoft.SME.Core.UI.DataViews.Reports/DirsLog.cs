using System;
using System.Collections.Generic;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(5)]
public class DirsLog : DataView<DirsLogDataProvider.DataRow, DirsLogDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly DirsLog _dataView;

		private readonly DateEditControl _opStartDate;

		private readonly DateEditControl _opEndDate;

		private readonly DropDownControl _dirType;

		private readonly DropDownControl _userName;

		public DataViewDialog(DirsLog dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_opStartDate = AddDateControl(container, "OpStartDate", ArmSoft.SME.Common.Resources.OperationDatePeriod, useLongDate: false, isRequired: false, !useParamValues);
			_opStartDate.EditValue = (useParamValues ? dataView.ParamOpStartDate : ((DateOnly?)null));
			_opEndDate = AddDateControl(container, "OpEndDate", string.Empty, useLongDate: false, isRequired: false, !useParamValues);
			_opEndDate.EditValue = (useParamValues ? dataView.ParamOpEndDate : ((DateOnly?)null));
			AddSpecialDateButton(container, _opStartDate, _opEndDate);
			List<string> values = Settings.ServiceProvider.GetRequiredService<IPODefinitionHelper>().GetLoggablePOTypes().ConvertAll((short p) => p.ToString());
			_dirType = AddDropDownControl("DirType", ArmSoft.SME.Common.Resources.TypeWord, new SmartEnum(typeof(DirectoryTypeSE), values, padLeft: false, '0', allSubTypes: true), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_dirType.Code = (useParamValues ? dataView.ParamDirType : "");
			_userName = AddDropDownControl("UserName", ArmSoft.SME.Common.Resources.User, new UsersFilter(), isRequired: false, allowAdditionButtons: false, storeValue: true);
			_userName.Code = (useParamValues ? dataView.ParamUserName : string.Empty);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ParamOpStartDate = _opStartDate.EditValue;
			_dataView.ParamOpEndDate = _opEndDate.EditValue;
			_dataView.ParamDirType = _dirType.Code;
			_dataView.ParamUserName = _userName.Code;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_opStartDate.EditValue, _opEndDate.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	public override bool DialogSupported => true;

	public override string HelpId => "Admintools_DirectoriesHistory";

	public DateOnly? ParamOpStartDate
	{
		get
		{
			return (DateOnly?)Parameters["OpStartDate"].Value;
		}
		set
		{
			Parameters["OpStartDate"].Value = value;
		}
	}

	public DateOnly? ParamOpEndDate
	{
		get
		{
			return (DateOnly?)Parameters["OpEndDate"].Value;
		}
		set
		{
			Parameters["OpEndDate"].Value = value;
		}
	}

	public string ParamDirType
	{
		get
		{
			return Convert.ToString(Parameters["DirType"].Value);
		}
		set
		{
			Parameters["DirType"].Value = value;
		}
	}

	public string ParamUserName
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

	public DirsLog()
	{
		DataProvider = new DirsLogDataProvider(Settings.CreateApiClient());
		AddParameter("OpStartDate", Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("OpEndDate", Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("DirType", Resources.TypeWord, "", "", includeCaptionToolTip: true, new SmartEnum(typeof(DirectoryTypeSE)));
		AddParameter("UserName", Resources.User, "");
		FillColumnsFromDataProvider();
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["UserID"].Visible = false;
		Columns["DirID"].Visible = false;
		Columns["ChangeDescription"].Visible = false;
		Columns.AddSorting("Date");
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.OpStartDate = ParamOpStartDate;
		ParametersProvider.OpEndDate = ParamOpEndDate;
		ParametersProvider.DirType = ParamDirType.ConvertSelected<byte>();
		ParametersProvider.UserId = Settings.CoreCacheService.User.GetItemByName(ParamUserName)?.UserId ?? (-1);
	}
}
