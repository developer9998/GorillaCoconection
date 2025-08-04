using GorillaCoconection.Behaviours;
using HarmonyLib;
using UnityEngine;

namespace GorillaCoconection.Patches
{
    [HarmonyPatch(typeof(LckSocialCamera))]
    internal class SocialCameraPatches
    {
        [HarmonyPatch(nameof(LckSocialCamera.OnEnable))]
        [HarmonyPostfix]
        [HarmonyWrapSafe]
        public static void StartPatch(LckSocialCamera __instance)
        {
            if (__instance.IsLocallyOwned || __instance.CoconutCamera.GetComponent<CustomCoconutCamera>()) return;
            var component = __instance.CoconutCamera.gameObject.AddComponent<CustomCoconutCamera>();
            component.Player = __instance.Owner;
        }

        [HarmonyPatch(nameof(LckSocialCamera.OnDisable))]
        [HarmonyPostfix]
        public static void DisablePatch(LckSocialCamera __instance)
        {
            if (__instance.CoconutCamera.TryGetComponent(out CustomCoconutCamera component)) Object.Destroy(component);
        }
    }
}
