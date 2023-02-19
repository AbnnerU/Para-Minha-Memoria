
using UnityEngine;

public class StopSoundAction : InteractionAction
{
    [Header("Sound")]
    [SerializeField] private AudioChannel channel;
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private Transform positionReference;

    public override void ExecuteAction()
    {
        if (positionReference)
            channel?.StopAudioRequest(audioConfig, positionReference.position);
        else
            channel?.StopAudioRequest(audioConfig, Vector3.zero);
    }
}
