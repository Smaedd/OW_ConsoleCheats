using DeveloperConsole;
using OWML.ModHelper;
using System;
using System.Reflection;
using UnityEngine;
using System.Linq;
using HarmonyLib;

namespace ConsoleCheats
{
    [ConsoleContainer]
    internal static class CheatRunner
    {
        public static void Log(string message, ConsoleLogType type = ConsoleLogType.Message) => ConsoleCheats.DevConsole.Log(message, type);

        private static PlayerResources PResources => Locator.GetPlayerTransform()?.GetComponent<PlayerResources>();
        private static JetpackThrusterModel Jetpack => PResources?.GetComponent<JetpackThrusterModel>();
        private static ShipThrusterModel ShipThruster => Locator.GetShipTransform()?.GetComponent<ShipThrusterModel>();

        private const float MAX_OXYGEN = 450f;
        private const float MAX_FUEL = 100f;
        private const float MAX_HEALTH = 100f;
        private const float MAX_BOOST = 1f;

        [ConsoleData("refill")]
        public static void Refill()
        {
            PResources._currentOxygen = MAX_OXYGEN;
            PResources._currentFuel = MAX_FUEL;
            PResources._currentHealth = MAX_HEALTH;
            Jetpack._boostChargeFraction = MAX_BOOST;

            Transform shipTransform = Locator.GetShipTransform();
            if (shipTransform != null)
            {
                foreach (ShipHull hull in shipTransform.GetComponentsInChildren<ShipHull>())
                {
                    hull._damaged = false;
                    hull._integrity = 1f;
                }

                foreach (ShipComponent component in shipTransform.GetComponentsInChildren<ShipComponent>())
                {
                    component.SetDamaged(false);
                }
            }

            Log("Refilling resources...");
        }

        [ConsoleData("toggle_launchcodes")]
        public static void ToggleLaunchCodes()
        {
            bool newState = !(PlayerData.IsLoaded() && PlayerData.KnowsLaunchCodes());

            DialogueConditionManager.SharedInstance.SetConditionState("TalkedToHornfels", newState);
            DialogueConditionManager.SharedInstance.SetConditionState("SCIENTIST_3", newState);
            DialogueConditionManager.SharedInstance.SetConditionState("LAUNCH_CODES_GIVEN", newState);
            StandaloneProfileManager.SharedInstance.currentProfileGameSave.SetPersistentCondition("LAUNCH_CODES_GIVEN", newState);

            if (newState)
                GlobalMessenger.FireEvent("LearnLaunchCodes");
            else
                GameObject.FindWithTag("Global")?.GetComponent<KeyInfoPromptController>()?.Invoke("OnLaunchCodesEntered", 0f);

            PlayerData.SaveCurrentGame();

            string learntStr = newState ? "learnt" : "unlearnt";
            Log($"Launch codes {learntStr}.");
        }

        [ConsoleData("toggle_eyecoords")]
        public static void ToggleEyeCoords()
        {
            const string factName = "OPC_EYE_COORDINATES_X1";

            ShipLogManager shipLog = Locator.GetShipLogManager();

            if (!(PlayerData.IsLoaded() && shipLog != null))
                return;

            bool shouldLearn = !shipLog.IsFactRevealed(factName);

            if (shouldLearn)
            {
                shipLog.RevealFact(factName, false, false);
            }
            else
            {
                ShipLogFactSave fact = StandaloneProfileManager.SharedInstance.currentProfileGameSave.shipLogFactSaves[factName];
                fact.newlyRevealed = false;
                fact.read = false;
                fact.revealOrder = -1;
            }

            GameObject.FindWithTag("Global")?.GetComponent<KeyInfoPromptController>()?._eyeCoordinatesPrompt?.SetVisibility(shouldLearn);

            PlayerData.SaveCurrentGame();

            string learntStr = shouldLearn ? "learnt" : "unlearnt";
            Log($"Eye coordinates {learntStr}.");
        }

        [ConsoleData("toggle_allfrequencies")]
        public static void ToggleAllFrequencies()
        {
            if (!PlayerData.IsLoaded())
                return;

            var freqs = (SignalFrequency[])Enum.GetValues(typeof(SignalFrequency));
            var knowsAll = freqs.All(f => (f == SignalFrequency.Default) || PlayerData.KnowsFrequency(f));

            if (knowsAll)
            {
                foreach (SignalFrequency frequency in freqs)
                {
                    if (frequency == SignalFrequency.Default || frequency == SignalFrequency.Traveler)
                        continue;

                    PlayerData.ForgetFrequency(frequency);
                }
            }
            else
            {
                foreach (SignalFrequency frequency in freqs)
                {
                    PlayerData.LearnFrequency(frequency);
                }
            }

            PlayerData.SaveCurrentGame();

            string freqStr = !knowsAll ? "enabled" : "disabled";
            Log($"All frequencies {freqStr}.");
        }

        [ConsoleData("toggle_allsignals")]
        public static void ToggleAllSignals()
        {
            if (!PlayerData.IsLoaded())
                return;

            if (StandaloneProfileManager.SharedInstance.currentProfileGameSave == null)
                return;

            var signals = (SignalName[])Enum.GetValues(typeof(SignalName));
            var knowsAll = signals.All(s => s == SignalName.Default && PlayerData.KnowsSignal(s));

            foreach (SignalName signal in signals)
            {
                if (knowsAll)
                    StandaloneProfileManager.SharedInstance.currentProfileGameSave.knownSignals.Remove((int)signal);
                else
                    StandaloneProfileManager.SharedInstance.currentProfileGameSave.knownSignals[(int)signal] = true;
            }

            PlayerData.SaveCurrentGame();

            string signStr = !knowsAll ? "enabled" : "disabled";
            Log($"All signals {signStr}.");
        }

        [ConsoleData("toggle_allfacts")]
        public static void ToggleAllFacts()
        {
            var shipLog = Locator.GetShipLogManager();
            if (shipLog == null)
                return;

            bool knowAll = shipLog._factList.All(x => x.IsRevealed());

            bool shouldLearn = !knowAll;
            foreach (ShipLogFact fact in shipLog._factList)
            {
                string factID = fact.GetID() ?? fact.GetEntryID();

                if (shouldLearn)
                {
                    shipLog.RevealFact(factID, false, false);
                }
                else
                {
                    ShipLogFactSave shipLogFactSave = StandaloneProfileManager.SharedInstance.currentProfileGameSave.shipLogFactSaves[factID];
                    shipLogFactSave.newlyRevealed = false;
                    shipLogFactSave.read = false;
                    shipLogFactSave.revealOrder = -1;
                }
            }

            PlayerData.SaveCurrentGame();

            string learnStr = shouldLearn ? "enabled" : "disabled";
            Log($"All facts and rumors {learnStr}.");
        }

        [ConsoleData("toggle_helmet")]
        public static void ToggleHelmet()
        {
            var suit = Locator.GetPlayerSuit();
            if (suit == null)
            {
                Log("Unable to get player suit in toggle_helmet", ConsoleLogType.Error);
                return;
            }

            bool wearingHelmet = suit.IsWearingSuit() && suit.IsWearingHelmet();
            if (wearingHelmet)
            {
                suit.RemoveHelmet();
            }
            else
            {
                suit.PutOnHelmet();
            }

            string enableStr = !wearingHelmet ? "enabled" : "disabled";
            Log($"Player helmet {enableStr}.");
        }

        [ConsoleData("player_gravity")]
        public static bool PlayerGravity
        {
            get => Locator.GetPlayerBody()?.GetComponentInChildren<ForceApplier>()?.GetApplyForces() ?? true;
            set => Locator.GetPlayerBody()?.GetComponentInChildren<ForceApplier>()?.SetApplyForces(value);
        }

        [ConsoleData("ship_gravity")]
        public static bool ShipGravity
        {
            get => Locator.GetShipBody()?.GetComponentInChildren<ForceApplier>()?.GetApplyForces() ?? true;
            set => Locator.GetShipBody()?.GetComponentInChildren<ForceApplier>()?.SetApplyForces(value);
        }

        [ConsoleData("player_collision")]
        public static bool PlayerCollision
        {
            get => Locator.GetPlayerBody()?.GetRequiredComponent<Rigidbody>()?.detectCollisions ?? true;
            set
            {
                if (Locator.GetPlayerBody() == null)
                    return;

                if (!value)
                    Locator.GetPlayerBody().DisableCollisionDetection();
                else
                    Locator.GetPlayerBody().EnableCollisionDetection();
            }
        }

        [ConsoleData("ship_collision")]
        public static bool ShipCollision
        {
            get => Locator.GetShipBody()?.GetRequiredComponent<Rigidbody>()?.detectCollisions ?? true;
            set
            {
                var shipBody = Locator.GetShipBody();
                if (shipBody == null)
                    return;

                if (!value)
                    shipBody.DisableCollisionDetection();
                else
                    shipBody.EnableCollisionDetection();

                foreach (Collider collider in shipBody.GetComponentsInChildren<Collider>())
                {
                    if (!collider.isTrigger)
                        collider.enabled = value;
                }
            }
        }

        [ConsoleData("player_fluid_collision")]
        public static bool PlayerFluidCollision
        {
            get => Locator.GetPlayerBody()?.GetComponentInChildren<ForceApplier>()?.GetApplyFluids() ?? true;
            set => Locator.GetPlayerBody()?.GetComponentInChildren<ForceApplier>()?.SetApplyFluids(value);
        }

        [ConsoleData("ship_fluid_collision")]
        public static bool ShipFluidCollision
        {
            get => Locator.GetShipBody()?.GetComponentInChildren<ForceApplier>()?.GetApplyFluids() ?? true;
            set => Locator.GetPlayerBody()?.GetComponentInChildren<ForceApplier>()?.SetApplyFluids(value);
        }

        [ConsoleData("toggle_training_suit")]
        public static void ToggleTrainingSuit()
        {
            var suit = Locator.GetPlayerSuit();
            if (suit == null)
            {
                Log("Unable to get player suit in toggle_training_suit", ConsoleLogType.Error);
                return;
            }

            bool hasSpaceSuit = suit.IsWearingSuit(false);
            bool hasTrainingSuit = !hasSpaceSuit && suit.IsWearingSuit();

            if (hasTrainingSuit)
                suit.RemoveSuit();
            else
            {
                if (hasSpaceSuit)
                {
                    suit.RemoveSuit(true);
                    suit.SuitUp(true, true);
                }
                else
                {
                    suit.SuitUp(true);
                }
            }

            string enableStr = !hasTrainingSuit ? "enabled" : "disabled";
            Log($"Training suit {enableStr}.");
        }

        [ConsoleData("toggle_suit")]
        public static void ToggleSuit()
        {
            var suit = Locator.GetPlayerSuit();
            if (suit == null)
            {
                Log("Unable to get player suit in toggle_suit", ConsoleLogType.Error);
                return;
            }

            bool hasSpaceSuit = suit.IsWearingSuit(false);
            bool hasTrainingSuit = !hasSpaceSuit && suit.IsWearingSuit();

            if (hasSpaceSuit)
                suit.RemoveSuit();
            else
            {
                if (hasTrainingSuit)
                {
                    suit.RemoveSuit(true);
                    suit.SuitUp(false, true);
                }
                else
                {
                    suit.SuitUp(false);
                }
            }

            string enableStr = !hasSpaceSuit ? "enabled" : "disabled";
            Log($"Spacesuit {enableStr}.");
        }

        [ConsoleData("god")]
        public static bool GodMode = false;

        [ConsoleData("infinite_fuel")]
        public static bool InfiniteFuel = false;

        [ConsoleData("infinite_oxygen")]
        public static bool InfiniteOxygen = false;

        [ConsoleData("infinite_health")]
        public static bool InfiniteHealth = false;

        [ConsoleData("infinite_boost")]
        public static bool InfiniteBoost = false;

        public static float LoopFreezeTime = float.NaN;
        [ConsoleData("pause_loop")]
        public static bool PauseLoop
        {
            get => !float.IsNaN(LoopFreezeTime);
            set => LoopFreezeTime = value ? TimeLoop.GetSecondsRemaining() : float.NaN;
        }

        [ConsoleData("loop_time")]
        public static float LoopTime
        {
            get => TimeLoop.GetSecondsRemaining();
            set => TimeLoop.SetSecondsRemaining(value);
        }

        [ConsoleData("player_thrust")]
        public static float PlayerThrust
        {
            get => Jetpack?.GetMaxTranslationalThrust() ?? 6f;
            set
            {
                if (Jetpack == null)
                    return;

                // Compiler complains about Jetpack?. :(
                Jetpack._maxTranslationalThrust = value;
            }
        }

        [ConsoleData("ship_thrust")]
        public static float ShipThrust
        {
            get => ShipThruster?.GetMaxTranslationalThrust() ?? 50f;
            set
            {
                if (ShipThruster == null)
                    return;

                // Compiler complains about ShipThruster?. :(
                ShipThruster._maxTranslationalThrust = value;
            }
        }

        [ConsoleData("quantum_moon_collapse")]
        public static void QuantumMoonCollapse()
        {
            Locator.GetAstroObject(AstroObject.Name.QuantumMoon).GetComponentInChildren<QuantumMoon>().Invoke("Collapse", 0);
        }

        public static void Update()
        {
            if (InfiniteBoost)
                Jetpack._boostChargeFraction = MAX_BOOST;
            if (InfiniteFuel)
                PResources._currentFuel = MAX_FUEL;
            if (InfiniteHealth)
                PResources._currentHealth = MAX_HEALTH;
            if (InfiniteOxygen)
                PResources._currentOxygen = MAX_OXYGEN;

            if (GodMode)
                PResources._invincible = true;

            if (PauseLoop)
                TimeLoop.SetSecondsRemaining(LoopFreezeTime);

            AIRunner.Update();
            DebugRunner.Update();
        }
    }

    public class ConsoleCheats : ModBehaviour
    {
        public static ConsoleWrapper DevConsole = null;

        private void Start()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            var iManager = ModHelper.Interaction.TryGetModApi<IConsoleManager>("Smaed.DeveloperConsole");
            DevConsole = new(iManager);

            DevConsole.Link(Assembly.GetExecutingAssembly());
        }

        private void Update()
        {
            CheatRunner.Update();
        }
    }
}
