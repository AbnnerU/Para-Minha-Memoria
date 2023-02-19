using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAction : InteractionAction
{
    [SerializeField] private LoadLevelData levelData;

    [SerializeField] private string sceneName;

    [SerializeField] private string loadingScene = "LOADING";

    [Header("Delay")]
    [Space(10)]
    [SerializeField] private float delayToLoadScene = 0f;

    public override void ExecuteAction()
    {
        levelData.levelToLoad = sceneName;

        if (delayToLoadScene <= 0)
            SceneManager.LoadScene(loadingScene);
        else
            StartCoroutine(DelayToLoad());
    }

    IEnumerator DelayToLoad()
    {
        yield return new WaitForSeconds(delayToLoadScene);
        SceneManager.LoadScene(loadingScene);

    }
}
