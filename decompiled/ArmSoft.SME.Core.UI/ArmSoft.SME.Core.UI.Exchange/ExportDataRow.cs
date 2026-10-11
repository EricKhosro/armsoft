using System;
using System.Collections.Generic;
using System.ComponentModel;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Exchange;

public class ExportDataRow
{
	public bool Selected { get; set; }

	public short ExportDataType { get; set; }

	public string DataName { get; set; }

	public int ProgressCount { get; set; }

	public int ErrorsCount { get; set; }

	public string FileId { get; set; }

	public ExportDataRowState State { get; set; }

	public ExportDataRow(short exportDataType, string exportDataCaption)
	{
		ExportDataType = exportDataType;
		DataName = exportDataCaption;
		ProgressCount = 0;
		ErrorsCount = 0;
		State = ExportDataRowState.None;
	}

	public static BindingList<ExportDataRow> GetList()
	{
		BindingList<ExportDataRow> bindingList = new BindingList<ExportDataRow>();
		List<short> exchangableDirectoryTypes = Settings.ServiceProvider.GetRequiredService<IPODefinitionHelper>().GetExchangableDirectoryTypes();
		foreach (CodeName item in SmartEnumHelper.GetSmartEnumCodeCaption(typeof(DirectoryTypeSE), exchangableDirectoryTypes.ConvertAll((short i) => i.ToString())))
		{
			bindingList.Add(new ExportDataRow(Convert.ToInt16(item.Code), item.Name));
		}
		return bindingList;
	}
}
