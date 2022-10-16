using DeveloperConsole;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ConsoleCheats
{
    [ConsoleContainer]
    internal static class WarpRunner
    {
        public static void Log(string message, ConsoleLogType type = ConsoleLogType.Message) => ConsoleCheats.DevConsole.Log(message, type);

        [ConsoleData("warp", "Warps the player to the given location")]
        public static void Warp(string location)
        {
            IEnumerable<Sector> GetSectors(Sector.Name name) => SectorManager.GetRegisteredSectors().Where(s => s.GetName().Equals(name));

            OWRigidbody destination;
            Vector3 position = Vector3.zero;
            Vector3 velocity = Vector3.zero;
            Vector3 angularVelocity = Vector3.zero;
            Vector3 acceleration = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            bool ignoreSand = false;
            bool relativeToDestination = true;

            switch (location.ToLower())
            {
                case "sun":
                    destination = Locator.GetAstroObject(AstroObject.Name.Sun)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 5000f, 0f);
                    break;
                case "sunstation":
                    destination = Locator.GetMinorAstroObject("Sun Station")?.GetAttachedOWRigidbody();
                    break;
                case "embertwin":
                    destination = Locator.GetAstroObject(AstroObject.Name.CaveTwin)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 165f, 0f);
                    break;
                case "ashtwin":
                    destination = Locator.GetAstroObject(AstroObject.Name.TowerTwin)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 180f, 0f);
                    break;
                case "atp":
                case "ashtwinproject":
                    // Ignoresand
                    destination = Locator.GetAstroObject(AstroObject.Name.TowerTwin)?.GetAttachedOWRigidbody();
                    ignoreSand = true;
                    relativeToDestination = false;

                    if (destination == null)
                        break; // We handle this later

                    Transform platform = Locator.GetWarpReceiver(NomaiWarpPlatform.Frequency.TimeLoop).GetPlatformCenter();
                    var localPosition = platform.position - destination.GetPosition();
                    float ratio = 0f;
                    if (!PlayerState.IsInsideShip())
                    {
                        ratio = (localPosition.magnitude - 1.85f) / localPosition.magnitude;
                    }
                    else
                    {
                        ratio = (localPosition.magnitude - 6f) / localPosition.magnitude;
                    }
                    position = new Vector3(localPosition.x * ratio + destination.GetPosition().x, localPosition.y * ratio + destination.GetPosition().y, localPosition.z * ratio + destination.GetPosition().z);
                    velocity = destination.GetPointVelocity(position);
                    acceleration = destination.GetAcceleration();
                    rotation = platform.rotation;

                    break;
                case "timberhearth":
                    destination = Locator.GetAstroObject(AstroObject.Name.TimberHearth)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 280f, 0f);
                    break;
                case "timberhearthprobe":
                    destination = Locator.GetAstroObject(AstroObject.Name.TimberHearth)?.GetSatellite()?.GetAttachedOWRigidbody();
                    position = PlayerState.IsInsideShip() ? new Vector3(0f, 0f, -8f) : new Vector3(0f, 0f, -1f);
                    break;
                case "attlerock":
                    destination = Locator.GetAstroObject(AstroObject.Name.TimberHearth)?.GetMoon()?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 85f, 0f);
                    break;
                case "brittlehollow":
                    destination = Locator.GetAstroObject(AstroObject.Name.BrittleHollow)?.GetAttachedOWRigidbody();
                    relativeToDestination = false;

                    Transform bhfCenter = Locator.GetWarpReceiver(NomaiWarpPlatform.Frequency.BrittleHollowForge).GetPlatformCenter();

                    position = new Vector3(bhfCenter.position.x, bhfCenter.position.y - 2f, bhfCenter.position.z);
                    velocity = destination.GetVelocity();
                    angularVelocity = destination.GetAngularVelocity();
                    acceleration = destination.GetAcceleration();
                    rotation = bhfCenter.rotation;

                    break;
                case "hollowlantern":
                case "hollowslantern":
                    destination = Locator.GetAstroObject(AstroObject.Name.BrittleHollow)?.GetMoon()?.GetAttachedOWRigidbody();
                    position = PlayerState.IsInsideShip() ? new Vector3(27.9f, 98.6f, 34.7f) : new Vector3(30.3f, 92.8f, 34.2f);
                    break;
                case "giantsdeep":
                    destination = Locator.GetAstroObject(AstroObject.Name.GiantsDeep)?.GetAttachedOWRigidbody();
                    position = PlayerState.IsInsideShip() ? new Vector3(0f, 520f, 0f) : new Vector3(0f, 505f, 0f);
                    break;
                case "probecannon":
                    destination = Locator.GetAstroObject(AstroObject.Name.ProbeCannon)?.GetAttachedOWRigidbody();
                    break;
                case "probecannonmodule":
                    destination = Locator.GetAstroObject(AstroObject.Name.GiantsDeep)?.GetAttachedOWRigidbody();
                    GlobalMessenger.FireEvent("PlayerEnterGiantsDeep");
                    position = new Vector3(-14.5f, -76f, -16f);
                    break;
                case "darkbramble":
                    destination = Locator.GetAstroObject(AstroObject.Name.DarkBramble)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 950f, 0f);
                    break;
                case "vessel":
                    var vesselSectors = GetSectors(Sector.Name.VesselDimension);

                    destination = vesselSectors.First(body => OuterFogWarpVolume.Name.Vessel == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody();
                    position = new Vector3(149.1f, 11.9f, -8.6f);

                    GlobalMessenger.FireEvent("WarpPlayer");
                    if (PlayerState.IsInsideShip())
                        GlobalMessenger.FireEvent("EnterShip");
                    break;
                case "ship":
                    destination = Locator.GetShipBody();
                    break;
                case "probe":
                case "scout":
                    destination = Locator.GetProbe()?.GetAttachedOWRigidbody();
                    break;
                case "nomaiprobe":
                    destination = Locator.GetAstroObject(AstroObject.Name.ProbeCannon)?.GetComponent<OrbitalProbeLaunchController>()?._probeBody;
                    position = new Vector3(0f, 0f, -25f);
                    break;
                case "interloper":
                    destination = Locator.GetAstroObject(AstroObject.Name.Comet)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 85f, 0f);
                    break;
                case "whitehole":
                    destination = Locator.GetAstroObject(AstroObject.Name.WhiteHole)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 0f, 40f);
                    break;
                case "whiteholestation":
                    destination = Locator.GetAstroObject(AstroObject.Name.WhiteHoleTarget)?.GetAttachedOWRigidbody();
                    break;
                case "stranger":
                    destination = Locator.GetAstroObject(AstroObject.Name.RingWorld)?.GetAttachedOWRigidbody();
                    position = new Vector3(45.5f, -169f, -290f);
                    break;
                case "dreamworld":
                    destination = Locator.GetAstroObject(AstroObject.Name.DreamWorld)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 100f, 0f);
                    break;
                case "quantummoon":
                    destination = Locator.GetAstroObject(AstroObject.Name.QuantumMoon)?.GetAttachedOWRigidbody();
                    position = new Vector3(0f, 80f, 0f);

                    GlobalMessenger.FireEvent("WarpPlayer");
                    if (PlayerState.IsInsideShip())
                        GlobalMessenger.FireEvent("EnterShip");
                    break;
                default:
                    Log("Invalid location!", ConsoleLogType.Error);
                    return;
            };

            if (destination == null)
            {
                Log($"Failed to get destination for {location}!", ConsoleLogType.Error);
                return;
            }

            // Ignore sand
            if (PlayerState.IsInsideShip())
            {
                foreach (SandLevelController sandLevelController in Object.FindObjectsOfType<SandLevelController>())
                {
                    foreach (Collider collider in Locator.GetShipBody().GetComponentsInChildren<Collider>())
                    {
                        Physics.IgnoreCollision(sandLevelController.GetSandCollider(), collider, ignoreSand);
                    }
                }
            }

            GlobalMessenger<OWRigidbody>.FireEvent(ignoreSand ? "EnterTimeLoopCentral" : "ExitTimeLoopCentral", Locator.GetPlayerBody());

            if (relativeToDestination)
            {
                // Actually teleport the player
                position = destination.transform.TransformPoint(position);
                velocity = velocity + destination.GetPointTangentialVelocity(position) + destination.GetVelocity();
                angularVelocity = angularVelocity + destination.GetAngularVelocity();
                acceleration = acceleration + destination.GetAcceleration();
                rotation = rotation * destination.GetRotation();
            }

            OWRigidbody playerBody = PlayerState.IsInsideShip() ? Locator.GetShipBody() : Locator.GetPlayerBody();
            if (!playerBody)
            {
                Log($"Player/Ship body null in warp command!", ConsoleLogType.Error);
                return;
            }

            playerBody.SetPosition(position);
            playerBody.SetVelocity(velocity);
            playerBody.SetRotation(rotation);
            playerBody.SetAngularVelocity(angularVelocity);
            playerBody._lastPosition = position;
            playerBody._currentVelocity = velocity;
            playerBody._lastVelocity = velocity;
            playerBody._currentAngularVelocity = angularVelocity;
            playerBody._lastAngularVelocity = angularVelocity;
            playerBody._currentAccel = acceleration;
            playerBody._lastAccel = acceleration;

            Log($"Warped to {location}.");
        }

        [ConsoleData("recall_ship", "Teleports the ship to the player")]
        public static void RecallShip()
        {
            OWRigidbody shipBody = Locator.GetShipBody();
            OWRigidbody playerBody = Locator.GetPlayerBody();

            if (!shipBody || !playerBody)
            {
                Log("Unable to find ship/player body in recall_ship", ConsoleLogType.Error);
                return;
            }

            Vector3 position = playerBody.GetPosition();
            Vector3 velocity = playerBody.GetPointTangentialVelocity(position) + playerBody.GetVelocity();
            Vector3 angularVelocity = playerBody.GetAngularVelocity();
            Vector3 acceleration = playerBody.GetAcceleration();
            Quaternion rotation = playerBody.GetRotation();

            shipBody.SetPosition(position);
            shipBody.SetVelocity(velocity);
            shipBody.SetRotation(rotation);
            shipBody.SetAngularVelocity(angularVelocity);
            shipBody._lastPosition = position;
            shipBody._currentVelocity = velocity;
            shipBody._lastVelocity = velocity;
            shipBody._currentAngularVelocity = angularVelocity;
            shipBody._lastAngularVelocity = angularVelocity;
            shipBody._currentAccel = acceleration;
            shipBody._lastAccel = acceleration;

            Log("Teleported the ship to your location.");
        }
    }
}
