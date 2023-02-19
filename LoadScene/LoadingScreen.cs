
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [Header("Level Load")]
    [SerializeField] private LoadLevelData levelData;

    [SerializeField] private string loadingScene = "LOADING";

    public void LoadScene(string sceneName)
    {
        levelData.levelToLoad = sceneName;
        SceneManager.LoadScene(loadingScene);
    }
}
