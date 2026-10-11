using System;
using System.Collections.Generic;
using System.Windows.Input;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns.MetaData;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Services;

public class DataViewService : IDataViewService
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public IDataView CreateDataView(short dataProviderType, bool changeParameterSupported = true)
	{
		if (DefinitionMappingsService.TryResolveDataView(dataProviderType, out var dataView))
		{
			if (!changeParameterSupported)
			{
				return (IDataView)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, dataView, changeParameterSupported);
			}
			return (IDataView)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, dataView);
		}
		throw new RESTException(string.Format(Resources.DataViewNotSupported, dataProviderType));
	}

	public void ShowDocDataView(short docType, List<PropertyWithValue> propertyWithValues = null, bool showDialog = true, bool changeParameterSupported = true)
	{
		short documentTypeSEProviderType = SmartEnumHelper.GetDocumentTypeSEProviderType(docType);
		ShowDataView(documentTypeSEProviderType, propertyWithValues, showDialog, changeParameterSupported);
	}

	public void ShowDataView(short dataProviderType, List<PropertyWithValue> propertyWithValues = null, bool showDialog = true, bool changeParameterSupported = true)
	{
		IDataView dataView = CreateDataView(dataProviderType, changeParameterSupported);
		Type type = dataView.GetType();
		if (propertyWithValues != null)
		{
			foreach (PropertyWithValue propertyWithValue in propertyWithValues)
			{
				type.GetProperty(propertyWithValue.Name).ASSetValue(dataView, propertyWithValue.Value);
			}
		}
		if (showDialog)
		{
			dataView.Show(useParamValues: true, openAsChild: false);
		}
		else
		{
			dataView.Show(useParamValues: false, openAsChild: true, null, withoutShowDialog: true);
		}
		Keyboard.Focus(dataView.DVP);
	}

	public void ShowDataView(IDataView dataView, List<PropertyWithValue> propertyWithValues = null, bool showDialog = true)
	{
		Type type = dataView.GetType();
		if (propertyWithValues != null)
		{
			foreach (PropertyWithValue propertyWithValue in propertyWithValues)
			{
				type.GetProperty(propertyWithValue.Name).ASSetValue(dataView, propertyWithValue.Value);
			}
		}
		if (showDialog)
		{
			dataView.Show(useParamValues: true, openAsChild: false);
		}
		else
		{
			dataView.Show(useParamValues: false, openAsChild: true, null, withoutShowDialog: true);
		}
		Keyboard.Focus(dataView.DVP);
	}

	public void ShowDocDataViewWithExtenstionChoose(short docType, List<PropertyWithValue> propertyWithValues)
	{
		short documentTypeSEProviderType = SmartEnumHelper.GetDocumentTypeSEProviderType(docType);
		ShowDataViewWithExtenstionChoose(documentTypeSEProviderType, propertyWithValues);
	}

	public void ShowDataViewWithExtenstionChoose(short dataProviderType, List<PropertyWithValue> propertyWithValues)
	{
		bool flag = true;
		IDataView dataView = CreateDataView(dataProviderType, changeParameterSupported: false);
		if (!string.IsNullOrWhiteSpace(dataView.DataViewGroup))
		{
			Type type = DefinitionMappingsService.ResolveDVS(dataView.DataViewGroup);
			IDataViewSettingsHandler dataViewSettingsHandler = null;
			if (type != null)
			{
				dataViewSettingsHandler = (IDataViewSettingsHandler)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, type);
				if (dataViewSettingsHandler != null)
				{
					flag = dataViewSettingsHandler.DefaultViewEnabled();
				}
			}
			List<string> list = Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(dataView.DataViewGroup);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (dataViewSettingsHandler != null && !dataViewSettingsHandler.IsViewEnabled(list[num]))
				{
					list.RemoveAt(num);
				}
			}
			if (flag ? (list.Count > 0) : (list.Count > 1))
			{
				DialogWindow dialogWindow = new DialogWindow
				{
					Caption = SmartEnumHelper.GetPartialSmartEnumCaption(typeof(DataProviderTypeSE), dataProviderType.ToString()),
					Name = dataView.DataViewGroup
				};
				LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
				dialogWindow.AddStoreValuesButton(container);
				DropDownControl dropDownControl = dialogWindow.AddDropDownControl(idv: new ArmSoft.SME.Core.UI.DropDowns.MetaData.DataViewDefinitions(dataView.DataViewGroup, dataProviderType, !flag, isDefinedExceptHeaderGroupParameter: true), container: container, name: "DataView", caption: Resources.DataViewDefinition, isRequired: true, allowAdditionButtons: false, storeValue: true, isShowComment: false, codeWidth: 10);
				if (flag)
				{
					dropDownControl.Code = dataView.DataViewGroup;
				}
				else if (list.Count > 0)
				{
					dropDownControl.Code = list[0];
				}
				if (dialogWindow.ShowDialog().Value)
				{
					dataView.ApplyDataViewDefinition(dropDownControl.Code);
				}
			}
			else if (list != null && list.Count == 1)
			{
				dataView.ApplyDataViewDefinition(list[0]);
			}
		}
		ShowDataView(dataView, propertyWithValues, showDialog: false);
	}
}
