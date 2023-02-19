using System;
using UnityEngine;

public class Movement2D : MonoBehaviour,IMovementGeneral
{
    [SerializeField]private bool drawGizmos;

    [SerializeField] private bool active=true;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float defaltGravityScale = 3f;

    [SerializeField] public PlayerState state;

    [Header("Ground Detection")]
    [Space(10)]
    [SerializeField] private Transform startPoint;

    [SerializeField] private Vector2 size;

    [SerializeField] private float angle;

    [SerializeField] private LayerMask layerMask;

    [Header("On Ground Movement")]
    [Space(10)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;

    [Header("On Air Movement")]
    [Space(10)]
    [SerializeField] private float onAirMoveSpeed;
    [SerializeField] private float onAirAcceleration;
    [SerializeField] private float onAirDecceleration;

    [Header("Jump")]
    [Space(10)]
    [SerializeField] private bool useJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBuffer;

    private Transform _transform;

    private InputController inputController;

    private Vector3 lastPosition;
    private Vector2 inputValue;

    public Action<float> OnNewInputValue;
    public Action<bool> OnGroundState;
    public Action OnJump;

    private float coyoteTimerCounter;
    private float jumpBufferCounter;

    private bool facingRight = true;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        inputController = FindObjectOfType<InputController>();

        _transform = GetComponent<Transform>();

        ConfigureInputs();
    }

    private void Start()
    {
        lastPosition = _transform.position;
    }

    private void ConfigureInputs()
    {
        inputController.OnMoveEvent += InputController_OnMoveEvent;

        inputController.OnJumpEvent += InputController_OnJumpEvent;
    }

    private void Update()
    {
        if (active)
        {
            if (state == PlayerState.JUMPING)
            {
                OnGroundState?.Invoke(false);

                coyoteTimerCounter = -1;
                return;
            }
            else if (state == PlayerState.JUMPINGTOAIR)
            {
                OnGroundState?.Invoke(false);

                state = PlayerState.ONAIR;
                return;
            }

            if (OnGround())
            {
                OnGroundState?.Invoke(true);

                state = PlayerState.ONGROUND;

                coyoteTimerCounter = coyoteTime;
            }
            else
            {
                OnGroundState?.Invoke(false);

                coyoteTimerCounter -= Time.deltaTime;
             
                state = PlayerState.ONAIR;            
            }
         
        lastPosition = _transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (active)
        {
            if (state == PlayerState.JUMPING)
            {
                Jump();
            }
            else
            {
                if (state == PlayerState.ONGROUND)
                    CalculateMovement(moveSpeed, acceleration, decceleration);
                else if (state == PlayerState.ONAIR)
                    CalculateMovement(onAirMoveSpeed, onAirAcceleration, onAirDecceleration);

                FacingDirection();
            }
        }

    }

    #region Input

    private void InputController_OnMoveEvent(Vector2 input)
    {
        //if (active == false)
        //    return;

        if (input == Vector2.zero)
            OnNewInputValue?.Invoke(0);
        else
            OnNewInputValue?.Invoke(1);

        input.y = 0f;
        this.inputValue = input;
    }

    private void InputController_OnJumpEvent()
    {
        if (useJump == false)
            return;

        if (active)
        {

            if (state == PlayerState.ONGROUND)
            {
                coyoteTimerCounter = -1;
                state = PlayerState.JUMPING;

            }
            else if (coyoteTimerCounter > 0) 
            {
                coyoteTimerCounter = -1;
                state = PlayerState.JUMPING;
            }
 
        }
      
    }

    #endregion

    private void FacingDirection()
    {
        if (active == false)
            return;

        if (inputValue.x != 0)
        {
            _transform.localScale = new Vector3(Mathf.Sign(inputValue.x), 1, 1);

            if (inputValue.x > 0)
                facingRight = true;
            else
                facingRight = false;
        }

    }

    public void SetFacingDirection(float inputValue)
    {
        if (inputValue != 0)
        {
            _transform.localScale = new Vector3(Mathf.Sign(inputValue), 1, 1);

            if (inputValue > 0)
                facingRight = true;
            else
                facingRight = false;
        }
    }

    private void Jump()
    {
        if (active == false)
            return;

        OnJump?.Invoke();

        coyoteTimerCounter = -1;
        jumpBufferCounter = -1;

        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
           
        state = PlayerState.JUMPINGTOAIR;       
    }

    public void Jump(float force)
    {
        if (active == false)
            return;

        coyoteTimerCounter = -1;
        jumpBufferCounter = -1;

        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);        

        state = PlayerState.JUMPINGTOAIR;
    }

    private void CalculateMovement(float speedReference,float accelerationReference, float deccelerationReference)
    {
        if (active == false)
            return;

        float movementValue = 0;

        float targetSpeed = speedReference *  Sign(inputValue.x);

        float diff = targetSpeed - rb.velocity.x;

        float accelValue = 0;

        if (GetAbsolutyValue(targetSpeed) > 0.01f)
            accelValue = accelerationReference;
        else
            accelValue = deccelerationReference;

        movementValue = diff * accelValue;

        rb.AddForce(Vector2.right * movementValue);
    }
   
    private bool OnGround(out Collider2D result)
    {

        Collider2D[] hits = new Collider2D[1];

        if (Physics2D.OverlapBoxNonAlloc(startPoint.position, size, angle, hits, layerMask) > 0)
        {
            result = hits[0];

            return true;
        }
        else
        {
            result = null;

            return false;
        }
    }

    private bool OnGround()
    {

        Collider2D[] hits = new Collider2D[1];

        if (Physics2D.OverlapBoxNonAlloc(startPoint.position, size, angle, hits, layerMask) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public PlayerState GetPlayerState()
    {
        return state;
    }

    public void DisableMovementAndFreeze()
    {
        active = false;

        rb.velocity = Vector2.zero;

        rb.isKinematic = true;

        state = PlayerState.ONGROUND;

        jumpBufferCounter = -1;
        coyoteTimerCounter = -1;

        OnNewInputValue?.Invoke(0);
    }

    public void DisableMovement()
    {
        active = false;

        state = PlayerState.ONGROUND;

        rb.velocity = Vector2.zero;

        jumpBufferCounter = -1;
        coyoteTimerCounter = -1;
    }

    public void DisableMovement(bool setOnGroundState)
    {
        active = false;

        if(setOnGroundState)
            state = PlayerState.ONGROUND;

        jumpBufferCounter = -1;
        coyoteTimerCounter = -1;
    }

    public void EnableMovement()
    {
        state = PlayerState.ONGROUND;

        rb.isKinematic = false;

        rb.gravityScale = defaltGravityScale;

        active = true;

        print("Input:" + inputValue);
        if (inputValue == Vector2.zero)
            OnNewInputValue?.Invoke(0);
        else
            OnNewInputValue?.Invoke(1);

        //OnNewInputValue?.Invoke(0);
    }

    public bool IsFacingRight()
    {
        return facingRight; 
    }

    public void Disable()
    {
        DisableMovementAndFreeze();
    }

    public void Enable()
    {
        EnableMovement();
    }

    private float Sign(float reference)
    {
        if (reference > 0)
            return 1;
        else if (reference < 0)
            return -1;
        else
            return 0;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(startPoint.position, size);
        }
    }

    private float GetAbsolutyValue(float value)
    {
        if (value < 0)
            return value * -1;
        else
            return value;
    }

}

public enum PlayerState
{
    ONGROUND,
    JUMPING,
    JUMPINGTOAIR,
    ONAIR
}
