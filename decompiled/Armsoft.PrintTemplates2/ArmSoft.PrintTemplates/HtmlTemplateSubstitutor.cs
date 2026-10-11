using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Armsoft.PrintTemplates2;
using Armsoft.PrintTemplates2.Resources;
using Microsoft.VisualBasic;

namespace ArmSoft.PrintTemplates;

public class HtmlTemplateSubstitutor : TemplateSubstitutor, IContentProvider
{
	private readonly XmlDocument xmlValueParser = new XmlDocument();

	private XmlDocument XmlDocument { get; set; }

	private HtmlImageOption Options { get; set; }

	private Encoding FileEncoding { get; set; }

	public HtmlTemplateSubstitutor(IPrintTemplateSubstitution printTemplateSubstitution, bool isUnicode, HtmlImageOption options = HtmlImageOption.RelativePath, bool check = false)
		: base(printTemplateSubstitution, check)
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		XmlDocument = new XmlDocument
		{
			XmlResolver = null,
			PreserveWhitespace = true
		};
		Options = options;
		FileEncoding = (isUnicode ? Encoding.UTF8 : Encoding.GetEncoding(1252));
	}

	public override void SaveToFile(string file)
	{
		DoSubstitute(file);
		File.WriteAllText(file, RemoveIncorrectBrackets(XmlDocument.OuterXml), FileEncoding);
	}

	public override Stream GetStream(byte[] file)
	{
		DoSubstitute(file);
		return new MemoryStream(FileEncoding.GetBytes(RemoveIncorrectBrackets(XmlDocument.OuterXml)));
	}

	public override async Task SaveToFileAsync(string file)
	{
		await DoSubstituteAsync(file);
		await File.WriteAllTextAsync(file, RemoveIncorrectBrackets(XmlDocument.OuterXml), FileEncoding);
	}

	public override Task<Stream> GetStreamAsync(byte[] file)
	{
		DoSubstitute(file);
		return Task.FromResult(GetStream(file));
	}

	private Task DoSubstituteAsync(string file)
	{
		DoSubstitute(file);
		return Task.CompletedTask;
	}

	private Task DoSubstituteAsync(byte[] file)
	{
		DoSubstitute(file);
		return Task.CompletedTask;
	}

	private void DoSubstitute(string file)
	{
		XmlDocument.Load(file);
		InnerSubstitute();
	}

	private void DoSubstitute(byte[] file)
	{
		using MemoryStream inStream = new MemoryStream(file);
		XmlDocument.Load(inStream);
		InnerSubstitute();
	}

	public string GetContent(string file)
	{
		DoSubstitute(file);
		return RemoveIncorrectBrackets(XmlDocument.OuterXml);
	}

	public string GetContent(byte[] file)
	{
		DoSubstitute(file);
		return RemoveIncorrectBrackets(XmlDocument.OuterXml);
	}

	private void InnerSubstitute()
	{
		SubstituteUsingId(XmlDocument);
		SubstituteUsingData(XmlDocument);
	}

	private string RemoveIncorrectBrackets(string invalidHtml)
	{
		int num = invalidHtml.IndexOf("[]>");
		if (num > 0)
		{
			StringBuilder stringBuilder = new StringBuilder(invalidHtml);
			stringBuilder.Remove(num, 2);
			invalidHtml = stringBuilder.ToString();
		}
		return invalidHtml;
	}

	private void SubstituteUsingId(XmlDocument xmlDoc)
	{
		int num = 0;
		XmlNodeList xmlNodeList;
		using (xmlNodeList = xmlDoc.SelectNodes("//*[(@id and substring(@id,1,1)='#') or (@ID and substring(@ID,1,1)='#') or (@Id and substring(@Id,1,1)='#')]"))
		{
			foreach (XmlNode item in xmlNodeList)
			{
				XmlNode namedItem = item.Attributes.GetNamedItem("id");
				if (namedItem == null)
				{
					namedItem = item.Attributes.GetNamedItem("ID");
				}
				if (namedItem == null)
				{
					namedItem = item.Attributes.GetNamedItem("Id");
				}
				if (namedItem == null)
				{
					continue;
				}
				_ = namedItem.Name;
				string value = namedItem.Value;
				if (Strings.Len(value) != 1)
				{
					string text = Strings.Trim(Strings.Mid(value, 2)).ToUpper();
					if (PrintTemplateSubstitution.AtomicSubstitutions.Contains(text))
					{
						string value2 = PrintTemplateSubstitution.AtomicSubstitutions.Value(text).ToString();
						InsertAtomicValue(value2, item);
						num++;
					}
					else if (!PrintTemplateSubstitution.ImageSubstitutions.Contains(text))
					{
						throw new InvalidTemplateException(string.Format(Resource.InvalidSubstitutionElement, text));
					}
				}
			}
			if (PrintTemplateSubstitution.GridSubstitutions.Count() > 0)
			{
				foreach (string key in PrintTemplateSubstitution.GridSubstitutions.Substitutions.Keys)
				{
					((IDisposable)xmlNodeList)?.Dispose();
					xmlNodeList = xmlDoc.SelectNodes("//table[thead and @id='%" + key.Trim() + "']");
					foreach (XmlNode item2 in xmlNodeList)
					{
						SubstituteGridHTML(item2, PrintTemplateSubstitution.GridSubstitutions.Value(key));
					}
				}
			}
			if (PrintTemplateSubstitution.ImageSubstitutions.Count() == 0)
			{
				return;
			}
			foreach (string key2 in PrintTemplateSubstitution.ImageSubstitutions.Substitutions.Keys)
			{
				((IDisposable)xmlNodeList)?.Dispose();
				xmlNodeList = xmlDoc.SelectNodes("//img[@id='#" + Strings.Trim(key2) + "']");
				foreach (XmlNode item3 in xmlNodeList)
				{
					SubstituteImageHTML(item3, PrintTemplateSubstitution.ImageSubstitutions.Value(key2));
				}
			}
		}
	}

	private void SubstituteImageHTML(XmlNode imageNode, PrintTemplateSubstitutionImage tempSubImage)
	{
		XmlNode xmlNode = imageNode.Attributes.GetNamedItem("src");
		if (xmlNode == null)
		{
			xmlNode = imageNode.OwnerDocument.CreateAttribute("src");
			imageNode.Attributes.SetNamedItem(xmlNode);
		}
		if (tempSubImage.Width != 0m)
		{
			XmlAttribute xmlAttribute = imageNode.Attributes["width"];
			if (xmlAttribute == null)
			{
				xmlAttribute = imageNode.OwnerDocument.CreateAttribute("width");
				xmlAttribute.Value = tempSubImage.Width.ToString();
				imageNode.Attributes.SetNamedItem(xmlAttribute);
			}
		}
		if (tempSubImage.Height != 0m)
		{
			XmlAttribute xmlAttribute2 = imageNode.Attributes["height"];
			if (xmlAttribute2 == null)
			{
				xmlAttribute2 = imageNode.OwnerDocument.CreateAttribute("height");
				xmlAttribute2.Value = tempSubImage.Height.ToString();
				imageNode.Attributes.SetNamedItem(xmlAttribute2);
			}
		}
		switch (Options)
		{
		case HtmlImageOption.AbsolutePath:
			xmlNode.Value = tempSubImage.FileName.Trim();
			break;
		case HtmlImageOption.Base64:
		{
			string text = Path.GetExtension(tempSubImage.FileName);
			if (text != "")
			{
				string text2 = text;
				text = text2.Substring(1, text2.Length - 1);
			}
			xmlNode.Value = "data:image/" + text + ";base64," + Convert.ToBase64String(tempSubImage.File);
			break;
		}
		default:
			xmlNode.Value = Path.GetFileName(tempSubImage.FileName);
			break;
		case HtmlImageOption.None:
			break;
		}
	}

	private void InsertAtomicValue(string value, XmlNode parentNode)
	{
		if (value.Contains('<'))
		{
			try
			{
				xmlValueParser.LoadXml(value);
				parentNode.AppendChild(xmlValueParser.DocumentElement);
				return;
			}
			catch
			{
				try
				{
					parentNode.InnerXml = value;
					return;
				}
				catch
				{
					parentNode.InnerText = value;
					return;
				}
			}
		}
		parentNode.InnerText = value;
	}

	private void SubstituteGridHTML(XmlNode tableNode, IPrintTemplateSubstitutionGrid tempSubGrid)
	{
		List<Tuple<string, string, XmlNode>> list = new List<Tuple<string, string, XmlNode>>();
		XmlNode xmlNode = tableNode.SelectSingleNode("./thead");
		if (xmlNode == null)
		{
			return;
		}
		XmlNode xmlNode2 = tableNode.SelectSingleNode("./thead/tr");
		if (xmlNode2 == null)
		{
			return;
		}
		if (tempSubGrid.RowCount == 0)
		{
			xmlNode.RemoveChild(xmlNode2);
			return;
		}
		bool flag = false;
		using XmlNodeList xmlNodeList = xmlNode2.SelectNodes("./th");
		foreach (XmlNode item in xmlNodeList)
		{
			XmlNode namedItem = item.Attributes.GetNamedItem("id");
			if (namedItem == null)
			{
				continue;
			}
			string text = namedItem.Value.Trim();
			if ((Strings.Left(text, Strings.Len(tempSubGrid.Name.Trim()) + 2) ?? "") == "%" + Strings.Trim(tempSubGrid.Name) + ".")
			{
				flag = true;
				text = Strings.Trim(Strings.Mid(text, Strings.InStrRev(text, ".") + 1));
				if (!tempSubGrid.Columns.ContainsKey(text))
				{
					throw new InvalidTemplateException(string.Format(Resource.ColumnNotExists, text, tempSubGrid.Name));
				}
				list.Add(Tuple.Create(tempSubGrid.Columns[text].Name, item.InnerText, item));
			}
			else
			{
				list.Add(Tuple.Create("", item.InnerText, item));
			}
		}
		if (!flag)
		{
			xmlNode.RemoveChild(xmlNode2);
			return;
		}
		XmlNode xmlNode4 = tableNode.SelectSingleNode("./tbody");
		if (xmlNode4 == null)
		{
			xmlNode4 = tableNode.OwnerDocument.CreateElement("tbody");
			tableNode.AppendChild(xmlNode4);
		}
		int i = 0;
		for (int num = tempSubGrid.RowCount - 1; i <= num; i++)
		{
			XmlElement xmlElement = tableNode.OwnerDocument.CreateElement("tr");
			foreach (Tuple<string, string, XmlNode> item2 in list)
			{
				XmlElement tdElement = tableNode.OwnerDocument.CreateElement("td");
				if (string.IsNullOrEmpty(item2.Item1))
				{
					InsertAtomicValue(item2.Item2, tdElement);
				}
				else
				{
					string text = tempSubGrid.GetValueConsideringFormatter(i, item2.Item1)?.ToString() ?? string.Empty;
					InsertAtomicValue(text, tdElement);
				}
				AppendAttributes(ref tdElement, item2.Item3);
				xmlElement.AppendChild(tdElement);
			}
			xmlNode4.AppendChild(xmlElement);
		}
		xmlNode.RemoveChild(xmlNode2);
	}

	private void AppendAttributes(ref XmlElement tdElement, XmlNode thNode)
	{
		if (thNode.Attributes.Count == 0)
		{
			return;
		}
		foreach (XmlNode attribute in thNode.Attributes)
		{
			if (attribute.Name.Trim().ToLower() != "id")
			{
				tdElement.Attributes.SetNamedItem(attribute.CloneNode(deep: true));
			}
		}
	}

	private void SubstituteUsingData(XmlDocument xmlDoc)
	{
		XmlNodeList xmlNodeList;
		using (xmlNodeList = xmlDoc.SelectNodes("//*[@data-value]"))
		{
			foreach (XmlNode item in xmlNodeList)
			{
				XmlAttribute xmlAttribute = item.Attributes["data-value"];
				string value = xmlAttribute.Value;
				item.Attributes.Remove(xmlAttribute);
				if (Strings.Len(value) != 1)
				{
					string text = Strings.Trim(Strings.Mid(value, 2)).ToUpper();
					if (!PrintTemplateSubstitution.AtomicSubstitutions.Contains(text))
					{
						throw new InvalidTemplateException(string.Format(Resource.InvalidSubstitutionElement, text));
					}
					string value2 = PrintTemplateSubstitution.AtomicSubstitutions.Value(text).ToString();
					SubstituteValue(item, value2);
				}
			}
			if (PrintTemplateSubstitution.GridSubstitutions.Count() > 0)
			{
				foreach (string key in PrintTemplateSubstitution.GridSubstitutions.Substitutions.Keys)
				{
					((IDisposable)xmlNodeList)?.Dispose();
					xmlNodeList = xmlDoc.SelectNodes($"//*[@data-table='%{key}']");
					foreach (XmlNode item2 in xmlNodeList)
					{
						SubstituteGridUsingData(PrintTemplateSubstitution.GridSubstitutions.Value(key), item2);
					}
				}
			}
			if (PrintTemplateSubstitution.ImageSubstitutions.Count() == 0)
			{
				return;
			}
			foreach (string key2 in PrintTemplateSubstitution.ImageSubstitutions.Substitutions.Keys)
			{
				((IDisposable)xmlNodeList)?.Dispose();
				xmlNodeList = xmlDoc.SelectNodes($"//img[@data-image='#{key2}']");
				foreach (XmlNode item3 in xmlNodeList)
				{
					SubstituteImageHTML(item3, PrintTemplateSubstitution.ImageSubstitutions.Value(key2));
					item3.Attributes.Remove(item3.Attributes["data-image"]);
				}
			}
		}
	}

	private void SubstituteGridUsingData(IPrintTemplateSubstitutionGrid grid, XmlNode tableNode)
	{
		XmlNode xmlNode = tableNode;
		int i = 0;
		for (int num = grid.RowCount - 1; i <= num; i++)
		{
			XmlNode xmlNode2 = tableNode.Clone();
			foreach (string key in grid.Columns.Keys)
			{
				XmlNodeList xmlNodeList;
				using (xmlNodeList = xmlNode2.SelectNodes($"//*[@data-column='%{grid.Name}.{key}']"))
				{
					foreach (XmlNode item in xmlNodeList)
					{
						item.Attributes.Remove(item.Attributes["data-column"]);
						string value = grid.GetValueConsideringFormatter(i, key)?.ToString() ?? string.Empty;
						SubstituteValue(item, value);
					}
				}
			}
			xmlNode2.Attributes.Remove(xmlNode2.Attributes["data-table"]);
			xmlNode.ParentNode.InsertAfter(xmlNode2, xmlNode);
			xmlNode = xmlNode2;
		}
		tableNode.ParentNode.RemoveChild(tableNode);
	}

	private void SubstituteValue(XmlNode node, string value)
	{
		XmlAttribute xmlAttribute = node.Attributes["data-type"];
		if (xmlAttribute != null && xmlAttribute.Value == "html")
		{
			node.Attributes.Remove(xmlAttribute);
			node.InnerXml = value;
		}
		else
		{
			node.InnerText = value;
		}
	}
}
