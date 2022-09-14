
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Knorssman
{
    [BepInPlugin("knorssman.BetterCart","BetterCart", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class Mod : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("Knorssman.BetterCart");

		private static ManualLogSource logger;

		private ConfigEntry<float> minimumMassConfig;

		private ConfigEntry<float> maximumMassConfig;

        void Awake()
        {
            harmony.PatchAll();
			logger = Logger;
			minimumMassConfig = ((BaseUnityPlugin)this).Config.Bind<float>("BetterCart", "minimumMass", SetMass_Patch.minimumMass, "Cart is flying around and flipping too much? help it stick to the ground.");
			maximumMassConfig = ((BaseUnityPlugin)this).Config.Bind<float>("BetterCart", "maximumMass", SetMass_Patch.maximumMass, "Customize what the maximum mass of the cart while loaded can be.");

			SetMass_Patch.minimumMass = minimumMassConfig.Value;
			SetMass_Patch.maximumMass = maximumMassConfig.Value;
			if(SetMass_Patch.minimumMass > SetMass_Patch.maximumMass)
            {
				logger.LogWarning("Minimum Mass is greater than maximum mass. minimum mass will take priority");
            }
			logger.LogInfo($"Using Min mass {SetMass_Patch.minimumMass}");
			logger.LogInfo($"Using Max mass {SetMass_Patch.maximumMass}");
		}

		[HarmonyPatch(typeof(Vagon), "SetMass")]
		private static class SetMass_Patch
		{
			public static float minimumMass = 20f;
			public static float maximumMass = 50f;
			private static void Prefix(Vagon __instance, ZNetView ___m_nview, ref float mass)
			{
				if (___m_nview.IsOwner())
				{
					mass = Mathf.Max(minimumMass, Mathf.Min(maximumMass, mass));
					logger.LogDebug($"final mass {mass}");
				}
			}
		}
	}
}
