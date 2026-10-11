using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.PersistableObject;

public interface IPOWindow : IScanable, IMagneticCardReadable
{
	int DVPID { get; set; }

	bool IsOfflineDoc { get; set; }

	bool IsLoadingDataToControls { get; set; }

	Dictionary<string, string> GetTabs();

	void Show(WindowViewMode? viewMode = null, bool disableGridValidators = false);

	DropDownControl AddDropDownControl(LayoutGroup container, string caption, bool isRequired, IDropDownView idv, bool allowAdditionButtons = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "");

	PopupCalcEditExt AddNumericControl(LayoutGroup container, string caption, bool isRequired, short length, short precision = 0, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string dataItemName = "", Type editValueType = null, string name = "", bool isNew = false, string helpText = "");

	Button AddButton(LayoutGroup container, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string name = "", bool isNew = false, string helpText = "");

	LayoutGroupExt AddLayoutGroup(LayoutGroup container, LayoutGroupView view, Orientation orientation, string header = "");

	TextEditExt AddStringControl(LayoutGroup container, string caption, bool isRequired, short length, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string dataItemName = "", string name = "", bool isNew = false, string helpText = "");

	List<CallsRecordingHelper.ControlInfo> GetControlsInfoForRecord(bool withoutValues = false);
}
