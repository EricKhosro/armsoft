using System.IO;
using System.Threading.Tasks;

namespace ArmSoft.PrintTemplates;

public interface ITemplateMerger
{
	void MergeFile(string targetFile, string sourceFile, bool insertPageBreak);

	Task MergeFileAsync(string targetFile, string sourceFile, bool insertPageBreak);

	Stream MergeFile(byte[] targetFile, byte[] sourceFile, bool insertPageBreak);

	Task<Stream> MergeFileAsync(byte[] targetFile, byte[] sourceFile, bool insertPageBreak);
}
