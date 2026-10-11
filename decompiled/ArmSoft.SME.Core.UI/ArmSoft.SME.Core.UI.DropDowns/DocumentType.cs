using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(11)]
public class DocumentType : SmartEnum
{
	public DocumentType(List<short> values = null, bool checkViewAccess = false, bool checkAddAccess = false, CaptionMember captionMember = CaptionMember.FullCaption, bool orderByCode = false)
	{
		Type? typeFromHandle = typeof(DocumentTypeSE);
		bool orderByCode2 = orderByCode;
		int num = 1;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = DocumentTypeSE.TaxDocument.Value.ToString();
		base._002Ector(typeFromHandle, null, padLeft: true, '0', allSubTypes: true, ArmSoft.SME.Common.Models.SmartEnums.ValueMember.Code, captionMember, null, null, orderByCode2, list);
		ParametersProvider.Values = new List<string>();
		if (checkViewAccess | checkAddAccess)
		{
			foreach (KeyValuePair<short, POPermission> documentsPermission in Settings.UserAccessService.GetDocumentsPermissions())
			{
				if ((!checkViewAccess || documentsPermission.Value.AllowView) && (!checkAddAccess || documentsPermission.Value.AllowAdd) && (values == null || values.Count == 0 || values.Contains(documentsPermission.Key)))
				{
					ParametersProvider.Values.Add(documentsPermission.Key.ToString());
				}
			}
			return;
		}
		ParametersProvider.Values = values?.Select((short p) => p.ToString()).ToList();
	}
}
