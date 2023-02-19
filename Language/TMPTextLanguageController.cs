
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Language
{
    public class TMPTextLanguageController : MonoBehaviour
    {
        [SerializeField] private TMP_Text textComponent;

        [SerializeField] private List<TextLanguageContent> languageContent = new List<TextLanguageContent>();

        private GameLanguage gameLanguage;


        private void Awake()
        {
            gameLanguage = FindObjectOfType<GameLanguage>();

            textComponent.text = languageContent.Find(x => x.language == gameLanguage.GetGameLanguage()).text;
        }

        [System.Serializable]
        public struct TextLanguageContent
        {
            public LanguageName language;
            public string text;
        }
    }
}