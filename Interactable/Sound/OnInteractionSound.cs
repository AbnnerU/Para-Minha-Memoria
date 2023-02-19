
using UnityEngine;

public class OnInteractionSound : MonoBehaviour
{
    [SerializeField] private InteractionEvents interactionEvents;

    [Header("Sound")]
    [SerializeField] private AudioChannel channel;
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private Transform positionReference;


    private void Awake()
    {
        if (interactionEvents == null)
            interactionEvents = GetComponent<InteractionEvents>();

        interactionEvents.OnClickEvent += InteractionEvents_OnClick;
    }

    private void InteractionEvents_OnClick()
    {
        if (positionReference)
            channel.AudioRequest(audioConfig, positionReference.position);
        else
            channel.AudioRequest(audioConfig, Vector3.zero);
    }
}
