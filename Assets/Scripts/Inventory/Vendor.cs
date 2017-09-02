using UnityEngine;
using System.Collections;
using System;

public class Vendor : Inventory {

    public int[] itemIdArray; // 小贩售卖的物品

    public override void Start()
    {
        base.Start();
        InitShop();
    }

    /// <summary>
    /// 初始化小贩售卖的物品
    /// </summary>
    private void InitShop()
    {
        foreach (int itemId in itemIdArray)
        {
            StoreItem(itemId);
        }
    }
}
