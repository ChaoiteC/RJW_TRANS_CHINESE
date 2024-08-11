using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using Multiplayer.API;

namespace rjw
{
	public class JobGiver_Masturbate : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			//--ModLog.Message(" JobGiver_Masturbate::TryGiveJob( " + xxx.get_pawnname(pawn) + " ) called");

			if (pawn.Drafted) return null;

			if (!xxx.can_be_fucked(pawn) && !xxx.can_fuck(pawn)) return null;

			// Whores only fap if frustrated, unless imprisoned.
			if ((SexUtility.ReadyForLovin(pawn) && (!xxx.is_whore(pawn) || pawn.IsPrisoner || xxx.is_slave(pawn))) || xxx.is_frustrated(pawn))
			{
				if (RJWPreferenceSettings.FapInBed && pawn.jobs.curDriver is JobDriver_LayDown)
				{
					Building_Bed bed = ((JobDriver_LayDown)pawn.jobs.curDriver).Bed;

					if (bed != null)
					{
						if ((xxx.is_frustrated(pawn) || xxx.has_quirk(pawn, "Exhibitionist")) || bed.GetRoom().Role == RoomRoleDefOf.Bedroom || bed.GetRoom().Role == RoomRoleDefOf.PrisonCell)
							return JobMaker.MakeJob(xxx.Masturbate, null, bed, bed.Position);
					}
				}
				else if (RJWPreferenceSettings.FapEverywhere && (xxx.is_frustrated(pawn) || xxx.has_quirk(pawn, "Exhibitionist")))
				{
					return JobMaker.MakeJob(xxx.Masturbate, null, null, CasualSex_Helper.FindSexLocation(pawn));
				}
			}
			return null;
		}
	}
}