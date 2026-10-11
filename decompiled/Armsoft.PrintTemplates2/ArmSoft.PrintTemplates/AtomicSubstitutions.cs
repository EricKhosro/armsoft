using System;
using System.Collections.Generic;
using ArmSoft.AS8X.Common.Language.Extensions;
using ArmSoft.PrintTemplates.Text;
using Armsoft.PrintTemplates2.Helper;

namespace ArmSoft.PrintTemplates;

public sealed class AtomicSubstitutions : AtomicSubstitutions<string>
{
	public override void Add(string code, string value)
	{
		base.Add(code, StringConverter.RemoveXMLRestrictedCharacters(value));
	}
}
public abstract class AtomicSubstitutions<T> : IAtomicSubstitutions
{
	public Dictionary<string, object> Substitutions { get; private set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

	public int Count()
	{
		return Substitutions.Count;
	}

	public bool Contains(string code)
	{
		return Substitutions.ContainsKey(code);
	}

	public T Value(string code)
	{
		return GetValue(code).ToGeneric<T>();
	}

	public virtual void Add(string code, T value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, object value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, bool value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, byte value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, char value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, decimal value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, double value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, float value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, int value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, long value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, short value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, DateTime value)
	{
		Substitutions.Add(code, value);
	}

	public void Add(string code, DateTime? value)
	{
		Substitutions.Add(code, (!value.HasValue) ? string.Empty : ((object)value.Value));
	}

	public void Add(string code, TimeSpan? value)
	{
		Substitutions.Add(code, (!value.HasValue) ? string.Empty : ((object)value.Value));
	}

	object IAtomicSubstitutions.Value(string code)
	{
		return GetValue(code);
	}

	private object GetValue(string code)
	{
		if (Substitutions.TryGetValue(code, out var value))
		{
			return value;
		}
		return string.Empty;
	}

	void IAtomicSubstitutions.ConvertToArmenianUnicode()
	{
		if (Substitutions == null)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		foreach (KeyValuePair<string, object> substitution in Substitutions)
		{
			if (substitution.Value is string)
			{
				dictionary.Add(substitution.Key, substitution.Value.ToString().ToArmenianUnicode());
			}
			else
			{
				dictionary.Add(substitution.Key, substitution.Value);
			}
		}
		Substitutions = dictionary;
	}

	List<AtomicSubstitution> IAtomicSubstitutions.GetSortedByLengthSubstitutions()
	{
		List<AtomicSubstitution> list = new List<AtomicSubstitution>();
		if (Substitutions.Count > 0)
		{
			AtomicSubstitution[] array = new AtomicSubstitution[Substitutions.Count];
			int num = 0;
			foreach (string key in Substitutions.Keys)
			{
				array[num] = new AtomicSubstitution(key, Substitutions[key]);
				num++;
			}
			Array.Sort(array);
			list.AddRange(array);
		}
		return list;
	}
}
