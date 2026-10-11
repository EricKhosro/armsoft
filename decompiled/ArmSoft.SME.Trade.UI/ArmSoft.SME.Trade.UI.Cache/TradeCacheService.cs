namespace ArmSoft.SME.Trade.UI.Cache;

public class TradeCacheService
{
	public AccountInBankCache AccountInBank { get; } = new AccountInBankCache();

	public CashCache Cash { get; } = new CashCache();

	public CashierShift CashierShift { get; } = new CashierShift();

	public CountryCache Country { get; } = new CountryCache();

	public StandardCommentsCache StandardComments { get; } = new StandardCommentsCache();

	public ProducerCache Producer { get; } = new ProducerCache();

	public SubstituteItemsGroupCache SubstituteItemsGroup { get; } = new SubstituteItemsGroupCache();

	public StorageCache Storage { get; } = new StorageCache();

	public EmployeeCache Employee { get; } = new EmployeeCache();

	public MTGroupCache MTGroup { get; } = new MTGroupCache();

	public MaterialCache Material { get; } = new MaterialCache();

	public MaterialExtensionsCache MaterialExtensions { get; } = new MaterialExtensionsCache();

	public PartnerGoodCache PartnerGood { get; } = new PartnerGoodCache();

	public PartnerGroupCache PartnerGroup { get; } = new PartnerGroupCache();

	public PriceTypeCache PriceType { get; } = new PriceTypeCache();

	public PropertyListCache PropertyList { get; } = new PropertyListCache();

	public PropertyCache Property { get; } = new PropertyCache();

	public QntUnitCache QntUnit { get; } = new QntUnitCache();

	public PartnerCache Partner { get; } = new PartnerCache();

	public PartnerExtensionsCache PartnerExtensions { get; } = new PartnerExtensionsCache();

	public DiscountCache Discount { get; } = new DiscountCache();

	public DiscountGroupCache DiscountGroup { get; } = new DiscountGroupCache();

	public SegmentCache Segment { get; } = new SegmentCache();

	public MTFavoriteGroupCache MTFavoriteGroup { get; } = new MTFavoriteGroupCache();

	public MTFavoriteCache MTFavorite { get; } = new MTFavoriteCache();

	public CurrentPricesCache CurrentPrices { get; } = new CurrentPricesCache();

	public void Clear()
	{
		AccountInBank.Clear();
		Cash.Clear();
		CashierShift.Clear();
		Country.Clear();
		StandardComments.Clear();
		Producer.Clear();
		SubstituteItemsGroup.Clear();
		Storage.Clear();
		Employee.Clear();
		MTGroup.Clear();
		Material.Clear();
		MaterialExtensions.Clear();
		PartnerGood.Clear();
		PartnerGroup.Clear();
		PriceType.Clear();
		PropertyList.Clear();
		Property.Clear();
		QntUnit.Clear();
		Partner.Clear();
		PartnerExtensions.Clear();
		Discount.Clear();
		DiscountGroup.Clear();
		Segment.Clear();
		MTFavoriteGroup.Clear();
		MTFavorite.Clear();
		CurrentPrices.Clear();
	}
}
