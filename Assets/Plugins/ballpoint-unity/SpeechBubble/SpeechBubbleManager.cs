﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ballpoint.SpeechBubble {

    [RequireComponent(typeof(InkManager))]
    [DisallowMultipleComponent]
    [HelpURL(InkManager.HelpURL)]
    public class SpeechBubbleManager : MonoBehaviour {
        private InkManager ink;

        [SerializeField]
        [ContextMenuItem("Auto Detect All", "AutodetectTalkables")]
        public List<Talkable> talkables = new List<Talkable>();

        [SerializeField]
        [Tooltip("If left unset, will attempt to set to \"None\".")]
        public Talkable speaker;

        private void OnValidate() {
            ink = ink ?? GetComponent<InkManager>();
            ink.GetOrAddTagChangeWatcher("said"); // Make sure it shows up in the inspector 
        }

        void Start() {
            HideAllSpeechBubbles();
            ink.AddTagListener("said", SetSpeaker);
            ink.storyUpdate.AddListener(StoryUpdate);
            ink.storyEnded.AddListener(HideAllSpeechBubbles);
            ink.Initialize();
            SetSpeaker(speaker?.name ?? ink.story.variablesState["lastKnownSpeaker"] as string ?? "None");
            ink.BeginStory();
        }

        private void HideAllSpeechBubbles() => talkables?.ForEach(o => o?.SetAsSpeaker(false));

        private void SetSpeaker(string value) {
            //Debug.Log($"speaker is {value}");
            speaker?.SetAsSpeaker(false);
            speaker = talkables.Find(o => o?.name == value); // yes it can be set to null, aka hidden
            speaker?.SetAsSpeaker(true);

            ink.story.variablesState["lastKnownSpeaker"] = value;
        }

        private void StoryUpdate(Ballpoint.StoryUpdate update) => speaker?.StoryUpdate(update);

        [ContextMenu("Autodetect All Talkables")]
        private void AutodetectTalkables() => talkables = (FindObjectsOfType(typeof(Talkable)) as Talkable[])?.ToList<Talkable>();
    }
}
