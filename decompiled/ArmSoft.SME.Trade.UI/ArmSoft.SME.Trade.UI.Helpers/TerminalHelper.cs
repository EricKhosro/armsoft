using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.Devices.Terminal;

namespace ArmSoft.SME.Trade.UI.Helpers;

public static class TerminalHelper
{
	public class TerminalImporter
	{
		private readonly Window _owner;

		private readonly Dictionary<string, TerminalHandler> _handlers;

		private TerminalHandler _handler;

		private Dictionary<int, string> _reportRows;

		public TerminalImporter(Window owner)
		{
			_owner = owner;
			_handlers = new Dictionary<string, TerminalHandler>();
			TerminalHandler terminalHandler = new HoneywellHandler();
			if (terminalHandler.DeserializationImplemented)
			{
				_handlers.Add(Convert.ToString(TerminalTypesSE.Honeywell.Value), terminalHandler);
			}
			terminalHandler = new CipherLabHandler();
			if (terminalHandler.DeserializationImplemented)
			{
				_handlers.Add(Convert.ToString(TerminalTypesSE.CipherLab.Value), terminalHandler);
			}
		}

		public bool ImportFromFile(Action<decimal, MaterialCacheRow, MaterialCacheRow.BarCodeRow> action, bool allowZeroQuantity = false)
		{
			try
			{
				string empty = string.Empty;
				if (_handlers.Count > 1)
				{
					SmartEnum smartEnum = new SmartEnum(typeof(VatTypesSE), _handlers.Keys.ToList());
					ModalBrowser modalBrowser = new ModalBrowser(smartEnum)
					{
						WindowStartupLocation = WindowStartupLocation.CenterScreen,
						Owner = _owner
					};
					if (!modalBrowser.Show())
					{
						return false;
					}
					empty = smartEnum.GetRowByIndex(modalBrowser.SelectedRowIndex).Code;
				}
				else
				{
					if (_handlers.Count != 1)
					{
						return false;
					}
					empty = _handlers.First().Key;
				}
				if (_handlers.ContainsKey(empty))
				{
					_handler = _handlers[empty];
				}
				if (_handler != null)
				{
					_handler.Initialize();
					string fileNameFromImportDialog = Settings.CommonCoreHelper.GetFileNameFromImportDialog(_handler.FileExtension, _handler.FilesFilter);
					if (!string.IsNullOrWhiteSpace(fileNameFromImportDialog))
					{
						_reportRows = new Dictionary<int, string>();
						foreach (TerminalRow item in _handler.Deserialize(fileNameFromImportDialog))
						{
							try
							{
								_handler.Validate(allowZeroQuantity, item, out var goodsRow, out var goodBarCodeRow);
								action(item.Quantity.Value, goodsRow, goodBarCodeRow);
							}
							catch (TerminalHandlerException ex)
							{
								_reportRows.Add(ex.LineNumber, ex.Message);
							}
							catch (Exception)
							{
								throw;
							}
						}
					}
				}
			}
			catch (Exception ex3)
			{
				Settings.ErrorHandler(ex3);
			}
			return true;
		}

		public void ShowReport()
		{
			if (_reportRows != null && _reportRows.Count > 0)
			{
				new ImportFromFileReportViewer(_reportRows).Show();
			}
		}
	}
}
