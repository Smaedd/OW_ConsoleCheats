using DeveloperConsole;
using OWML.ModHelper;
using System.Reflection;
using PacificEngine.OW_CommonResources.Game.Player;
using PacificEngine.OW_CommonResources.Game.State;
using PacificEngine.OW_CommonResources.Game.Resource;

namespace ConsoleCheats
{
    [ConsoleContainer]
    internal static class CheatRunner
    {
        [Console("refill")]
        public static void Refill()
        {
            Player.oxygenSeconds = Player.maxOxygenSeconds;
            Player.fuelSeconds = Player.maxFuelSeconds;
            Player.health = Player.maxHealth;
            Player.boostSeconds = Player.maxBoostSeconds;
            Ship.repair();

            ConsoleCheats.DevConsole.Log("Refilling resources...");
        }

        [Console("toggle_launchcodes")]
        public static void ToggleLaunchCodes()
        {
            Data.launchCodes = !Data.launchCodes;

            string learntStr = Data.launchCodes ? "learnt" : "unlearnt";
            ConsoleCheats.DevConsole.Log($"Launch codes {learntStr}.");
        }

        [Console("toggle_eyecoords")]
        public static void ToggleEyeCoords()
        {
            Data.eyeCoordinates = !Data.eyeCoordinates;

            string learntStr = Data.eyeCoordinates ? "learnt" : "unlearnt";
            ConsoleCheats.DevConsole.Log($"Eye coordinates {learntStr}.");
        }

        [Console("toggle_override_frequencies")]
        public static void ToggleOverrideFrequencies()
        {
            if (Data.knowAllFrequencies && Data.knowAllSignals)
            {
                Data.knowAllSignals = false;
                Data.knowAllFrequencies = false;
            }
            else if (Data.knowAllFrequencies)
            {
                Data.knowAllSignals = true;
            }
            else
            {
                Data.knowAllFrequencies = true;
            }

            string freqStr = Data.knowAllFrequencies ? "enabled" : "disabled";
            string signStr = Data.knowAllSignals ? "enabled" : "disabled";

            ConsoleCheats.DevConsole.Log($"All frequencies override {freqStr}.");
            ConsoleCheats.DevConsole.Log($"All signals override {signStr}.");
        }

        [Console("toggle_override_facts")]
        public static void ToggleOverrideFacts()
        {
            if (Data.knowAllRumors && Data.knowAllFacts)
            {
                Data.knowAllRumors = false;
                Data.knowAllFacts = false;
            }
            else if (Data.knowAllRumors)
            {
                Data.knowAllFacts = true;
            }
            else
            {
                Data.knowAllRumors = true;
            }

            string rumoStr = Data.knowAllRumors ? "enabled" : "disabled";
            string factStr = Data.knowAllFacts ? "enabled" : "disabled";

            ConsoleCheats.DevConsole.Log($"All rumors override {rumoStr}.");
            ConsoleCheats.DevConsole.Log($"All facts override {factStr}.");
        }

        [Console("warp")]
        public static void Warp(string location)
        {
            switch (location.ToLower()) 
            {
                case "sun":
                    Teleportation.teleportPlayerToSun();
                    break;
                case "sunstation":
                    Teleportation.teleportPlayerToSunStation();
                    break;
                case "embertwin":
                    Teleportation.teleportPlayerToEmberTwin();
                    break;
                case "ashtwin":
                    Teleportation.teleportPlayerToAshTwin();
                    break;
                case "atp":
                case "ashtwinproject":
                    Teleportation.teleportPlayerToAshTwinProject();
                    break;
                case "timberhearth":
                    Teleportation.teleportPlayerToTimberHearth();
                    break;
                case "timberhearthprobe":
                    Teleportation.teleportPlayerToTimberHearthProbe();
                    break;
                case "attlerock":
                    Teleportation.teleportPlayerToAttlerock();
                    break;
                case "brittlehollow":
                    Teleportation.teleportPlayerToBlackHoleForgeTeleporter();
                    break;
                case "hollowlantern":
                case "hollowslantern":
                    Teleportation.teleportPlayerToHollowLattern();
                    break;
                case "giantsdeep":
                    Teleportation.teleportPlayerToGiantsDeep();
                    break;
                case "probecannon":
                    Teleportation.teleportPlayerToProbeCannon();
                    break;
                case "probecannonmodule":
                    Teleportation.teleportPlayerToProbeCannonCommandModule();
                    break;
                case "darkbramble":
                    Teleportation.teleportPlayerToDarkBramble();
                    break;
                case "vessel":
                    Teleportation.teleportPlayerToVessel();
                    break;
                case "ship":
                    Teleportation.teleportPlayerToShip();
                    break;
                case "probe":
                case "scout":
                    Teleportation.teleportPlayerToProbe();
                    break;
                case "nomaiprobe":
                    Teleportation.teleportPlayerToNomaiProbe();
                    break;
                case "interloper":
                    Teleportation.teleportPlayerToInterloper();
                    break;
                case "whitehole":
                    Teleportation.teleportPlayerToWhiteHole();
                    break;
                case "whiteholestation":
                    Teleportation.teleportPlayerToWhiteHoleStation();
                    break;
                case "stranger":
                    Teleportation.teleportPlayerToStranger();
                    break;
                case "dreamworld":
                    Teleportation.teleportPlayerToDreamWorld();
                    break;
                case "quantummoon":
                    Teleportation.teleportPlayerToQuantumMoon();
                    break;
                default:
                    ConsoleCheats.DevConsole.Log("Invalid location!");
                    return;
            };

            ConsoleCheats.DevConsole.Log($"Warped to {location}.");
        }

        [Console("recall_ship")]
        public static void RecallShip()
        {
            Teleportation.teleportShipToPlayer();

            ConsoleCheats.DevConsole.Log("Teleported the ship to your location.");
        }

        [Console("toggle_helmet")]
        public static void ToggleHelmet()
        {
            Player.helmet = !Player.helmet;

            string enableStr = Player.helmet ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Player helmet {enableStr}.");
        }

        [Console("toggle_player_gravity")]
        public static void TogglePlayerGravity()
        {
            Player.gravity = !Player.gravity;

            string enableStr = Player.gravity ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Player gravity {enableStr}.");
        }

        [Console("toggle_ship_gravity")]
        public static void ToggleShipGravity()
        {
            Ship.gravity = !Ship.gravity;

            string enableStr = Ship.gravity ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Ship gravity {enableStr}.");
        }

        [Console("toggle_player_collision")]
        public static void TogglePlayerCollision()
        {
            Player.collision = !Player.collision;

            string enableStr = Player.collision ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Player collision {enableStr}.");
        }

        [Console("toggle_ship_collision")]
        public static void ToggleShipCollision()
        {
            Ship.collision = !Ship.collision;

            string enableStr = Ship.collision ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Ship collision {enableStr}.");
        }

        [Console("toggle_player_fluid_collision")]
        public static void TogglePlayerFluidCollision()
        {
            Player.fluidCollision = !Player.fluidCollision;

            string enableStr = Player.fluidCollision ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Player fluid collision {enableStr}.");
        }

        [Console("toggle_ship_fluid_collision")]
        public static void ToggleShipFluidCollision()
        {
            Ship.fluidCollision = !Ship.fluidCollision;

            string enableStr = Ship.fluidCollision ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Ship fluid collision {enableStr}.");
        }

        [Console("toggle_training_suit")]
        public static void ToggleTrainingSuit()
        {
            Player.trainingSuit = !Player.trainingSuit;

            string enableStr = Player.trainingSuit ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Training suit {enableStr}.");
        }

        [Console("toggle_suit")]
        public static void ToggleSuit()
        {
            Player.spaceSuit = !Player.spaceSuit;

            string enableStr = Player.spaceSuit ? "enabled" : "disabled";
            ConsoleCheats.DevConsole.Log($"Spacesuit {enableStr}.");
        }

        [Console("god")]
        public static bool GodMode
        {
            get => Player.isInvincible;
            set => Player.isInvincible = value;
        }

        [Console("infinite_fuel")]
        public static bool InfiniteFuel
        {
            get => Player.hasUnlimitedFuel;
            set => Player.hasUnlimitedFuel = value;
        }

        [Console("infinite_oxygen")]
        public static bool InfiniteOxygen
        {
            get => Player.hasUnlimitedOxygen;
            set => Player.hasUnlimitedOxygen = value;
        }

        [Console("infinite_health")]
        public static bool InfiniteHealth
        {
            get => Player.hasUnlimitedHealth;
            set => Player.hasUnlimitedHealth = value;
        }

        [Console("infinite_boost")]
        public static bool InfiniteBoost
        {
            get => Player.hasUnlimitedBoost;
            set => Player.hasUnlimitedBoost = value;
        }

        [Console("anglerfish_ai")]
        public static bool AnglerfishAI
        {
            get => Anglerfish.enabledAI ?? true;
            set => Anglerfish.enabledAI = value;
        }

        [Console("inhabitants_ai")]
        public static bool InhabitantsAI
        {
            get => Inhabitants.enabledAI;
            set => Inhabitants.enabledAI = value;
        }

        [Console("inhabitants_hostile")]
        public static bool InhabitantsHostile
        {
            get => Inhabitants.enabledHostility;
            set => Inhabitants.enabledHostility = value;
        }

        [Console("toggle_loop_pause")]
        public static void ToggleLoopPause()
        {
            SuperNova.freeze = !SuperNova.freeze;

            string enableStr = SuperNova.freeze ? "paused" : "unpaused";
            ConsoleCheats.DevConsole.Log($"Time loop {enableStr}.");
        }

        [Console("loop_time")]
        public static float LoopTime
        {
            get => SuperNova.remaining;
            set => SuperNova.remaining = value;
        }

        [Console("player_thrust")]
        public static float PlayerThrust
        {
            get => Player.thrust;
            set => Player.thrust = value;
        }

        [Console("ship_thrust")]
        public static float ShipThrust
        {
            get => Ship.thrust;
            set => Ship.thrust = value;
        }

        [Console("quantum_moon_collapse")]
        public static void QuantumMoonCollapse()
        {
            QuantumMoonHelper.collapse();
        }

        [Console("give")]
        public static void Give(string item)
        {
            switch (item.ToLower())
            {
                case "warpcore":
                case "warpcorevessel":
                    Possession.pickUpWarpCore(WarpCoreType.Vessel);
                    break;
                case "warpcorebroken":
                    Possession.pickUpWarpCore(WarpCoreType.VesselBroken);
                    break;
                case "warpcoreblack":
                    Possession.pickUpWarpCore(WarpCoreType.Black);
                    break;
                case "warpcorewhite":
                    Possession.pickUpWarpCore(WarpCoreType.White);
                    break;
                case "warpcorenone":
                    Possession.pickUpWarpCore(WarpCoreType.SimpleBroken);
                    break;
                case "lantern":
                case "lanternbasic":
                    Possession.pickUpLantern(false, true);
                    break;
                case "lanternbroken":
                    Possession.pickUpLantern(true, false);
                    break;
                case "artifactgen1":
                    Possession.pickUpDreamLantern(DreamLanternType.Nonfunctioning, false);
                    break;
                case "artifactgen2":
                    Possession.pickUpDreamLantern(DreamLanternType.Malfunctioning, true);
                    break;
                case "artifact":
                case "artifactgen3":
                    Possession.pickUpDreamLantern(DreamLanternType.Functioning, false);
                    break;
                case "slide_story1":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Story_1, false);
                    break;
                case "slide_story2":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Story_2, false);
                    break;
                case "slide_story3":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Story_3, false);
                    break;
                case "slide_story4":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Story_4, false);
                    break;
                case "slide_story5":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Story_5_Complete, false);
                    break;
                case "slide_path1":
                    Possession.pickUpSlideReel(Items.SlideReelStory.LibraryPath_1, false);
                    break;
                case "slide_path2":
                    Possession.pickUpSlideReel(Items.SlideReelStory.LibraryPath_2, false);
                    break;
                case "slide_path3":
                    Possession.pickUpSlideReel(Items.SlideReelStory.LibraryPath_3, false);
                    break;
                case "slide_seal1":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Seal_1, false);
                    break;
                case "slide_seal2":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Seal_2, false);
                    break;
                case "slide_seal3":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Seal_3, false);
                    break;
                case "slide_rule1":
                    Possession.pickUpSlideReel(Items.SlideReelStory.DreamRule_1, false);
                    break;
                case "slide_rule2":
                    Possession.pickUpSlideReel(Items.SlideReelStory.DreamRule_2_v1, false);
                    break;
                case "slide_rule3":
                    Possession.pickUpSlideReel(Items.SlideReelStory.DreamRule_2_v2, false);
                    break;
                case "slide_rule4":
                    Possession.pickUpSlideReel(Items.SlideReelStory.DreamRule_3, false);
                    break;
                case "slide_burning":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Burning, false);
                    break;
                case "slide_experiment":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Experiment, false);
                    break;
                case "slide_damagereport":
                    Possession.pickUpSlideReel(Items.SlideReelStory.DamageReport, false);
                    break;
                case "slide_lanternsecret":
                    Possession.pickUpSlideReel(Items.SlideReelStory.LanternSecret, false);
                    break;
                case "slide_prisoner":
                    Possession.pickUpSlideReel(Items.SlideReelStory.PrisonPeephole_Vision, false);
                    break;
                case "slide_prisonerfarewell":
                    Possession.pickUpSlideReel(Items.SlideReelStory.PrisonerFarewellVision, false);
                    break;
                case "slide_tower":
                    Possession.pickUpSlideReel(Items.SlideReelStory.TowerVision, false);
                    break;
                case "slide_signaljammer":
                    Possession.pickUpSlideReel(Items.SlideReelStory.SignalJammer, false);
                    break;
                case "slide_homeworld":
                    Possession.pickUpSlideReel(Items.SlideReelStory.Homeworld, false);
                    break;
                case "slide_supernovaescape":
                    Possession.pickUpSlideReel(Items.SlideReelStory.SupernovaEscape, false);
                    break;
                default:
                    ConsoleCheats.DevConsole.Log($"Invalid item!");
                    return;
            }

            ConsoleCheats.DevConsole.Log($"Gave {item} to player.");
        }

        [Console("fog")]
        public static bool FogVar
        {
            get => Fog.enabled;
            set => Fog.enabled = value;
        }

        [Console("debug_playerpos")]
        public static bool DebugPlayerPosition
        {
            get => Position.debugPlayerPosition;
            set
            {
                Position.debugPlayerPosition = value;
                Position.logFrequency = value ? 1000 : 0;
            }
        }

        [Console("debug_planetpos")]
        public static bool DebugPlanetPosition
        {
            get => Planet.debugPlanetPosition;
            set
            {
                Planet.debugPlanetPosition = value;
                Planet.logPlanetPositionFrequency = value ? 1000 : 0;
            }
        }

        [Console("debug_bramble")]
        public static bool DebugBramblePortals
        {
            get => BramblePortals.debugMode;
            set
            {
                BramblePortals.debugMode = value;
                BramblePortals.logFrequency = value ? 1000 : 0;
            }
        }

        [Console("debug_warppad")]
        public static bool DebugWarpPad
        {
            get => WarpPad.debugMode;
            set
            {
                WarpPad.debugMode = value;
                WarpPad.logFrequency = value ? 1000 : 0;
            }
        }

        [Console("debug_facts")]
        public static bool DebugFacts
        {
            get => Data.debugFacts;
            set => Data.debugFacts = value;
        }

        [Console("debug_saveconditions")]
        public static bool DebugSaveConditions
        {
            get => Data.debugPersistentConditions;
            set => Data.debugPersistentConditions = value;
        }

        [Console("debug_dialogconditions")]
        public static bool DebugDialogConditions
        {
            get => Data.debugDialogConditions;
            set => Data.debugDialogConditions = value;
        }
    }

    public class ConsoleCheats : ModBehaviour
    {
        public static ConsoleWrapper DevConsole = null;

        private void Start()
        {
            var iManager = ModHelper.Interaction.TryGetModApi<IConsoleManager>("Smaed.DeveloperConsole");
            DevConsole = new(iManager);

            DevConsole.Link(Assembly.GetExecutingAssembly());
        }
    }
}
