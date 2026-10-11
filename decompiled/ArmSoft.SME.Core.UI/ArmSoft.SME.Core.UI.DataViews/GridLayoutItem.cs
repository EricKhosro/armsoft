using System;
using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Core.UI.DataViews;

internal class GridLayoutItem : DocumentGridRow
{
	public string Caption { get; set; }

	public string Tag { get; set; }

	public DataViewTypes Type { get; set; }

	public string Layout { get; set; }

	static GridLayoutItem()
	{
		DocumentGridRow.RegisterPropertyValidators(typeof(GridLayoutItem), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, ErrorInfo>> { 
		{
			"Caption",
			(DocumentGridRow row, ErrorInfo info) =>
			{
				ValidateCaption((GridLayoutItem)row, info);
			}
		} });
	}

	private static void ValidateCaption(GridLayoutItem row, ErrorInfo info)
	{
		PropertyValidator.EmptyStringValidate(row.Caption, info, Resources.Name);
	}
}
