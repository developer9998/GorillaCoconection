using BepInEx;
using HarmonyLib;

namespace GorillaCoconection
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            Harmony.CreateAndPatchAll(GetType().Assembly, Constants.GUID);
        }
    }
}
