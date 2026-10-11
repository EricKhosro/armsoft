using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Devices.Terminal;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class TerminalExportWindow : DialogWindow
{
	private DateEditControl _date;

	private DropDownControl _priceType;

	private DropDownControl _terminalType;

	private CheckEditExt _forAltUnit;

	private readonly List<TerminalRow> _source;

	private TerminalHandler _handler;

	private string _fileName;

	private readonly bool _allowChangeUnit;

	public TerminalExportWindow(List<TerminalRow> source, bool allowChangeUnit = false)
	{
		Caption = ArmSoft.SME.Trade.Common.Resources.ExportForTerminal;
		Name = "ExportForTerminal";
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		_source = source;
		_allowChangeUnit = allowChangeUnit;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		_date = AddDateControl("_date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true);
		_priceType = AddDropDownControl("_priceType", ArmSoft.SME.Common.Resources.PriceList, new PriceTypes(), isRequired: true);
		_terminalType = AddDropDownControl("_terminalType", ArmSoft.SME.Trade.Common.Resources.TerminalType, new SmartEnum(typeof(TerminalTypesSE), ElScaleAndTerminalHelper.TerminalTypes.Keys.ToList()), isRequired: true);
		if (_allowChangeUnit)
		{
			_forAltUnit = AddBooleanControl("_forAltUnit", ArmSoft.SME.Trade.Common.Resources.ExportForAltUnit);
		}
		AssignDefaultValues();
	}

	private void AssignDefaultValues()
	{
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_priceType.Code = Settings.ParametersService.DefaultSalePrice();
		_terminalType.Code = Convert.ToString(TerminalTypesSE.Honeywell.Value);
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			base.DialogValidation(args);
			if (ElScaleAndTerminalHelper.TerminalTypes.TryGetValue(_terminalType.Code, out var value))
			{
				_handler = value;
			}
			if (_handler != null)
			{
				_fileName = Settings.CommonCoreHelper.GetFileNameFromExportDialog(_handler.FileExtension, _handler.FilesFilter, "scanin");
				if (!string.IsNullOrWhiteSpace(_fileName))
				{
					Export();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void Export()
	{
		Mouse.OverrideCursor = Cursors.Wait;
		_handler.Initialize();
		StreamWriter streamWriter = File.CreateText(_fileName);
		streamWriter.Write(_handler.Serialize(_source, _priceType.Code, _date.EditValue.ToDateTime().Value, _allowChangeUnit && Convert.ToBoolean(_forAltUnit.IsChecked)));
		streamWriter.Flush();
		streamWriter.Close();
		Mouse.OverrideCursor = null;
		ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OlapExpDone, ArmSoft.SME.Trade.Common.Resources.ExportForTerminal, MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}
}
