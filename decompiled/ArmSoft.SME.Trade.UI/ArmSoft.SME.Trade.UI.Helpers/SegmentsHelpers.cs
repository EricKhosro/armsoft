using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Helpers;

public static class SegmentsHelpers
{
	public static bool IsGoodInMySegment(this DiscountCacheRow discount, int goodId, List<GoodsBySegmentDataProvider.DataRow> segmentsProviderRows)
	{
		if (!string.IsNullOrWhiteSpace(discount.SegmentCode))
		{
			return IsGoodInSegment(goodId, discount.SegmentCode, discount.SegmentNegation, segmentsProviderRows);
		}
		return true;
	}

	public static bool IsGoodInSegment(int materialId, string segmentCode, bool segmentNegation, List<GoodsBySegmentDataProvider.DataRow> segmentsProviderRows)
	{
		bool flag = segmentsProviderRows.Any((GoodsBySegmentDataProvider.DataRow x) => x.MTID == materialId && x.SegmentCode == segmentCode);
		if (!segmentNegation)
		{
			return flag;
		}
		return !flag;
	}
}
