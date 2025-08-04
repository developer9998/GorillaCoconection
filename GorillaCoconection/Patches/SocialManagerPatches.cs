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
        public static void EnablePatch(LckSocialCameraManager __instance)
        {
            if (__instance.CoconutCamera.GetComponent<CustomCoconutCamera>()) return;
            var component = __instance.CoconutCamera.gameObject.AddComponent<CustomCoconutCamera>();
            component.IsLocalCamera = true;
        }

        [HarmonyPatch(nameof(LckSocialCameraManager.OnDisable))]
        [HarmonyPostfix]
        public static void DisablePatch(LckSocialCameraManager __instance)
        {
            if (__instance.CoconutCamera.TryGetComponent(out CustomCoconutCamera component)) Object.Destroy(component);
        }
    }
}
