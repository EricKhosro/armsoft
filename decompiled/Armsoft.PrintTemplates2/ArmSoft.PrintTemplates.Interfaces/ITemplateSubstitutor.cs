using System.IO;
using System.Threading.Tasks;

namespace ArmSoft.PrintTemplates.Interfaces;

public interface ITemplateSubstitutor
{
	IPrintTemplateSubstitution PrintTemplateSubstitution { get; }

	Stream GetStream(byte[] file);

	Task<Stream> GetStreamAsync(byte[] file);

	void SaveToFile(string file);

	Task SaveToFileAsync(string file);
}
