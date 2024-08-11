using System.Collections.Generic;
using Verse;
using System;

namespace rjw
{
	/// <summary>
	/// Defines all RJW configuration related to a specific race or group of races.
	/// Core races should have RaceGroupDefs in RJW.
	/// Non-core races should have RaceGroupDefs in the separate RJWRaceSupport mod.
	/// Technically any mod could add a RaceGroupDef for its own races but we expect that to be rare.
	/// </summary>
	public class RaceGroupDef : Def
	{
		public List<string> raceNames = null;
		public List<string> pawnKindNames = null;

		public List<PartAdder> partAdders;
		public List<string> anuses = null;
		public List<float> chanceanuses = null;
		public List<string> femaleBreasts = null;
		public List<float> chancefemaleBreasts = null;
		public List<string> femaleGenitals = null;
		public List<float> chancefemaleGenitals = null;
		public List<string> maleBreasts = null;
		public List<float> chancemaleBreasts = null;
		public List<string> maleGenitals = null;
		public List<float> chancemaleGenitals = null;

		public List<string> hybridRaceParents = null;
		public List<string> hybridChildKindDef = null;

		public List<string> tags = null;
		public bool hasSingleGender = false;
		public bool hasSexNeed = true;
		public bool hasFertility = true;
		public bool hasPregnancy = true;
		public bool oviPregnancy = false;
		public bool ImplantEggs = false;
		public bool HasUdder = false;
		[Obsolete("isDemon is obsolete, use tags instead.")]
		public bool isDemon = false;
		[Obsolete("isSlime is obsolete, use tags instead.")]
		public bool isSlime = false;
		public float raceSexDrive = 1.0f;

		public string eggFertilizedDef = "RJW_EggFertilized";
		public string eggUnfertilizedDef = "RJW_EggUnfertilized";
		public float eggProgressUnfertilizedMax = 1.0f;
		public float eggLayIntervalDays = 3.5f;

		public List<string> GetRacePartDefNames(SexPartType sexPartType)
		{
			return sexPartType switch
			{
				SexPartType.Anus => anuses,
				SexPartType.FemaleBreast => femaleBreasts,
				SexPartType.FemaleGenital => femaleGenitals,
				SexPartType.MaleBreast => maleBreasts,
				SexPartType.MaleGenital => maleGenitals,
				_ => throw new ApplicationException($"Unrecognized sexPartType: {sexPartType}"),
			};
		}

		public List<float> GetChances(SexPartType sexPartType)
		{
			return sexPartType switch
			{
				SexPartType.Anus => chanceanuses,
				SexPartType.FemaleBreast => chancefemaleBreasts,
				SexPartType.FemaleGenital => chancefemaleGenitals,
				SexPartType.MaleBreast => chancemaleBreasts,
				SexPartType.MaleGenital => chancemaleGenitals,
				_ => throw new ApplicationException($"Unrecognized sexPartType: {sexPartType}"),
			};
		}
	}
}
