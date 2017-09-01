using System;
using System.Text;

/// <summary>
/// 物品
/// </summary>
[Serializable]
public class Item {

    public int ID { get; set; }
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public ItemQuality Quality { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }   // 最大可持有数量
    public int BuyPrice { get; set; }   // 买入价格
    public int SellPrice { get; set; }  // 卖出价格
    public string Sprite { get; set; }    // 图标


    public Item() // 空物品
    {
        this.ID = -1; 
    }
    public Item(int id, string name, ItemType type, ItemQuality quality, string description, int capacity, int buyPrice, int sellPrice, string sprite)
    {
        this.ID = id;
        this.Name = name;
        this.Type = type;
        this.Quality = quality;
        this.Description = description;
        this.Capacity = capacity;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
        this.Sprite = sprite;
    }


    /// <summary>
    /// 物品类型
    /// </summary>
    public enum ItemType
    {
        Consumable, // 消耗品
        Equipment,  // 装备
        Weapon,     // 武器
        Material,   // 素材
    }

    /// <summary>
    /// 物品品质类型
    /// </summary>
    public enum ItemQuality
    {
        Common,     // 一般的
        Uncommon,   // 不一般的
        Rare,       // 稀有的  
        Epic,       // 史诗的
        Legendary,  // 传说的
        Artifact,   // 远古的
    }

    /// <summary>
    /// 得到提示面板显示的内容
    /// </summary>
    /// <returns></returns>
    public virtual string GetToolTipText()
    {
        StringBuilder sb = new StringBuilder();

        string color = ""; // 不同品质的颜色
        switch (Quality)
        {
            case ItemQuality.Common:
                color = "white";
                break;
            case ItemQuality.Uncommon:
                color = "lime";
                break;
            case ItemQuality.Rare:
                color = "navy";
                break;
            case ItemQuality.Epic:
                color = "magenta";
                break;
            case ItemQuality.Legendary:
                color = "orange";
                break;
            case ItemQuality.Artifact:
                color = "red";
                break;
            default:
                break;
        }

        sb.Append("<color=" + color + ">" + Name + "</color>" + "\n");
        sb.Append("购买价格：" + BuyPrice + " 出售价格：" + SellPrice + "\n");
        sb.Append(Description + "\n");

        return sb.ToString(); 
    }
}
