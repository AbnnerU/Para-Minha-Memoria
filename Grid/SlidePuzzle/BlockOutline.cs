using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOutline : MonoBehaviour, IBlockVisual
{
  

    [SerializeField] private SpriteRenderer outlineSprite;
    [SerializeField] private bool disableSpriteOnClick;




  

    public void OnClick()
    {
        if (disableSpriteOnClick)
            outlineSprite.enabled = false;
    }

    public void OnEnter()
    {
        outlineSprite.enabled = true;
    }

    public void OnExit()
    {
        outlineSprite.enabled = false;
    }
}
