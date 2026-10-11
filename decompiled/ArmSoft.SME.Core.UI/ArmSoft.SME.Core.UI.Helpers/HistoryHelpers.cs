using System;
using System.Runtime.CompilerServices;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.StandartWindow;

namespace ArmSoft.SME.Core.UI.Helpers;

public class HistoryHelpers
{
	public static void WriteDocumentItemInHistory(Document doc)
	{
		HistoryItem historyItem = new HistoryItem(HistoryItem.ItemCategory.Document)
		{
			Type = 101,
			Id = doc.ISN.ToString(),
			Description = string.Format(Resources.DocDescrWithDate, SmartEnumHelper.GetSmartEnumRowCaption(typeof(DocumentTypeSE), doc.POType.ToString()), doc.DocNum, doc.DocDate.FormatDDMMYYYY()) + " [" + DateTime.Now.ToString() + "]"
		};
		historyItem.Details = historyItem.Description;
		historyItem.Icon = "doc_16.svg";
		Settings.HostingEnvironment.WriteInHistory(historyItem);
	}

	public static void WriteDirectoryItemInHistory(Directory directory)
	{
		HistoryItem historyItem = new HistoryItem(HistoryItem.ItemCategory.Document)
		{
			Type = 102,
			Id = directory.POType.ToString().PadLeft(2) + directory.Code
		};
		string partialSmartEnumCaption = SmartEnumHelper.GetPartialSmartEnumCaption(typeof(DirectoryTypeSE), directory.POType.ToString());
		string directoryHistoryItemCaption = Resources.DirectoryHistoryItemCaption;
		InlineArray4<object> buffer = default;
		buffer[0] = partialSmartEnumCaption;
		buffer[1] = directory.Code;
		buffer[2] = directory.Name;
		buffer[3] = DateTime.Now;
		historyItem.Description = string.Format(directoryHistoryItemCaption, (ReadOnlySpan<object?>)buffer);
		historyItem.Details = historyItem.Description;
		historyItem.Icon = "table_16.svg";
		Settings.HostingEnvironment.WriteInHistory(historyItem);
	}
}
