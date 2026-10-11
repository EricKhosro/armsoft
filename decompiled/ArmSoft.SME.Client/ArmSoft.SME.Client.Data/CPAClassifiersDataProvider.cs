using System;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class CPAClassifiersDataProvider : DataProvider<CPAClassifiersDataProvider.DataRow, CPAClassifiersDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Type { get; set; }

		public string TypeName { get; set; }

		public DateOnly? MarkInitDate { get; set; }

		public DataRow()
		{
		}

		public DataRow(string code, string caption, string type, string typeName, DateOnly? markInitDate, byte[] ts)
		{
			Code = code;
			Caption = caption;
			Type = type;
			TypeName = typeName;
			MarkInitDate = markInitDate;
		}

		public object GetKey()
		{
			return Code;
		}

		public string GetCode()
		{
			return Code;
		}

		public string GetName()
		{
			return Caption;
		}

		public bool Matched(object key)
		{
			return key.ToString() == Code;
		}
	}

	public class Param : ExtendableParameter
	{
	}

	public override string Name => "CPAClassifiers";

	public CPAClassifiersDataProvider(ApiClient client)
		: base(client)
	{
	}
}
