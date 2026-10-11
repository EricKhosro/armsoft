using System.Xml;
using ArmSoft.SME.Core.UI.MetaData.UI_Definition;

namespace ArmSoft.SME.Core.UI.MetaData;

public abstract class UIElement : UIElementDescription
{
	public abstract XmlElement ToXml(XmlDocument xmlDoc);
}
