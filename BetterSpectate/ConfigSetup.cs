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
			ConfigEntry<bool> isZoomEnabled = ConfigSetup.config.Bind<bool>("Zoom Settings", "Third Person Zoom Enabled", true, "Allows spectators to zoom in and out.");
			ConfigEntry<float> maxZoomDistance = ConfigSetup.config.Bind<float>("Zoom Settings", "Max Zoom Distance", 15f, "Furthest distance a spectator can zoom out.");
			ConfigEntry<float> minZoomDistance = ConfigSetup.config.Bind<float>("Zoom Settings", "Min Zoom Distance", 1f, "Closest distance a spectator can zoom in.");
			ConfigEntry<float> defaultZoomDistance = ConfigSetup.config.Bind<float>("Zoom Settings", "Default Zoom Distance", 1.4f, "Distance a spectator is set upon death.");
			ConfigEntry<float> zoomSpeed = ConfigSetup.config.Bind<float>("Zoom Settings", "Zoom Speed", 0.4f, "Speed that scrolling will zoom in or out.");
			ConfigEntry<bool> isFirstPersonEnabled = ConfigSetup.config.Bind<bool>("General Settings", "First Person Spectate Enabled", true, "Allows spectators to enter a first person view.");
			ConfigEntry<bool> isFirstPersonDefault = ConfigSetup.config.Bind<bool>("General Settings", "Default to First Person", true, "Defaults players to a first person view on death.");
			ConfigEntry<string> firstPersonKeybind = ConfigSetup.config.Bind<string>("General Settings", "First Person Keybind", "P", "Sets the keybind to switch between perspectives.");
			PlayerControllerB_Patch.SetFirstPersonEnabled(isFirstPersonEnabled.Value);
			PlayerControllerB_Patch.SetFirstPersonToggle(isFirstPersonDefault.Value);
			PlayerControllerB_Patch.InitializeFirstPersonSpectateInputAction("<Keyboard>/" + firstPersonKeybind.Value);
			PlayerControllerB_Patch.SetZoomEnabled(isZoomEnabled.Value);
			PlayerControllerB_Patch.SetZoomDistance(defaultZoomDistance.Value);
			PlayerControllerB_Patch.SetDefaultZoomDistance(defaultZoomDistance.Value);
			PlayerControllerB_Patch.SetZoomSpeed(zoomSpeed.Value);
			SpectateUtils.SetMaxZoom(maxZoomDistance.Value);
			SpectateUtils.SetMinZoom(minZoomDistance.Value);
		}

		private const string CONFIG_FILE_NAME = "Fusition.BetterSpectate";

		private static ConfigFile config;
	}
}
