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

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string weaponType = null;
        switch (WpType)
        {
            case WeaponType.OffHand:
                weaponType = "副手武器";
                break;
            case WeaponType.MainHand:
                weaponType = "主手武器";
                break;
            default:
                break;
        }

        string newText = string.Format("{0}\n武器类型：{0}\n 攻击力：{1} \n", text, weaponType, Damage);

        return newText;
    }
}
