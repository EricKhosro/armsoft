using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class Goods : DataProvider<Goods.DataRow, Goods.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Name { get; set; }

		public string FullName { get; set; }

		public string Type { get; set; }

		public string TypeName { get; set; }

		public string Group { get; set; }

		public string CPACode { get; set; }

		public string GroupCaption { get; set; }

		public string GroupPath { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public string AltUnit { get; set; }

		public string AltUnitBrief { get; set; }

		public string BarCode { get; set; }

		public string Description { get; set; }

		public string ExternalCode { get; set; }

		public string ExpenseMethod { get; set; }

		public string ExpenseMethodName { get; set; }

		public bool ShowInPriceList { get; set; }

		public int MainSupplierId { get; set; }

		public string MainSupplier { get; set; }

		public string Country { get; set; }

		public string CountryName { get; set; }

		public string Producer { get; set; }

		public string ProducerName { get; set; }

		public string Property1 { get; set; }

		public string Property2 { get; set; }

		public decimal MinQty { get; set; }

		public decimal MaxQty { get; set; }

		public decimal Extra { get; set; }

		public decimal Bonus { get; set; }

		public bool IgnoreBonus { get; set; }

		public decimal Point { get; set; }

		public bool VAT { get; set; }

		public decimal EcoFeeRate { get; set; }

		public bool IsWeight { get; set; }

		public bool CheckMarking { get; set; }

		public string LabelUnit { get; set; }

		public string LabelUnitBrief { get; set; }

		public decimal LabelCoeff { get; set; }

		public decimal Coeff { get; set; }

		public decimal Discount { get; set; }

		public int? PLUCode { get; set; }

		public string SubstituteItemsGroupCode { get; set; }

		public string SubstituteItemsGroupCaption { get; set; }

		public string Body { get; set; }

		public string LastModifierName { get; set; } = "";

		public DateTime? LastModifyDate { get; set; }

		public string CreatorName { get; set; } = "";

		public DateTime? CreationDate { get; set; }

		public string CaptionEN { get; set; }

		public string CaptionRU { get; set; }

		public string AdditionalDescr1 { get; set; }

		public string AdditionalDescr2 { get; set; }

		public string ImgStr => MTID + "," + Unit.Trim();

		public byte[] Image { get; set; }

		public bool ImageExists { get; set; }

		public MaterialCacheRow MaterialRow { get; set; }

		public DataRow()
		{
			MTID = -1;
			MTCode = "";
			Name = "";
			FullName = "";
			Type = "";
			TypeName = "";
			Group = "";
			CPACode = "";
			GroupCaption = "";
			GroupPath = "";
			Unit = "";
			UnitBrief = "";
			AltUnit = "";
			AltUnitBrief = "";
			BarCode = "";
			Description = "";
			ExternalCode = "";
			ExpenseMethod = "";
			ExpenseMethodName = "";
			Country = "";
			CountryName = "";
			Producer = "";
			ProducerName = "";
			Property1 = "";
			Property2 = "";
			MinQty = 0m;
			MaxQty = 0m;
			EcoFeeRate = 0m;
			Coeff = 1m;
			Discount = 0m;
			SubstituteItemsGroupCode = "";
			SubstituteItemsGroupCaption = "";
			LastModifierName = "";
			CreatorName = "";
			CaptionEN = "";
			CaptionRU = "";
			AdditionalDescr1 = "";
			AdditionalDescr2 = "";
			Image = null;
			ImageExists = false;
			MainSupplierId = -1;
			MainSupplier = "";
		}

		public object GetKey()
		{
			return MTID;
		}

		public string GetCode()
		{
			return MTCode;
		}

		public string GetName()
		{
			return Name;
		}

		public bool Matched(object key)
		{
			return MTID == (int)key;
		}

		public override object ReferenceValue(string name, string source)
		{
			if (source.StartsWith("MaterialCache") && MaterialRow != null)
			{
				return MaterialRow.GetReferenceValue(source);
			}
			return null;
		}
	}

	public class Param
	{
		public string Group { get; set; }

		public string ItemType { get; set; }

		public string Show { get; set; }

		public string PriceType { get; set; }

		public string MTCode { get; set; }

		public bool ShowExtended { get; set; }

		public bool ShowRems { get; set; }

		public DateTime? LastModifiedDate { get; set; }
	}

	public override string Name => "Goods";

	public Goods(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
