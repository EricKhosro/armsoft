using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Parameter;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.Controls;

public abstract class OptionsWindowBase : ThemedWindow
{
	public class ControlDesc
	{
		public short ParamId { get; set; }

		public bool UserParam { get; set; }

		public bool ParamUseFromCache { get; set; }

		public System.Windows.Controls.Control Control { get; set; }

		public UsersParamButton UsersParamButton { get; set; }

		public bool ShowOnlyUserValue { get; set; }

		public string ValueFromService { get; set; }

		public ControlDesc(short paramId, System.Windows.Controls.Control control, UsersParamButton usersParamButton, bool userParam, bool paramUseFromCache, string valueFromService, bool showOnlyUserValue = false)
		{
			ParamId = paramId;
			Control = control;
			UsersParamButton = usersParamButton;
			UserParam = userParam;
			ParamUseFromCache = paramUseFromCache;
			ShowOnlyUserValue = showOnlyUserValue;
			ValueFromService = valueFromService;
		}
	}

	public class GridControlDesc
	{
		public bool ParamUseFromCache { get; set; }

		public DocumentGridControl Control { get; set; }

		public GridControlDesc(DocumentGridControl control, bool paramUseFromCache)
		{
			Control = control;
			ParamUseFromCache = paramUseFromCache;
		}
	}

	public class GridParamValueRow : DocumentGridRow
	{
		public short ParamId { get; set; }

		public string ParamCaption { get; set; }

		public string OldValue { get; set; }

		public string NewValue { get; set; }

		public string Group { get; set; }

		public GridParamValueRow(short paramId, string paramCaption, string oldValue, string paramGroup = "")
		{
			ParamId = paramId;
			ParamCaption = paramCaption;
			OldValue = oldValue;
			NewValue = oldValue;
			Group = paramGroup;
		}

		public GridParamValueRow(ParameterDefinition<string> parameterDefinition, string paramGroup = "")
		{
			ParamId = parameterDefinition.Id;
			ParamCaption = parameterDefinition.Caption;
			OldValue = ParametersFromService[parameterDefinition.Id];
			NewValue = OldValue;
			Group = paramGroup;
		}

		public GridParamValueRow(ParameterDefinition<bool> parameterDefinition, string paramGroup = "")
		{
			ParamId = parameterDefinition.Id;
			ParamCaption = parameterDefinition.Caption;
			OldValue = ParametersFromService[parameterDefinition.Id];
			NewValue = OldValue;
			Group = paramGroup;
		}
	}

	public class DocPrintTemplateGridRow : GridParamValueRow
	{
		static DocPrintTemplateGridRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(DocPrintTemplateGridRow), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>> { 
			{
				"NewValue",
				(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
				{
					ValidatePrintTemplate((DocPrintTemplateGridRow)row, info);
				}
			} });
		}

		public DocPrintTemplateGridRow(short paramId, string paramCaption, string oldValue, string paramGroup = "")
			: base(paramId, paramCaption, oldValue, paramGroup)
		{
		}

		private static void ValidatePrintTemplate(DocPrintTemplateGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.NewValue))
			{
				IReadOnlyList<TemplatesCacheRow> templates = Settings.CoreCacheService.Template.GetTemplates(GetPrintTemplateDocType(row.ParamId));
				if (templates.Count == 0 || templates.FirstOrDefault((TemplatesCacheRow p) => p.Key == row.NewValue.Trim()) == null)
				{
					info.ErrorType = ErrorType.Critical;
					info.ErrorText = string.Format(ArmSoft.SME.Common.Resources.WrongPrintTemplate, row.ParamCaption, row.NewValue);
				}
			}
		}
	}

	protected class ComboBoxWithImageItem
	{
		public ImageSource ItemImage { get; set; }

		public string Caption { get; set; }

		public string Tag { get; set; }
	}

	protected DockLayoutManager _dockLayoutManager1;

	protected System.Windows.Controls.TreeView _mnuTree;

	protected System.Windows.Controls.Button _okButton;

	protected System.Windows.Controls.Button _cancelButton;

	protected LayoutPanel _panelOptions;

	protected DevExpress.Xpf.LayoutControl.LayoutGroup _optionsPanels;

	protected bool _firstActivation;

	protected List<ParameterStoreRequestModel> _parameterStoreRequest = new List<ParameterStoreRequestModel>();

	protected HashSet<string> _loadedPanels = new HashSet<string>();

	protected Dictionary<string, List<ControlDesc>> _controlsByPanels = new Dictionary<string, List<ControlDesc>>();

	protected Dictionary<string, string> _treeViewItemsHelpIds = new Dictionary<string, string>();

	protected List<GridControlDesc> _loadedGridControls = new List<GridControlDesc>();

	public bool ChangedParamWhichAffectAfterNextRun;

	protected const string _controlNamePrefix = "Param_";

	private string _helpId;

	protected readonly bool _tradeShop = Settings.LoginService.ContractType == 1;

	protected CheckEditExt _participateCEIP;

	protected LayoutControl _panelPrintTemplates;

	protected LayoutControl _panelDocNumbers;

	protected LayoutControl _panelSystemConfigs;

	protected LayoutControl _panelB2B;

	protected LayoutControl _panelGeneral;

	protected LayoutControl _panelCompany;

	protected AsFontsPanel _panelThemes;

	protected AsPageSetup _panelPageSetup;

	protected CheckEditExt _localBackUp;

	protected DropDownCommentControl _localBackUpPath;

	protected CheckEditExt _dailyBackupEnabled;

	protected SpinEditExt _dailyBackupCount;

	protected CheckEditExt _monthlyBackupEnabled;

	protected SpinEditExt _monthlyBackupDay;

	protected SpinEditExt _monthlyBackupCount;

	protected CheckEditExt _annualBackupEnabled;

	protected SpinEditExt _annualBackupDay;

	protected SpinEditExt _annualBackupMonth;

	protected SpinEditExt _annualBackupCount;

	protected CheckEditExt _createOnExistedLogin;

	protected System.Windows.Controls.Button _restoreAutoBackupDefaultValues;

	protected TreeViewItem _menuItemGeneral;

	protected TreeViewItem _menuItemComp;

	protected TreeViewItem _menuItemPrintOptions;

	protected TreeViewItem _menuItemPrintTemplates;

	protected TreeViewItem _menuItemDocNum;

	protected TreeViewItem _menuItemB2B;

	protected TreeViewItem _menuItemThemesAndFonts;

	protected TreeViewItem _menuItemSystemConfigs;

	private List<DocPrintTemplateGridRow> _docTemplates;

	public bool FontOrThemeChanged { get; set; }

	public static Dictionary<short, string> ParametersFromService { get; set; }

	protected virtual bool GroupTemplatesByTypes => true;

	protected virtual bool GroupDocNumsByTypes => true;

	public OptionsWindowBase()
	{
		Name = "OptionsWindow";
		Title = ArmSoft.SME.Common.Resources.Options;
		Owner = Settings.HostingEnvironment;
		_firstActivation = true;
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		Width = 1100.0;
		Height = 760.0;
		MaxHeight = Screen.PrimaryScreen.WorkingArea.Height;
		ShowIcon = false;
		ShowInTaskbar = false;
		WindowStartupLocation = WindowStartupLocation.CenterScreen;
		UseLayoutRounding = true;
		TitleAlignment = WindowTitleAlignment.Center;
		ControlBoxButtonSet = ControlBoxButtons.Close | ControlBoxButtons.MaximizeRestore;
		RenderOptions.SetBitmapScalingMode((DependencyObject)(object)this, BitmapScalingMode.HighQuality);
		Grid grid = (Grid)(Content = new Grid());
		BooleanToVisibilityConverter value = new BooleanToVisibilityConverter();
		Resources.Add("VisibilityOfBool", value);
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = GridLength.Auto
		});
		_dockLayoutManager1 = new DockLayoutManager
		{
			Name = "DockLayoutManager1",
			Margin = new Thickness(0.0, 0.0, 0.0, 0.0)
		};
		Grid.SetRow(_dockLayoutManager1, 0);
		grid.Children.Add(_dockLayoutManager1);
		DevExpress.Xpf.Docking.LayoutGroup layoutGroup = new DevExpress.Xpf.Docking.LayoutGroup
		{
			Name = "LayoutRoot"
		};
		_dockLayoutManager1.LayoutRoot = layoutGroup;
		LayoutPanel layoutPanel = new LayoutPanel
		{
			Name = "MenuCaption",
			AllowClose = false,
			AllowDrag = false,
			AllowFloat = false,
			AllowHide = false,
			FloatOnDoubleClick = false,
			ShowCaption = false,
			ItemWidth = GridLength.Auto
		};
		_mnuTree = new System.Windows.Controls.TreeView
		{
			Name = "MnuTree",
			BorderThickness = new Thickness(0.0),
			Background = Settings.GetBackgroundBrush(),
			Padding = new Thickness(0.0, 2.0, 0.0, 0.0)
		};
		layoutPanel.Content = _mnuTree;
		_panelOptions = new LayoutPanel
		{
			Name = "PanelOptions",
			AllowClose = false,
			AllowDrag = false,
			AllowFloat = false,
			AllowHide = false,
			FloatOnDoubleClick = false,
			ItemWidth = new GridLength(1.0, GridUnitType.Star)
		};
		_optionsPanels = new DevExpress.Xpf.LayoutControl.LayoutGroup
		{
			Name = "OptionsPanels"
		};
		_panelOptions.Content = _optionsPanels;
		layoutGroup.Items.Add(layoutPanel);
		layoutGroup.Items.Add(_panelOptions);
		Grid grid3 = new Grid();
		Grid.SetRow(grid3, 1);
		Grid.SetIsSharedSizeScope(grid3, value: true);
		grid.Children.Add(grid3);
		grid3.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid3.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = GridLength.Auto,
			SharedSizeGroup = "Buttons"
		});
		grid3.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = GridLength.Auto,
			SharedSizeGroup = "Buttons"
		});
		grid3.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		_okButton = new System.Windows.Controls.Button
		{
			Name = "OkButton",
			Content = ArmSoft.SME.Common.Resources.Ok,
			Margin = new Thickness(15.0, 5.0, 15.0, 5.0),
			UseLayoutRounding = true
		};
		_okButton.Click += OkButton_Click;
		Grid.SetColumn(_okButton, 1);
		grid3.Children.Add(_okButton);
		_cancelButton = new System.Windows.Controls.Button
		{
			Name = "CancelButton",
			Content = ArmSoft.SME.Common.Resources.Cancel,
			Margin = new Thickness(15.0, 5.0, 15.0, 5.0),
			IsCancel = true,
			UseLayoutRounding = true
		};
		_cancelButton.Click += CancelButton_Click;
		Grid.SetColumn(_cancelButton, 2);
		grid3.Children.Add(_cancelButton);
		Activated += new SafeEventHandler<EventArgs>(OptionsWindow_Activated).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(OptionsWindow_Closing).Handler;
		KeyDown += new SafeEventHandler<System.Windows.Input.KeyEventArgs>(OptionsWindow_KeyDown).Handler;
	}

	private void OptionsWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivation)
		{
			Mouse.OverrideCursor = null;
			_firstActivation = false;
		}
	}

	private void OptionsWindow_Closing(object sender, CancelEventArgs e)
	{
		if (DialogResult.HasValue && !Convert.ToBoolean(DialogResult))
		{
			e.Cancel = true;
		}
	}

	private void OptionsWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected Obj, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected Obj, but got Unknown
		if (((int)e.Key == 93 && (int)Keyboard.Modifiers == 2) || ((int)Keyboard.Modifiers == 0 && (int)e.Key == 13))
		{
			e.Handled = true;
			Close();
		}
		else if ((int)e.Key == 6)
		{
			if ((int)Keyboard.Modifiers == 2)
			{
				if (Save())
				{
					e.Handled = true;
				}
			}
			else
			{
				if ((int)Keyboard.Modifiers != 0)
				{
					return;
				}
				LayoutControl layoutControl = null;
				foreach (object child in _optionsPanels.Children)
				{
					if (child is LayoutControl && ((LayoutControl)child).Visibility == Visibility.Visible)
					{
						layoutControl = (LayoutControl)child;
						break;
					}
				}
				if (e.OriginalSource is TreeViewItem)
				{
					if (layoutControl != null)
					{
						layoutControl.MoveFocus(new TraversalRequest((FocusNavigationDirection)2));
						e.Handled = true;
					}
					else
					{
						_panelPageSetup.MoveFocus(new TraversalRequest((FocusNavigationDirection)2));
					}
				}
			}
		}
		else if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 90)
		{
			Settings.HostingEnvironment.ShowHelp(_helpId);
		}
	}

	protected bool Save()
	{
		try
		{
			Validate();
			StoreOptions();
			DialogResult = true;
			return true;
		}
		catch (Exception ex)
		{
			DialogResult = false;
			Settings.ErrorHandler(ex);
		}
		return false;
	}

	protected virtual void Validate()
	{
		string gridErrorMgs;
		foreach (List<ControlDesc> value in _controlsByPanels.Values)
		{
			foreach (ControlDesc item in value)
			{
				ControlsHelper.DoValidateAndSetFocus(item.Control, out gridErrorMgs, validateWithoutRequired: false, showValidationError: true);
			}
		}
		foreach (GridControlDesc loadedGridControl in _loadedGridControls)
		{
			ControlsHelper.DoValidateAndSetFocus(loadedGridControl.Control, out gridErrorMgs, validateWithoutRequired: false, showValidationError: true);
		}
	}

	protected void StoreOptions()
	{
		foreach (string loadedPanel in _loadedPanels)
		{
			StoreOptionsControlsInner(loadedPanel);
		}
		foreach (GridControlDesc loadedGridControl in _loadedGridControls)
		{
			StoreOptionsGridInner(loadedGridControl);
		}
		if (!Settings.IsOffline)
		{
			Settings.ParametersService.SetValues(_parameterStoreRequest);
		}
	}

	protected virtual void StoreOptionsControlsInner(string panelName)
	{
		if (_controlsByPanels.TryGetValue(panelName, out var value))
		{
			foreach (ControlDesc item in value)
			{
				string controlValue = GetControlValue(item.Control);
				if (controlValue != item.ValueFromService)
				{
					if (item.ParamUseFromCache)
					{
						ChangedParamWhichAffectAfterNextRun = true;
					}
					_parameterStoreRequest.Add(new ParameterStoreRequestModel
					{
						Id = item.ParamId,
						Value = controlValue,
						IsDefault = false,
						UserId = ((!item.UserParam || (Settings.LoginService.IsAdmin && !item.ShowOnlyUserValue)) ? (-1) : Settings.LoginService.UserId)
					});
				}
				if (!Settings.LoginService.IsAdmin || item.UsersParamButton == null)
				{
					continue;
				}
				foreach (UsersParamButton.UsersParamRow changedParam in item.UsersParamButton.ChangedParams)
				{
					ChangedParamWhichAffectAfterNextRun = true;
					_parameterStoreRequest.Add(new ParameterStoreRequestModel
					{
						Id = item.ParamId,
						Value = changedParam.ParamValue,
						IsDefault = changedParam.IsDefault,
						UserId = changedParam.UserId
					});
				}
			}
		}
		if (panelName == _panelThemes.Name)
		{
			StoreThemesAndFonts();
		}
		if (panelName == _panelPageSetup.Name)
		{
			StorePageSetup();
		}
		if (panelName == _panelSystemConfigs.Name)
		{
			Set_CEIP_status(_participateCEIP.IsChecked.Value ? 1 : 0);
			if (Settings.LoginService.IsAdmin && !ConfigHelper.ASAdminLogin)
			{
				SetAutoBackupProperties();
			}
		}
	}

	protected virtual void StoreOptionsGridInner(GridControlDesc gridControlDesc)
	{
		List<GridParamValueRow> list = ((!(gridControlDesc.Control.Name == "DocumentsPrintTemplates")) ? ((List<GridParamValueRow>)gridControlDesc.Control.ItemsSource) : ((List<DocPrintTemplateGridRow>)gridControlDesc.Control.ItemsSource).Cast<GridParamValueRow>().ToList());
		foreach (GridParamValueRow item in list)
		{
			if (item.NewValue.Trim() != item.OldValue.Trim())
			{
				if (gridControlDesc.ParamUseFromCache)
				{
					ChangedParamWhichAffectAfterNextRun = true;
				}
				_parameterStoreRequest.Add(new ParameterStoreRequestModel
				{
					Id = item.ParamId,
					Value = item.NewValue.Trim(),
					IsDefault = false,
					UserId = (Settings.LoginService.IsAdmin ? (-1) : Settings.LoginService.UserId)
				});
			}
		}
	}

	protected void BindVisibility(FrameworkElement element, TreeViewItem treeViewItem)
	{
		System.Windows.Data.Binding binding = new System.Windows.Data.Binding("IsSelected")
		{
			Source = treeViewItem,
			Converter = (IValueConverter)FindResource("VisibilityOfBool")
		};
		element.SetBinding(UIElement.VisibilityProperty, binding);
	}

	public void SelectGeneralTab()
	{
		_menuItemGeneral.IsSelected = true;
	}

	public bool? Invoke()
	{
		return ShowDialog();
	}

	protected void OkButton_Click(object sender, RoutedEventArgs e)
	{
		if (Save())
		{
			e.Handled = true;
		}
	}

	protected void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		e.Handled = true;
		Close();
	}

	protected virtual void FillPanelCompany()
	{
	}

	protected virtual void FillPanelGeneral()
	{
	}

	protected virtual void FillPanelSystemConfigs()
	{
	}

	protected void FillPrintTemplates()
	{
		if (_loadedPanels.Contains(_panelPrintTemplates.Name))
		{
			return;
		}
		Dictionary<string, List<string>> dictionary = InitDocTemplatesList();
		_docTemplates = new List<DocPrintTemplateGridRow>();
		foreach (KeyValuePair<string, List<string>> item in dictionary)
		{
			foreach (string item2 in item.Value)
			{
				short printTemplateParamId = GetPrintTemplateParamId(Convert.ToInt16(item2));
				ParametersFromService.TryGetValue(printTemplateParamId, out var value);
				_docTemplates.Add(new DocPrintTemplateGridRow(printTemplateParamId, SmartEnumHelper.GetSmartEnumRowCaption(typeof(PrintTemplateDocumentTypeSE), item2), value ?? "", item.Key));
			}
		}
		DocumentGridControl documentGridControl = AddGrid(_panelPrintTemplates, _panelPrintTemplates, "", System.Windows.HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isRequired: false, "DocumentsPrintTemplates", _menuItemPrintTemplates.Header.ToString(), paramUseFromCache: true);
		DataControlBase.AllowInfiniteGridSize = true;
		documentGridControl.ItemsSource = _docTemplates;
		DocumentGridColumn documentGridColumn = documentGridControl.AddStringColumn("Group", ArmSoft.SME.Common.Resources.Group, 255);
		documentGridColumn.Visible = false;
		documentGridColumn.ShowInColumnChooser = false;
		if (GroupTemplatesByTypes)
		{
			documentGridControl.Grid.GroupBy(documentGridColumn);
			documentGridControl.Grid.CollapseAllGroups();
			documentGridControl.Grid.ExpandGroupRow(-1);
		}
		DocumentGridColumn documentGridColumn2 = documentGridControl.AddStringColumn("ParamCaption", ArmSoft.SME.Common.Resources.DocumentType, 50);
		documentGridColumn2.Width = SizeCalculator.WidthForString(25);
		documentGridColumn2.AllowMoving = DefaultBoolean.False;
		documentGridColumn2.AllowEditing = DefaultBoolean.True;
		documentGridColumn2.ReadOnly = true;
		DocumentGridColumn documentGridColumn3 = documentGridControl.AddDropDownColumn("NewValue", ArmSoft.SME.Common.Resources.PrintTemplate, 20, trimValue: false);
		documentGridColumn3.Width = SizeCalculator.WidthForString(15);
		documentGridColumn3.AllowMoving = DefaultBoolean.False;
		documentGridColumn3.AllowEditing = DefaultBoolean.True;
		documentGridControl.ClickDropDown += DocumentsPrintTemplates_ClickDropDown;
		_loadedPanels.Add(_panelPrintTemplates.Name);
	}

	protected virtual Dictionary<string, List<string>> InitDocTemplatesList()
	{
		throw new NotImplementedException();
	}

	private void DocumentsPrintTemplates_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		if (!(sender is DocumentGridControl documentGridControl))
		{
			return;
		}
		GridParamValueRow gridParamValueRow = (GridParamValueRow)documentGridControl.Grid.GetRow(e.RowHandle);
		if (gridParamValueRow == null)
		{
			return;
		}
		Template template = new Template(GetPrintTemplateDocType(gridParamValueRow.ParamId));
		if (template != null)
		{
			ModalBrowser modalBrowser = new ModalBrowser(template)
			{
				LookUpValue = e.Value,
				Owner = this,
				Left = e.Left,
				Top = e.Top,
				Caption = gridParamValueRow.ParamCaption,
				SearchString = documentGridControl.Grid.GetFocusedValue().ToString()
			};
			if (modalBrowser.Show(e.UpClick))
			{
				e.NewValue = template.ValueMember(modalBrowser.SelectedRowIndex);
				documentGridControl.View.Grid.SetCellValue(e.RowHandle, "NewValue", e.NewValue);
				documentGridControl.Grid.RefreshRow(e.RowHandle);
			}
		}
	}

	private void FillDocNumPanel()
	{
		if (!_loadedPanels.Contains(_panelDocNumbers.Name))
		{
			List<GridParamValueRow> itemsSource = InitDocNumsList();
			DocumentGridControl documentGridControl = AddGrid(_panelDocNumbers, _panelDocNumbers, "", System.Windows.HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isRequired: false, "DocNumbers", _menuItemDocNum.Header.ToString());
			DataControlBase.AllowInfiniteGridSize = true;
			documentGridControl.ItemsSource = itemsSource;
			DocumentGridColumn documentGridColumn = documentGridControl.AddStringColumn("Group", ArmSoft.SME.Common.Resources.Group, 255);
			documentGridColumn.Visible = false;
			documentGridColumn.ShowInColumnChooser = false;
			if (GroupDocNumsByTypes)
			{
				documentGridControl.Grid.GroupBy(documentGridColumn);
				documentGridControl.Grid.CollapseAllGroups();
				documentGridControl.Grid.ExpandGroupRow(-1);
			}
			DocumentGridColumn documentGridColumn2 = documentGridControl.AddStringColumn("ParamCaption", ArmSoft.SME.Common.Resources.DocumentType, 255);
			documentGridColumn2.AllowMoving = DefaultBoolean.False;
			documentGridColumn2.AllowEditing = DefaultBoolean.True;
			documentGridColumn2.ReadOnly = true;
			DocumentGridColumn documentGridColumn3 = documentGridControl.AddStringColumn("NewValue", ArmSoft.SME.Common.Resources.Number, 12);
			documentGridColumn3.AllowMoving = DefaultBoolean.False;
			documentGridColumn3.AllowEditing = DefaultBoolean.True;
			documentGridColumn3.ReadOnly = !Settings.LoginService.IsAdmin && !_tradeShop;
			_loadedPanels.Add(_panelDocNumbers.Name);
		}
	}

	protected virtual List<GridParamValueRow> InitDocNumsList()
	{
		throw new NotImplementedException();
	}

	protected void FillAutoBackupProperties(DevExpress.Xpf.LayoutControl.LayoutGroup backupsLayoutGroup)
	{
		backupsLayoutGroup = AddLayoutGroup(backupsLayoutGroup, System.Windows.Controls.Orientation.Horizontal, ArmSoft.SME.Common.Resources.AutoBackupOptions, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
		LayoutGroupExt container = AddLayoutGroup(backupsLayoutGroup, System.Windows.Controls.Orientation.Vertical, "", LayoutGroupView.Group);
		LayoutGroupExt container2 = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
		AddTextBlock(container2, ArmSoft.SME.Common.Resources.Periodicity, new Thickness(10.0, 0.0, 50.0, 0.0));
		((System.Windows.Controls.Control)(object)AddTextBlock(container2, ArmSoft.SME.Common.Resources.CountOfLastCopies, new Thickness(50.0, 0.0, 0.0, 0.0)).Parent).HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
		container = AddLayoutGroup(container, System.Windows.Controls.Orientation.Vertical, "", LayoutGroupView.Group);
		LayoutGroupExt container3 = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
		_dailyBackupEnabled = AddBooleanControl(container3, ArmSoft.SME.Common.Resources.Daily);
		_dailyBackupCount = AddSpinEditControl(container3, "", 1m, 45m);
		((System.Windows.Controls.Control)(object)_dailyBackupCount.Parent).HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
		LayoutGroupExt container4 = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
		_monthlyBackupEnabled = AddBooleanControl(container4, ArmSoft.SME.Common.Resources.PeriodicityMonthly);
		_monthlyBackupDay = AddSpinEditControl(container4, ArmSoft.SME.Common.Resources.Day, 1m, 31m);
		_monthlyBackupCount = AddSpinEditControl(container4, "", 1m, 15m);
		((System.Windows.Controls.Control)(object)_monthlyBackupCount.Parent).HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
		LayoutGroupExt container5 = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
		_annualBackupEnabled = AddBooleanControl(container5, ArmSoft.SME.Common.Resources.Annual);
		_annualBackupDay = AddSpinEditControl(container5, ArmSoft.SME.Common.Resources.Day, 1m, 31m);
		_annualBackupMonth = AddSpinEditControl(container5, ArmSoft.SME.Common.Resources.Month.ToLower(), 1m, 12m);
		_annualBackupCount = AddSpinEditControl(container5, "", 1m, 5m);
		((System.Windows.Controls.Control)(object)_annualBackupCount.Parent).HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
		container = AddLayoutGroup(container, System.Windows.Controls.Orientation.Vertical, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Left);
		container.Margin = new Thickness(10.0, 25.0, 0.0, 0.0);
		_createOnExistedLogin = AddBooleanControl(container, ArmSoft.SME.Common.Resources.BackupOnExistedLogin, System.Windows.HorizontalAlignment.Left, isThreeState: false, "", showCaptionAtRight: true);
		_restoreAutoBackupDefaultValues = AddButton(container, "", ArmSoft.SME.Common.Resources.RestoreDefaultValues);
		_restoreAutoBackupDefaultValues.Click += new SafeEventHandler<RoutedEventArgs>(RestoreDefaultValues_Click).Handler;
		LoadAutoBackupProperties();
		SetAutoBackupEvents();
		SetReadonlyAutoBackupProperties();
	}

	private void LoadAutoBackupProperties()
	{
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		CloudDatabaseInfo cloudDatabase = cloudClient.AS7X.GetCloudDatabase(Settings.LoginService.DatabaseId);
		_dailyBackupEnabled.IsChecked = cloudDatabase.DailyBackupEnabled;
		_dailyBackupCount.Value = cloudDatabase.DailyBackupCount;
		_monthlyBackupEnabled.IsChecked = cloudDatabase.MonthlyBackupEnabled;
		_monthlyBackupDay.Value = cloudDatabase.MonthlyBackupDay;
		_monthlyBackupCount.Value = cloudDatabase.MonthlyBackupCount;
		_annualBackupEnabled.IsChecked = cloudDatabase.AnnualBackupEnabled;
		_annualBackupDay.Value = cloudDatabase.AnnualBackupDay;
		_annualBackupMonth.Value = cloudDatabase.AnnualBackupMonth;
		_annualBackupCount.Value = cloudDatabase.AnnualBackupCount;
		_createOnExistedLogin.IsChecked = cloudDatabase.CreateOnExistedLogin;
	}

	private void SetAutoBackupProperties()
	{
		CloudDatabaseBackupUpdateRequest request = new CloudDatabaseBackupUpdateRequest
		{
			DailyBackupEnabled = _dailyBackupEnabled.IsChecked.Value,
			DailyBackupCount = (short)_dailyBackupCount.Value,
			MonthlyBackupEnabled = _monthlyBackupEnabled.IsChecked.Value,
			MonthlyBackupDay = (short)_monthlyBackupDay.Value,
			MonthlyBackupCount = (short)_monthlyBackupCount.Value,
			AnnualBackupEnabled = _annualBackupEnabled.IsChecked.Value,
			AnnualBackupDay = (short)_annualBackupDay.Value,
			AnnualBackupMonth = (short)_annualBackupMonth.Value,
			AnnualBackupCount = (short)_annualBackupCount.Value,
			CreateOnExistedLogin = _createOnExistedLogin.IsChecked.Value
		};
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		cloudClient.AS7X.UpdateCloudDatabaseBackupSettings(Settings.LoginService.DatabaseId, request);
	}

	protected void RestoreDefaultValues_Click(object sender, RoutedEventArgs e)
	{
		_dailyBackupEnabled.IsChecked = true;
		_dailyBackupCount.EditValue = 35;
		_monthlyBackupEnabled.IsChecked = true;
		_monthlyBackupDay.EditValue = 1;
		_monthlyBackupCount.EditValue = 11;
		_annualBackupEnabled.IsChecked = true;
		_annualBackupDay.EditValue = 15;
		_annualBackupMonth.EditValue = 1;
		_annualBackupCount.EditValue = 5;
		_createOnExistedLogin.IsChecked = true;
	}

	private void SetAutoBackupEvents()
	{
		_dailyBackupEnabled.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DailyBackupEnabled_EditValueChanged).Handler;
		_monthlyBackupEnabled.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MonthlyBackupEnabled_EditValueChanged).Handler;
		_annualBackupEnabled.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AnnualBackupEnabled_EditValueChanged).Handler;
	}

	private void SetReadonlyAutoBackupProperties()
	{
		bool flag = Settings.LoginService.IsAdmin && !ConfigHelper.ASAdminLogin;
		_dailyBackupEnabled.IsEnabled = flag;
		_dailyBackupCount.IsEnabled = flag && _dailyBackupEnabled.IsChecked.Value;
		_monthlyBackupEnabled.IsEnabled = flag;
		_monthlyBackupDay.IsEnabled = flag && _monthlyBackupEnabled.IsChecked.Value;
		_monthlyBackupCount.IsEnabled = flag && _monthlyBackupEnabled.IsChecked.Value;
		_annualBackupEnabled.IsEnabled = flag;
		_annualBackupDay.IsEnabled = flag && _annualBackupEnabled.IsChecked.Value;
		_annualBackupMonth.IsEnabled = flag && _annualBackupEnabled.IsChecked.Value;
		_annualBackupCount.IsEnabled = flag && _annualBackupEnabled.IsChecked.Value;
		_createOnExistedLogin.IsEnabled = flag;
		_restoreAutoBackupDefaultValues.IsEnabled = flag;
	}

	private void DailyBackupEnabled_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_dailyBackupCount.IsEnabled = _dailyBackupEnabled.IsChecked.Value;
	}

	private void MonthlyBackupEnabled_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_monthlyBackupDay.IsEnabled = _monthlyBackupEnabled.IsChecked.Value;
		_monthlyBackupCount.IsEnabled = _monthlyBackupEnabled.IsChecked.Value;
	}

	private void AnnualBackupEnabled_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_annualBackupDay.IsEnabled = _annualBackupEnabled.IsChecked.Value;
		_annualBackupMonth.IsEnabled = _annualBackupEnabled.IsChecked.Value;
		_annualBackupCount.IsEnabled = _annualBackupEnabled.IsChecked.Value;
	}

	protected void MnuTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		TreeViewItem treeViewItem = (TreeViewItem)_mnuTree.SelectedItem;
		_panelOptions.Caption = treeViewItem.Header.ToString();
		_helpId = _treeViewItemsHelpIds[treeViewItem.Name];
	}

	protected void PanelPageSetup_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (!_loadedPanels.Contains(_panelPageSetup.Name) && Convert.ToBoolean(e.NewValue))
		{
			_panelPageSetup.FillSettings();
			_loadedPanels.Add(_panelPageSetup.Name);
		}
	}

	protected void PanelGeneral_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (Convert.ToBoolean(e.NewValue))
		{
			FillPanelGeneral();
		}
	}

	protected void PanelThemes_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (!_loadedPanels.Contains(_panelThemes.Name) && Convert.ToBoolean(e.NewValue))
		{
			_panelThemes.FillSettings();
			_loadedPanels.Add(_panelThemes.Name);
		}
	}

	protected void PanelCompany_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (Convert.ToBoolean(e.NewValue))
		{
			FillPanelCompany();
		}
	}

	protected void PanelPrintTemplates_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (Convert.ToBoolean(e.NewValue))
		{
			FillPrintTemplates();
		}
	}

	protected void PanelDocNumbers_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (Convert.ToBoolean(e.NewValue))
		{
			FillDocNumPanel();
		}
	}

	protected void PanelSystemConfigs_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (Convert.ToBoolean(e.NewValue))
		{
			FillPanelSystemConfigs();
		}
	}

	protected static short GetPrintTemplateParamId(short docType)
	{
		return Convert.ToInt16(docType + 400);
	}

	public static short GetPrintTemplateDocType(short paramId)
	{
		return Convert.ToInt16(paramId - 400);
	}

	private static int Get_CEIP_status()
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, writable: false);
		int result;
		if (registryKey == null)
		{
			result = 1;
		}
		else
		{
			result = (int)registryKey.GetValue("CEIP", 1);
			registryKey.Close();
		}
		return result;
	}

	public void Set_CEIP_status(int value)
	{
		_participateCEIP.IsChecked = Convert.ToBoolean(value);
		RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		if (registryKey != null)
		{
			registryKey.SetValue("CEIP", value);
			registryKey.Close();
		}
	}

	public static bool IsParamParticipateCEIP()
	{
		return Convert.ToBoolean(Get_CEIP_status());
	}

	private void StoreThemesAndFonts()
	{
		FontOrThemeChanged = _panelThemes.SaveSettings();
	}

	private void StorePageSetup()
	{
		_panelPageSetup.SaveOptions();
	}

	protected TreeViewItem AddMenuTreeViewItem(string name, string header, string helpId)
	{
		TreeViewItem treeViewItem = new TreeViewItem
		{
			Header = header,
			Name = name,
			Margin = new Thickness(-15.0, 0.0, 0.0, 0.0)
		};
		_mnuTree.Items.Add(treeViewItem);
		_treeViewItemsHelpIds.Add(name, helpId);
		return treeViewItem;
	}

	protected LayoutControl AddMenuLayoutControl(TreeViewItem menuTreeViewItem, string name)
	{
		LayoutControl layoutControl = new LayoutControl
		{
			Name = name,
			Orientation = System.Windows.Controls.Orientation.Vertical,
			VerticalAlignment = VerticalAlignment.Stretch
		};
		BindVisibility(layoutControl, menuTreeViewItem);
		_optionsPanels.Children.Add(layoutControl);
		return layoutControl;
	}

	protected static string GetControlValue(System.Windows.Controls.Control control)
	{
		if (control is DateEditControl dateEditControl)
		{
			return Convertor.ConvertDateOnlyToString(dateEditControl.EditValue);
		}
		return Convert.ToString(ControlsHelper.GetControlValue(control));
	}

	protected static LayoutItemWithRequired AddLayoutItem(DevExpress.Xpf.LayoutControl.LayoutGroup container, UIElement myControl, string caption, System.Windows.HorizontalAlignment horizontalAlignment)
	{
		LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired
		{
			Label = caption,
			HorizontalAlignment = horizontalAlignment,
			Content = myControl
		};
		container?.Children.Add(layoutItemWithRequired);
		return layoutItemWithRequired;
	}

	public static LayoutGroupExt AddLayoutGroup(DevExpress.Xpf.LayoutControl.LayoutGroup container, System.Windows.Controls.Orientation orientation, string header, LayoutGroupView view, System.Windows.HorizontalAlignment alignment = System.Windows.HorizontalAlignment.Stretch, string name = "", bool isCollapsible = true, bool isCollapsed = true)
	{
		LayoutGroupExt layoutGroupExt = new LayoutGroupExt
		{
			View = view,
			Orientation = orientation,
			Name = name,
			IsCollapsible = isCollapsible,
			IsCollapsed = isCollapsed,
			HorizontalAlignment = alignment
		};
		if (!string.IsNullOrWhiteSpace(header))
		{
			layoutGroupExt.Header = header;
		}
		container?.Children.Add(layoutGroupExt);
		return layoutGroupExt;
	}

	protected void AddControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, System.Windows.Controls.Control control, UsersParamButton usersParamButton, bool userParam, bool paramUseFromCache, string valueFromService, bool showOnlyUserValue = false)
	{
		if (_controlsByPanels.TryGetValue(panel.Name, out var value))
		{
			value.Add(new ControlDesc(Convert.ToInt16(control.Name.Substring("Param_".Length)), control, usersParamButton, userParam, paramUseFromCache, valueFromService, showOnlyUserValue));
			return;
		}
		Dictionary<string, List<ControlDesc>> controlsByPanels = _controlsByPanels;
		string name = panel.Name;
		int num = 1;
		List<ControlDesc> list = new List<ControlDesc>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new ControlDesc(Convert.ToInt16(control.Name.Substring("Param_".Length)), control, usersParamButton, userParam, paramUseFromCache, valueFromService, showOnlyUserValue);
		controlsByPanels.Add(name, list);
	}

	protected void AddGridControl(DocumentGridControl control, bool paramUseFromCache)
	{
		_loadedGridControls.Add(new GridControlDesc(control, paramUseFromCache));
	}

	protected static UsersParamButton AddUserParamButton<T>(DevExpress.Xpf.LayoutControl.LayoutGroup layoutGroup, ParameterDefinition<T> parameterDefinition, System.Windows.Controls.Control connControl)
	{
		UsersParamButton usersParamButton = new UsersParamButton(parameterDefinition.Id, parameterDefinition.Caption, connControl, Settings.LoginService.IsAdmin)
		{
			Visibility = ((!Settings.LoginService.IsAdmin) ? Visibility.Collapsed : Visibility.Visible)
		};
		layoutGroup.Children.Add(usersParamButton);
		return usersParamButton;
	}

	protected DropDownCommentControl AddDropDownCommentControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<string> parameterDefinition, bool isRequired, IDropDownView idv, short length, out UsersParamButton usersParamButton, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		DropDownCommentControl dropDownCommentControl;
		if (parameterDefinition.UserParam)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			dropDownCommentControl = AddDropDownCommentControl(layoutGroupExt, caption ?? parameterDefinition.Caption, isRequired, idv, length, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, dropDownCommentControl);
		}
		else
		{
			dropDownCommentControl = AddDropDownCommentControl(container, caption ?? parameterDefinition.Caption, isRequired, idv, length, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = null;
		}
		dropDownCommentControl.IsEnabled = Settings.LoginService.IsAdmin;
		dropDownCommentControl.Text = ParametersFromService[parameterDefinition.Id];
		AddControl(panel, dropDownCommentControl, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return dropDownCommentControl;
	}

	protected DropDownCommentControl AddDropDownCommentControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, IDropDownView idv, short length = 50, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		DropDownCommentControl dropDownCommentControl = new DropDownCommentControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, dropDownCommentControl, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(dropDownCommentControl, li);
		dropDownCommentControl.IsRequired = isRequired;
		dropDownCommentControl.DataView = idv;
		dropDownCommentControl.MaxLength = length;
		dropDownCommentControl.MinWidth = 5.0;
		dropDownCommentControl.Owner = this;
		dropDownCommentControl.Text = "";
		if (horizontalAlignment != System.Windows.HorizontalAlignment.Stretch)
		{
			dropDownCommentControl.Width = SizeCalculator.WidthForString(length);
		}
		TabValidationHelper.RegisterControl(dropDownCommentControl);
		return dropDownCommentControl;
	}

	protected TextEditExt AddStringControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<string> parameterDefinition, bool isRequired, short length, out UsersParamButton usersParamButton, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		TextEditExt textEditExt;
		if (parameterDefinition.UserParam)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			textEditExt = AddStringControl(layoutGroupExt, caption ?? parameterDefinition.Caption, isRequired, length, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, textEditExt);
		}
		else
		{
			textEditExt = AddStringControl(container, caption ?? parameterDefinition.Caption, isRequired, length, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = null;
		}
		textEditExt.IsEnabled = Settings.LoginService.IsAdmin;
		textEditExt.Text = ParametersFromService[parameterDefinition.Id];
		AddControl(panel, textEditExt, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return textEditExt;
	}

	public static TextEditExt AddStringControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, short length, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		TextEditExt textEditExt = new TextEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, textEditExt, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(textEditExt, li);
		textEditExt.IsRequired = isRequired;
		textEditExt.MaxLength = length;
		textEditExt.MinWidth = 5.0;
		if (horizontalAlignment != System.Windows.HorizontalAlignment.Stretch)
		{
			textEditExt.Width = SizeCalculator.WidthForString(length);
		}
		TabValidationHelper.RegisterControl(textEditExt);
		return textEditExt;
	}

	public ButtonEditExt AddButtonEditControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, short length, short estimatedButtonsCount = 1, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		ButtonEditExt buttonEditExt = new ButtonEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, buttonEditExt, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(buttonEditExt, li);
		buttonEditExt.IsRequired = isRequired;
		buttonEditExt.MaxLength = length;
		buttonEditExt.MinWidth = 5.0;
		buttonEditExt.Owner = this;
		if (horizontalAlignment != System.Windows.HorizontalAlignment.Stretch)
		{
			buttonEditExt.Width = SizeCalculator.WidthForString(length) + (double)(20 * estimatedButtonsCount);
		}
		TabValidationHelper.RegisterControl(buttonEditExt);
		return buttonEditExt;
	}

	public static ButtonInfo AddButtonInfoToButtonEdit(ButtonEditExt buttonEdit, GlyphKind glyph, string tooltip = "", object content = null)
	{
		ButtonInfo buttonInfo = new ButtonInfo
		{
			GlyphKind = glyph,
			ToolTip = tooltip
		};
		if (content != null)
		{
			buttonInfo.Content = content;
		}
		buttonEdit.Buttons.Add(buttonInfo);
		return buttonInfo;
	}

	protected MemoEditControl AddMemoControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<string> parameterDefinition, bool isRequired, short length, out UsersParamButton usersParamButton, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		MemoEditControl memoEditControl;
		if (parameterDefinition.UserParam)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			memoEditControl = AddMemoControl(layoutGroupExt, caption ?? parameterDefinition.Caption, isRequired, length, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, memoEditControl);
		}
		else
		{
			memoEditControl = AddMemoControl(container, caption ?? parameterDefinition.Caption, isRequired, length, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = null;
		}
		memoEditControl.IsEnabled = Settings.LoginService.IsAdmin;
		memoEditControl.Text = ParametersFromService[parameterDefinition.Id];
		AddControl(panel, memoEditControl, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return memoEditControl;
	}

	public MemoEditControl AddMemoControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, short length, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		MemoEditControl memoEditControl = new MemoEditControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, memoEditControl, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(memoEditControl, li);
		memoEditControl.IsRequired = isRequired;
		memoEditControl.MaxLength = length;
		memoEditControl.Owner = this;
		memoEditControl.DropDownCaption = caption;
		memoEditControl.MinWidth = 32.0;
		if (horizontalAlignment != System.Windows.HorizontalAlignment.Stretch)
		{
			memoEditControl.Width = SizeCalculator.WidthForString(length);
		}
		TabValidationHelper.RegisterControl(memoEditControl);
		return memoEditControl;
	}

	protected DateEditControl AddDateControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<DateTime?> parameterDefinition, bool isRequired, out UsersParamButton usersParamButton, bool showOnlyUserValue = false, bool useLongDate = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		DateEditControl dateEditControl;
		if (parameterDefinition.UserParam && !showOnlyUserValue)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			dateEditControl = AddDateControl(layoutGroupExt, caption ?? parameterDefinition.Caption, isRequired, useLongDate, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, dateEditControl);
		}
		else
		{
			dateEditControl = AddDateControl(container, caption ?? parameterDefinition.Caption, isRequired, useLongDate, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = null;
		}
		if (!showOnlyUserValue)
		{
			dateEditControl.IsEnabled = Settings.LoginService.IsAdmin;
		}
		dateEditControl.EditValue = Convertor.ConvertStringToDateOnly(ParametersFromService[parameterDefinition.Id]);
		AddControl(panel, dateEditControl, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return dateEditControl;
	}

	protected DateEditControl AddDateControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<DateOnly?> parameterDefinition, bool isRequired, out UsersParamButton usersParamButton, bool showOnlyUserValue = false, bool useLongDate = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		DateEditControl dateEditControl;
		if (parameterDefinition.UserParam && !showOnlyUserValue)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			dateEditControl = AddDateControl(layoutGroupExt, caption ?? parameterDefinition.Caption, isRequired, useLongDate, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, dateEditControl);
		}
		else
		{
			dateEditControl = AddDateControl(container, caption ?? parameterDefinition.Caption, isRequired, useLongDate, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = null;
		}
		if (!showOnlyUserValue)
		{
			dateEditControl.IsEnabled = Settings.LoginService.IsAdmin;
		}
		dateEditControl.EditValue = Convertor.ConvertStringToDateOnly(ParametersFromService[parameterDefinition.Id]);
		AddControl(panel, dateEditControl, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return dateEditControl;
	}

	public static DateEditControl AddDateControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, bool useLongDate = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		DateEditControl dateEditControl = new DateEditControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, dateEditControl, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(dateEditControl, li);
		dateEditControl.UseLongDate = useLongDate;
		dateEditControl.IsRequired = isRequired;
		dateEditControl.Width = SizeCalculator.WidthForDate(useLongDate);
		TabValidationHelper.RegisterControl(dateEditControl);
		return dateEditControl;
	}

	public static TimeEditControl AddTimeControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, bool showSeconds = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		TimeEditControl timeEditControl = new TimeEditControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, timeEditControl, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(timeEditControl, li);
		timeEditControl.ShowSeconds = showSeconds;
		timeEditControl.IsRequired = isRequired;
		timeEditControl.Width = SizeCalculator.WidthForTime(showSeconds);
		TabValidationHelper.RegisterControl(timeEditControl);
		return timeEditControl;
	}

	protected PopupCalcEditExt AddNumericControl<T>(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<T> parameterDefinition, bool isRequired, short length, short precision, out UsersParamButton usersParamButton, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		decimal value = Convertor.ConvertStringToDecimal(ParametersFromService[parameterDefinition.Id]);
		PopupCalcEditExt popupCalcEditExt;
		if (parameterDefinition.UserParam)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			popupCalcEditExt = AddNumericControl(layoutGroupExt, caption ?? parameterDefinition.Caption, isRequired, length, precision, allowNegativeNumbers, useCurrencyFormat, horizontalAlignment, "Param_" + parameterDefinition.Id, value);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, popupCalcEditExt);
		}
		else
		{
			popupCalcEditExt = AddNumericControl(container, caption ?? parameterDefinition.Caption, isRequired, length, precision, allowNegativeNumbers, useCurrencyFormat, horizontalAlignment, "Param_" + parameterDefinition.Id, value);
			usersParamButton = null;
		}
		popupCalcEditExt.IsEnabled = Settings.LoginService.IsAdmin;
		AddControl(panel, popupCalcEditExt, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return popupCalcEditExt;
	}

	public static PopupCalcEditExt AddNumericControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, short length, short precision = 0, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "", decimal value = 0m)
	{
		PopupCalcEditExt popupCalcEditExt = new PopupCalcEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, popupCalcEditExt, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(popupCalcEditExt, li);
		popupCalcEditExt.IsRequired = isRequired;
		popupCalcEditExt.AllowNegativeNumbers = allowNegativeNumbers;
		popupCalcEditExt.Mask = (useCurrencyFormat ? DecimalExtension.GetCurrencyFormat(length, precision) : DecimalExtension.GetNumberFormat(length, precision));
		popupCalcEditExt.Precision = precision;
		popupCalcEditExt.Width = SizeCalculator.WidthForNumber(length);
		popupCalcEditExt.EditValue = value;
		TabValidationHelper.RegisterControl(popupCalcEditExt);
		return popupCalcEditExt;
	}

	protected CheckEditExt AddBooleanControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<bool> parameterDefinition, out UsersParamButton usersParamButton, bool showOnlyUserValue = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null, bool showCaptionAtRight = false, bool addPadding = false)
	{
		if (caption == null)
		{
			caption = parameterDefinition.Caption;
		}
		CheckEditExt checkEditExt;
		if (parameterDefinition.UserParam && !showOnlyUserValue)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			checkEditExt = AddBooleanControl(layoutGroupExt, caption, horizontalAlignment, isThreeState: false, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, checkEditExt);
		}
		else
		{
			usersParamButton = null;
			if (showCaptionAtRight)
			{
				LayoutGroupExt container2 = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
				checkEditExt = AddBooleanControl(container2, "", horizontalAlignment, isThreeState: false, "Param_" + parameterDefinition.Id, showCaptionAtRight: false, addPadding);
				AddTextBlock(container2, caption, new Thickness(0.0, 3.0, 0.0, 0.0), isBold: false, addPadding);
			}
			else
			{
				checkEditExt = AddBooleanControl(container, caption, horizontalAlignment, isThreeState: false, "Param_" + parameterDefinition.Id);
			}
		}
		if (!showOnlyUserValue)
		{
			checkEditExt.IsEnabled = Settings.LoginService.IsAdmin;
		}
		checkEditExt.IsChecked = ((parameterDefinition.UserParam & showOnlyUserValue) ? Settings.ParametersService.GetCurrentUserBoolParam(parameterDefinition) : Convertor.ConvertStringToBool(ParametersFromService[parameterDefinition.Id]));
		AddControl(panel, checkEditExt, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, checkEditExt.IsChecked.ToString(), showOnlyUserValue);
		return checkEditExt;
	}

	public static CheckEditExt AddBooleanControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isThreeState = false, string name = "", bool showCaptionAtRight = false, bool addPadding = false)
	{
		if (showCaptionAtRight)
		{
			LayoutGroupExt container2 = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			CheckEditExt result = AddBooleanControl(container2, "", horizontalAlignment, isThreeState: false, name, showCaptionAtRight: false, addPadding);
			AddTextBlock(container2, caption, new Thickness(0.0, 3.0, 0.0, 0.0), isBold: false, addPadding);
			return result;
		}
		CheckEditExt checkEditExt = new CheckEditExt
		{
			Name = name
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, checkEditExt, caption, horizontalAlignment);
		CommonEventHandlers.AddIsReadonlyBinding(checkEditExt, layoutItemWithRequired);
		if (addPadding)
		{
			layoutItemWithRequired.Padding = new Thickness(0.0, -4.0, 0.0, 0.0);
		}
		checkEditExt.Focusable = true;
		checkEditExt.IsChecked = false;
		checkEditExt.IsThreeState = isThreeState;
		return checkEditExt;
	}

	public static TextBlock AddTextBlock(DevExpress.Xpf.LayoutControl.LayoutGroup container, string text, Thickness margin, bool isBold = false, bool addPadding = false)
	{
		TextBlock textBlock = new TextBlock
		{
			Text = text,
			Margin = margin,
			TextWrapping = TextWrapping.Wrap
		};
		if (isBold)
		{
			textBlock.FontWeight = FontWeights.Bold;
		}
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, textBlock, "", System.Windows.HorizontalAlignment.Left);
		if (addPadding)
		{
			layoutItemWithRequired.Padding = new Thickness(0.0, -3.0, 0.0, 0.0);
		}
		CommonEventHandlers.AddIsReadonlyBinding(textBlock, layoutItemWithRequired);
		return textBlock;
	}

	protected DropDownControl AddDropDownControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<string> parameterDefinition, bool isRequired, IDropDownView idv, out UsersParamButton usersParamButton, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		DropDownControl dropDownControl;
		if (parameterDefinition.UserParam)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			dropDownControl = AddDropDownControl(layoutGroupExt, caption ?? parameterDefinition.Caption, isRequired, idv, horizontalAlignment, "Param_" + parameterDefinition.Id);
			dropDownControl.ShowComment = false;
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, dropDownControl);
		}
		else
		{
			dropDownControl = AddDropDownControl(container, caption ?? parameterDefinition.Caption, isRequired, idv, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = null;
		}
		dropDownControl.IsEnabled = Settings.LoginService.IsAdmin;
		dropDownControl.Code = ParametersFromService[parameterDefinition.Id];
		AddControl(panel, dropDownControl, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return dropDownControl;
	}

	public DropDownControl AddDropDownControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, IDropDownView idv, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		DropDownControl dropDownControl = new DropDownControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, dropDownControl, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(dropDownControl, li);
		dropDownControl.IsRequired = isRequired;
		dropDownControl.DataView = idv;
		dropDownControl.Owner = this;
		dropDownControl.Code = "";
		TabValidationHelper.RegisterControl(dropDownControl.InnerCodeEdit);
		if (horizontalAlignment == System.Windows.HorizontalAlignment.Stretch)
		{
			dropDownControl.InnerCommentEdit.MaxWidth = double.PositiveInfinity;
		}
		return dropDownControl;
	}

	protected MultiSelectDropDownControl AddMultiSelectDropDownControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<string> parameterDefinition, bool isRequired, IDropDownView idv, out UsersParamButton usersParamButton, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		MultiSelectDropDownControl multiSelectDropDownControl;
		if (parameterDefinition.UserParam)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			multiSelectDropDownControl = AddMultiSelectDropDownControl(layoutGroupExt, caption ?? parameterDefinition.Caption, isRequired, idv, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, multiSelectDropDownControl);
		}
		else
		{
			multiSelectDropDownControl = AddMultiSelectDropDownControl(container, caption ?? parameterDefinition.Caption, isRequired, idv, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = null;
		}
		multiSelectDropDownControl.IsEnabled = Settings.LoginService.IsAdmin;
		multiSelectDropDownControl.SelectedString = ParametersFromService[parameterDefinition.Id];
		AddControl(panel, multiSelectDropDownControl, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return multiSelectDropDownControl;
	}

	public MultiSelectDropDownControl AddMultiSelectDropDownControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, bool isRequired, IDropDownView idv, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		MultiSelectDropDownControl multiSelectDropDownControl = new MultiSelectDropDownControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, multiSelectDropDownControl, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(multiSelectDropDownControl, li);
		multiSelectDropDownControl.IsRequired = isRequired;
		multiSelectDropDownControl.DataView = idv;
		multiSelectDropDownControl.Owner = this;
		multiSelectDropDownControl.SelectedString = "";
		TabValidationHelper.RegisterControl(multiSelectDropDownControl.InnerCodeEdit);
		return multiSelectDropDownControl;
	}

	public static System.Windows.Controls.Button AddButton(DevExpress.Xpf.LayoutControl.LayoutGroup container, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		System.Windows.Controls.Button button = new System.Windows.Controls.Button
		{
			Name = name
		};
		AddLayoutItem(container, button, labelCaption, horizontalAlignment);
		if (!string.IsNullOrEmpty(tooltip))
		{
			button.ToolTip = tooltip;
		}
		button.IsTabStop = isTabStop;
		button.Margin = new Thickness(2.0, 0.0, 0.0, 2.0);
		if (string.IsNullOrWhiteSpace(imageURI))
		{
			button.Content = caption;
		}
		else
		{
			StackPanel stackPanel = new StackPanel();
			stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
			ImageSource image = ImageService.GetImage(imageURI);
			DXImage element = new DXImage
			{
				Source = image,
				Width = 16.0,
				Height = 16.0
			};
			stackPanel.Children.Add(element);
			if (!string.IsNullOrWhiteSpace(caption))
			{
				TextBlock element2 = new TextBlock
				{
					Text = caption,
					Margin = new Thickness(3.0, 0.0, 0.0, 0.0)
				};
				stackPanel.Children.Add(element2);
			}
			stackPanel.Margin = new Thickness(-7.0, 0.0, -7.0, 0.0);
			button.Content = stackPanel;
		}
		return button;
	}

	public static SimpleButton AddSimpleButton(DevExpress.Xpf.LayoutControl.LayoutGroup container, double glyphHeight, double glyphWidth, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, System.Windows.Controls.Dock glyphAlignment = System.Windows.Controls.Dock.Top, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, double glyphToContentOffset = 0.0, string name = "")
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		SimpleButton simpleButton = new SimpleButton
		{
			Name = name
		};
		AddLayoutItem(container, simpleButton, caption, horizontalAlignment);
		if (!string.IsNullOrEmpty(tooltip))
		{
			simpleButton.ToolTip = tooltip;
		}
		simpleButton.IsTabStop = isTabStop;
		simpleButton.Margin = new Thickness(2.0, 0.0, 0.0, 2.0);
		simpleButton.Content = caption;
		simpleButton.GlyphToContentOffset = glyphToContentOffset;
		simpleButton.Glyph = ImageService.GetImage(imageURI);
		simpleButton.GlyphHeight = glyphHeight;
		simpleButton.GlyphWidth = glyphWidth;
		simpleButton.GlyphAlignment = glyphAlignment;
		return simpleButton;
	}

	protected ComboBoxEditExt AddComboBoxControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<string> parameterDefinition, short length, bool isRequired, ArrayList itemsSource, out UsersParamButton usersParamButton, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null)
	{
		ComboBoxEditExt comboBoxEditExt;
		if (parameterDefinition.UserParam)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			comboBoxEditExt = AddComboBoxControl(layoutGroupExt, caption ?? parameterDefinition.Caption, length, isRequired, itemsSource, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, comboBoxEditExt);
		}
		else
		{
			comboBoxEditExt = AddComboBoxControl(container, caption ?? parameterDefinition.Caption, length, isRequired, itemsSource, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = null;
		}
		comboBoxEditExt.IsEnabled = Settings.LoginService.IsAdmin;
		comboBoxEditExt.Text = ParametersFromService[parameterDefinition.Id];
		AddControl(panel, comboBoxEditExt, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return comboBoxEditExt;
	}

	public static ComboBoxEditExt AddComboBoxControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, short length, bool isRequired, ArrayList itemsSource, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		ComboBoxEditExt comboBoxEditExt = new ComboBoxEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, comboBoxEditExt, caption, horizontalAlignment);
		CommonEventHandlers.AddIsRequiredBinding(comboBoxEditExt, li);
		comboBoxEditExt.IsRequired = isRequired;
		comboBoxEditExt.MaxLength = length;
		comboBoxEditExt.MinWidth = 5.0;
		comboBoxEditExt.ItemsSource = itemsSource;
		if (horizontalAlignment != System.Windows.HorizontalAlignment.Stretch)
		{
			comboBoxEditExt.Width = SizeCalculator.WidthForString(length);
		}
		TabValidationHelper.RegisterControl(comboBoxEditExt);
		return comboBoxEditExt;
	}

	protected SpinEditExt AddSpinEditControl<T>(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<T> parameterDefinition, decimal minValue, decimal maxValue, out UsersParamButton usersParamButton, double minWidth = 50.0, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string caption = null, bool showCaptionAtRight = false)
	{
		SpinEditExt spinEditExt;
		if (parameterDefinition.UserParam)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			spinEditExt = AddSpinEditControl(layoutGroupExt, caption ?? parameterDefinition.Caption, minValue, maxValue, minWidth, horizontalAlignment, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, spinEditExt);
		}
		else
		{
			spinEditExt = AddSpinEditControl(container, caption ?? parameterDefinition.Caption, minValue, maxValue, minWidth, horizontalAlignment, "Param_" + parameterDefinition.Id, showCaptionAtRight);
			usersParamButton = null;
		}
		spinEditExt.IsEnabled = Settings.LoginService.IsAdmin;
		spinEditExt.EditValue = Convertor.ConvertStringToDecimal(ParametersFromService[parameterDefinition.Id]);
		AddControl(panel, spinEditExt, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, ParametersFromService[parameterDefinition.Id]);
		return spinEditExt;
	}

	public static SpinEditExt AddSpinEditControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, decimal minValue, decimal maxValue, double minWidth = 50.0, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "", bool showCaptionAtRight = false)
	{
		if (showCaptionAtRight)
		{
			LayoutGroupExt container2 = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			SpinEditExt result = AddSpinEditControl(container2, "", minValue, maxValue, minWidth, horizontalAlignment, name);
			AddTextBlock(container2, caption, new Thickness(0.0, 3.0, 0.0, 0.0));
			return result;
		}
		SpinEditExt spinEditExt = new SpinEditExt
		{
			Name = name,
			IsFloatValue = false,
			MinValue = minValue,
			MaxValue = maxValue,
			IsReadOnly = false,
			MinWidth = minWidth
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, spinEditExt, caption, horizontalAlignment);
		layoutItemWithRequired.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
		layoutItemWithRequired.Label = caption;
		layoutItemWithRequired.Content = spinEditExt;
		TabValidationHelper.RegisterControl(spinEditExt);
		return spinEditExt;
	}

	protected PasswordBoxEditExt AddPasswordControl(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, ParameterDefinition<string> parameterDefinition, short length, bool isRequired, out UsersParamButton usersParamButton, bool showOnlyUserValue = false, string caption = null, bool showCaptionAtRight = false)
	{
		if (caption == null)
		{
			caption = parameterDefinition.Caption;
		}
		PasswordBoxEditExt passwordBoxEditExt;
		if (parameterDefinition.UserParam && !showOnlyUserValue)
		{
			LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			passwordBoxEditExt = AddPasswordControl(layoutGroupExt, caption, length, isRequired, "Param_" + parameterDefinition.Id);
			usersParamButton = AddUserParamButton(layoutGroupExt, parameterDefinition, passwordBoxEditExt);
		}
		else
		{
			usersParamButton = null;
			if (showCaptionAtRight)
			{
				LayoutGroupExt container2 = AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
				passwordBoxEditExt = AddPasswordControl(container2, "", length, isRequired, "Param_" + parameterDefinition.Id);
				AddTextBlock(container2, caption, new Thickness(0.0, 3.0, 0.0, 0.0));
			}
			else
			{
				passwordBoxEditExt = AddPasswordControl(container, caption, length, isRequired, "Param_" + parameterDefinition.Id);
			}
		}
		if (!showOnlyUserValue)
		{
			passwordBoxEditExt.IsEnabled = Settings.LoginService.IsAdmin;
		}
		passwordBoxEditExt.Text = ((parameterDefinition.UserParam & showOnlyUserValue) ? Settings.ParametersService.GetCurrentUserStringParam(parameterDefinition) : ParametersFromService[parameterDefinition.Id]);
		AddControl(panel, passwordBoxEditExt, usersParamButton, parameterDefinition.UserParam, parameterDefinition.UseFromCacheUI, passwordBoxEditExt.Text, showOnlyUserValue);
		return passwordBoxEditExt;
	}

	public static PasswordBoxEditExt AddPasswordControl(DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption, short length, bool isRequired = false, string name = "")
	{
		PasswordBoxEditExt passwordBoxEditExt = new PasswordBoxEditExt
		{
			Name = name,
			MaxLength = length,
			PasswordChar = '*'
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, passwordBoxEditExt, caption, System.Windows.HorizontalAlignment.Left);
		CommonEventHandlers.AddIsRequiredBinding(passwordBoxEditExt, layoutItemWithRequired);
		passwordBoxEditExt.IsRequired = isRequired;
		passwordBoxEditExt.Width = SizeCalculator.WidthForString(length);
		layoutItemWithRequired.Content = passwordBoxEditExt;
		TabValidationHelper.RegisterControl(passwordBoxEditExt);
		return passwordBoxEditExt;
	}

	public DocumentGridControl AddGrid(DevExpress.Xpf.LayoutControl.LayoutGroup panel, DevExpress.Xpf.LayoutControl.LayoutGroup container, string caption = "", System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Stretch, VerticalAlignment verticalAlignment = VerticalAlignment.Stretch, bool isRequired = false, string name = "", string toolTip = "", bool paramUseFromCache = false)
	{
		DocumentGridControl documentGridControl = new DocumentGridControl();
		container?.Children.Add(documentGridControl);
		documentGridControl.HorizontalAlignment = horizontalAlignment;
		documentGridControl.VerticalAlignment = verticalAlignment;
		documentGridControl.Owner = this;
		documentGridControl.ToolTip = (string.IsNullOrWhiteSpace(toolTip) ? caption : toolTip);
		documentGridControl.Caption = caption;
		documentGridControl.ShowButtons = false;
		documentGridControl.AllowDelete = false;
		documentGridControl.AllowDuplicate = false;
		documentGridControl.IsVisibleDuplicateButton = false;
		documentGridControl.ControlEnterPressed += new SafeEventHandler<EventArgs>(OnGridControlEnterPressed).Handler;
		documentGridControl.Name = name;
		documentGridControl.IsRequired = isRequired;
		AddGridControl(documentGridControl, paramUseFromCache);
		TabValidationHelper.RegisterControl(documentGridControl);
		TableView view = documentGridControl.View;
		view.NavigationStyle = GridViewNavigationStyle.Cell;
		view.AutoWidth = true;
		view.AllowColumnFiltering = false;
		view.AllowGrouping = false;
		view.AllowSorting = false;
		view.AllowDrop = false;
		view.ShowGroupPanel = false;
		view.AllowColumnMoving = false;
		return documentGridControl;
	}

	public void AddSpecialDateButton(DevExpress.Xpf.LayoutControl.LayoutGroup container, DateEditControl dateControl, DateEditControl periodEndControl = null)
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			throw new InvalidOperationException("Dialog name is empty");
		}
		SpecialDateButton specialDateButton = new SpecialDateButton
		{
			Name = "SpecialDate_" + dateControl.Name,
			DateControl = dateControl,
			DatePeriodEndControl = periodEndControl,
			ToolTip = ArmSoft.SME.Common.Resources.Dates + " (Alt+D)",
			IsEnabled = dateControl.IsEnabled
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, specialDateButton, "", System.Windows.HorizontalAlignment.Left);
		layoutItemWithRequired.Margin = new Thickness(15.0, 0.0, 0.0, 0.0);
		dateControl.ToolTip = ArmSoft.SME.Common.Resources.SpecialDateHotKeyMsg;
		if (periodEndControl != null)
		{
			periodEndControl.ToolTip = ArmSoft.SME.Common.Resources.SpecialDateHotKeyMsg;
		}
		specialDateButton.IsTabStop = false;
		layoutItemWithRequired.Content = specialDateButton;
	}

	protected internal void OnGridControlEnterPressed(object sender, EventArgs e)
	{
		Save();
	}
}
