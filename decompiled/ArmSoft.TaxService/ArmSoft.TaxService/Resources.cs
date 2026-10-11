using System.Collections.Generic;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService;

public static class Resources
{
	public static LanguageType Language;

	public static Dictionary<ResourceKey, string> ResourceAM;

	public static Dictionary<ResourceKey, string> ResourceEN;

	static Resources()
	{
		Language = LanguageType.English;
		ResourceAM = new Dictionary<ResourceKey, string>();
		ResourceEN = new Dictionary<ResourceKey, string>();
		FillResourceEN();
		FillResourceAM();
	}

	public static string Get(ResourceKey key)
	{
		return Get(key, Language);
	}

	public static string Get(ResourceKey key, LanguageType language)
	{
		string value = "";
		if (language == LanguageType.Armenian)
		{
			ResourceAM.TryGetValue(key, out value);
		}
		if (language == LanguageType.English || string.IsNullOrWhiteSpace(value))
		{
			ResourceEN.TryGetValue(key, out value);
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			value = key.ToString();
		}
		return value;
	}

	private static void FillResourceEN()
	{
		ResourceEN.Add(ResourceKey.EInvoicingError, "e-invoicing error: ");
		ResourceEN.Add(ResourceKey.DocAlreadyExistWithId, "The document already exist in e-invoicing system                                                                                                           \r\nSystem number - {0}");
		ResourceEN.Add(ResourceKey.OtherTypeDocAlreadyExist, "The other type document with current system number already exist in e-invoicing system \r\nDelivery date - {0}\r\nType - {1}\r\nSystem number - {2}");
		ResourceEN.Add(ResourceKey.DocAlreadyExistWithDeliveryDate, "The document already exist in e-invoicing system\r\nDelivery date - {0}                                                                                                    \r\nSystem number - {1}");
		ResourceEN.Add(ResourceKey.DocAlreadyExistWithDeliveryDateBuyer, "The document already exist in e-invoicing system\r\nDelivery date - {0}\r\nBuyer - {1}                                                                                                                      \r\nSystem number - {2}");
		ResourceEN.Add(ResourceKey.DocNotExistWithId, "The document with \"{0}\" system number doesn't exist in e-invoicing system");
		ResourceEN.Add(ResourceKey.DocNotExistWithSerialNumber, "The document with \"{0}\" serial number doesn't exist in e-invoicing system");
		ResourceEN.Add(ResourceKey.Special, "Special");
		ResourceEN.Add(ResourceKey.TaxPayer, "On behalf of the taxpayer");
		ResourceEN.Add(ResourceKey.Supplier, "On behalf of the supplier");
		ResourceEN.Add(ResourceKey.Principal, "On behalf of the principal");
		ResourceEN.Add(ResourceKey.JointCaseMng, "On behalf of the joint business");
		ResourceEN.Add(ResourceKey.VatReturn, "VAT refund");
		ResourceEN.Add(ResourceKey.Approved, "Approved");
		ResourceEN.Add(ResourceKey.PendingAppSt, "Pending");
		ResourceEN.Add(ResourceKey.AccDocGoods, "Accounting document of products and goods supply");
		ResourceEN.Add(ResourceKey.AccDocServices, "Accounting document of services' provision and works' performance");
		ResourceEN.Add(ResourceKey.AccDocTransportation, "Accounting document of product transportation");
		ResourceEN.Add(ResourceKey.AccDocExportEEU, "Accounting document of EEU export");
		ResourceEN.Add(ResourceKey.AccDocTraceableG, "Accounting document of traceable product transportation");
		ResourceEN.Add(ResourceKey.InvoiceGoods, "Tax account of products and goods supply");
		ResourceEN.Add(ResourceKey.InvoiceServices, "Tax account of services' provision and works' performance");
		ResourceEN.Add(ResourceKey.InvoiceExcise, "Tax account of taxable products by excise tax");
		ResourceEN.Add(ResourceKey.InvoiceVatReturn, "Vat refund tax account");
		ResourceEN.Add(ResourceKey.InvoiceLeasing, "Act of acceptance of the subject of leasing");
		ResourceEN.Add(ResourceKey.Corrected, "Corrected");
		ResourceEN.Add(ResourceKey.Corrector, "Corrector");
		ResourceEN.Add(ResourceKey.Correcting, "Correcting");
		ResourceEN.Add(ResourceKey.Accepted, "Accepted");
		ResourceEN.Add(ResourceKey.Declined, "Declined");
		ResourceEN.Add(ResourceKey.PendingCnclSt, "Pending");
		ResourceEN.Add(ResourceKey.Increasing, "Increasing");
		ResourceEN.Add(ResourceKey.Decreasing, "Decreasing");
		ResourceEN.Add(ResourceKey.InvoiceIsNotEditable, "Invoice is not editable․ Status - {0}");
		ResourceEN.Add(ResourceKey.Issued, "Issued");
		ResourceEN.Add(ResourceKey.Draft, "Draft");
		ResourceEN.Add(ResourceKey.Invalid, "Invalid");
		ResourceEN.Add(ResourceKey.WaitingInvalidationAcceptance, "Waiting invalidation acceptance");
		ResourceEN.Add(ResourceKey.Cancelled, "Cancelled");
		ResourceEN.Add(ResourceKey.Deleted, "Deleted");
		ResourceEN.Add(ResourceKey.PhoneOperatorCannotBeEmpty, "Phone operator cannot be empty");
		ResourceEN.Add(ResourceKey.PhoneNumberCannotBeEmpty, "Phone number cannot be empty");
		ResourceEN.Add(ResourceKey.IncorrectDirectoryPath, "\"{0}\" path is incorrect or is not accessible");
		ResourceEN.Add(ResourceKey.FileDoesNotExist, "\"{0}\" file does not exist");
		ResourceEN.Add(ResourceKey.FileAlreadyExists, "\"{0}\" file already exists");
		ResourceEN.Add(ResourceKey.RepeatedRowsInListNeedToDoFromEInvoicing, "There are repeated rows in list. You should do correction from e-invoicing.");
		ResourceEN.Add(ResourceKey.ProductAbsentInEInvoicing, "There is no product in e-invoicing.\r\nProduct - {0}\r\nMeasure unit - {1}\r\nPrice - {2}");
		ResourceEN.Add(ResourceKey.HashCalculating, "Preparation for signing");
		ResourceEN.Add(ResourceKey.SignWaiting, "Subject to sign");
		ResourceEN.Add(ResourceKey.Failed, "Failed");
		ResourceEN.Add(ResourceKey.SignFailed, "Sign failed");
		ResourceEN.Add(ResourceKey.SignProcessing, "Sign processing");
		ResourceEN.Add(ResourceKey.Signed, "Signed");
		ResourceEN.Add(ResourceKey.SignStatusChanged, "There is a change in signing status, please refresh the page.");
		ResourceEN.Add(ResourceKey.EntityNotFound, "Invoice or request package is not found in e-invoicing.");
		ResourceEN.Add(ResourceKey.SpecialTaxSystem, "Special taxation system");
		ResourceEN.Add(ResourceKey.VatExempt, "VAT exempt");
		ResourceEN.Add(ResourceKey.VatZeroRate, "Taxable at 0% VAT rate");
		ResourceEN.Add(ResourceKey.Export, "Export");
		ResourceEN.Add(ResourceKey.ExportEAEUAir, "Export by EEA air transport");
		ResourceEN.Add(ResourceKey.ExcludedFromVAT, "Excluded from VAT");
		ResourceEN.Add(ResourceKey.VatNonPayers, "VAT non-payers");
		ResourceEN.Add(ResourceKey.VatOther, "Other");
		ResourceEN.Add(ResourceKey.InvoiceFormatIsIncorrect, "The invoice document doesn't comply with the required format");
		ResourceEN.Add(ResourceKey.AccDocFormatIsIncorrect, "The accounting document doesn't comply with the required format");
		ResourceEN.Add(ResourceKey.WaitTimeoutExpired, "The waiting time has expired");
		ResourceEN.Add(ResourceKey.NewRegistered, "New registered");
		ResourceEN.Add(ResourceKey.Active, "Active");
		ResourceEN.Add(ResourceKey.Bankrupt, "Bankrupt");
		ResourceEN.Add(ResourceKey.Dissolved, "Dissolved");
		ResourceEN.Add(ResourceKey.Died, "Died");
		ResourceEN.Add(ResourceKey.IncorrectlyProvided, "Incorrectly provided");
		ResourceEN.Add(ResourceKey.DissolvedMerged, "Dissolved merged");
		ResourceEN.Add(ResourceKey.DissolvedUnited, "Dissolved united");
		ResourceEN.Add(ResourceKey.TimedSuspended, "Timed suspended");
		ResourceEN.Add(ResourceKey.DissolvedProcess, "Dissolved process");
		ResourceEN.Add(ResourceKey.ExtractedFromAccounting, "Extracted from accounting");
		ResourceEN.Add(ResourceKey.InRiskOfBankruptcy, "In risk of bankruptcy");
		ResourceEN.Add(ResourceKey.Unknown, "Unknown");
		ResourceEN.Add(ResourceKey.WrongClassifier, "The \"{1}\" classifier code for product named \"{0}\" is wrong");
		ResourceEN.Add(ResourceKey.ProductParentIdAbsentInEInvoicing, "There is no parent id for the product named \"{0}\" in the base invoice.");
		ResourceEN.Add(ResourceKey.NeedToFillDateTime, "Please specify both a start and end date for at least one of the date fields");
		ResourceEN.Add(ResourceKey.NeedToFillDateTimeOrSerialNumber, "Please specify serial number or both a start and end date for at least one of the date fields");
		ResourceEN.Add(ResourceKey.EInvoicingLoginError, "\"{0}\" login is possible after some time");
		ResourceEN.Add(ResourceKey.FieldValueCannotExceedLength, "The value of the \"{0}\" field cannot exceed {1} characters");
		ResourceEN.Add(ResourceKey.PlaceFromWhereGoodsAreSupplied, "Place from which the goods are supplied (shipped)");
		ResourceEN.Add(ResourceKey.DestinationAddressOfGoods, "Destination (address) of the goods to be supplied (delivered)");
		ResourceEN.Add(ResourceKey.AdditionalInfoPersonSupplyingGoods, "Additional information about the person supplying (delivering) the goods");
		ResourceEN.Add(ResourceKey.AdditionalInfoPersonPurchasingGoods, "Additional information about the person purchasing the goods");
		ResourceEN.Add(ResourceKey.AdditionalInfoPersonProvidingServices, "Additional information of the person providing services (performing works)");
		ResourceEN.Add(ResourceKey.AdditionalInfoPersonReceivingServices, "Additional information the person receiving services (works)");
		ResourceEN.Add(ResourceKey.AdditionalInfo, "Additional information");
		ResourceEN.Add(ResourceKey.LessorAdditionalInfo, "Lessor additional information");
		ResourceEN.Add(ResourceKey.LesseeAdditionalInfo, "Lessee additional information");
		ResourceEN.Add(ResourceKey.LessorActivityCarriedOutPlaceAddress, "Lessor address of the place where the activity is actually carried out");
		ResourceEN.Add(ResourceKey.LessorDeliveryHandoverLeasingObjectPlaceAddress, "Lessor address of the place of delivery or handover of the leasing object");
		ResourceEN.Add(ResourceKey.LesseeActivityCarriedOutPlaceAddress, "Lessee address of the place where the activity is actually carried out");
		ResourceEN.Add(ResourceKey.LesseeDestinationLeasingObjectPlaceAddress, "Lessee address of the place of destination of the leasing object");
		ResourceEN.Add(ResourceKey.Pending, "Pending");
		ResourceEN.Add(ResourceKey.CheckedInProgress, "Verified, during Owner change");
		ResourceEN.Add(ResourceKey.CheckedChanged, "Verified, Owner changed");
		ResourceEN.Add(ResourceKey.CheckedNotChanged, "Verified, Owner not changed");
		ResourceEN.Add(ResourceKey.CheckedPartiallyChanged, "Verified, Owner partially changed");
		ResourceEN.Add(ResourceKey.UncheckedInProgress, "Unverified, Owner changing in progress");
		ResourceEN.Add(ResourceKey.UncheckedChanged, "Unverified, Owner changed");
		ResourceEN.Add(ResourceKey.UncheckedNotChanged, "Unverified, Owner not changed");
		ResourceEN.Add(ResourceKey.UncheckedPartiallyChanged, "Unverified, Owner partially changed");
		ResourceEN.Add(ResourceKey.UncheckedAutoChanged, "Unverified, Automatic owner changed");
		ResourceEN.Add(ResourceKey.UncheckedAutoNotChanged, "Unverified, Automatic owner not changed");
		ResourceEN.Add(ResourceKey.UncheckedAutoPartiallyChanged, "Unverified, Automatic partial owner changed");
		ResourceEN.Add(ResourceKey.CheckedRevInProgress, "Verified, Owner reverting in progress");
		ResourceEN.Add(ResourceKey.CheckedRevChanged, "Verified, Owner reverted");
		ResourceEN.Add(ResourceKey.CheckedRevNotChanged, "Verified, Owner not reverted");
		ResourceEN.Add(ResourceKey.CheckedRevPartiallyChanged, "Verified, Owner partially reverted");
		ResourceEN.Add(ResourceKey.UncheckedRevInProgress, "Unverified, Owner reverting in progress");
		ResourceEN.Add(ResourceKey.UncheckedRevChanged, "Unverified, Owner reverted");
		ResourceEN.Add(ResourceKey.UncheckedRevNotChanged, "Unverified, Owner not reverted");
		ResourceEN.Add(ResourceKey.UncheckedRevPartiallyChanged, "Unverified, Owner partially reverted");
		ResourceEN.Add(ResourceKey.VatPayer, "Vat payer");
		ResourceEN.Add(ResourceKey.SalesTaxPayer, "Sales tax payer");
		ResourceEN.Add(ResourceKey.MicroenterpriseEntity, "Microenterprise entity");
		ResourceEN.Add(ResourceKey.OtherTaxPayer, "'Other' tax payer");
		ResourceEN.Add(ResourceKey.SubjectToApproval, "Subject to approval");
		ResourceEN.Add(ResourceKey.ApprovedByBuyer, "Approved by buyer");
		ResourceEN.Add(ResourceKey.ErrorAccuredDuringWebResponse, "A technical error occurred: the '{0}' call did not return a response.");
		ResourceEN.Add(ResourceKey.MarkingsAreAlreadyReflected, "Markings are already reflected.");
		ResourceEN.Add(ResourceKey.AccessDeniedForOperation, "You do not have permission to perform this action in the e-invoicing.");
		ResourceEN.Add(ResourceKey.ImpossibleReflectMarkings, "Cannot reflect markings in document with \"{0}\" serial number.\r\nThe document has already been signed by the buyer");
	}

	private static void FillResourceAM()
	{
		ResourceAM.Add(ResourceKey.EInvoicingError, "e-invoicing սխալ․ ");
		ResourceAM.Add(ResourceKey.DocAlreadyExistWithId, "Տվյալ պարամետրերով փաստաթուղթ e-invoicing համակարգում արդեն կա                                                                                                           \r\nՍիստեմային համար - {0}");
		ResourceAM.Add(ResourceKey.OtherTypeDocAlreadyExist, "Տվյալ սիստեմային համարով այլ տեսակի փաստաթուղթ e-invoicing համակարգում արդեն կա\r\nՄատակարարման ամսաթիվ - {0}\r\nՏեսակ - {1}\r\nՍիստեմային համար - {2}");
		ResourceAM.Add(ResourceKey.DocAlreadyExistWithDeliveryDate, "Տվյալ պարամետրերով փաստաթուղթ e-invoicing համակարգում արդեն կա\r\nՄատակարարման ամսաթիվ - {0}                                                                                                    \r\nՍիստեմային համար - {1}");
		ResourceAM.Add(ResourceKey.DocAlreadyExistWithDeliveryDateBuyer, "Տվյալ պարամետրերով փաստաթուղթ e-invoicing համակարգում արդեն կա\r\nՄատակարարման ամսաթիվ - {0}\r\nԳնորդ - {1}                                                                                                                   \r\nՍիստեմային համար - {2}");
		ResourceAM.Add(ResourceKey.DocNotExistWithId, "«{0}» սիստեմային համարով փաստաթուղթ e-invoicing համակարգում չկա");
		ResourceAM.Add(ResourceKey.DocNotExistWithSerialNumber, "«{0}» սերիա համարով փաստաթուղթ e-invoicing համակարգում չկա");
		ResourceAM.Add(ResourceKey.Special, "Հատուկ");
		ResourceAM.Add(ResourceKey.TaxPayer, "Հարկ վճարողի անունից");
		ResourceAM.Add(ResourceKey.Supplier, "Մատակարարի անունից");
		ResourceAM.Add(ResourceKey.Principal, "Հանձնարարողի(պրինցիպալի) անունից");
		ResourceAM.Add(ResourceKey.JointCaseMng, "Համատեղ գործերը վարողի անունից");
		ResourceAM.Add(ResourceKey.VatReturn, "ԱԱՀ-ի Վերադարձի Հաշիվ");
		ResourceAM.Add(ResourceKey.Approved, "Հաստատված");
		ResourceAM.Add(ResourceKey.PendingAppSt, "Ենթակա է հաստատման");
		ResourceAM.Add(ResourceKey.AccDocGoods, "Ապրանքների մատակարարման հաշիվ վավերագիր");
		ResourceAM.Add(ResourceKey.AccDocServices, "Ծառայությունների մատուցման, աշխատանքների կատարման հաշիվ վավերագիր");
		ResourceAM.Add(ResourceKey.AccDocTransportation, "Ապրանքի տեղափոխության բեռնագիր");
		ResourceAM.Add(ResourceKey.AccDocExportEEU, "ԵՏՄ արտահանման հաշիվ վավերագիր(արտադրանքների, ապրանքների մատակարարման)");
		ResourceAM.Add(ResourceKey.AccDocTraceableG, "Հետագծելիության ենթակա ապրանքի տեղափոխության բեռնագիր");
		ResourceAM.Add(ResourceKey.InvoiceGoods, "Ապրանքների մատակարարման հարկային հաշիվ");
		ResourceAM.Add(ResourceKey.InvoiceServices, "Ծառայությունների մատուցման, աշխատանքների կատարման հարկային հաշիվ");
		ResourceAM.Add(ResourceKey.InvoiceExcise, "Ակցիզային հարկով հարկվող ապրանքների հարկային հաշիվ");
		ResourceAM.Add(ResourceKey.InvoiceVatReturn, "ԱԱՀ-ի վերադարձի հաշիվ");
		ResourceAM.Add(ResourceKey.InvoiceLeasing, "Լիզինգի առարկայի հանձնման-ընդունման ակտ");
		ResourceAM.Add(ResourceKey.Corrected, "Ճշգրտված");
		ResourceAM.Add(ResourceKey.Corrector, "Ճշգրտող");
		ResourceAM.Add(ResourceKey.Correcting, "Ճշգրտվող");
		ResourceAM.Add(ResourceKey.Accepted, "Հաստատված");
		ResourceAM.Add(ResourceKey.Declined, "Մերժված");
		ResourceAM.Add(ResourceKey.PendingCnclSt, "Չեղարկման ընթացքում");
		ResourceAM.Add(ResourceKey.Increasing, "Ավելացում");
		ResourceAM.Add(ResourceKey.Decreasing, "Նվազեցում");
		ResourceAM.Add(ResourceKey.InvoiceIsNotEditable, "Հաշիվը խմբագրման ենթակա չէ։ Վիճակ - {0}");
		ResourceAM.Add(ResourceKey.Issued, "Դուրս գրված");
		ResourceAM.Add(ResourceKey.Draft, "Սևագիր");
		ResourceAM.Add(ResourceKey.Invalid, "Անվավեր");
		ResourceAM.Add(ResourceKey.WaitingInvalidationAcceptance, "Սպասում չեղարկման հաստատմանը");
		ResourceAM.Add(ResourceKey.Cancelled, "Չեղարկված");
		ResourceAM.Add(ResourceKey.Deleted, "Հեռացված");
		ResourceAM.Add(ResourceKey.PhoneOperatorCannotBeEmpty, "Հեռախոսային օպերատորը չի կարող լինել դատարկ");
		ResourceAM.Add(ResourceKey.PhoneNumberCannotBeEmpty, "Հեռախոսահամարը չի կարող լինել դատարկ");
		ResourceAM.Add(ResourceKey.IncorrectDirectoryPath, "«{0}» ճանապարհը սխալ է կամ հասանելի չէ");
		ResourceAM.Add(ResourceKey.FileDoesNotExist, "«{0}» ֆայլը գոյություն չունի");
		ResourceAM.Add(ResourceKey.FileAlreadyExists, "«{0}» ֆայլը գոյություն ունի");
		ResourceAM.Add(ResourceKey.RepeatedRowsInListNeedToDoFromEInvoicing, "Ցուցակում կան կրկնվող տողեր։ Անհրաժեշտ է ճշգրտումը կատարել e-invoicing համակարգից");
		ResourceAM.Add(ResourceKey.ProductAbsentInEInvoicing, "Ապրանքը բացակայում է e-invoicing համակարգում\r\nԱպրանք - {0}\r\nՉափման միավոր - {1}\r\nԳին - {2}");
		ResourceAM.Add(ResourceKey.HashCalculating, "Նախապատրաստում ստորագրման");
		ResourceAM.Add(ResourceKey.SignWaiting, "Ենթակա է ստորագրման");
		ResourceAM.Add(ResourceKey.Failed, "Ձախողված");
		ResourceAM.Add(ResourceKey.SignFailed, "Ստորագրման ձախողում");
		ResourceAM.Add(ResourceKey.SignProcessing, "Ստորագրման ընթացքում");
		ResourceAM.Add(ResourceKey.Signed, "Ստորագրված");
		ResourceAM.Add(ResourceKey.SignStatusChanged, "Առկա է ստորագրման կարգավիճակի փոփոխություն, խնդրում ենք թարմացնել էջը։");
		ResourceAM.Add(ResourceKey.EntityNotFound, "Հաշիվը կամ փաթեթը բացակայում է e-invoicing համակարգից։");
		ResourceAM.Add(ResourceKey.SpecialTaxSystem, "Հարկման հատուկ համակարգ");
		ResourceAM.Add(ResourceKey.VatExempt, "ԱԱՀ-ից ազատված");
		ResourceAM.Add(ResourceKey.VatZeroRate, "ԱԱՀ-ի 0% դրույքաչափով հարկվող");
		ResourceAM.Add(ResourceKey.Export, "Արտահանում");
		ResourceAM.Add(ResourceKey.ExportEAEUAir, "ԵՏՄ օդային տրանսպորտով արտահանում");
		ResourceAM.Add(ResourceKey.ExcludedFromVAT, "ԱԱՀ-ով հարկման օբյեկտ չհամարվող");
		ResourceAM.Add(ResourceKey.VatNonPayers, "ԱԱՀ չվճարողներ");
		ResourceAM.Add(ResourceKey.VatOther, "Այլ");
		ResourceAM.Add(ResourceKey.InvoiceFormatIsIncorrect, "Հարկային հաշիվը չի համապատասխանում պահանջվող ձևաչափին");
		ResourceAM.Add(ResourceKey.AccDocFormatIsIncorrect, "Հաշվարկային փաստաթուղթը չի համապատասխանում պահանջվող ձևաչափին");
		ResourceAM.Add(ResourceKey.WaitTimeoutExpired, "Սպասելու ժամանակը սպառվել է");
		ResourceAM.Add(ResourceKey.NewRegistered, "Նոր գրանցված");
		ResourceAM.Add(ResourceKey.Active, "Գործող");
		ResourceAM.Add(ResourceKey.Bankrupt, "Սնանկ");
		ResourceAM.Add(ResourceKey.Dissolved, "Լուծարված");
		ResourceAM.Add(ResourceKey.Died, "Մահացած");
		ResourceAM.Add(ResourceKey.IncorrectlyProvided, "Սխալմամբ տրամադրված");
		ResourceAM.Add(ResourceKey.DissolvedMerged, "Լուծարված-միաձուլված");
		ResourceAM.Add(ResourceKey.DissolvedUnited, "Լուծարված-միավորված");
		ResourceAM.Add(ResourceKey.TimedSuspended, "Ժամանակավոր դադարեցված");
		ResourceAM.Add(ResourceKey.DissolvedProcess, "Լուծարման գործընթացում");
		ResourceAM.Add(ResourceKey.ExtractedFromAccounting, "Հաշվառումից հանված");
		ResourceAM.Add(ResourceKey.InRiskOfBankruptcy, "Սնանկության վտանգի վարույթում գտնվող");
		ResourceAM.Add(ResourceKey.Unknown, "Անհայտ");
		ResourceAM.Add(ResourceKey.WrongClassifier, "«{0}» անունով ապրանքի համար «{1}» ԱՏԳԱԱ ծածկագիրը սխալ է");
		ResourceAM.Add(ResourceKey.ProductParentIdAbsentInEInvoicing, "«{0}» անունով ապրանքի նույնականացուցիչը հիմք հաշվում բացակայում է");
		ResourceAM.Add(ResourceKey.NeedToFillDateTime, "Անհրաժեշտ է նշել ամսաթվային դաշտերից առնվազն մեկի և՛ սկիզբը և՛ ավարտը");
		ResourceAM.Add(ResourceKey.NeedToFillDateTimeOrSerialNumber, "Անհրաժեշտ է նշել սերիա համարը կամ ամսաթվային դաշտերից առնվազն մեկի և՛ սկիզբը և՛ ավարտը");
		ResourceAM.Add(ResourceKey.EInvoicingLoginError, "«{0}» օգտագործողի մուտքը համակարգ հնարավոր է որոշ ժամանակ անց");
		ResourceAM.Add(ResourceKey.FieldValueCannotExceedLength, "«{0}» դաշտի արժեքը չի կարող գերազանցել {1} նիշը");
		ResourceAM.Add(ResourceKey.PlaceFromWhereGoodsAreSupplied, "Վայր, որտեղից ապրանքները մատակարարվում (առաքվում) են");
		ResourceAM.Add(ResourceKey.DestinationAddressOfGoods, "Մատակարարվող (առաքվող) ապրանքների նշանակման վայր (հասցե)");
		ResourceAM.Add(ResourceKey.AdditionalInfoPersonSupplyingGoods, "Ապրանքներ մատակարարող (առաքող) անձի լրացուցիչ տվյալներ");
		ResourceAM.Add(ResourceKey.AdditionalInfoPersonPurchasingGoods, "Ապրանքներ ձեռք բերող անձի լրացուցիչ տվյալներ");
		ResourceAM.Add(ResourceKey.AdditionalInfoPersonProvidingServices, "Ծառայություններ մատուցող (աշխատանքներ կատարող) անձի տվյալներ");
		ResourceAM.Add(ResourceKey.AdditionalInfoPersonReceivingServices, "Ծառայություններ (աշխատանքներ) ստացող անձի լրացուցիչ տվյալներ");
		ResourceAM.Add(ResourceKey.AdditionalInfo, "Լրացուցիչ տվյալներ");
		ResourceAM.Add(ResourceKey.LessorAdditionalInfo, "Լիզինգատուի լրացուցիչ տվյալներ");
		ResourceAM.Add(ResourceKey.LesseeAdditionalInfo, "Լիզինգառուի լրացուցիչ տվյալներ");
		ResourceAM.Add(ResourceKey.LessorActivityCarriedOutPlaceAddress, "Լիզինգատուի գործունեությունը փաստացի իրականացնելու վայրի հասցե");
		ResourceAM.Add(ResourceKey.LessorDeliveryHandoverLeasingObjectPlaceAddress, "Լիզինգի առարկայի առաքման կամ հանձնման վայրի հասցե");
		ResourceAM.Add(ResourceKey.LesseeActivityCarriedOutPlaceAddress, "Լիզինգառուի գործունեությունը փաստացի իրականացնելու վայրի հասցե");
		ResourceAM.Add(ResourceKey.LesseeDestinationLeasingObjectPlaceAddress, "Լիզինգի առարկայի նշանակման վայրի հասցե");
		ResourceAM.Add(ResourceKey.Pending, "Ընթացիկ");
		ResourceAM.Add(ResourceKey.CheckedInProgress, "Ստուգված, Սեփականատերը փոխելու ընթացքում");
		ResourceAM.Add(ResourceKey.CheckedChanged, "Ստուգված, Սեփականատերը փոխված");
		ResourceAM.Add(ResourceKey.CheckedNotChanged, "Ստուգված, Սեփականատերը չփոխված");
		ResourceAM.Add(ResourceKey.CheckedPartiallyChanged, "Ստուգված, Սեփականատերը մասնակի փոխված");
		ResourceAM.Add(ResourceKey.UncheckedInProgress, "Չստուգված, Սեփականատերը փոխելու ընթացքում");
		ResourceAM.Add(ResourceKey.UncheckedChanged, "Չստուգված, Սեփականատերը փոխված");
		ResourceAM.Add(ResourceKey.UncheckedNotChanged, "Չստուգված, Սեփականատերը չփոխված");
		ResourceAM.Add(ResourceKey.UncheckedPartiallyChanged, "Չստուգված, Սեփականատերը մասնակի փոխված");
		ResourceAM.Add(ResourceKey.UncheckedAutoChanged, "Չստուգված, ինքնաշխատ սեփականատերը փոխված");
		ResourceAM.Add(ResourceKey.UncheckedAutoNotChanged, "Չստուգված, ինքնաշխատ սեփականատերը չփոխված");
		ResourceAM.Add(ResourceKey.UncheckedAutoPartiallyChanged, "Չստուգված, ինքնաշխատ մասնակի սեփականատերը փոխված");
		ResourceAM.Add(ResourceKey.CheckedRevInProgress, "Ստուգված, Սեփականատերը հետ փոխելու ընթացքում");
		ResourceAM.Add(ResourceKey.CheckedRevChanged, "Ստուգված, Սեփականատերը հետ փոխված");
		ResourceAM.Add(ResourceKey.CheckedRevNotChanged, "Ստուգված, Սեփականատերը հետ չփոխված");
		ResourceAM.Add(ResourceKey.CheckedRevPartiallyChanged, "Ստուգված, Սեփականատերը մասնակի հետ փոխված");
		ResourceAM.Add(ResourceKey.UncheckedRevInProgress, "Չստուգված, Սեփականատերը հետ փոխելու ընթացքում");
		ResourceAM.Add(ResourceKey.UncheckedRevChanged, "Չստուգված, Սեփականատերը հետ փոխված");
		ResourceAM.Add(ResourceKey.UncheckedRevNotChanged, "Չստուգված, Սեփականատերը հետ չփոխված");
		ResourceAM.Add(ResourceKey.UncheckedRevPartiallyChanged, "Չստուգված, Սեփականատերը մասնակի հետ փոխված");
		ResourceAM.Add(ResourceKey.VatPayer, "ԱԱՀ վճարող");
		ResourceAM.Add(ResourceKey.SalesTaxPayer, "Շրջանառության հարկ վճարող");
		ResourceAM.Add(ResourceKey.MicroenterpriseEntity, "Միկրոձեռնարկատիրության սուբյեկտ");
		ResourceAM.Add(ResourceKey.OtherTaxPayer, "«Այլ» հարկ վճարող");
		ResourceAM.Add(ResourceKey.SubjectToApproval, "Ենթակա է հաստատման");
		ResourceAM.Add(ResourceKey.ApprovedByBuyer, "Հաստատված գնորդի կողմից");
		ResourceAM.Add(ResourceKey.ErrorAccuredDuringWebResponse, "Առկա է տեխնիկական սխալ. '{0}' կանչը պատասխան չի վերադարձրել");
		ResourceAM.Add(ResourceKey.MarkingsAreAlreadyReflected, "Դրոշմավորման ծածկագրերն արդեն արտացոլված են");
		ResourceAM.Add(ResourceKey.AccessDeniedForOperation, "Տվյալ գործողությունը e-invoicing համակարգում կատարելու իրավասություն չունեք");
		ResourceAM.Add(ResourceKey.ImpossibleReflectMarkings, "\"{0}\" սերիա համարով փաստաթղթում հնարավոր չէ արտացոլել դրոշմավորման ծածկագրերը։\r\nՀաշիվը գնորդի կողմից արդեն ստորագրված է");
	}
}
