using BepInEx.Bootstrap;
using BetterSpectate.Patches;
using SpectateEnemy;
using System.Runtime.CompilerServices;

namespace BetterSpectate.Compatibility
{
    public static class SpectateEnemyCompat
    {
        public static bool enabled
        {
            get
            {
                bool flag = SpectateEnemyCompat._enabled == null;
                if (flag)
                {
                    SpectateEnemyCompat._enabled = new bool?(Chainloader.PluginInfos.ContainsKey("SpectateEnemy"));
                }
                return SpectateEnemyCompat._enabled.Value;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void CheckIfSpectatingEnemies()
        {
            bool isSpectating = SpectateEnemiesAPI.IsLoaded && SpectateEnemiesAPI.IsSpectatingEnemies;
            if (isSpectating)
            {
                if (!_wasSpectating)
                {
                    PlayerControllerB_Patch.SetFirstPersonToggle(false);
                    PlayerControllerB_Patch.SetInputDisabled(true);
                }
                _wasSpectating = true;
            }
            else
            {
                if (_wasSpectating)
                {
                    PlayerControllerB_Patch.SetInputDisabled(false);
                }
                _wasSpectating = false;
            }
        }

        private static bool? _enabled;
        private static bool _wasSpectating = false;
    }
}
