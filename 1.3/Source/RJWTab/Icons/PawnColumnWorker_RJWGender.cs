using RimWorld;
using UnityEngine;
using Verse;

namespace rjw.MainTab.Icon
{
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_RJWGender : PawnColumnWorker_Gender
	{
		public static readonly Texture2D hermIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genders", true);

		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return ((Genital_Helper.has_penis_fertile(pawn) || Genital_Helper.has_penis_infertile(pawn)) && Genital_Helper.has_vagina(pawn)) ? hermIcon : pawn.gender.GetIcon();
		}
		protected override string GetIconTip(Pawn pawn)
		{
			return ((Genital_Helper.has_penis_fertile(pawn) || Genital_Helper.has_penis_infertile(pawn)) && Genital_Helper.has_vagina(pawn)) ? "PawnColumnWorker_RJWGender_IsHerm".Translate() : pawn.GetGenderLabel().CapitalizeFirst();
		}
	}
}