using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Multiplayer.API;

namespace rjw
{
	[HarmonyPatch(typeof(Hediff_Pregnant), "DoBirthSpawn")]
	internal static class PATCH_Hediff_Pregnant_DoBirthSpawn
	{
		/// <summary>
		/// This one overrides vanilla pregnancy hediff behavior.
		/// 0 - try to find suitable father for debug pregnancy
		/// 1st part if character pregnant and rjw pregnancies enabled - creates rjw pregnancy and instantly births it instead of vanilla
		/// 2nd part if character pregnant with rjw pregnancy - birth it
		/// 3rd part - debug - create rjw/vanila pregnancy and birth it
		/// </summary>
		/// <param name="mother"></param>
		/// <param name="father"></param>
		/// <returns></returns>
		[HarmonyPrefix]
		[SyncMethod]
		private static bool on_begin_DoBirthSpawn(ref Pawn mother, ref Pawn father)
		{
			//--Log.Message("patches_pregnancy::PATCH_Hediff_Pregnant::DoBirthSpawn() called");

			if (mother == null)
			{
				ModLog.Error("Hediff_Pregnant::DoBirthSpawn() - no mother defined -> exit");
				return false;
			}

			//vanilla debug?
			if (mother.gender == Gender.Male)
			{
				ModLog.Error("Hediff_Pregnant::DoBirthSpawn() - mother is male -> exit");
				return false;
			}

			// get a reference to the hediff we are applying
			//do birth for vanilla pregnancy Hediff
			//if using rjw pregnancies - add RJW pregnancy Hediff and birth it instead
			Hediff_Pregnant self = (Hediff_Pregnant)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("Pregnant"));
			if (self != null)
			{
				return ProcessVanillaPregnancy(self, mother, father);
			}

			// do birth for existing RJW pregnancies
			if (ProcessRJWPregnancy(mother, father))
			{
				return false;
			}

			return ProcessDebugPregnancy(mother, father);
		}


		private static bool ProcessVanillaPregnancy(Hediff_Pregnant pregnancy, Pawn mother, Pawn father)
		{
			void CreateAndBirth<T>() where T : Hediff_BasePregnancy
			{
				T hediff = Hediff_BasePregnancy.Create<T>(mother, father);
				hediff.GiveBirth();
				if (pregnancy != null)
					mother.health.RemoveHediff(pregnancy);
			}

			if (father == null)
			{
				father = Hediff_BasePregnancy.Trytogetfather(ref mother);
			}

			ModLog.Message("patches_pregnancy::PATCH_Hediff_Pregnant::DoBirthSpawn():Vanilla_pregnancy birthing:" + xxx.get_pawnname(mother));
			if (RJWPregnancySettings.animal_pregnancy_enabled && ((father == null || xxx.is_animal(father)) && xxx.is_animal(mother)))
			{
				//RJW Bestial pregnancy animal-animal
				ModLog.Message(" override as Bestial birthing(animal-animal): Father-" + xxx.get_pawnname(father) + " Mother-" + xxx.get_pawnname(mother));
				CreateAndBirth<Hediff_BestialPregnancy>();
				return false;
			}
			else if (RJWPregnancySettings.bestial_pregnancy_enabled && ((xxx.is_animal(father) && xxx.is_human(mother)) || (xxx.is_human(father) && xxx.is_animal(mother))))
			{
				//RJW Bestial pregnancy human-animal
				ModLog.Message(" override as Bestial birthing(human-animal): Father-" + xxx.get_pawnname(father) + " Mother-" + xxx.get_pawnname(mother));
				CreateAndBirth<Hediff_BestialPregnancy>();
				return false;
			}
			else if (RJWPregnancySettings.humanlike_pregnancy_enabled && (xxx.is_human(father) && xxx.is_human(mother)))
			{
				//RJW Humanlike pregnancy
				ModLog.Message(" override as Humanlike birthing: Father-" + xxx.get_pawnname(father) + " Mother-" + xxx.get_pawnname(mother));
				CreateAndBirth<Hediff_HumanlikePregnancy>();
				return false;
			}
			else
			{
				ModLog.Warning("Hediff_Pregnant::DoBirthSpawn() - checks failed, vanilla pregnancy birth");
				ModLog.Warning("Hediff_Pregnant::DoBirthSpawn(): Father-" + xxx.get_pawnname(father) + " Mother-" + xxx.get_pawnname(mother));
				//vanilla pregnancy code, no effects on rjw

				return true;
			}
		}

		private static bool ProcessRJWPregnancy(Pawn mother, Pawn father)
		{
			Hediff_BasePregnancy preg =
			   (Hediff_BasePregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy")) ??		//RJW Humanlike pregnancy
			   (Hediff_BasePregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_beast")) ??	//RJW Bestial pregnancy
			   (Hediff_BasePregnancy)mother.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_pregnancy_mech"));	//RJW Bestial pregnancy

			if (preg != null)
			{
				ModLog.Message($"patches_pregnancy::{preg.GetType().Name}::DoBirthSpawn() birthing:" + xxx.get_pawnname(mother));
				preg.GiveBirth();
				return true;
			}

			return false;
		}

		private static bool ProcessDebugPregnancy(Pawn mother, Pawn father)
		{
			void CreateAndBirth<T>() where T : Hediff_BasePregnancy
			{
				T hediff = Hediff_BasePregnancy.Create<T>(mother, father);
				hediff.GiveBirth();
			}
			//CreateAndBirth<Hediff_HumanlikePregnancy>();
			//CreateAndBirth<Hediff_BestialPregnancy>();
			//CreateAndBirth<Hediff_MechanoidPregnancy>();
			//return false;

			//debug, add RJW pregnancy and birth it
			ModLog.Message("patches_pregnancy::PATCH_Hediff_Pregnant::DoBirthSpawn():Debug_pregnancy birthing:" + xxx.get_pawnname(mother));
			if (father == null)
			{
				father = Hediff_BasePregnancy.Trytogetfather(ref mother);

				if (RJWPregnancySettings.bestial_pregnancy_enabled && ((xxx.is_animal(father) || xxx.is_animal(mother)))
					|| (xxx.is_animal(mother) && RJWPregnancySettings.animal_pregnancy_enabled))
				{
					//RJW Bestial pregnancy
					ModLog.Message(" override as Bestial birthing, mother: " + xxx.get_pawnname(mother));
					CreateAndBirth<Hediff_BestialPregnancy>();
				}
				else if (RJWPregnancySettings.humanlike_pregnancy_enabled && ((father == null || xxx.is_human(father)) && xxx.is_human(mother)))
				{
					//RJW Humanlike pregnancy
					ModLog.Message(" override as Humanlike birthing, mother: " + xxx.get_pawnname(mother));
					CreateAndBirth<Hediff_HumanlikePregnancy>();
				}
				else
				{
					ModLog.Warning("Hediff_Pregnant::DoBirthSpawn() - debug vanilla pregnancy birth");
					return true;
				}
			}
			return false;
		}
	}


	[HarmonyPatch(typeof(Hediff_Pregnant), "Tick")]
	class PATCH_Hediff_Pregnant_Tick
	{
		[HarmonyPrefix]
		static bool abort_on_missing_genitals(Hediff_Pregnant __instance)
		{
			if (__instance.pawn.IsHashIntervalTick(1000))
			{
				if (!Genital_Helper.has_vagina(__instance.pawn))
				{
					__instance.pawn.health.RemoveHediff(__instance);
				}
			}
			return true;
		}
	}


    [HarmonyPatch(typeof(PawnColumnWorker_Pregnant), "GetIconFor")]
    public class PawnColumnWorker_Patch_Icon
    {
        public static void Postfix(Pawn pawn, ref Texture2D __result)
        {
            if (pawn.IsVisiblyPregnant()) __result = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);
        }

    }

    [HarmonyPatch(typeof(PawnColumnWorker_Pregnant), "GetTooltipText")]
    public class PawnColumnWorker_Patch_Tooltip
    {
        public static bool Prefix(Pawn pawn, ref string __result)
        {
            float gestationProgress = PregnancyHelper.GetPregnancy(pawn).Severity;
            int num = (int)(pawn.RaceProps.gestationPeriodDays * 60000f);
            int numTicks = (int)(gestationProgress * (float)num);
            __result = "PregnantIconDesc".Translate(numTicks.ToStringTicksToDays("F0"), num.ToStringTicksToDays("F0"));
            return false;
        }

    }

    [HarmonyPatch(typeof(TransferableUIUtility), "DoExtraAnimalIcons")]
    public class TransferableUIUtility_Patch_Icon
    {
        //private static readonly Texture2D PregnantIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);
        public static void Postfix(Transferable trad, Rect rect, ref float curX, Texture2D ___PregnantIcon)
        {
            Pawn pawn = trad.AnyThing as Pawn;
            if (pawn?.health?.hediffSet != null && pawn.IsVisiblyPregnant())
            {
                Rect rect3 = new Rect(curX - 24f, (rect.height - 24f) / 2f, 24f, 24f);
                curX -= 24f;
                if (Mouse.IsOver(rect3))
                {
                    TooltipHandler.TipRegion(rect3, PawnColumnWorker_Pregnant.GetTooltipText(pawn));
                }
                GUI.DrawTexture(rect3, ___PregnantIcon);
            }
        }
    }

}
