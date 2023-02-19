using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PointAndClick : MonoBehaviour
{
    [SerializeField] private bool active=true;

    [SerializeField] private Camera cameraReference;

    [SerializeField] private LayerMask layerMask;

    private InputController inputController;

    private RaycastHit2D[] results = new RaycastHit2D[1];

    private IInteractable currentTarget = null;

    private void Awake()
    {
        if (cameraReference == null)
            cameraReference = Camera.main;

        inputController = FindObjectOfType<InputController>();

        inputController.OnLeftClickEvent += InputController_OnLeftClickEvent;

    }

    private void Update()
    {
        if (active == false)
            return;

        Ray ray = cameraReference.ScreenPointToRay(Mouse.current.position.ReadValue());

        IInteractable interactable = null;

        if (Physics2D.RaycastNonAlloc(ray.origin, ray.direction, results, Mathf.Infinity, layerMask, 0) > 0)
        {
            interactable = results[0].transform.gameObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (currentTarget == null)
                {
                    currentTarget = interactable;
                    currentTarget.OnEnter();
                }
                else
                {
                    if (currentTarget == interactable)
                    {
                        return;
                    }
                    else
                    {
                        currentTarget.OnExit();

                        currentTarget = interactable;

                        currentTarget.OnEnter();
                    }
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.OnExit();

                    currentTarget = null;
                }
            }

        }
        else
        {
            if (currentTarget != null)
            {
                currentTarget.OnExit();

                currentTarget = null;
            }
        }
    }

    public void SetDetectionActive(bool isActive)
    {
        active = isActive;

        if (isActive == false && currentTarget != null)
        {
            currentTarget.OnExit();
            currentTarget = null;
        }
            
    }

    private void InputController_OnLeftClickEvent()
    {
        if (currentTarget != null)
            currentTarget.OnClick();
    }
}
