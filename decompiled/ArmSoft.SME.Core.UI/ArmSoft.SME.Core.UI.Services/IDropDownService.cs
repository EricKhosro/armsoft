using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.Services;

public interface IDropDownService
{
	IDropDownView CreateDropDown(short dropDownType, params object[] parameters);

	IDropDownView CreateDropDown(string dropDownGroup, params object[] parameters);
}
