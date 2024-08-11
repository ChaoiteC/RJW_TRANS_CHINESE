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
	public class PawnColumnWorker_AverageMoneyByWhore : PawnColumnWorker_TextCenter
	{
		protected override string GetTextFor(Pawn pawn)
		{
			return ((int)GetValueToCompare(pawn)).ToString();
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
		}

		private float GetValueToCompare(Pawn pawn)
		{
			float total = pawn.records.GetValue(xxx.EarnedMoneyByWhore);
			float count = pawn.records.GetValue(xxx.CountOfWhore);
			if ((int)count == 0)
			{
				return 0;
			}
			return (total / count);
		}
	}
}
