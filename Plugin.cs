using BepInEx;
using BepInEx.Configuration;
using Eremite.Controller;
using Eremite.Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HarmonyLib;
using UnityEngine.InputSystem;
using Eremite.Model.Configs;

namespace BubbleStormTweaks
{
    [BepInPlugin("AtsModdingTools", "AtsModdingTools", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony harmony;
        private void Awake()
        {
            harmony = Harmony.CreateAndPatchAll(typeof(Plugin));
        }

        [HarmonyPatch(typeof(MainController), nameof(MainController.InitBuildData))]
        [HarmonyPostfix]
        private static void ForceExperimentalBuild(ref MainController __instance){
            var build = __instance.Build;
            build.type = BuildType.InternalDevelopment;
            build.isExperimental = true;
            build.skipIntro = true;
            build.skipTutorial = true;
        }
    }
}