using System;
using System.Collections.Generic;
using ArmSoft.SME.Client.Data.MetaData;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.DropDowns.MetaData;

[DropDown(20)]
public class DataViewDefinitions : DropDownView<DataViewDefinitionsDataProvider.DataRow, DataViewDefinitionsDataProvider.Param>
{
	private readonly string _group;

	private readonly short _providerType;

	private bool _isExceptHeaderGroup;

	private readonly bool _isDefinedExceptHeaderGroupParameter;

	private string _defaultDataViewCode = "";

	public string DefaultDataViewCode => _defaultDataViewCode;

	public DataViewDefinitions(string group, short providerType, bool isExceptHeaderGroup = false, bool isDefinedExceptHeaderGroupParameter = false)
	{
		_group = group;
		_providerType = providerType;
		_isExceptHeaderGroup = isExceptHeaderGroup;
		_isDefinedExceptHeaderGroupParameter = isDefinedExceptHeaderGroupParameter;
		DataProvider = new DataViewDefinitionsDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		if (string.IsNullOrWhiteSpace(_group))
		{
			return;
		}
		Columns["Group"].Visible = false;
		Columns["GroupName"].Visible = false;
		Columns["Default"].Visible = false;
		Columns["Enabled"].Visible = false;
		Columns["LastExecutionTime"].Visible = false;
		Columns["System"].Visible = false;
		Columns["DropDown"].Visible = false;
		if (!_isExceptHeaderGroup && !_isDefinedExceptHeaderGroupParameter)
		{
			bool flag = true;
			Type instanceType = DefinitionMappingsService.ResolveDVS(_group);
			IDataViewSettingsHandler dataViewSettingsHandler = (IDataViewSettingsHandler)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, instanceType);
			if (dataViewSettingsHandler != null)
			{
				flag = dataViewSettingsHandler.DefaultViewEnabled();
			}
			_isExceptHeaderGroup = !flag;
		}
		if (_isExceptHeaderGroup)
		{
			List<string> list = Settings.ServiceProvider.GetService<IDVDService>().Load(_group);
			if (list.Count > 0)
			{
				_defaultDataViewCode = list[0];
			}
		}
		Columns.AddSorting("Name");
	}

	protected override void ApplyParametersToDataProvider()
	{
		base.ApplyParametersToDataProvider();
		ParametersProvider.Group = _group;
		ParametersProvider.ProviderType = _providerType;
	}

	public override short ValueMemberLength()
	{
		return 30;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
