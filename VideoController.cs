
using UnityEngine;
using UnityEngine.Video;
public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private InteractionAction newSceneAction;

    private void Start()
    {
        videoPlayer.Play();

        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        newSceneAction.ExecuteAction();
    }
}
