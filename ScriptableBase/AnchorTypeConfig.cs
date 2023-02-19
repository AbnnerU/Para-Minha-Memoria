using System;

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnchorTypeConfig", menuName = "Assets/AnchorsTypeValue")]
public class AnchorTypeConfig : ScriptableObject
{
    public List<AnchorConfig> anchorConfigs;
    public List<AnchorStretchConfig> anchorStretchConfigs;
}

[Serializable]
public struct AnchorConfig
{
    public Anchortype name;
    public Vector2 anchorMin;
    public Vector2 anchorMax;
}

[Serializable]
public struct AnchorStretchConfig
{
    public AnchorTypeStretch name;
    public Vector2 anchorMin;
    public Vector2 anchorMax;
}

public enum Anchortype
{
    TOP_LEFT,
    TOP_CENTER,
    TOP_RIGHT,
    CENTER_LEFT,
    CENTER,
    CENTER_RIGHT,
    BOTTOM_LEFT,
    BOTTOM_CENTER,
    BOTTOM_RIGHT,
    NONE
    
}

public enum AnchorTypeStretch
{
    TOP_STRETCH,
    CENTER_STRETCH,
    BOTTOM_STRETCH,
    NONE
}

