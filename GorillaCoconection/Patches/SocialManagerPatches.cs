using GorillaCoconection.Behaviours;
using HarmonyLib;
using UnityEngine;

namespace GorillaCoconection.Patches
{
    [HarmonyPatch(typeof(LckSocialCameraManager))]
    internal static class SocialManagerPatches
    {
        [HarmonyPatch(nameof(LckSocialCameraManager.OnEnable))]
        [HarmonyPostfix]
        [HarmonyWrapSafe]
        public static void ApplyPatch(LckSocialCameraManager __instance)
        {
            Plugin.Logger.LogMessage($"LckSocialCamera apply patch: {__instance.CoconutCamera.gameObject.name}");

            if (__instance.CoconutCamera.GetComponent<CustomCoconutCamera>())
            {
                Plugin.Logger.LogWarning($"Coconut already still has changes");
                return;
            }

            var component = __instance.CoconutCamera.gameObject.AddComponent<CustomCoconutCamera>();
            component.IsLocalCamera = true;
        }

        [HarmonyPatch(nameof(LckSocialCameraManager.OnDisable))]
        [HarmonyPostfix]
        public static void RemovePatch(LckSocialCameraManager __instance)
        {
            Plugin.Logger.LogMessage($"LckSocialCamera remove patch: {__instance.CoconutCamera.gameObject.name}");

            if (__instance.CoconutCamera.TryGetComponent(out CustomCoconutCamera component)) Object.Destroy(component);
        }
    }
}
