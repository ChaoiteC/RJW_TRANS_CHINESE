using RimWorld;
using Verse;
using Verse.AI;

namespace rjw
{
	public class JobGiver_JoinInBed : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!RJWHookupSettings.HookupsEnabled)
				return null;

			if (pawn.Drafted)
				return null;

			if (!SexUtility.ReadyForHookup(pawn))
				return null;

			// We increase the time right away to prevent the fairly expensive check from happening too frequently
			SexUtility.IncreaseTicksToNextHookup(pawn);

			// If the pawn is a whore, or recently had sex, skip the job unless they're really horny
			if (!xxx.is_frustrated(pawn) && (xxx.is_whore(pawn) || !SexUtility.ReadyForLovin(pawn)))
				return null;

			// This check attempts to keep groups leaving the map, like guests or traders, from turning around to hook up
			if (pawn.mindState?.duty?.def == DutyDefOf.TravelOrLeave)
			{
				// TODO: Some guest pawns keep the TravelOrLeave duty the whole time, I think the ones assigned to guard the pack animals.
				// That's probably ok, though it wasn't the intention.
				if (RJWSettings.DebugLogJoinInBed) ModLog.Message($"JoinInBed.TryGiveJob:({xxx.get_pawnname(pawn)}): has TravelOrLeave, no time for lovin!");
				return null;
			}

			if (pawn.CurJob == null || pawn.CurJob.def == JobDefOf.LayDown)
			{
				//--Log.Message("   checking pawn and abilities");
				if (CasualSex_Helper.CanHaveSex(pawn))
				{
					//--Log.Message("   finding partner");
					Pawn partner = CasualSex_Helper.find_partner(pawn, pawn.Map, true);

					//--Log.Message("   checking partner");
					if (partner == null)
						return null;

					// Can never be null, since find checks for bed.
					Building_Bed bed = partner.CurrentBed();

					// Interrupt current job.
					if (pawn.CurJob != null && pawn.jobs.curDriver != null)
						pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);

					//--Log.Message("   returning job");
					return JobMaker.MakeJob(xxx.casual_sex, partner, bed);
				}
			}

			return null;
		}
	}
}
