using System;
using System.IO;

namespace ArmSoft.OnlineBanking.Common.Utility;

public class CsvExportWriter : IDisposable
{
	private readonly TextWriter writer;

	private bool disposed;

	public CsvExportWriter(TextWriter writer)
	{
		this.writer = writer ?? throw new ArgumentNullException("writer");
	}

	public void Write(string value)
	{
		writer.Write(EscapeCsv(value));
		writer.Write(",");
	}

	public void WriteFullRow(params string[] values)
	{
		if (values != null)
		{
			foreach (string value in values)
			{
				Write(value);
			}
		}
		writer.WriteLine();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				writer?.Dispose();
			}
			disposed = true;
		}
	}

	private string EscapeCsv(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return string.Empty;
		}
		if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
		{
			value = value.Replace("\"", "\"\"");
			return "\"" + value + "\"";
		}
		return value;
	}
}
