using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Forms;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Statistics;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Helpers;

public class CallsRecordingHelper
{
	public class TestConfig
	{
		public string FolderPath { get; set; } = "";

		public string CallCaseName { get; set; } = "";

		public bool RecordCallSteps { get; set; }

		public bool RecordDataView { get; set; }

		public bool RecordDocument { get; set; }

		public bool RecordDirectory { get; set; }

		public bool RecordDocumentDefinition { get; set; }

		public bool RecordDirectoryDefinition { get; set; }

		public bool RecordDialogDefinition { get; set; }

		public DateTime RecordStartTime { get; set; }
	}

	public class ControlInfo
	{
		public string Name { get; set; }

		public string Type { get; set; }

		public string Caption { get; set; }

		public string GroupName { get; set; }

		public bool IsReadOnly { get; set; }

		public object Value { get; set; }

		public ControlInfo()
		{
		}

		public ControlInfo(string name, string type, object value)
		{
			Type = type;
			Value = value;
			Name = name;
		}

		public ControlInfo(string name, string type, string caption, string groupName, bool isReadOnly, object value)
		{
			Type = type;
			GroupName = groupName;
			Caption = caption;
			Value = value;
			Name = name;
			IsReadOnly = isReadOnly;
		}
	}

	private static ButtonEditExt _folderPath;

	public static List<CallCountStatisticStoreRequest> Calls { get; protected set; } = new List<CallCountStatisticStoreRequest>();

	public static List<CallDurationStatisticStoreRequest> CallsDurations { get; protected set; } = new List<CallDurationStatisticStoreRequest>();

	public static TestConfig Config { get; set; }

	public static bool EditRecordCallsConfig()
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Name = "RecordCalls",
			Caption = "Record calls"
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		dialogWindow.AddStoreValuesButton(container);
		_folderPath = dialogWindow.AddButtonEditControl("FolderPath", "Folder path", 255, isRequired: true, 1, storeValue: true);
		_folderPath.AllowDefaultButton = true;
		_folderPath.IsTextEditable = true;
		_folderPath.IsReadOnly = false;
		_folderPath.DefaultButtonClick += new SafeEventHandler<RoutedEventArgs>(FolderPathBrowse_Click).Handler;
		dialogWindow.AddStringControl("CallCaseName", "Call case Name", 100, isRequired: true, storeValue: true);
		dialogWindow.AddBooleanControl("RecordCallSteps", "Record call steps", storeValue: true);
		dialogWindow.AddBooleanControl("RecordDataView", "Record data view", storeValue: true);
		dialogWindow.AddBooleanControl("RecordDocument", "Record document", storeValue: true);
		dialogWindow.AddBooleanControl("RecordDirectory", "Record directory", storeValue: true);
		dialogWindow.AddBooleanControl("RecordDocumentDefinition", "Record document definition", storeValue: true);
		dialogWindow.AddBooleanControl("RecordDirectoryDefinition", "Record directory definition", storeValue: true);
		dialogWindow.AddBooleanControl("RecordDialogDefinition", "Record dialog definition", storeValue: true);
		bool flag = Convert.ToBoolean(dialogWindow.ShowDialog());
		if (flag)
		{
			Config = new TestConfig
			{
				FolderPath = dialogWindow["FolderPath"].ToString(),
				CallCaseName = dialogWindow["CallCaseName"].ToString(),
				RecordCallSteps = Convert.ToBoolean(dialogWindow["RecordCallSteps"]),
				RecordDataView = Convert.ToBoolean(dialogWindow["RecordDataView"]),
				RecordDocument = Convert.ToBoolean(dialogWindow["RecordDocument"]),
				RecordDirectory = Convert.ToBoolean(dialogWindow["RecordDirectory"]),
				RecordDocumentDefinition = Convert.ToBoolean(dialogWindow["RecordDocumentDefinition"]),
				RecordDirectoryDefinition = Convert.ToBoolean(dialogWindow["RecordDirectoryDefinition"]),
				RecordDialogDefinition = Convert.ToBoolean(dialogWindow["RecordDialogDefinition"]),
				RecordStartTime = DateTime.Now
			};
			return flag;
		}
		Config = null;
		return flag;
	}

	private static void FolderPathBrowse_Click(object sender, RoutedEventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		if (!string.IsNullOrWhiteSpace(_folderPath.Text))
		{
			folderBrowserDialog.SelectedPath = _folderPath.Text;
		}
		folderBrowserDialog.Description = "Folder path";
		folderBrowserDialog.ShowNewFolderButton = true;
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			_folderPath.Text = folderBrowserDialog.SelectedPath;
		}
	}

	public static void Clear(bool onlyRecords = false)
	{
		Calls.Clear();
		CallsDurations.Clear();
		Config = null;
	}

	public static void RecordCallDuration(CallDurationStatisticStoreRequest statistic, int minDurationInSForRecord = 10)
	{
		if (!(statistic.DurationInS < (double)minDurationInSForRecord))
		{
			CallsDurations.Add(statistic);
		}
	}

	public static void RecordCallStep(string type, string key)
	{
		Calls.Add(new CallCountStatisticStoreRequest
		{
			Type = type,
			Key = key,
			Count = 1
		});
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			ExportToTxt(DateTime.Now.ToString("HH.mm.ss") + ": type " + type + ", key" + key, Path.Combine(Config.FolderPath, "Metadata"), "steps_" + Config.CallCaseName + Config.RecordStartTime.ToString("MMdd_HHmm"));
		}
	}

	public static string RecordCallStepInFile(string data)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		return ExportToTxt(DateTime.Now.ToString("HH.mm.ss") + ": " + data, Path.Combine(Config.FolderPath, "Metadata"), "steps_" + Config.CallCaseName + Config.RecordStartTime.ToString("MMdd_HHmm"));
	}

	public static void ExportDataView(string dataViewName, short providerType, DataViewPanel panel)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		string text = "expected_" + dataViewName + "_" + Config.CallCaseName;
		string dataViewAccessIdName = GetDataViewAccessIdName(providerType);
		string path = (string.IsNullOrWhiteSpace(dataViewAccessIdName) ? "" : (dataViewAccessIdName + "_")) + dataViewName;
		string text2 = Path.Combine(Config.FolderPath, "DataViewResults", path);
		if (!Directory.Exists(text2))
		{
			Directory.CreateDirectory(text2);
		}
		string text3 = Path.Combine(text2, text + ".csv");
		if (File.Exists(text3))
		{
			text3 = Path.Combine(text2, text + "_" + DateTime.Now.ToString("MMdd_HHmmss") + ".csv");
		}
		using (File.Create(text3))
		{
		}
		panel.ExportToExcelDataAware("csv", text3, "Sheet1", showSaveDialog: false, showFile: false);
	}

	public static void ExportDataViewDialogResult(string dataViewName, short providerType, string dialogName, string dialogCaption, List<ControlInfo> param)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + dataViewName + " data view dialog result export start");
		}
		string dataViewAccessIdName = GetDataViewAccessIdName(providerType);
		string path = (string.IsNullOrWhiteSpace(dataViewAccessIdName) ? "" : (dataViewAccessIdName + "_")) + dataViewName;
		string exportFolder = Path.Combine(Config.FolderPath, "DialogParams", path);
		ExportToJson(new
		{
			Name = dialogName,
			Caption = dialogCaption,
			Controls = param
		}, exportFolder, "params_" + dataViewName + "_" + Config.CallCaseName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + dataViewName + " data view dialog result export finished");
		}
	}

	public static void ExportDialogDefinition(string name, string caption, List<ControlInfo> data)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + name + " dialog definition export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "DialogDefinitions", name);
		ExportToJson(new
		{
			Name = name,
			Caption = caption,
			IsDialogWindow = true,
			Controls = data
		}, exportFolder, "dialog_" + name);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + name + " dialog definition export finished");
		}
	}

	public static void ExportPOWindow(IPOWindow documentWindow, string fileName)
	{
		ExportToJson(documentWindow.GetControlsInfoForRecord(), fileName);
	}

	public static void ExportDialogWindow(DialogWindow dialogWindow, string fileName)
	{
		ExportToJson(new
		{
			Name = dialogWindow.Name,
			Caption = dialogWindow.Caption,
			IsDialogWindow = true,
			Controls = dialogWindow.GetControlsInfoForRecord()
		}, fileName);
	}

	public static void ExportDocumentWindow(string poName, IPOWindow documentWindow)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " document export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "DocumentWindows", poName);
		ExportToJson(documentWindow.GetControlsInfoForRecord(), exportFolder, "docWindow_" + poName + "_" + Config.CallCaseName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " document export finished");
		}
	}

	public static void ExportDirectoryWindow(string poName, IPOWindow directoryWindow)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " directory export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "DirectoryWindows", poName);
		ExportToJson(directoryWindow.GetControlsInfoForRecord(), exportFolder, "directoryWindow_" + poName + "_" + Config.CallCaseName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " directory export finished");
		}
	}

	public static void ExportDirectoryWindow(string poName, List<ControlInfo> data)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " directory export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "DirectoryWindows", poName);
		ExportToJson(data, exportFolder, "directoryWindow_" + poName + "_" + Config.CallCaseName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " directory export finished");
		}
	}

	public static void ExportDirectoryWindowDefinition(string poName, string caption, bool isDialogWindow, List<ControlInfo> data)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " directory definition export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "DirectoryWindowsDefinitions", poName);
		ExportToJson(new
		{
			Name = poName,
			Caption = caption,
			IsDialogWindow = isDialogWindow,
			Controls = data
		}, exportFolder, "directoryWindow_" + poName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " directory definition export finished");
		}
	}

	public static void ExportDocumetWindowDefinition(string poName, string caption, List<ControlInfo> data)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " document definition export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "DocumentWindowsDefinitions", poName);
		ExportToJson(new
		{
			Name = poName,
			Caption = caption,
			Controls = data
		}, exportFolder, "documentWindow_" + poName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " document definition export finished");
		}
	}

	public static void ExportDocument(string poName, string jsonData)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " document export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "Documents", poName);
		ExportToJson(jsonData, exportFolder, "document_" + poName + "_" + Config.CallCaseName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " document export finished");
		}
	}

	public static void ExportDirectory(string poName, string jsonData)
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " directory export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "Directories", poName);
		ExportToJson(jsonData, exportFolder, "directory_" + poName + "_" + Config.CallCaseName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + poName + " directory export finished");
		}
	}

	public static void ExportDataProviderResult<R>(string providerName, object param, List<R> rows) where R : ExtendableRow, new()
	{
		if (Config == null)
		{
			throw new ASException("Turn on record calls");
		}
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + providerName + " data provider result export start");
		}
		string exportFolder = Path.Combine(Config.FolderPath, "DataProviderResults", providerName);
		ExportToJson(rows, exportFolder, "expected_" + providerName + "_" + Config.CallCaseName);
		exportFolder = Path.Combine(Config.FolderPath, "DataProviderParams", providerName);
		ExportToJson(param, exportFolder, "params_" + providerName + "_" + Config.CallCaseName);
		if (Convert.ToBoolean(Config?.RecordCallSteps))
		{
			RecordCallStepInFile("Calls recording - " + providerName + " data provider result export finished");
		}
	}

	private static void ExportToJson<R>(List<R> data, string fileFullName) where R : new()
	{
		try
		{
			using FileStream stream = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write);
			using StreamWriter streamWriter = new StreamWriter(stream);
			foreach (R datum in data)
			{
				string value = JsonSerializer.Serialize(datum, JsonSerializerFactory.JsonOptions);
				streamWriter.WriteLine(value);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void ExportToJson(object data, string fileFullName)
	{
		try
		{
			string contents = JsonSerializer.Serialize(data, JsonSerializerFactory.JsonOptions);
			File.WriteAllText(fileFullName, contents);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static string ExportToJson<R>(List<R> data, string exportFolder, string fileName) where R : new()
	{
		try
		{
			if (!Directory.Exists(exportFolder))
			{
				Directory.CreateDirectory(exportFolder);
			}
			string text = Path.Combine(exportFolder, fileName + ".json");
			if (File.Exists(text))
			{
				text = Path.Combine(exportFolder, fileName + "_" + DateTime.Now.ToString("MMdd_HHmmss") + ".json");
			}
			ExportToJson(data, text);
			return text;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return "";
	}

	private static string ExportToJson(string jsonData, string exportFolder, string fileName)
	{
		try
		{
			if (!Directory.Exists(exportFolder))
			{
				Directory.CreateDirectory(exportFolder);
			}
			string text = Path.Combine(exportFolder, fileName + ".json");
			if (File.Exists(text))
			{
				text = Path.Combine(exportFolder, fileName + "_" + DateTime.Now.ToString("MMdd_HHmmss") + ".json");
			}
			File.WriteAllText(text, jsonData);
			return text;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return "";
	}

	private static string ExportToJson(object data, string exportFolder, string fileName)
	{
		try
		{
			if (!Directory.Exists(exportFolder))
			{
				Directory.CreateDirectory(exportFolder);
			}
			string contents = JsonSerializer.Serialize(data, JsonSerializerFactory.JsonOptions);
			string text = Path.Combine(exportFolder, fileName + ".json");
			if (File.Exists(text))
			{
				text = Path.Combine(exportFolder, fileName + "_" + DateTime.Now.ToString("MMdd_HHmmss") + ".json");
			}
			File.WriteAllText(text, contents);
			return text;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return "";
	}

	private static string ExportToTxt(string data, string exportFolder, string fileName)
	{
		try
		{
			if (!Directory.Exists(exportFolder))
			{
				Directory.CreateDirectory(exportFolder);
			}
			string text = Path.Combine(exportFolder, fileName + ".txt");
			if (!File.Exists(text))
			{
				using (File.Create(text))
				{
				}
			}
			File.AppendAllText(text, Environment.NewLine + data);
			return text;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return "";
	}

	private static string GetDataViewAccessIdName(short dataViewId)
	{
		foreach (dynamic smartEnumField in DefinitionMappingsService.GetPartialSmartEnum(typeof(UserAccessIdSE).Name).GetSmartEnumFields())
		{
			if (smartEnumField.RefObjectType == NavigationCommandRefObjectType.DataProvider && smartEnumField.RefObjectId == dataViewId)
			{
				return Convert.ToString(smartEnumField.Name);
			}
		}
		return null;
	}
}
