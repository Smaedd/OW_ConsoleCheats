using DeveloperConsole;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ConsoleCheats
{
    [ConsoleContainer]
    [HarmonyPatch]
    internal class FogRunner
    {
        private static bool _FogEnabled = true;

        private static HashSet<FogWarpVolume> _WarpVolumes = new HashSet<FogWarpVolume>();
        private static HashSet<PlanetaryFogController> _Controllers = new HashSet<PlanetaryFogController>();
        private static HashSet<FogOverrideVolume> _OverrideVolumes = new HashSet<FogOverrideVolume>();
        private static Dictionary<object, Color> _OriginalColors = new Dictionary<object, Color>();

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FogWarpVolume), nameof(FogWarpVolume.Awake))]
        private static void AddWarpVolume(ref FogWarpVolume __instance)
        {
            _WarpVolumes.Add(__instance);
            _OriginalColors.Add(__instance, __instance.GetFogColor());
            __instance._fogColor = _FogEnabled ? _OriginalColors[__instance] : Color.clear;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FogWarpVolume), nameof(FogWarpVolume.OnDestroy))]
        private static void RemoveWarpVolume(ref FogWarpVolume __instance)
        {
            _WarpVolumes.Remove(__instance);
            _OriginalColors.Remove(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlanetaryFogController), nameof(PlanetaryFogController.OnEnable))]
        private static void AddController(ref PlanetaryFogController __instance)
        {
            _Controllers.Add(__instance);
            _OriginalColors.Add(__instance, __instance.fogTint);
            __instance.fogTint = _FogEnabled ? _OriginalColors[__instance] : Color.clear;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlanetaryFogController), nameof(PlanetaryFogController.OnDisable))]
        private static void RemoveController(ref PlanetaryFogController __instance)
        {
            _Controllers.Remove(__instance);
            _OriginalColors.Remove(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FogOverrideVolume), nameof(FogOverrideVolume.Awake))]
        private static void AddOverrideVolume(ref FogOverrideVolume __instance)
        {
            _OverrideVolumes.Add(__instance);
            _OriginalColors.Add(__instance, __instance.tint);
            __instance.tint = _FogEnabled ? _OriginalColors[__instance] : Color.clear;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FogOverrideVolume), nameof(FogOverrideVolume.OnDestroy))]
        private static void RemoveOverrideVolume(ref FogOverrideVolume __instance)
        {
            _OverrideVolumes.Remove(__instance);
            _OriginalColors.Remove(__instance);
        }

        [Console("fog")]
        public static bool FogVar
        {
            get => _FogEnabled;
            set
            {
                _FogEnabled = value;

                foreach (FogWarpVolume warpVolume in _WarpVolumes)
                {
                    warpVolume._fogColor = value ? _OriginalColors[warpVolume] : Color.clear;
                }

                foreach (PlanetaryFogController controller in _Controllers)
                {
                    controller.fogTint = value ? _OriginalColors[controller] : Color.clear;
                }

                foreach (FogOverrideVolume overrideVolume in _OverrideVolumes)
                {
                    overrideVolume.tint = value ? _OriginalColors[overrideVolume] : Color.clear;
                }
            }
        }
    }
}
