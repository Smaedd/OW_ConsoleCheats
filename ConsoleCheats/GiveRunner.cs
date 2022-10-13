using DeveloperConsole;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace ConsoleCheats
{
    // Contains code specifically for the Give command. Tedious and long :(

    [ConsoleContainer]
    [HarmonyPatch]
    internal static class GiveRunner
    {
        public static void Log(string message, ConsoleLogType type = ConsoleLogType.Message) => ConsoleCheats.DevConsole.Log(message, type);
        private static void SetItemVisible(OWItem item, bool visible)
        {
            item._visible = visible;
            foreach (OWRenderer renderer in item._renderers)
            {
                if (renderer == null)
                    continue;

                renderer.enabled = true;
                renderer.SetActivation(true);
                renderer.SetLODActivation(visible);
                if (renderer.GetRenderer() != null)
                {
                    renderer.GetRenderer().enabled = true;
                }
            }

            foreach (ParticleSystem particles in item._particleSystems)
            {
                if (particles == null)
                    continue;

                if (visible)
                    particles.Play(true);
                else
                    particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            foreach (OWLight2 light in item._lights)
            {
                if (light == null)
                    continue;

                light.enabled = true;
                light.SetActivation(true);
                light.SetLODActivation(visible);
                if (light.GetLight() != null)
                {
                    light.GetLight().enabled = true;
                }
            }
        }

        private static WarpCoreItem CreateWarpCore(WarpCoreType type)
        {
            foreach (WarpCoreItem warpCoreItem in Resources.FindObjectsOfTypeAll<WarpCoreItem>())
            {
                if (warpCoreItem.GetWarpCoreType().Equals(type))
                {
                    WarpCoreItem warpCoreItem2 = Object.Instantiate(warpCoreItem, Locator.GetAstroObject(AstroObject.Name.Sun)?.transform ?? Locator.GetAstroObject(AstroObject.Name.Eye)?.transform ?? Locator.GetPlayerBody()?.transform);
                    SetItemVisible(warpCoreItem2, true);
                    return warpCoreItem2;
                }
            }

            return null;
        }

        private static SimpleLanternItem CreateLantern(bool broken, bool lit)
        {
            foreach (SimpleLanternItem existing in Resources.FindObjectsOfTypeAll<SimpleLanternItem>())
            {
                if (existing.IsInteractable() && broken == existing.name.Contains("BROKEN"))
                {
                    SimpleLanternItem newLantern = Object.Instantiate(existing, Locator.GetAstroObject(AstroObject.Name.Sun)?.transform ?? Locator.GetAstroObject(AstroObject.Name.Eye)?.transform ?? Locator.GetPlayerBody()?.transform);
                    SetItemVisible(newLantern, true);
                    newLantern._startsLit = lit;
                    newLantern._lit = lit;
                    newLantern._lightSourceShape = existing._lightSourceShape;
                    return newLantern;
                }
            }

            return null;
        }

        private static DreamLanternItem CreateDreamLantern(DreamLanternType type, bool lit)
        {
            foreach (DreamLanternItem existing in Resources.FindObjectsOfTypeAll<DreamLanternItem>())
            {
                if (existing.IsInteractable() && existing.GetLanternType().Equals(type))
                {
                    DreamLanternItem newLantern = Object.Instantiate(existing, Locator.GetAstroObject(AstroObject.Name.Sun)?.transform ?? Locator.GetAstroObject(AstroObject.Name.Eye)?.transform ?? Locator.GetPlayerBody()?.transform);
                    SetItemVisible(newLantern, true);
                    if (newLantern.GetLanternController() != null)
                    {
                        newLantern.SetLit(lit);
                    }

                    return newLantern;
                }
            }

            return null;
        }

        private enum SlideReelStory
        {
            None,
            Story_1,
            Story_2,
            Story_3,
            Story_4,
            Story_5_Complete,
            Story_5_NoVessel,
            Story_5_NoInterloper,
            Story_5_NoInterloperNoVessel,
            LibraryPath_1,
            LibraryPath_2,
            LibraryPath_3,
            Seal_1,
            Seal_2,
            Seal_3,
            DreamRule_1,
            DreamRule_2_v1,
            DreamRule_2_v2,
            DreamRule_3,
            Burning,
            Experiment,
            DamageReport,
            LanternSecret,
            SupernovaEscape,
            SignalJammer,
            Homeworld,
            PrisonPeephole_Vision,
            PrisonerFarewellVision,
            TowerVision
        }

        private static HashSet<SlideCollectionContainer> inverted = new();

        private static SlideReelItem CreateSlideReel(SlideReelStory type, bool burned)
        {
            string burnedText = (burned ? "Burned" : "Complete");
            bool invertColors = false;
            string name;
            string container;
            switch (type)
            {
                case SlideReelStory.Story_1:
                    name = "Reel_1_Story_" + burnedText;
                    container = name;
                    break;
                case SlideReelStory.Story_2:
                    name = "Reel_2_Story_" + burnedText;
                    container = name;
                    break;
                case SlideReelStory.Story_3:
                    name = "Reel_3_Story_" + burnedText;
                    container = name;
                    break;
                case SlideReelStory.Story_4:
                    name = "Reel_4_Story_Burned";
                    container = name;
                    if (!burned)
                    {
                        container = "Reel_4_Story_Vision";
                        invertColors = true;
                    }

                    break;
                case SlideReelStory.LibraryPath_1:
                    name = "Reel_1_LibraryPath";
                    container = name;
                    break;
                case SlideReelStory.LibraryPath_2:
                    name = "Reel_2_LibraryPath";
                    container = name;
                    break;
                case SlideReelStory.LibraryPath_3:
                    name = "Reel_3_LibraryPath";
                    container = name;
                    break;
                case SlideReelStory.Seal_1:
                    name = "Reel_1_Seal";
                    container = name;
                    break;
                case SlideReelStory.Seal_2:
                    name = "Reel_2_Seal";
                    container = name;
                    break;
                case SlideReelStory.Seal_3:
                    name = "Reel_3_Seal";
                    container = name;
                    break;
                case SlideReelStory.DreamRule_1:
                    name = "Reel_1_DreamRule";
                    container = name;
                    break;
                case SlideReelStory.DreamRule_2_v1:
                    name = "Reel_2_DreamRule_v1";
                    container = name;
                    break;
                case SlideReelStory.DreamRule_2_v2:
                    name = "Reel_2_DreamRule_v2";
                    container = name;
                    break;
                case SlideReelStory.DreamRule_3:
                    name = "Reel_3_DreamRule";
                    container = name;
                    break;
                case SlideReelStory.Burning:
                    name = "Reel_Burning";
                    container = name;
                    break;
                case SlideReelStory.Experiment:
                    name = ((!burned) ? "Reel_ExperimentWatch" : ("Reel_ExperimentWatch_" + burnedText));
                    container = name;
                    break;
                case SlideReelStory.DamageReport:
                    name = "Reel_DamageReport";
                    container = name;
                    break;
                case SlideReelStory.LanternSecret:
                    name = "Reel_LanternSecret";
                    container = name;
                    break;
                case SlideReelStory.Story_5_Complete:
                    name = "Reel_Destroyed_8";
                    container = "Reel_5_Story_Vision_Complete";
                    invertColors = true;
                    break;
                case SlideReelStory.Story_5_NoVessel:
                    name = "Reel_Destroyed_8";
                    container = "Reel_5_Story_Vision_NoVessel";
                    invertColors = true;
                    break;
                case SlideReelStory.Story_5_NoInterloper:
                    name = "Reel_Destroyed_8";
                    container = "Reel_5_Story_Vision_NoInterloper";
                    invertColors = true;
                    break;
                case SlideReelStory.Story_5_NoInterloperNoVessel:
                    name = "Reel_Destroyed_8";
                    container = "Reel_5_Story_Vision_NoInterloperNoVessel";
                    invertColors = true;
                    break;
                case SlideReelStory.PrisonPeephole_Vision:
                    name = "Reel_Destroyed_8";
                    container = "Reel_PrisonPeephole_Vision";
                    invertColors = true;
                    break;
                case SlideReelStory.PrisonerFarewellVision:
                    name = "Reel_Destroyed_8";
                    container = "Reel_PrisonerFarewellVision";
                    invertColors = true;
                    break;
                case SlideReelStory.TowerVision:
                    name = "Reel_Destroyed_8";
                    container = "Reel_TowerVision";
                    invertColors = true;
                    break;
                case SlideReelStory.SignalJammer:
                    name = "Reel_Destroyed_8";
                    container = "AutoProjector_SignalJammer";
                    break;
                case SlideReelStory.Homeworld:
                    name = "Reel_Destroyed_8";
                    container = "AutoProjector_Homeworld";
                    break;
                case SlideReelStory.SupernovaEscape:
                    name = "Reel_Destroyed_8";
                    container = "AutoProjector_SupernovaEscape";
                    break;
                default:
                    name = "Reel_Destroyed_8";
                    container = name;
                    break;
            }

            foreach (SlideReelItem reel in Resources.FindObjectsOfTypeAll<SlideReelItem>())
            {
                if (!reel.name.Contains(name))
                {
                    continue;
                }

                SlideReelItem newReel = Object.Instantiate(reel, Locator.GetAstroObject(AstroObject.Name.Sun)?.transform ?? Locator.GetAstroObject(AstroObject.Name.Eye)?.transform ?? Locator.GetPlayerBody()?.transform);
                SetItemVisible(newReel, visible: true);
                if (invertColors)
                {
                    inverted.Add(newReel.slidesContainer);
                }

                if (!name.Equals(container))
                {
                    newReel.slidesContainer.ClearSlides();
                    newReel.slidesContainer._changeSlidesAllowed = true;
                    HashSet<int> expandedIndex = new HashSet<int>();
                    foreach (SlideCollectionContainer slideCollectionContainer in Resources.FindObjectsOfTypeAll<SlideCollectionContainer>())
                    {
                        if (!slideCollectionContainer.name.Contains(container))
                        {
                            continue;
                        }

                        newReel.slidesContainer._shipLogOnComplete = slideCollectionContainer._shipLogOnComplete;
                        newReel.slidesContainer._autoLoadStreaming = slideCollectionContainer._autoLoadStreaming;
                        newReel.slidesContainer._invertBlackFrames = slideCollectionContainer._invertBlackFrames;
                        newReel.slidesContainer._musicRanges = slideCollectionContainer._musicRanges;
                        newReel.slidesContainer.slideCollection.streamingAssetIdentifier = slideCollectionContainer.slideCollection.streamingAssetIdentifier;
                        newReel.slidesContainer.slideCollection.SetAssetBundle(slideCollectionContainer.slideCollection.GetAssetBundle());
                        foreach (Slide slide in slideCollectionContainer.slideCollection.slides)
                        {
                            if (!expandedIndex.Contains(slide.GetStreamingIndex()))
                            {
                                expandedIndex.Add(slide.GetStreamingIndex());
                                Slide newSlide = new Slide(slide);
                                newReel.slidesContainer.AddSlide(newSlide);
                            }
                        }
                    }
                }

                return newReel;
            }

            return null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SlideCollectionContainer), nameof(SlideCollectionContainer.OnTextureLoaded))]
        private static bool InvertedSlideFix(SlideCollectionContainer __instance, ref int index, ref Texture texture)
        {
            if (inverted.Contains(__instance))
            {
                Slide[] slides = __instance.slideCollection.slides;
                foreach (Slide slide in slides)
                {
                    if (slide.GetStreamingIndex() == index)
                    {
                        slide._image = InvertColors(MakeReadable(texture)) as Texture2D;
                    }
                }

                if (index == __instance.GetCurrentSlide().GetStreamingIndex())
                {
                    __instance.GetCurrentSlide().InvokeTextureUpdate();
                }

                return false;
            }

            return true;
        }

        private static Texture MakeReadable(Texture texture)
        {
            if (!texture.isReadable)
            {
                RenderTexture renderTex = RenderTexture.GetTemporary(
                            texture.width,
                            texture.height,
                            0,
                            RenderTextureFormat.Default,
                            RenderTextureReadWrite.Linear);

                Graphics.Blit(texture, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(texture.width, texture.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);

                return readableText;
            }

            return texture;
        }

        private static Texture InvertColors(Texture texture)
        {
            Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
            var colors = (texture as Texture2D).GetPixels();
            var newColors = new Color[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                var color = colors[i];
                newColors[i] = new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
            }
            newTexture.SetPixels(newColors);
            newTexture.Apply();

            return newTexture;
        }

        [ConsoleData("give")]
        public static void Give(string item)
        {
            OWItem pickupItem;

            switch (item.ToLower())
            {
                case "warpcore":
                case "warpcorevessel":
                    pickupItem = CreateWarpCore(WarpCoreType.Vessel);
                    break;
                case "warpcorebroken":
                    pickupItem = CreateWarpCore(WarpCoreType.VesselBroken);
                    break;
                case "warpcoreblack":
                    pickupItem = CreateWarpCore(WarpCoreType.Black);
                    break;
                case "warpcorewhite":
                    pickupItem = CreateWarpCore(WarpCoreType.White);
                    break;
                case "warpcorenone":
                    pickupItem = CreateWarpCore(WarpCoreType.SimpleBroken);
                    break;
                case "lantern":
                case "lanternbasic":
                    pickupItem = CreateLantern(false, true);
                    break;
                case "lanternbroken":
                    pickupItem = CreateLantern(true, false);
                    break;
                case "artifactgen1":
                    pickupItem = CreateDreamLantern(DreamLanternType.Nonfunctioning, false);
                    break;
                case "artifactgen2":
                    pickupItem = CreateDreamLantern(DreamLanternType.Malfunctioning, true);
                    break;
                case "artifact":
                case "artifactgen3":
                    pickupItem = CreateDreamLantern(DreamLanternType.Functioning, false);
                    break;
                case "slide_story1":
                    pickupItem = CreateSlideReel(SlideReelStory.Story_1, false);
                    break;
                case "slide_story2":
                    pickupItem = CreateSlideReel(SlideReelStory.Story_2, false);
                    break;
                case "slide_story3":
                    pickupItem = CreateSlideReel(SlideReelStory.Story_3, false);
                    break;
                case "slide_story4":
                    pickupItem = CreateSlideReel(SlideReelStory.Story_4, false);
                    break;
                case "slide_story5":
                    pickupItem = CreateSlideReel(SlideReelStory.Story_5_Complete, false);
                    break;
                case "slide_path1":
                    pickupItem = CreateSlideReel(SlideReelStory.LibraryPath_1, false);
                    break;
                case "slide_path2":
                    pickupItem = CreateSlideReel(SlideReelStory.LibraryPath_2, false);
                    break;
                case "slide_path3":
                    pickupItem = CreateSlideReel(SlideReelStory.LibraryPath_3, false);
                    break;
                case "slide_seal1":
                    pickupItem = CreateSlideReel(SlideReelStory.Seal_1, false);
                    break;
                case "slide_seal2":
                    pickupItem = CreateSlideReel(SlideReelStory.Seal_2, false);
                    break;
                case "slide_seal3":
                    pickupItem = CreateSlideReel(SlideReelStory.Seal_3, false);
                    break;
                case "slide_rule1":
                    pickupItem = CreateSlideReel(SlideReelStory.DreamRule_1, false);
                    break;
                case "slide_rule2":
                    pickupItem = CreateSlideReel(SlideReelStory.DreamRule_2_v1, false);
                    break;
                case "slide_rule3":
                    pickupItem = CreateSlideReel(SlideReelStory.DreamRule_2_v2, false);
                    break;
                case "slide_rule4":
                    pickupItem = CreateSlideReel(SlideReelStory.DreamRule_3, false);
                    break;
                case "slide_burning":
                    pickupItem = CreateSlideReel(SlideReelStory.Burning, false);
                    break;
                case "slide_experiment":
                    pickupItem = CreateSlideReel(SlideReelStory.Experiment, false);
                    break;
                case "slide_damagereport":
                    pickupItem = CreateSlideReel(SlideReelStory.DamageReport, false);
                    break;
                case "slide_lanternsecret":
                    pickupItem = CreateSlideReel(SlideReelStory.LanternSecret, false);
                    break;
                case "slide_prisoner":
                    pickupItem = CreateSlideReel(SlideReelStory.PrisonPeephole_Vision, false);
                    break;
                case "slide_prisonerfarewell":
                    pickupItem = CreateSlideReel(SlideReelStory.PrisonerFarewellVision, false);
                    break;
                case "slide_tower":
                    pickupItem = CreateSlideReel(SlideReelStory.TowerVision, false);
                    break;
                case "slide_signaljammer":
                    pickupItem = CreateSlideReel(SlideReelStory.SignalJammer, false);
                    break;
                case "slide_homeworld":
                    pickupItem = CreateSlideReel(SlideReelStory.Homeworld, false);
                    break;
                case "slide_supernovaescape":
                    pickupItem = CreateSlideReel(SlideReelStory.SupernovaEscape, false);
                    break;
                default:
                    Log($"Invalid item!", ConsoleLogType.Error);
                    return;
            }

            ItemTool itemTool = Object.FindObjectOfType<ItemTool>();
            if (itemTool != null && pickupItem != null && pickupItem.gameObject != null && itemTool.GetHeldItem() == null)
            {
                itemTool.PickUpItemInstantly(pickupItem);
            }

            Log($"Gave {item} to player.");
        }
    }
}
