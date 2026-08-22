using BepInEx.Bootstrap;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;

namespace BetterSpectate.Compatibility
{
    public static class InputUtilsCompat
    {
        public static bool enabled
        {
            get
            {
                bool flag = InputUtilsCompat._enabled == null;
                if (flag)
                {
                    InputUtilsCompat._enabled = new bool?(Chainloader.PluginInfos.ContainsKey(InputUtilsCompat.InputUtilsGuid));
                }
                return InputUtilsCompat._enabled.Value;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static InputAction TryGetTogglePerspectiveAction(string binding)
        {
            bool flag = !InputUtilsCompat.enabled;
            if (flag)
            {
                return null;
            }
            try
            {
                BetterSpectateInputs inputs = new BetterSpectateInputs();
                InputAction action = inputs.TogglePerspective;
                bool flag2 = action != null && action.bindings.Count > 0 && string.IsNullOrEmpty(action.bindings[0].overridePath);
                if (flag2)
                {
                    action.ChangeBinding(0).WithPath(binding);
                }
                return action;
            }
            catch (Exception e)
            {
                BetterSpectateBase.fusLogSource?.LogWarning("Failed to setup LethalCompanyInputUtils keybind, falling back to config keybind: " + e.Message);
                return null;
            }
        }

        private const string InputUtilsGuid = "com.rune580.LethalCompanyInputUtils";

        private static bool? _enabled;
    }
}
