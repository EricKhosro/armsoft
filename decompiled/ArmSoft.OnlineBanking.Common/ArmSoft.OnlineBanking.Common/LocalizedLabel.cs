using System;
using System.Xml.Serialization;

namespace ArmSoft.OnlineBanking.Common;

public class LocalizedLabel : ICloneable
{
	[XmlAttribute("text")]
	public string Text { get; set; }

	[XmlAttribute("language")]
	public string Language { get; set; }

	public LocalizedLabel()
	{
		Text = string.Empty;
		Language = string.Empty;
	}

	public LocalizedLabel(string text, string language)
	{
		Text = text ?? string.Empty;
		Language = language;
	}

	public object Clone()
	{
		return new LocalizedLabel
		{
			Text = Text,
			Language = Language
		};
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		LocalizedLabel localizedLabel = (LocalizedLabel)obj;
		if (Language.Equals(localizedLabel.Language))
		{
			return Text.Equals(localizedLabel.Text);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Language.GetHashCode() ^ Text.GetHashCode();
	}
}
