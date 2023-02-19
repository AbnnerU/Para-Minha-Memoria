
using UnityEngine;

public class PlaySoundAction : InteractionAction
{
    [Header("Sound")]
    [SerializeField] private AudioChannel channel;
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private Transform positionReference;

    public override void ExecuteAction()
    {
        if (positionReference)
            channel?.AudioRequest(audioConfig, positionReference.position);
        else
            channel?.AudioRequest(audioConfig, Vector3.zero);
    }
}

