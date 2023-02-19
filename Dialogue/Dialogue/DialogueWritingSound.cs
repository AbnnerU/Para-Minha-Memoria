using System.Collections;
using UnityEngine;


public class DialogueWritingSound : MonoBehaviour
{
    [SerializeField] private bool active;
    [SerializeField] private DialogueVisual dialogueVisual;
    [SerializeField] private AudioChannel channel;
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private float soundInterval;

    private bool writing = false;

    private void Awake()
    {
        if (dialogueVisual == null)
            dialogueVisual = FindObjectOfType<DialogueVisual>();

        dialogueVisual.OnWritingText += DialogueVisual_OnWritingText;
        dialogueVisual.OnStopWriting += DialogueVisual_OnStopWriting;
    }

    private void DialogueVisual_OnWritingText()
    {
        writing = true;
        StartCoroutine(PlayWritingSound());
    }

    private void DialogueVisual_OnStopWriting()
    {
        writing = false;
        StopCoroutine(PlayWritingSound());
        channel.StopAudioRequest(audioConfig, Vector3.zero);
    }

    IEnumerator PlayWritingSound()
    {
        float currentTime = 0;
        while (active && writing)
        {
            currentTime = 0;
            channel.AudioRequest(audioConfig, Vector3.zero);

            do
            {
                currentTime += Time.deltaTime;

                if (active == false || writing == false)
                {
                    channel.StopAudioRequest(audioConfig, Vector3.zero);
                    yield break;
                }
                yield return null;

            } while (currentTime < soundInterval);
        }
        channel.StopAudioRequest(audioConfig, Vector3.zero);
    }
}
