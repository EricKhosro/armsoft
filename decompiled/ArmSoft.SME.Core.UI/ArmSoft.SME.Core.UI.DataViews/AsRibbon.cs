using System.Windows.Media;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;

namespace ArmSoft.SME.Core.UI.DataViews;

public class AsRibbon : RibbonPage
{
	public bool FillingCompleted { get; set; }

	public AsRibbon()
	{
		FillingCompleted = false;
		RibbonPageGroup item = new RibbonPageGroup
		{
			Name = "Root",
			Caption = ""
		};
		Groups.Add(item);
	}

	public void AddGroup(string sName, string sCaption, string sSmallImage = "")
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if (FillingCompleted)
		{
			return;
		}
		foreach (RibbonPageGroup group in Groups)
		{
			if (group.Name == sName)
			{
				return;
			}
		}
		RibbonPageGroup ribbonPageGroup = new RibbonPageGroup
		{
			Name = sName,
			Caption = sCaption
		};
		if (!string.IsNullOrWhiteSpace(sSmallImage))
		{
			ImageSource image = ImageService.GetImage(sSmallImage);
			if (image != null)
			{
				ribbonPageGroup.Glyph = image;
			}
		}
		Groups.Add(ribbonPageGroup);
	}

	public void DeleteEmptyGroups()
	{
		for (int i = Groups.Count - 1; i >= 0; i += -1)
		{
			RibbonPageGroup ribbonPageGroup = Groups[i];
			if (ribbonPageGroup.ItemLinks.Count == 0)
			{
				Groups.Remove(ribbonPageGroup);
			}
		}
	}

	public void AddFunction(string sGroup, BarButtonItemLink btnLink)
	{
		RibbonPageGroup ribbonPageGroup = null;
		if (FillingCompleted)
		{
			return;
		}
		if (sGroup == "")
		{
			ribbonPageGroup = Groups[0];
		}
		else
		{
			foreach (RibbonPageGroup group in Groups)
			{
				if (group.Name == sGroup)
				{
					ribbonPageGroup = group;
					break;
				}
			}
		}
		if (ribbonPageGroup == null)
		{
			ribbonPageGroup = Groups[0];
		}
		ribbonPageGroup.ItemLinks.Add(btnLink);
	}

	public void AddSeparator(string sGroup)
	{
		RibbonPageGroup ribbonPageGroup = null;
		if (FillingCompleted)
		{
			return;
		}
		if (sGroup == "")
		{
			ribbonPageGroup = Groups[0];
		}
		else
		{
			foreach (RibbonPageGroup group in Groups)
			{
				if (group.Name == sGroup)
				{
					ribbonPageGroup = group;
					break;
				}
			}
		}
		if (ribbonPageGroup == null)
		{
			ribbonPageGroup = Groups[0];
		}
		ribbonPageGroup.ItemLinks.Add(new BarItemLinkSeparator());
	}

	public bool IsEmpty()
	{
		return (Groups.Count == 1) & (Groups[0].ItemLinks.Count == 0);
	}
}
