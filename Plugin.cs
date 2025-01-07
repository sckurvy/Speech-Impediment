using System;
using System.Text.RegularExpressions;
using System.Collections;
using BepInEx;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utilla;

namespace SpeechImpediment
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            ReplaceAllTextInGame();
            StartCoroutine(DelayedTextCheck(5f));
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(DelayedTextCheckAfterSceneLoad(6f));
        }

        private IEnumerator DelayedTextCheckAfterSceneLoad(float delay)
        {
            yield return new WaitForSeconds(delay);
            ReplaceAllTextInGame();
        }

        private void ReplaceAllTextInGame()
        {
            TextMeshPro[] tmpTextObjects = FindObjectsOfType<TextMeshPro>(true);
            foreach (TextMeshPro textObject in tmpTextObjects)
            {
                if (!string.IsNullOrEmpty(textObject.text))
                {
                    textObject.text = ApplySpeechImpediment(textObject.text);
                }
            }

            TextMeshProUGUI[] tmpUGUITextObjects = FindObjectsOfType<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI textObject in tmpUGUITextObjects)
            {
                if (!string.IsNullOrEmpty(textObject.text))
                {
                    textObject.text = ApplySpeechImpediment(textObject.text);
                }
            }

            UnityEngine.UI.Text[] uiTextObjects = FindObjectsOfType<UnityEngine.UI.Text>(true);
            foreach (UnityEngine.UI.Text textObject in uiTextObjects)
            {
                if (!string.IsNullOrEmpty(textObject.text))
                {
                    textObject.text = ApplySpeechImpediment(textObject.text);
                }
            }

            TextMesh[] textMeshObjects = FindObjectsOfType<TextMesh>(true);
            foreach (TextMesh textObject in textMeshObjects)
            {
                if (!string.IsNullOrEmpty(textObject.text))
                {
                    textObject.text = ApplySpeechImpediment(textObject.text);
                }
            }
        }

        private string ApplySpeechImpediment(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            input = Regex.Replace(input, @"\bs\b", "TH", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\bs", "TH", RegexOptions.IgnoreCase);

            input = Regex.Replace(input, @"r", "W", RegexOptions.IgnoreCase);

            input = Regex.Replace(input, @"l", "W", RegexOptions.IgnoreCase);

            input = Regex.Replace(input, @"th", "F", RegexOptions.IgnoreCase);

            return input.ToUpper();
        }

        private IEnumerator DelayedTextCheck(float delay)
        {
            yield return new WaitForSeconds(delay);
            ReplaceAllTextInGame();
        }
    }
}
