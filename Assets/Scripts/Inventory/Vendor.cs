using UnityEngine;
using System.Collections;
using System;

public class Vendor : Inventory {

    private static Vendor instance;
    public static Vendor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("VendorPanel").GetComponent<Vendor>();
            }
            return instance;
        }
    }

    public int[] itemIdArray; // 小贩售卖的物品
    private Player player;

    public override void Start()
    {
        base.Start();
        InitShop();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Hide();
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

    /// <summary>
    /// 玩家购买物品
    /// </summary>
    /// <param name="item"></param>
    public void BuyItem(Item item)
    {
        bool isSuccess = player.ConsumeCoin(item.BuyPrice);
        if (isSuccess)
        {
            // 物品加入到背包中
            Knapsack.Instance.StoreItem(item);
        }
    }

    /// <summary>
    /// 玩家出售手上的物品。
    /// 按下Ctrl时，只出售一个。否则出售全部数量
    /// </summary>
    public void SellItem()
    {
        int sellAmount; // 出售的数量
        if (Input.GetKey(KeyCode.LeftControl))
        {
            sellAmount = 1;
        }
        else
        {
            sellAmount = InventoryManager.Instance.PickedItem.Amount;
        }

        // 售价 = 单价 * 数量
        int coinAmount = InventoryManager.Instance.PickedItem.Item.SellPrice * sellAmount;
        // 获得金币
        player.EarnCoin(coinAmount);
        // 物品持有数减少
        InventoryManager.Instance.RemoveItem(sellAmount);
    }
    
}
