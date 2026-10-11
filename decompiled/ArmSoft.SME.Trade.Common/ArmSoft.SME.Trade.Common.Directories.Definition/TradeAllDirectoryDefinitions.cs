using ArmSoft.SME.Common.Directories;

namespace ArmSoft.SME.Trade.Common.Directories.Definition;

public class TradeAllDirectoryDefinitions : AllDirectoryDefinitions
{
	public static TradeDirectoryDefinition MTGroup => TradeDefinitionCreation.CreateMTGroup();

	public static TradeDirectoryDefinition Storage => TradeDefinitionCreation.CreateStorage();

	public static TradeDirectoryDefinition PartnerGroup => TradeDefinitionCreation.CreatePartnerGroup();

	public static TradeDirectoryDefinition Cash => TradeDefinitionCreation.CreateCash();

	public static TradeDirectoryDefinition AccountInBank => TradeDefinitionCreation.CreateAccountInBank();

	public static TradeDirectoryDefinition Country => TradeDefinitionCreation.CreateCountry();

	public static TradeDirectoryDefinition StandardComment => TradeDefinitionCreation.CreateStdCom();

	public static TradeDirectoryDefinition Producer => TradeDefinitionCreation.CreateProducer();

	public static TradeDirectoryDefinition DiscountGroup => TradeDefinitionCreation.CreateDiscountGroup();

	public static TradeDirectoryDefinition PropertyList => TradeDefinitionCreation.CreatePropertyList();

	public static TradeDirectoryDefinition Property => TradeDefinitionCreation.CreateProperty();

	public static TradeDirectoryDefinition Price => TradeDefinitionCreation.CreatePrice();

	public static TradeDirectoryDefinition PriceType => TradeDefinitionCreation.CreatePriceType();

	public static TradeDirectoryDefinition QntUnit => TradeDefinitionCreation.CreateQntUnit();

	public static TradeDirectoryDefinition Segment => TradeDefinitionCreation.CreateSegment();

	public static TradeDirectoryDefinition BonusScale => TradeDefinitionCreation.CreateBonusScale();

	public static TradeDirectoryDefinition Good => TradeDefinitionCreation.CreateGood();

	public static TradeDirectoryDefinition MTQntUnit => TradeDefinitionCreation.CreateMTQntUnit();

	public static TradeDirectoryDefinition MTComplects => TradeDefinitionCreation.CreateMTComplects();

	public static TradeDirectoryDefinition Contract => TradeDefinitionCreation.CreateContract();

	public static TradeDirectoryDefinition PartnerAccountInBank => TradeDefinitionCreation.CreatePartnerAccountInBank();

	public static TradeDirectoryDefinition GoodsBySegment => TradeDefinitionCreation.CreateGoodsBySegment();

	public static TradeDirectoryDefinition ItemBarCodes => TradeDefinitionCreation.CreateItemBarCodes();

	public static TradeDirectoryDefinition OrderMinMax => TradeDefinitionCreation.CreateOrderMinMax();

	public static TradeDirectoryDefinition SubstituteItemsGroups => TradeDefinitionCreation.CreateSubstituteItemsGroups();

	public static TradeDirectoryDefinition MTFavoriteMaterial => TradeDefinitionCreation.CreateMTFavoriteMaterial();

	public static TradeDirectoryDefinition Employee => TradeDefinitionCreation.CreateEmployee();

	public static TradeDirectoryDefinition MTFavoritesGroup => TradeDefinitionCreation.CreateMTFavoritesGroup();

	public static TradeDirectoryDefinition Partner => TradeDefinitionCreation.CreatePartner();

	public static TradeDirectoryDefinition PartnersGood => TradeDefinitionCreation.CreatePartnersGood();

	public static TradeDirectoryDefinition CashierShift => TradeDefinitionCreation.CreateCashierShift();

	public static TradeDirectoryDefinition Discount => TradeDefinitionCreation.CreateDiscount();

	public static TradeDirectoryDefinition UserCashesAccess => TradeDefinitionCreation.CreateUserCashesAccess();

	public static TradeDirectoryDefinition UserStorageOptions => TradeDefinitionCreation.CreateUserStorageOptions();

	public static TradeDirectoryDefinition ElScaleOrTerminal => TradeDefinitionCreation.CreateElScaleOrTerminal();

	public static TradeDirectoryDefinition EMarkReceipt => TradeDefinitionCreation.CreateEMarkReceipt();
}
