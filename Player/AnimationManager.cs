using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private bool active = true;
    [SerializeField] private Animator anim;

    private Movement2D movement;

    [SerializeField]private LedgeGrab ledgeGrab;

    [SerializeField] private Climb climb;

    private void Awake()
    {
        movement = GetComponent<Movement2D>();


        if (movement)
        {
            movement.OnNewInputValue += Movement_OnDisplacement;
            movement.OnGroundState += Movement_OnGrundState;
            movement.OnJump += Movement_OnJump;
        }

        if (ledgeGrab)
            ledgeGrab.OnGrab += LedgeGrab_OnGrab;

        if (climb)
        {
            climb.OnClimb += Climb_OnClimb;
            climb.OnMoveOnClimb += Climb_OnMoveOnClimb;
            climb.OnStartClimb += Climp_OnstartClimb;
        }
    }

    private void Climp_OnstartClimb()
    {
        if (anim && active)
            anim.SetTrigger("Climb");
    }

    private void Climb_OnMoveOnClimb(float obj)
    {
        if (anim && active)
            anim.SetFloat("MovementOnClimb", obj);
    }

    private void Climb_OnClimb(bool obj)
    {
        if (anim && active)
            anim.SetBool("OnClimb",obj);
    }

    private void LedgeGrab_OnGrab()
    {
        if (anim && active)
            anim.SetTrigger("LedgeGrab");
    }

    private void Movement_OnGrundState(bool onGround)
    {
        if (anim && active)
            anim.SetBool("OnGround", onGround);
    }

    private void Movement_OnJump()
    {
        if (anim && active)
            anim.SetTrigger("OnJump");
    }

    private void Movement_OnDisplacement(float displacementValue)
    {
        if (anim && active)
            anim.SetFloat("Input", displacementValue);
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }

    public void SetAnimation(string animName)
    {
        if (anim)
            anim.Play(animName, 0, 0);
    }

    public void SetFloat(string parameter, float value)
    {
        if (anim)
            anim.SetFloat(parameter, value);
    }

    public void SetBool(string parameter, bool value)
    {
        if (anim)
            anim.SetBool(parameter, value);
    }
}
