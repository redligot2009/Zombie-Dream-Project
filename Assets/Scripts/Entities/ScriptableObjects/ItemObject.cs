using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Untitled Item", menuName = "Custom Objects/Item", order = 1)]
[System.Serializable]
public class ItemObject : ScriptableObject {
    public enum PickUpType { ON_TOUCH, ON_PRESS }
    public enum ItemType { HEALTH_PICKUP, ITEM_PICKUP }
    public PickUpType pickUpType;
    public ItemType itemType;
    public Sprite objectSprite;
    public WeaponObject associatedWeapon;
    public int healthRestore = 0;
}