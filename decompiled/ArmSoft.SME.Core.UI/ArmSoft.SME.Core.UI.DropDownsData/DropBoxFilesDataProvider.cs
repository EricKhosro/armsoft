using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace ArmSoft.SME.Core.UI.DropDownsData;

public class DropBoxFilesDataProvider : DataProvider<DropBoxFilesDataProvider.DataRow, DropBoxFilesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Name { get; set; }

		public decimal Size { get; set; }

		public DateTime ModifiedDate { get; set; }

		public DataRow()
		{
		}

		public DataRow(FileMetadata fileMetadata)
		{
			Name = fileMetadata.Name;
			Size = ((fileMetadata.Size != 0L) ? ((decimal)fileMetadata.Size / 1048576m).RoundAs(2) : 0m);
			ModifiedDate = fileMetadata.ServerModified.ToLocalTime();
		}
	}

	public class Param
	{
	}

	private readonly string _token;

	public override string Name => "DropBoxFiles";

	public DropBoxFilesDataProvider(string token)
		: base((ApiClient)null)
	{
		_token = token;
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.FilesFromDropBox
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Name",
				Caption = Resources.Database,
				Source = "Name"
			},
			new Column
			{
				Name = "Size",
				Caption = Resources.CompressedSize,
				Source = "Size",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "ModifiedDate",
				Caption = Resources.DateWord,
				Source = "ModifiedDate",
				Type = SupportedType.DateTime,
				Format = "dd/MM/yy"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override async Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		DataProviderResult<DataRow> result = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		using (DropboxClient dbx = new DropboxClient(_token))
		{
			ListFolderResult listFolderResult = await dbx.Files.ListFolderAsync("");
			string backupExtension = "." + ArmSoft.SME.Core.UI.Settings.BackupExtension;
			foreach (Metadata entry in listFolderResult.Entries)
			{
				if (entry.IsFile && entry.Name.EndsWith(backupExtension))
				{
					result.Rows.Add(new DataRow(entry.AsFile));
				}
			}
			while (listFolderResult.HasMore)
			{
				listFolderResult = await dbx.Files.ListFolderContinueAsync(listFolderResult.Cursor);
				foreach (Metadata entry2 in listFolderResult.Entries)
				{
					if (entry2.IsFile && entry2.Name.EndsWith(backupExtension))
					{
						result.Rows.Add(new DataRow(entry2.AsFile));
					}
				}
			}
		}
		return result;
	}
}
