using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlidePuzzle_Visuals : MonoBehaviour
{

    [SerializeField] private SlidePuzzleManager slidePuzzleManager;

    private RaycastHit2D[] results = new RaycastHit2D[1];

    private Camera cameraReference;

    private LayerMask layerMask;

    private IBlockVisual currentBlockVisual=null;

    private bool started = false;

    private void Awake()
    {
        if (slidePuzzleManager == null)
            slidePuzzleManager = GetComponent<SlidePuzzleManager>();

        slidePuzzleManager.OnMoveBlock += SlidePuzzleManager_OnMoveBlock;
    }

    private void Start()
    {
        cameraReference = slidePuzzleManager.GetCamera();
        layerMask = slidePuzzleManager.GetLayerMask();
    }

    private void OnDisable()
    {
        if (currentBlockVisual != null)
        {
            currentBlockVisual.OnExit();

            currentBlockVisual = null;
        }
    }

    private void SlidePuzzleManager_OnMoveBlock()
    {
        if (currentBlockVisual != null)
        {
            currentBlockVisual.OnClick();

            currentBlockVisual = null;
        }
    }

    void Update()
    {
        if (slidePuzzleManager.IsActive() && Time.timeScale>0)
        {
            Ray ray = cameraReference.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics2D.RaycastNonAlloc(ray.origin, ray.direction, results, Mathf.Infinity, layerMask, 0) > 0)
            {
                IBlockVisual blockVisual = results[0].collider.GetComponent<IBlockVisual>();

                if (blockVisual != null)
                {
                    if (currentBlockVisual == null)
                    {
                        currentBlockVisual = blockVisual;
                        currentBlockVisual.OnEnter();
                    }
                    else
                    {
                        if (currentBlockVisual == blockVisual)
                        {
                            return;
                        }
                        else
                        {
                            currentBlockVisual.OnExit();

                            currentBlockVisual = blockVisual;

                            currentBlockVisual.OnEnter();
                        }
                    }
                }
            }
            else
            {
                if (currentBlockVisual != null)
                {
                    currentBlockVisual.OnExit();

                    currentBlockVisual = null;
                }
            }
        }
        else
        {
            if (currentBlockVisual != null)
            {
                currentBlockVisual.OnExit();

                currentBlockVisual = null;
            }
        }
    }
}
