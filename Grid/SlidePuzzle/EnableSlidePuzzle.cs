using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSlidePuzzle : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform newSpawnPointParent;

    [SerializeField] private string targetTag;

    [SerializeField] private GameObject obstacleBlock;

    [SerializeField] private SlidePuzzleManager slidePuzzleManager;

    [SerializeField] private SwitchCameraManager switchCameraManager;

    //[Header("Spawn Point")]
    //[Space(5)]


    [Header("On Start")]
    [Space(5)]

    [SerializeField] private DialogueAsset onStartDialogue;

    [SerializeField] private CameraConfig slidePuzzleCameraConfig;

    [SerializeField] private bool useCameraDefaltsConfigs;

    [SerializeField] private float cameraSize;

    [SerializeField] private Vector3 cameraPosition;

    [SerializeField] private GameObject[] objectsToDisable;

    [Header("On Complete")]
    [Space(5)]
    [SerializeField] private DialogueAsset onEndDialogue;

    [SerializeField] private SpriteRenderer emptySlot;

    [SerializeField] private string obstacleBlockAnimation= "BlockHide";

    [SerializeField] private bool selfDisable = true;

    [SerializeField] private GameObject[] objectsToEnable;

    [SerializeField] private BackgroundImage backgroundImage;

    [SerializeField] private Sprite sprite01;

    [SerializeField] private Sprite sprite02;

    [SerializeField] private string transitionName;

    private DialogueManager dialogueManager;

    private GameObject slidePuzzleObject;

    private Transform _transform;

    private SpawnPoint spawnPoint;

    private Animator obstacleBlockAnimator;

    private void Awake()
    {
        if (switchCameraManager == null)
            switchCameraManager = FindObjectOfType<SwitchCameraManager>();

        if (backgroundImage == null)
            backgroundImage = FindObjectOfType<BackgroundImage>();

        if (emptySlot.enabled)
            emptySlot.enabled = false;

        obstacleBlockAnimator = obstacleBlock.GetComponent<Animator>();

        dialogueManager = FindObjectOfType<DialogueManager>();

        _transform = GetComponent<Transform>();

        spawnPoint = FindObjectOfType<SpawnPoint>();

        newSpawnPointParent = this.gameObject.transform.parent;

        slidePuzzleManager.OnCompleteAction += SlidePuzzleManager_OnCompleteAction;

        slidePuzzleManager.SetActive(false);

        slidePuzzleObject = slidePuzzleManager.gameObject;
    }

    private void SlidePuzzleManager_OnCompleteAction()
    {
        emptySlot.enabled = true;

        if (onEndDialogue)
        {
            slidePuzzleManager.SetActive(false);

            dialogueManager.SetNewDialogue(onEndDialogue, () => CompleteSlidePuzzle());
        }
        else
        {
            Debug.LogWarning("None dialogue");
            CompleteSlidePuzzle();
        }
        
    }

    public void CompleteSlidePuzzle()
    {
        //SpawnPoint
        spawnPoint.SetNewParent(newSpawnPointParent);
        spawnPoint.SetNewPosition(_transform.position);

        //Camera
        switchCameraManager.SetActive(true);
        switchCameraManager.ExecuteEditorMode(false);

        //ObjectsToEnable
        for (int i = 0; i < objectsToEnable.Length; i++)
        {
            objectsToEnable[i].SetActive(true);
        }


        if (selfDisable)
            this.gameObject.SetActive(false);

        slidePuzzleObject.SetActive(false);

        obstacleBlockAnimator.Play(obstacleBlockAnimation,0,0);

        backgroundImage.SetSprites(sprite01, sprite02);
        backgroundImage.PlayAnimation(transitionName);

        //obstracleBlock.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) == false)
            return;

        StartSlidePuzzle();
    }

    private void StartSlidePuzzle()
    {
        slidePuzzleObject.SetActive(true);

        if (onStartDialogue != null)
        {
            slidePuzzleManager.SetActive(false);
            dialogueManager.SetNewDialogue(onStartDialogue, () => slidePuzzleManager.SetActive(true));
        }
        else
        {
            slidePuzzleManager.SetActive(true);
        }

        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            objectsToDisable[i].SetActive(false);
        }

        if (useCameraDefaltsConfigs)
            slidePuzzleCameraConfig.SetConfigs();
        else
            slidePuzzleCameraConfig.SetConfigs(cameraSize,cameraPosition);

        switchCameraManager.SetActive(false);
    }
}
