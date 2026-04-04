using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BetterSpectate.Patches;

namespace BetterSpectate
{
	public class ConfigSetup
	{
		public static void Initialize()
		{
			string text = Path.Combine(Paths.ConfigPath, "Fusition.BetterSpectate.cfg");
			ConfigSetup.config = new ConfigFile(text, true);
			ConfigEntry<bool> configEntry = ConfigSetup.config.Bind<bool>("Zoom Settings", "Third Person Zoom Enabled", true, "Allows spectators to zoom in and out.");
			ConfigEntry<float> configEntry2 = ConfigSetup.config.Bind<float>("Zoom Settings", "Max Zoom Distance", 15f, "Furthest distance a spectator can zoom out.");
			ConfigEntry<float> configEntry3 = ConfigSetup.config.Bind<float>("Zoom Settings", "Min Zoom Distance", 1f, "Closest distance a spectator can zoom in.");
			ConfigEntry<float> configEntry4 = ConfigSetup.config.Bind<float>("Zoom Settings", "Default Zoom Distance", 1.4f, "Distance a spectator is set upon death.");
			ConfigEntry<float> configEntry5 = ConfigSetup.config.Bind<float>("Zoom Settings", "Zoom Speed", 0.4f, "Speed that scrolling will zoom in or out.");
			ConfigEntry<bool> configEntry6 = ConfigSetup.config.Bind<bool>("General Settings", "First Person Spectate Enabled", true, "Allows spectators to enter a first person view.");
			ConfigEntry<bool> configEntry7 = ConfigSetup.config.Bind<bool>("General Settings", "Default to First Person", true, "Defaults players to a first person view on death.");
			ConfigEntry<string> configEntry8 = ConfigSetup.config.Bind<string>("General Settings", "First Person Keybind", "P", "Sets the keybind to switch between perspectives.");
			PlayerControllerB_Patch.SetFirstPersonEnabled(configEntry6.Value);
			PlayerControllerB_Patch.SetFirstPersonToggle(configEntry7.Value);
			PlayerControllerB_Patch.InitializeFirstPersonSpectateInputAction("<Keyboard>/" + configEntry8.Value);
			PlayerControllerB_Patch.SetZoomEnabled(configEntry.Value);
			PlayerControllerB_Patch.SetZoomDistance(configEntry4.Value);
			PlayerControllerB_Patch.SetDefaultZoomDistance(configEntry4.Value);
			PlayerControllerB_Patch.SetZoomSpeed(configEntry5.Value);
			SpectateUtils.SetMaxZoom(configEntry2.Value);
			SpectateUtils.SetMinZoom(configEntry3.Value);
		}

		private const string CONFIG_FILE_NAME = "Fusition.BetterSpectate";

		private static ConfigFile config;

		private ConfigEntry<bool> isZoomEnabled;

		private ConfigEntry<float> maxZoomDistance;

		private ConfigEntry<float> minZoomDistance;

		private ConfigEntry<float> defaultZoomDistance;

		private ConfigEntry<float> zoomSpeed;

		private ConfigEntry<bool> isFirstPersonEnabled;

		private ConfigEntry<bool> isFirstPersonDefault;

		private ConfigEntry<string> firstPersonKeybind;
	}
}
