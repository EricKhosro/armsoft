using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Common.MetaData;

public abstract class Environment
{
	private readonly Regex _constRegex;

	private readonly Regex _idRegex;

	private readonly Dictionary<string, string> _constants;

	public string CurrentDirectory { get; set; }

	public Dictionary<string, string> Constants => _constants;

	public Environment()
	{
		_constRegex = new Regex("@[A-Za-z_][A-Za-z_0-9]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		_idRegex = new Regex("[A-Za-z_][A-Za-z_0-9]*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		_constants = new Dictionary<string, string>();
		CurrentDirectory = "";
	}

	public bool IsIdentifier(string str)
	{
		return _idRegex.IsMatch(str);
	}

	public string SubstituteConstants(string str)
	{
		return _constRegex.Replace(str, SubstituteConstant);
	}

	private string SubstituteConstant(Match m)
	{
		string key = m.ToString().Mid(2);
		if (Constants.TryGetValue(key, out var value))
		{
			return value;
		}
		return m.ToString();
	}

	public short ParseShort(string str)
	{
		string arg = str;
		if (short.TryParse(str, out var result))
		{
			return result;
		}
		str = SubstituteConstants(str);
		if (short.TryParse(str, out result))
		{
			return result;
		}
		throw new RESTException(string.Format(Resources.InvalidIntegerExpression, arg));
	}

	public static bool ParseBoolean(string str)
	{
		if (!(str == "true"))
		{
			return str == "1";
		}
		return true;
	}

	public static decimal ParseDecimal(string str)
	{
		decimal result = 0m;
		if (!string.IsNullOrWhiteSpace(str))
		{
			decimal.TryParse(str, out result);
		}
		return result;
	}

	public static AttachType ParseAttachType(string str)
	{
		AttachType attachType = AttachType.None;
		string[] array = str.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text == AttachType.All.ToString())
			{
				return AttachType.All;
			}
			if (text == AttachType.File.ToString())
			{
				attachType |= AttachType.File;
			}
			else if (text == AttachType.Link.ToString())
			{
				attachType |= AttachType.Link;
			}
			else if (text == AttachType.Doc.ToString())
			{
				attachType |= AttachType.Doc;
			}
			else if (text != AttachType.None.ToString())
			{
				throw new RESTException(string.Format(Resources.InvalidIdentifier, "AttachmentsTypes - " + text));
			}
		}
		return attachType;
	}

	public abstract string GetStringResource(string resourceName);

	public abstract string LoadScriptFromFile(string fileRelativeName);

	public abstract TreeCacheRow Tree(string treeId);
}
