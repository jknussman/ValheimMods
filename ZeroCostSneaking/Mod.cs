
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace ZeroCostSneaking
{
    [BepInPlugin("knorssman.ZeroCostSneaking", "ZeroCostSneaking", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class Mod : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("Knorssman.ZeroCostSneaking");

        void Awake()
        {
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Player), "OnSneaking")]
        private static class OnSneaking_Patch
        {
            private static void Prefix(Player __instance, Skills ___m_skills)
            {
                if (!((Object)(object)Player.m_localPlayer == (Object)null) && !((Object)(object)Player.m_localPlayer != (Object)(object)__instance))
                {
                    __instance.m_sneakStaminaDrain = 0f;
                    //Debug.Log("successfully set sneak stamina drain from knorssman.ZeroCostSneaking.OnSneaking_Prefix");
                }
            }
        }
    }
}
