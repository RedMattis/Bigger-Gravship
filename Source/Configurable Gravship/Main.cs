using Verse;
using System.Diagnostics.Eventing.Reader;
using System;
using System.Runtime;
using static UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure;
using UnityEngine;
using RimWorld;
using System.Linq;
using static GravshipSize.SettingsWidgets;

namespace GravshipSize
{
    [DefOf]
    public static class GSDefOfs
    {
        public static ThingDef GravEngine;
        public static ThingDef GravFieldExtender;
        public static ThingDef ChemfuelTank;
        public static ThingDef LargeChemfuelTank;
        public static ThingDef SmallThruster;
        public static ThingDef LargeThruster;
        public static ThingDef GravcorePowerCell;
    }

    public class DefEditGC : GameComponent
    {
        public DefEditGC(Game game) { }
        public override void LoadedGame()
        {
            GravshipSizeSettings.ApplySettingsNow();
        }
        public override void StartedNewGame()
        {
            GravshipSizeSettings.ApplySettingsNow();
        }
    }

    [StaticConstructorOnStartup]
    public partial class GravshipSizeSettings : Mod
    {

        public static GravshipSettings settings = null;
        public GravshipSizeSettings(ModContentPack content) : base(content)
        {
            settings = GetSettings<GravshipSettings>();
        }

        private static Vector2 scrollPosition = Vector2.zero;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listStd = new Listing_Standard();

            Rect rect = inRect.ContractedBy(10f);
            rect.height -= listStd.CurHeight;
            rect.y += listStd.CurHeight;
            Widgets.DrawBoxSolid(rect, Color.grey);
            Rect rect2 = rect.ContractedBy(1f);
            Widgets.DrawBoxSolid(rect2, new ColorInt(42, 43, 44).ToColor);
            Rect rect3 = rect2.ContractedBy(5f);
            Rect rect4 = rect3;
            rect4.x = 0f;
            rect4.y = 0f;
            rect4.width -= 20f;
            rect4.height = 950f;
            Widgets.BeginScrollView(rect3, ref scrollPosition, rect4);
            listStd.Begin(rect4.AtZero());

            // Add a new button to set settings to their defaults.
            if (listStd.ButtonText("BG_ModDefaults".Translate()))
            {
                SetDefault();
                ApplySettingsNow();
            }
            if (listStd.ButtonText("BG_LudeonDefaults".Translate()))
            {
                SetLudeonDefault();
                ApplySettingsNow();
            }

            listStd.GapLine();

            // Settings
            CreateSettingsSlider(listStd, "BG_GravEngine_Support".Translate(), ref settings.gravEngineSupport, 250, 5000, value => $"{value:F0}");
            CreateSettingsSlider(listStd, "BG_GravEngine_MaxDistance".Translate(), ref settings.gravEngineMaxRadius, 1f, 56, value => $"{value:F0}");


            CreateSettingsSlider(listStd, "BG_GravFieldExtender_Support".Translate(), ref settings.gravExtenderSupport, 100, 5000, value => $"{value:F0}");
            CreateSettingsSlider(listStd, "BG_GravFieldExtender_Max".Translate(), ref settings.gravExtenderMax, 1f, 42f, value => $"{value:F0}");
            CreateSettingsSlider(listStd, "BG_GravFieldExtender_MaxDistance".Translate(), ref settings.gravExtenderMaxRadius, 1f, 56, value => $"{value:F0}");
            CreateSettingsSlider(listStd, "BG_GravFieldExtender_MaxDistanceFromEngine".Translate(), ref settings.gravExtenderMaxDistanceFromEngine, 1f, 56, value => $"{value:F0}");

            listStd.GapLine();

            CreateSettingsSlider(listStd, "BG_ChemFuelTankSize".Translate(), ref settings.chemFuelTankSize, 100, 10000, value => $"{value:F0}");
            CreateSettingsSlider(listStd, "BG_LargeChemFuelTankSize".Translate(), ref settings.largeChemFuelTankSize, 300, 30000, value => $"{value:F0}");

            listStd.GapLine();

            CreateSettingsSlider(listStd, "BG_SmallThrusterRange".Translate(), ref settings.smallThrusterRange, 1f, 100f, value => $"{value:F0}");
            CreateSettingsSlider(listStd, "BG_LargeThrusterRange".Translate(), ref settings.largeThrusterRange, 1f, 300f, value => $"{value:F0}");

            listStd.GapLine();

            CreateSettingsSlider(listStd, "BG_GravCorePowerCellPower".Translate(), ref settings.gravCorePowerCellPower, 800, 5000, value => $"{value:F0}");

            listStd.GapLine();

            if (listStd.ButtonText("BG_ApplySettingsNow".Translate()))
            {
                ApplySettingsNow();
            }

            listStd.End();
            Widgets.EndScrollView();

            base.DoSettingsWindowContents(inRect);
        }

        private void SetDefault()
        {
            settings.gravEngineSupport = GravshipSettings.gravEngineSupport_Default;
            settings.gravEngineMaxRadius = GravshipSettings.gravEngineMaxRadius_Default;
            settings.gravExtenderSupport = GravshipSettings.gravExtenderSupport_Default;
            settings.gravExtenderMax = GravshipSettings.gravExtenderMax_Default;
            settings.gravExtenderMaxRadius = GravshipSettings.gravExtenderMaxRadius_Default;
            settings.gravExtenderMaxDistanceFromEngine = GravshipSettings.gravExtenderMaxDistanceFromEngine_Default;
            settings.chemFuelTankSize = GravshipSettings.chemFuelTankSize_Default;
            settings.largeChemFuelTankSize = GravshipSettings.largeChemFuelTankSize_Default;
            settings.smallThrusterRange = GravshipSettings.smallThrusterRange_Default;
            settings.largeThrusterRange = GravshipSettings.largeThrusterRange_Default;
            settings.gravCorePowerCellPower = GravshipSettings.gravCorePowerCellPower_Default;

        }

        private void SetLudeonDefault()
        {
            settings.gravEngineSupport = GravshipSettings.gravEngineSupport_LudeonDefault;
            settings.gravEngineMaxRadius = GravshipSettings.gravEngineMaxRadius_LudeonDefault;
            settings.gravExtenderSupport = GravshipSettings.gravExtenderSupport_LudeonDefault;
            settings.gravExtenderMax = GravshipSettings.gravExtenderMax_LudeonDefault;
            settings.gravExtenderMaxRadius = GravshipSettings.gravExtenderMaxRadius_LudeonDefault;
            settings.gravExtenderMaxDistanceFromEngine = GravshipSettings.gravExtenderMaxDistanceFromEngine_LudeonDefault;
            settings.chemFuelTankSize = GravshipSettings.chemFuelTankSize_LudeonDefault;
            settings.largeChemFuelTankSize = GravshipSettings.largeChemFuelTankSize_LudeonDefault;
            settings.smallThrusterRange = GravshipSettings.smallThrusterRange_LudeonDefault;
            settings.largeThrusterRange = GravshipSettings.largeThrusterRange_LudeonDefault;
            settings.gravCorePowerCellPower = GravshipSettings.gravCorePowerCellPower_LudeonDefault;
        }

        public static void ApplySettingsNow()
        {
            var engine = GSDefOfs.GravEngine;
            var extender = GSDefOfs.GravFieldExtender;

            engine.statBases.Where(x => x.stat == StatDefOf.SubstructureSupport).FirstOrDefault().value = settings.gravEngineSupport;
            //engine.SetStatBaseValue(StatDefOf.SubstructureSupport, settings.gravEngineSupport);

            CompProperties_SubstructureFootprint substructureFootprintEngine = engine.GetCompProperties<CompProperties_SubstructureFootprint>();
            if (substructureFootprintEngine != null)
            {
                substructureFootprintEngine.radius = settings.gravEngineMaxRadius;
            }


            CompProperties_GravshipFacility gravshipFacility = extender.GetCompProperties<CompProperties_GravshipFacility>();
            if (gravshipFacility != null)
            {
                gravshipFacility.maxSimultaneous = (int)settings.gravExtenderMax;
                gravshipFacility.statOffsets.Where(x => x.stat == StatDefOf.SubstructureSupport).FirstOrDefault().value = settings.gravExtenderSupport;
                gravshipFacility.maxDistance = settings.gravEngineMaxRadius;
            }
            CompProperties_SubstructureFootprint substructureFootprint = extender.GetCompProperties<CompProperties_SubstructureFootprint>();
            if (substructureFootprint != null)
            {
                substructureFootprint.radius = settings.gravExtenderMaxRadius;
            }

            var chemTank = GSDefOfs.ChemfuelTank;
            var largeChemTank = GSDefOfs.LargeChemfuelTank;

            CompProperties_Refuelable compProperties_Refuelable = chemTank.GetCompProperties<CompProperties_Refuelable>();
            if (compProperties_Refuelable != null)
            {
                compProperties_Refuelable.fuelCapacity = settings.chemFuelTankSize;
            }
            compProperties_Refuelable = largeChemTank.GetCompProperties<CompProperties_Refuelable>();
            if (compProperties_Refuelable != null)
            {
                compProperties_Refuelable.fuelCapacity = settings.largeChemFuelTankSize;
            }

            var smallThruster = GSDefOfs.SmallThruster;
            var largeThruster = GSDefOfs.LargeThruster;

            CompProperties_GravshipThruster gravshipThruster = smallThruster.GetCompProperties<CompProperties_GravshipThruster>();
            if (gravshipThruster != null)
            {
                gravshipThruster.statOffsets.Where(x => x.stat == StatDefOf.GravshipRange).FirstOrDefault().value = settings.smallThrusterRange;
            }
            gravshipThruster = largeThruster.GetCompProperties<CompProperties_GravshipThruster>();
            if (gravshipThruster != null)
            {
                gravshipThruster.statOffsets.Where(x => x.stat == StatDefOf.GravshipRange).FirstOrDefault().value = settings.largeThrusterRange;
            }

            var gravCorePowerCell = GSDefOfs.GravcorePowerCell;
            CompProperties_Power compProperties_Power = gravCorePowerCell.GetCompProperties<CompProperties_Power>();
            if (compProperties_Power != null)
            {
                //compProperties_Power.basePowerConsumption = -settings.gravCorePowerCellPower;
                var field = typeof(CompProperties_Power).GetField("basePowerConsumption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                field?.SetValue(compProperties_Power, -settings.gravCorePowerCellPower);
            }
        }

        public override string SettingsCategory()
        {
            return "BG_GravshipSettings".Translate();
        }
    }

    public class GravshipSettings : ModSettings
    {
        // Settings
        public float gravEngineSupport = gravEngineSupport_Default;
        public float gravEngineMaxRadius = gravEngineMaxRadius_Default;
        public float gravExtenderSupport = gravExtenderSupport_Default;
        public float gravExtenderMax = gravExtenderMax_Default;
        public float gravExtenderMaxRadius = gravExtenderMaxRadius_Default;
        public float gravExtenderMaxDistanceFromEngine = gravExtenderMaxDistanceFromEngine_Default;
        public float chemFuelTankSize = chemFuelTankSize_Default;
        public float largeChemFuelTankSize = largeChemFuelTankSize_Default;
        public float smallThrusterRange = smallThrusterRange_Default;
        public float largeThrusterRange = largeThrusterRange_Default;
        public float gravCorePowerCellPower = gravCorePowerCellPower_Default;


        // Default values
        public const float gravEngineSupport_Default = 500;
        public const float gravEngineMaxRadius_Default = 25.9f;
        public const float gravExtenderSupport_Default = 500;
        public const float gravExtenderMax_Default = 8f;
        public const float gravExtenderMaxRadius_Default = 25.9f;
        public const float gravExtenderMaxDistanceFromEngine_Default = 25.9f;
        public const float chemFuelTankSize_Default = 500;
        public const float largeChemFuelTankSize_Default = 1500;
        public const float smallThrusterRange_Default = 10f;
        public const float largeThrusterRange_Default = 16;
        public const float gravCorePowerCellPower_Default = 1200;

        // Ludeon defaults
        public const float gravEngineSupport_LudeonDefault = 500;
        public const float gravEngineMaxRadius_LudeonDefault = 18.9f;
        public const float gravExtenderSupport_LudeonDefault = 250;
        public const float gravExtenderMax_LudeonDefault = 6f;
        public const float gravExtenderMaxRadius_LudeonDefault = 18.9f;
        public const float gravExtenderMaxDistanceFromEngine_LudeonDefault = 18.9f;
        public const float chemFuelTankSize_LudeonDefault = 250;
        public const float largeChemFuelTankSize_LudeonDefault = 750;
        public const float smallThrusterRange_LudeonDefault = 10f;
        public const float largeThrusterRange_LudeonDefault = 16;
        public const float gravCorePowerCellPower_LudeonDefault = 1200;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref gravEngineSupport, "BG_gravEngineSupport", gravEngineSupport_Default);
            Scribe_Values.Look(ref gravEngineMaxRadius, "BG_gravEngineMaxDistance", gravEngineMaxRadius_Default);
            Scribe_Values.Look(ref gravExtenderSupport, "BG_gravExtenderSupport", gravExtenderSupport_Default);
            Scribe_Values.Look(ref gravExtenderMax, "BG_gravExtenderMax", gravExtenderMax_Default);
            Scribe_Values.Look(ref gravExtenderMaxDistanceFromEngine, "BG_gravExtenderMaxDistanceFromEngine", gravExtenderMaxDistanceFromEngine_Default);
            Scribe_Values.Look(ref gravExtenderMaxRadius, "BG_gravExtenderMaxDistance", gravExtenderMaxRadius_Default);
            Scribe_Values.Look(ref chemFuelTankSize, "BG_chemFuelTankSize", chemFuelTankSize_Default);
            Scribe_Values.Look(ref largeChemFuelTankSize, "BG_largeChemFuelTankSize", largeChemFuelTankSize_Default);
            Scribe_Values.Look(ref smallThrusterRange, "BG_smallThrusterRange", smallThrusterRange_Default);
            Scribe_Values.Look(ref largeThrusterRange, "BG_largeThrusterRange", largeThrusterRange_Default);
            Scribe_Values.Look(ref gravCorePowerCellPower, "BG_gravCorePowerCellPower", gravCorePowerCellPower_Default);
        }
    }
}
