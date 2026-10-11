using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Data;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.DataViews;

public abstract class View<R, P> : IView where R : ExtendableRow, new() where P : class, new()
{
	private Schema _schemaProvider;

	public P ParametersProvider { get; set; } = new P();

	public DataProvider<R, P> DataProvider { get; set; }

	public HashSet<string> ColumnsProvider { get; set; } = new HashSet<string>();

	public virtual bool WithoutAccessCheck { get; }

	public bool DataLoadedAtLeastOnce { get; set; }

	protected byte[] CachedTS { get; set; }

	public virtual string Name => SchemaProvider.Name;

	public virtual string DataViewGroup => SchemaProvider.Name;

	public virtual string Caption
	{
		get
		{
			return SchemaProvider.Caption;
		}
		set
		{
			SchemaProvider.Caption = value;
		}
	}

	public virtual string HelpId { get; }

	protected ObservableCollection<R> Data { get; } = new ObservableCollection<R>();

	public Dictionary<string, DataViewParameter> Parameters { get; } = new Dictionary<string, DataViewParameter>();

	public string FilterString { get; set; }

	public string ConditionalFormatting { get; }

	public ViewColumnsCollection Columns { get; } = new ViewColumnsCollection();

	public virtual int FetchSize => 5000;

	public virtual int FirstFetchSize => 100;

	public Schema SchemaProvider
	{
		get
		{
			if (_schemaProvider == null)
			{
				_schemaProvider = DataProvider.GetSchema();
			}
			return _schemaProvider;
		}
	}

	IList IView.Data => Data;

	object IView.ParametersProvider => ParametersProvider;

	public int RowsCount => Data.Count;

	public R GetRowByIndex(int index)
	{
		return Data[index];
	}

	public bool TryGetRowByIndex(int index, out R value)
	{
		value = null;
		if (index >= 0 && index < Data.Count)
		{
			value = Data[index];
			return true;
		}
		return false;
	}

	public Dictionary<string, string> GetKeyValuesDict()
	{
		return null;
	}

	public void LoadData(LoadSource source = LoadSource.None)
	{
		if (Data.Count > 0)
		{
			Data.Clear();
		}
		SetDataProviderProperties();
		ExecutionSettings settings = null;
		if (source != LoadSource.None)
		{
			settings = new ExecutionSettings
			{
				ReloadData = true
			};
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile(DataProvider.GetType().Name + " data provider execute start");
		}
		List<R> rows = DataProvider.Execute(ParametersProvider, settings, null, WithoutAccessCheck, ColumnsProvider).Rows;
		foreach (R item in rows)
		{
			Data.Add(item);
		}
		rows.Clear();
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile(DataProvider.GetType().Name + " data provider execute finished");
		}
		DataLoadedAtLeastOnce = true;
	}

	protected void SetDataProviderProperties()
	{
		DataProvider.Client = ((!Settings.IsOffline && Settings.LoginService.IsLoggedIn) ? Settings.CreateApiClient() : null);
		ApplyParametersToDataProvider();
		ColumnsProvider.Clear();
		SetDataProviderColumns();
		SetColumnsVisibleByDataProvider();
	}

	protected virtual void ApplyParametersToDataProvider()
	{
	}

	protected virtual void SetDataProviderColumns()
	{
	}

	protected virtual void SetColumnsVisibleByDataProvider()
	{
	}

	protected void FillColumnsFromDataProvider()
	{
		foreach (Column column in SchemaProvider.Columns)
		{
			if (column.SourceType == ColumnSourceType.Dynamic)
			{
				Columns.AddDynamic(column.Name, column.Caption, column.Source, column.Type.CastToDataColumnType(), column.FooterSum, column.After, column.Before, isExtended: false, column.Format);
			}
			else if (!column.DataOnly)
			{
				Columns.Add(column);
			}
		}
		SetColumnsProperties();
	}

	protected virtual void SetColumnsProperties()
	{
	}

	protected void FillDynamicColumnsFromDataProvider(Schema schema)
	{
		Columns.RemoveAllDynamicColumns();
		foreach (Column column in schema.Columns)
		{
			if (column.SourceType == ColumnSourceType.Dynamic)
			{
				Columns.AddDynamic(column.Name, column.Caption, column.Source, column.Type.CastToDataColumnType(), column.FooterSum, column.After, column.Before, isExtended: false, column.Format);
			}
		}
	}

	public virtual bool TreeViewAllowed(ref bool setIsDefault)
	{
		setIsDefault = false;
		return false;
	}

	public virtual string KeyFieldName()
	{
		return "";
	}

	public virtual string ParentFieldName()
	{
		return "";
	}

	public virtual string IsLeafFieldName()
	{
		return "";
	}

	public virtual bool ShowExpanded()
	{
		return false;
	}

	public DataColumnType GetColumnType(string name)
	{
		if (Columns.Contains(name))
		{
			ViewColumn viewColumn = Columns[name];
			if (viewColumn.Type == DataColumnType.Unknown)
			{
				PropertyInfo property = typeof(R).GetProperty(viewColumn.Source);
				if (property != null)
				{
					Type propertyType = property.PropertyType;
					if (propertyType == typeof(string))
					{
						viewColumn.Type = DataColumnType.String;
					}
					else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
					{
						viewColumn.Type = DataColumnType.DateTime;
					}
					else if (propertyType == typeof(DateOnly) || propertyType == typeof(DateOnly?))
					{
						viewColumn.Type = DataColumnType.DateOnly;
					}
					else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
					{
						viewColumn.Type = DataColumnType.Decimal;
					}
					else if (propertyType == typeof(bool))
					{
						viewColumn.Type = DataColumnType.Boolean;
					}
					else if (propertyType == typeof(int))
					{
						viewColumn.Type = DataColumnType.Integer;
					}
					else if (propertyType == typeof(long))
					{
						viewColumn.Type = DataColumnType.Integer;
					}
					else if (propertyType == typeof(byte[]))
					{
						viewColumn.Type = DataColumnType.ButtonToolTip;
					}
					else
					{
						viewColumn.Type = DataColumnType.String;
					}
				}
			}
			return viewColumn.Type;
		}
		throw new InvalidOperationException("Invalid column name " + name);
	}
}
