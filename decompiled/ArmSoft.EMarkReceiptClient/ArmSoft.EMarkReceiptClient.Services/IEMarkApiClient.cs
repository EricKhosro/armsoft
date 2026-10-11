using System;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.EMarkReceiptClient.Models;

namespace ArmSoft.EMarkReceiptClient.Services;

public interface IEMarkApiClient
{
	Task<DatamatrixResponse> GetDatamatrixAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<ActivationStatusResponse> CheckActivationAsync(bool updateTokens = true, CancellationToken cancellationToken = default(CancellationToken));

	Task<RefreshTokenResponse> GetRefreshTokenAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<bool> UploadMetadataAsync(Guid id, string name, byte[] iconBytes, string iconName, CancellationToken cancellationToken = default(CancellationToken));

	Task<PrintResponse> PrintAsync(PrintRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetReceiptResponse> GetReceiptAsync(GetReceiptRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<FindReceiptByCodeResponse> FindReceiptByCodeAsync(FindReceiptByCodeRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<ReturnReceiptResponse> ReturnReceiptAsync(ReturnReceiptRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
