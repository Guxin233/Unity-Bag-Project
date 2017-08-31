using UnityEngine;
using System.Collections;

/// <summary>
/// 装备
/// </summary>
public class Equipment : Item {

    public int Strength { get; set; }       // 力量
    public int Intelligence { get; set; }   // 智力
    public int Agility { get; set; }        // 敏捷
    public int Stamina { get; set; }        // 体力
    public EquipmentType EquipType { get; set; } 

    public Equipment(int id, string name, ItemType type, ItemQuality quality, string description, int capacity, int buyPrice, int sellPrice, 
        int strength, int intelligence, int agility, int stamina, EquipmentType equipType, string sprite) 
        : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.Strength = strength;
        this.Intelligence = intelligence;
        this.Agility = agility;
        this.Stamina = stamina;
        this.EquipType = equipType;
    }

    public enum EquipmentType
    {
        Head,       // 头部
        Neck,       // 脖子
        Chest,      // 胸部
        Ring,       // 戒指
        Leg,        // 腿部
        Bracer,     // 护腕
        Boots,      // 靴子
        Shoulder,   // 肩部
        Belt,       // 腰带
        OffHand,    // 副手
    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string equipType = null;
        switch (EquipType)
        {
            case EquipmentType.Head:
                equipType = "头部";
                break;
            case EquipmentType.Neck:
                equipType = "脖子";
                break;
            case EquipmentType.Chest:
                equipType = "胸甲";
                break;
            case EquipmentType.Ring:
                equipType = "戒指";
                break;
            case EquipmentType.Leg:
                equipType = "腿部";
                break;
            case EquipmentType.Bracer:
                equipType = "护腕";
                break;
            case EquipmentType.Boots:
                equipType = "靴子";
                break;
            case EquipmentType.Shoulder:
                equipType = "肩部";
                break;
            case EquipmentType.Belt:
                equipType = "腰带";
                break;
            case EquipmentType.OffHand:
                equipType = "副手";
                break;
            default:
                break;
        }


        string newText = string.Format("{0}\n装备类型：{1} 力量：{2} \n智力：{3} \n敏捷：{4} \n体力：{5}", text, equipType, Strength, Intelligence, Agility, Stamina);

        return newText;
    }
}
