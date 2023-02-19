using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpTransition : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private Image image;

    [SerializeField] private CameraFollow cameraFollow;

    [SerializeField] private List<UPTransitionConfig> transitionConfigs;

    [SerializeField] private bool disableMovementScripts=true;

    private List<IMovementGeneral> allMovementScripts = new List<IMovementGeneral>();

    private Transform camTransform;

    private RectTransform imageTransform;

    private void Awake()
    {
        if (disableMovementScripts)
        {
            var reference = FindObjectsOfType<MonoBehaviour>().OfType<IMovementGeneral>();

            foreach (IMovementGeneral mg in reference)
            {
                allMovementScripts.Add(mg);
            }
        }

        if (cam == null)
            cam = Camera.main;

        camTransform = cam.transform;
        imageTransform = image.GetComponent<RectTransform>();

        if (cameraFollow == null)
            cameraFollow = FindObjectOfType<CameraFollow>();
    }


    public void ExecuteTransition(int transitionID)
    {
        int index = transitionConfigs.FindIndex(x => x.id == transitionID);

        if (index < 0)
        {
            Debug.LogWarning(transitionID + "none transition whit this id");
            return;
        }

        StartCoroutine(TransitionAnimation(index));

    }

    IEnumerator TransitionAnimation(int id)
    {
        if (disableMovementScripts)
        {
            foreach (IMovementGeneral mg in allMovementScripts)
                mg.Disable();

        }

        cameraFollow.enabled = false;

        UPTransitionConfig config = transitionConfigs[id];

        Vector2 imageStartValue = config.imageStartPosition;
        Vector2 imageMoveDifference = config.imageFinalPosition - config.imageStartPosition;
        AnimationCurve imageCurve = config.imageMoveCurve;

        Vector3 cameraStartPosition = Vector3.zero;

        if (config.useCurrentCameraPositionAsStart)
            cameraStartPosition = camTransform.position;
        else
            cameraStartPosition = config.camStartPosition;
        
        Vector3 cameraDisplacement = config.cameraAddValue;
        AnimationCurve camCurve = config.cameraMoveCurve;

        float durantion = config.totalDurantion;
        float currentTime = 0;
        float percentage = 0;

        imageTransform.anchoredPosition = imageStartValue;
        imageTransform.localEulerAngles = config.imageRotation;

        do
        {
            currentTime += Time.deltaTime;
            percentage = ((currentTime * 100) / durantion) / 100;

            //Image
            imageTransform.anchoredPosition = imageStartValue + (imageCurve.Evaluate(percentage) * imageMoveDifference);


            //Camera
            camTransform.position = cameraStartPosition + (camCurve.Evaluate(percentage) * cameraDisplacement);

            yield return null;

        } while (currentTime < durantion);

       
        yield break;
    }

}

[System.Serializable]
public struct UPTransitionConfig
{
    public int id;
    public int totalDurantion;
    [Header("Camera")]
    public bool useCurrentCameraPositionAsStart;
    public Vector3 camStartPosition;
    public Vector3 cameraAddValue;
    public AnimationCurve cameraMoveCurve;

    [Header("Image")]
    public Vector3 imageRotation;
    public Vector2 imageStartPosition;
    public Vector2 imageFinalPosition;
    public AnimationCurve imageMoveCurve;
}
