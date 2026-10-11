using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArmSoft.PrintTemplates.Interfaces;
using Armsoft.PrintTemplates2.Resources;

namespace ArmSoft.PrintTemplates;

public abstract class TemplateSubstitutor(IPrintTemplateSubstitution printTemplateSubstitution, bool check = false) : ITemplateSubstitutor
{
	public IPrintTemplateSubstitution PrintTemplateSubstitution { get; } = printTemplateSubstitution;

	public bool Check { get; } = check;

	public virtual void SaveToFile(string file)
	{
	}

	public virtual Stream GetStream(byte[] file)
	{
		throw new NotImplementedException();
	}

	public virtual Task<Stream> GetStreamAsync(byte[] file)
	{
		throw new NotImplementedException();
	}

	public virtual Task SaveToFileAsync(string file)
	{
		throw new NotImplementedException();
	}

	private void CheckSubstitutionElement(string source, string target, string atomic, Func<string, bool> specificCheck = null, string exception = "", string pattern = "")
	{
		if (!Check || !target.Contains(atomic))
		{
			return;
		}
		MatchCollection matchCollection = new Regex(atomic + pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Matches(target);
		if (matchCollection == null || matchCollection.Count <= 0)
		{
			return;
		}
		foreach (Match item in matchCollection.Cast<Match>())
		{
			if (item != null && item.Value != null && !string.IsNullOrWhiteSpace(item.Value) && source.Contains(item.Value) && (specificCheck == null || specificCheck(item.Value)))
			{
				string value = item.Value;
				throw new InvalidTemplateException(string.Format(exception, value.Substring(1, value.Length - 1)));
			}
		}
	}

	protected void ValidateAtomic(string source, string target, Func<string, bool> specificCheck = null, string atomic = "#")
	{
		CheckSubstitutionElement(source, target, atomic, specificCheck, Resource.InvalidSubstitutionElement, "\\w+");
	}

	protected void ValidateGrid(string source, string target, Func<string, bool> specificCheck = null)
	{
		CheckSubstitutionElement(source, target, "%", specificCheck, Resource.InvalidGridSubstitution, "\\w+\\.\\w+\\b");
	}
}
