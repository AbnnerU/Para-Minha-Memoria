
using UnityEngine;

public class OnInteractionSoundArray : MonoBehaviour
{
    [SerializeField] private InteractionEvents interactionEvents;

    [Header("Sound")]
    [SerializeField] private AudioChannel channel;
    [SerializeField] private AudioConfig[] audioConfig;
    [SerializeField] private Transform positionReference;

    [SerializeField] private bool neverRepeatSound = true;

    private int lastIndex = -1;

    private void Awake()
    {
        if (interactionEvents == null)
            interactionEvents = GetComponent<InteractionEvents>();

        interactionEvents.OnClickEvent += InteractionEvents_OnClick;
    }

    private void InteractionEvents_OnClick()
    {
        int index = Random.Range(0, audioConfig.Length);

        if(neverRepeatSound && index == lastIndex)
        {
            if (index + 1 > audioConfig.Length - 1)
                index = 0;
            else
                index++;
        }

        lastIndex = index;

        if (positionReference) 
            channel.AudioRequest(audioConfig[index], positionReference.position);
        else
            channel.AudioRequest(audioConfig[index], Vector3.zero);
    }
}
