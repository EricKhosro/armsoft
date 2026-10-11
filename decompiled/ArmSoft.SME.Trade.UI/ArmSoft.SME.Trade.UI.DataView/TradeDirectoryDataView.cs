using System;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Dialog;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.DataView;

public abstract class TradeDirectoryDataView<R, P> : DirectoryDataView<R, P> where R : ExtendableRow, IRowWithKey, new() where P : class, new()
{
	private bool? _preservePreviousSelectionByBarcodeFind;

	protected bool PreservePreviousSelectionByBarcodeFind
	{
		get
		{
			if (!_preservePreviousSelectionByBarcodeFind.HasValue)
			{
				DialogValues values = Settings.DialogService.GetValues("FindRowByBarCodeDialog");
				if (values?.Values != null && values.Values.TryGetValue("PreservePreviousSelection".ToUpper(), out var value))
				{
					_preservePreviousSelectionByBarcodeFind = value.ToBoolean();
				}
				else
				{
					_preservePreviousSelectionByBarcodeFind = false;
				}
			}
			return _preservePreviousSelectionByBarcodeFind.Value;
		}
		set
		{
			_preservePreviousSelectionByBarcodeFind = value;
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		if (currentViewPanel.FocusedRow() != null && AllowFindRowByMTBarcode)
		{
			currentViewPanel.AddContextFunction("ContextFunctions", "FindRowByBarcode", Resources.SearchByBarcodeContextMenu, FindRowByBarcode, "", "", storeOpContext: true);
		}
	}

	public override void OnScanner(string scannerData)
	{
		if (AllowFindRowByMTBarcode)
		{
			InitContextFunctionsHelper.FocusRowByBarcode(scannerData, MTCodeColumnName, PreservePreviousSelectionByBarcodeFind);
		}
	}

	public override void OnF2()
	{
		try
		{
			if (AllowFindRowByMTBarcode)
			{
				InitContextFunctionsHelper.FindRowByBarcode(MTCodeColumnName, out var preservePreviousSelection);
				PreservePreviousSelectionByBarcodeFind = preservePreviousSelection;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FindRowByBarcode()
	{
		InitContextFunctionsHelper.FindRowByBarcode(MTCodeColumnName, out var preservePreviousSelection);
		PreservePreviousSelectionByBarcodeFind = preservePreviousSelection;
	}
}
