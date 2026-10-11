using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PartnersGoodsDataProvider : DataProvider<PartnersGoodsDataProvider.DataRow, PartnersGoodsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class PartnerGoodsRowKey
		{
			public int PartID { get; set; }

			public string PartItemCode { get; set; }

			public string PartItemUnit { get; set; }
		}

		public int PartID { get; set; }

		public string PartCode { get; set; }

		public string PartCaption { get; set; }

		public string PartTIN { get; set; }

		public string Type { get; set; }

		public string PartItemCode { get; set; }

		public string PartItemCaption { get; set; }

		public string PartItemUnit { get; set; }

		public short PartItemUnitID { get; set; }

		public string PartItemUnitName { get; set; }

		public decimal Coeff { get; set; }

		public int ItemID { get; set; }

		public string ItemCode { get; set; }

		public string ItemCaption { get; set; }

		public string ItemUnit { get; set; }

		public string ItemUnitName { get; set; }

		public object GetKey()
		{
			return new PartnerGoodsRowKey
			{
				PartID = PartID,
				PartItemCode = PartItemCode,
				PartItemUnit = PartItemUnit
			};
		}

		public string GetCode()
		{
			return "";
		}

		public string GetName()
		{
			return "";
		}

		public bool Matched(object key)
		{
			PartnerGoodsRowKey partnerGoodsRowKey = (PartnerGoodsRowKey)key;
			if (partnerGoodsRowKey.PartID == PartID && partnerGoodsRowKey.PartItemCode == PartItemCode)
			{
				return partnerGoodsRowKey.PartItemUnit == PartItemUnit;
			}
			return false;
		}
	}

	public class Param
	{
		public string PartCode { get; set; }

		public string MaterialCode { get; set; }
	}

	public override string Name => "PartnersGoods";

	public PartnersGoodsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
