
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PointAndClickInventoryVisual : MonoBehaviour
{
    [SerializeField] private bool active = true;

    [SerializeField] private InventoryGeneral inventory;

    [SerializeField] private Camera cameraReference;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private PointAndClick pointAndClickScript;

    [SerializeField] private InventorySlot[] slotsOrder; 

    [SerializeField] private bool disableEmptySlots=true;

    [SerializeField] private bool disableAllOnStart=true;

    //private Dictionary<InventoryItem, Image> items = new Dictionary<InventoryItem, Image>();

    private RaycastHit2D[] results = new RaycastHit2D[1];

    private IDragAndDropInteractable currentTarget = null;

    private void Awake()
    {
        if (cameraReference == null)
            cameraReference = Camera.main;

        inventory.OnAddNewItem += Inventory_OnAddNewItem;
        inventory.OnRemoveItem += Inventory_OnRemoveItem;
       
    }

    private void Start()
    {
        SetUpSlots();

        if (pointAndClickScript == null)
            pointAndClickScript = FindObjectOfType<PointAndClick>();

        if (disableAllOnStart)
        {
            for (int i = 0; i < slotsOrder.Length; i++)
            {
                slotsOrder[i].SetImageActive(false);
            }
        }
    }

    private void SetUpSlots()
    {
        foreach(InventorySlot i in slotsOrder)
        {
            i.OnStartDrag += ObjectDrag_OnStartDrag;
            i.OnEndDrag += ObjectDrag_OnEndDrag;

            i.SetUpDefaltPosition();
        }

    }

    private void ObjectDrag_OnStartDrag(Vector3 position, InventorySlot slotReference)
    {
        if (active == false)
            return;

        pointAndClickScript.SetDetectionActive(false);

        slotReference.GetTransform().position = position;

        Detection(slotReference);

    }

    private void ObjectDrag_OnEndDrag(Vector3 endPosition, InventorySlot slotReference)
    {
        if (active == false)
            return;

        Ray ray = cameraReference.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics2D.RaycastNonAlloc(ray.origin, ray.direction, results, Mathf.Infinity, layerMask, 0) > 0)
        {
            IDragAndDropInteractable dragAndDropInteractable = results[0].transform.gameObject.GetComponent<IDragAndDropInteractable>();

            if (dragAndDropInteractable != null)
            {
                bool consumeItem = false;

                dragAndDropInteractable.OnClick(slotReference.GetItemOnSlot(), out consumeItem);

                if (consumeItem)
                {
                    slotReference.SetIsEmpty(true);

                    if (disableEmptySlots)
                        slotReference.SetImageActive(false);
                }
            }
        }

        currentTarget = null;

        slotReference.SetOnDefaltPosition();

        pointAndClickScript.SetDetectionActive(true);
    }

    private void Detection(InventorySlot slotReference)
    {
        if (active == false)
            return;

        Ray ray = cameraReference.ScreenPointToRay(Mouse.current.position.ReadValue());

        IDragAndDropInteractable interactable = null;

        if (Physics2D.RaycastNonAlloc(ray.origin, ray.direction, results, Mathf.Infinity, layerMask, 0) > 0)
        {
            interactable = results[0].transform.gameObject.GetComponent<IDragAndDropInteractable>();
            print("Entrou aqui");
            if (interactable != null)
            {
                if (currentTarget == null)
                {
                    currentTarget = interactable;
                    currentTarget.OnEnter(slotReference.GetItemOnSlot());
                }
                else
                {
                    if (currentTarget == interactable)
                    {
                        return;
                    }
                    else
                    {
                        currentTarget.OnExit(slotReference.GetItemOnSlot());

                        currentTarget = interactable;

                        currentTarget.OnEnter(slotReference.GetItemOnSlot());
                    }
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.OnExit(slotReference.GetItemOnSlot());

                    currentTarget = null;
                }
            }

        }
        else
        {
            if (currentTarget != null)
            {
                currentTarget.OnExit(slotReference.GetItemOnSlot());

                currentTarget = null;
            }
        }
    }

    private void Inventory_OnAddNewItem(InventoryItem newItem)
    {
        if (active == false)
            return;

        for (int i = 0; i < slotsOrder.Length; i++)
        {
            if (slotsOrder[i].IsThisSlotEmpty())
            {
                slotsOrder[i].AddItem(newItem);

                slotsOrder[i].SetImageActive(true);
                break;
            }
        }
    }

    private void Inventory_OnRemoveItem(InventoryItem newItem)
    {
        if (active == false)
            return;

        //if (items.ContainsKey(newItem))
        //{
        //    Image itemImage;

        //    items.TryGetValue(newItem, out itemImage);

        //    itemImage.sprite = null;

        //    if (disableEmptySlots)
        //        itemImage.enabled = false;

        //    items.Remove(newItem);

        //}
        //else
        //{
        //    Debug.Log("Not contain: " + newItem.name);
        //}
    }

    public void SetDetectionActive(bool isActive)
    {
        active = isActive;       

    }
}
