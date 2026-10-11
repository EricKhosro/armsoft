using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models;

namespace ArmSoft.SME.Core.UI.DataViewsData.Reports;

public abstract class TaxBaseDataProvider : DataProvider<TaxBaseDataProvider.DataRow, TaxBaseDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public Guid ISN { get; set; }

		public string TaxDocInnerType { get; set; }

		public string TaxDocInnerTypeName { get; set; }

		public bool IsCorrectionDoc { get; set; }

		public CorrectionState CorrectionState { get; set; }

		public string TaxSerialNumber { get; set; }

		public string Status { get; set; }

		public string ApprovalState { get; set; }

		public string BehalfOfName { get; set; }

		public string SubStatusName { get; set; }

		public bool FinalUse { get; set; }

		public string AdditionalData { get; set; }

		public string SupplierTin { get; set; }

		public string SupplierName { get; set; }

		public string SupplierAddress { get; set; }

		public string BuyerTin { get; set; }

		public string BuyerName { get; set; }

		public string BuyerBusinessAddress { get; set; }

		public decimal SumWithoutVAT { get; set; }

		public decimal VATSum { get; set; }

		public decimal Sum { get; set; }

		public DateTime? CreationDate { get; set; }

		public DateOnly? SupplyDate { get; set; }

		public DateTime? SubmitDate { get; set; }

		public DateTime? CoSignDate { get; set; }

		public DateTime? CancelledDate { get; set; }

		public DateTime? CancelResponseDate { get; set; }

		public string ReceivedOrSubmitted { get; set; }

		public string SupplierTaxRegimeCaption { get; set; }

		public DateOnly? SupplierTaxRegimeStart { get; set; }

		public bool Matched(object key)
		{
			return (Guid)key == ISN;
		}
	}

	public class Param
	{
		public DateOnly? CreationStartDate { get; set; }

		public DateOnly? CreationEndDate { get; set; }

		public DateOnly? SubmitStartDate { get; set; }

		public DateOnly? SubmitEndDate { get; set; }

		public DateOnly? SupplyStartDate { get; set; }

		public DateOnly? SupplyEndDate { get; set; }

		public string SupplierTin { get; set; } = "";

		public string SupplierName { get; set; } = "";

		public string BuyerTin { get; set; } = "";

		public string BuyerName { get; set; } = "";

		public List<short> TaxDocTypes { get; set; }

		public string TaxSerialNumber { get; set; } = "";

		public bool ShowOutSumsAsNegatives { get; set; }
	}

	public class TaxParameter : Parameter
	{
		public object DefaultValue { get; private set; }

		public string Format { get; private set; }

		public TaxParameter(string name, string caption, SupportedType type, object defaultValue, string format = "")
			: base(name, caption, type)
		{
			DefaultValue = defaultValue;
			Format = format;
		}
	}

	public override string Name
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public abstract string Caption { get; }

	public abstract TaxDocType TaxDocType { get; }

	public abstract Status TaxDocStatus { get; }

	public abstract ReceivedOrSubmittedType ReceivedOrSubmittedType { get; }

	public TaxBaseDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = Caption
		};
		List<Column> list = new List<Column>
		{
			new Column
			{
				Name = "CreationDate",
				Caption = Resources.CreationDate,
				Source = "CreationDate",
				Type = SupportedType.DateTime,
				Format = "dd/MM/yy HH:mm"
			},
			new Column
			{
				Name = "SubmitDate",
				Caption = Resources.SubmissionDate,
				Source = "SubmitDate",
				Type = SupportedType.DateTimeNullable,
				Format = "dd/MM/yy HH:mm"
			},
			new Column
			{
				Name = "SupplyDate",
				Caption = Resources.SupplyDate,
				Source = "SupplyDate",
				Type = SupportedType.DateOnly
			},
			new Column
			{
				Name = "TaxSerialNumber",
				Caption = Resources.TaxSerialNumber,
				Source = "TaxSerialNumber",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "BuyerTin",
				Caption = Resources.BuyerTaxCode,
				Source = "BuyerTin",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "BuyerName",
				Caption = Resources.BuyerName,
				Source = "BuyerName",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "SupplierTin",
				Caption = Resources.SupplierTaxCode,
				Source = "SupplierTin",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "SupplierName",
				Caption = Resources.SupplierName,
				Source = "SupplierName",
				Type = SupportedType.String
			}
		};
		if (TaxDocType == TaxDocType.AccDoc)
		{
			list.Add(new Column
			{
				Name = "SumWithoutVAT",
				Caption = Resources.Value,
				Source = "SumWithoutVAT",
				Type = SupportedType.Decimal,
				FooterSum = true
			});
		}
		else if (TaxDocType == TaxDocType.Invoice)
		{
			list.Add(new Column
			{
				Name = "Sum",
				Caption = Resources.TotalSum,
				Source = "Sum",
				Type = SupportedType.Decimal,
				FooterSum = true
			});
			list.Add(new Column
			{
				Name = "SumWithoutVAT",
				Caption = Resources.TheTurn + " " + Resources.WithoutVAT,
				Source = "SumWithoutVAT",
				Type = SupportedType.Decimal,
				FooterSum = true
			});
			list.Add(new Column
			{
				Name = "VATSum",
				Caption = Resources.OptionsVAT,
				Source = "VATSum",
				Type = SupportedType.Decimal,
				FooterSum = true
			});
		}
		list.Add(new Column
		{
			Name = "TaxDocInnerTypeName",
			Caption = Resources.TypeWord,
			Source = "TaxDocInnerTypeName",
			Type = SupportedType.String
		});
		list.Add(new Column
		{
			Name = "IsCorrectionDoc",
			Caption = Resources.CorrectionInvoice,
			Source = "IsCorrectionDoc",
			Type = SupportedType.Boolean
		});
		list.Add(new Column
		{
			Name = "BehalfOfName",
			Caption = Resources.InvoiceProcedureName,
			Source = "BehalfOfName",
			Type = SupportedType.String
		});
		list.Add(new Column
		{
			Name = "Status",
			Caption = Resources.InvalidationTypeName,
			Source = "Status",
			Type = SupportedType.String
		});
		list.Add(new Column
		{
			Name = "SubStatusName",
			Caption = Resources.MarkingStatus,
			Source = "SubStatusName",
			Type = SupportedType.String
		});
		list.Add(new Column
		{
			Name = "ReceivedOrSubmitted",
			Caption = Resources.EInvoicingReceivedOrSubmitted,
			Source = "ReceivedOrSubmitted",
			Type = SupportedType.String
		});
		list.Add(new Column
		{
			Name = "FinalUse",
			Caption = Resources.FinalUse,
			Source = "FinalUse",
			Type = SupportedType.Boolean
		});
		list.Add(new Column
		{
			Name = "CoSignDate",
			Caption = Resources.SignDate,
			Source = "CoSignDate",
			Type = SupportedType.DateTimeNullable,
			Format = "dd/MM/yy HH:mm"
		});
		list.Add(new Column
		{
			Name = "CancelledDate",
			Caption = Resources.CancelledDate,
			Source = "CancelledDate",
			Type = SupportedType.DateTime,
			Format = "dd/MM/yy HH:mm"
		});
		list.Add(new Column
		{
			Name = "CancelResponseDate",
			Caption = Resources.CanceleApproveDate,
			Source = "CancelResponseDate",
			Type = SupportedType.DateTime,
			Format = "dd/MM/yy HH:mm"
		});
		list.Add(new Column
		{
			Name = "AdditionalData",
			Caption = Resources.AdditionalData,
			Source = "AdditionalData",
			Type = SupportedType.String
		});
		list.Add(new Column
		{
			Name = "SupplierTaxRegimeCaption",
			Caption = Resources.TaxRegime,
			Source = "SupplierTaxRegimeCaption",
			Type = SupportedType.String
		});
		list.Add(new Column
		{
			Name = "SupplierTaxRegimeStart",
			Caption = Resources.TaxRegimeStart,
			Source = "SupplierTaxRegimeStart",
			Type = SupportedType.DateTimeNullable
		});
		schema.Columns = list;
		schema.Parameters = GetParameters();
		return schema;
	}

	public abstract IEnumerable<TaxParameter> GetParameters();

	public override async Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		DataProviderResult<DataRow> result = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		List<TaxDoc> list = ((keys != null) ? ArmSoft.SME.Core.UI.Settings.TaxService.Client.Invoice.GetInvoiceList(keys.Select((object p) => (Guid)p).ToList()) : (await ArmSoft.SME.Core.UI.Settings.TaxService.Client.Invoice.GetListAsync(ReceivedOrSubmittedType, TaxDocStatus, TaxDocType, param.CreationStartDate.ToDateTime(), param.CreationEndDate.ToDateTime(), param.SubmitStartDate.ToDateTime(), param.SubmitEndDate.ToDateTime(), param.SupplierTin, param.SupplierName, param.BuyerTin, param.BuyerName, param.TaxSerialNumber, param.SupplyStartDate.ToDateTime(), param.SupplyEndDate.ToDateTime(), 0, 0, "createdAt", sortAsc: false, CorrectionState.None, ApprovalState.None, showOnlyCurrentUser: false, CancellationState.None, param.TaxDocTypes, SupplierTaxRegime.None, cancellationToken)));
		foreach (TaxDoc item in list)
		{
			string text;
			if (TaxDocType != TaxDocType.Invoice)
			{
				text = ((item.AccDocType != AccDocType.None) ? Convert.ToString((int)item.AccDocType) : "");
			}
			else
			{
				text = ((item.InvoiceType != InvoiceType.None) ? Convert.ToString((int)item.InvoiceType) : "");
			}
			bool flag = Convert.ToBoolean(item.CorrectionState);
			result.Rows.Add(new DataRow
			{
				ISN = item.Id,
				CorrectionState = item.CorrectionState,
				IsCorrectionDoc = flag,
				CreationDate = item.CreatedAt.Value,
				SupplyDate = item.DeliveredAt.ToDateOnly(),
				SubmitDate = item.IssuedAt,
				CoSignDate = item.ApprovedAt,
				TaxSerialNumber = item.SerialNo,
				SupplierTin = item.SupplierTin,
				SupplierName = item.SupplierName,
				SupplierAddress = ((ReceivedOrSubmittedType == ReceivedOrSubmittedType.Received) ? item.DeliveryAddress : item.DestinationAddress),
				BuyerTin = item.BuyerTin,
				BuyerName = item.BuyerName,
				BuyerBusinessAddress = ((ReceivedOrSubmittedType == ReceivedOrSubmittedType.Received) ? item.DestinationAddress : item.DeliveryAddress),
				SumWithoutVAT = (((param.ShowOutSumsAsNegatives & flag) && item.CorrectionType == CorrectionType.Decreasing) ? (-Convert.ToDecimal(item.TotalWithoutVat)) : Convert.ToDecimal(item.TotalWithoutVat)),
				VATSum = (((param.ShowOutSumsAsNegatives & flag) && item.CorrectionType == CorrectionType.Decreasing) ? (-Convert.ToDecimal(item.TotalVatAmount)) : Convert.ToDecimal(item.TotalVatAmount)),
				Sum = (((param.ShowOutSumsAsNegatives & flag) && item.CorrectionType == CorrectionType.Decreasing) ? (-Convert.ToDecimal(item.Total)) : Convert.ToDecimal(item.Total)),
				TaxDocInnerType = text,
				TaxDocInnerTypeName = ((!string.IsNullOrWhiteSpace(text)) ? SmartEnum<TaxAccAndInvoiceDocsTypeSE, string>.FromValue(text).Caption : ""),
				BehalfOfName = item.SortCaption,
				Status = item.StatusCaption,
				SubStatusName = item.SubStatusCaption,
				FinalUse = item.FinalUse,
				ApprovalState = ((item.ApprovalState != ApprovalState.None) ? SmartEnum<TaxInvoiceApprovalStateSE, string>.FromName(Convert.ToString(item.ApprovalState)).Caption : ""),
				ReceivedOrSubmitted = ((item.BuyerTin == ArmSoft.SME.Core.UI.Settings.TaxService.Client.Tin) ? TaxDocReceivedOrSubmittedSE.Received.Caption : TaxDocReceivedOrSubmittedSE.Submitted.Caption),
				CancelledDate = item.CancelledAt,
				CancelResponseDate = item.CancellationResponseAt,
				AdditionalData = item.AdditionalInfo,
				SupplierTaxRegimeCaption = item.SupplierTaxRegimeCaption,
				SupplierTaxRegimeStart = item.SupplierTaxRegimeStart.ToDateOnly()
			});
		}
		return result;
	}
}
