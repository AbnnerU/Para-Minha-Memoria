using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneOnEnter : MonoBehaviour
{
    [SerializeField] private string targetTag="Player";

    [SerializeField] private string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
            SceneManager.LoadScene(sceneName);
    }
}
