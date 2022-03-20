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

    [HarmonyPatch(typeof(TileManager))]
    public class TileManagerPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(TileManager.Awake))]
        private static void Awake(TileManager __instance)
        {
            ResearchPlugin.ResearchManager = new GameObject("ResearchManager").AddComponent<ResearchManager>();
        }

        [HarmonyPrefix, HarmonyPatch(nameof(TileManager.SpawnNewTile))]
        private static void SpawnNewTile(TileManager __instance, int posX, int posY, int eulerAngle)
        {
            ResearchPlugin.ResearchManager.UpdateResearch();
        }
    }
}
