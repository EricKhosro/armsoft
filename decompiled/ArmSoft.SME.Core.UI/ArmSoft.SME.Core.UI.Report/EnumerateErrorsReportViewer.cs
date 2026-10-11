using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Schema;
using ArmSoft.Cloud.Client;
using ArmSoft.ECRConnector;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.OpenBanking.Api.Client.Core;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.TaxService;
using ArmSoft.b2b.V2;
using Serilog;

namespace ArmSoft.SME.Core.UI.Report;

public class EnumerateErrorsReportViewer : AsReport
{
	private int _errCount;

	private readonly string _headerCaption;

	private readonly string _name;

	private bool _isRowAdded;

	private bool _atLeastOneGUID;

	private readonly bool _allowOperation;

	public override bool AllowEdit
	{
		get
		{
			if (_atLeastOneGUID)
			{
				return _allowOperation;
			}
			return false;
		}
	}

	public override bool AllowView => _atLeastOneGUID;

	public override string Name => _name;

	public bool HasErrors => _errCount != 0;

	public EnumerateErrorsReportViewer(string headerCaption, string name)
	{
		ReportPanel.IsSpreadSheetMode = false;
		AllowRefresh = false;
		_headerCaption = headerCaption;
		_name = name;
		_isRowAdded = false;
		_atLeastOneGUID = false;
		_allowOperation = true;
	}

	public override string Caption()
	{
		return _headerCaption;
	}

	public void AddError(string message)
	{
		AddError(message, Guid.Empty);
	}

	public void AddError(string message, Guid isn)
	{
		if (!_isRowAdded)
		{
			Recreate();
			_isRowAdded = true;
		}
		_errCount++;
		List<object> values = new List<object> { _errCount, message };
		if (isn == Guid.Empty)
		{
			ReportPanel.AddTableRow("MainTable", values);
			return;
		}
		ReportPanel.AddTableRow("MainTable", values, isn);
		_atLeastOneGUID = true;
	}

	public void AddError(Exception ex, Guid isn = default(Guid), bool sendToGibraltar = true)
	{
		if (!(ex is InvisibleException) && !(ex is ArmSoft.SME.Common.Exceptions.InvisibleException))
		{
			AddError(ex.Message, isn);
			if (sendToGibraltar && !(ex is ASException) && !(ex is ArmSoft.SME.Client.ApiException { IsRestException: not false }) && !(ex is ArmSoft.TaxService.ApiException) && !(ex is ArmSoft.InternetBank.APIClient.Core.ApiException) && !(ex is ArmSoft.OpenBanking.Api.Client.Core.ApiException) && !(ex is ArmSoft.Cloud.Client.RESTException) && !(ex is B2BException) && !(ex is Utilities.ECRException) && !(ex is EndpointNotFoundException) && !(ex is XmlSchemaValidationException) && !(ex is FaultException))
			{
				Log.Error(ex, "App.Errors", propertyValue: true);
			}
		}
	}

	public override void Edit(object rowInfo)
	{
		if (rowInfo != null)
		{
			Settings.DocumentService.ShowWindow((Guid)rowInfo, WindowViewMode.Edit);
		}
	}

	public override void View(object rowInfo)
	{
		if (rowInfo != null)
		{
			Settings.DocumentService.ShowWindow((Guid)rowInfo, WindowViewMode.View);
		}
	}

	public void Recreate()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		short num = 40;
		short num2 = 640;
		list.Add(new AsRepColumn(num));
		list.Add(new AsRepColumn(num2));
		_errCount = 0;
		ReportPanel.ClearAll();
		ReportPanel.AddTableDefinition("MainTable", list);
		ReportPanel.EvenOddStyle = true;
	}
}
