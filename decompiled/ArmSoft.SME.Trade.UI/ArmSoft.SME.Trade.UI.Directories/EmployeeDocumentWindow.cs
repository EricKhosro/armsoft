using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryDocumentWindow(3)]
public class EmployeeDocumentWindow : DirectoryDocumentWindow<Employee>
{
	private TextEditExt _fullName;

	private TextEditExt _phone;

	private TextEditExt _passportNumber;

	private TextEditExt _address;

	private TextEditExt _employeeCard;

	private CheckEditExt _isSalesConsultant;

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private RibbonPageGroup _ribbonPageGroupJournals;

	private ReportBarButtonItem _barButtonDocumentsJournal;

	protected override double WidthValue => 600.0;

	protected override double HeightValue => 720.0;

	protected override bool WriteInHistory => true;

	public EmployeeDocumentWindow()
	{
	}

	public EmployeeDocumentWindow(Employee doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_code = AddButtonEditControl(null, Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_fullName = AddStringControl(null, Definition.Fields["Name"], HorizontalAlignment.Stretch);
		_address = AddStringControl(null, Definition.Fields["Address"], HorizontalAlignment.Stretch);
		_phone = AddStringControl(null, Definition.Fields["Phone"], HorizontalAlignment.Stretch);
		_passportNumber = AddStringControl(null, Definition.Fields["Passport"], HorizontalAlignment.Stretch);
		_employeeCard = AddStringControl(null, Definition.Fields["EmployeeCard"], HorizontalAlignment.Stretch);
		_isSalesConsultant = AddBooleanControl(null, Definition.Fields["IsSalesConsultant"]);
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_fullName.Text = "";
		_address.Text = "";
		_phone.Text = "";
		_passportNumber.Text = "";
		_employeeCard.Text = "";
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _fullName;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_code.IsReadOnly = StoredInDb;
		}
	}

	protected override void AfterStore(DirStoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		dvp.Update(PO.Id);
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_journal_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		AddRibbonPageGroupJournals();
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("journal_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("journal_32.svg");
		_barButtonDocumentsJournal = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: true, ArmSoft.SME.Common.Resources.DocumentsJournal, null, "journal_16.svg", "journal_32.svg");
		_barButtonDocumentsJournal.AddInBarSubItem(ref _ribbonPageGroupJournals, ref _barSubItemJournals);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonDocumentsJournal = _barButtonDocumentsJournal;
		int num = 1;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("EmplCode", PO.Code.ToString());
		barButtonDocumentsJournal.PropertyWithValues = list;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonDocumentsJournal.IsEnabled = isEnabled;
	}

	private void AddRibbonPageGroupJournals()
	{
		if (_ribbonPageOther == null)
		{
			AddOtherRibbonPage();
		}
		if (_ribbonPageGroupJournals == null)
		{
			_ribbonPageGroupJournals = new RibbonPageGroup
			{
				AllowCollapse = false
			};
			_ribbonPageOther.Groups.Add(_ribbonPageGroupJournals);
			_ribbonPageGroupJournals.Caption = ArmSoft.SME.Common.Resources.Journals;
		}
	}

	public override void OnMagneticCardReader(string cardData)
	{
		ReadMagneticCard(cardData);
	}

	public override void OnScanner(string scannerData)
	{
		ReadMagneticCard(scannerData);
	}

	private void ReadMagneticCard(string cardData)
	{
		if (MCRInfo.GetInstance().IsValidCard(cardData))
		{
			string cardNumber = MCRInfo.GetInstance().GetCardNumber(cardData);
			_employeeCard.EditValue = cardNumber;
		}
	}
}
