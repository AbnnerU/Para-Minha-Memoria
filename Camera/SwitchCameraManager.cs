using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraManager : MonoBehaviour
{
    [SerializeField] private bool active=true;

    [Header("Start")]
    [SerializeField] private bool mainGridEnabled=false;
    [SerializeField] private CameraState cameraState;

    public enum CameraState
    {
        EDITOR,
        TRANSITION,
        GAMEPLAY      
    }


    [Header("General")]

    [SerializeField] private Animator animator;
    [SerializeField] private AnimationManager animationManager;

    [SerializeField] private SpawnPoint spawnPoint;

    //[SerializeField] private TimeBody timeBody;

    [SerializeField] private DistorcionEffect distorcionEffect;

    [SerializeField] private SlidePuzzleManager mainGrid;

    [SerializeField] private GameObject resetButton;

    [Header("Gameplay Mode")]
    [Space(7)]

    [SerializeField] private CameraConfig cameraGameplayMode;

    [Header("Editor Mode")]
    [Space(7)]

    [SerializeField] private CameraConfig cameraEditorMode;
    [SerializeField] private string onEditorFParameter = "Input";
    [SerializeField] private float onEditorFValue = 0;
    [SerializeField] private string onEditorBParameter = "OnGround";
    [SerializeField] private bool onEditorBValue = true;

    [Header("Transition")]
    [Space(7)]
    [SerializeField] private bool useCameraTransition;

    [SerializeField] private float transitionTime;

    private List<IMovementGeneral> allMovementScripts = new List<IMovementGeneral>();

    private InputController inputController;

    private void OnDisable()
    {
        active = false;
    }

    private void Awake()
    {
        var reference = FindObjectsOfType<MonoBehaviour>().OfType<IMovementGeneral>();

        foreach (IMovementGeneral mg in reference)
        {
            allMovementScripts.Add(mg);
        }

        if (animator == null)
            animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        if (spawnPoint == null)
            spawnPoint = FindObjectOfType<SpawnPoint>();

        if (animationManager == null)
            animationManager = FindObjectOfType<AnimationManager>();

        inputController = FindObjectOfType<InputController>();

        inputController.OnChangeMode += InputController_OnChangeMode;

        //timeBody.OnEnd += ExecuteEditorMode;

        if (cameraState == CameraState.EDITOR)
        {
            ExecuteEditorMode(false,mainGridEnabled);
        }
        else
        {
            ExecuteGameplayMode();
        }
    }

    private void InputController_OnChangeMode()
    {      
        if (cameraState == CameraState.TRANSITION || active == false)
            return;

        print(cameraState);

        if (cameraState == CameraState.EDITOR)
        {
            ExecuteGameplayMode();
        }
        else if(cameraState == CameraState.GAMEPLAY)
        {

            foreach (IMovementGeneral mg in allMovementScripts)
            {
                mg.Disable();
            }
                    
            //timeBody.StartRewind();

            distorcionEffect.ExecuteAnimation(0, () => ExecuteEditorMode());

            cameraState = CameraState.TRANSITION;      
        }
               
    }

    private void ExecuteGameplayMode()
    {
        foreach (IMovementGeneral mg in allMovementScripts)
        {
            mg.Enable(); 
        }

        //timeBody.CanRecord(true);

        animationManager.SetActive(true);

        mainGrid.SetActive(false);

        cameraGameplayMode.SetConfigs();

        spawnPoint.SetNewObjectParent(null);

        spawnPoint.SetObjectAtSpawnPoint();

        cameraState = CameraState.GAMEPLAY;

        resetButton.SetActive(false);
    }

    public void ExecuteEditorMode()
    {
        EditorModeMainActions();

        distorcionEffect.ExecuteAnimation(1);

        if (useCameraTransition)
        {
            cameraState = CameraState.TRANSITION;
            StartCoroutine(CameraTransition());
        }
        else
        {
            cameraEditorMode.SetConfigs();
            mainGrid.SetActive(true);
            cameraState = CameraState.EDITOR;
        }
    }

    public void ExecuteEditorMode(bool cameraTransition)
    {
        EditorModeMainActions();

        if (cameraTransition)
        {
            cameraState = CameraState.TRANSITION;
            StartCoroutine(CameraTransition());
        }
        else
        {
            cameraEditorMode.SetConfigs();
            mainGrid.SetActive(true);
            cameraState = CameraState.EDITOR;
        }
    }

    public void ExecuteEditorMode(bool cameraTransition, bool changeMainGridActiveState, float transitionTime)
    {
        EditorModeMainActions();

        if (cameraTransition)
        {
            cameraState = CameraState.TRANSITION;
            StartCoroutine(CameraTransition(changeMainGridActiveState,transitionTime));
        }
        else
        {
            cameraEditorMode.SetConfigs();
            mainGrid.SetActive(true);
            cameraState = CameraState.EDITOR;
        }
    }

    public void ExecuteEditorMode(bool cameraTransition, bool mainGridEnabled)
    {
        EditorModeMainActions();

        if (cameraTransition)
        {
            cameraState = CameraState.TRANSITION;
            StartCoroutine(CameraTransition());
        }
        else
        {
            cameraEditorMode.SetConfigs();
            mainGrid.SetActive(mainGridEnabled);
            cameraState = CameraState.EDITOR;
        }
    }

    private void EditorModeMainActions()
    {
        //timeBody.ResetAll();

        //timeBody.CanRecord(false);
        animationManager.SetActive(false);

        animationManager.SetFloat(onEditorFParameter, onEditorFValue);
        animationManager.SetBool(onEditorBParameter, onEditorBValue);
        animationManager.SetBool("OnClimb", false);

        spawnPoint.SetObjectAtSpawnPoint();

        spawnPoint.SetNewObjectParent(spawnPoint.GetSpawnPointTransform());

        foreach (IMovementGeneral mg in allMovementScripts)
        {
            mg.Disable();
        }

        resetButton.SetActive(true);
    }

    IEnumerator CameraTransition()
    {
        Camera camera = cameraEditorMode.GetCamera();
        Transform cameraTransform = camera.transform;

        Vector3 startPosition = cameraTransform.position;
        Vector3 positionDifference = cameraEditorMode.GetConfigPosition() - startPosition;

        float startSize = camera.orthographicSize;
        float sizeDifference = cameraEditorMode.GetCameraSizeConfig() - startSize;

        float currentTime = 0;
        float addValue = 1 / transitionTime;
        float value = 0;

        do
        {
            //Timer
            currentTime += Time.deltaTime;

            //Transition Value
            value += addValue * Time.deltaTime;

            //ChangePosition
            cameraTransform.position = startPosition + (positionDifference * value);

            //ChangeSize
            camera.orthographicSize = startSize + (sizeDifference * value);

            yield return null;

        } while (currentTime < transitionTime);

        mainGrid.SetActive(true);

        cameraEditorMode.SetConfigs();

        cameraState = CameraState.EDITOR;

        yield break;
    }

    IEnumerator CameraTransition(bool changeMainGridActiveState, float transitionTime)
    {
        Camera camera = cameraEditorMode.GetCamera();
        Transform cameraTransform = camera.transform;

        Vector3 startPosition = cameraTransform.position;
        Vector3 positionDifference = cameraEditorMode.GetConfigPosition() - startPosition;

        float startSize = camera.orthographicSize;
        float sizeDifference = cameraEditorMode.GetCameraSizeConfig() - startSize;

        float currentTime = 0;
        float addValue = 1 / transitionTime;
        float value = 0;

        do
        {
            //Timer
            currentTime += Time.deltaTime;

            //Transition Value
            value += addValue * Time.deltaTime;

            //ChangePosition
            cameraTransform.position = startPosition + (positionDifference * value);

            //ChangeSize
            camera.orthographicSize = startSize + (sizeDifference * value);

            yield return null;

        } while (currentTime < transitionTime);

        if(changeMainGridActiveState)
            mainGrid.SetActive(true);

        cameraEditorMode.SetConfigs();

        cameraState = CameraState.EDITOR;

        yield break;
    }

    public void SetActive(bool isActive)
    {
        active = isActive;
    }

}
