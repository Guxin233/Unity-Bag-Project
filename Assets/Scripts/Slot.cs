using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

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
}
