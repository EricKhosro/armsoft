using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class MTFavoritesDataProvider : DataProvider<MTFavoritesDataProvider.DataRow, MTFavoritesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class MTFavoritesRowKey
		{
			public int MTID { get; set; }

			public string MTCode { get; set; }

			public string MTUnitCode { get; set; }

			public int UserID { get; set; }
		}

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string GroupCode { get; set; }

		public string GroupCaption { get; set; }

		public string Caption { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public bool HasUnit { get; set; }

		public decimal Price { get; set; }

		public bool HasPrice { get; set; }

		public bool HasImage { get; set; }

		public bool IsGroup { get; set; }

		public string HotKey { get; set; }

		public bool HasHotKey { get; set; }

		public int UserID { get; set; }

		public int OrderNum { get; set; }

		public byte FavType { get; set; }

		public string ImgStr { get; set; }

		public object GetKey()
		{
			return new MTFavoritesRowKey
			{
				MTID = MTID,
				MTCode = MTCode,
				MTUnitCode = UnitCode,
				UserID = UserID
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

		bool IMatchedUpdateKey.Matched(object key)
		{
			MTFavoritesRowKey mTFavoritesRowKey = (MTFavoritesRowKey)key;
			if (FavType == 1)
			{
				if (mTFavoritesRowKey.MTID == MTID && mTFavoritesRowKey.UserID == UserID)
				{
					return mTFavoritesRowKey.MTUnitCode == UnitCode;
				}
				return false;
			}
			if (mTFavoritesRowKey.MTCode == MTCode && mTFavoritesRowKey.UserID == UserID)
			{
				return mTFavoritesRowKey.MTID == MTID;
			}
			return false;
		}
	}

	public class Param
	{
		public bool OnlyMaterials { get; set; }

		public bool OnlyGroups { get; set; }

		public string GroupCode { get; set; }

		public int UserId { get; set; }

		public string PriceType { get; set; }

		public DateOnly Date { get; set; }

		public string VatType { get; set; }

		public string CurrCode { get; set; }
	}

	public override string Name => "MTFavorites";

	public MTFavoritesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
