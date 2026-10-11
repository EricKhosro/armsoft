using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI;

public static class StoredValuesExtensions
{
	public static void Add(this List<DialogWindow.StoreValueDescriptor> storedValues, string name, string caption, bool storeValue)
	{
		DialogWindow.StoreValueDescriptor item = new DialogWindow.StoreValueDescriptor
		{
			Name = name,
			DefaultStoreFlag = storeValue,
			ActualStoreFlag = storeValue,
			Caption = caption.Replace("\r\n", ""),
			ShareStoreFlagWithPreviousControl = string.IsNullOrWhiteSpace(caption)
		};
		storedValues.Add(item);
	}

	public static void Remove(this List<DialogWindow.StoreValueDescriptor> storedValues, string name)
	{
		for (int i = 0; i <= storedValues.Count - 1; i++)
		{
			if (storedValues[i].Name == name)
			{
				storedValues.RemoveAt(i);
				break;
			}
		}
	}

	public static DialogWindow.StoreValueDescriptor GetItem(this List<DialogWindow.StoreValueDescriptor> storedValues, string name)
	{
		for (int i = 0; i <= storedValues.Count - 1; i++)
		{
			if (storedValues[i].Name == name)
			{
				return storedValues[i];
			}
		}
		return null;
	}
}
