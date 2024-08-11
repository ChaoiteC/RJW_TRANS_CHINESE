using RimWorld;
using System.Linq;
using Verse;
using Verse.AI;

namespace rjw
{
	public abstract class JobDriver_SexBaseInitiator : JobDriver_Sex
	{

		public void Start()
		{
			if (Partner == null) //TODO: solo sex descriptions
			{
				isEndytophile = xxx.has_quirk(pawn, "Endytophile");
				//Sexprops = SexUtility.SelectSextype(pawn, Partner, isRape, isWhoring, Partner);
				//sexType = Sexprops.SexType;
				//SexUtility.LogSextype(Sexprops.Giver, Sexprops.Reciever, Sexprops.RulePack, Sexprops.DictionaryKey);
			}
			else if (Partner.Dead)
			{
				isRape = true;
				isEndytophile = xxx.has_quirk(pawn, "Endytophile");
				isAnimalOnAnimal = xxx.is_animal(pawn) && xxx.is_animal(Partner);
				if (Sexprops == null)
					Sexprops = SexUtility.SelectSextype(pawn, Partner, isRape, isWhoring, Partner);
				sexType = Sexprops.SexType;
				SexUtility.LogSextype(Sexprops.Giver, Sexprops.Reciever, Sexprops.RulePack, Sexprops.DictionaryKey);
			}
			else if (Partner.jobs?.curDriver is JobDriver_SexBaseReciever)
			{
				(Partner.jobs.curDriver as JobDriver_SexBaseReciever).parteners.AddDistinct(pawn);

				//prevent downed Receiver standing up and interrupting rape
				if (Partner.health.hediffSet.HasHediff(xxx.submitting))
					Partner.health.AddHediff(xxx.submitting);

				//(Target.jobs.curDriver as JobDriver_SexBaseReciever).parteners.Count; //TODO: add multipartner support so sex doesn't repeat, maybe, someday
				isRape = Partner?.CurJob.def == xxx.gettin_raped;
				isWhoring = pawn?.CurJob.def == xxx.whore_is_serving_visitors;
				isEndytophile = xxx.has_quirk(pawn, "Endytophile");
				isAnimalOnAnimal = xxx.is_animal(pawn) && xxx.is_animal(Partner);

				//non succubus focus gain
				if (xxx.is_nympho(pawn))
				{
					shouldGainFocus = true;
					SexUtility.OffsetPsyfocus(pawn, 0.01f);
				}
				else if (xxx.is_zoophile(pawn) && xxx.is_animal(Partner) && MeditationFocusTypeAvailabilityCache.PawnCanUse(pawn, MeditationFocusDefOf.Natural))
				{
					shouldGainFocus = true;
				}

				if (xxx.is_nympho(Partner))
				{
					shouldGainFocusP = true;
				}
				else if (xxx.is_zoophile(Partner) && xxx.is_animal(pawn) && MeditationFocusTypeAvailabilityCache.PawnCanUse(Partner, MeditationFocusDefOf.Natural))
				{
					shouldGainFocusP = true;
				}

				//succubus focus gain
				if (xxx.RoMIsActive)
				{
					if (xxx.has_traits(pawn))
						if (pawn.story.traits.HasTrait(xxx.Succubus))
						{
							isSuccubus = true;
						}

					if (xxx.has_traits(Partner))
						if (Partner.story.traits.HasTrait(xxx.Succubus))
						{
							isSuccubusP = true;
						}
				}
				if (xxx.NightmareIncarnationIsActive)
				{
					if (xxx.has_traits(pawn))
						foreach (var x in pawn.AllComps?.Where(x => x?.props?.ToStringSafe() == "NightmareIncarnation.CompProperties_SuccubusRace"))
						{
							isSuccubus = true;
							break;
						}

					if (xxx.has_traits(Partner))
						foreach (var x in Partner.AllComps?.Where(x => x?.props?.ToStringSafe() == "NightmareIncarnation.CompProperties_SuccubusRace"))
						{
							isSuccubusP = true;
							break;
						}
				}

				if (Sexprops == null)
					Sexprops = SexUtility.SelectSextype(pawn, Partner, isRape, isWhoring, Partner);
				sexType = Sexprops.SexType;
				SexUtility.LogSextype(Sexprops.Giver, Sexprops.Reciever, Sexprops.RulePack, Sexprops.DictionaryKey);
			}
			//Log.Message("sexType: " + sexType.ToString());
			//props = new SexProps(pawn, Partener, sexType, isRape);//maybe merge everything into this ?
		}

		//public void Change(xxx.rjwSextype sexType)
		//{
		//	if (pawn.jobs?.curDriver is JobDriver_SexBaseInitiator)
		//	{
		//		(pawn.jobs.curDriver as JobDriver_SexBaseInitiator).increase_time(duration);
		//		Sexprops = SexUtility.SelectSextype(pawn, Partner, isRape, isWhoring, Partner);
		//		sexType = Sexprops.SexType;
		//		SexUtility.LogSextype(Sexprops.Giver, Sexprops.Reciever, Sexprops.RulePack, Sexprops.DictionaryKey);
		//	}
		//	if (Partner.jobs?.curDriver is JobDriver_SexBaseReciever)
		//	{
		//		(Partner.jobs.curDriver as JobDriver_SexBaseReciever).increase_time(duration);
		//		Sexprops = SexUtility.SelectSextype(pawn, Partner, isRape, isWhoring, Partner);
		//		sexType = Sexprops.SexType;
		//		SexUtility.LogSextype(Sexprops.Giver, Sexprops.Reciever, Sexprops.RulePack, Sexprops.DictionaryKey);
		//	}
		//	sexType = sexType
		//}

		public void End()
		{
			if (xxx.is_human(pawn))
				pawn.Drawer.renderer.graphics.ResolveApparelGraphics();
			if (Partner?.jobs?.curDriver is JobDriver_SexBaseReciever)
			{
				(Partner?.jobs.curDriver as JobDriver_SexBaseReciever).parteners.Remove(pawn);
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			//ModLog.Message("shouldreserve " + shouldreserve);
			if (shouldreserve && Target != null)
				return pawn.Reserve(Target, job, xxx.max_rapists_per_prisoner, stackCount, null, errorOnFailed);
			else if (shouldreserve && Bed != null)
				return pawn.Reserve(Bed, job, Bed.SleepingSlotsCount, 0, null, errorOnFailed);
			else
				return true; // No reservations needed.

			//return this.pawn.Reserve(this.Partner, this.job, 1, 0, null) && this.pawn.Reserve(this.Bed, this.job, 1, 0, null);
		}
	}
}