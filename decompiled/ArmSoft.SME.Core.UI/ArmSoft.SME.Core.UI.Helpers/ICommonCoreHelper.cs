using System;
using System.IO;
using System.Xml;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Core.UI.Helpers;

public interface ICommonCoreHelper
{
	string GetDirectorySafe(string path);

	string GetExportDirectory();

	string GetFileNameFromExportDialog(string defaultExt, string filter, string fileName, bool overwritePrompt = true);

	string GetFileNameFromImportDialog(string defaultExt, string filter, string initialDirectory = "");

	(string FolderPath, string FileNames) GetFileNamesFromImportDialog(string defaultExt, string filter, string initialDirectory = "");

	void SaveXMLToFile(XmlDocument xmlDoc, string fileName);

	void CopyToFile(string filename, MemoryStream ms);

	void CheckWorkingPeriod(DateOnly currentDate, string controlName = null);

	WorkingPeriod GetWorkingPeriod();

	string GetRandomString(int length);
}
