using BepInEx;
using BepInEx.Logging;
using BetterSpectate.Compatibility;
using BetterSpectate.Patches;
using HarmonyLib;
using UnityEngine;

namespace BetterSpectate
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERS)]
    [BepInDependency("SpectateEnemy", BepInDependency.DependencyFlags.SoftDependency)]
    public class BetterSpectateBase : BaseUnityPlugin
    {
        private void Awake()
        {
            bool flag = BetterSpectateBase.instance == null;
            if (flag)
            {
                BetterSpectateBase.instance = this;
            }
            base.gameObject.hideFlags = HideFlags.HideAndDontSave;
            BetterSpectateBase.fusLogSource = global::BepInEx.Logging.Logger.CreateLogSource("Fusition.BetterSpectate");
            BetterSpectateBase.fusLogSource.LogInfo(SpectateEnemyCompat.enabled);
            ConfigSetup.Initialize();
            this.harmony.PatchAll(typeof(PlayerControllerB_Patch));
            this.harmony.PatchAll(typeof(StartOfRound_Patch));
        }

        public const string MOD_GUID = "Fusition.BetterSpectate";

        internal const string MOD_NAME = "BetterSpectate";

        internal const string MOD_VERS = "1.2.1.0";

        private readonly Harmony harmony = new Harmony("Fusition.BetterSpectate");

        public static BetterSpectateBase instance;

        internal static ManualLogSource fusLogSource;
    }
}
