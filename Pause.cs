
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private bool isPaused=false;
    [SerializeField] private GameObject pauseOPT;
    private InputController inputController;

    private void Awake()
    {
        Time.timeScale = 1;
        inputController = FindObjectOfType<InputController>();

        pauseOPT.SetActive(false);
       
    }


    public void PauseInput()
    {
        if (isPaused)
        {
            if (inputController)
                inputController.SetActive(true);

            Time.timeScale = 1;

            pauseOPT.SetActive(false);

            isPaused = false;
        }
        else
        {
            if (inputController)
                inputController.SetActive(false);

            Time.timeScale = 0;

            pauseOPT.SetActive(true);

            isPaused = true;
        }

    }
}
