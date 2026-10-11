using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.Directories;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DataViewsData;

public class b2bUnitedProductsDataProviderBase : DataProvider<b2bUnitedProductRowBase, b2bUnitedProductsDataProviderBase.Param>
{
	public class Param
	{
		public string ProductName { get; set; }

		public string Sphere { get; set; }
	}

	public override string Name => "b2bUnitedProducts";

	public b2bUnitedProductsDataProviderBase()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Resources.UnitedProductsDirectory
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "Barcode",
				Caption = Resources.BarCode,
				Source = "Barcode",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "Name",
				Caption = Resources.Name,
				Source = "Name",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "FullName",
				Caption = Resources.FullName,
				Source = "FullName",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "CPACode",
				Caption = Resources.CPACodeShort,
				Source = "CPACode",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "CPACaption",
				Caption = Resources.CPACaption,
				Source = "CPACaption",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "IsVatTaxable",
				Caption = Resources.OptionsVAT,
				Source = "IsVatTaxable",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "Description",
				Caption = Resources.Description,
				Source = "Description",
				Type = SupportedType.String
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<b2bUnitedProductRowBase> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		try
		{
			DataProviderResult<b2bUnitedProductRowBase> dataProviderResult = new DataProviderResult<b2bUnitedProductRowBase>
			{
				Rows = new List<b2bUnitedProductRowBase>()
			};
			using CloudClient cloudClient = ArmSoft.SME.Core.UI.Settings.CloudClient;
			cloudClient.Token = ArmSoft.SME.Core.UI.Settings.GetApplicationToken();
			List<UnitedProduct> list = null;
			string sphere = param.Sphere;
			if (!(sphere == "1"))
			{
				if (sphere == "2")
				{
					list = cloudClient.Directories.GetUnitedProducts(param.ProductName, SphereType.Pharmacy);
				}
			}
			else
			{
				list = cloudClient.Directories.GetUnitedProducts(param.ProductName, SphereType.Grocery | SphereType.Economic);
			}
			if (list != null)
			{
				foreach (UnitedProduct item in list)
				{
					dataProviderResult.Rows.Add(GetGoodFromCloudProduct(item));
				}
			}
			return dataProviderResult;
		}
		catch (EndpointNotFoundException)
		{
			throw new ASException(Resources.IBEndpointNotFound);
		}
		catch (Exception ex2)
		{
			throw new ASException(ex2.Message);
		}
	}

	private static b2bUnitedProductRowBase GetGoodFromCloudProduct(UnitedProduct product)
	{
		b2bUnitedProductRowBase b2bUnitedProductRowBase2 = new b2bUnitedProductRowBase
		{
			Barcode = product.Barcode,
			Name = product.Name,
			FullName = product.FullName,
			CPACode = product.Classifier.Trim(),
			IsVatTaxable = product.IsVatTaxable.Value,
			Description = product.Description,
			CPACaption = string.Empty
		};
		CPACodesCacheRow item = ArmSoft.SME.Core.UI.Settings.CoreCacheService.CPACode.GetItem(b2bUnitedProductRowBase2.CPACode);
		b2bUnitedProductRowBase2.CPACaption = ((item == null) ? string.Empty : item.Name);
		return b2bUnitedProductRowBase2;
	}
}
