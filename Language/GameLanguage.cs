
using UnityEngine;

namespace Assets.Scripts.Language{
    public class GameLanguage : MonoBehaviour
    {
        [SerializeField] private LanguageName gameLanguageConfig;

        public LanguageName GetGameLanguage()
        {
            return gameLanguageConfig;
        }
    }

    public enum LanguageName
    {
        PT,
        USA
    }
}