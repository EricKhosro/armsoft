using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView.Directories;

[DataView(10126)]
public class B2BProductsCheck : TradeDataView<B2BProductsCheckDataProvider.DataRow, B2BProductsCheckDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly B2BProductsCheck _dataView;

		private readonly DropDownControl _group;

		private readonly CheckEditExt _caption;

		private readonly CheckEditExt _unit;

		private readonly CheckEditExt _cpa;

		private readonly CheckEditExt _vat;

		public DataViewDialog(B2BProductsCheck dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.ProductVarianceCheckB2B;
			Name = dataView.Name;
			HelpId = dataView.HelpId;
			_group = AddDropDownControl("Group", ArmSoft.SME.Common.Resources.Group, new GoodsGroups());
			_group.Code = (useParamValues ? dataView.Group : "");
			TextBlock element = new TextBlock
			{
				Text = ArmSoft.SME.Trade.Common.Resources.IdnetifyProductsByEAN,
				Foreground = ColorsAS.Foreground.BlueBrush,
				TextWrapping = TextWrapping.Wrap,
				FontWeight = FontWeights.Thin
			};
			Image element2 = new Image
			{
				Width = 16.0,
				Height = 16.0,
				Source = ImageService.GetImage("information_16.svg"),
				VerticalAlignment = VerticalAlignment.Top
			};
			LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
			layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Left;
			layoutGroupExt.Width = 390.0;
			layoutGroupExt.Children.Add(element2);
			layoutGroupExt.Children.Add(element);
			LayoutGroupExt layoutGroupExt2 = AddVerticalLayoutGroup(null, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.VarianceFieldsSelection);
			layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Stretch;
			_caption = AddBooleanControl(layoutGroupExt2, "CheckForCaption", ArmSoft.SME.Common.Resources.Name);
			_caption.IsChecked = useParamValues && dataView.CheckForCaption;
			_cpa = AddBooleanControl(layoutGroupExt2, "CheckForCPA", ArmSoft.SME.Common.Resources.CPACodeShort);
			_cpa.IsChecked = !useParamValues || dataView.CheckForCPA;
			_unit = AddBooleanControl(layoutGroupExt2, "CheckForUnit", ArmSoft.SME.Common.Resources.QuantityUnitShort);
			_unit.IsChecked = useParamValues && dataView.CheckForUnit;
			_vat = AddBooleanControl(layoutGroupExt2, "CheckForVAT", ArmSoft.SME.Common.Resources.OptionsVAT);
			_vat.IsChecked = useParamValues && dataView.CheckForVAT;
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.Group = _group.Code;
			_dataView.CheckForCaption = _caption.IsChecked.Value;
			_dataView.CheckForUnit = _unit.IsChecked.Value;
			_dataView.CheckForCPA = _cpa.IsChecked.Value;
			_dataView.CheckForVAT = _vat.IsChecked.Value;
		}
	}

	private static readonly string _captionFormatingExpression = "[CaptionDifferent] = True";

	private static readonly string _cpaCodeFormatingExpression = "[CPACodeDifferent] = True";

	private static readonly string _unitFormatingExpression = "[UnitDifferent] = True";

	private static readonly string _vatFormatingExpression = "[VATDifferent] = True";

	private static readonly UserDefinedFormat _cellFormat = new UserDefinedFormat
	{
		IconName = "Symbols3_2"
	};

	public override string HelpId => "Purchases_Lists_CheckProductsWithb2b_armsoft";

	public string Group
	{
		get
		{
			return (string)Parameters["Group"].Value;
		}
		set
		{
			Parameters["Group"].Value = value;
		}
	}

	public bool CheckForCaption
	{
		get
		{
			return (bool)Parameters["CheckForCaption"].Value;
		}
		set
		{
			Parameters["CheckForCaption"].Value = value;
		}
	}

	public bool CheckForUnit
	{
		get
		{
			return (bool)Parameters["CheckForUnit"].Value;
		}
		set
		{
			Parameters["CheckForUnit"].Value = value;
		}
	}

	public bool CheckForCPA
	{
		get
		{
			return (bool)Parameters["CheckForCPA"].Value;
		}
		set
		{
			Parameters["CheckForCPA"].Value = value;
		}
	}

	public bool CheckForVAT
	{
		get
		{
			return (bool)Parameters["CheckForVAT"].Value;
		}
		set
		{
			Parameters["CheckForVAT"].Value = value;
		}
	}

	public override bool DialogSupported => true;

	public B2BProductsCheck()
	{
		DataProvider = new B2BProductsCheckDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("Code");
		AddParameter("Group", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("CheckForCaption", ArmSoft.SME.Common.Resources.Name, false);
		AddParameter("CheckForUnit", ArmSoft.SME.Common.Resources.QuantityUnitShort, false);
		AddParameter("CheckForCPA", ArmSoft.SME.Common.Resources.CPACodeShort, false);
		AddParameter("CheckForVAT", ArmSoft.SME.Common.Resources.OptionsVAT, false);
		AddConditionalFormatting();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Goods, "Code", "CaptionDB", "", ArmSoft.SME.Common.Resources.Product);
	}

	private void AddConditionalFormatting()
	{
		AsCondititionFormat asCondititionFormat = new AsCondititionFormat
		{
			Expression = _captionFormatingExpression,
			FieldName = "CaptionDB",
			FormatName = "Custom",
			CustomFormat = _cellFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat2 = new AsCondititionFormat
		{
			Expression = _captionFormatingExpression,
			FieldName = "CaptionB2B",
			FormatName = "Custom",
			CustomFormat = _cellFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat3 = new AsCondititionFormat
		{
			Expression = _cpaCodeFormatingExpression,
			FieldName = "CPACodeDB",
			FormatName = "Custom",
			CustomFormat = _cellFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat4 = new AsCondititionFormat
		{
			Expression = _cpaCodeFormatingExpression,
			FieldName = "CPACodeB2B",
			FormatName = "Custom",
			CustomFormat = _cellFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat5 = new AsCondititionFormat
		{
			Expression = _unitFormatingExpression,
			FieldName = "UnitDB",
			FormatName = "Custom",
			CustomFormat = _cellFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat6 = new AsCondititionFormat
		{
			Expression = _unitFormatingExpression,
			FieldName = "UnitB2B",
			FormatName = "Custom",
			CustomFormat = _cellFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat7 = new AsCondititionFormat
		{
			Expression = _vatFormatingExpression,
			FieldName = "VATDB",
			FormatName = "Custom",
			CustomFormat = _cellFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat8 = new AsCondititionFormat
		{
			Expression = _vatFormatingExpression,
			FieldName = "VATB2B",
			FormatName = "Custom",
			CustomFormat = _cellFormat,
			IsFixed = true
		};
		ConditionalFormattingList.AddRange(new _003C_003Ez__ReadOnlyArray<AsFormatting>(new AsFormatting[8] { asCondititionFormat, asCondititionFormat2, asCondititionFormat3, asCondititionFormat4, asCondititionFormat5, asCondititionFormat6, asCondititionFormat7, asCondititionFormat8 }));
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Group = Group;
		ParametersProvider.CheckForCaption = CheckForCaption;
		ParametersProvider.CheckForUnit = CheckForUnit;
		ParametersProvider.CheckForCPA = CheckForCPA;
		ParametersProvider.CheckForVAT = CheckForVAT;
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ViewVariance", ArmSoft.SME.Common.Resources.View, ViewVariance);
	}

	public override string GetDefaultFunction()
	{
		return "ViewVariance";
	}

	private void ViewVariance()
	{
		B2BProductsCheckDataProvider.DataRow dataRow = (B2BProductsCheckDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			DialogWindow dialogWindow = new DialogWindow
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.ProductDataFromB2B,
				Mode = DialogWindowMode.ViewMode,
				HelpId = HelpId
			};
			TextEditExt textEditExt = dialogWindow.AddStringControl("Code", ArmSoft.SME.Common.Resources.Code, 20);
			textEditExt.Text = dataRow.Code;
			textEditExt.IsReadOnly = true;
			TextEditExt textEditExt2 = dialogWindow.AddStringControl("BarCode", ArmSoft.SME.Common.Resources.BarCode, 20);
			textEditExt2.Text = dataRow.BarCode;
			textEditExt2.IsReadOnly = true;
			LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
			LayoutGroupExt container2 = dialogWindow.AddVerticalLayoutGroup(container, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.DataStoredInDB);
			TextEditExt textEditExt3 = dialogWindow.AddStringControl(container2, "CaptionDB", ArmSoft.SME.Common.Resources.Name, 200);
			textEditExt3.Text = dataRow.CaptionDB;
			textEditExt3.IsReadOnly = true;
			TextEditExt textEditExt4 = dialogWindow.AddStringControl(container2, "UnitDB", ArmSoft.SME.Common.Resources.QuantityUnitShort, 6);
			textEditExt4.Text = dataRow.UnitDB;
			textEditExt4.IsReadOnly = true;
			TextEditExt textEditExt5 = dialogWindow.AddStringControl(container2, "CPACodeDB", ArmSoft.SME.Common.Resources.CPACodeShort, 50);
			textEditExt5.Text = dataRow.CPACodeDB + " - " + dataRow.CPACaptionDB;
			textEditExt5.IsReadOnly = true;
			CheckEditExt checkEditExt = dialogWindow.AddBooleanControl(container2, "VATDB", ArmSoft.SME.Common.Resources.OptionsVAT);
			checkEditExt.IsChecked = dataRow.VATDB;
			checkEditExt.IsReadOnly = true;
			LayoutGroupExt container3 = dialogWindow.AddVerticalLayoutGroup(container, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.DataStoredInB2B);
			TextEditExt textEditExt6 = dialogWindow.AddStringControl(container3, "CaptionB2B", "", 200);
			textEditExt6.Text = dataRow.CaptionB2B;
			textEditExt6.IsReadOnly = true;
			TextEditExt textEditExt7 = dialogWindow.AddStringControl(container3, "UnitB2B", "", 6);
			textEditExt7.Text = dataRow.UnitB2B;
			textEditExt7.IsReadOnly = true;
			TextEditExt textEditExt8 = dialogWindow.AddStringControl(container3, "CPACodeB2B", "", 50);
			textEditExt8.Text = dataRow.CPACodeB2B + " - " + dataRow.CPACaptionB2B;
			textEditExt8.IsReadOnly = true;
			CheckEditExt checkEditExt2 = dialogWindow.AddBooleanControl(container3, "VATB2B", "");
			checkEditExt2.IsChecked = dataRow.VATB2B;
			checkEditExt2.IsReadOnly = true;
			dialogWindow.ShowDialog();
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}
}
