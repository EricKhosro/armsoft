using System.Collections.Generic;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Core.UI.MetaData;

public abstract class ContextFunctionHandler<T> : IContextFunctionHandler where T : IDynamicValue
{
	private readonly List<T> _rows = new List<T>();

	public List<T> Rows => _rows;

	object IContextFunctionHandler.InnerHandler => this;

	void IContextFunctionHandler.SetRows(List<IDynamicValue> rows)
	{
		foreach (IDynamicValue row in rows)
		{
			_rows.Add((T)row);
		}
	}

	public virtual void UserDefinedContextFunction(IDataViewForExtensions idv)
	{
	}

	public ContextFunctionDialog CreateDialog()
	{
		return new ContextFunctionDialog();
	}
}
