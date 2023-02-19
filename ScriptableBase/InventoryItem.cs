using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Assets/New Inventory Item")]
public class InventoryItem : ScriptableObject
{   
    public Sprite itemSprite;

    public AudioClip onTakedSound;

    public string inventoryTag = "Inventory";

}

//public enum ItemType
//{
//    GENERIC,
//    KEY
//}