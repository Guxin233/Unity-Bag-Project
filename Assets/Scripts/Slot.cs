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
         *              1.1.2、没按Ctrl键，一次放完全部物品
         *                  1.1.2.1、格子里剩余数量足够，可以完全放下所有物品
         *                  1.1.2.2、格子里还有剩余不够，只能放下部分物品
         *          1.2、pickedItem.id ！= 格子里的物品id，则将pickedItem与格子里的物品交换
         *      2、IsPickedItem != true，拾取格子里的物品，变为选中的物品pickedItem
         *          2.1、按下Ctrl键，取出一半物品（向上取整）放到pickedItem上   
         *          2.2、没按Ctrl键，取出所有物品放到pickedItem上   
         */

        if (transform.childCount > 0) // 格子不是空的
        {
            ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
            if (InventoryManager.Instance.IsPickedItem == false) // pickedItem为空，拾取格子里的物品，变为选中的物品pickedItem
            {
                if (Input.GetKey(KeyCode.LeftControl)) // 按下Ctrl键，取出一半物品（向上取整）放到pickedItem上   
                {

                }
                else // 没按Ctrl键，取出所有物品放到pickedItem上   
                {
                    InventoryManager.Instance.PickedItem.SetItemUI(currentItemUI);
                    InventoryManager.Instance.IsPickedItem = true;
                    Destroy(currentItemUI.gameObject); // 销毁格子里的物品
                }
            }
        }
        else  // 格子是空的
        {

        }

    }
}
