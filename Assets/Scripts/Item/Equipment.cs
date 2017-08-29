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
        Ring,       // 戒指
        Leg,        // 腿部
        Bracer,     // 护腕
        Boots,      // 靴子
        Trinket,    // 视频
        Shoulder,   // 肩部
        Belt,       // 腰带
        OffHand,    // 副手
    }

}
