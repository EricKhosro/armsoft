using ArmSoft.SME.Common.Directories.Definition;

namespace ArmSoft.SME.Common.Directories;

public class AllDirectoryDefinitions
{
	public static DirectoryDefinition Currency => DefinitionCreation.CreateCurrency();

	public static DirectoryDefinition RatesOfExchange => DefinitionCreation.CreateRatesOfExchange();

	public static DirectoryDefinition Bank => DefinitionCreation.CreateBank();

	public static DirectoryDefinition CPAClassifier => DefinitionCreation.CreateCPACodes();
}
