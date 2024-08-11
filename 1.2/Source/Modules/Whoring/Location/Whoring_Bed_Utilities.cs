using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using System;
using UnityEngine;

namespace rjw
{
	public static class WhoreBed_Utility
	{
		public static readonly RoomRoleDef roleDefBrothel = DefDatabase<RoomRoleDef>.GetNamed("Brothel");


		// find the best bed for a customer; whore is needed as parameter to only select beds that are reachable
		public static Building_Bed FindBed(Pawn whore, Pawn customer)
		{
			List<Building_Bed> b = FindReachableAndAvailableWhoreBeds(whore, customer);
			return GetBestBedForCustomer(customer, b);
		}
		public static bool CanUseForWhoring(Pawn pawn, Building_Bed bed)
		{
			bool flag = bed.IsAvailableForWhoring(pawn) && pawn.CanReserveAndReach(bed, PathEndMode.InteractionCell, Danger.Unspecified) && !bed.IsForbidden(pawn);
			return flag;
		}

		public static Building_Bed GetBestBedForCustomer(Pawn customer, List<Building_Bed> beds)
		{
			if (beds != null && beds.Any())
			{
				return beds.MaxBy(bed => CalculateBedScore(customer, bed));
			}
			else
			{
				return null;
			}
		}
		public static float GetCheapestBedFactor(Pawn whore, Pawn customer)
		{
			List<Building_Bed> beds = FindReachableAndAvailableWhoreBeds(whore, customer);
			return GetCheapestBedFactor(beds);
		}
		public static float GetCheapestBedFactor(List<Building_Bed> beds)
		{
			if (beds != null && beds.Any())
			{
				return CalculatePriceFactor(beds.MinBy(bed => CalculatePriceFactor(bed)));
			}
			else
			{
				return 0f;
			}
		}
		// unused
		/*public static float GetMostExpensiveBedFactor(List<Building_Bed> beds)
		{
			if (beds != null && beds.Any())
			{
				return CalculatePriceFactor(beds.MaxBy(bed => CalculatePriceFactor(bed)));
			}
			else
			{
				return 0f;
			}
		}//*/

		public static float CalculatePriceFactor(Building_Bed bed, bool force=false)
		{
			// cache result (no need for saving): if no result available, calculate; otherwise save tick at which it has been calculated.
			// additional parameter "force" to make it update in certain circumstances even if game is paused
			// Rand.Int % 10 flattens the spike over time if many beds are toggled at once
			if (!force
				&& SaveStorage.DataStore.GetBedData(bed).lastScoreUpdateTick > 
					GenTicks.TicksGame - (SaveStorage.DataStore.GetBedData(bed).scoreUpdateTickDelay + Rand.Int % 10)
				&& SaveStorage.DataStore.GetBedData(bed).bedScore >= 0f)
			{
				//Log.Message("[RJW] lastScoreUpdateTick: " + SaveStorage.DataStore.GetBedData(bed).lastScoreUpdateTick.ToString() + " / TicksGame: "+ GenTicks.TicksGame.ToString());
				return SaveStorage.DataStore.GetBedData(bed).bedScore;
			}

			// uncomfortable beds reduce price, comfortable beds make customers pay a tip
			float comfort = bed.GetStatValue(StatDefOf.Comfort);

			// 
			Room room = bed.Map != null && bed.Map.regionAndRoomUpdater.Enabled ? bed.GetRoom() : null;

			if (room == null || room.PsychologicallyOutdoors) // apparently "outside" is a room
			{
				//if (RJWSettings.DebugWhoring) 
				//	Log.Message("[RJW] bed "+ bed.thingIDNumber.ToString() + ((room == null)?": room is null":" is outside") + " -> score = " + (0.1f * comfort).ToString("F2"));
				SaveStorage.DataStore.GetBedData(bed).lastScoreUpdateTick = GenTicks.TicksGame;
				SaveStorage.DataStore.GetBedData(bed).bedScore = 0.1f * comfort;
				return 0.1f * comfort;
			}

			float room_multiplier = 1f;

			//Room sharing is not liked by patrons
			int numBeds = room.ContainedBeds.Where(x => x.def.building.bed_humanlike).Count();

			// TODO: maybe more differentiation between various room types?
			if (room.Role == roleDefBrothel)
				room_multiplier *= (float)Math.Pow(0.8, numBeds - 1);
			else // if(room.Role == RoomRoleDefOf.Barracks)
				room_multiplier /= (2 * (numBeds - 1) + 1);

			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			//Room impressiveness factor
			//0 < scoreStageIndex < 10 (Last time checked)
			//3 is mediocore
			room_multiplier *= (float)(scoreStageIndex <= 3? .4f+scoreStageIndex*.2f : 1f+.3f*(scoreStageIndex-3));

			float price_factor = room_multiplier * comfort;

			// delay recalculation if result is the same as before
			if(price_factor == SaveStorage.DataStore.GetBedData(bed).bedScore)
			{
				if (SaveStorage.DataStore.GetBedData(bed).scoreUpdateTickDelay < 720)
				{
					// slowly increase recalculation delay to two seconds on speed 3
					SaveStorage.DataStore.GetBedData(bed).scoreUpdateTickDelay += 60;
				}
			} else
			{
				// reset recalculation delay
				SaveStorage.DataStore.GetBedData(bed).scoreUpdateTickDelay = 60;
			}

			// update bed data
			SaveStorage.DataStore.GetBedData(bed).lastScoreUpdateTick = GenTicks.TicksGame;
			SaveStorage.DataStore.GetBedData(bed).bedScore = price_factor;

			// this is quite spammy
			//if (RJWSettings.DebugWhoring)
			//	Log.Message("[RJW]CalculatePriceFactor for bed " + bed.thingIDNumber.ToString() + ": "
			//		+ "room_multiplier (num beds, impressiveness) ("+room_multiplier.ToString() +") * "
			//		+ "comfort (" + comfort.ToString() + ") = " + price_factor.ToString());

			return price_factor;
		}
		// "old" function
		/*public static float CalculatePriceFactorSimple(Building_Bed bed)
		{
			// copied from RJW :D
			float room_multiplier = 1f;
			Room room = bed.Map != null && bed.Map.regionAndRoomUpdater.Enabled ? bed.GetRoom() : null;

			if (room == null) return 0f;

			//Room sharing is not liked by patrons
			room_multiplier = room_multiplier / (2 * (room.ContainedBeds.Count() - 1) + 1);
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			//Room impressiveness factor
			//0 < scoreStageIndex < 10 (Last time checked)
			//3 is mediocore
			if (scoreStageIndex == 0)
			{
				room_multiplier *= 0.3f;
			}
			else if (scoreStageIndex > 3)
			{
				room_multiplier *= 1 + ((float)scoreStageIndex - 3) / 3.5f;
			} //top room triples the price

			return room_multiplier;
		}*/

		// customers would want the best bed
		// TODO: price as factor, rebalance
		public static float CalculateBedScore(Pawn customer, Building_Bed bed)
		{
			float basePriceFactor = CalculatePriceFactor(bed);

			// ascetic pawns want the least impressive room
			if (customer.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				float comfort = bed.GetStatValue(StatDefOf.Comfort);
				basePriceFactor = comfort * comfort / basePriceFactor; // inverse room effects - may be cheap, but should still be comfortable. ascetic isn't masochistic!
				if (RJWSettings.DebugWhoring)
					Log.Message("[RJW]CalculateBedScore - Customer is ascetic");
			}

			basePriceFactor *= 200; // make a larger number for better distance scaling (and random effect)

			// horny pawns are in a hurry and want a closer bed
			int distance = 0;
			if (xxx.is_hornyorfrustrated(customer))
			{
				distance = (int)bed.Position.DistanceTo(customer.Position);
				//if (RJWSettings.DebugWhoring)
				//	Log.Message("[RJW]CalculateBedScore - Pawn is horny - distance = "+distance.ToString());
			}

			int random_factor = Rand.Int % 100;
			float score = basePriceFactor - distance + random_factor;

			if (RJWSettings.DebugWhoring)
				Log.Message("[RJW]CalculateBedScore for bed "+ bed.thingIDNumber.ToString() + ": "
					+ "score from price ("+ basePriceFactor.ToString() +") "
					+ "- distance ("+distance.ToString()+") "
					+ "+ randomness ("+random_factor.ToString()+") "
					+ "= " + score.ToString());

			return score;
		}

		public static List<Building_Bed> FindReachableAndAvailableWhoreBeds(Pawn whore, Pawn customer)
		{
			List<Building_Bed> wb = new List<Building_Bed>();

			wb = whore.MapHeld.GetWhoreBeds().Where(bed =>
				!bed.IsForbidden(whore) &&
				!bed.IsForbidden(customer) &&
				!bed.IsBurning() &&
				bed.WhoringIsAllowedForPawn(whore) &&
				bed.IsAvailableForWhoring(whore) &&
				whore.CanReserveAndReach(bed, PathEndMode.OnCell, Danger.Unspecified) &&
				customer.CanReach(bed, PathEndMode.OnCell, Danger.Some)
				// TODO: price/affordable?
				).ToList();
			if (RJWSettings.DebugWhoring)
				Log.Message("[RJW]FindReachableAndAvailableWhoreBeds - found "+ wb.Count().ToString() +" beds");
			return wb;
		}

		public static IEnumerable<Building_Bed> GetWhoreBeds(this Map map, Area area = null)
		{
			if (map == null) return new Building_Bed[0];
			if (area == null) return map.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>();
			return map.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>().Where(b => area[b.Position]);
		}

		public static bool WhoringIsAllowedForPawn(this Building_Bed bed, Pawn pawn)
		{
			if (bed.IsAllowedForWhoringAll())
				return true;
			if (bed == pawn.ownership.OwnedBed && bed.IsAllowedForWhoringOwner())
				return true;
			return false;
		}

		public static void SetAllowedForWhoringOwner(this Building_Bed bed, bool isAllowed)
		{
			if (!isAllowed)
			{
				// if whoring is disallowed for owner, also disallow for all
				bed.SetAllowedForWhoringAll(false);
			}
			SaveStorage.DataStore.GetBedData(bed).allowedForWhoringOwner = isAllowed;
		}
		public static void ToggleAllowedForWhoringOwner(this Building_Bed bed)
		{
			bed.SetAllowedForWhoringOwner(!SaveStorage.DataStore.GetBedData(bed).allowedForWhoringOwner);
		}
		public static bool IsAllowedForWhoringOwner(this Building_Bed bed)
		{
			if (bed.Medical || bed.def.defName.Contains("Guest"))
			{
				return false;
			}
			if (bed.ForPrisoners)
			{
				// no toggle on prisoner beds, they may always use their own bed (if they are supposed to whore, anyway)
				return true;
			}
			return SaveStorage.DataStore.GetBedData(bed).allowedForWhoringOwner;
		}

		public static void SetAllowedForWhoringAll(this Building_Bed bed, bool isAllowed)
		{
			if (isAllowed)
			{
				// if whoring is allowed for all, also visibly allow for owner
				bed.SetAllowedForWhoringOwner(true);
				// if bed is designated for whoring, disable prisoner/medical use
				bed.ForPrisoners = false;
				bed.Medical = false;
			}
			SaveStorage.DataStore.GetBedData(bed).allowedForWhoringAll = isAllowed;

			bed.GetRoom()?.Notify_BedTypeChanged();
			bed.Notify_ColorChanged();
		}
		public static void ToggleAllowedForWhoringAll(this Building_Bed bed)
		{
			bed.SetAllowedForWhoringAll(!SaveStorage.DataStore.GetBedData(bed).allowedForWhoringAll);
		}
		public static bool IsAllowedForWhoringAll(this Building_Bed bed)
		{
			if (SaveStorage.DataStore.GetBedData(bed).allowedForWhoringAll)
			{
				if (bed.Medical || bed.ForPrisoners || bed.def.defName.Contains("Guest") || (bed.GetRoom()?.isPrisonCell == true))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static void ReserveForWhoring(this Building_Bed bed, Pawn p, int num_ticks)
		{
			SaveStorage.DataStore.GetBedData(bed).reservedUntilGameTick = GenTicks.TicksGame + num_ticks;
			SaveStorage.DataStore.GetBedData(bed).reservedForPawnID = p.thingIDNumber;
		}
		public static void UnreserveForWhoring(this Building_Bed bed)
		{
			SaveStorage.DataStore.GetBedData(bed).reservedUntilGameTick = 0;
			SaveStorage.DataStore.GetBedData(bed).reservedForPawnID = 0;
		}
		public static bool IsAvailableForWhoring(this Building_Bed bed, Pawn p)
		{
			// check for active reservation
			if (SaveStorage.DataStore.GetBedData(bed).reservedUntilGameTick > GenTicks.TicksGame)
			{
				if(SaveStorage.DataStore.GetBedData(bed).reservedForPawnID != p.thingIDNumber)
				{
					// a different pawn has reserved this bed
					return false;
				}
			}
			
			if(bed.OwnersForReading.Any())
			{
				for(int i=0; i<bed.OwnersForReading.Count; i++)
				{
					if(bed.OwnersForReading[i].InBed() && bed.OwnersForReading[i].CurrentBed() == bed)
					{
						// someone sleeping in this bed
						return false;
					}
				}
			}

			return true;
		}
	}
}