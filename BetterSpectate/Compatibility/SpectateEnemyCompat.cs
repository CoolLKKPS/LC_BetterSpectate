using System;
using System.Runtime.CompilerServices;
using BepInEx.Bootstrap;
using BetterSpectate.Patches;
using SpectateEnemy;

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
			bool flag = SpectateEnemiesAPI.IsLoaded && SpectateEnemiesAPI.IsSpectatingEnemies;
			if (flag)
			{
				PlayerControllerB_Patch.SetFirstPersonToggle(false);
				PlayerControllerB_Patch.SetInputDisabled(true);
			}
		}

		private static bool? _enabled;
	}
}
