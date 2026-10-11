using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.SmartEnums;

namespace ArmSoft.SME.Client.Data;

public class SmartEnumDataProvider : DataProvider<SmartEnumDataProvider.DataRow, SmartEnumDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string ValueUI { get; set; }

		public string Caption { get; set; }
	}

	public class Param
	{
		public List<string> Values { get; set; }

		public List<string> ExcludeValues { get; set; }
	}

	private string _codeColumnCaption;

	private string _captionColumnCaption;

	private readonly List<Type> _smartEnumTypes;

	private readonly bool _padLeft;

	private readonly char _paddingChar;

	private readonly int _totalWidth;

	private readonly ValueMember _valueMember;

	private readonly CaptionMember _captionMember;

	public override string Name => "SmartEnum";

	public SmartEnumDataProvider(Type type, ValueMember valueMember = ValueMember.Code, CaptionMember captionMember = CaptionMember.Caption, bool padLeft = false, int totalWidth = 0, char paddingChar = '0', string codeColumnCaption = null, string captionColumnCaption = null)
		: base((ApiClient)null)
	{
		_smartEnumTypes = new List<Type> { type };
		_valueMember = valueMember;
		_captionMember = captionMember;
		_padLeft = padLeft;
		_paddingChar = paddingChar;
		_totalWidth = totalWidth;
		_codeColumnCaption = codeColumnCaption;
		_captionColumnCaption = captionColumnCaption;
	}

	public SmartEnumDataProvider(List<Type> type, ValueMember valueMember = ValueMember.Code, CaptionMember captionMember = CaptionMember.Caption, bool padLeft = false, int totalWidth = 0, char paddingChar = '0', string codeColumnCaption = null, string captionColumnCaption = null)
		: base((ApiClient)null)
	{
		_smartEnumTypes = type;
		_valueMember = valueMember;
		_captionMember = captionMember;
		_padLeft = padLeft;
		_paddingChar = paddingChar;
		_totalWidth = totalWidth;
		_codeColumnCaption = codeColumnCaption;
		_captionColumnCaption = captionColumnCaption;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = "SmartEnum",
			Caption = "SmartEnum"
		};
		if (_codeColumnCaption == null)
		{
			_codeColumnCaption = Resources.Code;
		}
		if (_captionColumnCaption == null)
		{
			_captionColumnCaption = Resources.Name;
		}
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Code",
				Caption = _codeColumnCaption,
				Source = "Code"
			},
			new Column
			{
				Name = "ValueUI",
				Caption = _codeColumnCaption,
				Source = "ValueUI"
			},
			new Column
			{
				Name = "Caption",
				Caption = _captionColumnCaption,
				Source = "Caption"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		foreach (Type smartEnumType in _smartEnumTypes)
		{
			foreach (dynamic smartEnumField in smartEnumType.GetSmartEnumFields())
			{
				bool flag = param == null;
				if (!flag && !((flag | ((param.Values == null || param.Values.Count == 0 || param.Values.Contains(smartEnumField.Value.ToString())) && (param.ExcludeValues == null || param.ExcludeValues.Count == 0 || !param.ExcludeValues.Contains(smartEnumField.Value.ToString())))) ? true : false))
				{
					continue;
				}
				DataRow dataRow2;
				if (_valueMember == ValueMember.Code)
				{
					DataRow dataRow = new DataRow();
					dataRow.Code = smartEnumField.Value.ToString();
					dataRow.Caption = ((_captionMember == CaptionMember.Caption) ? smartEnumField.Caption : smartEnumField.FullCaption);
					dataRow2 = dataRow;
					if (_padLeft)
					{
						dataRow2.Code = dataRow2.Code.PadLeft(_totalWidth, _paddingChar);
					}
				}
				else if (_valueMember == ValueMember.ValueUI)
				{
					DataRow dataRow = new DataRow();
					dataRow.Code = smartEnumField.Value.ToString();
					dataRow.ValueUI = smartEnumField.ValueUI.ToString();
					dataRow.Caption = ((_captionMember == CaptionMember.Caption) ? smartEnumField.Caption : smartEnumField.FullCaption);
					dataRow2 = dataRow;
				}
				else
				{
					DataRow dataRow = new DataRow();
					dataRow.Code = ((_captionMember == CaptionMember.Caption) ? smartEnumField.Caption : smartEnumField.FullCaption);
					dataRow.Caption = smartEnumField.Value.ToString();
					dataRow2 = dataRow;
				}
				dataProviderResult.Rows.Add(dataRow2);
			}
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
