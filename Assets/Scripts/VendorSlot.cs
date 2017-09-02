using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VendorSlot : Slot {

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right // 右键买东西
            && !InventoryManager.Instance.IsPickedItem) // 手上没有物品，才能购买
        {
            if (transform.childCount > 0) // 当前格子里有物品，才能购买
            {
                Item currentItem = transform.GetChild(0).GetComponent<ItemUI>().Item;
                transform.parent.parent.SendMessage("BuyItem", currentItem);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left  // 左键卖东西
            && InventoryManager.Instance.IsPickedItem) // 手上有物品，才能卖出
        {
            transform.parent.parent.SendMessage("SellItem");
        }
    }

}
