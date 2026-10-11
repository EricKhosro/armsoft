using System;
using System.Collections.Generic;

namespace ArmSoft.PrintTemplates;

public interface IAtomicSubstitutions
{
	Dictionary<string, object> Substitutions { get; }

	bool Contains(string code);

	int Count();

	object Value(string code);

	internal void ConvertToArmenianUnicode();

	internal List<AtomicSubstitution> GetSortedByLengthSubstitutions();

	void Add(string code, object value);

	void Add(string code, bool value);

	void Add(string code, byte value);

	void Add(string code, char value);

	void Add(string code, decimal value);

	void Add(string code, double value);

	void Add(string code, float value);

	void Add(string code, int value);

	void Add(string code, long value);

	void Add(string code, short value);

	void Add(string code, DateTime value);

	void Add(string code, DateTime? value);

	void Add(string code, TimeSpan? value);
}
