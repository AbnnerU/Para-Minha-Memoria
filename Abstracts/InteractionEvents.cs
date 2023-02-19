using System;
using UnityEngine;

public abstract class InteractionEvents : MonoBehaviour
{
    public Action OnClickEvent;
    public Action OnEnterEvent;
    public Action OnExitEvent;
}
