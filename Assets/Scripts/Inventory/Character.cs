using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : Inventory{

    private static Character instance;
    public static Character Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("CharacterPanel").GetComponent<Character>();
            }
            return instance;
        }
    }

    private Text propertyText;
    private Player player;

    public override void Start()
    {
        base.Start(); // 保留父类的方法调用
        propertyText = transform.FindChild("PropertyPanel/Text").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        UpdateProertyText();
        Hide();
    }

    /// <summary>
    /// 更新属性面板
    /// </summary>
    private void UpdateProertyText()
    {
        int strength = 0, intelligence = 0, agility = 0, stamina = 0, damage = 0;

        // 遍历当前装备栏提供的属性值
        foreach (EquipmentSlot slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                Item item = slot.transform.GetChild(0).GetComponent<ItemUI>().Item;
                if (item is Equipment)
                {
                    Equipment e = (Equipment)item;
                    strength += e.Strength;
                    intelligence += e.Intelligence;
                    agility += e.Agility;
                    stamina += e.Stamina;
                }
                else if (item is Weapon)
                {
                    damage += ((Weapon)item).Damage;
                }
            }
        }

        // 属性值 = 基础属性 + 装备提供的属性
        strength += player.BasicStrength;
        intelligence += player.BasicIntelligence;
        agility += player.BasicAgility;
        stamina += player.BasicStamina;
        damage += player.BasicDamage;

        // 更新UI
        string text = string.Format("力量：{0}\n智力：{1}\n敏捷：{2}\n体力：{3}\n攻击力：{4}\n", strength, intelligence, agility, stamina, damage);
        propertyText.text = text;
    }

    /// <summary>
    /// 鼠标右键，穿上装备
    /// </summary>
    /// <param name="item"></param>
    public void PutOn(Item item)
    {
        Item itemTemp = null; // 用于做交换
        // 遍历所有装备槽，找到合适的格子
        foreach (Slot slot in slotList)
        {
            EquipmentSlot equipmentSlot = slot as EquipmentSlot;
            if (equipmentSlot.IsItemMatchSlotType(item))
            {
                if (equipmentSlot.transform.childCount > 0) // 格子里已经有装备了
                {
                    ItemUI currentItemUI = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                    // 记录交换前格子里的装备
                    itemTemp = currentItemUI.Item;
                    // 变更格子里的装备
                    currentItemUI.SetItem(item, 1);
                    // 换下来的装备放回到背包里
                    Knapsack.Instance.StoreItem(itemTemp);
                }
                else
                {
                    equipmentSlot.StoreItem(item);
                }
                break;
            }
        }

        UpdateProertyText();
    }

    /// <summary>
    /// 鼠标右键，卸下装备
    /// </summary>
    /// <param name="item"></param>
    public void PutOff(Item item)
    {
        // 换下来的装备放回到背包里
        Knapsack.Instance.StoreItem(item);
        UpdateProertyText();
    }


}
