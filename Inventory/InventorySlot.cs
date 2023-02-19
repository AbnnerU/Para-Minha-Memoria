using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private bool isEmpty=true;

    [SerializeField] private Image imageComponent;

    [SerializeField] private InventoryItem itemOnSlot;

    [SerializeField] private Vector3 defaltPosition;

    public Action<Vector3,InventorySlot> OnStartDrag;

    public Action<Vector3,InventorySlot> OnEndDrag;

    private Transform _transform;

    private RectTransform _rectTransform;

    private void Awake()
    {
        if (imageComponent == null)
            imageComponent = GetComponent<Image>();

        imageComponent.preserveAspect = true;

        _transform = transform;

        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);

        print("Start drag "+gameObject.name);

        OnStartDrag?.Invoke(eventData.position, this);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);

        print("End drag " + gameObject.name);

        OnEndDrag?.Invoke(eventData.position, this);
    }

    public void SetUpDefaltPosition()
    {
        defaltPosition = _rectTransform.anchoredPosition;
    }

    public void SetDefaltPosition(Vector3 positionReference)
    {
        defaltPosition = positionReference;
    }

    public void SetImageActive(bool isActive)
    {
        imageComponent.enabled = isActive;
    }

    public void SetIsEmpty(bool isEmpty)
    {
        this.isEmpty = isEmpty;

        if (isEmpty)
            RemoveItem();
    }

    public bool IsThisSlotEmpty()
    {
        return isEmpty;
    }

    public void AddItem(InventoryItem item)
    {
        itemOnSlot = item;

        imageComponent.sprite = item.itemSprite;

        isEmpty = false;
    }

    public Transform GetTransform()
    {
        return _transform;
    }

    public void SetOnDefaltPosition()
    {
        _rectTransform.anchoredPosition = defaltPosition;
    }

    public InventoryItem GetItemOnSlot()
    {
        return itemOnSlot;
    }

    private void RemoveItem()
    {
        itemOnSlot = null;
        imageComponent.sprite = null;
    }
}
