using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[System.Serializable] public class OnPressKey : UnityEvent { }


public class OnPreesKey : MonoBehaviour
{
    [SerializeField] private bool active;
    [SerializeField] private InputActionReference input;
    public OnPressKey onPressKey;

    private InputController inputController;

    private void Awake()
    {
        inputController = FindObjectOfType<InputController>();

        input.action.Enable();
        input.action.performed += ctx => CallEvent();
    }

    private void CallEvent()
    {      
        if(active)
            onPressKey?.Invoke();        
    }

    public void SetActive(bool setActive)
    {
        active = setActive;
    }
}
