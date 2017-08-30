using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 管理所有的物品槽
/// </summary>
public class Inventory : MonoBehaviour {

    private Slot[] slotList;

	// Use this for initialization
    public virtual void Start () {
        slotList = GetComponentsInChildren<Slot>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    /// <summary>
    /// 存储物品到格子里
    /// </summary>
    /// <param name="id">要存储的物品的ID</param>
    /// <returns>操作成功返回True；否则False，如格子已满</returns>
    public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemById(id);
        return StoreItem(item);
    }


    /// <summary>
    /// 存储物品到格子里
    /// </summary>
    /// <param name="item">要存储的物品</param>
    /// <returns>操作成功返回True；否则False，如格子已满</returns>
    public bool StoreItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("要存储的物品的ID不存在！");
            return false;
        }

        if (item.Capacity == 1) // 该种类物品只允许单格最大数量为1，就在新的格子里存储
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.LogWarning("没有空的物品槽了！");
                return false;
            }
            else
            {
                // 把物品存到空的物品槽
                slot.StoreItem(item);
            }

        }
        else // 可以存储多个同类型的物品，数量累加
        {
            Slot slot = FindSameTypeSlot(item);
            if (slot != null) // 找到同类型还未满的格子，存到该格子里
            {
                slot.StoreItem(item);
            }
            else // 没找到，就找一个空的格子，存物品
            {
                Slot emptySlot = FindEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.StoreItem(item);
                }
                else
                {
                    Debug.LogWarning("已经没有空格子保存物品了！");
                    return false;
                }
            }
        }

        return true;
    }


    /// <summary>
    /// 找到一个空的物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindEmptySlot()
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }


    /// <summary>
    /// 找到存储了与指定物品类型一致的物品的物品槽
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private Slot FindSameTypeSlot(Item item)
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount >= 1 && slot.GetItemType() == item.Type && slot.IsFilled() == false)
            {
                return slot;
            }
        }
        return null;
    }

}
