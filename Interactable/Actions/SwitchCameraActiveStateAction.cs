
using UnityEngine;

public class SwitchCameraActiveStateAction : InteractionAction
{
    [SerializeField] private SwitchCameraManager SwitchCameraManager;
    [SerializeField] private bool setActive;

    private void Awake()
    {
        if (SwitchCameraManager == null)
            SwitchCameraManager = FindObjectOfType<SwitchCameraManager>();
    }

    public override void ExecuteAction()
    {
        if (SwitchCameraManager == null)
            SwitchCameraManager = FindObjectOfType<SwitchCameraManager>();

        SwitchCameraManager.SetActive(setActive);
    }
}
