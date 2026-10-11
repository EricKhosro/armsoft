using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ArmSoft.b2b.V2;

public class FieldsFilter<T>
{
	private Expression<Func<T, object>> _filter;

	public FieldsFilter(Expression<Func<T, object>> filter)
	{
		_filter = filter;
	}

	public List<string> GetFieldNames()
	{
		if (_filter == null)
		{
			return null;
		}
		string text = _filter.Body.ToString();
		int num = text.IndexOf("(") + 1;
		int num2 = text.LastIndexOf(")");
		return (from item in text.Substring(num, num2 - num).Split(',')
			select Regex.Replace(item.Split('=')[1].Trim(), "^" + _filter.Parameters[0].Name + ".", "")).ToList();
	}
}
