using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;

namespace BetterSpectate.Compatibility
{
    internal class BetterSpectateInputs : LcInputActions
    {
        [InputAction("<Keyboard>/p", Name = "Toggle First/Third Person Spectating", ActionId = "toggle_perspective")]
        public InputAction TogglePerspective { get; set; }
    }
}
