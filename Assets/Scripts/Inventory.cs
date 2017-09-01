using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 管理所有的物品槽
/// </summary>
public class Inventory : MonoBehaviour {

    private Slot[] slotList;

    private float targetAlpha = 1;
    public float smoothing = 4; // 调节透明度变化的速度

    private CanvasGroup canvasGroup;

	// Use this for initialization
    public virtual void Start () {
        slotList = GetComponentsInChildren<Slot>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.1f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
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
            Slot slot = FindSameIdSlot(item);
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
    private Slot FindSameIdSlot(Item item)
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount >= 1 && slot.GetItemId() == item.ID && slot.IsFilled() == false)
            {
                return slot;
            }
        }
        return null;
    }

    public void Show()
    {
        targetAlpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        targetAlpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 改变背包/箱子的显隐状态
    /// </summary>
    public void DisplaySwitch()
    {
        if (targetAlpha == 0)
        {
            this.Show();
        }
        else
        {
            this.Hide();
        }
    }

}
