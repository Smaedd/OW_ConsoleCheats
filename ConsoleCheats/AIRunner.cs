using DeveloperConsole;
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

        [ConsoleData("anglerfish_ai", "Stores whether the anglerfish AI is enabled or not")]
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

        [ConsoleData("inhabitants_ai", "Stores whether the 'inhabitants' (EOTE) AI is enabled or not")]
        public static bool InhabitantsAI = true;

        [ConsoleData("inhabitants_hostile", "Stores whether the 'inhabitants' (EOTE) AI are hostile or not")]
        public static bool InhabitantsHostile = true;

        public static void Update()
        {
            UpdateAllAnglerfish();
        }
    }
}
