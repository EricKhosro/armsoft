using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Armsoft.PrintTemplates2.Resources;

namespace ArmSoft.PrintTemplates;

public class TextTemplateSubstitutor : TemplateSubstitutor, IContentProvider
{
	protected readonly Regex TableRegEx = new Regex("%(?<TableName>[\\w]+)\\.(?<ColumnName>[\\w]+)", RegexOptions.Compiled);

	protected readonly Regex TableRegExRow = new Regex("^.*?(%(?<TableName>\\w+)\\.(?<ColumnName>\\w+))+.*?$", RegexOptions.Multiline | RegexOptions.Compiled);

	private Encoding FileEncoding { get; set; }

	private StringBuilder ResultBuilder { get; set; }

	public TextTemplateSubstitutor(IPrintTemplateSubstitution printTemplateSubstitution, bool isUnicode, bool check = false)
		: base(printTemplateSubstitution, check)
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		FileEncoding = (isUnicode ? Encoding.UTF8 : Encoding.GetEncoding(1252));
	}

	public override Stream GetStream(byte[] file)
	{
		DoSubstitute(file);
		return new MemoryStream(FileEncoding.GetBytes(ResultBuilder.ToString()));
	}

	public override void SaveToFile(string file)
	{
		DoSubstitute(file);
		File.WriteAllText(file, ResultBuilder.ToString(), FileEncoding);
	}

	public override async Task SaveToFileAsync(string file)
	{
		await DoSubstituteAsync(file);
		await File.WriteAllTextAsync(file, ResultBuilder.ToString(), FileEncoding);
	}

	public override Task<Stream> GetStreamAsync(byte[] file)
	{
		DoSubstituteAsync(file);
		return Task.FromResult(GetStream(file));
	}

	private async Task DoSubstituteAsync(string file)
	{
		ResultBuilder = new StringBuilder();
		InnerSubstitute(await File.ReadAllTextAsync(file, FileEncoding));
	}

	private Task DoSubstituteAsync(byte[] file)
	{
		DoSubstitute(file);
		return Task.CompletedTask;
	}

	private void DoSubstitute(byte[] file)
	{
		ResultBuilder = new StringBuilder();
		InnerSubstitute(FileEncoding.GetString(file));
	}

	private void DoSubstitute(string file)
	{
		ResultBuilder = new StringBuilder();
		InnerSubstitute(File.ReadAllText(file, FileEncoding));
	}

	public string GetContent(string file)
	{
		DoSubstitute(file);
		return ResultBuilder.ToString();
	}

	public string GetContent(byte[] file)
	{
		DoSubstitute(file);
		return ResultBuilder.ToString();
	}

	private void InnerSubstitute(string fileContent)
	{
		SubstituteAtomicsSharp(ref fileContent);
		int num = 0;
		int num2;
		foreach (Match item in TableRegExRow.Matches(fileContent).Cast<Match>())
		{
			string value = item.Groups["TableName"].Value;
			StringBuilder resultBuilder = ResultBuilder;
			string text = fileContent;
			num2 = num;
			resultBuilder.Append(text.Substring(num2, item.Index - num2));
			SubstituteTable(PrintTemplateSubstitution.GridSubstitutions.Substitutions[value], value, item.Value);
			num = item.Index + item.Length;
		}
		StringBuilder resultBuilder2 = ResultBuilder;
		string text2 = fileContent;
		num2 = num;
		resultBuilder2.Append(text2.Substring(num2, text2.Length - num2));
	}

	private void SubstituteAtomicsSharp(ref string fileContent)
	{
		List<AtomicSubstitution> sortedByLengthSubstitutions = PrintTemplateSubstitution.AtomicSubstitutions.GetSortedByLengthSubstitutions();
		StringBuilder stringBuilder = new StringBuilder(fileContent);
		foreach (AtomicSubstitution item in sortedByLengthSubstitutions)
		{
			stringBuilder.Replace("#" + item.Code, item.Value.ToString());
		}
		string text = stringBuilder.ToString();
		ValidateAtomic(fileContent, text);
		fileContent = text;
	}

	private void SubstituteTable(IPrintTemplateSubstitutionGrid grid, string gridName, string templateText)
	{
		MatchCollection matchCollection = TableRegEx.Matches(templateText);
		if (matchCollection.Count == 0)
		{
			return;
		}
		foreach (Match item in matchCollection.Cast<Match>())
		{
			if (item.Groups["TableName"].Value != gridName)
			{
				throw new InvalidTemplateException(string.Format(Resource.InvalidTextTemplateRow, templateText));
			}
		}
		int num = 0;
		for (int i = 0; i < grid.RowCount; i++)
		{
			int num2;
			foreach (Match item2 in matchCollection.Cast<Match>())
			{
				string value = item2.Groups["ColumnName"].Value;
				StringBuilder resultBuilder = ResultBuilder;
				num2 = num;
				resultBuilder.Append(templateText.Substring(num2, item2.Index - num2));
				ResultBuilder.Append(grid.GetValueConsideringFormatter(i, value)?.ToString() ?? string.Empty);
				num = item2.Index + item2.Length;
			}
			StringBuilder resultBuilder2 = ResultBuilder;
			num2 = num;
			resultBuilder2.AppendLine(templateText.Substring(num2, templateText.Length - num2));
		}
	}
}
