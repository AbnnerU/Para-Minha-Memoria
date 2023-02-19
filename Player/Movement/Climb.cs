using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour,IMovementGeneral
{
    [SerializeField] private bool active = true;
    [SerializeField] private bool drawGizmos;
    [SerializeField] private string ladderTag = "Ladder";

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Collider2D playerCollider;

    [SerializeField] private Movement2D movement;

    [Header("Ground detection")]
    [SerializeField] private Transform ladderDetectionStartPoint;

    [SerializeField] private Vector2 ladderDetectionSize;

    [SerializeField] private LayerMask ladderLayerMask;

    [Header("Movement")]
    [Space(10)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;

    [Header("Jump")]
    [Space(10)]
    [SerializeField] private Vector2 jumpForce;

    public Action<bool> OnClimb;

    public Action<float> OnMoveOnClimb;

    public Action OnStartClimb;

    private Transform _transform;

    private InputController inputController;

    private Vector2 movementInput;

    private Vector2 climbModeInput;

    private Ladder ladder = null;

    private bool isClimbing = false;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (playerCollider == null)
            playerCollider = GetComponent<Collider2D>();

        inputController = FindObjectOfType<InputController>();

        _transform = GetComponent<Transform>();

        inputController.OnClimbEvent += InputController_OnClimbMovement;

        inputController.OnEnterInClimbMode += InputController_OnEnterClimbMode;

        //inputController.OnJumpEvent += InputController_OnJump;
    }
   
    private void Update()
    {
        if (isClimbing == true || active == false)
            return;

        if (GetAbsolutyValue(climbModeInput.y) > 0 && movementInput.x==0)
        {
            if (IsOnLadder(out ladder))
            {
                if(ladder.GetLadderPart() == Ladder.LadderPart.END && climbModeInput.y > 0)
                {
                    print("Invalid");
                    return;
                }
                else if(ladder.GetLadderPart() == Ladder.LadderPart.START && climbModeInput.y < 0)
                {
                    print("Invalid");
                    return;
                }
                else
                {
                    OnStartClimb?.Invoke();
                    OnClimb?.Invoke(true);

                    isClimbing = true;
                    movement.DisableMovement();

                    rb.velocity = Vector2.zero;

                    rb.position = new Vector2(ladder.GetLadderMiddlePosition().x, rb.position.y);

                    rb.gravityScale = 0;

                    if (ladder?.GetPlataformCollider() != null)
                        Physics2D.IgnoreCollision(playerCollider, ladder.GetPlataformCollider(), true);
                }                                         
            }
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing == false || active==false)
            return;

        if (IsOnLadder() == false)
        {
            rb.velocity = Vector2.zero;
            StopClimb();
        }

        if (GetAbsolutyValue(movementInput.x) > 0)
        {
            StopClimb();

            Vector2 jump = new Vector2(jumpForce.x * movementInput.x, jumpForce.y);

            rb.AddForce(jump, ForceMode2D.Impulse);

            return;
        }

        float movementValue = 0;

        float targetSpeed = moveSpeed * movementInput.y;

        float diff = targetSpeed - rb.velocity.y;

        float accelValue = 0;

        if (GetAbsolutyValue(targetSpeed) > 0.01f)
            accelValue = acceleration;
        else
            accelValue = decceleration;

        movementValue = diff * accelValue;

        rb.AddForce(Vector2.up * movementValue);

    }

    private void StopClimb()
    {
        rb.velocity = Vector2.zero;

        isClimbing = false;

        movement.EnableMovement();

        movement.SetFacingDirection(movementInput.x);
    
        if (ladder?.GetPlataformCollider() != null)
            Physics2D.IgnoreCollision(playerCollider, ladder.GetPlataformCollider(), false);

        ladder = null;

        OnClimb?.Invoke(false);
    }

    private float GetAbsolutyValue(float value)
    {
        if (value < 0)
            return value * -1;
        else
            return value;
    }

    private bool IsOnLadder()
    {
        Collider2D[] hits = new Collider2D[1];

        if (Physics2D.OverlapBoxNonAlloc(ladderDetectionStartPoint.position, ladderDetectionSize, 0, hits, ladderLayerMask,0) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsOnLadder(out Ladder result)
    {
        Collider2D[] hits = new Collider2D[1];

        if (Physics2D.OverlapBoxNonAlloc(ladderDetectionStartPoint.position, ladderDetectionSize, 0, hits, ladderLayerMask,0) > 0)
        {

            result = hits[0].GetComponent<Ladder>();
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    private void InputController_OnEnterClimbMode(Vector2 input)
    {
        if(active)
            climbModeInput = input;             
    }

    private void InputController_OnClimbMovement(Vector2 input)
    {
        if (active)
        {
            movementInput = input;

            if (input.y < 0)
                OnMoveOnClimb?.Invoke(-1);
            else if (input.y > 0)
                OnMoveOnClimb?.Invoke(1);
            else
                OnMoveOnClimb?.Invoke(0);
        }
    }
    
    public void DisableClimb()
    {
        active = false;

        isClimbing = false;

        if (ladder?.GetPlataformCollider() != null)
            Physics2D.IgnoreCollision(playerCollider, ladder.GetPlataformCollider(), false);

        ladder = null;

        
    }

    public void EnableClimb()
    {
        active = true;
    }

    public void Disable()
    {
        DisableClimb();
    }

    public void Enable()
    {
        EnableClimb();
    }


    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawCube(ladderDetectionStartPoint.position, ladderDetectionSize);
        }
    }

   
}

