using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Services;

public class DropDownService : IDropDownService
{
	public IDropDownView CreateDropDown(short dropDownType, params object[] parameters)
	{
		if (DefinitionMappingsService.TryResolveDropDown(dropDownType, out var dropDown))
		{
			return (IDropDownView)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, dropDown, parameters);
		}
		throw new RESTException(string.Format(Resources.DataViewNotSupported, dropDownType));
	}

	public IDropDownView CreateDropDown(string dropDownGroup, params object[] parameters)
	{
		short dropDownType = Convert.ToInt16(SmartEnumHelper.GetValue<short>(typeof(DropDownTypeSE), dropDownGroup));
		return CreateDropDown(dropDownType, parameters);
	}
}
