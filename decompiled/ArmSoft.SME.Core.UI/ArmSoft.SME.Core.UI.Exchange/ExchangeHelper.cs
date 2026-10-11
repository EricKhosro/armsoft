using System.Collections.Generic;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Models.Exchange;

namespace ArmSoft.SME.Core.UI.Exchange;

public static class ExchangeHelper
{
	private static readonly Dictionary<string, ExchangeEntityInfo> _registeredTypes = new Dictionary<string, ExchangeEntityInfo>();

	public static List<ParseResult> Parse(string filename)
	{
		int num = 0;
		List<ParseResult> list = new List<ParseResult>();
		using (XmlTextReader xmlTextReader = new XmlTextReader(filename))
		{
			xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
			xmlTextReader.Read();
			while (!xmlTextReader.EOF && xmlTextReader.ReadState != ReadState.Error)
			{
				switch (num)
				{
				case 0:
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						if (!(xmlTextReader.Name == "Exchange"))
						{
							throw new RESTException(string.Format(Resources.XMLRootElementMustBeExchange, xmlTextReader.LineNumber, xmlTextReader.LinePosition));
						}
						num = 1;
					}
					xmlTextReader.Read();
					break;
				case 1:
					switch (xmlTextReader.NodeType)
					{
					case XmlNodeType.EndElement:
						if (xmlTextReader.Name == "Exchange")
						{
							num = 2;
							break;
						}
						throw new RESTException(string.Format(Resources.UnexpectedXMLConstruction, xmlTextReader.LineNumber, xmlTextReader.LinePosition, xmlTextReader.Name));
					case XmlNodeType.Element:
					{
						if (!_registeredTypes.TryGetValue(xmlTextReader.Name, out var value))
						{
							value = Settings.CreateApiClient().Exchange.GetEntityInfo(xmlTextReader.Name);
							_registeredTypes.Add(xmlTextReader.Name, value);
						}
						ParseResult item = new ParseResult
						{
							LineNumber = xmlTextReader.LineNumber,
							LinePosition = xmlTextReader.LinePosition,
							Caption = value.Caption,
							EntityName = xmlTextReader.Name,
							EntityXML = xmlTextReader.ReadOuterXml()
						};
						list.Add(item);
						break;
					}
					case XmlNodeType.Comment:
						xmlTextReader.Read();
						break;
					default:
						throw new RESTException(string.Format(Resources.UnexpectedXMLConstruction, xmlTextReader.LineNumber, xmlTextReader.LinePosition, xmlTextReader.Name));
					}
					break;
				case 2:
					goto end_IL_01aa;
				}
				continue;
				end_IL_01aa:
				break;
			}
		}
		return list;
	}
}
