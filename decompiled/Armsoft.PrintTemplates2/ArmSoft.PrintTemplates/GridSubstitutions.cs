using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmSoft.PrintTemplates;

public abstract class GridSubstitutions<T> : IGridSubstitutions where T : IPrintTemplateSubstitutionGrid
{
	public Dictionary<string, T> Substitutions { get; private set; }

	Dictionary<string, IPrintTemplateSubstitutionGrid> IGridSubstitutions.Substitutions => ((IEnumerable<KeyValuePair<string, T>>)Substitutions).ToDictionary((Func<KeyValuePair<string, T>, string>)((KeyValuePair<string, T> x) => x.Key), (Func<KeyValuePair<string, T>, IPrintTemplateSubstitutionGrid>)((KeyValuePair<string, T> y) => y.Value));

	public GridSubstitutions()
	{
		Substitutions = new Dictionary<string, T>();
	}

	public int Count()
	{
		return Substitutions.Count;
	}

	public bool Contains(string name)
	{
		return Substitutions.ContainsKey(name);
	}

	public T Value(string name)
	{
		return Substitutions[name];
	}

	public T Add(string name)
	{
		T val = (T)Activator.CreateInstance(typeof(T), name);
		Substitutions.Add(name, val);
		return val;
	}

	IPrintTemplateSubstitutionGrid IGridSubstitutions.Value(string name)
	{
		return Substitutions[name];
	}

	void IGridSubstitutions.ConvertToArmenianUnicode()
	{
		if (Substitutions == null)
		{
			return;
		}
		foreach (KeyValuePair<string, T> substitution in Substitutions)
		{
			substitution.Value.ConvertToArmenianUnicode();
		}
	}

	IPrintTemplateSubstitutionGrid IGridSubstitutions.Add(string name)
	{
		return Add(name);
	}
}
public sealed class GridSubstitutions : GridSubstitutions<PrintTemplateSubstitutionGrid>
{
}
