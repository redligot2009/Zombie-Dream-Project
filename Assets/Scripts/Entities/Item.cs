using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Item : MonoBehaviour {

    public ItemObject itemStats;
    SpriteRenderer spriteRenderer;
    bool pressed = false;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        if(itemStats != null)
        {
            if (itemStats.pickUpType == ItemObject.PickUpType.ON_PRESS && pressed ||
                itemStats.pickUpType == ItemObject.PickUpType.ON_TOUCH)
            {
                PlayerControls player = FindObjectOfType<PlayerControls>();
                switch (itemStats.itemType)
                {
                    case ItemObject.ItemType.HEALTH_PICKUP:
                        player.health.Heal(itemStats.healthRestore);
                        break;
                    case ItemObject.ItemType.ITEM_PICKUP:
                        player.currentWeapon = itemStats.associatedWeapon;
                        break;
                }
                Destroy(transform.gameObject);
            }
        }
        else
        {
            Debug.Log("No item stats! Please select one.");
            Destroy(transform.gameObject);
        }
    }

    void Update () {
        pressed = Input.GetKey(KeyCode.C);
        if (itemStats != null)
        {
            spriteRenderer.sprite = itemStats.objectSprite;
        }
    }
}
