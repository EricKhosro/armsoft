using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.DropDowns;

public class DropDownType : SmartEnum
{
	public DropDownType(bool onlyUsedInExtensions = false, bool showOnlyCaption = false)
		: base(typeof(DropDownTypeSE), null, padLeft: false, '0', allSubTypes: true, showOnlyCaption ? ArmSoft.SME.Common.Models.SmartEnums.ValueMember.Caption : ArmSoft.SME.Common.Models.SmartEnums.ValueMember.ValueUI)
	{
		if (onlyUsedInExtensions)
		{
			ParametersProvider.Values = new List<string>();
			foreach (dynamic smartEnumField in DefinitionMappingsService.GetPartialSmartEnum(typeof(DropDownTypeSE).Name).GetSmartEnumFields())
			{
				if (onlyUsedInExtensions && smartEnumField.CanUsedInExtensions)
				{
					ParametersProvider.Values.Add(Convert.ToString(smartEnumField.Value));
				}
			}
		}
		if (showOnlyCaption)
		{
			Columns["Caption"].Visible = false;
		}
	}
}
