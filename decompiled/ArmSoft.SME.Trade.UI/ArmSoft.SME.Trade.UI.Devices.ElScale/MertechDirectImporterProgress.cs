using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ArmSoft.ElScaleConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using ArmSoft.SME.Core.UI.Progress;
using ArmSoft.SME.Trade.Client.DataProviders.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class MertechDirectImporterProgress
{
	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly ElScalesAndTerminalsDataProvider.DataRow _row;

	public List<ElScaleGoodsShowDataProvider.DataRow> ResultRows;

	public MertechDirectImporterProgress(ElScalesAndTerminalsDataProvider.DataRow row)
	{
		_row = row;
		_progress = new MultilineProgressWindow
		{
			Title = ArmSoft.SME.Trade.Common.Resources.ViewScaleItemList
		};
		_phases.Add(ArmSoft.SME.Common.Resources.Connecting);
		_phases.Add(ArmSoft.SME.Common.Resources.RowsLoaded);
		_progress.PreparePrimaryPhases(_phases);
		_progress.BackgroundWorkerDoWork += BackgroundWorkerDoWorkEventHandler;
		_progress.ShowDialogWithExceptionHandling();
	}

	private void BackgroundWorkerDoWorkEventHandler(object sender, DoWorkEventArgs e)
	{
		DirectImportForMertech();
	}

	public void DirectImportForMertech()
	{
		ScalePhase scalePhase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Connecting,
			DisableCancel = true
		};
		_progress.ProcessPhase(scalePhase);
		Mertech mertech = new Mertech(_row.ExportFile, _row.Password, _row.DeleteAllItemsBeforeExporting);
		if (!mertech.CheckConnection())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
		}
		scalePhase.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase);
		ScalePhase phase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.RowsLoaded,
			DisableCancel = true
		};
		_progress.ProcessPhase(phase);
		ResultRows = new List<ElScaleGoodsShowDataProvider.DataRow>();
		Mertech.ProductFileClass productFileClass = null;
		try
		{
			productFileClass = mertech.Download();
		}
		catch
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ElScalePasswordIsIncorrect);
		}
		List<ElScaleGoodsShowDataProvider.DataRow> list = new List<ElScaleGoodsShowDataProvider.DataRow>();
		if (productFileClass != null)
		{
			foreach (Mertech.Product item in productFileClass.Products)
			{
				ElScaleGoodsShowDataProvider.DataRow dataRow = new ElScaleGoodsShowDataProvider.DataRow
				{
					CodeInScale = item.Code.ToString(),
					Name = item.Name,
					PLU = item.PluNumber.Value,
					Price = (decimal)item.Price,
					IsWeight = (item.ProductType == ScaleItemType.Weight.ToString().ToUpper()),
					Ingredients = item.Ingredients
				};
				if (!string.IsNullOrWhiteSpace(item.BarcodePrefixType) && item.BarcodePrefixType == "GROUP_CODE")
				{
					dataRow.BarCodePrefix = int.Parse(productFileClass.Categories.FirstOrDefault((Mertech.Category x) => x.IdCategory == item.Category).Name);
				}
				list.Add(dataRow);
			}
		}
		_progress.ProcessPhase(phase);
		ResultRows = list;
	}
}
