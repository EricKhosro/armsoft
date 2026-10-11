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

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class DigiDirectImporterProgress
{
	public List<ElScaleGoodsShowDataProvider.DataRow> ResultRows = new List<ElScaleGoodsShowDataProvider.DataRow>();

	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly string _ipAddress;

	public DigiDirectImporterProgress(string ipAddress)
	{
		_ipAddress = ipAddress;
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
		Digi digi = new Digi(_ipAddress);
		bool flag = digi.CheckConnection();
		if (!flag)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
		}
		scalePhase.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase);
		if (!flag)
		{
			return;
		}
		ScalePhase scalePhase2 = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.RowsProcessed,
			DisableCancel = false,
			Row = 0
		};
		_progress.ProcessPhase(scalePhase2);
		int num = 1;
		Digi.DigiItem digiItem = null;
		do
		{
			if (_progress.Canceled)
			{
				return;
			}
			digiItem = digi.Download();
			if (digiItem != null)
			{
				ResultRows.Add(new ElScaleGoodsShowDataProvider.DataRow
				{
					CodeInScale = digiItem.ItemCode.ToString(),
					Name = digiItem.Name1,
					BarCodePrefix = digiItem.Prefix,
					IsWeight = digiItem.IsWeight,
					PLU = digiItem.PluNumber,
					LabelN = digiItem.LabelN,
					BarCodeN = digiItem.BarCodeN,
					Price = DecimalExtension.RoundAs(digiItem.UnitPrice, 2)
				});
				scalePhase2.Row = num++;
				_progress.ProcessPhase(scalePhase2);
			}
		}
		while (digiItem != null);
		scalePhase2.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase2);
	}
}
