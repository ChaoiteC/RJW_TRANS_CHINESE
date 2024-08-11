// #define TESTMODE // Uncomment to enable logging.

using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using System.Diagnostics;
using System;
using UnityEngine;
using Verse.AI.Group;
using Multiplayer.API;

namespace rjw
{
	/// <summary>
	/// Helper for whoring related stuff
	/// </summary>
	public class WhoringHelper
	{
		public const float baseMinPrice = 10f;
		public const float baseMaxPrice = 20f;

		public static readonly HashSet<string> backstories = new HashSet<string>(DefDatabase<StringListDef>.GetNamed("WhoreBackstories").strings);

		public static int WhoreMinPrice(Pawn whore)
		{
			float min_price = baseMinPrice;
			min_price *= WhoreAgeAdjustment(whore);
			min_price *= WhoreGenderAdjustment(whore);
			min_price *= WhoreInjuryAdjustment(whore);
			min_price *= WhoreAbilityAdjustmentMin(whore);
			//min_price *= WhoreRoomAdjustment(whore);
			if (xxx.has_traits(whore))
				min_price *= WhoreTraitAdjustmentMin(whore);

			return (int)min_price;
		}

		public static int WhoreMaxPrice(Pawn whore)
		{
			float max_price = baseMaxPrice;
			max_price *= WhoreAgeAdjustment(whore);
			max_price *= WhoreGenderAdjustment(whore);
			max_price *= WhoreInjuryAdjustment(whore);
			max_price *= WhoreAbilityAdjustmentMax(whore);
			//max_price *= WhoreRoomAdjustment(whore);
			if (xxx.has_traits(whore))
				max_price *= WhoreTraitAdjustmentMax(whore);

			return (int)max_price;
		}

		public static float WhoreGenderAdjustment(Pawn whore)
		{
			if (GenderHelper.GetSex(whore) == GenderHelper.Sex.futa)
				return 1.2f;
			return 1f;
		}

		public static float WhoreAgeAdjustment(Pawn whore)
		{
			return AgeConfigDef.Instance.whoringPriceByAge.Evaluate(SexUtility.ScaleToHumanAge(whore));
		}

		public static float WhoreInjuryAdjustment(Pawn whore)
		{
			float modifier = 1.0f;
			int injuries = whore.health.hediffSet.hediffs.Count(x => x.Visible && x.def.everCurableByItem && x.IsPermanent());

			if (injuries == 0) return modifier;

			while (injuries > 0)
			{
				modifier *= 0.85f;
				injuries--;
			}
			return modifier;
		}

		public static float WhoreAbilityAdjustmentMin(Pawn whore)
		{
			int b = backstories.Contains(whore.story?.adulthood?.titleShort) ? 30 : 0;
			int score = whore.records.GetAsInt(xxx.CountOfWhore);
			float multiplier = (score + b) / 100;
			multiplier = Math.Min(multiplier, 2);
			multiplier = (multiplier - 0.5f) * 0.5f + 1;
			return multiplier;
		}

		public static float WhoreAbilityAdjustmentMax(Pawn whore)
		{
			int b = backstories.Contains(whore.story?.adulthood?.titleShort) ? 30 : 0;
			int score = whore.records.GetAsInt(xxx.CountOfWhore);
			float multiplier = (score + b) / 100;
			multiplier = Math.Min(multiplier, 2);
			multiplier = (multiplier - 0.5f) * 0.5f + 1;
			return multiplier;
		}

		public static float WhoreTraitAdjustmentMin(Pawn whore)
		{
			float multiplier = WhoreTraitAdjustment(whore);
			if (xxx.is_masochist(whore)) // Won't haggle, may settle for low price
				multiplier *= 0.7f;
			if (xxx.is_nympho(whore)) // Same as above
				multiplier *= 0.4f;
			return multiplier;
		}

		public static float WhoreTraitAdjustmentMax(Pawn whore)
		{
			float multiplier = WhoreTraitAdjustment(whore);
			if (xxx.IndividualityIsActive && whore.story.traits.HasTrait(xxx.SYR_Haggler))
				multiplier *= 1.5f;
			if (whore.story.traits.HasTrait(TraitDefOf.Greedy))
				multiplier *= 2f;
			return multiplier;
		}

		public static float WhoreTraitAdjustment(Pawn whore)
		{
			float multiplier = 1f;

			if (xxx.has_traits(whore))
			{
				if (whore.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) == 2) // Industrious
					multiplier *= 1.2f;
				if (whore.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) == 1) // Hard Worker
					multiplier *= 1.1f;
				if (whore.story.traits.HasTrait(TraitDefOf.CreepyBreathing))
					multiplier *= 0.75f;

				if (whore.GetStatValue(StatDefOf.PawnBeauty) >= 2)
					multiplier *= 3.5f;
				else if (whore.GetStatValue(StatDefOf.PawnBeauty) >= 1)
					multiplier *= 2f;
				else if (whore.GetStatValue(StatDefOf.PawnBeauty) < 0)
					if (whore.GetStatValue(StatDefOf.PawnBeauty) >= -1)
						multiplier *= 0.8f;
					else
						multiplier *= 0.6f;
			}
			return multiplier;
		}

		/*public static float WhoreRoomAdjustment(Pawn whore)
		{
			float room_multiplier = 1f;
			Room ownedRoom = whore.ownership.OwnedRoom;

			if (ownedRoom == null) return 0f;

			//Room sharing is not liked by patrons
			room_multiplier = room_multiplier / (2 * (ownedRoom.Owners.Count() - 1) + 1);
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness));
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
		}//*/

		[SyncMethod]
		public static int PriceOfWhore(Pawn whore)
		{
			float NeedSexFactor = xxx.is_hornyorfrustrated(whore) ? 1 - xxx.need_some_sex(whore) / 8 : 1f;

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			float price = Rand.Range(WhoreMinPrice(whore), WhoreMaxPrice(whore));

			price *= NeedSexFactor;
			//--ModLog.Message(" xxx::PriceOfWhore - price is " + price);

			return (int)Math.Round(price);
		}

		public static bool CanAfford(Pawn targetPawn, Pawn whore, int priceOfWhore = -1)
		{
			if (targetPawn.Faction == whore.Faction) return true;

			//if (RJWSettings.DebugWhoring) ModLog.Message($"CanAfford for client {xxx.get_pawnname(targetPawn)}");
			int price = priceOfWhore < 0 ? PriceOfWhore(whore) : priceOfWhore;
			if (price == 0)
				return true;

			// can customer afford the cheapest bed? - skip check, should rarely make a difference
			//float bed_factor = WhoreBed_Utility.GetCheapestBedFactor(whore, targetPawn);
			//price = (int)(price * bed_factor);

			//if (RJWSettings.DebugWhoring) ModLog.Message($" whore price {price}");

			Lord lord = targetPawn.GetLord();
			Faction faction = targetPawn.Faction;
			int clientAmountOfSilvers = targetPawn.inventory.innerContainer.TotalStackCountOfDef(ThingDefOf.Silver);
			int totalAmountOfSilvers = clientAmountOfSilvers;

			if (faction != null)
			{
				List<Pawn> caravanMembers = targetPawn.Map.mapPawns.PawnsInFaction(targetPawn.Faction).Where(x => x.GetLord() == lord && x.inventory?.innerContainer?.TotalStackCountOfDef(ThingDefOf.Silver) > 0).ToList();
				if (!caravanMembers.Any())
				{
					//if (RJWSettings.DebugWhoring) ModLog.Message($" client not in caravan");
					//if (RJWSettings.DebugWhoring) ModLog.Message("CanAfford::(" + xxx.get_pawnname(targetPawn) + "," + xxx.get_pawnname(whore) + ") - totalAmountOfSilvers is " + totalAmountOfSilvers);
					return totalAmountOfSilvers >= price;
				}

				totalAmountOfSilvers += caravanMembers.Sum(member => member.inventory.innerContainer.TotalStackCountOfDef(ThingDefOf.Silver));
			}
			//if (RJWSettings.DebugWhoring) ModLog.Message($" client silver = {clientAmountOfSilvers} caravan silver = {totalAmountOfSilvers - clientAmountOfSilvers}");

			//if (RJWSettings.DebugWhoring) ModLog.Message("CanAfford:: can afford the whore: " + (totalAmountOfSilvers >= price));
			return totalAmountOfSilvers >= price;
		}

		//priceOfWhore is assumed >=0, and targetPawn is assumed to be able to pay the price(either by caravan, or by himself)
		//This means that targetPawn has total stackcount of silvers >= priceOfWhore.
		public static int PayPriceToWhore(Pawn targetPawn, int priceOfWhore, Pawn whore)
		{
			if (RJWSettings.DebugWhoring) ModLog.Message($"PayPriceToWhore for client {xxx.get_pawnname(targetPawn)}");
			int AmountLeft = priceOfWhore;
			if (targetPawn.Faction == whore.Faction || priceOfWhore == 0)
			{
				if (RJWSettings.DebugWhoring) ModLog.Message($" No need to pay price");
				return AmountLeft;
			}
			Lord lord = targetPawn.GetLord();
			//Caravan guestCaravan = Find.WorldObjects.Caravans.Where(x => x.Spawned && x.ContainsPawn(targetPawn) && x.Faction == targetPawn.Faction && !x.IsPlayerControlled).FirstOrDefault();
			List<Pawn> caravan = targetPawn.Map.mapPawns.PawnsInFaction(targetPawn.Faction).Where(x => x.GetLord() == lord && x.inventory?.innerContainer != null && x.inventory.innerContainer.TotalStackCountOfDef(ThingDefOf.Silver) > 0).ToList();

			IEnumerable<Thing> TraderSilvers;
			if (!caravan.Any())
			{
				if (RJWSettings.DebugWhoring) ModLog.Message($" (not a caravan member), try to pay with own silver");
				TraderSilvers = targetPawn.inventory.innerContainer.Where(x => x.def == ThingDefOf.Silver);
				foreach (Thing silver in TraderSilvers)
				{
					if (AmountLeft <= 0)
						break;
					int dropAmount = silver.stackCount >= AmountLeft ? AmountLeft : silver.stackCount;
					if (targetPawn.inventory.innerContainer.TryDrop(silver, whore.Position, whore.Map, ThingPlaceMode.Near, dropAmount, out Thing resultingSilvers))
					{
						if (resultingSilvers is null)
						{
							if (RJWSettings.DebugWhoring) ModLog.Message($" have no silver");
								continue;
						}
						AmountLeft -= resultingSilvers.stackCount;
						if (AmountLeft <= 0)
						{
							if (RJWSettings.DebugWhoring) ModLog.Message($" {xxx.get_pawnname(targetPawn)} paid {resultingSilvers.stackCount} silver");
							break;
						}
					}
					else
					{
						if (RJWSettings.DebugWhoring) ModLog.Message($" TryDrop failed");
							break;
					}
				}
				if (RJWSettings.DebugWhoring) ModLog.Message($" price: {priceOfWhore}, paid: {priceOfWhore-AmountLeft}");
			}
			else
			{
				if (RJWSettings.DebugWhoring) ModLog.Message($" (caravan member), try to pay {AmountLeft} silver with caravan silver");
				foreach (Pawn caravanMember in caravan)
				{
					TraderSilvers = caravanMember.inventory.innerContainer.Where(x => x.def == ThingDefOf.Silver);
					if (RJWSettings.DebugWhoring) ModLog.Message($" try to pay with {xxx.get_pawnname(caravanMember)} silver");
					foreach (Thing silver in TraderSilvers)
					{
						if (AmountLeft <= 0)
							break;
						int dropAmount = silver.stackCount >= AmountLeft ? AmountLeft : silver.stackCount;
						if (caravanMember.inventory.innerContainer.TryDrop(silver, whore.Position, whore.Map, ThingPlaceMode.Near, dropAmount, out Thing resultingSilvers))
						{
							if (resultingSilvers is null)
							{
								if (RJWSettings.DebugWhoring) ModLog.Message($" have no silver");
								continue;
							}
							AmountLeft -= resultingSilvers.stackCount;
							if (AmountLeft <= 0)
							{
								if (RJWSettings.DebugWhoring) ModLog.Message($" {xxx.get_pawnname(caravanMember)} paid {resultingSilvers.stackCount} silver");
								break;
							}
						}
					}
				}
			}
			if (RJWSettings.DebugWhoring) ModLog.Message($" price: {priceOfWhore}, paid: {priceOfWhore - AmountLeft}");
			return AmountLeft;
		}

		[SyncMethod]
		public static bool IsHookupAppealing(Pawn target, Pawn whore)
		{
			if (PawnUtility.WillSoonHaveBasicNeed(target))
			{
				//Log.Message("IsHookupAppealing - fail: " + xxx.get_pawnname(target) + " has need to do");
				return false;
			}
			float num = target.relations.SecondaryRomanceChanceFactor(whore) / 1.5f;
			if (xxx.is_frustrated(target))
			{
				num *= 3.0f;
			}
			else if (xxx.is_hornyorfrustrated(target))
			{
				num *= 2.0f;
			}
			if (xxx.is_zoophile(target) && !xxx.is_animal(whore))
			{
				num *= 0.5f;
			}
			if (xxx.AlienFrameworkIsActive)
			{
				if (xxx.is_xenophile(target))
				{
					if (target.def.defName == whore.def.defName)
						num *= 0.5f; // Same species, xenophile less interested.
					else
						num *= 1.5f; // Different species, xenophile more interested.
				}
				else if (xxx.is_xenophobe(target))
				{
					if (target.def.defName != whore.def.defName)
						num *= 0.25f; // Different species, xenophobe less interested.
				}
			}

			num *= 0.8f + ((float)whore.skills.GetSkill(SkillDefOf.Social).Level / 40); // 0.8 to 1.3
			num *= Mathf.InverseLerp(-100f, 0f, target.relations.OpinionOf(whore)); // 1 to 0 reduce score by negative opinion/relations to whore
			//Log.Message("IsHookupAppealing - score: " + num);
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			return Rand.Range(0.05f, 1f) < num;
		}

		// Summary:
		//   Check if the pawn is willing to hook up. Checked for both target and the whore(when hooking colonists).
		[SyncMethod]
		public static bool WillPawnTryHookup(Pawn target)
		{
			if (target.story.traits.HasTrait(TraitDefOf.Asexual))
			{
				return false;
			}
			Pawn lover = LovePartnerRelationUtility.ExistingMostLikedLovePartner(target, false);
			if (lover == null)
			{
				return true;
			}
			float num = target.relations.OpinionOf(lover);
			float num2 = Mathf.InverseLerp(30f, -80f, num);

			if (xxx.is_prude(target))
			{
				num2 = 0f;
			}
			else if (xxx.is_lecher(target))
			{
				//Lechers are always up for it.
				num2 = Mathf.InverseLerp(100f, 50f, num);
			}
			else if (target.Map == lover.Map)
			{
				//Less likely to cheat if the lover is on the same map.
				num2 = Mathf.InverseLerp(70f, 15f, num);
			}
			//else default values

			if (xxx.is_frustrated(target))
			{
				num2 *= 1.8f;
			}
			else if (xxx.is_hornyorfrustrated(target))
			{
				num2 *= 1.4f;
			}
			num2 /= 1.5f;
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			return Rand.Range(0f, 1f) < num2;
		}
	}
}