using GorillaCoconection.Behaviours;
using HarmonyLib;
using UnityEngine;

namespace GorillaCoconection.Patches
{
    [HarmonyPatch(typeof(LckSocialCamera))]
    internal class SocialCameraPatches
    {
        [HarmonyPatch(nameof(LckSocialCamera.Start))]
        [HarmonyPostfix]
        [HarmonyWrapSafe]
        public static void ApplyPatch(LckSocialCamera __instance)
        {
            Plugin.Logger.LogMessage($"LckSocialCamera apply patch: {__instance.CoconutCamera.gameObject.name}");

            if (__instance.IsLocallyOwned)
            {
                Plugin.Logger.LogWarning($"Patch cannot change local coconut");
                return;
            }

            if (__instance.CoconutCamera.GetComponent<CustomCoconutCamera>())
            {
                Plugin.Logger.LogWarning($"Coconut already still has changes");
                return;
            }

            var component = __instance.CoconutCamera.gameObject.AddComponent<CustomCoconutCamera>();
            component.Player = __instance.Owner;
        }

        [HarmonyPatch(nameof(LckSocialCamera.OnDisable))]
        [HarmonyPostfix]
        public static void RemovePatch(LckSocialCamera __instance)
        {
            Plugin.Logger.LogMessage($"LckSocialCamera remove patch: {__instance.CoconutCamera.gameObject.name}");

            if (__instance.CoconutCamera.TryGetComponent(out CustomCoconutCamera component)) Object.Destroy(component);
        }
    }
}
