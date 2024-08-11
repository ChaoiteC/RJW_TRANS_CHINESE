using System;
using Verse;
using System.Linq;
using RimWorld;

namespace rjw
{
	public class BedData : IExposable
	{
		public Building_Bed bed = null;
		public bool allowedForWhoringOwner = true;
		public bool allowedForWhoringAll = false;
		public int reservedUntilGameTick = 0;
		public int reservedForPawnID = 0;

		public int lastScoreUpdateTick = -70; // GenTicks.TicksGame
		public float bedScore = -1f;
		public int scoreUpdateTickDelay = 60;

		public BedData() { }
		public BedData(Building_Bed bed)
		{
			this.bed = bed;
		}

		public void ExposeData()
		{
			Scribe_References.Look(ref bed, "Bed");
			Scribe_Values.Look(ref allowedForWhoringOwner, "allowedForWhoringOwner", true, true);
			Scribe_Values.Look(ref allowedForWhoringAll, "allowedForWhoringAll", false, true);
			Scribe_Values.Look(ref reservedUntilGameTick, "lastUsed", 0, true);
			Scribe_Values.Look(ref reservedForPawnID, "lastUsedBy", 0, true);
		}

		public bool IsValid { get { return bed != null; } }
	}
}
