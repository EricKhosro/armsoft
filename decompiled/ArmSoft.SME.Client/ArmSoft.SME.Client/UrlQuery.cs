using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ArmSoft.SME.Client;

public class UrlQuery
{
	private readonly List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();

	public void Add(string name, string value, bool addEmpty = false)
	{
		if (addEmpty || !string.IsNullOrEmpty(value))
		{
			parameters.Add(new Tuple<string, string>(name, Uri.EscapeDataString(value)));
		}
	}

	public void Add(string name, long? value)
	{
		if (value.HasValue)
		{
			Add(name, value.Value);
		}
	}

	public void Add(string name, long value)
	{
		parameters.Add(new Tuple<string, string>(name, Uri.EscapeDataString(value.ToString(CultureInfo.InvariantCulture))));
	}

	public void Add(string name, decimal value)
	{
		parameters.Add(new Tuple<string, string>(name, Uri.EscapeDataString(value.ToString(CultureInfo.InvariantCulture))));
	}

	public void Add(string name, decimal? value)
	{
		if (value.HasValue)
		{
			Add(name, value.Value);
		}
	}

	public void Add(string name, int? value)
	{
		if (value.HasValue)
		{
			Add(name, value.Value);
		}
	}

	public void Add(string name, int value)
	{
		parameters.Add(new Tuple<string, string>(name, Uri.EscapeDataString(value.ToString(CultureInfo.InvariantCulture))));
	}

	public void Add(string name, DateTime? value)
	{
		if (value.HasValue)
		{
			Add(name, value.Value);
		}
	}

	public void Add(string name, DateTime value)
	{
		parameters.Add(new Tuple<string, string>(name, Uri.EscapeDataString(value.ToString("s", CultureInfo.InvariantCulture))));
	}

	public void Add(string name, DateOnly? value)
	{
		if (value.HasValue)
		{
			Add(name, value.Value);
		}
	}

	public void Add(string name, DateOnly value)
	{
		parameters.Add(new Tuple<string, string>(name, Uri.EscapeDataString(value.ToString("o", CultureInfo.InvariantCulture))));
	}

	public void Add(string name, bool? value)
	{
		if (value.HasValue)
		{
			Add(name, value.Value);
		}
	}

	public void Add(string name, bool value)
	{
		parameters.Add(new Tuple<string, string>(name, value ? "true" : "false"));
	}

	public void Add(string name, Guid? value)
	{
		if (value.HasValue)
		{
			Add(name, value.Value);
		}
	}

	public void Add(string name, Guid value)
	{
		parameters.Add(new Tuple<string, string>(name, value.ToString("N")));
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		AppendTo(stringBuilder);
		return stringBuilder.ToString();
	}

	public void AppendTo(StringBuilder uriBuilder)
	{
		if (parameters.Count <= 0)
		{
			return;
		}
		uriBuilder.Append('?');
		bool flag = true;
		foreach (Tuple<string, string> parameter in parameters)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				uriBuilder.Append('&');
			}
			uriBuilder.Append(parameter.Item1);
			uriBuilder.Append('=');
			uriBuilder.Append(parameter.Item2);
		}
	}
}
