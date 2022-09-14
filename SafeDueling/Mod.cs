
using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;


namespace SafeDueling
{
    [BepInPlugin("knorssman.SafeDueling", "SafeDueling", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class Mod : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("Knorssman.SafeDueling");
        private static ManualLogSource logger;

        void Awake()
        {
            harmony.PatchAll();
            logger = Logger;
        }

        [HarmonyPatch(typeof(Skills), nameof(Skills.LowerAllSkills))]
        private static class LowerAllSkillsPatch
        {
            private static bool Prefix(ref Player ___m_player)
            {
                if (((Object)(object)Player.m_localPlayer != (Object)null) && ((Object)(object)Player.m_localPlayer == (Object)(object)___m_player) 
                    && ___m_player.IsPVPEnabled())
                {
                    logger.LogDebug("skip LowerAllSkills");
                    ___m_player.ClearHardDeath();
                    return false;
                    
                }
                return true;
            }
        }
    }
}
