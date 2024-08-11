using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace rjw
{
	[StaticConstructorOnStartup]
	static class RMB_Menu2
	{
		//Widgets.InfoCardButton
		//Find.WindowStack.Add(new Dialog_InfoCard(thing));
		//Find.WindowStack.Add(new Dialog_InfoCard(def));
		static RMB_Menu2()
		{
			Harmony harmony = new Harmony("rjw");
			//start sex options
			//harmony.Patch(AccessTools.Method(typeof(FloatMenuMakerMap), "AddHumanlikeOrders"), prefix: null,
			//	postfix: new HarmonyMethod(typeof(RMB_Menu), nameof(SexFloatMenuOption)));
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

		private static void SexFloatMenuOption(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
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

			//not heromode or override_control - quit
			if (!(RJWSettings.RPG_hero_control || RJWSettings.override_control))
				return;

			var HeroOK0 = false;    //is hero
			var HeroOK1 = false;    //owned hero?
			var HeroOK2 = false;    //not owned hero? maybe prison check etc in future
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

			//not owned hero - quit
			if (HeroOK0 && !HeroOK2)
				return;

			//Log.Message("show options");

			// Find valid targets for sex.
			var validtargets = GenUI.TargetsAt(clickPos, TargetParameters);
			//Log.Message("targets count " + validtargets.Count());
			foreach (LocalTargetInfo target in validtargets)
			{
				// Ensure target is reachable.
				if (!pawn.CanReach(target, PathEndMode.ClosestTouch, Danger.Deadly))
				{
					//option = new FloatMenuOption("CannotReach".Translate(target.Thing.LabelCap, target.Thing) + " (" + "NoPath".Translate() + ")", null);
					continue;
				}

				Log.Message("target " +  target.Label);
				// Add menu options to masturbate
				FloatMenuOption option = null;

				if (target.Pawn == pawn)
				{
					option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Masturbate".Translate(), delegate ()
					{
						List<FloatMenuOption> opts1 = new List<FloatMenuOption>();
						FloatMenuOption option1 = null;

						option1 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Masturbate on chair or bed".Translate(), delegate ()
						{
							Find.Targeter.BeginTargeting(TargetParemetersMasturbationChairOrBed(target), (LocalTargetInfo targetThing) =>
							{
								pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate_Quick, null, null, targetThing.Thing.PositionHeld));
							});
						}, MenuOptionPriority.High), pawn, target);
						opts1.AddDistinct(option1);

						option1 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Masturbate at".Translate(), delegate ()
						{
							Find.Targeter.BeginTargeting(TargetParemetersMasturbationLoc(target), (LocalTargetInfo targetThing) =>
							{
								pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate_Quick, null, null, targetThing.Cell));
							});
						}, MenuOptionPriority.High), pawn, target);
						opts1.AddDistinct(option1);

						option1 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Masturbate here".Translate(), delegate ()
						{
							pawn.jobs.TryTakeOrderedJob(new Job(xxx.Masturbate_Quick, null, null, target.Pawn.PositionHeld));
						}, MenuOptionPriority.High), pawn, target);
						opts1.AddDistinct(option1);

						FloatMenuUtility.MakeMenu(opts1, (FloatMenuOption opt) => opt.Label, (FloatMenuOption opt) => opt.action);

					}, MenuOptionPriority.High), pawn, target);
					opts.AddDistinct(option);
				}

				//wip
				if (target.Thing != pawn as Thing)
				{
					if (target.Pawn != null)
						if (!target.Pawn.HostileTo(pawn) && xxx.is_human(target.Pawn))
						{
							// Add menu option to whore sex with target
							//option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Solicit " + target.Pawn.Name, delegate ()
							//{
							//	if (!target.Pawn.IsColonist && !target.Pawn.IsPrisoner && pawn.CurrentBed() != null)
							//		pawn.jobs.TryTakeOrderedJob(new Job(xxx.whore_is_serving_visitors, target.Pawn, pawn.CurrentBed()));
							//}, MenuOptionPriority.High), pawn, target);
							//opts.AddDistinct(option);

							// Add menu option to have sex with target
							option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Have sex with " + target.Pawn.NameShortColored, delegate ()
							{
								if (target.Pawn.InBed())
									pawn.jobs.TryTakeOrderedJob(new Job(xxx.casual_sex, target.Pawn, target.Pawn.CurrentBed()));
								else
									pawn.jobs.TryTakeOrderedJob(new Job(xxx.quick_sex, target.Pawn));
							}, MenuOptionPriority.High), pawn, target);
							opts.AddDistinct(option);
						}

					if (xxx.can_rape(pawn))
					{
						if (target.Thing is Corpse)
						{
							// Add menu option to rape target.
							option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Rape " + (((Corpse)target.Thing).InnerPawn).NameShortColored, delegate ()
							{
								if (RJWSettings.necrophilia_enabled && target.Thing is Corpse)
									pawn.jobs.TryTakeOrderedJob(new Job(xxx.RapeCorpse, target.Thing));
							}, MenuOptionPriority.High), pawn, target);
							opts.AddDistinct(option);
						}
						else if (target.Pawn != null)
						{
							// Add menu option to rape target.
							option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Rape " + target.Pawn.NameShortColored, delegate ()
							{
								if (target.Pawn.IsDesignatedComfort())
									pawn.jobs.TryTakeOrderedJob(new Job(xxx.RapeCP, target.Pawn));
								else if (target.Pawn.HostileTo(pawn))
									pawn.jobs.TryTakeOrderedJob(new Job(xxx.RapeEnemy, target.Pawn));
								else
									pawn.jobs.TryTakeOrderedJob(new Job(xxx.RapeRandom, target.Pawn));
							}, MenuOptionPriority.High), pawn, target);
							opts.AddDistinct(option);
						}
					}
				}
			}
		}
	}
}