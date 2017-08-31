using UnityEngine;
using System.Collections;

/// <summary>
/// 材料
/// </summary>
public class Material : Item
{
    public Material(int id, string name, ItemType type, ItemQuality quality, string description, int capacity, int buyPrice, int sellPrice, string sprite) 
        : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
    }
}
