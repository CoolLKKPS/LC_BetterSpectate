using BepInEx;
using BetterSpectate.Compatibility;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BetterSpectate.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class PlayerControllerB_Patch
    {
        [HarmonyPatch(typeof(PlayerControllerB), "LateUpdate")]
        [HarmonyPrefix]
        public static void LateUpdate_Patch(PlayerControllerB __instance)
        {
            bool flag = __instance == GameNetworkManager.Instance.localPlayerController && __instance.spectatedPlayerScript != null;
            if (flag)
            {
                bool isHoldingObject = __instance.spectatedPlayerScript.isHoldingObject;
                if (isHoldingObject)
                {
                    __instance.spectatedPlayerScript.currentlyHeldObjectServer.parentObject = PlayerControllerB_Patch.firstPersonSpectateToggle ? __instance.spectatedPlayerScript.localItemHolder.transform : __instance.spectatedPlayerScript.serverItemHolder.transform;
                }
                bool flag2 = UnityInput.Current.mouseScrollDelta.y != 0f && PlayerControllerB_Patch.isZoomEnabled;
                if (flag2)
                {
                    __instance.thisPlayerModel.enabled = false;
                    PlayerControllerB_Patch.SetZoomDistance((PlayerControllerB_Patch.zoomDistance + (UnityInput.Current.mouseScrollDelta.y / -120f * PlayerControllerB_Patch.zoomSpeed)).ZoomClamp());
                }
                bool enabled = SpectateEnemyCompat.enabled;
                if (enabled)
                {
                    SpectateEnemyCompat.CheckIfSpectatingEnemies();
                }
                bool flag3 = !PlayerControllerB_Patch.inputDisabledForCompat && PlayerControllerB_Patch.firstPersonSpectateAction.WasPressedThisFrame() && PlayerControllerB_Patch.isFirstPersonEnabled;
                if (flag3)
                {
                    PlayerControllerB_Patch.SwitchPerspective(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "RaycastSpectateCameraAroundPivot")]
        [HarmonyPrefix]
        public static bool RaycastSpectateCameraAroundPivot_Patch(PlayerControllerB __instance, RaycastHit ___hit, int ___walkableSurfacesNoPlayersMask)
        {
            bool flag = __instance.spectatedPlayerScript != null;
            if (flag)
            {
                Transform transform = __instance.spectatedPlayerScript.visorCamera.transform;
                bool flag2 = PlayerControllerB_Patch.isFirstPersonEnabled && PlayerControllerB_Patch.firstPersonSpectateToggle;
                if (flag2)
                {
                    __instance.playersManager.spectateCamera.transform.position = transform.position;
                    __instance.playersManager.spectateCamera.transform.rotation = transform.rotation;
                    PlayerControllerB_Patch.isInFirstPerson = true;
                    return false;
                }
                else
                {
                    PlayerControllerB_Patch.isInFirstPerson = false;
                    if (PlayerControllerB_Patch.isZoomEnabled)
                    {
                        PlayerControllerB_Patch.RaycastCameraToZoomDistance(__instance, ___hit, ___walkableSurfacesNoPlayersMask);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "SpectateNextPlayer")]
        [HarmonyPrefix]
        public static bool SpectateNextPlayer_Patch(PlayerControllerB __instance, RaycastHit ___hit, int ___walkableSurfacesNoPlayersMask)
        {
            if (GameNetworkManager.Instance == null) return true;
            int num = 0;
            bool flag = __instance.spectatedPlayerScript != null;
            if (flag)
            {
                num = (int)__instance.spectatedPlayerScript.playerClientId;
                bool flag2 = GameNetworkManager.Instance.localPlayerController != null && __instance == GameNetworkManager.Instance.localPlayerController;
                if (flag2)
                {
                    PlayerControllerB_Patch.SetModelVisibilityForThirdPerson(__instance.spectatedPlayerScript);
                }
            }
            if (__instance.playersManager == null || __instance.playersManager.allPlayerScripts == null) return true;
            for (int i = 0; i < __instance.playersManager.allPlayerScripts.Length; i++)
            {
                num = (num + 1) % __instance.playersManager.allPlayerScripts.Length;
                var playerScript = __instance.playersManager.allPlayerScripts[num];
                if (playerScript == null) continue;
                bool flag3 = !playerScript.isPlayerDead && playerScript.isPlayerControlled && playerScript != __instance;
                if (flag3)
                {
                    __instance.spectatedPlayerScript = playerScript;
                    bool flag4 = GameNetworkManager.Instance.localPlayerController != null && __instance == GameNetworkManager.Instance.localPlayerController;
                    if (flag4)
                    {
                        bool flag5 = !PlayerControllerB_Patch.firstPersonSpectateToggle;
                        if (flag5)
                        {
                            BetterSpectateBase.fusLogSource.LogInfo("Model visibility adjusted for dead player in third person");
                            PlayerControllerB_Patch.SetModelVisibilityForThirdPerson(__instance.spectatedPlayerScript);
                        }
                        else
                        {
                            BetterSpectateBase.fusLogSource.LogInfo("Model visibility adjusted for dead player in first person");
                            PlayerControllerB_Patch.SetModelVisibilityForFirstPerson(__instance.spectatedPlayerScript);
                        }
                    }
                    try
                    {
                        __instance.SetSpectatedPlayerEffects(false);
                    }
                    catch (NullReferenceException)
                    {
                        BetterSpectateBase.fusLogSource?.LogWarning("NullReferenceException in SetSpectatedPlayerEffects caught and suppressed");
                    }
                    return false;
                }
            }
            bool flag6 = __instance.deadBody != null && __instance.deadBody.gameObject.activeSelf;
            if (flag6)
            {
                __instance.spectateCameraPivot.position = __instance.deadBody.bodyParts[0].position;
                PlayerControllerB_Patch.RaycastSpectateCameraAroundPivot_Patch(__instance, ___hit, ___walkableSurfacesNoPlayersMask);
            }
            if (StartOfRound.Instance != null) StartOfRound.Instance.SetPlayerSafeInShip();
            return false;
        }

        public static void InitializeFirstPersonSpectateInputAction(string binding)
        {
            PlayerControllerB_Patch.firstPersonSpectateAction = new InputAction("FirstPersonSpectatePressed", InputActionType.Value, binding, null, null, null);
            PlayerControllerB_Patch.firstPersonSpectateAction.Enable();
        }

        public static bool IsPlayerInFirstPerson()
        {
            return PlayerControllerB_Patch.isInFirstPerson;
        }

        public static bool GetZoomEnabled()
        {
            return PlayerControllerB_Patch.isZoomEnabled;
        }

        public static void SetZoomEnabled(bool enabled)
        {
            PlayerControllerB_Patch.isZoomEnabled = enabled;
        }

        public static bool GetFirstPersonEnabled()
        {
            return PlayerControllerB_Patch.isFirstPersonEnabled;
        }

        public static void SetFirstPersonEnabled(bool enabled)
        {
            PlayerControllerB_Patch.isFirstPersonEnabled = enabled;
        }

        public static void SwitchPerspective(PlayerControllerB controller)
        {
            if (!PlayerControllerB_Patch.isFirstPersonEnabled)
            {
                return;
            }
            PlayerControllerB_Patch.firstPersonSpectateToggle = !PlayerControllerB_Patch.firstPersonSpectateToggle;
            bool flag = !PlayerControllerB_Patch.firstPersonSpectateToggle;
            if (flag)
            {
                BetterSpectateBase.fusLogSource.LogInfo("Player Toggled to Third Person");
                PlayerControllerB_Patch.SetModelVisibilityForThirdPerson(controller.spectatedPlayerScript);
                controller.spectateCameraPivot.transform.rotation = controller.spectatedPlayerScript.visorCamera.transform.rotation;
                PlayerControllerB_Patch.zoomDistance = PlayerControllerB_Patch.defaultZoomDistance;
            }
            else
            {
                BetterSpectateBase.fusLogSource.LogInfo("Player Toggled to First Person");
                PlayerControllerB_Patch.SetModelVisibilityForFirstPerson(controller.spectatedPlayerScript);
            }
        }

        public static void SetFirstPersonToggle(bool value)
        {
            if (PlayerControllerB_Patch.isFirstPersonEnabled)
            {
                PlayerControllerB_Patch.firstPersonSpectateToggle = value;
            }
            else
            {
                PlayerControllerB_Patch.firstPersonSpectateToggle = false;
            }
        }

        public static void SetModelVisibilityForFirstPerson(PlayerControllerB controller)
        {
            if (controller == null) return;
            if (controller.thisPlayerModelArms != null) controller.thisPlayerModelArms.enabled = true;
            if (controller.thisPlayerModel != null) controller.thisPlayerModel.enabled = false;
            if (controller.thisPlayerModelLOD1 != null) controller.thisPlayerModelLOD1.enabled = false;
            if (controller.thisPlayerModelLOD2 != null) controller.thisPlayerModelLOD2.enabled = false;
        }

        public static void SetModelVisibilityForThirdPerson(PlayerControllerB controller)
        {
            if (controller == null) return;
            if (controller.thisPlayerModelArms != null) controller.thisPlayerModelArms.enabled = false;
            if (controller.thisPlayerModel != null) controller.thisPlayerModel.enabled = true;
            if (controller.thisPlayerModelLOD1 != null) controller.thisPlayerModelLOD1.enabled = true;
            if (controller.thisPlayerModelLOD2 != null) controller.thisPlayerModelLOD2.enabled = true;
        }

        public static float GetZoomDistance()
        {
            return PlayerControllerB_Patch.zoomDistance;
        }

        public static void SetZoomDistance(float value)
        {
            PlayerControllerB_Patch.zoomDistance = value;
        }

        public static void SetDefaultZoomDistance(float value)
        {
            PlayerControllerB_Patch.defaultZoomDistance = value;
        }

        public static float GetZoomSpeed()
        {
            return PlayerControllerB_Patch.zoomSpeed;
        }

        public static void SetZoomSpeed(float value)
        {
            PlayerControllerB_Patch.zoomSpeed = value;
        }

        public static void SetInputDisabled(bool value)
        {
            PlayerControllerB_Patch.inputDisabledForCompat = value;
        }

        private static void RaycastCameraToZoomDistance(PlayerControllerB controller, RaycastHit hit, int walkableSurfacesNoPlayersMask)
        {
            Ray ray = new Ray(controller.spectateCameraPivot.position, -controller.spectateCameraPivot.forward);
            bool flag = Physics.Raycast(ray, out hit, PlayerControllerB_Patch.zoomDistance, walkableSurfacesNoPlayersMask, QueryTriggerInteraction.Ignore);
            if (flag)
            {
                controller.playersManager.spectateCamera.transform.position = ray.GetPoint(hit.distance - 0.25f);
            }
            else
            {
                controller.playersManager.spectateCamera.transform.position = ray.GetPoint(PlayerControllerB_Patch.zoomDistance - 0.1f);
            }
            controller.playersManager.spectateCamera.transform.LookAt(controller.spectateCameraPivot);
        }

        private static bool isZoomEnabled;

        private static float zoomDistance = 1.4f;

        private static float zoomSpeed = 0.4f;

        private static float defaultZoomDistance;

        private static bool isFirstPersonEnabled;

        private static InputAction firstPersonSpectateAction;

        private static bool firstPersonSpectateToggle = false;

        private static bool isInFirstPerson = false;

        private static bool inputDisabledForCompat = false;
    }
}
