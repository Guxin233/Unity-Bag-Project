using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// 角色的装备面板，只能存放装备类型的物品
/// </summary>
public class EquipmentSlot : Slot {

    public Equipment.EquipmentType equipType;   // 当前装备槽能够装备的装备类型
    public Weapon.WeaponType wpType;            // 当前装备槽能够装备的武器类型

    public override void OnPointerDown(PointerEventData eventData)
    {

        // 右键直接卸下装备
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!InventoryManager.Instance.IsPickedItem && transform.childCount > 0)
            {
                // 装备从装备槽移动到背包中
                ItemUI currentitemUI = transform.GetChild(0).GetComponent<ItemUI>();
                Item tempItem = currentitemUI.Item;

                DestroyImmediate(currentitemUI.gameObject);
                transform.parent.SendMessage("PutOff", tempItem);
                InventoryManager.Instance.HideToolTip();
            }
        }


        if (eventData.button != PointerEventData.InputButton.Left) return;
        /*
         * 手上有东西
         *      当前装备槽有装备，判断物品类型能不能交换
         *      当前装备槽无装备，判断物品类型能不能放进装备槽
         * 手上没东西
         *      当前装备槽有装备，捡起装备槽中的装备，放到手上
         *      当前装备槽无装备，不作处理
         */

        bool needUpdateProperty = false; // 是否需要更新能力值
        if (InventoryManager.Instance.IsPickedItem) // 手上有东西
        {
            ItemUI pickedItem = InventoryManager.Instance.PickedItem;
            if (transform.childCount > 0) // 当前装备槽有装备，判断物品类型能不能交换
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>(); // 当前装备槽里的物品
                if (IsItemMatchSlotType(pickedItem.Item))
                {
                    // 交换手上的物品和格子里的物品
                    InventoryManager.Instance.PickedItem.Exchange(currentItemUI);
                    needUpdateProperty = true;
                }
            }
            else // 当前装备槽无装备，判断物品类型能不能放进装备槽
            {
                if (IsItemMatchSlotType(pickedItem.Item))
                {
                    // 手上的装备 --放到--> 格子里
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    // 手上的装备减少（一般来说装备Capacity为1，移除一个就没了）
                    InventoryManager.Instance.RemoveItem(1);
                    needUpdateProperty = true;
                }
            }
        }
        else // 手上没东西
        {
            if (transform.childCount > 0) // 当前装备槽有装备，捡起来放到手上
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                InventoryManager.Instance.PickUpItem(currentItemUI.Item, currentItemUI.Amount);
                Destroy(currentItemUI.gameObject);
                needUpdateProperty = true;
            }
        }

        // 更新角色能力值
        if (needUpdateProperty)
        {
            transform.parent.parent.SendMessage("UpdatePropertyText");
        }

    }

    /// <summary>
    /// 判断一个物品是否放到当前的格子里
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsItemMatchSlotType(Item item)
    {
        if ((item is Equipment && ((Equipment)item).EquipType == this.equipType) ||
            (item is Weapon && ((Weapon)item).WpType == this.wpType))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
