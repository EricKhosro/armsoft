using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Utils;
using DevExpress.Xpf.Reports.UserDesigner;
using DevExpress.XtraReports.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.DataViews;

public class TemplatesBase : DataView<TemplatesDataProvider.DataRow, TemplatesDataProvider.Param>
{
	private readonly ITemplateService _templateService;

	private readonly PrintTemplateTypeSE _templateType;

	public override bool AllowAdd => Settings.LoginService.IsAdmin;

	public override bool AllowEdit => Settings.LoginService.IsAdmin;

	public override bool AllowView => Settings.LoginService.IsAdmin;

	public override bool AllowDelete => Settings.LoginService.IsAdmin;

	public override bool AllowCreateCopy => Settings.LoginService.IsAdmin;

	public override string HelpId => "AdminTools_PrintingTemplates";

	public TemplatesBase(PrintTemplateTypeSE templateType)
	{
		DataProvider = new TemplatesDataProvider(Settings.CreateApiClient());
		_templateService = Settings.ServiceProvider.GetRequiredService<ITemplateService>();
		_templateType = templateType;
		FillColumnsFromDataProvider();
		Columns["File"].Visible = false;
		Columns["DocType"].Visible = false;
		Columns["FileFormat"].Visible = false;
		Columns["IsValidFileExtension"].Visible = false;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Name");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.TemplateType = _templateType;
	}

	public override void InitContextFunctions()
	{
		TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null || !Settings.LoginService.IsAdmin)
		{
			return;
		}
		if (!dataRow.IsSystem)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ImportTemplate", Resources.ImportFile, ImportTemplate, null, null, storeOpContext: true);
		}
		if (dataRow.IsValidFileExtension)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExportTemplate", Resources.ExportFile, ExportTemplate, null, null, storeOpContext: true);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ExploreFolder", Resources.ExploreFileFolder, ExploreFolder, null, null, storeOpContext: true);
			if (dataRow.FileFormat == "XML")
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenPrintTemplateDesignerForXML", Resources.EditPrintTemplate, OpenPrintTemplateDesignerForXML, null, null, storeOpContext: true);
			}
		}
	}

	public override void Add()
	{
		TemplateDialogWindow templateDialogWindow = new TemplateDialogWindow(DialogWindowMode.NewMode, _templateType, Settings.HostingEnvironment);
		if (Convert.ToBoolean(templateDialogWindow.ShowDialog()))
		{
			CreateTemplate(templateDialogWindow, DialogWindowMode.NewMode);
			Settings.HostingEnvironment.CurrentViewPanel.Update(Convert.ToString(templateDialogWindow["Name"]));
		}
	}

	public override void Edit()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			TemplateDialogWindow templateDialogWindow = new TemplateDialogWindow(DialogWindowMode.EditMode, _templateType, Settings.HostingEnvironment);
			if (Convert.ToBoolean(templateDialogWindow.ShowDialog()))
			{
				CreateTemplate(templateDialogWindow, DialogWindowMode.EditMode);
				Settings.HostingEnvironment.CurrentViewPanel.Update(Convert.ToString(templateDialogWindow["Name"]));
			}
		}
	}

	public override void CreateCopy()
	{
		TemplateDialogWindow templateDialogWindow = new TemplateDialogWindow(DialogWindowMode.CreateCopyMode, _templateType, Settings.HostingEnvironment);
		if (Convert.ToBoolean(templateDialogWindow.ShowDialog()))
		{
			CreateTemplate(templateDialogWindow, DialogWindowMode.CreateCopyMode);
			Settings.HostingEnvironment.CurrentViewPanel.Update(Convert.ToString(templateDialogWindow["Name"]));
		}
	}

	public override void Delete()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			if (dataRow.IsSystem)
			{
				throw new ASException(Resources.SystemTemplateDeleteNotAllowed);
			}
			if (MessageBox.Show(string.Format(Resources.ConfRecordDel, dataRow.Name), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				_templateService.Delete(dataRow.Name);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Name);
			}
		}
	}

	private void CreateTemplate(TemplateDialogWindow dialog, DialogWindowMode mode)
	{
		if (mode == DialogWindowMode.CreateCopyMode)
		{
			TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				Template template = new Template
				{
					Name = Convert.ToString(dialog["Name"]),
					TemplateType = _templateType,
					Caption = Convert.ToString(dialog["Caption"]),
					Enabled = Convert.ToBoolean(dialog["Enabled"])
				};
				if (!string.IsNullOrWhiteSpace(Convert.ToString(dialog["DocType"])))
				{
					template.DocType = Convert.ToInt16(dialog["DocType"]);
				}
				_templateService.Store(template);
				_templateService.CopyFile(dataRow.Name, template.Name, template.TemplateType);
			}
			return;
		}
		Template template2 = null;
		switch (mode)
		{
		case DialogWindowMode.NewMode:
			template2 = new Template
			{
				Name = Convert.ToString(dialog["Name"]),
				TemplateType = _templateType
			};
			break;
		case DialogWindowMode.EditMode:
			template2 = _templateService.Load(Convert.ToString(dialog["Name"]));
			break;
		}
		template2.Caption = Convert.ToString(dialog["Caption"]);
		if (!string.IsNullOrWhiteSpace(Convert.ToString(dialog["DocType"])))
		{
			template2.DocType = Convert.ToInt16(dialog["DocType"]);
		}
		template2.Enabled = Convert.ToBoolean(dialog["Enabled"]);
		_templateService.Store(template2);
	}

	private void ImportTemplate()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		OpenFileDialog openFileDialog = new OpenFileDialog();
		if (dataRow.IsValidFileExtension)
		{
			string text = dataRow.File;
			string text2 = Path.GetDirectoryName(text);
			if (!File.Exists(text) && !System.IO.Directory.Exists(text2))
			{
				text2 = GetTemplatesHomeDir();
				text = text2 + Path.DirectorySeparatorChar + Path.GetFileName(text);
			}
			openFileDialog.FileName = text;
			openFileDialog.InitialDirectory = text2;
		}
		else
		{
			string text2 = GetTemplatesHomeDir();
			openFileDialog.InitialDirectory = text2;
		}
		openFileDialog.Title = Resources.ImportFileSelection;
		openFileDialog.DefaultExt = ".xml";
		openFileDialog.Filter = "XML Files|*.xml;*.xsd|Word Documents|*.doc;*.docx;*.docm|Excel Workbooks|*.xls;*.xlsx;*.xlsm";
		if (Convert.ToBoolean(openFileDialog.ShowDialog()))
		{
			_templateService.ImportFile(dataRow.Name, _templateType, openFileDialog.FileName);
			Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Name);
		}
	}

	private static string GetTemplatesHomeDir()
	{
		string text = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Templates";
		if (!System.IO.Directory.Exists(text))
		{
			try
			{
				System.IO.Directory.CreateDirectory(text);
			}
			catch
			{
				text = Environment.CurrentDirectory;
			}
		}
		return text;
	}

	private void ExportTemplate()
	{
		TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null || !dataRow.IsValidFileExtension)
		{
			return;
		}
		string text = dataRow.File;
		string text2 = Path.GetDirectoryName(text);
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		if (!File.Exists(text) && !System.IO.Directory.Exists(text2))
		{
			text2 = GetTemplatesHomeDir();
			text = text2 + Path.DirectorySeparatorChar + Path.GetFileName(text);
		}
		saveFileDialog.FileName = text;
		saveFileDialog.InitialDirectory = text2;
		saveFileDialog.Title = Resources.ExportFileSelection;
		saveFileDialog.DefaultExt = dataRow.FileFormat;
		string fileFormat = dataRow.FileFormat;
		if (fileFormat != null)
		{
			int length = fileFormat.Length;
			if (length != 4)
			{
				if (length == 5)
				{
					char c = fileFormat[1];
					if (c != 'd')
					{
						if (c != 'r')
						{
							if (c == 'x')
							{
								if (fileFormat == ".xlsx")
								{
									goto IL_01a3;
								}
								if (fileFormat == ".xlsm")
								{
									saveFileDialog.Filter = "Excel Workbooks|*.xls;*.xlsx;*.xlsm";
									goto IL_01e6;
								}
							}
						}
						else if (fileFormat == ".repx")
						{
							saveFileDialog.Filter = "Report Files|*.repx";
							goto IL_01e6;
						}
					}
					else if (fileFormat == ".docx" || fileFormat == ".docm")
					{
						goto IL_0196;
					}
				}
			}
			else
			{
				switch (fileFormat[2])
				{
				case 'o':
					break;
				case 'l':
					goto IL_0169;
				case 'm':
					goto IL_0179;
				default:
					goto IL_01ca;
				}
				if (fileFormat == ".doc")
				{
					goto IL_0196;
				}
			}
		}
		goto IL_01ca;
		IL_0196:
		saveFileDialog.Filter = "Word Documents|*.doc;*.docx;*.docm";
		goto IL_01e6;
		IL_0169:
		if (fileFormat == ".xls")
		{
			goto IL_01a3;
		}
		goto IL_01ca;
		IL_01e6:
		if (Convert.ToBoolean(saveFileDialog.ShowDialog()))
		{
			text = saveFileDialog.FileName;
			byte[] bytes = _templateService.ExportFile(dataRow.Name, _templateType);
			File.WriteAllBytes(text, bytes);
		}
		return;
		IL_01ca:
		saveFileDialog.Filter = dataRow.FileFormat + " Files|*" + dataRow.FileFormat;
		goto IL_01e6;
		IL_0179:
		if (!(fileFormat == ".xml"))
		{
			goto IL_01ca;
		}
		saveFileDialog.Filter = "XML Files|*.xml";
		goto IL_01e6;
		IL_01a3:
		saveFileDialog.Filter = "Excel Workbooks|*.xls;*.xlsx";
		goto IL_01e6;
	}

	private void ExploreFolder()
	{
		TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null && dataRow.IsValidFileExtension)
		{
			string file = dataRow.File;
			if (!File.Exists(file))
			{
				throw new ASException(string.Format(Resources.FileDoesNotExist, file));
			}
			string directoryName = Path.GetDirectoryName(file);
			Process.Start("explorer.exe", directoryName);
		}
	}

	private void OpenPrintTemplateDesignerForXML()
	{
		TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Mouse.OverrideCursor = Cursors.Wait;
		string extension = Path.GetExtension(dataRow.File);
		string tmpFile = Settings.GetTempFileName(extension);
		byte[] bytes = Settings.CreateApiClient().TemplateRoutes.ExportFile(dataRow.Name, _templateType);
		File.WriteAllBytes(tmpFile, bytes);
		DevExpress.XtraReports.Configuration.Settings.Default.StorageOptions.Extension = extension;
		XtraReportTemplate xrep = new XtraReportTemplate(dataRow.DocType);
		DeserializationSettings.InvokeTrusted(() =>
		{
			xrep.LoadLayoutFromXml(tmpFile);
		});
		Mouse.OverrideCursor = null;
		ReportDesigner reportDesigner = new ReportDesigner();
		reportDesigner.DocumentSource = xrep;
		reportDesigner.ShowWindow(Settings.HostingEnvironment);
	}
}
