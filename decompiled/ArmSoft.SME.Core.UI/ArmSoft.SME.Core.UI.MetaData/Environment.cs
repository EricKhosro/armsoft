using System;
using System.IO;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.HelpClasses;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.MetaData;

public class Environment : ArmSoft.SME.Common.MetaData.Environment
{
	public override string GetStringResource(string resourceName)
	{
		return Settings.StringResourceService.GetStringResource(resourceName);
	}

	public override string LoadScriptFromFile(string fileRelativeName)
	{
		string text = Path.Combine(CurrentDirectory, fileRelativeName);
		if (!File.Exists(text))
		{
			throw new ASException(string.Format(Resources.FileDoesNotExist, text));
		}
		return File.ReadAllText(text);
	}

	public override TreeCacheRow Tree(string treeId)
	{
		return Settings.CoreCacheService.Tree.GetItem(treeId);
	}

	public static string GetFileNameForExportToXML()
	{
		try
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (!Directory.Exists(RegistryHelper.ReadStringFromRegistry("ExportDirectory", "", Directory.GetCurrentDirectory())))
			{
				saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
			}
			saveFileDialog.FileName = "Export_" + DateTime.Now.ToString("MMdd_HHmm");
			saveFileDialog.DefaultExt = ".xml";
			saveFileDialog.Filter = "XML Files (.xml)|*.xml";
			if (saveFileDialog.ShowDialog().Value)
			{
				string fileName = saveFileDialog.FileName;
				RegistryHelper.WriteExportDirectory(Path.GetDirectoryName(fileName));
				return fileName;
			}
		}
		catch (Exception)
		{
			return "";
		}
		return "";
	}
}
