using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOutline : MonoBehaviour
{
    [SerializeField] private InteractionEvents interactionEvents;

    [SerializeField] private Material objectMaterial;

    [SerializeField] private bool useDefaltColor = true;

    [SerializeField] private Color outlineCustomColor;

    [SerializeField] private bool disableOutLineOnClick;

    [Header("Outline Values")]
    [Space(5)]
    [Range(0,10)]
    [SerializeField] private float onMouseEnterValue;
    [SerializeField] private bool mouseEnterVisisble=true;
    [Range(0, 10)]
    [SerializeField] private float onMouseExitValue;
    [SerializeField] private bool mouseExitVisible = false;

    private void Awake()
    {
        if (objectMaterial == null)
            objectMaterial = GetComponent<Renderer>().material;

        if (interactionEvents == null)
            interactionEvents = GetComponent<InteractionEvents>();

        interactionEvents.OnClickEvent += InteractionEvents_OnClick;
        interactionEvents.OnEnterEvent += InteractionEvents_OnEnter;
        interactionEvents.OnExitEvent += InteractionEvents_OnExit;

        if (useDefaltColor == false)
            objectMaterial.SetColor("_OutlineColor", outlineCustomColor);

        if (mouseExitVisible)
            objectMaterial.SetFloat("_Visibility", 1);
        else
            objectMaterial.SetFloat("_Visibility", 0);

    }

    private void InteractionEvents_OnClick()
    {
        if (disableOutLineOnClick)
        {
            objectMaterial.SetFloat("_OutlineValue", onMouseExitValue);

            if (mouseExitVisible)
                objectMaterial.SetFloat("_Visibility", 1);
            else
                objectMaterial.SetFloat("_Visibility", 0);
        }
    }

    private void InteractionEvents_OnEnter()
    {
        objectMaterial.SetFloat("_OutlineValue", onMouseEnterValue);

        if (mouseEnterVisisble)
            objectMaterial.SetFloat("_Visibility", 1);
        else
            objectMaterial.SetFloat("_Visibility", 0);
    }


    private void InteractionEvents_OnExit()
    {
        objectMaterial.SetFloat("_OutlineValue", onMouseExitValue);

        if (mouseExitVisible)
            objectMaterial.SetFloat("_Visibility", 1);
        else
            objectMaterial.SetFloat("_Visibility", 0);
    }

   
  
}
