using Liv.Lck.GorillaTag;
using System;
using TMPro;
using UnityEngine;

namespace GorillaCoconection.Behaviours
{
    [RequireComponent(typeof(CoconutCamera)), DisallowMultipleComponent]
    public class CustomCoconutCamera : MonoBehaviour
    {
        public bool IsLocalCamera;

        public NetPlayer Player;

        private TMP_Text playerNameTag;

        public void Start()
        {
            Plugin.Logger.LogMessage($"+ CustomCoconutCamera: {(IsLocalCamera ? "local player" : Player.NickName)}");

            CoconutCamera coconut = GetComponent<CoconutCamera>();
            Transform visuals = coconut._visuals.transform;

            if (visuals.Find(Constants.TagName))
            {
                Destroy(this);
                return;
            }

            GameObject gameObject = Instantiate(Plugin.Template);
            gameObject.name = Constants.TagName;
            gameObject.transform.SetParent(visuals, false);
            gameObject.transform.localPosition = Vector3.up * 0.5f;
            gameObject.transform.localRotation = Quaternion.Euler(Vector3.up * 180f);
            gameObject.transform.localScale = Vector3.one * 0.06f;
            gameObject.SetActive(true);
            playerNameTag = gameObject.GetComponent<TextMeshPro>();

            if (IsLocalCamera) VRRigCache.Instance.localRig.Rig.OnNameChanged += OnNameChanged;
            else VRRigCache.OnRigNameChanged += OnNameChanged; // TODO: test name change for other players

            UpdateName();
        }

        public void OnDestroy()
        {
            Plugin.Logger.LogMessage($"- CustomCoconutCamera: {(IsLocalCamera ? "local player" : Player.NickName)}");

            if (IsLocalCamera) VRRigCache.Instance.localRig.Rig.OnNameChanged -= OnNameChanged;
            else VRRigCache.OnRigNameChanged -= OnNameChanged;

            if (playerNameTag is null || !playerNameTag) return;
            Destroy(playerNameTag.gameObject);
        }

        public void OnNameChanged(RigContainer playerRig)
        {
            try
            {
                if (IsLocalCamera ? (VRRigCache.Instance.localRig != playerRig) : (playerRig.Creator != Player)) return;
                UpdateName();
            }
            catch(Exception)
            {

            }
        }

        public void UpdateName()
        {
            // NOTE 1: NetPlayer.SanitizedNickName property is set after the event we rely on is invocated, hence why VRRig.playerNameVisible is used instead

            // NOTE 2: VRRigCache.rigsInUse is used in favour of VRRigCache.TryGetVrrig to not fall back on creating the rig for the player, since it might be early yet

            try
            {
                NetPlayer player = IsLocalCamera ? NetworkSystem.Instance.GetLocalPlayer() : Player;
                playerNameTag.text = VRRigCache.rigsInUse.TryGetValue(player, out RigContainer playerRig) ? (playerRig.Rig.playerNameVisible ?? player.NickName) : player.NickName;
            }
            catch (Exception)
            {

            }
        }
    }
}
