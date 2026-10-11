using System;
using System.Collections.Generic;
using System.ComponentModel;
using ArmSoft.ElScaleConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using ArmSoft.SME.Core.UI.Progress;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class MassaKDirectImporterProgress
{
	public List<ElScaleGoodsShowDataProvider.DataRow> ResultRows = new List<ElScaleGoodsShowDataProvider.DataRow>();

	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly string _ipAddress;

	private readonly string _name;

	public MassaKDirectImporterProgress(string iPAddress, string name)
	{
		_ipAddress = iPAddress;
		_name = name;
		_progress = new MultilineProgressWindow
		{
			Title = ArmSoft.SME.Trade.Common.Resources.ViewScaleItemList
		};
		_phases.Add(ArmSoft.SME.Common.Resources.Connecting);
		_phases.Add(ArmSoft.SME.Common.Resources.RowsProcessed);
		_progress.PreparePrimaryPhases(_phases);
		_progress.BackgroundWorkerDoWork += BackgroundWorkerDoWorkEventHandler;
		_progress.ShowDialogWithExceptionHandling();
	}

	private void BackgroundWorkerDoWorkEventHandler(object sender, DoWorkEventArgs e)
	{
		DirectImportFromElScale();
	}

	public void DirectImportFromElScale()
	{
		ScalePhase scalePhase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Connecting,
			DisableCancel = true
		};
		_progress.ProcessPhase(scalePhase);
		MassaK.MassaKDriverDevices massaKDriverDevices;
		MassaK.MassaKDriverGoods massaKDriverGoods;
		try
		{
			massaKDriverDevices = new MassaK.MassaKDriverDevices();
			massaKDriverGoods = new MassaK.MassaKDriverGoods();
		}
		catch
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.MassakDeviceSettingsAreMissing);
		}
		scalePhase.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase);
		int device = massaKDriverDevices.GetDevice(_ipAddress, _name);
		if (device != 0)
		{
			throw new ASException(ElScaleAndTerminalHelper.DecodeMassakErrorCode(device));
		}
		if (device == 0)
		{
			ScalePhase scalePhase2 = new ScalePhase
			{
				Name = ArmSoft.SME.Common.Resources.RowsProcessed,
				DisableCancel = false,
				Row = 0
			};
			_progress.ProcessPhase(scalePhase2);
			int num = 1;
			massaKDriverGoods.Clear();
			massaKDriverDevices.ReadGoods();
			int num2 = massaKDriverGoods.GetFirst();
			while (num2 == 0 && !_progress.Canceled)
			{
				ResultRows.Add(new ElScaleGoodsShowDataProvider.DataRow
				{
					ID = massaKDriverGoods.ID,
					CodeInScale = massaKDriverGoods.Code,
					Name = massaKDriverGoods.Name,
					BarCodePrefix = massaKDriverGoods.BarcodePrefix,
					IsWeight = (massaKDriverGoods.GoodsTypeID == 0),
					Ingredients = massaKDriverGoods.Ingredients,
					PLU = massaKDriverGoods.PLU,
					Price = DecimalExtension.RoundAs(massaKDriverGoods.Price / 100 + massaKDriverGoods.Price % 100, 2)
				});
				num2 = massaKDriverGoods.GetNext();
				scalePhase2.Row = num++;
				_progress.ProcessPhase(scalePhase2);
			}
			massaKDriverGoods.Clear();
			scalePhase2.EndDate = DateTime.Now;
			_progress.ProcessPhase(scalePhase2);
		}
	}
}
