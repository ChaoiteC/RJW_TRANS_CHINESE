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
	public class PawnColumnWorker_Mood : PawnColumnWorker_TextCenter
	{
		protected override string GetTextFor(Pawn pawn)
		{
			return GetValueToCompare(pawn).ToStringPercent();
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
		}

		private float GetValueToCompare(Pawn pawn)
		{
			return pawn.needs.mood.CurLevelPercentage;
		}
	}
}
