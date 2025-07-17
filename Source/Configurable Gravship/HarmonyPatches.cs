using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GUI;

namespace GravshipSize
{
    [HarmonyPatch]
    public class HarmonyPatches
    {
        [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
        [HarmonyPrefix]
        public static void GenerateImpliedDefs_Prefix(bool hotReload)
        {
            GravshipSizeSettings.ApplySettingsNow();
        }
    }
}
