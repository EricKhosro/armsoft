using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.SmartEnums;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Ribbon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.PersistableObject;

public class POWindow<T> : ThemedWindow, IPOWindow, IScanable, IMagneticCardReadable, IPOWindowInfo, INotifyPropertyChanged where T : ArmSoft.SME.Common.Models.PersistableObject, new()
{
	public delegate void PreviewKeyDownControlKeyGEventHandler();

	protected class ControlDescriptor
	{
		public Control Control { get; set; }

		public LayoutItemWithRequired LayoutItem { get; set; }

		public string Caption { get; set; }

		public string Name { get; set; }

		public string DataItemName { get; set; }
	}

	protected const string _tabNamePrefix = "Tab_";

	protected WindowViewMode _viewMode;

	private ExtensionsManager _extensionManager;

	private ArmSoft.SME.Core.UI.MetaData.DocumentExtensionDefinition _extensionDefinition;

	protected Control _setFocusOnActivation;

	protected Control _setFocusAfterInvalidSave;

	private bool _firstActivation = true;

	protected bool _isReadOnly;

	protected bool _isDialog;

	protected string _registryKey = "";

	private Button _createUserDefinedFields;

	protected BindingList<TemplatesCacheRow> _printTemplates;

	protected byte[] _printTemplatesTS;

	protected TemplatesCacheRow _printTemplate;

	protected bool _forceClose;

	private bool _ribbonToolbarShowAbove = true;

	private bool _ribbonIsMinimized = true;

	private bool _restoringSettings;

	public static readonly DependencyProperty IsGridModeProperty = DependencyProperty.Register("GridMode", typeof(bool), typeof(POWindow<T>));

	private List<Button> _addedButtons;

	private List<DocumentGridControl> _addedGridsWithSupportDialogMode;

	private DXTabControl _generalTabControl;

	protected BarManager _barManager;

	private readonly BarButtonItem _barButtonSave;

	private readonly BarButtonItem _barButtonSaveAndClose;

	private readonly BarButtonItem _barButtonSaveAndNew;

	private readonly BarButtonItem _barButtonClose;

	private readonly BarButtonItem _barButtonPrint;

	private readonly BarButtonItem _barButtonPrintPreview;

	private readonly BarButtonItem _barButtonWord;

	private readonly BarSubItem _barSubItemExport;

	private readonly BarButtonItem _barButtonExcel;

	private readonly BarButtonItem _barButtonPdf;

	private readonly BarButtonItem _barButtonDraft;

	private readonly BarButtonItem _barButtonReload;

	private readonly BarSubItem _barItemAttachments;

	private readonly RibbonPage _ribbonAttachmentPage;

	private readonly RibbonPageGroup _ribbonAttachmentPageGroup;

	private readonly BarButtonItem _barButtonAttachmentFiles;

	private readonly BarButtonItem _barButtonAttachmentLinks;

	private readonly BarButtonItem _barButtonAttachmentDocs;

	private readonly BarButtonItem _barButtonHelp;

	private readonly BarCheckItem _barCheckDialogMode;

	private readonly BarCheckItem _barCheckGridRowMode;

	private readonly BarEditItem _barEditCurrentTemplate;

	private readonly RibbonPage _ribbonPageHome;

	private readonly RibbonPageGroup _ribbonPageSave;

	private readonly RibbonPageGroup _ribbonPagePrint;

	private readonly RibbonPageGroup _ribbonPageExport;

	private readonly RibbonPageGroup _ribbonPagePrintTemplate;

	private readonly RibbonPageGroup _ribbonPageGridSettings;

	protected readonly RibbonPageGroup _ribbonPageForOther;

	private readonly BarButtonItem _barButtonExportXML;

	protected readonly BarButtonItem _barButtonShowISN;

	private readonly LayoutControl _layoutControl;

	private readonly ComboBoxEditSettings _currentTemplateComboEdit;

	private readonly PropertyChangeNotifier _isChangedNotifier;

	protected RibbonStatusBarControl _ribbonStatusBar;

	protected RibbonPage _ribbonPageOther;

	protected RibbonPageGroup _ribbonPageGroupActions;

	protected RibbonPageGroup _ribbonPageGroupReports;

	public static readonly DependencyProperty IsChangedProperty = DependencyProperty.Register("IsChanged", typeof(bool), typeof(POWindow<T>));

	public virtual bool StoredInDb { get; set; }

	public virtual byte State { get; set; }

	public bool IsOfflineDoc { get; set; }

	public virtual PODefinition Definition { get; }

	public T PO { get; set; }

	protected virtual string Caption => Definition.FullCaption;

	protected List<DXTabItem> TabControls { get; set; }

	protected List<ControlDescriptor> Controls { get; set; }

	protected virtual double WidthValue => 700.0;

	protected virtual double HeightValue => 350.0;

	protected virtual double MinWidthValue => 300.0;

	protected virtual double MinHeightValue => 200.0;

	protected virtual bool WriteInHistory { get; }

	protected RibbonControl Ribbon { get; private set; }

	protected BarStaticItem StatusBarCurrentRow { get; private set; }

	protected BarStaticItem StatusBarDocumentState { get; private set; }

	public int DVPID { get; set; }

	public bool IsLoadingDataToControls { get; set; }

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

	private bool GridMode
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsGridModeProperty);
		}
		set
		{
			if (value != (bool)((DependencyObject)this).GetValue(IsGridModeProperty) || _firstActivation)
			{
				((DependencyObject)this).SetValue(IsGridModeProperty, (object)value);
				GridModeChanged(GridMode);
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GridMode"));
		}
	}

	public string GetTitle
	{
		get
		{
			if (PO == null)
			{
				return "";
			}
			if (StoredInDb)
			{
				if (_viewMode == WindowViewMode.View)
				{
					return Settings.LoginService.Database + " - " + Caption + ArmSoft.SME.Common.Resources.CaptionView;
				}
				return Settings.LoginService.Database + " - " + Caption + ArmSoft.SME.Common.Resources.CaptionEdit;
			}
			return Settings.LoginService.Database + " - " + Caption + (_isReadOnly ? "" : ArmSoft.SME.Common.Resources.CaptionNew);
		}
	}

	public DocumentExtension Extension { get; set; }

	public ArmSoft.SME.Core.UI.MetaData.DocumentExtensionDefinition ExtensionDefinition
	{
		get
		{
			return _extensionDefinition;
		}
		set
		{
			if (value == null)
			{
				_extensionDefinition = null;
				Extension = null;
				PO.ExtensionValues = null;
			}
			else if (_extensionDefinition != value)
			{
				_extensionDefinition = value;
				Extension = new DocumentExtension(value)
				{
					Values = PO.ExtensionValues
				};
			}
		}
	}

	protected virtual bool AllowExportToXml => Definition.IsExchangeable;

	private static RoutedEventHandler TextChangedHandler { get; set; }

	private static RoutedEventHandler GridRowsChangedHandler { get; set; }

	public BarButtonItem InnerBarButtonSave => _barButtonSave;

	public BarButtonItem InnerBarButtonSaveAndNew => _barButtonSaveAndNew;

	public BarButtonItem InnerBarButtonDraft => _barButtonDraft;

	public BarButtonItem InnerBarButtonReload => _barButtonReload;

	public event PreviewKeyDownControlKeyGEventHandler PreviewKeyDownControlKeyG;

	public event PropertyChangedEventHandler PropertyChanged;

	private void IsChangedChanged(object sender, EventArgs e)
	{
		Title = GetTitle + (IsChanged ? "*" : "");
	}

	public virtual void Show(WindowViewMode? viewMode = null, bool disableGridValidators = false)
	{
		WindowViewMode valueOrDefault = viewMode.GetValueOrDefault();
		if (!viewMode.HasValue)
		{
			valueOrDefault = ((!StoredInDb) ? WindowViewMode.NewCreate : WindowViewMode.Edit);
			viewMode = valueOrDefault;
		}
		_viewMode = viewMode.Value;
		_isReadOnly = _viewMode == WindowViewMode.View;
		_isDialog = false;
		ConstructUI();
		if (!Settings.IsOffline)
		{
			if (Settings.ParametersService.KeepUICacheUpToDate())
			{
				Settings.CacheFunctionsService.RefreshCaches();
			}
			if (PO.POName != null)
			{
				ExtensionDefinition = Settings.ServiceProvider.GetRequiredService<IDocumentExtensionDefinitionService>().Load(Definition.POTypeDef, Definition.POType);
			}
		}
		if (Definition.IsExtendable == POExtendableType.Simple)
		{
			DXTabControl tc = ((TabControls.Count > 0) ? ((DXTabControl)(object)TabControls[0].Parent) : AddTabControl(null));
			LayoutGroup container = AddTabbedGroup(tc, ArmSoft.SME.Common.Constants.Extensions.DefaultTabCaption, ArmSoft.SME.Common.Constants.Extensions.DefaultTabName, "", FeatureStatusTypeSE.New);
			LayoutGroupExt container2 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Horizontal);
			_createUserDefinedFields = AddButton(container2, "layout_add16.svg", ArmSoft.SME.Common.Resources.CreateUserDefinedFields, "", "", isTabStop: false, System.Windows.HorizontalAlignment.Left, "", isNew: true, ArmSoft.SME.Common.Resources.UDFieldsCreationExplantation);
			_createUserDefinedFields.Click += CreateUserDefinedFields_Click;
		}
		if (ExtensionDefinition != null)
		{
			ConstructExtensionUI(ExtensionDefinition);
			if (TabControls.Count > 0)
			{
				ConstructExtensionTabbedUI(ExtensionDefinition, (DXTabControl)(object)TabControls[0].Parent);
			}
		}
		RestoreSettings(WidthValue, HeightValue);
		if (_ribbonPageGroupReports != null && _ribbonPageGroupReports.ItemLinks.Count > 0)
		{
			AddNewItemOnRibbonPageOther(_ribbonPageGroupReports, 0);
		}
		if (_ribbonPageGroupActions != null && _ribbonPageGroupActions.ItemLinks.Count > 0)
		{
			AddNewItemOnRibbonPageOther(_ribbonPageGroupActions, 0);
		}
		if (StoredInDb)
		{
			if (_viewMode == WindowViewMode.View)
			{
				OnViewUpper();
			}
			else
			{
				OnEditUpper();
			}
		}
		else if (_viewMode == WindowViewMode.View)
		{
			OnViewUpper();
		}
		else
		{
			OnNewUpper();
		}
		if (!disableGridValidators)
		{
			PropertyValidator.AddDocumentGridStandardValidators(Definition, this);
		}
		else
		{
			PropertyValidator.RemoveDocumentGridValidators(Definition, this);
			for (int i = 0; i <= Controls.Count - 1; i++)
			{
				if (Controls[i].Control is DocumentGridControl documentGridControl)
				{
					documentGridControl.CalculateHasValidationError(out var _, validateWithoutRequired: true, validateAgain: true);
				}
			}
		}
		RefreshRequiredGrids();
		SetEvents();
		_restoringSettings = true;
		try
		{
			base.Show();
		}
		finally
		{
			_restoringSettings = false;
		}
	}

	public Dictionary<string, string> GetTabs()
	{
		if (TabControls == null)
		{
			ConstructUI();
			if (Definition.IsExtendable == POExtendableType.Simple)
			{
				DXTabControl tc = ((TabControls.Count > 0) ? ((DXTabControl)(object)TabControls[0].Parent) : AddTabControl(null));
				LayoutGroup container = AddTabbedGroup(tc, ArmSoft.SME.Common.Constants.Extensions.DefaultTabCaption, ArmSoft.SME.Common.Constants.Extensions.DefaultTabName, "");
				AddInfoMessageControl(container, ArmSoft.SME.Common.Resources.UDFieldsCreationExplantation, "information_32.svg");
				_createUserDefinedFields = AddButton(container, "layout_add16.svg", ArmSoft.SME.Common.Resources.CreateUserDefinedFields);
				_createUserDefinedFields.Click += CreateUserDefinedFields_Click;
			}
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (DXTabItem tabControl in TabControls)
		{
			dictionary.Add(tabControl.Name, tabControl.Header.ToString());
		}
		return dictionary;
	}

	public bool? ShowDialog(WindowViewMode? viewMode = null)
	{
		WindowViewMode valueOrDefault = viewMode.GetValueOrDefault();
		if (!viewMode.HasValue)
		{
			valueOrDefault = ((!StoredInDb) ? WindowViewMode.NewCreate : WindowViewMode.Edit);
			viewMode = valueOrDefault;
		}
		_viewMode = viewMode.Value;
		_isReadOnly = _viewMode == WindowViewMode.View;
		_isDialog = true;
		ConstructUI();
		RestoreSettings(WidthValue, HeightValue);
		if (_ribbonPageGroupReports != null)
		{
			AddNewItemOnRibbonPageOther(_ribbonPageGroupReports, 0);
		}
		if (_ribbonPageGroupActions != null)
		{
			AddNewItemOnRibbonPageOther(_ribbonPageGroupActions, 0);
		}
		if (StoredInDb)
		{
			if (_viewMode == WindowViewMode.View)
			{
				OnViewUpper();
			}
			else
			{
				OnEditUpper();
			}
		}
		else if (_viewMode == WindowViewMode.View)
		{
			OnViewUpper();
		}
		else
		{
			OnNewUpper();
		}
		PropertyValidator.AddDocumentGridStandardValidators(Definition, this);
		RefreshRequiredGrids();
		SetEvents();
		_restoringSettings = true;
		try
		{
			return base.ShowDialog();
		}
		finally
		{
			_restoringSettings = false;
		}
	}

	protected virtual void RefreshRequiredGrids()
	{
		for (int i = 0; i <= Controls.Count - 1; i++)
		{
			if (Controls[i].Control is DocumentGridControl documentGridControl)
			{
				documentGridControl.ConfigureButtons();
			}
		}
	}

	protected virtual void ConstructUI()
	{
		TabControls = new List<DXTabItem>();
		Controls = new List<ControlDescriptor>();
		_extensionManager = new ExtensionsManager();
		_ribbonPageHome.Caption = ArmSoft.SME.Common.Resources.Home;
		_ribbonPageSave.Caption = ArmSoft.SME.Common.Resources.Save;
		_ribbonPageExport.Caption = "";
		_ribbonPageGridSettings.Caption = ArmSoft.SME.Common.Resources.DocumentGridMode;
		_barButtonSave.Content = ArmSoft.SME.Common.Resources.Save;
		_barButtonSaveAndClose.Content = ArmSoft.SME.Common.Resources.SaveAndClose;
		_barButtonDraft.Content = ArmSoft.SME.Common.Resources.Draft;
		_barButtonHelp.Content = ArmSoft.SME.Common.Resources.Help;
		_barButtonReload.Content = ArmSoft.SME.Common.Resources.Reload;
		_ribbonAttachmentPage.Caption = string.Format(ArmSoft.SME.Common.Resources.Attachments, "...");
		_ribbonAttachmentPageGroup.Caption = string.Format(ArmSoft.SME.Common.Resources.Attachments, "...");
		_barButtonAttachmentFiles.Content = ArmSoft.SME.Common.Resources.Files;
		_barButtonAttachmentLinks.Content = ArmSoft.SME.Common.Resources.Links;
		_barButtonAttachmentDocs.Content = ArmSoft.SME.Common.Resources.Documents;
		SuperTip superTip = new SuperTip();
		SuperTipHeaderItem item = new SuperTipHeaderItem
		{
			Content = ArmSoft.SME.Common.Resources.SaveAndClose + " (Ctrl+Enter)"
		};
		superTip.Items.Add(item);
		_barButtonSaveAndClose.SuperTip = superTip;
		_barButtonSaveAndNew.Content = ArmSoft.SME.Common.Resources.SaveAndNew;
		_barButtonClose.Content = ArmSoft.SME.Common.Resources.Close;
		_barCheckDialogMode.Content = ArmSoft.SME.Common.Resources.DocumentGridModeDialog;
		_barCheckGridRowMode.Content = ArmSoft.SME.Common.Resources.DocumentGridModeRow;
		_barButtonExportXML.Content = ArmSoft.SME.Common.Resources.ExportToXML;
		_barButtonShowISN.Content = ArmSoft.SME.Common.Resources.ISN;
		if (!IsOfflineDoc)
		{
			SetupPrinting();
		}
		if (!AllowChangeGridMode())
		{
			_barCheckDialogMode.IsVisible = false;
			_barCheckGridRowMode.IsVisible = false;
			_ribbonPageGridSettings.IsVisible = false;
		}
		else
		{
			_barCheckDialogMode.IsThreeState = false;
			_barCheckGridRowMode.IsThreeState = false;
		}
		if (_isDialog)
		{
			_barButtonSaveAndNew.IsVisible = false;
		}
		else
		{
			_barButtonSaveAndNew.IsVisible = AllowNewAfterSave();
		}
		_barButtonDraft.IsVisible = AllowDraft();
		_barButtonAttachmentFiles.IsVisible = Definition.AttachmentsTypes.HasFlag(AttachType.File);
		_barButtonAttachmentLinks.IsVisible = Definition.AttachmentsTypes.HasFlag(AttachType.Link);
		_barButtonAttachmentDocs.IsVisible = Definition.AttachmentsTypes.HasFlag(AttachType.Doc);
		_ribbonAttachmentPageGroup.IsVisible = _barButtonAttachmentFiles.IsVisible || _barButtonAttachmentLinks.IsVisible || _barButtonAttachmentDocs.IsVisible;
		_ribbonAttachmentPage.IsVisible = _ribbonAttachmentPageGroup.IsVisible;
		_barItemAttachments.IsVisible = _ribbonAttachmentPageGroup.IsVisible;
		AddItemsToRibbon();
		SetReportBarButtonsProperties();
		Ribbon.IsMinimized = _ribbonIsMinimized;
		Ribbon.ToolbarShowMode = ((!_ribbonToolbarShowAbove) ? RibbonQuickAccessToolbarShowMode.ShowBelow : RibbonQuickAccessToolbarShowMode.Default);
		MinWidth = SizeCalculator.Width(MinWidthValue);
		MinHeight = SizeCalculator.Height(MinHeightValue);
		_registryKey = PO.POName;
		Icon = Settings.HostingEnvironment.Icon;
	}

	protected virtual void SetControlsReadonly()
	{
		if (_viewMode == WindowViewMode.View)
		{
			SetControlsReadonly(value: true);
			if (IsOfflineDoc)
			{
				_ribbonPageHome.IsEnabled = false;
				_ribbonPageOther.IsEnabled = false;
				_ribbonAttachmentPage.IsEnabled = false;
				_ribbonPageGroupActions.IsEnabled = false;
				_ribbonStatusBar.IsEnabled = false;
				Ribbon.ToolbarShowMode = RibbonQuickAccessToolbarShowMode.Hide;
			}
			SetExtensionUIReadOnly(isReadOnly: true);
		}
		if (_viewMode == WindowViewMode.NewAfterSave)
		{
			SetControlsReadonly(value: false);
		}
	}

	protected void SetControlsReadonly(bool value)
	{
		foreach (ControlDescriptor control in Controls)
		{
			SetControlReadonly(control.Control, value);
		}
	}

	protected virtual void SetControlReadonly(Control control, bool value)
	{
		if (control is TextEditExt textEditExt)
		{
			textEditExt.IsReadOnly = value;
		}
		else if (control is ButtonEditExt buttonEditExt)
		{
			buttonEditExt.IsReadOnly = value;
		}
		else if (control is DropDownControl dropDownControl)
		{
			dropDownControl.IsReadOnly = value;
		}
		else if (control is DateEditControl dateEditControl)
		{
			dateEditControl.IsReadOnly = value;
		}
		else if (control is TimeEditControl timeEditControl)
		{
			timeEditControl.IsReadOnly = value;
		}
		else if (control is PopupCalcEditExt popupCalcEditExt)
		{
			popupCalcEditExt.IsReadOnly = value;
		}
		else if (control is CheckEditExt checkEditExt)
		{
			checkEditExt.IsReadOnly = value;
		}
		else if (control is MultiSelectDropDownControl multiSelectDropDownControl)
		{
			multiSelectDropDownControl.IsReadOnly = value;
		}
		else if (control is DropDownCommentControl dropDownCommentControl)
		{
			dropDownCommentControl.IsReadOnly = value;
		}
		else if (control is ComboBoxEditExt comboBoxEditExt)
		{
			comboBoxEditExt.IsReadOnly = value;
		}
		else if (control is ExchangeRateControl exchangeRateControl)
		{
			exchangeRateControl.IsReadOnly = value;
		}
		else if (control is MemoEditControl memoEditControl)
		{
			memoEditControl.IsReadOnly = value;
		}
		else if (control is DocumentGridControl documentGridControl)
		{
			documentGridControl.IsReadOnly = value;
		}
	}

	private void LoadDataToControlsWithStatusChange()
	{
		try
		{
			IsLoadingDataToControls = true;
			LoadDataToControls();
			IsLoadingDataToControls = false;
		}
		finally
		{
			IsLoadingDataToControls = false;
		}
	}

	protected virtual void LoadDataToControls()
	{
		foreach (ControlDescriptor control in Controls)
		{
			if (!string.IsNullOrWhiteSpace(control.DataItemName) && Definition.Fields.ContainsKey(control.DataItemName))
			{
				SetControlValue(control.Control, GetPropertyValue(PO, control.DataItemName));
			}
		}
		_extensionManager.LoadDataToExtensionControls(Extension);
	}

	protected virtual void SetControlValue(Control control, object value)
	{
		ControlsHelper.SetControlValue(control, value);
	}

	protected virtual object GetControlValue(Control control)
	{
		return ControlsHelper.GetControlValue(control);
	}

	protected virtual void FillDocument(POStoreRequest sr)
	{
		foreach (ControlDescriptor control in Controls)
		{
			if (!string.IsNullOrWhiteSpace(control.DataItemName) && Definition.Fields.ContainsKey(control.DataItemName))
			{
				SetPropertyValue(PO, control, GetControlValue(control.Control));
			}
		}
		_extensionManager.FillExtension(Extension);
	}

	public static object GetPropertyValue(object src, string propName)
	{
		if (propName.IndexOf('.') > 0)
		{
			string[] array = propName.Split('.');
			object value = src.GetType().GetProperty(array[0]).GetValue(src, null);
			return value.GetType().GetProperty(array[1]).GetValue(value, null);
		}
		return src.GetType().GetProperty(propName).GetValue(src, null);
	}

	private static void SetPropertyValue(object src, ControlDescriptor controlDescriptor, object value)
	{
		string name;
		object obj;
		if (controlDescriptor.DataItemName.IndexOf('.') > 0)
		{
			string[] array = controlDescriptor.DataItemName.Split('.');
			name = array[1];
			obj = src.GetType().GetProperty(array[0]).GetValue(src, null);
		}
		else
		{
			name = controlDescriptor.DataItemName;
			obj = src;
		}
		PropertyInfo property = obj.GetType().GetProperty(name);
		if (controlDescriptor.Control is DropDownControl || controlDescriptor.Control is PopupCalcEditExt)
		{
			if (property.PropertyType.Name.Contains("Nullable"))
			{
				property.SetValue(obj, value);
			}
			else
			{
				property.ASSetValue(obj, Convert.ToString(value));
			}
		}
		else
		{
			property.SetValue(obj, value);
		}
	}

	protected void ConstructExtensionUI(ArmSoft.SME.Core.UI.MetaData.DocumentExtensionDefinition documentExtensionDef)
	{
		if (documentExtensionDef == null)
		{
			return;
		}
		foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in documentExtensionDef.Elements)
		{
			AddExtensionUIElement(element, null);
		}
	}

	protected void ConstructExtensionTabbedUI(ArmSoft.SME.Core.UI.MetaData.DocumentExtensionDefinition documentExtensionDef, DXTabControl tabControl)
	{
		if (documentExtensionDef == null || documentExtensionDef.Tabs.Count <= 0)
		{
			return;
		}
		if (tabControl == null)
		{
			tabControl = AddTabControl(null);
		}
		foreach (UITab tab in documentExtensionDef.Tabs)
		{
			LayoutGroup layoutGroup = null;
			foreach (DXTabItem tabControl2 in TabControls)
			{
				if (!string.IsNullOrWhiteSpace(tabControl2.Name) && !string.IsNullOrWhiteSpace(tab.CaptionId) && tabControl2.Name.Trim() == tab.CaptionId.Trim())
				{
					layoutGroup = (LayoutControl)tabControl2.Content;
					break;
				}
			}
			if (layoutGroup == null)
			{
				layoutGroup = AddTabbedGroup(tabControl, tab.Caption);
			}
			foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in tab.Elements)
			{
				AddExtensionUIElement(element, layoutGroup);
			}
		}
	}

	private void AddExtensionUIElement(ArmSoft.SME.Core.UI.MetaData.UIElement headerUIElement, LayoutGroup headerLayoutGroup)
	{
		if (headerUIElement is UIGroup uIGroup)
		{
			LayoutGroup layoutGroup = AddLayoutGroup(headerLayoutGroup, (!string.IsNullOrWhiteSpace(uIGroup.Caption)) ? LayoutGroupView.GroupBox : LayoutGroupView.Group, (!uIGroup.IsHorizontal) ? Orientation.Vertical : Orientation.Horizontal, uIGroup.Caption);
			layoutGroup.IsCollapsible = uIGroup.IsCollapsible;
			layoutGroup.IsCollapsed = uIGroup.IsCollapsed;
			{
				foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in uIGroup.Elements)
				{
					AddExtensionUIElement(element, layoutGroup);
				}
				return;
			}
		}
		if (headerUIElement is UIField uf)
		{
			AddExtensionFieldControl(uf, headerLayoutGroup);
		}
	}

	private void AddExtensionFieldControl(UIField uf, LayoutGroup lg)
	{
		DataItem dataItem = uf.DataItem;
		System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left;
		switch (uf.HorizontalAlignment)
		{
		case ArmSoft.SME.Core.UI.MetaData.HorizontalAlignment.Stretch:
			horizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			break;
		case ArmSoft.SME.Core.UI.MetaData.HorizontalAlignment.Left:
			horizontalAlignment = System.Windows.HorizontalAlignment.Left;
			break;
		case ArmSoft.SME.Core.UI.MetaData.HorizontalAlignment.Right:
			horizontalAlignment = System.Windows.HorizontalAlignment.Right;
			break;
		case ArmSoft.SME.Core.UI.MetaData.HorizontalAlignment.Center:
			horizontalAlignment = System.Windows.HorizontalAlignment.Center;
			break;
		}
		string caption;
		if (uf.HideCaption)
		{
			caption = string.Empty;
		}
		else
		{
			caption = ((!string.IsNullOrWhiteSpace(uf.Caption)) ? uf.Caption : dataItem.Caption);
		}
		if (dataItem is StringDataItem stringDataItem)
		{
			if (uf.DropDownItem == null)
			{
				TextEditExt textEditExt = AddStringControl(lg, caption, stringDataItem.Required, stringDataItem.Length, horizontalAlignment);
				if (stringDataItem.LeftAlign)
				{
					textEditExt.EditValueChanged += CommonEventHandlers.TrimEditValueChanged;
				}
				_extensionManager.AddControlToExtensionsList(textEditExt, dataItem, uf.ReadOnlySource);
				return;
			}
			IDropDownView dropDownView = Settings.ServiceProvider.GetRequiredService<IDropDownService>().CreateDropDown(uf.DropDownItem.Group);
			if (uf.DropDownItem.Group != uf.DropDownItem.Name)
			{
				ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dataViewDefinition = Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(uf.DropDownItem.Group, uf.DropDownItem.Name);
				if (dataViewDefinition != null && !dataViewDefinition.IsDefault)
				{
					dropDownView.ApplyDataViewDefinition(dataViewDefinition);
				}
			}
			if (uf.DropDownItem.IsDropDownComment)
			{
				DropDownCommentControl dropDownCommentControl = AddDropDownCommentControl(lg, caption, stringDataItem.Required, dropDownView, stringDataItem.Length, horizontalAlignment);
				dropDownCommentControl.IsTextEditable = uf.DropDownItem.IsTextEditable;
				_extensionManager.AddControlToExtensionsList(dropDownCommentControl, dataItem, uf.ReadOnlySource);
			}
			else if (uf.DropDownItem.IsMultiSelect)
			{
				MultiSelectDropDownControl myControl = AddMultiSelectDropDownControl(lg, caption, stringDataItem.Required, dropDownView, horizontalAlignment);
				_extensionManager.AddControlToExtensionsList(myControl, dataItem, uf.ReadOnlySource);
			}
			else
			{
				DropDownControl dropDownControl = AddDropDownControl(lg, caption, stringDataItem.Required, dropDownView, uf.DropDownItem.AllowAdditionButtons, horizontalAlignment);
				dropDownControl.CheckValue = uf.DropDownItem.CheckValue;
				dropDownControl.ShowComment = uf.DropDownItem.ShowComment;
				_extensionManager.AddControlToExtensionsList(dropDownControl, dataItem, uf.ReadOnlySource);
			}
		}
		else if (dataItem is TreeNodeDataItem treeNodeDataItem)
		{
			TreeNodes idv = new TreeNodes(treeNodeDataItem.TreeName);
			if (uf.DropDownItem != null && uf.DropDownItem.IsDropDownComment)
			{
				DropDownCommentControl dropDownCommentControl2 = AddDropDownCommentControl(lg, caption, treeNodeDataItem.Required, idv, treeNodeDataItem.Tree.CaptionLen, horizontalAlignment);
				dropDownCommentControl2.IsTextEditable = uf.DropDownItem.IsTextEditable;
				_extensionManager.AddControlToExtensionsList(dropDownCommentControl2, dataItem, uf.ReadOnlySource);
				return;
			}
			if (uf.DropDownItem != null && uf.DropDownItem.IsMultiSelect)
			{
				MultiSelectDropDownControl myControl2 = AddMultiSelectDropDownControl(lg, caption, treeNodeDataItem.Required, idv, horizontalAlignment);
				_extensionManager.AddControlToExtensionsList(myControl2, dataItem, uf.ReadOnlySource);
				return;
			}
			DropDownControl dropDownControl2 = AddDropDownControl(lg, caption, treeNodeDataItem.Required, idv, treeNodeDataItem.AllowAdditionButtons, horizontalAlignment);
			if (uf.DropDownItem != null)
			{
				dropDownControl2.CheckValue = uf.DropDownItem.CheckValue;
				dropDownControl2.ShowComment = uf.DropDownItem.ShowComment;
			}
			_extensionManager.AddControlToExtensionsList(dropDownControl2, dataItem, uf.ReadOnlySource);
		}
		else if (dataItem is NumberDataItem numberDataItem)
		{
			PopupCalcEditExt myControl3 = AddNumericControl(lg, caption, numberDataItem.Required, numberDataItem.Length, numberDataItem.Precision, numberDataItem.AllowNegativeNumbers, numberDataItem.UseCurrencyFormat, horizontalAlignment);
			_extensionManager.AddControlToExtensionsList(myControl3, dataItem, uf.ReadOnlySource);
		}
		else if (dataItem is DateDataItem dateDataItem)
		{
			DateEditControl myControl4 = AddDateControl(lg, caption, dateDataItem.Required, dateDataItem.UseLongDate, horizontalAlignment);
			_extensionManager.AddControlToExtensionsList(myControl4, dataItem, uf.ReadOnlySource);
		}
		else if (dataItem is TimeDataItem timeDataItem)
		{
			TimeEditControl myControl5 = AddTimeControl(lg, caption, timeDataItem.Required, timeDataItem.ShowSeconds, horizontalAlignment);
			_extensionManager.AddControlToExtensionsList(myControl5, dataItem, uf.ReadOnlySource);
		}
		else if (dataItem is BooleanDataItem)
		{
			CheckEditExt myControl6 = AddBooleanControl(lg, caption, horizontalAlignment);
			_extensionManager.AddControlToExtensionsList(myControl6, dataItem, uf.ReadOnlySource);
		}
	}

	protected void SetExtensionUIReadOnly(bool isReadOnly)
	{
		_extensionManager.SetExtensionUIReadOnly(isReadOnly);
	}

	protected void LoadDataToExtensionControls()
	{
		if (ExtensionDefinition != null)
		{
			_extensionManager.LoadDataToExtensionControls(Extension);
		}
	}

	protected void SetExtensionDefaultValues()
	{
		_extensionManager.SetExtensionDefaultValues();
	}

	protected void SetExtensionValues()
	{
		_extensionManager.SetExtensionValues(Extension);
	}

	private void SetupPrinting()
	{
		_barButtonExportXML.IsVisible = AllowExportToXml;
		_ribbonPagePrint.Caption = ArmSoft.SME.Common.Resources.Print;
		_ribbonPagePrintTemplate.Caption = ArmSoft.SME.Common.Resources.PrintTemplate;
		_barButtonPrint.Content = ArmSoft.SME.Common.Resources.Print;
		_barButtonPrintPreview.Content = ArmSoft.SME.Common.Resources.View;
		_barButtonExcel.Content = ArmSoft.SME.Common.Resources.ExportToExcel;
		_barButtonWord.Content = ArmSoft.SME.Common.Resources.ExportToWord;
		_barButtonPdf.Content = ArmSoft.SME.Common.Resources.ExportToPDF;
		_barSubItemExport.Content = ArmSoft.SME.Common.Resources.Export;
		if (AllowPrint())
		{
			_printTemplates = new BindingList<TemplatesCacheRow>();
			foreach (TemplatesCacheRow item in PrintTemplates())
			{
				if (item.FileFormat == PrintTemplateFileFormatSE.XML || item.FileFormat == PrintTemplateFileFormatSE.DOC || item.FileFormat == PrintTemplateFileFormatSE.DOCX || item.FileFormat == PrintTemplateFileFormatSE.DOCM)
				{
					_printTemplates.Add(item);
				}
			}
			if (_printTemplates.Count == 0)
			{
				_barEditCurrentTemplate.IsVisible = false;
				_printTemplate = null;
				_ribbonPagePrintTemplate.IsVisible = false;
				return;
			}
			if (_printTemplates.Count == 1)
			{
				_barEditCurrentTemplate.IsVisible = false;
				_printTemplate = _printTemplates[0];
				_ribbonPagePrintTemplate.IsVisible = false;
				return;
			}
			_printTemplate = DefaultPrintTemplate();
			_printTemplate = _printTemplates.FirstOrDefault((TemplatesCacheRow p) => p.Name == _printTemplate.Name);
			_currentTemplateComboEdit.ItemsSource = _printTemplates;
			_barEditCurrentTemplate.EditValue = _printTemplate;
		}
		else
		{
			_barEditCurrentTemplate.IsVisible = false;
			_barButtonPrint.IsVisible = false;
			_barButtonPrintPreview.IsVisible = false;
			_barButtonWord.IsVisible = false;
			_barButtonExcel.IsVisible = false;
			_barButtonPdf.IsVisible = false;
			_ribbonPagePrint.IsVisible = false;
			_ribbonPageExport.IsVisible = AllowExportToXml;
			_ribbonPagePrintTemplate.IsVisible = false;
		}
	}

	protected void RestoreSettings(double defaultWidth, double defaultHeight)
	{
		bool flag = false;
		if (string.IsNullOrWhiteSpace(_registryKey))
		{
			flag = true;
		}
		else
		{
			try
			{
				_restoringSettings = true;
				using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"Document Windows"}\\{_registryKey}", writable: false);
				if (registryKey == null)
				{
					flag = true;
				}
				else
				{
					RestoreSettings(defaultWidth, defaultHeight, registryKey);
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
			RestoreSettings(defaultWidth, defaultHeight, null);
			_restoringSettings = false;
			GridMode = true;
		}
	}

	protected virtual void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		if (rk == null)
		{
			Width = SizeCalculator.Width(defaultWidth);
			Height = SizeCalculator.Height(defaultHeight);
			Ribbon.ToolbarShowMode = RibbonQuickAccessToolbarShowMode.Default;
			Ribbon.IsMinimized = true;
		}
		else
		{
			Settings.RestoreWindowSize(this, rk, defaultWidth, defaultHeight);
			bool isMinimized = rk.GetValue("RibbonMinimized", "1").ToString() == "1";
			bool isSimplified = rk.GetValue("RibbonSimplified", "1").ToString() == "1";
			bool flag = rk.GetValue("RibbonToolbarMode", "1").ToString() == "1";
			bool flag2 = rk.GetValue("State", "1").ToString() == "0";
			GridMode = rk.GetValue("GridMode", "1").ToString() == "1";
			if (flag2)
			{
				WindowState = WindowState.Maximized;
			}
			else
			{
				WindowState = WindowState.Normal;
			}
			Ribbon.ToolbarShowMode = ((!flag) ? RibbonQuickAccessToolbarShowMode.ShowBelow : RibbonQuickAccessToolbarShowMode.Default);
			Ribbon.IsMinimized = isMinimized;
			Ribbon.IsSimplified = isSimplified;
		}
		SetCurrentTemplate();
	}

	protected void StoreSettings()
	{
		if (string.IsNullOrWhiteSpace(_registryKey))
		{
			return;
		}
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\Document Windows\\" + _registryKey);
			if (registryKey != null)
			{
				StoreSettings(registryKey);
			}
		}
		catch (Exception)
		{
		}
	}

	protected virtual void StoreSettings(RegistryKey rk)
	{
		if (WindowState == WindowState.Normal)
		{
			rk.SetValue("Width", Width.ToString());
			rk.SetValue("Height", Height.ToString());
			rk.SetValue("State", "1");
		}
		else if (WindowState == WindowState.Maximized)
		{
			rk.SetValue("State", "0");
		}
		rk.SetValue("RibbonMinimized", Ribbon.IsMinimized ? "1" : "0");
		rk.SetValue("RibbonSimplified", Ribbon.IsSimplified ? "1" : "0");
		rk.SetValue("RibbonToolbarMode", (Ribbon.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.Default) ? "1" : "0");
		rk.SetValue("GridMode", GridMode ? "1" : "0");
	}

	protected virtual void AfterStore(POStoreRequest sr, bool isNewDocument)
	{
	}

	protected void UpdateDataViewPanel(int dvpId)
	{
		DataViewPanel dataViewPanel = Settings.HostingEnvironment.FindDataViewPanel(dvpId);
		if (dataViewPanel != null && dataViewPanel.Id != -1)
		{
			UpdateDataViewPanel(dataViewPanel);
		}
	}

	protected virtual void UpdateDataViewPanel(DataViewPanel dvp)
	{
	}

	protected virtual Control ControlByName(string name)
	{
		return Controls.Find((ControlDescriptor p) => p.DataItemName == name)?.Control;
	}

	protected void MakeWindowTitle()
	{
		_barButtonSave.IsEnabled = !_isReadOnly;
		_barButtonSaveAndClose.IsEnabled = !_isReadOnly;
		_barButtonSaveAndNew.IsEnabled = !_isReadOnly && AllowNewAfterSave();
		_barButtonDraft.IsEnabled = !_isReadOnly && AllowDraft();
		_barButtonReload.IsEnabled = StoredInDb;
		_barItemAttachments.IsEnabled = StoredInDb;
		_barButtonExportXML.IsEnabled = StoredInDb && AllowExportToXml;
		_barButtonAttachmentFiles.IsEnabled = _barItemAttachments.IsEnabled && Definition.AttachmentsTypes.HasFlag(AttachType.File);
		_barButtonAttachmentLinks.IsEnabled = _barItemAttachments.IsEnabled && Definition.AttachmentsTypes.HasFlag(AttachType.Link);
		_barButtonAttachmentDocs.IsEnabled = _barItemAttachments.IsEnabled && Definition.AttachmentsTypes.HasFlag(AttachType.Doc);
		IsChanged = !IsChanged;
		IsChanged = !IsChanged;
		if (StoredInDb)
		{
			StatusBarDocumentState.Content = SmartEnumHelper.GetPartialSmartEnumCaption(typeof(DocumentStateSE), Convert.ToString(State));
		}
		else
		{
			StatusBarDocumentState.Content = "";
		}
		if (AllowPrint() && _printTemplate != null)
		{
			bool isEnabled = PO.StoredInDb || AllowPrintOnNew();
			_barButtonPrint.IsEnabled = isEnabled;
			_barButtonPrintPreview.IsEnabled = isEnabled;
			_barButtonWord.IsEnabled = isEnabled;
			_barButtonExcel.IsEnabled = isEnabled;
			_barButtonPdf.IsEnabled = isEnabled;
		}
		SetReportBarButtonsEnable(PO.StoredInDb);
		AddToolbarItems();
	}

	protected virtual void OnNew()
	{
		LoadDataToControlsWithStatusChange();
	}

	private void OnNewUpper()
	{
		OnNew();
		MakeWindowTitle();
		SetExtensionValues();
		SetControlsReadonly();
	}

	protected virtual void OnEdit()
	{
		LoadDataToControlsWithStatusChange();
	}

	private void OnEditUpper()
	{
		OnEdit();
		MakeWindowTitle();
		SetControlsReadonly();
	}

	protected virtual void OnNewAfterSave()
	{
	}

	public void OnNewAfterSaveUpper()
	{
		if (PO.ISN != Guid.Empty)
		{
			DocumentGridRow.RemoveDocWindow(PO.ISN);
		}
		OnNewAfterSave();
		MakeWindowTitle();
		SetExtensionDefaultValues();
		SetControlsReadonly();
	}

	protected virtual void OnView()
	{
		LoadDataToControlsWithStatusChange();
	}

	private void OnViewUpper()
	{
		OnView();
		MakeWindowTitle();
		SetControlsReadonly();
	}

	protected virtual void OnPrintPreview(TemplatesCacheRow template, PrintMode mode)
	{
		BeforePrintPreview(template, mode);
	}

	protected virtual void BeforePrintPreview(TemplatesCacheRow template, PrintMode mode)
	{
	}

	protected virtual bool AllowPrint()
	{
		return false;
	}

	protected virtual bool AllowPrintOnNew()
	{
		return false;
	}

	protected virtual bool AllowDraft()
	{
		return false;
	}

	protected virtual bool AllowNewAfterSave()
	{
		return true;
	}

	protected virtual IReadOnlyList<TemplatesCacheRow> PrintTemplates()
	{
		return Settings.CoreCacheService.Template.GetTemplates(Definition.POType);
	}

	protected virtual TemplatesCacheRow DefaultPrintTemplate()
	{
		return Settings.CoreCacheService.Template.GetDefaultPrintTemplate(Definition.POType);
	}

	protected virtual void OnF2()
	{
	}

	protected virtual void OnF3()
	{
	}

	protected virtual void OnF4()
	{
	}

	protected virtual void OnF6()
	{
	}

	protected virtual void OnF7()
	{
	}

	protected virtual void OnF8()
	{
	}

	protected virtual void OnF9()
	{
	}

	protected virtual void OnCtrlF9()
	{
	}

	protected virtual void OnShiftF9()
	{
	}

	protected virtual void OnShiftF8()
	{
	}

	protected virtual void OnF11()
	{
	}

	protected virtual void OnCtrlF11()
	{
	}

	protected virtual void OnCtrl1()
	{
	}

	public virtual void OnScanner(string scannerData)
	{
	}

	public virtual void OnMagneticCardReader(string mcrData)
	{
	}

	protected virtual bool AllowChangeGridMode()
	{
		return false;
	}

	public TextEditExt AddStringControl(LayoutGroup container, FieldDefinition definition, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddStringControl(container, stringFieldDefinition.Caption, stringFieldDefinition.IsRequired, stringFieldDefinition.Length, horizontalAlignment, stringFieldDefinition.Name, stringFieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	public TextEditExt AddStringControl(LayoutGroup container, string caption, bool isRequired, short length, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		TextEditExt textEditExt = new TextEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, textEditExt, horizontalAlignment, isNew, helpText);
		AddControl(textEditExt, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(textEditExt, li);
		textEditExt.IsRequired = GetIsRequired(name, isRequired);
		textEditExt.MaxLength = length;
		textEditExt.MinWidth = 5.0;
		if (horizontalAlignment != System.Windows.HorizontalAlignment.Stretch)
		{
			textEditExt.Width = SizeCalculator.WidthForString(length);
		}
		TabValidationHelper.RegisterControl(textEditExt);
		return textEditExt;
	}

	private bool GetIsRequired(string name, bool isSystemRequired)
	{
		if (isSystemRequired)
		{
			return true;
		}
		DocumentValidatorExpressionRow documentValidatorExpressionRow = Settings.CoreCacheService.DocumentExtensions.GetItem(new DocumentExtensionKey(Definition.POTypeDef, Definition.POType))?.DocumentValidators?.FirstOrDefault((DocumentValidatorExpressionRow p) => p.FieldName == name);
		if (documentValidatorExpressionRow != null && (string.IsNullOrWhiteSpace(documentValidatorExpressionRow.UserIds) || documentValidatorExpressionRow.UserIds.Split(',').Any((string el) => int.TryParse(el, out var result) && result == Settings.LoginService.UserId) != documentValidatorExpressionRow.Except))
		{
			return documentValidatorExpressionRow.IsRequired;
		}
		return false;
	}

	public ButtonEditExt AddButtonEditControl(LayoutGroup container, FieldDefinition definition, short estimatedButtonsCount = 1, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddButtonEditControl(container, stringFieldDefinition.Caption, stringFieldDefinition.IsRequired, stringFieldDefinition.Length, estimatedButtonsCount, horizontalAlignment, stringFieldDefinition.Name, stringFieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	public ButtonEditExt AddButtonEditControl(LayoutGroup container, string caption, bool isRequired, short length, short estimatedButtonsCount = 1, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		ButtonEditExt buttonEditExt = new ButtonEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, buttonEditExt, horizontalAlignment, isNew, helpText);
		AddControl(buttonEditExt, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(buttonEditExt, li);
		buttonEditExt.IsRequired = GetIsRequired(name, isRequired);
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

	public ButtonInfo AddButtonInfoToButtonEdit(ButtonEditExt buttonEdit, GlyphKind glyph, string tooltip = "", object content = null)
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

	public MemoEditControl AddMemoControl(LayoutGroup container, FieldDefinition definition, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddMemoControl(container, stringFieldDefinition.Caption, stringFieldDefinition.IsRequired, stringFieldDefinition.Length, horizontalAlignment, stringFieldDefinition.Name, stringFieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	public MemoEditControl AddMemoControl(LayoutGroup container, string caption, bool isRequired, short length, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		MemoEditControl memoEditControl = new MemoEditControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, memoEditControl, horizontalAlignment, isNew, helpText);
		AddControl(memoEditControl, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(memoEditControl, li);
		memoEditControl.IsRequired = GetIsRequired(name, isRequired);
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

	public DateEditControl AddDateControl(LayoutGroup container, FieldDefinition definition, bool useLongDate = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		return AddDateControl(container, definition.Caption, definition.IsRequired, useLongDate, horizontalAlignment, definition.Name, definition.Name.Replace('.', '_'), isNew, helpText);
	}

	public DateEditControl AddDateControl(LayoutGroup container, string caption, bool isRequired, bool useLongDate = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		DateEditControl dateEditControl = new DateEditControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, dateEditControl, horizontalAlignment, isNew, helpText);
		AddControl(dateEditControl, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(dateEditControl, li);
		dateEditControl.UseLongDate = useLongDate;
		dateEditControl.IsRequired = GetIsRequired(name, isRequired);
		dateEditControl.Width = SizeCalculator.WidthForDate(useLongDate);
		TabValidationHelper.RegisterControl(dateEditControl);
		dateEditControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ControlValueChanged).Handler;
		return dateEditControl;
	}

	public TimeEditControl AddTimeControl(LayoutGroup container, FieldDefinition definition, bool showSeconds = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		return AddTimeControl(container, definition.Caption, definition.IsRequired, showSeconds, horizontalAlignment, definition.Name, definition.Name.Replace('.', '_'), isNew, helpText);
	}

	public TimeEditControl AddTimeControl(LayoutGroup container, string caption, bool isRequired, bool showSeconds = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		TimeEditControl timeEditControl = new TimeEditControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, timeEditControl, horizontalAlignment, isNew, helpText);
		AddControl(timeEditControl, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(timeEditControl, li);
		timeEditControl.ShowSeconds = showSeconds;
		timeEditControl.IsRequired = GetIsRequired(name, isRequired);
		timeEditControl.Width = SizeCalculator.WidthForTime(showSeconds);
		TabValidationHelper.RegisterControl(timeEditControl);
		timeEditControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ControlValueChanged).Handler;
		return timeEditControl;
	}

	public ExchangeRateControl AddExchangeRateControl(LayoutGroup container, FieldDefinition definition, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		return AddExchangeRateControl(container, definition.Caption, definition.IsRequired, horizontalAlignment, definition.Name, definition.Name.Replace('.', '_'), isNew, helpText);
	}

	public ExchangeRateControl AddExchangeRateControl(LayoutGroup container, string caption, bool isRequired, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		ExchangeRateControl exchangeRateControl = new ExchangeRateControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, exchangeRateControl, horizontalAlignment, isNew, helpText);
		AddControl(exchangeRateControl, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(exchangeRateControl, li);
		exchangeRateControl.IsRequired = GetIsRequired(name, isRequired);
		TabValidationHelper.RegisterControl(exchangeRateControl.InnerBaseControl);
		TabValidationHelper.RegisterControl(exchangeRateControl.InnerRateControl);
		exchangeRateControl.EditValueChanged += new SafeEventHandler<RateEditValueChangedEventArgs>(ExchRateChanged).Handler;
		return exchangeRateControl;
	}

	public PopupCalcEditExt AddNumericControl(LayoutGroup container, FieldDefinition definition, bool useCurrencyFormat = true, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "", bool isNew = false, string helpText = "")
	{
		NumberFieldDefinition numberFieldDefinition = (NumberFieldDefinition)definition;
		if (string.IsNullOrWhiteSpace(name))
		{
			name = numberFieldDefinition.Name.Replace('.', '_');
		}
		if (numberFieldDefinition.PrecisionDefinedInParam)
		{
			return AddNumericControl(container, numberFieldDefinition.Caption, numberFieldDefinition.IsRequired, numberFieldDefinition.LengthFromParam(Settings.ParametersService), numberFieldDefinition.PrecisionFromParam(Settings.ParametersService), !numberFieldDefinition.IsNonNegative, useCurrencyFormat, horizontalAlignment, numberFieldDefinition.Name, numberFieldDefinition.Type, name, isNew, helpText);
		}
		return AddNumericControl(container, numberFieldDefinition.Caption, numberFieldDefinition.IsRequired, numberFieldDefinition.Length, numberFieldDefinition.Precision, !numberFieldDefinition.IsNonNegative, useCurrencyFormat, horizontalAlignment, numberFieldDefinition.Name, numberFieldDefinition.Type, name, isNew, helpText);
	}

	public PopupCalcEditExt AddNumericControl(LayoutGroup container, string caption, bool isRequired, short length, short precision = 0, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", Type editValueType = null, string name = "", bool isNew = false, string helpText = "")
	{
		PopupCalcEditExt popupCalcEditExt = new PopupCalcEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, popupCalcEditExt, horizontalAlignment, isNew, helpText);
		AddControl(popupCalcEditExt, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(popupCalcEditExt, li);
		popupCalcEditExt.IsRequired = GetIsRequired(name, isRequired);
		popupCalcEditExt.AllowNegativeNumbers = allowNegativeNumbers;
		popupCalcEditExt.Mask = (useCurrencyFormat ? DecimalExtension.GetCurrencyFormat(length, precision) : DecimalExtension.GetNumberFormat(length, precision));
		popupCalcEditExt.Precision = precision;
		popupCalcEditExt.Width = SizeCalculator.WidthForNumber(length);
		if (editValueType != null)
		{
			popupCalcEditExt.EditValueType = editValueType;
		}
		TabValidationHelper.RegisterControl(popupCalcEditExt);
		return popupCalcEditExt;
	}

	public CheckEditExt AddBooleanControl(LayoutGroup container, FieldDefinition definition, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isThreeState = false, bool isNew = false, string helpText = "")
	{
		return AddBooleanControl(container, definition.Caption, horizontalAlignment, isThreeState, definition.Name, definition.Name.Replace('.', '_'), isNew, helpText);
	}

	public CheckEditExt AddBooleanControl(LayoutGroup container, string caption, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isThreeState = false, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		CheckEditExt checkEditExt = new CheckEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, checkEditExt, horizontalAlignment, isNew, helpText);
		AddControl(checkEditExt, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsReadonlyBinding(checkEditExt, li);
		checkEditExt.Focusable = true;
		checkEditExt.IsChecked = false;
		checkEditExt.IsThreeState = isThreeState;
		checkEditExt.Checked += new SafeEventHandler<RoutedEventArgs>(CheckEditChanged).Handler;
		return checkEditExt;
	}

	public DropDownControl AddTreeDropDownControl(LayoutGroup container, FieldDefinition fieldDefinition, bool allowAdditionButtons = false, bool fieldWithCaption = true, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		TreeNodeFieldDefinition treeNodeFieldDefinition = (TreeNodeFieldDefinition)fieldDefinition;
		return AddDropDownControl(container, fieldWithCaption ? treeNodeFieldDefinition.Caption : "", treeNodeFieldDefinition.IsRequired, new TreeNodes(treeNodeFieldDefinition.TreeId, "", showAsTree: true, showExpanded: true, allowOperationWithAdditionButtons: true, treeNodeFieldDefinition.OnlyLeaf), allowAdditionButtons, horizontalAlignment, fieldDefinition.Name, fieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	public DropDownControl AddSmartEnumDropDownControl(LayoutGroup container, FieldDefinition fieldDefinition, bool allowAdditionButtons = false, bool fieldWithCaption = true, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		SmartEnumFieldDefinition smartEnumFieldDefinition = (SmartEnumFieldDefinition)fieldDefinition;
		return AddDropDownControl(container, fieldWithCaption ? smartEnumFieldDefinition.Caption : "", smartEnumFieldDefinition.IsRequired, new SmartEnum(smartEnumFieldDefinition.SmartEnumType, smartEnumFieldDefinition.Values, padLeft: false, '0', allSubTypes: false, smartEnumFieldDefinition.ValueMember), allowAdditionButtons, horizontalAlignment, fieldDefinition.Name, fieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	public DropDownControl AddDropDownControl(LayoutGroup container, FieldDefinition fieldDefinition, IDropDownView idv, bool allowAdditionButtons = false, bool fieldWithCaption = true, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		return AddDropDownControl(container, fieldWithCaption ? fieldDefinition.Caption : "", fieldDefinition.IsRequired, idv, allowAdditionButtons, horizontalAlignment, fieldDefinition.Name, fieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	public DropDownControl AddDropDownControl(LayoutGroup container, string caption, bool isRequired, IDropDownView idv, bool allowAdditionButtons = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		DropDownControl dropDownControl = new DropDownControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, dropDownControl, horizontalAlignment, isNew, helpText);
		AddControl(dropDownControl, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(dropDownControl, li);
		dropDownControl.IsRequired = GetIsRequired(name, isRequired);
		dropDownControl.AllowAdditionButtons = allowAdditionButtons;
		dropDownControl.DataView = idv;
		dropDownControl.Owner = this;
		dropDownControl.Code = "";
		TabValidationHelper.RegisterControl(dropDownControl.InnerCodeEdit);
		dropDownControl.BrowseEnd += DropDownBrowseEnd;
		dropDownControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DropDownValueChanged).Handler;
		if (horizontalAlignment == System.Windows.HorizontalAlignment.Stretch)
		{
			dropDownControl.InnerCommentEdit.MaxWidth = double.PositiveInfinity;
		}
		return dropDownControl;
	}

	public MultiSelectDropDownControl AddMultiSelectDropDownControl(LayoutGroup container, FieldDefinition fieldDefinition, IDropDownView idv, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		return AddMultiSelectDropDownControl(container, fieldDefinition.Caption, fieldDefinition.IsRequired, idv, horizontalAlignment, fieldDefinition.Name, fieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	public MultiSelectDropDownControl AddMultiSelectDropDownControl(LayoutGroup container, string caption, bool isRequired, IDropDownView idv, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		MultiSelectDropDownControl multiSelectDropDownControl = new MultiSelectDropDownControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, multiSelectDropDownControl, horizontalAlignment, isNew, helpText);
		AddControl(multiSelectDropDownControl, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(multiSelectDropDownControl, li);
		multiSelectDropDownControl.IsRequired = GetIsRequired(name, isRequired);
		multiSelectDropDownControl.DataView = idv;
		multiSelectDropDownControl.Owner = this;
		multiSelectDropDownControl.SelectedString = "";
		TabValidationHelper.RegisterControl(multiSelectDropDownControl.InnerCodeEdit);
		multiSelectDropDownControl.BrowseEnd += DropDownBrowseEnd;
		multiSelectDropDownControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DropDownValueChanged).Handler;
		return multiSelectDropDownControl;
	}

	public InfoMessageControl AddInfoMessageControl(LayoutGroup container, string msg, string icon, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, VerticalAlignment verticalAlignment = VerticalAlignment.Top, string name = "")
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		InfoMessageControl infoMessageControl = new InfoMessageControl
		{
			Name = name
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, infoMessageControl, horizontalAlignment);
		layoutItemWithRequired.VerticalAlignment = verticalAlignment;
		AddControl(infoMessageControl, "", layoutItemWithRequired, "", container);
		infoMessageControl.Message = msg;
		if (!string.IsNullOrWhiteSpace(icon))
		{
			ImageSource image = ImageService.GetImage(icon);
			infoMessageControl.ImageSource = image;
		}
		return infoMessageControl;
	}

	public Button AddButton(LayoutGroup container, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "", bool isNew = false, string helpText = "")
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		Button button = new Button
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, button, horizontalAlignment, isNew, helpText);
		AddControl(button, labelCaption, li, "", container);
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
			stackPanel.Orientation = Orientation.Horizontal;
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
		if (_addedButtons == null)
		{
			_addedButtons = new List<Button>();
		}
		_addedButtons.Add(button);
		return button;
	}

	public SimpleButton AddSimpleButton(LayoutGroup container, double glyphHeight, double glyphWidth, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, System.Windows.Controls.Dock glyphAlignment = System.Windows.Controls.Dock.Top, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, double glyphToContentOffset = 0.0, string name = "")
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		SimpleButton simpleButton = new SimpleButton
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, simpleButton, horizontalAlignment);
		AddControl(simpleButton, labelCaption, li, "", container);
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
		if (_addedButtons == null)
		{
			_addedButtons = new List<Button>();
		}
		_addedButtons.Add(simpleButton);
		return simpleButton;
	}

	public SplitButton AddSplitButton(LayoutGroup container, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, double imgWidth = 16.0, double imgHeight = 16.0, IEnumerable<IBarItem> popupItems = null, bool showGlyphsInPopupMenu = true, string name = "")
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		SplitButton splitButton = new SplitButton
		{
			Name = name,
			IsTabStop = isTabStop,
			ArrowPadding = new Thickness(2.0, 0.0, 2.0, 0.0)
		};
		if (!string.IsNullOrWhiteSpace(tooltip))
		{
			splitButton.ToolTip = tooltip;
		}
		if (string.IsNullOrWhiteSpace(imageURI))
		{
			splitButton.Content = caption;
		}
		else
		{
			StackPanel stackPanel = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Margin = new Thickness(-10.0, 0.0, -3.0, 0.0)
			};
			stackPanel.Children.Add(new DXImage
			{
				Source = ImageService.GetImage(imageURI),
				Width = imgWidth,
				Height = imgHeight
			});
			if (!string.IsNullOrWhiteSpace(caption))
			{
				stackPanel.Children.Add(new TextBlock
				{
					Text = caption,
					Margin = new Thickness(3.0, 0.0, 0.0, 0.0)
				});
			}
			splitButton.Content = stackPanel;
		}
		if (popupItems != null)
		{
			PopupMenu popupMenu = new PopupMenu();
			BarManager.SetShowGlyphsInPopupMenus((DependencyObject)(object)popupMenu, showGlyphsInPopupMenu);
			foreach (IBarItem popupItem in popupItems)
			{
				popupMenu.Items.Add(popupItem);
			}
			splitButton.PopupContent = popupMenu;
		}
		LayoutItemWithRequired li = AddLayoutItem(container, splitButton, horizontalAlignment);
		AddControl(splitButton, labelCaption, li, "", container);
		if (_addedButtons == null)
		{
			_addedButtons = new List<Button>();
		}
		_addedButtons.Add(splitButton);
		return splitButton;
	}

	public DropDownButton AddDropDownButton(LayoutGroup container, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, double imgWidth = 16.0, double imgHeight = 16.0, IEnumerable<IBarItem> popupItems = null, bool showGlyphsInPopupMenu = true)
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		DropDownButton dropDownButton = new DropDownButton
		{
			IsTabStop = isTabStop,
			ArrowPadding = new Thickness(2.0, 0.0, 2.0, 0.0)
		};
		if (!string.IsNullOrWhiteSpace(tooltip))
		{
			dropDownButton.ToolTip = tooltip;
		}
		if (string.IsNullOrWhiteSpace(imageURI))
		{
			dropDownButton.Content = caption;
		}
		else
		{
			StackPanel stackPanel = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Margin = new Thickness(-10.0, 0.0, -3.0, 0.0)
			};
			stackPanel.Children.Add(new DXImage
			{
				Source = ImageService.GetImage(imageURI),
				Width = imgWidth,
				Height = imgHeight
			});
			if (!string.IsNullOrWhiteSpace(caption))
			{
				stackPanel.Children.Add(new TextBlock
				{
					Text = caption,
					Margin = new Thickness(3.0, 0.0, 0.0, 0.0)
				});
			}
			dropDownButton.Content = stackPanel;
		}
		if (popupItems != null)
		{
			PopupMenu popupMenu = new PopupMenu();
			BarManager.SetShowGlyphsInPopupMenus((DependencyObject)(object)popupMenu, showGlyphsInPopupMenu);
			foreach (IBarItem popupItem in popupItems)
			{
				popupMenu.Items.Add(popupItem);
			}
			dropDownButton.PopupContent = popupMenu;
		}
		LayoutItemWithRequired li = AddLayoutItem(container, dropDownButton, horizontalAlignment);
		AddControl(dropDownButton, labelCaption, li, "", container);
		if (_addedButtons == null)
		{
			_addedButtons = new List<Button>();
		}
		_addedButtons.Add(dropDownButton);
		return dropDownButton;
	}

	protected string Get_ControlCaption(Control myControl)
	{
		foreach (ControlDescriptor control in Controls)
		{
			if (control.Control == myControl)
			{
				return control.Caption;
			}
		}
		throw new InvalidOperationException("Invalid control");
	}

	protected void Set_ControlCaption(Control myControl, string value)
	{
		foreach (ControlDescriptor control in Controls)
		{
			if (control.Control == myControl)
			{
				control.Caption = value;
				if (control.LayoutItem != null)
				{
					control.LayoutItem.Label = value;
				}
				return;
			}
		}
		throw new InvalidOperationException("Invalid control");
	}

	public DropDownCommentControl AddDropDownCommentControl(LayoutGroup container, FieldDefinition fieldDefinition, IDropDownView idv, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)fieldDefinition;
		return AddDropDownCommentControl(container, stringFieldDefinition.Caption, stringFieldDefinition.IsRequired, idv, stringFieldDefinition.Length, horizontalAlignment, stringFieldDefinition.Name, stringFieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	protected DropDownCommentControl AddDropDownCommentControl(LayoutGroup container, string caption, bool isRequired, IDropDownView idv, short length = 50, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		DropDownCommentControl dropDownCommentControl = new DropDownCommentControl
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, dropDownCommentControl, horizontalAlignment, isNew, helpText);
		AddControl(dropDownCommentControl, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(dropDownCommentControl, li);
		dropDownCommentControl.IsRequired = GetIsRequired(name, isRequired);
		dropDownCommentControl.DataView = idv;
		dropDownCommentControl.MaxLength = length;
		dropDownCommentControl.MinWidth = 5.0;
		dropDownCommentControl.Owner = this;
		dropDownCommentControl.Text = "";
		if (horizontalAlignment != System.Windows.HorizontalAlignment.Stretch)
		{
			dropDownCommentControl.Width = SizeCalculator.WidthForString(length);
		}
		TabValidationHelper.RegisterControl(dropDownCommentControl.InnerCodeEdit);
		dropDownCommentControl.BrowseEnd += DropDownBrowseEnd;
		dropDownCommentControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DropDownValueChanged).Handler;
		return dropDownCommentControl;
	}

	public ComboBoxEditExt AddComboBoxControl(LayoutGroup container, FieldDefinition definition, IEnumerable<string> itemsSource, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, bool isNew = false, string helpText = "")
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddComboBoxControl(container, stringFieldDefinition.Caption, stringFieldDefinition.Length, stringFieldDefinition.IsRequired, itemsSource, horizontalAlignment, stringFieldDefinition.Name, stringFieldDefinition.Name.Replace('.', '_'), isNew, helpText);
	}

	public ComboBoxEditExt AddComboBoxControl(LayoutGroup container, string caption, short length, bool isRequired, IEnumerable<string> itemsSource, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "")
	{
		ComboBoxEditExt comboBoxEditExt = new ComboBoxEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, comboBoxEditExt, horizontalAlignment, isNew, helpText);
		AddControl(comboBoxEditExt, caption, li, dataItemName, container);
		CommonEventHandlers.AddIsRequiredBinding(comboBoxEditExt, li);
		comboBoxEditExt.IsRequired = GetIsRequired(name, isRequired);
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

	public PopupColorEditExt AddColorControl(LayoutGroup container, string caption, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "", bool isNew = false, string helpText = "")
	{
		PopupColorEditExt popupColorEditExt = new PopupColorEditExt
		{
			Name = name
		};
		LayoutItemWithRequired li = AddLayoutItem(container, popupColorEditExt, horizontalAlignment, isNew, helpText);
		AddControl(popupColorEditExt, caption, li, "", container);
		CommonEventHandlers.AddIsReadonlyBinding(popupColorEditExt, li);
		if (horizontalAlignment != System.Windows.HorizontalAlignment.Stretch)
		{
			popupColorEditExt.Width = SizeCalculator.WidthForString(9);
		}
		popupColorEditExt.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DropDownValueChanged).Handler;
		TabValidationHelper.RegisterControl(popupColorEditExt);
		return popupColorEditExt;
	}

	public ImageEdit AddImageControl(LayoutGroup container, double height = 120.0, double width = 120.0, bool isTabStop = false, bool showBorder = true, bool showMenu = true, ShowMenuMode showMenuMode = ShowMenuMode.Hover, ShowLoadDialogOnClickMode showLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Always, Stretch stretch = Stretch.Uniform, VerticalAlignment verticalAlignment = VerticalAlignment.Top, System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left, string name = "")
	{
		ImageEdit imageEdit = new ImageEdit
		{
			Name = name,
			ShowMenu = showMenu,
			ShowMenuMode = showMenuMode,
			ShowLoadDialogOnClickMode = showLoadDialogOnClickMode,
			IsTabStop = isTabStop,
			Height = height,
			Width = width,
			Stretch = stretch,
			ShowBorder = showBorder
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, imageEdit, horizontalAlignment);
		layoutItemWithRequired.VerticalAlignment = verticalAlignment;
		AddControl(imageEdit, "", layoutItemWithRequired, "", container);
		return imageEdit;
	}

	public DocumentGridControl AddGrid(LayoutGroup container, GridDefinition gridDefinition, System.Windows.HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, bool isSupportDialogMode = false)
	{
		return AddGrid(container, gridDefinition.Caption, horizontalAlignment, verticalAlignment, isSupportDialogMode, gridDefinition.Name, gridDefinition.IsRequired, gridDefinition.Name.Replace('.', '_'));
	}

	public DocumentGridControl AddGrid(LayoutGroup container, string caption, System.Windows.HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, bool isSupportDialogMode = false, string dataItemName = "", bool isRequired = false, string name = "")
	{
		DocumentGridControl documentGridControl = new DocumentGridControl(isSupportDialogMode);
		if (container == null)
		{
			_layoutControl.Children.Add(documentGridControl);
		}
		else
		{
			container.Children.Add(documentGridControl);
		}
		documentGridControl.HorizontalAlignment = horizontalAlignment;
		documentGridControl.VerticalAlignment = verticalAlignment;
		documentGridControl.Owner = this;
		documentGridControl.ToolTip = caption;
		documentGridControl.Caption = caption;
		documentGridControl.WriteToStatusBarForCurrentRow += WriteToStatusBarForCurrentRow;
		documentGridControl.EscapePressed += new SafeEventHandler<EventArgs>(OnGridEscapePressed).Handler;
		documentGridControl.ControlEnterPressed += new SafeEventHandler<EventArgs>(OnGridControlEnterPressed).Handler;
		documentGridControl.Name = name;
		documentGridControl.IsRequired = isRequired;
		AddControl(documentGridControl, caption, null, dataItemName, container);
		TabValidationHelper.RegisterControl(documentGridControl);
		if (isSupportDialogMode)
		{
			if (_addedGridsWithSupportDialogMode == null)
			{
				_addedGridsWithSupportDialogMode = new List<DocumentGridControl>();
			}
			_addedGridsWithSupportDialogMode.Add(documentGridControl);
		}
		documentGridControl.View.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(GridCellValueChanged).Handler;
		return documentGridControl;
	}

	public GridControlBand AddGridBand(DocumentGridControl grid, string name, string caption = "")
	{
		GridControlBand gridControlBand = new GridControlBand
		{
			Name = name,
			Header = caption,
			Visible = true
		};
		grid.View.Grid.Bands.Add(gridControlBand);
		return gridControlBand;
	}

	protected internal LayoutItemWithRequired AddLayoutItem(LayoutGroup container, Control myControl, System.Windows.HorizontalAlignment horizontalAlignment, bool isNew = false, string helpText = "")
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		myControl.VerticalAlignment = VerticalAlignment.Center;
		LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired(isNew)
		{
			HorizontalAlignment = horizontalAlignment,
			Content = myControl
		};
		LayoutGroup layoutGroup = null;
		if (!string.IsNullOrWhiteSpace(helpText))
		{
			layoutGroup = new LayoutGroup
			{
				Orientation = Orientation.Horizontal
			};
			DXImage element = new DXImage
			{
				Margin = new Thickness(10.0, 0.0, 0.0, 0.0),
				Source = ImageService.GetImage("help_16.svg"),
				Width = 16.0,
				Height = 16.0,
				ToolTip = helpText
			};
			layoutGroup.Children.Add(layoutItemWithRequired);
			layoutGroup.Children.Add(element);
		}
		if (container == null)
		{
			if (layoutGroup == null)
			{
				_layoutControl.Children.Add(layoutItemWithRequired);
			}
			else
			{
				_layoutControl.Children.Add(layoutGroup);
			}
		}
		else if (layoutGroup == null)
		{
			container.Children.Add(layoutItemWithRequired);
		}
		else
		{
			container.Children.Add(layoutGroup);
		}
		return layoutItemWithRequired;
	}

	public LayoutGroupExt AddLayoutGroup(LayoutGroup container, LayoutGroupView view, Orientation orientation, string header = "")
	{
		LayoutGroupExt layoutGroupExt = new LayoutGroupExt
		{
			View = view,
			Orientation = orientation
		};
		if (!string.IsNullOrWhiteSpace(header))
		{
			layoutGroupExt.Header = header;
		}
		if (container == null)
		{
			_layoutControl.Children.Add(layoutGroupExt);
		}
		else
		{
			container.Children.Add(layoutGroupExt);
		}
		return layoutGroupExt;
	}

	public DXTabControl AddTabControl(LayoutGroup container)
	{
		DXTabControl dXTabControl = new DXTabControl();
		if (_generalTabControl == null)
		{
			_generalTabControl = dXTabControl;
		}
		if (container == null)
		{
			_layoutControl.Children.Add(dXTabControl);
		}
		else
		{
			container.Children.Add(dXTabControl);
		}
		return dXTabControl;
	}

	public LayoutGroup AddTabbedGroup(DXTabControl tc, string header, string name = "", string prefix = "Tab", FeatureStatusTypeSE featureStatus = null)
	{
		DXTabItem dXTabItem = new DXTabItem();
		tc.Items.Add(dXTabItem);
		dXTabItem.Header = header;
		if (featureStatus != null && featureStatus != FeatureStatusTypeSE.Regular)
		{
			dXTabItem.Tag = featureStatus;
		}
		if (!string.IsNullOrWhiteSpace(name))
		{
			dXTabItem.Name = (string.IsNullOrWhiteSpace(prefix) ? name : (prefix + "_" + name));
		}
		dXTabItem.HeaderTemplate = (DataTemplate)Settings.GenericResources["TabHeader"];
		dXTabItem.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(OnKeyDownEnterMoveNextTabItem).Handler;
		LayoutControl result = (LayoutControl)(dXTabItem.Content = new LayoutControl
		{
			Margin = (Settings.IsWin11Theme ? new Thickness(-8.0, -8.0, -8.0, -8.0) : new Thickness(-15.0, -15.0, -15.0, -15.0)),
			Orientation = Orientation.Vertical
		});
		TabControls.Add(dXTabItem);
		return result;
	}

	public void OnKeyDownEnterMoveNextTabItem(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected Obj, but got Unknown
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && sender is DXTabItem dXTabItem && !((LayoutGroup)dXTabItem.Content).IsKeyboardFocusWithin)
		{
			dXTabItem.MoveFocus(new TraversalRequest((FocusNavigationDirection)0));
			e.Handled = true;
		}
	}

	protected void AddControl(Control control, string caption, LayoutItemWithRequired li, string dataItemName = "", LayoutGroup container = null)
	{
		if (string.IsNullOrWhiteSpace(control.Name))
		{
			control.Name = dataItemName.Replace('.', '_');
		}
		ControlDescriptor item = new ControlDescriptor
		{
			Control = control,
			Caption = caption,
			LayoutItem = li,
			Name = control.Name,
			DataItemName = dataItemName
		};
		if (li != null)
		{
			li.Label = caption;
		}
		Controls.Add(item);
		if (_setFocusOnActivation == null)
		{
			_setFocusOnActivation = control;
		}
		if (_setFocusAfterInvalidSave == null)
		{
			_setFocusAfterInvalidSave = control;
		}
		if (control is BaseEdit baseEdit)
		{
			baseEdit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ControlValueChanged).Handler;
		}
	}

	protected virtual void CloseDocument()
	{
		if (_isDialog)
		{
			DialogResult = false;
		}
		else
		{
			Close();
		}
		if (PO.ISN != Guid.Empty)
		{
			DocumentGridRow.RemoveDocWindow(PO.ISN);
		}
	}

	protected void SaveAndCloseDocument()
	{
		if (!_isReadOnly && InnerSave())
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

	protected bool SaveDocument(bool isAsDraft = false)
	{
		bool flag = false;
		if (!_isReadOnly)
		{
			flag = InnerSave(isAsDraft);
			IsChanged = !flag;
			if (flag)
			{
				if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
				{
					CallsRecordingHelper.RecordCallStepInFile($"{Definition.FullCaption} ({Definition.POType}) directory window reload after save start");
				}
				_viewMode = WindowViewMode.Edit;
				OnEditUpper();
				RefreshRequiredGrids();
				SetReportBarButtonsProperties();
				if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
				{
					CallsRecordingHelper.RecordCallStepInFile($"{Definition.FullCaption} ({Definition.POType}) directory window reload after save finished");
				}
			}
			IsChanged = !flag;
		}
		return flag;
	}

	protected bool SaveAndNewDocument()
	{
		bool flag = false;
		if (!_isReadOnly)
		{
			if (!_isDialog)
			{
				flag = InnerSave();
			}
			if (flag)
			{
				_viewMode = WindowViewMode.NewAfterSave;
				OnNewAfterSaveUpper();
				bool flag2 = false;
				if (_generalTabControl != null && _generalTabControl.Items.Count > 1)
				{
					for (int i = 1; i <= _generalTabControl.Items.Count - 1; i++)
					{
						DXTabItem dXTabItem = (DXTabItem)_generalTabControl.Items[i];
						if (dXTabItem.IsKeyboardFocusWithin)
						{
							flag2 = true;
							break;
						}
					}
				}
				if (flag2)
				{
					DXTabItem dXTabItem = (DXTabItem)_generalTabControl.Items[0];
					_generalTabControl.SelectedContainer = dXTabItem;
				}
				ControlsHelper.SetFocus(_setFocusOnActivation);
			}
			IsChanged = !flag;
		}
		return flag;
	}

	protected virtual void ReLoad()
	{
	}

	protected virtual bool InnerSave(bool isAsDraft = false)
	{
		return true;
	}

	protected bool InnerValidate(out string errorMgs, bool validateWithoutRequired = false)
	{
		for (int i = 0; i <= Controls.Count - 1; i++)
		{
			Control control = Controls[i].Control;
			try
			{
				if (!ControlsHelper.DoValidateAndSetFocus(control, out errorMgs, validateWithoutRequired))
				{
					return false;
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				CustomControlValidate(control, validateWithoutRequired);
			}
		}
		errorMgs = null;
		return Validate();
	}

	protected virtual bool CustomControlValidate(Control control, bool validateWithoutRequired)
	{
		return true;
	}

	protected virtual bool Validate()
	{
		return true;
	}

	private void BarButtonSave_ItemClick(object sender, ItemClickEventArgs e)
	{
		SaveDocument();
	}

	private void BarButtonSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
	{
		SaveAndCloseDocument();
	}

	private void BarButtonSaveAndNew_ItemClick(object sender, ItemClickEventArgs e)
	{
		SaveAndNewDocument();
	}

	private void BarButtonDraft_ItemClick(object sender, ItemClickEventArgs e)
	{
		SaveDocument(isAsDraft: true);
	}

	private void BarButtonReload_ItemClick(object sender, ItemClickEventArgs e)
	{
		ReLoad();
		if (_isReadOnly)
		{
			OnViewUpper();
		}
		else
		{
			OnEditUpper();
		}
		RefreshRequiredGrids();
		IsChanged = false;
	}

	private void BarButtonAttachmentFiles_ItemClick(object sender, ItemClickEventArgs e)
	{
		ShowAttachmentsWindow(AttachType.File);
	}

	private void BarButtonAttachmentLinks_ItemClick(object sender, ItemClickEventArgs e)
	{
		ShowAttachmentsWindow(AttachType.Link);
	}

	private void BarButtonAttachmentDocs_ItemClick(object sender, ItemClickEventArgs e)
	{
		ShowAttachmentsWindow(AttachType.Doc);
	}

	private void ShowAttachmentsWindow(AttachType attachedType)
	{
		AttachmentsWindow attachmentsWindow = new AttachmentsWindow(POISN(), attachedType, _isReadOnly);
		attachmentsWindow.Caption = Caption;
		attachmentsWindow.RegistryKey = _registryKey;
		attachmentsWindow.ShowDialog();
	}

	private void BarButtonClose_ItemClick(object sender, ItemClickEventArgs e)
	{
		CloseDocument();
	}

	private void BarButtonPrint_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint();
		OnPrintPreview(_printTemplate, PrintMode.Print);
	}

	private void BarButtonPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint();
		OnPrintPreview(_printTemplate, PrintMode.PrintPreview);
	}

	private void BarButtonWord_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint();
		OnPrintPreview(_printTemplate, PrintMode.ExportToWord);
	}

	protected virtual void BarButtonExportXML_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected virtual void BarButtonShowISN_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	private void BarButtonExcel_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint();
		OnPrintPreview(_printTemplate, PrintMode.ExportToExcel);
	}

	private void BarButtonPdf_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint();
		OnPrintPreview(_printTemplate, PrintMode.ExportToPdf);
	}

	private void BarEditCurrentTemplate_EditValueChanged(object sender, RoutedEventArgs e)
	{
		_printTemplate = (TemplatesCacheRow)_barEditCurrentTemplate.EditValue;
	}

	private void BarButtonHelp_ItemClick(object sender, ItemClickEventArgs e)
	{
		Settings.HostingEnvironment.ShowHelp(Definition.HelpId);
	}

	protected void ValidateWithNoneAndAlsoFillDocumentForPrint(bool isForExportToXML = false)
	{
	}

	private void BarCheckDialogMode_CheckedChanged(object sender, ItemClickEventArgs e)
	{
		GridMode = !_barCheckDialogMode.IsChecked.Value;
	}

	private void BarCheckGridRowMode_CheckedChanged(object sender, ItemClickEventArgs e)
	{
		GridMode = _barCheckGridRowMode.IsChecked.Value;
	}

	private void GridModeChanged(bool mode)
	{
		if (_addedGridsWithSupportDialogMode == null)
		{
			return;
		}
		foreach (DocumentGridControl item in _addedGridsWithSupportDialogMode)
		{
			item.IsGridMode = mode;
		}
	}

	private void DropDownBrowseEnd(object sender)
	{
		StatusBarCurrentRow.Content = "";
	}

	protected internal void WriteToStatusBarForCurrentRow(object sender, string msg)
	{
		StatusBarCurrentRow.Content = msg;
	}

	protected internal void OnGridControlEnterPressed(object sender, EventArgs e)
	{
		SaveAndCloseDocument();
	}

	protected internal void OnGridEscapePressed(object sender, EventArgs e)
	{
		CloseDocument();
	}

	private void DocumentWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivation)
		{
			ControlsHelper.SetFocus(_setFocusOnActivation);
			_firstActivation = false;
		}
	}

	private static void TextChanged(object sender, RoutedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected Obj, but got Unknown
		if (Window.GetWindow((DependencyObject)sender) is POWindow<T> { IsChanged: false, _firstActivation: false } pOWindow)
		{
			pOWindow.IsChanged = true;
		}
	}

	protected virtual void SetEvents()
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
			bool flag = _ribbonIsMinimized != Ribbon.IsMinimized;
			if (!flag)
			{
				flag = ((!_ribbonToolbarShowAbove) ? (Ribbon.ToolbarShowMode != RibbonQuickAccessToolbarShowMode.ShowBelow) : (Ribbon.ToolbarShowMode != RibbonQuickAccessToolbarShowMode.Default));
			}
			if (flag)
			{
				StoreSettings();
			}
			_ribbonIsMinimized = Ribbon.IsMinimized;
			_ribbonToolbarShowAbove = Ribbon.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.Default;
			SetCurrentTemplate();
		}
	}

	private void SetCurrentTemplate()
	{
		if (Settings.IsWin11Theme)
		{
			if (Ribbon.IsSimplified)
			{
				_barEditCurrentTemplate.EditHeight = 30.0;
				_currentTemplateComboEdit.ItemTemplate = (DataTemplate)Settings.GenericResources["CurrentTemplateComboSimplifiedTemplate"];
			}
			else
			{
				_barEditCurrentTemplate.EditHeight = 60.0;
				_currentTemplateComboEdit.ItemTemplate = (DataTemplate)Settings.GenericResources["CurrentTemplateComboTemplate"];
			}
		}
		else
		{
			_currentTemplateComboEdit.ItemTemplate = (DataTemplate)Settings.GenericResources["CurrentTemplateComboTemplate"];
		}
	}

	private void DocumentWindow_Closing(object sender, CancelEventArgs e)
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
			if (MessageBox.Show(ArmSoft.SME.Common.Resources.QuestionWhenCloseWindow, Caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, this) == MessageBoxResult.No)
			{
				e.Cancel = true;
			}
		}
		if (!e.Cancel)
		{
			StoreSettings();
		}
	}

	private void DocumentWindow_Initialized(object sender, EventArgs e)
	{
	}

	private void DocumentWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Invalid comparison between Unknown and I4
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Invalid comparison between Unknown and I4
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Invalid comparison between Unknown and I4
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Invalid comparison between Unknown and I4
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Invalid comparison between Unknown and I4
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Invalid comparison between Unknown and I4
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Invalid comparison between Unknown and I4
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Invalid comparison between Unknown and I4
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Invalid comparison between Unknown and I4
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Invalid comparison between Unknown and I4
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Invalid comparison between Unknown and I4
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Invalid comparison between Unknown and I4
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Invalid comparison between Unknown and I4
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Invalid comparison between Unknown and I4
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Invalid comparison between Unknown and I4
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Invalid comparison between Unknown and I4
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Invalid comparison between Unknown and I4
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Invalid comparison between Unknown and I4
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Invalid comparison between Unknown and I4
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Invalid comparison between Unknown and I4
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 4)
		{
			Settings.HostingEnvironment.ClosePanels();
			e.Handled = true;
			return;
		}
		if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			Close();
			return;
		}
		if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 0 && !(e.Source is PopupCalcEditExt))
		{
			OnF4();
			e.Handled = true;
		}
		if ((int)e.Key == 91 && (int)Keyboard.Modifiers == 0)
		{
			OnF2();
			e.Handled = true;
		}
		else if ((int)e.Key == 92 && (int)Keyboard.Modifiers == 0)
		{
			OnF3();
			e.Handled = true;
		}
		else if ((int)e.Key == 95 && (int)Keyboard.Modifiers == 0)
		{
			OnF6();
			e.Handled = true;
		}
		else if ((int)e.Key == 96 && (int)Keyboard.Modifiers == 0)
		{
			OnF7();
			e.Handled = true;
		}
		else if ((int)e.Key == 97 && (int)Keyboard.Modifiers == 0)
		{
			OnF8();
			e.Handled = true;
		}
		else if ((int)e.Key == 98 && (int)Keyboard.Modifiers == 0)
		{
			OnF9();
			e.Handled = true;
		}
		else if ((int)e.Key == 98 && (int)Keyboard.Modifiers == 2)
		{
			OnCtrlF9();
			e.Handled = true;
		}
		else if ((int)e.Key == 98 && (int)Keyboard.Modifiers == 4)
		{
			OnShiftF9();
			e.Handled = true;
		}
		else if ((int)e.Key == 97 && (int)Keyboard.Modifiers == 4)
		{
			OnShiftF8();
			e.Handled = true;
		}
		else if ((int)e.Key == 100 && (int)Keyboard.Modifiers == 0)
		{
			OnF11();
			e.Handled = true;
		}
		else if ((int)e.Key == 100 && (int)Keyboard.Modifiers == 2)
		{
			OnCtrlF11();
			e.Handled = true;
		}
		else if ((int)e.Key == 35 && (int)Keyboard.Modifiers == 2)
		{
			OnCtrl1();
			e.Handled = true;
		}
		else
		{
			if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
			{
				if (_addedButtons == null)
				{
					return;
				}
				{
					foreach (Button addedButton in _addedButtons)
					{
						if (addedButton.IsKeyboardFocused)
						{
							e.Handled = true;
							SaveAndCloseDocument();
							break;
						}
					}
					return;
				}
			}
			if ((int)Keyboard.Modifiers == 2 && (int)e.Key == 58)
			{
				Settings.HostingEnvironment.ShowOptionsWindow();
				e.Handled = true;
			}
			else if ((int)Keyboard.Modifiers == 2 && (int)e.Key == 50)
			{
				PreviewKeyDownControlKeyG?.Invoke();
				e.Handled = true;
			}
			else if ((int)e.Key == 94 && (int)Keyboard.Modifiers == 0 && _barButtonPrintPreview.IsVisible && _barButtonPrintPreview.IsEnabled)
			{
				BarButtonPrintPreview_ItemClick(this, null);
				e.Handled = true;
			}
		}
	}

	private void CurrentTemplateComboEdit_DefaultButtonClick(object sender, RoutedEventArgs e)
	{
	}

	protected virtual void GetPO()
	{
	}

	public POWindow()
		: this((T)null)
	{
	}

	public POWindow(T doc)
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_096c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
		Initialized += new SafeEventHandler<EventArgs>(DocumentWindow_Initialized).Handler;
		if (doc == null)
		{
			GetPO();
		}
		else
		{
			PO = doc;
		}
		WindowKind = WindowKind.Ribbon;
		MinHeight = 100.0;
		Width = 651.0;
		DVPID = -1;
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
		_barButtonSaveAndNew = new BarButtonItem
		{
			Content = "Save and New",
			KeyGesture = new KeyGesture((Key)54, (ModifierKeys)2)
		};
		RenderOptions.SetBitmapScalingMode((DependencyObject)(object)_barButtonSaveAndNew, BitmapScalingMode.HighQuality);
		_barButtonSaveAndNew.Glyph = ImageService.GetImage("save_new_16.svg");
		_barButtonSaveAndNew.LargeGlyph = ImageService.GetImage("save_new_32.svg");
		_barManager.Items.Add(_barButtonSaveAndNew);
		_barButtonClose = new BarButtonItem
		{
			Content = "Close",
			Name = "BarButtonClose",
			KeyGesture = new KeyGesture((Key)13),
			Glyph = ImageService.GetImage("close_16.svg"),
			LargeGlyph = ImageService.GetImage("close_32.svg")
		};
		_barManager.Items.Add(_barButtonClose);
		StatusBarCurrentRow = new BarStaticItem
		{
			ItemMinWidth = 150.0,
			AutoSizeMode = BarItemAutoSizeMode.Content,
			ContentAlignment = System.Windows.HorizontalAlignment.Center
		};
		_barManager.Items.Add(StatusBarCurrentRow);
		StatusBarDocumentState = new BarStaticItem
		{
			ItemMinWidth = 150.0,
			AutoSizeMode = BarItemAutoSizeMode.Content,
			ContentAlignment = System.Windows.HorizontalAlignment.Center
		};
		_barManager.Items.Add(StatusBarDocumentState);
		_barButtonPrint = new BarButtonItem
		{
			Name = "PrintButton",
			Content = "Print",
			KeyGesture = new KeyGesture((Key)59, (ModifierKeys)2),
			Glyph = ImageService.GetImage("print_16.svg"),
			LargeGlyph = ImageService.GetImage("print_32.svg")
		};
		_barManager.Items.Add(_barButtonPrint);
		_barButtonPrintPreview = new BarButtonItem
		{
			Name = "PrintPreviewButton",
			Content = "Print Preview",
			KeyGesture = new KeyGesture((Key)94),
			Glyph = ImageService.GetImage("doc_preview_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_preview_32.svg")
		};
		_barManager.Items.Add(_barButtonPrintPreview);
		_barSubItemExport = new BarSubItem
		{
			Content = "",
			Glyph = ImageService.GetImage("export_16.svg"),
			LargeGlyph = ImageService.GetImage("export_32.svg")
		};
		_barManager.Items.Add(_barSubItemExport);
		_barButtonWord = new BarButtonItem
		{
			Content = "Export to Word",
			KeyGesture = new KeyGesture((Key)96, (ModifierKeys)2),
			Glyph = ImageService.GetImage("doc_export_word_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_export_word_32.svg")
		};
		_barManager.Items.Add(_barButtonWord);
		_barSubItemExport.ItemLinks.Add(_barButtonWord.CreateLink());
		_barButtonExcel = new BarButtonItem
		{
			Content = "Export to Excel",
			KeyGesture = new KeyGesture((Key)97, (ModifierKeys)2),
			Glyph = ImageService.GetImage("doc_export_excel_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_export_excel_32.svg")
		};
		_barManager.Items.Add(_barButtonExcel);
		_barSubItemExport.ItemLinks.Add(_barButtonExcel.CreateLink());
		_barButtonPdf = new BarButtonItem
		{
			Content = "Export to Pdf",
			Glyph = ImageService.GetImage("doc_export_pdf_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_export_pdf_32.svg")
		};
		_barManager.Items.Add(_barButtonPdf);
		_barSubItemExport.ItemLinks.Add(_barButtonPdf.CreateLink());
		_barButtonDraft = new BarButtonItem
		{
			Content = "Draft",
			KeyGesture = new KeyGesture((Key)47, (ModifierKeys)6),
			Glyph = ImageService.GetImage("draft_16.svg"),
			LargeGlyph = ImageService.GetImage("draft_32.svg")
		};
		_barManager.Items.Add(_barButtonDraft);
		_barButtonReload = new BarButtonItem
		{
			Content = "Reload",
			Glyph = ImageService.GetImage("reload_16.svg"),
			LargeGlyph = ImageService.GetImage("reload_32.svg")
		};
		_barManager.Items.Add(_barButtonReload);
		_barCheckDialogMode = new BarCheckItem
		{
			Content = "Dialog",
			GroupIndex = 1,
			Glyph = ImageService.GetImage("Dialog_16.svg"),
			LargeGlyph = ImageService.GetImage("Dialog_32.svg")
		};
		_barManager.Items.Add(_barCheckDialogMode);
		_barCheckGridRowMode = new BarCheckItem
		{
			Content = "Row",
			GroupIndex = 1,
			Glyph = ImageService.GetImage("Grid_row_16.svg"),
			LargeGlyph = ImageService.GetImage("Grid_row_32.svg")
		};
		_barManager.Items.Add(_barCheckGridRowMode);
		_barButtonHelp = new BarButtonItem
		{
			Content = "Help",
			KeyGesture = new KeyGesture((Key)90),
			Glyph = ImageService.GetImage("help_16.svg"),
			LargeGlyph = ImageService.GetImage("help_32.svg")
		};
		_barManager.Items.Add(_barButtonHelp);
		_barButtonShowISN = new BarButtonItem
		{
			Content = "ISN",
			Glyph = ImageService.GetImage("isn_16.svg"),
			LargeGlyph = ImageService.GetImage("isn_32.svg")
		};
		_barManager.Items.Add(_barButtonShowISN);
		_barButtonExportXML = new BarButtonItem
		{
			Content = "Export to XML",
			Glyph = ImageService.GetImage("doc_export_xml_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_export_xml_32.svg")
		};
		_barManager.Items.Add(_barButtonExportXML);
		_barSubItemExport.ItemLinks.Add(_barButtonExportXML.CreateLink());
		_barButtonAttachmentFiles = new BarButtonItem
		{
			Content = "AttachmentFiles",
			Glyph = ImageService.GetImage("attach_file_16.svg"),
			LargeGlyph = ImageService.GetImage("attach_file_32.svg")
		};
		_barManager.Items.Add(_barButtonAttachmentFiles);
		_barButtonAttachmentLinks = new BarButtonItem
		{
			Content = "AttachmentLinks",
			Glyph = ImageService.GetImage("attach_link_16.svg"),
			LargeGlyph = ImageService.GetImage("attach_link_32.svg")
		};
		_barManager.Items.Add(_barButtonAttachmentLinks);
		_barButtonAttachmentDocs = new BarButtonItem
		{
			Content = "AttachmentDocs",
			Glyph = ImageService.GetImage("attach_document_link_16.svg"),
			LargeGlyph = ImageService.GetImage("attach_document_link_32.svg")
		};
		_barManager.Items.Add(_barButtonAttachmentDocs);
		_barItemAttachments = new BarSubItem
		{
			Content = string.Format(ArmSoft.SME.Common.Resources.Attachments, "...")
		};
		BarItemLink barItemLink = _barButtonAttachmentFiles.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barItemAttachments.ItemLinks.Add(barItemLink);
		barItemLink = _barButtonAttachmentLinks.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barItemAttachments.ItemLinks.Add(barItemLink);
		barItemLink = _barButtonAttachmentDocs.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barItemAttachments.ItemLinks.Add(barItemLink);
		_barItemAttachments.Glyph = ImageService.GetImage("attach_16.svg");
		_barItemAttachments.LargeGlyph = ImageService.GetImage("attach_32.svg");
		_barManager.Items.Add(_barItemAttachments);
		_barEditCurrentTemplate = new BarEditItem
		{
			EditWidth = 200.0,
			Focusable = false,
			EditHeight = 60.0
		};
		_currentTemplateComboEdit = new ComboBoxEditSettings();
		_barEditCurrentTemplate.EditSettings = _currentTemplateComboEdit;
		_barManager.Items.Add(_barEditCurrentTemplate);
		_currentTemplateComboEdit.PopupMaxHeight = 250.0;
		_currentTemplateComboEdit.IsTextEditable = false;
		_currentTemplateComboEdit.Focusable = false;
		_currentTemplateComboEdit.ApplyItemTemplateToSelectedItem = true;
		DockPanel dockPanel = new DockPanel
		{
			UseLayoutRounding = true
		};
		Ribbon = new RibbonControl
		{
			RibbonStyle = RibbonStyle.Office2010,
			UseLayoutRounding = true,
			AllowCustomization = false,
			IsTabStop = false,
			AllowSimplifiedRibbon = Settings.IsWin11Theme
		};
		ApplicationMenu applicationMenu = new ApplicationMenu();
		Ribbon.ApplicationMenu = (DependencyObject)(object)applicationMenu;
		Ribbon.ApplicationButtonText = "";
		Ribbon.ApplicationButtonSmallIcon = ImageService.GetImage("hamburger_horizontal_16.svg");
		applicationMenu.RightPaneWidth = 200.0;
		applicationMenu.ItemLinks.Add(_barButtonClose.CreateLink());
		RibbonDefaultPageCategory ribbonDefaultPageCategory = new RibbonDefaultPageCategory();
		Ribbon.Categories.Add(ribbonDefaultPageCategory);
		_ribbonPageHome = new RibbonPage
		{
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
		};
		ribbonDefaultPageCategory.Pages.Add(_ribbonPageHome);
		_ribbonPageHome.Caption = "Home";
		_ribbonPageSave = new RibbonPageGroup();
		_ribbonPageHome.Groups.Add(_ribbonPageSave);
		_ribbonPageSave.Caption = "Save";
		_ribbonPageSave.ItemLinks.Add(_barButtonSave.CreateLink());
		_ribbonPageSave.ItemLinks.Add(_barButtonSaveAndClose.CreateLink());
		_ribbonPageSave.ItemLinks.Add(_barButtonSaveAndNew.CreateLink());
		_ribbonPageSave.ItemLinks.Add(_barButtonDraft.CreateLink());
		_ribbonPageSave.ItemLinks.Add(_barButtonReload.CreateLink());
		_ribbonPagePrint = new RibbonPageGroup
		{
			Name = "PrintGroup"
		};
		_ribbonPageHome.Groups.Add(_ribbonPagePrint);
		_ribbonPagePrint.Caption = "Print";
		_ribbonPagePrint.ItemLinks.Add(_barButtonPrint.CreateLink());
		_ribbonPagePrint.ItemLinks.Add(_barButtonPrintPreview.CreateLink());
		_ribbonPageExport = new RibbonPageGroup();
		_ribbonPageHome.Groups.Add(_ribbonPageExport);
		_ribbonPageExport.Caption = "Export";
		_ribbonPageExport.ItemLinks.Add(_barSubItemExport.CreateLink());
		_ribbonPagePrintTemplate = new RibbonPageGroup();
		_ribbonPageHome.Groups.Add(_ribbonPagePrintTemplate);
		_ribbonPagePrintTemplate.Caption = "Print Template";
		BarItemLink barItemLink2 = _barEditCurrentTemplate.CreateLink();
		barItemLink2.RibbonStyle = RibbonItemStyles.Large;
		_ribbonPagePrintTemplate.ItemLinks.Add(barItemLink2);
		_ribbonPageGridSettings = new RibbonPageGroup();
		_ribbonPageHome.Groups.Add(_ribbonPageGridSettings);
		_ribbonPageGridSettings.Caption = "Grid mode";
		_ribbonPageGridSettings.ItemLinks.Add(_barCheckDialogMode.CreateLink());
		_ribbonPageGridSettings.ItemLinks.Add(_barCheckGridRowMode.CreateLink());
		Ribbon.PageHeaderItemLinks.Add(_barButtonHelp.CreateLink());
		_ribbonPageForOther = new RibbonPageGroup();
		_ribbonPageHome.Groups.Add(_ribbonPageForOther);
		_ribbonPageForOther.Caption = ArmSoft.SME.Common.Resources.Other;
		_ribbonPageForOther.ItemLinks.Add(_barButtonShowISN.CreateLink());
		_ribbonAttachmentPage = new RibbonPage
		{
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
		};
		ribbonDefaultPageCategory.Pages.Add(_ribbonAttachmentPage);
		_ribbonAttachmentPageGroup = new RibbonPageGroup();
		_ribbonAttachmentPage.Groups.Add(_ribbonAttachmentPageGroup);
		_ribbonAttachmentPageGroup.Caption = "Attachments";
		_ribbonAttachmentPageGroup.ItemLinks.Add(_barButtonAttachmentFiles.CreateLink());
		_ribbonAttachmentPageGroup.ItemLinks.Add(_barButtonAttachmentLinks.CreateLink());
		_ribbonAttachmentPageGroup.ItemLinks.Add(_barButtonAttachmentDocs.CreateLink());
		DockPanel.SetDock(Ribbon, System.Windows.Controls.Dock.Top);
		dockPanel.Children.Add(Ribbon);
		_ribbonStatusBar = new RibbonStatusBarControl();
		DockPanel.SetDock(_ribbonStatusBar, System.Windows.Controls.Dock.Bottom);
		_ribbonStatusBar.LeftItemLinks.AddWithSeparator(StatusBarCurrentRow.CreateLink());
		_ribbonStatusBar.RightItemLinks.AddWithSeparator(StatusBarDocumentState.CreateLink());
		dockPanel.Children.Add(_ribbonStatusBar);
		_layoutControl = new LayoutControl
		{
			Orientation = Orientation.Vertical
		};
		dockPanel.Children.Add(_layoutControl);
		_barManager.Child = dockPanel;
		Content = _barManager;
		_barButtonSave.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSave_ItemClick).Handler;
		_barButtonSaveAndClose.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSaveAndClose_ItemClick).Handler;
		_barButtonSaveAndNew.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSaveAndNew_ItemClick).Handler;
		_barButtonClose.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonClose_ItemClick).Handler;
		_barButtonPrint.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonPrint_ItemClick).Handler;
		_barButtonPrintPreview.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonPrintPreview_ItemClick).Handler;
		_barButtonWord.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonWord_ItemClick).Handler;
		_barButtonExcel.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonExcel_ItemClick).Handler;
		_barButtonPdf.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonPdf_ItemClick).Handler;
		_barButtonDraft.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonDraft_ItemClick).Handler;
		_barButtonReload.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonReload_ItemClick).Handler;
		_barButtonAttachmentFiles.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonAttachmentFiles_ItemClick).Handler;
		_barButtonAttachmentLinks.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonAttachmentLinks_ItemClick).Handler;
		_barButtonAttachmentDocs.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonAttachmentDocs_ItemClick).Handler;
		_barButtonHelp.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonHelp_ItemClick).Handler;
		_barCheckDialogMode.CheckedChanged += new SafeEventHandler<ItemClickEventArgs>(BarCheckDialogMode_CheckedChanged).Handler;
		_barCheckGridRowMode.CheckedChanged += new SafeEventHandler<ItemClickEventArgs>(BarCheckGridRowMode_CheckedChanged).Handler;
		_barEditCurrentTemplate.EditValueChanged += new SafeEventHandler<RoutedEventArgs>(BarEditCurrentTemplate_EditValueChanged).Handler;
		Ribbon.SizeChanged += new SafeEventHandler<SizeChangedEventArgs>(Ribbon_SizeChanged).Handler;
		_barButtonExportXML.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonExportXML_ItemClick).Handler;
		_barButtonShowISN.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonShowISN_ItemClick).Handler;
		_currentTemplateComboEdit.DefaultButtonClick += new SafeEventHandler<RoutedEventArgs>(CurrentTemplateComboEdit_DefaultButtonClick).Handler;
		_isChangedNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsChangedProperty);
		_isChangedNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsChangedChanged).Handler;
		Activated += new SafeEventHandler<EventArgs>(DocumentWindow_Activated).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(DocumentWindow_Closing).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(DocumentWindow_PreviewKeyDown).Handler;
		PropertyChanged += new SafeEventHandler<PropertyChangedEventArgs>(DocumentWindow_PropertyChanged).Handler;
	}

	protected virtual void AddItemsToRibbon()
	{
	}

	protected virtual void AddToolbarItems()
	{
		Ribbon.ToolbarItemLinks?.Clear();
		Ribbon.ToolbarItemLinks.Add(_barButtonSave.ConvertForToolbar("t_save_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonSaveAndClose.ConvertForToolbar("t_save_close_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonSaveAndNew.ConvertForToolbar("t_save_new_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonDraft.ConvertForToolbar("t_draft_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonPrintPreview.ConvertForToolbar("t_doc_preview_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barItemAttachments.ConvertForToolbar("t_attach_16.svg"));
	}

	protected virtual void SetReportBarButtonsEnable(bool isEnabled)
	{
	}

	protected virtual void SetReportBarButtonsProperties()
	{
	}

	protected void AddOtherRibbonPage()
	{
		_ribbonPageOther = new RibbonPage
		{
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
		};
		Ribbon.Categories[0].Pages.Add(_ribbonPageOther);
		_ribbonPageOther.Caption = ArmSoft.SME.Common.Resources.Other;
	}

	protected void AddRibbonPageGroupActions()
	{
		if (_ribbonPageOther == null)
		{
			AddOtherRibbonPage();
		}
		if (_ribbonPageGroupActions == null)
		{
			_ribbonPageGroupActions = new RibbonPageGroup
			{
				AllowCollapse = false
			};
			_ribbonPageOther.Groups.Add(_ribbonPageGroupActions);
			_ribbonPageGroupActions.Caption = ArmSoft.SME.Common.Resources.Operations;
		}
	}

	protected void CreateRibbonPageGroupActions()
	{
		if (_ribbonPageGroupActions == null)
		{
			_ribbonPageGroupActions = new RibbonPageGroup
			{
				AllowCollapse = false,
				Caption = ArmSoft.SME.Common.Resources.Operations
			};
		}
	}

	protected void CreateRibbonPageGroupReports()
	{
		if (_ribbonPageGroupReports == null)
		{
			_ribbonPageGroupReports = new RibbonPageGroup
			{
				AllowCollapse = false,
				Caption = ArmSoft.SME.Common.Resources.Reports
			};
		}
	}

	protected void AddNewItemOnRibbonPageOther(RibbonPageGroup newRibbonPage, int? index = null)
	{
		if (_ribbonPageOther == null)
		{
			AddOtherRibbonPage();
		}
		if (!index.HasValue)
		{
			_ribbonPageOther.Groups.Add(newRibbonPage);
		}
		else
		{
			_ribbonPageOther.Groups.Insert(index.Value, newRibbonPage);
		}
	}

	protected void AddNewItemOnRibbonPageHome(RibbonPageGroup newRibbonPage)
	{
		_ribbonPageHome.Groups.Add(newRibbonPage);
	}

	private static void GridRowsChanged(object sender, RoutedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected Obj, but got Unknown
		if (Window.GetWindow((DependencyObject)sender) is POWindow<T> { _firstActivation: false } pOWindow)
		{
			pOWindow.IsChanged = true;
		}
	}

	private void CheckEditChanged(object sender, RoutedEventArgs e)
	{
		if (!_firstActivation)
		{
			IsChanged = true;
			DocumentGridRow.RefreshDocWindow(this);
		}
	}

	private void DropDownValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_firstActivation)
		{
			IsChanged = true;
			DocumentGridRow.RefreshDocWindow(this);
		}
	}

	protected internal void GridCellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (!_firstActivation)
		{
			IsChanged = true;
		}
	}

	private void GridValueChanged()
	{
		if (!_firstActivation)
		{
			IsChanged = true;
		}
	}

	private void ControlValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_firstActivation)
		{
			IsChanged = true;
			DocumentGridRow.RefreshDocWindow(this);
		}
	}

	private void ExchRateChanged(object sender, RateEditValueChangedEventArgs e)
	{
		if (!_firstActivation)
		{
			IsChanged = true;
			DocumentGridRow.RefreshDocWindow(this);
		}
	}

	private void DocumentWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "GridMode")
		{
			if (_barCheckGridRowMode.IsChecked != GridMode)
			{
				_barCheckGridRowMode.IsChecked = GridMode;
			}
			if (_barCheckDialogMode.IsChecked != !GridMode)
			{
				_barCheckDialogMode.IsChecked = !GridMode;
			}
		}
	}

	public object GetControlValue(string datItemName)
	{
		Control control = Controls.Find((ControlDescriptor p) => p.DataItemName == datItemName).Control;
		return GetControlValue(control);
	}

	public Type GetGridRowType(string dataItemName)
	{
		Control control = Controls.FirstOrDefault((ControlDescriptor p) => p.DataItemName == dataItemName)?.Control;
		if (control != null)
		{
			return ((DocumentGridControl)control).ItemsSource.GetType().GetGenericArguments()[0];
		}
		return null;
	}

	public Guid POISN()
	{
		return PO.ISN;
	}

	public List<CallsRecordingHelper.ControlInfo> GetControlsInfoForRecord(bool withoutValues = false)
	{
		List<CallsRecordingHelper.ControlInfo> list = new List<CallsRecordingHelper.ControlInfo>();
		foreach (DXTabItem tabControl in TabControls)
		{
			list.Add(new CallsRecordingHelper.ControlInfo(tabControl.Name, ((object)tabControl).GetType().Name, null));
		}
		foreach (ControlDescriptor control in Controls)
		{
			AddControlInfoForRecord(list, control, withoutValues);
		}
		return list;
	}

	protected virtual void AddControlInfoForRecord(List<CallsRecordingHelper.ControlInfo> result, ControlDescriptor controlDesc, bool withoutValues = false)
	{
		object obj = null;
		if (!withoutValues)
		{
			try
			{
				obj = GetControlValue(controlDesc.Control);
				if (((object)controlDesc.Control).GetType().Name == "DocumentGridControl")
				{
					obj = ((DocumentGridControl)obj).ItemsSource;
				}
			}
			catch
			{
				obj = null;
			}
		}
		result.Add(new CallsRecordingHelper.ControlInfo(controlDesc.Control.Name, ((object)controlDesc.Control).GetType().Name, controlDesc.Caption, null, ControlsHelper.IsControlReadonly(controlDesc.Control), obj));
		if (!(((object)controlDesc.Control).GetType().Name == "DocumentGridControl"))
		{
			return;
		}
		foreach (GridColumn column in ((DocumentGridControl)controlDesc.Control).Grid.Columns)
		{
			result.Add(new CallsRecordingHelper.ControlInfo(column.Name, ((object)column).GetType().Name, Convert.ToString(column.Header), controlDesc.Control.Name, column.ReadOnly || column.AllowEditing == DefaultBoolean.False, null));
		}
	}

	protected static void SelectCellInGrid(TableView tableView, DocumentGridControl grid, int rowHnd, string columnToSelect)
	{
		tableView.Grid.CurrentItem = tableView.Grid.GetRow(rowHnd);
		grid.Grid.CurrentColumn = tableView.Grid.Columns[columnToSelect];
		tableView.Focus();
		tableView.ShowEditor();
		((DispatcherObject)tableView).Dispatcher.Invoke((Action)(() =>
		{
			if (tableView.ActiveEditor != null)
			{
				tableView.ActiveEditor.SelectAll();
			}
		}), (DispatcherPriority)6);
	}

	private void CreateUserDefinedFields_Click(object sender, EventArgs e)
	{
		ExtensionUIHelper.CreatePOExtensionBySimpleWindow(POTypeSE.Directory, Definition.POType, this);
	}
}
