using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace RogueTowerResearch
{
    /**
     * Houses can only research tower upgrades
     * Houses research upgrades related to the adjacent towers
     * Adjacent towers currently being researched have their range halved
     * Each tech costs 45 research points
     * Each house generates research points equal the the sum of the levels of the adjacent towers related to that tech
     * Houses don't generate money while researching
     * Research can be cancelled, but all research points are lost
     * Technologies currently being researched are taken out of the upgrade pool
     */
    [BepInPlugin("daloupe.roguetower.roguetowerresearch", "RogueTowerResearch", "0.0.3")]
    public class ResearchPlugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        public static ResearchManager ResearchManager;

        private void Awake()
        {
            Log = Logger;
            var harmony = new Harmony("daloupe.roguetower.roguetowerresearch");
            harmony.PatchAll();
        }
    }
}
