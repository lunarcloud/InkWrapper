﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InkPlusPlus.SpeechBubble
{

    [RequireComponent(typeof(InkManager))]
    [DisallowMultipleComponent]
    [HelpURL("https://github.com/lunarcloud/InkWrapper")]
    public class SpeechBubbleManager : MonoBehaviour
    {
        private InkManager ink;

        [SerializeField]
        [ContextMenuItem("Auto Detect All", "AutodetectTalkables")]
        public List<Talkable> talkables = new List<Talkable>();

        [SerializeField]
        [Tooltip("If left unset, will attempt to set to \"None\".")]
        public Talkable speaker;

        private void OnValidate()
        {
            ink = ink ?? GetComponent<InkManager>();
            ink.GetOrAddTagEvent("speaker"); // Make sure it shows up in the inspector
        }

        void Start()
        {
            HideAllSpeechBubbles();
            ink.AddTagListener("speaker", SetSpeaker);
            ink.storyUpdate.AddListener(StoryUpdate);
            ink.Initialize();
            SetSpeaker(speaker?.name ?? ink.story.variablesState["lastKnownSpeaker"] as string ?? "None");
            ink.Continue();
        }

        private void HideAllSpeechBubbles() => talkables.ForEach(o => o?.speechBubble.SetActive(false));

        private void SetSpeaker(string value)
        {
            speaker?.speechBubble.SetActive(false);
            speaker = talkables.Find(o => o?.name == value); // yes it can be set to null, aka hidden

            if (ink?.story != null)
                ink.story.variablesState["lastKnownSpeaker"] = value;
        }

        private void StoryUpdate(InkPlusPlus.StoryUpdate update)
        {
            speaker?.speechBubble.SetActive(false);
            speaker?.speechBubble.SetText(update.text);
            speaker?.speechBubble.SetContinueButtonActive(update.choices.Count == 0);
            speaker?.speechBubble.SetActive(true);
        }

        [ContextMenu("Autodetect All Talkables")]
        private void AutodetectTalkables() => talkables = (FindObjectsOfType(typeof(Talkable)) as Talkable[])?.ToList<Talkable>();
    }
}
