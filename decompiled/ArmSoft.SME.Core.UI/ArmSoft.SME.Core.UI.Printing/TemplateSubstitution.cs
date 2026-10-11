using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using ArmSoft.PrintTemplates;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Core.UI.Printing;

public class TemplateSubstitution
{
	private readonly IParametersService _parametersService;

	public PrintTemplateSubstitution PrintTemplateSubstitution { get; }

	public TemplateSubstitution()
	{
		PrintTemplateSubstitution = new PrintTemplateSubstitution();
	}

	public TemplateSubstitution(IParametersService parametersService, DocumentPrintWrapper doc)
		: this()
	{
		_parametersService = parametersService;
		DocumentTemplateSubstitutionDoRecursive(doc, "", -1);
	}

	private void DocumentTemplateSubstitutionDoRecursive(object sourceDoc, string typePrephix, int rowNum)
	{
		PropertyInfo[] properties = sourceDoc.GetType().GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(PrintDataMemberAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				continue;
			}
			PrintDataMemberAttribute printDataMemberAttribute = (PrintDataMemberAttribute)customAttributes[0];
			string text = propertyInfo.Name;
			if (propertyInfo.PropertyType.IsSealed)
			{
				AddAtomicSubstitution(text, propertyInfo.GetValue(sourceDoc, null), typePrephix, rowNum, propertyInfo.PropertyType, printDataMemberAttribute.DecimalPrecisionType);
			}
			else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
			{
				IEnumerable enumerable = ((IEnumerable)propertyInfo.GetValue(sourceDoc, null)).Cast<object>();
				int num = 0;
				if (!string.IsNullOrEmpty(typePrephix))
				{
					text = typePrephix + "." + text;
				}
				if (!PrintTemplateSubstitution.GridSubstitutions.Contains(text))
				{
					Type obj = enumerable.GetType().GetGenericArguments()[0];
					PrintTemplateSubstitution.GridSubstitutions.Add(text);
					PrintTemplateSubstitutionGrid printTemplateSubstitutionGrid = PrintTemplateSubstitution.GridSubstitutions.Value(text);
					PropertyInfo[] properties2 = obj.GetProperties();
					foreach (PropertyInfo propertyInfo2 in properties2)
					{
						printTemplateSubstitutionGrid.AddColumn(propertyInfo2.Name, propertyInfo2.Name);
					}
				}
				foreach (object item in enumerable)
				{
					PropertyInfo[] properties2 = item.GetType().GetProperties();
					foreach (PropertyInfo propertyInfo3 in properties2)
					{
						AddAtomicSubstitution(propertyInfo3.Name, propertyInfo3.GetValue(item, null), text, num, propertyInfo3.PropertyType, printDataMemberAttribute.DecimalPrecisionType);
					}
					num++;
				}
			}
			else if (propertyInfo.PropertyType.IsClass)
			{
				if (!string.IsNullOrEmpty(typePrephix))
				{
					text = typePrephix + "." + text;
				}
				if (propertyInfo.GetValue(sourceDoc, null) != null)
				{
					DocumentTemplateSubstitutionDoRecursive(propertyInfo.GetValue(sourceDoc, null), text, -1);
				}
				else
				{
					AddAtomicSubstitution(text, propertyInfo.GetValue(sourceDoc, null), typePrephix, rowNum, propertyInfo.PropertyType);
				}
			}
		}
	}

	private void AddAtomicSubstitution(string prephix, object value, string typePrephix, int rowNum, Type valueType, DecimalPrecisionType decimalPrecisionType = DecimalPrecisionType.TwoDigits)
	{
		if (rowNum == -1)
		{
			typePrephix = ((!string.IsNullOrEmpty(typePrephix)) ? (typePrephix + "." + prephix) : prephix);
			PrintTemplateSubstitution.AtomicSubstitutions.Add(typePrephix, ConvertToString(value, valueType, decimalPrecisionType));
			return;
		}
		PrintTemplateSubstitutionGrid printTemplateSubstitutionGrid = PrintTemplateSubstitution.GridSubstitutions.Value(typePrephix);
		if (printTemplateSubstitutionGrid == null)
		{
			PrintTemplateSubstitution.GridSubstitutions.Add(typePrephix);
			printTemplateSubstitutionGrid = PrintTemplateSubstitution.GridSubstitutions.Value(typePrephix);
		}
		while (printTemplateSubstitutionGrid.RowCount - 1 < rowNum)
		{
			printTemplateSubstitutionGrid.AddRow();
		}
		printTemplateSubstitutionGrid.SetValue(rowNum, prephix, ConvertToString(value, valueType, decimalPrecisionType));
	}

	protected virtual string ConvertToString(object value, Type valueType, DecimalPrecisionType decimalPrecisionType = DecimalPrecisionType.TwoDigits)
	{
		if (value == null)
		{
			return "";
		}
		if (valueType == typeof(string))
		{
			return value.ToString();
		}
		if (valueType == typeof(DateTime))
		{
			return Convert.ToDateTime(value).FormatDDMMYY();
		}
		if (valueType == typeof(DateTime?))
		{
			DateTime? dateTime = (DateTime?)value;
			if (dateTime.HasValue)
			{
				return dateTime.Value.FormatDDMMYY();
			}
			return "";
		}
		if (valueType == typeof(decimal))
		{
			if (Convert.ToDecimal(value) == 0m)
			{
				return "";
			}
			return decimalPrecisionType switch
			{
				DecimalPrecisionType.Quantity => Convert.ToDecimal(value).CurrencyFormat(13, 3), 
				DecimalPrecisionType.Sum => Convert.ToDecimal(value).CurrencyFormat(16, 2), 
				DecimalPrecisionType.SumFromParam => Convert.ToDecimal(value).CurrencyFormat(ArmSoft.SME.Common.Constants.Common.LenSumFromParam(_parametersService), ArmSoft.SME.Common.Constants.Common.LenSumPrecisionFromParam(_parametersService)), 
				DecimalPrecisionType.Price => Convert.ToDecimal(value).CurrencyFormat(16, 2), 
				DecimalPrecisionType.PriceFromParam => Convert.ToDecimal(value).CurrencyFormat(ArmSoft.SME.Common.Constants.Common.LenPriceFromParam(_parametersService), ArmSoft.SME.Common.Constants.Common.LenPricePrecisionFromParam(_parametersService)), 
				_ => Convert.ToDecimal(value).CurrencyFormat(16, 2), 
			};
		}
		if (valueType == typeof(int))
		{
			if (Convert.ToDecimal(value) == 0m)
			{
				return "";
			}
			return Convert.ToDecimal(value).CurrencyFormat(16, 0);
		}
		return value.ToString();
	}
}
