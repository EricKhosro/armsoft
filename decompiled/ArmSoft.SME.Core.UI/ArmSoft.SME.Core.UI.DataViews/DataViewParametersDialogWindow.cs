using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.DropDowns.MetaData;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DataViews;

public abstract class DataViewParametersDialogWindow : DialogWindow
{
	private readonly IDataView _idv;

	private DropDownControl _extendedView;

	private Dictionary<string, ArmSoft.SME.Core.UI.MetaData.DataViewDefinition> _dvds;

	private ArmSoft.SME.Core.UI.MetaData.DataViewDefinition _currentDVD;

	private Dictionary<string, LayoutGroup> _dvdLGs;

	private LayoutGroup _currentDVDLG;

	private bool _useParamValues;

	private DataViewColumnsSelectionControl _columnsSelectionControl;

	private BarItemMenuHeader _barItemMenuHeader;

	private BarButtonItem _barButtonSaveAs;

	private BarButtonItem _barButtonManageViews;

	private BarButtonItem _barButtonEditView;

	private BarSubItem _barSubItemAvailableViews;

	private List<BarButtonItem> _availableViews;

	private List<DataViewSetting> _dvsList;

	private DataViewSetting _currentDVS;

	private readonly bool _alwaysUseDataViewDefinition;

	internal DropDownControl ExtendedView => _extendedView;

	public DataViewParametersDialogWindow(IDataView idv, Window owner = null)
	{
		_idv = idv;
		_alwaysUseDataViewDefinition = false;
		Name = idv.Name;
		HelpId = idv.HelpId;
		_currentDVS = new DataViewSetting
		{
			ViewName = idv.Name,
			GroupName = ""
		};
		if (owner == null)
		{
			Owner = Settings.HostingEnvironment;
		}
		else
		{
			Owner = owner;
		}
		if (Settings.SystemType == SystemTypes.SalesManagement)
		{
			AllowParameterTemplates = true;
		}
		Closed += new SafeEventHandler<EventArgs>(DataViewParametersDialogWindow_Closed).Handler;
	}

	public virtual void ApplyParams(bool useParamValues, bool isRefreshMode)
	{
	}

	public bool ShowDialog(bool useParamValues, bool isRefreshMode)
	{
		List<string> list = null;
		_useParamValues = useParamValues;
		bool flag = false;
		bool flag2 = true;
		if (!string.IsNullOrWhiteSpace(_idv.DataViewGroup))
		{
			Mouse.OverrideCursor = Cursors.Wait;
			Type type = DefinitionMappingsService.ResolveDVS(_idv.DataViewGroup);
			IDataViewSettingsHandler dataViewSettingsHandler = null;
			if (type != null)
			{
				dataViewSettingsHandler = (IDataViewSettingsHandler)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, type);
				if (dataViewSettingsHandler != null)
				{
					flag2 = dataViewSettingsHandler.DefaultViewEnabled();
				}
			}
			list = Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(_idv.DataViewGroup);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (dataViewSettingsHandler != null && !dataViewSettingsHandler.IsViewEnabled(list[num]))
				{
					list.RemoveAt(num);
				}
			}
			flag = ((!_alwaysUseDataViewDefinition && flag2) ? (list.Count > 0) : (list.Count > 1));
			Mouse.OverrideCursor = null;
		}
		if (flag)
		{
			LayoutGroup container = null;
			if (HasControls())
			{
				AddLine();
			}
			else if (!isRefreshMode)
			{
				container = AddHorizontalLayoutGroup();
				AddStoreValuesButton(container);
			}
			ArmSoft.SME.Core.UI.DropDowns.MetaData.DataViewDefinitions idv = new ArmSoft.SME.Core.UI.DropDowns.MetaData.DataViewDefinitions(_idv.DataViewGroup, _idv.ProviderType, _alwaysUseDataViewDefinition || !flag2, isDefinedExceptHeaderGroupParameter: true);
			_extendedView = AddDropDownControl(container, "DataView", ArmSoft.SME.Common.Resources.DataViewDefinition, idv, isRequired: true, allowAdditionButtons: false, storeValue: true, isShowComment: false, 10);
			_extendedView.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ExtendedView_EditValueChanged).Handler;
			if (isRefreshMode)
			{
				if (_idv.DVD != null)
				{
					_extendedView.Code = _idv.DVD.Name;
				}
				else if (flag2)
				{
					_extendedView.Code = _idv.DataViewGroup;
				}
				else if (list.Count > 0)
				{
					_extendedView.Code = list[0];
				}
				_extendedView.IsReadOnly = true;
			}
			else if (flag2)
			{
				_extendedView.Code = _idv.DataViewGroup;
			}
			else if (list.Count > 0)
			{
				_extendedView.Code = list[0];
			}
		}
		if (ShowDialog().Value && BeforeApplyParam())
		{
			ApplyParams(useParamValues, isRefreshMode);
			ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dvd = null;
			if (isRefreshMode)
			{
				if (_idv.DVD != null)
				{
					ApplyParams(_idv.DVD);
				}
			}
			else if (flag)
			{
				if (ExtendedView != null && ((_alwaysUseDataViewDefinition & flag2) || ExtendedView.Code != _idv.DataViewGroup))
				{
					dvd = GetDVD(ExtendedView.Code);
					ApplyParams(dvd);
					Settings.ServiceProvider.GetRequiredService<IDVDService>().UpdateLastExecDate(_extendedView.Code);
				}
			}
			else if (list != null && list.Count > 0)
			{
				dvd = GetDVD(list[0]);
				ApplyParams(dvd);
				Settings.ServiceProvider.GetRequiredService<IDVDService>().UpdateLastExecDate(_extendedView.Code);
			}
			if (_columnsSelectionControl != null)
			{
				_columnsSelectionControl.Fill(_currentDVS);
				_currentDVS.Id = 0;
				_idv.ApplySettings(_currentDVS);
				if (AllowStoreValues)
				{
					if (StoreValuesIsChecked)
					{
						Settings.CreateApiClient().DataViewSettingsRoutes.Store(_currentDVS);
					}
					else
					{
						Settings.CreateApiClient().DataViewSettingsRoutes.Delete(_currentDVS);
					}
				}
			}
			if (!isRefreshMode)
			{
				_idv.ApplyDataViewDefinition(dvd);
			}
			return true;
		}
		return false;
	}

	protected virtual bool BeforeApplyParam()
	{
		return true;
	}

	private void ApplyParams(ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dvd)
	{
		if (dvd == null || dvd.DialogExtension == null || _dvdLGs == null)
		{
			return;
		}
		if (dvd.DialogExtension.Values == null)
		{
			dvd.DialogExtension.Values = new Dictionary<string, object>();
		}
		else
		{
			dvd.DialogExtension.Values.Clear();
		}
		if (!_dvdLGs.ContainsKey(dvd.Name))
		{
			return;
		}
		foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in dvd.DialogExtension.Elements)
		{
			ApplyParams(element, dvd);
		}
	}

	private void ApplyParams(ArmSoft.SME.Core.UI.MetaData.UIElement el, ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dvd)
	{
		if (el is UIField uIField)
		{
			string name = dvd.Name + "_" + uIField.Name;
			dvd.DialogExtension.Values.Add(uIField.Name, base[name]);
		}
		else
		{
			if (!(el is UIGroup uIGroup))
			{
				return;
			}
			foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in uIGroup.Elements)
			{
				ApplyParams(element, dvd);
			}
		}
	}

	private void ExtendedView_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			if (_currentDVD == null)
			{
				if (_extendedView.Code == _idv.DataViewGroup)
				{
					return;
				}
			}
			else if (_currentDVD.Name == _extendedView.Code)
			{
				return;
			}
			ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dVD = GetDVD(_extendedView.Code);
			ChangeUI(dVD);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeUI(ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dvd)
	{
		if (_dvdLGs == null)
		{
			_dvdLGs = new Dictionary<string, LayoutGroup>();
		}
		if (dvd == null)
		{
			if (_currentDVDLG != null)
			{
				_currentDVDLG.Visibility = Visibility.Collapsed;
			}
			_currentDVDLG = null;
			if (_currentDVD != null)
			{
				RemoveOldControls(_currentDVD.Name);
			}
			_currentDVD = null;
			return;
		}
		if (_currentDVDLG != null)
		{
			_currentDVDLG.Visibility = Visibility.Collapsed;
		}
		if (_currentDVD != null && _currentDVD.Name != dvd.Name)
		{
			RemoveOldControls(_currentDVD.Name);
			_currentDVDLG = null;
		}
		if (dvd.DialogExtension == null || dvd.DialogExtension.Elements == null || dvd.DialogExtension.Elements.Count == 0)
		{
			_currentDVD = dvd;
			_currentDVDLG = null;
			return;
		}
		if (_dvdLGs.TryGetValue(dvd.Name, out var value))
		{
			_currentDVDLG = value;
		}
		else
		{
			_currentDVDLG = AddVerticalLayoutGroup();
			_dvdLGs.Add(dvd.Name, _currentDVDLG);
			foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in dvd.DialogExtension.Elements)
			{
				AddExtensionControl(_currentDVDLG, element, dvd);
			}
		}
		_currentDVD = dvd;
		_currentDVDLG.Visibility = Visibility.Visible;
	}

	private void RemoveOldControls(string oldDVDName)
	{
		_dvdLGs.Clear();
		if (string.IsNullOrWhiteSpace(oldDVDName))
		{
			return;
		}
		ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dVD = GetDVD(oldDVDName);
		if (dVD == null || dVD.DialogExtension == null || dVD.DialogExtension.Elements == null)
		{
			return;
		}
		foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in dVD.DialogExtension.Elements)
		{
			RemoveExtensionControlFromDialog(element, oldDVDName);
		}
	}

	private void RemoveExtensionControlFromDialog(ArmSoft.SME.Core.UI.MetaData.UIElement el, string dvdName)
	{
		if (el is UIField uIField)
		{
			RemoveUIElement(dvdName + "_" + uIField.Name);
		}
		else
		{
			if (!(el is UIGroup uIGroup))
			{
				return;
			}
			foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in uIGroup.Elements)
			{
				el = element;
				RemoveExtensionControlFromDialog(el, dvdName);
			}
		}
	}

	private void AddExtensionControl(LayoutGroup container, ArmSoft.SME.Core.UI.MetaData.UIElement el, ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dvd)
	{
		System.Windows.HorizontalAlignment horizontalAlignment = System.Windows.HorizontalAlignment.Left;
		if (el is UIField uIField)
		{
			CheckControlExistence(uIField.Name);
			string name = dvd.Name + "_" + uIField.Name;
			switch (uIField.HorizontalAlignment)
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
			if (uIField.HideCaption)
			{
				caption = string.Empty;
			}
			else
			{
				caption = ((!string.IsNullOrWhiteSpace(uIField.Caption)) ? uIField.Caption : uIField.DataItem.Caption);
			}
			object obj;
			if (_useParamValues)
			{
				obj = GetParamValue(uIField.Name);
				if (obj == null)
				{
					obj = uIField.DataItem.DefaultValue();
				}
			}
			else
			{
				obj = uIField.DataItem.DefaultValue();
			}
			if (uIField.DataItem is StringDataItem stringDataItem)
			{
				if (uIField.DropDownItem == null)
				{
					AddStringControl(container, name, caption, stringDataItem.Length, stringDataItem.Required, uIField.StoreValue).Text = obj.ToString();
					return;
				}
				IDropDownView dropDownView = Settings.ServiceProvider.GetRequiredService<IDropDownService>().CreateDropDown(uIField.DropDownItem.Group);
				dropDownView.ApplyDefaultDataViewDefinition();
				if (uIField.DropDownItem.Group != uIField.DropDownItem.Name)
				{
					_ = Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(uIField.DropDownItem.Group, uIField.DropDownItem.Name)?.IsDefault;
				}
				if (uIField.DropDownItem.IsDropDownComment)
				{
					DropDownCommentControl dropDownCommentControl = AddDropDownCommentControl(container, name, caption, dropDownView, stringDataItem.Required, uIField.StoreValue, stringDataItem.Length, horizontalAlignment);
					dropDownCommentControl.IsTextEditable = uIField.DropDownItem.IsTextEditable;
					dropDownCommentControl.Text = obj.ToString();
				}
				else if (uIField.DropDownItem.IsMultiSelect)
				{
					AddMultiSelectDropDownControl(container, name, caption, dropDownView, stringDataItem.Required, uIField.StoreValue).SelectedString = obj.ToString();
				}
				else
				{
					DropDownControl dropDownControl = AddDropDownControl(container, name, caption, dropDownView, stringDataItem.Required, allowAdditionButtons: false, uIField.StoreValue, isShowComment: true, stringDataItem.Length);
					dropDownControl.CheckValue = uIField.DropDownItem.CheckValue;
					dropDownControl.ShowComment = uIField.DropDownItem.ShowComment;
					dropDownControl.Code = obj.ToString();
				}
			}
			else if (uIField.DataItem is TreeNodeDataItem treeNodeDataItem)
			{
				ArmSoft.SME.Core.UI.DropDowns.TreeNodes idv = new ArmSoft.SME.Core.UI.DropDowns.TreeNodes(treeNodeDataItem.Tree.Id);
				if (uIField.DropDownItem != null && uIField.DropDownItem.IsDropDownComment)
				{
					DropDownCommentControl dropDownCommentControl2 = AddDropDownCommentControl(container, name, caption, idv, treeNodeDataItem.Required, uIField.StoreValue, treeNodeDataItem.Tree.CaptionLen, horizontalAlignment);
					dropDownCommentControl2.IsTextEditable = uIField.DropDownItem.IsTextEditable;
					dropDownCommentControl2.Text = obj.ToString();
					return;
				}
				if (uIField.DropDownItem != null && uIField.DropDownItem.IsMultiSelect)
				{
					AddMultiSelectDropDownControl(container, name, caption, idv, treeNodeDataItem.Required, uIField.StoreValue).SelectedString = obj.ToString();
					return;
				}
				DropDownControl dropDownControl2 = AddDropDownControl(container, name, caption, idv, treeNodeDataItem.Required, allowAdditionButtons: false, uIField.StoreValue, isShowComment: true, treeNodeDataItem.Tree.CodeLen);
				if (uIField.DropDownItem != null)
				{
					dropDownControl2.CheckValue = uIField.DropDownItem.CheckValue;
					dropDownControl2.ShowComment = uIField.DropDownItem.ShowComment;
				}
				dropDownControl2.Code = obj.ToString();
			}
			else if (uIField.DataItem is NumberDataItem numberDataItem)
			{
				AddNumericControl(container, name, caption, numberDataItem.Length, numberDataItem.Precision, numberDataItem.Required, numberDataItem.AllowNegativeNumbers, numberDataItem.UseCurrencyFormat, uIField.StoreValue).Value = Convert.ToDecimal(obj);
			}
			else if (uIField.DataItem is DateDataItem dateDataItem)
			{
				DateEditControl dateEditControl = AddDateControl(container, name, caption, dateDataItem.UseLongDate, dateDataItem.Required, uIField.StoreValue);
				if (obj != null)
				{
					dateEditControl.EditValue = Convert.ToDateTime(obj).ToDateOnly();
				}
			}
			else if (uIField.DataItem is TimeDataItem timeDataItem)
			{
				AddTimeControl(container, name, caption, timeDataItem.ShowSeconds, timeDataItem.Required, uIField.StoreValue).EditValue = obj.ToString();
			}
			else if (uIField.DataItem is BooleanDataItem)
			{
				AddBooleanControl(container, name, caption, uIField.StoreValue).IsChecked = Convert.ToBoolean(obj);
			}
		}
		else
		{
			if (!(el is UIGroup uIGroup))
			{
				return;
			}
			LayoutGroup container2 = ((!uIGroup.IsHorizontal) ? AddVerticalLayoutGroup(container, LayoutGroupView.Group, uIGroup.Caption) : AddHorizontalLayoutGroup(container, LayoutGroupView.Group, uIGroup.Caption));
			foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in uIGroup.Elements)
			{
				AddExtensionControl(container2, element, dvd);
			}
		}
	}

	private object GetParamValue(string controlName)
	{
		if (_idv.DVD != null)
		{
			DataViewDialogExtension dialogExtension = _idv.DVD.DialogExtension;
			if (dialogExtension != null && dialogExtension.Values != null && dialogExtension.Values.TryGetValue(controlName, out var value))
			{
				return value;
			}
		}
		return null;
	}

	private ArmSoft.SME.Core.UI.MetaData.DataViewDefinition GetDVD(string dvdName)
	{
		if (_dvds == null)
		{
			_dvds = new Dictionary<string, ArmSoft.SME.Core.UI.MetaData.DataViewDefinition>();
		}
		if (_dvds.TryGetValue(dvdName, out var value))
		{
			return value;
		}
		ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dataViewDefinition = Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(_idv.DataViewGroup, dvdName);
		_dvds.Add(dvdName, dataViewDefinition);
		return dataViewDefinition;
	}

	public LayoutGroup AddColumnsSelectionTab(DXTabControl tc, LayoutGroup lgBindWidth, LayoutGroup lgBindHeight)
	{
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		LayoutGroup layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.Columns);
		if (lgBindWidth != null)
		{
			Binding binding = new Binding("ActualWidth")
			{
				Source = lgBindWidth
			};
			layoutGroup.SetBinding(FrameworkElement.MaxWidthProperty, binding);
			DialogWindow.BindWidth(layoutGroup, lgBindWidth);
		}
		else
		{
			layoutGroup.Width = 400.0;
		}
		if (lgBindHeight != null)
		{
			Binding binding2 = new Binding("ActualHeight")
			{
				Source = lgBindHeight
			};
			layoutGroup.SetBinding(FrameworkElement.MaxHeightProperty, binding2);
			DialogWindow.BindHeight(layoutGroup, lgBindHeight);
		}
		else
		{
			layoutGroup.Height = 300.0;
		}
		_columnsSelectionControl = new DataViewColumnsSelectionControl();
		_columnsSelectionControl.ResetClick += ColumnsSelectionControl_ResetClick;
		_columnsSelectionControl.IDV = _idv;
		_currentDVS = Settings.CreateApiClient().DataViewSettingsRoutes.Load(_idv.Name, "", 0);
		_columnsSelectionControl.DVS = _currentDVS;
		_columnsSelectionControl.Configure();
		layoutGroup.Children.Add(_columnsSelectionControl);
		if (AllowStoreValues)
		{
			if (_optionsPopupMenu == null)
			{
				InitializeOptionsPopupMenu();
			}
			_barItemMenuHeader = new BarItemMenuHeader
			{
				ShowContentHeader = AllowParameterTemplates,
				Content = ArmSoft.SME.Common.Resources.DataViewDefinition
			};
			_barManager.Items.Add(_barItemMenuHeader);
			_barButtonSaveAs = new BarButtonItem
			{
				Content = ArmSoft.SME.Common.Resources.SaveAs,
				Glyph = ImageService.GetImage("save_as_16.svg")
			};
			_barButtonSaveAs.ItemClick += new SafeEventHandler<RoutedEventArgs>(BarButtonSaveAs_Click).Handler;
			_barItemMenuHeader.Items.Add(_barButtonSaveAs);
			_barButtonManageViews = new BarButtonItem
			{
				Content = ArmSoft.SME.Common.Resources.ManageDataViewSettings
			};
			_barButtonManageViews.ItemClick += new SafeEventHandler<RoutedEventArgs>(BarButtonManageViews_Click).Handler;
			_barItemMenuHeader.Items.Add(_barButtonManageViews);
			_barButtonEditView = new BarButtonItem
			{
				Content = ArmSoft.SME.Common.Resources.EditViewSetting
			};
			_barButtonEditView.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonEditView_ItemClick).Handler;
			_barItemMenuHeader.Items.Add(_barButtonEditView);
			_barSubItemAvailableViews = new BarSubItem
			{
				Content = ArmSoft.SME.Common.Resources.LayoutLoad
			};
			_barItemMenuHeader.Items.Add(_barSubItemAvailableViews);
			_availableViews = new List<BarButtonItem>();
			_dvsList = Settings.CreateApiClient().DataViewSettingsRoutes.GetList(_idv.Name, "");
			_optionsPopupMenu.ItemLinks.Add(_barItemMenuHeader.CreateLink());
			RefreshOptionsPopupMenu();
		}
		return layoutGroup;
	}

	private protected override void RefreshOptionsPopupMenu()
	{
		base.RefreshOptionsPopupMenu();
		if (_barItemMenuHeader == null)
		{
			return;
		}
		_barItemMenuHeader.ItemLinks.Clear();
		BarItemLink item = _barButtonSaveAs.CreateLink();
		_barItemMenuHeader.ItemLinks.Add(item);
		item = _barButtonEditView.CreateLink();
		_barItemMenuHeader.ItemLinks.Add(item);
		if (_dvsList.Count == 0)
		{
			return;
		}
		item = _barButtonManageViews.CreateLink();
		_barItemMenuHeader.ItemLinks.Add(item);
		item = _barSubItemAvailableViews.CreateLink();
		_barItemMenuHeader.ItemLinks.Add(item);
		_barSubItemAvailableViews.ItemLinks.Clear();
		foreach (BarButtonItem availableView in _availableViews)
		{
			availableView.Tag = null;
		}
		int num = 0;
		foreach (DataViewSetting dvs in _dvsList)
		{
			if (num < _availableViews.Count)
			{
				_availableViews[num].Content = dvs.Caption;
				_availableViews[num].Tag = dvs.Id;
			}
			else
			{
				BarButtonItem barButtonItem = new BarButtonItem
				{
					Content = dvs.Caption,
					Tag = dvs.Id
				};
				barButtonItem.ItemClick += LoadAndConfigureDataViewSetting;
				_availableViews.Add(barButtonItem);
			}
			item = _availableViews[num].CreateLink();
			_barSubItemAvailableViews.ItemLinks.Add(item);
			num++;
		}
	}

	private void DataViewParametersDialogWindow_Closed(object sender, EventArgs e)
	{
		if (_availableViews != null)
		{
			_availableViews.Clear();
		}
	}

	private void LoadAndConfigureDataViewSetting(object sender, RoutedEventArgs e)
	{
		try
		{
			BarButtonItem barButtonItem = (BarButtonItem)sender;
			short id = (short)barButtonItem.Tag;
			_ = (string)barButtonItem.Content;
			_currentDVS = Settings.CreateApiClient().DataViewSettingsRoutes.Load(_idv.Name, "", id);
			_columnsSelectionControl.DVS = _currentDVS;
			_columnsSelectionControl.Configure();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BarButtonSaveAs_Click(object sender, RoutedEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Owner = this,
			Title = ArmSoft.SME.Common.Resources.DataViewDefinition + ArmSoft.SME.Common.Resources.CaptionNew
		};
		dialogWindow.AddStringControl("LayoutCaption", ArmSoft.SME.Common.Resources.Name, 50, isRequired: true).EditValue = Title;
		dialogWindow.WindowValidation += LayoutWindowEditValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			string caption = ((string)dialogWindow["LayoutCaption"]).Trim();
			if (_columnsSelectionControl != null)
			{
				_columnsSelectionControl.Fill(_currentDVS);
				_currentDVS = _currentDVS.Clone();
				_currentDVS.Id = -1;
				_currentDVS.Caption = caption;
				Settings.CreateApiClient().DataViewSettingsRoutes.Store(_currentDVS);
				_dvsList.Add(_currentDVS);
				RefreshOptionsPopupMenu();
			}
		}
	}

	private void LayoutWindowEditValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		string layoutCaption = ((string)dialog["LayoutCaption"]).Trim();
		new DataViewSetting
		{
			ViewName = _idv.Name,
			GroupName = "",
			Caption = layoutCaption
		};
		if (Settings.CreateApiClient().DataViewSettingsRoutes.GetList(_idv.Name, "").Any((DataViewSetting p) => p.Caption == layoutCaption))
		{
			MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.LayoutDuplicate, layoutCaption));
			args.isValid = false;
		}
	}

	private void BarButtonManageViews_Click(object sender, RoutedEventArgs e)
	{
		DataViewManage dataViewManage = new DataViewManage();
		dataViewManage.Owner = this;
		dataViewManage.IDV = _idv;
		dataViewManage.DVSList = _dvsList;
		dataViewManage.FillViewNames();
		if (dataViewManage.ShowDialog().Value)
		{
			RefreshOptionsPopupMenu();
		}
	}

	private void BarButtonEditView_ItemClick(object sender, ItemClickEventArgs e)
	{
		_columnsSelectionControl.Fill(_currentDVS);
		if (new DataViewSettingsWindow(_idv, this, _currentDVS, isDefault: true).ShowDialog().Value)
		{
			_columnsSelectionControl.Configure();
		}
	}

	private void ColumnsSelectionControl_ResetClick()
	{
		try
		{
			foreach (ViewColumnSorting item in _idv.Columns.SortOrder())
			{
				_currentDVS.SortOrder.Add(new ArmSoft.SME.Common.Models.Data.ColumnOrder
				{
					Name = item.Column.Name,
					Descending = item.Descending
				});
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
