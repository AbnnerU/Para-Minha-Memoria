using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private bool active=true;

    private Controls controls;

    //Gameplay
    public Action<Vector2> OnMoveEvent;

    public Action OnJumpEvent;

    public Action OnLeftClickEvent;

    public Action OnChangeMode;

    public Action<Vector2> OnEnterInClimbMode;

    public Action<Vector2> OnClimbEvent;

    public Action OnDialogueSkipEvent;

    //public Action OnPauseEvent;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();

        //inputActived = InputMapActive.GAMEPLAY;

        //---------------Gameplay---------------
        //Perfomed
        controls.Gameplay.Movement.performed += ctx => Movement_input(ctx.ReadValue<Vector2>());
        controls.Gameplay.Jump.performed += ctx => Jump_Input();
        controls.Gameplay.LeftClick.performed += ctx => LeftClick_Input();
        controls.Gameplay.ChangeMode.performed += ctx => ChangeMode_Input();
        controls.Gameplay.Climb.performed += ctx => Climb_Input(ctx.ReadValue<Vector2>());
        controls.Gameplay.EnterInClimbMode.performed += ctx => EnterInClimbMode_Input(ctx.ReadValue<Vector2>());
        controls.Gameplay.DialogueNextLine.performed += ctx => DialogueNextLine_Input();
        //controls.Gameplay.Pause.performed += ctx => Pause_Input();
        //controls.Gameplay.MouseDelta.performed += ctx => MousePositionUpdate(ctx.ReadValue<Vector2>());

        //Canceled
        controls.Gameplay.Movement.canceled += ctx => Movement_input(ctx.ReadValue<Vector2>());
        controls.Gameplay.Climb.canceled+= ctx => Climb_Input(ctx.ReadValue<Vector2>());
        controls.Gameplay.EnterInClimbMode.canceled += ctx => EnterInClimbMode_Input(ctx.ReadValue<Vector2>());

    }

    public void SetActive(bool active)
    {
        this.active = active;
    }

    #region Gameplay

    public Controls GetControls()
    {
        return controls;
    }

    //private void Pause_Input()
    //{
    //    if (active)
    //        OnPauseEvent?.Invoke();
    //}

    private void Movement_input(Vector2 ctx)
    {
        if (active)
            OnMoveEvent?.Invoke(ctx);
    }


    private void Jump_Input()
    {
        if (active)
            OnJumpEvent?.Invoke();
    }

    private void LeftClick_Input()
    {
        if (active)
            OnLeftClickEvent?.Invoke();
    }

    private void ChangeMode_Input()
    {
        if (active)
            OnChangeMode?.Invoke();
    }

    private void Climb_Input(Vector2 ctx)
    {
        if (active)
            OnClimbEvent?.Invoke(ctx);
    }

    private void EnterInClimbMode_Input(Vector2 ctx)
    {
        //print(ctx);
        if (active)
            OnEnterInClimbMode?.Invoke(ctx);
    }

    private void DialogueNextLine_Input()
    {
        if (active)
            OnDialogueSkipEvent?.Invoke();
    }

    #endregion




}


public enum InputMapActive
{
    GAMEPLAY,
    INVENTORY,
    PAUSE,
    CONTROLSDISABLED
}