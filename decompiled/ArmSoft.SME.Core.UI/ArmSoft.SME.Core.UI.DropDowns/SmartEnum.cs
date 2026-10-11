using System;
using System.Collections.Generic;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(14)]
public class SmartEnum : DropDownView<SmartEnumDataProvider.DataRow, SmartEnumDataProvider.Param>
{
	private readonly SmartEnumProviderSupportAttribute _smartEnumAttribute;

	private readonly bool _isValueUIMember;

	public override string Caption => Settings.StringResourceService.GetStringResource(_smartEnumAttribute.CaptionResourceId);

	public SmartEnum(Type type, List<string> values = null, bool padLeft = false, char paddingChar = '0', bool allSubTypes = false, ValueMember valueMember = ArmSoft.SME.Common.Models.SmartEnums.ValueMember.Code, CaptionMember captionMember = CaptionMember.Caption, string codeColumnCaption = null, string captionColumnCaption = null, bool orderByCode = true, List<string> excludeValues = null)
	{
		_smartEnumAttribute = (SmartEnumProviderSupportAttribute)Attribute.GetCustomAttribute(type, typeof(SmartEnumProviderSupportAttribute));
		_isValueUIMember = valueMember == ArmSoft.SME.Common.Models.SmartEnums.ValueMember.ValueUI;
		if (_smartEnumAttribute == null)
		{
			throw new ASException("The type " + type.FullName + " not supported for smart enum view");
		}
		if (allSubTypes)
		{
			Type partialSmartEnum = DefinitionMappingsService.GetPartialSmartEnum(type.Name);
			if (partialSmartEnum == null)
			{
				DataProvider = new SmartEnumDataProvider(type, valueMember, captionMember, padLeft, _smartEnumAttribute.ValueMemberLength, paddingChar, codeColumnCaption, captionColumnCaption);
			}
			else
			{
				DataProvider = new SmartEnumDataProvider(partialSmartEnum, valueMember, captionMember, padLeft, _smartEnumAttribute.ValueMemberLength, paddingChar, codeColumnCaption, captionColumnCaption);
			}
		}
		else
		{
			DataProvider = new SmartEnumDataProvider(type, valueMember, captionMember, padLeft, _smartEnumAttribute.ValueMemberLength, paddingChar, codeColumnCaption, captionColumnCaption);
		}
		ParametersProvider.Values = values;
		ParametersProvider.ExcludeValues = excludeValues;
		FillColumnsFromDataProvider();
		if (_isValueUIMember)
		{
			Columns["Code"].Visible = false;
		}
		else
		{
			Columns["ValueUI"].Visible = false;
		}
		if (orderByCode)
		{
			if (_isValueUIMember)
			{
				Columns.AddSorting("ValueUI");
			}
			else
			{
				Columns.AddSorting("Code");
			}
		}
	}

	public override short ValueMemberLength()
	{
		return _smartEnumAttribute.ValueMemberLength;
	}

	public override object KeyMember(int index)
	{
		return Data[index].Code;
	}

	public override string ValueMember(int index)
	{
		if (!_isValueUIMember)
		{
			return Data[index].Code;
		}
		return Data[index].ValueUI;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
