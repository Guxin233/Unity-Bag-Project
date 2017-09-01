using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler{

    public GameObject itemPrefab;

    /// <summary>
    /// 把item存放在自身UI下面，作为格子物品的子物体。
    /// 如果当前格子已经有了子物体，就给该子物体Amount++。
    /// 没有就根据item的Prefab实例化一个物体，作为格子物品的子物体。
    /// </summary>
    /// <param name="item"></param>
	public void StoreItem(Item item)
    {
        if (transform.childCount == 0) // 实例化一个子物体，挂到格子下面
        {
            GameObject go = Instantiate(itemPrefab);
            go.transform.SetParent(this.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            go.GetComponent<ItemUI>().SetItem(item);
        }
        else // 格子里已经存有物品了，就给该物体Amount++
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }


    /// <summary>
    /// 获得当前物品槽中，存储的物品的类型
    /// </summary>
    /// <returns></returns>
    public Item.ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }


    /// <summary>
    /// 获得当前物品槽中，存储的物品的ID
    /// </summary>
    /// <returns></returns>
    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.ID;
    }


    /// <summary>
    /// 判断当前格子中存放的物品个数是否已满了
    /// </summary>
    /// <returns></returns>
    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            InventoryManager.Instance.HideToolTip();
        }
    }


    /// <summary>
    /// 鼠标移入格子，如果格子里有物品，就显示提示面板
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<ItemUI>().Item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(toolTipText);
        }
    }

    /// <summary>
    /// 物品槽内，鼠标按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        /*
         * 格子是空的：
         *      1、IsPickedItem == true，则把拖拽的物品放入格子里
         *          1.1、按下Ctrl键，一次只放入一个物品
         *          1.2、没按Ctrl键，一次放完全部物品
         *      2、IsPickedItem == false，不做任何处理
         * 格子不是空的：
         *      1、IsPickedItem == true
         *          1.1、pickedItem.id == 格子里的物品id
         *              1.1.1、按下Ctrl键，一次只放入一个物品
         *                  1.1.1.1、当前格子的剩余容量足够，可以放入物品
         *                  1.1.1.2、当前格子的剩余容量不够，直接返回
         *              1.1.2、没按Ctrl键，一次放完全部物品
         *                  1.1.2.1、当前格子的剩余容量足够，可以完全放下所有物品
         *                  1.1.2.2、当前格子的剩余容量不够，只能放下部分物品
         *          1.2、pickedItem.id ！= 格子里的物品id，则将pickedItem与格子里的物品交换
         *      2、IsPickedItem != true，拾取格子里的物品，变为选中的物品pickedItem
         *          2.1、按下Ctrl键，取出一半物品（向上取整）放到pickedItem上   
         *          2.2、没按Ctrl键，取出所有物品放到pickedItem上   
         */

        if (transform.childCount > 0) // 格子不是空的
        {
            ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
            if (!InventoryManager.Instance.IsPickedItem) // pickedItem为空，拾取格子里的物品，变为选中的物品pickedItem
            {
                if (Input.GetKey(KeyCode.LeftControl)) // 按下Ctrl键，取出一半物品（向上取整）放到pickedItem上   
                {
                    int amountPicked = (currentItemUI.Amount + 1) / 2;
                    InventoryManager.Instance.PickUpItem(currentItemUI.Item, amountPicked);
                    int amountRemained = currentItemUI.Amount - amountPicked;
                    if (amountRemained <= 0) // 格子里是否还有剩余个数
                    {
                        Destroy(currentItemUI.gameObject); // 销毁格子里的物品
                    }
                    else
                    {
                        currentItemUI.SetAmount(amountRemained);
                    }
                }
                else // 没按Ctrl键，取出所有物品放到pickedItem上   
                {
                    InventoryManager.Instance.PickUpItem(currentItemUI.Item, currentItemUI.Amount);
                    Destroy(currentItemUI.gameObject); // 销毁格子里的物品
                }
            }
            else // 格子里有物品，且PickedItem不为空
            {
                if (currentItemUI.Item.ID == InventoryManager.Instance.PickedItem.Item.ID) // 格子里的物品ID == PickedItem.ID
                {
                    if (Input.GetKey(KeyCode.LeftControl)) // 按下Ctrl键，一次只放入一个物品
                    {
                        if (currentItemUI.Item.Capacity > currentItemUI.Amount) // 格子剩余容量足够
                        {
                            currentItemUI.AddAmount(); // 格子里的物品数量+1
                            InventoryManager.Instance.RemoveItem(); // 手上的物品数量-1
                        }
                        else
                        {
                            return;  // 格子剩余容量不够
                        }
                    }
                    else // 没按Ctrl键，一次放完全部物品
                    {
                        if (currentItemUI.Item.Capacity > currentItemUI.Amount) // 格子还有剩余容量
                        {
                            // 当前物品槽剩余的容量
                            int amountRemain = currentItemUI.Item.Capacity - currentItemUI.Amount;
                            if (amountRemain >= InventoryManager.Instance.PickedItem.Amount) // 可以完全放得下
                            {
                                currentItemUI.AddAmount(InventoryManager.Instance.PickedItem.Amount); // 格子里的物品数量+N
                                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount); // 手上的物品数量-N
                            }
                            else // 只能放下部分数量
                            {
                                currentItemUI.AddAmount(amountRemain);  // 格子里的物品数量+N
                                InventoryManager.Instance.RemoveItem(amountRemain); // 手上的物品数量-N
                            }
                        }
                        else
                        {
                            return; // 格子剩余容量不够
                        }
                    }
                }
                else
                {

                }
            }
        }
        else  // 格子是空的
        {
            /*
            1、IsPickedItem == true，则把拖拽的物品放入格子里
                * 1.1、按下Ctrl键，一次只放入一个物品
                * 1.2、没按Ctrl键，一次放完全部物品
            2、IsPickedItem == false，不做任何处理
            */
            if (InventoryManager.Instance.IsPickedItem) // 格子为空，且手上有东西，则东西放入格子里
            {
                if (Input.GetKey(KeyCode.LeftControl)) // 按下Ctrl键，放入一个物品
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item); // 格子里新建一个物品子物体，个数为1
                    InventoryManager.Instance.RemoveItem(); // 手上物品个数-1
                }
                else // 没按Ctrl键，一次放完全部物品
                {
                    for (int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++)
                    {
                        this.StoreItem(InventoryManager.Instance.PickedItem.Item); // 格子里的物品数量+1
                    }
                    InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount); // 手上物品个数-N
                }
            }
            else
            {
                return; // 不做任何处理
            }
        }

    }
}
