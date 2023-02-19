using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextScreenIPointAndClick : MonoBehaviour
{
    [SerializeField] private TextScreen textScreen;

    [SerializeField] private bool disablePointAndClickOnOpen = true;

    [SerializeField] private bool enablePointAndClickOnClose= true;

    private PointAndClick pointAndClick;
    private PointAndClickInventoryVisual pointAndClickInventoryVisual;

    private bool pointAndClickEnabled = true;

    private void Awake()
    {
        if (textScreen == null)
            textScreen = FindObjectOfType<TextScreen>();

        pointAndClick = FindObjectOfType<PointAndClick>();
        pointAndClickInventoryVisual = FindObjectOfType<PointAndClickInventoryVisual>();

        textScreen.OnOpenTextScrenEvent += TextScreen_OnOpenTextScreenEvent;
        textScreen.OnCloseTextScreenEvent += TextScreen_OnCloseTextScreenEvent;
    }

    private void TextScreen_OnOpenTextScreenEvent()
    {
        if (pointAndClickEnabled && disablePointAndClickOnOpen)
        {
            if (pointAndClick != null)
                pointAndClick.SetDetectionActive(false);

            if (pointAndClickInventoryVisual)
                pointAndClickInventoryVisual.SetDetectionActive(false);

            pointAndClickEnabled = false;
        }

    }

    private void TextScreen_OnCloseTextScreenEvent()
    {
        if (enablePointAndClickOnClose)
        {
            if (pointAndClick != null)
                pointAndClick.SetDetectionActive(true);

            if (pointAndClickInventoryVisual)
                pointAndClickInventoryVisual.SetDetectionActive(true);

            pointAndClickEnabled = true;
        }
    }
}
