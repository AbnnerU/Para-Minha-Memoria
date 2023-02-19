using System;
using UnityEngine;

public class LedgeGrab : MonoBehaviour,IMovementGeneral
{
    [Header("General")]
    [SerializeField] private bool active = true;

    [SerializeField] private bool drawGizmos;

    //[SerializeField] private Animator anim;

    [SerializeField] private Transform _transform;

    [SerializeField] private Movement2D movement;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private string exclusionTag;

    [SerializeField] private Vector2 displacementValue;

    [Header("Ground detection")]
    [SerializeField] private Transform groundDetectionStartPoint;

    [SerializeField] private float groundDetectionSize;

    //[SerializeField] private float groundDetectionAngle;
    
    [Header("Ledge detection")]
    [Space(10)]
    [SerializeField] private Transform LedgeDetectionStartPoint;

    [SerializeField] private Vector2 LedgeDetectionSize;

    //[SerializeField] private float LedgeDetectionAngle;

    public Action OnGrab;

    private InputController inputController;

    private Coroutine ledgeCoroutine = null;

    private bool grabing = false;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        inputController = FindObjectOfType<InputController>();

        if(_transform==null)    
            _transform = GetComponent<Transform>();

    }

    private void Update()
    {
        if (active == false)
            return;

        if (movement.GetPlayerState() == PlayerState.JUMPING || movement.GetPlayerState() == PlayerState.ONAIR || movement.GetPlayerState() == PlayerState.JUMPINGTOAIR)
        {
            if (LedgeCheck()==false && GroundCheck() && grabing==false)
            {
                movement.DisableMovement();

                rb.velocity = Vector3.zero;
                rb.gravityScale = 0;

                grabing = true;

                OnGrab?.Invoke();
            }
        }
    }

    public void ChangePosition()
    {
        if (active)
        {
            _transform.position = new Vector2(_transform.position.x + (displacementValue.x * _transform.localScale.x), _transform.position.y + displacementValue.y);

            grabing = false;

            movement.EnableMovement();
        }
    }

   


    private bool GroundCheck()
    {
        RaycastHit2D[] hits = new RaycastHit2D[2];

        if (Physics2D.RaycastNonAlloc(groundDetectionStartPoint.position, _transform.localScale.x * Vector2.right, hits, groundDetectionSize, layerMask, 0) > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null)
                    continue;

                if (hits[i].collider.CompareTag(exclusionTag))
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private bool LedgeCheck()
    {
        Collider2D[] hits = new Collider2D[1];

        if (Physics2D.OverlapBoxNonAlloc(LedgeDetectionStartPoint.position, LedgeDetectionSize, 0, hits, layerMask) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetActive(bool active)
    {
        this.active = active;

        if (active == false)
            grabing = false;
    }

    public void Disable()
    {
        SetActive(false);
    }

    public void Enable()
    {
        SetActive(true);
    }


    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (_transform == null)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundDetectionStartPoint.position, groundDetectionStartPoint.position + ( _transform.localScale.x * Vector3.right * groundDetectionSize));

            Gizmos.color = Color.red;
            Gizmos.DrawCube(LedgeDetectionStartPoint.position, LedgeDetectionSize);
        }
    }

   
}
