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
	public class PawnColumnWorker_RoomAdjustmentOfWhore : PawnColumnWorker_TextCenter
	{
		protected override string GetTextFor(Pawn pawn)
		{
			float score = GetValueToCompare(pawn);
			Room ownedRoom = pawn.ownership.OwnedRoom;
			int scoreStageIndex;
			string scoreStageName;
			if (ownedRoom == null)
			{
				scoreStageName = "No room";
				scoreStageIndex = 0;
			}
			else
			{
				scoreStageName = RoomStatDefOf.Impressiveness.GetScoreStage(ownedRoom.GetStat(RoomStatDefOf.Impressiveness)).label;
				scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness));
			}
			return string.Format("{0} ({1})", scoreStageName, score.ToStringPercent());
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
		}

		private float GetValueToCompare(Pawn pawn)
		{
			return WhoringHelper.WhoreRoomAdjustment(pawn);
		}
	}
}
