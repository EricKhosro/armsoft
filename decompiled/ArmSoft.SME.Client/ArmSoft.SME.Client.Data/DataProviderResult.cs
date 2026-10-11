using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.Error;
using ArmSoft.SME.Common.Models.Job;

namespace ArmSoft.SME.Client.Data;

public class DataProviderResult<R> where R : ExtendableRow, new()
{
	public List<R> Rows { get; set; } = new List<R>();

	public Progress Progress { get; set; }

	public string ErrorMessage { get; set; }

	public SQLiteRowsInfo RowsDb { get; set; }

	public R ErrorRow { get; set; }

	public Guid FetchID { get; set; }

	public int TotalRows { get; set; }

	public DataProviderRowsBinaryReader Reader { get; set; }

	public List<ErrorDetail> ErrorDetails { get; set; }

	public bool ErrorIsRestException { get; set; }

	public string ErrorMessageTitle { get; set; }
}
public class DataProviderResult
{
	public List<object> Rows { get; set; }

	public DPProgress Progress { get; set; }

	public string ErrorMessage { get; set; }

	public SQLiteRowsInfo RowsDb { get; set; }

	public List<Column> Columns { get; set; }

	public Guid FetchID { get; set; }

	public int TotalRows { get; set; }

	public DataProviderRowsBinaryReader Reader { get; set; }

	public List<ErrorDetail> ErrorDetails { get; set; }

	public bool ErrorIsRestException { get; set; }

	public string ErrorMessageTitle { get; set; }
}
