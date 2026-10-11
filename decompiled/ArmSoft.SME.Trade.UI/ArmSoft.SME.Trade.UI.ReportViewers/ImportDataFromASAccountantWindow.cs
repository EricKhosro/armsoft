using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Exchange;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.ASReport;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.ReportViewers;

public class ImportDataFromASAccountantWindow
{
	private static ApiClient _apiClient;

	public static bool ImportDataFromASAccountant(out List<string> repHeadersFooters, out List<ImportDataFromASAccountantReportResult> repRows)
	{
		repHeadersFooters = new List<string>();
		repRows = new List<ImportDataFromASAccountantReportResult>();
		int successCount = 0;
		int errorCount = 0;
		try
		{
			string fileNameFromImportDialog = Settings.CommonCoreHelper.GetFileNameFromImportDialog(".xml", "XML Files (.xml)|*.xml");
			repHeadersFooters.Add(fileNameFromImportDialog);
			if (string.IsNullOrWhiteSpace(fileNameFromImportDialog))
			{
				return false;
			}
			new XDocument();
			using (StreamReader textReader = new StreamReader(fileNameFromImportDialog, Encoding.Default))
			{
				XDocument xDocument = XDocument.Load(textReader);
				_apiClient = Settings.CreateApiClient();
				foreach (XElement item in xDocument.Elements())
				{
					if (!(item.Name.LocalName == "Exchange"))
					{
						continue;
					}
					foreach (XElement item2 in item.Elements())
					{
						if (item2.Name.LocalName == "Material")
						{
							XMLImportGoodOrService(isGood: true, item2, repRows, ref successCount, ref errorCount);
						}
					}
					repHeadersFooters.Add(errorCount.ToString());
					repHeadersFooters.Add(successCount.ToString());
				}
			}
			Settings.DirectoryService.RefreshDirectoryUICaches(DirectoryTypeSE.Goods.Value);
		}
		catch (Exception ex)
		{
			throw new ASException(ex.Message);
		}
		return true;
	}

	private static void XMLImportGoodOrService(bool isGood, XElement item, List<ImportDataFromASAccountantReportResult> repRows, ref int successCount, ref int errorCount)
	{
		if (!isGood)
		{
			return;
		}
		string docNumOrCode = "";
		try
		{
			docNumOrCode = item.Elements().FirstOrDefault((XElement e) => e.Name.LocalName == "Code")?.Value.Trim();
			ExchangeResponse exchangeResponse = _apiClient.Exchange.Import(new ExchangeRequest(item.Name.LocalName, item.ToString(), ImportMode.PreserveState));
			if (exchangeResponse.Success)
			{
				repRows.Add(new ImportDataFromASAccountantReportResult
				{
					Imported = ArmSoft.SME.Common.Resources.Yes,
					Type = ArmSoft.SME.Trade.Common.Resources.MaterialValue,
					DocNumOrCode = docNumOrCode,
					Reason = ""
				});
				successCount++;
			}
			else
			{
				repRows.Add(new ImportDataFromASAccountantReportResult
				{
					Imported = ArmSoft.SME.Common.Resources.No,
					Type = ArmSoft.SME.Trade.Common.Resources.MaterialValue,
					DocNumOrCode = docNumOrCode,
					Reason = exchangeResponse.Message
				});
				errorCount++;
			}
		}
		catch (Exception ex)
		{
			repRows.Add(new ImportDataFromASAccountantReportResult
			{
				Imported = ArmSoft.SME.Common.Resources.No,
				Type = ArmSoft.SME.Trade.Common.Resources.MaterialValue,
				DocNumOrCode = docNumOrCode,
				Reason = ex.Message
			});
			errorCount++;
		}
	}
}
