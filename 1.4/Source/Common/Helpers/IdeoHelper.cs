using System.Collections.Generic;
using Verse;
using RimWorld;

using static rjw.PreceptDefOf;

namespace rjw
{
	public static class IdeoHelper
	{
		public static readonly List<PreceptDef> freeLovePrecepts;

		public static readonly List<PreceptDef> nonPolyPrecepts;

		public static readonly List<PreceptDef> prudePrecepts;

		public static bool ClassicMode => !ModsConfig.IdeologyActive || Find.IdeoManager.classicMode;

		static IdeoHelper()
		{
			if (ModsConfig.IdeologyActive)
			{	
				freeLovePrecepts = new() { Lovin_Free, Lovin_FreeApproved };
				nonPolyPrecepts = new() {
					Lovin_SpouseOnly_Mild,
					Lovin_SpouseOnly_Moderate,
					Lovin_SpouseOnly_Strict
				};
				prudePrecepts = new() { Lovin_Prohibited, Lovin_Horrible };

			}
			else
			{
				freeLovePrecepts = new() { Lovin_Free };
			}
		}

		/// <summary>
		/// Check whether pawn has an ideo with a free love precept.
		/// </summary>
		/// <param name="pawn">The pawn whose ideo is being checked</param>
		/// <param name="allowClassic">
		/// 	Whether the hidden 'Lovin: Free' precept included in classic and non-DLC ideos should be respected.<br />
		/// 	In other words, whether we are strong enough to accept canon or cling fearfully to our silly 21st century mores.
		/// </param>
		/// <returns>True if etc etc, false otherwise</returns>
		public static bool IsIdeologicallyPoly(this Pawn pawn, bool allowClassic = true)
		{
			if (pawn.Ideo == null)
			{
				return false;
			}
			if (ClassicMode)
			{
				return allowClassic;
			}

			var lovinPrecept = GetLovinPreceptDef(pawn.Ideo);
			if (lovinPrecept == null)
			{
				return false;
			}
			return freeLovePrecepts.Contains(lovinPrecept);
		}

		public static bool IsIdeologicallyPrudish(this Pawn pawn)
		{
			if (pawn.Ideo == null || ClassicMode)
			{
				return false;
			}

			var lovinPrecept = GetLovinPreceptDef(pawn.Ideo);
			if (lovinPrecept == null)
			{
				return false;
			}

			return prudePrecepts.Contains(lovinPrecept);
		}

		private static PreceptDef GetLovinPreceptDef(Ideo ideo)
		{
			return ideo.PreceptsListForReading.FirstOrDefault(p => p.def.issue == IssueDefOf.Lovin)?.def;
		}
	}
}