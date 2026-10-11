using System;
using System.Globalization;
using System.Windows.Data;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.Converters;

public class TemplateTypeToImage : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		if (value != null)
		{
			PrintTemplateFileFormatSE printTemplateFileFormatSE = SmartEnum<PrintTemplateFileFormatSE, string>.FromValue(value.ToString());
			if (printTemplateFileFormatSE == PrintTemplateFileFormatSE.XLS || printTemplateFileFormatSE == PrintTemplateFileFormatSE.XLSM || printTemplateFileFormatSE == PrintTemplateFileFormatSE.XLSX)
			{
				return ImageService.GetImage("doc_export_excel_16.svg");
			}
			if (printTemplateFileFormatSE == PrintTemplateFileFormatSE.DOC || printTemplateFileFormatSE == PrintTemplateFileFormatSE.DOCM || printTemplateFileFormatSE == PrintTemplateFileFormatSE.DOCX)
			{
				return ImageService.GetImage("doc_export_word_16.svg");
			}
			if (printTemplateFileFormatSE == PrintTemplateFileFormatSE.REPX)
			{
				return ImageService.GetImage("doc_preview_16.svg");
			}
			if (printTemplateFileFormatSE == PrintTemplateFileFormatSE.XML)
			{
				return ImageService.GetImage("doc_export_xml_16.svg");
			}
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
