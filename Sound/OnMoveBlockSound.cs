
using UnityEngine;

public class OnMoveBlockSound : MonoBehaviour
{
    [SerializeField] private SlidePuzzleManager puzzleManager;

    [Header("Sound")]
    [SerializeField] private AudioChannel channel;
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private Transform positionReference;

    private void Awake()
    {
        if (puzzleManager == null)
            puzzleManager = GetComponent<SlidePuzzleManager>();

        puzzleManager.OnMoveBlock += SlidePuzzleManager_OnMoveBlock;
    }

    private void SlidePuzzleManager_OnMoveBlock()
    {
        if (positionReference)
            channel.AudioRequest(audioConfig, positionReference.position);
        else
            channel.AudioRequest(audioConfig, Vector3.zero);
    }
}
