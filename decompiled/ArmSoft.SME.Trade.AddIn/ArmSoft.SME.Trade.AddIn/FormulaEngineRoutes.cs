using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using ArmSoft.SME.Core.AddIn;
using ArmSoft.SME.Trade.AddIn.Client.Models;

namespace ArmSoft.SME.Trade.AddIn;

public class FormulaEngineRoutes
{
	private readonly TradeAddInClient client;

	internal FormulaEngineRoutes(TradeAddInClient client)
	{
		this.client = client;
	}

	public void Clear()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/Clear");
		client.Execute<bool>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyName()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyName");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyAddress()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyAddress");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyBussAddress()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyBussAddress");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyStateRegN()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyStateRegN");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyTIN()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyTIN");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyManagerName()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyManagerName");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyManagerPost()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyManagerPost");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyGenAccName()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyGenAccName");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyGenAccPost()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyGenAccPost");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetCompanyPhone()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetCompanyPhone");
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetPartNameByCode(string partnerCode, bool isFullName = false)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("partnerCode", partnerCode);
		urlQuery.Add("isFullName", isFullName);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetPartNameByCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetPartCodeByTaxCode(string taxCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("taxCode", taxCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetPartCodeByTaxCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetPartCodeByPassport(string passport)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("passport", passport);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetPartCodeByPassport", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTNameByCode(string mtCode, bool isFullName = false)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		urlQuery.Add("isFullName", isFullName);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTNameByCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTCodeByName(string mtName, bool isFullName = false)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtName", mtName);
		urlQuery.Add("isFullName", isFullName);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTCodeByName", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTCPACodeByMTCode(string mtCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTCPACodeByMTCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTDescriptionByMTCode(string mtCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTDescriptionByMTCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTUnitCodeByMTBarCode(string barCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("barCode", barCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTUnitCodeByMTBarCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTCodeByMTExternalCode(string externalCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("externalCode", externalCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTCodeByMTExternalCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTGroupCodeByMTCode(string mtCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTGroupCodeByMTCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTGroupNameByMTCode(string mtCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTGroupNameByMTCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public decimal TGetMTCoeffByMTCodeAndUnit(string mtCode, string unit = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		urlQuery.Add("unit", unit);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTCoeffByMTCodeAndUnit", urlQuery);
		return client.Execute<decimal>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetQntUnitCodeByName(string unitName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("unitName", unitName);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetQntUnitCodeByName", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetDefContractCodeByPartCode(string partCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("partCode", partCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetDefContractCodeByPartCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public decimal TMTRemSum(Enums.SummaTypes summaType, string materials = "", DateTime? remDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.RemDate = remDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTRemSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupRemSum(Enums.SummaTypes summaType, string mtGroups, DateTime? remDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.MTGroup = mtGroups;
		generalModel.RemDate = remDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupRemSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTRemRetailSum(string materials = "", DateTime? remDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.RemDate = remDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTRemRetailSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupRemRetailSum(string group = "", DateTime? remDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroup = group;
		generalModel.RemDate = remDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupRemRetailSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTRemCount(string materials = "", DateTime? remDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.RemDate = remDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTRemCount");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupRemCount(string group = "", DateTime? remDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroup = group;
		generalModel.RemDate = remDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupRemCount");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTTurnInSum(Enums.SummaTypes summaType, string materials, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTTurnInSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupTurnInSum(Enums.SummaTypes summaType, string mtGroup, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.MTGroup = mtGroup;
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupTurnInSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTTurnInRetailSum(string materials, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTTurnInRetailSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupTurnInRetailSum(string mtGroup, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroup = mtGroup;
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupTurnInRetailSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTTurnInCount(string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTTurnInCount");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupTurnInCount(string mtGroup, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroup = mtGroup;
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupTurnInCount");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTTurnOutSum(Enums.SummaTypes summaType, string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTTurnOutSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupTurnOutSum(Enums.SummaTypes summaType, string mtGroup, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.MTGroup = mtGroup;
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupTurnOutSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTTurnOutRetailSum(string materials, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTTurnOutRetailSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupTurnOutRetailSum(string mtGroup, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroup = mtGroup;
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupTurnOutRetailSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTTurnOutCount(string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTTurnOutCount");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupTurnOutCount(string mtGroup, DateTime? startDate = null, DateTime? endDate = null, string storages = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroup = mtGroup;
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupTurnOutCount");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTMoveSum(Enums.SummaTypes summaType, string storageIn, string storageOut, string materials = "", DateTime? startDate = null, DateTime? endDate = null)
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.StorageIn = storageIn;
		generalModel.StorageOut = storageOut;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTMoveSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupMoveSum(Enums.SummaTypes summaType, string mtGroups, string storageIn, string storageOut, DateTime? startDate = null, DateTime? endDate = null)
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.MTGroups = mtGroups?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.StorageIn = storageIn;
		generalModel.StorageOut = storageOut;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupMoveSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTMoveCount(string storageIn, string storageOut, string materials = "", DateTime? startDate = null, DateTime? endDate = null)
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.StorageIn = storageIn;
		generalModel.StorageOut = storageOut;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTMoveCount");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupMoveCount(string mtGroups, string storageIn, string storageOut, DateTime? startDate = null, DateTime? endDate = null)
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroups = mtGroups?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.StorageIn = storageIn;
		generalModel.StorageOut = storageOut;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupMoveCount");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTSaleQty(string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTSaleQty");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTSaleQtyByDescr(string description, string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		generalModel.Description = description;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTSaleQtyByDescr");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupSaleQty(string mtGroups, DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroups = mtGroups.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupSaleQty");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTSaleCostSum(Enums.SummaTypes summaType, string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTSaleCostSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupSaleCostSum(Enums.SummaTypes summaType, string mtGroups, DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.MTGroups = mtGroups.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupSaleCostSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTRetailSum(string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTRetailSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupRetailSum(string mtGroups, DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.MTGroups = mtGroups?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupRetailSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TLastSaleSum(Enums.SummaTypes summaType, string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TLastSaleSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTSaleSum(Enums.SummaTypes summaType, string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "", string discount = "0")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		generalModel.Discount = discount;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTSaleSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTSaleSumByDescr(Enums.SummaTypes summaType, string description, string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "", string discount = "0")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		generalModel.Discount = discount;
		generalModel.Description = description;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTSaleSumByDescr");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupSaleSum(Enums.SummaTypes summaType, string mtGroups, DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "", string discount = "0")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.MTGroups = mtGroups?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		generalModel.Discount = discount;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupSaleSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TPrGroupSaleSum(Enums.SummaTypes summaType, string mtGroups, DateTime? startDate = null, DateTime? endDate = null, string storages = "", string materials = "", string opTypes = "", string discount = "0")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.MTGroups = mtGroups?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		generalModel.Discount = discount;
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TPrGroupSaleSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTSaleDiscountSum(Enums.SummaTypes summaType, string materials = "", DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.Materials = materials?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTSaleDiscountSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTGroupSaleDiscountSum(Enums.SummaTypes summaType, string mtGroups, DateTime? startDate = null, DateTime? endDate = null, string storages = "", string partners = "", string opTypes = "")
	{
		GeneralModel generalModel = new GeneralModel();
		generalModel.SummaType = summaType;
		generalModel.MTGroups = mtGroups?.Split(',').ToList();
		generalModel.StartDate = startDate;
		generalModel.EndDate = endDate;
		generalModel.Storages = storages?.Split(',').ToList();
		generalModel.Partners = partners?.Split(',').ToList();
		generalModel.OpTypes = opTypes?.Split(',').ToList();
		GeneralModel requestObject = generalModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTGroupSaleDiscountSum");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TMTPriceByPriceType(string material, string priceType, string unit = "", DateTime? date = null)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("material", material);
		urlQuery.Add("priceType", priceType);
		urlQuery.Add("unit", unit);
		urlQuery.Add("date", date);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TMTPriceByPriceType", urlQuery);
		return client.Execute<decimal>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTCodeByBarCode(string barCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("barCode", barCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTCodeByBarCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTExternalCodeByMTCode(string code)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("code", code);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTExternalCodeByMTCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTUnitByCode(string mtCode, bool isAltUnit = false)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		urlQuery.Add("isAltUnit", isAltUnit);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTUnitByCode", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public string TGetMTBarCodeByCodeAndUnit(string mtCode, string unit = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		urlQuery.Add("unit", unit);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTBarCodeByCodeAndUnit", urlQuery);
		return client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public decimal TGetMTDiscountByCode(string mtCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTDiscountByCode", urlQuery);
		return client.Execute<decimal>(HttpMethod.Get, stringBuilder.ToString());
	}

	public DateTime? TGetMTLastStockTakingDate(string mtCode, string storage = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtCode", mtCode);
		urlQuery.Add("storage", storage);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TGetMTLastStockTakingDate", urlQuery);
		return client.Execute<DateTime?>(HttpMethod.Get, stringBuilder.ToString());
	}

	public decimal TRemCash(DateTime? remDate = null, string cash = "", string cur = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("remDate", remDate);
		urlQuery.Add("cash", cash);
		urlQuery.Add("cur", cur);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TRemCash", urlQuery);
		return client.Execute<decimal>(HttpMethod.Get, stringBuilder.ToString());
	}

	public decimal TCurRemCash(string cur, DateTime? remDate = null, string cash = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("remDate", remDate);
		urlQuery.Add("cash", cash);
		urlQuery.Add("cur", cur);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurRemCash", urlQuery);
		return client.Execute<decimal>(HttpMethod.Get, stringBuilder.ToString());
	}

	public decimal TTurnCashIn(DateTime? startDate = null, DateTime? endDate = null, string cash = "", string prGroup = "", string partners = "", string cur = "", string opTypes = "")
	{
		CashModel cashModel = new CashModel();
		cashModel.StartDate = startDate;
		cashModel.EndDate = endDate;
		cashModel.Cash = cash;
		cashModel.Cur = cur;
		cashModel.OpTypes = opTypes;
		cashModel.Partners = partners?.Split(',').ToList();
		cashModel.PrGroup = prGroup;
		CashModel requestObject = cashModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnCashIn");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnCashOut(DateTime? startDate = null, DateTime? endDate = null, string cash = "", string prGroup = "", string partners = "", string cur = "", string opTypes = "")
	{
		CashModel cashModel = new CashModel();
		cashModel.StartDate = startDate;
		cashModel.EndDate = endDate;
		cashModel.Cash = cash;
		cashModel.Cur = cur;
		cashModel.OpTypes = opTypes;
		cashModel.Partners = partners?.Split(',').ToList();
		cashModel.PrGroup = prGroup;
		CashModel requestObject = cashModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnCashOut");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnCashin(string cur, DateTime? startDate = null, DateTime? endDate = null, string cash = "", string prGroup = "", string partners = "")
	{
		CashModel cashModel = new CashModel();
		cashModel.StartDate = startDate;
		cashModel.EndDate = endDate;
		cashModel.Cash = cash;
		cashModel.Cur = cur;
		cashModel.Partners = partners?.Split(',').ToList();
		cashModel.PrGroup = prGroup;
		CashModel requestObject = cashModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnCashin");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnCashOut(string cur, DateTime? startDate = null, DateTime? endDate = null, string cash = "", string prGroup = "", string partners = "")
	{
		CashModel cashModel = new CashModel();
		cashModel.StartDate = startDate;
		cashModel.EndDate = endDate;
		cashModel.Cash = cash;
		cashModel.Cur = cur;
		cashModel.Partners = partners?.Split(',').ToList();
		cashModel.PrGroup = prGroup;
		CashModel requestObject = cashModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnCashOut");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TRemBank(DateTime? remDate = null, string bank = "", string cur = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("remDate", remDate);
		urlQuery.Add("bank", bank);
		urlQuery.Add("cur", cur);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TRemBank", urlQuery);
		return client.Execute<decimal>(HttpMethod.Get, stringBuilder.ToString());
	}

	public decimal TCurRemBank(string cur, DateTime? remDate = null, string bank = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("remDate", remDate);
		urlQuery.Add("bank", bank);
		urlQuery.Add("cur", cur);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurRemBank", urlQuery);
		return client.Execute<decimal>(HttpMethod.Get, stringBuilder.ToString());
	}

	public decimal TTurnBankIn(DateTime? startDate = null, DateTime? endDate = null, string bank = "", string prGroup = "", string partners = "", string cur = "", string opTypes = "")
	{
		CashModel cashModel = new CashModel();
		cashModel.StartDate = startDate;
		cashModel.EndDate = endDate;
		cashModel.Bank = bank;
		cashModel.Cur = cur;
		cashModel.Partners = partners?.Split(',').ToList();
		cashModel.PrGroup = prGroup;
		cashModel.OpTypes = opTypes;
		CashModel requestObject = cashModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnBankIn");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnBankOut(DateTime? startDate = null, DateTime? endDate = null, string bank = "", string prGroup = "", string partners = "", string cur = "", string opTypes = "")
	{
		CashModel cashModel = new CashModel();
		cashModel.StartDate = startDate;
		cashModel.EndDate = endDate;
		cashModel.Bank = bank;
		cashModel.Cur = cur;
		cashModel.Partners = partners?.Split(',').ToList();
		cashModel.PrGroup = prGroup;
		cashModel.OpTypes = opTypes;
		CashModel requestObject = cashModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnBankOut");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnBankIn(string cur, DateTime? startDate = null, DateTime? endDate = null, string bank = "", string prGroup = "", string partners = "")
	{
		CashModel cashModel = new CashModel();
		cashModel.StartDate = startDate;
		cashModel.EndDate = endDate;
		cashModel.Bank = bank;
		cashModel.Cur = cur;
		cashModel.Partners = partners?.Split(',').ToList();
		cashModel.PrGroup = prGroup;
		CashModel requestObject = cashModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnBankIn");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnBankOut(string cur, DateTime? startDate = null, DateTime? endDate = null, string bank = "", string prGroup = "", string partners = "")
	{
		CashModel cashModel = new CashModel();
		cashModel.StartDate = startDate;
		cashModel.EndDate = endDate;
		cashModel.Bank = bank;
		cashModel.Cur = cur;
		cashModel.Partners = partners?.Split(',').ToList();
		cashModel.PrGroup = prGroup;
		CashModel requestObject = cashModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnBankOut");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TRemPr(DateTime? remDate = null, string partners = "", string cur = "", short supplier = -1, short customer = -1)
	{
		RemModel remModel = new RemModel();
		remModel.RemDate = remDate;
		remModel.Cur = cur;
		remModel.Partners = partners?.Split(',').ToList();
		remModel.Supplier = supplier;
		remModel.Customer = customer;
		RemModel requestObject = remModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TRemPr");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurRemPr(string cur, DateTime? remDate = null, string partners = "", short supplier = -1, short customer = -1)
	{
		RemModel remModel = new RemModel();
		remModel.RemDate = remDate;
		remModel.Cur = cur;
		remModel.Partners = partners?.Split(',').ToList();
		remModel.Supplier = supplier;
		remModel.Customer = customer;
		RemModel requestObject = remModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurRemPr");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TRemPrByGroup(string prGroup, DateTime? remDate = null, string cur = "", short supplier = -1, short customer = -1)
	{
		RemModel requestObject = new RemModel
		{
			RemDate = remDate,
			Cur = cur,
			Supplier = supplier,
			Customer = customer,
			PrGroup = prGroup
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TRemPrByGroup");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurRemPrByGroup(string cur, string prGroup, DateTime? remDate = null, short supplier = -1, short customer = -1)
	{
		RemModel requestObject = new RemModel
		{
			RemDate = remDate,
			Cur = cur,
			Supplier = supplier,
			Customer = customer,
			PrGroup = prGroup
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurRemPrByGroup");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TRemPrByContract(string partner, string contract, DateTime? remDate = null)
	{
		RemModel requestObject = new RemModel
		{
			RemDate = remDate,
			Contract = contract,
			Partner = partner
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TRemPrByContract");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurRemPrByContract(string partner, string contract, DateTime? remDate = null)
	{
		RemModel requestObject = new RemModel
		{
			RemDate = remDate,
			Contract = contract,
			Partner = partner
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurRemPrByContract");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrIn(DateTime? startDate = null, DateTime? endDate = null, string partners = "", string cur = "", short supplier = -1, short customer = -1)
	{
		TurnModel turnModel = new TurnModel();
		turnModel.StartDate = startDate;
		turnModel.EndDate = endDate;
		turnModel.Partners = partners?.Split(',').ToList();
		turnModel.Cur = cur;
		turnModel.Supplier = supplier;
		turnModel.Customer = customer;
		TurnModel requestObject = turnModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrIn");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrInByGroup(string prGroup, DateTime? startDate = null, DateTime? endDate = null, string cur = "", short supplier = -1, short customer = -1)
	{
		TurnModel requestObject = new TurnModel
		{
			StartDate = startDate,
			EndDate = endDate,
			Cur = cur,
			Supplier = supplier,
			Customer = customer,
			PrGroup = prGroup
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrInByGroup");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrOut(DateTime? startDate = null, DateTime? endDate = null, string partners = "", string cur = "", short supplier = -1, short customer = -1)
	{
		TurnModel turnModel = new TurnModel();
		turnModel.StartDate = startDate;
		turnModel.EndDate = endDate;
		turnModel.Partners = partners?.Split(',').ToList();
		turnModel.Cur = cur;
		turnModel.Supplier = supplier;
		turnModel.Customer = customer;
		TurnModel requestObject = turnModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrOut");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrOutByGroup(string prGroup, DateTime? startDate = null, DateTime? endDate = null, string cur = "", short supplier = -1, short customer = -1)
	{
		TurnModel requestObject = new TurnModel
		{
			StartDate = startDate,
			EndDate = endDate,
			Cur = cur,
			Supplier = supplier,
			Customer = customer,
			PrGroup = prGroup
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrOutByGroup");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnPrIn(string cur, DateTime? startDate = null, DateTime? endDate = null, string partners = "", short supplier = -1, short customer = -1)
	{
		TurnModel turnModel = new TurnModel();
		turnModel.StartDate = startDate;
		turnModel.EndDate = endDate;
		turnModel.Partners = partners?.Split(',').ToList();
		turnModel.Cur = cur;
		turnModel.Supplier = supplier;
		turnModel.Customer = customer;
		TurnModel requestObject = turnModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnPrIn");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrInByGroup(string cur, string prGroup, DateTime? startDate = null, DateTime? endDate = null, short supplier = -1, short customer = -1)
	{
		TurnModel requestObject = new TurnModel
		{
			StartDate = startDate,
			EndDate = endDate,
			Cur = cur,
			Supplier = supplier,
			Customer = customer,
			PrGroup = prGroup
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrInByGroup");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnPrOut(string cur, DateTime? startDate = null, DateTime? endDate = null, string partners = "", short supplier = -1, short customer = -1)
	{
		TurnModel turnModel = new TurnModel();
		turnModel.StartDate = startDate;
		turnModel.EndDate = endDate;
		turnModel.Partners = partners?.Split(',').ToList();
		turnModel.Cur = cur;
		turnModel.Supplier = supplier;
		turnModel.Customer = customer;
		TurnModel requestObject = turnModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnPrOut");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnPrOutByGroup(string cur, string prGroup, DateTime? startDate = null, DateTime? endDate = null, short supplier = -1, short customer = -1)
	{
		TurnModel requestObject = new TurnModel
		{
			StartDate = startDate,
			EndDate = endDate,
			Cur = cur,
			Supplier = supplier,
			Customer = customer,
			PrGroup = prGroup
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnPrOutByGroup");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrInByContract(string partner, string contract, DateTime? startDate = null, DateTime? endDate = null)
	{
		TurnModel requestObject = new TurnModel
		{
			StartDate = startDate,
			EndDate = endDate,
			Partner = partner,
			Contract = contract
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrInByContract");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnPrInByContract(string partner, string contract, DateTime? startDate = null, DateTime? endDate = null)
	{
		TurnModel requestObject = new TurnModel
		{
			StartDate = startDate,
			EndDate = endDate,
			Partner = partner,
			Contract = contract
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnPrInByContract");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrOutByContract(string partner, string contract, DateTime? startDate = null, DateTime? endDate = null)
	{
		TurnModel requestObject = new TurnModel
		{
			StartDate = startDate,
			EndDate = endDate,
			Partner = partner,
			Contract = contract
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrOutByContract");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnPrOutByContract(string partner, string contract, DateTime? startDate = null, DateTime? endDate = null)
	{
		TurnModel requestObject = new TurnModel
		{
			StartDate = startDate,
			EndDate = endDate,
			Partner = partner,
			Contract = contract
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnPrOutByContract");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrOper(short opType, DateTime? startDate = null, DateTime? endDate = null, string partners = "", string cur = "", short supplier = -1, short customer = -1)
	{
		TurnModel turnModel = new TurnModel();
		turnModel.OpType = opType;
		turnModel.StartDate = startDate;
		turnModel.EndDate = endDate;
		turnModel.Partners = partners?.Split(',').ToList();
		turnModel.Cur = cur;
		turnModel.Supplier = supplier;
		turnModel.Customer = customer;
		TurnModel requestObject = turnModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrOper");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrOperByGroup(short opType, string prGroup, DateTime? startDate = null, DateTime? endDate = null, string cur = "", short supplier = -1, short customer = -1)
	{
		TurnModel requestObject = new TurnModel
		{
			OpType = opType,
			StartDate = startDate,
			EndDate = endDate,
			Cur = cur,
			Supplier = supplier,
			Customer = customer,
			PrGroup = prGroup
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrOperByGroup");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnPrOper(short opType, DateTime? startDate = null, DateTime? endDate = null, string partners = "", string cur = "", short supplier = -1, short customer = -1)
	{
		TurnModel turnModel = new TurnModel();
		turnModel.OpType = opType;
		turnModel.StartDate = startDate;
		turnModel.EndDate = endDate;
		turnModel.Partners = partners?.Split(',').ToList();
		turnModel.Cur = cur;
		turnModel.Supplier = supplier;
		turnModel.Customer = customer;
		TurnModel requestObject = turnModel;
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnPrOper");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnPrOperByGroup(short opType, string prGroup, DateTime? startDate = null, DateTime? endDate = null, string cur = "", short supplier = -1, short customer = -1)
	{
		TurnModel requestObject = new TurnModel
		{
			OpType = opType,
			StartDate = startDate,
			EndDate = endDate,
			Cur = cur,
			Supplier = supplier,
			Customer = customer,
			PrGroup = prGroup
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnPrOperByGroup");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TTurnPrOperByContract(short opType, string partner, string contract, DateTime? startDate = null, DateTime? endDate = null)
	{
		TurnModel requestObject = new TurnModel
		{
			OpType = opType,
			StartDate = startDate,
			EndDate = endDate,
			Partner = partner,
			Contract = contract
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TTurnPrOperByContract");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public decimal TCurTurnPrOperByContract(short opType, string partner, string contract, DateTime? startDate = null, DateTime? endDate = null)
	{
		TurnModel requestObject = new TurnModel
		{
			OpType = opType,
			StartDate = startDate,
			EndDate = endDate,
			Partner = partner,
			Contract = contract
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/FormulaEngine/TCurTurnPrOperByContract");
		return client.Execute<decimal>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}
}
