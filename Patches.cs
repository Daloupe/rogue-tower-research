using HarmonyLib;
using UnityEngine;

namespace RogueTowerResearch
{
    [HarmonyPatch(typeof(House))]
    public class HousePatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(House.Start))]
        private static void Start(House __instance)
        {
            ResearchPlugin.ResearchManager.AddResearchGenerator(__instance.GetInstanceID(), __instance.gameObject.AddComponent<ResearchGenerator>());
        }

        [HarmonyPostfix, HarmonyPatch(nameof(House.CheckTowers))]
        private static void CheckTowers(House __instance)
        {
            ResearchPlugin.ResearchManager.CheckTowers(__instance.GetInstanceID());
        }

        [HarmonyPostfix, HarmonyPatch(nameof(House.SpawnUI))]
        private static void SpawnUI(House __instance)
        {
            ResearchPlugin.ResearchManager.SpawnUI(__instance.GetInstanceID());
        }
    }

    [HarmonyPatch(typeof(SpawnManager))]
    public class SpawnManagerPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(SpawnManager.Awake))]
        private static void Awake()
        {
            ResearchPlugin.ResearchManager = new GameObject("ResearchManager").AddComponent<ResearchManager>();
        }

        [HarmonyPostfix, HarmonyPatch(nameof(SpawnManager.StartNextWave))]
        private static void StartNextWave()
        {
            ResearchPlugin.ResearchManager.UpdateResearch();
        }
    }
}
