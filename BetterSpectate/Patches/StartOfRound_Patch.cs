using GameNetcodeStuff;
using HarmonyLib;

namespace BetterSpectate.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRound_Patch
    {
        [HarmonyPatch(typeof(StartOfRound), "ReviveDeadPlayers")]
        [HarmonyPostfix]
        public static void ReviveDeadPlayers_Patch(PlayerControllerB[] ___allPlayerScripts)
        {
            for (int i = 0; i < ___allPlayerScripts.Length; i++)
            {
                bool flag = ___allPlayerScripts[i] != GameNetworkManager.Instance.localPlayerController;
                if (flag)
                {
                    ___allPlayerScripts[i].thisPlayerModelArms.enabled = false;
                    ___allPlayerScripts[i].thisPlayerModel.enabled = true;
                    ___allPlayerScripts[i].thisPlayerModelLOD1.enabled = true;
                    ___allPlayerScripts[i].thisPlayerModelLOD2.enabled = true;
                    bool isHoldingObject = ___allPlayerScripts[i].isHoldingObject;
                    if (isHoldingObject)
                    {
                        ___allPlayerScripts[i].currentlyHeldObjectServer.parentObject = ___allPlayerScripts[i].serverItemHolder.transform;
                    }
                }
            }
        }
    }
}
