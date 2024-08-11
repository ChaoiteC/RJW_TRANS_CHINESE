using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using HarmonyLib;
using Multiplayer.API;

namespace rjw
{
	public class SexUtility
	{
		private const float base_sat_per_fuck = 0.40f;
		private const float base_sat_per_quirk = 0.20f;

		//Consensual types
		private static readonly InteractionDef analSex = DefDatabase<InteractionDef>.GetNamed("AnalSex");
		private static readonly InteractionDef vaginalSex = DefDatabase<InteractionDef>.GetNamed("VaginalSex");
		private static readonly InteractionDef doublePenetration = DefDatabase<InteractionDef>.GetNamed("DoublePenetration");
		//Oral sex
		private static readonly InteractionDef rimming = DefDatabase<InteractionDef>.GetNamed("Rimming");
		private static readonly InteractionDef cunnilingus = DefDatabase<InteractionDef>.GetNamed("Cunnilingus");
		private static readonly InteractionDef fellatio = DefDatabase<InteractionDef>.GetNamed("Fellatio");
		private static readonly InteractionDef beakjob = DefDatabase<InteractionDef>.GetNamed("Beakjob");
		private static readonly InteractionDef sixtynine = DefDatabase<InteractionDef>.GetNamed("Sixtynine");
		
		//Other sex types
		private static readonly InteractionDef breastjob = DefDatabase<InteractionDef>.GetNamed("Breastjob");
		private static readonly InteractionDef handjob = DefDatabase<InteractionDef>.GetNamed("Handjob");
		private static readonly InteractionDef footjob = DefDatabase<InteractionDef>.GetNamed("Footjob");
		private static readonly InteractionDef fingering = DefDatabase<InteractionDef>.GetNamed("Fingering");
		private static readonly InteractionDef fisting = DefDatabase<InteractionDef>.GetNamed("Fisting");
		private static readonly InteractionDef scissoring = DefDatabase<InteractionDef>.GetNamed("Scissoring");

		private static readonly InteractionDef mutualMasturbation = DefDatabase<InteractionDef>.GetNamed("MutualMasturbation");

		//femdom
		//Consensual types
		private static readonly InteractionDef analSexF = DefDatabase<InteractionDef>.GetNamed("AnalSexF");
		private static readonly InteractionDef vaginalSexF = DefDatabase<InteractionDef>.GetNamed("VaginalSexF");
		private static readonly InteractionDef doublePenetrationF = DefDatabase<InteractionDef>.GetNamed("DoublePenetrationF");
		//Oral sex
		private static readonly InteractionDef rimmingF = DefDatabase<InteractionDef>.GetNamed("RimmingF");
		private static readonly InteractionDef cunnilingusF = DefDatabase<InteractionDef>.GetNamed("CunnilingusF");
		private static readonly InteractionDef fellatioF = DefDatabase<InteractionDef>.GetNamed("FellatioF");
		private static readonly InteractionDef beakjobF = DefDatabase<InteractionDef>.GetNamed("BeakjobF");
		private static readonly InteractionDef sixtynineF = DefDatabase<InteractionDef>.GetNamed("SixtynineF");
		
		//Other sex types
		private static readonly InteractionDef breastjobF = DefDatabase<InteractionDef>.GetNamed("BreastjobF");
		private static readonly InteractionDef handjobF = DefDatabase<InteractionDef>.GetNamed("HandjobF");
		private static readonly InteractionDef footjobF = DefDatabase<InteractionDef>.GetNamed("FootjobF");
		private static readonly InteractionDef fingeringF = DefDatabase<InteractionDef>.GetNamed("FingeringF");
		private static readonly InteractionDef fistingF = DefDatabase<InteractionDef>.GetNamed("FistingF");
		private static readonly InteractionDef scissoringF = DefDatabase<InteractionDef>.GetNamed("ScissoringF");

		private static readonly InteractionDef mutualMasturbationF = DefDatabase<InteractionDef>.GetNamed("MutualMasturbationF");



		//Rape types
		private static readonly InteractionDef analRape = DefDatabase<InteractionDef>.GetNamed("AnalRape");
		private static readonly InteractionDef vaginalRape = DefDatabase<InteractionDef>.GetNamed("VaginalRape");
		private static readonly InteractionDef doublePenetrationRape = DefDatabase<InteractionDef>.GetNamed("DoublePenetrationRape");

		private static readonly InteractionDef breastjobRape = DefDatabase<InteractionDef>.GetNamed("BreastjobRape");
		private static readonly InteractionDef handjobRape = DefDatabase<InteractionDef>.GetNamed("HandjobRape");
		private static readonly InteractionDef footjobRape = DefDatabase<InteractionDef>.GetNamed("FootjobRape");
		private static readonly InteractionDef fingeringRape = DefDatabase<InteractionDef>.GetNamed("FingeringRape");
		private static readonly InteractionDef fistingRape = DefDatabase<InteractionDef>.GetNamed("FistingRape");
		private static readonly InteractionDef scissoringRape = DefDatabase<InteractionDef>.GetNamed("ScissoringRape");

		private static readonly InteractionDef rimmingRape = DefDatabase<InteractionDef>.GetNamed("RimmingRape");
		private static readonly InteractionDef cunnilingusRape = DefDatabase<InteractionDef>.GetNamed("CunnilingusRape");
		private static readonly InteractionDef fellatioRape = DefDatabase<InteractionDef>.GetNamed("FellatioRape");
		private static readonly InteractionDef beakjobRape = DefDatabase<InteractionDef>.GetNamed("BeakjobRape");
		private static readonly InteractionDef sixtynineRape = DefDatabase<InteractionDef>.GetNamed("SixtynineRape");

		private static readonly InteractionDef otherRape = DefDatabase<InteractionDef>.GetNamed("OtherRape");
		private static readonly InteractionDef violateCorpse = DefDatabase<InteractionDef>.GetNamed("ViolateCorpse");
		private static readonly InteractionDef mechImplant = DefDatabase<InteractionDef>.GetNamed("MechImplant");

		//femdom
		private static readonly InteractionDef analRapeF = DefDatabase<InteractionDef>.GetNamed("AnalRapeF");
		private static readonly InteractionDef vaginalRapeF = DefDatabase<InteractionDef>.GetNamed("VaginalRapeF");
		private static readonly InteractionDef doublePenetrationRapeF = DefDatabase<InteractionDef>.GetNamed("DoublePenetrationRapeF");

		private static readonly InteractionDef breastjobRapeF = DefDatabase<InteractionDef>.GetNamed("BreastjobRapeF");
		private static readonly InteractionDef handjobRapeF = DefDatabase<InteractionDef>.GetNamed("HandjobRapeF");
		private static readonly InteractionDef footjobRapeF = DefDatabase<InteractionDef>.GetNamed("FootjobRapeF");
		private static readonly InteractionDef fingeringRapeF = DefDatabase<InteractionDef>.GetNamed("FingeringRapeF");
		private static readonly InteractionDef fistingRapeF = DefDatabase<InteractionDef>.GetNamed("FistingRapeF");
		private static readonly InteractionDef scissoringRapeF = DefDatabase<InteractionDef>.GetNamed("ScissoringRapeF");

		private static readonly InteractionDef rimmingRapeF = DefDatabase<InteractionDef>.GetNamed("RimmingRapeF");
		private static readonly InteractionDef cunnilingusRapeF = DefDatabase<InteractionDef>.GetNamed("CunnilingusRapeF");
		private static readonly InteractionDef fellatioRapeF = DefDatabase<InteractionDef>.GetNamed("FellatioRapeF");
		private static readonly InteractionDef beakjobRapeF = DefDatabase<InteractionDef>.GetNamed("BeakjobRapeF");
		private static readonly InteractionDef sixtynineRapeF = DefDatabase<InteractionDef>.GetNamed("SixtynineRapeF");

		//Breeding
		private static readonly InteractionDef vaginalBreeding = DefDatabase<InteractionDef>.GetNamed("VaginalBreeding");
		private static readonly InteractionDef analBreeding = DefDatabase<InteractionDef>.GetNamed("AnalBreeding");
		private static readonly InteractionDef oralBreeding = DefDatabase<InteractionDef>.GetNamed("OralBreeding");
		private static readonly InteractionDef forcedOralBreeding = DefDatabase<InteractionDef>.GetNamed("ForcedOralBreeding");
		private static readonly InteractionDef forcedFellatioBreeding = DefDatabase<InteractionDef>.GetNamed("ForcedFellatioBreeding");
		private static readonly InteractionDef fingeringBreeding = DefDatabase<InteractionDef>.GetNamed("FingeringBreeding");
		private static readonly InteractionDef requestVaginalBreeding = DefDatabase<InteractionDef>.GetNamed("RequestVaginalBreeding");
		private static readonly InteractionDef requestAnalBreeding = DefDatabase<InteractionDef>.GetNamed("RequestAnalBreeding");

		public static readonly InteractionDef AnimalSexChat = DefDatabase<InteractionDef>.GetNamed("AnimalSexChat");

		private static readonly ThingDef cum = ThingDef.Named("FilthCum");
		private static readonly ThingDef girlcum = ThingDef.Named("FilthGirlCum");

		public static readonly Dictionary<InteractionDef, xxx.rjwSextype> sexActs = new Dictionary<InteractionDef, xxx.rjwSextype>
		{
			{analSex, xxx.rjwSextype.Anal },
			{analSexF, xxx.rjwSextype.Anal },
			{vaginalSex, xxx.rjwSextype.Vaginal },
			{vaginalSexF, xxx.rjwSextype.Vaginal },
			{fellatio, xxx.rjwSextype.Fellatio },
			{fellatioF, xxx.rjwSextype.Fellatio },
			{rimming, xxx.rjwSextype.Rimming },
			{rimmingF, xxx.rjwSextype.Rimming },
			{cunnilingus, xxx.rjwSextype.Cunnilingus },
			{cunnilingusF, xxx.rjwSextype.Cunnilingus },
			{sixtynine, xxx.rjwSextype.Sixtynine },
			{sixtynineF, xxx.rjwSextype.Sixtynine },
			{handjob, xxx.rjwSextype.Handjob },
			{handjobF, xxx.rjwSextype.Handjob },
			{breastjob, xxx.rjwSextype.Boobjob },
			{breastjobF, xxx.rjwSextype.Boobjob },
			{doublePenetration, xxx.rjwSextype.DoublePenetration },
			{doublePenetrationF, xxx.rjwSextype.DoublePenetration },
			{footjob, xxx.rjwSextype.Footjob },
			{footjobF, xxx.rjwSextype.Footjob },
			{fingering, xxx.rjwSextype.Fingering },
			{fingeringF, xxx.rjwSextype.Fingering },
			{scissoring, xxx.rjwSextype.Scissoring },
			{scissoringF, xxx.rjwSextype.Scissoring },
			{mutualMasturbation, xxx.rjwSextype.MutualMasturbation },
			{mutualMasturbationF, xxx.rjwSextype.MutualMasturbation },
			{fisting, xxx.rjwSextype.Fisting },
			{fistingF, xxx.rjwSextype.Fisting },

			{analRape, xxx.rjwSextype.Anal },
			{analRapeF, xxx.rjwSextype.Anal },
			{vaginalRape, xxx.rjwSextype.Vaginal },
			{vaginalRapeF, xxx.rjwSextype.Vaginal },
			{fellatioRape, xxx.rjwSextype.Fellatio },
			{fellatioRapeF, xxx.rjwSextype.Fellatio },
			{rimmingRape, xxx.rjwSextype.Rimming },
			{rimmingRapeF, xxx.rjwSextype.Rimming },
			{cunnilingusRape, xxx.rjwSextype.Cunnilingus },
			{cunnilingusRapeF, xxx.rjwSextype.Cunnilingus },
			{sixtynineRape, xxx.rjwSextype.Sixtynine },
			{sixtynineRapeF, xxx.rjwSextype.Sixtynine },
			{handjobRape, xxx.rjwSextype.Handjob },
			{handjobRapeF, xxx.rjwSextype.Handjob },
			{breastjobRape, xxx.rjwSextype.Boobjob },
			{breastjobRapeF, xxx.rjwSextype.Boobjob },
			{doublePenetrationRape, xxx.rjwSextype.DoublePenetration },
			{doublePenetrationRapeF, xxx.rjwSextype.DoublePenetration },
			{footjobRape, xxx.rjwSextype.Footjob },
			{footjobRapeF, xxx.rjwSextype.Footjob },
			{fingeringRape, xxx.rjwSextype.Fingering },
			{fingeringRapeF, xxx.rjwSextype.Fingering },
			{scissoringRape, xxx.rjwSextype.Scissoring },
			{scissoringRapeF, xxx.rjwSextype.Scissoring },
			{fistingRape, xxx.rjwSextype.Fisting },
			{fistingRapeF, xxx.rjwSextype.Fisting },

			{vaginalBreeding, xxx.rjwSextype.Vaginal },
			{analBreeding, xxx.rjwSextype.Anal },
			{oralBreeding, xxx.rjwSextype.Oral },
			{forcedOralBreeding, xxx.rjwSextype.Oral },
			{forcedFellatioBreeding, xxx.rjwSextype.Fellatio },
			{fingeringBreeding, xxx.rjwSextype.Fingering },
			{requestVaginalBreeding, xxx.rjwSextype.Vaginal },
			{requestAnalBreeding, xxx.rjwSextype.Anal },

			{otherRape, xxx.rjwSextype.Oral },
			{mechImplant, xxx.rjwSextype.MechImplant },
			{violateCorpse, xxx.rjwSextype.None } // Sextype as none, since this cannot result in pregnancy etc.
		};

		// same as below but most rape/breed interactions removed as they make no sence
		public static readonly InteractionDef[,] rmb_dictionarykeys =
		{
			// Interactions:
			// 1: Consensual, 2: Rape, 3: Bestiality
			{vaginalSex, vaginalRape, vaginalBreeding },			// Vaginal - giving (fuck victim)
			{vaginalSexF, vaginalRapeF, requestVaginalBreeding },			// Vaginal - receiving (female force victim to fuck her vaginally sex)
			{analSex, analRape, analBreeding },						// Anal
			{analSexF, analRapeF, requestAnalBreeding },					// Anal - reseiving
			{cunnilingus, cunnilingusRape, null },			// Cunnilingus
			{cunnilingusF, cunnilingusRapeF, null },								// Cunnilingus - receiving
			{rimming, rimmingRape, null },							// Rimming
			{rimmingF, rimmingRapeF, null },									// Rimming - receiving
			{fellatio, fellatioRape, null },				// Fellatio
			{fellatioF, fellatioRapeF, null },								// Fellatio - receiving
			{doublePenetration, doublePenetrationRape, null },		// Double penetration
			{doublePenetrationF, doublePenetrationRapeF, null },		// Double penetration - receiving
			{breastjob, breastjobRape, null },						// Breastjob
			{breastjobF, breastjobRapeF, null },								// Breastjob - receiving
			{handjob, handjobRape, null },							// Handjob
			{handjobF, handjobRapeF, null },									// Handjob - receiving
			{footjob, footjobRape, null },							// Footjob
			{footjobF, footjobRapeF, null },									// Footjob - receiving
			{fingering, fingeringRape, null },			// Fingering
			{fingeringF, fingeringRapeF, null },								// Fingering - receiving
			{scissoring, scissoringRape, null },					// Scissoring
			{scissoringF, scissoringRapeF, null },								// Scissoring - receiving
			{mutualMasturbation, null, null },						// Mutual Masturbation
			{mutualMasturbationF, null, null },						// Mutual Masturbation - receiving
			{fisting, fistingRape, null },							// Fisting
			{fistingF, fistingRapeF, null },									// Fisting - receiving
			{sixtynine, sixtynineRape, null },						// 69
			{sixtynineF, sixtynineRapeF, null },								// 69 - receiving
		};

		// This and the following array are used to generate text from interactions.
		public static readonly InteractionDef[,] dictionarykeys =
		{
			// 1: Interactions_Sex (consensual), 2: Interactions_Rape (forced), 3: Interactions_Breed (animals)
			{vaginalSex, vaginalRape, vaginalBreeding },					// Vaginal
			{vaginalSexF, vaginalRapeF, requestVaginalBreeding },				// Vaginal - reseiving
			{analSex, analRape, analBreeding },								// Anal
			{analSexF, analRapeF, requestAnalBreeding },						// Anal - reseiving
			{cunnilingus, cunnilingusRape, oralBreeding },					// Cunnilingus
			{cunnilingusF, cunnilingusRapeF, forcedOralBreeding },					// Cunnilingus - receiving
			{rimming, rimmingRape, oralBreeding },							// Rimming
			{rimmingF, rimmingRapeF, forcedOralBreeding },						// Rimming - receiving
			{fellatio, fellatioRape, oralBreeding },						// Fellatio
			{fellatioF, fellatioRapeF, forcedOralBreeding },						// Fellatio - receiving
			{doublePenetration, vaginalRape, vaginalBreeding },				// Double penetration
			{doublePenetrationF, vaginalRapeF, requestVaginalBreeding },		// Double penetration - receiving
			{breastjob, breastjobRape, null },						// Breastjob
			{breastjobF, breastjobRapeF, null },								// Breastjob - receiving
			{handjob, handjobRape, null },							// Handjob
			{handjobF, handjobRapeF, null },									// Handjob - receiving
			{footjob, footjobRape, null },							// Footjob
			{footjobF, footjobRapeF, null },									// Footjob - receiving
			{fingering, fingeringRape, fingeringBreeding },					// Fingering
			{fingeringF, fingeringRapeF, null },						// Fingering - receiving
			{scissoring, scissoringRape, null },						// Scissoring
			{scissoringF, scissoringRapeF, null },							// Scissoring - receiving
			{mutualMasturbation, null, null },			// Mutual Masturbation
			{mutualMasturbationF, null, null },			// Mutual Masturbation - receiving
			{fisting, fistingRape, null },								// Fisting
			{fistingF, fistingRapeF, null },									// Fisting - receiving
			{sixtynine, sixtynineRape, null },						// 69
			{sixtynineF, sixtynineRapeF, null },								// 69 - receiving
		};

		// Alert checker that is called from several jobs. Checks the pawn relation, and whether it should sound alert.
		// notification in top left corner
		// rape attempt
		public static void RapeTargetAlert(Pawn rapist, Pawn target)
		{
			if (target.IsDesignatedComfort() && rapist.jobs.curDriver.GetType() == typeof(JobDriver_RapeComfortPawn))
				if (!RJWPreferenceSettings.ShowForCP)
					return;
			if (target.IsDesignatedComfort() && rapist.jobs.curDriver.GetType() == typeof(JobDriver_Breeding))
				if (target.IsDesignatedBreeding())
					if (!RJWPreferenceSettings.ShowForBreeding)
						return;

			bool silent = false;
			PawnRelationDef relation = rapist.GetMostImportantRelation(target);
			string rapeverb = "rape";

			if (xxx.is_mechanoid(rapist)) rapeverb = "assault";
			else if (xxx.is_animal(rapist) || xxx.is_animal(target)) rapeverb = "breed";

			// TODO: Need to write a cherker method for family relations. Would be useful for other things than just this, such as incest settings/quirk.

			string message = (xxx.get_pawnname(rapist) + " is trying to " + rapeverb + " " + xxx.get_pawnname(target));
			message += relation == null ? "." : (", " + rapist.Possessive() + " " + relation.GetGenderSpecificLabel(target) + ".");

			switch (RJWPreferenceSettings.rape_attempt_alert)
			{
				case RJWPreferenceSettings.RapeAlert.Enabled:
					break;
				case RJWPreferenceSettings.RapeAlert.Humanlikes:
					if (!xxx.is_human(target))
						return;
					break;
				case RJWPreferenceSettings.RapeAlert.Colonists:
					if (!target.IsColonist)
						return;
					break;
				case RJWPreferenceSettings.RapeAlert.Silent:
					silent = true;
					break;
				default:
					return;
			}

			if (!silent)
			{
				Messages.Message(message, rapist, MessageTypeDefOf.NegativeEvent);
			}
			else
			{
				Messages.Message(message, rapist, MessageTypeDefOf.SilentInput);
			}
		}

		// Alert checker that is called from several jobs.
		// notification in top left corner
		// rape started
		public static void BeeingRapedAlert(Pawn rapist, Pawn target)
		{
			if (target.IsDesignatedComfort() && rapist.jobs.curDriver.GetType() == typeof(JobDriver_RapeComfortPawn))
				if (!RJWPreferenceSettings.ShowForCP)
					return;
			if (target.IsDesignatedComfort() && rapist.jobs.curDriver.GetType() == typeof(JobDriver_Breeding))
				if (target.IsDesignatedBreeding())
					if (!RJWPreferenceSettings.ShowForBreeding)
						return;

			bool silent = false;

			switch (RJWPreferenceSettings.rape_alert)
			{
				case RJWPreferenceSettings.RapeAlert.Enabled:
					break;
				case RJWPreferenceSettings.RapeAlert.Humanlikes:
					if (!xxx.is_human(target))
						return;
					break;
				case RJWPreferenceSettings.RapeAlert.Colonists:
					if (!target.IsColonist)
						return;
					break;
				case RJWPreferenceSettings.RapeAlert.Silent:
					silent = true;
					break;
				default:
					return;
			}

			if (!silent)
			{
				Messages.Message(xxx.get_pawnname(target) + " is getting raped.", target, MessageTypeDefOf.NegativeEvent);
			}
			else
			{
				Messages.Message(xxx.get_pawnname(target) + " is getting raped.", target, MessageTypeDefOf.SilentInput);
			}
		}

		// Quick method that return a body part by name. Used for checking if a pawn has a specific body part, etc.
		public static BodyPartRecord GetPawnBodyPart(Pawn pawn, string bodyPart)
		{
			return pawn.RaceProps.body.AllParts.Find(x => x.def == DefDatabase<BodyPartDef>.GetNamed(bodyPart));
		}

		public static void CumFilthGenerator(Pawn pawn)
		{
			if (pawn.Dead) return;
			if (xxx.is_slime(pawn)) return;
			if (!RJWSettings.cum_filth) return;

			// Larger creatures, larger messes.
			float pawn_cum = Math.Min(80 / ScaleToHumanAge(pawn), 2.0f) * pawn.BodySize;

			// Increased output if the pawn has the Messy quirk.
			if (xxx.has_quirk(pawn, "Messy"))
				pawn_cum *= 2.0f;

			var partBPR = Genital_Helper.get_genitalsBPR(pawn);
			var parts = Genital_Helper.get_PartsHediffList(pawn, partBPR);

			if (Genital_Helper.has_vagina(pawn, parts))
				FilthMaker.TryMakeFilth(pawn.PositionHeld, pawn.MapHeld, girlcum, pawn.LabelIndefinite(), (int)Math.Max(pawn_cum/2, 1.0f));

			if (Genital_Helper.has_penis_fertile(pawn, parts))
				FilthMaker.TryMakeFilth(pawn.PositionHeld, pawn.MapHeld, cum, pawn.LabelIndefinite(), (int)Math.Max(pawn_cum, 1.0f));
		}

		// The pawn may or may not clean up the mess after fapping.
		[SyncMethod]
		public static bool ConsiderCleaning(Pawn fapper)
		{
			if (!RJWSettings.cum_filth) return false;
			if (!xxx.has_traits(fapper) || fapper.story == null) return false;
			if (fapper.WorkTagIsDisabled(WorkTags.Cleaning)) return false;

			float do_cleaning = 0.5f; // 50%

			if (!fapper.PositionHeld.Roofed(fapper.Map))
				do_cleaning -= 0.25f; // Less likely to clean if outdoors.

			if (xxx.CTIsActive && fapper.story.traits.HasTrait(xxx.RCT_NeatFreak))
				do_cleaning += 1.00f;

			if (xxx.has_quirk(fapper, "Messy"))
				do_cleaning -= 0.75f;

			switch (fapper.needs?.rest?.CurCategory)
			{
				case RestCategory.Exhausted:
					do_cleaning -= 0.5f;
					break;
				case RestCategory.VeryTired:
					do_cleaning -= 0.3f;
					break;
				case RestCategory.Tired:
					do_cleaning -= 0.1f;
					break;
				case RestCategory.Rested:
					do_cleaning += 0.3f;
					break;
			}

			if (fapper.story.traits.DegreeOfTrait(TraitDefOf.NaturalMood) == -2) // Depressive
				do_cleaning -= 0.3f;
			if (fapper.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) == 2) // Industrious
				do_cleaning += 1.0f;
			else if (fapper.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) == 1) // Hard worker
				do_cleaning += 0.5f;
			else if (fapper.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) == -1) // Lazy
				do_cleaning -= 0.5f;
			else if (fapper.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) == -2) // Slothful
				do_cleaning -= 1.0f;

			if (xxx.is_ascetic(fapper))
				do_cleaning += 0.2f;

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			return Rand.Chance(do_cleaning);
		}

		// <summary>Handles after-sex trait and thought gain, and fluid creation. Initiator of the act (whore, rapist, female zoophile, etc) should be first.</summary>
		[SyncMethod]
		public static void Aftersex(Pawn pawn, Pawn partner, bool usedCondom = false, bool rape = false, bool isCoreLovin = false, xxx.rjwSextype sextype = xxx.rjwSextype.Vaginal)
		{
			bool bothInMap = false;

			if (!partner.Dead)
				bothInMap = pawn.Map != null && partner.Map != null; //Added by Hoge. false when called this function for despawned pawn: using for background rape like a kidnappee

			//Catch-all timer increase, for ensuring that pawns don't get stuck repeating jobs.

			pawn.rotationTracker.Face(partner.DrawPos);
			pawn.rotationTracker.FaceCell(partner.Position);

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			if (bothInMap)
			{
				if (!partner.Dead)
				{
					partner.rotationTracker.Face(pawn.DrawPos);
					partner.rotationTracker.FaceCell(pawn.Position);

					if (RJWSettings.sounds_enabled)
					{
						if (rape)
						{
							if (Rand.Value > 0.30f)
								LifeStageUtility.PlayNearestLifestageSound(partner, (ls) => ls.soundAngry, 1.2f);
							else
								LifeStageUtility.PlayNearestLifestageSound(partner, (ls) => ls.soundCall, 1.2f);

							pawn.Drawer.Notify_MeleeAttackOn(partner);
							partner.stances.StaggerFor(Rand.Range(10, 300));
						}
						else
							LifeStageUtility.PlayNearestLifestageSound(partner, (ls) => ls.soundCall);
					}
					if (sextype == xxx.rjwSextype.Vaginal || sextype == xxx.rjwSextype.DoublePenetration)
						if (xxx.is_Virgin(partner))
						{
							//TODO: bind virginity to parts of pawn
							/*
							string thingdef_penis_name = Genital_Helper.get_penis_all(pawn)?.def.defName ?? "";
							ThingDef thingdef_penis = null;

							Log.Message("SexUtility::thingdef_penis_name " + thingdef_penis_name);
							Log.Message("SexUtility::thingdef_penis 1 " + thingdef_penis);

							if (thingdef_penis_name != "")
								thingdef_penis = (from x in DefDatabase<ThingDef>.AllDefs where x.defName == thingdef_penis_name select x).RandomElement();
							Log.Message("SexUtility::thingdef_penis 2 " + thingdef_penis);

							partner.TakeDamage(new DamageInfo(DamageDefOf.Stab, 1, 999, -1.0f, null, xxx.genitals, thingdef_penis));
							*/
						}
				}

				if (RJWSettings.sounds_enabled)
					SoundDef.Named("Cum").PlayOneShot(!partner.Dead
						? new TargetInfo(partner.Position, pawn.Map)
						: new TargetInfo(pawn.Position, pawn.Map));

				if (rape)
				{
					if (Rand.Value > 0.30f)
						LifeStageUtility.PlayNearestLifestageSound(pawn, (ls) => ls.soundAngry, 1.2f);
					else
						LifeStageUtility.PlayNearestLifestageSound(pawn, (ls) => ls.soundCall, 1.2f);
				}
				else
					LifeStageUtility.PlayNearestLifestageSound(pawn, (ls) => ls.soundCall);
			}

			if (!usedCondom)
			{
				CumFilthGenerator(pawn);

				if (bothInMap && !isCoreLovin)
					CumFilthGenerator(partner);

				//apply cum to body:
				SemenHelper.calculateAndApplySemen(pawn, partner, sextype);

				//--Log.Message("SexUtility::aftersex( " + pawn_name + ", " + partner_name + " ) - checking satisfaction");

				if (!pawn.Dead && !partner.Dead)
				{
					PregnancyHelper.impregnate(pawn, partner, sextype);
					//The dead have no hediff, so no need to roll_to_catch; TO DO: add a roll_to_catch_from_corpse to std
					//--Log.Message("SexUtility::aftersex( " + pawn_name + ", " + partner_name + " ) - checking disease");
					if (!(xxx.is_animal(pawn) || xxx.is_animal(partner)))
						std_spreader.roll_to_catch(pawn, partner);

					xxx.TransferNutrition(pawn, partner, sextype);
				}
			}
			else
			{
				if (CondomUtility.UsedCondom != null) 
					GenSpawn.Spawn(CondomUtility.UsedCondom, pawn.Position, pawn.Map);
				CondomUtility.useCondom(pawn);
				CondomUtility.useCondom(partner);
			}

			if (rape && !partner.Dead)
				xxx.processBrokenPawn(partner);

			//Satisfy(pawn, partner, sextype, rape);

			//TODO: below is fucked up, unfuck it someday
			xxx.UpdateRecords(pawn, partner, sextype, rape, isCoreLovin);
			CheckTraitGain(pawn, partner, rape);
			CheckTraitGain(partner, pawn, rape);
		}

		// <summary>Solo acts.</summary>
		public static void Aftersex(Pawn pawn, xxx.rjwSextype sextype = xxx.rjwSextype.Masturbation)
		{
			IncreaseTicksToNextLovin(pawn);

			//if (Mod_Settings.sounds_enabled)
			//{
			//	SoundDef.Named("Cum").PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
			//}

			CumFilthGenerator(pawn);

			//apply cum to body:
			SemenHelper.calculateAndApplySemen(pawn, null, sextype);

			//--Log.Message("SexUtility::aftersex( " + pawn_name + ", " + partner_name + " ) - checking satisfaction");
			xxx.UpdateRecords(pawn, null, sextype);
			//Satisfy(pawn, null, sextype);

			// No traits from solo. Enable if some are edded. (Voyerism?)
			//check_trait_gain(pawn);
		}

		public static void Satisfy(Pawn pawn, Pawn partner, xxx.rjwSextype sextype, bool rape = false)
		{
			//--Log.Message("xxx::satisfy( " + pawn_name + ", " + partner_name + ", " + violent + "," + isCoreLovin + " ) called");

			if (pawn == null && partner == null) return; // Unlikely, but checking this here makes other checks simpler.

			//--Log.Message("xxx::satisfy( " + pawn_name + ", " + partner_name + ", " + violent + "," + isCoreLovin + " ) - calculate base satisfaction");
			// Base satisfaction is based on partner's ability
			float pawn_satisfaction = base_sat_per_fuck;
			float partner_satisfaction = base_sat_per_fuck;

			// Measure by own ability instead if masturbating.
			// Low pawn_ability usually means things like impaired manipulation, which should make masturbation less satisfactory.
			if (sextype == xxx.rjwSextype.Masturbation)
				pawn_satisfaction = base_sat_per_fuck;

			// In rape, pawn should always be the initiator
			bool pawn_is_raping = false;
			if (rape)
			{
				pawn_is_raping = true;
			}

			SatisfyPersonal(pawn, partner, sextype, rape, pawn_is_raping, pawn_satisfaction);
			SatisfyPersonal(partner, pawn, sextype, rape, false, partner_satisfaction);
			//--Log.Message("xxx::satisfy( " + pawn_name + ", " + partner_name + ", " + rape + " ) - pawn_satisfaction = " + pawn_satisfaction + ", partner_satisfaction = " + partner_satisfaction);
		}

		// Scales alien lifespan to human age. 
		// Some aliens have broken lifespans, that can be manually corrected here.
		public static int ScaleToHumanAge(Pawn pawn, int humanLifespan = 80)
		{
			float pawnAge = pawn.ageTracker.AgeBiologicalYearsFloat;
			float pawnLifespan = pawn.RaceProps.lifeExpectancy;

			if (pawn.def.defName == "Human") return (int)pawnAge; // Human, no need to scale anything.

			// Xen races, all broken and need a fix.
			if (pawn.def.defName.ContainsAny("Alien_Sergal", "Alien_SergalNME", "Alien_Xenn", "Alien_Racc", "Alien_Ferrex", "Alien_Wolvx", "Alien_Frijjid", "Alien_Fennex") && pawnLifespan >= 2000f)
			{
				pawnAge = Math.Min(pawnAge, 80f); // Clamp to 80.
				pawnLifespan = 80f;
			}
			if (pawn.def.defName.ContainsAny("Alien_Gnoll", "Alien_StripedGnoll") && pawnLifespan >= 2000f)
			{
				pawnAge = Math.Min(pawnAge, 60f); // Clamp to 60.
				pawnLifespan = 60f; // Mature faster than humans.
			}

			// Immortal races that mature at similar rate to humans.
			if (pawn.def.defName.ContainsAny("LF_Dragonia", "LotRE_ElfStandardRace", "Alien_Crystalloid", "Alien_CrystalValkyrie"))
			{
				pawnAge = Math.Min(pawnAge, 40f); // Clamp to 40 - never grow 'old'.
				pawnLifespan = 80f;
			}

			float age_scaling = humanLifespan / pawnLifespan;
			float scaled_age = pawnAge * age_scaling;

			if (scaled_age < 1)
				scaled_age = 1;

			return (int)scaled_age;
		}

		// Used in complex impregnation calculation. Pawns/animals with similar parts have better compatibility.
		public static float BodySimilarity(Pawn pawn, Pawn partner)
		{
			float size_adjustment = Mathf.Lerp(0.3f, 1.05f, 1.0f - Math.Abs(pawn.BodySize - partner.BodySize));

			//ModLog.Message(" Size adjustment: " + size_adjustment);

			List<BodyPartDef> pawn_partlist = new List<BodyPartDef> { };
			List<BodyPartDef> pawn_mismatched = new List<BodyPartDef> { };
			List<BodyPartDef> partner_mismatched = new List<BodyPartDef> { };

			//ModLog.Message("Checking compatibility for " + xxx.get_pawnname(pawn) + " and " + xxx.get_pawnname(partner));
			bool pawnHasHands = pawn.health.hediffSet.GetNotMissingParts().Any(part => part.IsInGroup(BodyPartGroupDefOf.RightHand) || part.IsInGroup(BodyPartGroupDefOf.LeftHand));

			foreach (BodyPartRecord part in pawn.RaceProps.body.AllParts)
			{
				pawn_partlist.Add(part.def);
			}
			float pawn_count = pawn_partlist.Count();

			foreach (BodyPartRecord part in partner.RaceProps.body.AllParts)
			{
				partner_mismatched.Add(part.def);
			}
			float partner_count = partner_mismatched.Count();

			foreach (BodyPartDef part in pawn_partlist)
			{
				if (partner_mismatched.Contains(part))
				{
					pawn_mismatched.Add(part);
					partner_mismatched.Remove(part);
				}
			}

			float pawn_mismatch = pawn_mismatched.Count() / pawn_count;
			float partner_mismatch = partner_mismatched.Count() / partner_count;

			//ModLog.Message("Body type similarity for " + xxx.get_pawnname(pawn) + " and " + xxx.get_pawnname(partner) + ": " + Math.Round(((pawn_mismatch + partner_mismatch) * 50) * size_adjustment, 1) + "%");

			return ((pawn_mismatch + partner_mismatch) / 2) * size_adjustment;
		}



		public static void SatisfyPersonal(Pawn pawn, Pawn partner, xxx.rjwSextype sextype, bool violent, bool pawn_is_raping, float satisfaction)
		{
			//--Log.Message("xxx::satisfy( " + pawn_name + ", " + partner_name + ", " + violent + "," + isCoreLovin + " ) - modifying partner satisfaction");
			if (pawn?.needs?.TryGetNeed<Need_Sex>() == null) return;

			// Bonus satisfaction from traits
			if (pawn != null && partner != null)
			{
				if (xxx.is_animal(partner) && xxx.is_zoophile(pawn))
				{
					satisfaction *= 1.5f;
				}
				if (partner.Dead && xxx.is_necrophiliac(pawn))
				{
					satisfaction *= 1.5f;
				}
			}

			// Calculate bonus satisfaction from quirks
			var quirkCount = Quirk.CountSatisfiedQuirks(pawn, partner, sextype, violent);
			satisfaction += quirkCount * base_sat_per_quirk;

			// Violent trait adjustments now handled in xxx.getSatisfactionMultiplier
			// HumpShroomAddiction now handles in XML using sex satisfaction stat

			// Apply sex satisfaction stat (min 0.1 default 1) as a modifier to total satisfaction
			satisfaction *= Math.Max(xxx.get_sex_satisfaction(pawn), 0.1f);

			// Apply extra multiplier for special circumstances
			satisfaction *= Math.Max(xxx.get_satisfaction_circumstance_multiplier(pawn, violent, pawn_is_raping), 0.1f);

			//Log.Message("xxx::satisfy( " + pawn + ", " + pawn + ", " + violent + "," + pawn_is_raping + "," + satisfaction + " ) - setting pawn joy");
			pawn.needs.TryGetNeed<Need_Sex>().CurLevel += satisfaction;

			if (pawn.needs.joy != null)
			{
				pawn.needs.joy.CurLevel += satisfaction * 0.50f;	// convert half of satisfaction to joy
			}

			if (quirkCount > 0)
			{
				Quirk.AddThought(pawn);
			}
		}

		[SyncMethod]
		private static void CheckTraitGain(Pawn pawn, Pawn partner, bool rape = false)
		{
			if (!xxx.has_traits(pawn) || pawn.records.GetValue(xxx.CountOfSex) <= 10) return;

			if (RJWSettings.AddTrait_Necrophiliac && !xxx.is_necrophiliac(pawn) && partner.Dead && pawn.records.GetValue(xxx.CountOfSexWithCorpse) > 0.5 * pawn.records.GetValue(xxx.CountOfSex))
			{
				pawn.story.traits.GainTrait(new Trait(xxx.necrophiliac));
				//Log.Message(xxx.get_pawnname(necro) + " aftersex, not necro, adding necro trait");
			}
			if (RJWSettings.AddTrait_Rapist && !xxx.is_rapist(pawn) && !xxx.is_masochist(pawn) && rape && pawn.records.GetValue(xxx.CountOfRapedHumanlikes) > 0.12 * pawn.records.GetValue(xxx.CountOfSex))
			{
				var chance = 0.5f;
				if (xxx.is_kind(pawn)) chance -= 0.25f;
				if (xxx.is_prude(pawn)) chance -= 0.25f;
				if (xxx.is_zoophile(pawn)) chance -= 0.25f; // Less interested in raping humanlikes.
				if (xxx.is_ascetic(pawn)) chance -= 0.2f;
				if (xxx.is_bloodlust(pawn)) chance += 0.2f;
				if (xxx.is_psychopath(pawn)) chance += 0.25f;

				//Rand.PopState();
				//Rand.PushState(RJW_Multiplayer.PredictableSeed());
				if (Rand.Chance(chance))
				{
					pawn.story.traits.GainTrait(new Trait(xxx.rapist));
					//--Log.Message(xxx.get_pawnname(pawn) + " aftersex, not rapist, adding rapist trait");
				}
			}
			if (RJWSettings.AddTrait_Zoophiliac && !xxx.is_zoophile(pawn) && xxx.is_animal(partner) && (pawn.records.GetValue(xxx.CountOfSexWithAnimals) + pawn.records.GetValue(xxx.CountOfSexWithInsects) > 0.5 * pawn.records.GetValue(xxx.CountOfSex)))
			{
				pawn.story.traits.GainTrait(new Trait(xxx.zoophile));
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(xxx.got_bred);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(xxx.got_anal_bred);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(xxx.got_groped);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(xxx.got_licked);
				//--Log.Message(xxx.get_pawnname(pawn) + " aftersex, not zoo, adding zoo trait");
			}
			if (RJWSettings.AddTrait_Nymphomaniac && !xxx.is_nympho(pawn))
			{
				if (pawn.health.hediffSet.HasHediff(HediffDef.Named("HumpShroomAddiction")))
				{
					pawn.story.traits.GainTrait(new Trait(xxx.nymphomaniac));
					//Log.Message(xxx.get_pawnname(pawn) + " is HumpShroomAddicted, not nymphomaniac, adding nymphomaniac trait");
				}
			}
		}

		// Checks if enough time has passed from previous lovin'.
		public static bool ReadyForLovin(Pawn pawn)
		{
			return Find.TickManager.TicksGame > pawn.mindState.canLovinTick;
		}

		// Checks if enough time has passed from previous search for a hookup.
		// Checks if hookups allowed during working hours, exlcuding nymphs
		public static bool ReadyForHookup(Pawn pawn)
		{
			if (!xxx.is_nympho(pawn) && RJWHookupSettings.NoHookupsDuringWorkHours && ((pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything) == TimeAssignmentDefOf.Work) return false;
			return Find.TickManager.TicksGame > CompRJW.Comp(pawn).NextHookupTick;
		}

		private static void IncreaseTicksToNextLovin(Pawn pawn)
		{
			if (pawn == null || pawn.Dead) return;
			int currentTime = Find.TickManager.TicksGame;
			if (pawn.mindState.canLovinTick <= currentTime)
				pawn.mindState.canLovinTick = currentTime + GenerateMinTicksToNextLovin(pawn);
		}

		[SyncMethod]
		public static int GenerateMinTicksToNextLovin(Pawn pawn)
		{
			if (DebugSettings.alwaysDoLovin) return 100;
			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());

			float interval = AgeConfigDef.Instance.lovinIntervalHoursByAge.Evaluate(ScaleToHumanAge(pawn));
			float rinterval = Math.Max(0.5f, Rand.Gaussian(interval, 0.3f));

			float tick = 1.0f;

			// Nymphs automatically get the tick increase from the trait influence on sex drive.
			if (xxx.is_animal(pawn))
			{
				//var mateMtbHours = pawn.RaceProps.mateMtbHours / 24 * 60000;
				//if (mateMtbHours > 0)
				//	interval = mateMtbHours
				tick = 0.75f;
			}
			else if (xxx.is_prude(pawn))
				tick = 1.5f;

			if (pawn.Has(Quirk.Vigorous))
				tick *= 0.8f;

			float sex_drive = xxx.get_sex_drive(pawn);
			if (sex_drive <= 0.05f)
				sex_drive = 0.05f;

			return (int)(tick * rinterval * (2500.0f / sex_drive));
		}

		public static void IncreaseTicksToNextHookup(Pawn pawn)
		{
			if (pawn == null || pawn.Dead)
				return;

			// There are 2500 ticks per rimworld hour. Sleeping an hour between checks seems like a good start.
			// We could get fancier and weight it by sex drive and stuff, but would people even notice?
			const int TicksBetweenHookups = 2500;

			int currentTime = Find.TickManager.TicksGame;
			CompRJW.Comp(pawn).NextHookupTick = currentTime + TicksBetweenHookups;
		}

		// <summary>
		// Determines the sex type and handles the log output.
		// "Pawn" should be initiator of the act (rapist, whore, etc), "Partner" should be the target.
		// </summary>
		public static void ProcessSex(Pawn pawn, Pawn partner, bool usedCondom = false, bool rape = false, bool isCoreLovin = false, bool whoring = false, xxx.rjwSextype sextype = xxx.rjwSextype.None)
		{
			//Log.Message("usedCondom=" + usedCondom);
			if (pawn == null || partner == null)
			{
				if (pawn == null)
					ModLog.Error("[SexUtility] ERROR: pawn is null.");
				if (partner == null)
					ModLog.Error("[SexUtility] ERROR: partner is null.");
				return;
			}

			// Re-draw clothes.
			if (xxx.is_human(pawn))
				pawn.Drawer.renderer.graphics.ResolveApparelGraphics();
			if (xxx.is_human(partner))
				partner.Drawer.renderer.graphics.ResolveApparelGraphics();

			IncreaseTicksToNextLovin(pawn);
			IncreaseTicksToNextLovin(partner);

			rape = rape || pawn?.CurJob?.def == xxx.gettin_raped || (!partner.Dead && partner.CurJob?.def == xxx.gettin_raped); // Double-checking.

			Pawn receiving = partner;
			if(sextype == xxx.rjwSextype.None)
				sextype = DetermineSextype(pawn, partner, rape, whoring, receiving);
			Aftersex(pawn, partner, usedCondom, rape, isCoreLovin, sextype);

			//--Log.Message("SexUtility::processsex( " + pawn_name + ", " + partner_name + " ) - checking thoughts");
			xxx.think_about_sex(pawn, partner, receiving == pawn, rape, sextype, isCoreLovin, whoring);
			xxx.think_about_sex(partner, pawn, receiving == partner, rape, sextype, isCoreLovin, false);

			pawn.Drawer.Notify_MeleeAttackOn(partner);
		}

		public static Dictionary<string, bool> DetermineSexParts(Pawn pawn)
		{
			var result = new Dictionary<string, bool>();
			var pawnpartBPR = Genital_Helper.get_genitalsBPR(pawn);
			var pawnparts = Genital_Helper.get_PartsHediffList(pawn, pawnpartBPR);

			bool pawnHasAnus = Genital_Helper.has_anus(pawn) && !Genital_Helper.anus_blocked(pawn);

			bool pawnHasBreasts = Genital_Helper.has_breasts(pawn) && !Genital_Helper.breasts_blocked(pawn);
			bool pawnHasBigBreasts = pawnHasBreasts && Genital_Helper.can_do_breastjob(pawn);

			bool pawnHasVagina = Genital_Helper.has_vagina(pawn, pawnparts) && !Genital_Helper.vagina_blocked(pawn);

			bool pawnHasPenis = (Genital_Helper.has_penis_fertile(pawn, pawnparts) || Genital_Helper.has_penis_infertile(pawn, pawnparts) || Genital_Helper.has_ovipositorF(pawn, pawnparts)) && !Genital_Helper.penis_blocked(pawn);
			bool pawnHasBigPenis = (pawnHasPenis && pawnparts.Where(x => x.Severity > 0.8f && x.def.defName.ToLower().Contains("penis")).Any());
			bool pawnHasMultiPenis = pawnHasPenis && Genital_Helper.has_multipenis(pawn, pawnparts);

			bool pawnHasTail = Genital_Helper.has_tail(pawn);
			bool pawnHasMouth = Genital_Helper.has_mouth(pawn) && !Genital_Helper.oral_blocked(pawn);

			bool pawnHasHands = pawn.health.hediffSet.GetNotMissingParts().Any(part => part.IsInGroup(BodyPartGroupDefOf.RightHand) || part.IsInGroup(BodyPartGroupDefOf.LeftHand)) && !Genital_Helper.hands_blocked(pawn);
			if (pawnHasHands)
				pawnHasHands = pawn.health?.capacities?.GetLevel(PawnCapacityDefOf.Manipulation) > 0;

			result.Add("HasMouth", pawnHasMouth);
			result.Add("HasAnus", pawnHasAnus);
			result.Add("HasBreasts", pawnHasBreasts);
			result.Add("HasBigBreasts", pawnHasBigBreasts);
			result.Add("HasVagina", pawnHasVagina);
			result.Add("HasPenis", pawnHasPenis);
			result.Add("HasBigPenis", pawnHasBigPenis); // masturbation autofellatio, self breastjob
			result.Add("HasMultiPenis", pawnHasMultiPenis);
			result.Add("HasHands", pawnHasHands);
			result.Add("HasTail", pawnHasTail); // masturbation, tail fuck

			return result;
		}

		[SyncMethod]
		public static SexProps SelectSextype(Pawn pawn, Pawn partner, bool rape, bool whoring, Pawn receiving, List<float> sexScores = null)
		{
			if (sexScores.NullOrEmpty())
				sexScores = DetermineSexScores(pawn, partner, rape, whoring, receiving);

			// Bit of randomization..
			RandomizeSexTypes(ref sexScores);

			List<RulePackDef> extraSentencePacks = new List<RulePackDef>();
			InteractionDef dictionaryKey = null;

			int typeIndex1 = 0;
			int typeIndex2 = 0;
			float maxValue = 0;
			Pawn giving = pawn;

			if (xxx.is_animal(pawn) || xxx.is_animal(partner)) //bestiality
			{
				typeIndex2 = 2;
			}
			else if (rape) // rape
			{
				typeIndex2 = 1;
			}

			//1st pass, try to get custom defs
			{
				maxValue = sexScores.Max();
				typeIndex1 = sexScores.IndexOf(maxValue);

				if (true)//test random interaction selection
				{
					List<InteractionDef> rnwintdefs = DefDatabase<InteractionDef>.AllDefsListForReading;
					List<InteractionDef> validintdefs = new List<InteractionDef>();
					var f1 = rnwintdefs.Where(x => x.HasModExtension<InteractionExtension>());
					foreach (InteractionDef d in f1)
					{
						var f2 = d.GetModExtension<InteractionExtension>();
						//Log.Message("SelectSextype search inter  " + f2.rjwSextype + " " + d.ToStringSafe());
						//Log.Message("SelectSextype search inter  " + f2.rjwSextype + " " + xxx.rjwSextype.Vaginal.ToStringSafe());
						if (f2.rjwSextype == sexActs.TryGetValue(dictionarykeys[typeIndex1, 0]).ToStringSafe() && (
							(f2.sextype1 == "Normal" && typeIndex2 == 0) ||
							(f2.sextype1 == "Rape" && typeIndex2 == 1) ||
							(f2.sextype1 == "Bestiality" && typeIndex2 == 2)
							))
							validintdefs.Add(d);
					}

					//ModLog.Message("SelectSextype validintdefs count " + validintdefs.Count);
					if (!validintdefs.NullOrEmpty())
						dictionaryKey = validintdefs.RandomElement();
					//Log.Message("SelectSextype dictionaryKey " + dictionaryKey);
				}
			}

			//2nd pass, try to get hardcoded defs, excluding nulls
			//use default
			{
				if (dictionaryKey == null)
				{
					//remove scores for null interactions to avoid errors
					for (int index = 0; index < sexScores.Count; index++)
					{
						if (dictionarykeys[index, typeIndex2] == null)
						{
							sexScores[index] = 0;
						}
					}

					maxValue = sexScores.Max();
					typeIndex1 = sexScores.IndexOf(maxValue);
					dictionaryKey = dictionarykeys[typeIndex1, typeIndex2];
				}
			}

			if (sexScores.IndexOf(maxValue) % 2 != 0 && !rape)
			{
				giving = partner;
				receiving = pawn;
			}
			if (!(maxValue > 0f))
			{
				ModLog.Warning(" ERROR: No available sex types for " + xxx.get_pawnname(pawn) + " and " + xxx.get_pawnname(partner));
				ModLog.Warning(" ERROR: your pawns missing parts hediffs?");
				return new SexProps(pawn, partner, xxx.rjwSextype.None, rape, giving, receiving, null, null);
			}

			// Override for fellatio with beakjob.
			if (dictionaryKey == fellatio)
			{
				if (GetPawnBodyPart(pawn, "Beak") != null && pawn == giving || GetPawnBodyPart(partner, "Beak") != null && pawn == receiving)
				{
					dictionaryKey = beakjob;
				}
			}

			xxx.rjwSextype sextype = xxx.rjwSextype.None;
			try
			{
				sextype = sexActs[dictionaryKey];
			}
			catch
			{
				if (RJWSettings.DevMode) ModLog.Message("SelectSextype no default interaction for " + dictionaryKey.defName + ", trying find sextype");
				var f2 = dictionaryKey.GetModExtension<InteractionExtension>();
				if (!f2.rjwSextype.NullOrEmpty())
					sextype = ParseHelper.FromString<xxx.rjwSextype>(f2.rjwSextype);

				if (RJWSettings.DevMode) ModLog.Message("SelectSextype sextype " + sextype);
			}

			if (sextype == xxx.rjwSextype.None)
				ModLog.Warning(" ERROR: sextype is None for interactiondef " + dictionaryKey);

			// Override for mechanoid implanting. Add the defNames for species that should be allowed to do it.
			// Currently only includes the core mechanoids, plus some from Orion's More Mechanoids mod.
			if (pawn.kindDef.race.defName.ContainsAny("Mech_"))
			{
				dictionaryKey = mechImplant;
			}

			// Override for corpse violation.
			if (pawn.CurJob.def == xxx.RapeCorpse)
				dictionaryKey = violateCorpse;

			string extraSentenceRulePack = SexRulePackGet(dictionaryKey);
			return new SexProps(pawn, partner, sextype, rape, giving, receiving, extraSentenceRulePack, dictionaryKey);
		}

		public static void LogSextype(Pawn giving, Pawn receiving, string rulepack, InteractionDef dictionaryKey)
		{
			List<RulePackDef> extraSentencePacks = new List<RulePackDef>();
			if (!rulepack.NullOrEmpty())
				extraSentencePacks.Add(RulePackDef.Named(rulepack));
			LogSextype(giving, receiving, extraSentencePacks, dictionaryKey);
		}

		public static void LogSextype(Pawn giving, Pawn receiving, List<RulePackDef> extraSentencePacks, InteractionDef dictionaryKey)
		{
			if (extraSentencePacks.NullOrEmpty())
			{
				extraSentencePacks = new List<RulePackDef>();
				string extraSentenceRulePack = SexRulePackGet(dictionaryKey);
				if (!extraSentenceRulePack.NullOrEmpty())
					extraSentencePacks.Add(RulePackDef.Named(extraSentenceRulePack));
			}
			PlayLogEntry_Interaction playLogEntry = new PlayLogEntry_Interaction(dictionaryKey, giving, receiving, extraSentencePacks);
			Find.PlayLog.Add(playLogEntry);
		}

		[SyncMethod]
		public static string SexRulePackGet(InteractionDef dictionaryKey)
		{
			var extension = dictionaryKey.GetModExtension<InteractionExtension>();
			string extraSentenceRulePack = "";
			if (!extension.rulepack_defs.NullOrEmpty())
			{
				extraSentenceRulePack = extension.rulepack_defs.RandomElement();
			}

			try
			{
				if (RulePackDef.Named(extraSentenceRulePack) != null)
				{
				}
			}
			catch
			{
				ModLog.Warning("RulePackDef " + extraSentenceRulePack + " for " + dictionaryKey + " not found");
				extraSentenceRulePack = "";
			}
			return extraSentenceRulePack;
		}

		public static xxx.rjwSextype DetermineSextype(Pawn pawn, Pawn partner, bool rape, bool whoring, Pawn receiving)
		{
			var props = SelectSextype(pawn, partner, rape, whoring, receiving);
			LogSextype(props.Giver, props.Reciever, props.RulePack, props.DictionaryKey);
			return props.SexType;
		}

		public static List<float> DetermineSexScores(Pawn pawn, Pawn partner, bool rape, bool whoring, Pawn receiving)
		{
			//--ModLog.Message("SexUtility::processSex is called for pawn" + xxx.get_pawnname(pawn) + " and partner " + xxx.get_pawnname(partner));
			var pawnDic = DetermineSexParts(pawn);
			var partenerDic = DetermineSexParts(partner);

			bool pawnHasMouth = pawnDic.TryGetValue("HasMouth");
			bool pawnHasAnus = pawnDic.TryGetValue("HasAnus");
			bool pawnHasBigBreasts = pawnDic.TryGetValue("HasBigBreasts");
			bool pawnHasVagina = pawnDic.TryGetValue("HasVagina");
			bool pawnHasPenis = pawnDic.TryGetValue("HasPenis");
			bool pawnHasMultiPenis = pawnDic.TryGetValue("HasMultiPenis");
			bool pawnHasHands = pawnDic.TryGetValue("HasHands");

			bool partnerHasMouth = partenerDic.TryGetValue("HasMouth");
			bool partnerHasAnus = partenerDic.TryGetValue("HasAnus");
			bool partnerHasBigBreasts = partenerDic.TryGetValue("HasBigBreasts");
			bool partnerHasVagina = partenerDic.TryGetValue("HasVagina");
			bool partnerHasPenis = partenerDic.TryGetValue("HasPenis");
			bool partnerHasMultiPenis = partenerDic.TryGetValue("HasMultiPenis");
			bool partnerHasHands = partenerDic.TryGetValue("HasHands");

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());

			/*Things to keep in mind:
			 - Both the initiator and the partner can be female, male, or futa.
			 - Can be rape or consensual.
			 - Includes pawns with blocked or no genitalia.
			
			 Need to add support here when new types get added.
			 Types to be added: 69, spooning...?

			 This would be much 'better' code as arrays, but that'd hurt readability and make it harder to modify.
			 If this weren't 3.5, I'd use tuples.*/

			// Range 1.0 to 0.0 [100% to 0%].
			float vagiIni = RJWPreferenceSettings.vaginal;				// Vaginal
			float VagiRec = RJWPreferenceSettings.vaginal;				// Vaginal - receiving
			float analIni = RJWPreferenceSettings.anal;					// Anal
			float analRec = RJWPreferenceSettings.anal;					// Anal - receiving
			float cunnIni = RJWPreferenceSettings.cunnilingus;			// Cunnilingus
			float cunnRec = RJWPreferenceSettings.cunnilingus;			// Cunnilingus - receiving
			float rimmIni = RJWPreferenceSettings.rimming;				// Rimming
			float rimmRec = RJWPreferenceSettings.rimming;				// Rimming - receiving
			float fellIni = RJWPreferenceSettings.fellatio;				// Fellatio
			float fellRec = RJWPreferenceSettings.fellatio;				// Fellatio - receiving
			float doubIni = RJWPreferenceSettings.double_penetration;	// DoublePenetration
			float doubRec = RJWPreferenceSettings.double_penetration;	// DoublePenetration - receiving
			float bresIni = RJWPreferenceSettings.breastjob;			// Breastjob
			float bresRec = RJWPreferenceSettings.breastjob;			// Breastjob - receiving
			float handIni = RJWPreferenceSettings.handjob;				// Handjob
			float handRec = RJWPreferenceSettings.handjob;				// Handjob - receiving
			float footIni = RJWPreferenceSettings.footjob;				// Footjob
			float footRec = RJWPreferenceSettings.footjob;				// Footjob - receiving
			float fingIni = RJWPreferenceSettings.fingering;			// Fingering
			float fingRec = RJWPreferenceSettings.fingering;			// Fingering - receiving
			float scisIni = RJWPreferenceSettings.scissoring;			// Scissoring
			float scisRec = RJWPreferenceSettings.scissoring;			// Scissoring - receiving
			float mutuIni = RJWPreferenceSettings.mutual_masturbation;	// MutualMasturbation
			float mutuRec = RJWPreferenceSettings.mutual_masturbation;	// MutualMasturbation - receiving
			float fistIni = RJWPreferenceSettings.fisting;				// Fisting
			float fistRec = RJWPreferenceSettings.fisting;				// Fisting - receiving
			float sixtIni = RJWPreferenceSettings.sixtynine;			// 69
			float sixtRec = RJWPreferenceSettings.sixtynine;			// 69 - receiving

			string pawn_quirks = CompRJW.Comp(pawn).quirks.ToString();
			string partner_quirks = CompRJW.Comp(partner).quirks.ToString();

			// Modifiers > 1.0f = higher chance of being picked
			// Modifiers < 1.0f = lower chance of being picked
			// 0 = disables types.

			// Pawn does not need sex, or is not horny. Mostly whores, sexbots, etc.
			if (xxx.need_some_sex(pawn) < 1.0f)
			{
				vagiIni *= 0.6f;
				analIni *= 0.6f;
				cunnRec *= 0.6f;
				rimmRec *= 0.6f;
				fellRec *= 0.6f;
				doubIni *= 0.6f;
				bresRec *= 0.6f;
				handRec *= 0.6f;
				footRec *= 0.6f;
				fingRec *= 0.6f;
				sixtIni *= 0.6f;
				sixtRec *= 0.6f;
			}

			// Adjusts initial chances
			if (pawnHasPenis)
			{
				vagiIni *= 1.5f;
				analIni *= 1.5f;
				fellRec *= 1.5f;
				doubIni *= 1.5f;
				if (partnerHasVagina)
				{
					fistRec *= 0.5f;
					rimmIni *= 0.8f;
					rimmRec *= 0.5f;
				}
			}
			else if (pawnHasVagina)
			{
				VagiRec *= 1.2f;
				scisRec *= 1.2f;
			}

			//Size adjustments. Makes pawns reluctant to have penetrative sex if there's large size difference.
			if (partner.BodySize > pawn.BodySize * 2 && !rape && !xxx.is_animal(pawn))
			{
				VagiRec *= 0.6f;
				analRec *= 0.6f;
				fistRec *= 0.2f;
				sixtIni *= 0.2f;
				sixtRec *= 0.2f;
			}
			else if (pawn.BodySize > partner.BodySize * 2 && !rape && !xxx.is_animal(pawn) && !xxx.is_psychopath(pawn))
			{
				vagiIni *= 0.6f;
				analIni *= 0.6f;
				fistIni *= 0.3f;
				sixtIni *= 0.2f;
				sixtRec *= 0.2f;
			}

			if (partner.Dead || partner.Downed || !partner.health.capacities.CanBeAwake) // This limits options a lot, for obvious reason.
			{
				VagiRec = 0f;
				analRec = 0f;
				cunnIni *= 0.3f;
				cunnRec = 0f;
				rimmIni *= 0.1f;
				rimmRec = 0f;
				fellRec *= 0.2f;
				doubRec = 0f;
				bresRec = 0f;
				handRec = 0f;
				footRec = 0f;
				fingRec = 0f;
				fingIni *= 0.5f;
				scisIni *= 0.2f;
				scisRec = 0f;
				mutuIni = 0f;
				mutuRec = 0f;
				fistRec = 0f;
				sixtRec = 0f;
				if (pawnHasPenis)
				{
					sixtIni *= 0.5f; // Can facefuck the unconscious (or corpse). :/
				}
				else
				{
					sixtIni = 0f;
				}
				if (partner.Dead)
				{
					fellIni = 0f;
					handIni = 0f;
					footIni = 0f;
					bresIni = 0f;
					fingIni = 0f;
					fistIni *= 0.2f; // Fisting a corpse? Whatever floats your boat, I guess.
				}
				else
				{
					fellIni *= 0.4f;
					handIni *= 0.5f;
					footIni *= 0.2f;
					bresIni *= 0.2f;
					fistIni *= 0.6f;
				}
			}

			if (rape)
			{
				// This makes most types less likely to happen during rape, but doesn't disable them. 
				// Things like forced blowjob can happen, so it shouldn't be impossible in rjw.
				VagiRec *= 0.5f; //Forcing vaginal on male.
				analRec *= 0.3f; //Forcing anal on male.
				cunnIni *= 0.3f; //Forced cunnilingus.
				cunnRec *= 0.6f;
				rimmIni *= 0.1f;
				fellIni *= 0.4f;
				doubRec *= 0.2f; //Rapist forcing the target to double-penetrate her - unlikely.
				bresIni *= 0.2f;
				bresRec *= 0.2f;
				handIni *= 0.6f;
				handRec *= 0.2f;
				footIni *= 0.2f;
				footRec *= 0.1f;
				fingIni *= 0.8f;
				fingRec *= 0.1f;
				scisIni *= 0.6f;
				scisRec *= 0.1f;
				mutuIni = 0f;
				mutuRec = 0f;
				fistIni *= 1.2f;
				fistRec = 0f;
				sixtIni *= 0.5f;
				sixtRec = 0f;
			}

			if (xxx.is_animal(pawn))
			{
				if (pawn.relations.DirectRelationExists(PawnRelationDefOf.Bond, partner))
				{	//Bond animals
					VagiRec *= 1.8f; //Presenting
					analRec *= 1.2f;
					fellIni *= 1.2f;
					cunnIni *= 1.2f;
				}
				else
				{
					VagiRec *= 0.3f;
					analRec *= 0.3f;
				}
				vagiIni *= 1.8f;
				if (Genital_Helper.has_ovipositorF(pawn) || Genital_Helper.has_ovipositorM(pawn))// insect dont care where to lay eggs?
					analIni *= 1.8f;
				else
					analIni *= 0.9f;
				cunnRec *= 0.2f;
				rimmRec *= 0.1f;
				fellRec *= 0.1f;
				doubIni *= 0.6f;
				doubRec *= 0.1f;
				bresIni = 0f;
				bresRec *= 0.1f;
				handIni *= 0.4f; //Enabled for primates.
				handRec *= 0.1f;
				footIni = 0f;
				footRec *= 0.1f;
				fingIni *= 0.3f; //Enabled for primates.
				fingRec *= 0.2f;
				scisIni *= 0.2f;
				scisRec *= 0.1f;
				mutuIni *= 0.1f;
				mutuRec *= 0.1f;
				fistIni *= 0.2f; //Enabled for primates...
				fistRec *= 0.6f;
				sixtIni *= 0.2f;
				sixtRec *= 0.2f;
			}

			if (xxx.is_animal(partner)) // Zoophilia and animal-on-animal
			{
				if (pawn.Faction != partner.Faction && rape) // Wild animals && animals from other factions
				{
					cunnRec *= 0.1f; // Wild animals bite, colonists should be smart enough to not try to force oral from them.
					rimmRec *= 0.1f;
					fellRec *= 0.1f;
				}
				else
				{
					cunnRec *= 0.5f;
					rimmRec *= 0.4f;
					fellRec *= 0.4f;
				}
				cunnIni *= 0.7f;
				rimmIni *= 0.1f;
				fellIni *= 1.2f;
				doubIni *= 0.6f;
				doubRec *= 0.1f;
				bresIni *= 0.3f; //Giving a breastjob to animals - unlikely.
				bresRec = 0f;
				handIni *= 1.2f;
				handRec *= 0.4f; //Animals are not known for giving handjobs, but enabled for primates and such.
				footIni *= 0.3f;
				footRec = 0f;
				fingIni *= 0.8f;
				fingRec *= 0.2f; //Enabled for primates.
				scisIni *= 0.1f;
				scisRec = 0f;
				mutuIni *= 0.6f;
				mutuRec *= 0.1f;
				fistIni *= 0.6f;
				fistRec *= 0.1f;
				sixtIni *= 0.2f;
				sixtRec *= 0.2f;
			}

			//Quirks
			if (pawn_quirks.Contains("Podophile")) // Foot fetish
			{
				footIni *= 2.0f;
				footRec *= 2.5f;
			}
			if (partner_quirks.Contains("Podophile"))
			{
				footIni *= 2.5f;
				footRec *= 2.0f;
			}
			if (pawn_quirks.Contains("Impregnation fetish") && (PregnancyHelper.CanImpregnate(pawn, partner) || PregnancyHelper.CanImpregnate(partner, pawn)))
			{
				vagiIni *= 2.5f;
				VagiRec *= 2.5f;
			}

			if (whoring) // Paid sex
			{
				VagiRec *= 1.5f;
				analIni *= 0.7f; //Some customers may pay for this.
				analRec *= 1.2f;
				cunnIni *= 1.2f;
				cunnRec *= 0.3f; //Customer paying to lick the whore - uncommon.
				rimmRec *= 0.2f;
				fellIni *= 1.5f; //Classic.
				fellRec *= 0.2f;
				doubIni *= 0.8f;
				doubRec *= 1.2f;
				bresIni *= 1.2f;
				bresRec *= 0.1f;
				handIni *= 1.5f;
				handRec *= 0.1f;
				footIni *= 0.6f;
				footRec *= 0.1f;
				fingIni *= 0.6f;
				fingRec *= 0.2f;
				scisRec *= 0.2f;
				mutuIni *= 0.2f;
				mutuRec *= 0.2f;
				fistIni *= 0.6f;
				fistRec *= 0.7f;
				sixtIni *= 0.7f;
				sixtRec *= 0.7f;
			}

			// Pawn lacks vagina, disable related types.
			if (!pawnHasVagina)
			{
				VagiRec = 0f;
				cunnRec = 0f;
				doubRec = 0f;
				fingRec = 0f;
				scisIni = 0f;
				scisRec = 0f;
			}
			if (!partnerHasVagina)
			{
				vagiIni = 0f;
				cunnIni = 0f;
				doubIni = 0f;
				fingIni = 0f;
				scisIni = 0f;
				scisRec = 0f;
			}

			// Pawn lacks penis, disable related types.
			if (!pawnHasPenis)
			{
				vagiIni = 0f;
				analIni = 0f;
				fellRec = 0f;
				doubIni = 0f;
				bresRec = 0f;
				handRec = 0f;
				footRec = 0f;
			}
			else if (pawnHasMultiPenis && partnerHasVagina && partnerHasAnus)
			{
				// Pawn has multi-penis and can use it. Single-penetration chance down.
				vagiIni *= 0.8f;
				analIni *= 0.8f;
				doubIni *= 1.5f;
			}
			else
			{
				doubIni = 0f;
			}

			if (!partnerHasPenis)
			{
				VagiRec = 0f;
				analRec = 0f;
				fellIni = 0f;
				doubRec = 0f;
				bresIni = 0f;
				handIni = 0f;
				footIni = 0f;
			}
			else if (partnerHasMultiPenis && pawnHasVagina && pawnHasAnus)
			{
				// Pawn has multi-penis and can use it. Single-penetration chance down.
				VagiRec *= 0.8f;
				analRec *= 0.8f;
				doubRec *= 1.5f;
			}
			else
			{
				doubRec = 0f;
			}

			// One pawn lacks genitalia: no mutual masturbation or 69.
			if (!(pawnHasPenis || pawnHasVagina) || !(partnerHasPenis || partnerHasVagina))
			{
				mutuIni = 0f;
				mutuRec = 0f;
				sixtIni = 0f;
				sixtRec = 0f;
			}

			// Pawn lacks anus... 
			if (!pawnHasAnus)
			{
				analRec = 0f;
				rimmRec = 0f;
				doubRec = 0f;
				fistRec = 0f;
			}
			if (!partnerHasAnus)
			{
				analIni = 0f;
				rimmIni = 0f;
				doubIni = 0f;
				fistIni = 0f;
			}

			// Pawn lacks big enough boobs
			if (!pawnHasBigBreasts)
			{
				bresIni = 0f;
			}
			if (!partnerHasBigBreasts)
			{
				bresRec = 0f;
			}

			// Pawn lacks hands
			if (!pawnHasHands)
			{
				handIni = 0f;
				fingIni = 0f;
				mutuIni = 0f;
				fistIni = 0f;
			}
			if (!partnerHasHands)
			{
				handRec = 0f;
				fingRec = 0f;
				mutuRec = 0f;
				fistRec = 0f;
			}

			// Pawn lacks mouth
			if (!pawnHasMouth)
			{
				cunnIni = 0f;
				rimmIni = 0f;
				fellIni = 0f;
				sixtIni = 0f;
			}
			if (!partnerHasMouth)
			{
				cunnIni = 0f;
				rimmIni = 0f;
				fellIni = 0f;
				sixtIni = 0f;
			}

			List<float> sexScores = new List<float> {
				vagiIni, VagiRec,	//  0,  1
				analIni, analRec,	//  2,  3
				cunnIni, cunnRec,	//  4,  5
				rimmIni, rimmRec,	//  6,  7 
				fellIni, fellRec,	//  8,  9
				doubIni, doubRec,	// 10, 11
				bresIni, bresRec,	// 12, 13
				handIni, handRec,	// 14, 15
				footIni, footRec,	// 16, 17
				fingIni, fingRec,	// 18, 19
				scisIni, scisRec,	// 20, 21
				mutuIni, mutuRec,	// 22, 23
				fistIni, fistRec,	// 24, 25
				sixtIni, sixtRec	// 26, 27
			};

			//override for mechanoids, since they dont have parts/hediffs
			if (pawn.kindDef.race.defName.ContainsAny("Mech_") || xxx.is_mechanoid(pawn))
			{
				if (partnerHasVagina)
				{
					sexScores[0] = 1;
					sexScores[1] = 1;
				}
				else if(partnerHasAnus)
				{
					sexScores[2] = 1;
					sexScores[3] = 1;
				}
				else // if(partnerHasMouth) fallback
				{
					sexScores[8] = 1;
					sexScores[9] = 1;
				}
			}


			return sexScores;
		}

		[SyncMethod]
		private static void RandomizeSexTypes(ref List<float> sexTypes)
		{
			for (int i = 0; i < sexTypes.Count; i++)
			{
				sexTypes[i] = Rand.Range(0f, sexTypes[i]);
			}
		}

		[SyncMethod]
		public static void Sex_Beatings(Pawn pawn, Pawn partner, bool isRape = false)
		{
			if ((xxx.is_animal(pawn) && xxx.is_animal(partner)))
				return;

			//dont remember what it does, probably manhunter stuff or not? disable and wait reports
			//if (!xxx.is_human(pawn))
			//	return;

			//If a pawn is incapable of violence/has low melee, they most likely won't beat their partner
			if (pawn.skills?.GetSkill(SkillDefOf.Melee).Level < 1)
				return;

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			float rand_value = Rand.Value;
			//float rand_value = RJW_Multiplayer.RJW_MP_RAND();
			float victim_pain = partner.health.hediffSet.PainTotal;
			// bloodlust makes the aggressor more likely to hit the prisoner
			float beating_chance = xxx.config.base_chance_to_hit_prisoner * (xxx.is_bloodlust(pawn) ? 1.5f : 1.0f);
			// psychopath makes the aggressor more likely to hit the prisoner past the significant_pain_threshold
			float beating_threshold = xxx.is_psychopath(pawn) ? xxx.config.extreme_pain_threshold : pawn.HostileTo(partner) ? xxx.config.significant_pain_threshold : xxx.config.minor_pain_threshold;

			//--Log.Message("roll_to_hit:  rand = " + rand_value + ", beating_chance = " + beating_chance + ", victim_pain = " + victim_pain + ", beating_threshold = " + beating_threshold);
			if ((victim_pain < beating_threshold && rand_value < beating_chance) || (rand_value < (beating_chance / 2) && xxx.is_bloodlust(pawn)))
			{
				Sex_Beatings_Dohit(pawn, partner, isRape);
			}
		}

		public static void Sex_Beatings_Dohit(Pawn pawn, Pawn partner, bool isRape = false)
		{
			//--Log.Message("   done told her twice already...");
			if (InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out Verb v))
			{
				//Log.Message("   v. : " + v);
				//Log.Message("   v.GetDamageDef : " + v.GetDamageDef());
				//Log.Message("   v.v.tool - " + v.tool.label);
				//Log.Message("   v.v.tool.power base - " + v.tool.power);
				var orgpower = v.tool.power;
				//in case something goes wrong
				try
				{
					//Log.Message("   v.v.tool.power base - " + v.tool.power);
					if (RJWSettings.gentle_rape_beating || !isRape)
					{
						v.tool.power = 0;
						//partner.stances.stunner.StunFor(600, pawn);
					}
					//Log.Message("   v.v.tool.power mod - " + v.tool.power);
					pawn.meleeVerbs.TryMeleeAttack(partner, v);
				}
				catch
				{ }
				v.tool.power = orgpower;
				//Log.Message("   v.v.tool.power reset - " + v.tool.power);
			}
		}

		// Overrides the current clothing. Defaults to nude, with option to keep headgear on.
		public static void DrawNude(Pawn pawn, bool keep_hat_on = false)
		{
			if (!xxx.is_human(pawn)) return;
			if (pawn.Map != Find.CurrentMap) return;
			if (RJWPreferenceSettings.sex_wear == RJWPreferenceSettings.Clothing.Clothed) return;

			//undress
			pawn.Drawer.renderer.graphics.ClearCache();
			pawn.Drawer.renderer.graphics.apparelGraphics.Clear();
			//pawn.Drawer.renderer.graphics.ResolveApparelGraphics();

			//add "clothes"
			foreach (Apparel current in pawn.apparel.WornApparel.Where(x
				=> x.def is bondage_gear_def
				|| x.def.thingCategories.Any(x=> x.defName.ToLower().ContainsAny("vibrator", "piercing", "strapon"))
				|| RJWPreferenceSettings.sex_wear == RJWPreferenceSettings.Clothing.Headgear
				|| keep_hat_on
				&& (x.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead)
					|| x.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))))
			{
				ApparelGraphicRecord item;
				if (ApparelGraphicRecordGetter.TryGetGraphicApparel(current, pawn.story.bodyType, out item))
				{
					pawn.Drawer.renderer.graphics.apparelGraphics.Add(item);
				}
			}
			pawn.Draw();
		}
		
		public static void reduce_rest(Pawn pawn, float x = 1f)
		{
			if (pawn.Has(Quirk.Vigorous)) x -= x/2;

			Need_Rest need_rest = pawn.needs.TryGetNeed<Need_Rest>();
			if (need_rest == null)
				return;

			need_rest.CurLevel -= need_rest.RestFallPerTick * x;
		}
		public static void OffsetPsyfocus(Pawn pawn, float x = 0)//0-1
		{
			if (ModsConfig.RoyaltyActive)
			{
				//pawn.psychicEntropy.Notify_Meditated();
				if (pawn.HasPsylink)
				{
					pawn.psychicEntropy.OffsetPsyfocusDirectly(x);
				}
			}
		}
	}
}
