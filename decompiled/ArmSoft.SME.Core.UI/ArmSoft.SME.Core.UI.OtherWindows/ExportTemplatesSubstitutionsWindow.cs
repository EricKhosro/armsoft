using System;
using System.Diagnostics;
using System.IO;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.OtherWindows;

public class ExportTemplatesSubstitutionsWindow : DialogWindow
{
	private readonly DropDownControl _docType;

	private readonly string _fileName;

	public ExportTemplatesSubstitutionsWindow()
	{
		Caption = ArmSoft.SME.Common.Resources.PrintTemplates;
		_docType = AddDropDownControl("DocType", ArmSoft.SME.Common.Resources.DocumentType, new SmartEnum(typeof(PrintTemplateDocumentTypeSE), null, padLeft: false, '0', allSubTypes: true));
		if (Convert.ToBoolean(ShowDialog()))
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				DefaultExt = "*.txt",
				Filter = "Text document|*.txt",
				AddExtension = true,
				FileName = "TemplatesFields",
				OverwritePrompt = true,
				Title = "Save file",
				ValidateNames = true
			};
			if (Convert.ToBoolean(saveFileDialog.ShowDialog()))
			{
				_fileName = saveFileDialog.FileName;
				ExportTemplateBindingsFields(_fileName, _docType.Code);
			}
		}
	}

	public static void ExportTemplateBindingsFields(string fileName, string docType)
	{
		try
		{
			byte[] array = Settings.CreateApiClient().TemplateRoutes.ExportPrintModelDefinition(docType, fileName);
			using FileStream fileStream = File.Create(fileName);
			fileStream.Write(array);
			fileStream.Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return;
		}
		Process.Start(new ProcessStartInfo(fileName)
		{
			UseShellExecute = true
		});
	}
}
