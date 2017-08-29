using UnityEngine;
using System.Collections;

/// <summary>
/// 武器
/// </summary>
public class Weapon : Item {

    public int Damage { get; set; }

    public WeaponType WpType { get; set; }

    public Weapon(int id, string name, ItemType type, ItemQuality quality, string description, int capacity, int buyPrice, int sellPrice, string sprite,
        int damage, WeaponType weaponType) : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.Damage = damage;
        this.WpType = weaponType;
    }

    public enum WeaponType
    {
        OffHand,    // 副手武器
        MainHand,   // 主手武器
    }
}
