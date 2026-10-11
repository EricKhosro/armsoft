using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Client.Data;

public class b2bUnitedProductsDataProvider : DataProvider<b2bUnitedProductRowBase, b2bUnitedProductsDataProvider.Param>
{
	public class Param
	{
	}

	private readonly List<b2bUnitedProductRowBase> _rows;

	public override string Name => "b2bUnitedProducts";

	public b2bUnitedProductsDataProvider(List<b2bUnitedProductRowBase> rows)
		: base((ApiClient)null)
	{
		_rows = rows;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = "b2bUnitedProducts",
			Caption = "b2bUnitedProducts"
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Barcode",
				Caption = Resources.BarCode,
				Source = "Barcode"
			},
			new Column
			{
				Name = "Name",
				Caption = Resources.Name,
				Source = "Name"
			},
			new Column
			{
				Name = "FullName",
				Caption = Resources.FullName,
				Source = "FullName"
			},
			new Column
			{
				Name = "CPACode",
				Caption = Resources.CPACodeShort,
				Source = "CPACode"
			},
			new Column
			{
				Name = "CPACaption",
				Caption = Resources.CPACaption,
				Source = "CPACaption"
			},
			new Column
			{
				Name = "IsWeight",
				Caption = Resources.IsWeight,
				Source = "IsWeight"
			},
			new Column
			{
				Name = "IsVatTaxable",
				Caption = Resources.OptionsVAT,
				Source = "IsVatTaxable"
			},
			new Column
			{
				Name = "Description",
				Caption = Resources.Description,
				Source = "Description"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<b2bUnitedProductRowBase> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		return new DataProviderResult<b2bUnitedProductRowBase>
		{
			Rows = _rows
		};
	}

	public override Task<DataProviderResult<b2bUnitedProductRowBase>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
