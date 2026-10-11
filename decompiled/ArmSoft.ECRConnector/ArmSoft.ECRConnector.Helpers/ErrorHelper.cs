using System;
using System.IO;

namespace ArmSoft.ECRConnector.Helpers;

public static class ErrorHelper
{
	public static string GetErrorMessage(int errCode, Utilities.ErrMessageLanguage errMsgLang = Utilities.ErrMessageLanguage.Armenian)
	{
		string text = "ՀԴՄ սխալ. ";
		if (errMsgLang == Utilities.ErrMessageLanguage.English)
		{
			text = "ECR Error: ";
		}
		string text2 = errCode switch
		{
			500 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Internal error, try to restart ECR" : "ՀԴՄ ներքին սխալ, փորձեք վերաբեռնավորել(restart) ՀԴՄ-ն", 
			400 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Request error" : "Հարցման սխալ", 
			402 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect protocol version" : "Սխալ արձանագրության տարբերակ", 
			403 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Unauthorized connection, in ECR You need to fill Your computer's IP address as automation system IP. " : "Չարտոնագրված գործողություն, ՀԴՄ-ում որպես ավտոմատ համակարգի IP հասցե անհրաժեշտ է նշել Ձեր համակարգչի IP հասցեն", 
			404 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect operation code" : "Գործողության սխալ կոդ", 
			101 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Encoding error: Incorrect password" : "Գաղտնաբառով կոդավորման սխալ", 
			102 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect session key" : "Սեսիայի բանալիով կոդավորման սխալ", 
			103 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect request header" : "Գլխագրի ֆորմատի սխալ", 
			104 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect request sequential number" : "Հարցման հերթական համարի սխալ", 
			105 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect JSON format" : "JSON ֆորմատավորման սխալ", 
			111 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Login error: Incorrect user password" : "Օպերատորի գաղտնաբառի սխալ", 
			112 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Login error: Such user does not exist" : "Այդպիսի օպերատոր գոյություն չունի", 
			113 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Login error: Inactive user" : "Օպերատորը ակտիվ չէ", 
			121 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect user" : "Սխալ օգտվող", 
			141 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? " Last receipt’s  record is missing" : "Վերջին կտրոնի գրառումը բացակայում է", 
			142 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Last receipt belongs to another cashier" : "Վերջին կտրոնը պատկանում է այլ օգտատիրոջը", 
			143 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Printer error" : "Տպիչի ընդհանուր սխալ", 
			144 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Printer initialization error" : "Տպիչի ինիցիալիզացիայի սխալ", 
			145 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "No paper" : "Տպիչում վերջացել է թուղթը", 
			151 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect department code" : "Այդպիսի բաժին գոյություն չունի", 
			152 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Paid amount is less than total amount" : "Մուծված գումարը ընդհանուր գումարից պակաս է", 
			153 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Receipt amount exceeds specified limit" : "Կտրոնի գումարը գերազանզում է սահմանված շեմը", 
			154 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Receipt amount must be positive" : "Կտրոնի գումարը պետք է լինի դրական թիվ", 
			155 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Synchronization needed" : "Անհրաժեշտ է համաժամանակեցնել ՀԴՄ-ն", 
			156 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Synchronization failed" : "Համաժամանակեցումը չհաջողվեց", 
			157 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Invalid ID of return receipt" : "Սխալ վերադարձի կտրոնի համար", 
			158 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Receipt is already returned" : "Կտրոնն արդեն վերադարձված է", 
			159 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Product price and quantity cannot be negative" : "Ապրանքի գինը և քանակը չի կարող լինել ոչ դրական", 
			160 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Discount percent must be not negative and less than 100" : "Զեղչի տոկոսը պետք է լինի ոչ բացասական թիվ և լինի 100-ից փոքր", 
			161 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Product code is incorrect" : "Ապրանքի կոդը սխալ է", 
			162 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Product name is incorrect" : "Ապրանքի անվանումը սխալ է", 
			163 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Product unit of measure cannot be empty" : "Ապրանքի չափման միավորը չի կարող լինել դատարկ", 
			164 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Non cash payment failed" : "Անկանխիկ վճարման խափանում", 
			165 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Product price cannot be 0" : "Ապրանքի գինը չի կարող լինել 0", 
			166 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Final price calculation error" : "Վերջնական գնի հաշվարկի սխալ", 
			167 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Non cash amount is greater than receipt’s total amount" : "Անկանխիկ գումարը ավելի մեծ է քան կտրոնի ընդհանուր գումարը", 
			168 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Non cash amount covers the total amount (no cash is needed)" : "Անկանխիկ գումարը ծածկում է ընդհանուր գումարը (Կանխիկ գումարն ավելորդ է)", 
			169 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect selection of fiscal report’s filters (more than one filters field has been sent)" : "Ֆիսկալ հաշվետվության ֆիլտրների սխալ ընտրություն (մեկից ավել ֆիլտրերի դաշտ է ուղարկվել)", 
			170 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "In Fiscal report incorrect date interval has been sent. Interval must not exceed 2 months" : "Ֆիսկալ հաշվետվության ժամանակ սխալ ամսաթվային միջակայք է ուղարկվել: Միջակայքը չպետք է գերազանցի 2 ամիսը", 
			171 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Product price invalid value" : "Ապրանքի գնի անթույլատրելի արժեք", 
			172 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Receipt is not products, simple or prepayment receipt" : "Կտրոնը ապրանքներով, պարզ կամ կանխավճարի կտրոն չէ", 
			173 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Incorrect discount type" : "Սխալ զեղչի տեսակ", 
			174 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Return receipt does not exist" : "Վերադարձվող կտրոնը գոյություն չունի", 
			175 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Return receipt’s incorrect registration number" : "Վերադարձվող կտրոնի սխալ գրանցման համար", 
			176 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Last receipt does not exists" : "Վերջին կտրոնը գոյություն չունի", 
			177 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "For selected types of receipts return cannot be made" : "Նշված տիպի կտրոնների համար վերադարձ հնարավոր չէ կատարել", 
			178 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Requested amount cannot be returned" : "Հարցված գումարը հնարավոր չէ վերադարձնել", 
			179 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Partial payment receipt must be fully returned" : "Մասնակի վճարում կտրոնը պետք է վերադարձվի ամբողջությամբ", 
			180 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Full return amount is greater than receipt’s total amount" : "Ամբողջական վերադարձի ավելի գումար (վերադարձվող գումարը գերազանցում է կտրոնի ընդհանուր գումարը)", 
			181 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Returned product’s invalid quantity" : "Վերադարձվող ապրանքի սխալ քանակ", 
			182 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Return receipt is a receipt with return type" : "Վերադարձվող կտրոնը իրենից ներկայացնում է վերադարձ տիպի կտրոն", 
			183 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Invalid CPA classifier" : "Սխալ ԱՏԳԱԱ/ԱԴԳՏ կոդ", 
			184 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Invalid request of prepayment return" : "Կանխավճարի վերադարձի անթույլատրելի հարցում", 
			185 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Return for selected receipt cannot be made. ECR system synchronization is needed" : "Հնարավոր չէ կատարել տվյալ կտրոնի վերադարձը: Անհրաժեշտ է ՀԴՄ ծրագրի համաժամանակեցում", 
			186 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Invalid sum during prepayment" : "Կանխավճարի դեպքում սխալ գումար", 
			187 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Invalid list during prepayment" : "Կանխավճարի դեպքում սխալ ցուցակ", 
			188 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Invalid sums" : "Սխալ գումարներ", 
			189 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Invalid rounding" : "Սխալ կլորացում", 
			190 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Payment is unavailable" : "Վճարումը հասանելի չէ", 
			191 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Sum must be greater than zero during cash in/out" : "Կանխիկի մուտքի/ելքի ժամանակ գումարը պետք է մեծ լինի 0-ից", 
			192 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "CPA classifier is required" : "ԱՏԳ կոդը պարտադիր է", 
			193 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Invalid Format of Partner's Tin" : "Գնորդի ՀՎՀՀ-ի ֆորմատը սխալ է", 
			501 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "SRC server connection error" : "ՊԵԿ սերվերի հետ կապի խնդիր", 
			502 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "No internet access" : "Չկա համացանցի հասանելիություն", 
			612 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "The amount of data being transferred exceeds the specified size (30 kb)" : "Փոխանցվող տվյալների չափը գերազանցում է սահմանված չափը (30 կբ)", 
			613 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Only cashless transaction is allowed with buyer's TIN" : "Գնորդի ՀՎՀՀ-ով թույլատրվում է միայն անկանխիկ գործարք", 
			614 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Only cashless transaction is allowed, limited by ECR settings" : "Թույլատրվում է միայն անկանխիկ գործարքներ, սահմանափակված է ՀԴՄ-ի կարգաբերումներից", 
			615 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Only cashless transaction is allowed, the amount exceeds the limit (300,000.00 AMD)" : "Թույլատրվում է միայն անկանխիկ գործարքներ, գումարը գերազանցում է սահմանաչափը (300,000.00 դրամ)", 
			194 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "No emark codes should be sent in case of prepayment." : "Կանխավճարի դեպքում չպետք է ուղարկվեն emark կոդեր:", 
			195 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Emark code format error." : "Emark կոդի ֆորմատի սխալ:", 
			196 => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Wrong country code." : "Այլ երկրի ծածկագիր:", 
			_ => (errMsgLang != Utilities.ErrMessageLanguage.Armenian) ? "Unknown Error" : "Անհայտ սխալ", 
		};
		string contents = Environment.NewLine + text + errCode + " " + text2 + Environment.NewLine;
		File.AppendAllText(ECR.GetLogFilePath(), contents);
		return text + text2;
	}
}
