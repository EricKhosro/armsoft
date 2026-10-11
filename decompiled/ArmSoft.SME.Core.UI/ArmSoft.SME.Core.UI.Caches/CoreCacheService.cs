using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.Caches;

public class CoreCacheService
{
	public BankCache Bank { get; } = new BankCache();

	public CurrencyCache Currency { get; } = new CurrencyCache();

	public TemplateCache Template { get; } = new TemplateCache();

	public CPACodeCache CPACode { get; } = new CPACodeCache();

	public UserCache User { get; } = new UserCache();

	public TreeCache Tree { get; } = new TreeCache();

	public StringResourceCache StringResources { get; } = new StringResourceCache();

	public DocumentExtensionsCache DocumentExtensions { get; } = new DocumentExtensionsCache();

	public Dictionary<short, TreeNodesCache> TreeNodes { get; } = new Dictionary<short, TreeNodesCache>();

	public CurrentExchangeRatesCache CurrentExchangeRates { get; } = new CurrentExchangeRatesCache();

	public void Clear()
	{
		Bank.Clear();
		Currency.Clear();
		Template.Clear();
		CPACode.Clear();
		User.Clear();
		Tree.Clear();
		TreeNodes.Clear();
		StringResources.Clear();
		DocumentExtensions.Clear();
		CurrentExchangeRates.Clear();
	}
}
