using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common.Parameters;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class TradeCommonCoreHelper : CommonCoreHelper
{
	public override void CheckWorkingPeriod(DateOnly currentDate, string controlName = null)
	{
		if (currentDate == DateOnly.MinValue)
		{
			throw new ASException(string.Format(Resources.FieldIsEmpty, Resources.DateWord), controlName);
		}
		WorkingPeriod workingPeriod = GetWorkingPeriod();
		DateOnly value = currentDate;
		DateOnly? endDate = workingPeriod.EndDate;
		if (!(value > endDate))
		{
			value = currentDate;
			endDate = workingPeriod.StartDate;
			if (!(value < endDate))
			{
				return;
			}
		}
		if (workingPeriod.IsUserPeriod || workingPeriod.IsUseCurrentDate)
		{
			throw new RESTException(string.Format(Resources.ErrDateNotInUserPeriod, Settings.LoginService.UserName, workingPeriod.StartDate.FormatDDMMYY(), workingPeriod.EndDate.FormatDDMMYY()), controlName);
		}
		throw new RESTException(string.Format(Resources.ErrDateIsNotInPeriod, workingPeriod.StartDate.FormatDDMMYY(), workingPeriod.EndDate.FormatDDMMYY()), controlName);
	}

	public override WorkingPeriod GetWorkingPeriod()
	{
		WorkingPeriod workingPeriod = new WorkingPeriod();
		DateOnly? dateOnly;
		try
		{
			dateOnly = Settings.ParametersService.DateBegin();
		}
		catch (Exception)
		{
			dateOnly = DateTime.Today.ToDateOnly();
		}
		try
		{
			workingPeriod.IsUseCurrentDate = Settings.ParametersService.UseCurrentDateInDocs();
		}
		catch (Exception)
		{
			workingPeriod.IsUseCurrentDate = false;
		}
		workingPeriod.IsUserPeriod = dateOnly.HasValue;
		if (workingPeriod.IsUseCurrentDate)
		{
			workingPeriod.StartDate = DateTime.Today.ToDateOnly();
			workingPeriod.EndDate = workingPeriod.StartDate;
		}
		else if (workingPeriod.IsUserPeriod)
		{
			workingPeriod.StartDate = dateOnly.Value;
			try
			{
				workingPeriod.EndDate = Settings.ParametersService.DateEnd().Value;
			}
			catch (Exception)
			{
				workingPeriod.EndDate = DateTime.Today.ToDateOnly();
			}
		}
		else
		{
			workingPeriod.StartDate = Settings.ParametersService.GetGeneralValue(Definitions.DateBegin.Id).YYYYMMDDToDateOnly();
			workingPeriod.EndDate = Settings.ParametersService.GetGeneralValue(Definitions.DateEnd.Id).YYYYMMDDToDateOnly();
		}
		return workingPeriod;
	}
}
