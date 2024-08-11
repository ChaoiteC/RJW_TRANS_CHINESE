using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace rjw
{
	public class JobGiver_WhoreInvitingVisitors : ThinkNode_JobGiver
	{
		// Checks if pawn has a memory. 
		// Maybe not the best place for function, might be useful elsewhere too.
		public static bool MemoryChecker(Pawn pawn, ThoughtDef thought)
		{
			Thought_Memory val = pawn.needs.mood.thoughts.memories.Memories.Find((Thought_Memory x) => (object)x.def == thought);
			return val == null ? false : true;
		}

		[SyncMethod]
		private static bool Roll_to_skip(Pawn client, Pawn whore)
		{
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			float fuckability = SexAppraiser.would_fuck(client, whore); // 0.0 to 1.

			// More likely to skip other whores, because they're supposed to be working.
			if (client.IsDesignatedService())
				fuckability *= 0.6f;
			return fuckability >= 0.1f && xxx.need_some_sex(client) >= 1f && Rand.Chance(0.5f);
		}

		/*
		public static Pawn Find_pawn_to_fuck(Pawn whore, Map map)
		{
			Pawn best_fuckee = null;
			float best_distance = 1.0e6f;
			foreach (Pawn q in map.mapPawns.FreeColonists)
				if ((q != whore) &&
					xxx.need_some_sex(q)>0 &&
					whore.CanReserve(q, 1, 0) &&
					q.CanReserve(whore, 1, 0) &&
					Roll_to_skip(whore, q) &&
					(!q.Position.IsForbidden(whore)) &&
					xxx.is_healthy(q))
				{
					var dis = whore.Position.DistanceToSquared(q.Position);
					if (dis < best_distance)
					{
						best_fuckee = q;
						best_distance = dis;
					}
				}
			return best_fuckee;
		}
		*/

		private sealed class FindAttractivePawnHelper
		{
			internal Pawn whore;

			internal bool TraitCheckFail(Pawn client)
			{
				if (!xxx.is_human(client))
					return true;
				if (!xxx.has_traits(client))
					return true;
				if (!(xxx.can_fuck(client) || xxx.can_be_fucked(client)) || !xxx.IsTargetPawnOkay(client))
					return true;

				//Log.Message("client:" + client + " whore:" + whore);
				if (CompRJW.CheckPreference(client, whore) == false)
					return true;
				return false; // Everything ok.
			}

			//Use this check when client is not in the same faction as the whore
			internal bool FactionCheckPass(Pawn client)
			{
				return ((client.Map == whore.Map) && (client.Faction != null && client.Faction != whore.Faction) && !client.IsPrisoner && !client.HostileTo(whore));
			}

			//Use this check when client is in the same faction as the whore
			[SyncMethod]
			internal bool RelationCheckPass(Pawn client)
			{
				//Rand.PopState();
				//Rand.PushState(RJW_Multiplayer.PredictableSeed());
				if (xxx.isSingleOrPartnerNotHere(client) || xxx.is_lecher(client) || Rand.Value < 0.9f)
				{
					if (client != LovePartnerRelationUtility.ExistingLovePartner(whore))
					{ //Exception for prisoners to account for PrisonerWhoreSexualEmergencyTree, which allows prisoners to try to hook up with anyone who's around (mostly other prisoners or warden)
						return (client != whore) & (client.Map == whore.Map) && (client.Faction == whore.Faction || whore.IsPrisoner) && (client.IsColonist || whore.IsPrisoner) && WhoringHelper.IsHookupAppealing(whore, client);
					}
				}
				return false;
			}
		}

		[SyncMethod]
		public static Pawn FindAttractivePawn(Pawn whore, out int price)
		{
			price = 0;
			if (whore == null || xxx.is_asexual(whore))
			{
				if (RJWSettings.DebugWhoring) ModLog.Message($" {xxx.get_pawnname(whore)} is asexual, abort");
				return null;
			}
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());

			FindAttractivePawnHelper client = new FindAttractivePawnHelper
			{
				whore = whore
			};
			price = WhoringHelper.PriceOfWhore(whore);
			int priceOfWhore = price;

			IntVec3 pos = whore.Position;

			IEnumerable<Pawn> potentialClients = whore.Map.mapPawns.AllPawnsSpawned;
			potentialClients = potentialClients.Where(x
				=> x != whore
				&& !x.IsForbidden(whore)
				&& !x.HostileTo(whore)
				&& !x.IsPrisoner
				&& x.Position.DistanceTo(pos) < 100 
				&& xxx.is_healthy_enough(x));

				
			potentialClients = potentialClients.Except(potentialClients.Where(client.TraitCheckFail));

			if (!whore.IsPrisoner)
				potentialClients = potentialClients.Except(potentialClients.Where(x => !whore.CanReserveAndReach(x, PathEndMode.ClosestTouch, Danger.Some, 1)));
			else
				potentialClients = potentialClients.Except(potentialClients.Where(x => !x.CanReserveAndReach(whore, PathEndMode.ClosestTouch, Danger.Some, 1)));

			if (!potentialClients.Any()) return null;

			if (RJWSettings.DebugWhoring) ModLog.Message($" FindAttractivePawn number of all potential clients {potentialClients.Count()}");

			List<Pawn> valid_targets = new List<Pawn>();

			if (!whore.IsPrisoner)
				foreach (Pawn target in potentialClients)
				{
					if(xxx.cells_to_target_casual(whore, target.Position))
						if (xxx.can_path_to_target(whore, target.Position))
							valid_targets.Add(target);
				}
			else
				foreach (Pawn target in potentialClients)
				{
					if (xxx.cells_to_target_casual(target, whore.Position))
						if (xxx.can_path_to_target(target, whore.Position))
							valid_targets.Add(target);
				}

			if (RJWSettings.DebugWhoring) ModLog.Message($" number of reachable clients {valid_targets.Count()}");


			//IEnumerable<Pawn> guestsSpawned = valid_targets.Where(x => x.Faction != whore.Faction
			//	&& WhoringHelper.CanAfford(x, whore, priceOfWhore));
			
			//if (RJWSettings.DebugWhoring) ModLog.Message($" number of clients can afford {guestsSpawned.Count()}");

			//guestsSpawned = valid_targets.Where(x => x.Faction != whore.Faction
			//	&& x != LovePartnerRelationUtility.ExistingLovePartner(whore));

			//if (RJWSettings.DebugWhoring) ModLog.Message($" number of relations OK {guestsSpawned.Count()}");
			//guestsSpawned = valid_targets.Where(x => x.Faction != whore.Faction
			//	&& !MemoryChecker(x, ThoughtDef.Named("RJWFailedSolicitation")));

			//if (RJWSettings.DebugWhoring) ModLog.Message($" number of clients can memory OK {guestsSpawned.Count()}");

			IEnumerable<Pawn> guestsSpawned = valid_targets.Where(x => x.Faction != whore.Faction
				&& !MemoryChecker(x, ThoughtDef.Named("RJWFailedSolicitation"))
				&& WhoringHelper.CanAfford(x, whore, priceOfWhore)
				&& x != LovePartnerRelationUtility.ExistingLovePartner(whore));

			if (guestsSpawned.Any())
			{
				if (RJWSettings.DebugWhoring) ModLog.Message($" number of all acceptable Guests {guestsSpawned.Count()}");
				return guestsSpawned.RandomElement();
			}

			return null;

			//use casual sex for colonist hooking
			if (RJWSettings.DebugWhoring) ModLog.Message($" found no guests, trying colonists");

			if (!WhoringHelper.WillPawnTryHookup(whore))// will hookup colonists?
			{
				return null;
			}
			IEnumerable<Pawn> freeColonists = valid_targets.Where(x => x.Faction == whore.Faction
				&& Roll_to_skip(x, whore));

			if (RJWSettings.DebugWhoring) ModLog.Message($" number of free colonists {freeColonists.Count()}");

			freeColonists = freeColonists.Where(x => client.RelationCheckPass(x) && !MemoryChecker(x, ThoughtDef.Named("RJWTurnedDownWhore")));

			if (freeColonists.Any())
			{
				if (RJWSettings.DebugWhoring) ModLog.Message($" number of all acceptable Colonists {freeColonists.Count()}");
				return freeColonists.RandomElement();
			}

			return null;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{

			// Most things are now checked in the ThinkNode_ConditionalWhore.

			if (pawn.Drafted) return null;
			if (MP.IsInMultiplayer) return null; //fix error someday, maybe

			if (pawn.jobs.curDriver is JobDriver_Sex || pawn.jobs.curDriver is JobDriver_WhoreInvitingVisitors) return null; // already having sex

			if (!SexUtility.ReadyForLovin(pawn))
			{
				//Whores need rest too, but this'll reduxe the wait a bit every it triggers.
				pawn.mindState.canLovinTick -= 40;
				return null;
			}

			if (RJWSettings.DebugWhoring) ModLog.Message($"JobGiver_WhoreInvitingVisitors.TryGiveJob:({xxx.get_pawnname(pawn)})");

			int price;
			Pawn client = FindAttractivePawn(pawn, out price);
			if (client == null)
			{
				if (RJWSettings.DebugWhoring) ModLog.Message($" no clients found");
				return null;
			}

			if (RJWSettings.DebugWhoring) ModLog.Message($" {xxx.get_pawnname(client)} is client");

			Building_Bed whorebed = WhoreBed_Utility.FindBed(pawn, client);
			if (whorebed == null)
			{
				if (RJWSettings.DebugWhoring) ModLog.Message($" {xxx.get_pawnname(pawn)} + {xxx.get_pawnname(client)} - no usable bed found");
				return null;
			}
			whorebed.ReserveForWhoring(pawn, 600); // reserve for a short while until whore can actually ask customer

			return JobMaker.MakeJob(xxx.whore_inviting_visitors, client, whorebed);
		}
	}
}