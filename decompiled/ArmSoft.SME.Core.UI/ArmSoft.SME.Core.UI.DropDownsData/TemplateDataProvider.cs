using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class TemplateDataProvider : DataProvider<TemplateDataProvider.DataRow, TemplateDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public string FileFormat { get; set; }
	}

	public class Param
	{
		public short DocType { get; set; }

		public string FileFormat { get; set; }

		public IReadOnlyList<TemplatesCacheRow> Templates { get; set; }
	}

	public override string Name => "Template";

	public TemplateDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.PrintTemplates
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Name",
				Caption = Resources.Code,
				Source = "Name"
			},
			new Column
			{
				Name = "Caption",
				Caption = Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "FileFormat",
				Caption = Resources.PrintTemplateType,
				Source = "FileFormat"
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
		if (settings != null && settings.ReloadData)
		{
			ArmSoft.SME.Core.UI.Settings.CoreCacheService.Template.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		IReadOnlyList<TemplatesCacheRow> readOnlyList;
		if (param.DocType > 0)
		{
			readOnlyList = ArmSoft.SME.Core.UI.Settings.CoreCacheService.Template.GetTemplates(param.DocType);
		}
		else
		{
			readOnlyList = ((param.Templates == null) ? ArmSoft.SME.Core.UI.Settings.CoreCacheService.Template.GetList() : param.Templates);
		}
		if (param.FileFormat != null)
		{
			List<TemplatesCacheRow> list = new List<TemplatesCacheRow>();
			list.AddRange(readOnlyList.Where((TemplatesCacheRow cr) => cr.FileFormat == param.FileFormat));
			readOnlyList = new _003C_003Ez__ReadOnlyList<TemplatesCacheRow>(list);
		}
		foreach (TemplatesCacheRow item2 in readOnlyList)
		{
			DataRow item = new DataRow
			{
				Name = item2.Name,
				Caption = item2.Caption,
				FileFormat = item2.FileFormat?.Caption
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.Name).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
