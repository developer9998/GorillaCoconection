using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using TMPro;
using UnityEngine;

namespace GorillaCoconection
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;

        public static GameObject Template;

        public void Awake()
        {
            Logger = base.Logger;

            Harmony.CreateAndPatchAll(GetType().Assembly, Constants.GUID);

            GorillaTagger.OnPlayerSpawned(Initialize);
        }

        public void Initialize()
        {
            try
            {
                Template = new(Constants.TagName);

                TextMeshPro textMeshPro = Template.AddComponent<TextMeshPro>();
                textMeshPro.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
                textMeshPro.overflowMode = TextOverflowModes.Overflow;
                textMeshPro.enableWordWrapping = false;
                textMeshPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
                textMeshPro.verticalAlignment = VerticalAlignmentOptions.Middle;
                textMeshPro.text = "TEST";
                textMeshPro.ForceMeshUpdate();

                Template.SetActive(false);
            }
            catch (Exception ex)
            {
                Logger.LogFatal("GorillaCoconection failed to initialize");
                Logger.LogError(ex);
            }
        }
    }
}
