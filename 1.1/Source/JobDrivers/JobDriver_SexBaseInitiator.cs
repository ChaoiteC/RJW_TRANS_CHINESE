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
				isRape = false;
				isWhoring = false;
				//Sexprops = SexUtility.SelectSextype(pawn, Partner, isRape, isWhoring, Partner);
				//sexType = Sexprops.SexType;
				//SexUtility.LogSextype(Sexprops.Giver, Sexprops.Reciever, Sexprops.RulePack, Sexprops.DictionaryKey);
			}
			else if (Partner.Dead)
			{
				isRape = true;
				isWhoring = false;
				if (Sexprops == null)
					Sexprops = SexUtility.SelectSextype(pawn, Partner, isRape, isWhoring, Partner);
				sexType = Sexprops.SexType;
				SexUtility.LogSextype(Sexprops.Giver, Sexprops.Reciever, Sexprops.RulePack, Sexprops.DictionaryKey);
			}
			else if (Partner.jobs?.curDriver is JobDriver_SexBaseReciever)
			{
				(Partner.jobs.curDriver as JobDriver_SexBaseReciever).parteners.AddDistinct(pawn);
				(Partner.jobs.curDriver as JobDriver_SexBaseReciever).increase_time(duration);

				//prevent Receiver standing up and interrupting rape
				if (Partner.health.hediffSet.HasHediff(HediffDef.Named("Hediff_Submitting")))
					Partner.health.AddHediff(HediffDef.Named("Hediff_Submitting"));

				//(Target.jobs.curDriver as JobDriver_SexBaseReciever).parteners.Count; //TODO: add multipartner support so sex doesn't repeat, maybe, someday
				isRape = Partner?.CurJob.def == xxx.gettin_raped;
				isWhoring = pawn?.CurJob.def == xxx.whore_is_serving_visitors;
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
			ModLog.Message("shouldreserve " + shouldreserve);
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