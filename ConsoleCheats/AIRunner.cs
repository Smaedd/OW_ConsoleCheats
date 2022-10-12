﻿using DeveloperConsole;
using System.Collections.Generic;
using HarmonyLib;

namespace ConsoleCheats
{
    [HarmonyPatch]
    [ConsoleContainer]
    internal static class AIRunner
    {
        public static void Log(string message, ConsoleLogType type = ConsoleLogType.Message) => ConsoleCheats.DevConsole.Log(message, type);

        private static HashSet<AnglerfishController> _AnglerFish = new();

        private static void UpdateAnglerfish(AnglerfishController controller)
        {
            controller.enabled = _AnglerFishAI;
        }

        private static void UpdateAllAnglerfish()
        {
            List<AnglerfishController> toRemove = new();

            foreach (AnglerfishController controller in _AnglerFish)
            {
                if (controller == null)
                {
                    toRemove.Add(controller);
                    continue;
                }

                UpdateAnglerfish(controller);
            }

            // Clean up here since we can't override Destroy()
            foreach (AnglerfishController controller in toRemove)
            {
                _AnglerFish.Remove(controller);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AnglerfishController), nameof(AnglerfishController.Awake))]
        private static void AddAnglerfish(AnglerfishController __instance)
        {
            _AnglerFish.Add(__instance);
            UpdateAnglerfish(__instance);
        }

        private static bool _AnglerFishAI = true;

        [Console("anglerfish_ai")]
        public static bool AnglerfishAI
        {
            get => _AnglerFishAI;
            set
            {
                _AnglerFishAI = value;
                
                UpdateAllAnglerfish();
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WaitAction), nameof(WaitAction.CalculateUtility))]
        private static bool DisableNonHostileInhabitants(ref float __result)
        {
            if (InhabitantsAI)
                return true;

            __result = float.MaxValue;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ChaseAction), nameof(WaitAction.CalculateUtility))]
        [HarmonyPatch(typeof(HuntAction), nameof(WaitAction.CalculateUtility))]
        [HarmonyPatch(typeof(StalkAction), nameof(WaitAction.CalculateUtility))]
        [HarmonyPatch(typeof(GrabAction), nameof(WaitAction.CalculateUtility))]
        private static bool DisableHostileInhabitants(ref float __result)
        {
            if (InhabitantsHostile)
                return true;

            __result = float.MinValue;
            return false;
        }

        [Console("inhabitants_ai")]
        public static bool InhabitantsAI = true;

        [Console("inhabitants_hostile")]
        public static bool InhabitantsHostile = true;

        public static void Update()
        {
            UpdateAllAnglerfish();
        }
    }
}
