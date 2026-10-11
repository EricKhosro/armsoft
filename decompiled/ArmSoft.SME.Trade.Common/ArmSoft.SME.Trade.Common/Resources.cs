using ArmSoft.SME.Common;

namespace ArmSoft.SME.Trade.Common;

public static class Resources
{
	public static string ABCAnalysisByGoods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ABC analysis of sales by products";
				}
				return "ABC анализ продаж по товарам";
			}
			return "Վաճառքների ABC վերլուծություն ըստ ապրանքների";
		}
	}

	public static string ABCAnalysisByPartners
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ABC analysis of sales by partners";
				}
				return "ABC анализ продаж по партнерам";
			}
			return "Վաճառքների ABC վերլուծություն ըստ գործընկերների";
		}
	}

	public static string ABCCategory
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Category";
				}
				return "{0} классификация";
			}
			return "{0} դաս";
		}
	}

	public static string ABCCategoryPercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Category range (%)";
				}
				return "Лимит классификации {0} (%)";
			}
			return "{0} դասի շեմ (%)";
		}
	}

	public static string AccDocExportedCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} accounting documents were exported";
				}
				return "Экспортировано {0} учетных документов";
			}
			return "Արտահանվել է {0} հաշիվ վավերագիր";
		}
	}

	public static string AccDocNotForExport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "This accounting document cannot be exported";
				}
				return "Отмеченный расчетный документ не подлежит экспорту.";
			}
			return "Նշված հաշիվ վավերագիրը արտահանման ենթակա չէ";
		}
	}

	public static string AccDocType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Accounting document type";
				}
				return "Тип расчетного документа";
			}
			return "Հաշիվ վավերագրի տեսակ";
		}
	}

	public static string AccEntryPartNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transaction exists by \"{0}\" contract and \"{1}\" Inner system number (ISN) of partner, but partner misses from \"Partners\" directory";
				}
				return "Найдена операция по договору \"{0}\" партнера с внутренним идентификатором \"{1}\", но партнер отсутствует в справочнике Партнеры";
			}
			return "\"{0}\" պայմանագրով և \"{1}\" գործընկերի ներքին սիստեմային բնութագրիչով կա գործառնություն,բայց գործընկերը բացակայում է \"Գործընկերներ\" տեղեկատուից";
		}
	}

	public static string AccessForUserToAllSubsystemsDenied
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" user's access to all subsystems of \"{1}\" database is denied.";
				}
				return "Вход пользователя \"{0}\" в подсистемы базы данных \"{1}\" запрещен.";
			}
			return "\"{0}\" օգտագործողի մուտքը \"{1}\" տվյալների պահոցի բոլոր ենթահամակարգեր արգելված է:";
		}
	}

	public static string AccessToCashesDenied
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "User does not have access to any cash desk";
				}
				return "Пользователю не разрешен доступ ни к одной кассе";
			}
			return "Օգտագործողին ոչ մի դրամարկղ հասանելի չէ";
		}
	}

	public static string AccHI
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks, Settlement accounts and partners turnover";
				}
				return "Оборот касс, банковских счетов и партнеров";
			}
			return "Դրամարկղերի, հաշվարկային հաշիվների, գործընկերների շրջանառություն";
		}
	}

	public static string AccInBankBankNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{1}\" bank code of \"{0}\" account \"{1}\" is missing from directory \"Banks\"";
				}
				return "Банк \"{1}\" банковского счета \"{0}\" отсутствует в справочнике \"Банки\"";
			}
			return "\"{0}\" հաշվարկային հաշվի \"{1}\" բանկը բացակայում է \"Բանկեր\" տեղեկատուից";
		}
	}

	public static string AccountN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Account N";
				}
				return "Счет.N";
			}
			return "Հաշիվ.N";
		}
	}

	public static string AccRemPartNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balance exists for \"{0}\" contract and \"{1}\" Inner system number (ISN) of partner, but partner misses from \"Partners\" directory";
				}
				return "Найден остаток по договору \"{0}\" партнера с внутренним идентификатором \"{1}\", но партнер отсутствует в справочнике Партнеры";
			}
			return "\"{0}\" պայմանագրով և \"{1}\" գործընկերի ներքին սիստեմային բնութագրիչով կա մնացորդ,բայց գործընկերը բացակայում է \"Գործընկերներ\" տեղեկատուից";
		}
	}

	public static string AccRest
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks, Settlement accounts and partners balances";
				}
				return "Остатки по кассам, банковским счетам и партнерам";
			}
			return "Դրամարկղերի, հաշվարկային հաշիվների, գործընկերների մնացորդներ";
		}
	}

	public static string AccumulateBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Accumulated bonus";
				}
				return "Начисления бонуса";
			}
			return "Հաշվարկված բոնուսներ";
		}
	}

	public static string AccumulateBonusForDiscountedGoods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Earn bonus for discounted products";
				}
				return "Начислять для товаров со скидкой";
			}
			return "Զեղչված ապրանքների համար կուտակել";
		}
	}

	public static string AccumulatedBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Earned bonus";
				}
				return "Начисленный бонус";
			}
			return "Կուտակված բոնուս";
		}
	}

	public static string AccumulatedBonusShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Acc. bon.";
				}
				return "Накоп. бон.";
			}
			return "Կուտ. բոն.";
		}
	}

	public static string ActualnessDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reliability date";
				}
				return "Дата актуальности ";
			}
			return "Հավաստիության ամսաթիվ";
		}
	}

	public static string ActualnessDateShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reliability date - ";
				}
				return "Дата актуальности - ";
			}
			return "Հավաստիության ա/թ - ";
		}
	}

	public static string AddCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add number \"{1}\" shift of \"{0}\" cashdesk in Shifts directory";
				}
				return "Добавление в справочнике Смены строки для кассы \"{0}\"  с номером смены \"{1}\"";
			}
			return "Հերթափոխերի տեղեկատուում \"{0}\" դրամարկղով \"{1}\" համարի հերթափոխի ավելացում";
		}
	}

	public static string AddedBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Added to bonus";
				}
				return "Added to bonus";
			}
			return "Ավելացված բոնուսին";
		}
	}

	public static string AddGoodsAndServices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add your products and services";
				}
				return "Добавьте Ваши товары и услуги";
			}
			return "Ավելացրեք Ձեր ապրանքները և ծառայությունները";
		}
	}

	public static string AddMaterialToPartnersMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "During purchase add the products in the supplier's products list";
				}
				return "При регистрации покупки товар добавлять в список Товары поставщиков";
			}
			return "Ձեռքբերում գրանցելուց ապրանքները ավելացնել Մատակարարի ապրանքացանկում";
		}
	}

	public static string AddNewMTOrService
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add new product or service";
				}
				return "Добавить новый товар или услугу";
			}
			return "Ավելացնել նոր ապրանք կամ ծառայություն";
		}
	}

	public static string AddNewRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add new row";
				}
				return "Добавить новую строку";
			}
			return "ավելացնել նոր տող";
		}
	}

	public static string AddShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add(+)";
				}
				return "Доб.(+)";
			}
			return "Ավել.(+)";
		}
	}

	public static string AddToBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add to bonus";
				}
				return "Добавить к бонусам";
			}
			return "Ավելացնել բոնուսին";
		}
	}

	public static string AddToBonusShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add bon.";
				}
				return "Доб. к бон.";
			}
			return "Ավ. բոն.";
		}
	}

	public static string AllAccountIsAccumulate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "None of the selected accounts is of final level";
				}
				return "Ни один из выделенных счетов не является последнего уровня";
			}
			return "Նշված հաշիվներից ոչ մեկը վերջին մակարդակի չէ";
		}
	}

	public static string AllDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "All documents";
				}
				return "Все документы";
			}
			return "Բոլոր փաստաթղթերը";
		}
	}

	public static string AllowChangeDiscountPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow discount price change";
				}
				return "Разрешить редактирование цены со скидкой";
			}
			return "Թույլ տալ խմբագրել զեղչված գինը";
		}
	}

	public static string AllowChangeDiscountPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to change discount price type";
				}
				return "Разрешить редактирование типа цены со скидкой";
			}
			return "Թույլ տալ խմբագրել զեղչի գնի տեսակը";
		}
	}

	public static string AllowChangeGoodRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow editing(deleting) product row";
				}
				return "Разрешить редактирование(удаление) строку товара";
			}
			return "Թույլ տալ խմբագրել (հեռացնել) ապրանքի տողը";
		}
	}

	public static string AllowChangePriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow editing of price type";
				}
				return "Разрешить редактирование типа цены";
			}
			return "Թույլ տալ խմբագրել գնի տեսակը";
		}
	}

	public static string AllowCloseCashierShiftWhenNotHavePosponedSales
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Don't allow to close cashier shift in case of having delayed sales documents";
				}
				return "Запретить закрытие смены при наличии отложенных документов продаж";
			}
			return "Թույլ չտալ փակել հերթափոխը հետաձգված փաստաթղթերի առկայության դեպքում";
		}
	}

	public static string AllowDecreaseQuantity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to decrease quantity";
				}
				return "Разрешить уменьшение количества";
			}
			return "Թույլ տալ քանակի նվազեցում";
		}
	}

	public static string AllowDeleteRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to delete row";
				}
				return "Разрешить удаление строки ";
			}
			return "Թույլ տալ տողի հեռացում";
		}
	}

	public static string AllowDiscountInPurchase
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow discount during purchase";
				}
				return "В документах закупки использовать скидки";
			}
			return "Գնման ժամանակ թույլատրել օգտագործել զեղչ";
		}
	}

	public static string AllowEditGoodsPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to edit the price";
				}
				return "Разрешить редактирование цены";
			}
			return "Թույլ տալ գնի խմբագրում";
		}
	}

	public static string AllowExceedDebtLimit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow exceeding receivables limit during the sale";
				}
				return "Разрешить превышение лимита дебиторской задолженности при продаже";
			}
			return "Վաճառք գրանցելուց թույլ տալ գերազանցել պարտքի սահմանաչափը";
		}
	}

	public static string AllowInputFromKeyboard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to enter card number from keyboard";
				}
				return "Разрешить ввод карты покупателя";
			}
			return "Թույլ տալ ստեղնաշարից քարտի մուտքագրում";
		}
	}

	public static string AllowMakeReturnDocument
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to create Return document";
				}
				return "Разрешить оформление возврата";
			}
			return "Թույլ տալ վերադարձի գրանցում";
		}
	}

	public static string AllowNegativExp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allowing negative quantities";
				}
				return "Разрешить превышение остатков при списании";
			}
			return "Թույլատրել առկայությունից ավելի ելքագրում";
		}
	}

	public static string AllowPurchaseWith0PriceOr100Percent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to puchase at 0 price or with 100% discount";
				}
				return "Разрешить приобретение по 0-ой цене или со 100% скидкой";
			}
			return "Թույլ տալ ձեռք բերել 0 գնով կամ 100% զեղչով";
		}
	}

	public static string AllowRemoveCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to remove the selected card";
				}
				return "Разрешить отмену введенной карты покупателя";
			}
			return "Թույլ տալ մաքրել նշված քարտը";
		}
	}

	public static string AllowUse100PercentSale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow 100% discount on all rows";
				}
				return "Разрешить реализацию всех строк со 100%-ой скидкой";
			}
			return "Թույլ տալ բոլոր տողերը վաճառել 100% զեղչով";
		}
	}

	public static string AllStoragesAvailForAdmin
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "All storages are available for Administrator";
				}
				return "Администратору доступны все склады ";
			}
			return "Ադմինիստրատորին հասանելի են բոլոր պահեստները";
		}
	}

	public static string AlreadyExportedB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product or Service {0} has already exported to b2b.armsoft.am";
				}
				return "Товар или услуга с кодом {0}  уже был экспортирован на b2b.armsoft";
			}
			return "{0} կոդով ապրանքը կամ ծառայությունը արդեն արտահանված է b2b.armsoft հարթակ";
		}
	}

	public static string ALTQUNIT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Alternative unit of measure";
				}
				return "Дополнительная единица измерения";
			}
			return "Լրացուցիչ չափման միավոր";
		}
	}

	public static string ALTQUNITShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Alternative U/M";
				}
				return "Дополнительная ед. изм.";
			}
			return "Լրացուցիչ չ/մ";
		}
	}

	public static string AlwaysRoundUp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Always round up";
				}
				return "Всегда округлять в большую сторону";
			}
			return "Կլորացնել միշտ դեպի վերև";
		}
	}

	public static string AmountDue
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount due";
				}
				return "К оплате";
			}
			return "Վճարման ենթակա";
		}
	}

	public static string AnalysSumTypeCur
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In {0} (including VAT)";
				}
				return "{0} (вкл. НДС)";
			}
			return "{0}-ով (ներառյալ ԱԱՀ)";
		}
	}

	public static string AnalysSumTypeNcc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In NCC (without VAT)";
				}
				return "Драм (без НДС)";
			}
			return "Դրամով (Առանց ԱԱՀ)";
		}
	}

	public static string AnalysSumTypeVat
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In NCC (including VAT)";
				}
				return "Драм (вкл. НДС)";
			}
			return "Դրամով (ներառյալ ԱԱՀ)";
		}
	}

	public static string Appendix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enclosure";
				}
				return "Приложение";
			}
			return "Հավելված";
		}
	}

	public static string ArchivationClosingGoodsSecondPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Creating products’ balances";
				}
				return "Регистрируются документы Ввода остатков товаров";
			}
			return "Գրանցվում են Ապրանքների մնացորդների մուտք փաստաթղթերը";
		}
	}

	public static string ArchivationDBCreation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Creating database \"{0}\"";
				}
				return "Создание базы данных \"{0}\"";
			}
			return "\"{0}\" տվյալների պահոցի ստեղծում";
		}
	}

	public static string ArchivationDBNameDuplicate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Database with \"{0}\" name already exists";
				}
				return "\"{0}\" база данных уже существует";
			}
			return "\"{0}\" անվանումով տվյալների պահոց գոյություն ունի";
		}
	}

	public static string ArchivationDeleteDocumentsFistPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting all documents starting {0}";
				}
				return "Удаляются все документы начиная с даты {0}";
			}
			return "Հեռացվում են բոլոր փաստաթղթերը սկսած {0}-ից";
		}
	}

	public static string ArchivationDeleteDocumentsSecondPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting all documents before {0}";
				}
				return "Удаляются все документы до даты {0}";
			}
			return "Հեռացվում են բոլոր փաստաթղթերը մինչև {0}-ը";
		}
	}

	public static string ArchivationDeleteFromACCHIFirstPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting cash desks, banks and partners operations starting {0}";
				}
				return "Удаляются операции кассы, банковских счетов и партнеров начиная с даты {0}";
			}
			return "Հեռացվում են դրամարկղերի, հաշվարկային հաշիվների և գործընկերների գործողությունները {0}-ից սկսած";
		}
	}

	public static string ArchivationDeleteFromACCHISecondPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting cash desks, banks and partners operations before {0}";
				}
				return "Удаляются операции кассы, банковских счетов и партнеров до даты {0}";
			}
			return "Հեռացվում են դրամարկղերի, հաշվարկային հաշիվների և գործընկերների գործողությունները մինչև {0}-ը";
		}
	}

	public static string ArchivationDeleteFromMTHIAndSalesFirstPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting rows starting {0} from Products operations and Sales journals";
				}
				return "Из журнала Операции товаров и услуг и Журнала продаж удаляются строки начиная с даты {0}";
			}
			return "Ապրանքների և ծառայությունների գործողությունների և Վաճառքների մատյաններից հեռացվում են տողերը սկսած {0}-ից";
		}
	}

	public static string ArchivationDeleteFromMTHIAndSalesSecondPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting rows before {0} from Products operations and Sales journals";
				}
				return "Из журнала Операции товаров и услуг и Журнала продаж удаляются строки до даты {0}";
			}
			return "Ապրանքների և ծառայությունների գործողությունների և Վաճառքների մատյաններից հեռացվում են տողերը մինչև {0}-ը";
		}
	}

	public static string ArchivationDeletePartiesFirstPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting materials batches created after {0}";
				}
				return "Удаляются партии созданные до {0}";
			}
			return "Հեռացվում են {0}-ից հետո ստեղծված խմբաքանակները";
		}
	}

	public static string ArchivationDeletingPartiesSecondPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting materials batches consumed as of {0}";
				}
				return "Удаляются партии товаров расходованных на дату {0}";
			}
			return "Հեռացվում են {0}-ի դրությամբ սպառված ապրանքների խմբաքանակները";
		}
	}

	public static string ArchivationExistsDocBasedBeforeClosingDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Checking base documents’ dates";
				}
				return "Проверка даты документов основания";
			}
			return "Հիմք փաստաթղթերի ամսաթվերի ստուգում";
		}
	}

	public static string ArchivationExistsNegativeGoodRemOnClosingDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Checking batches with negative balance";
				}
				return "Проверка партий с отрицательными остатками";
			}
			return "Բացասական մնացորդով խմբաքանակների ստուգում";
		}
	}

	public static string ArchivationFirstDBComment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "First database comment";
				}
				return "Комментарий 1-ой БД";
			}
			return "Առաջին ՏՊ-ի մեկնաբանություն";
		}
	}

	public static string ArchivationFirstDBName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "First database name";
				}
				return "Наименование 1-ой БД";
			}
			return "Առաջին ՏՊ-ի անվանում";
		}
	}

	public static string ArchivationFormingGoodsRemsSecondPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Creating products’ balances";
				}
				return "Формирование остатков товаров";
			}
			return "Ապրանքների մնացորդների ձևավորում";
		}
	}

	public static string ArchivationFormingGoodsRetailRemsSecondPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Creating products’ balances (retail)";
				}
				return "Формирование остатков товаров в розницу";
			}
			return "Ապրանքների մանրածախ մնացորդների ձևավորում";
		}
	}

	public static string ArchivationIndexingSecondPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Indexing database";
				}
				return "Индексация базы данных";
			}
			return "ՏՊ-ի ինդեքսավորում";
		}
	}

	public static string ArchivationResults
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Archivation results";
				}
				return "Результаты архивации";
			}
			return "Արխիվացման արդյունքներ";
		}
	}

	public static string ArchivationSecondDBComment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Second database comment";
				}
				return "Комментарий 2-ой БД";
			}
			return "Երկրորդ ՏՊ-ի մեկնաբանություն";
		}
	}

	public static string ArchivationSecondDBName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Second database name";
				}
				return "Наименование 2-ой БД";
			}
			return "Երկրորդ ՏՊ-ի անվանում";
		}
	}

	public static string ArchivationStart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Archivation start";
				}
				return "Начало архивации";
			}
			return "Արխիվացման սկիզբ";
		}
	}

	public static string ArchivationSuccessful
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Archivation successfully completed";
				}
				return "Архивация прошла успешно";
			}
			return "Արխիվացումն անցավ բարեհաջող";
		}
	}

	public static string ArchivationUpdateAccRestFirstPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Closing cash desks, banks and partners balances";
				}
				return "Закрытие остатков кассы, банковских счетов и партнеров";
			}
			return "Դրամարկղերի, հաշվարկային հաշիվների և գործընկերների մնացորդների փակում";
		}
	}

	public static string Archive
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Archive";
				}
				return "Архивация";
			}
			return "Արխիվացում";
		}
	}

	public static string AssignMTCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Assign to internal product's code";
				}
				return "Создать соответствие с внутренними кодами";
			}
			return "Համապատասխանեցնել ներքին ապրանքին";
		}
	}

	public static string AssignMTMoveInvNums
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Assignment of serial numbers";
				}
				return "Присвоение серии и номера налоговой ТТН";
			}
			return "Սերիա-համարների վերագրում";
		}
	}

	public static string AssignNewBarcode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Assign new barcode";
				}
				return "Назначить новый штрихкод";
			}
			return "Վերագրել նոր գծիկավոր կոդ";
		}
	}

	public static string AssignNewBarcodeCompleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New barcode assignment completed. Number of assigned codes: {0}";
				}
				return "Присвоение новых штрихкодов завершена. Количество присвоенных кодов {0}";
			}
			return "Նոր գծիկավոր կոդերի վերագրումն ավարտվեց: Վերագրված գծ. կոդերի քանակը` {0}";
		}
	}

	public static string ASTrade7TrainingLessons
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online Course - 18 lessons";
				}
				return "Учебный курс - 18 уроков";
			}
			return "ՏԵՍԱԴԱՍԸՆԹԱՑ - 18 դաս";
		}
	}

	public static string ASTradeUser
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "AS-Trade User";
				}
				return "AS-Trade Пользователь";
			}
			return "ՀԾ-Առևտուր համակարգի օգտագործող";
		}
	}

	public static string Attr1EmptyButAttr2HasValue
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property 1 empty, but Property 2 has value";
				}
				return "Значение поля Свойство 2 заполнено, в то время как значение поля Свойство 1 не задано.";
			}
			return "Հատկություն 1 դատարկ է, սակայն հատկություն 2 ունի արժեք";
		}
	}

	public static string Attr1EqualAttr2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Types of Property 1 and Property 2 are the same";
				}
				return "Типы свойств 1 и 2 совпадают";
			}
			return "Հատկություն 1-ի և հատկություն 2-ի տեսակները համընկնում են";
		}
	}

	public static string Attribute
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property";
				}
				return "Свойство";
			}
			return "Հատկություն";
		}
	}

	public static string Attribute1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property 1";
				}
				return "Свойство 1";
			}
			return "Հատկություն 1";
		}
	}

	public static string Attribute2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property 2";
				}
				return "Свойство 2";
			}
			return "Հատկություն 2";
		}
	}

	public static string AttributeInMaterials
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} property cannot be deleted";
				}
				return "Свойство \"{0}\" нельзя удалить";
			}
			return "\"{0}\" հատկությունը չի կարելի հեռացնել";
		}
	}

	public static string AttributeType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property type";
				}
				return "Тип свойства";
			}
			return "Հատկության տեսակ";
		}
	}

	public static string AttributeType1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property type 1";
				}
				return "Тип свойства 1";
			}
			return "Հատկության տեսակ 1";
		}
	}

	public static string AttributeType2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property type 2";
				}
				return "Тип свойства 2";
			}
			return "Հատկության տեսակ 2";
		}
	}

	public static string AttributeValue
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property value";
				}
				return "Значение свойства";
			}
			return "Հատկության արժեք";
		}
	}

	public static string AutoForming
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Form prices by formula";
				}
				return "Формировать цены с помощью формулы";
			}
			return "Ձևավորել գները բանաձևի միջոցով";
		}
	}

	public static string AvailabilityAndTurn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Availability and turnover";
				}
				return "Остатки и оборот";
			}
			return "Առկայություն և շարժ";
		}
	}

	public static string Available
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Available";
				}
				return "Доступен";
			}
			return "Հասանելի է";
		}
	}

	public static string AvailAndTurn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Avail. and turn.";
				}
				return "Ост. и оборот";
			}
			return "Առկ. և շարժ";
		}
	}

	public static string AverageQuantityOfRows
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Average quantity of rows";
				}
				return "Среднее количество по строке";
			}
			return "Տողերի միջին քանակը";
		}
	}

	public static string AverageSumOfBills
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Average sum of bills";
				}
				return "Средняя сумма по чеку";
			}
			return "Կտրոնների միջին գումարը";
		}
	}

	public static string AverageSumOfRows
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Average sum of rows ";
				}
				return "Средняя сумма по строке";
			}
			return "Տողերի միջին գումարը";
		}
	}

	public static string AvgWeigtItemsRevaluatedSeparately
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products with average expense method must be revaluated separately.";
				}
				return "Товары с средневзвешенным учетом переоцениваются отдельно";
			}
			return "Միջին կշռված գնով հաշվառվող ապրանքները վերագնահատվում են առանձին";
		}
	}

	public static string b2bProductComparisonMaterialFromB2BNotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product from b2b.armsoft.am is not found in internal directory";
				}
				return "Товар из b2b.armsoft не обнаружен во внутреннем справочнике";
			}
			return "Ապրանքը b2b.armsoft հարթակից գտնված չէ ներքին տեղեկատուում";
		}
	}

	public static string b2bProductComparisonMaterialNotFoundInB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product is not found in b2b.armsoft.am products";
				}
				return "Товар не обнаружен в списке товаров на b2b.armsoft";
			}
			return "Ապրանքը գտնված չէ b2b.armsoft հարթակի ապրանքների ցուցակում";
		}
	}

	public static string b2bProductComparisonServiceFromB2BNotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Service from b2b.armsoft.am is not found in services";
				}
				return "Услуга из b2b.armsoft не обнаружена во внутреннем справочнике";
			}
			return "Ծառայությունը b2b.armsoft հարթակից գտնված չէ";
		}
	}

	public static string b2bProductComparisonServiceNotFoundInB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Service is not found in b2b.armsoft.am services";
				}
				return "Услуга не обнаружена в списке услуг на b2b.armsoft";
			}
			return "Ծառայությունը գտնված չէ b2b.armsoft հարթակում";
		}
	}

	public static string BackOffice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Central DB";
				}
				return "Центральная БД";
			}
			return "Կենտրոնական ՏՊ";
		}
	}

	public static string Bank
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank";
				}
				return "Банк";
			}
			return "Բանկ";
		}
	}

	public static string BankAccDuplicate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Settlement account repetition in the table";
				}
				return "Повтор банковского счета в таблице";
			}
			return "Աղյուսակում հաշվարկային հաշվի կրկնություն";
		}
	}

	public static string BankAccHaveEntry
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are operations with {0} settlement account";
				}
				return "С банковским счетом \"{0}\" существуют операции.";
			}
			return "\"{0}\" հաշվարկային հաշվով գոյություն ունեն գործողություններ";
		}
	}

	public static string BankDocumentsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank documents journal";
				}
				return "Банковские документы";
			}
			return "Բանկային փաստաթղթեր";
		}
	}

	public static string BankIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank input";
				}
				return "Поступление на банк. счет";
			}
			return "Բանկի մուտք";
		}
	}

	public static string BankNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{1} bank code selected in {0} partner's bank account is missing from \"Banks\" directory";
				}
				return "Банк \"{1}\" указанный в банковском счете партнера \"{0}\" отсутствует в справочнике \"Банки\"";
			}
			return "\"{0}\" գործընկերոջ հաշվարկային հաշվում նշված \"{1}\" բանկը բացակայում է \"Բանկեր\" տեղեկատուից";
		}
	}

	public static string BankOp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank accounts' operations";
				}
				return "Операции по банковским счетам";
			}
			return "Բանկային հաշիվների գործողություններ";
		}
	}

	public static string BankOpJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank accounts operations journal";
				}
				return "Банковские операции";
			}
			return "Բանկային հաշիվների գործողությունների մատյան";
		}
	}

	public static string BankOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank output";
				}
				return "Списание с банк. счета";
			}
			return "Բանկի ելք";
		}
	}

	public static string BankRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank accounts balances";
				}
				return "Остатки на банковских счетах";
			}
			return "Բանկային հաշիվների մնացորդներ";
		}
	}

	public static string BanksTAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Banks T-account";
				}
				return "Карточки операций банковских счетов";
			}
			return "Բանկային հաշիվների քարտ";
		}
	}

	public static string BankTAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank T-account";
				}
				return "Карточка операций банковского счета";
			}
			return "Բանկային հաշվի քարտ";
		}
	}

	public static string BankTurn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank accounts turnover journal";
				}
				return "Обороты банковских счетов";
			}
			return "Բանկային հաշիվների շրջանառություն";
		}
	}

	public static string BarCodeAndCodeInputNullText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input barcode and press Enter or input code and press Code (F8)";
				}
				return "Введите штрих код и нажмите на Enter, или код товара и нажмите (F8)";
			}
			return "Մուտքագրեք գծ․ կոդը և սեղմեք Enter կամ ապրանքի կոդը և Կոդ (F8) կոճակը";
		}
	}

	public static string BarCodeNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product or service with the specified {0} barcode does not exist.";
				}
				return "Товара или услуги с штрих кодом {0} не существует";
			}
			return "{0} գծիկավոր կոդով ապրանք կամ ծառայություն գոյություն չունի:";
		}
	}

	public static string BarCodesNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No product found for this barcode.\r\nBarcodes` {0}";
				}
				return "Товар со штрихкодом не найден.\r\nШтрихкоды` {0}";
			}
			return "Գծիկավոր կոդով ապրանք չի հայտնաբերվել։\r\nԳծիկավոր կոդեր` {0}";
		}
	}

	public static string Base
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "base";
				}
				return "основание";
			}
			return "հիմք";
		}
	}

	public static string BaseDocNotExistByDocType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The base document for \"{0}\" is missing from the document's journal\r\n(internal number {1})";
				}
				return "Основание документа \"{0}\" отсутствует в журнале документов (внутренний номер {1})";
			}
			return "\"{0}\" փաստաթղթի հիմք փաստաթուղթը բացակայում է փաստաթղթերի մատյանից\r\n(ներքին համար {1})";
		}
	}

	public static string BaseDocNotInDocs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base document is missing";
				}
				return "Документ основания отсутствует";
			}
			return "Հիմք փաստաթուղթը բացակայում է";
		}
	}

	public static string BaseDocument
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base document";
				}
				return "Документ основания";
			}
			return "Հիմք փաստաթուղթ";
		}
	}

	public static string BaseECRCheckDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base ECR Checks";
				}
				return "Чеки основания";
			}
			return "Հիմք կտրոններ";
		}
	}

	public static string BasePriceCurrVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base price type:   \r\n   {0}\r\n    ({1}{2})";
				}
				return "Базовые цены:\r\n{0}\r\n({1}{2})";
			}
			return "Հիմքային գնի տեսակ`\r\n     {0}\r\n     ({1}{2})";
		}
	}

	public static string BaseQUNIT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base unit of measure";
				}
				return "Основная единица измерения";
			}
			return "Հիմնական չափման միավոր";
		}
	}

	public static string BaseQUNITShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base U/M";
				}
				return "Основная ед. изм.";
			}
			return "Հիմնական չ/մ";
		}
	}

	public static string BeforContinueStoreOrDefer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Before proceeding with the sale, you must register or put on hold current sale";
				}
				return "Перед продолжением продажи нужно сохранить или отменить текущую продажу";
			}
			return "Վաճառքը շարունակելուց առաջ անհրաժեշտ է ընթացիկ վաճառքը գրանցել կամ հետաձգել";
		}
	}

	public static string BeforExitStoreOrDefer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Before closing the sale, you must register or put on hold current sale";
				}
				return "Перед закрытием продажи нужно сохранить или отложить текущую продажу";
			}
			return "Վաճառքը փակելուց առաջ անհրաժեշտ է ընթացիկ վաճառքը գրանցել կամ հետաձգել";
		}
	}

	public static string BeforReturnStoreOrDefer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Before registering the return, you must register or put on hold current sale";
				}
				return "Для формирования возврата нужно сохранить или отменить текущую продажу";
			}
			return "Վերադարձ գրանցելու համար անհրաժեշտ է ընթացիկ վաճառքը գրանցել կամ հետաձգել";
		}
	}

	public static string BeforSaleStoreOrDefer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Before registering the sale, you must register or put on hold current return";
				}
				return "Перед вводом документа возврата необходимо провести или отложить текущую продажу";
			}
			return "Վաճառք գրանցելու համար անհրաժեշտ է ընթացիկ վերադարձը գրանցել կամ հետաձգել";
		}
	}

	public static string BigDifference
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Difference cannot be greater than {0} drams";
				}
				return "Разница не может быть больше {0} драм";
			}
			return "Տարբերությունը չի կարող մեծ լինել {0} դրամից";
		}
	}

	public static string Bonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus ({0})";
				}
				return "Бонус ({0})";
			}
			return "Բոնուս ({0})";
		}
	}

	public static string BonusAccumulation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Accumulation of bonuses";
				}
				return "Накопление бонуса";
			}
			return "Բոնուսի կուտակում";
		}
	}

	public static string BonusAccumulationBase
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus earning’s condition";
				}
				return "Условие начисления";
			}
			return "Կուտակման պայման ";
		}
	}

	public static string BonusAccumulationBaseAccumulatedSalesAmount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Accumulated sales amount";
				}
				return "Сумма накопленных продаж";
			}
			return "Կուտակված վաճառքների գումար";
		}
	}

	public static string BonusAccumulationBaseOneTimeSaleSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "One-time sale amount";
				}
				return "Сумма документа продажи";
			}
			return "Միանվագ վաճառքի գումար";
		}
	}

	public static string BonusAccumulationBases
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus earning’s conditions";
				}
				return "Условие начисления";
			}
			return "Կուտակման պայմաններ ";
		}
	}

	public static string BonusAccumulationBaseWithoutCondition
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Without condition";
				}
				return "Без условий ";
			}
			return "Առանց պայմանի";
		}
	}

	public static string BonusAccumulationConditionPeriod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus earning’s condition - period";
				}
				return "Условие начисления - Период";
			}
			return "Կուտակման  պայման - Ժամանակահատված";
		}
	}

	public static string BonusAccumulationConditionSumGreater
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus earning’s condition - amount is greater";
				}
				return "Условие начисления – Сумма большая";
			}
			return "Կուտակման  պայման - Արժեքը մեծ է";
		}
	}

	public static string BonusAccumulationMethod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus earning’s method";
				}
				return "Метод начисления";
			}
			return "Կուտակման եղանակ";
		}
	}

	public static string BonusAccumulationMethodPartnerCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use partner’s card %";
				}
				return "Применить % карты партнера";
			}
			return "Գործընկերոջ քարտի %-ի  կիրառում";
		}
	}

	public static string BonusAccumulationMethods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus earning’s methods";
				}
				return "Методы начисления";
			}
			return "Կուտակման եղանակներ";
		}
	}

	public static string BonusAccumulationMethodScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use scale";
				}
				return "Применить шкалу";
			}
			return "Սանդղակի կիրառում";
		}
	}

	public static string BonusAccumulationMethodSimple
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Simple (use of selected % or point)";
				}
				return "Простой (указанный % или бонусный балл)";
			}
			return "Ապրանքի համար նշված %-ի կամ միավորի կիրառում";
		}
	}

	public static string BonusCalcValidate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For calculating bonus, no bonus redemption should be done";
				}
				return "Для расчета бонуса необходимо чтобы не было погашения бонусом";
			}
			return "Բոնուս հաշվարկելու համար անհրաժեշտ է, որ  բոնուսով մարում չլինի կատարված";
		}
	}

	public static string BonusCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus card";
				}
				return "Бонусная карта";
			}
			return "Բոնուսային քարտ";
		}
	}

	public static string BonusCardCanceled
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Canceled";
				}
				return "Аннулирована";
			}
			return "Կասեցված";
		}
	}

	public static string BonusCardMustBeNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contract with foreign currency cannot be bonus Card";
				}
				return "Валютный договор не может быть бонусной картой";
			}
			return "Արտարժութային պայմանագիրը չի կարող հանդիսանալ բոնուսային քարտ";
		}
	}

	public static string BonusCardNotFoundInDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The card is not found in the Discount server's cards directory";
				}
				return "Карта отсутствует в справочнике \"Карты Бонусного сервера\"";
			}
			return "Քարտը բացակայում է Բոնուսային սերվերի քարտերի տեղեկատուից";
		}
	}

	public static string BonusCards
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus Cards";
				}
				return "Бонусные карты";
			}
			return "Բոնուսային քարտեր";
		}
	}

	public static string BonusCardValidationPeriod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Validation Period";
				}
				return "Период действия ";
			}
			return "Վավերականության ժամկետ";
		}
	}

	public static string Bonuses
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonuses";
				}
				return "Бонусы";
			}
			return "Բոնուսներ";
		}
	}

	public static string BonusesLabelForPrint
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "“Bonus” field name";
				}
				return "Название поля «Бонус»";
			}
			return "\"Բոնուս\" դաշտի անվանում";
		}
	}

	public static string BonusGroupCorrect
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonuses group revision";
				}
				return "Групповая корректировка бонуса";
			}
			return "Բոնուսների խմբային վերանայում";
		}
	}

	public static string BonusMustBeZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You cannot add bonus (% or point) on non-bonus card";
				}
				return "Ни в бонусной карте нельзя отметить бонус (% или единица)";
			}
			return "Ոչ բոնուսային քարտի վրա հնարավոր չէ նշել բոնուս (% կամ միավոր)";
		}
	}

	public static string BonusOnly
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus";
				}
				return "Бонус";
			}
			return "Բոնուս";
		}
	}

	public static string BonusOp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonuses operations";
				}
				return "Операции по бонусам";
			}
			return "Բոնուսների գործողություններ";
		}
	}

	public static string BonusOperationAccumulate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Accumulate";
				}
				return "Накопление";
			}
			return "Կուտակում";
		}
	}

	public static string BonusOperationCorrection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Correction";
				}
				return "Исправление";
			}
			return "Վերանայում";
		}
	}

	public static string BonusOperationUse
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use";
				}
				return "Использование";
			}
			return "Մարում";
		}
	}

	public static string BonusOpJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonuses operations journal";
				}
				return "Операции бонуса";
			}
			return "Բոնուսների գործողությունների մատյան";
		}
	}

	public static string BonusPassiveRem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{1}\" bonus contract’s balance of  \"{0}\"  partner cannot be negative";
				}
				return "Остаток бонусного договора \"{1}\" у партнера \"{0}\" не может быть отрицательной";
			}
			return "\"{0}\" գործընկերոջ \"{1}\" բոնուսային պայմանագրի մնացորդը չի կարող լինել բացասական";
		}
	}

	public static string BonusPlusCashOverflow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The amount of bonus payments {0} , cash and plastic card paymnets must not exceed the total amount of return";
				}
				return "Сумма выплаты бонусом {0}, наличным и безналичным, не должна превышать общую сумму возврата";
			}
			return "Բոնուսներով մարման {0} , կանխիկի և անկանխիկի գումարը չպետք է գերազանցի վերադարձի ընդհանուր գումարը";
		}
	}

	public static string BonusPointsEndRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Points closing balance";
				}
				return "Конечный остаток единиц";
			}
			return "Միավորների վերջնական մնացորդ";
		}
	}

	public static string BonusPointsIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Points earning";
				}
				return "Начисления баллов";
			}
			return "Միավորների աճ";
		}
	}

	public static string BonusPointsInitRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Points initial balance";
				}
				return "Начальный остаток единиц";
			}
			return "Միավորների սկզբնական մնացորդ";
		}
	}

	public static string BonusPointsOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Points redemption";
				}
				return "Списания баллов";
			}
			return "Միավորների նվազում";
		}
	}

	public static string BonusPointsRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Points balance";
				}
				return "Остаток единиц";
			}
			return "Միավորների մնացորդ";
		}
	}

	public static string BonusRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus balances";
				}
				return "Остатки по бонусам";
			}
			return "Բոնուսների մնացորդներ";
		}
	}

	public static string BonusReports
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus reports";
				}
				return "Отчеты по бонусам";
			}
			return "Բոնուսների հաշվետվություններ";
		}
	}

	public static string BonusRestore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus restoration";
				}
				return "Восстановление бонуса";
			}
			return "Բոնուսի վերականգնում";
		}
	}

	public static string BonusScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus scale";
				}
				return "Бонусная шкала";
			}
			return "Բոնուսային սանդղակ";
		}
	}

	public static string BonusTAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus t-account";
				}
				return "Карточка бонуса";
			}
			return "Բոնուսների գործողությունների քարտ";
		}
	}

	public static string BonusTransactions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonuses operations journal";
				}
				return "Операции с бонусами";
			}
			return "Բոնուսային քարտերով գործողությունների մատյան";
		}
	}

	public static string BonusTurnOver
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonuses turnover journal";
				}
				return "Обороты по бонусам";
			}
			return "Բոնուսների շրջանառություն";
		}
	}

	public static string BonusType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus type";
				}
				return "Тип бонуса";
			}
			return "Բոնուսի տեսակ";
		}
	}

	public static string BonusTypeByPercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By percentage";
				}
				return "Процентом";
			}
			return "Տոկոսային";
		}
	}

	public static string BonusTypeBySum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By amount";
				}
				return "Суммой";
			}
			return "Գումարային";
		}
	}

	public static string BonusTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus types";
				}
				return "Типы бонусов";
			}
			return "Բոնուսի տեսակներ";
		}
	}

	public static string BonusUse
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus redemption";
				}
				return "Погашение бонуса";
			}
			return "Բոնուսի մարում";
		}
	}

	public static string BonusUseMaxPercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Maximum limit for bonus repayment";
				}
				return "Максимальный лимит оплаты бонусом";
			}
			return "Բոնուսով մարելու առավելագույն սահման";
		}
	}

	public static string BonusUseShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus red.";
				}
				return "Пог. бонуса";
			}
			return "Բոն. մարում";
		}
	}

	public static string ButtonSelectedGoods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products";
				}
				return "товар.";
			}
			return "ապրնք.";
		}
	}

	public static string BuyerAddedFromPOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Buyer (Added from POS)";
				}
				return "Покупатель (Добавлено из POS)";
			}
			return "Գնորդ (Ավելացված է POS-ից)";
		}
	}

	public static string Buyers
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Buyer's";
				}
				return "Бон./Скд.";
			}
			return "Գնորդի";
		}
	}

	public static string BuyingPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Purchasing price type";
				}
				return "Тип цен покупки";
			}
			return "Գնման գնի տեսակ";
		}
	}

	public static string ByCardPOSLabel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-cash (POS-terminal)";
				}
				return "Безнал. (POS-терминал)";
			}
			return "Անկանխիկ (POS-տերմինալ)";
		}
	}

	public static string ByDifPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By different sales prices";
				}
				return "По разным ценам продажи";
			}
			return "Ըստ տարբեր վաճառքի գների";
		}
	}

	public static string ByScaleFormat
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By el. scale format";
				}
				return "В формате эл. весов";
			}
			return "Էլ. կշեռքի ֆորմատով";
		}
	}

	public static string ByTerminalFormat
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By terminal format";
				}
				return "В формате ТСД (терминала сбора данных)";
			}
			return "Տերմինալի ֆորմատով";
		}
	}

	public static string CalcBonuses
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Calculate bonuses";
				}
				return "Рассчитать бонусы";
			}
			return "Հաշվարկել բոնուսները";
		}
	}

	public static string CalcBonusesShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Earned Bonus";
				}
				return "Расчит. бон.";
			}
			return "Հաշվ. բոն.";
		}
	}

	public static string CalcDiscountAuto
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Auto discount the amount by registering new sale";
				}
				return "Во время продажи скидку рассчитать автоматически";
			}
			return "Նոր վաճառք գրանցելուց գումարը զեղչել ավտոմատ";
		}
	}

	public static string Cancelled
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cancelled rows";
				}
				return "Cancelled rows";
			}
			return "Չեղարկված տողեր";
		}
	}

	public static string CantDeleteAlt
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Alternative unit of measure is not allowed to delete";
				}
				return "Дополнительную единицу измерения нельзя удалять";
			}
			return "Լրացուցիչ չափման միավորը չի կարելի հեռացնել";
		}
	}

	public static string CantDeleteDef
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} cannot be deleted";
				}
				return "{0} нельзя удалять";
			}
			return "{0}ը չի կարելի հեռացնել";
		}
	}

	public static string CantDeleteProperty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} property has values";
				}
				return "\"{0}\" свойство имеет значения";
			}
			return "\"{0}\" հատկությունը ունի արժեքներ";
		}
	}

	public static string CantEditQntUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} unit of measure cannot be changed";
				}
				return "{0} единицу измерения нельзя изменять";
			}
			return "{0} չափման միավորը չի կարելի փոփոխել";
		}
	}

	public static string Card
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Card";
				}
				return "карта";
			}
			return "քարտ";
		}
	}

	public static string CardCapitalLetter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Card";
				}
				return "Карта";
			}
			return "Քարտ";
		}
	}

	public static string CardHolder
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cardholder";
				}
				return "Держатель карты";
			}
			return "Քարտապան";
		}
	}

	public static string CashBank
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash/Bank";
				}
				return "Касса и Банк";
			}
			return "Դրամարկղ/Բանկ";
		}
	}

	public static string CashBankReports
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash/Bank Reports";
				}
				return "Отчеты по кассе/банку";
			}
			return "Դրամարկղի/Բանկի հաշվետվություններ";
		}
	}

	public static string CashDeskCashFlow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash in desk";
				}
				return "Денежные потоки кассы";
			}
			return "Կանխիկը դրամարկղում";
		}
	}

	public static string CashDeskIsNotAccessible
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" cashdesk is not accessible";
				}
				return "\"{0}\" касса недоступна";
			}
			return "\"{0}\" դրամարկղը հասանելի չէ Ձեր օգտագործողին";
		}
	}

	public static string CashDeskNotAvailForUser
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" user does not have access to \"{1}\" cash desk";
				}
				return "Пользователю \"{0}\" доступ к кассе \"{1}\" не разрешен";
			}
			return "\"{0}\" օգտագործողին \"{1}\" դրամարկղը հասանելի չէ";
		}
	}

	public static string CashDesksTAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks T-account";
				}
				return "Карточка касс";
			}
			return "Դրամարկղերի քարտ";
		}
	}

	public static string CashDeskTAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desk T-account";
				}
				return "Карточка кассы";
			}
			return "Դրամարկղի քարտ";
		}
	}

	public static string CashDocumentsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash documents journal";
				}
				return "Кассовые документы";
			}
			return "Դրամական փաստաթղթեր";
		}
	}

	public static string CashesAccess
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Access to Cash desks";
				}
				return "Настройки прав пользователя по кассам";
			}
			return "Դրամարկղերի հասանելիություն";
		}
	}

	public static string CashHaveEntry
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are operations with {0} cash desk";
				}
				return "С кассой \"{0}\" существуют операции";
			}
			return "\"{0}\" դրամարկղով գոյություն ունեն գործողություններ";
		}
	}

	public static string CashierDisplayOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashier POS options";
				}
				return "Параметры рабочего места продавца";
			}
			return "Վաճառողի աշխատատեղի պարամետրեր";
		}
	}

	public static string CashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shift";
				}
				return "Смена";
			}
			return "Հերթափոխ";
		}
	}

	public static string CashierShiftClose
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shift close";
				}
				return "Закрытие смены";
			}
			return "Հերթափոխի փակում";
		}
	}

	public static string CashierShiftEnd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shift end";
				}
				return "Конец смены";
			}
			return "Հերթափոխի ավարտ";
		}
	}

	public static string CashierShiftForExportNotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashier shift for export is not found";
				}
				return "Смены, подлежащие обмену, не обнаружены";
			}
			return "Փոխանակման ենթակա հերթափոխ չի հայտնաբերվել";
		}
	}

	public static string CashierShiftNextNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shift number";
				}
				return "Порядковый номер смены";
			}
			return "Հերթափոխի հերթական համար";
		}
	}

	public static string CashierShiftNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is not \"{0}\" cashier shift with \"{1}\" cash desk";
				}
				return "Номер смены \"{0}\" кассы \"{1}\" не существует";
			}
			return "\"{1}\" դրամարկղով  \"{0}\" հերթափոխ չկա";
		}
	}

	public static string CashierShiftNotExistWithDefaultCashdesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is no open cashier shift with user's default \"{0}\" cashdesk";
				}
				return "Смена не открыта для кассы \"{0}\" указанного по умолчанию";
			}
			return "Օգտագործողի լռությամբ \"{0}\" դրամարկղով բաց հերթափոխ չկա";
		}
	}

	public static string CashierShiftNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashier shift N";
				}
				return "Смена N";
			}
			return "Հերթափոխի N";
		}
	}

	public static string CashierShiftNumShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shift N";
				}
				return "Смена N";
			}
			return "Հերթ. N";
		}
	}

	public static string CashierShiftReport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashier shift report";
				}
				return "Отчет смены";
			}
			return "Հերթափոխի հաշվետվություն";
		}
	}

	public static string CashierShifts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shifts";
				}
				return "Смены";
			}
			return "Հերթափոխեր";
		}
	}

	public static string CashierShiftStart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shift start";
				}
				return "Начало смены";
			}
			return "Հերթափոխի սկիզբ";
		}
	}

	public static string CashierShiftWasClosedSuccessfully
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashiers shift with \"{0}\" cash desk and \"{1}\" number was closed";
				}
				return "Смена \"{1}\" для кассы \"{0}\" закрыта";
			}
			return "\"{0}\" Դրամարկղով և \"{1}\" համարով հերթափոխը  փակվեց";
		}
	}

	public static string CashierShiftWasOpenedSuccessfully
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashiers shift with \"{0}\" cash desk and \"{1}\" number was opened";
				}
				return "Смена \"{1}\" для кассы \"{0}\" открыта";
			}
			return "\"{0}\" Դրամարկղով և \"{1}\" համարով հերթափոխը բացվեց";
		}
	}

	public static string CashInCashierShiftInCashDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In \"{0}\" cashdesk available cash amount in the beginning of shift = {1}";
				}
				return "Сумма остатка в кассе \"{0}\"  \r\nна начало смены = {1}";
			}
			return "\"{0}\" դրամարկղում հերթափոխի սկզբի առկա \r\nկանխիկ գումարը  = {1}";
		}
	}

	public static string CashInCreatedNotification
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashin with {0} document number is registered";
				}
				return "Приходный ордер с номером {0} зарегистрирован";
			}
			return "{0} համարով ԴՄՕ փաստաթուղթը գրանցված է";
		}
	}

	public static string CashInDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input cash desk";
				}
				return "В кассу";
			}
			return "Մուտքի դրամարկղ";
		}
	}

	public static string CashInEndOfCashierShiftInCashDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In \"{0}\" cashdesk available cash amount in the end of shift = {1}";
				}
				return "Сумма остатка в кассе \"{0}\"  \r\nна конец смены = {1}";
			}
			return "\"{0}\" դրամարկղում հերթափոխի վերջում առկա \r\nկանխիկ գումարը  = {1}";
		}
	}

	public static string CashInErrSameCashDesks
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input and output cash desks must be different.";
				}
				return "Касса прихода должна отличаться от кассы расхода ";
			}
			return "Մուտքի և ելքի դրամարկղերը պետք է լինեն տարբեր";
		}
	}

	public static string CashInOpTypeByPlCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-cash purchase (Prepayment ECR check printing)";
				}
				return "Оплата пластиковой картой (печать чека ККМ на безнал. аванс)";
			}
			return "Պլաստիկ քարտով վճարում (կանխավճարի ՀԴՄ կտրոնի տպում)";
		}
	}

	public static string OpTypeFromPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash input from partner";
				}
				return "Оплата от покупателя";
			}
			return "Գումարի մուտքագրում գործընկերոջից";
		}
	}

	public static string OpTypeFromSettlAcc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash input from bank account";
				}
				return "Получение денежных средств с банковского счета";
			}
			return "Գումարի մուտքագրում բանկային հաշվից";
		}
	}

	public static string CashInOpTypeRevenueInput
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Retail revenue input";
				}
				return "Розничная выручка";
			}
			return "Գումարի մուտքագրում այլ դրամարկղից";
		}
	}

	public static string OpType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operation type";
				}
				return "Вид операции";
			}
			return "Գործ. տեսակ";
		}
	}

	public static string CashOutDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Output cash desk";
				}
				return "Из кассы";
			}
			return "Ելքի դրամարկղ";
		}
	}

	public static string CashOutOpTypeAmountOutput
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash out to retail point";
				}
				return "Выдача денежных средств в розницу";
			}
			return "Գումարի ելք դեպի այլ դրամարկղ";
		}
	}

	public static string OpTypeToPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash payment to partner";
				}
				return "Оплата поставщику";
			}
			return "Գումարի վճարում գործընկերոջը";
		}
	}

	public static string OpTypeToSettlAcc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash output to bank account";
				}
				return "Передача денежных средств на банковский счет";
			}
			return "Գումարի ելք դեպի բանկային հաշիվ";
		}
	}

	public static string CashPayment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By cash (F6)";
				}
				return "Наличные (F6)";
			}
			return "Կանխիկ (F6)";
		}
	}

	public static string CashPaymentForPrint
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash";
				}
				return "Наличные";
			}
			return "Կանխիկ";
		}
	}

	public static string CashShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash";
				}
				return "Налич.";
			}
			return "Կանխ․";
		}
	}

	public static string CategorizationCriteria
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Classification criteria";
				}
				return "Показатель классификации";
			}
			return "Դասակարգման ցուցանիշ";
		}
	}

	public static string Category
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Category";
				}
				return "Классификация";
			}
			return "Դաս";
		}
	}

	public static string CBInvalidDocPasses
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wrong document";
				}
				return "Неверный документ";
			}
			return "Սխալ փաստաթուղթ";
		}
	}

	public static string ChangeActualnessDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transfer reliability date";
				}
				return "Управление датой актуальности";
			}
			return "Տեղափոխել հավաստիության ամսաթիվը";
		}
	}

	public static string ChangeActualnessDateSucc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reliability date transfer completed successfully";
				}
				return "Перенос даты актуальности прошел успешно!";
			}
			return "Հավաստիության ամսաթվի տեղափոխումը ավարտվեց բարեհաջող";
		}
	}

	public static string ChangeMTDecomplectationRowID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reverse kitting document RowID column value configuration";
				}
				return "Регулировка значений колонки RowID документа Декомплектации";
			}
			return "Ապակոմպլեկտավորում փաստաթղթերի RowID սյան արժեքների կարգավորում";
		}
	}

	public static string ChangeMTDecomplectationRowIDError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Failed to configure values of RowID column in Reverse kitting document.";
				}
				return "Не удалось отрегулировать значения колонки RowID документа Декомплектации";
			}
			return "Չհաջողվեց կարգավորել Ապակոմպլեկտավորում փաստաթղթերի RowID սյան արժեքները";
		}
	}

	public static string ChangeNewPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Form new prices";
				}
				return "Сформировать новые цены";
			}
			return "Ձևավորել նոր գները";
		}
	}

	public static string ChangeStorageValues
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Change values in Storage column?";
				}
				return "Заменить значения в колонке Склад?";
			}
			return "Փոխե՞լ պահեստ դաշտի արժեքները";
		}
	}

	public static string ChangingActualnessDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reliability date change";
				}
				return "Перенос даты актуальности";
			}
			return "Հավաստիության տեղափոխում";
		}
	}

	public static string ChangingMTHIRowIDs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ChangingMTHIRowIDs";
				}
				return "Идет процесс изменения значений колонки RowID таблицы MTHI";
			}
			return "Փոփոխվում են MTHI աղյուսակի RowID սյան արժեքները";
		}
	}

	public static string ChangingMTHITrans
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ChangingMTHITrans";
				}
				return "Идет процесс изменения значений колонки Trans таблицы MTHI";
			}
			return "Փոփոխվում են MTHI աղյուսակի Trans սյան արժեքները";
		}
	}

	public static string ChangingMTOverheadExpDocsBaseISN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Changing base documents of \"Allocation of overhead expenses\" documents";
				}
				return "Идет процесс изменения документов оснований для документа Распределение доп. расходов";
			}
			return "Փոփոխվում են ՀԾԲ փաստաթղթերի հիմք փաստաթղթերը";
		}
	}

	public static string ChartRowCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Number of rows in the chart";
				}
				return "Количество строк, показанных в диаграмме";
			}
			return "Դիագրամում ցուցադրվող տողերի թիվը";
		}
	}

	public static string Check
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check";
				}
				return "Чек";
			}
			return "Կտրոն";
		}
	}

	public static string CheckAccRestIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Account's balance integrity checking";
				}
				return "Проверка целостности реестра Остатки (Партнеры, Касса, Банк)";
			}
			return "Հաշիվների մնացորդների ամբողջականության ստուգում";
		}
	}

	public static string CheckConnection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check connection";
				}
				return "Проверка соединения";
			}
			return "Կապի ստուգում";
		}
	}

	public static string CheckContractsIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contracts integrity checking";
				}
				return "Проверка целостности подчиненного справочника Договоры партнеров";
			}
			return "Պայմանագրերի ամբողջականության ստուգում";
		}
	}

	public static string CheckCurrExchangeIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Currency exchange integrity checking";
				}
				return "Проверка целостности справочника Курсы валют";
			}
			return "Արտարժույթի փոխարժեքների ամբողջականության ստուգում";
		}
	}

	public static string CheckListsIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Integrity checking  of Storage documents lists";
				}
				return "Проверка целостности Табличных элементов складских документов ";
			}
			return "Պահեստի փաստաթղթերի ցուցակների ամբողջականության ստուգում";
		}
	}

	public static string CheckMaterialIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services integrity checking";
				}
				return "Проверка целостности справочника Товары и услуги";
			}
			return "Ապրանքների և ծառայությունների ամբողջականության ստուգում";
		}
	}

	public static string CheckMTBarCodesIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services barcodes integrity checking ";
				}
				return "Проверка целостности штрихкодов товаров и услуг";
			}
			return "Ապրանքների և ծառայությունների գծ. կոդերի ամբողջականության ստուգում";
		}
	}

	public static string CheckMTComplectsIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kits' integrity checking";
				}
				return "Проверка целостности справочника Комплекты";
			}
			return "Կոմպլեկտների ամբողջականության ստուգում";
		}
	}

	public static string CheckMTGroupIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services groups integrity checking";
				}
				return "Проверка целостности справочника Группы товаров и услуг";
			}
			return "Ապրանքների և ծառայությունների խմբերի ամբողջականության ստուգում";
		}
	}

	public static string CheckMTQntUnitsIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services units of measures integrity checking";
				}
				return "Проверка целостности подчиненного справочника Единицы измерения товаров и услуг";
			}
			return "Ապրանքների և ծառայությունների չափման միավորների ամբողջականության ստուգում";
		}
	}

	public static string CheckPartiesIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Batches integrity check";
				}
				return "Проверка целостности справочника Партии товаров";
			}
			return "Խմբաքանակների ամբողջականության ստուգում";
		}
	}

	public static string CheckPricesIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prices integrity checking";
				}
				return "Проверка целостности справочника Цены";
			}
			return "Գների ամբողջականության ստուգում";
		}
	}

	public static string CheckPrinter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check printer";
				}
				return "Чековый принтер";
			}
			return "Կտրոնի տպիչ";
		}
	}

	public static string CheckReportActualnessDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Report date is greater than reliability date. Data can be incorrect. Continue?";
				}
				return "Отчет будет сформирован датой более поздней чем Дата актуальности. Данные отчета могут быть неверными. Возможно, следует перепровести документы. Продолжить?";
			}
			return "Հաշվետվությունը կանչված է հավաստիության ամսաթվից ավելի ուշ ամսաթվով: Տվյալները կարող են ճիշտ չլինել: Շարունակե՞լ:";
		}
	}

	public static string CheckSalesIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales journal integrity check";
				}
				return "Проверка целостности журнала Продажи";
			}
			return "Վաճառքների մատյանի ամբողջականության ստուգում";
		}
	}

	public static string CheckStoragesAccessIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Integrity checking of storages accessibility";
				}
				return "Проверка целостности настроек прав доступа справочника Склады";
			}
			return "Պահեստների հասանելիությունների ամբողջականության ստուգում";
		}
	}

	public static string CheckTurnOverIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Turnover integrity checking";
				}
				return "Проверка целостности регистра Обороты";
			}
			return "Շրջանառության ամբողջականության ստուգում";
		}
	}

	public static string CheckTurnOverIntegrityMaterial
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations journal integrity checking";
				}
				return "Проверка целостности реестра Операции (Товары)";
			}
			return "Ապրանքների գործողությունների մատյանի ամբողջականության ստուգում";
		}
	}

	public static string CheckTurnOverRetailIntegrityMaterial
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products turnover (Retail) integrity checking";
				}
				return "Проверка целостности регистра Обороты (Товары в рознице)";
			}
			return "Ապրանքների շրջանառության(Մանրածախ) ամբողջականության ստուգում";
		}
	}

	public static string ClearCardInfo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Buyer's card is indicated. You must clear the entered card first.";
				}
				return "Карта покупателя указана. Сначала следует очистить введённую карту";
			}
			return "Կա ընտրված գնորդի քարտ, նախ պետք է մաքրել գնորդի քարտը";
		}
	}

	public static string ClientMap
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Clients map";
				}
				return "Карта заказчиков";
			}
			return "Պատվիրողների քարտեզ";
		}
	}

	public static string CloseCashBalance
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Closing cash balance";
				}
				return "Закрытие кассового остатка";
			}
			return "Դրամարկղի մնացորդի փակում";
		}
	}

	public static string CloseCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Close cashier shift";
				}
				return "Закрыть смену";
			}
			return "Փակել հերթափոխը";
		}
	}

	public static string cmdBarCodeEnter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bar code";
				}
				return "Штрихкод";
			}
			return "Գծ.կոդ";
		}
	}

	public static string cmdCodeEnter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Code";
				}
				return "Код";
			}
			return "Կոդ";
		}
	}

	public static string cmdNameEnter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Name";
				}
				return "Наимен.";
			}
			return "Անվ.";
		}
	}

	public static string cmdQtyEnter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "QTY";
				}
				return "Кол.";
			}
			return "Քան.";
		}
	}

	public static string ComfirmDiscountCalc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm discounts calculation";
				}
				return "Подтвердить расчет скидок";
			}
			return "Հաստատել զեղչերի հաշվարկը";
		}
	}

	public static string Command
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Command";
				}
				return "Команда";
			}
			return "Հրաման";
		}
	}

	public static string Commands
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Commands";
				}
				return "Команды";
			}
			return "Հրամաններ";
		}
	}

	public static string Commodity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product";
				}
				return "Товар";
			}
			return "Ապրանք";
		}
	}

	public static string CompareWithDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Compare with Discount Server";
				}
				return "Сравнить с Бонусным сервером";
			}
			return "Համեմատել Բոնուսային սերվերի հետ";
		}
	}

	public static string Complection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kitting";
				}
				return "Комплектация";
			}
			return "Կոմպլեկտավորում";
		}
	}

	public static string ConfirmCashierShiftClosing
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm closing cashier shift";
				}
				return "Подтвердите закрытие смены";
			}
			return "Հաստատել հերթափոխի փակումը";
		}
	}

	public static string ConfirmCashierShiftOpening
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm opening cashier shift";
				}
				return "Подтвердите открытие смены";
			}
			return "Հաստատել հերթափոխի բացումը";
		}
	}

	public static string ConfirmSummChange
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm amount change";
				}
				return "Подтвердите изменение суммы";
			}
			return "Հաստատեք գումարի փոփոխությունը";
		}
	}

	public static string ConfirmUser
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm user eligibility";
				}
				return "Подтверждение права";
			}
			return "Իրավասության հաստատում";
		}
	}

	public static string Connected
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connected";
				}
				return "Подключен";
			}
			return "Միացված է";
		}
	}

	public static string Connection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connection";
				}
				return "Подключение";
			}
			return "Միացում";
		}
	}

	public static string ConnectionConfirmedWithDB
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connection to  \"{0}\" database established successfully";
				}
				return "Соединение с базой данных \"{0}\" установлено";
			}
			return "Կապը \"{0}\" Տվյալների պահոցի հետ բարեհաջող հաստատվեց";
		}
	}

	public static string ConnectTob2barmsoftDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connect to b2b.armsoft discount server";
				}
				return "Подключиться к b2b.armsoft Бонусному серверу";
			}
			return "Միանալ b2b.armsoft Բոնուսային սերվերին";
		}
	}

	public static string ConnectTob2barmsoftDiscServInfoMsg
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "During transactions with the card the possibility:\r\n- To perform online requests to the server regarding sales and accumulated bonuses in all stores\r\n- To perform online registration of accumulated or used bonus units in the server";
				}
				return "Возможность при сделке с картой.  \r\n-  Выполнять онлайн запросы серверу относительно продаж и накопленных бонусов во всех магазинах, \r\n-  Выполнять онлайн регистрацию накопленных или использованных бонусных единиц в сервере.";
			}
			return "Հնարավորություն քարտով գործարքի ժամանակ.  \r\n-  օնլայն հարցում կատարել սերվերին բոլոր խանութներով վաճառքների և կուտակված բոնուսների  վերաբերյալ, \r\n-  կուտակված կամ օգտագործված բոնուսային միավորները օնլայն գրանցել սերվերում:";
		}
	}

	public static string ContinueShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Continue";
				}
				return "Продолж.";
			}
			return "Շարուն.";
		}
	}

	public static string ContractCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contract code";
				}
				return "Код договора";
			}
			return "Պայմանագրի կոդ";
		}
	}

	public static string ContractCurrCodeAndDocCurrCodeIsNotValid
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Currencies of \"{0}\" contract and document are not the same";
				}
				return "Валюты \"{0}\" договора и документа не совпадают";
			}
			return "\"{0}\" պայմանգրի և փաստաթղթի արժույթները չեն համընկնում";
		}
	}

	public static string ContractCurrIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{2}\" currency of \"{1}\" Contract by \"{0}\" partner is missing from \"Currencies\" directory";
				}
				return "Валюта \"{2}\" договора \"{1}\" с партнером \"{0}\" отсутствует в справочнике Валюты";
			}
			return "\"{0}\" գործընկերոջ \"{1}\" պայմանագրի \"{2}\" արժույթը բացակայում է \"Արտարժույթներ\" տեղեկատուից";
		}
	}

	public static string ContractForCalcBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For calculating bonus, contract with bonus must be selected";
				}
				return "Для расчета бонуса необходимо выбрать бонусный договор";
			}
			return "Բոնուս հաշվարկելու համար անհրաժեշտ է նշել բոնուսային պայմանագիր";
		}
	}

	public static string ContractForUseBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For bonus redemption bonus contract should be selected";
				}
				return "Для погашения бонуса необходимо выбрать бонусный договор";
			}
			return "Բոնուսը մարելու համար անհրաժեշտ է նշել բոնուսային պայմանագիր";
		}
	}

	public static string ContractInUse
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is registered operation with {0} Partner's \"{1}\" contract";
				}
				return "По договору \"{1}\" партнера \"{0}\" записана транзакция";
			}
			return "\"{0}\" գործընկերոջ \"{1}\" պայմանագրով գրանցված է գործողություն";
		}
	}

	public static string ContractInUseGrpRevalList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is registered \"Partners group revaluation\" document with {0} partner's \"{1}\"contracts";
				}
				return "По договору \"{1}\" партнера \"{0}\" зарегистрирован документ Групповая переоценка остатков по партнерам";
			}
			return "\"{0}\" գործընկերոջ \"{1}\" պայմանագրով գրանցված է \"Գործընկերների խմբային վերագնահատում\" փաստաթուղթ";
		}
	}

	public static string ContractIsNotBonusCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"Bonus card\" is not selected on \"{0}\" partner \"{1}\" contract";
				}
				return "На договоре \"{0}\" партнера \"{0}\" не указан значок «Бонусная карта»";
			}
			return "\"{0}\" գործընկերոջ  \"{1}\" կոդով պայմանագրի վրա բոնուսային քարտ հայտանիշը նշված չէ";
		}
	}

	public static string ContractLimit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contract amount limit";
				}
				return "Лимит суммы по договору";
			}
			return "Պայմանագրի գումարի սահման";
		}
	}

	public static string ContractPartnerAbsentIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner with {0} system number (ISN) is missing from \"Partners\" directory, contract \"{1}\", currency \"{2}\"";
				}
				return "Партнер с внутренним системным идентификатором \"{0}\" отсутствует в справочнике Партнеры, договор \"{1}\", валюта \"{2}\"";
			}
			return "\"{0}\" ներքին սիստեմային բնութագրիչով գործընկերը բացակայում է \"Գործընկերներ\" տեղեկատուից, պայմանագիր \"{1}\", արժույթ \"{2}\"";
		}
	}

	public static string ContractPartnerEmptyIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner inner system number (ISN) is missing (-1), contract \"{0}\", currency \"{1}\".";
				}
				return "Внутренний системный идентификатор партнера пуст (-1), договор \"{0}\", валюта \"{1}\" ";
			}
			return "Գործընկերոջ ներքին սիստեմային բնութագրիչը դատարկ է(-1), պայմանագիր \"{0}\" , արժույթ \"{1}\"";
		}
	}

	public static string ContractSalePercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Percent of contract discount";
				}
				return "Процент скидки договора";
			}
			return "Պայմանագրի զեղչի տոկոս";
		}
	}

	public static string ContractWithPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contract with {0} partner";
				}
				return "Договор с {0} партнером";
			}
			return "Պայմանագիր {0} գործընկերոջ հետ";
		}
	}

	public static string ControlReceivables
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receivables control";
				}
				return "Контроль дебиторской задолженности";
			}
			return "Դեբիտորական պարտքի հսկողություն";
		}
	}

	public static string CostSummaCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost in {0}";
				}
				return "Себест. в {0}";
			}
			return "Ինքնարժեքը {0}-ով";
		}
	}

	public static string CostSummaNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost in AMD";
				}
				return "Себест. в драмах";
			}
			return "Ինքնարժեքը դրամով";
		}
	}

	public static string CostVATSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "VAT of cost's amount";
				}
				return "НДС себестоим. ";
			}
			return "Ինքնարժեքի գումարի ԱԱՀ";
		}
	}

	public static string CountOfBills
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Count of bills";
				}
				return "Количество чеков";
			}
			return "Կտրոնների քանակը";
		}
	}

	public static string CountOfRows
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Count of rows";
				}
				return "Количество строк";
			}
			return "Տողերի քանակը";
		}
	}

	public static string CountryInMaterials
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} country cannot be deleted";
				}
				return "\"{0}\" страну нельзя удалять";
			}
			return "\"{0}\" երկիրը չի կարելի հեռացնել";
		}
	}

	public static string CountryInParties
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "With {0} country there is a batch";
				}
				return "С \"{0}\" страной существует партия";
			}
			return "\"{0}\" երկրով գոյություն ունի խմբաքանակ";
		}
	}

	public static string CreateBasedMTInvoiceError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale (Invoice) document can be created only for wholesale storage";
				}
				return "Документ Реализация (Счет-Фактура) можно создать только для оптового склада";
			}
			return "Վաճառք (Հաշիվ-Ապրանքագիր) փաստաթուղթ հնարավոր է ստեղծել միայն Մեծածախ պահեստ նշված լինելու դեպքում";
		}
	}

	public static string CreateBasedMTInvoiceRetailError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale (Retail) document can be created only for retail storage";
				}
				return "Документ Продажа (Розница) можно создать только для розничного склада";
			}
			return "Վաճառք (Մանրածախ) փաստաթուղթ հնարավոր է ստեղծել միայն մանրածախ պահեստ նշված լինելու դեպքում";
		}
	}

	public static string CreateMTMoveOrder
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Create Products movement order";
				}
				return "Создать заказ на перемещение товаров";
			}
			return "Ստեղծել ապրանքների տեղաշարժի պատվեր";
		}
	}

	public static string CreateNewDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Create documents";
				}
				return "Запись документов";
			}
			return "Փաստաթղթերի գրանցում";
		}
	}

	public static string CreateRetToSuppForCorrInv
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For correction invoice only Return to supplier document can be created";
				}
				return "Для корректировочного счета можно создать только документ Возврат поставщику ";
			}
			return "Ճշգրտող հաշվի համար կարելի է ստեղծել միայն վերադարձ մատակարարին փաստաթուղթ";
		}
	}

	public static string CreditNC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Credit in Dram";
				}
				return "Кредит в Драмах";
			}
			return "Կրեդիտ դրամով";
		}
	}

	public static string CreditSale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Credit";
				}
				return "Кредит";
			}
			return "Ապառիկ";
		}
	}

	public static string CreditSaleBuyer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Select buyer";
				}
				return "Введите покупателя";
			}
			return "Անհրաժեշտ է նշել գնորդին";
		}
	}

	public static string CreditSaleShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale credit";
				}
				return "Кредит ";
			}
			return "Ապառ.";
		}
	}

	public static string CumulativePercentage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cumulative percentage";
				}
				return "Накопительный процент";
			}
			return "Կուտակային տոկոս";
		}
	}

	public static string CurrentAccumulate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Current earning";
				}
				return "Текущее начисление";
			}
			return "Ընթացիկ կուտակում";
		}
	}

	public static string CurrentBonusRemain
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Current balance";
				}
				return "Текущий остаток";
			}
			return "Ընթացիկ մնացորդ";
		}
	}

	public static string Currentprice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Current price";
				}
				return "Текущая цена";
			}
			return "Ընթացիկ գին";
		}
	}

	public static string CurrPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price in currency";
				}
				return "Сумма в валюте";
			}
			return "Գին արժույթով";
		}
	}

	public static string CustomDuty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customs duty";
				}
				return "Таможенная пошлина";
			}
			return "Մաքսատուրք";
		}
	}

	public static string Customers
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customers";
				}
				return "Покупатели";
			}
			return "Գնորդներ";
		}
	}

	public static string CustomFee
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customs fee (AMD)";
				}
				return "Таможенный сбор (драм)";
			}
			return "Մաքսավճար (դրամ)";
		}
	}

	public static string CustomHouse
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customs";
				}
				return "Таможня";
			}
			return "Մաքսատուն";
		}
	}

	public static string CustomSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customs value";
				}
				return "Таможенная стоимость";
			}
			return "Մաքսային արժեք";
		}
	}

	public static string DatabaseId
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Database identifier";
				}
				return "Идентификатор базы данных";
			}
			return "Տվյալների պահոցի նույնացուցիչ";
		}
	}

	public static string DatabaseShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Database";
				}
				return "База";
			}
			return "Պահոց";
		}
	}

	public static string DatabaseTransferStart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" database transfer";
				}
				return "Перенос базы данных \"{0}\"";
			}
			return "\"{0}\" տվյալների պահոցի տեղափոխում";
		}
	}

	public static string DataExchange
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data Exchange";
				}
				return "Обмен данными";
			}
			return "Տվյալների փոխանակում";
		}
	}

	public static string DataImport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data import";
				}
				return "Импорт данных";
			}
			return "Տվյալների ներմուծում";
		}
	}

	public static string DataTransferEnd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data transfer end";
				}
				return "Конец обмена данными";
			}
			return "Տվյալների տեղափոխման ավարտ";
		}
	}

	public static string DataTransferStart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data transfer start";
				}
				return "Начало обмена данными";
			}
			return "Տվյալների տեղափոխման սկիզբ";
		}
	}

	public static string DBVersionInconsistencyForConverted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For converting data the database version must be 3.4.6 or 3.4.7";
				}
				return "Для конвертации необходимо чтобы данные соответствовали версиям 3.4.6 или 3.4.7";
			}
			return "Փոխարկման համար անհրաժեշտ է, որ տվյալները  համապատասխանեն  3.4.6 կամ 3.4.7 տարբերակին:";
		}
	}

	public static string DebitNC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Debit in Dram";
				}
				return "Дебет в Драмах";
			}
			return "Դեբետ դրամով";
		}
	}

	public static string DebtDecrease
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Decrease of debt";
				}
				return "Уменьшение долга";
			}
			return "Պարտքի նվազում";
		}
	}

	public static string DebtIncrease
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Increase of debt";
				}
				return "Увеличение долга";
			}
			return "Պարտքի աճ";
		}
	}

	public static string DebtSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Debt amount";
				}
				return "Сумма долга";
			}
			return "Պարտքի գումարը";
		}
	}

	public static string DecCrAccMustNotBeSame
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Debit and credit accounts must be different.";
				}
				return "Банковские счета дебета и кредита должны быть разными";
			}
			return "Դեբետ և կրեդիտ հաշվարկային հաշիվները պետք է լինեն տարբեր:";
		}
	}

	public static string Decomplection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reverse kitting";
				}
				return "Разукомплектация";
			}
			return "Ապակոմպլեկտավորում";
		}
	}

	public static string Decrease
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Qty -";
				}
				return "Кол. -";
			}
			return "Քան. -";
		}
	}

	public static string DecreasedBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Decrease of bonus";
				}
				return "Уменьшение бонуса";
			}
			return "Բոնուսի նվազում";
		}
	}

	public static string DefaultBankAcc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Default settlement account";
				}
				return "Основной банковский счет";
			}
			return "Հիմնական հաշվարկային հաշիվ";
		}
	}

	public static string DefaultContract
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Default contract";
				}
				return "Основной договор";
			}
			return "Հիմնական պայմանագիր";
		}
	}

	public static string DefaultContractMustBeAtLeastOne
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "At least one contract must be checked";
				}
				return "Один из договоров должен быть отмечен как основной";
			}
			return "Առնվազն մեկ պայմանագիր պետք է նշված լինի որպես հիմնական:";
		}
	}

	public static string DefaultSettlAccInBankMustBeAtLeastOne
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "At least one settlement account must be checked";
				}
				return "Один из банковских счетов должен быть отмечен как основной";
			}
			return "Առնվազն մեկ հաշվարկային հաշիվ պետք է նշված լինի որպես հիմնական:";
		}
	}

	public static string Defer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Hold";
				}
				return "Отложить";
			}
			return "Հետաձգել";
		}
	}

	public static string DeferShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Put on hold";
				}
				return "Отложить";
			}
			return "Հետաձգ․";
		}
	}

	public static string DeleteCashierShiftsForFirstDB
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting the shifts opened after {0}";
				}
				return "Удаление смен открытых после {0}";
			}
			return "{0}-ից հետո բացված հերթափոխերի հեռացում";
		}
	}

	public static string DeleteCashierShiftsForSecondDB
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting shifts closed before {0}";
				}
				return "Удаление смен закрытых до {0}";
			}
			return "Մինչև {0}-ը փակված հերթափոխերի հեռացում";
		}
	}

	public static string DeletedShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "D";
				}
				return "У.";
			}
			return "Հ.";
		}
	}

	public static string DeleteFromDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Delete from discount server";
				}
				return "Удаление из бонусного сервера";
			}
			return "Հեռացում բոնուսային սերվերից";
		}
	}

	public static string DeleteRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Del.row";
				}
				return "Удал.строку";
			}
			return "Հեռ․տողը";
		}
	}

	public static string DenyAccessToOtherOwnerDocs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Limit access to other users documents";
				}
				return "Запретить работу с документами другого пользователя";
			}
			return "Սահմանափակել աշխատանքը այլ օգտագործողներին պատկանող փաստաթղթերի հետ";
		}
	}

	public static string DetailByContrac
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Opened by contracts";
				}
				return "Детализировать по договорам";
			}
			return "Բացված ըստ պայմանագրերի";
		}
	}

	public static string DetailByStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Opened by storages";
				}
				return "Детализировать по складам";
			}
			return "Բացված ըստ պահեստների";
		}
	}

	public static string Difference
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Difference";
				}
				return "Разница";
			}
			return "Տարբերություն";
		}
	}

	public static string Discount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount";
				}
				return "Скидка";
			}
			return "Զեղչ";
		}
	}

	public static string DiscountBonusCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Buyer";
				}
				return "Бон./Скд.";
			}
			return "Գնորդի";
		}
	}

	public static string DiscountCardCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount card code";
				}
				return "Код карты";
			}
			return "Քարտի կոդ";
		}
	}

	public static string DiscountCurSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount amount in {0}";
				}
				return "Сумма со скидкой {0}";
			}
			return "Զեղչի գումար {0}-ով";
		}
	}

	public static string DiscountDescription
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount % / Description";
				}
				return "% скидки / Описание";
			}
			return "Զեղչի % / նկարագրություն";
		}
	}

	public static string DiscountedSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discounted amount";
				}
				return "Сумма со скидкой";
			}
			return "Զեղչված գումար";
		}
	}

	public static string DiscountPercentSymbol
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount %";
				}
				return "% скидки";
			}
			return "Զեղչի %";
		}
	}

	public static string DiscountPeriod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount Period";
				}
				return "Период применения";
			}
			return "Կիրառման ժամանակահատված";
		}
	}

	public static string DiscountPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount price type";
				}
				return "Тип скидочной цены";
			}
			return "Զեղչի գնի տեսակ";
		}
	}

	public static string Discounts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discounts";
				}
				return "Скидки";
			}
			return "Զեղչեր";
		}
	}

	public static string DiscountsAndBonuses
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discounts and bonuses";
				}
				return "Скидки и бонусы";
			}
			return "Զեղչեր և բոնուսներ";
		}
	}

	public static string DiscountScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount range";
				}
				return "Шкала скидок";
			}
			return "Զեղչի սանդղակ";
		}
	}

	public static string DiscountsDeclaration
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Public discounts posting";
				}
				return "Объявление об публичных скидках";
			}
			return "Հրապարակային զեղչերի հայտարարություն";
		}
	}

	public static string DiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount Server";
				}
				return "Бонусный сервер";
			}
			return "Բոնուսային սերվեր";
		}
	}

	public static string DiscountServerBonusSumComparison
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Comparison with database data";
				}
				return "Сверка с данными базы";
			}
			return "Համեմատում պահոցի տվյալների հետ";
		}
	}

	public static string DiscountsForDeclarationNotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No discounted products or services for posting founded";
				}
				return "Товаров или услуг со скидкой которые подлежат объявлению не обнаружено ";
			}
			return "Հայտարարման ենթակա զեղչված ապրանքներ կամ ծառայություններ չեն հայտնաբերվել";
		}
	}

	public static string DiscountShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount";
				}
				return "Скидка";
			}
			return "Զեղչ";
		}
	}

	public static string DiscountSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount amount";
				}
				return "Сумма скидки";
			}
			return "Զեղչի գումար";
		}
	}

	public static string DiscountType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount type";
				}
				return "Тип скидки";
			}
			return "Զեղչի տեսակ";
		}
	}

	public static string DiscountTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount types";
				}
				return "Типы скидок";
			}
			return "Զեղչի տեսակներ";
		}
	}

	public static string DiscountUse
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow discount during sale";
				}
				return "В документах продажи использовать скидки";
			}
			return "Վաճառքի ժամանակ թույլատրել օգտագործել զեղչ";
		}
	}

	public static string DiscountVATSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "VAT of discount’s amount";
				}
				return "НДС скидки";
			}
			return "Զեղչի գումարի ԱԱՀ";
		}
	}

	public static string DistributedSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allocated amount";
				}
				return "Распределенная сумма в драмах";
			}
			return "Բաշխված գումար դրամով";
		}
	}

	public static string DistributionSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Distribution amount";
				}
				return "Распределяемая сумма";
			}
			return "Բաշխվող գումար";
		}
	}

	public static string DocAllowchangePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to edit sales price";
				}
				return "Разрешить редактирование цен продаж  ";
			}
			return "Թույլ տալ խմբագրել վաճառքի գինը";
		}
	}

	public static string DocBaseISNDuplicate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Overhead expenses document already exist for the chosen base document.";
				}
				return "Для указанного документа основания зарегистрирован документ Распределение дополнительных расходов";
			}
			return "Ընտրված հիմք փաստաթղթով հավելյալ ծախսերի բաշխման փաստաթուղթ գոյություն ունի";
		}
	}

	public static string DocCantBeEdit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Given operation is not allowed for the document created by \"Archive\" operation.";
				}
				return "Данная операция не разрешена для документов созданных при Архивации ";
			}
			return "\"Արխիվացում\" գործողությամբ ստեղծված փաստաթղթի համար տվյալ գործողությունը թույլատրելի չէ";
		}
	}

	public static string DocConstraints
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Work restrictions";
				}
				return "Ограничение на работу";
			}
			return "Աշխատանքի սահմանափակումներ";
		}
	}

	public static string DocCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document  currency";
				}
				return "Валюта документа";
			}
			return "Փաստաթղթի արժույթ";
		}
	}

	public static string DocCurrDifferSettAccCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document's currency is different from settlement account currency";
				}
				return "Валюта документа не совпадает с валютой банковского счета";
			}
			return "Փաստաթղթի արժույթը տարբերվում է հաշվարկային հաշվի արժույթից";
		}
	}

	public static string DocDateNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Date, doc. N";
				}
				return "Дата, номер док.";
			}
			return "Ա/թ, փաստ. N";
		}
	}

	public static string DocInfo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Information";
				}
				return "Информация";
			}
			return "Տեղեկություն";
		}
	}

	public static string DocInvoice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale (Invoice)";
				}
				return "Реализация (Счет-Фактура)";
			}
			return "Վաճառք (Հաշիվ-Ապրանքագիր)";
		}
	}

	public static string DocMTBill
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale (Bill)";
				}
				return "Реализация (Чек)";
			}
			return "Վաճառք (Կտրոն)";
		}
	}

	public static string DocMTComplectation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product kitting";
				}
				return "Комплектация";
			}
			return "Կոմպլեկտավորում";
		}
	}

	public static string DocMTComplectationShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kitting";
				}
				return "Комплектация";
			}
			return "Կոմպլեկտավորում";
		}
	}

	public static string DocMTIncomeImport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Importing foreign products";
				}
				return "Поступление товаров (Импорт)";
			}
			return "Պահեստի մուտքի օրդեր (Ներմուծում)";
		}
	}

	public static string DocMTIncomeImportShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Imp. foreign products";
				}
				return "Импорт";
			}
			return "Ներմուծում";
		}
	}

	public static string DocMTIncomeRem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products balances input";
				}
				return "Ввод остатков товаров";
			}
			return "Ապրանքների մնացորդների մուտք";
		}
	}

	public static string DocMTIncomeRetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Storage input order (Retail)";
				}
				return "Поступление товаров (Розница)";
			}
			return "Պահեստի մուտքի օրդեր (Մանրածախ)";
		}
	}

	public static string DocMTInventory
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products stocktaking";
				}
				return "Инвентаризация товаров";
			}
			return "Ապրանքների գույքագրում";
		}
	}

	public static string DocMTInventoryRetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products stocktaking (Retail)";
				}
				return "Инвентаризация товаров (розница)";
			}
			return "Ապրանքների գույքագրում (Մանրածախ)";
		}
	}

	public static string DocMTInventoryShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products stocktaking";
				}
				return "Инвентаризация товаров";
			}
			return "Ապրանքների գույքագրում";
		}
	}

	public static string DocMTMove
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products movement";
				}
				return "Перемещение товаров";
			}
			return "Ապրանքների տեղաշարժ";
		}
	}

	public static string DocMTMoveShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products movement";
				}
				return "Перемещение товаров";
			}
			return "Ապրանքների տեղաշարժ";
		}
	}

	public static string DocMTReturnFromCustomer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return from customer";
				}
				return "Возврат от покупателя";
			}
			return "Վերադարձ գնորդից";
		}
	}

	public static string DocMTReturnToSupplier
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return to supplier";
				}
				return "Возврат поставщику";
			}
			return "Վերադարձ մատակարարին";
		}
	}

	public static string DocMTRevaluationRetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products revaluation (Retail)";
				}
				return "Переоценка товаров (розница)";
			}
			return "Ապրանքների վերագնահատում (Մանրածախ)";
		}
	}

	public static string DocMTStockTaking
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products stocktaking";
				}
				return "Инвентаризация товаров";
			}
			return "Ապրանքների գույքագրում";
		}
	}

	public static string DocMTStockTakingShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products stocktaking";
				}
				return "Инвентаризация ТМЦ";
			}
			return "ՆԱ գույքագրում";
		}
	}

	public static string DocNextMTBillNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale (Bill) serial number";
				}
				return "Порядковый номер Реализации (Чек)";
			}
			return "Վաճառք (Կտրոն) հերթական համար";
		}
	}

	public static string DocOnlyForRetailStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} document can be created only for retail storage";
				}
				return "Документ \"{0}\" можно создать только для розничного склада";
			}
			return "\"{0}\" փաստաթուղթը կարելի է ստեղծել միայն մանրածախ պահեստի համար";
		}
	}

	public static string DocOnlyForWholesaleStore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} document can be created only for wholesale storage";
				}
				return "Документ \"{0}\" можно создать только для оптового склада";
			}
			return "\"{0}\" փաստաթուղթը կարելի է ստեղծել միայն մեծածախ պահեստի համար";
		}
	}

	public static string DocOwnerChanged
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Owner changed";
				}
				return "Автор изменен";
			}
			return "Հեղինակը փոփոխվել է";
		}
	}

	public static string DocOwnerChangedCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Changed documents number: {0}";
				}
				return "Количество измененных документов - {0}";
			}
			return "Փոփոխված փաստաթղթերի քանակը` {0}";
		}
	}

	public static string DocRoundDegree
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document sum rounding degree";
				}
				return "Степень округления документа";
			}
			return "Փաստաթղթի գումարի կլորացման աստիճան";
		}
	}

	public static string DocumentDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document date";
				}
				return "Дата документа";
			}
			return "Փաստաթղթի ամսաթիվ ";
		}
	}

	public static string DocumentsInvoiceVATIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "VAT Included";
				}
				return "Включая НДС-";
			}
			return "ԱԱՀ-ն այդ թվում՝";
		}
	}

	public static string DocumentsTotal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total";
				}
				return "Всего";
			}
			return "Ընդամենը";
		}
	}

	public static string DocumentSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document sum";
				}
				return "Сумма документа";
			}
			return "Փաստաթղթի գումար";
		}
	}

	public static string Dram
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "dram";
				}
				return "драм";
			}
			return "ՀՀ դրամ";
		}
	}

	public static string DuplicateInMTList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Duplicate row exists in products list {0}";
				}
				return "В списке товаров существует повторная строка {0}";
			}
			return "Ապրանքների ցուցակում գոյություն ունի կրկնվող տող {0}";
		}
	}

	public static string ECRBaseCheckNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base ECR check N";
				}
				return "N базового чека ККМ";
			}
			return "Հիմք ՀԴՄ կտրոնի N";
		}
	}

	public static string ECRCashier
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashier - all";
				}
				return "Кассир - все";
			}
			return "Գանձապահ - բոլորը";
		}
	}

	public static string ECRCheckPrintForBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print ECR check for bonus";
				}
				return "Распечатать ККМ чек для бонуса";
			}
			return "Տպել ՀԴՄԿ բոնուսի համար";
		}
	}

	public static string ECRCheckPrintForCredit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print ECR check for credit";
				}
				return "Распечатать ККМ чек для кредита";
			}
			return "Տպել ՀԴՄԿ ապառիկի համար";
		}
	}

	public static string ECRChecksListJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ECR check items list";
				}
				return "Список товаров ККМ чеков";
			}
			return "ՀԴՄ կտրոնների ապրանքացանկ";
		}
	}

	public static string ECRCrnReport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Z report";
				}
				return "Z отчет";
			}
			return "Z հաշվետվություն";
		}
	}

	public static string ECRCrnShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "RN";
				}
				return "ЗН:";
			}
			return "ԳՀ:";
		}
	}

	public static string ECRDep
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ECR department";
				}
				return "Секция ККМ";
			}
			return "ՀԴՄ բաժին";
		}
	}

	public static string ECRDepVATSumLabel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Որից ԱԱՀ";
				}
				return "Որից ԱԱՀ";
			}
			return "Որից ԱԱՀ";
		}
	}

	public static string ECRDepWithOutVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Department without VAT";
				}
				return "Секция ККМ не облагаемая НДС";
			}
			return "ԱԱՀ-ով չհարկվող ՀԴՄ բաժին";
		}
	}

	public static string ECRDepWithOutVATNotSet
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The department without VAT not set";
				}
				return "Секция ККМ не облагаемая НДС не заполнена";
			}
			return "ԱԱՀ-ով չհարկվող ՀԴՄ բաժինը լրացված չէ";
		}
	}

	public static string ECRDepWithVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Department with VAT";
				}
				return "Секция ККМ облагаемая НДС";
			}
			return "ԱԱՀ-ով հարկվող ՀԴՄ բաժին";
		}
	}

	public static string ECRDepWithVATNotSet
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The department with VAT not set";
				}
				return "Секция ККМ облагаемая НДС не заполнена";
			}
			return "ԱԱՀ-ով հարկվող ՀԴՄ բաժինը լրացված չէ";
		}
	}

	public static string ECRNotWinMessage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ԿՏՐՈՆԸ ՉԻ ՇԱՀԵԼ";
				}
				return "ԿՏՐՈՆԸ ՉԻ ՇԱՀԵԼ";
			}
			return "ԿՏՐՈՆԸ ՉԻ ՇԱՀԵԼ";
		}
	}

	public static string ECROpeations
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operations";
				}
				return "Операции";
			}
			return "Գործարքներ";
		}
	}

	public static string ECRPOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ECR POS-terminal";
				}
				return "ККМ POS-терминал";
			}
			return "ՀԴՄ-ի POS-տերմինալ";
		}
	}

	public static string ECRReturnCheckNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return ECR Check N";
				}
				return "N чека возврата ККМ";
			}
			return "Վերադարձի ՀԴՄ կտրոնի N";
		}
	}

	public static string ECRRowDiscountTypePercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On one unit of product with %";
				}
				return "Процентная на товар";
			}
			return "%-ային ապրանքի մեկ միավորի վրա";
		}
	}

	public static string ECRRowDiscountTypePrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On one unit of product with AMD";
				}
				return "Суммарная на товар";
			}
			return "Դրամային ապրանքի մեկ միավորի վրա";
		}
	}

	public static string ECRRowDiscountTypeTotal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On total amount of products with AMD";
				}
				return "Суммарная на общее количество товара";
			}
			return "Դրամային ապրանքի ընդհանուր քանակի վրա";
		}
	}

	public static string ECRSN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ECR serial number";
				}
				return "Номер ККМ";
			}
			return "ՀԴՄ սարքի համար";
		}
	}

	public static string ECRSNShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "SN";
				}
				return "СН:";
			}
			return "ՍՀ:";
		}
	}

	public static string ECRWinMessage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ԴՈՒՔ ՇԱՀԵԼ ԵՔ";
				}
				return "ԴՈՒՔ ՇԱՀԵԼ ԵՔ";
			}
			return "ԴՈՒՔ ՇԱՀԵԼ ԵՔ";
		}
	}

	public static string ECRWin_NotWin_MessageFieldName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wօn / Not Wօn";
				}
				return "Выиграл / Не выиграл";
			}
			return "Շահել է / Չի շահել";
		}
	}

	public static string ElScaleOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "El. scale options";
				}
				return "Настройки эл. весов";
			}
			return "Էլ. կշեռքի պարամետրեր";
		}
	}

	public static string EmplIDCardTemplateName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Employee ID card";
				}
				return "ID карта сотрудника";
			}
			return "Աշխատակցի ID քարտ";
		}
	}

	public static string EmployeeExistenceInUserParamsTable
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In Users documents options there is an entry with the given Seller.";
				}
				return "В настройках пользователя для документов обнаружена запись по данному сотруднику";
			}
			return "Օգտագործողների փաստաթղթի դրույթներում գոյություն ունի գրառում տրված աշխատակցով:";
		}
	}

	public static string EmployeeIDNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" inner system number(ISN) of employee is missing from \"Employees\" directory";
				}
				return "Сотрудник с внутренним системным идентификатором \"{0}\" отсутствует в справочнике Сотрудники.";
			}
			return "Աշխատակցի ներքին սիստեմային բնութագրիչը \"{0}\" բացակայում է \"Աշխատակիցներ\" տեղեկատուից";
		}
	}

	public static string EmployeeIsNotSaleConsultant
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" Employee is not a seller.";
				}
				return "Сотрудник '{0}' не продавец";
			}
			return "\"{0}\" աշխատակիցը վաճառող չէ:";
		}
	}

	public static string EmployeeNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" employee is missing from \"Employees\" directory";
				}
				return "Сотрудник с табельным номером \"{0}\" отсутствует в справочнике Сотрудники";
			}
			return "\"{0}\" տաբելային համարով աշխատակիցը բացակայում է \"Աշխատակիցներ\" տեղեկատուից";
		}
	}

	public static string EmptyDocumentsBaseISN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Clearing of not existing values in fBASEISN column of Documents Journal.";
				}
				return "Удаление не существующие значения из колонки fBASEISN журнал документов";
			}
			return "Փաստաթղթերի մատյանի fBASEISN սյան գոյություն չունեցող արժեքների մաքրում";
		}
	}

	public static string EmptyDocumentsBaseISNError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Failed to clear not existing values in fBASEISN column of Documents Journal.";
				}
				return "Не удалось удалить не существующие значения из колонки fBASEISN журнал документов ";
			}
			return "Չհաջողվեց մաքրել փաստաթղթերի մատյանի fBASEISN սյան գոյություն չունեցող արժեքները";
		}
	}

	public static string EmptyPartiesList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Batches list is empty: storage {0}, product {1} and quantity - {2}";
				}
				return "Список партий пуст: склад - {0}, товар - {1}, количество - {2}";
			}
			return "Խմբաքանակների ցուցակը դատարկ է` պահեստ {0}, ապրանք {1},  քանակ - {2}";
		}
	}

	public static string EnableSubstituteItems
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use substitute items during sales";
				}
				return "При продаже применять замены товаров";
			}
			return "Վաճառքի ժամանակ օգտագործել փոխարինվող ապրանքներ";
		}
	}

	public static string EndRem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ending balance";
				}
				return "Конечный остаток (перерасход)";
			}
			return "Վերջնական մնացորդ(գերածախսը)";
		}
	}

	public static string EndRemainsAMD
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ending balance in AMD";
				}
				return "Конечный остаток в драмах";
			}
			return "Վերջնական մնացորդ դրամով";
		}
	}

	public static string EndRemainsCur
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ending balance in currency";
				}
				return "Конечный остаток в валюте";
			}
			return "Վերջնական մնացորդ արժույթով";
		}
	}

	public static string EndSumGreaterThanBeginSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Final amount must be greater than initial amount";
				}
				return "Конечная сумма должна быть большой первоначальной суммы";
			}
			return "Վերջնական գումարը պետք է լինի մեծ սկզբնական գումարից";
		}
	}

	public static string EndSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Final amount (AMD)";
				}
				return "Конечная сумма (GEL)";
			}
			return "Վերջն. գումար (AMD)";
		}
	}

	public static string EnterGoodsRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enter products balances";
				}
				return "Введите начальные остатки товаров";
			}
			return "Մուտքագրեք ապրանքների մնացորդները";
		}
	}

	public static string EnterPartAccBalance
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enter opening balances for partners";
				}
				return "Введите начальные остатки партнеров";
			}
			return "Մուտքագրեք գործընկերների սկզբնական մնացորդները";
		}
	}

	public static string EnterYourCashes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enter your cash desks";
				}
				return "Введите кассы Вашей организации";
			}
			return "Մուտքագրեք Ձեր դրամարկղերը";
		}
	}

	public static string EntPatternEditCParam
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Parameter";
				}
				return "Параметр";
			}
			return "Պարամետր";
		}
	}

	public static string EntPatternEditCRowNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Row number";
				}
				return "Номер строки";
			}
			return "Տողի համար";
		}
	}

	public static string EntrExitsDocumentNotEx
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{5}\" document by which was recorded transaction by: \r\ntype \"{1}\",\r\naccount/contract \"{2}\",\r\npartner\"{3}\",\r\ncurrency code \"{4}\" \r\non \"{0}\" is missing.";
				}
				return "Документ \"{0}\" числа, вида \"{1}\", по счету/договору \"{2}\", партнеру \"{3}\", валюте \"{4}\", проводке \"{5}\" не найден";
			}
			return "\"{0}\" ամսաթվով ըստ`\r\nտեսակ \"{1}\",\r\nհաշիվ/պայմանագիր \"{2}\",\r\nգործընկեր \"{3}\",\r\nարժույթ \"{4}\"\r\nգրանցված \"{5}\" գործառնության փաստաթուղթը բացակայում է";
		}
	}

	public static string ErasePayments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Remove payments";
				}
				return "Очистить оплаты";
			}
			return "Մաքրել վճ.";
		}
	}

	public static string ErrActualnessDateMustExclusive
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reliability date could be transfered only in single user mode";
				}
				return "Перенос даты актуальности может выполняться только в монопольном режиме.";
			}
			return "Հավաստիության ամսաթիվը կարելի է տեղափոխել միայն մենաշնորհային ռեժիմում:";
		}
	}

	public static string ErrBankAquaringPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In case of noncash amount \"POS Bank\" partner should be filled.";
				}
				return "При безналичной оплате (платежной картой) необходимо указать Банк (партнера) предоставляющий POS-терминал";
			}
			return "Անկանխիկ գումարի առկայության դեպքում \"POS բանկ\" գործընկերը պետք է լրացված լինի";
		}
	}

	public static string ErrBarCodeLen
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Barcode length is greater than {0} ({1})";
				}
				return "Длина штрихкода превышает {0} ({1})";
			}
			return "Գծիկավոր կոդի երկարությունը մեծ է {0}-ից ({1})";
		}
	}

	public static string ErrBarcodePrefixForNonWeightItem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Error Barcode Prefix For Non WeightItem";
				}
				return "Штрихкод штучного товара не может начинаться префиксом для штрихкода весового товара";
			}
			return "Ոչ քաշային ապրանքի գծ. կոդը չի թույլատրվում սկսել քաշային ապրանքի նախդիրով";
		}
	}

	public static string ErrCashGreaterTotal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash amount is greater than total amount";
				}
				return "Наличная сумма превышает итоговую сумму";
			}
			return "Կանխիկ գումարը մեծ է ընդհանուր գումարից";
		}
	}

	public static string ErrChildasParent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group cannot be as subgroup of itself";
				}
				return "Группа не может быть сама для себя подгруппой";
			}
			return "Խումբը չի կարող ինքն իր ենթախումբը լինել";
		}
	}

	public static string ErrContractNotExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} contract is missing from  \"{1} partners contracts\" directory";
				}
				return "Договор \"{0}\" отсутствует в справочнике \"Договоры партнера {1}\"";
			}
			return "\"{0}\" պայմանագիրը բացակայում է \"{1} գործընկերոջ պայմանագրեր\" տեղեկատուից";
		}
	}

	public static string ErrContractsWithDiffCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contracts currencies must be same";
				}
				return "Валюты договоров не совпадают";
			}
			return "Պայմանագրերի արժույթները պետք է լինեն նույնը";
		}
	}

	public static string ErrCreditSummaField
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale credit amount should be smaller than total amount";
				}
				return "Сумма кредита не должна превосходить итоговую сумму";
			}
			return "Ապառիկ գումարը պետք է փոքր լինի ընդհանուր գումարից";
		}
	}

	public static string ErrDifferentCurrNotAllowedForMTReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return from Customer document cannot be created based on sale rows with different currencies.";
				}
				return "Документ Возврат от покупателя невозможно создать на основе строк продаж с разными валютами";
			}
			return "Վերադարձ գնորդից փաստաթուղթը հնարավոր չէ ստեղծել տարբեր արժույթներով վաճառքի տողերի հիման վրա";
		}
	}

	public static string ErrDifferentVATTypeNotAllowedForMTReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return from customer document cannot be created based on sales rows with different VAT types.";
				}
				return "Документ Возврат от покупателя невозможно создать на основе строк продаж с разными типами расчета НДС";
			}
			return "Վերադարձ գնորդից փաստաթուղթը հնարավոր չէ ստեղծել տարբեր ԱԱՀ-ի հաշվարկի ձևերով վաճառքի տողերի հիման վրա";
		}
	}

	public static string ErrDuplicateUniqueKey
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Table key value repetition {0}";
				}
				return "Совпадение значения {0} являющегося ключом таблицы";
			}
			return "Աղյուսակում բանալի հանդիսացող արժեքի կրկնություն \r\n{0}";
		}
	}

	public static string ErrEmptyCurrExchange
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Currency exchange rate is missing up to selected date included";
				}
				return "Курс валюты до указанной даты включительно отсутствует";
			}
			return "Արտարժույթի փոխարժեքը մինչև նշված ամսաթիվը ներառյալ բացակայում է";
		}
	}

	public static string ErrFieldCantBeNegative
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} cannot be negative";
				}
				return "{0} не может быть отрицательным";
			}
			return "{0}ը չի կարող լինել բացասական";
		}
	}

	public static string ErrFieldIsGtZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} should be greater than zero";
				}
				return "{0} должен быть больше нуля";
			}
			return "{0}ը պետք է մեծ լինի զրոյից";
		}
	}

	public static string ErrForExportToScaleMustBeSetUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Error For Export To Scale Must Be Set Unit";
				}
				return "Для экспорта в настройках необходимо указать единицу измерения весового товара";
			}
			return "Արտահանման համար անհրաժեշտ է դրույթներում նշել քաշային ապրանքի չափման միավորը";
		}
	}

	public static string ErrInternalBarcodeLength
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Length of next barcode must have {0} characters";
				}
				return "Штрихкод должен содержать {0} символов";
			}
			return "Հերթական գծիկավոր կոդի երկարությունը պետք է լինի {0} նիշ";
		}
	}

	public static string ErrInternalBarcodeLenMax
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The length of barcode cannot be longer than {0}";
				}
				return "Длина поля штрихкода не может быть больше чем {0}";
			}
			return "Հերթական գծիկավոր կոդ դաշտի երկարությունը չի կարող լինել մեծ {0}-ից";
		}
	}

	public static string ErrInternalBarcodeLenMin
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The length of barcode cannot be shorter than {0}";
				}
				return "Длина поля штрихкода не может быть меньше чем {0}";
			}
			return "Հերթական գծիկավոր կոդ դաշտի երկարությունը չի կարող լինել փոքր {0}-ից";
		}
	}

	public static string ErrInternalBarcodeNumeric
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Value in the next barcode filed must be numerical";
				}
				return "Штрихкод должен содержать только цифровые  значение";
			}
			return "Հերթական գծիկավոր կոդ դաշտի արժեքը պետք է լինի թվային";
		}
	}

	public static string ErrInternalWeightBarcodesPrefix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Barcode prefixes of products accounted by weight and unit must be different.";
				}
				return "Префикс штрихкода весового товара и  префикс штрихкода штучного товаров не должны совпадать";
			}
			return "Քաշային և հատով հաշվառվող ապրանքների գծիկավոր կոդերի նախդիրները պետք է լինեն տարբեր";
		}
	}

	public static string ErrInvalidBaseDocTypeForMTReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return from Customer document can only be created with based on sales rows.";
				}
				return "Документ Возврат от покупателя можно создать только на основе строк продаж";
			}
			return "Վերադարձ գնորդից փաստաթուղթը հնարավոր է ստեղծել միայն վաճառքի տողերի հիման վրա";
		}
	}

	public static string ErrMTCodeLen
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's code length is more than {0} ({1})";
				}
				return "Длина кода товара превышает {0} ({1})";
			}
			return "Ապրանքի կոդի երկարությունը մեծ է {0}-ից ({1})";
		}
	}

	public static string ErrNegativeQuantity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's actual quantity cannot be negative number";
				}
				return "Фактическое количество товара не может быть отрицательным числом";
			}
			return "Ապրանքի փաստացի քանակը չի կարող լինել բացասական թիվ";
		}
	}

	public static string ErrNegOrZeroQuantity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's quantity cannot be negative number";
				}
				return "Количество товара не может быть не положительным числом";
			}
			return "Ապրանքի քանակը չի կարող լինել ոչ դրական թիվ";
		}
	}

	public static string ErrorMTBarCodeEdition
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" product or service has more than one barcode. Editing barcode is not allowed.";
				}
				return "Товар или услуга \"{0}\" имеет больше одного штрухкода. Редактирование штрихкода невозможна";
			}
			return "\"{0}\" կոդով ապրանքը կամ ծառայությունը ունի մեկից ավելի գծիկավոր կոդ, գծ. կոդի խմբագրում հնարավոր չէ";
		}
	}

	public static string ErrorOutStorageFromFile
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For importing products list, output storage should be mentioned.";
				}
				return "При импорте списка товаров необходимо указать склад расхода";
			}
			return "Ապրանքների ցուցակ ներմուծելիս անհրաժեշտ է նշել ելքի պահեստը";
		}
	}

	public static string ErrorRetailStorageFromFile
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For importing products list, storage should be retail type.";
				}
				return "При импорте списка товаров необходимо указать склад розничного типа";
			}
			return "Ապրանքների ցուցակ ներմուծելիս անհրաժեշտ է նշել մանրածախ տեսակի պահեստ";
		}
	}

	public static string ErrorServiceBarCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} is service Barcode";
				}
				return "{0} является штрихкодом услуги";
			}
			return "{0} ծառայության գծիկավոր կոդ է";
		}
	}

	public static string ErrorStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For using scanner You should choose wholesale storage.";
				}
				return "Для работы со сканером необходимо указывать склад оптового типа";
			}
			return "Սկաներով աշխատելու համար անհրաժեշտ է նշել մեծածախ տեսակի պահեստ";
		}
	}

	public static string ErrorStorageFromFile
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For importing products list, storage should be wholesale type.";
				}
				return "При импорте списка товаров необходимо указывать склад оптового типа";
			}
			return "Ապրանքների ցուցակ ներմուծելիս անհրաժեշտ է նշել մեծածախ տեսակի պահեստ";
		}
	}

	public static string ErrorStoringDocument
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" document's registration error - {1}";
				}
				return "Ошибка записи документа \"{0}\" - {1}";
			}
			return "\"{0}\" փաստաթղթի գրանցման սխալ` {1}";
		}
	}

	public static string ErrOutStorageScaner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For using scanner You should choose output storage.";
				}
				return "Для работы со сканером необходимо указывать склад расхода";
			}
			return "Սկաներով աշխատելու համար անհրաժեշտ է նշել ելքի պահեստը";
		}
	}

	public static string ErrPartnerContractsAreTheSame
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners and their contracts are the same";
				}
				return "Партнеры и их договора совпадают";
			}
			return "Գործընկերները և նրանց պայմանագրերը համընկնում են";
		}
	}

	public static string ErrPartnerNotCustomer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The partner with '{0}' code is not customer";
				}
				return "Партнер '{0}' не покупатель";
			}
			return "'{0}' կոդով գործընկերը գնորդ  չէ";
		}
	}

	public static string ErrPartnerNotSupplier
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The partner with '{0}' code is not supplier";
				}
				return "Партнер '{0}' не поставщик";
			}
			return "'{0}' կոդով գործընկերը մատակարար չէ";
		}
	}

	public static string ErrPrePaymentSumGreaterTotal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment redemption amount is greater than total amount";
				}
				return "Сумма зачета аванса больше итоговой суммы";
			}
			return "Կանխավճարի մարման գումարը մեծ է ընդհանուր գումարից";
		}
	}

	public static string ErrPriceListPttelOutPut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price list with different price types can be created by exporting to MS Excel";
				}
				return "Прайс-лист по нескольким типам цен можно получить экспортом в MS Excel";
			}
			return "Մի քանի գների տեսակներով գնացուցակ կարելի է ստանալ MS Excel արտահանելով";
		}
	}

	public static string ErrPrintSpoolerStopped
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Make sure \"Print Spooler\" service is started";
				}
				return "Убедитесь, что сервис \"Print Spooler\" работает";
			}
			return "Համոզվեք, որ \"Print Spooler\" ծառայությունն աշխատում է";
		}
	}

	public static string ErrQTYCannotBeMore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quantity of returned product ({0} - {1}) cannot exceed the quantity in the base document.";
				}
				return "Количество возвращаемого товара ({0} - {1}) не может превышать количество в документе основания";
			}
			return "Վերադարձվող ապրանքի ({0} - {1}) քանակը չի կարող գերազանցել հիմք-փաստաթղթում եղած քանակը:";
		}
	}

	public static string ErrRetailStorageScaner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For using scanner You should choose retail storage.";
				}
				return "Для работы со сканером необходимо указывать склад розничного типа";
			}
			return "Սկաներով աշխատելու համար անհրաժեշտ է նշել մանրածախ տեսակի պահեստ";
		}
	}

	public static string ErrWeightItemBarCodeLen
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Error WeightItem BarCode Length";
				}
				return "Длина штрихкода весового товара отличается от значения указанной в настройках";
			}
			return "Քաշային ապրանքի գծ. կոդի երկարությունը տարբերվում է դրույթներում նշված արժեքից";
		}
	}

	public static string ErrWrongLineFormat
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wrong row format";
				}
				return "Формат строки неверный";
			}
			return "Տողի ֆորմատը սխալ է";
		}
	}

	public static string ErrYearClosMustExclusive
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Archive must be made in exclusive mode";
				}
				return "Архивация должна проводиться в монопольном режиме";
			}
			return "Արխիվացումը պետք է կատարվի մենաշնորհային ռեժիմով";
		}
	}

	public static string ExchangeCertainProblem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "An error occured during data exchange";
				}
				return "Произошла ошибка во время обмена данных";
			}
			return "Տվյալների փոխանակման ժամանակ տեղի ունեցավ սխալ";
		}
	}

	public static string ExchangeSuccessfulEnd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data exchange completed successfully";
				}
				return "Обмен данными прошел успешно";
			}
			return "Տվյալների փոխանակումը անցավ բարեհաջող";
		}
	}

	public static string ExistBankDocWithSpecifiedCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank document with {0} bank code exists";
				}
				return "С кодом банковского счета \"{0}\" существует документ банковского типа";
			}
			return "\"{0}\" հաշվարկային հաշվի կոդով գոյություն ունի բանկային տիպի փաստաթուղթ:";
		}
	}

	public static string ExistCashDocWithSpecifiedCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash document with {0} cash desk exists";
				}
				return "С кодом кассы \"{0}\" существует кассовый документ";
			}
			return "\"{0}\" դրամարկղի կոդով գոյություն ունի դրամարկղային փաստաթուղթ:";
		}
	}

	public static string ExistDocByBaseISN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exist child document for the chosen base document";
				}
				return "На основании выбранного документа создан подчиненный документ";
			}
			return "Ընտրված փաստաթղթի հիման վրա ստեղծված է դուստր փաստաթուղթ";
		}
	}

	public static string ExistDocumentWithSpecifiedEmployee
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document with {0} employee exists";
				}
				return "С сотрудником \"{0}\" существует документ";
			}
			return "\"{0}\" աշխատակցով գոյություն ունի փաստաթուղթ";
		}
	}

	public static string ExistDocumentWithSpecifiedMaterial
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document with {0} product or service exists";
				}
				return "С товаром или услугой \"{0}\" существует документ";
			}
			return "\"{0}\" ապրանքով կամ ծառայությունով գոյություն ունի փաստաթուղթ";
		}
	}

	public static string ExistInvoiceWithSN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Invoice with {0} serial and number already exists ({1})";
				}
				return "Документ Счет-Фактура с серия-номером {0} существует ({1})";
			}
			return "{0} սերիայով և համարով հաշիվ ապրանքագիր գոյություն ունի ({1})";
		}
	}

	public static string ExistMTMoveWithSN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product movement with serial number {0} exists ({1})";
				}
				return "Документ Перемещение товаров с серия-номером {0} существует ({1})";
			}
			return "{0} սերիայով և համարով ՆԱ տեղաշարժ փաստաթուղթ գոյություն ունի ({1})";
		}
	}

	public static string ExistOpenCashierShiftWithCashdesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is an open cashier shift with \"{0}\" cashdesk.";
				}
				return "Для текущей кассы существует открытая смена {0}.";
			}
			return "\"{0}\" դրամարկղով արդեն կա բաց հերթափոխ:";
		}
	}

	public static string ExistOperWithCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exist operations with specified cashier shift";
				}
				return "Существуют связанные со сменой операции. Удаление невозможно";
			}
			return "Հերթափոխի հետ կապված կան գործողություններ:Հեռացնելն արգելվում է";
		}
	}

	public static string ExistPartMTForPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For selected partner products for supply exists";
				}
				return "По данному партнеру существуют товары поставки";
			}
			return "Տրված գործընկերով գոյություն ունեն մատակարարման ապրանքներ";
		}
	}

	public static string ExistPartMTForPartnerWithCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" partner has products for supply";
				}
				return "По {0} партнеру существуют товары поставки";
			}
			return "\"{0}\" գործընկերով գոյություն ունեն մատակարարման ապրանքներ";
		}
	}

	public static string ExistPasiveRem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" has negative balance \"{1}\"";
				}
				return "\"{0}\" остаток {1} отрицательный";
			}
			return "\"{0}\" մնացորդը {1} բացասական է";
		}
	}

	public static string ExistPostponedSaleDocs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are delayed docs. You are not allowed to close cashier shift";
				}
				return "Имеются отложенные документы. Закрытие кассовой смены запрещено.";
			}
			return "Կան հետաձգված փաստաթղթեր: Հերթափոխը չի թույլատրվում փակել";
		}
	}

	public static string ExistPriceWithPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price exists with {0} price type";
				}
				return "По типу цены \"{0}\" установлена цена";
			}
			return "\"{0}\" գնի տեսակով սահմանված է գին";
		}
	}

	public static string ExistsDocBasedBeforeClosingDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Archivation on {0} cannot be made, because after that date document \"{1}\" exists which is based on document registered before archivation date.";
				}
				return "Не возможно выполнить архивацию датой {0}, так как после этой даты существует \"{1}\" документ, который основывается на документа, созданный до архивации";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվից հետո գոյություն ունի \"{1}\" փաստաթուղթ, որը հիմնված է արխիվացման ամսաթվից առաջ գրանցված փաստաթղթի վրա";
		}
	}

	public static string ExistsMTRetWithOutBaseInvNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operation is not allowed, because values of document’s base serial number and submission date are empty";
				}
				return "Не разрешается выполнить операцию, так как не заполнены поля серия, номер и дата выписки документа основания";
			}
			return "Գործողությունը չի թույլատրվում կատարել, քանի որ փաստաթղթի հիմք սերիա, համար  և դուրսգրման ա/թ դաշտերի արժեքները լրացված չեն";
		}
	}

	public static string ExistsRowWithZeroAmount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operation is not allowed: in document’s item list row with 0 amount exists";
				}
				return "Не разрешается выполнить операцию, так как в списке товаров документа обнаружены строки с 0-ой суммой";
			}
			return "Գործողությունը չի թույլատրվում կատարել, քանի որ փաստաթղթի անվանացուցակում հայտնաբերվել է 0-ական գումարով տող:";
		}
	}

	public static string ExistTypeOfDocumentWithSpecifiedMaterial
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{1} type of document with {0} product or service exists";
				}
				return "С товаром или услугой \"{0}\" существует документ типа \"{1}\"";
			}
			return "\"{0}\" ապրանքով կամ ծառայությունով գոյություն ունի \"{1}\" տեսակի փաստաթուղթ";
		}
	}

	public static string ExistUnitForPartnerAndMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For the supplier {0} product 's {1} measurement unit {2} exists";
				}
				return "У товара {1} поставщика {0} существует единица измерения  {2} ";
			}
			return "{0} մատակարարի {1} կոդով ապրանքի {2} չ/մ գոյություն ունի";
		}
	}

	public static string ExistUnitForPartnerAndService
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For the supplier {0} service 's {1} measurement unit {2} exists";
				}
				return "У услуги {1} поставщика {0} существует единица измерения  {2}";
			}
			return "{0} մատակարարի {1} կոդով ծառայության {2} չ/մ գոյություն ունի";
		}
	}

	public static string ExitMTForPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Supplier’s {0} Item {1} exists";
				}
				return "Товар {1} поставщика {0} существует";
			}
			return "{0} մատակարարի {1} կոդով ապրանք գոյություն ունի";
		}
	}

	public static string ExitServiceForPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Supplier’s {0} Service {1} exists";
				}
				return "Услуга {1} поставщика {0} существует";
			}
			return "{0} մատակարարի {1} կոդով ծառայություն գոյություն ունի";
		}
	}

	public static string ExportedToDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exported to b2b.armsoft DS";
				}
				return "Экспортировано в b2b.armsoft БС";
			}
			return "Արտահանված է b2b.armsoft ԲՍ";
		}
	}

	public static string ExportElScaleNextCodeAs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export to el. scale {0} as a next code";
				}
				return "В эл. весах экспортировать {0} как порядковый номер";
			}
			return "Էլ. կշեռքում որպես {0}հերթական կոդ արտահանել";
		}
	}

	public static string ExportForAltUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export for additional unit of measure";
				}
				return "Экспортировать в дополнительных единицах";
			}
			return "Արտահանել լրացուցիչ չ/մ-ի համար";
		}
	}

	public static string ExportForScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export in scale format";
				}
				return "Экспорт в формате эл. весов";
			}
			return "Արտահանել էլ. կշեռքի ֆորմատով";
		}
	}

	public static string ExportForTerminal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export with terminal format";
				}
				return "Экспортировать  в формате ТСД (терминала  сбора данных)";
			}
			return "Արտահանել տերմինալի ֆորմատով";
		}
	}

	public static string ExportToDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export To Discount Server";
				}
				return "Экспортировать в Бонусный сервер";
			}
			return "Արտահանել Բոնուսային սերվեր";
		}
	}

	public static string ExternalUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "External unit";
				}
				return "Внешняя единица";
			}
			return "Արտաքին միավոր";
		}
	}

	public static string Extra
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Added value %";
				}
				return "% розничной наценки";
			}
			return "Հավելագին %";
		}
	}

	public static string FavoritsShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quick pick";
				}
				return "Избран.";
			}
			return "Ընտրյալ";
		}
	}

	public static string FCDiscountedPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discounted price in curr.";
				}
				return "Цена со скидкой в валюте";
			}
			return "Զեղչված գին արտ.";
		}
	}

	public static string FCDiscPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discounted price in curr.";
				}
				return "Цена со скидкой в валюте";
			}
			return "Զեղչված գին արտարժ.";
		}
	}

	public static string FEFODistibute
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "FEFO";
				}
				return "FEFO";
			}
			return "Բաշխել ըստ Օգտ. ժամկ.";
		}
	}

	public static string FillAllAvailability
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill all availability?";
				}
				return "Заполнить все наличие?";
			}
			return "Լրացնե՞լ ամբողջ առկայությունը";
		}
	}

	public static string FillCashierShiftNextNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill cashier shift next number for \"{0}\" cashdesk";
				}
				return "Для открытия или закрытия смены необходимо заполнить \"Порядковый номер смены\" для кассы \"{0}\"";
			}
			return "Հերթափոխ բացելու կամ փակելու համար \"{0}\" դրամարկղի համար լրացրեք Հերթափոխի N դաշտը:";
		}
	}

	public static string FillCurrentPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill current prices";
				}
				return "Заполнить текущие цены";
			}
			return "Լրաց․ գործող գները";
		}
	}

	public static string FillDefaultCashDeskForClosingCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill user default cash desk for closing cashier shift";
				}
				return "Для закрытия смены заполните кассу пользователя";
			}
			return "Հերթափոխ փակելու համար լրացրեք օգտագործողի լռությամբ դրամարկղը ";
		}
	}

	public static string FillDefaultCashDeskForOpeningCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill user default cash desk for opening cashier shift";
				}
				return "Для открытия смены заполните кассу пользователя";
			}
			return "Հերթափոխ բացելու համար լրացրեք օգտագործողի լռությամբ դրամարկղը";
		}
	}

	public static string FillFullCaptionWithCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export caption instead of full caption";
				}
				return "Имя экспортировать как полное имя";
			}
			return "Որպես լրիվ անվանում արտահանել անվանումը";
		}
	}

	public static string FillLatestPurchasePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill the latest purchase prices";
				}
				return "Заполнить по ценам последней закупки";
			}
			return "Լրացնել վերջին ձեռքբերման գներով";
		}
	}

	public static string FillNewPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill new price";
				}
				return "Заполнить новую цену";
			}
			return "Լրացնել նոր գինը";
		}
	}

	public static string FillOnlyOneField
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill only one of the fields";
				}
				return "Нужно заполнить только одно из полей";
			}
			return "Անհրաժեշտ է լրացնել դաշտերից միայն մեկը";
		}
	}

	public static string FillUsingBaseQ
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill documents fields by base document field values?";
				}
				return "Заполнить поля документа значениями из документа основания?";
			}
			return "Լրացնե՞լ փաստաթղթի դաշտերը հիմք փաստաթղթի դաշտերի արժեքներով";
		}
	}

	public static string FillZeroForActualQuantity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Do you want to fill zero actual quantity ?";
				}
				return "Заполнить фактические количества нулями?";
			}
			return "Փաստացի քանակները լրացնե՞լ զրո:";
		}
	}

	public static string FindByTaxCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Find by Tax Code";
				}
				return "Поиск по ИНН";
			}
			return "Փնտրել ՀՎՀՀ-ով";
		}
	}

	public static string FingerPrint
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Computer Fingerprint";
				}
				return "Отпечаток компьютера (Fingerprint)";
			}
			return "Համակարգչի նույնացուցիչ (Fingerprint)";
		}
	}

	public static string FiscalNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fiscal number";
				}
				return "Фискальный номер";
			}
			return "Ֆիսկալ համար";
		}
	}

	public static string FixedPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fixed price";
				}
				return "Фиксированная цена";
			}
			return "Հաստատուն գին";
		}
	}

	public static string ForDayOrderNumberChangeDocMustBeDraft
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "To change day order number the document must be in draft mode.";
				}
				return "П/н дня можно изменить только для документа черновика";
			}
			return "Օրվա հ/հ-ը կարելի է փոխել միայն  սևագիր փաստաթղթի համար";
		}
	}

	public static string ForeignPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Foreign partner";
				}
				return "Иностранец";
			}
			return "Օտարերկրացի";
		}
	}

	public static string FormingForAltUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Form for additional unit of measure";
				}
				return "Сформировать для дополнительных единиц";
			}
			return "Ձևավորել լրացուցիչ չ/մ-ի համար";
		}
	}

	public static string FormingPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Forming price type";
				}
				return "Формируемые цены";
			}
			return "Ձևավորվող գնի տեսակ";
		}
	}

	public static string FormingPriceCurrVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Forming price type: {0}\r\n({1}, {2})";
				}
				return "Формируемые цены:\r\n{0}\r\n({1}, {2})";
			}
			return "Ձևավորվող գնի տեսակ`\r\n     {0}\r\n     ({1}, {2})";
		}
	}

	public static string FormingPriceOldValues
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Current values of forming price";
				}
				return "Действующие значения формируемых цен";
			}
			return "Ձևավորվող գնի գործող արժեքներ";
		}
	}

	public static string ForPartMTNotExistMap
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For Item {1} of supplier {0} inner product code is not matched";
				}
				return "Для товара с кодом {1} от поставщика {0} не указано соответствие с внутренним кодом ";
			}
			return "{0}  մատակարարի {1} կոդով ապրանքի համար համապատասխանեցված չէ ներքին ապրանքի կոդ";
		}
	}

	public static string ForPay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount Due";
				}
				return "К оплате";
			}
			return "Վճարման";
		}
	}

	public static string ForPayShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "To pay";
				}
				return "К оплате";
			}
			return "Վճ. ենթ.";
		}
	}

	public static string ForPrintPrevMustSaveDoc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You must save the document for viewing it";
				}
				return "Для просмотра документа необходимо его зарегистрировать";
			}
			return "Փաստաթուղթը դիտելու համար այն անհրաժեշտ է գրանցել";
		}
	}

	public static string ForReportMustSaveDoc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For generating report  the document must be saved";
				}
				return "Для генерирования отчета документ необходимо зарегистрировать";
			}
			return "Հաշվետվությունը  գեներացնելու համար փաստաթուղթն անհրաժեշտ է գրանցել:";
		}
	}

	public static string ForRowSelectedParties
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For this row selected batches, after editing quantity batches will be deleted";
				}
				return "Для данной строки количество введено вручную на уровне партий. После редактирования количества в строке эти данные будут изменены.";
			}
			return "Այս տողի համար ընտրված են խմբաքանակներ, քանակը խմբագրելուց հետո այդ խմբաքանակները կհեռացվեն";
		}
	}

	public static string FromAdvanceHolder1Act
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unilateral purch. act";
				}
				return "Закупка через подотчетное лицо (односторонний акт)";
			}
			return "Առհաշվից` միակողմանի գնման ակտով";
		}
	}

	public static string FromAdvanceHolder2Act
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bilateral purch. act";
				}
				return "Закупка через подотчетное лицо (двусторонний акт)";
			}
			return "Առհաշվից` երկկողմանի գնման ակտով";
		}
	}

	public static string FromShopWithCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Without taxes";
				}
				return "Без налогов";
			}
			return "Առանց հարկերի";
		}
	}

	public static string FromSuplyerWithInvoice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "From supplier with invoice";
				}
				return "От поставщика на основе С/Ф";
			}
			return "Մատակարարից` Հ/Ա-ով";
		}
	}

	public static string GenerateBarcode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Generate barcode";
				}
				return "Добавить новую строку";
			}
			return "Գեներացնել գծիկավոր կոդ";
		}
	}

	public static string GenerateBarcodeCompleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Barcode assignment completed. Number of assigned codes: {0}";
				}
				return "Генерация штрихкодов завершена. Количество генерированных кодов {0}";
			}
			return "Գծիկավոր կոդերի գեներացումն ավարտվեց: Գեներացված գծ. կոդերի քանակը` {0}";
		}
	}

	public static string GenerateBarcodeConfirm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For generating new barcode, field’s value \"{0}\" will be deleted. Continue?";
				}
				return "При генерировании нового штрихкода значение поля \"{0}\" будет удалено. Продолжить?";
			}
			return "Նոր գծիկավոր կոդ գեներացնելիս դաշտի \"{0}\" արժեքը կհեռացվի: Շարունակե՞լ:";
		}
	}

	public static string GenerateBonusAccumulationDocument
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Create Bonuses group revision documents on accumulated bonuses";
				}
				return "Формирования сводных документов по корректировке бонусных баллов на основе начисленных бонусов";
			}
			return "Կուտակված բոնուսների հիման վրա բոնուսի խմբային վերանայում փաստաթղթերի ձևավորում";
		}
	}

	public static string GenerateOnCreditPaymentsDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Create Revise partner debt (group) documents on Credit payments";
				}
				return "Формирования сводных документов по корректировке задолжностей партнеров на основе оплата кредитами";
			}
			return "Ապառիկ վճարումների հիման վրա գործընկերոջ պարտքի խմբային վերանայում փաստաթղթերի ձևավորում";
		}
	}

	public static string GeneratePrepaymentsDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Create Revise partner debt (group) documents on Prepayments";
				}
				return "Формирования сводных документов по корректировке задолжностей партнеров на основе зачетов аванса";
			}
			return "Կանխավճարի մարումների հիման վրա գործընկերոջ պարտքի խմբային վերանայում փաստաթղթերի ձևավորում";
		}
	}

	public static string GenerateReturnPaymentsDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Create Revise partner debt (group) documents on Return documents";
				}
				return "Формирования сводных документов по корректировке задолжностей партнеров на основе возвратов";
			}
			return "Վերադարձների հիման վրա գործընկերոջ պարտքի խմբային վերանայում փաստաթղթերի ձևավորում";
		}
	}

	public static string GenerateUsedBonusDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Create Bonuses group revision documents on bonus payments";
				}
				return "Формирования сводных документов по корректировке бонусных баллов на основе оплат бонусом";
			}
			return "Բոնուսի մարումների հիման վրա բոնուսի խմբային վերանայում փաստաթղթերի ձևավորում";
		}
	}

	public static string GetCashierShiftsReadyForExport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Get cashier shifts ready for export";
				}
				return "Формирования списка смен участвующие в обмене";
			}
			return "Փոխանակման ենթակա հերթափոխերի ստացում";
		}
	}

	public static string GetDocumentGroupsByCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group the sale and return documents of \"{0}\" cashdesk's number \"{1}\" shift";
				}
				return "Свод документов продаж и возвратов произведенных в кассе \"{0}\" за смену \"{1}\"";
			}
			return "\"{0}\" դրամարկղով \"{1}\" համարի հերթափոխի վաճառքի և վերադարձի փաստաթղթերի խմբավորում";
		}
	}

	public static string GetDocumentsByCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Create grouped sales and return documents";
				}
				return "Формирования сводных документов продаж и возврата";
			}
			return "Խմբավորված վաճառքի և վերադարձի փաստաթղթերի ձևավորում";
		}
	}

	public static string GetPaymentsByCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Collection of payments of \"{0}\" cashdesk's number \"{1}\" shift";
				}
				return "Сбор оплат произведенных в кассе \"{0}\" за смену \"{1}\"";
			}
			return "\"{0}\" դրամարկղով \"{1}\" համարի հերթափոխի վճարումների հավաքագրում";
		}
	}

	public static string GiveDiscount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "To discount";
				}
				return "Рассчитать скидку";
			}
			return "Զեղչել";
		}
	}

	public static string GoodOrServiceExistsWithCPA
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "With mentioned CPA /CNFEA classifier, product or service exists";
				}
				return "Обнаружен товар или услуга с указанным кодом из классификатора ОКПД/ТНВЭД ";
			}
			return "Տվյալ ԱԴԳՏ/ԱԴԳԱԱ դասակարգչով գոյություն ունի ապրանք կամ ծառայություն";
		}
	}

	public static string Goods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products";
				}
				return "Товары";
			}
			return "Ապրանքներ";
		}
	}

	public static string GoodsCostCalculation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost calculation report";
				}
				return "Расчет себестоимости приобретения";
			}
			return "Ձեռքբերման ինքնարժեքի հաշվարկ";
		}
	}

	public static string GoodsCostCalculationHeader
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Purchase cost calculation ({0})";
				}
				return "Расчет себестоимости приобретения ({0})";
			}
			return "Ձեռքբերման ինքնարժեքի հաշվարկ ({0})";
		}
	}

	public static string GoodsCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quantity of products";
				}
				return "Количество товаров";
			}
			return "Ապրանքների քանակը";
		}
	}

	public static string GoodsDirectories
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services subdirectories";
				}
				return "Подчиненные справочники (товары и услуги)";
			}
			return "Ապրանքների և ծառայությունների ենթատեղեկատուներ";
		}
	}

	public static string GoodServiceWithNameDoesNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product or service with the name \"{0}\" does not exist.";
				}
				return "Товара или услуги с наименованием \"{0}\" не существует";
			}
			return "\"{0}\" անվանումով ապրանք կամ ծառայություն գոյություն չունի";
		}
	}

	public static string GoodsRem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balance";
				}
				return "Остаток";
			}
			return "Մնացորդ";
		}
	}

	public static string GoodsStockTakingRep
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products stocktaking journal";
				}
				return "Инвентаризации товаров";
			}
			return "Ապրանքների գույքագրման մատյան";
		}
	}

	public static string GoodUnitCost
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unit cost";
				}
				return "Себестоимость единицы";
			}
			return "Միավորի ինքնարժեք";
		}
	}

	public static string GoodWithNameDoesNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product with the name \"{0}\" does not exist.";
				}
				return "Товара с наименованием \"{0}\" не существует";
			}
			return "\"{0}\" անվանումով ապրանք գոյություն չունի";
		}
	}

	public static string GridColumnAutoWidth
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Column's auto width";
				}
				return "Автоматически растягивать колонки по ширине списка";
			}
			return "Լայնքի ավտոկարգավորում";
		}
	}

	public static string Groove
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The groove";
				}
				return "Детализация";
			}
			return "Բացվածք";
		}
	}

	public static string GroupAddition
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Grp. addition";
				}
				return "Групп. добавление";
			}
			return "Խմբ. ավելացում";
		}
	}

	public static string GroupBy
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group by";
				}
				return "Группировать по";
			}
			return "Խմբավորել ըստ";
		}
	}

	public static string GroupByContract
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group by contract";
				}
				return "Группировать по договору";
			}
			return "Խմբավորել ըստ պայմանագրի";
		}
	}

	public static string GroupByPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group by partner";
				}
				return "Группировать по партнерам";
			}
			return "Խմբավորել ըստ գործընկերոջ";
		}
	}

	public static string GroupsName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group's name";
				}
				return "Имя группы";
			}
			return "Խմբի անվանում";
		}
	}

	public static string GroupTreeView
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group (Tree-view)";
				}
				return "Группа (дерево)";
			}
			return "Խումբ (ծառատիպ)";
		}
	}

	public static string GrpByMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products";
				}
				return "Товарам";
			}
			return "Ապրանքների";
		}
	}

	public static string GrpByMTAndServices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products And Services";
				}
				return "Товарам и услугам";
			}
			return "Ապրանքների և ծառայությունների";
		}
	}

	public static string GrpByPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners";
				}
				return "Партнерам";
			}
			return "Գործընկերների";
		}
	}

	public static string HlpAbout
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "About AS-Trade";
				}
				return "О программе AS-Trade";
			}
			return "ՀԾ-Առևտուր համակարգի մասին";
		}
	}

	public static string HotKeys
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Hot keys";
				}
				return "Горячие клавиши";
			}
			return "Արագ հասանելիության ստեղներ";
		}
	}

	public static string HotKeysMustDiffer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Different commands cannot have same hot keys";
				}
				return "Разные команды не могут иметь одинаковые горячие клавиши";
			}
			return "Տարբեր հրամաններ չեն կարող ունենալ նույն արագ հասանելիության ստեղնը";
		}
	}

	public static string IDCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "IDCard";
				}
				return "Идентификационнaя карточкa";
			}
			return "Նույնականացման քարտ";
		}
	}

	public static string IDCardTemplateName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner's ID Card";
				}
				return "Идентификационная карточка покупателя";
			}
			return "Գնորդի ID քարտ";
		}
	}

	public static string Import
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products list import";
				}
				return "Импорт списка товаров";
			}
			return "Ապրանքների ցուցակի ներմուծում";
		}
	}

	public static string ImportExchangeRates
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import exchange rates";
				}
				return "Импорт курса валют";
			}
			return "Ներմուծել արտարժույթի փոխարժեքները";
		}
	}

	public static string ImportFromB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import from b2b.armsoft.am";
				}
				return "Импортировать из b2b.armsoft ";
			}
			return "Ներմուծել b2b.armsoft հարթակից";
		}
	}

	public static string ImportFromDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import from Discount Server";
				}
				return "Импортировать из Бонусного сервера";
			}
			return "Ներմուծել Բոնուսային սերվերից";
		}
	}

	public static string ImportFromFileErrors
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products list import errors";
				}
				return "Ошибки импорта списка товаров";
			}
			return "Ապրանքների ցուցակի ներմուծման սխալներ";
		}
	}

	public static string ImportPartnersMTFromB2b
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import products from b2b.armsoft.am";
				}
				return "Импорт списка товаров из b2b.armsoft ";
			}
			return "Ապրանքացանկի ներմուծում b2b.armsoft հարթակից";
		}
	}

	public static string ImportResults
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import results";
				}
				return "Результаты импорта";
			}
			return "Ներմուծման արդյունքներ";
		}
	}

	public static string ImportToBackOffice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import data from Point of sale (POS)";
				}
				return "Импортировать данные из POS";
			}
			return "Ներմուծել տվյալներ Վաճառակետերից (POS)";
		}
	}

	public static string IncorrectPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price type code should be different from \"{0}\".";
				}
				return "Код типа цены должен быть отличным от \"{0}\"";
			}
			return "Գնի տեսակի կոդը պետք է լինի տարբեր \"{0}\"-ից";
		}
	}

	public static string Increase
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Qty +";
				}
				return "Кол. +";
			}
			return "Քան. +";
		}
	}

	public static string IncreasedBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Increase of bonus";
				}
				return "Увеличение бонуса";
			}
			return "Բոնուսի աճ";
		}
	}

	public static string IncrementSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The amount must be greater than \"{0}\"";
				}
				return "Сумма должна быть больше \"{0}\"";
			}
			return "Գումարը պետք է լինի մեծ \"{0}\"-ից";
		}
	}

	public static string InCurrencyShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "currency";
				}
				return "Валюта";
			}
			return "արտարժ.";
		}
	}

	public static string InitRem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Initial balance";
				}
				return "Начальный остаток";
			}
			return "Սկզբնական մնացորդը";
		}
	}

	public static string InitRemainsAMD
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Starting balance in AMD";
				}
				return "Начальный остаток в драмах";
			}
			return "Սկզբնական մնացորդ դրամով";
		}
	}

	public static string InitRemainsCur
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Starting balance in currency";
				}
				return "Начальный остаток в валюте";
			}
			return "Սկզբնական մնացորդ արժույթով";
		}
	}

	public static string InMTComplect
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You cannot delete the \"{0}\" product, as it is an ingredient of \"{1}\" kit";
				}
				return "Удаление товара \"{0}\" запрещено, так как он является частью комплекта \"{1}\"";
			}
			return "\"{0}\" ապրանքը չի թույլատրվում հեռացնել, քանի որ այն հանդիսանում է \"{1}\" կոմպլեկտի բաղադրամաս";
		}
	}

	public static string InMTComplectExistsQNT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exists ingredient  in given measurement unit";
				}
				return "Существует комплектующая в данной единице измерения";
			}
			return "Գոյություն ունի տվյալ չափման միավորով կոմպլեկտի բաղադրամաս";
		}
	}

	public static string InMTHI
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is registered operation with {0} product";
				}
				return "С товаром \"{0}\" зарегистрирована операция";
			}
			return "\"{0}\" ապրանքով գրանցված է գործողություն";
		}
	}

	public static string InputBoxCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enter an value";
				}
				return "Ввести значение";
			}
			return "Մուտքագրել արժեք";
		}
	}

	public static string InputBoxIsEmpty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Entered field value is empty";
				}
				return "Введите значение в поле ввода!";
			}
			return "Մուտքագրման դաշտի արժեքը դատարկ է";
		}
	}

	public static string InputQtyMustBeGreater
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Inputed quantity must be greater than quantity in current row";
				}
				return "Вводимое количество должно быть больше чем количество в текущей строке";
			}
			return "Մուտքագրվող քանակը պետք է մեծ լինի ընթացիկ տողում նշվածից";
		}
	}

	public static string InStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Storage input";
				}
				return "В склад";
			}
			return "Պահեստ մուտքի";
		}
	}

	public static string IntegrityCheckBegin
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data integrity check begin";
				}
				return "Начало проверки целостности данных";
			}
			return "Տվյալների ամբողջականության ստուգման սկիզբ";
		}
	}

	public static string IntegrityCheckEnd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data integrity check end";
				}
				return "Конец проверки целостности данных";
			}
			return "Տվյալների ամբողջականության ստուգման ավարտ";
		}
	}

	public static string InternalBarcodeEANFormat
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "EAN format";
				}
				return "Формат EAN";
			}
			return "EAN ֆորմատ";
		}
	}

	public static string InternalBarcodeNext
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Next barcode";
				}
				return "Порядковое значение штрихкода";
			}
			return "Հերթական գծիկավոր կոդ";
		}
	}

	public static string InternalBarcodeParams
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Options of products accounted by unit";
				}
				return "Параметры генерации штрих-кодов";
			}
			return "Գծիկավոր կոդերի գեներացման պարամետրեր";
		}
	}

	public static string InvalidCardCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wrong card code";
				}
				return "Неверный код карты";
			}
			return "Սխալ քարտի կոդ";
		}
	}

	public static string InvalidGoodsType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Product or service type (\"{1}\") is wrong";
				}
				return "Неправильный вид (\"{1}\") товара или услуги \"{0}\"";
			}
			return "\"{0}\" Ապրանքի կամ ծառայության տեսակը (\"{1}\") սխալ է";
		}
	}

	public static string InvalidMTQntUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product or service with \"{0}\" code does not have  unit of measure with \"{1}\" code";
				}
				return "Товар или услуга с кодом \"{0}\" не имеет единицу измерения с кодом \"{1}\"";
			}
			return "\"{0}\" կոդով ապրանքը կամ ծառայությունը չունի \"{1}\" կոդով չափման միավոր";
		}
	}

	public static string InvalidMTQntUnitBrief
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Product or service has no unit of measure with \"{1}” abbreviation.";
				}
				return "Товар или услуга с кодом \"{0}\" не имеет единицу измерения с сокращением \"{1}\"";
			}
			return "\"{0}\" կոդով ապրանքը կամ ծառայությունը չունի \"{1}\" հապավումով չափման միավոր";
		}
	}

	public static string InvalidQntUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "This unit of measure does not registered for this product";
				}
				return "Для товара не зарегистрирована указанная единица измерения";
			}
			return "Ապրանքի համար գրանցված չէ նշված չափման միավորը";
		}
	}

	public static string InvalidRecAccKey
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receiver settlement account is incorrect.";
				}
				return "Неверный ключ подписанного счета ";
			}
			return "Ստացողի հաշվի բանալին սխալ է";
		}
	}

	public static string InvoiceJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Tax invoices journal";
				}
				return "Налоговые счета";
			}
			return "Հարկային հաշիվներ";
		}
	}

	public static string InvoiceNotForExport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "This invoice cannot be exported";
				}
				return "Отмеченный налоговый счет не подлежит экспорту";
			}
			return "Նշված հարկային հաշիվը արտահանման ենթակա չէ";
		}
	}

	public static string InvoiceNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Purchase document N";
				}
				return "Номер С/Ф";
			}
			return "Հ/Ա-ի համար";
		}
	}

	public static string InvoiceRowExistsQNT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exists invoice's row in given measurement unit";
				}
				return "Существует строка С/Ф в данной единице измерения";
			}
			return "Գոյություն ունի տվյալ չափման միավորով Հ/Ա-ի տող";
		}
	}

	public static string IsCanceled
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Canceled";
				}
				return "Отменено";
			}
			return "Չեղարկված";
		}
	}

	public static string IsConnectedTob2barmsoftDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Is connected to b2b.armsoft Discount server";
				}
				return "Подключен к b2b.armsoft Бонусному серверу";
			}
			return "Միացված է b2b.armsoft Բոնուսային սերվերին";
		}
	}

	public static string IsDone
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Is done";
				}
				return "Выполнено";
			}
			return "Կատարված է";
		}
	}

	public static string IsNotExistReportForDoc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is no report for {0} document";
				}
				return "Для {0} документы отчеты не отображаются";
			}
			return "{0} փաստաթղթի համար ցուցադրման ենթակա հաշվետվություններ չկան";
		}
	}

	public static string IsNotNumeric
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Entered value is not numerical";
				}
				return "Внесенное значение не является цифровым";
			}
			return "Մուտքագրված արժեքը թվային չէ";
		}
	}

	public static string ItemCodeLength
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Item Code Length";
				}
				return "Длина штрихкода";
			}
			return "Գծ.կոդի երկարություն";
		}
	}

	public static string ItemPrecision
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Item Precision";
				}
				return "Кол. символов после запятой";
			}
			return "Ստոր.-ից հետո նիշերի քանակ";
		}
	}

	public static string JournalPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prices Journal";
				}
				return "Журнал цен";
			}
			return "Գների մատյան";
		}
	}

	public static string KassaOp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks' operations";
				}
				return "Операции по кассам";
			}
			return "Դրամարկղերի գործողություններ";
		}
	}

	public static string KassaOpJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks operations journal";
				}
				return "Кассовые операции";
			}
			return "Դրամարկղերի գործողությունների մատյան";
		}
	}

	public static string KassaRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks balances";
				}
				return "Остатки по кассам";
			}
			return "Դրամարկղերի մնացորդներ";
		}
	}

	public static string KassaTurn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks turnover journal";
				}
				return "Обороты по кассам";
			}
			return "Դրամարկղերի շրջանառություն";
		}
	}

	public static string Label
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Label";
				}
				return "Этикетка";
			}
			return "Պիտակ";
		}
	}

	public static string LabelPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price label";
				}
				return "Ценник";
			}
			return "Գնապիտակ";
		}
	}

	public static string LabelPriceBarcode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price label (barcode)";
				}
				return "Ценник со штрих кодом";
			}
			return "Գնապիտակ (գծ.կոդով)";
		}
	}

	public static string LabelPrinter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Label printer";
				}
				return "Принтер этикеток";
			}
			return "Պիտակի տպիչ";
		}
	}

	public static string LastPurchasePrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Latest purchase price";
				}
				return "Цена последней закупки";
			}
			return "Վերջին ձեռքբերման գին";
		}
	}

	public static string LastUser
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Last user";
				}
				return "Последний пользователь";
			}
			return "Վերջին օգտագործող";
		}
	}

	public static string LoadMaterialsList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill the list of products (previous data will be deleted)?";
				}
				return "Заполнить список товаров (предыдущие данные будут удалены)?";
			}
			return "Լրացնե՞լ ապրանքների ցուցակը (նախկին տվյալները կհեռացվեն)";
		}
	}

	public static string LotteryNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Lottery number";
				}
				return "Номер лотереи";
			}
			return "Վիճակահանության համար";
		}
	}

	public static string Luma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "luma";
				}
				return "тетри";
			}
			return "լումա";
		}
	}

	public static string ManyDefaultAcc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Settlement account directory contains more than one default settlement account";
				}
				return "Справочник «Банковские счета» содержит более одного основного банковского счета";
			}
			return "\"Հաշվարկային հաշիվներ\" տեղեկատուն պարունակում է մեկից ավել հիմնական հաշվարկային հաշիվ";
		}
	}

	public static string ManyDefaultCashes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks directory contains more than one default cash desk";
				}
				return "Справочник «Кассы» содержит более одной основной кассы";
			}
			return "\"Դրամարկղեր\" տեղեկատուն պարունակում է մեկից ավել հիմնական դրամարկղ";
		}
	}

	public static string ManyDefaultContracts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are more than one main contracts with {0} partner";
				}
				return "С партнером \"{0}\" имеются более одного основного договора";
			}
			return "\"{0}\" գործընկերոջ հետ կա մեկից ավելի հիմնական պայմանագիր";
		}
	}

	public static string MaterGrps
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services groups";
				}
				return "Группы товаров и услуг";
			}
			return "Ապրանքների և ծառայությունների խմբեր";
		}
	}

	public static string MaterialCheckRepetition
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "When adding a product (service), check the uniqueness of the name";
				}
				return "При добавлении товара (услуги) проверять уникальность наименования";
			}
			return "Ապրանք (ծառայություն) ավելացնելիս ստուգել անվանման կրկնությունը";
		}
	}

	public static string MaterialExistsQNT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product or service exists with a given unit of measure";
				}
				return "Существует товар или услуга в данной единице измерения";
			}
			return "Գոյություն ունի տվյալ չափման միավորով ապրանք կամ ծառայության";
		}
	}

	public static string MaterialExpMethodNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" product's costing method \"{1}\" is incorrect";
				}
				return "Метод списания \"{1}\" товара \"{0}\" указан неверно";
			}
			return "\"{0}\" ապրանքի \"{1}\" հաշվառման մեթոդը նշված է սխալ";
		}
	}

	public static string MaterialIDNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" inner system number(ISN) of product or service is missing from \"Products and services\" directory";
				}
				return "Товар или услуга с внутренним системным идентификатором \"{0}\" отсутствует в справочнике Товары и услуги";
			}
			return "Ապրանքի կամ ծառայության ներքին սիստեմային բնութագրիչը \"{0}\" բացակայում է \"Ապրանքներ և ծառայություններ\" տեղեկատուից";
		}
	}

	public static string MaterialNameAlreadyExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product or service with \"{0}\" name and \"{1}\" code already exists, continue?";
				}
				return "Существует товар или услуга с наименованием \"{0}\" и с кодом \"{1}\", продолжить?";
			}
			return "\"{0}\" անվանումով գոյություն ունի \"{1}\" կոդով ապրանք կամ ծառայություն, շարունակե՞լ";
		}
	}

	public static string MaterialNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" product or service is missing from \"Products and services\" directory";
				}
				return "Код товара или услуги \"{0}\" отсутствует в справочнике \"Товары и услуги\"";
			}
			return "\"{0}\" կոդը բացակայում է \"Ապրանքներ և ծառայություններ\" տեղեկատուից";
		}
	}

	public static string MaterialOrService
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product or service";
				}
				return "Товар или услуга";
			}
			return "Ապրանք կամ ծառայություն";
		}
	}

	public static string MaterialQUNIT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unit of measure, {0} - {1}";
				}
				return "Единица измерения, {0} - {1}";
			}
			return "Չափման միավոր, {0} - {1}";
		}
	}

	public static string MaterialQUNITS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Units of measure, {0} - {1}";
				}
				return "Единицы измерения, {0} - {1}";
			}
			return "Չափման միավորներ, {0} - {1}";
		}
	}

	public static string MaterialRevaluation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product revaluation";
				}
				return "Переоценка себестоимости товара";
			}
			return "Ապրանքի ինքնարժեքի վերագնահատում";
		}
	}

	public static string MaterialsAndServices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services";
				}
				return "Товары и услуги";
			}
			return "Ապրանքներ և ծառայություններ";
		}
	}

	public static string MaterialValue
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product";
				}
				return "Товар";
			}
			return "Ապրանք";
		}
	}

	public static string MatOperations
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Types of operations with products";
				}
				return "Виды операций с ТМЦ";
			}
			return "ՆԱ գործողության տեսակներ";
		}
	}

	public static string MatOperRealization
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Realization";
				}
				return "Реализация";
			}
			return "Իրացում";
		}
	}

	public static string MatRepAvail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products balance";
				}
				return "Остатки по товарам";
			}
			return "Ապրանքների մնացորդներ";
		}
	}

	public static string MatRepAvailByParties
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products balance by batches";
				}
				return "Остатки на по партиям товаров ";
			}
			return "Ապրանքների մնացորդներ ըստ խմբաքանակների";
		}
	}

	public static string MatRepAvailShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Availability";
				}
				return "Остатки";
			}
			return "Առկայություն";
		}
	}

	public static string MatRepAvailTreeView
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products balance (tree view)";
				}
				return "Остатки по товарам (дерево)";
			}
			return "Ապրանքների մնացորդներ (ծառատիպ)";
		}
	}

	public static string MatRepCurrentCost
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price in drams";
				}
				return "Цена в драмах";
			}
			return "Գին դրամով";
		}
	}

	public static string MatRepJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations journal";
				}
				return "Операции товаров";
			}
			return "Ապրանքների գործողությունների մատյան";
		}
	}

	public static string MatRepJournalShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operations journal";
				}
				return "Операции";
			}
			return "Գործողությունների մատյան";
		}
	}

	public static string MatRepOperationType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operation type";
				}
				return "Вид операции";
			}
			return "Գործողության տեսակ";
		}
	}

	public static string MatRepSaleTotalSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale amount";
				}
				return "Сумма продажи в драмах";
			}
			return "Վաճառքի գումար դրամով";
		}
	}

	public static string MatRepSaleTotalSumCur
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale amount in {0}";
				}
				return "Сумма продажи в {0}";
			}
			return "Վաճառքի գումար {0}-ով";
		}
	}

	public static string MatRepSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prime cost amount";
				}
				return "Сумма себестоимости";
			}
			return "Ինքնարժեքի գումար";
		}
	}

	public static string MatRepTurn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products turnover journal";
				}
				return "Обороты по товарам";
			}
			return "Ապրանքների շրջանառություն";
		}
	}

	public static string MatRepTurnOver
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products turnover";
				}
				return "Обороты по товарам";
			}
			return "Ապրանքների շրջանառություն";
		}
	}

	public static string MatRepTurnShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Turnover journal";
				}
				return "Обороты";
			}
			return "Շրջանառություն";
		}
	}

	public static string MCROptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Magnetic Card Reader Options";
				}
				return "Параметры ридера магнитных карт";
			}
			return "Մագնիսական քարտ կարդացող սարքի պարամետրեր";
		}
	}

	public static string MergeDocumentsPayments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Distribution of received payments by groups of documents";
				}
				return "Распределение полученных оплат по группам документов";
			}
			return "Ստացված վճարումների բաշխում ըստ փաստաթղթերի խմբերի";
		}
	}

	public static string MergeWithCurrentRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add current row quantity";
				}
				return "Добавить количество текущей строки";
			}
			return "ավելացնել ընթացիկ տողի քանակը";
		}
	}

	public static string MergeWithFirstMatchedRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add first row quantity";
				}
				return "Добавить количество первой строки";
			}
			return "ավելացնել առաջին տողի քանակը";
		}
	}

	public static string Minus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(-)";
				}
				return "(-)";
			}
			return "(-)";
		}
	}

	public static string MoreInfo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "More Info";
				}
				return "Дополнительно";
			}
			return "Ավելին` ";
		}
	}

	public static string MoreThanOneGoodWithSameName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In \"Products and services\" directory has been found more than one product with the name \"{0}\".";
				}
				return "В справочнике «Товары и услуги» по наименованию \"{0}\" обнаружено больше одного товара";
			}
			return "\"Ապրանքներ և ծառայություններ\" տեղեկատուում հայտնաբերվել է \"{0}\" անվանումով մեկից ավելի ապրանք";
		}
	}

	public static string MoreThanOneRowWithSameName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In \"Products and services\" directory has been found more than one row with the name \"{0}\".";
				}
				return "В справочнике «Товары и услуги» по наименованию \"{0}\" обнаружено больше одной строки ";
			}
			return "\"Ապրանքներ և ծառայություններ\" տեղեկատուում հայտնաբերվել է \"{0}\" անվանումով մեկից ավելի տող";
		}
	}

	public static string MoreThanOneServiceWithSameName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In \"Products and services\" directory has been found more than one service with the name \"{0}\".";
				}
				return "В справочнике «Товары и услуги» по наименованию \"{0}\" обнаружено больше одной услуги";
			}
			return "\"Ապրանքներ և ծառայություններ\" տեղեկատուում հայտնաբերվել է \"{0}\" անվանումով մեկից ավելի ծառայություն";
		}
	}

	public static string MoveInRowNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exit row of products movement \"{0}\" was detected  - product \"{1}\", row \"{2}\". Entrance row corresponding to it is missing";
				}
				return "Обнаружена строка отгрузки перемещения товара у которого отсутствует соответствующая строка поступления. Документ Перемещение товаров N - \"{0}\", товар - \"{1}\", строка  - \"{2}\"";
			}
			return "Հայտնաբերվել է \"{0}\" համարով Ապրանքների տեղաշարժի ելքի տող` ապրանք-\"{1}\", տող-\"{2}\", որի համապատասխան մուտքի տողը բացակայում է";
		}
	}

	public static string MoveOutRowNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Entrance row of products movement \"{0}\" was detected  - product \"{1}\", row \"{2}\". Exit row corresponding to it is missing";
				}
				return "Обнаружена строка отгрузки перемещения товара у которого отсутствует соответствующая строка поступления. Документ Перемещение товаров N - \"{0}\", товар - \"{1}\", строка - \"{2}\"";
			}
			return "Հայտնաբերվել է \"{0}\" համարով Ապրանքների տեղաշարժի մուտքի տող` ապրանք-\"{1}\", տող-\"{2}\", որի համապատասխան ելքի տողը բացակայում է";
		}
	}

	public static string MTAndServicesPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services prices ({0} - {1})";
				}
				return "Цены товаров и услуг ({0} - {1})";
			}
			return "Ապրանքների և ծառայությունների գներ ({0} - {1})";
		}
	}

	public static string MTAndServicesPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services prices {0} - {1} (Price type` {2} - {3})";
				}
				return "Цены товаров и услуг {0} - {1} (Тип цены- {2} - {3})";
			}
			return "Ապրանքների և ծառայությունների գներ {0} - {1} (Գնի տեսակ` {2} - {3})";
		}
	}

	public static string MTAvailabilityRetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products Balance (Retail) -{0}";
				}
				return "Остатки по товарам (розница) - {0}";
			}
			return "Ապրանքների մնացորդներ (Մանրածախ) - {0}";
		}
	}

	public static string MTBarCodeForMTQntUnitExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "With \"{1}\" unit of measure of \"{0}\" product or service barcode exists (\"{2}\"). For deleting u/m, it must be deleted.";
				}
				return "С единицей измерения \"{1}\" у товара или услуги \"{0}\" существует штрих код (\"{2}\"). Для удаления ед. изм. необходимо это удалить";
			}
			return "\"{0}\" ապրանքի կամ ծառայության \"{1}\" չափման միավորով գրանցված է գծ. կոդ(\"{2}\"): Չ/մ-ն հեռացնելու համար անհրաժեշտ է հեռացնել այն:";
		}
	}

	public static string MTBarCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Barcode, {0} - {1} ";
				}
				return "Штрихкод, {0} - {1} ";
			}
			return "Գծիկավոր կոդ, {0} - {1} ";
		}
	}

	public static string MTBillAllowDifferentStorages
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Multiple Storage mode";
				}
				return "Режим продажи с нескольких складов";
			}
			return "Բազմապահեստային ռեժիմ";
		}
	}

	public static string DisplayableCols
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Displayed columns";
				}
				return "Отображаемые колонки";
			}
			return "Արտապատկերվող սյուներ";
		}
	}

	public static string MTBillColsOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Columns options";
				}
				return "Настройка колонок";
			}
			return "Սյուների կարգավորում";
		}
	}

	public static string MTBillDeferd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "N {0} sale is put on hold";
				}
				return "Документ продажи N {0} отложен";
			}
			return "N {0} վաճառքը հետաձգված է";
		}
	}

	public static string MTBillFields
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Editable";
				}
				return "Редактируемые ";
			}
			return "Խմբագրվող";
		}
	}

	public static string MTBillFieldsOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fields options";
				}
				return "Настройка полей";
			}
			return "Դաշտերի կարգավորում";
		}
	}

	public static string MTBillInfoPanel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Current row panel";
				}
				return "Панель текущей строки продажи";
			}
			return "Ընթացիկ տողի վահանակ";
		}
	}

	public static string MTBillRecordAllEntries
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show the amounts of sales and payments in buyer's reports";
				}
				return "Во взаиморасчетах партнеров отображать суммы продажи и гашения";
			}
			return "Գնորդի փոխհաշվարկներում արտացոլել վաճառքի և մարումների գումարները";
		}
	}

	public static string MTBillSimpleMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use bill in simple mode";
				}
				return "Использовать упрощенную форму документа";
			}
			return "Կտրոնը օգտագործել պարզ ռեժիմով";
		}
	}

	public static string MTBillVisibileCols
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Visible";
				}
				return "Отображаемые";
			}
			return "Ցուցադրվող";
		}
	}

	public static string MTCheckPrintOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print options of product check";
				}
				return "Настройки печати товарного чека";
			}
			return "Ապրանքային կտրոնի տպելու կարգավորումներ";
		}
	}

	public static string MTCodeHasAlsoComplect
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Attention, \"{0}\" product is a kit.";
				}
				return "Внимание, \"{0}\" товар также является комплектом.";
			}
			return "Ուշադրություն, \"{0}\" ապրանքը հանդիսանում է նաև կոմպլեկտ:";
		}
	}

	public static string MTCodeRepeatedInList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In the list, Item \"{0}\" exists, continue?";
				}
				return "В списке код товара \"{0}\" повторяется, продолжить?";
			}
			return "Ցուցակում \"{0}\" կոդով ապրանք գոյություն ունի, շարունակե՞լ";
		}
	}

	public static string MtCoefLong
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Recalculation coefficient of main unit of measure";
				}
				return "Коэффициент пересчета относительно основной единицы";
			}
			return "Հիմնական չ/մ-ի վերահաշվարկի գործակից";
		}
	}

	public static string MTCollationStatement
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Collation statement of products' stocktaking results (i-12)";
				}
				return "Сличительная ведомость результатов инвентаризации ТМЦ";
			}
			return "ՆԱ-ների գույքագրման արդյունքների համեմատական տեղեկագիր (գ-12)";
		}
	}

	public static string MTCollationStatementShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Collation statement of products' stocktaking results (i-12)";
				}
				return "Сличительная ведомость инвентаризации ТМЦ";
			}
			return "ՆԱ գույքագրման արդ. համեմատական տեղեկագիր (գ-12)";
		}
	}

	public static string MTComplect
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kit's components, {0} - {1}";
				}
				return "Комплектующие, {0} - {1}";
			}
			return "Կոմպլեկտի բաղադրամասեր, {0} - {1}";
		}
	}

	public static string MTComplectation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kit's components";
				}
				return "Комплектующие";
			}
			return "Կոմպլեկտի բաղադրամասեր";
		}
	}

	public static string MTComplectPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kit's components";
				}
				return "Комплектующая";
			}
			return "Կոմպլեկտի բաղադրամաս";
		}
	}

	public static string MTComplectPartNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The component of \"{0}\" kit with \"{1}\" inner system number(ISN) is missing from \"{2}\" directory";
				}
				return "Комплект \"{0}\" с внутренним системным идентификатором \"{1}\" отсутствует в справочнике \"{2}\"";
			}
			return "\"{0}\" կոմպլեկտի \"{1}\" ներքին սիստեմային բնութագրիչով բաղադրամասը բացակայում է \"{2}\" տեղեկատուից";
		}
	}

	public static string MTComplectsParts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kit's components";
				}
				return "Комплектующие";
			}
			return "Կոմպլեկտների բաղադրամասեր";
		}
	}

	public static string MTCurrency
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products currency";
				}
				return "Валюта себестоимости управ. учета";
			}
			return "Ապրանքների արժույթ";
		}
	}

	public static string MTEgineInavidMaterial
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product with {0} code is missing from products list";
				}
				return "Товар с кодом \"{0}\" отсутствует в списке товаров";
			}
			return "\"{0}\" կոդով ապրանքը բացակայում է ապրանքների ցուցակում";
		}
	}

	public static string MTEgineInavidNegativeQTY
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Negative quantity {0} \r\nStorage {1}, Product {2}, \r\nDate {3}";
				}
				return "Недостаточное количество {0} ' Склад {1}, Товар {2}, Дата {3}";
			}
			return "Բացասական քանակ {0} ' \r\nՊահեստ {1}, ապրանք {2},\r\nԱմսաթիվ {3}";
		}
	}

	public static string MTEgineInavidPartyMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Batch product's code does not match to product's code";
				}
				return "Код товара партии не соответствует коду товара";
			}
			return "Խմբաքանակի ապրանքի կոդը չի համապատասխանում ապրանքի կոդին";
		}
	}

	public static string MTEgineNegativeQTYForPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Negative quantity {0}, Storage {1}, Product {2}, Price {3}, Date {4}";
				}
				return "Недостаточное количество {0} ՚ Склад {1}, Товар {2}, Цена {3}, Дата {4}";
			}
			return "Բացասական քանակ {0} ՚ \r\nՊահեստ {1}, Ապրանք {2},\r\nԳին {3}, Ամսաթիվ {4}";
		}
	}

	public static string MTEngineNegativeQTY
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Negative quantity {0}, Storage {1}, Product {2}, Date {3}";
				}
				return "Недостаточное количество {0} ՚ Склад {1}, Товар {2}, Дата {3}";
			}
			return "Բացասական քանակ {0} ՚ \r\nՊահեստ {1}, Ապրանք {2},\r\nԱմսաթիվ {3}";
		}
	}

	public static string MTExistInMTList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In the list already exists product with \"{0}\" code, do you want to continue?";
				}
				return "В списке существует товар с кодом \"{0}\", продолжить?";
			}
			return "Անվանացուցակում \"{0}\" կոդով ապրանք գոյություն ունի, շարունակե՞լ";
		}
	}

	public static string MTExpense
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's output";
				}
				return "Списание товаров";
			}
			return "Ապրանքի ելք";
		}
	}

	public static string MTExpenseAverage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In average price";
				}
				return "По среднему";
			}
			return "Միջին կշռված գնով";
		}
	}

	public static string MTExpenseMethodFIFO
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Invariable FIFO";
				}
				return "По ФИФО";
			}
			return "ՖԻՖՈ (առաջինը մուտք, առաջինը ելք)";
		}
	}

	public static string MTExpenseMethodFIFOShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "FIFO";
				}
				return "ФИФО";
			}
			return "ՖԻՖՈ";
		}
	}

	public static string MTExpenseMethodLIFO
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Invariable LIFO";
				}
				return "По ЛИФО";
			}
			return "ԼԻՖՈ (վերջինը մուտք, առաջինը ելք)";
		}
	}

	public static string MTExpenseMethodLIFOShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "LIFO";
				}
				return "ЛИФО";
			}
			return "ԼԻՖՈ";
		}
	}

	public static string MTFavoriteInstruction
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The list of favorites is empty.\r\nFrequently sold goods and services are added from the context menu of the “Products and Services” directory or of the current line of the User.";
				}
				return "Список избранных пуст.\r\nЧасто продаваемые товары и услуги добавляются из контекстного меню справочника «Товары и услуги» или текущей строки Пользователя.";
			}
			return "Ընտրյալների ցուցակը դատարկ է:\r\nՀաճախ վաճառվող ապրանքները և ծառայությունները ավելացվում են Ապրանքներ և ծառայություններ տեղեկատուի կամ Օգտագործողի ընթացիկ տողի վրայից բացվող կոնտեքստային ցանկից:";
		}
	}

	public static string MTFavorites
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Favorite products and services";
				}
				return "Избранные товары и услуги";
			}
			return "Ընտրյալ ապրանքներ և ծառայություններ";
		}
	}

	public static string MTGpChildrenExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "This Products and services group cannot be deleted, because it has subgroups.";
				}
				return "Удаление данной группы товаров и услуг запрещено, так как она имеет подгруппы";
			}
			return "Ապրանքների և ծառայությունների այս խումբը չի թույլատրվում հեռացնել, քանի որ այն ունի ենթախմբեր";
		}
	}

	public static string MTGpChildrenExistWithCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" products and services group cannot be deleted, because it has subgroups.";
				}
				return "Удаление группы товаров и услуг \"{0}\" запрещено, так как она имеет подгруппы";
			}
			return "Ապրանքների և ծառայությունների \"{0}\" խումբը չի թույլատրվում հեռացնել, քանի որ այն ունի ենթախմբեր";
		}
	}

	public static string MTGpDialogCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services group";
				}
				return "Группа товара или услуги";
			}
			return "Ապրանքի կամ ծառայության խումբ";
		}
	}

	public static string MTGpMTExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "This Products and services group cannot be deleted, because in Products and services directories there are Products or services of this type.";
				}
				return "Удаление данной группы товаров и услуг запрещено, так как в справочнике товаров и услуг существуют товары или услуги данного типа";
			}
			return "Ապրանքների և ծառայությունների այս խումբը չի թույլատրվում հեռացնել, քանի որ ապրանքների և ծառայությունների տեղեկատուի մեջ կան այս տիպի ապրանքներ կամ ծառայություններ";
		}
	}

	public static string MTGpMTExistWithCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" products and services group cannot be deleted, because in Products and services directory there are Products or services of type \"{1}\".";
				}
				return "Удаление группы товаров и услуг \"{0}\" запрещено, так как в справочнике существуют товары и услуги типа \"{1}\"";
			}
			return "Ապրանքների և ծառայությունների \"{0}\" խումբը չի թույլատրվում հեռացնել, քանի որ ապրանքների և ծառայությունների տեղեկատուի մեջ կան \"{1}\" տիպի ապրանքներ կամ ծառայություններ";
		}
	}

	public static string MTGpRowChangedWhenDeleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" products and services group was changed during your deletion. Update the folder and retry the action.";
				}
				return "Группа товаров и услуг \"{0}\" была изменена во время вашего удаления. Обновите папку и повторите действие.";
			}
			return "Ապրանքների և ծառայությունների \"{0}\" խումբը Ձեր կողմից հեռացման ընթացքում փոփոխվել է ուրիշի կողմից: Թարմացրեք թղթապանակը և կրկնեք գործողությունը:";
		}
	}

	public static string MTGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's group";
				}
				return "Группа товаров";
			}
			return "Ապրանքների խումբ";
		}
	}

	public static string MTHasNegativeQty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" product has negative availability. Operation is not allowed.";
				}
				return "Недостаточное количество товара с кодом \"{0}\". Действие запрещено";
			}
			return "\"{0}\" կոդով ապրանքն ունի բացասական առկայություն: Գործողությունը չի թույլատրվում:";
		}
	}

	public static string MTHISalesIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales and products operations journals integrity checking";
				}
				return "Проверка целостности реестра Продаж и Операций по движению товаров";
			}
			return "Վաճառքների և ապրանքների գործողությունների մատյանների ամբողջականության ստուգում";
		}
	}

	public static string MTHISalesIntegrityBaseErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product with \"{1}\" code of \"{0}\" document is missing from Sales journal";
				}
				return "Запись товара с кодом \"{1}\" из документа \"{0}\" отсутствует в Журнале продаж";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի տողը բացակայում է վաճառքների մատյանից";
		}
	}

	public static string MTHISalesIntegrityCountErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Rows count mismatch of product with \"{1}\" code of {0} document in Products turnover({2}) and Sales journal ({3})";
				}
				return "Обнаружено расхождение между количеством строк журнала продаж (кол. строк - {3}) и журнала операций (кол. строк - {2}). Документ - \"{0}\", товар - \"{1}\"";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի տողերի քանակի անհամապատասխանություն ապրանքների շրջանառությունում ({2}) և վաճառքների մատյանում ({3})";
		}
	}

	public static string MTHISalesIntegrityCurSummErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total amount in foreign currency mismatch of product with \"{1}\" code of {0} document in Products turnover({2}) and Sales journal ({3})";
				}
				return "Обнаружена разница между итоговыми суммами журнала продаж (вал. сумма - {3}) и журнала операций (вал. сумма - {2}). Документ - \"{0}\", товар - \"{1}\"";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի ընդհանուր արժույթով գումարի անհամապատասխանություն ապրանքների շրջանառությունում ({2}) և վաճառքների մատյանում ({3})";
		}
	}

	public static string MTHISalesIntegrityQtyErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total quantity mismatch of product with \"{1}\" code of {0} document in Products turnover({2}) and Sales journal ({3})";
				}
				return "Обнаружена разница между общим количеством в журнале продаж (кол. - {3}) и в журнале операций (кол. - {2}). Документ - \"{0}\", товар - \"{1}\"";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի ընդհանուր քանակի անհամապատասխանություն ապրանքների շրջանառությունում ({2}) և վաճառքների մատյանում ({3})";
		}
	}

	public static string MTHISalesIntegritySummErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total amount mismatch of product with \"{1}\" code of {0} document in Products turnover({2}) and Sales journal ({3})";
				}
				return "Обнаружена разница между итоговыми суммами журнала продаж (сумма в ларах - {3}) и журнала операций (сумма в ларах - {2}). Документ - \"{0}\", товар - \"{1}\"";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի ընդհանուր գումարի անհամապատասխանություն ապրանքների շրջանառությունում ({2}) և վաճառքների մատյանում ({3})";
		}
	}

	public static string MTHISalesIntegritySummVATErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total VAT amount mismatch of product with \"{1}\" code of {0} document in Products turnover({2}) and Sales journal ({3})";
				}
				return "Обнаружена разница между итоговыми суммами НДС журнала продаж (сумма НДС - {3}) и журнала операций (сумма НДС - {2}). Документ - \"{0}\", товар - \"{1}\"";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի ընդհանուր ԱԱՀ-ի գումարի անհամապատասխանություն ապրանքների շրջանառությունում ({2}) և վաճառքների մատյանում ({3})";
		}
	}

	public static string MTHotKeysMustDiffer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Different rows cannot have same hot keys";
				}
				return "Разные строки не могут иметь одинаковые клавиши быстрого доступа";
			}
			return "Տարբեր տողեր չեն կարող ունենալ նույն արագ հասանելիության ստեղնը";
		}
	}

	public static string MTIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input";
				}
				return "Приход";
			}
			return "Մուտք";
		}
	}

	public static string MTIncome
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's input";
				}
				return "Оприходование товаров";
			}
			return "Ապրանքի մուտք";
		}
	}

	public static string MTIncomeSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input order amount";
				}
				return "Сумма поступления в драмах";
			}
			return "ՊՄՕ գումար դրամով";
		}
	}

	public static string MTIncomeType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's purchasing type";
				}
				return "Вид приобретения ТМЦ";
			}
			return "ՆԱ-ի ձեռք բերման տեսակ";
		}
	}

	public static string MTLIST
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products list";
				}
				return "Список товаров";
			}
			return "Ապրանքների ցուցակ";
		}
	}

	public static string MTListSalesIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales journal and sales documents lists integrity checking";
				}
				return "Проверка целостности реестра Продаж и Табличных элементов документов по продажам";
			}
			return "Վաճառքների մատյանի և վաճառքի փաստաթղթերի անվանացուցակների ամբողջականության ստուգում";
		}
	}

	public static string MTListsSalesIntegrityBaseErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's row with the \"{1}\" code  in {0} Documents' item list is missing from Sales journal";
				}
				return "Запись товара с кодом \"{1}\" из документа \"{0}\" отсутствует в Журнале продаж";
			}
			return "\"{0}\" փաստաթղթի անվանացուցակի \"{1}\" կոդով ապրանքի տողը բացակայում է վաճառքների մատյանից";
		}
	}

	public static string MTListsSalesIntegritySummErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total amount mismatch of product with \"{0}\" code in \"{1}\" documents items list ({2}) and Sales journal  ({3})";
				}
				return "Обнаружено расхождение между количеством строк журнала продаж (кол. строк - {3}) и списка документа (кол. строк - {2}). Документ - \"{0}\", товар - \"{1}\"";
			}
			return "\"{0}\" կոդով ապրանքի ընդհանուր գումարի անհամապատասխանություն \"{1}\" փաստաթղթի անվանացուցակում ({2}) և վաճառքների մատյանում ({3})";
		}
	}

	public static string MTMove
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products movement";
				}
				return "Перемещение товаров";
			}
			return "Ապրանքների տեղաշարժ";
		}
	}

	public static string MTMoveErrSameStorages
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input and output storages should differ";
				}
				return "Приходные и расходные склады должны быть разными";
			}
			return "Մուտքի և ելքի պահեստները պետք է տարբեր լինեն";
		}
	}

	public static string MTMoveExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products movement exists by given storage.";
				}
				return "По данному складу существует движение товаров";
			}
			return "Տվյալ պահեստով գոյություն ունի ապրանքների շարժ";
		}
	}

	public static string MTMoveOrder
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products movement order";
				}
				return "Заказ на перемещение товаров";
			}
			return "Ապրանքների տեղաշարժի պատվեր";
		}
	}

	public static string MTNewAndOldCostsAreEqual
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{2}\" product', entered with \"{1}\" document on {0} date, current and new prices are the same";
				}
				return "Для товара с приходом по дате \"{0}\" и с документом \"{1}\" учетная и новая цена одинаковы";
			}
			return "\"{0}\" ամսաթվով և \"{1}\" փաստաթղթով մուտքագրված \"{2}\" ապրանքների հաշվառման և նոր գները նույնն են";
		}
	}

	public static string MTNewAndOldCostsAreEqualForAvg
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Current and new prices are the same for \"{0}\" product";
				}
				return "Учетная и новая цена товара \"{0}\" одинаковы";
			}
			return "\"{0}\" ապրանքի հաշվառման և նոր գները նույնն են";
		}
	}

	public static string MTOperations
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations";
				}
				return "Операции товаров";
			}
			return "Ապրանքների գործողություններ";
		}
	}

	public static string MTOperationsBetween
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations journal  {0} - {1}";
				}
				return "Операции товаров {0} - {1}";
			}
			return "Ապրանքների գործողությունների մատյան {0} - {1}";
		}
	}

	public static string MTOperationsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations journal";
				}
				return "Операции товаров";
			}
			return "Ապրանքների գործողությունների մատյան";
		}
	}

	public static string MTOperationsRetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations (Retail)";
				}
				return "Операции товаров (Розница)";
			}
			return "Ապրանքների գործողություններ (Մանրածախ)";
		}
	}

	public static string MTOrServicePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services price {0} - {1}";
				}
				return "Цена на товар или услугу {0} - {1}";
			}
			return "Ապրանքի կամ ծառայության գին {0} - {1}";
		}
	}

	public static string MTOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Output";
				}
				return "Расход";
			}
			return "Ելք";
		}
	}

	public static string MTOverheadExpBaseEmpty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The base document for \"{1}\" overhead expenses on \"{0}\" is not selected";
				}
				return "Документ основания для распределения доп. расходов с номером \"{1}\" на дату \"{0}\" не заполнен";
			}
			return "\"{0}\" ամսաթվով \"{1}\" համարով hավելյալ ծախսերի բաշխման հիմք փաստաթուղթը լրացված չէ";
		}
	}

	public static string MTOvExpDateValidate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The date of Overhead expenses' allocation document cannot be less than the date of the base document";
				}
				return "Дата документа Распределение дополнительных расходов не может быть меньше даты документа основания.";
			}
			return "Հավելյալ ծախսերի բաշխման փաստաթղթի ամսաթիվը պետք է փոքր չլինի հիմք փաստաթղթի ամսաթվից";
		}
	}

	public static string MTPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's prices {0} - {1} ({2} - {3})";
				}
				return "Цены товара {0} - {1} ({2} - {3})";
			}
			return "Ապրանքի գներ {0} - {1} ({2} - {3})";
		}
	}

	public static string MTPricesByPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's prices {0} - {1} (Product {2} - {3}, price type {4})";
				}
				return "Цены товара {0} - {1} (Товар- {2} - {3}, Тип цены- {4})";
			}
			return "Ապրանքի գներ {0} - {1} (Ապրանք` {2} - {3}, Գնի տեսակ` {4})";
		}
	}

	public static string MTPricesCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's price {0} - {1}";
				}
				return "Цена товара {0} - {1}";
			}
			return "Ապրանքի գին {0} - {1}";
		}
	}

	public static string MTPropTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Properties types";
				}
				return "Типы свойств";
			}
			return "Հատկությունների տեսակներ";
		}
	}

	public static string MTPropValue
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property value, {0} - {1}";
				}
				return "Значение свойства, {0} - {1}";
			}
			return "Հատկության արժեք, {0} - {1}";
		}
	}

	public static string MTPropValues
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property values";
				}
				return "Значения свойства";
			}
			return "Հատկության արժեքներ";
		}
	}

	public static string MTQntUnitHavntFractionalPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The U/M of \"{0}\" product has no fractional part";
				}
				return "Единица измерения товара с кодом \"{0}\" не имеет дробной части";
			}
			return "\"{0}\" կոդով ապրանքի չափման միավորը չունի կոտորակային մաս";
		}
	}

	public static string MTQntUnits
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and services units of measures";
				}
				return "Единицы измерения товаров и услуг";
			}
			return "Ապրանքների և ծառայությունների չ/մ";
		}
	}

	public static string MTQuantityIntegrityCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Integrity checking of documents lists and products operations journal quantities.";
				}
				return "Проверка целостности реестра Операций по движению товаров и Табличных элементов документов.";
			}
			return "Փաստաթղթերի ցուցակների և ապրանքների գործողությունների մատյանի քանակների ամբողջականության ստուգում";
		}
	}

	public static string MTQuantityIntegrityErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} document's product with \"{1}\" code total quantity mismatch in documents lists and operations journal.";
				}
				return "Обнаружено расхождение между общим количеством в журнале операций и списка документа. Документ - \"{0}\", товар - \"{1}\"";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի ընդհանուր քանակի անհամապատասխանություն փաստաթղթի ցուցակում և գործողությունների մատյանում";
		}
	}

	public static string MTRepRetailAvail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products balance (Retail)";
				}
				return "Остатки по товарам (розница)";
			}
			return "Ապրանքների մնացորդներ (Մանրածախ)";
		}
	}

	public static string MTRepRetailAvailShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Availability (Retail)";
				}
				return "Остатки (розница)";
			}
			return "Առկայություն (Մանրածախ)";
		}
	}

	public static string MTRepRetailOpJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations journal (Retail)";
				}
				return "Операции товаров (розница)";
			}
			return "Ապրանքների գործողությունների մատյան (Մանրածախ)";
		}
	}

	public static string MTRepRetailOpJournalShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operations journal (Retail)";
				}
				return "Операции (Розница)";
			}
			return "Գործողությունների մատյան (Մանրածախ)";
		}
	}

	public static string MTRepRetailTurn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products turnover (Retail) journal";
				}
				return "Обороты по товарам (розница)";
			}
			return "Ապրանքների շրջանառություն (Մանրածախ)";
		}
	}

	public static string MTRepRetailTurnShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Turnover journal (Retail)";
				}
				return "Обороты (Розница)";
			}
			return "Շրջանառություն (Մանրածախ)";
		}
	}

	public static string MTRepSalesAnalysis
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales analysis";
				}
				return "Анализ продаж";
			}
			return "Վաճառքների վերլուծություն";
		}
	}

	public static string MTRetailHIIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products retail operations journal integrity checking";
				}
				return "Проверка целостности журнала Операции по движению товаров в рознице";
			}
			return "Ապրանքների մանրածախ գործողությունների մատյանի ամբողջականության ստուգում";
		}
	}

	public static string MTRetailHIIntegrityBaseErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In {0} document row with \"{1}\" product's code is missing from retail turnover journal.";
				}
				return "Обнаружено расхождение между журналом операций (розница) и списка документа. Товар \"{1}\" из документа \"{0}\" отсутствует в журнале операций (розница)";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի տողը բացակայում է մանրածախ շրջանառության մատյանից";
		}
	}

	public static string MTRetailHIIntegrityQtyErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} document's product with \"{1}\" code total quantity mismatch in Products turnover ({2}) and wholesale turnover ({3}) journals.";
				}
				return "Обнаружена разница между общим количеством в журнале операций (розница) (кол. - {3}) и в журнале операций (кол. - {2}). Документ - \"{0}\", товар - \"{1}\"";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի ընդհանուր քանակների անհամապատասխանություն ապրանքների շրջանառության ({2}) և մանրածախ շրջանառության ({3}) մատյաններում";
		}
	}

	public static string MTRetailOperationsBetween
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations journal (Retail) {0} - {1}";
				}
				return "Операции товаров (розница) {0} - {1}";
			}
			return "Ապրանքների գործողությունների մատյան(Մանրածախ) {0} - {1}";
		}
	}

	public static string MTRetFromCustomerDeferd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "N {0} return is put on hold";
				}
				return "Документ возврата N {0} отложен";
			}
			return "N {0} վերադարձը հետաձգված է";
		}
	}

	public static string MTRetFromCustomerDocumentsCorrection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Managing Return from buyer documents, which base documents were created before {0}";
				}
				return "Настройка документов Возврат от покупателя, документы основания которых зарегистрированы до {0}";
			}
			return "Վերադարձ գնորդից փաստաթղթերի կարգավորում, որոնց հիմք փաստաթղթերը {0}-ից առաջ են";
		}
	}

	public static string MTReturnFromCustomerIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Till {0} there are “Return from customer” documents where cash desk selected, but cash amount is 0";
				}
				return "Документы Возврат от покупателя созданные до {0} , в которых указана касса, но сумма наличной оплаты 0.";
			}
			return "Մինչև {0}-ը ստեղծված վերադարձ գնորդից փաստաթղթեր, որոնցում նշված է դրամարկղ, բայց կանխիկի գումարը 0 է";
		}
	}

	public static string MtSales
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales";
				}
				return "Продажи";
			}
			return "Վաճառքներ";
		}
	}

	public static string MTTAccByAllStorages
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By all storages";
				}
				return "По всем складам";
			}
			return "Բոլոր պահեստներով";
		}
	}

	public static string MTTAccByTheStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Storage {0} - {1}";
				}
				return "Склад: {0} - {1}";
			}
			return "Պահեստ՝ {0} - {1}";
		}
	}

	public static string MTTAccCaptionMaterial
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Code- {1}";
				}
				return "{0} Код - {1}";
			}
			return "{0} Կոդ - {1}";
		}
	}

	public static string MTTAccCorObjectName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner/Storage";
				}
				return "Партнер/Склад";
			}
			return "Գործընկեր/Պահեստ";
		}
	}

	public static string MTTAccDlgGroupByCorObj
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group by Partner/Storage Column";
				}
				return "Группировать по колонке партнер/склад";
			}
			return "Խմբավորել ըստ Գործընկեր/պահեստ սյան";
		}
	}

	public static string MTTAccDlgShowDailyRem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show daily balance";
				}
				return "Выводить остатки в конце дня";
			}
			return "Ցույց տալ մնացորդը օրվա վերջում";
		}
	}

	public static string MTTAccQuantitative
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quantitative";
				}
				return "Количество";
			}
			return "Քանակ";
		}
	}

	public static string MTTurnOver
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products turnover journal {0} - {1}";
				}
				return "Обороты по товарам за период {0} - {1}";
			}
			return "Ապրանքների շրջանառություն {0} - {1} ժամանակահատվածում";
		}
	}

	public static string MTTurnOverRetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products retail turnover journal {0} - {1}";
				}
				return "Обороты по товарам (Розница) за период {0} - {1}";
			}
			return "Ապրանքների շրջանառություն (Մանրածախ) {0} - {1} ժամանակահատվածում ";
		}
	}

	public static string MustClearPartnerCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Buyer's card is already chosen, first it must be removed";
				}
				return "Карта покупателя указана. Сначало необходимо очистить данное поле.";
			}
			return "Գնորդի քարտ արդեն ընտրված է, նախ պետք է այն մաքրել";
		}
	}

	public static string MustStoreGoodDocForEditUnits
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You must save this document to edit the units of measurement";
				}
				return "Для изменения единиц измерения необходимо зарегистрировать документ";
			}
			return "Չափման միավորները փոփոխելու համար անհրաժեշտ է գրանցել այս փաստաթուղթը";
		}
	}

	public static string NameInterpretation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Take as a name";
				}
				return "Использовать как название";
			}
			return "Որպես անվանում վերցնել";
		}
	}

	public static string NameServerOrDBIsNotSpecified
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Server or Database are not specified";
				}
				return "Не указан Сервер или База данных ";
			}
			return "Սերվերը կամ Տվյալների պահոցը նշված չէ";
		}
	}

	public static string NCCName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Armenian Dram";
				}
				return "Драм";
			}
			return "Հայկական դրամ";
		}
	}

	public static string NCCSalePrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales price in AMD";
				}
				return "Цена продажи в драмах";
			}
			return "Վաճառքի գին դրամով";
		}
	}

	public static string NCOnly
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In Dram";
				}
				return "в драмах";
			}
			return "ԱԴՄ-ով";
		}
	}

	public static string NewBarcode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New barcode";
				}
				return "Новый штрихкод";
			}
			return "Նոր գծիկավոր կոդ";
		}
	}

	public static string NewBarcodeShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New barcode";
				}
				return "Новый штрихкод";
			}
			return "Նոր գծ. կոդ";
		}
	}

	public static string NewInternalBarcodeFormationCompleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Creation of new barcodes finished. Assigned barcode quantity {0}.";
				}
				return "Генерация новых штрихкодов завершена. Количество сгенерированных кодов {0}";
			}
			return "Նոր գծիկավոր կոդերի ձևավորումն ավարտվեց: Վերագրված գծ. կոդերի քանակը` {0}";
		}
	}

	public static string NewMaterialInMtIncome
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's input";
				}
				return "Приход товара";
			}
			return "Ապրանքի մուտք";
		}
	}

	public static string NewOrderNumberInDay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New order number (date {0})";
				}
				return "Новый порядковый номер (дата {0}) ";
			}
			return "Նոր հերթական համարը (ա/թ {0})";
		}
	}

	public static string NewPriceCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New price in {0}";
				}
				return "Новая цена в {0}";
			}
			return "Նոր գին {0}-ով";
		}
	}

	public static string NewPriceFormationMethod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New price formation method";
				}
				return "Метод формирования новой цены";
			}
			return "Նոր գնի ձևավորման մեթոդ";
		}
	}

	public static string NewPriceFormationMethods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New price formation methods";
				}
				return "Методы формирования новой цены";
			}
			return "Նոր գնի ձևավորման մեթոդներ";
		}
	}

	public static string NextNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Number";
				}
				return "Порядковый номер";
			}
			return "Հերթական համար";
		}
	}

	public static string NextPONum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Next payment order number";
				}
				return "Порядковый номер пл. поручения";
			}
			return "Հերթական Վճ.Հանձ. համար";
		}
	}

	public static string NoAvailablePartiesWithBarcodeInProperty1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is no batch with value of Property 1";
				}
				return "Не существует партий по значению свойства 1";
			}
			return "Հատկություն 1-ի արժեքով խմբաքանակ չկա";
		}
	}

	public static string NoCalcBonusesWhenRePay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Do not calculate bonuses in case of bonus payments";
				}
				return "Начисление бонусов при оплате бонусами не производить";
			}
			return "Բոնուսով մարելիս բոնուսային միավորներ չհաշվարկել";
		}
	}

	public static string NoCashShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-cash";
				}
				return "Безнал.";
			}
			return "Անկանխ․";
		}
	}

	public static string NoneOfContractsIsBonusCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"Bonus card\" is not selected on these contracts";
				}
				return "Ни один из указанных контрактов не является бонусной картой";
			}
			return "Նշված պայմանագրերից ոչ մեկը չի հանդիսանում բոնուսային քարտ";
		}
	}

	public static string NoPriceChanged
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No product's price has been changed";
				}
				return "Цена ни на один товар не изменилась";
			}
			return "Ոչ մի ապրանքի գին չի փոփոխվել";
		}
	}

	public static string NoPricesToExport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Pricelist with Your specified filters is empty";
				}
				return "Прайс-лист по указанным Вами параметрам пуст";
			}
			return "Ձեր նշած ֆիլտրերով կազմված գնացուցակը դատարկ է";
		}
	}

	public static string NotAllowAddRowForReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not allowed to add a new row in the return document with a base";
				}
				return "При возврате с основой добавление новой строки не разрешается";
			}
			return "Հիմքով վերադարձի դեպքում նոր տող ավելացնել չի թույլատրվում";
		}
	}

	public static string NotAllowChangeBonusForReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not allowed to add or reduce bonus in the return document with a base";
				}
				return "При возврате с основой добавление или уменьшение бонуса не разрешается";
			}
			return "Հիմքով վերադարձի դեպքում բոնուս ավելացնել կամ պակասեցնել չի թույլատրվում";
		}
	}

	public static string NotAllowChangeCardForReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not allowed to remove the buyer's card in the return document with a base";
				}
				return "При возврате с основой удаление карты покупателя не разрешается";
			}
			return "Հիմքով վերադարձի դեպքում գնորդի քարտը մաքրել չի թույլատրվում";
		}
	}

	public static string NotAllowChangePriceForReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In return document product's sale price cannot be edited";
				}
				return "В случае возврата цена продажи не подлежит редактированию";
			}
			return "Հիմքով վերադարձի դեպքում վաճառքի գինը խմբագրման ենթակա չէ";
		}
	}

	public static string NotAllowedToChangeBillCashDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You are not allowed to choose cash desk in ECR Check other than \"{0}\" cash desk mentioned in Documents options.";
				}
				return "Вы не можете выбрать кассу отличную от указанной в Настройках пользователя как. Касса по умолчанию - \"{0}\"";
			}
			return "Ձեզ չի թույլատրվում Կտրոնում Փաստաթղթերի դրույթներում նշված \"{0}\" դրամարկղից տարբեր դրամարկղ նշել";
		}
	}

	public static string NotAllowedToChangeBillStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You are not allowed to choose storage in ECR Check other than \"{0}\" storage mentioned in Documents options.";
				}
				return "Вы не можете выбрать склад отличный от указанного в Настройках пользователя.  Склад по умолчанию - \"{0}\"";
			}
			return "Ձեզ չի թույլատրվում Կտրոնում Փաստաթղթերի դրույթներում նշված \"{0}\" պահեստից տարբեր պահեստ նշել";
		}
	}

	public static string NotExistMapForAllMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On {0} in order {1}  for Item {2} supplier external code is not matched";
				}
				return "В заказе по дате {0} и номером {1} для товара с кодом {2} не указано соответствие с внешним кодом поставщика";
			}
			return "{0} ամսաթվով և {1} համարով պատվերում {2} կոդով  ապրանքի համար համապատասխանեցված չէ մատակարարի արտաքին կոդ";
		}
	}

	public static string NotExistRev
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "None of the selected accounts can be revaluated";
				}
				return "Ни один из выбранных Партнеров/Договоров не подлежит переоценке";
			}
			return "Ընտրված Գործընկերներ/Պայմանագրեր-ից ոչ մեկը վերագնահատման ենթակա չէ";
		}
	}

	public static string NotHaveAParent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Product group's accumulator: \"{1}\" is missing from \"Products and services groups\" directory";
				}
				return "Группа \"{1}\" товара \"{0}\" отсутствует в справочнике Группы товаров и услуг";
			}
			return "\"{0}\" ապրանքի խմբի կուտակիչը` \"{1}\", բացակայում է \"Ապրանքների և ծառայությունների խմբեր\" տեղեկատուից";
		}
	}

	public static string NotInAccountsInBank
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Settlement account is missing from \"Settlement accounts\" directory";
				}
				return "Счет  \"{0}\" отсутствует в справочнике Банковские счета";
			}
			return "\"{0}\" հաշվարկային հաշիվը բացակայում է \"Հաշվարկային հաշիվներ\" տեղեկատուից";
		}
	}

	public static string NotInContract
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{1}\" contract is not signed with {0} partner";
				}
				return "С партнером \"{0}\" не заключен договор \"{1}\"";
			}
			return "\"{0}\" գործընկերոջ հետ կնքված չէ \"{1}\" պայմանագիր";
		}
	}

	public static string NotInCountry
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} country is missing from \"Countries\" directory";
				}
				return "Страна \"{0}\" отсутствует в справочнике Страны";
			}
			return "\"{0}\" երկիրը բացակայում է \"Երկրներ\" տեղեկատուից";
		}
	}

	public static string NotInCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} currency is missing from \"Currencies\" directory";
				}
				return "Валюта \"{0}\" отсутствует в справочнике Валюты";
			}
			return "\"{0}\" արժույթը բացակայում է \"Արտարժույթներ\" տեղեկատուից";
		}
	}

	public static string NotInKassa
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} cash desk is missing from \"Cash desks\" directory";
				}
				return "Касса \"{0}\" отсутствует в справочнике Кассы";
			}
			return "\"{0}\" դրամարկղը բացակայում է \"Դրամարկղեր\" տեղեկատուից";
		}
	}

	public static string NotInMaterials
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} product is missing from \"Products and services\" directory";
				}
				return "Товар \"{0}\" отсутствует в справочнике Товары и услуги";
			}
			return "\"{0}\" ապրանքը բացակայում է \"Ապրանքներ և ծառայություններ\" տեղեկատուից";
		}
	}

	public static string NotInMaterialsByMTID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product with {0} inner system number(ISN) is missing from \"Products and Services\" directory.";
				}
				return "Товар с внутренним системным идентификатором \"{0}\" отсутствует в справочнике Товары и услуги";
			}
			return "\"{0}\" ներքին սիստեմային բնութագրիչով ապրանքը բացակայում է \"Ապրանքներ և ծառայություններ\" տեղեկատուից";
		}
	}

	public static string NotInMTGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} product group is missing from \"Products and services groups\" directory";
				}
				return "Группа товара \"{0}\" отсутствует в справочнике Группы товаров и услуг";
			}
			return "\"{0}\" ապրանքի խումբը բացակայում է \"Ապրանքների և ծառայությունների խմբեր\" տեղեկատուից";
		}
	}

	public static string NotInMTQntUnits
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{1} product with \"{0}\" unit of measure is missing from \"Unit of measures\" subdirectory of \"Products and Services\" directory.";
				}
				return "Единица измерения \"{0}\" товара \"{1}\" отсутствует в подчиненном справочнике \"Единицы измерения\" товаров и услуг";
			}
			return "\"{1}\" ապրանքի \"{0}\" չափման միավորը բացակայում է \"Ապրանքներ և ծառայություններ\" տեղեկատուի \"Չափման միավորներ\" ենթատեղեկատուից";
		}
	}

	public static string NotInMTQntUnitsByMTID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" Unit of measure of product with {1} inner system number (ISN) is missing from \"Unit of measures\" subdirectory of \"Products and Services\" directory.";
				}
				return "Единица измерения \"{0}\" товара с внутренним системным идентификатором \"{1}\" отсутствует в подчиненном справочнике Единицы измерения товаров и услуг";
			}
			return "\"{1}\" ներքին սիստեմային բնութագրիչով ապրանքի \"{0}\" չափման միավորը բացակայում է \"Ապրանքներ և ծառայություններ\" տեղեկատուի \"Չափման միավորներ\" ենթատեղեկատուից";
		}
	}

	public static string NotInParties
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0}, ({1}) batch does not exist";
				}
				return "{0}, партия ({1}) не найдена";
			}
			return "{0}, ({1}) խմբաքանակ գոյություն չունի";
		}
	}

	public static string NotInPartnerGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} partner group is missing from \"Partners groups\" directory";
				}
				return "Группа \"{0}\" отсутствует в справочнике Группы партнеров";
			}
			return "\"{0}\" գործընկերի խումբը բացակայում է \"Գործընկերների խմբեր\" տեղեկատուից";
		}
	}

	public static string NotInPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} price type is missing from \"Price type\" directory";
				}
				return "Тип цены \"{0}\" отсутствует в справочнике Типы цен";
			}
			return "\"{0}\" գնի տեսակը բացակայում է \"Գների տեսակներ\" տեղեկատուից";
		}
	}

	public static string NotInProducer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} producer is missing from \"Producers\" directory";
				}
				return "Производитель \"{0}\" отсутствует в справочнике Производители";
			}
			return "\"{0}\" արտադրողը բացակայում է \"Արտադրողներ\" տեղեկատուից";
		}
	}

	public static string NotInProperies
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" properties type is missing from directory “Properties types”";
				}
				return "Тип свойства \"{0}\" отсутствует в справочнике «Типы свойств»";
			}
			return "\"{0}\" հատկության տեսակը բացակայում է \"Հատկությունների տեսակներ\" տեղեկատուից";
		}
	}

	public static string NotInQntUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{1} Product's \"{0}\" unit of measure is missing from \"Unit of measure\" Directory";
				}
				return "Единица измерения \"{0}\" товара \"{1}\" отсутствует в справочнике Единицы измерения";
			}
			return "\"{1}\" ապրանքի \"{0}\" չափման միավորը բացակայում է \"Չափման միավորներ\" տեղեկատուից";
		}
	}

	public static string NotInQntUnitByMTID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" Unit of measure of product with {1} inner system number(ISN) is missing from \"Unit of measures\" directory.";
				}
				return "Единица измерения \"{0}\" товара с внутренним системным идентификатором \"{1}\" отсутствует в справочнике Единицы измерения";
			}
			return "\"{1}\" ներքին սիստեմային բնութագրիչով ապրանքի \"{0}\" չափման միավորը բացակայում է \"Չափման միավորներ\" տեղեկատուից";
		}
	}

	public static string NotInService
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Service is missing from \"Products and services\" directory";
				}
				return "Услуга \"{0}\" отсутствует в справочнике Товары и услуги";
			}
			return "\"{0}\" ծառայությունը բացակայում է \"Ապրանքներ և ծառայություններ\" տեղեկատուից";
		}
	}

	public static string NotInStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Storage is missing from  \"Storages\" directory";
				}
				return "Склад \"{0}\" отсутствует в справочнике Склады";
			}
			return "\"{0}\" պահեստը բացակայում է \"Պահեստներ\" տեղեկատուից";
		}
	}

	public static string NotInSubstituteItemGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" substitute item group is missing from \"Substitute Item Groups\" directory";
				}
				return "\"{0}\" группа заменяемых товаров отсутствует из справочника \"Группы заменяемых товаров\"";
			}
			return "\"{0}\" փոխարինվող ապրանքների խումբը բացակայում է \"Փոխարինվող ապրանքների խմբեր\" տեղեկատուից";
		}
	}

	public static string NotIsBonusedContracts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "None of selected contracts is with bonus";
				}
				return "Ни один из отмеченных контрактов не бонусный";
			}
			return "Նշված պայմանագրերից ոչ մեկը բոնուսային չէ";
		}
	}

	public static string NotMatchedDocs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Does not match any documents";
				}
				return "Ни один документ не соответствует";
			}
			return "Ոչ մի փաստաթուղթ չի համապատասխանել";
		}
	}

	public static string NotUnitForMaterial
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} product does not have {1} unit of measure";
				}
				return "Товар \"{0}\" не имеет единицу измерения \"{1}\"";
			}
			return "\"{0}\" ապրանքը չունի \"{1}\" չափման միավոր";
		}
	}

	public static string NotUnitForMaterialID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product with {0} system number does not have {1} Unit of measure";
				}
				return "Товар с внутренним системным идентификатором \"{0}\" не имеет единицу измерения {1}";
			}
			return "{0} ներքին սիստեմային բնութագրիչով ապրանքը չունի {1} չափման միավոր";
		}
	}

	public static string NPassport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By";
				}
				return "по";
			}
			return "Ըստ";
		}
	}

	public static string OldCost
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Old cost";
				}
				return "Старая цена ";
			}
			return "Հին գին";
		}
	}

	public static string OnlineBankQueries
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online queries";
				}
				return "Онлайн запросы";
			}
			return "Օնլայն հարցումներ";
		}
	}

	public static string OnlineBankShowDailyRemTurn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show daily rems/turns";
				}
				return "Показывать в конце дня остатки/ движение ";
			}
			return "Ցույց տալ մնաց./շարժը օրվա վերջում";
		}
	}

	public static string OnlineBankShowInNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show in \"{0}\"";
				}
				return "Показывать эквиваленты в \"{0}\"";
			}
			return "Ցույց տալ համարժեքները \"{0}\"-ով";
		}
	}

	public static string OnlineBankStatement
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank statement";
				}
				return "Выписка по банковскому счету";
			}
			return "Հաշվի քաղվածք";
		}
	}

	public static string OnlyOneContractCanbeIsDefault
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Default contract can be only one";
				}
				return "Лишь один основной договор может быть указан";
			}
			return "Թույլատրվում է նշել միայն մեկ հիմնական պայմանագիր";
		}
	}

	public static string OnlyOneSettlAccInBankCanbeIsDefault
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Default settlement account can be only one";
				}
				return "Лишь один основной банковский счет может быть указан";
			}
			return "Թույլատրվում է նշել միայն մեկ հիմնական հաշվարկային հաշիվ";
		}
	}

	public static string OpenCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Open shift";
				}
				return "Открыть смену";
			}
			return "Հերթափոխի բացում";
		}
	}

	public static string OpenMTBillExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unclosed Sale (Bill) need to be registered or drafted";
				}
				return "Незакрытый документ Реализация (Чек) необходимо зарегистрировать или сделать черновиком";
			}
			return "Անհրաժեշտ է գրանցել կամ սևագիր պահպանել չփակված վաճառք(կտրոն) փաստաթղթերը";
		}
	}

	public static string OperationDetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show the operations";
				}
				return "Детализировать по операциям";
			}
			return "Բացված ըստ գործողությունների";
		}
	}

	public static string OperationsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operations journal";
				}
				return "Операции";
			}
			return "Գործողությունների մատյան";
		}
	}

	public static string OperAvailability
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operation access";
				}
				return "Доступно";
			}
			return "Գործողության հասանելիություն";
		}
	}

	public static string OtherCashin
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Other Cash In";
				}
				return "Другой приход";
			}
			return "Այլ մուտք";
		}
	}

	public static string OtherCashOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Other Cash Out";
				}
				return "Другой расход";
			}
			return "Այլ ելք";
		}
	}

	public static string OtherExpense
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Other decrease of debt";
				}
				return "Уменьшение долга (прочие)";
			}
			return "Պարտքի այլ նվազում";
		}
	}

	public static string OtherIncome
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Other income";
				}
				return "Увеличение долга (прочие)";
			}
			return "Պարտքի այլ աճ";
		}
	}

	public static string OtherInformation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Other data";
				}
				return "Прочие данные";
			}
			return "Այլ տվյալներ";
		}
	}

	public static string OutStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Storage output";
				}
				return "Из склада";
			}
			return "Պահեստ ելքի";
		}
	}

	public static string OverwritePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are defined prices on {0} for several products. Register new prices for them?";
				}
				return "Некоторые товары имеют установленные цены на дату {0}. Установить для них новые цены?";
			}
			return "{0} ամսաթվով որոշ ապրանքների համար կան սահմանված գներ: Գրանցե՞լ դրանց համար նոր գներ";
		}
	}

	public static string PageDown
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Page down";
				}
				return "Стр. вниз";
			}
			return "Էջ ներքև";
		}
	}

	public static string PageUp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Page up";
				}
				return "Стр. вверх";
			}
			return "Էջ վերև";
		}
	}

	public static string PaidByBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Paid by bonus";
				}
				return "Оплата бонусом";
			}
			return "Մարում բոնուսից";
		}
	}

	public static string ParTAccRepCurFC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show amounts in currency";
				}
				return "Показать валютные суммы";
			}
			return "Ցույց տալ գումարները արժույթով";
		}
	}

	public static string ParTAccRepCurNC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show amounts in AMD";
				}
				return "Показать суммы  в драмах";
			}
			return "Ցույց տալ գումարները դրամով";
		}
	}

	public static string ParTAccRepCurNFC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show amounts both in AMD and in currency";
				}
				return "Показать валютные суммы и экв. в драмах";
			}
			return "Ցույց տալ գումարները և դրամով և արժույթով";
		}
	}

	public static string PartGpChildrenExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting this partner group is not allowed, it has subgroups";
				}
				return "Удаление данной группы партнеров запрещено, так как она имеет подгруппы";
			}
			return "Գործընկերների այս խումբը չի թույլատրվում հեռացնել, քանի որ այն ունի ենթախմբեր";
		}
	}

	public static string PartGpChildrenExistWithCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleting \"{0}\" partners group is not allowed, it has subgroups";
				}
				return "Удаление группы партнеров \"{0}\" запрещено, так как она имеет подгруппы";
			}
			return "Գործընկերների \"{0}\" խումբը չի թույլատրվում հեռացնել, քանի որ այն ունի ենթախմբեր";
		}
	}

	public static string PartiesValuesChanged
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Batches values changed";
				}
				return "После редактирования поля \"{0}\" значения в колонке Партия для всех строк измениться на Автоподбор";
			}
			return "\"{0}\" դաշտի արժեքի փոփոխությունից հետո խմբաքանակների արժեքները դառնալու են \"Ավտոընտրություն\"";
		}
	}

	public static string PartnerCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners card";
				}
				return "Карточка партнера";
			}
			return "Գործընկերոջ քարտ";
		}
	}

	public static string PartnerCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner code";
				}
				return "Код партнера";
			}
			return "Գործընկերոջ կոդ";
		}
	}

	public static string PartnerCodeIsNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" code of partner is missing from \"Partners\" directory";
				}
				return "Код партнера {0} отсутствует в справочнике \"Партнеры\"";
			}
			return "{0} գործընկերոջ կոդը բացակայում է \"Գործընկերներ\" տեղեկատուից";
		}
	}

	public static string PartnerContracts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contracts, {0} - {1}";
				}
				return "Договора, {0} - {1}";
			}
			return "Պայմանագրեր, {0} - {1}";
		}
	}

	public static string PartnerDataFromB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner's data from b2b.armsoft.am";
				}
				return "Данные партнера на b2b.armsoft ";
			}
			return "Գործընկերոջ տվյալները b2b.armsoft հարթակում ";
		}
	}

	public static string PartnerExistenceInTableByField
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Table {0} by field {1} contains row with specified partner.";
				}
				return "По данному партнеру существует запись с полем \"{1}\" в таблице \"{0}\"";
			}
			return "\"{0}\" աղյուսակում \"{1}\" դաշտով գոյություն ունի գրառում տրված գործընկերով:";
		}
	}

	public static string PartnerExistenceInUserParamsTable
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In Users documents options there is an entry with the given partner.";
				}
				return "В настройках пользователя для документов обнаружена запись по данному партнеру";
			}
			return "Օգտագործողների փաստաթղթի դրույթներում գոյություն ունի գրառում տրված գործընկերով:";
		}
	}

	public static string PartnerExistenceInUserParamsTableWithCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In Users' Documents options there is an entry with the \"{0}\" partner.";
				}
				return "В настройках пользователя для документов обнаружена запись по партнеру \"{0}\".";
			}
			return "Օգտագործողների փաստաթղթի դրույթներում գոյություն ունի գրառում \"{0}\" գործընկերով:";
		}
	}

	public static string PartnerExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exists partner of given type";
				}
				return "Существует партнер данного типа";
			}
			return "Գոյություն ունի տվյալ տիպի գործընկեր";
		}
	}

	public static string PartnerGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner's group";
				}
				return "Группа партнера";
			}
			return "Գործընկերոջ խումբ";
		}
	}

	public static string PartnerGroupExistsPartnerTypeWithCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners' group \"{0}\": There is a partner of type \"{1}\"";
				}
				return "Группа партнеров \"{0}\": Существует партнер типа \"{1}\"";
			}
			return "Գործընկերների \"{0}\" խումբ: Գոյություն ունի \"{1}\" տիպի գործընկեր";
		}
	}

	public static string PartnerGroupRowChangedWhenDeleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" partners' group was changed during your deletion. Update the folder and retry the action.";
				}
				return "Группа партнеров \"{0}\" была изменена во время вашего удаления. Обновите папку и повторите действие.";
			}
			return "Գործընկերների \"{0}\" խումբ Ձեր կողմից հեռացման ընթացքում փոփոխվել է ուրիշի կողմից: Թարմացրեք թղթապանակը և կրկնեք գործողությունը:";
		}
	}

	public static string PartnerIDIsNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" inner system number(ISN) of partner is missing from \"Partners\" directory";
				}
				return "Партнер с внутренним системным идентификатором \"{0}\" отсутствует в справочнике Партнеры";
			}
			return "Գործընկերոջ ներքին սիստեմային բնութագրիչը \"{0}\" բացակայում է \"Գործընկերներ\" տեղեկատուից";
		}
	}

	public static string PartnerMTDirectoryForMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Suppliers products (product {0} - {1})";
				}
				return "Товары поставщика (товар {0} - {1})";
			}
			return "Մատակարարի ապրանքացանկ (ապրանք {0} - {1})";
		}
	}

	public static string PartnerMTDirectoryForServ
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Suppliers services (service {0} - {1})";
				}
				return "Услуги поставщика (услуга {0} - {1})";
			}
			return "Մատակարարի ծառայություններ (ծառայություն  {0} - {1})";
		}
	}

	public static string PartnerMTOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Suppliers external Items list accounting";
				}
				return "Учет внешнего списка товаров поставщика";
			}
			return "Մատակարարների արտաքին ապրանքացանկի հաշվառում";
		}
	}

	public static string PartnerMTOrService
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Supplier product or service";
				}
				return "Товар или услуга поставщика";
			}
			return "Մատակարարի ապրանք կամ ծառայություն";
		}
	}

	public static string PartnerOp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners' operations";
				}
				return "Операции по партнерам";
			}
			return "Գործընկերների գործողություններ";
		}
	}

	public static string PartnerOpJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners operations journal";
				}
				return "Операции с партнерами";
			}
			return "Գործընկերների գործողությունների մատյան";
		}
	}

	public static string PartnerOpJournalShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operations journal";
				}
				return "Операции";
			}
			return "Գործողությունների մատյան";
		}
	}

	public static string PartnerRem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Debt";
				}
				return "Долг";
			}
			return "Պարտք";
		}
	}

	public static string PartnerRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners balances";
				}
				return "Остатки по партнерам";
			}
			return "Գործընկերների մնացորդներ";
		}
	}

	public static string PartnerRemainsShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balances";
				}
				return "Остатки";
			}
			return "Մնացորդներ";
		}
	}

	public static string PartnerRemCorrection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Revise partner debt";
				}
				return "Корректировка долга партнера";
			}
			return "Գործընկերոջ պարտքի վերանայում";
		}
	}

	public static string PartnerRemGroupCorrection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Revise partner debt (group)";
				}
				return "Корректировка долга партнера (групповая)";
			}
			return "Գործընկերոջ պարտքի վերանայում (Խմբային)";
		}
	}

	public static string PartnerRemMove
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner's debt movement";
				}
				return "Перенос задолженности партнера";
			}
			return "Գործընկերոջ պարտքի տեղափոխում";
		}
	}

	public static string PartnerRowChangedWhenDeleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" partner's row was changed during your deletion. Update the folder and retry the action.";
				}
				return "Строка партнера \"{0}\" была изменена во время вашего удаления. Обновите папку и повторите действие.";
			}
			return "Գործընկերոջ \"{0}\" տողը Ձեր կողմից հեռացման ընթացքում փոփոխվել է ուրիշի կողմից: Թարմացրեք թղթապանակը և կրկնեք գործողությունը:";
		}
	}

	public static string PartnersAccountsInBank
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners settlement accounts";
				}
				return "Банковские счета партнеров";
			}
			return "Գործընկերների հաշվարկային հաշիվներ";
		}
	}

	public static string PartnersCashesAccounts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners, Cashes, Banks";
				}
				return "Партнеры, Касса, Банк";
			}
			return "Գործընկերներ, Դրամարկղ/Բանկ";
		}
	}

	public static string PartnersDirectories
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners subdirectories";
				}
				return "Подчиненные справочники (партнеры)";
			}
			return "Գործընկերների ենթատեղեկատուներ";
		}
	}

	public static string PartnerSepContract
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner/Contract";
				}
				return "Партнер/Договор";
			}
			return "Գործընկեր/Պայմանագիր";
		}
	}

	public static string PartnerSepContractList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners/Contracts";
				}
				return "Партнеры/Договора";
			}
			return "Գործընկերներ/Պայմանագրեր";
		}
	}

	public static string PartnersGroupRevaluation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners group revaluation";
				}
				return "Групповая переоценка остатков по партнерам";
			}
			return "Գործընկերների խմբային վերագնահատում";
		}
	}

	public static string PartnersGroupRevaluationShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners group reval.";
				}
				return "Груп. переоцен. партнер.";
			}
			return "Գործ. խմբ. վերագն.";
		}
	}

	public static string PartnersMTAndServices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Suppliers products";
				}
				return "Товары поставщиков";
			}
			return "Մատակարարների ապրանքացանկեր";
		}
	}

	public static string PartnersMTDirectory
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Supplier Item list";
				}
				return "Товары поставщиков";
			}
			return "Մատակարարի ապրանքացանկ";
		}
	}

	public static string PartnersServicesDirectory
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Suppliers services";
				}
				return "Услуги поставщиков";
			}
			return "Մատակարարների ծառայություններ";
		}
	}

	public static string PartnersTAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners T-account";
				}
				return "Карточка партнеров";
			}
			return "Գործընկերների քարտ";
		}
	}

	public static string PartnerTurn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners turnover journal";
				}
				return "Обороты по партнерам";
			}
			return "Գործընկերների շրջանառություն";
		}
	}

	public static string PartnerTurnShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Turnover journal";
				}
				return "Обороты";
			}
			return "Շրջանառություն";
		}
	}

	public static string PartnerTypeNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" partners \"{1}\" group is missing from \"Partners groups\" directory";
				}
				return "Группа \"{1}\" партнера \"{0}\" отсутствует в справочнике \"Группы партнеров\"";
			}
			return "\"{0}\" գործընկերոջ \"{1}\" խումբը բացակայում է \"Գործընկերների խմբեր\" տեղեկատուից";
		}
	}

	public static string PartnerWithNameDoesNotExistInB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "With imputed data no company has been found.";
				}
				return "Организация с указанным наименованием не обнаружена";
			}
			return "Մուտքագրված անվանումով կազմակերպություն չի հայտնաբերվել";
		}
	}

	public static string PartSettlAccIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" partners \"{1}\" settlement account is missing from \"Partners settlement accounts\" directory";
				}
				return "Банковский счет \"{1}\" партнера \"{0}\" отсутствует в справочнике Банковские счета партнеров";
			}
			return "\"{0}\" գործընկերոջ \"{1}\" հաշվարկային հաշիվը բացակայում է \"Գործընկերների հաշվարկային հաշիվներ\" տեղեկատուից";
		}
	}

	public static string PartStateRegister
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "S/P state regist. N";
				}
				return "Гос. рег. номер ИП";
			}
			return "Ա/Ձ-ի պետ. գրանց. վկ. N";
		}
	}

	public static string PartTurnOverWrong
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" contract current balance of \"{2}\" partner in \"{1}\" currency does not equal to debit turnover - credit turnover";
				}
				return "Конечный остаток не совпадает с разницей между дебетовым и кредитовым оборотами - партнер \"{2}\", договор \"{0}\", валюта \"{1}\"";
			}
			return "\"{0}\" պայմանագրի ընթացիկ մնացորդը \"{1}\" արտարժույթով և \"{2}\" գործընկերով հավասար չէ դեբետային շրջան. - կրեդիտային շրջան.";
		}
	}

	public static string PartyDetailes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Usable batch properties";
				}
				return "Используемые свойства партии";
			}
			return "Խմբաքանակի օգտագործվող հատկություններ";
		}
	}

	public static string PartyExistsOperations
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exist operations with {0} batch";
				}
				return "Существуют операции с партией {0}, удаление запрещено";
			}
			return "{0} խմբաքանակով գոյություն ունեն գործողություններ, հեռացնելն արգելվում է";
		}
	}

	public static string PartyExistsOperationsForDraft
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exist operations with {0} batch";
				}
				return "Существуют операции с партией {0}, сделать черновым запрещено";
			}
			return "{0} խմբաքանակով գոյություն ունեն գործողություններ, սևագիր դարձնելն արգելվում է";
		}
	}

	public static string PartyMTIDinequality
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's mismatch in \"{0}\" batch document";
				}
				return "Обнаружено несоответствие товара в документе партии \"{0}\"";
			}
			return "\"{0}\" խմբաքանակի փաստաթղթի մեջ ապրանքի անհամապատասխանություն";
		}
	}

	public static string PartyNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" batch not exists.";
				}
				return "Партия \"{0}\" отсутствует.";
			}
			return "\"{0}\" խմբաքանակը բացակայում է:";
		}
	}

	public static string PartyOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Batch options";
				}
				return "Свойства партий";
			}
			return "Խմբաքանակի հատկություններ";
		}
	}

	public static string PassMagneticCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Swipe the card through the reader";
				}
				return "Считайте карту при помощи считывателя карт";
			}
			return "Անցկացրեք օգտագործողի քարտը սարքի միջով";
		}
	}

	public static string PayByBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "With bonus";
				}
				return "Бонус ";
			}
			return "Բոնուսով";
		}
	}

	public static string Payer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Received";
				}
				return "Оплачено";
			}
			return "Ստացված է";
		}
	}

	public static string PaymentByBank
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-cash payment";
				}
				return "Оплата банковским счетом";
			}
			return "Անկանխիկ մարում";
		}
	}

	public static string PaymentByBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Payment by bonus";
				}
				return "Оплата бонусом";
			}
			return "Բոնուսով մարում";
		}
	}

	public static string PaymentByCash
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash payment";
				}
				return "Оплата наличными";
			}
			return "Կանխիկ մարում";
		}
	}

	public static string PaymentByPrePay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment";
				}
				return "Оплата авансом";
			}
			return "Կանխավճարով մարում";
		}
	}

	public static string PaymentFromCustomer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customer payment";
				}
				return "Оплата от покупателя";
			}
			return "Վճարում գնորդից";
		}
	}

	public static string PaymentsForPrint
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Payments";
				}
				return "Оплаты";
			}
			return "Վճարումներ";
		}
	}

	public static string PaymentShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Paym.";
				}
				return "оплат.";
			}
			return "վճար.";
		}
	}

	public static string PaymentsLabelForPrint
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Label for \"Payments\" field";
				}
				return "Наименование поля \"Выплаты\"";
			}
			return "\"Վճարումներ\" դաշտի անվանում";
		}
	}

	public static string PaySummLessTotalSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Payables amount bigger than total amount";
				}
				return "Сумма платежей меньше общей суммы";
			}
			return "Վճարումների գումարը փոքր է ընդհանուր գումարից";
		}
	}

	public static string PayWithPlasticCart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-cash payment";
				}
				return "Оплата платежной картой";
			}
			return "Պլաստիկ քարտով վճարում";
		}
	}

	public static string PercentCustomDuty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Percent/Customs duty";
				}
				return "Процент / Таможенная пошлина";
			}
			return "Տոկոս / Մաքսատուրք";
		}
	}

	public static string PercentSymbol
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "%";
				}
				return "%";
			}
			return "%";
		}
	}

	public static string Picture
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Picture";
				}
				return "Изображение";
			}
			return "Նկար";
		}
	}

	public static string PieceItemOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Parameters for piece items on scales";
				}
				return "Параметры для штучных товаров в эл. весах";
			}
			return "Էլ. կշեռքում հատով հաշվառվող ապրանքի պարամետրեր";
		}
	}

	public static string PlasticCart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-cash";
				}
				return "Платежная карта";
			}
			return "Պլաստիկ քարտ";
		}
	}

	public static string PlasticCartPayment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-cash";
				}
				return "Безналич.";
			}
			return "Անկանխ.";
		}
	}

	public static string PlasticCartPaymentDialog
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-cash";
				}
				return "Безналичные";
			}
			return "Անկանխիկ";
		}
	}

	public static string PlasticCartTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Plastic cards types";
				}
				return "Виды платежных карт";
			}
			return "Պլաստիկ քարտերի տեսակներ";
		}
	}

	public static string PLUCodeAlreadyExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" PLU code is assigned to \"{1}\" product";
				}
				return "\"{0}\" PLU код присвоен \"{1}\" товару";
			}
			return "\"{0}\" PLU կոդը վերագրված է \"{1}\" ապրանքին ";
		}
	}

	public static string PLUCodeInElScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "PLU code in el. scale";
				}
				return "PLU код в эл. весах";
			}
			return "PLU կոդը էլ․ կշեռքում ";
		}
	}

	public static string PLUCodeNegetivValueErrorMessage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "PLU code can not be negative";
				}
				return "PLU код не должен быть отрицательным";
			}
			return "PLU կոդը պետք է լինի ոչ բացասական թիվ";
		}
	}

	public static string PLUCodeOutOfLengthErrorMessage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "PLU code's length cannot exceed {0} symbols";
				}
				return "Длина PLU кода не может превышать {0} символов";
			}
			return "PLU կոդի երկարությունը չի կարող գերազանցել {0} նիշը";
		}
	}

	public static string Plus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(+)";
				}
				return "(+)";
			}
			return "(+)";
		}
	}

	public static string Point
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Point";
				}
				return "Единица";
			}
			return "Միավոր";
		}
	}

	public static string PointOfSalePOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Point of sale";
				}
				return "Точка продаж (POS)";
			}
			return "Վաճառակետ (POS)";
		}
	}

	public static string PoleDisplay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customer display";
				}
				return "Дисплей покупателя";
			}
			return "Հաճախորդի էկրան";
		}
	}

	public static string PoleDisplayOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customer display options";
				}
				return "Параметры дисплея покупателя";
			}
			return "Հաճախորդի էկրանի պարամետրեր";
		}
	}

	public static string PoleDisplayOptionsBackground
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Background";
				}
				return "Фон";
			}
			return "Ֆոն";
		}
	}

	public static string PoleDisplayOptionsHeader
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Header";
				}
				return "Заглавный текст";
			}
			return "Գլխագիր";
		}
	}

	public static string PoleDisplayOptionsMonitor
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Monitor";
				}
				return "Дисплей";
			}
			return "Մոնիտոր";
		}
	}

	public static string PoleDisplayOptionsPicture
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product Picture";
				}
				return "Изображение товара";
			}
			return "Ապրանքի նկար";
		}
	}

	public static string POSOptionsInfoMessage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Full regulations of user's Point of sale (POS) workplace are made from Document's Options.\r\n-  In General section you set default values - Storage, Cash, Currency, Seller\r\n-  In Sale (Bill) section you regulate the following options - Current row panel, Price type, Price rounding degree, Product and ECR checks, POS-terminal and hot keys of Commands";
				}
				return "Полноценная настройка пользователя рабочего места POS осуществляется из текущего и прилегающих страниц \"Настроек для документов\".\r\n-  На странице \"Общие\" указываются значения по умолчанию - Склад, Касса, Валюта, Продавец \r\n-  На странице Реализация (Чек) регулируются следующие настройки - Дисплей покупателя, Tип цены, Метод округления суммы, Tоварный чек и чек ККМ, POS-терминал и горячие клавиши Команд";
			}
			return "Օգտագործողի Վաճառակետ (POS) աշխատատեղի լիարժեք կարգավորումը կատարվում է «Լրացուցիչ դրույթներ և իրավասություններ» պատուհանի ընթացիկ և հարակից էջերից:\r\n-  «Ընդհանուր» էջում նշվում են լռությամբ կիրառվող արժեքները` Պահեստ, Դրամարկղ, Արժույթ, Վաճառող\r\n-  «Վաճառք (Կտրոն)» էջից կարգավորվում են հետևյալ դրույթները` Հաճախորդի էկրանը, Գնի տեսակը, Գումարի կլորացման եղանակը, Ապրանքային և ՀԴՄ կտրոնները, POS-տերմինալը և Հրամանների արագ հասանելիության ստեղները";
		}
	}

	public static string POSTerminal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "POS terminal";
				}
				return "POS терминал";
			}
			return "POS-տերմինալ";
		}
	}

	public static string POSTerminalContract
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "POS Terminal provision contract";
				}
				return "Договор по предоставлению POS-терминала";
			}
			return "POS-տերմինալի տրամադրման պայմանագիր";
		}
	}

	public static string POSTerminalPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "POS Bank";
				}
				return "Банк предоставляющий POS-терминал";
			}
			return "POS-տերմինալը տրամադրող բանկ";
		}
	}

	public static string Postponed
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Delayed bills";
				}
				return "Отложенные чеки";
			}
			return "Հետաձգված կտրոններ";
		}
	}

	public static string Prefix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prefix";
				}
				return "Префикс";
			}
			return "Նախդիր";
		}
	}

	public static string PrePayment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment";
				}
				return "Аванс";
			}
			return "Կանխավճար";
		}
	}

	public static string PrepaymentByCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment (Card)";
				}
				return "Аванс (Безналичные)";
			}
			return "Կանխավճարի մուտք (անկանխիկ)";
		}
	}

	public static string PrepaymentCash
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment (Cash)";
				}
				return "Аванс (Наличные)";
			}
			return "Կանխավճարի մուտք (կանխիկ)";
		}
	}

	public static string PrepaymentReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment return";
				}
				return "Возврат аванса";
			}
			return "Կանխավճարի վերադարձ";
		}
	}

	public static string PriceCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price in {0}";
				}
				return "Цена в {0}";
			}
			return "Գին {0}-ով";
		}
	}

	public static string PriceExistsQNT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exists price in given measurement unit";
				}
				return "Обнаружена цена в данной единице измерения";
			}
			return "Գոյություն ունի տվյալ չափման միավորով գին";
		}
	}

	public static string PriceForMTandQNTExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is registered price (Pricelist-\"{2}\", date-\"{3}\") with {0} product's {1} unit of measure.  Firstly, delete that price.";
				}
				return "У товара {0} установлена цена для единицы измерения {1} (тип цены - \"{2}\", дата - \"{3}\"). Сначала удалите все существующие цены";
			}
			return "{0} ապրանքի {1} չափման միավորով գրանցված է գին(գնացուցակ-\"{2}\", ամսաթիվ-\"{3}\"): Նախ հեռացրեք այդ գինը:";
		}
	}

	public static string PriceIsPositiveForMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price must be greater than zero (product '{0}')";
				}
				return "Сумма должна быть больше 0  (товар \"{0}\")";
			}
			return "Գինը պետք է մեծ լինի զրոյից (ապրանք \"{0}\")";
		}
	}

	public static string PriceListCost
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price in Pricelist";
				}
				return "Цена в прейскуранте";
			}
			return "Գնացուցակի գին";
		}
	}

	public static string PriceListCostWithoutVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price in Pricelist (without VAT)";
				}
				return "Цена в прейскуранте (без НДС)";
			}
			return "Գնացուցակի գին (Առանց ԱԱՀ)";
		}
	}

	public static string PriceRoundDegree
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price rounding degree";
				}
				return "Точность округления";
			}
			return "Գնի կլորացման աստիճան";
		}
	}

	public static string PricesFormation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Formation of prices";
				}
				return "Формирование цен";
			}
			return "Գների ձևավորում";
		}
	}

	public static string PricesInputing
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enter new prices";
				}
				return "Ввести новые цены";
			}
			return "Մուտքագրել նոր գներ";
		}
	}

	public static string PricesNotChange
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No price has been changed during the operation";
				}
				return "В результате действия ни одна цена не изменилась";
			}
			return "Գործողության արդյունքում ոչ մի գին չի փոխվել";
		}
	}

	public static string PriceSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price/Amount";
				}
				return "Цена/Сумма";
			}
			return "Գին / Գումար";
		}
	}

	public static string PriceSummaCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price in curr./Amount in curr.";
				}
				return "Цена в валюте / Сумма в валюте";
			}
			return "Գին արտ. / Գումար արտ.";
		}
	}

	public static string PriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price type";
				}
				return "Тип цены";
			}
			return "Գնի տեսակ";
		}
	}

	public static string PriceTypeinContract
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} price type is used in \"{1}\" partner's \"{2}\" contract";
				}
				return "Тип цены \"{0}\" использован в договоре \"{1}\" партнера \"{2}\"";
			}
			return "\"{0}\" գնի տեսակը օգտագործված է \"{1}\" գործընկերոջ \"{2}\" պայմանագրում";
		}
	}

	public static string PrintBonusAsCash
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print bonus as cash";
				}
				return "Бонус напечатать как Наличный";
			}
			return "Բոնուսը տպել որպես կանխիկ";
		}
	}

	public static string PrintCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print";
				}
				return "Печать";
			}
			return "Տպել";
		}
	}

	public static string PrintCopies
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Copies";
				}
				return "Кол. копий";
			}
			return "Տպաքան.";
		}
	}

	public static string PrintCreditAsCash
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print credit as cash";
				}
				return "Кредит напечатать как Наличный";
			}
			return "Ապառիկը տպել որպես կանխիկ";
		}
	}

	public static string PrintCreditAsCompensation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print credit as compensation";
				}
				return "Кредит напечатать как Компенсация";
			}
			return "Ապառիկը տպել որպես փոխհատուցում";
		}
	}

	public static string PrintDiscount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print discount";
				}
				return "Печатать скидку";
			}
			return "Տպել զեղչ";
		}
	}

	public static string PrintECRCashIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print cash in ECRC";
				}
				return "Распечатать ПКО";
			}
			return "Տպել կանխիկի մուտքի ՀԴՄԿ";
		}
	}

	public static string PrintECRCashOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print cash out ECRC";
				}
				return "Распечатать РКО";
			}
			return "Տպել կանխիկի ելքի ՀԴՄԿ";
		}
	}

	public static string PrintECRPrepayment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print prepayment ECRC";
				}
				return "Распечатать аванс";
			}
			return "Տպել կանխավճարի ՀԴՄԿ";
		}
	}

	public static string PrintECRPrepaymentReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print prepaym. ret. ECRC";
				}
				return "Распечатать возврат аванса";
			}
			return "Տպել կանխավճ. վրդ. ՀԴՄԿ";
		}
	}

	public static string PrintForAltUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print for additional measurement unit";
				}
				return "Напечатать для дополнительной ед. изм.";
			}
			return "Տպել լրացուցիչ չ/մ-ի համար";
		}
	}

	public static string PrintIDCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print ID Card";
				}
				return "Напечатать Идентификационную карточку";
			}
			return "Տպել նույնականացման քարտ";
		}
	}

	public static string PrintLabel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print label";
				}
				return "Напечатать этикетку";
			}
			return "Տպել պիտակ";
		}
	}

	public static string PrintOnCheckPrinter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print on check printer";
				}
				return "Печатать на чековом принтере";
			}
			return "Տպել կտրոնի տպիչով";
		}
	}

	public static string PrintShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Pr.";
				}
				return "Печать";
			}
			return "Տպ․";
		}
	}

	public static string ProcessUnprocessed
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Register draft documents";
				}
				return "Проводить черновые документы";
			}
			return "Գրանցել սևագիր փաստաթղթերը";
		}
	}

	public static string ProducerInMaterials
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Producer cannot be deleted";
				}
				return "Удаление производителя \"{0}\" запрещено";
			}
			return "\"{0}\" արտադրողին չի կարելի հեռացնել";
		}
	}

	public static string ProducerInParties
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "A batch exists with {0} producer";
				}
				return "По производителю \"{0}\" существует партия";
			}
			return "\"{0}\" արտադրողով գոյություն ունի խմբաքանակ";
		}
	}

	public static string Profit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Profit in AMD";
				}
				return "Прибыль в драмах";
			}
			return "Շահույթը դրամով";
		}
	}

	public static string ProfitCur
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Profit in {0}";
				}
				return "Прибыль в {0}";
			}
			return "Շահույթը {0}-ով";
		}
	}

	public static string ProfitPercentCur
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Profit (%) in {0}";
				}
				return "Прибыль (%) {0}";
			}
			return "Շահույթը (%) {0}-ով";
		}
	}

	public static string ProgramName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "AS-Trade";
				}
				return "AS-Trade";
			}
			return "ՀԾ-Առևտուր";
		}
	}

	public static string Properties
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Properties";
				}
				return "Свойства";
			}
			return "Հատկություններ";
		}
	}

	public static string PropertiesListsValues
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Properties values";
				}
				return "Значение свойств";
			}
			return "Հատկությունների արժեքներ";
		}
	}

	public static string PropertyValueDoesNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For the property type \"{0}\" value \"{1}\" does not exist";
				}
				return "Для типа свойства \"{0}\" значение \"{1}\" не существует";
			}
			return "\"{0}\" հատկության տեսակի համար \"{1}\" արժեք գոյություն չունի";
		}
	}

	public static string PurchaseDocumentsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Purchase documents";
				}
				return "Документы покупок";
			}
			return "Գնման փաստաթղթեր";
		}
	}

	public static string PurchasePrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document's purchase price";
				}
				return "Цена закупки указанной в документе";
			}
			return "Փաստաթղթում նշված ձեռքբերման գին";
		}
	}

	public static string Purchases
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Purchases";
				}
				return "Покупки";
			}
			return "Գնումներ";
		}
	}

	public static string QntUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unit of measure";
				}
				return "Единица измерения";
			}
			return "Չափման միավոր";
		}
	}

	public static string QTYBegin
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Starting quantity";
				}
				return "Начальное количество";
			}
			return "Սկզբնական քանակ";
		}
	}

	public static string QTYEnd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ending quantity";
				}
				return "Конечное количество";
			}
			return "Վերջնական քանակ";
		}
	}

	public static string QTYIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Qnt. in";
				}
				return "Количество прихода";
			}
			return "Քանակ մուտք";
		}
	}

	public static string QtyMinus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Qty. -";
				}
				return "Кол. -";
			}
			return "Քան. -";
		}
	}

	public static string QTYOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Qnt. out";
				}
				return "Количество расхода";
			}
			return "Քանակ ելք";
		}
	}

	public static string QtyPlus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Qty. +";
				}
				return "Кол. +";
			}
			return "Քան. +";
		}
	}

	public static string QU
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "U/M";
				}
				return "Ед. изм.";
			}
			return "Չ/Մ";
		}
	}

	public static string QuantityAndSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Qauntity/sum";
				}
				return "Количество/Сумма";
			}
			return "Քանակ/Գումար";
		}
	}

	public static string QuantityIsPositiveForMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quantity must be greater than zero (product \"{0}\" )";
				}
				return "Количество должно быть больше чем 0 (товар \"{0}\"))";
			}
			return "Քանակը պետք է մեծ լինի զրոյից (ապրանք \"{0}\")";
		}
	}

	public static string ReceivablesConfirmExceed
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners receivables exceeding the receivables limit in \"{0}\" contract. Confirm document registration.";
				}
				return "Дебиторская задолженность партнера превышает лимит по договору \"{0}\". Подтвердите регистрацию документа";
			}
			return "Գործընկերոջ պարտքը գերազանցում է \"{0}\" պայմանագրի վրա նշված դեբիտորական պարտքի սահմանաչափը: Հաստատեք փաստաթղթի գրանցումը:";
		}
	}

	public static string ReceivablesExceedDenied
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners receivables exceeding the receivables limit in \"{0}\" contract ";
				}
				return "Дебиторская задолженность партнера превышает лимит по договору \"{0}\"";
			}
			return "Գործընկերոջ պարտքը գերազանցում է \"{0}\" պայմանագրի վրա նշված դեբիտորական պարտքի սահմանաչափը";
		}
	}

	public static string ReceivablesLimit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receivables limit";
				}
				return "Лимит дебиторской задолженности";
			}
			return "Դեբիտորական պարտքի սահմանաչափ";
		}
	}

	public static string ReceivablesLimitMustBeZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receivables limit cannot be selected if receivables control function is not enabled";
				}
				return "Лимит дебиторской задолженности не может быть выбран, если не включена функция управления дебиторской задолженности";
			}
			return "Դեբիտորական պարտքի սահմանաչափ հնարավոր չէ նշել, եթե նշված չէ դեբիտորական պարտքի հսկողություն դրոշակը";
		}
	}

	public static string ReceivedServicesJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input order and received services journal";
				}
				return "Полученные услуги и приходные ордера";
			}
			return "ՊՄՕ-ի և ստացված ծառայությունների մատյան";
		}
	}

	public static string ReceivedTaxInvoicesJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Received invoices journal";
				}
				return "Журнал полученных счетов";
			}
			return "Ստացված հաշիվների մատյան";
		}
	}

	public static string Receiver
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receiver";
				}
				return "Перенести на";
			}
			return "Ընդունող";
		}
	}

	public static string ReconciliationStatementsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reconciliation statements journal";
				}
				return "Журнал актов о взаиморасчете";
			}
			return "Փոխադարձ ակտերի մատյան";
		}
	}

	public static string RecordInDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Registration In Discount Server";
				}
				return "Регистрация в бонусном сервере";
			}
			return "Գրանցում բոնուսային սերվերում";
		}
	}

	public static string RemainsAMD
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balance in AMD";
				}
				return "Остаток в драмах";
			}
			return "Մնացորդ դրամով";
		}
	}

	public static string RemainsCur
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balance in currency";
				}
				return "Остаток в валюте";
			}
			return "Մնացորդ արժույթով";
		}
	}

	public static string RemainsforDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Account balance as of {0}";
				}
				return "Остатки по счетам на {0}";
			}
			return "Հաշիվների մնացորդներ {0} դրությամբ";
		}
	}

	public static string RemExistWithAccAndCur
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balance by \"{0}\" account and in \"{1}\" currency has been entered";
				}
				return "Остаток по счету \"{0}\" и валюте \"{1}\" внесен";
			}
			return "\"{0}\" հաշվով և \"{1}\" արտարժույթով մնացորդ մուտք արված է ";
		}
	}

	public static string RemInput
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enter balance";
				}
				return "Ввод остатка";
			}
			return "Մնացորդի մուտք";
		}
	}

	public static string RemoveDSTransactions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Delete discount server's operations";
				}
				return "Удалить операции бонусного сервера";
			}
			return "Հեռացնել բոնուսային սերվերի գործողությունները";
		}
	}

	public static string RemovePrepaymentTypeAndMergeAccRest
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Modifying partners' prepayment rows";
				}
				return "Изменение строк авансов партнеров";
			}
			return "Գործընկերների կանխավճարների տողերի ձևափոխում";
		}
	}

	public static string RemShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bal.";
				}
				return "Ост.";
			}
			return "Մն.";
		}
	}

	public static string RemWithCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return " - in currency";
				}
				return " - в валюте";
			}
			return " - արժույթով";
		}
	}

	public static string RemWithNC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return " - identical Drams";
				}
				return " - эквивалент в драмах";
			}
			return " - համարժեք դրամով";
		}
	}

	public static string RePay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Repay(-)";
				}
				return "Оплат.(-)";
			}
			return "Մարել(-)";
		}
	}

	public static string RequestQuantity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Request quantity";
				}
				return "Запрашивать количество";
			}
			return "Լրացուցիչ նշել քանակը";
		}
	}

	public static string RetailAvailabilityPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Retail price";
				}
				return "Цена продажи (Розница)";
			}
			return "Մանրածախ հաշվառման վաճառքի գին";
		}
	}

	public static string RetailReports
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Retail reports";
				}
				return "Отчеты розничной торговли";
			}
			return "Մանրածախ հաշվետվություններ";
		}
	}

	public static string RetailRepsForRetailStorages
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} report can be created only for retail storages";
				}
				return "Отчет \"{0}\" можно сформировать только для розничных складов";
			}
			return "\"{0}\" հաշվետվությունը կարելի է ստանալ միայն մանրածախ պահեստների համար";
		}
	}

	public static string RetailRevalOnlyForRetailStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products revaluation (Retail) document can be created only if input storage is retail type.";
				}
				return "Документ Переоценка товаров (розница) можно создать если товары приходуются на розничный склад";
			}
			return "Ապրանքների վերագնահատում (Մանրածախ) փաստաթուղթը կարելի է ստեղծել միայն այն դեպքում, երբ մուտքի պահեստը մանրածախ է";
		}
	}

	public static string RetailWord
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Retail";
				}
				return "Розничный";
			}
			return "Մանրածախ";
		}
	}

	public static string ReturnArchivationLogComment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "During archivation process base document's field has been emptied.";
				}
				return "В результате архивации очищено поле Документ основания";
			}
			return "Արխիվացման արդյունքում մաքրվել է հիմք փաստաթուղթ դաշտը`";
		}
	}

	public static string ReturnByCardSumGreaterThanBaseDocByCardSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Returning noncash amount is greater than noncash amount in base document";
				}
				return "Сумма возврата безналичной оплаты больше суммы безналичной оплаты документа основания";
			}
			return "Վերադարձվող անկանխիկ գումարը մեծ է հիմք փաստաթղթի անկանխիկ գումարից";
		}
	}

	public static string ReturnByCardSumMustBeZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In case of return, non-cash amount must be 0";
				}
				return "В случае возврата безналичная сумма должна быть 0";
			}
			return "Վերադարձի դեպքում անկանխիկ գումարը պետք է լինի 0";
		}
	}

	public static string ReturnByCashSumGreaterThanBaseDocByCashSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Returning cash amount is greater than cash amount in base document";
				}
				return "Сумма возврата наличной оплаты больше суммы наличной оплаты документа основания";
			}
			return "Վերադարձվող կանխիկ գումարը մեծ է հիմք փաստաթղթի կանխիկ գումարից";
		}
	}

	public static string ReturnSumGreaterThanTotalSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Returning total amount is greater than total amount";
				}
				return "Итоговая сумма возврата больше итоговой суммы";
			}
			return "Վերադարձվող ընդհանուր գումարը մեծ է Ընդհանուր գումարից";
		}
	}

	public static string ReturnSumLessThanTotalSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Returning total amount is less than total amount";
				}
				return "Итоговая сумма возврата меньше итоговой суммы";
			}
			return "Վերադարձվող ընդհանուր գումարը փոքր է Ընդհանուր գումարից";
		}
	}

	public static string ReturnSumsNotSet
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You need to fill returned amounts";
				}
				return "Укажите сумму возврата оплаты";
			}
			return "Անհրաժեշտ է նշել վերադարձվող գումարները";
		}
	}

	public static string ReturnToSuppvalidate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return to supplier document can be created only for correction invoice";
				}
				return "Документ Возврат поставщику можно создать только для корректировочного счета";
			}
			return "Վերադարձ մատակարարին փաստաթուղթ կարելի է ստեղծել միայն ճշգրտող հաշվի համար";
		}
	}

	public static string ReturnWithoutBase
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale's bill number is not specified, register return without base?";
				}
				return "Номер чека продажи не введен. Осуществить возврат без основания?";
			}
			return "Վաճառքի կտրոնի համար նշված չէ, կատարե՞լ վերադարձը առանց հիմքի";
		}
	}

	public static string ReturnWord
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return";
				}
				return "Возврат";
			}
			return "Վերադարձ";
		}
	}

	public static string RevalBankAccs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Settlement accounts balances";
				}
				return "Остатки на банковских счетах";
			}
			return "Հաշվարկային հաշիվների մնացորդները";
		}
	}

	public static string RevalCashes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash desks balances";
				}
				return "Остатки по кассам";
			}
			return "Դրամարկղերի մնացորդները";
		}
	}

	public static string RevalCashesBankAccsPartners
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "At least one of the check boxes must be checked";
				}
				return "Необходимо отметить хотя бы один из валютных ресурсов";
			}
			return "Հայտանիշերից առնվազն մեկը պետք է նշված լինի:";
		}
	}

	public static string RevalPartners
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partners balances";
				}
				return "Остатки по партнерам";
			}
			return "Գործընկերների մնացորդները";
		}
	}

	public static string RevMaterialFromRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product revaluation is done from context menu of \"Products balance\" report\r\n (If \"By different accounts \" and \"By different batches\" check boxes are selected)";
				}
				return "Переоценка себестоимости товара осуществляется из контекстного меню отчета «Остатки по товарам» (При выборе значка «По разным партиям»)";
			}
			return "Ապրանքի ինքնարժեքի վերագնահատումը կատարվում է \r\n\"Ապրանքների մնացորդներ\" հաշվետվության կոնտեքստային ցանկից \r\n(\"Ըստ տարբեր խմբաքանակների\"  հայտանիշը \r\n նշված լինելու դեպքում):";
		}
	}

	public static string RevPartnerFromRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group revaluation of partners is done from context menu \r\nopening from report of  \"Partners balance\".";
				}
				return "Групповая переоценка партнеров осуществляется из контекстного меню текущей строки отчета \"Остатки по партнерам\"";
			}
			return "Գործընկերների խմբային վերագնահատումը կատարվում է\r\n\"Գործընկերների մնացորդներ\" հաշվետվության ընթացիկ տողի վրայից  բացվող կոնտեքստային ցանկից:";
		}
	}

	public static string RoundTo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Round to {0}";
				}
				return "Округлять до {0}";
			}
			return "Կլորացնել մինչև {0}";
		}
	}

	public static string RoundToUpper
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Always round up";
				}
				return "Округлять всегда в большую сторону";
			}
			return "Կլորացնել միշտ դեպի վերև";
		}
	}

	public static string RowIsAlreadyDeleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The row is already deleted";
				}
				return "Строка уже удалена";
			}
			return "Տողն արդեն հեռացված է";
		}
	}

	public static string RowsMustHaveSameStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The selected row must have the same storage.";
				}
				return "Склады на указанных строках должны быть одинаковыми";
			}
			return "Նշված տողերի պահեստները պետք է լինեն նույնը:";
		}
	}

	public static string Sale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale";
				}
				return "Продажа";
			}
			return "Վաճառք";
		}
	}

	public static string SaleConsultantCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Remove";
				}
				return "Удалить";
			}
			return "Հեռացնել";
		}
	}

	public static string SaleConsultantCaption1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Seller";
				}
				return "Продавец";
			}
			return "վաճառողին";
		}
	}

	public static string SaleNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale N";
				}
				return "Продажа N";
			}
			return "Վաճառք N";
		}
	}

	public static string SalePrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The sale price listed in the document";
				}
				return "Цена продажи указанной в документе";
			}
			return "Փաստաթղթում նշված վաճառքի գին";
		}
	}

	public static string SalePriceCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale price {0}";
				}
				return "Цена продажи в {0}";
			}
			return "Վաճառքի գինը {0}-ով";
		}
	}

	public static string SalePriceShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales price";
				}
				return "Цена";
			}
			return "Վճ. գին";
		}
	}

	public static string SalePriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales price type";
				}
				return "Тип цен продажи";
			}
			return "Վաճառքի գնի տեսակ";
		}
	}

	public static string SaleRetailForSameStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not allowed to create Sale (Retail) document for different storages";
				}
				return "Создание документа Продажи (Розница) для разных складов запрещено";
			}
			return "Վաճառք (Մանրածախ) փաստաթուղթը չի կարելի ստեղծել տարբեր պահեստների համար";
		}
	}

	public static string SalesAnalisysCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales analysis for {0} - {1} period (Storages:{2}, {3}, {4} )";
				}
				return "Анализ продаж за период {0} - {1} (Склад- {2}, {3}, {4})";
			}
			return "Վաճառքների վերլուծություն {0} - {1} ժամանակահատվածում (Պահեստ` {2}, {3}, {4})";
		}
	}

	public static string SalesBetween
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales journal {0} - {1}";
				}
				return "Анализ продаж {0} - {1}";
			}
			return "Վաճառքների վերլուծություն {0} - {1}";
		}
	}

	public static string SalesConsultant
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Seller";
				}
				return "Продавец";
			}
			return "Վաճառող";
		}
	}

	public static string SalesConsultants
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sellers";
				}
				return "Продавцы";
			}
			return "Վաճառողներ";
		}
	}

	public static string SaleShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale";
				}
				return "продаж.";
			}
			return "վաճ.";
		}
	}

	public static string SaleSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale amount";
				}
				return "Сумма продажи";
			}
			return "Վաճառքի գումար";
		}
	}

	public static string SalesWithZeroCost
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sell with 0 cost";
				}
				return "При продаже регистрировать строки с нулевой себестоимостью";
			}
			return "Վաճառելիս գրանցել 0 ինքնարժեք";
		}
	}

	public static string SaleVATSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "VAT of sale’s amount";
				}
				return "НДС продажи";
			}
			return "Վաճառքի գումարի ԱԱՀ";
		}
	}

	public static string SaleWithERCRNNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is no {0} with mentioned ECR check";
				}
				return "Не существует {0} с указанным номером ККМ чека";
			}
			return "Նշված ՀԴՄ կտրոնի համարով {0} գոյություն չունի";
		}
	}

	public static string SavePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Save prices";
				}
				return "Сохранить цены";
			}
			return "Հիշել գները";
		}
	}

	public static string SaveSalePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Save sale prices";
				}
				return "Сохранить прод. цены";
			}
			return "Հիշել վաճ. գները";
		}
	}

	public static string ScaleType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Scale type";
				}
				return "Тип весов";
			}
			return "Կշեռքի տեսակ";
		}
	}

	public static string ScaleTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Scale types";
				}
				return "Типы весов";
			}
			return "Կշեռքի տեսակներ";
		}
	}

	public static string ScannerOptionsMustBeSet
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "To turn scanner on the parameters should be filled.";
				}
				return "При подключении сканера параметры должны быть заполнены";
			}
			return "Սկաները միացնելու դեպքում պարամետրերը պետք է լրացված լինեն:";
		}
	}

	public static string ScannerReadMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bar code double scanning";
				}
				return "Повторное сканирование штрих кода";
			}
			return "Գծ. կոդը կրկնակի կարդալիս";
		}
	}

	public static string ScannerReadModes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Scanner use modes";
				}
				return "Режимы использования сканера";
			}
			return "Սկաների կիրառության ռեժիմներ";
		}
	}

	public static string SearchBarCodeInPartiesWithProperty1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Search barcode by Property 1 code";
				}
				return "Поиск штрихкода по коду свойства 1";
			}
			return "Փնտրել գծիկավոր կոդը ըստ Հատկություն1-ի կոդի";
		}
	}

	public static string SelectedDocsDatesAreDifferent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Selected documents dates are different";
				}
				return "У указанных документов разные даты";
			}
			return "Նշված փաստաթղթերի ամսաթվերը տարբեր են";
		}
	}

	public static string SelectProductsAndImport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Select products and import to AS-Trade system.";
				}
				return "Отметьте товары и импортируйте в программу AS-Trade.";
			}
			return "Նշեք ապրանքները և ներմուծեք ՀԾ-Առևտուր համակարգ:";
		}
	}

	public static string Sender
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sender";
				}
				return "Списать с";
			}
			return "Փոխանցող";
		}
	}

	public static string ServiceIncomType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Service purchasing type";
				}
				return "Вид приобретения услуги";
			}
			return "Ծառայության ձեռք բերման տեսակ";
		}
	}

	public static string ServiceIsComplectPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" service is already a component of the \"{1}\" kit";
				}
				return "Услуга \"{0}\" уже является частью комплекта \"{1}\"";
			}
			return "\"{0}\" ծառայությունն արդեն կազմում է \"{1}\" կոմպլեկտի բաղադրամաս";
		}
	}

	public static string ServicePrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Service price {0} - {1}";
				}
				return "Цена услуги {0} - {1}";
			}
			return "Ծառայության գին {0} - {1}";
		}
	}

	public static string ServicePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Service prices  {0} - {1} ({2} - {3})";
				}
				return "Цены услуги {0} - {1} ({2} - {3})";
			}
			return "Ծառայության գներ {0} - {1} ({2} - {3})";
		}
	}

	public static string ServicePricesByPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Service prices {0} - {1} (Service: {2} - {3},price type:{4})";
				}
				return "Цены услуги {0} - {1} (Услуга- {2} - {3}, Тип цены- {4})";
			}
			return "Ծառայության գներ {0} - {1} (Ծառայություն` {2} - {3}, Գնի տեսակ` {4})";
		}
	}

	public static string ServicesList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Services list";
				}
				return "Список услуг";
			}
			return "Ծառայությունների ցուցակ";
		}
	}

	public static string ServiceWithNameDoesNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Service with the name \"{0}\" does not exist.";
				}
				return "Услуга с наименованием \"{0}\" не существует";
			}
			return "\"{0}\" անվանումով ծառայություն գոյություն չունի";
		}
	}

	public static string SetAsPOSForCurrentDB
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add to POS list";
				}
				return "Добавить в список POS";
			}
			return "Ավելացնել Վաճառակետերի ցանկում (POS)";
		}
	}

	public static string SetFocusOnField
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shift to field \"{0}\"";
				}
				return "Переход на поле \"{0}\"";
			}
			return "Անցում \"{0}\" դաշտին";
		}
	}

	public static string SetFocusOnGrid
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shift to Items list";
				}
				return "Список товаров";
			}
			return "Ապրանքացանկ";
		}
	}

	public static string SetSalePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill sale price";
				}
				return "Заполнить цены продаж";
			}
			return "Լրացնել վաճառքի գները";
		}
	}

	public static string SettlAccCurrIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{1}\" currency of \"{0}\" settlement account is missing from \"Currencies\" directory";
				}
				return "Валюта \"{1}\" банковского счета \"{0}\" отсутствует в справочнике \"Валюты\"";
			}
			return "\"{0}\" հաշվարկային հաշվի \"{1}\" արժույթը բացակայում է \"Արտարժույթներ\" տեղեկատուից";
		}
	}

	public static string ShiftNumberWithCashdeskAlreadyExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is \"{1}\" shift number with \"{0}\" cashier already.";
				}
				return "Номер смены \"{1}\" для кассира \"{0}\" уже существует.";
			}
			return "\"{0}\" դրամարկղով \"{1}\" համարի հերթափոխ արդեն գոյություն ունի";
		}
	}

	public static string ShortCostPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost․ Price in AMD";
				}
				return "Себ: Цена в драмах";
			}
			return "Ինք․ Գին դրամով";
		}
	}

	public static string ShortCostPriceCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost․ Price in {0}";
				}
				return "Себ: Цена в {0}";
			}
			return "Ինք․ Գին {0}-ով";
		}
	}

	public static string ShortCostSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost․ Amount in AMD";
				}
				return "Себ: Сумма в драмах";
			}
			return "Ինք․ Գումար դրամով";
		}
	}

	public static string ShortCostSummaCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost․ Amount in {0}";
				}
				return "Себ: Сумма в {0}";
			}
			return "Ինք․ Գումար {0}-ով";
		}
	}

	public static string ShortCostVATPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost․ Price VAT";
				}
				return "Себ: НДС цены";
			}
			return "Ինք․ Գնի ԱԱՀ";
		}
	}

	public static string ShortCostVATSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cost․ Amount VAT";
				}
				return "Себ: НДС суммы";
			}
			return "Ինք․ Գումարի ԱԱՀ";
		}
	}

	public static string Show
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show";
				}
				return "Показать";
			}
			return "Ցույց տալ";
		}
	}

	public static string ShowALTQUNIT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show quantities with additional units";
				}
				return "Показать количество в доп. единице ";
			}
			return "Քանակները ցույց տալ լրացուցիչ չափման միավորով";
		}
	}

	public static string ShowAvailables
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show availables";
				}
				return "Включить товары, которые в наличии";
			}
			return "Ցույց տալ առկաները";
		}
	}

	public static string ShowBonusPoints
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show bonus points";
				}
				return "Показать бонусные баллы";
			}
			return "Ցույց տալ բոնուսային միավորները";
		}
	}

	public static string ShowCostAndSalePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show cost and sale prices";
				}
				return "Показать себестоимость и продажные цены";
			}
			return "Ցույց տալ ինքնարժեքի և վաճառքի գները";
		}
	}

	public static string ShowCurrSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show sums in {0}";
				}
				return "Показать суммы в {0}";
			}
			return "Ցույց տալ գումարները {0}-ով";
		}
	}

	public static string ShowDeleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show deleted";
				}
				return "Показать удаленные";
			}
			return "Ցույց տալ հեռացվածները";
		}
	}

	public static string ShowDescriptionOnInfoPanel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show description on the panel";
				}
				return "Показать характеристику";
			}
			return "Վահանակում ցուցադրել բնութագիրը";
		}
	}

	public static string ShowDiscountSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show discounts";
				}
				return "Показать скидки";
			}
			return "Ցույց տալ զեղչերը";
		}
	}

	public static string ShowGoodsCostInReportsAndDocs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show cost of products";
				}
				return "Показывать себестоимость товаров";
			}
			return "Ցույց տալ ապրանքների ինքնարժեքների գումարները";
		}
	}

	public static string ShowInfoPanel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show current row panel";
				}
				return "Показать панель текущей строки";
			}
			return "Ցուցադրել ընթացիկ տողի վահանակը";
		}
	}

	public static string Showing
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show";
				}
				return "Показать";
			}
			return "Ցուցադրվող";
		}
	}

	public static string ShowInPriceListes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show in price lists";
				}
				return "Включить в прайс-лист";
			}
			return "Ցույց տալ գնացուցակներում";
		}
	}

	public static string ShowItemPictureOnInfoPanel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show image on the panel";
				}
				return "Показать изображение на панели";
			}
			return "Վահանակում ցուցադրել նկարը";
		}
	}

	public static string ShowMinCounts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show quantities less than the set minimum";
				}
				return "Показать количества ниже минимального";
			}
			return "Ցույց տալ նվազագույնից փոքր քանակները";
		}
	}

	public static string ShowMTInclInPriceList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show only them which included in price list";
				}
				return "Показывать только включенные в прайс-лист товары";
			}
			return "Ցույց տալ միայն գնացուցակում ներառվածները";
		}
	}

	public static string ShowNCCSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show sums in drams without VAT";
				}
				return "Показать суммы в драмах (без НДС)";
			}
			return "Ցույց տալ գումարները դրամով (Առանց ԱԱՀ)";
		}
	}

	public static string ShowNCCSummaWithVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show sums in drams with VAT";
				}
				return "Показать суммы в драмах (включая НДС)";
			}
			return "Ցույց տալ գումարները դրամով (Ներառյալ ԱԱՀ)";
		}
	}

	public static string ShowOnlyInPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Included in pricelist";
				}
				return "Включенные в прайс-лист";
			}
			return "Գնացուցակում ընդգրկվածները";
		}
	}

	public static string ShowOnlyNotInPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Not included in pricelist";
				}
				return "Не включенные в прайс-лист";
			}
			return "Գնացուցակում չընդգրկվածները";
		}
	}

	public static string ShowPartnerData
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show partner data";
				}
				return "Показать доп. данные партнера";
			}
			return "Ցույց տալ գործընկերոջ տվյալները";
		}
	}

	public static string ShowPayments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Display payments by types";
				}
				return "Показать детализацию суммы";
			}
			return "Ցույց տալ գումարի բացվածքը";
		}
	}

	public static string ShowPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show sale price";
				}
				return "Показать цены продаж";
			}
			return "Ցույց տալ վաճառքի գները";
		}
	}

	public static string ShowPricesByPT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show prices by price type";
				}
				return "Показать цены по типу цен";
			}
			return "Ցույց տալ գները ըստ գնի տեսակի";
		}
	}

	public static string ShowRems
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show the balance";
				}
				return "Показать остатки";
			}
			return "Ցույց տալ մնացորդները";
		}
	}

	public static string ShowStorageAvailabilityOnInfoPanel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show the storage availability on the panel";
				}
				return "Показать остаток на складе";
			}
			return "Վահանակում ցուցադրել պահեստի մնացորդը";
		}
	}

	public static string ShowVATSumms
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show VAT amounts";
				}
				return "Показать суммы НДС";
			}
			return "Ցույց տալ ԱԱՀ գումարները";
		}
	}

	public static string ShowWholeAvailabilityOnInfoPanel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show the total balance on the panel";
				}
				return "Показать общий остаток  ";
			}
			return "Վահանակում ցուցադրել ընդհանուր մնացորդը";
		}
	}

	public static string Start
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Start";
				}
				return "Начало";
			}
			return "Սկիզբ";
		}
	}

	public static string StartingSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Initial amount";
				}
				return "Начальная сумма (AMD)";
			}
			return "Սկզբն. գումար (AMD)";
		}
	}

	public static string Statistics
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Statistics";
				}
				return "Статистика";
			}
			return "Վիճակագրություն";
		}
	}

	public static string StorageExistenceInUserParamsTable
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In Users documents options there is an entry with the given storage.";
				}
				return "В настройках пользователя для документов обнаружена запись по данному складу";
			}
			return "Օգտագործողների փաստաթղթի դրույթներում գոյություն ունի գրառում տրված պահեստով:";
		}
	}

	public static string StorageIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input storage";
				}
				return "Склад прихода";
			}
			return "Մուտքի պահեստ";
		}
	}

	public static string StorageInMTHI
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "With {0} storage there is registered operation";
				}
				return "По складу \"{0}\" записана транзакция";
			}
			return "\"{0}\" պահեստով գրանցված է գործողություն";
		}
	}

	public static string StorageNotAvailForUser
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For {0} user \"{1}\" storage is not available";
				}
				return "Склад \"{1}\" недоступен для пользователя \"{0}\"";
			}
			return "\"{0}\" օգտագործողին \"{1}\" պահեստը հասանելի չէ";
		}
	}

	public static string StorageNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" storage is missing from \"Storages\" directory";
				}
				return "Склад \"{0}\" отсутствует в справочнике \"Склады\"";
			}
			return "\"{0}\" պահեստը բացակայում է \"Պահեստներ\" տեղեկատուից";
		}
	}

	public static string StorageOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Storage options";
				}
				return "Настройки прав пользователя по складам";
			}
			return "Պահեստների դրույթներ";
		}
	}

	public static string SubDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Supplementary (additional) documents";
				}
				return "Вспомогательные документы";
			}
			return "Օժանդակ փաստաթղթեր";
		}
	}

	public static string Substitute
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Substitute";
				}
				return "Заменяемый";
			}
			return "Փոխարինել";
		}
	}

	public static string SubstituteItems
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Substitute items";
				}
				return "Заменяемые товары";
			}
			return "Փոխարինվող ապրանքներ";
		}
	}

	public static string SubstituteItemsGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Substitute items group";
				}
				return "Группы заменяемых товаров";
			}
			return "Փոխարինվող ապրանքների խումբ";
		}
	}

	public static string SubstituteItemsGroupInMaterials
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" substitute items group cannot be deleted";
				}
				return "\"{0}\" группа заменяемых товаров нельзя удалять";
			}
			return "\"{0}\" փոխարինվող ապրանքների խումբը չի կարելի հեռացնել";
		}
	}

	public static string SubstituteItemsGroups
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Substitute items groups";
				}
				return "Группы заменяемых товаров";
			}
			return "Փոխարինվող ապրանքների խմբեր";
		}
	}

	public static string SubstituteItemsGroupShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sub. item. group";
				}
				return "Груп. замен. тов.";
			}
			return "Փոխ. ապր. խումբ";
		}
	}

	public static string SubstitutionsExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Substitute items exist";
				}
				return "Существуют заменяемые товары";
			}
			return "Կան փոխարինող ապրանքներ";
		}
	}

	public static string Suffix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Suffix";
				}
				return "Суффикс";
			}
			return "Վերջավորություն";
		}
	}

	public static string SumCurIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input amount in currency";
				}
				return "Сумма прихода в валюте";
			}
			return "Մուտքի գումար արժույթով";
		}
	}

	public static string SumCurOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Output amount in currency";
				}
				return "Сумма расхода в валюте";
			}
			return "Ելքի գումար արժույթով";
		}
	}

	public static string SummaLowerCase
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "amount";
				}
				return "сумма";
			}
			return "գումար";
		}
	}

	public static string SUMMABeginCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Starting amount in {0}";
				}
				return "Начальная сумма в {0}";
			}
			return "Սկզբնական գումար {0}-ով";
		}
	}

	public static string SUMMABeginNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Starting amount in AMD";
				}
				return "Начальная сумма в драмах";
			}
			return "Սկզբնական գումար դրամով";
		}
	}

	public static string SummaChange
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash change";
				}
				return "Сдача";
			}
			return "Մանր";
		}
	}

	public static string SummaChange1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash change 1";
				}
				return "Сдача 1";
			}
			return "Մանր 1";
		}
	}

	public static string SUMMAEndCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Final amount in {0}";
				}
				return "Конечная сумма в {0}";
			}
			return "Վերջնական գումար {0}-ով";
		}
	}

	public static string SUMMAEndNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Final amount in AMD";
				}
				return "Конечная сумма в драмах";
			}
			return "Վերջնական գումար դրամով";
		}
	}

	public static string SUMMAIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "/Sum in";
				}
				return "/Сум. приход";
			}
			return "/Գում. մուտք";
		}
	}

	public static string SUMMAInCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount input in {0}";
				}
				return "Приход суммы в {0}";
			}
			return "Գումարի մուտք {0}-ով";
		}
	}

	public static string SUMMAInNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount input in drams";
				}
				return "Приход суммы в драмах";
			}
			return "Գումարի մուտք դրամով";
		}
	}

	public static string SummaInWordsAMD
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount in words in national currency";
				}
				return "Сумма в драмах прописью";
			}
			return "Գումարը դրամով բառերով";
		}
	}

	public static string SummAMD
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount in national currency";
				}
				return "Сумма в драмах";
			}
			return "Գումարը դրամով";
		}
	}

	public static string SUMMAOutCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount output in {0}";
				}
				return "Расход суммы в {0}";
			}
			return "Գումարի ելք {0}-ով";
		}
	}

	public static string SUMMAOutNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount output in AMD";
				}
				return "Расход суммы в драмах";
			}
			return "Գումարի ելք դրամով";
		}
	}

	public static string SummaPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price part";
				}
				return "Доля стоимости";
			}
			return "Գնի մաս";
		}
	}

	public static string SummaRecived
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Received (F6)";
				}
				return "Оплачено (F6)";
			}
			return "Ստացված է (F6)";
		}
	}

	public static string SummaType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount type";
				}
				return "Тип цены";
			}
			return "Գումարի տեսակ";
		}
	}

	public static string SummCurrIso
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount in {0}";
				}
				return "Сумма в {0}";
			}
			return "Գումար {0}-ով";
		}
	}

	public static string SummPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount part";
				}
				return "Доля суммы";
			}
			return "Գումարի մաս";
		}
	}

	public static string SumNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount in Dram";
				}
				return "Сумма в драмах";
			}
			return "Գումար դրամով";
		}
	}

	public static string SumNCCIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input amount in AMD";
				}
				return "Сумма прихода в драмах";
			}
			return "Մուտքի գումար դրամով";
		}
	}

	public static string SumNCCOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Output amount in AMD";
				}
				return "Сумма расхода в драмах";
			}
			return "Ելքի գումար դրամով";
		}
	}

	public static string SumOfDistribution
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Distribution amount";
				}
				return "Сумма распределения";
			}
			return "Բաշխման գումար";
		}
	}

	public static string SupplyDateIsSmallerThanDocDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Supply date is smaller than document's date";
				}
				return "Дата поставки меньше даты документа";
			}
			return "Մատակարարման ամսաթիվը փոքր է փաստաթղթի ամսաթվից";
		}
	}

	public static string TableConvertion
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Conversion of table \"{0}\"";
				}
				return "Конвертация таблицы \"{0}\"";
			}
			return "\"{0}\" աղյուսակի փոխարկում";
		}
	}

	public static string TAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's card";
				}
				return "Карточка товара";
			}
			return "Ապրանքի քարտ";
		}
	}

	public static string TAccountByBank
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "T-account (Bank - {0})";
				}
				return "Карточка банковского счета - {0}";
			}
			return "Բանկային հաշվի քարտ - {0}";
		}
	}

	public static string TAccountByBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus card {0}";
				}
				return "Карточка бонуса {0}";
			}
			return "Բոնուսի քարտ {0}";
		}
	}

	public static string TAccountByCashDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "T-account (Cash desk - {0})";
				}
				return "Карточка кассы - {0}";
			}
			return "Դրամարկղի քարտ - {0}";
		}
	}

	public static string TAccountByMaterial
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's card {0} - {1} in period, {2} - {3}";
				}
				return "Карточка товара за период  {0} - {1}, {2} - {3}";
			}
			return "Ապրանքի քարտ {0} - {1} ժամանակահատվածում, {2} - {3}";
		}
	}

	public static string TAccountByPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "T-account (Partner - {0})";
				}
				return "Карточка партнера - {0}";
			}
			return "Գործընկերոջ քարտ {0}";
		}
	}

	public static string TakeRowQty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Take row quantity";
				}
				return "Учитывать количество строк";
			}
			return "Հաշվի առնել տողի քանակը";
		}
	}

	public static string TaxInvTypeAccDoc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Waybill";
				}
				return "Накладная";
			}
			return "Ապրանքների առաքման Հ/Ա";
		}
	}

	public static string TemplatesDescription
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Description";
				}
				return "Описание";
			}
			return "Նկարագրություն";
		}
	}

	public static string TemplatesInvList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Reference about tax accounts";
				}
				return "Справка по налоговым счетам";
			}
			return "Տեղեկանք հարկային հաշիվների վերաբերյալ";
		}
	}

	public static string TerminalType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Terminal type";
				}
				return "Тип ТСД (терминала  сбора данных)";
			}
			return "Տերմինալի տեսակ";
		}
	}

	public static string TerminalTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Terminal types";
				}
				return "Типы ТСД (терминалов  сбора данных)";
			}
			return "Տերմինալի տեսակներ";
		}
	}

	public static string TheBarCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Barcode";
				}
				return "Штрихкод";
			}
			return "Գծիկավոր կոդը";
		}
	}

	public static string TheCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "the card";
				}
				return "карту";
			}
			return "քարտը";
		}
	}

	public static string TheMaterialCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's code";
				}
				return "Кoд товара";
			}
			return "Ապրանքի կոդը";
		}
	}

	public static string ThereIsNoSubstitution
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is no substitutes for \"{0}\" product";
				}
				return "Нет товаров заменяющих \"{0}\" товар";
			}
			return "\"{0}\" ապրանքին փոխարինող ապրանքներ չկան";
		}
	}

	public static string TimePeriodOverlapping
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The time period overlaps";
				}
				return "Время смены совпадает со временем другой смены";
			}
			return "Ժամանակահատվածը հատվում է այլ հերթափոխի հետ:";
		}
	}

	public static string ToBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "To bonus";
				}
				return "к бонусам";
			}
			return "բոնուսին";
		}
	}

	public static string TotalBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total (Bonus)";
				}
				return "Итого (бонус)";
			}
			return "Ընդամենը (բոնուս)";
		}
	}

	public static string TotalShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total";
				}
				return "Итого";
			}
			return "Ընդ.";
		}
	}

	public static string TouchScreenMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Touch screen";
				}
				return "Сенсорный экран";
			}
			return "Սենսորային Էկրան";
		}
	}

	public static string TransactionDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transaction date ";
				}
				return "Дата сделки";
			}
			return "Գործողության ամսաթիվ";
		}
	}

	public static string TransferringMTOverheadExpToDocs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transferring overhead expenses to documents table";
				}
				return "Перемещение документов распределения доп. расходов в специальную таблицу";
			}
			return "ՀԾԲ փաստաթղթերի տեղափոխում դեպի հատուկ աղյուսակ";
		}
	}

	public static string TransitBankPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transit Bank";
				}
				return "Транзитный банк";
			}
			return "Տարանցիկ բանկ";
		}
	}

	public static string TransitPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transit Partner";
				}
				return "Транзитный партнер";
			}
			return "Տարանցիկ գործընկեր";
		}
	}

	public static string TurnedOnb2barmsoftDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connected to b2b.armsoft DS";
				}
				return "Подключено к b2b.armsoft БС";
			}
			return "Միացված է b2b.armsoft ԲՍ";
		}
	}

	public static string TurnInAMDIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input in AMD";
				}
				return "Приход в драмах";
			}
			return "Մուտք դրամով";
		}
	}

	public static string TurnInAMDOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Output in AMD";
				}
				return "Расход в драмах";
			}
			return "Ելք դրամով";
		}
	}

	public static string TurnInCurrency
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input in currency";
				}
				return "Приход в валюте";
			}
			return "Մուտք արժույթով";
		}
	}

	public static string TurnInCurrencyOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Output in currency";
				}
				return "Расход в валюте";
			}
			return "Ելք արժույթով";
		}
	}

	public static string TurnOverAcc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Turn. debit {0} | Turn. credit {1} | Curr. balance {2} (must be {3})";
				}
				return "Деб. оборот {0}  |  Кред. оборот {1}  | Кон. остаток {2} (должно быть = {3})";
			}
			return "Շրջ. դեբետ {0}  |  Շրջ. կրեդիտ {1}  | Ընթ. մնացորդ  {2} (պետք է լինի = {3})";
		}
	}

	public static string TurnOverWrong
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" \"{1}\" current balance in \"{1}\" currency does not equal to debit turnover - credit turnover";
				}
				return "Конечный остаток не совпадает с разницей между дебетовым и кредитовым оборотами - \"{0}\", \"{1}\", валюта \"{2}\"";
			}
			return "\"{0}\" \"{1}\" ընթացիկ մնացորդը \"{2}\" արտարժույթով հավասար չէ դեբետային շրջան. - կրեդիտային շրջան.";
		}
	}

	public static string UnitInMTQNTUNITS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} unit of measure cannot be deleted: it is assigned unit of measure for Product or service.";
				}
				return "Удаление единицы измерения \"{0}\" запрещено: она является единицей измерения товара или услуги";
			}
			return "\"{0}\" չափման միավորը չի կարելի հեռացնել. այն հանդիսանում է ապրանքի կամ ծառայության չափման միավոր";
		}
	}

	public static string UnsetAsPOSForCurrentDB
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Remove from POS list";
				}
				return "Удалить из списка POS";
			}
			return "Հեռացնել Վաճառակետերի ցանկից (POS)";
		}
	}

	public static string UpdatedDocumentsCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Updated documents quantity - {0}";
				}
				return "Количество обновленных документов - {0}";
			}
			return "Թարմացված փաստաթղթերի քանակ` {0}";
		}
	}

	public static string UseBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "During sale use bonus system";
				}
				return "При продаже применить бонусную программу";
			}
			return "Վաճառքի ժամանակ օգտագործել բոնուսային համակարգ";
		}
	}

	public static string UseBonusExtension
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use bonus calculation’s extension mode";
				}
				return "Применить внешний модуль расчета бонуса";
			}
			return "Կիրառել բոնուսի հաշվարկման արտաքին մոդուլը";
		}
	}

	public static string UseCurrentDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use current date";
				}
				return "Использовать текущую дату компьютера";
			}
			return "Օգտագործել ընթացիկ ամսաթիվը";
		}
	}

	public static string UsedBonusValidate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus redemption must not exceed earned bonus";
				}
				return "Погашенный бонус не должен превышать начисленный бонус";
			}
			return "Մարվող բոնուսը չպետք է գերազանցի կուտակված բոնուսին";
		}
	}

	public static string UseDiscountExtension
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use discount calculation’s extension mode";
				}
				return "Применить внешний модуль расчета скидок";
			}
			return "Կիրառել զեղչերի հաշվարկման արտաքին մոդուլը";
		}
	}

	public static string UsedMaxBonusValidate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus repayment can not exceed the maximum limit - {0} dram";
				}
				return "Сумма оплаты бонусом не может превышать максимальный лимит - {0} драм";
			}
			return "Բոնուսով մարումը չպետք է գերազանցի բոնուսով մարելու առավելագույն չափը` {0} դրամ";
		}
	}

	public static string UseECRPOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Make noncash transaction via ECR POS terminal";
				}
				return "Безналичный платеж произвести POS-терминалом ККМ";
			}
			return "Անկանխիկ գործարքը կատարել ՀԴՄ-ի POS-տերմինալով";
		}
	}

	public static string UseECRPOSWrapped
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Make transaction by ECR {0} POS-terminal";
				}
				return "Выполнить операцию {0} POS-терминалом кассы";
			}
			return "Գործարքը կատարել ՀԴՄ-ի {0}POS-տերմինալով";
		}
	}

	public static string UseLife
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exp. date";
				}
				return "Срок годн.";
			}
			return "Օգտ.ժամկետ";
		}
	}

	public static string UseLifeLong
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Expiration date";
				}
				return "Срок годности";
			}
			return "Օգտագործման ժամկետ";
		}
	}

	public static string UseOnlyNCCForCashierShift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You aren't allowed to use currency when using cashier shift";
				}
				return "Продажи/возвраты за смену можно оформлять только в национальной валюте ";
			}
			return "Հերթափոխեր օգտագործելիս թույլատրվում է օգտագործել միայն ազգային արժույթ";
		}
	}

	public static string UsePrePayment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment redemption";
				}
				return "Зачет аванса";
			}
			return "Կանխավճարի մարում";
		}
	}

	public static string UsePrePaymentPart1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepay.";
				}
				return "Аванс:";
			}
			return "Կանխավ.";
		}
	}

	public static string UsePrePaymentPart2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use";
				}
				return "Опл.";
			}
			return "մար.";
		}
	}

	public static string UsePrePaymentShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prep. redemption";
				}
				return "Зачет аванса";
			}
			return "Կանխավ. մար.";
		}
	}

	public static string UserCannotCreateCashIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} user has no permission to create Cashin document";
				}
				return "{0} пользователь не имеет полномочия создавать приходный ордер";
			}
			return "{0} օգտագործողն իրավասու չէ ստեղծել ԴՄՕ փաստաթուղթ";
		}
	}

	public static string UserCardNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Card number";
				}
				return "Номер карты";
			}
			return "Քարտի համար";
		}
	}

	public static string UserCashesAvailability
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" user's access to cash desks";
				}
				return "Доступ к кассам, пользователь \"{0}\"";
			}
			return "\"{0}\" օգտագործողի դրամարկղերի հասանելիություն";
		}
	}

	public static string UserOwnsDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are documents created by \"{0}\" user. Deleting user is not allowed.";
				}
				return "Есть документы, которые принадлежат пользователю \"{0}\". Запрещено удалять пользователя";
			}
			return "Գոյություն ունեն \"{0}\" օգտագործողին պատկանող փաստաթղթեր: Օգտագործողին հեռացնելն արգելվում է:";
		}
	}

	public static string UserStorageAvailability
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} user's storage availability";
				}
				return "Полномочия {0} по складам";
			}
			return "\"{0}\" օգտագործողի պահեստների հասանելիություն";
		}
	}

	public static string ValidateAccAndActualQty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Accounted and actual quantities cannot be simultaneously zero";
				}
				return "Фактическое количество и количество по данным бухгалтерского учета не могут одновременно быть равными нулю";
			}
			return "Հաշվառված և փաստացի քանակները միաժամանակ զրո լինել չեն կարող";
		}
	}

	public static string ValidateAccAndActualSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Accounted and actual amounts cannot be simultaneously zero";
				}
				return "Фактическая себестоимость и сумма по данным бухгалтерского учета не могут одновременно быть равными нулю";
			}
			return "Հաշվառված և փաստացի գումարները միաժամանակ զրո լինել չեն կարող";
		}
	}

	public static string ValidateAmountDramGTZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount in AMD should be greater than 0";
				}
				return "Сумма в драмах должна быть больше нуля";
			}
			return "Դրամային գումարը պետք է մեծ լինի զրոյից";
		}
	}

	public static string ValidateBankAccRemCurrZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On {0} cannot archive, because on that date \"{1}\" bank account balance is zero in currency, but is not zero in equivalent AMD.";
				}
				return "Не удается выполнить Архивацию, так как на дату операции {0} обнаружен банковский счет \"{1}\", у которого валютный остаток равен 0, но эквивалент не нулевой";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ \"{1}\" բանկային հաշիվն ունի մնացորդ, որն արժույթով զրոյական է, իսկ համարժեք դրամով` ոչ զրոյական";
		}
	}

	public static string ValidateBankAccRemNccZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On {0} cannot archive, because on that date \"{1}\" bank account balance is not zero in currency, but is zero in equivalent AMD.";
				}
				return "Не удается выполнить Архивацию, так как на дату операции {0} обнаружен банковский счет \"{1}\", у которого валютный ненулевой остаток , но эквивалент в драмах равен 0";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ \"{1}\" բանկային հաշիվն ունի մնացորդ, որը արժույթով ոչ զրոյական է, իսկ համարժեք դրամով` զրոյական";
		}
	}

	public static string ValidateBankAccRemNegative
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On {0} cannot archive, because on that date \"{1}\" bank account has negative balance.";
				}
				return "Не удается выполнить Архивацию, так как на дату операции {0} обнаружен отрицательный остаток по банковскому счету \"{1}\"";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ \"{1}\" բանկային հաշիվն ունի բացասական մնացորդ";
		}
	}

	public static string ValidateBonusRemNegative
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not possible to do the archivation at {0}, since at this date the balance of \"{2}\" bonus agreement of \"{1}\" partner is negative";
				}
				return "Не возможно выполнить архивацию датой {0}, так как на данную дату остаток по бонусному договору \"{2}\"  партнера \"{1}\" отрицательный";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ \"{1}\" գործընկերոջ \"{2}\" բոնուսային պայմանագրի մնացորդը բացասական է";
		}
	}

	public static string ValidateCashRemCurrZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On {0} cannot archive, because on that date \"{1}\" cash desk balance is zero in currency, but is not zero in equivalent AMD.";
				}
				return "Не удается выполнить Архивацию, так как на дату операции {0} обнаружена касса \"{1}\", у которой валютный остаток равен 0, но эквивалент не нулевой";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ \"{1}\" դրամարկղն ունի մնացորդ, որն արժույթով զրոյական է, իսկ համարժեք դրամով` ոչ զրոյական";
		}
	}

	public static string ValidateCashRemNccZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On {0} cannot archive, because on that date \"{1}\" cash desk balance is not zero in currency, but is zero in equivalent AMD.";
				}
				return "Не удается выполнить Архивацию, так как на дату операции {0} обнаружена касса \"{1}\", у которой валютный ненулевой остаток , но эквивалент в драмах равен 0";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ \"{1}\" դրամարկղն ունի մնացորդ, որը արժույթով ոչ զրոյական է, իսկ համարժեք դրամով` զրոյական";
		}
	}

	public static string ValidateCashRemNegative
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On {0} cannot archive, because on that date \"{1}\" cash desk has negative balance.";
				}
				return "Не удается выполнить Архивацию, так как на дату операции {0} обнаружен отрицательный остаток по кассе \"{1}\"";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ \"{1}\" դրամարկղն ունի բացասական մնացորդ";
		}
	}

	public static string ValidateCashSmallerTotal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The cash sum is smaller from total sum";
				}
				return "Наличная сумма оплаты меньше итоговой суммы";
			}
			return "Կանխիկ գումարը փոքր է ընդհանուր գումարից:";
		}
	}

	public static string ValidateChangeDiscountPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow discount prices change";
				}
				return "Подтвердите изменение цен со скидкой";
			}
			return "Հաստատեք զեղչված գների փոփոխումը";
		}
	}

	public static string ValidateChangePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm prices change";
				}
				return "Подтвердите изменение цен";
			}
			return "Հաստատեք գների փոփոխումը";
		}
	}

	public static string ValidateChngCustomsSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm the change of customs value";
				}
				return "Подтвердите изменение таможенных стоимостей";
			}
			return "Հաստատեք մաքսային արժեքների փոփոխումը";
		}
	}

	public static string ValidateCurCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exchange rate must be greater than zero.";
				}
				return "Значение поля Кратность должно быть больше нуля";
			}
			return "Առը պետք է մեծ լինի զրոյից";
		}
	}

	public static string ValidateCustomContrIsAMD
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customs contracts currency should be in AMD";
				}
				return "Валюта договора таможни должна быть в драмах";
			}
			return "Մաքսատան պայմանագրի արժույթը պետք է լինի դրամային";
		}
	}

	public static string ValidateDepthLevIsLimit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is impossible to provide that level depth";
				}
				return "Данная глубина вложенности для справочника групп не поддерживается";
			}
			return "Խմբերի տեղեկատուի այդ աստիճան մակարդակների խորությունը հնարավոր չէ ապահովել";
		}
	}

	public static string ValidateDocNAlreadyExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{1}\" by \"{0}\" number exists";
				}
				return "\"{1}\" с номером \"{0}\" существует";
			}
			return "\"{0}\" համարով \"{1}\" գոյություն ունի";
		}
	}

	public static string ValidateECROperatorOrECROperatorPin
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ECR operator or ECR operator pin not set for \"{0}\" cashdesk.";
				}
				return "Для кассы \"{0}\" не задан код оператора ККМ или пароль оператора ККМ";
			}
			return "\"{0}\" դրամարկղի ՀԴՄ օպերատորի կոդը կամ ՀԴՄ օպերատորի գաղտնաբառը տրված չէ:";
		}
	}

	public static string ValidateEmptyCurrCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Currency code is not filled";
				}
				return "Код валюты не заполнен";
			}
			return "Արժույթի կոդը լրացված չէ";
		}
	}

	public static string ValidateEmptyMtList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products list is empty";
				}
				return "Список товаров пуст";
			}
			return "Ապրանքների ցուցակը դատարկ է";
		}
	}

	public static string ValidateEntrListIsEmpty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transactions list is empty";
				}
				return "Список проводок пустой";
			}
			return "Գործառնությունների ցանկը դատարկ է";
		}
	}

	public static string ValidateExchRate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exchange rate should be greater than zero";
				}
				return "Курс должен быть больше нуля";
			}
			return "Փոխարժեքը պետք է մեծ լինի զրոյից";
		}
	}

	public static string ValidateIntervalsOfBonusScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus scale rows must not intersect";
				}
				return "Строки бонусной шкалы не должны пересекаться друг с другом";
			}
			return "Բոնուսային սանդղակի տողերը իրար հետ պետք է հատում չունենան";
		}
	}

	public static string ValidateInvalidRecAccKey
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receiver settlement account is incorrect. Do you want to continue?";
				}
				return "Ключ счета получателя неверный, продолжить?";
			}
			return "Ստացողի հաշվի բանալին սխալ է, շարունակե՞լ";
		}
	}

	public static string ValidateMTReturnQntUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{1}\" unit of measure of returned Item \"{0}\" cannot be different from unit of measure mentioned in base document";
				}
				return "Единица измерения \"{1}\" возвращаемого товара \"{0}\" не должна отличатся от единицы измерения указанного в документе основании";
			}
			return "\"{0}\" վերադարձվող ապրանքի \"{1}\" չափման միավորը չի կարող տարբերվել հիմք-փաստաթղթում նշված չափման միավորից:";
		}
	}

	public static string ValidatePartnerContractRemCurrZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not possible to do the archivation at {0}, because at this date the balance of \"{2}\" agreement of \"{1}\" partner is zero in currency, but is not zero in equivalent AMD.";
				}
				return "Не возможно выполнить архивацию датой {0}, так как на данную дату по договору \"{2}\"  партнера \"{1}\" есть остаток, который в валюте нулевой, а эквивалент в драмах не нулевой";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ “{1}” գործընկերը “{2}” պայմանագրով ունի մնացորդ, որն արժույթով զրոյական է, իսկ համարժեք դրամով` ոչ զրոյական";
		}
	}

	public static string ValidatePartnerContractRemNccZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not possible to do the archivation at {0}, because at this date the balance of \"{2}\" agreement of \"{1}\" partner is not zero in currency, but is zero in equivalent AMD.";
				}
				return "Не возможно выполнить архивацию датой {0}, так как на данную дату по договору \"{2}\"  партнера \"{1}\" есть остаток, который в валюте не нулевой, а эквивалент в драмах нулевой";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվի դրությամբ “{1}” գործընկերը “{2}” պայմանագրով ունի մնացորդ, որն արժույթով ոչ զրոյական է, իսկ համարժեք դրամով` զրոյական";
		}
	}

	public static string ValidatePartnerRowInvalidPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wrong price type {0}";
				}
				return "Неверный тип цены {0}";
			}
			return "Սխալ գնի տեսակ {0}";
		}
	}

	public static string ValidatePercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Percent must be greater than \"{0}\" and less than \"{1}\"";
				}
				return "Процент должен быть больше \"{0}\" и меньше \"{1}\"";
			}
			return "Տոկոսը պետք է լինի մեծ \"{0}\"-ից և փոքր \"{1}\"-ից";
		}
	}

	public static string ValidatePercentInterval
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount range rows should not have intersection.";
				}
				return "Интервалы шкалы скидок не должны пересекаться";
			}
			return "Զեղչի սանդղակի տողերը իրար հետ պետք է հատում չունենան";
		}
	}

	public static string ValidatePriceIsPositive
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price should be greater than zero";
				}
				return "Цена должна быть больше нуля";
			}
			return "Գինը պետք է մեծ լինի զրոյից";
		}
	}

	public static string ValidateQuantIsPositive
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quantity must be greater than zero";
				}
				return "Количество должно быть больше нуля";
			}
			return "Քանակը պետք է մեծ լինի զրոյից";
		}
	}

	public static string ValidateRowQTYAndPartiesQty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Rows quantity is not equal to quantity of chosen batches: rows quant. {0}, batches quant. - {1}, storage {2}, product {3}";
				}
				return "Количество строк не совпадает с количеством выбранных партий:\r\nЧисло строк - {0}, количество партий - {1}, склад - {2}, товар {3}";
			}
			return "Տողի քանակը հավասար չէ ընտրված խմբաքանակների քանակների գումարին`  տողի քանակ - {0}, խմբաքանակների քանակ - {1}\r\nպահեստ {2}, ապրանք {3}";
		}
	}

	public static string ValidBarCodeUnique
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Barcode is given to  \"{1}\" product or service";
				}
				return "Штрихкод \"{0}\" присвоен товару или услуге с кодом \"{1}\"";
			}
			return "\"{0}\" գծիկավոր կոդը վերագրված է \"{1}\" կոդով ապրանքին կամ ծառայությանը";
		}
	}

	public static string ValidBarCodeUniqueForQUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} Barcode is given to  \"{1}\" product or service with \"{2}\" unit of measure";
				}
				return "Штрихкод \"{0}\" присвоен товару или услуге с кодом \"{1}\" и единицей измерения \"{2}\"";
			}
			return "\"{0}\" գծիկավոր կոդը վերագրված է \"{1}\" կոդով \"{2}\" չափման միավորով ապրանքին կամ ծառայությանը";
		}
	}

	public static string ValidBusAddrListDuplicated
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Record of \"{0}\" additional addresses is duplicate.";
				}
				return "Повтор записи \"{0}\" в списке дополнительных адресов";
			}
			return "\"{0}\" գրառման կրկնություն լրացուցիչ հասցեներ ցուցակում:\r\n";
		}
	}

	public static string ValidCardNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" Card doesn’t exist in Partners/Employees directory";
				}
				return "Карта с номером \"{0}\" не существует в директории партнеры/сотрудники";
			}
			return "\"{0}\" համարով քարտը գոյություն չունի գործընկերների/աշխատակիցների տեղեկատուում:";
		}
	}

	public static string ValidClosingAndActualneesDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Archivation on {0} cannot be made, because it is greater than reliability date {1}.";
				}
				return "Не удается выполнить Архивацию, так как дата операции {0} больше Даты актуальности {1}";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այն մեծ է հավաստիության ամսաթվից` {1}";
		}
	}

	public static string ValidClosingDateForDocTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On {0} cannot archive, because after that date \"{1}\" document exists, which is based on document registered before archive date.";
				}
				return "Не удается выполнить Архивацию, так как после даты операции {0} обнаружен документ \"{1}\", который основан на документе зарегистрированный до даты архивации   ";
			}
			return "{0} ամսաթվով արխիվացում հնարավոր չէ կատարել, քանի որ այդ ամսաթվից հետո գոյություն ունի \"{1}\" փաստաթուղթ, որը հիմնված է արխիվացման ամսաթվից առաջ գրանցված փաստաթղթի վրա";
		}
	}

	public static string ValidCreateDocFromGoodsAvail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Documents can be created from the context menu of \"Products balance\" report, if \"By different storages\" and \"By different batches\" check boxes are selected.";
				}
				return "Для создания документов на основе отчета по Остаткам товаров необходимо создать отчет указывая значки \"Детализировать по складам\" или \"По разным партиям\".";
			}
			return "Ապրանքների մնացորդների հիման վրա փաստաթղթեր ստեղծելու համար անհրաժեշտ է հաշվետվությունը ստեղծել \"Ըստ տարբեր խմբաքանակների\" կամ \"Բացված ըստ պահեստների\" դրոշակները նշված:";
		}
	}

	public static string ValidCreateDocFromGoodsRetailAvail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Documents can be created from the context menu of \"Products balance (Retail)\" report, if \"By different storages\" check box is selected.";
				}
				return "Для создания документов на основе отчета по Остаткам товаров (розница) необходимо создать отчет указывая значок \"Детализировать по складам\".";
			}
			return "Ապրանքների մնացորդների (Մանրածախ) հիման վրա փաստաթղթեր ստեղծելու համար անհրաժեշտ է հաշվետվությունը ստեղծել \"Բացված ըստ պահեստների\" դրոշակը նշված:";
		}
	}

	public static string ValidCreateDocFromGoodsTurnOver
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Documents can be created from the context menu of \"Products turnover\" report, if \"By different storages\" and \"By different batches\" check boxes are selected.";
				}
				return "Для создания документов на основе Операций товаров необходимо создать отчет указывая значки \"Детализировать по складам\" или \"По разным партиям\".";
			}
			return "Ապրանքների շրջանառությունների հիման վրա փաստաթղթեր ստեղծելու համար անհրաժեշտ է հաշվետվությունը ստեղծել \"Ըստ տարբեր խմբաքանակների\" կամ \"Բացված ըստ պահեստների\" դրոշակները նշված:";
		}
	}

	public static string ValidDbCrCurrs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Debit and Credit currencies do not match";
				}
				return "Валюты банковских счетов плательщика и получателя отличаются";
			}
			return "Վճարողի և ստացողի հաշվարկային հաշիվների արժույթները տարբերվում են";
		}
	}

	public static string ValidDefaultAccInBankDelete
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The main settlement account cannot be deleted";
				}
				return "Удаление основного банковского счета запрещено";
			}
			return "Հիմնական հաշվարկային հաշիվը չի կարելի հեռացնել";
		}
	}

	public static string ValidDiscountCardNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" Card Not Exist in Partners contracts directory.";
				}
				return "Карточка с номером \"{0}\" отсутствует в справочнике договоров с партнерами";
			}
			return "\"{0}\" համարով քարտը գոյություն չունի գործընկերների պայմանագրեր տեղեկատուում:";
		}
	}

	public static string ValidDiscountCardUnique
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} card code is assigned to \"{1}\" partner, \"{2}\" contract";
				}
				return "Код карточки \"{0}\" присвоен договору \"{2}\" с партнером \"{1}\"";
			}
			return "\"{0}\" քարտի կոդը վերագրված է \"{1}\" գործընկերոջ \"{2}\" պայմանագրին";
		}
	}

	public static string ValidEmployeeCardUnique
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" Card is assigned to employee \"{1}\"";
				}
				return "Код карты \"{0}\" приписан к сотруднику \"{1}\"";
			}
			return "\"{0}\" քարտի կոդը վերագրված է \"{1}\" աշխատակցին";
		}
	}

	public static string ValidExtraValue
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Added value % should be greater than 100 and smaller than 1000";
				}
				return "% розничной наценки должен быть больше -100 и меньше 1000";
			}
			return "Հավելագնի %-ը պետք է լինի մեծ -100-ից և փոքր 1000-ից";
		}
	}

	public static string ValidMasterRestore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The backup of {0} cannot be restored on another database";
				}
				return "Восстановление резервной копии базы {0} на другую базу запрещено";
			}
			return "{0} պահոցի պահուստային պատճենը չի կարելի վերականգնել այլ պահոցի վրա";
		}
	}

	public static string ValidMtInventory
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In products list \"{0}\" product exists";
				}
				return "В списке товаров существует товар \"{0}\"";
			}
			return "Ապրանքների ցուցակում գոյություն ունի \"{0}\" ապրանք";
		}
	}

	public static string ValidMtInventoryRetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In products list \"{0}\" product with  \"{1}\" unit of measure and {2} price exists";
				}
				return "В списке товаров существует товар \"{0}\" с единицей измерения \"{1}\" и по цене продажи {2}";
			}
			return "Ապրանքների ցուցակում գոյություն ունի \"{0}\" ապրանք \"{1}\" չափման միավորով և {2} վաճառքի գնով";
		}
	}

	public static string ValidMtInventoryRetail2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In products list \"{0}\" product with  \"{1}\" unit of measure and {2} price exists";
				}
				return "В списке существует товар \"{0}\" с продажной ценой \"{2}\" для единицы измерения \"{1}\". Введенное Вами продажная цена \"{4}\" и единица измерения \"{3}\" совпадают с данной строкой";
			}
			return "Ապրանքների ցուցակում գոյություն ունի \"{0}\" ապրանք \"{1}\" չափման միավորով և {2} վաճառքի գնով:\r\n     Ձեր մուտքագրած \"{3}\" չափման միավորը և {4} վաճառքի գինը համարժեք է այդ տողին";
		}
	}

	public static string ValidMtInventoryRetail3
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In products list \"{0}\" product with  \"{1}\" unit of measure and {2} price exists";
				}
				return "В списке товаров существует товар \"{0}\" с единицей измерения \"{1}\" и по цене продажи {2}: Строка с единицей измерения \"{3}\" и по цене продажи {4} эквивалентна данной строке";
			}
			return "Ապրանքների ցուցակում գոյություն ունի \"{0}\" ապրանք \"{1}\" չափման միավորով և {2} վաճառքի գնով:\r\n\"{3}\" չափման միավորով և {4} վաճառքի գնով տողը համարժեք է այդ տողին";
		}
	}

	public static string ValidMTMoveStoragesvalues
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Entrance and exit storages are the same";
				}
				return "Склады прихода и расхода совпадают";
			}
			return "Մուտքի և ելքի պահեստները համընկնում են";
		}
	}

	public static string ValidPOSCOntractCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "POS Terminal provision contract should be in AMD";
				}
				return "Договор POS-терминала должен быть в драмах";
			}
			return "POS-տերմինալի պայմանագիրը պետք է լինի դրամային";
		}
	}

	public static string ValidRepeatedRowsPartnerContract
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" partner by \"{1}\" contract is already entered";
				}
				return "В списке Партнеры/Договоры существует строка договора \"{1}\" с партнерами \"{0}\"";
			}
			return "Գործընկերներ/Պայմանագրերի ցուցակում գոյություն ունի \"{0}\" գործընկերներ \"{1}\" պայմանագրով տող";
		}
	}

	public static string ValidSummaVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The value of \"VAT amount\" must be greater than zero";
				}
				return "Значение поля \"Сумма НДС\" должно быть больше нуля";
			}
			return "\"Գումար ԱԱՀ\" դաշտի արժեքը պետք է մեծ լինի զրոյից";
		}
	}

	public static string ValidUserCardUnique
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" Card is assigned to user\"{1}\"";
				}
				return "\"{0}\" Карта закреплена на пользователя \"{1}\"";
			}
			return "\"{0}\" քարտի համարը վերագրված է \"{1}\" օգտագործողին";
		}
	}

	public static string ValidUserPeriod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The beginning and the end of the period must both be filled in or empty";
				}
				return "Начало и конец периода должны одновременно быть заполнены или пусты";
			}
			return "Ժամանակահատվածի սկիզբն ու վերջը պետք է միաժամանակ լինեն լրացված կամ դատարկ";
		}
	}

	public static string ValueDoesNotExistInTheListWithName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" field  value \"{1}\" does not exist in the \"{2}\" directory";
				}
				return "Значение \"{1}\" поля \"{0}\" отсутствует в справочнике \"{2}\"";
			}
			return "\"{0}\" դաշտի \"{1}\" արժեքը բացակայում է \"{2}\"  տեղեկատուից";
		}
	}

	public static string ViewAvailability
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "View access";
				}
				return "Разрешение на просмотр";
			}
			return "Դիտելու հասանելիություն";
		}
	}

	public static string WarehouseDocumentsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Warehouse documents";
				}
				return "Документы склада";
			}
			return "Պահեստի փաստաթղթեր";
		}
	}

	public static string WarningNegativExp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Warn in case of withdraw more than available";
				}
				return "Уведомить в случае списания в количестве превышающее фактическое количество";
			}
			return "Զգուշացնել առկայությունից ավելի ելքագրումից";
		}
	}

	public static string WatchASTrade7TrainingVideos
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "AS-Trade 7 Training Videos";
				}
				return "Посмотрите обучающие ролики о программе !";
			}
			return "ՈՒՍՈՒՄՆԱԿԱՆ ՀՈԼՈՎԱԿՆԵՐ";
		}
	}

	public static string WatchPOSVideo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Watch POS Video";
				}
				return "Посмотрите ролик !";
			}
			return "Դիտեք POS-ի հոլովակը";
		}
	}

	public static string Weight
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Weight";
				}
				return "Вес";
			}
			return "Քաշ";
		}
	}

	public static string WeightItem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "";
				}
				return "Весовой";
			}
			return "Քաշային";
		}
	}

	public static string WeightItemOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Weight Item Options";
				}
				return "Параметры для весовых товаров в эл. весах";
			}
			return "Էլ. կշեռքում քաշային ապրանքի պարամետրեր";
		}
	}

	public static string Whole
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Whole";
				}
				return "В целом";
			}
			return "Ընդհանուր";
		}
	}

	public static string WholeSaleOrRetailStorages
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} storages";
				}
				return "{0} Склады";
			}
			return "{0} պահեստներ";
		}
	}

	public static string WholeSaleWord
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wholesale";
				}
				return "Оптовый";
			}
			return "Մեծածախ";
		}
	}

	public static string Window
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Windows";
				}
				return "Окна";
			}
			return "Պատուհաններ";
		}
	}

	public static string WithAccCurrNotExistAccRest
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are no balance for \"{0}\" account \"{1}\" currency";
				}
				return "Остаток по счету \"{0}\" и в валюте \"{1}\" не найден";
			}
			return "\"{0}\" հաշվով և \"{1}\" արժույթով մնացորդը բացակայում է";
		}
	}

	public static string WithAccPartCurrNotExistAccRest
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are no balance for \"{0}\" partner \"{1}\" contract \"{2}\" currency";
				}
				return "Для партнера \"{0}\" остаток по договору \"{1}\" и в валюте \"{2}\" не найден";
			}
			return "\"{0}\" գործընկերոջ համար \"{1}\" պայմանագրով և \"{2}\" արժույթով մնացորդը բացակայում է";
		}
	}

	public static string WithAccPartCurrRemExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balance exists for \"{0}\" partner by \"{1}\" contract in \"{2}\" currency";
				}
				return "По партнеру \"{0}\" с договором \"{1}\" и валютой \"{2}\" имеется остаток";
			}
			return "\"{0}\" գործընկերի համար \"{1}\" պայմանագրով և \"{2}\" արտարժույթով կա մնացորդ";
		}
	}

	public static string WithAMD
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return " AMD";
				}
				return " в драмах";
			}
			return " դրամով";
		}
	}

	public static string WithBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By bonus";
				}
				return "бонусом";
			}
			return "բոնուսով";
		}
	}

	public static string WithCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return " In currency";
				}
				return " в валюте";
			}
			return " արժույթով";
		}
	}

	public static string WithDeletedRows
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "With deleted rows";
				}
				return "С удаленными строками";
			}
			return "Հեռացված տողերով";
		}
	}

	public static string WithNDS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(VAT included)";
				}
				return "(Включая НДС)";
			}
			return "(Ներառյալ ԱԱՀ)";
		}
	}

	public static string WithNDS_no_bracket
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "VAT included";
				}
				return "Включая НДС";
			}
			return "Ներառյալ ԱԱՀ";
		}
	}

	public static string WithoutNDS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(Without VAT)";
				}
				return "(Без НДС)";
			}
			return "(Առանց ԱԱՀ)";
		}
	}

	public static string WithoutNDS_no_bracket
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Without VAT";
				}
				return "Без НДС";
			}
			return "Առանց ԱԱՀ";
		}
	}

	public static string WithPrepayment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "With prepayment";
				}
				return "Авансом";
			}
			return "Կանխավճարով";
		}
	}

	public static string WrongComplectPart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} product is already part of \"{1}\" kit";
				}
				return "Товар \"{0}\" уже является частью комплекта \"{1}\"";
			}
			return "\"{0}\" ապրանքն արդեն կազմում է \"{1}\" կոմպլեկտի բաղադրամաս";
		}
	}

	public static string WrongNonCashSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wrong non-cash amount";
				}
				return "Неправильная сумма безналичной оплаты";
			}
			return "Անկանխիկի սխալ գումար";
		}
	}

	public static string XlEmptyServerName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Server name is empty. Login to AS-Trade program to set server name automatically.";
				}
				return "Не указано имя сервера. Для регистрации имени сервера необходимо запустить программу AS-Trade";
			}
			return "Սերվերի անունը դատարկ է, մուտք գործեք ՀԾ-Առևտուր համակարգ, սերվերի անունը կլրացվի ավտոմատ կերպով:";
		}
	}

	public static string ExportArmsoftIdentifier
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export AS-identifier";
				}
				return "Экспортировать AS-идентификатор";
			}
			return "Արտահանել ՀԾ-նույնացուցիչը";
		}
	}

	public static string QuantityIsFromElScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quantity is assigned from Electronic Scale";
				}
				return "Количество (вес) получено с эл. весов";
			}
			return "Քանակ դաշտի արժեքի վերագրումը կատարվել է էլ. կշեռքից";
		}
	}

	public static string QuantityWeightShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Qt/Weight";
				}
				return "Кол./Вес";
			}
			return "Քն./Քաշ";
		}
	}

	public static string ExistsFavMTByGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is a product or service belonging to this group";
				}
				return "В данной группе зарегистрирован товар или услуга";
			}
			return "Գոյություն ունի տվյալ խմբին պատկանող ապրանք կամ ծառայություն";
		}
	}

	public static string FavoriteGroups
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Groups of favorites";
				}
				return "Группа избранных";
			}
			return "Ընտրյալների խմբեր";
		}
	}

	public static string Groups
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Groups";
				}
				return "Группы";
			}
			return "Խմբեր";
		}
	}

	public static string ReturnMainLevel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return to the main level";
				}
				return "Возврат на главный уровень ";
			}
			return "Վերադառնալ գլխավոր մակարդակ";
		}
	}

	public static string BankCardWithECRPOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(ECR POS)";
				}
				return "(ECR POS)";
			}
			return "(ՀԴՄ POS)";
		}
	}

	public static string BankCardWithIngenicoPOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(Ingenico POS)";
				}
				return "(Ingenico POS)";
			}
			return "(Ingenico POS)";
		}
	}

	public static string BankCardWithNoPOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(no POS)";
				}
				return "(no POS)";
			}
			return "(առանց POS)";
		}
	}

	public static string BankCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bank card";
				}
				return "Банк. карта";
			}
			return "Բանկ. քարտ";
		}
	}

	public static string AmountMustBePositiveForAtLeastOneRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The amount of at least one line must be greater than 0";
				}
				return "Сумма хотя бы одной из строк должна быть больше 0";
			}
			return "Առնվազն մեկ տողի գումարը պետք է մեծ լինի 0-ից";
		}
	}

	public static string AllRowQtyOutZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For all specified rows, the value in the column \"Output quantity\" is zero";
				}
				return "Для всех указанных строк значение в колонке \"Количество расхода\" нулевое";
			}
			return "Բոլոր տողերի ելքագրված քանակները 0-ական են";
		}
	}

	public static string AllRowQtyRemZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For all specified lines, the remainders are 0";
				}
				return "Для всех указанных строк остатки равны 0";
			}
			return "Բոլոր տողերի վերջնական քանակները 0-ական են";
		}
	}

	public static string AllowCashInOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow cash input and output";
				}
				return "Разрешить внесение и выемку денег";
			}
			return "Թույլ տալ կանխիկի մուտք և ելք";
		}
	}

	public static string TransitCashDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transit cashdesk";
				}
				return "Транзитная касса";
			}
			return "Տարանցիկ դրամարկղ";
		}
	}

	public static string FillDefaultTransitCashDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For cash input or output you need to specify transit cashdesk in the user settings";
				}
				return "Для внесение или выемки денег необходимо в настройках пользователя указать транзитную кассу";
			}
			return "Կանխիկի մուտք կամ ելք անելու համար լրացրեք օգտագործողի տարանցիկ դրամարկղը";
		}
	}

	public static string CashIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash input";
				}
				return "Внесение денег";
			}
			return "Կանխիկի մուտք";
		}
	}

	public static string CashOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash output";
				}
				return "Выемка денег";
			}
			return "Կանխիկի ելք";
		}
	}

	public static string CashOutCreatedNotification
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash output order with number {0} is reagistered";
				}
				return "Расходный кассовый ордер с номером {0} зарегистрирован";
			}
			return "{0} համարով ԴԵՕ փաստաթուղթը գրանցված է";
		}
	}

	public static string InCashDesk
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Available cash in cashdesk";
				}
				return "Наличные в кассе";
			}
			return "Առկա կանխիկ գումարը";
		}
	}

	public static string ImmediatlyEditRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "After selecting the product (service) from the list, open the row's edit window";
				}
				return "При подборе товара (услуги) из списка открывать окно редактирования строки ";
			}
			return "Ցանկից ապրանքը(ծառայություն) ընտրելուց հետո բացել տողի խմբագրման պատուհանը";
		}
	}

	public static string ValidateMTReturnMTCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product with the code \"{0}\" does not exist in the base document";
				}
				return "В документе основании товар с кодом  \"{0}\" не существует";
			}
			return "Հիմք փաստաթղթում \"{0}\" կոդով ապրանք գոյություն չունի";
		}
	}

	public static string QRCodeChooseTransaction
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "QR Code choose transaction";
				}
				return "QR Code выбрать операцию";
			}
			return "QR Code ընտրել գործարքը";
		}
	}

	public static string TransactionId
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transaction ID";
				}
				return "Операции ID";
			}
			return "Գործարքի ID";
		}
	}

	public static string TransactionRowNotListed
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Operation string is not specified";
				}
				return "Строка операции не указана";
			}
			return "Գործարքի տող նշված չէ";
		}
	}

	public static string DontForgetYourCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Don't forget to take your bank card";
				}
				return "Не забудьте взять свою банковскую карту";
			}
			return "Չմոռանաք վերցնել Ձեր բանկային քարտը";
		}
	}

	public static string DontForgetYourChange
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Don't forget to take your cash change";
				}
				return "Не забудьте взять свою сдачу";
			}
			return "Չմոռանաք վերցնել Ձեր մանրը`";
		}
	}

	public static string DontForgetYourChangeAndCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Don't forget to take your bank card and cash change";
				}
				return "Не забудьте взять свою банковскую карту и сдачу";
			}
			return "Չմոռանաք վերցնել Ձեր բանկային քարտը և մանրը`";
		}
	}

	public static string ReturnBuyerCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return buyer's bank card";
				}
				return "Вернуть банковскую карту покупателя";
			}
			return "Վերադարձնել գնորդի բանկային քարտը";
		}
	}

	public static string ReturnBuyerChange
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return buyer's cash change";
				}
				return "Вернуть сдачу покупателя";
			}
			return "Վերադարձնել գնորդի մանրը`";
		}
	}

	public static string ReturnBuyerChangeAndCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return buyer's bank card and cash change";
				}
				return "Вернуть банковскую карту и сдачу покупателя";
			}
			return "Վերադարձնել գնորդի բանկային քարտը և մանրը`";
		}
	}

	public static string EditPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Edit Partner";
				}
				return "Редактировать партнера";
			}
			return "Խմբագրել գործընկերոջը";
		}
	}

	public static string EndDateIsRequired
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "End date and time of the shift must be both filled or empty.";
				}
				return "Дата и время закрытия смены должны быть обе заполнены или пусты. ";
			}
			return "Հերթափոխի ավարտի ամսաթիվը և ժամը միաժամանակ պետք է լինեն լրացված կամ դատարկ:";
		}
	}

	public static string EditGoodOrService
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Edit Product or Service";
				}
				return "Редактировать Товар или Услугу";
			}
			return "Խմբագրել Ապրանքը կամ Ծառայությունը";
		}
	}

	public static string ChooseDocType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Select the document type";
				}
				return "Выберите вид документа";
			}
			return "Ընտրեք փաստաթղթի տեսակը";
		}
	}

	public static string DocMTBillReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return (Bill)";
				}
				return "Возврат (Чек)";
			}
			return "Վերադարձ (Կտրոն)";
		}
	}

	public static string ApplyLastPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Apply the last set price and recalculate to the base U/M";
				}
				return "Применить последнюю установленную цену и пересчитать в основную единицу";
			}
			return "Կիրառել վերջին սահմանված գինը և վերահաշվարկել հիմնական չ/մ-ի";
		}
	}

	public static string MTBillPageInfo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "From the current page, the Sale (Check) and Return (Check) documents are configured";
				}
				return "На данной странице осуществляется настройка работы документов Реализация (Чек) и Возврат (Чек)";
			}
			return "Ընթացիկ էջից կարգավորվում է Վաճառք (Կտրոն) և Վերադարձ (Կտրոն) փաստաթղթերի աշխատանքը";
		}
	}

	public static string PartnerEmployeeCardCodeOptions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner/Employee Card Code Parameters";
				}
				return "Параметры карты партнера/сотрудника";
			}
			return "Գործընկերոջ/աշխատակցի քարտի կոդի պարամետրեր";
		}
	}

	public static string PartnerEmployeeCardCodePrefix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partner/Employee Card Code Prefix";
				}
				return "Префикс кода карты партнера/сотрудника ";
			}
			return "Գործընկերոջ/աշխատակցի քարտի կոդի նախդիրներ";
		}
	}

	public static string UsePartnerEmployeeCardcodePrefix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use prefix to determine card code";
				}
				return "Использовать префикс для определения кода карты";
			}
			return "Օգտագործել քարտի կոդի տարբերակման նախդիր";
		}
	}

	public static string DocNextMTBillReturnNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return (Bill) serial number";
				}
				return "Порядковый номер Возврата (Чек)";
			}
			return "Վերադարձ (Կտրոն) հերթական համար";
		}
	}

	public static string ValueDoesNotExistInTհeMTList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{1}\" value  of \"{0}\" field is not in the \"{3}\" list of \"{2}\" product";
				}
				return "Значения \"{1}\" поля \"{0}\" отсутствует из списка \"{3}\" товара \"{2}\"";
			}
			return "\"{0}\" դաշտի \"{1}\" արժեքը բացակայում է \"{2}\" կոդով  ապրանքի \"{3}\" ցուցակից:";
		}
	}

	public static string GoodsBySegment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Goods by segments";
				}
				return "Товары по сегментам";
			}
			return "Ապրանքները ըստ սեգմենտների";
		}
	}

	public static string GoodsSegmentInGoodsBySegment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product segment \"{0}\" cannot be deleted";
				}
				return "Сегмент товаров \"{0}\" нельзя удалить";
			}
			return "\"{0}\" ապրանքների սեգմենտը չի կարելի հեռացնել";
		}
	}

	public static string GoodsSegments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Goods segments";
				}
				return "Товарные сегменты";
			}
			return "Ապրանքների սեգմենտներ";
		}
	}

	public static string SegmentCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Segment name";
				}
				return "Наименование сегмента";
			}
			return "Սեգմենտի անվանում";
		}
	}

	public static string GoodBySegment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Good by segment";
				}
				return "Товар по сегменту";
			}
			return "Ապրանք ըստ սեգմենտի";
		}
	}

	public static string GoodsBySegmentAlreadyExistByGood
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" product is already included in \"{1}\" segment";
				}
				return "Товар \"{0}\" уже включен в сегмент \"{1}\"";
			}
			return "\"{0}\" ապրանքն արդեն ներառված է \"{1}\" սեգմենտում";
		}
	}

	public static string GoodsBySegmentAlreadyExistByService
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" service is already included in \"{1}\" segment";
				}
				return "Услуга \"{0}\" уже включена в сегмент \"{1}\"";
			}
			return "\"{0}\" ծառայությունն արդեն ներառված է \"{1}\" սեգմենտում";
		}
	}

	public static string Segment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Segment";
				}
				return "Сегмент";
			}
			return "Սեգմենտ";
		}
	}

	public static string MaxPercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Maximum discount";
				}
				return "Максимальная скидка";
			}
			return "Առավելագույն զեղչ";
		}
	}

	public static string Priority
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Priority";
				}
				return "Приоритет";
			}
			return "Առաջնահերթություն";
		}
	}

	public static string SequentialUsage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Successive application";
				}
				return "Последовательное применение";
			}
			return "Հաջորդական կիրառում";
		}
	}

	public static string Summation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Addition";
				}
				return "Сложение";
			}
			return "Գումարում";
		}
	}

	public static string DiscountConditionTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Types of conditions";
				}
				return "Виды условий";
			}
			return "Պայմանի տեսակներ";
		}
	}

	public static string DiscountsMergeMethods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discounts merge methods";
				}
				return "Методы объединения скидок";
			}
			return "Զեղչերի միավորման եղանակներ";
		}
	}

	public static string UseSelectedPercentDiscount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Application of % specified for the item";
				}
				return "Применение % указанного для товара";
			}
			return "Ապրանքի համար նշված %-ի կիրառում";
		}
	}

	public static string UseDiscountPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Apply discount price type";
				}
				return "Применить тип скидочной цены";
			}
			return "Զեղչի գնի տեսակի կիրառում";
		}
	}

	public static string UseDiscountPercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "% application";
				}
				return "% скидки";
			}
			return "%-ի կիրառում";
		}
	}

	public static string UsageMethods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Application methods";
				}
				return "Методы применения";
			}
			return "Կիրառման եղանակներ";
		}
	}

	public static string AlwaysCalcDiscountAuto
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Automatic calculation of discount after each action";
				}
				return "Автоматический расчет скидок при редактировании документа";
			}
			return "Ավտոմատ զեղչի հաշվարկ ամեն գործողությունից հետո";
		}
	}

	public static string AddToSegment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add to segment";
				}
				return "Добавить в сегмент";
			}
			return "Ավելացնել սեգմենտին";
		}
	}

	public static string Segments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Segments";
				}
				return "Сегменты";
			}
			return "Սեգմենտներ";
		}
	}

	public static string MtReturnsFromCustomer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Returns";
				}
				return "Возвраты";
			}
			return "Վերադարձներ";
		}
	}

	public static string ArithmeticOverflowMessage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Violation of the limit of numerical value in the pricing formula";
				}
				return "Нарушения лимита числового значения в формуле формирования цены ";
			}
			return "Գնի ձևավորման բանաձևում թվային արժեքի սահմանաչափի խախտում:";
		}
	}

	public static string MTBillRetDiffDocError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return (Bill) document cannot be created based on different documents.";
				}
				return "Документ Возврата (Чек) не возможно создать на основе разных документов.";
			}
			return "Վերադարձ (Կտրոն) փաստաթուղթը հնարավոր չէ ստեղծել տարբեր փաստաթղթերի հիման վրա:";
		}
	}

	public static string ErrInvalidBaseDocTypeForMTBillReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return (Bill) document can only be created based on Sales (Bill) lines ";
				}
				return "Документ Возврата (Чек) можно создать только на основании документа Реализации (Чек).";
			}
			return "Վերադարձ (Կտրոն) փաստաթուղթը հնարավոր է ստեղծել միայն Վաճառք (Կտրոն)-ի տողերի հիման վրա";
		}
	}

	public static string DiscountGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount group";
				}
				return "Группа скидки";
			}
			return "Զեղչի խումբ";
		}
	}

	public static string DiscountGroups
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount groups";
				}
				return "Группы скидок";
			}
			return "Զեղչերի խմբեր";
		}
	}

	public static string DiscountExistsWithGivenDiscountGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is a discount from this discount group";
				}
				return "С данной группой связана запись из справочника Скидки";
			}
			return "Տվյալ զեղչի խմբով գոյություն ունի զեղչ";
		}
	}

	public static string IsActive
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enabled";
				}
				return "Действует";
			}
			return "Գործում է";
		}
	}

	public static string UsageMethod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Application method";
				}
				return "Метод применения";
			}
			return "Կիրառման եղանակ";
		}
	}

	public static string ValueGreaterOrEqual
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Value is greater or equal";
				}
				return "Значение больше или равно";
			}
			return "Արժեքը մեծ կամ հավասար է";
		}
	}

	public static string Conditions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Conditions";
				}
				return "Условия";
			}
			return "Պայմաններ";
		}
	}

	public static string DiscountWindowCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount (Application, Terms)";
				}
				return "Скидка (Действия, Условия)";
			}
			return "Զեղչ (Կիրառում, Պայման)";
		}
	}

	public static string GoodDiscountPercentInformation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount % is inherited from the directory (card) of goods";
				}
				return "% скидки наследуется из справочника (карточки) товара ";
			}
			return "Զեղչի % ժառանգվում է ապրանքի տեղեկատուից";
		}
	}

	public static string IncludeCurrentSale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Include current sale in accumulation";
				}
				return "Учитывать текущую продажу в накоплениях";
			}
			return "Կուտակման մեջ ներառել ընթացիկ վաճառքը";
		}
	}

	public static string OpenList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Open list";
				}
				return "Открыть список";
			}
			return "Բացել ցուցակը";
		}
	}

	public static string PartnerCardInformation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount % is inherited from the partner contract directory";
				}
				return "% скидки наследуется из справочника договора партнера";
			}
			return "Զեղչի % ժառանգվում է գործընկերոջ պայմանագրերի տեղեկատուից";
		}
	}

	public static string Period
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Period";
				}
				return "Период";
			}
			return "Ժամկետ";
		}
	}

	public static string PeriodEnd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "End date";
				}
				return "Дата окончания действия";
			}
			return "Ժամկետի վերջ";
		}
	}

	public static string PeriodStart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Start date";
				}
				return "Дата начала действия";
			}
			return "Ժամկետի սկիզբ";
		}
	}

	public static string ScaleInformation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Scale is applied to the amount of sales document";
				}
				return "Шкала применяется на сумму документа реализации";
			}
			return "Սանդղակը կիրառվում է վաճառքի փաստաթղթի գումարի վրա";
		}
	}

	public static string SegmentInformation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The discount applies to products from the specified segment. If the segment is not specified, then for all products.";
				}
				return "Скидка применяться на товары из указанного сегмента. Если сегмент не указан, то на все товары.";
			}
			return "Զեղչը կիրառվում է նշված սեգմենտի ապրանքների վրա: Եթե սեգմենտ նշված չէ, ապա կամայական ապրանքի վրա:";
		}
	}

	public static string CloseAfterSelect
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Close after pick";
				}
				return "Закрыть после выбора";
			}
			return "Փակել ընտրելուց հետո";
		}
	}

	public static string ContractName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contract name";
				}
				return "Наименование договора";
			}
			return "Պայմանագրի անվանում";
		}
	}

	public static string DiscountCodeDoesNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount with code {0} does not exist";
				}
				return "Скидка с кодом {0} уже существует";
			}
			return "{0} կոդով զեղչ գոյություն չունի";
		}
	}

	public static string UserCannotCreateBillReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} user does not have permission to create a Return (Bill) document";
				}
				return "{0} пользователь не имеет полномочия создавать документ Возврата (Чек)";
			}
			return "{0} օգտագործողն իրավասու չէ ստեղծել Վերադարձ(Կտրոն) փաստաթուղթ";
		}
	}

	public static string EditOrderMinMax
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Edit ordering parameters";
				}
				return "Редактировать параметры формирование заказов";
			}
			return "Խմբագրել Պատվերների ձևավորման պարամետրերը";
		}
	}

	public static string ExistsOrderMinMax
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ordering parameters for \"{0}\" product and \"{1}\" storage already exists";
				}
				return "Параметр формирования заказa для товара \"{0}\" и склада \"{1}\" уже существует";
			}
			return "\"{0}\" կոդով \"{1}\"\" պահեստով պատվերի ձևավորման պարամետր արդեն գոյություն ունի";
		}
	}

	public static string MTOrderMinMax
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ordering Parameters (min-max)";
				}
				return "Параметры формирования заказов (min-max)";
			}
			return "Պատվերների ձևավորման պարամետրեր (min-max)";
		}
	}

	public static string BringExpensesToDayEnd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Move Sales, Returns and\r\nStorage outputs to the end of the day";
				}
				return "Документы Реализации, Возврата и \r\nСписание переместить в конец дня";
			}
			return "Վաճառքները, վերադարձները  և \r\nՊԵՕ-ները տեղափոխել օրվա վերջ";
		}
	}

	public static string BringIncomesToDayBegin
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Move entries documents to the beginning of the day";
				}
				return "Документы Поступления переместить в начало дня";
			}
			return "ՊՄՕ-ները տեղափոխել օրվա սկիզբ";
		}
	}

	public static string DiscountLogsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount calculated data";
				}
				return "Расчетные данные скидок";
			}
			return "Զեղչերի հաշվարկային տվյալներ";
		}
	}

	public static string DiscountPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount price";
				}
				return "Цена со скидкой";
			}
			return "Զեղչված գին";
		}
	}

	public static string ConfOrderMinMaxDelWithoutStoage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm removal of \"{0}\" product ordering parameters.";
				}
				return "Подтвердить удаление параметров формирования заказa для товара \"{0}\"";
			}
			return "Հաստատել \"{0}\" ապրանքի  պատվերի ձևավորման պարամետրերի հեռացնելը:";
		}
	}

	public static string ConfOrderMinMaxDelWithStoage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm removal of \"{0}\" product of \"{1}\" storage ordering parameters.";
				}
				return "Подтвердить удаление параметров формирования заказa для товара \"{0}\" и склада \"{1}\"";
			}
			return "Հաստատել \"{0}\" ապրանքի \"{1}\" պահեստի պատվերի ձևավորման պարամետրերի հեռացնելը:";
		}
	}

	public static string DiscountsMergeMethod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discounts merge method";
				}
				return "Метод объединения скидок";
			}
			return "Զեղչերի միավորման եղանակ";
		}
	}

	public static string MethodName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Method name";
				}
				return "Наименование метода";
			}
			return "Եղանակի անվանում";
		}
	}

	public static string MaterialCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Material caption";
				}
				return "Наименование товара";
			}
			return "Ապրանքի անվանում";
		}
	}

	public static string MaterialCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Material code";
				}
				return "Код товара";
			}
			return "Ապրանքի կոդ";
		}
	}

	public static string RowNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "N in the product list";
				}
				return "N в списке тов.";
			}
			return "Հ/Հ ապր. ցուցակում";
		}
	}

	public static string DiscountGroupCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount group name";
				}
				return "Наименование группы скидок";
			}
			return "Զեղչի խմբի անվանում";
		}
	}

	public static string UseTypeCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Application method name";
				}
				return "Наименование метода применения";
			}
			return "Կիրառման եղանակի անվանում";
		}
	}

	public static string SubDirectories
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Subdirectories";
				}
				return "Подчиненные справочники";
			}
			return "Ենթատեղեկատուներ";
		}
	}

	public static string PartnerCardCodeLenght
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Card code length";
				}
				return "Длина кода карты";
			}
			return "Քարտի կոդի երկարություն";
		}
	}

	public static string PartnerEmployeeBarcodePrefixButtonInfo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use delimiter symbol \"||\" to list more than one prefix in the field, e.g. 100||500||700.";
				}
				return "В поле для указания более одного префиксов используется символ разделения «||». Пример 100||500||700.";
			}
			return "Դաշտում մեկից ավելի նախդիրները թվարկելու համար օգտագործվում են «||» բաժանման նշանները, օր.` 100||500||700:";
		}
	}

	public static string UsePartnerCardCodeLenght
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Use card code length";
				}
				return "Учитывать длину кода карты";
			}
			return "Հաշվի առնել քարտի կոդի երկարությունը";
		}
	}

	public static string ByStorages
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "By storages";
				}
				return "По складам ...";
			}
			return "Ըստ պահեստների ...";
		}
	}

	public static string MultipleRowsEdited
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} lines have been edited";
				}
				return "Отредактировано {0} строк";
			}
			return "խմբագրվել է {0} տող";
		}
	}

	public static string NoStrageAvailable
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "None of the storages in specified lines are accessible for user {0}";
				}
				return "Склады в указанных строках не доступны пользователю {0}";
			}
			return "Նշված տողերից ոչ մեկի պահեստը {0} օգտագործողին հասանելի չէ";
		}
	}

	public static string OneTimeSaleRowQuantity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quantity of line in sales document";
				}
				return "Количество в строке документа продажи";
			}
			return "Միանվագ վաճառքի տողի քանակ";
		}
	}

	public static string DiscountCalcTestMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New discount calculation mechanism works in test mode.";
				}
				return "Новый механизм расчета скидок работает в тестовом режиме.";
			}
			return "Զեղչերի հաշվարկման նոր մեխանիզը գործում է տեստավորման ռեժիմում:";
		}
	}

	public static string DiscountsCalculation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discounts calculation";
				}
				return "Расчет скидок";
			}
			return "Զեղչերի հաշվարկ";
		}
	}

	public static string ShowDiscCalcData
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show discount calculations";
				}
				return "Показать расчеты скидок";
			}
			return "Ցույց տալ հաշվարկները";
		}
	}

	public static string DuplicatePrintShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Duplicate print";
				}
				return "послед. Чека ККМ";
			}
			return "կրկնօր. տպում";
		}
	}

	public static string ECRLastCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Last ECRC";
				}
				return "Печать копии";
			}
			return "Վերջին ՀԴՄԿ-ի";
		}
	}

	public static string CheckRet
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bill (Return)";
				}
				return "Чек (возврат)";
			}
			return "Կտրոն (վերադարձ)";
		}
	}

	public static string ECRCheckRet
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ECR chek (Return)";
				}
				return "Чек ККМ (возврат)";
			}
			return "ՀԴՄ կտրոն (վերադարձ)";
		}
	}

	public static string MTCheckRet
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales Bill (Return)";
				}
				return "Товарный чек (возврат)";
			}
			return "Ապրանքային կտրոն (վերադարձ)";
		}
	}

	public static string StorageExistenceInMTOrderMinMaxTable
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are ordering parameters for this storage";
				}
				return "Для данного склада указаны параметры формирования заказов";
			}
			return "Տվյալ պահեստով գոյություն ունեն պատվերների ձևավորման պարամետրեր";
		}
	}

	public static string ShowMessage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show message";
				}
				return "Вывести оповещение";
			}
			return "Ցուցադրել հաղորդագրություն";
		}
	}

	public static string Message
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Message text";
				}
				return "Текст сообщения";
			}
			return "Հաղորդագրության տեքստ";
		}
	}

	public static string WrongPeriod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Start date is greater than End date";
				}
				return "Дата начала периода действия не может быть больше даты конца периода";
			}
			return "Ժամկետի սկզբի ամսաթիվը մեծ է վերջի ամսաթվից";
		}
	}

	public static string ValidatePrefixContainSymbol
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prefix contains symbol \"|\" which is not allowed";
				}
				return "Префикс содержит недопустимый «|» символ";
			}
			return "Նախդիրը պարունակում է անթույլատրելի \"|\" սիմվոլ";
		}
	}

	public static string Prefixes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prefixes";
				}
				return "Префикс";
			}
			return "Նախդիրներ";
		}
	}

	public static string OneTimeSaleTotalQuantity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total quantity of sales document";
				}
				return "Итоговое количество в документе продаж ";
			}
			return "Միանվագ վաճառքի ընդհանուր քանակ";
		}
	}

	public static string AllowGroupDelete
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow batch deletion of documents";
				}
				return "Разрешить групповое удаление документов";
			}
			return "Թույլ տալ փաստաթղթերի խմբային հեռացում";
		}
	}

	public static string AllowPOSPrePaymentECRCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to register prepayment receipt and return ECRC";
				}
				return "Разрешить регистрацию ККМ  чека для получения и возврата аванса";
			}
			return "Թույլ տալ կանխավճարի ստացման և վերադարձի ՀԴՄԿ-ի գրանցում";
		}
	}

	public static string AllowPOSChooseBuyer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to choose buyer from the list";
				}
				return "Разрешить выбор покупателя из списка";
			}
			return "Թույլ տալ գնորդի ընտրությունը ցանկից";
		}
	}

	public static string POSFindBuyerLine2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "the buyer";
				}
				return "покупателя";
			}
			return "գնորդին";
		}
	}

	public static string TableCellValueIsEmpty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In the item list the value of \"{1}\" field of \"{0}\" row is empty";
				}
				return "Не заполнено значения поля \"{1}\" в строке \"{0}\" списка товаров ";
			}
			return "Անվանացուցակում  \"{0}\" տողի  \"{1}\" դաշտի արժեքը դատարկ է ";
		}
	}

	public static string PriceLowerCase
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "price";
				}
				return "цена";
			}
			return "գին";
		}
	}

	public static string ECRCodeNameWarning
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Attention! It is not recommended to modify line print settings (code, name) during system exploitation. Before editing, you need to evaluate the consequences.";
				}
				return "Внимание! Во время эксплуатации не рекомендуется изменять настройки печати строки (код, наименование). Перед редактированием необходимо оценить последствия.";
			}
			return "Ուշադրություն: Շահագործման ընթացքում խորհուրդ չի տրվում փոխել տողի տպելու դրույթները (կոդ, անվանում): Խմբագրելուց առաջ անհրաժեշտ է գնահատել հետևանքները:";
		}
	}

	public static string ForReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Refund";
				}
				return "К возврату";
			}
			return "Վերադարձման";
		}
	}

	public static string StorageWarning
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Attention! Enabling the checkbox will cause the system to strictly control the accounting of goods by retail prices. After registering the goods turnover, you won't be allowed to disable the checkbox. We strictly recommend to study the application of this accounting in advance.";
				}
				return "Внимание! При установке флага система будет жестко контролировать учета остатков товаров по розничным ценам. Флаг не разрешается снимать после регистрации движения. Рекомендуется изучить специфику применения данного учета заранее";
			}
			return "Ուշադրություն: Դրոշակը նշելիս համակարգը խստորեն վերահսկելու է ապրանքների մնացորդների հաշվառումը մանրածախ գներով: Ապրանքային շարժ գրանցելուց հետո դրոշակը չի թույլատրվում հանել։ Խորհուրդ է տրվում նախապես ուսումնասիրել տվյալ հաշվառման կիրառումը:";
		}
	}

	public static string RemoveFromSegment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Remove from segment";
				}
				return "Удалить из сегмента";
			}
			return "Հեռացնել սեգմենտից";
		}
	}

	public static string POSFindBuyerLine1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Find";
				}
				return "Поиск";
			}
			return "Փնտրել";
		}
	}

	public static string DiscountLogsExistsWithGivenDiscount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Lines are registered for this discount in \"Discount calculated data\" report";
				}
				return "Для данной скидки зарегистрированы строки в журнале \"Расчетные данные скидок\"";
			}
			return "Տվյալ զեղչով գրանցված են տողեր Զեղչերի հաշվարկային տվյալներում";
		}
	}

	public static string GetChange
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Get change";
				}
				return "Получить сдачу";
			}
			return "Ստանալ մանրը";
		}
	}

	public static string PrintPrePaymentRetECR
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print prepayment return ECRC ";
				}
				return "Распечатать чек ККМ для возврата аванса";
			}
			return "Տպել կանխավճարի վերադարձի ՀԴՄԿ";
		}
	}

	public static string FilECRNforPrePaymentReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Specify prepayment return ECR check";
				}
				return "Укажите номер авансового ККМ чека, подлежащего";
			}
			return "Նշել վերադարձվող կանխավճարի ՀԴՄ կտրոնի ";
		}
	}

	public static string number
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "number";
				}
				return "возврату";
			}
			return "համարը";
		}
	}

	public static string Ret
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "rtrn";
				}
				return "Возвр.";
			}
			return "վրդ.";
		}
	}

	public static string Return
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return";
				}
				return "Возвр.";
			}
			return "Վերադ.";
		}
	}

	public static string PrePayBase
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base";
				}
				return "N чека";
			}
			return "Հիմք";
		}
	}

	public static string PrePaymentECRN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment ECRC N";
				}
				return "N аванс. ККМ чека";
			}
			return "Կանխավճ. ՀԴՄ կտրոն N";
		}
	}

	public static string CheckN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "check N";
				}
				return "N основания";
			}
			return "կտրոնի N";
		}
	}

	public static string CurrentPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Current prices";
				}
				return "Текущие цены";
			}
			return "Գործող գներ";
		}
	}

	public static string NewPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New prices";
				}
				return "Новые цены";
			}
			return "Նոր գներ";
		}
	}

	public static string SavePricesWhenSaveGood
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "When saving a product card, new prices will be registered with the current date";
				}
				return "При сохранении карточки товара новые цены будут зарегистрированы текущей датой";
			}
			return "Ապրանքը գրանցելիս կգրանցվեն լրացված նոր գներ ընթացիկ ամսաթվով";
		}
	}

	public static string FixedDiscountAmount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fixed amount discount";
				}
				return "Скидка на фиксированную сумму";
			}
			return "Ֆիքսված գումարի զեղչ";
		}
	}

	public static string SyncContractsWithb2barmsoftDiscountServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus cards automatic synchronization";
				}
				return "Автоматическая синхронизация бонусных карт";
			}
			return "Բոնուսային քարտերի ավտոմատ համաժամանակեցում";
		}
	}

	public static string SyncDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Last synchronization date";
				}
				return "Дата последней синхронизации";
			}
			return "Վերջին սինխրոնիզացիայի ամսաթիվ";
		}
	}

	public static string DiscountServerCardsSynced
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data of the partners' bonus agreements were successfully updated";
				}
				return "Данные бонусных договоров партнеров были успешно обновлены";
			}
			return "Գործընկերների բոնուսային պայմանագրերի տվյալները բարեհաջող թարմացվեցին";
		}
	}

	public static string PriceDeletetionSuccess
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prices have been successfully removed";
				}
				return "Цены успешно удалены";
			}
			return "Գները բարեհաջող հեռացվել են";
		}
	}

	public static string PricesFormationSuccess
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prices have been successfully formed";
				}
				return "Цены успешно сформированы";
			}
			return "Գները բարեհաջող ձևավորվել են";
		}
	}

	public static string PrintECRAndProductCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print ECRC and sales bill";
				}
				return "Распечатать ККМ и товарный чек";
			}
			return "Տպել ՀԴՄԿ և ապրանքային կտրոն";
		}
	}

	public static string IsMultipleToConditions
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Apply more than once";
				}
				return "Применить более одного раза";
			}
			return "Կիրառել մեկից ավել անգամ";
		}
	}

	public static string Branch
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Branch";
				}
				return "Филиал";
			}
			return "Մասնաճյուղ";
		}
	}

	public static string Cashier
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cashier";
				}
				return "Кассир";
			}
			return "Գանձապահ";
		}
	}

	public static string StaticQR
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Static QR";
				}
				return "Статический QR";
			}
			return "Ստատիկ QR";
		}
	}

	public static string Exclude
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exclude";
				}
				return "Исключить";
			}
			return "Բացառել";
		}
	}

	public static string Include
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Include";
				}
				return "Включить";
			}
			return "Ներառել";
		}
	}

	public static string Application
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Application";
				}
				return "Действие";
			}
			return "Կիրառում";
		}
	}

	public static string Method
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Method";
				}
				return "Метод";
			}
			return "Եղանակ";
		}
	}

	public static string PhoneNumberForSMS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Phone number (PayX SMS)";
				}
				return "Номер телефона (PayX SMS)";
			}
			return "Հեռախոսահամար (PayX SMS)";
		}
	}

	public static string PhoneNumberNullText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "8 digits` XX123456";
				}
				return "8 цифр` XX123456";
			}
			return "8 նիշ` XX123456";
		}
	}

	public static string SMSSentText1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Payment SMS message was sent";
				}
				return "Ссылка на оплату по SMS была отправлена";
			}
			return "{0} հեռախոսահամարին ուղարկվել է";
		}
	}

	public static string SMSSentText2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "to the phone number {0}";
				}
				return "на номер {0}";
			}
			return "վճարում կատարելու SMS հաղորդագրությունը";
		}
	}

	public static string GiftWithLowestPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Gift (discount on the minimum price from the list of products for sale)";
				}
				return "Подарок (скидка на минимальную цену из списка товаров на продажу)";
			}
			return "Նվեր (վաճառվող ապրանքացանկից նվազագույն գնի չափով զեղչ)";
		}
	}

	public static string GiftQuantity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Quantity of gift";
				}
				return "Количество подарков";
			}
			return "Նվերի քանակ";
		}
	}

	public static string FillCustomerPhone
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill in the buyer's phone number";
				}
				return "Заполнить номер телефона покупателя";
			}
			return "Լրացնել գնորդի հեռախոսահամարը";
		}
	}

	public static string CreateType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Creation type";
				}
				return "Тип создания";
			}
			return "Ստեղծվելու տեսակ";
		}
	}

	public static string AdditionalDescription
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Additional specification";
				}
				return "Дополнительная характеристика";
			}
			return "Լրացուցիչ բնութագիր";
		}
	}

	public static string MTNameEn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Name (English)";
				}
				return "Наименование (английский)";
			}
			return "Անվանում (անգլերեն)";
		}
	}

	public static string MTNameRu
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Name (Russian)";
				}
				return "Наименование (русский)";
			}
			return "Անվանում (ռուսերեն)";
		}
	}

	public static string OnlineShop
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online store";
				}
				return "Онлайн-магазин";
			}
			return "Օնլայն խանութ";
		}
	}

	public static string Gift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Gift";
				}
				return "Подарок";
			}
			return "Նվեր";
		}
	}

	public static string DiscountByGiftSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(discount amount by product's price)";
				}
				return "(скидка по цене товара)";
			}
			return "(ապրանքի գնի չափով զեղչ)";
		}
	}

	public static string DiscountCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount name";
				}
				return "Название скидки";
			}
			return "Զեղչի անվանում";
		}
	}

	public static string SegmentNegationInformation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The discount applies to all products except those specified in the segment.";
				}
				return "Скидка распространяется на все товары, кроме указанных в сегменте.";
			}
			return "Զեղչը կիրառվում է բոլոր ապրանքների վրա, բացի նշված սեգմենտի ապրանքներից:";
		}
	}

	public static string SimpleModeSettingsInfo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Attention! When using coupon in simple mode some setting are ignored.";
				}
				return "Внимание! Во время использования чека в упрощенном режиме некоторые настройки игнорируются.";
			}
			return "Ուշադրություն: Կտրոնի օգտագործման պարզ ռեժիմի դեպքում որոշ կարգավորումներ անտեսվում են:";
		}
	}

	public static string DocDiscountPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount price of document";
				}
				return "Цена со скидкой указанная в документе";
			}
			return "Փաստաթղթում նշված զեղչված գին";
		}
	}

	public static string ShowAlsoZeroRows
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show also rows containing 0";
				}
				return "Показать также строки равные нулю";
			}
			return "Ցույց տալ նաև 0 պարունակող տողերը";
		}
	}

	public static string BySuppliers
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(by suppliers)";
				}
				return "(по поставщикам)";
			}
			return "(ըստ մատակարարների)";
		}
	}

	public static string GiftSet
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Gift set";
				}
				return "Комплект подарков";
			}
			return "Նվերների կոմպլեկտ";
		}
	}

	public static string SelectGift
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Choose one of the gifts";
				}
				return "Выбрать один из подарков";
			}
			return "Ընտրել նվերներից մեկը";
		}
	}

	public static string CardCurrCodeIsInvalid
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{1}\" currency of partner card with number \"{0}\" is not allowed";
				}
				return "Валюта \"{1}\"  карты партнера с номером \"{0}\" не разрешается";
			}
			return "Գործընկերոջ \"{0}\" համարով քարտի \"{1}\" արժույթն անթույլատրելի է";
		}
	}

	public static string EnableExport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Activate export";
				}
				return "Включить экспорт";
			}
			return "Միացնել արտահանումը";
		}
	}

	public static string ExportInterval
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export every";
				}
				return "Экспортировать каждые";
			}
			return "Արտահանել յուր.";
		}
	}

	public static string OnlineShopsProductsPublishing
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Uploading goods and balances of online store";
				}
				return "Экспорт товаров и остатков онлайн-магазина";
			}
			return "Օնլայն խանութի ապրանքների և մնացորդների հրապարակում";
		}
	}

	public static string RemainsStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balances storage";
				}
				return "Склад остатков";
			}
			return "Մնացորդների պահեստ";
		}
	}

	public static string PerMinute
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "minutes";
				}
				return "минут";
			}
			return "րոպեն մեկ";
		}
	}

	public static string PluggedDevice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connected device";
				}
				return "Подключенное устройство";
			}
			return "Միացված սարք";
		}
	}

	public static string POSTerminals
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "POS terminals";
				}
				return "POS-терминалы";
			}
			return "POS-տերմինալներ";
		}
	}

	public static string CreatedDocCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} document(s) was/were created";
				}
				return "Создан(о) {0} документ(ов)";
			}
			return "Ստեղծվեց {0} փաստաթուղթ";
		}
	}

	public static string SupplyAddress
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Delivery address";
				}
				return "Адрес доставки";
			}
			return "Մատակարարման հասցե";
		}
	}

	public static string APIKeysAndSecrets
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "API key and secrets ";
				}
				return "Доступы к API (ключи и секретные коды)";
			}
			return "API հասանելիություն (բանալիներ և գաղտնի կոդեր)";
		}
	}

	public static string PublishOnlineShopsRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Uploading goods and balances";
				}
				return "Экспорт товаров и остатков";
			}
			return "Ապրանքների և մնացորդների հրապարակում";
		}
	}

	public static string PublishSettings
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Upload settings";
				}
				return "Настройка экспорта";
			}
			return "Հրապարակման կարգավորումներ";
		}
	}

	public static string AcceptedDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Valid since";
				}
				return "Действует с";
			}
			return "Վավեր է սկսած";
		}
	}

	public static string AppID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "External application ID";
				}
				return "ID внешнего приложения";
			}
			return "Արտաքին ծրագրի ID";
		}
	}

	public static string AppKey
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "External application API key";
				}
				return "API ключ внешнего приложения";
			}
			return "Արտաքին ծրագրի API բանալի";
		}
	}

	public static string AppName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "External application name";
				}
				return "Наименования внешнего приложения";
			}
			return "Արտաքին ծրագրի անվանում";
		}
	}

	public static string AppSecret
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "API secret";
				}
				return "API секретный код";
			}
			return "API գաղտնի կոդ";
		}
	}

	public static string ExpiredDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Valid before";
				}
				return "Годен до";
			}
			return "Վավեր է մինչև";
		}
	}

	public static string GenerateNewAppSecret
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Generate a new secret code";
				}
				return "Сгенерировать секретный код";
			}
			return "Գեներ. նոր գաղտնի կոդ";
		}
	}

	public static string MessageForAPIConnection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "To connect to the API, you need to send the following App ID and Secret Code to the external application.";
				}
				return "Для подключения к API внешнему приложению необходимо отправить нижеследующий идентификатор (App ID) и секретный код (Secret Key). ";
			}
			return "API-ին միանալու համար անհրաժեշտ է արտաքին ծրագրին ուղարկել ստորև նշված նույնացուցիչը (App ID) և գաղտնի կոդը (Secret Key):";
		}
	}

	public static string ProductRead
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Permission to read the balances";
				}
				return "Разрешить обращения к товарам и остаткам";
			}
			return "Մնացորդները կարդալու իրավասություն";
		}
	}

	public static string ExportRealRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export real balances";
				}
				return "Экспортировать реальные остатки";
			}
			return "Արտահանել իրական մնացորդները";
		}
	}

	public static string MessageForSyncSettings
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For synchronization You should give the appropriate settings in Options.";
				}
				return "Для экспорта товаров и остатков необходимо указать соответствующие настройки.";
			}
			return "Համաժամանակեցման համար անհրաժեշտ է դրույթներում տալ համապատասխան կարգավորումները:";
		}
	}

	public static string NotDeclaredPricesCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Number of goods which do not have specified prices: {0}";
				}
				return "Количество товаров для которых не указаны цены продаж: {0}";
			}
			return "Չհայտարարված գներով ապրանքների քանակը ` {0}";
		}
	}

	public static string PublishedProductsCount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Uploaded goods quantity - {0}";
				}
				return "Количество экспортированных товаров: {0}";
			}
			return "Հրապարակված ապրանքների քանակը ` {0}";
		}
	}

	public static string RowsNotFoundForSynhronization
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No product / service lines were found for synchronization.";
				}
				return "Товары для синхронизации не обнаружены.  ";
			}
			return "Համաժամանակեցման ապրանքների տողեր չեն հայտնաբերվել:";
		}
	}

	public static string OnlineShopServer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online store server";
				}
				return "Сервер онлайн-магазина";
			}
			return "Օնլայն խանութի սերվեր";
		}
	}

	public static string OnlineShopRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online store balances";
				}
				return "Остатки онлайн-магазина";
			}
			return "Օնլայն խանութի մնացորդներ";
		}
	}

	public static string WasNotExported
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Not exported";
				}
				return "Не экспортированы";
			}
			return "Չի արտահանվել";
		}
	}

	public static string Exported
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Exported";
				}
				return "Экспортированы";
			}
			return "Արտահանվել է";
		}
	}

	public static string ExportResult
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export result";
				}
				return "Результат экспорта";
			}
			return "Արտահանման արդյունք";
		}
	}

	public static string ExportResults
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export results";
				}
				return "Результаты экспорта";
			}
			return "Արտահանման արդյունքներ";
		}
	}

	public static string BonusCardIsCanceled
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The bonus card has been canceled. You should choose an active bonus card to calculate the bonus.";
				}
				return "Бонусная карта аннулирована. Для расчета бонуса нужно указать активную бонусную карту. ";
			}
			return "Բոնուսային քարտը կասեցված է: Բոնուս հաշվարկելու համար անհրաժեշտ է նշել ակտիվ բոնուսային քարտ:";
		}
	}

	public static string CashSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Cash";
				}
				return "Наличные";
			}
			return "Առձեռն գումար";
		}
	}

	public static string CPACode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "CPA code";
				}
				return "ВЭД код";
			}
			return "ԱՏԳ կոդ";
		}
	}

	public static string ECRetCheckCrn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ECR registration number of Return check";
				}
				return "Регистрационный номер ККМ возвр. чека";
			}
			return "Վերադարձ. կտրոնի ՀԴՄ-ի գրանցման համար";
		}
	}

	public static string ECRetCheckNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return check number";
				}
				return "Номер возвр. чека";
			}
			return "Վերադարձ. կտրոնի համար";
		}
	}

	public static string JSON
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "JSON";
				}
				return "JSON";
			}
			return "JSON";
		}
	}

	public static string PartSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Partial amount";
				}
				return "Частичная сумма";
			}
			return "Մասնակի գումար";
		}
	}

	public static string ProportionallyDiscount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Proportional discount";
				}
				return "Пропорциональная скидка";
			}
			return "Համամասնորեն զեղչ";
		}
	}

	public static string RegistrDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Registration date and time";
				}
				return "Дата и время регистрации";
			}
			return "Գրանցման ա/թ և ժամ";
		}
	}

	public static string RowID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Lines ID";
				}
				return "ID линий";
			}
			return "Տողերի ID";
		}
	}

	public static string SectionNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Dep. ID";
				}
				return "Номер раздела";
			}
			return "Բաժնի համար";
		}
	}

	public static string SectionVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "VAT of dep.";
				}
				return "НДС раздела";
			}
			return "Բաժնի ԱԱՀ";
		}
	}

	public static string SectionVatInv
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Dep. taxation mode";
				}
				return "Режим налогообложения раздела";
			}
			return "Բաժնի հարկման ռեժիմ";
		}
	}

	public static string TotalWithoutVat
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total amount without VAT";
				}
				return "Общая сумма без НДС";
			}
			return "Ընդամենը գումար առանց ԱԱՀ";
		}
	}

	public static string TotalWithVat
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total amount with VAT";
				}
				return "Общая сумма с НДС";
			}
			return "Ընդամենը գումար ԱԱՀ-ով";
		}
	}

	public static string UsedPrePayment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Used prepayment";
				}
				return "Использованный аванс";
			}
			return "Օգտագործված կանխավճար";
		}
	}

	public static string ViewECRCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "View ECR check";
				}
				return "Посмотреть кассовый чек";
			}
			return "Դիտել ՀԴՄ կտրոնը";
		}
	}

	public static string APIAccess
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "API  accessibility";
				}
				return "API доступ";
			}
			return "API  հասանելիության";
		}
	}

	public static string DenyAccess
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Do not provide";
				}
				return "Не предоставлять";
			}
			return "Չտրամադրել";
		}
	}

	public static string EmptyExtCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "External code is empty";
				}
				return "Внешний код пуст";
			}
			return "Արտաքին կոդը դատարկ է ";
		}
	}

	public static string EmptyPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prices are not set";
				}
				return "Цены не установлены";
			}
			return "Չհայտարարված գներ";
		}
	}

	public static string FullAccess
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Provide real balances";
				}
				return "Предоставлять реальные остатки";
			}
			return "Տրամադրել իրական մնացորդները";
		}
	}

	public static string LimitedAccess
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Provide with \"in stock\"/\"out of stock\" variations";
				}
				return "Предоставлять с вариантом есть в наличии/нет в наличии";
			}
			return "Տրամադրել առկա է/առկա չէ տարբերակով";
		}
	}

	public static string NegativeQnt
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Negative quantity";
				}
				return "Недостаточное количество";
			}
			return "Բացասական քանակ";
		}
	}

	public static string ZeroQnt
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Zero quantity";
				}
				return "Нулевое количество";
			}
			return "Զրոյական քանակ";
		}
	}

	public static string EnableExportChgInfo
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Goods balances automate uploading options change will be affected after next program run!";
				}
				return "Чтобы изменение настроек автоматического экспорта остатков товаров вступило в силу, необходимо перезапустить программу";
			}
			return "Ապրանքների մնացորդների ավտոմատ հրապարակման փոփոխությունը կկատարվի համակարգի հաջորդ աշխատեցնելու ժամանակ:";
		}
	}

	public static string InvalidCharacters
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Invalid characters: {0}";
				}
				return "Недопустимые символы {0}";
			}
			return "Անթույլատրելի սիմվոլներն են ` {0}";
		}
	}

	public static string PointOfSale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Point of sale";
				}
				return "Точка продаж ";
			}
			return "Վաճառակետ";
		}
	}

	public static string BonusCardIsNotValid
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The bonus card is not valid (sale date is out of validation period).";
				}
				return "Бонусная карта недействительна (дата продажи вне периода действия).";
			}
			return "Բոնուսային քարտը վավեր չէ (վաճառքի ամսաթիվը դուրս է վավերականության ժամանակահատվածից)։";
		}
	}

	public static string MTEditQuick
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Edit without check (fast version)";
				}
				return "Редактирование без проверок (быстрая версия)";
			}
			return "Խմբագրում առանց ստուգումների (արագ տարբերակ)";
		}
	}

	public static string BonusCardIsNotValidForBSUser
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The bonus card is not valid  (beginning of the card validation period is not specified or sale date is out of validation period).";
				}
				return "Бонусная карта недействительна (начало срока действия карты не указано или дата продажи вне периода действия).";
			}
			return "Բոնուսային քարտը վավեր չէ (քարտի վավերականության ժամկետի սկիզբը նշված չէ կամ վաճառքի ամսաթիվը դուրս է վավերականության ժամանակահատվածից)։";
		}
	}

	public static string QRImageContains1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Image contains";
				}
				return "Изображение содержит";
			}
			return "Պատկերը պարունակում է";
		}
	}

	public static string QRImageContains2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "external application ID and secret";
				}
				return "ID внешнего приложения и секретный код";
			}
			return "արտաքին ծրագրի ID և գաղտնի կոդ";
		}
	}

	public static string ScanQRForMobileApp1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Scan the QR code";
				}
				return "Отсканируйте QR-код";
			}
			return "Սկանավորեք QR կոդը մոբայլ հավելվածի";
		}
	}

	public static string ScanQRForMobileApp2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "to connect to the mobile app";
				}
				return "для подключения к мобильному приложению";
			}
			return "հետ կապ հաստատելու համար";
		}
	}

	public static string UseBuyerAddOrEditQuickMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show mane fields when adding or editing a buyer";
				}
				return "Показывать основные поля при добавлении или редактировании покупателя";
			}
			return "Գնորդ ավելացնելուց կամ խմբագրելուց ցուցադրել հիմնական դաշտերը";
		}
	}

	public static string CalcNewRemsFromDB
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Calculation of product balances from database , result ({0})";
				}
				return "Расчет остатков продуктов из базы данных, результат ({0})";
			}
			return "Ապրանքների մնացորդների հաշվարկ տվյալների պահոցից, արդյունք({0})";
		}
	}

	public static string LastProductsRemsStateComparison
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Comparison with goods sent as a result of recent synchronization, result ({0})";
				}
				return "Сравнение с товарами, отправленными в результате последней синхронизации, результат ({0})";
			}
			return "Վերջին համաժամանակեցման արդյունքում ուղարկված ապրանքների հետ համեմատում, արդյունք({0})";
		}
	}

	public static string LastProductsRemsSynchronizationStateCalled
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Call of products sent as a result of the last synchronization, result ({0})";
				}
				return "Вызов товаров, отправленных в результате последней синхронизации, результат ({0})";
			}
			return "Վերջին համաժամանակեցման արդյունքում ուղարկված ապրանքների կանչ, արդյունք({0})";
		}
	}

	public static string OnlineShopMakingHiddens
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Hidden products editing in the cloud, result ({0})";
				}
				return "Редактирование hidden продуктов в облаке, результат ({0})";
			}
			return "Hidden ապրանքների խմբագրում ամպում, արդյունք({0})";
		}
	}

	public static string OnlineShopSyncEnd
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "End of products balances synchronization";
				}
				return " Конец синхронизации остатков продуктов";
			}
			return "Ապրանքների մնացորդների համաժամանակեցման ավարտ";
		}
	}

	public static string OnlineShopSyncStart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Start of products balances synchronization";
				}
				return "Начало синхронизации остатков продуктов";
			}
			return "Ապրանքների մնացորդների համաժամանակեցման սկիզբ";
		}
	}

	public static string OnlineShopTimerActivity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Timer activity";
				}
				return "Активность Timer-а";
			}
			return "Timer-ի ակտիվություն";
		}
	}

	public static string SyncronizedProductsSendingToCloud
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transfer of synchronized products balances to the cloud, result ({0})";
				}
				return "Перенос синхронизированных остатков продуктов в облако, результат ({0})";
			}
			return "Համաժամանակեցված ապրանքների մնացորդների տեղափոխում դեպի ամպ, արդյունք({0})";
		}
	}

	public static string OnlineShopUserActivity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "User activity";
				}
				return "Активность пользователя";
			}
			return "Օգտագործողի ակտիվություն";
		}
	}

	public static string OnlineShopProductsFiltering
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Filtering of product balances, result ({0})";
				}
				return "Фильтрация остатков продуктов, результат ({0})";
			}
			return "Ապրանքների մնացորդների ֆիլտրում, արդյունք({0})";
		}
	}

	public static string AllowToRegisterNewBuyer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to add new buyer";
				}
				return "Разрешить добавление нового покупателя";
			}
			return "Թույլ տալ նոր գնորդի ավելացում";
		}
	}

	public static string NewBuyer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "New buyer";
				}
				return "Новый покупатель";
			}
			return "Նոր գնորդ";
		}
	}

	public static string OneOfTheFieldsMustBeFilledIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "One of these fields must be filled in: buyer's name, phone number or email";
				}
				return "Заполните имя покупателя, номер телефона или эл. почту";
			}
			return "Լրացրեք գնորդի անվանում, հեռախոսահամար կամ էլ. փոստ դաշտերից մեկը";
		}
	}

	public static string PartnerWithCurrentPhoneNumberExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is already a buyer with the mentioned phone number";
				}
				return "Покупатель с указанным номером телефона уже существует";
			}
			return "Նշված հեռախոսահամարով գնորդ արդեն գոյություն ունի";
		}
	}

	public static string PrintDoc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print {0} document";
				}
				return "Распечатать документ {0}";
			}
			return "Տպել {0} փաստաթուղթը";
		}
	}

	public static string PrintTemplateIsMissing
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print template for {1} document is not selected in options for {0} user.";
				}
				return "Печатная форма для документа {1} не указана в настройках пользователя {0}.";
			}
			return "{0} օգտագործողի համար {1} փաստաթղթի տպելու ձևանմուշը  դրույթներում նշված չէ։";
		}
	}

	public static string FillLatestCostPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill last cost price";
				}
				return "Заполнить по последней себестоимости";
			}
			return "Լրացնել վերջին ինքնարժեքով";
		}
	}

	public static string ShiftAlreadyClosed
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "This shift is closed. You should reload the sales window.";
				}
				return "Смена закрыта. Необходимо перезагрузить окно продаж.";
			}
			return "Տվյալ հերթափոխը փակված է։ Անհրաժեշտ է վերաբեռնել վաճառքի պատուհանը։";
		}
	}

	public static string FieldValueMustBeLessThanParameter
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The value of \"{0}\" field must be less than {1}";
				}
				return "Значение поля \"{0}\" должно быть меньше {0} символов";
			}
			return "\"{0}\" դաշտի արժեքը պետք է փոքր լինի {1} -ից";
		}
	}

	public static string FieldMustBeNumeric
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The value of \"{0}\" field must be numeric";
				}
				return "Значение поля \"{0}\" должно быть числовым";
			}
			return "\"{0}\" դաշտի արժեքը պետք է լինի թվային";
		}
	}

	public static string AllowToAddNewBuyerWithNotRegisteredBuyerCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to add a new buyer when entering an unregistered card";
				}
				return "Разрешить добавление нового покупателя при вводе незарегистрированной карты";
			}
			return "Թույլ տալ չգրանցված քարտ մուտքագրելիս նոր գնորդի ավելացում";
		}
	}

	public static string AcceptToAddNewBuyerWithCardCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm to add a new buyer (card code {0})";
				}
				return "Подтвердить добавление нового покупателя  (код карты {0})";
			}
			return "Հաստատել նոր գնորդի ավելացումը (քարտի կոդ {0})";
		}
	}

	public static string BonusAddShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add. to bonus";
				}
				return "Зачисл. на бонус";
			}
			return "Բոն. ավելացում";
		}
	}

	public static string POS_N
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "POS-term.";
				}
				return "POS-терм.";
			}
			return "POS-տերմ.";
		}
	}

	public static string ShowGoodsListWithPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show product list with prices";
				}
				return "Показать список продуктов с ценами";
			}
			return "Ցույց տալ գին սյունը ապրանքների ընտրման ցուցակներում";
		}
	}

	public static string AccRemNotSameCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document Revise partner debt (group) can only be created when partners with the same currency contract are selected";
				}
				return "Документ Корректировка долга партнера (групповая) можно создать только в том случае, если указаны партнеры с контрактом в одной и той же валюте";
			}
			return "Գործընկերոջ պարտքի վերանայում (Խմբային) փաստաթուղթը հնարավոր է ստեղծել միայն նույն արժույթով պայմանագիր ունեցող գործընկերներ նշված լինելու դեպքում";
		}
	}

	public static string ShowCashDeskAmount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Display cash balance";
				}
				return "Показать наличные деньги в кассе";
			}
			return "Ցույց տալ դրամարկղում առկա կանխիկ գումարը";
		}
	}

	public static string FiscalIncome
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fiscal revenue";
				}
				return "Фискальный доход";
			}
			return "Ֆիսկալ հասույթ";
		}
	}

	public static string FiscalIncomeVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fiscal revenue VAT";
				}
				return "НДС на фискальный доход";
			}
			return "Ֆիսկալ հասույթի ԱԱՀ";
		}
	}

	public static string TotalReturn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total returns";
				}
				return "Общий возврат";
			}
			return "Ընդամենը վերադարձ";
		}
	}

	public static string TotalReturnVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total ret. VAT";
				}
				return "НДС общего возврата";
			}
			return "Ընդամենը վեր. ԱԱՀ";
		}
	}

	public static string TotalSale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total sales";
				}
				return "Общая продажа";
			}
			return "Ընդամենը վաճառք";
		}
	}

	public static string TotalSaleVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total sales VAT";
				}
				return "НДС общей продажи";
			}
			return "Ընդամենը վաճ. ԱԱՀ";
		}
	}

	public static string WithUsingPrePaymentSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Incl. the use of prep.";
				}
				return "В том чис. с испол. пред.";
			}
			return "Այդ թվ. կանխավճ․ oգտ․";
		}
	}

	public static string PrePaymentReturnShort
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment ret.";
				}
				return "Возв. предоплаты";
			}
			return "Կանխավճարի վեր․";
		}
	}

	public static string SummMustBeGreater
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The total amount of non-cash, credit and bonus payments must be less than the total amount";
				}
				return "Общая сумма безналичных, кредитных и бонусных выплат должна быть меньше общей суммы";
			}
			return "Անկանխիկ, ապառիկ և բոնուսով մարումների հանրագումարը պետք է փոքր լինի ընդամենը գումարից";
		}
	}

	public static string GoodsOperationJournalByParty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products operations by batches";
				}
				return "Операции товаров по партиям";
			}
			return "Ապրանքների գործողություններ ըստ խմբաքանակի";
		}
	}

	public static string PrePaymentSymbol
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "(P)";
				}
				return "(П)";
			}
			return "(Կ)";
		}
	}

	public static string BirthDayDiscount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "On the occasion of a birthday";
				}
				return "По случаю дня рождения";
			}
			return "Ծննդյան օրվա առթիվ";
		}
	}

	public static string AfterBDay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Apply after birthday (day)";
				}
				return "Применить после дня рождения (день)";
			}
			return "Կիրառել ծնունդից հետո (օր)";
		}
	}

	public static string BeforBDay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Apply before birthday (day)";
				}
				return "Применить до дня рождения (день)";
			}
			return "Կիրառել մինչև ծնունդը (օր)";
		}
	}

	public static string CorrStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Corresponding storage";
				}
				return "Корреспондирующий склад";
			}
			return "Թղթակցող պահեստ";
		}
	}

	public static string ParentCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Parent name";
				}
				return "Наименование родителя";
			}
			return "Կուտակիչի անվանում";
		}
	}

	public static string SalePriceRetail
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sale price (Retail)";
				}
				return "Цена продажи (Розничная)";
			}
			return "Վաճառքի գին(Մանրածախ)";
		}
	}

	public static string CorrStorageCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Corresponding storage name";
				}
				return "Наименование корреспондирующего склада";
			}
			return "Թղթակցող պահեստի անվանում";
		}
	}

	public static string NotTheSameTypeOfStorages
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The warehouses of these lines must be of the same type.";
				}
				return "Склады этих строк должны быть одного типа.";
			}
			return "Նշված տողերի պահեստները պետք է լինեն նույն տիպի";
		}
	}

	public static string WrongMinLengthOfBarCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Barcode length must be at least {0}  characters. {1}";
				}
				return "Длина штрих-кода должна быть не менее {0} символов. {1}";
			}
			return "Գծիկավոր կոդի երկարությունը պետք է լինի նվազագույնը {0} նիշ : {1}";
		}
	}

	public static string ShowDocumentsJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Open the documents journal";
				}
				return "Открыть журнал документов";
			}
			return "Բացել փաստաթղթերի մատյանը";
		}
	}

	public static string ExistChildDocByBaseISN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "A document was created based on the selected document ({0})";
				}
				return "На основе выбранного документа создан документ ({0})";
			}
			return "Ընտրված փաստաթղթի հիման վրա ստեղծված է փաստաթուղթ ({0})";
		}
	}

	public static string GoodsCacheRefreshByTimer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Autoupdate selection lists of goods every {0} minutes";
				}
				return "Обновлять автоматически списки подбора товаров каждые {0} минут";
			}
			return "Ավտոմատ թարմացնել ապրանքների ընտրման ցուցակները յուրաքանչյուր {0} րոպեն մեկ";
		}
	}

	public static string WithoutParty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Without batches";
				}
				return "Без партий";
			}
			return "Առանց խմբաքանակների";
		}
	}

	public static string RemainsStorages
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Balances storages";
				}
				return "Склады остатков";
			}
			return "Մնացորդների պահեստներ";
		}
	}

	public static string FEFODistibuteLong
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Distribute the quantity by FEFO";
				}
				return "Распределить количество по Срокам годности";
			}
			return "Քանակը բաշխել ըստ Օգտագործման ժամկետի";
		}
	}

	public static string AdminToolsText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Management of databases, backups, users and their  permissions";
				}
				return "Управление базами данных, резервными копиями, пользователями и правами";
			}
			return "Տվյալների պահոցների, պահուստային պատճենների և օգտագործողների իրավասությունների ղեկավարում";
		}
	}

	public static string AnalysisText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Analytical graphs and charts, ABC-analysis, best selling products";
				}
				return "Аналитические графики и диаграммы, ABC-анализ, самые продаваемые товары";
			}
			return "Վերլուծական գրաֆիկներ և դիագրամներ, ABC վերլուծություն, լավագույն վաճառվող ապրանքներ";
		}
	}

	public static string B2BText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Informative services and el. documents exchange platform";
				}
				return "Информационные сервисы и платформа для обмена эл. документами";
			}
			return "Տեղեկատվական ծառայություններ և էլ. փաստաթղթերի փոխանակման հարթակ";
		}
	}

	public static string CashBankText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Accounting of cash and non-cash funds, data exchange with AS-Internet Bank";
				}
				return "Учет наличных и безналичных денежных средств, обмен данными с системой AS-InternetBank";
			}
			return "Դրամական միջոցների մուտքագրում և ելքագրում, տվյալների փոխանակում ՀԾ-ԻնտերնետԲանկի հետ";
		}
	}

	public static string PointOfSaleText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Point of sale for quick customer service, work with shifts";
				}
				return "Рабочее место кассира для быстрого обслуживания клиентов, работа со сменами";
			}
			return "Հաճախորդների արագ սպասարկման պատուհան, հերթափոխերի հետ աշխատանք";
		}
	}

	public static string PurchasesText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Procurement and return of goods, cost accounting, settlements with vendors";
				}
				return "Закупка и возврат товаров, учет себестоимости, взаиморасчеты с поставщиками";
			}
			return "Ապրանքների ձեռքբերում և վերադարձ, ինքնարժեքի ձևավորում, մատակարարների հետ փոխհաշվարկներ";
		}
	}

	public static string SaleText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wholesale and retail sales and returns, calculation of discounts and bonuses, settlements with clients";
				}
				return "Оптовая и розничная продажа и возврат, расчет скидок и бонусов, взаиморасчеты с покупателями";
			}
			return "Մանրամեծածախ վաճառք և վերադարձ, զեղչերի և բոնուսների հաշվարկ, գնորդների հետ փոխհաշվարկներ";
		}
	}

	public static string StorageText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Movement, stocktaking and write off of goods, kitting";
				}
				return "Перемещение, инвентаризация и списание товаров, сборка комплектов";
			}
			return "Ապրանքների տեղաշարժ, գույքագրում և դուրս գրում, հավաքածուների կոմպլեկտավորում";
		}
	}

	public static string EInvoicingText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Tax invoices export, import and online view";
				}
				return "Экспорт, импорт и онлайн просмотр эл. налоговых документов";
			}
			return "Հարկային փաստաթղթերի արտահանում, ներմուծում և օնլայն դիտում";
		}
	}

	public static string MTBillReturnIntegrity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return (Bill) documents, where there are discounts, but that were not reflected in the Sales analysis";
				}
				return "Документы Возврата (Чек), в которых указан процент скидки, но сумма скидки не была отражена в Анализе продаж.";
			}
			return "Վերադարձ (Կտրոն) փաստաթղթեր, որոնցում նշված է զեղչի տոկոս, բայց Վաճառքների վերլուծությունում այն չի արտացոլվել";
		}
	}

	public static string ActivateLinksForCodeColumn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enable links for product Code";
				}
				return "Включить ссылки для колонки Код товара";
			}
			return "Կոդ սյան համար ակտիվացնել հղումները";
		}
	}

	public static string ActivateLinksForCodeColumns
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Enable links for product and partner Code";
				}
				return "Включить ссылки для колонок Код товара и партнера";
			}
			return "Կոդ սյուների համար ակտիվացնել հղումները";
		}
	}

	public static string ProfitPercentCurByCost
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Profit (%) by cost in {0}";
				}
				return "Прибыль от стоимости (%) в {0}";
			}
			return "Շահույթը ինքնարժեք. (%) {0}-ով";
		}
	}

	public static string ProfitPercentCurBySale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Profit (%) by sale in {0}";
				}
				return "Прибыль от продажи (%) в {0}";
			}
			return "Շահույթը վաճառք. (%) {0}-ով";
		}
	}

	public static string GiftOnlyForMTBill
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You could define discount type \"{0}\" only for Sale (Bill)";
				}
				return "Скидка типа \"{0}\" может назначаться только для документа Продажа (Чек)";
			}
			return "\"{0}\" տեսակի զեղչ կարելի է սահմանել միայն Վաճառք (Կտրոն) փաստաթղթի համար";
		}
	}

	public static string DiscountAccumulativeCondition
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"Accumulated sales amount\" condition is valid only if the partner is mentioned";
				}
				return "Условие Сумма накопленных продаж действует только если выбран партнер";
			}
			return "\"Կուտակված վաճառքների գումար\" պայմանը գործում է միայն գործընկեր նշելու դեպքում";
		}
	}

	public static string ActualQuantitiesCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check the actual quantities";
				}
				return "Проверка фактического количества";
			}
			return "Փաստացի քանակների ստուգում";
		}
	}

	public static string ShowOnlyVariations
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show only variations";
				}
				return "Отображать только расхождения";
			}
			return "Ցույց տալ միայն շեղումները";
		}
	}

	public static string RowIsInDocument
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The row cannot be removed, it is in the document";
				}
				return "Строку удалить нельзя, т.к. она присутствует в документе.";
			}
			return "Տողը չի կարելի հեռացնել, այն առկա է փաստաթղթում";
		}
	}

	public static string QuantitiesCheckCloseConfirmation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Complete the inventory without moving the variations to a document?";
				}
				return "Завершить проверку без переноса расхождений в документ?";
			}
			return "Ավարտե՞լ գույքագրումը առանց շեղումները փաստաթուղթ տեղափոխելու";
		}
	}

	public static string CorrectVariations
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discrepancies in the document and verified quantities were found.\r\nCorrect deviations?";
				}
				return "Обнаружены расхождения между количеством в документе и результатом проверки.\r\nПринять расхождения?";
			}
			return "Հայտնաբերվել Է փաստաթղթի և ստուգված քանակների անհամապատասխանություն:\r\nԿատարե՞լ շեղումների ճշգրտում";
		}
	}

	public static string AQCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check Qty";
				}
				return "Пров. количество";
			}
			return "Քան. ստուգում";
		}
	}

	public static string ErrDifferentDocTypeNotAllowedForDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Grouped {0} document can only be created on the basis of \"{1}\" documents.";
				}
				return "Сводный документ \"{0}\" можно создать только на основе документов {1}:";
			}
			return "\"{0}\" փաստաթուղթը խմբային եղանակով կարող է ստեղծվել միայն \"{1}\" փաստաթղթերի հիման վրա:";
		}
	}

	public static string ErrDifferentContractTypeNotAllowedForDocument
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" document cannot be created on the basis of lines with different contracts";
				}
				return "Документ \"{0}\" невозможно создать на основе строк с разными договорами";
			}
			return "\"{0}\" փաստաթուղթը չի կարող ստեղծվել տարբեր պայմանագրեր ունեցող տողերի հիման վրա";
		}
	}

	public static string ErrDraftStateDocNotAllowedForMTIncomeCashOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "CashOut document cannot be created on the basis of draft documents.";
				}
				return "Документ РКО невозможно создать на основе черновых документы";
			}
			return "ԴԵՕ փաստաթուղթ չի կարող ստեղծվել սևագիր փաստաթղթերի հիման վրա";
		}
	}

	public static string PartContractsCacheRefreshByTimer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Automatically update lists of partner and contracts every {0} minute";
				}
				return "Обновлять автоматически списки подбора партнеров и договоров каждые {0} минут";
			}
			return "Ավտոմատ թարմացնել գործընկերների և պայմանագրերի ընտրման ցուցակները յուրաքանչյուր {0} րոպեն մեկ";
		}
	}

	public static string RefreshLsits
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and partners lists";
				}
				return "Список товаров и партнеров";
			}
			return "Ապրանքների և գործ. ցուցակներ";
		}
	}

	public static string RefreshMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "products list";
				}
				return "список товаров";
			}
			return "ապրան. ցուցակը";
		}
	}

	public static string RefreshPartners
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "partners list";
				}
				return "список партнеров";
			}
			return "գործընկ. ցուցակը";
		}
	}

	public static string VariancePercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Dev. %";
				}
				return "Откл. %";
			}
			return "Շեղմ. %";
		}
	}

	public static string ShowLastPurchasePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show \"Last Purchase Price\" column";
				}
				return "Показывать колонку \"Цена последней закупки\"";
			}
			return "Ցույց տալ \"Վերջին ձեռքբերման գին\" սյունը";
		}
	}

	public static string DirectoriesLog
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "History of directories";
				}
				return "История справочников";
			}
			return "Տեղեկատուների պատմություն";
		}
	}

	public static string Directory
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Types of directories";
				}
				return "Типы справочников";
			}
			return "Տեղեկատուների տեսակներ";
		}
	}

	public static string RetailAccounting
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products' accounting by retail prices in storage ";
				}
				return "Учет остатков товаров по розничным ценам";
			}
			return "Պահեստում ապրանքների մնացորդների հաշվառում մանրածախ գներով";
		}
	}

	public static string RetailAccountingDeactivateErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The flag \"Accounting of products by retail prices\" cannot be deactivated, because there is accounting data by retail prices.";
				}
				return "Флажок \"Учет остатков товаров по розничным ценам\" невозможно деактивировать, так как в БД используется розничный учет.";
			}
			return "\"Պահեստում ապրանքների մնացորդների հաշվառում մանրածախ գներով\" դրոշակը հնարավոր չէ ապաակտիվացնել, քանի որ պահոցում կիրառվում է մանրածախ հաշվառում:";
		}
	}

	public static string CashOutOpTypeByPlCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Return to the plastic card (Print ECR receipt - return of non-cash prepayment)";
				}
				return "Возврат на пластиковую карту (печать чека ККМ на безнал. аванс)";
			}
			return "Պլաստիկ քարտին վերադարձ (անկանխ. կանխ. ՀԴՄ կտրոնի տպում)";
		}
	}

	public static string AllowPrintMTCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to print Sales Bill";
				}
				return "Разрешить напечатать товарный чек";
			}
			return "Թույլ տալ տպել ապրանքային կտրոն";
		}
	}

	public static string UpdateCurrentRowLatestPurchasePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Update the last purchase price of the current row";
				}
				return "Обновить цену последней закупки для текущей строки";
			}
			return "Թարմացնել ընթացիկ տողի վերջին ձեռքբերման գինը";
		}
	}

	public static string UpdateLatestPurchasePrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Update last purchase prices";
				}
				return "Обновить цену последней закупки";
			}
			return "Թարմացնել վերջին ձեռքբերման գները";
		}
	}

	public static string BarCodeNameAndCodeInputNullText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input barcode and press Enter or product code and Code (F8) button or product name and Name (F9) button";
				}
				return "Введите штрих код и нажмите на Enter, или код товара и нажмите (F8), или наименование товара и нажмите (F9)";
			}
			return "Մուտքագրեք գծ․ կոդը և սեղմեք Enter կամ ապրանքի կոդը և Կոդ (F8) կոճակը կամ ապր. անվանումը և Անվ. (F9) կոճակը";
		}
	}

	public static string DiscountDaysHours
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount days and hours";
				}
				return "Дни и часы скидок";
			}
			return "Զեղչի օրեր և ժամեր";
		}
	}

	public static string WrongTimePeriod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Beginning time is greater than final time, day {0}";
				}
				return "Время начала больше, чем время окончания, день {0}";
			}
			return "Սկզբի ժամը մեծ է ավարտի ժամից, օր` {0}";
		}
	}

	public static string ConditionOnlyForMTBill
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You could define discount condition \"{0}\" only for Sale (Bill)";
				}
				return "Условие \"{0}\" может назначаться только для документа Продажа (Чек)";
			}
			return "\"{0}\" պայմանով զեղչ կարելի է սահմանել միայն Վաճառք (Կտրոն) փաստաթղթի համար";
		}
	}

	public static string AllowShowAvailability
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Display stock availability";
				}
				return "Разрешить просмотр остатков";
			}
			return "Թույլ տալ տեսնել առկայությունը";
		}
	}

	public static string FATransportationDocuments
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "FA transportation documents journal";
				}
				return "Журнал документов по перемещению ОС";
			}
			return "ՀՄ տեղափոխման փաստաթղթերի մատյան";
		}
	}

	public static string WriteOffTaxInvoicesJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales invoices journal";
				}
				return "Журнал выписанных счетов";
			}
			return "Դուրս գրված հաշիվների մատյան";
		}
	}

	public static string ImportSorce
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Source of import";
				}
				return "Источник импорта";
			}
			return "Ներմուծման աղբյուր";
		}
	}

	public static string DocTypeNotSpecified
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document type is not specified";
				}
				return "Тип документа не указан";
			}
			return "Փաստաթղթի տեսակը նշված չէ";
		}
	}

	public static string SubmissionDatePeriod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Issued period";
				}
				return "Период списания";
			}
			return "Դուրս գրման ժամանակահատված";
		}
	}

	public static string SupplyDatePeriod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Delivery period";
				}
				return "Пеиод доставки";
			}
			return "Մատակարարման ժամանակահատված";
		}
	}

	public static string WithImage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "with image";
				}
				return "с изображением";
			}
			return "նկարով";
		}
	}

	public static string OldVersion
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Old version";
				}
				return "Старая версия";
			}
			return "Հին տարբերակ";
		}
	}

	public static string TaxCodeIsNotRequired
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "TIN is not required";
				}
				return "Не обязательно иметь ИНН";
			}
			return "ՀՎՀՀ ունենալը պարտադիր չէ";
		}
	}

	public static string DifferentPrepaymentPartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Customer and Customer prepayment redemption are different. You must use the document Return (Bill)";
				}
				return "Покупатель и Покупатель зачет аванса различаются. Необходимо использовать документ Возврат (Чек)";
			}
			return "Գնորդ և Կանխավճարի մարման գնորդ դաշտերը տարբերվում են: Անհրաժեշտ է օգտագործել Վերադարձ (Կտրոն) փաստաթուղթը";
		}
	}

	public static string ReturnByPrePaymentSumGreaterThanBaseDocByPrePaymentSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Returning prepayment amount is greater than prepayment amount in base document";
				}
				return "Сумма возврата аванса больше суммы аванса документа основания";
			}
			return "Վերադարձվող կանխավճարի գումարը մեծ է հիմք փաստաթղթի կանխավճարի գումարից";
		}
	}

	public static string TypeDoesNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product type {0} does not exist";
				}
				return "Тип продукта {0} не существует";
			}
			return "Ապրանքի համար {0} տեսակ գոյություն չունի";
		}
	}

	public static string CanNotCreateDocWithRetailStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In the case of a retail storage, it is impossible to create a Sales (Invoice) document";
				}
				return "В случае с розничным складом создание документа Продажи (Счет-фактура) невозможно";
			}
			return "Մանրածախ պահեստի դեպքում հնարավոր չէ ստեղծել Վաճառք (Հաշիվ-Ապրանքագիր) փաստաթուղթ";
		}
	}

	public static string PartnerBelongToPartGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are partners belonging to this group of partners. Continue?";
				}
				return "Есть партнеры, принадлежащие к данной группе партнеров. Продолжить?";
			}
			return "Գոյություն ունեն տվյալ գործընկերոջ խմբին պատկանող գործընկերներ: Շարունակե՞լ:";
		}
	}

	public static string ProductBelongToGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are goods or services belonging to this group. Continue?";
				}
				return "Есть товары или услуги, принадлежащие к этой группе. Продолжить?";
			}
			return "Գոյություն ունեն տվյալ խմբին պատկանող ապրանքներ կամ ծառայություններ: Շարունակե՞լ:";
		}
	}

	public static string SalesAveragePrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Average Sales Price";
				}
				return "Средняя цена продаж";
			}
			return "Վաճառքների միջին գին";
		}
	}

	public static string AvgPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Average Sales Price";
				}
				return "Средняя цена продажи";
			}
			return "Վաճառքի միջին գին";
		}
	}

	public static string AvgPricesFormation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Average price formation";
				}
				return "Формирование средней цены";
			}
			return "Միջին գնի ձևավորում";
		}
	}

	public static string AvgPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Type of avarage price";
				}
				return "Тип средней цены";
			}
			return "Միջին գնի տեսակ";
		}
	}

	public static string DicountPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount price type";
				}
				return "Тип скидочной цены";
			}
			return "Զեղչված գնի տեսակ";
		}
	}

	public static string InconsistencyInTotal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Total amounts discrepancy";
				}
				return "Несоответствие общих сумм";
			}
			return "Ընդհանուր գումարների անհամապատասխանություն";
		}
	}

	public static string UseScriptInDiscount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Apply external module";
				}
				return "Применить внешний модуль";
			}
			return "Կիրառել արտաքին մոդուլ";
		}
	}

	public static string MTSummIntegrityCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Checking the integrity of the amounts of document lists and the products operations journal";
				}
				return "Проверка целостности сумм списков документов и журнала операций товаров";
			}
			return "Փաստաթղթերի ցուցակների և ապրանքների գործողությունների մատյանի գումարների ամբողջականության ստուգում";
		}
	}

	public static string MTSummIntegrityErr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discrepancy between the total amount of good with code \"{1}\" of document \"{0}\" in the document list and in the operations journal";
				}
				return "Несоответствие общей суммы товара кодом \"{1}\" документа \"{0}\" в списке документа и в журнале действий";
			}
			return "\"{0}\" փաստաթղթի \"{1}\" կոդով ապրանքի ընդհանուր գումարի անհամապատասխանություն փաստաթղթի ցուցակում և գործողությունների մատյանում";
		}
	}

	public static string MTBarCodes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Barcodes, {0} - {1} ";
				}
				return "Штрихкоды, {0} - {1} ";
			}
			return "Գծիկավոր կոդեր, {0} - {1} ";
		}
	}

	public static string ComplectationsWithServices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kitting document {0} contains a service row";
				}
				return "Документ комплектации {0} содержит строку услуги";
			}
			return "{0} փաստաթուղթը պարունակում է ծառայության տող";
		}
	}

	public static string IsDistrInProportion
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Distribute proportionately";
				}
				return "Распределить пропорционально";
			}
			return "Բաշխել համամասնորեն";
		}
	}

	public static string ContractCurrNotSame
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The currency of \"{1}\" contract  of \"{0}\" partner is incorrect";
				}
				return "Валюта контракта \"{1}\" партнера \"{0}\" указано неверно";
			}
			return "\"{0}\" գործընկերոջ \"{1}\" պայմանագրի արժույթը սխալ է նշված";
		}
	}

	public static string InitialPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Original price";
				}
				return "Начальная цена";
			}
			return "Սկզբնական գին";
		}
	}

	public static string InitialPriceWirhCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Original price with {0}";
				}
				return "Начальная цена с {0}";
			}
			return "Սկզբնական գին {0}-ով";
		}
	}

	public static string InitialSumm
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Original amount";
				}
				return "Начальная сумма";
			}
			return "Սկզբնական գումար";
		}
	}

	public static string InitialSummWirhCurr
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Original amount with {0}";
				}
				return "Начальная сумма с  {0}";
			}
			return "Սկզբնական գումար {0}-ով";
		}
	}

	public static string CardIsCanceled
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The card has been canceled.";
				}
				return "Карта аннулирована.";
			}
			return "Քարտը կասեցված է:";
		}
	}

	public static string CardIsNotValid
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The card is not valid (sale date is out of validation period).";
				}
				return "Карта недействительна (дата продажи вне периода действия).";
			}
			return "Քարտը վավեր չէ (վաճառքի ամսաթիվը դուրս է վավերականության ժամանակահատվածից)։";
		}
	}

	public static string ReportType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Report type";
				}
				return "Тип отчёта";
			}
			return "Հաշվետվության տեսակ";
		}
	}

	public static string ReturnByPrePaySumMustBeZero
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In case of return, the prepayment amount should be 0";
				}
				return "В случае возврата сумма предоплаты должна быть 0";
			}
			return "Վերադարձի դեպքում կանխավճարի գումարը պետք է լինի 0";
		}
	}

	public static string RussianPassport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Passport issued by foreign country (RF citizen)";
				}
				return "Паспорт иностранного гражданина (гражданин РФ)";
			}
			return "Օտարերկրյա անձնագիր (ՌԴ քաղաքացի)";
		}
	}

	public static string IDDocType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ID document type";
				}
				return "Документ, удостоверяющий личность";
			}
			return "Անձը հաստատող փաստաթղթի տեսակ";
		}
	}

	public static string WithCurrency
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In currency";
				}
				return "В валюте";
			}
			return "Արժույթով";
		}
	}

	public static string TerminalCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Terminal code";
				}
				return "Код терминала";
			}
			return "Տերմինալի կոդ";
		}
	}

	public static string ProgramNameStart
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "AS-Shop";
				}
				return "AS-Shop";
			}
			return "ՀԾ-Խանութ";
		}
	}

	public static string TrialModeTimeEndDay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Your free trial will expire in";
				}
				return "Ваш бесплатный пробный период заканчивается";
			}
			return "Անվճար ժամանակահատվածի ավարտին մնացել է";
		}
	}

	public static string TrialModeTimeEnded
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Your free trial has expired.";
				}
				return "Ваш бесплатный пробный период закончился.";
			}
			return "Անվճար ժամանակահատվածը ավարտվել է";
		}
	}

	public static string CheckMarkingAndBarCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check compliance of products marking and barcode";
				}
				return "Проверять соответствие маркировки и штрихкода";
			}
			return "Ստուգել դրոշմանիշի և գծիկավոր կոդի համապատասխանությունը";
		}
	}

	public static string ChooseOnlineStore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Choose online store";
				}
				return "Выбрать онлайн-магазин";
			}
			return "Ընտրել օնլայն խանութը";
		}
	}

	public static string CodeInTrade
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "AS code";
				}
				return "AS код";
			}
			return "ՀԾ կոդ";
		}
	}

	public static string Connections
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connections";
				}
				return "Подключение";
			}
			return "Միացումներ";
		}
	}

	public static string ImportGoodsFromOnlineStores
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import online store product list";
				}
				return "Загрузка списка товаров онлайн-магазина";
			}
			return "Օնլայն խանութի ապրանքացանկի ներմուծում";
		}
	}

	public static string IntegrationSettings
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connection settings";
				}
				return "Настройки подключения";
			}
			return "Համակցման կարգավորումներ";
		}
	}

	public static string Match
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Match";
				}
				return "Сопоставить";
			}
			return "Համապատասխանեցնել";
		}
	}

	public static string MatchWith
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Match by";
				}
				return "Сопоставить по";
			}
			return "Համաատասխանեցնել ըստ";
		}
	}

	public static string OnlineStoresGoods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online store product list";
				}
				return "Товары онлайн-магазина";
			}
			return "Օնլայն խանութների ապրանքացանկեր";
		}
	}

	public static string ProductsAndBalances
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products and balances";
				}
				return "Товары и остатки";
			}
			return "Ապրանքներ և մնացորդներ";
		}
	}

	public static string Token
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Token";
				}
				return "Токен";
			}
			return "Թոկեն";
		}
	}

	public static string UnitInTrade
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "AS m/u";
				}
				return "AS ед. изм.";
			}
			return "ՀԾ չ/մ";
		}
	}

	public static string OnlineStoreGood
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online store product";
				}
				return "Товар онлайн-магазина";
			}
			return "Օնլայն խանութի ապրանք";
		}
	}

	public static string EditDiscounts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount editing";
				}
				return "Редактирование скидки";
			}
			return "Զեղչի խմբագրում";
		}
	}

	public static string KeyboardMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Keyboard";
				}
				return "Клавиатурный";
			}
			return "Ստեղնաշարային";
		}
	}

	public static string TouchScreenWithFavoritesMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Touch screen (Favorites)";
				}
				return "Сенсорный экран (Избранные)";
			}
			return "Սենսորային էկրան (Ընտրյալներ)";
		}
	}

	public static string YouCanNotRemoveThisRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You cannot delete this line";
				}
				return "Вы не можете удалить эту строку";
			}
			return "Դուք չեք կարող հեռացնել այս տողը";
		}
	}

	public static string AtLeastOneShouldBeSelected
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "At least one of the fields of Sales Price Type or Discount Price Type must be specified";
				}
				return "Необходимо указать хотя бы одно из полей Тип цены продажи или Тип цены со скидкой";
			}
			return "Վաճառքի գնի տեսակ կամ զեղչի գնի տեսակ դաշտերից առնվազն մեկը պետք է նշված լինի";
		}
	}

	public static string CanNotBelongToTheSameStore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The listed products must belong to the same store";
				}
				return "Перечисленные товары должны принадлежать одному и тому же магазину";
			}
			return "Նշված ապրանքները պետք է պատկանեն միևնույն խանութին";
		}
	}

	public static string ErrorsDuringImportPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price import errors";
				}
				return "Ошибки при загрузке цен";
			}
			return "Գների ներմուծման սխալներ";
		}
	}

	public static string ImportPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import prices";
				}
				return "Загружать цены";
			}
			return "Ներմուծել գները";
		}
	}

	public static string InnerCodeSearch
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Internal code search";
				}
				return "Поиск внутреннего кода";
			}
			return "Ներքին կոդի որոնում";
		}
	}

	public static string InFavoritesFirtsGroups
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show favorite product groups in the list: at the beginning (Yes), at the end (No)";
				}
				return "Показывать группы избранных товаров в списке: в начале (Да), в конце (Нет)";
			}
			return "Ընտրյալ ապրանքների խմբերը ցույց տալ ցանկի սկզբում` «Այո», վերջում` «Ոչ»";
		}
	}

	public static string RecalcQty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Recalculate quantity when editing the final amount (applicable to weighted goods)";
				}
				return "Пересчитать количество при редактировании конечной суммы (применяется для весового товара)";
			}
			return "Վերջնական գումարը խմբագրելիս վերահաշվարկել քանակը (գործում է քաշային ապրանքի դեպքում)";
		}
	}

	public static string ExportPrices
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export prices";
				}
				return "Выгружать цены";
			}
			return "Արտահանել գները";
		}
	}

	public static string ImportPriceDifferentCurrency
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The currency \"{0}\" of the import price is different from the currency \"{2}\" of the price type \"{1}\"";
				}
				return "Валюта \"{0}\" цены импорта  отличается от валюты \"{2}\" типа цены \"{1}\" ";
			}
			return "Ներմուծվող գնի \"{0}\" արժույթը տարբեր է \"{1}\" Գնի տեսակի \"{2}\" արժույթից";
		}
	}

	public static string PriceNotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The price of the product or service \"{0}\" with the date \"{1}\", price type \"{2}\", measurement unit \"{3}\" is not found.";
				}
				return "Цена на товар или услугу \"{0}\" с датой \"{1}\", типом цены \"{2}\", единицей измерения \"{3}\" не найдена.";
			}
			return "\"{0}\" ապրանքի կամ ծառայության համար \"{1}\" ամսաթվով, \"{2}\" գնի տեսակով, \"{3}\" չափման միավորով գին չի հայտնաբերվել";
		}
	}

	public static string CurrencyMismatch
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price type currency mismatch";
				}
				return "Несоответствие валюты типа цены";
			}
			return "Գնի տեսակի արժույթի անհամապատասխանություն";
		}
	}

	public static string AddMTFavToUser
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add to user's favorite products";
				}
				return "Добавить в избранные товары пользователя";
			}
			return "Ավելացնել օգտագործողի ընտրյալ ապրանքներին";
		}
	}

	public static string SelectedMaterialsAddToSegment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The specified products have been added to segment.";
				}
				return "Указанные товары добавлены в сегмент.";
			}
			return "Նշված ապրանքները ավելացվեցին սեգմենտին";
		}
	}

	public static string SelectedMaterialsAddToSelectedUser
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The specified products have been added to the favorites of \"{0}\" user.";
				}
				return "Указанные товары добавлены в избранные пользователя \"{0}\".";
			}
			return "Նշված ապրանքները ավելացվեցին \"{0}\" օգտագործողի ընտրյալներին";
		}
	}

	public static string GroupList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group list";
				}
				return "Список групп";
			}
			return "Խմբերի ցանկ";
		}
	}

	public static string GroupsSettings
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group settings";
				}
				return "Настройка групп";
			}
			return "Խմբերի կարգավորում";
		}
	}

	public static string GroupsViewSettings
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group display settings";
				}
				return "Настройка отображения групп";
			}
			return "Խմբերի ցուցադրման կարգավորում";
		}
	}

	public static string SelectedMTCanBeGroupedByGroup
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Favorite products can be grouped into groups.";
				}
				return "Избранные товары можно группировать по группам.";
			}
			return "Ընտրյալ ապրանքները հնարավոր է խմբավորել ըստ խմբերի:";
		}
	}

	public static string AllowChangeCardViewSize
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to change the height of the favorite products card";
				}
				return "Разрешить изменения высоты карточки избранных товаров";
			}
			return "Թույլ տալ փոխել ընտրյալ ապրանքների քարտի բարձրությունը";
		}
	}

	public static string ShowBarCodeInGoodsSelectList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show barcode in product selection list";
				}
				return "Показывать штрихкод в списке подбора товаров";
			}
			return "Ցույց տալ գծ. կոդը ապրանքների ընտրման ցուցակում";
		}
	}

	public static string InnerStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Internal storage";
				}
				return "Внутренний склад";
			}
			return "Ներիքն պահեստ";
		}
	}

	public static string ExportRemains
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export balances";
				}
				return "Выгружать остатки";
			}
			return "Արտահանել մնացորդները";
		}
	}

	public static string ExportRemainsErrors
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Errors during export of balances";
				}
				return "Ошибки при выгрузке остатков";
			}
			return "Մնացորդների արտահանման սխալներ";
		}
	}

	public static string OpenItemInStore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Open product in online store";
				}
				return "Открыть товар в онлайн-магазине";
			}
			return "Բացել ապրանքը օնլայն խանութում";
		}
	}

	public static string POSCardNumber_NullText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Buyer card";
				}
				return "Карта покупателя";
			}
			return "Գնորդի քարտ";
		}
	}

	public static string POSGoodName_NullText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add product. There are no products in list";
				}
				return "Добавьте товар сканером или из справочника";
			}
			return "Ավելացրեք ապրանքը սկաներով կամ տեղեկատուից";
		}
	}

	public static string SelectedMaterialsAlreadyExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The specified products are already in the favorites of user \"{0}\".";
				}
				return "Указанные товары уже есть в избранных у пользователя \"{0}\"";
			}
			return "Նշված ապրանքներն արդեն գոյություն ունեն \"{0}\" օգտագործողի ընտրյալներում";
		}
	}

	public static string StorageInStore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Srorage in online-store";
				}
				return "Склад в онлайн-магазине";
			}
			return "Պահեստը օնլայն խանութում";
		}
	}

	public static string ActionsAreRecordedWithTheStore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The store \"{0}\" has a registered transaction.";
				}
				return "В магазине \"{0}\" зарегистрирована операция.";
			}
			return "\"{0}\" խանութով գրանցված է գործողություն";
		}
	}

	public static string CantRemoveB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Removing the b2b.armsoft platform is prohibited";
				}
				return "Удаление платформы b2b.armsoft запрещено";
			}
			return "b2b.armsoft հարթակի հեռացումն արգելված է";
		}
	}

	public static string OnlineStoreDocs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online store documents";
				}
				return "Документы онлайн-магазина";
			}
			return "Օնլայն խանութի փաստաթղթեր";
		}
	}

	public static string OnlineStorePartner
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online store  (partner)";
				}
				return "Онлайн-магазин (партнер)";
			}
			return "Օնլայն խանութ (գործընկեր)";
		}
	}

	public static string OrderPrefix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Store order number prefix";
				}
				return "Префикс номера заказа магазина";
			}
			return "Խանութի պատվերի համարի նախդիր";
		}
	}

	public static string OrdersBill
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Orders (Sale)";
				}
				return "Заказы (Реализация)";
			}
			return "Պատվերներ (Վաճառք)";
		}
	}

	public static string ImportNewOrders
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import new orders";
				}
				return "Импорт новых заказов";
			}
			return "Ներմուծել նոր պատվերները";
		}
	}

	public static string ImportOrders
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import orders";
				}
				return "Импорт заказов";
			}
			return "Ներմուծել պատվերները";
		}
	}

	public static string ImportOrdersErrors
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Errors while importing orders";
				}
				return "Ошибки во время импорта заказов";
			}
			return "Պատվերնեի ներմուծման սխալներ";
		}
	}

	public static string OnlineStoreOrderID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "OS order ISN";
				}
				return "Идентификатор заказа OM";
			}
			return "ՕԽ պատվերի նույնացուցիչ";
		}
	}

	public static string UpdateStatus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Update state";
				}
				return "Обновить статус";
			}
			return "Թարմացնել վիճակը";
		}
	}

	public static string MatchingIsCompleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Matching completed";
				}
				return "Сопоставление завершено";
			}
			return "Համապատասխանեցումն ավարտված է";
		}
	}

	public static string DefaultCashIsEmpty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "User's default cash desk is not specified";
				}
				return "Касса пользователя не указана";
			}
			return "Օգտագործողի լռությամբ դրամարկղը նշված չէ";
		}
	}

	public static string OnlineStoreStorageIsMissing
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In the online store settings, Storage field is not specified";
				}
				return "В настройках онлайн-магазина поле Склад не указано";
			}
			return "Օնլայն խանութի կարգավորումներում պահեստ դաշտը նշված չէ";
		}
	}

	public static string UpdateOrdersStatusesQuestion
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Update orders status?";
				}
				return "Обновить статусы заказов?";
			}
			return "Թարմացնե՞լ պատվերների կարգավիճակները";
		}
	}

	public static string OnlineOrderExistsWithID
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online store order number {0} exists";
				}
				return "По номеру {0} существует заказ Онлайн-магазина";
			}
			return "{0} համարով Օնյալն խանութի պատվեր գոյություն ունի";
		}
	}

	public static string UpdateCompletedSuccessfully
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Update completed successfully";
				}
				return "Обновление успешно завершено";
			}
			return "Թարմացումը բարեհաջող կատարվեց";
		}
	}

	public static string OnlineStoresIntegration
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online stores integration";
				}
				return "Интеграция онлайн-магазинов";
			}
			return "Օնլայն խանութների համակցում";
		}
	}

	public static string OnlineStoresSettings
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Online stores settings";
				}
				return "Настройки онлайн-магазинов";
			}
			return "Օնլայն խանութների կարգավորումներ";
		}
	}

	public static string UserCanNotSellWith100PercentDiscount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The user does not have permission to sell all rows at a 100% discount";
				}
				return "Пользователь не имеет права продавать все строки со 100%-ой скидкой";
			}
			return "Օգտագործողը իրավասու չէ բոլոր տողերը վաճառել 100% զեղչով";
		}
	}

	public static string SameAccounts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input and output accounts must be different";
				}
				return "Счета ввода и вывода должны различаться";
			}
			return "Մուտքի և ելքի հաշիվները պետք է լինեն տարբեր";
		}
	}

	public static string OutputAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "From account";
				}
				return "Из счета";
			}
			return "Ելքի հաշիվ";
		}
	}

	public static string POSGoodRowDetail_NullText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add a product using a scanner or items list";
				}
				return "Добавьте товар сканером или из справочника";
			}
			return "Ավելացրեք ապրանքը սկաներով կամ տեղեկատուից";
		}
	}

	public static string TheItemIsNotImportedOrMatched
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product {2} of {1} sale by date {0} was not imported or matched";
				}
				return "Товар с кодом {2} продажи  {1} на дату {0} не был импортирован или сопоставлен";
			}
			return "{0} ամսաթվով {1} վաճառքի {2} կոդով ապրանքը ներմուծված կամ համապատասխանեցված չէ";
		}
	}

	public static string NotInSegment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" segment is missing from \"Segments\" directory";
				}
				return "Сегмент \"{0}\" отсутствует в справочнике \"Сегменты\"";
			}
			return "\"{0}\" սեգմենտը բացակայում է \"Սեգմենտներ\" տեղեկատուից";
		}
	}

	public static string ChangeUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Change of base and alternative UoM";
				}
				return "Изменение основной и допалнительной ед. изм.";
			}
			return "Հիմնական և լրացուցիչ չ/մ փոփոխություն";
		}
	}

	public static string DifferentCurrAccounts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Input and output accounts cannot have different currencies";
				}
				return "Приходные и расходные счета не могут иметь разные валюты";
			}
			return "Մուտքի և ելքի հաշիվները չեն կարող ունենալ տարբեր արժույթներ";
		}
	}

	public static string NotInDiscountGroups
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" discount group is missing from \"Doscounts groups\" directory";
				}
				return "Группа скидок \"{0}\" отсутствует в справочнике \"Группы скидок\"";
			}
			return "\"{0}\" զեղչի խումբը բացակայում է \"Զեղչերի խմբեր\" տեղեկատուից";
		}
	}

	public static string DistributedCurrSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allocated amount with VAT in currency";
				}
				return "Распределенная сумма с НДС в валюте";
			}
			return "Բաշխված գումար ԱԱՀ-ով արժույթով";
		}
	}

	public static string DistributedSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allocated amount without VAT in dram";
				}
				return "Распределенная сумма без НДС в драмах";
			}
			return "Բաշխված գումար առանց ԱԱՀ դրամով";
		}
	}

	public static string DistributedVATSumma
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allocated amount with VAT in dram";
				}
				return "Распределенная сумма с НДС в драмах";
			}
			return "Բաշխված գումար ԱԱՀ-ով դրամով";
		}
	}

	public static string ImportDataFromASAccountant
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import data from AS-Accountant";
				}
				return "Импорт данных из AS-Accountant";
			}
			return "Ներմուծել տվյալներ ՀԾ-Հաշվապահից";
		}
	}

	public static string DataImportFromASAccountant
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data import from AS-Accountant";
				}
				return "Импорт данных из программы AS-Accountant";
			}
			return "Տվյալների ներմուծում ՀԾ-Հաշվապահ համակարգից";
		}
	}

	public static string CodeOrBarCodeMustBeFIlled
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product or bar code must be filled";
				}
				return "Товар или штрих-код должен быть заполнен";
			}
			return "Ապրանքը կամ գծիկավոր կոդը պետք է լրացված լինի";
		}
	}

	public static string Requiredx32ClientForArcus2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-Cash payment cannot be made through ARCUS 2 POS. It is necessary to install the 64-bit version of ARCUS 2 or use the 32-bit version of the program.";
				}
				return "Безналичная оплата не может быть произведена через ARCUS 2 POS. Необходимо установить 64-битную версию ARCUS 2 или использовать 32-битную версию программы.";
			}
			return "Անկանխիկ մարումը ARCUS 2 POS-ի միջոցով հնարավոր չէ կատարել: Անհրաժեշտ է տեղադրել ARCUS 2-ի 64-բիթ տարբերակը կամ օգտագործել համակարգի 32-բիթ տարբերակը:";
		}
	}

	public static string Requiredx64ClientForArcus2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Non-Cash payment cannot be made through ARCUS 2 POS. It is necessary to install the 32-bit version of ARCUS 2 or use the 64-bit version of the program.";
				}
				return "Безналичная оплата не может быть произведена через ARCUS 2 POS. Необходимо установить 32-битную версию ARCUS 2 или использовать 64-битную версию программы.";
			}
			return "Անկանխիկ մարումը ARCUS 2 POS-ի միջոցով հնարավոր չէ կատարել: Անհրաժեշտ է տեղադրել ARCUS 2-ի 32-բիթ տարբերակը կամ օգտագործել համակարգի 64-բիթ տարբերակը:";
		}
	}

	public static string ContractCurrencyMismatch
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contract Currency Mismatch";
				}
				return "Несоответствие валюты контракта";
			}
			return "Պայմանագրի արժույթի անհամապատասխանություն";
		}
	}

	public static string DeviceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Device type";
				}
				return "Тип устройства";
			}
			return "Սարքի տեսակ";
		}
	}

	public static string ElScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "El. scale (Off-line)";
				}
				return "Эл. весы (Off-line)";
			}
			return "Էլ. կշեռք (Off-line)";
		}
	}

	public static string Terminal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Terminal (Off-line)";
				}
				return "Терминал (Off-line)";
			}
			return "Տերմինալ (Off-line)";
		}
	}

	public static string DeviceModel
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Device brand";
				}
				return "Марка устройства";
			}
			return "Սարքի մակնիշ";
		}
	}

	public static string FileToExport
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export file";
				}
				return "Файл экспорта";
			}
			return "Արտահանման ֆայլ";
		}
	}

	public static string Device
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Device";
				}
				return "Оборудование";
			}
			return "Սարքավորում";
		}
	}

	public static string ElScalesAndTerminals
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "El. scales, terminals (Off-line)";
				}
				return "Эл. весы, терминалы (Off-line)";
			}
			return "Էլ․ կշեռքներ, տերմինալներ (Off-line)";
		}
	}

	public static string ExportDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export date";
				}
				return "Дата экспорта";
			}
			return "Արտահ․ ա/թ";
		}
	}

	public static string ExportForElScaleAndTerminal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export (el. scale, terminal)";
				}
				return "Выгрузить (эл. весы, терминалы)";
			}
			return "Արտահանել (էլ․ կշեռք, տերմինալ)";
		}
	}

	public static string ExportProducts
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export products";
				}
				return "Выгрузить товары";
			}
			return "Արտահանել ապրանքները";
		}
	}

	public static string GroupElScalesAndTerminals
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Group (el. scales, terminals)";
				}
				return "Группа (эл. весы, терминалы)";
			}
			return "Խմբային (էլ․ կշեռքներ, տերմինալներ)";
		}
	}

	public static string AdditionalCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Additional caption";
				}
				return "Дополнительное наименование";
			}
			return "Լրացուցիչ անվանում";
		}
	}

	public static string TouchScreenWithFavoritesMinSizeMustBe1024
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Touch screen (Favorites) mode does not work on screens smaller than 1024";
				}
				return "Режим сенсорного экрана (Избранные) не работает на экранах размером менее 1024";
			}
			return "Սենսորային Էկրան (Ընրյալներ) ռեժիմը չի գործում 1024 չափից փոքր էկրանների համար";
		}
	}

	public static string InputAccount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Into account";
				}
				return "На счет";
			}
			return "Մուտքի հաշիվ";
		}
	}

	public static string DiscountAndPrePayment
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount and prepayment";
				}
				return "Скидка и предоплата";
			}
			return "Զեղչ և կանխավճար";
		}
	}

	public static string PrePaymentECRCheckNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment ECRC N";
				}
				return "Номер чека предоплаты ККМ";
			}
			return "Կանխավճարի ՀԴՄԿ N";
		}
	}

	public static string PrintBonusAs
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print bonus as";
				}
				return "Бонус напечатать как";
			}
			return "Բոնուսը տպել որպես";
		}
	}

	public static string AttributeWithCodeAlreadyExists
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{2}\" of property type \"{0}\" with code \"{1}\" already exists";
				}
				return "\"{2}\" типа свойства \"{0}\" с кодом \"{1}\" уже существует";
			}
			return "\"{0}\" Հատկության տեսակի \"{1}\" կոդով \"{2}\" արդեն գոյություն ունի";
		}
	}

	public static string AdditionalDescription1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Additional specification 1";
				}
				return "Дополнительная характеристика 1";
			}
			return "Լրացուցիչ բնութագիր 1";
		}
	}

	public static string AdditionalDescription2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Additional specification 2";
				}
				return "Дополнительная характеристика 2";
			}
			return "Լրացուցիչ բնութագիր 2";
		}
	}

	public static string AttributeValue1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property value 1";
				}
				return "Значение свойства 1";
			}
			return "Հատկության արժեք 1";
		}
	}

	public static string AttributeValue2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Property value 2";
				}
				return "Значение свойства 2";
			}
			return "Հատկության արժեք 2";
		}
	}

	public static string BonusPercent
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus (%)";
				}
				return "Бонус (%)";
			}
			return "Բոնուս (%)";
		}
	}

	public static string BonusPoint
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bonus (Unit)";
				}
				return "Бонус (Единица)";
			}
			return "Բոնուս (Միավոր)";
		}
	}

	public static string MTListWithoutParties
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products list (Without batches)";
				}
				return "Список товаров (Без партий)";
			}
			return "Ապրանքների ցուցակ (Առանց խմբաքանակների)";
		}
	}

	public static string MTListWithAll
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products list (All)";
				}
				return "Список товаров (Все)";
			}
			return "Ապրանքների ցուցակ (Բոլորը)";
		}
	}

	public static string MTPartyList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products list (Batch)";
				}
				return "Список товаров (Партия)";
			}
			return "Ապրանքների ցուցակ (Խմբաքանակ)";
		}
	}

	public static string GenderName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Gender - Name";
				}
				return "Пол - наименование";
			}
			return "Սեռ - անվանում";
		}
	}

	public static string TaxCode1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Taxpayer’s ID 1";
				}
				return "ИНН код 1";
			}
			return "ՀՎՀՀ 1";
		}
	}

	public static string TaxCode2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Taxpayer’s ID 2";
				}
				return "ИНН код 2";
			}
			return "ՀՎՀՀ 2";
		}
	}

	public static string TaxCode3
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Taxpayer’s ID 3";
				}
				return "ИНН код 3";
			}
			return "ՀՎՀՀ 3";
		}
	}

	public static string TaxCode5
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Taxpayer’s ID 5";
				}
				return "ИНН код 5";
			}
			return "ՀՎՀՀ 5";
		}
	}

	public static string TaxCode4
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Taxpayer’s ID 4";
				}
				return "ИНН код 4";
			}
			return "ՀՎՀՀ 4";
		}
	}

	public static string TaxCode8
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Taxpayer’s ID 8";
				}
				return "ИНН код 8";
			}
			return "ՀՎՀՀ 8";
		}
	}

	public static string TaxCode6
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Taxpayer’s ID 6";
				}
				return "ИНН код 6";
			}
			return "ՀՎՀՀ 6";
		}
	}

	public static string TaxCode7
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Taxpayer’s ID 7";
				}
				return "ИНН код 7";
			}
			return "ՀՎՀՀ 7";
		}
	}

	public static string UnitCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unit (code)";
				}
				return "Единица (код)";
			}
			return "Միավոր (կոդ)";
		}
	}

	public static string Summa1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount 1";
				}
				return "Сумма 1";
			}
			return "Գումար 1";
		}
	}

	public static string PriceWithoutVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price (without VAT)";
				}
				return "Цена (Без НДС)";
			}
			return "Գին (Առանց ԱԱՀ)";
		}
	}

	public static string PriceWithVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price (VAT included)";
				}
				return "Цена (Включая НДС)";
			}
			return "Գին (Ներառյալ ԱԱՀ)";
		}
	}

	public static string SumWithoutVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amout (without VAT)";
				}
				return "Сумма (Без НДС)";
			}
			return "Գումար (Առանց ԱԱՀ)";
		}
	}

	public static string PriceWithCurrency
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Price (in currency)";
				}
				return "Цена (в валюте)";
			}
			return "Գին (արժույթով)";
		}
	}

	public static string SumWithCurrency
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount (in currency)";
				}
				return "Сумма (в валюте)";
			}
			return "Գումար (արժույթով)";
		}
	}

	public static string DocNumWithBarCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document N (barcode)";
				}
				return "Номер док. (Штрихкод)";
			}
			return "Փաստաթղթի N (Գծիկավոր կոդ)";
		}
	}

	public static string ItemListWithDeletedRows
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Items list (With deleted rows)";
				}
				return "Список товаров и услуг (С удаленными строками)";
			}
			return "Անվանացուցակ (Հեռացված տողերով)";
		}
	}

	public static string ItemListWithParty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Items list (Batch)";
				}
				return "Список товаров и услуг (Партия)";
			}
			return "Անվանացուցակ (Խմբաքանակ)";
		}
	}

	public static string ContractReceiver
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Contract (Receiver)";
				}
				return "Договор (Перенести на)";
			}
			return "Պայմանագիր (Ընդունող)";
		}
	}

	public static string ComplectWithParty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Kit (Batch)";
				}
				return "Комплект (Партия)";
			}
			return "Կոմպլեկտ (Խմբաքանակ)";
		}
	}

	public static string DocOnlyForWholesaleStoreAndNegativeQty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} document can be created only for wholesale storage";
				}
				return "Документ \"{0}\" можно создать только для оптового склада";
			}
			return "\"{0}\" փաստաթուղթը կարելի է ստեղծել միայն մեծածախ պահեստի և բացասական մնացորդների համար";
		}
	}

	public static string ContractCurrMustBeNCC
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The currency of the POS terminal provision contract must be in national currency.";
				}
				return "Валюта договора по предоставлению POS-терминала должна быть в национальной валюте";
			}
			return "POS տերմինալի տրամադրման պայմանագրի արժույթը պետք է լինի ազգային արժույթով";
		}
	}

	public static string CreateDocOnlyForRetailStorage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document can be created only for retail storage";
				}
				return "Документ можно создать только для розничного склада";
			}
			return "Փաստաթուղթը կարելի է ստեղծել միայն մանրածախ պահեստի համար";
		}
	}

	public static string CreateDocOnlyForWholesaleStore
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Document can be created only for wholesale storage";
				}
				return "Документ можно создать только для оптового склада";
			}
			return "Փաստաթուղթը կարելի է ստեղծել միայն մեծածախ պահեստի համար";
		}
	}

	public static string AllowDeleteFullRow
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow to delete row comletely";
				}
				return "Разрешить удаление строки полностью";
			}
			return "Թույլ տալ տողը հեռացնել ամբողջությամբ";
		}
	}

	public static string MainSupplier
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Main provider";
				}
				return "Основной поставщик";
			}
			return "Հիմնական մատակարար";
		}
	}

	public static string CanNotPrintBonusAsPerPaymentWithECRPOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In case of ECR (POS) terminal, it is not possible to print bonus as a prepayment";
				}
				return "Распечатать бонус в качестве предоплаты с помощью POS-терминала ККМ невозможно";
			}
			return "ՀԴՄ(ՊՈՍ) տերմինալի դեպքում հնարավոր չէ տպել բոնուսը որպես կանխավճար";
		}
	}

	public static string ReturnToSupplierNotAllowedWithBonusAccumulation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not possible to create a Return from a customer based on a sale with bonus accumulation. Return is possible only with a document Return (Bill).";
				}
				return "Невозможно создать Возврат от покупателя на основе продажи с накоплением бонусов. Возврат возможен только документом Возврат (Чек).";
			}
			return "Վերադարձ գնորդից հնարավոր չէ ստեղծել բոնուսի կուտակում ունեցող վաճառքի հիման վրա։ Վերադարձ հնարավոր է կատարել միայն Վերադարձ (կտրոն) փաստաթղթի միջոցով";
		}
	}

	public static string GoodsReports
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Goods Reports";
				}
				return "Отчеты о товарах";
			}
			return "Ապրանքների հաշվետվություններ";
		}
	}

	public static string SalesReports
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Sales Reports";
				}
				return "Отчеты о продажах";
			}
			return "Վաճառքների հաշվետվություններ";
		}
	}

	public static string CasNew
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "CAS CL file exchange (xml)";
				}
				return "CAS CL обмен файлами (xml)";
			}
			return "CAS CL ֆայլային փոխանակում (xml)";
		}
	}

	public static string Cas
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "CAS CL file exchange (xls)";
				}
				return "CAS CL обмен файлами (xls)";
			}
			return "CAS CL ֆայլային փոխանակում (xls)";
		}
	}

	public static string ConnectingToElScaleError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Error connecting to scale.";
				}
				return "Ошибка подключения с весами.";
			}
			return "Կշեռքի հետ կապի հաստատման սխալ:";
		}
	}

	public static string Digi
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "DIGI SM file exchange (xml)";
				}
				return "DIGI SM обмен файлами(xml)";
			}
			return "DIGI SM ֆայլային փոխանակում (xml)";
		}
	}

	public static string EraseBeforeExporting
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Clear list before export";
				}
				return "Очистить список перед выгрузкой";
			}
			return "Մաքրել ցանկը արտահանումից առաջ";
		}
	}

	public static string ImportFromElScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Import (el. scale)";
				}
				return "Импорт (эл. весы)";
			}
			return "Ներմուծել (էլ․ կշեռքից)";
		}
	}

	public static string MassaK
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Massa-K RL (direct connection)";
				}
				return "Massa-K RL (прямое подключение)";
			}
			return "Massa-K RL (ուղիղ կապ)";
		}
	}

	public static string ShtrihPrint
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Shtrih-Print file exchange (txt)";
				}
				return "Shtrih-Print обмен файлами (txt)";
			}
			return "Shtrih-Print ֆայլային փոխանակում (txt)";
		}
	}

	public static string TMA
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "TM-A file exchange  (txt)";
				}
				return "TM-A обмен файлами (txt)";
			}
			return "TM-A ֆայլային փոխանակում (txt)";
		}
	}

	public static string MassakDeviceSettingsAreMissing
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The device driver is not installed. To connect, please install the required driver (MASSA-K Driver R v. 3.3.0). The driver can be downloaded from the website massa.ru or from the download.armsoft.am platform.";
				}
				return "Драйвер устройства не установлен. Для подключения необходимо установить драйвер (MASSA-K: Driver R v. 3.3.0), который можно скачать с сайта massa.ru или платформы download.armsoft.am.";
			}
			return "Սարքավորման դրայվերը տեղադրված չէ: Միանալու համար անհրաժեշտ է տեղադրել դրայվերը (MASSA-K: Driver R v. 3.3.0), որը կարելի է ներբեռնել massa.ru կայքից կամ download.armsoft.am հարթակից:";
		}
	}

	public static string ScaleExportedGoods
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products exported to scale";
				}
				return "Товары выгруженные на весы";
			}
			return "Կշեռք արտահանված ապրանքներ";
		}
	}

	public static string ItemsInElScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Scale products";
				}
				return "Товары на весах";
			}
			return "Կշեռքի ապրանքներ";
		}
	}

	public static string ViewScaleItemList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "View scale products";
				}
				return "Посмотреть товары на весах";
			}
			return "Դիտել կշեռքի ապրանքները";
		}
	}

	public static string BarCodeCanNotBeConvertedToString
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} barcode field format cannot be converted to numeric";
				}
				return "Формат поля штрих-кода {0} невозможно преобразовать в числовой";
			}
			return "{0} գծիկավոր կոդ դաշտի ֆորմատը հնարավոր չէ դարձնել թվային";
		}
	}

	public static string CodeCanNotBeConvertedToString
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} code field format cannot be converted to numeric";
				}
				return "Формат поля кода {0} невозможно преобразовать в числовой";
			}
			return "{0} կոդ դաշտի ֆորմատը հնարավոր չէ դարձնել թվային";
		}
	}

	public static string PLUCanNotBeConvertedToString
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} PLU field format cannot be converted to numeric";
				}
				return "Формат поля PLU {0} не может быть преобразован в числовой";
			}
			return "{0} PLU դաշտի ֆորմատը հնարավոր չէ դարձնել թվային";
		}
	}

	public static string AlreadyDocBased
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There is a document created based on this document, continue?";
				}
				return "На основании данного документа есть созданный документ. Продолжить?";
			}
			return "Նշված փաստաթղթի հիման վրա կա ստեղծված փաստաթուղթ, շարունակե՞լ";
		}
	}

	public static string AddNewMT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Add new product";
				}
				return "Добавить новый товар";
			}
			return "Ավելացնել նոր ապրանք";
		}
	}

	public static string Calcelled
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return " ";
				}
				return "Отмененные строки";
			}
			return " ";
		}
	}

	public static string ECRMessageForXXXXXXXX
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return " ";
				}
				return "Номера чека ККМ равно XXXXXXXX. Это означает, что была попытка распечатать Чек для данной реализации, но ККМ не возвратила порядковый номер чека. Необходимо проверить регистрацию Чека  реализации в ККМ. Для проверки распечатайте копию последнего чека или Z отчет.";
			}
			return " ";
		}
	}

	public static string WasNotExportedNotification
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return " ";
				}
				return "\"Не экспортировано {0}, из которых без установленных цен ({1}), без внешнего кода ({2}), недостаточное количество ({3})\"";
			}
			return " ";
		}
	}

	public static string ExportNotification
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return " ";
				}
				return "\"Экспортированы {0} , из которых с нулевым остатком  {1} \"";
			}
			return " ";
		}
	}

	public static string Removed
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Deleted {0}";
				}
				return "Удаленные {0}";
			}
			return "Հեռացված՝  {0}";
		}
	}

	public static string GoodAddMethod
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Method of adding a product";
				}
				return "Метод добавления товара";
			}
			return "Ապրանքի ավելացման եղանակ";
		}
	}

	public static string MustStoreGoodDocForAddUnits
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "To change units of measurement, you must register this directory";
				}
				return "Для изменения единиц измерения необходимо зарегистрировать этот справочник";
			}
			return "Չափման միավորները փոփոխելու համար անհրաժեշտ է գրանցել այս տեղեկատուն";
		}
	}

	public static string GoodAddSimpleMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Simple method of adding a product";
				}
				return "Упрощенный метод добавления товара";
			}
			return "Ապրանքի ավելացման պարզ եղանակ";
		}
	}

	public static string AltUnitCoeff
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Recalculation coefficient";
				}
				return "Коэффициент пересчета";
			}
			return "Վերահաշվարկի գործակից";
		}
	}

	public static string SendingItemsToElScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data transfer to scale";
				}
				return "Передача данных на весы";
			}
			return "Տվյալների ուղարկում կշեռքին";
		}
	}

	public static string BarCodeWithUnitDoesNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product {0} does not have a barcode with unit of measure {1}";
				}
				return "Товар {0} не имеет штрих-кода с единицей измерения {1}";
			}
			return "{0} ապրանքը {1} չափման միավորով գծիկավոր կոդ չունի";
		}
	}

	public static string CasDirectConnection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "CAS CL (direct connection)";
				}
				return "CAS CL (прямое подключение)";
			}
			return "CAS CL (ուղիղ կապ)";
		}
	}

	public static string CurrentOperationIsNotAvailableInOfflineMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unable to add {0} in offline mode";
				}
				return "Невозможно добавить {0} в офлайн режиме";
			}
			return "Հնարավոր չէ ավելացնել {0} օֆլայն ռեժիմում";
		}
	}

	public static string CodeInScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Code on scale";
				}
				return "Код на весах";
			}
			return "Կոդը կշեռքում";
		}
	}

	public static string ConnectionError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Connection error";
				}
				return "Ошибка подключения к терминалу";
			}
			return "Կշեռքին միանալու սխալ";
		}
	}

	public static string DataAnalyzingError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data analyzing error";
				}
				return "Ошибка обработки данных регистрации";
			}
			return "Գրանցման տվյալների մշակման սխալ";
		}
	}

	public static string DataCompressionHasBeenCompleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data compression has been completed";
				}
				return "Сжатие данных завершено";
			}
			return "Տվյալների սեղմումն ավարտված է";
		}
	}

	public static string DataCompressionIsInProgress
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data compression is in progress, please wait";
				}
				return "Идет сжатие данных, пожалуйста, подождите";
			}
			return "Կատարվում է տվյալների սեղմում, խնդրում ենք սպասել";
		}
	}

	public static string DataReceivingError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data receiving error";
				}
				return "Ошибка приема данных из терминала";
			}
			return "Կշեռքից տվյալների ստացման սխալ";
		}
	}

	public static string DataTransmittingError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Data transmitting error";
				}
				return "Ошибка передачи данных в терминал";
			}
			return "Տվյալները կշեռքին փոխանցման սխալ";
		}
	}

	public static string DriverIsBusy
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Driver is busy";
				}
				return "Драйвер занят";
			}
			return "Կշեռքի դրայվերը զբաղված է";
		}
	}

	public static string FileNotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "File not found";
				}
				return "Файл не найден";
			}
			return "Ֆայլը չի \u200b\u200bգտնվել";
		}
	}

	public static string NoMoreItemsInList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No more items in a list";
				}
				return "В справочнике больше нет записей";
			}
			return "Կշեռքի տեղեկատուում գրառումներ այլևս չկան";
		}
	}

	public static string NoMoreMemoryInTheTerminalForLoadingThisAmountOfData
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No more memory in the terminal for loading this amount of data";
				}
				return "Не хватает памяти в терминале для загрузки такого объема данных";
			}
			return "Կշեռքի հիշողությունը բավարար չէ նման ծավալի տվյալներ ներմուծելու համար";
		}
	}

	public static string NoMoreRegistrationsOnUSBFlash
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No more registrations on USB-Flash";
				}
				return "На USB-flash больше нет файлов регистраций";
			}
			return "USB ֆլեշ կրիչում աշգրանցման ֆայլեր չկան";
		}
	}

	public static string NoMoreSpaceInDriversBuffer
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "No more space in a driver's buffer";
				}
				return "Нет места для записи данных в буфер драйвера";
			}
			return "Դրայվերի բուֆերում տվյալների գրանցման տարածք չկա";
		}
	}

	public static string SettingModeError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Setting mode error";
				}
				return "Ошибка установки режима работы драйвера";
			}
			return "Դրայվերի աշխատանքային ռեժիմի սահմանման սխալ";
		}
	}

	public static string TerminalNotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Terminal not found";
				}
				return "Терминал не найден";
			}
			return "Կշեռքը չի \u200b\u200bգտնվել";
		}
	}

	public static string ThisErrorCodeIsNotSupportedByThisDriverVersion
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "This error code is not supported by this driver version";
				}
				return "Данный код ошибки не используется в текущей версии драйвер";
			}
			return "Տվյալ սխալի կոդը չի օգտագործվում դրայվերի ընթացիկ տարբերակում";
		}
	}

	public static string WrongPropertysValue
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Wrong property's value";
				}
				return "Неверное значение свойства";
			}
			return "Հատկության սխալ արժեք";
		}
	}

	public static string EraseItemsFromScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Clear items list from the scale";
				}
				return "Очистить список товары в весах";
			}
			return "Հեռացնել ապրանքները կշեռքից";
		}
	}

	public static string ErasingItemsFromScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Removing item list from the scale";
				}
				return "Очистка списка товаров на весах";
			}
			return "Ապրանքների հեռացում կշեռքից";
		}
	}

	public static string ConfirmErasingItemsFromScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Confirm to clear items list in scale";
				}
				return "Подтвердите очистку списка товаров в весах";
			}
			return "Հաստատեք կշեռքից ապրանքների հեռացումը";
		}
	}

	public static string DigiDirectConnection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "DIGI SM (direct connection)";
				}
				return "DIGI SM (прямое подключение)";
			}
			return "DIGI SM (ուղիղ կապ)";
		}
	}

	public static string TMADirectConnection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "TM-A (direct connection)";
				}
				return "TM-A (прямое подключение)";
			}
			return "TM-A (ուղիղ կապ)";
		}
	}

	public static string IDDocTypes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ID document type";
				}
				return "Типы документов, удостоверяющих личность";
			}
			return "Անձը հաստատող փաստաթղթի տեսակներ";
		}
	}

	public static string StorageMustBeWholesale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The document cannot be created for a warehouse with the Retail (accounting at sales prices) tag";
				}
				return "Документ не может быть создан для склада с тегом Розница (учет по отпускным ценам).";
			}
			return "Փաստաթուղթը հնարավոր չէ ստեղծել Մանրածախ (հաշվառում վաճառքի գներով) հայտանիշով պահեստի դեպքում";
		}
	}

	public static string AutoSaveInfoMessage
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The system automatically saves the information entered in the POS window to a local computer file.\r\nIf the system crashes, the saved information will be restored when the Sale window is opened for the first time.";
				}
				return "Указание параметра позволяет автоматически сохранять информацию, введенную в окне Продажа подсистемы \"Tочкa продаж (POS)\", во временный файл на компьютере.\r\nВ случае сбоя программы сохраненная информация будет восстановлена при первом открытии окна Продажи.";
			}
			return "Դրույթի նշումը թույլ է տալիս ավտոմատ կերպով պահպանել Վաճառակետ (POS) ենթահամակարգի Վաճառք պատուհանում մուտքագրված տեղեկատվությունը համակարգչի ժամանակավոր ֆայլում:\r\nԵթե ծրագրի աշխատանքը խափանվել է, ապա պահպանված տեղեկատվությունը կվերականգնվի Վաճառքի պատուհանը առաջին անգամ բացելիս:";
		}
	}

	public static string FoundAutoSavedSale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Autosave sale detected.";
				}
				return "Обнаружена автосохраненная продажа.";
			}
			return "Հայտնաբերվել է ավտոմատ պահպանված վաճառք:";
		}
	}

	public static string HappendEmergencyStopOfApplication
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The system crashed during the last operation.";
				}
				return "Во время последней операции произошел сбой системы.";
			}
			return "Վերջին գործողության ընթացքում տեղի է ունեցել ծրագրի աշխատանքի խափանում:";
		}
	}

	public static string OptionAutoSave
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Autosave document (sale, return) every";
				}
				return "Автоматически сохранять документы (продажи, возвраты) каждые";
			}
			return "Ավտոմատ պահպանել փաստաթուղթը (վաճառք, վերադարձ) յուր.";
		}
	}

	public static string PrintBarCodeN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Barcode format N";
				}
				return "Штрихкод формат N";
			}
			return "Պիտակի գծ․ կոդի ֆորմատի N";
		}
	}

	public static string PrintLabelN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print label N";
				}
				return "Этикетка печати N";
			}
			return "Պիտակի ձևանմուշի N";
		}
	}

	public static string PieceItem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Piece item";
				}
				return "Штучный товар";
			}
			return "Հատով հաշվառվող ապրանք";
		}
	}

	public static string WeighedItem
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Weight Item";
				}
				return "Весовой товар";
			}
			return "Քաշային ապրանք";
		}
	}

	public static string ShowOnlyUserAWP
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show only user navigator";
				}
				return "Показать только навигатор пользователя";
			}
			return "Ցույց տալ միայն օգտագործողի ուղղորդիչը \r\nև Վաճառակետ (POS) ենթահամակարգը";
		}
	}

	public static string ExistDiscountWithPriceType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Discount exists with {0} price type";
				}
				return "Скидка существует с типом цены \"{0}\"";
			}
			return "\"{0}\" գնի տեսակով սահմանված է զեղչ";
		}
	}

	public static string MertechDirectConnection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Mertech M-ER 727 (direct connection)";
				}
				return "Mertech M-ER 727 (прямое подключение)";
			}
			return "Mertech M-ER 727 (ուղիղ կապ)";
		}
	}

	public static string ElScalePasswordIsIncorrect
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Scale password is incorrect";
				}
				return "Неверный пароль весов";
			}
			return "Էլ. կշեռքի գաղտնաբառը սխալ է";
		}
	}

	public static string MerchantStatements
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Merchant Statements";
				}
				return "Выписка торгового эквайринга";
			}
			return "Սպասարկման կետերի քաղվածք";
		}
	}

	public static string PostingDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Posting Date";
				}
				return "Дата проводки";
			}
			return "Հաշվանցման ամսաթիվ";
		}
	}

	public static string IBTransactionDate
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transaction Date";
				}
				return "Дата сделки";
			}
			return "Գործառնության ամսաթիվ";
		}
	}

	public static string TransactionType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transaction Type";
				}
				return "Направление сдеки";
			}
			return "Գործարքի տիպ";
		}
	}

	public static string TransactionCurrency
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transaction Currency";
				}
				return "Валюта транзакции";
			}
			return "Գործարքի արժույթ";
		}
	}

	public static string TransactionAmount
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Transaction Amount";
				}
				return "Сумма транзакции";
			}
			return "Գործարքի գումար";
		}
	}

	public static string Commission
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Commission";
				}
				return "Комиссия";
			}
			return "Միջնորդավճար";
		}
	}

	public static string AmountWithoutCommision
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Amount Without Commision";
				}
				return "Сумма без комиссии";
			}
			return "Գումարն առանց միջնորդավճարի";
		}
	}

	public static string MerchantName
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Merchant Name";
				}
				return "Имя торгового эквайринга";
			}
			return "Սպասարկման կետի անվանում";
		}
	}

	public static string CardType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Card Type";
				}
				return "Тип карты";
			}
			return "Քարտի տիպ";
		}
	}

	public static string CardBelongingType
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Card Belonging Type";
				}
				return "Эмитент карты";
			}
			return "Քարտի թողարկող";
		}
	}

	public static string RRN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "RRN";
				}
				return "RRN";
			}
			return "RRN";
		}
	}

	public static string TerminalNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Terminal Number";
				}
				return "Терминал N";
			}
			return "Տերմինալի N";
		}
	}

	public static string AuthorizationCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Authorization Code";
				}
				return "Код авторизации";
			}
			return "Ավտորիզացիոն կոդ";
		}
	}

	public static string DetectedVariations
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Detected variations";
				}
				return "Обнаруженные расхождения";
			}
			return "Հայտնաբերված շեղումներ";
		}
	}

	public static string ProductDataFromB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product's data from b2b.armsoft";
				}
				return "Данные товара на b2b.armsoft";
			}
			return "Ապրանքի տվյալները b2b.armsoft հարթակում";
		}
	}

	public static string ProductVarianceCheckB2B
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product variance check (b2b.armsoft)";
				}
				return "Проверка расхождения товара (b2b.armsoft)";
			}
			return "Ապրանքների շեղման ստուգում (b2b.armsoft)";
		}
	}

	public static string VarianceFieldsSelection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Select fields to be checked for variance";
				}
				return "Выбрать поля для проверки расхождений";
			}
			return "Ընտրել շեղման ստուգման ենթակա դաշտերը";
		}
	}

	public static string IdnetifyProductsByEAN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products are identified by an EAN barcode.";
				}
				return "Товары идентифицируются EAN штрих кодом.";
			}
			return "Ապրանքները նույնականացվում են EAN գծ. կոդով:";
		}
	}

	public static string VarianceNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There's no variance";
				}
				return "Расхождений  не обнаружено.";
			}
			return "Շեղումներ չեն հայտնաբերվել";
		}
	}

	public static string TokenPan
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Apple Pay/Google Pay Number";
				}
				return "Номер Apple Pay/Google Pay";
			}
			return "Apple Pay/Google Pay համար";
		}
	}

	public static string SyncInterval
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Synchronize every (minutes)";
				}
				return "Синхронизировать каждые (минуты)";
			}
			return "Համաժամանակեցնել յուր.(րոպե)";
		}
	}

	public static string Brand
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Brand";
				}
				return "Марка";
			}
			return "Մակնիշ";
		}
	}

	public static string IncludeVATInCost
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Include VAT in cost";
				}
				return "НДС включить в себестоимость";
			}
			return "Ներառել ԱԱՀ-ն ինքնարժեքի մեջ";
		}
	}

	public static string IncludeVATInCostMustBeVAT
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "If \"{0}\" is specified, the document must be with VAT.";
				}
				return "Если указан «{0}», документ должен быть с НДС.";
			}
			return "\"{0}\" նշված լինելու դեպքում փաստաթուղթը պետք է լինի ԱԱՀ-ով";
		}
	}

	public static string CheckMarking
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check Marking";
				}
				return "Проверять заполнение маркировки";
			}
			return "Ստուգել դրոշմանիշերի լրացումը";
		}
	}

	public static string IgnoreBonus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Do not calculate bonus";
				}
				return "Do not calculate bonus";
			}
			return "Բոնուս չհաշվարկել";
		}
	}

	public static string MarkingHasMismatch
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The number of marking codes for the product with the code \"{0}\" does not correspond to the quantity of the product being sold.";
				}
				return "Количество кодов маркировки для товара с кодом «{0}» не соответствует количеству продаваемого товара.";
			}
			return "\"{0}\" կոդով ապրանքի դրոշմավորման կոդերի քանակը չի համապատասխանում վաճառվող ապրանքի քանակին։";
		}
	}

	public static string ScanDataMatrix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The product is subject to mandatory marking. Scan the product's DataMatrix code.";
				}
				return "Товар подлежит обязательной маркировке. Отсканируйте DataMatrix код товара.";
			}
			return "Ապրանքը ենթակա է պարտադիր դրոշմավորման։ Սկանավորեք ապրանքի DataMatrix ծածկագիրը։";
		}
	}

	public static string SaveAndAutoPrintECR
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Automatically print ECR check when registering a document";
				}
				return "Автоматически печатать кассовый чек при регистрации документа";
			}
			return "Փաստաթուղթը գրանցելիս ավտոմատ տպել ՀԴՄ կտրոնը";
		}
	}

	public static string SaveAndPrintECR
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Save and\r\nprint ECR check";
				}
				return "Зарегистрировать и\r\nраспечатать чек ККМ";
			}
			return "Գրանցել և\r\nտպել ՀԴՄԿ";
		}
	}

	public static string ClassifierCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are rows of material(s) with an empty CNFEA classifier.";
				}
				return "Обнаружены строки материальных ценностей с пустым классификатором ТНВЭД";
			}
			return "Առկա են դատարկ ԱՏԳԱԱ դասակարգչով ապրանք(ներ)-ի տողեր";
		}
	}

	public static string LabelUnit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unit of measure of label";
				}
				return "Единица измерения ценника";
			}
			return "Գնապիտակի չափման միավոր";
		}
	}

	public static string LabelPriceSettings
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Label Unit Price Settings (legislative change effective from 01.08.2025)";
				}
				return "Настройки цены за единицу, отображаемый на ценнике (изменение законодательства с 01.08.25)";
			}
			return "Գնապիտակում ցուցադրվող միավոր գնի կարգավորում (01․08․25-ից գործող օրենսդրական փոփոխություն)";
		}
	}

	public static string UnitPriceCalcExample
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Example of calculating unit price";
				}
				return "Пример расчета цены за единицу";
			}
			return "Միավոր գնի հաշվարկի օրինակ";
		}
	}

	public static string UnitPrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unit price";
				}
				return "Цена за единицу ";
			}
			return "Միավոր գին";
		}
	}

	public static string UnitPriceWithBrief
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unit price/1 UoM";
				}
				return "Цена за единицу/1Ед.изм.";
			}
			return "Միավորի գին/1 Չ/մ";
		}
	}

	public static string UnitPriceWithBriefWithoutRounding
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unit price/1 UoM (without rounding)";
				}
				return "Цена за единицу/1Ед.изм. (без округления)";
			}
			return "Միավորի գին/1 Չ/մ (առանց կլորացման)";
		}
	}

	public static string ViewExaple
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "View example";
				}
				return "Посмотреть пример расчета";
			}
			return "Դիտել օրինակը";
		}
	}

	public static string LabelPriceLawText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "According to Article 9.7 of the Law of the RA “On Trade and Services”, the price label must indicate the unit price of the product along with the selling price (the price is set for 1 kg, 1 l, 1 m, 1 m{0}, 1 m{1}, 1 piece).";
				}
				return "Согласно статье 9.7 закона РА «О торговле и услугах» на ценнике вместе с отпускной ценой должна быть указана цена за единицу товара (цена устанавливается за 1 кг, 1 л, 1 м, 1 м{0}, 1 м{1}, 1 шт.)";
			}
			return "\"Առևտրի և ծառայությունների մասին\" ՀՀ օրենքի 9․7 հոդվածի համաձայն գնապիտակում վաճառքի գնի հետ մեկտեղ պետք է ներկայացվի ապրանքի միավոր գինը (գինը սահմանվում է 1կգ, 1լ, 1մ, 1մ{0}, 1մ{1}, 1հատ միավորների համար)";
		}
	}

	public static string LabelPriceCalcText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Specify the price type and check the correctness of the calculation of the\r\nunit price of products on the label.";
				}
				return "Укажите тип цены и проверьте правильность расчета цены за единицу \r\nтовара на ценнике.";
			}
			return "Նշեք գնի տեսակը և ստուգեք գնապիտակում միավոր գնի\r\nհաշվարկի ճշտությունը:";
		}
	}

	public static string LabelPricePreview
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Preview of prices displayed on the label";
				}
				return "Предварительный просмотр цен, отображаемых на ценнике";
			}
			return "Գնապիտակում ցուցադրվող գների նախնական դիտում";
		}
	}

	public static string FieldEditedFromInvoicing
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The \"{0}\" has been edited when importing accounting documents";
				}
				return "\"{0}\" отредактирован при импорте Расчетных документов";
			}
			return "Խմբագրվել է \"{0}\" դաշտը Հաշվարկային փաստաթղթերի ներմուծմամբ";
		}
	}

	public static string ConfirmDeleteDataMatrix
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The scanned DataMatrix value is already in the list. Do you want to delete it?";
				}
				return "Отсканированное значение DataMatrix уже есть в списке. Хотите удалить?";
			}
			return "Սկանավորված DataMatrix-ի արժեքն առկա է ցուցակում։ Հեռացնե՞լ։";
		}
	}

	public static string ECRConfiguration
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "ECR Configuration";
				}
				return "Настройка ККМ";
			}
			return "ՀԴՄ-ի կարգավորում";
		}
	}

	public static string PosTerminalReceipt
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Pos Terminal Receipt";
				}
				return "Чек POS-терминала";
			}
			return "Պոս տերմինալի անդորրագիր";
		}
	}

	public static string Continue
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unhold";
				}
				return "Продолжить";
			}
			return "Շարունակել";
		}
	}

	public static string CheckStatus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check status";
				}
				return "Проверить статус";
			}
			return "Ստուգել կարգավիճակը";
		}
	}

	public static string CRN
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Registration N";
				}
				return "Регистрационный N";
			}
			return "Գրանցման համար";
		}
	}

	public static string EMarkReceipListJournal
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Product list of DataMatrix withdrawn from circulation";
				}
				return "Список товаров маркировки выведенных из оборота";
			}
			return "Շրջ․ դուրսգրված դրոշմանիշերի ապրանքացանկ";
		}
	}

	public static string EMarkReceiptAddText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "1․ Go to the \"Device Activation\" section in the e-Mark Receipt app.\r\n2. Scan the DataMatrix using the \"Activate Device\" command.\r\n3․ Click the ОК command in this window.";
				}
				return "1․ Перейдите в раздел \"Активация устройств\" в приложении e-Mark Receipt.\r\n2. Просканируйте изображение DataMatrix с помощью команды \"Активировать устройство\".\r\n3․ Нажмите команду «Выполнить» в этом окне. ";
			}
			return "1․ Մուտք գործեք e-Mark Receipt հավելվածի «Ավելացնել սարքեր» բաժին:\r\n2. «Ակտիվացնել սարք» հրամանով սկանավորեք պատկերված DataMatrix-ը:\r\n3․ Սեղմեք այս պատուհանի «Կատարել» հրամանը։ ";
		}
	}

	public static string EMarkReceiptApp
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "e-Mark Receipt application";
				}
				return "e-Mark Receipt приложение";
			}
			return "e-Mark Receipt հավելված";
		}
	}

	public static string EMarkReceiptButton
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "DataMatrix Withdrawal";
				}
				return "Вывод маркировки";
			}
			return "Դրոշմ․ դուրսգրում";
		}
	}

	public static string EMarkReceiptDevice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "e-Mark Receipt device";
				}
				return "e-Mark Receipt устройство";
			}
			return "e-Mark Receipt սարք";
		}
	}

	public static string EMarkReceiptDoc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "DataMatrix Withdrawal from circulation";
				}
				return "Вывод маркировки из оборота";
			}
			return "Դրոշմանիշերի շրջանառությունից դուրսգրում";
		}
	}

	public static string EMarkReceiptIntegration
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Integration with the e-Mark Receipt application";
				}
				return "Интеграция с приложением e-Mark Receipt";
			}
			return "Համակցում e-Mark Receipt հավելվածի հետ";
		}
	}

	public static string EMarkReceiptNumber
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "e-Mark Receipt IA N";
				}
				return "e-Mark Receipt ИП N";
			}
			return "e-Mark Receipt ՏՀ N";
		}
	}

	public static string ErrorsInList
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "There are errors in the list";
				}
				return "В списке обнаружены ошибки";
			}
			return "Ցուցակում առկա են սխալներ";
		}
	}

	public static string UserFieldIsEmpty
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The value of field \"{1}\" for user \"{0}\" is empty";
				}
				return "Значение поля \"{1}\" для пользователя \"{0}\" пустое.";
			}
			return "\"{0}\" օգտագործողի \"{1}\" դաշտի արժեքը դատարկ է";
		}
	}

	public static string EMarkReceiptText
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "This operation WITHDRAW marking FROM CIRCULATION before partial sale.\r\nIT IS NOT INTENDED for the sale of marked goods.";
				}
				return "Данная операция ВЫВОДИТ маркировку ИЗ ОБОРОТА до частичной продажи.\r\nОНО НЕ ПРЕДНАЗНАЧЕНА для продажи маркированного товара.";
			}
			return "Այս գործողությամբ դրոշմանիշը ԴՈՒՐՍ Է ԳՐՎՈՒՄ ՇՐՋԱՆԱՌՈՒԹՅՈՒՆԻՑ մինչև մասնակի վաճառքների\r\nիրականացումը։\r\nԱյն ՆԱԽԱՏԵՍՎԱԾ ՉԷ դրոշմանիշային ապրանքի վաճառքը գրանցելու համար։";
		}
	}

	public static string ElScaleOrTerminalGroupAndSegmentExclusion
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "You cannot fill in both the Group and Segment fields at the same time.";
				}
				return "Поля Группа и Сегмент нельзя заполнять одновременно.";
			}
			return "Խումբ և Սեգմենտ դաշտերը չեն կարող միաժամանակ լրացված լինել։";
		}
	}

	public static string ChangePrePaymentECRCheckNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Edit prepayment ECR check N";
				}
				return "Редактировать N чека ККМ предоплаты";
			}
			return "Խմբագրել կանխավճարի ՀԴՄ կտրոնի N-ը";
		}
	}

	public static string PLUCanNotBeZeroForCasCLScales
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "For CAS CL model scales the PLU code cannot be 0";
				}
				return "Для весов модели CAS CL код PLU не может быть 0";
			}
			return "CAS CL մոդելի կշեռքների դեպքում PLU կոդը չի կարող լինել 0";
		}
	}

	public static string AllowSaleWithQRCodes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Allow the use QR codes during sales";
				}
				return "Разрешить использовать QR-код во время продаж";
			}
			return "Թույլ տալ վաճառքի ժամանակ օգտագործել QR կոճակներ";
		}
	}

	public static string BonusCardIsNotAllowedInSamplePOS
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "It is not possible to select a bonus card in simple mode";
				}
				return "В простом режиме невозможно выбрать бонусную карту";
			}
			return "Պարզ ռեժիմում հնարավոր չէ ընտրել բոնուսային քարտ";
		}
	}

	public static string EMarkReceiptButtonPart1
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Mark․";
				}
				return "Марк․";
			}
			return "Դրոշմ․";
		}
	}

	public static string EMarkReceiptButtonPart2
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "withdrawal";
				}
				return "вывод";
			}
			return "դուրսգրում";
		}
	}

	public static string ExportEMarkCodes
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Export marking codes";
				}
				return "Экспортировать коды маркировки";
			}
			return "Արտահանել դրոշմավորման ծածկագրերը";
		}
	}

	public static string EMarkCodesExported
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Marking codes have been exported";
				}
				return "Коды маркировки экспортированы";
			}
			return "Դրոշմավորման ծածկագրերն արտահանված են";
		}
	}

	public static string SampleScreenMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Simple interface";
				}
				return "Простой экран";
			}
			return "Պարզ էկրան";
		}
	}

	public static string SampleScreenWithFavoritesMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Simple interface (Favorites)";
				}
				return "Простой экран (Избранные)";
			}
			return "Պարզ էկրան (Ընտրյալներ)";
		}
	}

	public static string MTListWithSalePrice
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products list (Sale price/amount)";
				}
				return "Список товаров (Цена продажи/сумма)";
			}
			return "Ապրանքների ցուցակ (Վաճառքի գին/գումար)";
		}
	}

	public static string POSWindowMinimumSizeMustBe1024
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Window POS is not supported on screens smaller than 1024";
				}
				return "Окно POS не доступно на экранах размером менее 1024";
			}
			return "ՊՈՍ պատուհանը հասանելի չէ 1024 չափից փոքր էկրանների համար";
		}
	}

	public static string EMarkExportIsNotAvailableForThisTypeOfDoc
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Marking codes for document type \"{0}\" cannot be exported.";
				}
				return "Коды маркировки для типа документа \"{0}\" не возможно экспортировать.";
			}
			return "\"{0}\" տեսակի փաստաթղթի դրոշմավորման ծածկագրերը հնարավոր չէ արտահանել:";
		}
	}

	public static string Ungrouped
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ungrouped";
				}
				return "Негруппированный";
			}
			return "Չխմբավորված";
		}
	}

	public static string BarCodeInSaleAnalysisNotExist
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Item with barcode {0} does not exist in “Sales analysis” report";
				}
				return "Товар с штрихкодом {0} отсутствует в отчете \"Анализ продаж\"";
			}
			return "{0} գծ. կոդով ապրանքը բացակայում է \"Վաճառքների վերլուծություն\" հաշվետվությունից";
		}
	}

	public static string PlaceProductOnScale
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Place the product on the scale";
				}
				return "Поместите товар на весы";
			}
			return "Տեղադրեք ապրանքը կշեռքի վրա";
		}
	}

	public static string WeightReading
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Weight reading";
				}
				return "Производится замер веса";
			}
			return "Կատարվում է քաշի ընթերցում";
		}
	}

	public static string WeightAssigned
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Weight assigned";
				}
				return "Вес присвоен";
			}
			return "Քաշը վերագրված է";
		}
	}

	public static string CheckScaleConnection
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Check scale settings";
				}
				return "Проверьте настройки весов";
			}
			return "Ստուգեք կշեռքի կարգավորումները";
		}
	}

	public static string SearchViaBarcode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Փնտրել գծիկավոր կոդով";
				}
				return "Փնտրել գծիկավոր կոդով";
			}
			return "Փնտրել գծիկավոր կոդով";
		}
	}

	public static string SearchViaProductCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Փնտրել ապրանքի կոդով";
				}
				return "Փնտրել ապրանքի կոդով";
			}
			return "Փնտրել ապրանքի կոդով";
		}
	}

	public static string SearchViaBarcodeAndProductCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Փնտրել գծիկավոր կոդով և ապրանքի կոդով";
				}
				return "Փնտրել գծիկավոր կոդով և ապրանքի կոդով";
			}
			return "Փնտրել գծիկավոր կոդով և ապրանքի կոդով";
		}
	}

	public static string ProductSearchMode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ապրանքի կամ ծառայության փնտրման ռեժիմ";
				}
				return "Ապրանքի կամ ծառայության փնտրման ռեժիմ";
			}
			return "Ապրանքի կամ ծառայության փնտրման ռեժիմ";
		}
	}

	public static string SalesDocsWithoutDocsCaption
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Վաճառքի";
				}
				return "Վաճառքի";
			}
			return "Վաճառքի";
		}
	}

	public static string TradeStorageAndOtherAccounting
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Trade, storage and other accounting";
				}
				return "Торговый, складской и прочий учет";
			}
			return "Առևտուր, պահեստ և այլ հաշվառումներ";
		}
	}

	public static string MTMoveWithConfirmation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products movement with confirmation";
				}
				return "Перемещение товаров с подтверждением";
			}
			return "Ապրանքների տեղաշարժ հաստատումով";
		}
	}

	public static string MTMoveWithEntryConfirmation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Products movement with entry confirmation";
				}
				return "Подтверждение перемещения товара при поступлении";
			}
			return "Ապրանքների տեղաշարժ մուտքի հաստատումով";
		}
	}

	public static string EntryConfirmation
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Entry confirmation";
				}
				return "Подтверждение приема";
			}
			return "Մուտքագրման հաստատում";
		}
	}

	public static string Status
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Status";
				}
				return "Статус";
			}
			return "Կարգավիճակ";
		}
	}

	public static string ShowStocktakingColumns
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Show stocktaking columns";
				}
				return "Показать колонки инвентаризации ";
			}
			return "Ցույց տալ գույքագրման սյուները";
		}
	}

	public static string OptionCannotBeDeactivatedError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "\"{0}\" դրոշակը հնարավոր չէ ապաակտիվացնել, քանի որ պահոցում առկա են այդ տիպի փաստաթղթեր:";
				}
				return "\"{0}\" դրոշակը հնարավոր չէ ապաակտիվացնել, քանի որ պահոցում առկա են այդ տիպի փաստաթղթեր:";
			}
			return "\"{0}\" դրոշակը հնարավոր չէ ապաակտիվացնել, քանի որ պահոցում առկա են այդ տիպի փաստաթղթեր:";
		}
	}

	public static string GoodsOnTheWay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Goods on the way";
				}
				return "Товары в пути";
			}
			return "Ապրանքներ ճանապարհին";
		}
	}

	public static string InTransit
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "In transit";
				}
				return "В пути";
			}
			return "Փոխադրման մեջ";
		}
	}

	public static string Completed
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Completed";
				}
				return "Завершенный";
			}
			return "Ավարտված";
		}
	}

	public static string MTMoveStatusOnWay
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "The products are on the way";
				}
				return "Товары в пути";
			}
			return "Ապրանքները գտնվում են ճանապարհին";
		}
	}

	public static string MTMoveStatusCompleted
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Movement completed";
				}
				return "Перемещение завершено";
			}
			return "Տեղաշարժը կատարված է";
		}
	}

	public static string UserCannotConfirmMTMove
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "{0} user does not have permission to change confirmation status";
				}
				return "{0} пользователь не имеет полномочия на изменение статуса подтверждения";
			}
			return "{0} օգտագործողը իրավասու չէ փոխել մուտքի հաստատման կարգավիճակը";
		}
	}

	public static string LegalStatus
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Legal Status";
				}
				return "Юр. статус";
			}
			return "Իրավ. կարգավիճակ";
		}
	}

	public static string ECRBasePrePaymentCheckNum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Base prepayment ECR check N";
				}
				return "N базового чека аванса ККМ";
			}
			return "Հիմք կանխավճարի ՀԴՄ կտրոնի N";
		}
	}

	public static string PrePaymentECRCheck
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment ECR Check";
				}
				return "Կանխավճարի ՀԴՄ կտրոն";
			}
			return "Կանխավճարի ՀԴՄ կտրոն";
		}
	}

	public static string PrepaymentOutByCard
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment output (Non-cash)";
				}
				return "Списание аванса (Безналичные)";
			}
			return "Կանխավճարի ելք (անկանխիկ)";
		}
	}

	public static string PrepaymentOutCash
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment output (cash)";
				}
				return "Списание аванса (наличные)";
			}
			return "Կանխավճարի ելք (կանխիկ)";
		}
	}

	public static string PrepaymentTotalIn
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment (total)";
				}
				return "Аванс (итого)";
			}
			return "Կանխավճարի մուտք (ընդհանուր)";
		}
	}

	public static string PrepaymentTotalOut
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Prepayment output (total)";
				}
				return "Списание аванса (итого)";
			}
			return "Կանխավճարի ելք (ընդհանուր)";
		}
	}

	public static string CancelPaymentQuestion
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Ուշադրություն։ ՀԴՄ կտրոնը չի տպվել, ցանկանու՞մ եք չեղարկել POS տերմինալով կատարված անկանխիկ վճարումը։";
				}
				return "Ուշադրություն։ ՀԴՄ կտրոնը չի տպվել, ցանկանու՞մ եք չեղարկել POS տերմինալով կատարված անկանխիկ վճարումը։";
			}
			return "Ուշադրություն։ ՀԴՄ կտրոնը չի տպվել, ցանկանու՞մ եք չեղարկել POS տերմինալով կատարված անկանխիկ վճարումը։";
		}
	}

	public static string PrintPrepaymentCheckSeparately
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Print prepayment ECR check separately";
				}
				return "Печатать чек ККМ \u200b\u200bо предоплате отдельно";
			}
			return "Կտրոնի տպիչով կանխավճարի\r\nՀԴՄ կտրոնը տպել առանձին";
		}
	}

	public static string FillByStockTakingResults
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Fill in the quantities, taking into account the inventory discrepancy:";
				}
				return "Заполнить количество, учитывая отклонение в запасах:";
			}
			return "Լրացնել քանակները՝ հաշվի առնելով գույքագրման տարբերությունը.";
		}
	}

	public static string ReturnByCreditSumGreaterThanBaseDocByCreditSum
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Returning credit amount is greater than credit amount in base document";
				}
				return "Сумма возврата кредитной оплаты больше суммы кредитной оплаты документа основания";
			}
			return "Վերադարձվող ապառիկ գումարը մեծ է հիմք փաստաթղթի ապառիկ գումարից";
		}
	}
}
