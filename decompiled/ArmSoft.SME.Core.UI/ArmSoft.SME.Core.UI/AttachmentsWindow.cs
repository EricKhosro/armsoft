using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Attachments;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Ribbon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public class AttachmentsWindow : ThemedWindow, INotifyPropertyChanged
{
	protected internal string Caption = "";

	private DateTime _lastModifyDate;

	private readonly bool _isReadOnly;

	private bool _isDialog;

	internal string RegistryKey = "";

	private bool _forceClose;

	private bool _firstActivation = true;

	private bool _ribbonToolbarShowAbove = true;

	private bool _ribbonIsMinimized = true;

	private bool _restoringSettings;

	private readonly BarManager _barManager;

	private readonly BarButtonItem _barButtonSave;

	private readonly BarButtonItem _barButtonSaveAndClose;

	private readonly BarButtonItem _barButtonAdd;

	private readonly BarButtonItem _barButtonDelete;

	private readonly BarButtonItem _barButtonClose;

	private readonly BarButtonItem _barButtonView;

	private readonly BarButtonItem _barButtonFileSaveAs;

	private readonly BarButtonItem _barButtonFileSaveAsAll;

	private readonly BarButtonItem _barButtonHelp;

	private readonly RibbonControl _ribbon;

	private readonly RibbonPage _ribbonPageHome;

	private readonly RibbonPageGroup _ribbonPageOperations;

	private readonly PropertyChangeNotifier _isChangedNotifier;

	public static readonly DependencyProperty IsChangedProperty = DependencyProperty.Register("IsChanged", typeof(bool), typeof(AttachmentsWindow));

	private IAttachmentsService _attachmentsService;

	private readonly AttachType _attachType;

	private readonly GridControl _grid;

	private readonly TableView _tableView;

	private BindingList<FileRow> _filesList;

	private BindingList<LinkRow> _linksList;

	public Guid DocISN { get; }

	public bool IsChanged
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsChangedProperty);
		}
		set
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChanged"));
			((DependencyObject)this).SetValue(IsChangedProperty, (object)value);
		}
	}

	public string GetTitle
	{
		get
		{
			if (DocISN == Guid.Empty)
			{
				return "";
			}
			string text;
			if (_attachType == AttachType.File)
			{
				text = ArmSoft.SME.Common.Resources.Files;
			}
			else
			{
				text = ((_attachType != AttachType.Link) ? ArmSoft.SME.Common.Resources.Documents : ArmSoft.SME.Common.Resources.Links);
			}
			text = text.Substring(0, 1).ToLower() + text.Substring(1);
			return Settings.LoginService.Database + " - " + string.Format(ArmSoft.SME.Common.Resources.Attachments, text);
		}
	}

	public virtual string HelpID => "UI_AttachedFiles";

	private static RoutedEventHandler TextChangedHandler { get; set; }

	private static RoutedEventHandler GridRowsChangedHandler { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	private void IsChangedChanged(object sender, EventArgs e)
	{
		Title = GetTitle + (IsChanged ? "*" : "");
	}

	public new bool? ShowDialog()
	{
		_isDialog = true;
		_attachmentsService = Settings.ServiceProvider.GetRequiredService<IAttachmentsService>();
		ConstructUI();
		LoadData();
		OnEdit();
		SetEvents();
		return base.ShowDialog();
	}

	protected virtual void ConstructUI()
	{
		MakeWindowTitle();
		_ribbonPageHome.Caption = ArmSoft.SME.Common.Resources.Home;
		_ribbonPageOperations.Caption = ArmSoft.SME.Common.Resources.Operations;
		_barButtonSave.Content = ArmSoft.SME.Common.Resources.Save;
		_barButtonSave.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSave_ItemClick).Handler;
		_barButtonSaveAndClose.Content = ArmSoft.SME.Common.Resources.SaveAndClose;
		_barButtonSaveAndClose.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSaveAndClose_ItemClick).Handler;
		_barButtonView.Content = ArmSoft.SME.Common.Resources.View;
		_barButtonView.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonView_ItemClick).Handler;
		_barButtonHelp.Content = ArmSoft.SME.Common.Resources.Help;
		_barButtonHelp.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonHelp_ItemClick).Handler;
		_barButtonFileSaveAs.Content = ArmSoft.SME.Common.Resources.FileSaveAs;
		_barButtonFileSaveAs.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonFileSaveAs_ItemClick).Handler;
		_barButtonFileSaveAsAll.Content = ArmSoft.SME.Common.Resources.FileaAllSaveAs;
		_barButtonFileSaveAsAll.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonFileSaveAsAll_ItemClick).Handler;
		SuperTip superTip = new SuperTip();
		SuperTipHeaderItem item = new SuperTipHeaderItem
		{
			Content = ArmSoft.SME.Common.Resources.SaveAndClose + " " + HotKeysSE.Ctrl_Enter.CaptionWithBrackets
		};
		superTip.Items.Add(item);
		_barButtonSaveAndClose.SuperTip = superTip;
		_barButtonAdd.Content = ArmSoft.SME.Common.Resources.NewAdd;
		_barButtonAdd.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonAdd_ItemClick).Handler;
		_barButtonDelete.Content = ArmSoft.SME.Common.Resources.Delete;
		_barButtonDelete.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonDelete_ItemClick).Handler;
		_barButtonClose.Content = ArmSoft.SME.Common.Resources.Close;
		_barButtonClose.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonClose_ItemClick).Handler;
		_barButtonSave.IsVisible = _barButtonSave.IsVisible && !_isReadOnly;
		_barButtonSaveAndClose.IsVisible = !_isReadOnly;
		_barButtonAdd.IsVisible = !_isReadOnly;
		_barButtonDelete.IsVisible = !_isReadOnly;
		_barButtonFileSaveAs.IsVisible = _attachType == AttachType.File;
		_barButtonFileSaveAsAll.IsVisible = _attachType == AttachType.File;
		_ribbon.IsMinimized = _ribbonIsMinimized;
		_ribbon.ToolbarShowMode = ((!_ribbonToolbarShowAbove) ? RibbonQuickAccessToolbarShowMode.ShowBelow : RibbonQuickAccessToolbarShowMode.Default);
		AddToolbarItems();
		MinWidth = SizeCalculator.Width(300.0);
		MinHeight = SizeCalculator.Height(200 + ((_attachType == AttachType.File) ? 25 : 0));
		Icon = Settings.HostingEnvironment.Icon;
		Width = 971 + ((_attachType == AttachType.Link) ? 80 : 0);
		Height = 300 + ((_attachType == AttachType.File) ? 25 : 0);
		RestoreSettings(Width, Height);
	}

	protected void RestoreSettings(double defaultWidth, double defaultHeight)
	{
		bool flag = false;
		if (string.IsNullOrWhiteSpace(RegistryKey))
		{
			flag = true;
		}
		else
		{
			try
			{
				_restoringSettings = true;
				using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"Attachment Windows"}\\{_attachType}{RegistryKey}", writable: false);
				if (registryKey == null)
				{
					flag = true;
				}
				else
				{
					Settings.RestoreWindowSize(this, registryKey, defaultWidth, defaultHeight);
					bool isMinimized = registryKey.GetValue("RibbonMinimized", "1").ToString() == "1";
					bool flag2 = registryKey.GetValue("RibbonToolbarMode", "1").ToString() == "1";
					if (registryKey.GetValue("State", "1").ToString() == "0")
					{
						WindowState = WindowState.Maximized;
					}
					else
					{
						WindowState = WindowState.Normal;
					}
					_ribbon.ToolbarShowMode = ((!flag2) ? RibbonQuickAccessToolbarShowMode.ShowBelow : RibbonQuickAccessToolbarShowMode.Default);
					_ribbon.IsMinimized = isMinimized;
				}
			}
			catch (Exception)
			{
				flag = true;
			}
			finally
			{
				_restoringSettings = false;
			}
		}
		if (flag)
		{
			_restoringSettings = true;
			Width = SizeCalculator.Width(defaultWidth);
			Height = SizeCalculator.Height(defaultHeight);
			_ribbon.ToolbarShowMode = RibbonQuickAccessToolbarShowMode.Default;
			_ribbon.IsMinimized = true;
			_restoringSettings = false;
		}
	}

	protected void StoreSettings()
	{
		if (string.IsNullOrWhiteSpace(RegistryKey))
		{
			return;
		}
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\Attachment Windows\\" + _attachType.ToString() + RegistryKey);
			if (registryKey != null)
			{
				if (WindowState == WindowState.Normal)
				{
					registryKey.SetValue("Width", Width.ToString());
					registryKey.SetValue("Height", Height.ToString());
					registryKey.SetValue("State", "1");
				}
				else if (WindowState == WindowState.Maximized)
				{
					registryKey.SetValue("State", "0");
				}
				registryKey.SetValue("RibbonMinimized", _ribbon.IsMinimized ? "1" : "0");
				registryKey.SetValue("RibbonToolbarMode", (_ribbon.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.Default) ? "1" : "0");
			}
		}
		catch (Exception)
		{
		}
	}

	protected virtual System.Windows.Controls.Control ControlByName(string name)
	{
		return null;
	}

	protected void MakeWindowTitle()
	{
		_barButtonSave.IsEnabled = !_isReadOnly;
		_barButtonSaveAndClose.IsEnabled = !_isReadOnly;
		_barButtonAdd.IsEnabled = !_isReadOnly;
		_barButtonDelete.IsEnabled = !_isReadOnly;
		IsChanged = !IsChanged;
		IsChanged = !IsChanged;
	}

	protected virtual void OnEdit()
	{
	}

	protected void CloseDocument()
	{
		if (_isDialog)
		{
			DialogResult = false;
		}
		else
		{
			Close();
		}
	}

	protected void SaveAndCloseDocument()
	{
		if (_isReadOnly || InnerSave())
		{
			_forceClose = true;
			if (_isDialog)
			{
				DialogResult = true;
			}
			else
			{
				Close();
			}
		}
	}

	protected bool SaveDocument()
	{
		bool flag = InnerSave();
		if (flag)
		{
			ReLoad();
			OnEdit();
		}
		IsChanged = !flag;
		return flag;
	}

	private bool AddRow()
	{
		bool flag = false;
		string compName = "";
		try
		{
			compName = Environment.MachineName;
		}
		catch (Exception)
		{
		}
		int visibleIndex = 0;
		if (_attachType == AttachType.File)
		{
			string text = "";
			try
			{
				Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
				{
					InitialDirectory = RegistryHelper.ReadImportDirectory(),
					Multiselect = true
				};
				if (openFileDialog.ShowDialog().Value)
				{
					Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
					RegistryHelper.WriteImportDirectory(Path.GetDirectoryName(openFileDialog.FileNames[0]));
					Stream[] array = null;
					string[] safeFileNames = openFileDialog.SafeFileNames;
					for (int i = 0; i <= openFileDialog.FileNames.Length - 1; i++)
					{
						text = openFileDialog.FileNames[i];
						FileRow fileRow = new FileRow
						{
							FileNameWithPath = text,
							FileName = safeFileNames[i],
							CompName = compName,
							UserId = Settings.LoginService.UserId,
							UserName = Settings.LoginService.UserName,
							LastModifyDate = DateTime.Now,
							IsNew = true
						};
						array = openFileDialog.OpenFiles();
						if (array[0].CanRead)
						{
							fileRow.Size = ((decimal)array[i].Length / 1024m).RoundAs(1);
							array[i].Close();
						}
						_filesList.Add(fileRow);
					}
					visibleIndex = _filesList.Count - 1;
					Mouse.OverrideCursor = null;
					flag = true;
				}
			}
			catch (Exception ex2)
			{
				ex2.ThrowStorageException(text);
			}
		}
		else if (_attachType == AttachType.Link)
		{
			LinkRow item = new LinkRow
			{
				CompName = compName,
				UserId = Settings.LoginService.UserId,
				UserName = Settings.LoginService.UserName,
				LastModifyDate = DateTime.Now,
				IsNew = true
			};
			_linksList.Add(item);
			visibleIndex = _linksList.Count - 1;
			flag = true;
		}
		if (flag)
		{
			_grid.RefreshData();
			IsChanged = true;
			int rowHandleByVisibleIndex = _grid.GetRowHandleByVisibleIndex(visibleIndex);
			_grid.RefreshRow(rowHandleByVisibleIndex);
			_tableView.MoveFocusedRow(_grid.GetRowVisibleIndexByHandle(rowHandleByVisibleIndex));
			_grid.CurrentColumn = _tableView.VisibleColumns[0];
			((DispatcherObject)this).Dispatcher.Invoke((Action)MyShowEditor, (DispatcherPriority)6);
		}
		return flag;
	}

	private void MyShowEditor()
	{
		_tableView.ShowEditor();
		if (_tableView.ActiveEditor != null)
		{
			_tableView.ActiveEditor.SelectAll();
		}
	}

	private void DeleteRow()
	{
		_tableView.CommitEditing();
		_tableView.HideEditor();
		int focusedRowHandle = _tableView.FocusedRowHandle;
		if (focusedRowHandle >= 0)
		{
			_tableView.DeleteRow(focusedRowHandle);
			_grid.RefreshData();
			IsChanged = true;
		}
	}

	private void FileSaveAs()
	{
		if (_attachType != AttachType.File)
		{
			return;
		}
		FileRow fileRow = (FileRow)_grid.GetFocusedRow();
		if (fileRow == null)
		{
			return;
		}
		string text = "";
		if (IsChanged && fileRow.IsNew && !string.IsNullOrWhiteSpace(fileRow.FileNameWithPath))
		{
			if (File.Exists(fileRow.FileNameWithPath))
			{
				text = fileRow.FileNameWithPath;
			}
		}
		else
		{
			string exportDirectory = Settings.GetExportDirectory();
			string fileName = fileRow.FileName;
			text = exportDirectory + Path.DirectorySeparatorChar + fileName;
			if (File.Exists(text) && MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.FileAlreadyExists, text) + Environment.NewLine + ArmSoft.SME.Common.Resources.ExploreFileFolder, Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, this) == MessageBoxResult.Yes)
			{
				Process process = new Process();
				process.StartInfo = new ProcessStartInfo("explorer.exe", exportDirectory)
				{
					UseShellExecute = true
				};
				process.Start();
				text = "";
			}
			else
			{
				try
				{
					text = "";
					Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
					{
						InitialDirectory = exportDirectory,
						FileName = fileName,
						AddExtension = true,
						OverwritePrompt = true,
						Title = ArmSoft.SME.Common.Resources.FileSaveAs,
						ValidateNames = true
					};
					if (saveFileDialog.ShowDialog().Value)
					{
						text = saveFileDialog.FileName;
						_attachmentsService.ExportFromDBToFile(DocISN, text, fileName);
						RegistryHelper.WriteExportDirectory(text);
					}
				}
				catch (Exception ex)
				{
					ex.ThrowStorageException(text);
				}
			}
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			try
			{
				Process process2 = new Process();
				process2.StartInfo = new ProcessStartInfo(text)
				{
					UseShellExecute = true
				};
				process2.Start();
			}
			catch (Exception)
			{
				MessageBox.Show(ArmSoft.SME.Common.Resources.ExportOk + " " + text, ArmSoft.SME.Common.Resources.FileSaveAs);
			}
		}
	}

	private void FileSaveAsAll()
	{
		if (_attachType != AttachType.File || _filesList.Count <= 0)
		{
			return;
		}
		_tableView.CommitEditing();
		_tableView.HideEditor();
		string text = "";
		try
		{
			if (IsChanged && (MessageBox.Show(ArmSoft.SME.Common.Resources.ForActionMustSaveDoc, ArmSoft.SME.Common.Resources.AttachedFiles, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes || !SaveDocument()))
			{
				return;
			}
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
			{
				SelectedPath = Settings.GetExportDirectory()
			};
			if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
			{
				return;
			}
			string selectedPath = folderBrowserDialog.SelectedPath;
			foreach (FileRow files in _filesList)
			{
				text = Path.Combine(selectedPath, files.FileName);
				if (!File.Exists(text) || MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.FileAlreadyExists, text) + Environment.NewLine + ArmSoft.SME.Common.Resources.AskContinue, Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, this) == MessageBoxResult.Yes)
				{
					_attachmentsService.ExportFromDBToFile(DocISN, text, files.FileName);
				}
			}
			RegistryHelper.WriteExportDirectory(selectedPath);
		}
		catch (Exception ex)
		{
			ex.ThrowStorageException(text);
		}
	}

	private void ViewRow()
	{
		_tableView.CommitEditing();
		_tableView.HideEditor();
		if (_attachType == AttachType.File)
		{
			FileRow fileRow = (FileRow)_grid.GetFocusedRow();
			if (fileRow == null)
			{
				return;
			}
			string text = "";
			if (IsChanged && fileRow.IsNew && !string.IsNullOrWhiteSpace(fileRow.FileNameWithPath))
			{
				if (File.Exists(fileRow.FileNameWithPath))
				{
					text = fileRow.FileNameWithPath;
				}
			}
			else
			{
				string text2 = Settings.GetTempPath() + DocISN;
				string fileName = fileRow.FileName;
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				text = text2 + Path.DirectorySeparatorChar + fileName;
				if (!File.Exists(text))
				{
					try
					{
						_attachmentsService.ExportFromDBToFile(DocISN, text, fileName);
					}
					catch (Exception ex)
					{
						ex.ThrowStorageException(text);
					}
				}
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				try
				{
					Process process = new Process();
					process.StartInfo = new ProcessStartInfo(text)
					{
						UseShellExecute = true
					};
					process.Start();
				}
				catch (Exception)
				{
					MessageBox.Show(ArmSoft.SME.Common.Resources.ExportOk + " " + text, ArmSoft.SME.Common.Resources.View);
				}
			}
		}
		else
		{
			if (_attachType != AttachType.Link)
			{
				return;
			}
			LinkRow linkRow = (LinkRow)_grid.GetFocusedRow();
			if (linkRow != null && !string.IsNullOrWhiteSpace(linkRow.Link))
			{
				try
				{
					Process process2 = new Process();
					process2.StartInfo = new ProcessStartInfo(linkRow.Link)
					{
						UseShellExecute = true
					};
					process2.Start();
				}
				catch (Exception)
				{
					MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.InvalidLink, linkRow.Link), ArmSoft.SME.Common.Resources.View);
				}
			}
		}
	}

	private bool InnerSave()
	{
		if (_lastModifyDate != _attachmentsService.GetLastModifyDate(DocISN))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ObjectChangedWhenEdited, Caption));
		}
		_tableView.CommitEditing();
		_tableView.HideEditor();
		if (_attachType == AttachType.File)
		{
			AttachmentRequestModel attachmentRequestModel = new AttachmentRequestModel
			{
				ISN = DocISN,
				FileRows = _filesList.ToList()
			};
			foreach (FileRow fileRow in attachmentRequestModel.FileRows)
			{
				if (!string.IsNullOrWhiteSpace(fileRow.FileNameWithPath) && !File.Exists(fileRow.FileNameWithPath))
				{
					throw new RESTException(string.Format(ArmSoft.SME.Common.Resources.FileDoesNotExist, fileRow.FileNameWithPath));
				}
			}
			_attachmentsService.StoreFiles(attachmentRequestModel);
			foreach (FileRow files in _filesList)
			{
				if (!string.IsNullOrWhiteSpace(files.FileNameWithPath) && files.IsNew)
				{
					_attachmentsService.ImportFromFileToDB(DocISN, files.FileName, File.ReadAllBytes(files.FileNameWithPath));
				}
			}
		}
		else if (_attachType == AttachType.Link)
		{
			_attachmentsService.StoreLinks(new AttachmentRequestModel
			{
				ISN = DocISN,
				LinkRows = _linksList.ToList()
			});
		}
		_lastModifyDate = _attachmentsService.GetLastModifyDate(DocISN);
		return true;
	}

	private void ReLoad()
	{
		LoadData();
		_grid.RefreshData();
		IsChanged = false;
	}

	private void BarButtonSave_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!_isReadOnly)
		{
			SaveDocument();
		}
	}

	private void BarButtonSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!_isReadOnly)
		{
			SaveAndCloseDocument();
		}
	}

	private void BarButtonAdd_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!_isReadOnly)
		{
			AddRow();
		}
	}

	private void BarButtonDelete_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!_isReadOnly)
		{
			DeleteRow();
		}
	}

	private void BarButtonView_ItemClick(object sender, ItemClickEventArgs e)
	{
		ViewRow();
	}

	private void BarButtonFileSaveAs_ItemClick(object sender, ItemClickEventArgs e)
	{
		FileSaveAs();
	}

	private void BarButtonFileSaveAsAll_ItemClick(object sender, ItemClickEventArgs e)
	{
		FileSaveAsAll();
	}

	private void BarButtonClose_ItemClick(object sender, ItemClickEventArgs e)
	{
		CloseDocument();
	}

	private void BarButtonHelp_ItemClick(object sender, ItemClickEventArgs e)
	{
		Settings.HostingEnvironment.ShowHelp(HelpID);
	}

	protected internal void OnGridEscapePressed(object sender, EventArgs e)
	{
		CloseDocument();
	}

	private void AttachmentsWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivation)
		{
			_grid.Focus();
			_firstActivation = false;
		}
	}

	private static void TextChanged(object sender, RoutedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected Obj, but got Unknown
		if (Window.GetWindow((DependencyObject)sender) is AttachmentsWindow { IsChanged: false, _firstActivation: false } attachmentsWindow)
		{
			attachmentsWindow.IsChanged = true;
		}
	}

	private static void SetEvents()
	{
		TextChangedHandler = TextChanged;
		EventManager.RegisterClassHandler(typeof(ImageEdit), BaseEdit.EditValueChangedEvent, TextChangedHandler);
		GridRowsChangedHandler = GridRowsChanged;
		EventManager.RegisterClassHandler(typeof(DocumentGridControl), DocumentGridControl.VisibleRowsCountChanged, GridRowsChangedHandler);
	}

	private void Ribbon_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (!_restoringSettings)
		{
			bool flag = _ribbonIsMinimized != _ribbon.IsMinimized;
			if (!flag)
			{
				flag = ((!_ribbonToolbarShowAbove) ? (_ribbon.ToolbarShowMode != RibbonQuickAccessToolbarShowMode.ShowBelow) : (_ribbon.ToolbarShowMode != RibbonQuickAccessToolbarShowMode.Default));
			}
			if (flag)
			{
				StoreSettings();
			}
			_ribbonIsMinimized = _ribbon.IsMinimized;
			_ribbonToolbarShowAbove = _ribbon.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.Default;
		}
	}

	private void AttachmentsWindow_Closing(object sender, CancelEventArgs e)
	{
		if (!Settings.LoginService.IsLoggedIn)
		{
			return;
		}
		if (!_forceClose && IsChanged && Settings.ParametersService.QuestWhenCloseWind())
		{
			if (WindowState == WindowState.Minimized)
			{
				WindowState = WindowState.Normal;
			}
			if (MessageBox.Show(ArmSoft.SME.Common.Resources.QuestionWhenCloseWindow, Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, this) == MessageBoxResult.No)
			{
				e.Cancel = true;
			}
		}
		if (!e.Cancel)
		{
			StoreSettings();
		}
	}

	private void AttachmentsWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			Close();
		}
		else if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			SaveAndCloseDocument();
		}
		else if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 13 && !_tableView.IsEditing)
		{
			e.Handled = true;
			OnGridEscapePressed(this, new EventArgs());
		}
	}

	private void TableView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		int rowHandleByMouseEventArgs = _tableView.GetRowHandleByMouseEventArgs(e);
		if (rowHandleByMouseEventArgs != int.MinValue && rowHandleByMouseEventArgs != -2147483645)
		{
			ViewRow();
		}
	}

	public AttachmentsWindow(Guid docISN, AttachType attachedType, bool isReadOnly)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		DocISN = docISN;
		_attachType = attachedType;
		_isReadOnly = isReadOnly;
		SizeToContent = SizeToContent.Manual;
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		_barManager = new BarManager
		{
			ShowShortcutInScreenTips = true,
			AllowCustomization = false,
			AllowHotCustomization = false,
			AllowQuickCustomization = false
		};
		_barButtonSave = new BarButtonItem
		{
			Content = "Save",
			KeyGesture = new KeyGesture((Key)62, (ModifierKeys)2),
			Glyph = ImageService.GetImage("save_16.svg"),
			LargeGlyph = ImageService.GetImage("save_32.svg")
		};
		_barManager.Items.Add(_barButtonSave);
		_barButtonSaveAndClose = new BarButtonItem
		{
			Content = "Save and Close",
			KeyGesture = new KeyGesture((Key)6, (ModifierKeys)2),
			Glyph = ImageService.GetImage("save_close_16.svg"),
			LargeGlyph = ImageService.GetImage("save_close_32.svg")
		};
		_barManager.Items.Add(_barButtonSaveAndClose);
		_barButtonAdd = new BarButtonItem
		{
			Content = "Add",
			KeyGesture = new KeyGesture((Key)57, (ModifierKeys)2),
			Glyph = ImageService.GetImage("add_16.svg"),
			LargeGlyph = ImageService.GetImage("add_32.svg")
		};
		_barManager.Items.Add(_barButtonAdd);
		_barButtonDelete = new BarButtonItem
		{
			Content = "Delete",
			KeyGesture = new KeyGesture((Key)47, (ModifierKeys)2),
			Glyph = ImageService.GetImage("delete_16.svg"),
			LargeGlyph = ImageService.GetImage("delete_32.svg")
		};
		_barManager.Items.Add(_barButtonDelete);
		_barButtonClose = new BarButtonItem
		{
			Content = "Close",
			Name = "BarButtonClose",
			KeyGesture = new KeyGesture((Key)13),
			Glyph = ImageService.GetImage("close_16.svg"),
			LargeGlyph = ImageService.GetImage("close_32.svg")
		};
		_barManager.Items.Add(_barButtonClose);
		_barButtonView = new BarButtonItem
		{
			Content = "View",
			KeyGesture = new KeyGesture((Key)94),
			Glyph = ImageService.GetImage("doc_preview_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_preview_32.svg")
		};
		_barManager.Items.Add(_barButtonView);
		_barButtonFileSaveAs = new BarButtonItem
		{
			Content = "FileSaveAs",
			Glyph = ImageService.GetImage("file_save_as_16.svg"),
			LargeGlyph = ImageService.GetImage("file_save_as_32.svg")
		};
		_barManager.Items.Add(_barButtonFileSaveAs);
		_barButtonFileSaveAsAll = new BarButtonItem
		{
			Content = "FileSaveAsAll",
			Glyph = ImageService.GetImage("file_save_as_all_16.svg"),
			LargeGlyph = ImageService.GetImage("file_save_as_all_32.svg")
		};
		_barManager.Items.Add(_barButtonFileSaveAsAll);
		_barButtonHelp = new BarButtonItem
		{
			Content = "Help",
			KeyGesture = new KeyGesture((Key)90),
			Glyph = ImageService.GetImage("help_16.svg"),
			LargeGlyph = ImageService.GetImage("help_32.svg")
		};
		_barManager.Items.Add(_barButtonHelp);
		DockPanel dockPanel = new DockPanel
		{
			UseLayoutRounding = true
		};
		_ribbon = new RibbonControl
		{
			RibbonStyle = RibbonStyle.Office2010,
			UseLayoutRounding = true,
			AllowCustomization = false,
			IsMinimized = true,
			IsTabStop = false,
			AllowSimplifiedRibbon = false
		};
		ApplicationMenu applicationMenu = new ApplicationMenu();
		_ribbon.ApplicationMenu = (DependencyObject)(object)applicationMenu;
		_ribbon.ApplicationButtonText = "";
		_ribbon.ApplicationButtonSmallIcon = ImageService.GetImage("hamburger_horizontal_16.svg");
		applicationMenu.RightPaneWidth = 200.0;
		applicationMenu.ItemLinks.Add(_barButtonClose.CreateLink());
		RibbonDefaultPageCategory ribbonDefaultPageCategory = new RibbonDefaultPageCategory();
		_ribbon.Categories.Add(ribbonDefaultPageCategory);
		_ribbonPageHome = new RibbonPage
		{
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
		};
		ribbonDefaultPageCategory.Pages.Add(_ribbonPageHome);
		_ribbonPageHome.Caption = "Home";
		_ribbonPageOperations = new RibbonPageGroup
		{
			Caption = "Operations"
		};
		_ribbonPageOperations.ItemLinks.Add(_barButtonSave.CreateLink());
		_ribbonPageOperations.ItemLinks.Add(_barButtonSaveAndClose.CreateLink());
		_ribbonPageOperations.ItemLinks.Add(_barButtonAdd.CreateLink());
		_ribbonPageOperations.ItemLinks.Add(_barButtonDelete.CreateLink());
		_ribbonPageOperations.ItemLinks.Add(_barButtonView.CreateLink());
		_ribbonPageOperations.ItemLinks.Add(_barButtonFileSaveAs.CreateLink());
		_ribbonPageOperations.ItemLinks.Add(_barButtonFileSaveAsAll.CreateLink());
		_ribbonPageHome.Groups.Add(_ribbonPageOperations);
		_ribbon.PageHeaderItemLinks.Add(_barButtonHelp.CreateLink());
		DockPanel.SetDock(_ribbon, Dock.Top);
		_ribbon.SizeChanged += new SafeEventHandler<SizeChangedEventArgs>(Ribbon_SizeChanged).Handler;
		dockPanel.Children.Add(_ribbon);
		_grid = new GridControl
		{
			ClipboardCopyMode = ClipboardCopyMode.ExcludeHeader
		};
		TextEditSettings textEditSetting = GetTextEditSetting(50);
		GridColumn gridColumn = GetGridColumn("Description", ArmSoft.SME.Common.Resources.Description, textEditSetting, UnboundColumnType.String);
		gridColumn.AllowEditing = DefaultBoolean.Default;
		gridColumn.ReadOnly = _isReadOnly;
		gridColumn.Width = SizeCalculator.WidthForString(textEditSetting.MaxLength);
		_grid.Columns.Add(gridColumn);
		if (_attachType == AttachType.File)
		{
			TextEditSettings textEditSetting2 = GetTextEditSetting(50);
			gridColumn = GetGridColumn("FileName", ArmSoft.SME.Common.Resources.File, textEditSetting2, UnboundColumnType.String);
			_grid.Columns.Add(gridColumn);
			CalcEditSettings calcEditSetting = GetCalcEditSetting(16, 1);
			gridColumn = GetGridColumn("Size", ArmSoft.SME.Common.Resources.SizeKB, calcEditSetting, UnboundColumnType.Integer);
			GridSummaryItem gridSummaryItem = new GridSummaryItem();
			TreeListSummarySettings.SetIsRecursive((DependencyObject)(object)gridSummaryItem, false);
			gridSummaryItem.FieldName = gridColumn.FieldName;
			gridSummaryItem.DisplayFormat = gridColumn.EditSettings.DisplayFormat;
			gridSummaryItem.SummaryType = SummaryItemType.Sum;
			gridSummaryItem.Visible = true;
			_grid.TotalSummary.Add(gridSummaryItem);
			_grid.Columns.Add(gridColumn);
		}
		else if (_attachType == AttachType.Link)
		{
			TextEditSettings textEditSetting3 = GetTextEditSetting(256);
			gridColumn = GetGridColumn("Link", ArmSoft.SME.Common.Resources.Link, textEditSetting3, UnboundColumnType.String);
			gridColumn.AllowEditing = DefaultBoolean.Default;
			gridColumn.ReadOnly = _isReadOnly;
			gridColumn.Width = SizeCalculator.WidthForString(textEditSetting3.MaxLength);
			_grid.Columns.Add(gridColumn);
		}
		TextEditSettings textEditSetting4 = GetTextEditSetting(50);
		gridColumn = GetGridColumn("UserName", ArmSoft.SME.Common.Resources.UserName, textEditSetting4, UnboundColumnType.String);
		_grid.Columns.Add(gridColumn);
		TextEditSettings textEditSetting5 = GetTextEditSetting(32);
		gridColumn = GetGridColumn("CompName", ArmSoft.SME.Common.Resources.Workstation, textEditSetting5, UnboundColumnType.String);
		_grid.Columns.Add(gridColumn);
		DateEditSettings dateEditSetting = GetDateEditSetting();
		gridColumn = GetGridColumn("LastModifyDate", ArmSoft.SME.Common.Resources.DateWord, dateEditSetting, UnboundColumnType.DateTime);
		_grid.Columns.Add(gridColumn);
		_tableView = new TableView
		{
			IsColumnChooserVisible = false,
			ShowGroupPanel = false,
			AllowGrouping = false,
			SearchPanelParseMode = SearchPanelParseMode.Mixed,
			ShowGroupedColumns = false,
			IsGroupPanelMenuEnabled = false,
			AutoWidth = true,
			ShowTotalSummary = (_attachType == AttachType.File),
			SearchPanelPosition = SearchPanelPosition.OverGroupPanel,
			VerticalScrollbarVisibility = ScrollBarVisibility.Auto
		};
		_tableView.HideColumnChooser();
		_tableView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(GridCellValueChanged).Handler;
		_tableView.PreviewMouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(TableView_MouseDoubleClick).Handler;
		_grid.View = _tableView;
		_grid.View.NavigationStyle = GridViewNavigationStyle.Cell;
		_grid.View.AllowColumnFiltering = false;
		_grid.View.AllowDrop = false;
		_grid.View.AllowSorting = false;
		_grid.View.AllowColumnMoving = false;
		_grid.View.AllowMoveColumnToDropArea = false;
		_grid.View.AllowScrollToFocusedRow = false;
		_grid.View.ShowFocusedRectangle = true;
		dockPanel.Children.Add(_grid);
		DockPanel.SetDock(_grid, Dock.Top);
		_barManager.Child = dockPanel;
		Content = _barManager;
		_isChangedNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsChangedProperty);
		_isChangedNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsChangedChanged).Handler;
		Activated += new SafeEventHandler<EventArgs>(AttachmentsWindow_Activated).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(AttachmentsWindow_Closing).Handler;
		PreviewKeyDown += new SafeEventHandler<System.Windows.Input.KeyEventArgs>(AttachmentsWindow_PreviewKeyDown).Handler;
	}

	private void AddToolbarItems()
	{
		_ribbon.ToolbarItemLinks.Add(_barButtonSave.ConvertForToolbar("t_save_16.svg"));
		_ribbon.ToolbarItemLinks.Add(_barButtonSaveAndClose.ConvertForToolbar("t_save_close_16.svg"));
		_ribbon.ToolbarItemLinks.Add(_barButtonAdd.ConvertForToolbar("t_add_16.svg"));
		_ribbon.ToolbarItemLinks.Add(_barButtonDelete.ConvertForToolbar("t_delete_16.svg"));
		_ribbon.ToolbarItemLinks.Add(_barButtonView.ConvertForToolbar("t_doc_preview_16.svg"));
		_ribbon.ToolbarItemLinks.Add(_barButtonFileSaveAs.ConvertForToolbar("t_file_save_as_16.svg"));
		_ribbon.ToolbarItemLinks.Add(_barButtonFileSaveAsAll.ConvertForToolbar("t_file_save_as_all_16.svg"));
	}

	private void LoadData()
	{
		_lastModifyDate = _attachmentsService.GetLastModifyDate(DocISN);
		if (_attachType == AttachType.File)
		{
			List<FileRow> files = _attachmentsService.GetFiles(DocISN);
			BindingList<FileRow> bindingList = new BindingList<FileRow>();
			foreach (FileRow item in files)
			{
				bindingList.Add(item);
			}
			_filesList = bindingList;
			_grid.ItemsSource = _filesList;
		}
		else
		{
			if (_attachType != AttachType.Link)
			{
				return;
			}
			List<LinkRow> links = _attachmentsService.GetLinks(DocISN);
			BindingList<LinkRow> bindingList2 = new BindingList<LinkRow>();
			foreach (LinkRow item2 in links)
			{
				bindingList2.Add(item2);
			}
			_linksList = bindingList2;
			_grid.ItemsSource = _linksList;
		}
	}

	private static GridColumn GetGridColumn(string name, string header, BaseEditSettings editSettings, UnboundColumnType columnType)
	{
		GridColumn gridColumn = new GridColumn
		{
			Name = name,
			Header = header,
			AllowEditing = DefaultBoolean.False,
			AllowGrouping = DefaultBoolean.False,
			ReadOnly = true,
			UnboundType = columnType,
			EditSettings = editSettings
		};
		gridColumn.FieldName = gridColumn.Name;
		return gridColumn;
	}

	private static TextEditSettings GetTextEditSetting(int length)
	{
		return new TextEditSettings
		{
			MaskType = MaskType.None,
			MaxLength = length,
			AllowNullInput = false,
			NullValue = ""
		};
	}

	private static CalcEditSettings GetCalcEditSetting(short length, short precision = 0, bool useCurrencyFormat = true)
	{
		return new CalcEditSettings
		{
			MaskType = MaskType.Numeric,
			Precision = precision,
			Mask = (useCurrencyFormat ? DecimalExtension.GetCurrencyFormat(length, precision) : DecimalExtension.GetNumberFormat(length, precision)),
			MaskUseAsDisplayFormat = true,
			AllowNullInput = false,
			AllowSpinOnMouseWheel = false,
			ValidateOnTextInput = false
		};
	}

	private static DateEditSettings GetDateEditSetting(bool useLongDate = false)
	{
		return new DateEditSettings
		{
			MaskType = MaskType.DateTime,
			Mask = (useLongDate ? "dd/MM/yyyy" : "dd/MM/yy") + " H:mm:ss",
			MaskUseAsDisplayFormat = true,
			AllowSpinOnMouseWheel = false,
			ValidateOnTextInput = false
		};
	}

	private static void GridRowsChanged(object sender, RoutedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected Obj, but got Unknown
		if (Window.GetWindow((DependencyObject)sender) is AttachmentsWindow { _firstActivation: false } attachmentsWindow)
		{
			attachmentsWindow.IsChanged = true;
		}
	}

	protected internal void GridCellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (!_firstActivation)
		{
			BaseRow baseRow = (BaseRow)_grid.GetRow(e.RowHandle);
			if (_attachType == AttachType.Link && baseRow != null && e.Column.Name == "Link" && (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()) || e.OldValue == null || e.OldValue.ToString().Trim() != e.Value.ToString().Trim()))
			{
				baseRow.IsNew = true;
			}
			if (baseRow != null && e.Column.Name == "Description" && e.Value == null)
			{
				e.Handled = true;
				_grid.SetCellValue(e.RowHandle, e.Column, " ");
			}
			IsChanged = true;
		}
	}
}
