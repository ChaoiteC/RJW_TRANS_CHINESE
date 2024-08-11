using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace rjw
{
	class OviHelper
	{
		static readonly IDictionary<PawnKindDef, ThingDef> FertileEggPawnKinds = new Dictionary<PawnKindDef, ThingDef>();
		static readonly IDictionary<PawnKindDef, ThingDef> UnfertileEggPawnKinds = new Dictionary<PawnKindDef, ThingDef>();
		public static CompProperties_EggLayer GenerateEggLayerProperties(PawnKindDef pawnKindDef, RaceGroupDef raceGroupDef)
		{
			CompProperties_EggLayer comp = new CompProperties_EggLayer();
			comp.eggFertilizedDef = DefDatabase<ThingDef>.GetNamed(raceGroupDef.eggFertilizedDef);
			comp.eggUnfertilizedDef = DefDatabase<ThingDef>.GetNamed(raceGroupDef.eggUnfertilizedDef);
			comp.eggProgressUnfertilizedMax = raceGroupDef.eggProgressUnfertilizedMax;
			comp.eggLayIntervalDays = raceGroupDef.eggLayIntervalDays;
			//comp.eggCountRange = raceGroupDef.eggCountRange;
			//comp.eggFertilizationCountMax = raceGroupDef.eggFertilizationCountMax;
			//comp.eggLayFemaleOnly = raceGroupDef.eggLayFemaleOnly;

			return comp;
		}

		//private static ThingDef createFertilizedDef(PawnKindDef pawnKindDef, RaceGroupDef raceGroupDef)
		//{
		//	ThingDef eggThingDef;
		//	if (FertileEggPawnKinds.TryGetValue(pawnKindDef, out eggThingDef))
		//	{
		//		return eggThingDef;
		//	}

		//	eggThingDef = EggFertBase();
		//	eggThingDef.defName = eggThingDef.defName + "_" + pawnKindDef.race.defName;

		//	eggThingDef.label = eggThingDef.label.Replace("PAWNKIND", pawnKindDef.race.label);
		//	eggThingDef.description = eggThingDef.label.Replace("PAWNKIND", pawnKindDef.race.label);

		//	CompRJWProperties_Hatcher compPropHatcher = new CompRJWProperties_Hatcher();
		//	compPropHatcher.hatcherDaystoHatch = 3.5f; // This should probably be some sort of race property
		//	compPropHatcher.hatcherPawn = pawnKindDef;

		//	eggThingDef.comps.Add(compPropHatcher);

		//	FertileEggPawnKinds.Add(pawnKindDef, eggThingDef);

		//	return eggThingDef;
		//}

		//private static ThingDef createUnfertilizedDef(PawnKindDef pawnKindDef, RaceGroupDef raceGroupDef)
		//{
		//	ThingDef eggThingDef;
		//	if (UnfertileEggPawnKinds.TryGetValue(pawnKindDef, out eggThingDef))
		//	{
		//		return eggThingDef;
		//	}

		//	eggThingDef = EggUnfertBase();
		//	eggThingDef.defName = eggThingDef.defName + "_" + pawnKindDef.race.defName;

		//	eggThingDef.label = eggThingDef.label.Replace("PAWNKIND", pawnKindDef.race.label);
		//	eggThingDef.description = eggThingDef.label.Replace("PAWNKIND", pawnKindDef.race.label);

		//	UnfertileEggPawnKinds.Add(pawnKindDef, eggThingDef);

		//	return eggThingDef;
		//}

		//private static ThingDef EggFertBase()
		//{
		//	ThingDef baseEgg = ThingDef.Named("RjwFertEggPawn");

		//	// There's no easy way to deep-copy a ThingDef, so here we go...
		//	ThingDef copiedEgg = new ThingDef();

		//	copiedEgg.thingClass = baseEgg.thingClass;
		//	copiedEgg.category = baseEgg.category;
		//	copiedEgg.resourceReadoutPriority = baseEgg.resourceReadoutPriority;
		//	copiedEgg.useHitPoints = baseEgg.useHitPoints;
		//	copiedEgg.altitudeLayer = baseEgg.altitudeLayer;
		//	copiedEgg.stackLimit = baseEgg.stackLimit;
		//	copiedEgg.alwaysHaulable = baseEgg.alwaysHaulable;
		//	copiedEgg.drawGUIOverlay = baseEgg.drawGUIOverlay;
		//	copiedEgg.rotatable = baseEgg.rotatable;
		//	copiedEgg.pathCost = baseEgg.pathCost;

		//	copiedEgg.defName = baseEgg.defName;
		//	copiedEgg.label = baseEgg.label;
		//	copiedEgg.description = baseEgg.description;
		//	copiedEgg.selectable = baseEgg.selectable;
		//	copiedEgg.tickerType = baseEgg.tickerType;
		//	copiedEgg.healthAffectsPrice = baseEgg.healthAffectsPrice;

		//	copiedEgg.graphicData = new GraphicData();
		//	copiedEgg.graphicData.texPath = baseEgg.graphicData.texPath;
		//	copiedEgg.graphicData.color = baseEgg.graphicData.color;
		//	copiedEgg.graphicData.graphicClass = baseEgg.graphicData.graphicClass;

		//	copiedEgg.ingestible = new IngestibleProperties();
		//	copiedEgg.ingestible.parent = baseEgg.ingestible.parent;
		//	copiedEgg.ingestible.canAutoSelectAsFoodForCaravan = baseEgg.ingestible.canAutoSelectAsFoodForCaravan;
		//	copiedEgg.ingestible.drugCategory = baseEgg.ingestible.drugCategory;
		//	copiedEgg.ingestible.optimalityOffsetFeedingAnimals = baseEgg.ingestible.optimalityOffsetFeedingAnimals;
		//	copiedEgg.ingestible.optimalityOffsetHumanlikes = baseEgg.ingestible.optimalityOffsetHumanlikes;
		//	copiedEgg.ingestible.nurseable = baseEgg.ingestible.nurseable;
		//	copiedEgg.ingestible.preferability = baseEgg.ingestible.preferability;
		//	copiedEgg.ingestible.sourceDef = baseEgg.ingestible.sourceDef;
		//	copiedEgg.ingestible.joyKind = baseEgg.ingestible.joyKind;
		//	copiedEgg.ingestible.joy = baseEgg.ingestible.joy;
		//	copiedEgg.ingestible.foodType = baseEgg.ingestible.foodType;
		//	copiedEgg.ingestible.ingestHoldOffsetStanding = baseEgg.ingestible.ingestHoldOffsetStanding;
		//	copiedEgg.ingestible.ingestReportStringEat = baseEgg.ingestible.ingestReportStringEat;
		//	copiedEgg.ingestible.ingestHoldUsesTable = baseEgg.ingestible.ingestHoldUsesTable;
		//	copiedEgg.ingestible.ingestCommandString = baseEgg.ingestible.ingestCommandString;
		//	copiedEgg.ingestible.maxNumToIngestAtOnce = baseEgg.ingestible.maxNumToIngestAtOnce;
		//	copiedEgg.ingestible.outcomeDoers = baseEgg.ingestible.outcomeDoers;
		//	copiedEgg.ingestible.baseIngestTicks = baseEgg.ingestible.baseIngestTicks;
		//	copiedEgg.ingestible.ingestReportString = baseEgg.ingestible.ingestReportString;
		//	copiedEgg.ingestible.useEatingSpeedStat = baseEgg.ingestible.useEatingSpeedStat;
		//	copiedEgg.ingestible.tasteThought = baseEgg.ingestible.tasteThought;
		//	copiedEgg.ingestible.chairSearchRadius = baseEgg.ingestible.chairSearchRadius;
		//	copiedEgg.ingestible.specialThoughtAsIngredient = baseEgg.ingestible.specialThoughtAsIngredient;
		//	copiedEgg.ingestible.ingestEffect = baseEgg.ingestible.ingestEffect;
		//	copiedEgg.ingestible.ingestEffectEat = baseEgg.ingestible.ingestEffectEat;
		//	copiedEgg.ingestible.ingestSound = baseEgg.ingestible.ingestSound;
		//	copiedEgg.ingestible.specialThoughtDirect = baseEgg.ingestible.specialThoughtDirect;

		//	copiedEgg.thingCategories = new List<ThingCategoryDef>
		//	{
		//		ThingCategoryDef.Named("EggsFertilized")
		//	};

		//	copiedEgg.statBases = new List<StatModifier>();
		//	foreach (StatModifier statMod in baseEgg.statBases)
		//	{
		//		StatModifier copiedMod = new StatModifier();
		//		copiedMod.stat = statMod.stat;
		//		copiedMod.value = statMod.value;

		//		copiedEgg.statBases.Add(copiedMod);
		//	}

		//	copiedEgg.comps = new List<CompProperties>();
		//	foreach (CompProperties compProp in baseEgg.comps)
		//	{
		//		if (compProp is CompProperties_TemperatureRuinable)
		//		{
		//			CompProperties_TemperatureRuinable baseTempRuin = compProp as CompProperties_TemperatureRuinable;
		//			CompProperties_TemperatureRuinable newTempRuin = new CompProperties_TemperatureRuinable
		//			{
		//				minSafeTemperature = baseTempRuin.minSafeTemperature,
		//				maxSafeTemperature = baseTempRuin.maxSafeTemperature,
		//				progressPerDegreePerTick = baseTempRuin.progressPerDegreePerTick
		//			};
		//			copiedEgg.comps.Add(newTempRuin);
		//		}

		//		if (compProp is CompProperties_Rottable)
		//		{
		//			CompProperties_Rottable baseRot = compProp as CompProperties_Rottable;
		//			CompProperties_Rottable newRot = new CompProperties_Rottable
		//			{
		//				daysToRotStart = baseRot.daysToRotStart,
		//				rotDestroys = baseRot.rotDestroys,
		//				disableIfHatcher = baseRot.disableIfHatcher
		//			};
		//			copiedEgg.comps.Add(newRot);
		//		}
		//	}

		//	return copiedEgg;
		//}

		//private static ThingDef EggUnfertBase()
		//{
		//	ThingDef baseEgg = ThingDef.Named("RjwUnfertEggPawn");

		//	// There's no easy way to deep-copy a ThingDef, so here we go...
		//	ThingDef copiedEgg = new ThingDef();

		//	copiedEgg.thingClass = baseEgg.thingClass;
		//	copiedEgg.category = baseEgg.category;
		//	copiedEgg.resourceReadoutPriority = baseEgg.resourceReadoutPriority;
		//	copiedEgg.useHitPoints = baseEgg.useHitPoints;
		//	copiedEgg.altitudeLayer = baseEgg.altitudeLayer;
		//	copiedEgg.stackLimit = baseEgg.stackLimit;
		//	copiedEgg.alwaysHaulable = baseEgg.alwaysHaulable;
		//	copiedEgg.drawGUIOverlay = baseEgg.drawGUIOverlay;
		//	copiedEgg.rotatable = baseEgg.rotatable;
		//	copiedEgg.pathCost = baseEgg.pathCost;

		//	copiedEgg.defName = baseEgg.defName;
		//	copiedEgg.label = baseEgg.label;
		//	copiedEgg.description = baseEgg.description;
		//	copiedEgg.selectable = baseEgg.selectable;
		//	copiedEgg.tickerType = baseEgg.tickerType;
		//	copiedEgg.healthAffectsPrice = baseEgg.healthAffectsPrice;

		//	copiedEgg.graphicData = new GraphicData();
		//	copiedEgg.graphicData.texPath = baseEgg.graphicData.texPath;
		//	copiedEgg.graphicData.color = baseEgg.graphicData.color;
		//	copiedEgg.graphicData.graphicClass = baseEgg.graphicData.graphicClass;

		//	copiedEgg.ingestible = new IngestibleProperties();
		//	copiedEgg.ingestible.parent = baseEgg.ingestible.parent;
		//	copiedEgg.ingestible.canAutoSelectAsFoodForCaravan = baseEgg.ingestible.canAutoSelectAsFoodForCaravan;
		//	copiedEgg.ingestible.drugCategory = baseEgg.ingestible.drugCategory;
		//	copiedEgg.ingestible.optimalityOffsetFeedingAnimals = baseEgg.ingestible.optimalityOffsetFeedingAnimals;
		//	copiedEgg.ingestible.optimalityOffsetHumanlikes = baseEgg.ingestible.optimalityOffsetHumanlikes;
		//	copiedEgg.ingestible.nurseable = baseEgg.ingestible.nurseable;
		//	copiedEgg.ingestible.preferability = baseEgg.ingestible.preferability;
		//	copiedEgg.ingestible.sourceDef = baseEgg.ingestible.sourceDef;
		//	copiedEgg.ingestible.joyKind = baseEgg.ingestible.joyKind;
		//	copiedEgg.ingestible.joy = baseEgg.ingestible.joy;
		//	copiedEgg.ingestible.foodType = baseEgg.ingestible.foodType;
		//	copiedEgg.ingestible.ingestHoldOffsetStanding = baseEgg.ingestible.ingestHoldOffsetStanding;
		//	copiedEgg.ingestible.ingestReportStringEat = baseEgg.ingestible.ingestReportStringEat;
		//	copiedEgg.ingestible.ingestHoldUsesTable = baseEgg.ingestible.ingestHoldUsesTable;
		//	copiedEgg.ingestible.ingestCommandString = baseEgg.ingestible.ingestCommandString;
		//	copiedEgg.ingestible.maxNumToIngestAtOnce = baseEgg.ingestible.maxNumToIngestAtOnce;
		//	copiedEgg.ingestible.outcomeDoers = baseEgg.ingestible.outcomeDoers;
		//	copiedEgg.ingestible.baseIngestTicks = baseEgg.ingestible.baseIngestTicks;
		//	copiedEgg.ingestible.ingestReportString = baseEgg.ingestible.ingestReportString;
		//	copiedEgg.ingestible.useEatingSpeedStat = baseEgg.ingestible.useEatingSpeedStat;
		//	copiedEgg.ingestible.tasteThought = baseEgg.ingestible.tasteThought;
		//	copiedEgg.ingestible.chairSearchRadius = baseEgg.ingestible.chairSearchRadius;
		//	copiedEgg.ingestible.specialThoughtAsIngredient = baseEgg.ingestible.specialThoughtAsIngredient;
		//	copiedEgg.ingestible.ingestEffect = baseEgg.ingestible.ingestEffect;
		//	copiedEgg.ingestible.ingestEffectEat = baseEgg.ingestible.ingestEffectEat;
		//	copiedEgg.ingestible.ingestSound = baseEgg.ingestible.ingestSound;
		//	copiedEgg.ingestible.specialThoughtDirect = baseEgg.ingestible.specialThoughtDirect;

		//	copiedEgg.thingCategories = new List<ThingCategoryDef>
		//	{
		//		ThingCategoryDef.Named("EggsUnfertilized")
		//	};

		//	copiedEgg.statBases = new List<StatModifier>();
		//	foreach (StatModifier statMod in baseEgg.statBases)
		//	{
		//		StatModifier copiedMod = new StatModifier();
		//		copiedMod.stat = statMod.stat;
		//		copiedMod.value = statMod.value;

		//		copiedEgg.statBases.Add(copiedMod);
		//	}

		//	copiedEgg.comps = new List<CompProperties>();
		//	foreach (CompProperties compProp in baseEgg.comps)
		//	{
		//		if (compProp is CompProperties_TemperatureRuinable)
		//		{
		//			CompProperties_TemperatureRuinable baseTempRuin = compProp as CompProperties_TemperatureRuinable;
		//			CompProperties_TemperatureRuinable newTempRuin = new CompProperties_TemperatureRuinable
		//			{
		//				minSafeTemperature = baseTempRuin.minSafeTemperature,
		//				maxSafeTemperature = baseTempRuin.maxSafeTemperature,
		//				progressPerDegreePerTick = baseTempRuin.progressPerDegreePerTick
		//			};
		//			copiedEgg.comps.Add(newTempRuin);
		//		}

		//		if (compProp is CompProperties_Rottable)
		//		{
		//			CompProperties_Rottable baseRot = compProp as CompProperties_Rottable;
		//			CompProperties_Rottable newRot = new CompProperties_Rottable
		//			{
		//				daysToRotStart = baseRot.daysToRotStart,
		//				rotDestroys = baseRot.rotDestroys,
		//				disableIfHatcher = baseRot.disableIfHatcher
		//			};
		//			copiedEgg.comps.Add(newRot);
		//		}
		//	}

		//	return copiedEgg;
		//}
	}
}