using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Devices;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class ElScaleExportWindow : DialogWindow
{
	private DateEditControl _date;

	private DropDownControl _priceType;

	private DropDownControl _nameInterpretation;

	private DropDownControl _scaleType;

	private DropDownControl _exportNextCode;

	private readonly List<MaterialCacheRow> _goods;

	public ElScaleExportWindow(List<MaterialCacheRow> goods)
	{
		_goods = goods;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		if (string.IsNullOrWhiteSpace(WeightItemInfo.GetInstance().Unit))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ErrForExportToScaleMustBeSetUnit, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return;
		}
		Caption = ArmSoft.SME.Trade.Common.Resources.ExportForScale;
		Name = "ExportForScale";
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		AddStoreValuesButton(container);
		_date = AddDateControl(container, "_date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true);
		_priceType = AddDropDownControl("DD_PRICETYPE", ArmSoft.SME.Common.Resources.PriceList, new PriceTypes(), isRequired: true, allowAdditionButtons: false, storeValue: true);
		_nameInterpretation = AddDropDownControl("DD_NAMEINTERPRETATION", ArmSoft.SME.Trade.Common.Resources.NameInterpretation, new SmartEnum(typeof(NameInterpretationsSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
		Dictionary<string, string> elScaleTypes = ElScaleAndTerminalHelper.ElScaleTypes.ToDictionary((KeyValuePair<string, ElScaleExportExtensionHandler> item) => item.Key, (KeyValuePair<string, ElScaleExportExtensionHandler> item) => item.Value.ScaleType);
		_scaleType = AddDropDownControl("DD_SCALETYPE", ArmSoft.SME.Trade.Common.Resources.ScaleType, new ElScaleOrTerminalTypes(elScaleTypes, isScale: true), isRequired: true, allowAdditionButtons: false, storeValue: true);
		_exportNextCode = AddDropDownControl("DD_EXPORTNEXTCODE", string.Format(ArmSoft.SME.Trade.Common.Resources.ExportElScaleNextCodeAs, Environment.NewLine), new SmartEnum(typeof(ElScaleExportNextCodesSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
		AssignDefaultValues();
	}

	private void AssignDefaultValues()
	{
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_priceType.Code = Settings.ParametersService.DefaultSalePrice();
		_nameInterpretation.Code = NameInterpretationsSE.InterpretationName;
		_scaleType.Code = ElScaleTypesSE.DIGI;
		_exportNextCode.Code = ElScaleExportNextCodesSE.MaterialCode;
	}

	protected override void DialogValidation(DialogWindowValidationArgs arfs)
	{
		try
		{
			base.DialogValidation(arfs);
			PricesDataProvider.Param param = new PricesDataProvider.Param
			{
				EndDate = _date.EditValue,
				PriceType = _priceType.Code
			};
			List<PricesDataProvider.DataRow> rows = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param, withoutAccessCheck: true);
			WeightItemInfo instance = WeightItemInfo.GetInstance();
			PieceItemInfo instance2 = PieceItemInfo.GetInstance();
			if (ElScaleAndTerminalHelper.ElScaleTypes[_scaleType.Code].Export(new ExtensionUIComponents(this), _goods, rows, _nameInterpretation.Code, instance.Unit, instance.Prefix, instance2.Unit, instance2.Prefix, _exportNextCode.Code))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OlapExpDone, ArmSoft.SME.Trade.Common.Resources.ExportForScale, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
