using System.Collections.Generic;
using System.IO;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Models.Exchange;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10034)]
public class MTComplects : TradeDataView<MTComplectsDataProvider.DataRow, MTComplectsDataProvider.Param>
{
	public override string HelpId => "Purchases_Lists_Productsandservicessublist_Complects";

	public override bool AllowAdd => false;

	public override bool AllowEdit => false;

	public override bool AllowDelete => false;

	public override bool AllowExport => true;

	public MTComplects()
	{
		DataProvider = new MTComplectsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("MTID", value: false);
		dvp.Set_ColumnVisible("Unit", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("MTCode");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MTComplectsParts).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowComplectPart", Resources.MTComplectation, ShowComplectPart);
		}
	}

	private void ShowComplectPart()
	{
		MTComplectsDataProvider.DataRow dataRow = (MTComplectsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			MTComplectsParts mTComplectsParts = new MTComplectsParts();
			mTComplectsParts.ComplectCode = dataRow.MTCode;
			mTComplectsParts.Show();
		}
	}

	public override void ExportToXML(string fileName, MTComplectsDataProvider.DataRow row)
	{
		List<MTComplectsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<MTComplectsDataProvider.DataRow>();
		List<ExchangeBase> list = new List<ExchangeBase>();
		foreach (MTComplectsDataProvider.DataRow item3 in selectedOrFocusedRows)
		{
			MTComplect mTComplect = new MTComplect
			{
				ComplectParts = new List<ComplectPart>()
			};
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(item3.MTCode);
			foreach (MaterialCacheRow.ComplectPartRow complectPart in item.ComplectParts)
			{
				ComplectPart item2 = new ComplectPart
				{
					DestCode = item.MTCode,
					ItemType = complectPart.ItemType,
					SourceCode = TradeSettings.TradeCacheService.Material.GetItemById(complectPart.ItemId, throwExceptionNotExits: true).MTCode,
					Unit = complectPart.QntUnit,
					Quantity = complectPart.Quantity,
					ComplectQuantity = complectPart.ComplectQnt,
					SummPart = complectPart.PricePart
				};
				mTComplect.ComplectParts.Add(item2);
			}
			list.Add(mTComplect);
		}
		byte[] array = TradeSettings.CreateTradeClient().Exchange.Export(list);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		fileStream.Write(array);
	}
}
