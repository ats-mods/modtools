using BepInEx;
using BepInEx.Configuration;
using Eremite.Controller;
using UnityEngine;
using HarmonyLib;
using Eremite.Model.Configs;
using Eremite.Services;
using Eremite.Tools.Runtime;

namespace ModdingTools
{
    [BepInPlugin("AtsModdingTools", "AtsModdingTools", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static Plugin Instance;
        private KeyboardShortcut dumpKeyBind;
        private KeyboardShortcut ffKeyBind;
        public static void Log(object obj) => Instance.Logger.LogInfo(obj);

        private Harmony harmony;
        private void Awake()
        {
            Instance = this;
            Logger.LogInfo("modding tools is loaded!");
            harmony = Harmony.CreateAndPatchAll(typeof(Plugin));
            dumpKeyBind = new(KeyCode.F1, KeyCode.LeftControl, KeyCode.LeftAlt);
            ffKeyBind = new(KeyCode.N, KeyCode.LeftControl);
        }

        private void Update(){
            if (dumpKeyBind.IsDown()) PrintInternalGameData();
            if (ffKeyBind.IsDown()) TimeRuntimeTools.SetNextSeason();
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

        private void PrintInternalGameData()
        {   
            Instance.Logger.LogInfo("=== PRINTING ALL PERKS ===");
            foreach (var effect in Serviceable.Settings.effects){
                Instance.Logger.LogInfo($"{effect.Name} || (\"{effect.DisplayName}\"): uses icon name {effect.GetIcon()?.name}");
            }
            Log("=== PRINTING SIMPLE SEASONAL EFFECTS ===");
            foreach (var effect in Serviceable.Settings.simpleSeasonalEffects){
                Log($"{effect.Name} || (\"{effect.DisplayName}\"): uses icon name {effect.Icon.name}");
            }
            Log("=== PRINTING CONDITIONAL SEASONAL EFFECTS ===");
            foreach (var effect in Serviceable.Settings.conditionalSeasonalEffects){
                Log($"{effect.Name} || (\"{effect.DisplayName}\"): uses icon name {effect.Icon.name}");
            }
        }
    }
}