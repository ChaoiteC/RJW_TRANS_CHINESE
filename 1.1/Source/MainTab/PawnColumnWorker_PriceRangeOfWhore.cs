using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace rjw.MainTab
{
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_PriceRangeOfWhore : PawnColumnWorker_TextCenter
	{
		protected internal int min;
		protected internal int max;

		protected override string GetTextFor(Pawn pawn)
		{
			min = WhoringHelper.WhoreMinPrice(pawn);
			max = WhoringHelper.WhoreMaxPrice(pawn);
			return string.Format("{0} - {1}", min, max);
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
		}

		protected override string GetTip(Pawn pawn)
		{
			string minPriceTip = string.Format(
				"  Base: {0}\n  Traits: {1}",
				WhoringHelper.baseMinPrice,
				(WhoringHelper.WhoreTraitAdjustmentMin(pawn) -1f).ToStringPercent()
			);
			string maxPriceTip = string.Format(
				"  Base: {0}\n  Traits: {1}",
				WhoringHelper.baseMaxPrice,
				(WhoringHelper.WhoreTraitAdjustmentMax(pawn) -1f).ToStringPercent()
			);
			string bothTip = string.Format(
				"  Gender: {0}\n  Age: {1}\n  Injuries: {2}\n  Room: {3}",
				(WhoringHelper.WhoreGenderAdjustment(pawn) - 1f).ToStringPercent(),
				(WhoringHelper.WhoreAgeAdjustment(pawn) - 1f).ToStringPercent(),
				(WhoringHelper.WhoreInjuryAdjustment(pawn) - 1f).ToStringPercent(),
				(WhoringHelper.WhoreRoomAdjustment(pawn) - 1f).ToStringPercent()
			);
			return string.Format("Min:\n{0}\nMax:\n{1}\nBoth:\n{2}", minPriceTip, maxPriceTip, bothTip);
		}

		private int GetValueToCompare(Pawn pawn)
		{
			return min;
		}
	}
}
