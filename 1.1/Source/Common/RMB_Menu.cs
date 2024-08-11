using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace rjw
{
	[StaticConstructorOnStartup]
	static class RMB_Menu
	{

		static RMB_Menu()
		{
			Harmony harmony = new Harmony("rjw");
			//start sex options
			harmony.Patch(AccessTools.Method(typeof(FloatMenuMakerMap), "AddHumanlikeOrders"), prefix: null,
				postfix: new HarmonyMethod(typeof(RMB_Menu), nameof(SexFloatMenuOption)));
			//change sex options
			//harmony.Patch(AccessTools.Method(typeof(FloatMenuMakerMap), "AddHumanlikeOrders"), prefix: null,
			//	postfix: new HarmonyMethod(typeof(RMB_Menu), nameof(SexFloatMenuOption)));
		}

		//show rmb on
		public static TargetingParameters TargetParameters
		{
			get
			{
				if (targetParameters == null)
				{
					targetParameters = new TargetingParameters()
					{
						canTargetHumans = true,
						canTargetAnimals = true,
						canTargetItems = true,
						mapObjectTargetsMustBeAutoAttackable = false,
					};
				}
				return targetParameters;
			}
		}

		private static TargetingParameters targetParameters = null;

		public static TargetingParameters TargetParemetersMasturbationChairOrBed(LocalTargetInfo target)
		{
			return new TargetingParameters()
			{
				canTargetBuildings = true,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = (TargetInfo target) =>
				{
					if (!target.HasThing)
						return false;
					Building building = target.Thing as Building;
					if (building == null)
						return false;
					if (building.def.building.isSittable)
						return true;
					if (building is Building_Bed)
						return true;
					return false;
				}
			};
		}

		public static TargetingParameters TargetParemetersMasturbationLoc(LocalTargetInfo target)
		{
			return new TargetingParameters()
			{
				canTargetLocations = true,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = (TargetInfo target) =>
				{
					if (!target.HasThing)
						return true;
					return false;
				}
			};
		}

		//TODO: dildo selection for masturbation/sex
		public static TargetingParameters TargetParemetersDildos(LocalTargetInfo target)
		{
			return new TargetingParameters()
			{
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = ((TargetInfo target) =>
				{
					if (!target.HasThing)
						return false;
					Thing dildo = target.Thing as Thing;
					if (dildo == null)
						return false;

					return true;
				})
			};
		}

		public static void SexFloatMenuOption(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			// If the pawn in question cannot take jobs, don't bother.
			if (pawn.jobs == null)
				return;

			// If the pawn is drafted - quit.
			if (pawn.Drafted)
				return;

			// No menu for you - quit.
			//if (!SaveStorage.DataStore.GetPawnData(pawn).ShowRMB_Menu)
			//	return;

			// Getting raped - no control
			if (pawn.jobs.curDriver is JobDriver_SexBaseRecieverRaped)
				return;

			//is colonist?, is hospitality colonist/guest?, no control for guests
			if (!pawn.IsFreeColonist || pawn.Faction == null || pawn.GetExtraHomeFaction(null) != null)
				return;

			//TODO: move checks below into ShowRMB_Menu
			//not heromode or override_control - quit
			if (!(RJWSettings.RPG_hero_control || RJWSettings.override_control))
				return;

			var HeroOK0 = false;    //is hero
			var HeroOK1 = false;    //owned hero?
			var HeroOK2 = false;    //not owned hero? maybe prison check etc in future
									// || xxx.is_slave(pawn)
			if (RJWSettings.RPG_hero_control)
			{
				HeroOK0 = pawn.IsDesignatedHero();
				HeroOK1 = HeroOK0 && pawn.IsHeroOwner();
				HeroOK2 = HeroOK0 && !pawn.IsHeroOwner();
			}
			else if (!RJWSettings.override_control)
				return;

			//not hero, not override_control - quit
			if (!HeroOK0 && !RJWSettings.override_control)
				return;

			//Log.Message("show options HeroOK0 " + HeroOK0);
			//Log.Message("show options HeroOK1 " + HeroOK1);
			//Log.Message("show options HeroOK2 " + HeroOK2);

			//not owned hero - quit
			if (HeroOK0 && HeroOK2)
				return;

			//Log.Message("show options");

			// Find valid targets for sex.
			var validtargets = GenUI.TargetsAt(clickPos, TargetParameters);
			//Log.Message("targets count " + validtargets.Count());


			//var t = DefDatabase<InteractionDef>.AllDefs;
			//Log.Message("count 1 " + t.Count());
			//var t1 = t.Where(x => !x.modExtensions.NullOrEmpty());
			//Log.Message("count 2 " + t1.Count());
			//foreach (var tt in t1)
			//{
			//	Log.Message("def " + tt.defName);
			//	if (tt.HasModExtension<InteractionExtension>())
			//	{
			//		Log.Message("rjw InteractionExtension def ");
			//		var ttt = tt.GetModExtension<InteractionExtension>();
			//		var tags = ttt.tags.ToCommaList();
			//		Log.Message("tags " + tags);
			//	}
			//}

			foreach (LocalTargetInfo target in validtargets)
			{
				if (target.Pawn != null && target.Pawn.Drafted)
					continue;
				// Ensure target is reachable.
				if (!pawn.CanReach(target, PathEndMode.ClosestTouch, Danger.Deadly))
				{
					//option = new FloatMenuOption("CannotReach".Translate(target.Thing.LabelCap, target.Thing) + " (" + "NoPath".Translate() + ")", null);
					continue;
				}

				//Log.Message("target " +  target.Label);
				opts.AddRange(GenerateSexOptions(pawn, target).Where(x => x.action != null));
				//sex-role?-pose ?
				//rjw?-sex(do fuck/rape checks)-role?-pose ?

			}
		}
		
		public static List<FloatMenuOption> GenerateSexOptions(Pawn pawn, LocalTargetInfo target)
		{
			List<FloatMenuOption> opts = new List<FloatMenuOption>();
			FloatMenuOption option = null;

			// social stuff
			//if (pawn.jobs.curDriver is JobDriver_Sex)
			{
				// Masturbating
				if (target.Pawn != null && target.Pawn != pawn && !pawn.interactions.InteractedTooRecentlyToInteract() && pawn.interactions.CanInteractNowWith(target.Pawn))
				{
					//if (pawn.jobs.curDriver is JobDriver_Masturbate)
					{
						option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Social".Translate(), delegate ()
						{
							FloatMenuUtility.MakeMenu(GenerateSocialOptions(pawn, target).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
						}, MenuOptionPriority.High), pawn, target);
						opts.AddDistinct(option);
					}
				}
			}

			// Already doing sex.
			// Pose switch
			if (pawn.jobs.curDriver is JobDriver_Sex)
			{
				// Masturbating
				if (target.Pawn == pawn)
				{
					if (pawn.jobs.curDriver is JobDriver_Masturbate)
					{
						option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate".Translate(), delegate ()
						{
							FloatMenuUtility.MakeMenu(GenerateSoloSexPoseOptions(pawn, target).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);

						}, MenuOptionPriority.High), pawn, target);
						opts.AddDistinct(option);
					}
				}
				// TODO: Add pose switch
				return opts;
			}

			// Start
			// Add menu options to masturbate, if self
			if (target.Pawn == pawn)
			{
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate".Translate(), delegate ()
				{
					FloatMenuUtility.MakeMenu(GenerateSoloSexRoleOptions(pawn, target).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);
			}

			// Add menu options for con sex, if not self
			if (target.Thing != pawn as Thing)
			{
				if (target.Pawn != null)
					if (!target.Pawn.Downed && !target.Pawn.HostileTo(pawn) && (xxx.can_be_fucked(target.Pawn) || xxx.can_fuck(target.Pawn)))
					{
						//TODO change whore JD to work manually
						// Add menu option to whore sex with target
						//option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Solicit " + target.Pawn.Name, delegate ()
						//{
						//	if (!target.Pawn.IsColonist && !target.Pawn.IsPrisoner && pawn.CurrentBed() != null)
						//		pawn.jobs.TryTakeOrderedJob(new Job(xxx.whore_is_serving_visitors, target.Pawn, pawn.CurrentBed()));
						//}, MenuOptionPriority.High), pawn, target);
						//opts.AddDistinct(option);

						// Add menu option to have sex with target, if target finds you sexy enough
						if (xxx.is_human(target.Pawn) && SexAppraiser.would_fuck(target.Pawn, pawn) > 0.1f)
						{
							option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Sex".Translate() + target.Pawn.NameShortColored, delegate ()
							{
								Job job = null;
								if (target.Pawn.InBed())
									job = new Job(xxx.casual_sex, target, target.Pawn.CurrentBed());
								else
									job = new Job(xxx.quick_sex, target);

								FloatMenuUtility.MakeMenu(GenerateSexRoleOptions(pawn, target, false, job).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
							}, MenuOptionPriority.High), pawn, target);
							opts.AddDistinct(option);
						}
						else if (xxx.is_animal(target.Pawn) && RJWSettings.bestiality_enabled && target.Pawn.Faction == pawn.Faction)
						{
							if (pawn.ownership.OwnedBed != null && target.Pawn.CanReach(pawn.ownership.OwnedBed, PathEndMode.OnCell, Danger.Some))
							{
								// Add menu option to have sex with animal(invite to bed)
								option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Sex".Translate() + target.Pawn.NameShortColored, delegate ()
								{
									Job job = new Job(xxx.bestialityForFemale, target, pawn.ownership.OwnedBed);
									FloatMenuUtility.MakeMenu(GenerateSexRoleOptions(pawn, target, false, job).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
									//pawn.jobs.TryTakeOrderedJob(new Job(xxx.quick_sex, target.Pawn));
								}, MenuOptionPriority.High), pawn, target);
								opts.AddDistinct(option);
							}
							//	// Add menu option to have sex with animal(service animal on spot)
							//	option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Sex".Translate() + target.Pawn.NameShortColored, delegate ()
							//	{
							//		pawn.jobs.TryTakeOrderedJob(new Job(xxx.bestialityForFemale, target.Pawn, pawn.ownership.OwnedBed));
							//		//pawn.jobs.TryTakeOrderedJob(new Job(xxx.quick_sex, target.Pawn));
							//	}, MenuOptionPriority.High), pawn, target);
							//	opts.AddDistinct(option);

						}
					}

					// Add menu options for non con sex, if not self
					if (xxx.can_rape(pawn, true))
					{
						//Log.Message("targets can_rape " + target.Label);
						string text = null;
						Action action = null;

						if (target.Thing is Corpse && RJWSettings.necrophilia_enabled)
						{
							text = "RJW_RMB_RapeCorpse".Translate() + ((Corpse)target.Thing).InnerPawn.NameShortColored;
							action = delegate ()
							{
								Job job = new Job(xxx.RapeCorpse, target);
								FloatMenuUtility.MakeMenu(GenerateSexRoleOptions(pawn, target, true, job).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
							};
						}
						else if (target.Pawn != null && xxx.can_be_fucked(target.Pawn) && (xxx.is_human(target.Pawn) || (xxx.is_animal(target.Pawn) && RJWSettings.bestiality_enabled)))
						{
							//Log.Message("targets can_rape 1 " + target.Label);
							if (target.Pawn.HostileTo(pawn))
							{
								//Log.Message("targets can_rape HostileTo " + target.Label);
								if (target.Pawn.Downed)
								{
									text = "RJW_RMB_RapeEnemy".Translate() + target.Pawn.NameShortColored;
									action = delegate ()
									{
										Job job = new Job(xxx.RapeEnemy, target);
										FloatMenuUtility.MakeMenu(GenerateSexRoleOptions(pawn, target, true, job).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
									};
								}
							}
							else if (xxx.is_animal(target.Pawn) && xxx.can_fuck(pawn))
							{
								text = "RJW_RMB_RapeAnimal".Translate() + target.Pawn.NameShortColored;
								action = delegate ()
								{
									Job job = new Job(xxx.bestiality, target);
									FloatMenuUtility.MakeMenu(GenerateSexRoleOptions(pawn, target, true, job).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
								};
							}
							else if (target.Pawn.IsDesignatedComfort())
							{
								//Log.Message("targets can_rape IsDesignatedComfort " + target.Label);
								text = "RJW_RMB_RapeCP".Translate() + target.Pawn.NameShortColored;
								action = delegate ()
								{
									Job job = new Job(xxx.RapeCP, target);
									FloatMenuUtility.MakeMenu(GenerateSexRoleOptions(pawn, target, true, job).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
								};
							}
							else if (xxx.can_get_raped(target.Pawn) && (xxx.get_vulnerability(target.Pawn) >= xxx.get_vulnerability(pawn)))
							{
								//Log.Message("targets can_rape else " + target.Label);
								text = "RJW_RMB_Rape".Translate() + target.Pawn.NameShortColored;
								action = delegate ()
								{
									Job job = new Job(xxx.RapeRandom, target);
									FloatMenuUtility.MakeMenu(GenerateSexRoleOptions(pawn, target, true, job).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
									//pawn.jobs.TryTakeOrderedJob(new Job(xxx.RapeRandom, target.Pawn));
								};
							}

						}

						option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.High), pawn, target);
						opts.AddDistinct(option);
					}
				}
			return opts;
		}
		public static List<FloatMenuOption> GenerateSexRoleOptions(Pawn pawn, LocalTargetInfo target, bool rape, Job job)
		{
			List<FloatMenuOption> opts = new List<FloatMenuOption>();
			FloatMenuOption option = null;
			var partner = target.Pawn;

			if (target.Thing is Corpse)
				partner = (target.Thing as Corpse).InnerPawn;

			var sexScores = SexUtility.DetermineSexScores(pawn, partner, rape, false, pawn);
			var arraytype = 0;
			if (xxx.is_animal(partner) && !rape)
			{
				arraytype = 2;
			}
			else if (rape)
			{
				arraytype = 1;
			}

			for (int i = 0; i < SexUtility.rmb_dictionarykeys.GetLength(0); i++)
			{
				InteractionDef dictionaryKey = null;
				if (sexScores[i] > 0)
					dictionaryKey = SexUtility.rmb_dictionarykeys[i, arraytype];
				if (dictionaryKey != null)
				{
					//Log.Message("dictionarykeys id: " + i);
					//Log.Message("dictionaryKey " + dictionaryKey);

					var prefix = "Do ";
					var postfix = "";
					//if (rape)
					//	prefix = " - Rape";
					var t2 = i;
					var giving = pawn;
					var receiving = partner;
					if (t2 % 2 != 0 && !rape)
					{
						giving = partner;
						receiving = pawn;
						prefix = "Ask for ";
						if(arraytype == 2)
							prefix = "Provide for ";

						postfix = "";
					}
					option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(prefix + dictionaryKey.label + postfix, delegate ()
					//option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Commit " + dictionaryKey.LabelCap + t1, delegate ()
					{
						pawn.jobs.TryTakeOrderedJob(job);
						//if (pawn.jobs.TryTakeOrderedJob(new Job(xxx.quick_sex, partner)))
						//{
						//if (t1 == " - Receive") 
						//{
						//Log.Message("i: " + t2 + " arraytype: " + arraytype);
						//Log.Message("job: " + pawn.jobs.curDriver);
						var rulepack = SexUtility.rulepacks[t2, arraytype];
						//Log.Message("rulepack: " + rulepack);
						var props = new SexProps(pawn, partner, SexUtility.sexActs[dictionaryKey], rape, giving, receiving, rulepack, dictionaryKey);
						//Log.Message("props: " + props);
						(pawn.jobs.curDriver as JobDriver_Sex).Sexprops = props;
						//Log.Message("fin: " + pawn.jobs.curDriver);
							//}
						//}


						//FloatMenuUtility.MakeMenu(GenerateSexPoseOptions(pawn, target).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
					}, MenuOptionPriority.High), pawn, target);
					opts.AddDistinct(option);
				}
			}

			if (opts.NullOrEmpty())
				opts.AddDistinct(new FloatMenuOption("none", null));

			return opts;
		}
		public static List<FloatMenuOption> GenerateSexPoseOptions(Pawn pawn, LocalTargetInfo target)
		{
			List<FloatMenuOption> opts = new List<FloatMenuOption>();
			FloatMenuOption option = null;

			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("give".Translate(), delegate ()
			{
				pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
			}, MenuOptionPriority.High), pawn, target);
			opts.AddDistinct(option);

			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("receive".Translate(), delegate ()
			{
				pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
			}, MenuOptionPriority.High), pawn, target);
			opts.AddDistinct(option);

			return opts;
		}

		public static List<FloatMenuOption> GenerateSocialOptions(Pawn pawn, LocalTargetInfo target)
		{
			List<FloatMenuOption> opts = new List<FloatMenuOption>();
			FloatMenuOption option = null;

			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Insult".Translate(), delegate ()
			{
				pawn.interactions.TryInteractWith(target.Pawn, InteractionDefOf.Insult);
			}, MenuOptionPriority.High), pawn, target);
			opts.AddDistinct(option);

			if (!pawn.HostileTo(target.Pawn))
			{
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_SocialFight".Translate(), delegate ()
				{
					pawn.interactions.StartSocialFight(target.Pawn);
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);

				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Chat".Translate(), delegate ()
				{
					pawn.interactions.TryInteractWith(target.Pawn, InteractionDefOf.Chitchat);
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);

				// OP shit +12 relations
				//option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_DeepTalk".Translate(), delegate ()
				//{
				//	pawn.interactions.TryInteractWith(target.Pawn, InteractionDefOf.DeepTalk);
				//}, MenuOptionPriority.High), pawn, target);
				//opts.AddDistinct(option);

				if (!LovePartnerRelationUtility.LovePartnerRelationExists(pawn, target.Pawn))
				{
					option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_RomanceAttempt".Translate(), delegate ()
					{
						pawn.interactions.TryInteractWith(target.Pawn, InteractionDefOf.RomanceAttempt);
					}, MenuOptionPriority.High), pawn, target);
					opts.AddDistinct(option);
				}

				if (pawn.relations.DirectRelationExists(PawnRelationDefOf.Lover, target.Pawn) || pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, target.Pawn))
				{
					option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_MarriageProposal".Translate(), delegate ()
					{
						pawn.interactions.TryInteractWith(target.Pawn, InteractionDefOf.MarriageProposal);
					}, MenuOptionPriority.High), pawn, target);
					opts.AddDistinct(option); 
				}
			}

			return opts;
		}
		//this options probably can be merged into 1, but for now/testing keep it separate
		public static List<FloatMenuOption> GenerateSoloSexRoleOptions(Pawn pawn, LocalTargetInfo target)
		{
			List<FloatMenuOption> opts = new List<FloatMenuOption>();
			FloatMenuOption option = null;

			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_Bed".Translate(), delegate ()
			{
				Find.Targeter.BeginTargeting(TargetParemetersMasturbationChairOrBed(target), (LocalTargetInfo targetThing) =>
				{
					FloatMenuUtility.MakeMenu(GenerateSoloSexPoseOptions(pawn, targetThing).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
				});
			}, MenuOptionPriority.High), pawn, target);
			opts.AddDistinct(option);

			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_At".Translate(), delegate ()
			{
				Find.Targeter.BeginTargeting(TargetParemetersMasturbationLoc(target), (LocalTargetInfo targetThing) =>
				{
					FloatMenuUtility.MakeMenu(GenerateSoloSexPoseOptions(pawn, targetThing).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
				});
			}, MenuOptionPriority.High), pawn, target);
			opts.AddDistinct(option);

			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_Here".Translate(), delegate ()
			{
				FloatMenuUtility.MakeMenu(GenerateSoloSexPoseOptions(pawn, target).Where(x => x.action != null), (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);
			}, MenuOptionPriority.High), pawn, target);
			opts.AddDistinct(option);
			return opts;
		}

		public static List<FloatMenuOption> GenerateSoloSexPoseOptions(Pawn pawn, LocalTargetInfo target)
		{
			List<FloatMenuOption> opts = new List<FloatMenuOption>();
			FloatMenuOption option = null;

			var pawnDic = SexUtility.DetermineSexParts(pawn);

			bool pawnHasMouth = pawnDic.TryGetValue("HasMouth");
			bool pawnHasAnus = pawnDic.TryGetValue("HasAnus");
			bool pawnHasBigBreasts = pawnDic.TryGetValue("HasBigBreasts");
			bool pawnHasVagina = pawnDic.TryGetValue("HasVagina");
			bool pawnHasPenis = pawnDic.TryGetValue("HasPenis");
			bool pawnHasBigPenis = pawnDic.TryGetValue("HasBigPenis");
			bool pawnHasMultiPenis = pawnDic.TryGetValue("HasMultiPenis");
			bool pawnHasHands = pawnDic.TryGetValue("HasHands");
			bool pawnHasTail = pawnDic.TryGetValue("HasTail");

			if (pawnHasPenis && pawnHasHands)
			{
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_Fap".Translate(), delegate ()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);
			}

			if (pawnHasPenis && pawnHasMouth && pawnHasBigPenis)
			{
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_AF".Translate(), delegate ()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);
			}

			if (pawnHasVagina && pawnHasHands)
			{
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_Schlic".Translate(), delegate ()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);
			}

			if (pawnHasVagina && pawnHasTail)
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_TJV".Translate(), delegate ()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
				}, MenuOptionPriority.High), pawn, target);
			opts.AddDistinct(option);

			if (pawnHasBigBreasts)
			{
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_Breasts".Translate(), delegate ()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);
			}

			if (pawnHasBigBreasts && pawnHasBigPenis)
			{
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_BJ".Translate(), delegate ()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);
			}

			if (pawnHasAnus && pawnHasHands)
			{
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_Anal".Translate(), delegate ()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
				}, MenuOptionPriority.High), pawn, target);
				opts.AddDistinct(option);
			}

			if (pawnHasAnus && pawnHasTail)
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RJW_RMB_Masturbate_TJA".Translate(), delegate ()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate, null, null, target.Cell));
				}, MenuOptionPriority.High), pawn, target);
			opts.AddDistinct(option);

			return opts;
		}
	}
}