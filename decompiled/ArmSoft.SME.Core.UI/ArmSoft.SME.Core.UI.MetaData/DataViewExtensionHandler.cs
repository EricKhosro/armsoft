using System.Collections;
using System.Collections.Generic;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Core.UI.MetaData;

public abstract class DataViewExtensionHandler<T> : IDataProviderExtensionHandler where T : ExtendableRow
{
	private T _row;

	private Dictionary<string, object> _parameters;

	public T Row => _row;

	public Dictionary<string, object> Parameters => _parameters;

	object IDataProviderExtensionHandler.InnerHandler => this;

	void IDataProviderExtensionHandler.SetRow(ExtendableRow row)
	{
		_row = (T)row;
	}

	public virtual void BeforeLoad(bool updateRows)
	{
	}

	public virtual bool OnEachRow()
	{
		return true;
	}

	public void EditRows(IEnumerable rows)
	{
		EditRows(rows as List<T>);
	}

	public virtual void EditRows(List<T> rows)
	{
	}

	public virtual List<AsFormatting> ConditionalFormatting()
	{
		return new List<AsFormatting>();
	}

	void IDataProviderExtensionHandler.SetParameters(Dictionary<string, object> parameters)
	{
		_parameters = parameters;
	}

	public virtual string Caption()
	{
		return null;
	}

	public virtual string CaptionToolTip()
	{
		return Caption();
	}
}
