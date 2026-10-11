using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ArmSoft.ElScaleConnector;
using ArmSoft.SME.Client.Data.MetaData;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.DataProviders.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataView.Reports;
using ArmSoft.SME.Trade.UI.DataView.Reports.Goods;
using ArmSoft.SME.Trade.UI.Devices;
using ArmSoft.SME.Trade.UI.Devices.ElScale;
using ArmSoft.SME.Trade.UI.Devices.Terminal;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class ElScaleAndTerminalHelper
{
	private static Dictionary<string, ElScaleExportExtensionHandler> _elScaleTypes;

	private static Dictionary<string, TerminalHandler> _terminalTypes;

	public static Dictionary<string, ElScaleExportExtensionHandler> ElScaleTypes
	{
		get
		{
			_elScaleTypes = new Dictionary<string, ElScaleExportExtensionHandler>
			{
				{
					ElScaleTypesSE.DIGI,
					new ElScaleExportDIGIHandler()
				},
				{
					ElScaleTypesSE.TM_A,
					new ElScaleExportTM_AHandler()
				},
				{
					ElScaleTypesSE.CAS,
					new ElScaleExportCASHandler()
				},
				{
					ElScaleTypesSE.Shtrih_Slim_POS2,
					new ElScaleExportShtrihHandler()
				},
				{
					ElScaleTypesSE.CASNew,
					new ElScaleExportCASNewHandler()
				}
			};
			List<UserScriptsDefinitionsDataProvider.DataRow> rows = new UserScriptsDefinitionsDataProvider(Settings.CreateApiClient()).GetRows(null, withoutAccessCheck: true);
			byte b = (byte)(Convert.ToByte(ElScaleTypesSE.MertechDirect.Value) + 1);
			foreach (UserScriptsDefinitionsDataProvider.DataRow item in rows)
			{
				Type instanceType = DefinitionMappingsService.ResolveUserScript(item.Name, typeof(ElScaleExportExtensionHandler));
				_elScaleTypes.Add(b.ToString(), (ElScaleExportExtensionHandler)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, instanceType));
				b++;
			}
			return _elScaleTypes;
		}
	}

	public static Dictionary<string, TerminalHandler> TerminalTypes
	{
		get
		{
			if (_terminalTypes == null)
			{
				_terminalTypes = new Dictionary<string, TerminalHandler>();
				TerminalHandler terminalHandler = new HoneywellHandler();
				if (terminalHandler.SerializationImplemented)
				{
					_terminalTypes.Add(TerminalTypesSE.Honeywell.Value, terminalHandler);
				}
				terminalHandler = new CipherLabHandler();
				if (terminalHandler.SerializationImplemented)
				{
					_terminalTypes.Add(TerminalTypesSE.CipherLab.Value, terminalHandler);
				}
			}
			return _terminalTypes;
		}
	}

	public static void ExportGoodsForElScales(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, MessageWithHeaderReport report, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		List<PricesDataProvider.DataRow> rows = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new PricesDataProvider.Param
		{
			EndDate = expDate.ToDateOnly(),
			PriceType = row.PriceList
		}, withoutAccessCheck: true);
		string msg = ArmSoft.SME.Common.Resources.OlapExpDone;
		bool flag = true;
		if (!System.IO.Directory.Exists(Path.GetDirectoryName(row.ExportFile)))
		{
			msg = string.Format(ArmSoft.SME.Common.Resources.IncorrectDirectoryPath, row.ExportFile);
			flag = false;
		}
		else if (!File.Exists(row.ExportFile))
		{
			if (System.IO.Directory.Exists(row.ExportFile) || string.IsNullOrWhiteSpace(Path.GetFileName(row.ExportFile.TrimEnd('\\'))))
			{
				row.ExportFile = row.ExportFile.TrimEnd('\\') + "\\" + ElScaleTypes[row.ElModel.Trim()].ExportedFileName + ElScaleTypes[row.ElModel.Trim()].ExportedFileExtension;
			}
			File.Create(row.ExportFile).Close();
		}
		if (flag && ElScaleTypes[row.ElModel.Trim()].Export(new ExtensionUIComponents(), goods.ToList(), rows, row.AsNameChoose.ToString(), WeightItemInfo.GetInstance().Unit, WeightItemInfo.GetInstance().Prefix, PieceItemInfo.GetInstance().Unit, PieceItemInfo.GetInstance().Prefix, row.AsCodeExport.ToString(), row.ExportFile))
		{
			ElScaleOrTerminal elScaleOrTerminal = (ElScaleOrTerminal)Settings.DirectoryService.Load(DirectoryTypeSE.ElScaleOrTerminal.Value, row.Code);
			elScaleOrTerminal.ExportDate = DateTime.Now;
			Settings.DirectoryService.Store(elScaleOrTerminal, new DirStoreRequest());
		}
		report.AddError(ArmSoft.SME.Trade.Common.Resources.ExportForScale, msg);
	}

	public static void ExportGoodsForTerminals(DateTime expDate, List<TerminalRow> source, MessageWithHeaderReport report, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		string msg = ArmSoft.SME.Common.Resources.OlapExpDone;
		bool flag = true;
		if (!System.IO.Directory.Exists(Path.GetDirectoryName(row.ExportFile)))
		{
			msg = string.Format(ArmSoft.SME.Common.Resources.IncorrectDirectoryPath, row.ExportFile);
			flag = false;
		}
		else if (!File.Exists(row.ExportFile))
		{
			if (System.IO.Directory.Exists(row.ExportFile) || string.IsNullOrWhiteSpace(Path.GetFileName(row.ExportFile.TrimEnd('\\'))))
			{
				row.ExportFile = row.ExportFile.TrimEnd('\\') + "\\scanin" + _terminalTypes[row.ElModel.Trim()].FileExtension;
			}
			File.Create(row.ExportFile).Close();
		}
		if (flag)
		{
			TerminalHandler terminalHandler = _terminalTypes[row.ElModel.Trim()];
			Mouse.OverrideCursor = Cursors.Wait;
			terminalHandler.Initialize();
			using (StreamWriter streamWriter = File.CreateText(row.ExportFile))
			{
				streamWriter.Write(terminalHandler.Serialize(source, row.PriceList, expDate, Convert.ToBoolean(row.ExportForAdditionalQntUnit)));
			}
			Mouse.OverrideCursor = null;
			ElScaleOrTerminal elScaleOrTerminal = (ElScaleOrTerminal)Settings.DirectoryService.Load(DirectoryTypeSE.ElScaleOrTerminal.Value, row.Code);
			elScaleOrTerminal.ExportDate = DateTime.Now;
			Settings.DirectoryService.Store(elScaleOrTerminal, new DirStoreRequest());
		}
		report.AddError(ArmSoft.SME.Trade.Common.Resources.ExportForTerminal, msg);
	}

	public static void ExportGoodsForMassak(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		MassaKDirectExporterProgress massaKDirectExporterProgress = new MassaKDirectExporterProgress(expDate, goods, row);
		ElScaleGoodsExportResultDataView elScaleGoodsExportResultDataView = new ElScaleGoodsExportResultDataView(row.Name, row.ElModel);
		elScaleGoodsExportResultDataView.ParamRows = massaKDirectExporterProgress.ResultRows;
		elScaleGoodsExportResultDataView.Show();
	}

	public static void ExportForCAS(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		CasDirectExporterProgress casDirectExporterProgress = new CasDirectExporterProgress(expDate, goods, row);
		ElScaleGoodsExportResultDataView elScaleGoodsExportResultDataView = new ElScaleGoodsExportResultDataView(row.Name, row.ElModel);
		elScaleGoodsExportResultDataView.ParamRows = casDirectExporterProgress.ResultRows;
		elScaleGoodsExportResultDataView.Show();
	}

	public static void ExportForDigi(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		DigiDirectExporterProgress digiDirectExporterProgress = new DigiDirectExporterProgress(expDate, goods, row);
		ElScaleGoodsExportResultDataView elScaleGoodsExportResultDataView = new ElScaleGoodsExportResultDataView(row.Name, row.ElModel);
		elScaleGoodsExportResultDataView.ParamRows = digiDirectExporterProgress.ResultRows;
		elScaleGoodsExportResultDataView.Show();
	}

	public static void ExportForTMA(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		TMADirectExporterProgress tMADirectExporterProgress = new TMADirectExporterProgress(expDate, goods, row);
		ElScaleGoodsExportResultDataView elScaleGoodsExportResultDataView = new ElScaleGoodsExportResultDataView(row.Name, row.ElModel);
		elScaleGoodsExportResultDataView.ParamRows = tMADirectExporterProgress.ResultRows;
		elScaleGoodsExportResultDataView.Show();
	}

	public static void ExportForMertech(DateTime expDate, IReadOnlyList<MaterialCacheRow> goods, ElScalesAndTerminalsDataProvider.DataRow row)
	{
		MertechDirectExporterProgress mertechDirectExporterProgress = new MertechDirectExporterProgress(expDate, goods, row);
		ElScaleGoodsExportResultDataView elScaleGoodsExportResultDataView = new ElScaleGoodsExportResultDataView(row.Name, row.ElModel);
		elScaleGoodsExportResultDataView.ParamRows = mertechDirectExporterProgress.ResultRows;
		elScaleGoodsExportResultDataView.Show();
	}

	public static void DirectImportFromElScale()
	{
		ElScalesAndTerminalsDataProvider.DataRow dataRow = (ElScalesAndTerminalsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow.ElModel == ElScaleTypesSE.MassaK)
		{
			MassaKDirectImporterProgress massaKDirectImporterProgress = new MassaKDirectImporterProgress(dataRow.ExportFile, dataRow.Name);
			ElScaleGoodsShowDataView elScaleGoodsShowDataView = new ElScaleGoodsShowDataView(dataRow.Name, dataRow.ElModel);
			elScaleGoodsShowDataView.ParamRows = massaKDirectImporterProgress.ResultRows;
			elScaleGoodsShowDataView.Show();
		}
		else if (dataRow.ElModel == ElScaleTypesSE.CasDirect)
		{
			CasDirectImporterProgress casDirectImporterProgress = new CasDirectImporterProgress(dataRow);
			ElScaleGoodsShowDataView elScaleGoodsShowDataView2 = new ElScaleGoodsShowDataView(dataRow.Name, dataRow.ElModel);
			elScaleGoodsShowDataView2.ParamRows = casDirectImporterProgress.ResultRows;
			elScaleGoodsShowDataView2.Show();
		}
		else if (dataRow.ElModel == ElScaleTypesSE.DigiDirect)
		{
			DigiDirectImporterProgress digiDirectImporterProgress = new DigiDirectImporterProgress(dataRow.ExportFile);
			ElScaleGoodsShowDataView elScaleGoodsShowDataView3 = new ElScaleGoodsShowDataView(dataRow.Name, dataRow.ElModel);
			elScaleGoodsShowDataView3.ParamRows = digiDirectImporterProgress.ResultRows;
			elScaleGoodsShowDataView3.Show();
		}
		else if (dataRow.ElModel == ElScaleTypesSE.TMADirect)
		{
			TMADirectImporterProgress tMADirectImporterProgress = new TMADirectImporterProgress(dataRow);
			ElScaleGoodsShowDataView elScaleGoodsShowDataView4 = new ElScaleGoodsShowDataView(dataRow.Name, dataRow.ElModel);
			elScaleGoodsShowDataView4.ParamRows = tMADirectImporterProgress.ResultRows;
			elScaleGoodsShowDataView4.Show();
		}
		else if (dataRow.ElModel == ElScaleTypesSE.MertechDirect)
		{
			MertechDirectImporterProgress mertechDirectImporterProgress = new MertechDirectImporterProgress(dataRow);
			ElScaleGoodsShowDataView elScaleGoodsShowDataView5 = new ElScaleGoodsShowDataView(dataRow.Name, dataRow.ElModel);
			elScaleGoodsShowDataView5.ParamRows = mertechDirectImporterProgress.ResultRows;
			elScaleGoodsShowDataView5.Show();
		}
	}

	public static void EraseItemsFromScale()
	{
		ElScalesAndTerminalsDataProvider.DataRow dataRow = (ElScalesAndTerminalsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Mouse.OverrideCursor = Cursors.Wait;
		if (dataRow.ElModel == ElScaleTypesSE.MassaK)
		{
			MassaK.MassaKDriverDevices massaKDriverDevices;
			try
			{
				massaKDriverDevices = new MassaK.MassaKDriverDevices();
			}
			catch
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.MassakDeviceSettingsAreMissing);
			}
			int num = massaKDriverDevices.DeleteGoods();
			if (num != 0)
			{
				throw new ASException(DecodeMassakErrorCode(num));
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, "", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
		}
		else if (dataRow.ElModel == ElScaleTypesSE.CasDirect)
		{
			Cas cas = new Cas(dataRow.ExportFile);
			if (!cas.CheckConnection())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
			}
			if (!cas.EraseData())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, "", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
		}
		else if (dataRow.ElModel == ElScaleTypesSE.DigiDirect)
		{
			Digi digi = new Digi(dataRow.ExportFile);
			if (!digi.CheckConnection())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
			}
			if (!digi.EraseData())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, "", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
		}
		else if (dataRow.ElModel == ElScaleTypesSE.TMADirect)
		{
			TM_A tM_A = new TM_A(dataRow.ExportFile);
			if (!tM_A.CheckConnection())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
			}
			if (!tM_A.EraseData())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, "", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
		}
		else if (dataRow.ElModel == ElScaleTypesSE.MertechDirect)
		{
			Mertech mertech = new Mertech(dataRow.ExportFile, dataRow.Password, dataRow.DeleteAllItemsBeforeExporting);
			if (!mertech.CheckConnection())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
			}
			if (!mertech.EraseData())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, "", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
		}
		Mouse.OverrideCursor = null;
	}

	public static string DecodeMassakErrorCode(int resultCode)
	{
		return resultCode switch
		{
			0 => "Operation is successful", 
			-1 => ArmSoft.SME.Trade.Common.Resources.DriverIsBusy, 
			-2 => ArmSoft.SME.Trade.Common.Resources.FileNotFound, 
			-3 => ArmSoft.SME.Trade.Common.Resources.TerminalNotFound, 
			-4 => ArmSoft.SME.Trade.Common.Resources.ConnectionError, 
			-5 => ArmSoft.SME.Trade.Common.Resources.DataTransmittingError, 
			-6 => ArmSoft.SME.Trade.Common.Resources.DataReceivingError, 
			-7 => ArmSoft.SME.Trade.Common.Resources.DataAnalyzingError, 
			-8 => ArmSoft.SME.Trade.Common.Resources.SettingModeError, 
			-9 => ArmSoft.SME.Trade.Common.Resources.ThisErrorCodeIsNotSupportedByThisDriverVersion, 
			-10 => ArmSoft.SME.Trade.Common.Resources.NoMoreSpaceInDriversBuffer, 
			-11 => ArmSoft.SME.Trade.Common.Resources.NoMoreMemoryInTheTerminalForLoadingThisAmountOfData, 
			-12 => ArmSoft.SME.Trade.Common.Resources.NoMoreItemsInList, 
			-13 => ArmSoft.SME.Trade.Common.Resources.NoMoreRegistrationsOnUSBFlash, 
			-14 => ArmSoft.SME.Trade.Common.Resources.DataCompressionIsInProgress, 
			-15 => ArmSoft.SME.Trade.Common.Resources.DataCompressionHasBeenCompleted, 
			-1000 => ArmSoft.SME.Trade.Common.Resources.WrongPropertysValue, 
			-1001 => ArmSoft.SME.Trade.Common.Resources.ThisErrorCodeIsNotSupportedByThisDriverVersion, 
			_ => "Unknown error", 
		};
	}
}
