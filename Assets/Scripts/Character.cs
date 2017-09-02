using UnityEngine;
using System.Collections;

public class Character : Inventory{

    private static Character instance;
    public static Character Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("CharacterPanel").GetComponent<Character>();
            }
            return instance;
        }
    }


    /// <summary>
    /// 穿上装备
    /// </summary>
    /// <param name="item"></param>
    public void PutOn(Item item)
    {
        Item itemTemp = null; // 用于做交换
        // 遍历所有装备槽，找到合适的格子
        foreach (Slot slot in slotList)
        {
            EquipmentSlot equipmentSlot = slot as EquipmentSlot;
            if (equipmentSlot.IsItemMatchSlotType(item))
            {
                if (equipmentSlot.transform.childCount > 0) // 格子里已经有装备了
                {
                    ItemUI currentItemUI = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                    // 记录交换前格子里的装备
                    itemTemp = currentItemUI.Item;
                    // 变更格子里的装备
                    currentItemUI.SetItem(item, 1);
                    // 换下来的装备放回到背包里
                    Knapsack.Instance.StoreItem(itemTemp);
                }
                else
                {
                    equipmentSlot.StoreItem(item);
                }
                break;
            }
        }
    }

    /// <summary>
    /// 卸下装备
    /// </summary>
    /// <param name="item"></param>
    public void PutOff(Item item)
    {
        // 换下来的装备放回到背包里
        Knapsack.Instance.StoreItem(item);
    }

}
