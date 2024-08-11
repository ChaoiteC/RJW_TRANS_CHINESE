using System.Collections.Generic;
using RimWorld;
using Verse;
using Multiplayer.API;
using System.Linq;
using System.Linq.Expressions;

namespace rjw
{
	public struct nymph_story
	{
		public Backstory child;
		public Backstory adult;
		public List<Trait> traits;
	}

	public struct nymph_passion_chances
	{
		public float major;
		public float minor;

		public nymph_passion_chances(float maj, float min)
		{
			major = maj;
			minor = min;
		}
	}

	public static class nymph_backstories
	{
		public struct child
		{
			public static Backstory vatgrown_sex_slave;
		};

		public struct adult
		{
			public static Backstory feisty;
			public static Backstory curious;
			public static Backstory tender;
			public static Backstory chatty;
			public static Backstory broken;
			public static Backstory homekeeper;
		};

		public static void init()
		{
			{
				Backstory bs = new Backstory();
				bs.identifier = ""; // identifier will be set by ResolveReferences
				MiscTranslationDef MTdef = DefDatabase<MiscTranslationDef>.GetNamedSilentFail("rjw_vatgrown_sex_slave");
				if (MTdef != null)
				{
					bs.SetTitle(MTdef.label, MTdef.label);
					bs.SetTitleShort(MTdef.stringA, MTdef.stringA);
					bs.baseDesc = MTdef.description;
				}
				else
				{
					bs.SetTitle("Vat-Grown Sex Slave", "Vat-Grown Sex Slave");
					bs.SetTitleShort("SexSlave", "SexSlave");
					bs.baseDesc = "SexSlave Nymph";
				}
				// bs.skillGains = new Dictionary<string, int> ();
				bs.skillGainsResolved.Add(SkillDefOf.Social, 8);
				// bs.skillGainsResolved = new Dictionary<SkillDef, int> (); // populated by ResolveReferences
				bs.workDisables = WorkTags.Intellectual;
				bs.requiredWorkTags = WorkTags.None;
				bs.slot = BackstorySlot.Childhood;
				bs.spawnCategories = new List<string>() { "rjw_nymphsCategory", "Slave" }; // Not necessary (I Think)
				Unprivater.SetProtectedValue("bodyTypeGlobal", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeFemale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeMale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeGlobalResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeFemaleResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeMaleResolved", bs, BodyTypeDefOf.Thin);
				//bs.bodyTypeFemale = BodyType.Female;
				//bs.bodyTypeMale = BodyType.Thin;
				bs.forcedTraits = new List<TraitEntry>();
				bs.forcedTraits.Add(new TraitEntry(xxx.nymphomaniac, 0));
				bs.disallowedTraits = new List<TraitEntry>();
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesMen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesWomen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.TooSmart, 0));
				bs.shuffleable = true;
				bs.ResolveReferences();
				BackstoryDatabase.AddBackstory(bs);
				child.vatgrown_sex_slave = bs;
				//--ModLog.Message("nymph_backstories::init() succeed0");
			}

			{
				Backstory bs = new Backstory();
				bs.identifier = "";
				MiscTranslationDef MTdef = DefDatabase<MiscTranslationDef>.GetNamedSilentFail("rjw_feisty");
				if (MTdef != null)
				{
					bs.SetTitle(MTdef.label, MTdef.label);
					bs.SetTitleShort(MTdef.stringA, MTdef.stringA);
					bs.baseDesc = MTdef.description;
				}
				else
				{
					bs.SetTitle("Feisty Nymph", "Feisty Nymph");
					bs.SetTitleShort("Nymph", "Nymph");
					bs.baseDesc = "Feisty Nymph";
				}
				bs.skillGainsResolved.Add(SkillDefOf.Social, -3);
				bs.skillGainsResolved.Add(SkillDefOf.Shooting, 2);
				bs.skillGainsResolved.Add(SkillDefOf.Melee, 9);
				bs.workDisables = (WorkTags.Cleaning | WorkTags.Animals | WorkTags.Caring | WorkTags.Artistic | WorkTags.ManualSkilled); 
				bs.requiredWorkTags = WorkTags.None;
				bs.slot = BackstorySlot.Adulthood;
				bs.spawnCategories = new List<string>() { "rjw_nymphsCategory", "Slave" }; // Not necessary (I Think)
				Unprivater.SetProtectedValue("bodyTypeGlobal", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeFemale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeMale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeGlobalResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeFemaleResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeMaleResolved", bs, BodyTypeDefOf.Thin);
				bs.forcedTraits = new List<TraitEntry>();
				bs.forcedTraits.Add(new TraitEntry(xxx.nymphomaniac, 0));
				bs.disallowedTraits = new List<TraitEntry>();
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesMen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesWomen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.TooSmart, 0));
				bs.shuffleable = true;
				bs.ResolveReferences();
				BackstoryDatabase.AddBackstory(bs);
				adult.feisty = bs;
				//--ModLog.Message("nymph_backstories::init() succeed1");
			}

			{
				Backstory bs = new Backstory();
				bs.identifier = "";
				MiscTranslationDef MTdef = DefDatabase<MiscTranslationDef>.GetNamedSilentFail("rjw_curious");
				if (MTdef != null)
				{
					bs.SetTitle(MTdef.label, MTdef.label);
					bs.SetTitleShort(MTdef.stringA, MTdef.stringA);
					bs.baseDesc = MTdef.description;
				}
				else
				{
					bs.SetTitle("Curious Nymph", "Curious Nymph");
					bs.SetTitleShort("Nymph", "Nymph");
					bs.baseDesc = "Curious Nymph";
				}
				bs.skillGainsResolved.Add(SkillDefOf.Construction, 2);
				bs.skillGainsResolved.Add(SkillDefOf.Crafting, 6);
				bs.workDisables = (WorkTags.Animals | WorkTags.Artistic | WorkTags.Caring | WorkTags.Cooking | WorkTags.Mining | WorkTags.PlantWork | WorkTags.Violent | WorkTags.ManualDumb);
				bs.requiredWorkTags = WorkTags.None;
				bs.slot = BackstorySlot.Adulthood;
				bs.spawnCategories = new List<string>() { "rjw_nymphsCategory", "Slave" }; // Not necessary (I Think)
				Unprivater.SetProtectedValue("bodyTypeGlobal", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeFemale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeMale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeGlobalResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeFemaleResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeMaleResolved", bs, BodyTypeDefOf.Thin);
				bs.forcedTraits = new List<TraitEntry>();
				bs.forcedTraits.Add(new TraitEntry(xxx.nymphomaniac, 0));
				bs.disallowedTraits = new List<TraitEntry>();
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesMen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesWomen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.TooSmart, 0));
				bs.shuffleable = true;
				bs.ResolveReferences();
				BackstoryDatabase.AddBackstory(bs);
				adult.curious = bs;
				//--ModLog.Message("nymph_backstories::init() succeed2");
			}

			{
				Backstory bs = new Backstory();
				bs.identifier = "";
				MiscTranslationDef MTdef = DefDatabase<MiscTranslationDef>.GetNamedSilentFail("rjw_tender");
				if (MTdef != null)
				{
					bs.SetTitle(MTdef.label, MTdef.label);
					bs.SetTitleShort(MTdef.stringA, MTdef.stringA);
					bs.baseDesc = MTdef.description;
				}
				else
				{
					bs.SetTitle("Tender Nymph", "Tender Nymph");
					bs.SetTitleShort("Nymph", "Nymph");
					bs.baseDesc = "Tender Nymph";
				}
				bs.skillGainsResolved.Add(SkillDefOf.Medicine, 4);
				bs.workDisables = (WorkTags.Animals | WorkTags.Artistic | WorkTags.Hauling | WorkTags.Violent | WorkTags.ManualSkilled);
				bs.requiredWorkTags = WorkTags.None;
				bs.slot = BackstorySlot.Adulthood;
				bs.spawnCategories = new List<string>() { "rjw_nymphsCategory", "Slave" }; // Not necessary (I Think)
				Unprivater.SetProtectedValue("bodyTypeGlobal", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeFemale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeMale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeGlobalResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeFemaleResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeMaleResolved", bs, BodyTypeDefOf.Thin);
				bs.forcedTraits = new List<TraitEntry>();
				bs.forcedTraits.Add(new TraitEntry(xxx.nymphomaniac, 0));
				bs.disallowedTraits = new List<TraitEntry>();
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesMen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesWomen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.TooSmart, 0));
				bs.shuffleable = true;
				bs.ResolveReferences();
				BackstoryDatabase.AddBackstory(bs);
				adult.tender = bs;
				//--ModLog.Message("nymph_backstories::init() succeed3");
			}

			{
				Backstory bs = new Backstory();
				bs.identifier = "";
				MiscTranslationDef MTdef = DefDatabase<MiscTranslationDef>.GetNamedSilentFail("rjw_chatty");
				if (MTdef != null)
				{
					bs.SetTitle(MTdef.label, MTdef.label);
					bs.SetTitleShort(MTdef.stringA, MTdef.stringA);
					bs.baseDesc = MTdef.description;
				}
				else
				{
					bs.SetTitle("Chatty Nymph", "Chatty Nymph");
					bs.SetTitleShort("Nymph", "Nymph");
					bs.baseDesc = "Chatty Nymph";
				}
				bs.skillGainsResolved.Add(SkillDefOf.Social, 6);
				bs.workDisables = (WorkTags.Animals | WorkTags.Caring | WorkTags.Artistic | WorkTags.Violent | WorkTags.ManualDumb | WorkTags.ManualSkilled);
				bs.requiredWorkTags = WorkTags.None;
				bs.slot = BackstorySlot.Adulthood;
				bs.spawnCategories = new List<string>() { "rjw_nymphsCategory", "Slave" }; // Not necessary (I Think)
				Unprivater.SetProtectedValue("bodyTypeGlobal", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeFemale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeMale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeGlobalResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeFemaleResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeMaleResolved", bs, BodyTypeDefOf.Thin);
				bs.forcedTraits = new List<TraitEntry>();
				bs.forcedTraits.Add(new TraitEntry(xxx.nymphomaniac, 0));
				bs.disallowedTraits = new List<TraitEntry>();
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesMen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesWomen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.TooSmart, 0));
				bs.shuffleable = true;
				bs.ResolveReferences();
				BackstoryDatabase.AddBackstory(bs);
				adult.chatty = bs;
				//--ModLog.Message("nymph_backstories::init() succeed4");
			}

			{
				Backstory bs = new Backstory();
				bs.identifier = "";
				MiscTranslationDef MTdef = DefDatabase<MiscTranslationDef>.GetNamedSilentFail("rjw_broken");
				if (MTdef != null)
				{
					bs.SetTitle(MTdef.label, MTdef.label);
					bs.SetTitleShort(MTdef.stringA, MTdef.stringA);
					bs.baseDesc = MTdef.description;
				}
				else
				{
					bs.SetTitle("Broken Nymph", "Broken Nymph");
					bs.SetTitleShort("Nymph", "Nymph");
					bs.baseDesc = "Broken Nymph";
				}
				bs.skillGainsResolved.Add(SkillDefOf.Social, -5);
				bs.skillGainsResolved.Add(SkillDefOf.Artistic, 8);
				bs.workDisables = (WorkTags.Cleaning | WorkTags.Animals | WorkTags.Caring | WorkTags.Violent | WorkTags.ManualSkilled);
				bs.requiredWorkTags = WorkTags.None;
				bs.slot = BackstorySlot.Adulthood;
				bs.spawnCategories = new List<string>() { "rjw_nymphsCategory", "Slave" }; // Not necessary (I Think)
				Unprivater.SetProtectedValue("bodyTypeGlobal", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeFemale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeMale", bs, "Thin");
				Unprivater.SetProtectedValue("bodyTypeGlobalResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeFemaleResolved", bs, BodyTypeDefOf.Thin);
				Unprivater.SetProtectedValue("bodyTypeMaleResolved", bs, BodyTypeDefOf.Thin);
				bs.forcedTraits = new List<TraitEntry>();
				bs.forcedTraits.Add(new TraitEntry(xxx.nymphomaniac, 0));
				bs.disallowedTraits = new List<TraitEntry>();
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesMen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesWomen, 0));
				bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.TooSmart, 0));
				bs.shuffleable = true;
				bs.ResolveReferences();
				BackstoryDatabase.AddBackstory(bs);
				adult.broken = bs;
				//--ModLog.Message("nymph_backstories::init() succeed5");
			}

			//{
			//	Backstory bs = new Backstory();
			//	bs.identifier = "";
			//	MiscTranslationDef MTdef = DefDatabase<MiscTranslationDef>.GetNamedSilentFail("rjw_homekeeper");
			//	if (MTdef != null)
			//	{
			//		bs.SetTitle(MTdef.label, MTdef.label);
			//		bs.SetTitleShort(MTdef.stringA, MTdef.stringA);
			//		bs.baseDesc = MTdef.description;
			//	}
			//	else
			//	{
			//		bs.SetTitle("Home keeper Nymph", "Home keeper  Nymph");
			//		bs.SetTitleShort("Nymph", "Nymph");
			//		bs.baseDesc = "Home keeper Nymph";
			//	}
			//	bs.skillGainsResolved.Add(SkillDefOf.Cooking, 8);
			//	bs.workDisables = (WorkTags.Animals | WorkTags.Caring | WorkTags.Violent | WorkTags.Artistic | WorkTags.Crafting | WorkTags.PlantWork | WorkTags.Mining);
			//	bs.requiredWorkTags = (WorkTags.Cleaning | WorkTags.Cooking);
			//	bs.slot = BackstorySlot.Adulthood;
			//	bs.spawnCategories = new List<string>() { "rjw_nymphsCategory", "Slave" }; // Not necessary (I Think)
			//	Unprivater.SetProtectedValue("bodyTypeGlobal", bs, "Thin");
			//	Unprivater.SetProtectedValue("bodyTypeFemale", bs, "Thin");
			//	Unprivater.SetProtectedValue("bodyTypeMale", bs, "Thin");
			//	Unprivater.SetProtectedValue("bodyTypeGlobalResolved", bs, BodyTypeDefOf.Thin);
			//	Unprivater.SetProtectedValue("bodyTypeFemaleResolved", bs, BodyTypeDefOf.Thin);
			//	Unprivater.SetProtectedValue("bodyTypeMaleResolved", bs, BodyTypeDefOf.Thin);
			//	bs.forcedTraits = new List<TraitEntry>();
			//	bs.forcedTraits.Add(new TraitEntry(xxx.nymphomaniac, 0));
			//	bs.disallowedTraits = new List<TraitEntry>();
			//	bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesMen, 0));
			//	bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.DislikesWomen, 0));
			//	bs.disallowedTraits.Add(new TraitEntry(TraitDefOf.TooSmart, 0));
			//	bs.shuffleable = true;
			//	bs.ResolveReferences();
			//	BackstoryDatabase.AddBackstory(bs);
			//	adult.homekeeper = bs;
			//	//--ModLog.Message("nymph_backstories::init() succeed6");
			//}
		}

		public static nymph_passion_chances get_passion_chances(Backstory child_bs, Backstory adult_bs, SkillDef skill_def)
		{
			var maj = 0.0f;
			var min = 0.0f;

			if (adult_bs == adult.feisty)
			{
				if (skill_def == SkillDefOf.Melee) { maj = 0.50f; min = 1.00f; }
				else if (skill_def == SkillDefOf.Shooting) { maj = 0.25f; min = 0.75f; }
				else if (skill_def == SkillDefOf.Social) { maj = 0.10f; min = 0.67f; }
			}
			else if (adult_bs == adult.curious)
			{
				if (skill_def == SkillDefOf.Construction) { maj = 0.15f; min = 0.40f; }
				else if (skill_def == SkillDefOf.Crafting) { maj = 0.50f; min = 1.00f; }
				else if (skill_def == SkillDefOf.Social) { maj = 0.20f; min = 1.00f; }
			}
			else if (adult_bs == adult.tender)
			{
				if (skill_def == SkillDefOf.Medicine) { maj = 0.20f; min = 0.60f; }
				else if (skill_def == SkillDefOf.Social) { maj = 0.50f; min = 1.00f; }
			}
			else if (adult_bs == adult.chatty)
			{
				if (skill_def == SkillDefOf.Social) { maj = 1.00f; min = 1.00f; }
			}
			else if (adult_bs == adult.broken)
			{
				if (skill_def == SkillDefOf.Artistic) { maj = 0.50f; min = 1.00f; }
				else if (skill_def == SkillDefOf.Social) { maj = 0.00f; min = 0.33f; }
			}
			else if (adult_bs == adult.homekeeper)
			{
				if (skill_def == SkillDefOf.Cooking) { maj = 0.50f; min = 1.00f; }
				else if (skill_def == SkillDefOf.Social) { maj = 0.00f; min = 0.33f; }
			}

			return new nymph_passion_chances(maj, min);
		}

		// Randomly chooses backstories and traits for a nymph
		[SyncMethod]
		public static nymph_story generate()
		{
			var tr = new nymph_story();

			tr.child = child.vatgrown_sex_slave;

			tr.traits = new List<Trait>();
			tr.traits.Add(new Trait(xxx.nymphomaniac, 0, true));

			//Rand.PopState();
			//Rand.PushState(RJW_Multiplayer.PredictableSeed());
			var beauty = 0;
			var rv2 = Rand.Value;
			var story = (BackstoryDatabase.allBackstories.Where(x => x.Value.spawnCategories.Contains("rjw_nymphsCategory") && x.Value.slot == BackstorySlot.Adulthood)).ToList().RandomElement().Value;

			if (story == adult.feisty)
			{
				tr.adult = adult.feisty;
				beauty = Rand.RangeInclusive(0, 2);
				if (rv2 < 0.33)
					tr.traits.Add(new Trait(TraitDefOf.Brawler));
				else if (rv2 < 0.67)
					tr.traits.Add(new Trait(TraitDefOf.Bloodlust));
				else 
					tr.traits.Add(new Trait(xxx.rapist));
			}
			else if (story == adult.curious)
			{
				tr.adult = adult.curious;
				beauty = Rand.RangeInclusive(0, 2);
				if (rv2 < 0.33)
					tr.traits.Add(new Trait(TraitDefOf.Transhumanist));
			}
			else if (story == adult.tender)
			{
				tr.adult = adult.tender;
				beauty = Rand.RangeInclusive(1, 2);
				if (rv2 < 0.50)
					tr.traits.Add(new Trait(TraitDefOf.Kind));
			}
			else if (story == adult.chatty)
			{
				tr.adult = adult.chatty;
				beauty = 2;
				if (rv2 < 0.33)
					tr.traits.Add(new Trait(TraitDefOf.Greedy));
			}
			else if (story == adult.broken)
			{
				tr.adult = adult.broken;
				beauty = Rand.RangeInclusive(0, 2);
				if (rv2 < 0.33)
					tr.traits.Add(new Trait(TraitDefOf.DrugDesire, 1));
				else if (rv2 < 0.67)
					tr.traits.Add(new Trait(TraitDefOf.DrugDesire, 2));
			}
			//else
			//{
			//	tr.adult = adult.homekeeper;
			//	beauty = Rand.RangeInclusive(1, 2);
			//	if (rv2 < 0.33)
			//		tr.traits.Add(new Trait(TraitDefOf.Kind));
			//}

			if (beauty > 0)
				tr.traits.Add(new Trait(TraitDefOf.Beauty, beauty, false));

			return tr;
		}
	}
}