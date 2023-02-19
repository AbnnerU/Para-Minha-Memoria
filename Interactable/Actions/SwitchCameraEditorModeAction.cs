using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraEditorModeAction : InteractionAction
{
    [SerializeField] private SwitchCameraManager SwitchCameraManager;
    [SerializeField] private bool cameraTransition;
    [SerializeField] private bool changeMainGridActiveState;
    [SerializeField] private float transitionTime=1;

    private void Awake()
    {
        if (SwitchCameraManager == null)
            SwitchCameraManager = FindObjectOfType<SwitchCameraManager>();
    }


    public override void ExecuteAction()
    {
        if (SwitchCameraManager == null)
            SwitchCameraManager = FindObjectOfType<SwitchCameraManager>();

        SwitchCameraManager.ExecuteEditorMode(cameraTransition,changeMainGridActiveState,transitionTime);
    }
}
