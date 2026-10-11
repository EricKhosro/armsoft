using ArmSoft.SME.Common;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Docking.Base;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.NavBar;
using DevExpress.Xpf.Ribbon;
using DevExpress.XtraPrinting.Localization;

namespace ArmSoft.SME.Core.UI.Localizers;

public static class Localizer
{
	private static Language? _currentLocalization;

	public static void Configure()
	{
		if (LanguageService.IsEnglish)
		{
			if (_currentLocalization != Language.English)
			{
				GridControlLocalizer.Active = new GridControlLocalizer();
				RibbonControlLocalizer.Active = new RibbonControlLocalizer();
				EditorLocalizer.Active = new EditorLocalizer();
				DXMessageBoxLocalizer.Active = new DXMessageBoxLocalizer();
				SearchPanelLocalizer.Active = new SearchPanelLocalizer();
				NavBarLocalizer.Active = new NavBarLocalizer();
				DockingLocalizer.Active = new DockingLocalizer();
				ConditionalFormattingLocalizer.Active = new ConditionalFormattingLocalizer();
				PreviewLocalizer.Active = new PreviewLocalizer();
				_currentLocalization = Language.English;
			}
		}
		else if (LanguageService.IsRussian)
		{
			if (_currentLocalization != Language.Russian)
			{
				GridControlLocalizer.Active = new GridControlLocalizer();
				RibbonControlLocalizer.Active = new RibbonControlLocalizer();
				EditorLocalizer.Active = new EditorLocalizer();
				DXMessageBoxLocalizer.Active = new DXMessageBoxLocalizer();
				SearchPanelLocalizer.Active = new SearchPanelLocalizer();
				NavBarLocalizer.Active = new NavBarLocalizer();
				DockingLocalizer.Active = new DockingLocalizer();
				ConditionalFormattingLocalizer.Active = new ConditionalFormattingLocalizer();
				PreviewLocalizer.Active = new PreviewLocalizer();
				_currentLocalization = Language.Russian;
			}
		}
		else if (_currentLocalization != Language.Armenian)
		{
			GridControlLocalizer.Active = new AsGridLocalizer();
			RibbonControlLocalizer.Active = new AsRibbonLocaLizer();
			EditorLocalizer.Active = new AsEditorsLocaLizer();
			DXMessageBoxLocalizer.Active = new AsMsgBoxLocaLizer();
			SearchPanelLocalizer.Active = new AsSearchPanelLocaLizer();
			NavBarLocalizer.Active = new AsNavBarLocalizer();
			DockingLocalizer.Active = new AsDockingLocalizer();
			ConditionalFormattingLocalizer.Active = new AsConditionalFormattingLocaLizer();
			PreviewLocalizer.Active = new AsReportPreviewLocaLizer();
			_currentLocalization = Language.Armenian;
		}
	}
}
