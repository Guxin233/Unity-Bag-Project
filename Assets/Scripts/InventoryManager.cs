using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class InventoryManager : MonoBehaviour {

    private static InventoryManager instance;
    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            }
            return instance;
        }
    }

    private ToolTip toolTip;
    private bool isToolTipShow = false;
    private Canvas canvas;
    private Vector2 toolTipPositionOffset = new Vector2(10, -10);

    private void Start()
    {
        ParseItemJson();
        toolTip = GameObject.FindObjectOfType<ToolTip>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public List<Item> itemList = new List<Item>();
    /// <summary>
    /// 解析物品Json
    /// </summary>
    public void ParseItemJson()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Items");
        string itemsJson = textAsset.text;

        //List<Item> itemList = JsonConvert.DeserializeObject<List<Item>>(itemsJson);
        JSONObject j = new JSONObject(itemsJson);

        foreach (JSONObject temp in j.list)
        {
            int id = (int)temp["id"].n;
            string name = temp["name"].str;
            string description = temp["description"].str;
            int capacity = (int)temp["capacity"].n;
            int buyPrice = (int)temp["buyPrice"].n;
            int sellPrice = (int)temp["sellPrice"].n;
            string sprite = temp["sprite"].str;
            Item.ItemQuality quality = (Item.ItemQuality)System.Enum.Parse(typeof(Item.ItemQuality), temp["quality"].str);
            Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), temp["type"].str);

            Item item = null;
            switch (type)
            {
                case Item.ItemType.Consumable:
                    int hp = (int)temp["hp"].n;
                    int mp = (int)temp["mp"].n;
                    item = new Consumable(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, hp, mp);
                    break;
                case Item.ItemType.Equipment:
                    int strength = (int)temp["strength"].n;
                    int intelligence = (int)temp["intelligence"].n;
                    int agility = (int)temp["agility"].n;
                    int stamina = (int)temp["stamina"].n;
                    Equipment.EquipmentType equipType = (Equipment.EquipmentType)System.Enum.Parse(typeof(Equipment.EquipmentType), temp["equipType"].str);
                    item = new Equipment(id, name, type, quality, description, capacity, buyPrice, sellPrice, strength, intelligence, agility, stamina, equipType, sprite);
                    break;
                case Item.ItemType.Weapon:
                    int damage = (int)temp["damage"].n;
                    Weapon.WeaponType weaponType = (Weapon.WeaponType)System.Enum.Parse(typeof(Weapon.WeaponType), temp["weaponType"].str);
                    item = new Weapon(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, damage, weaponType);
                    break;
                case Item.ItemType.Material:
                    item = new Material(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite);
                    break;
                default:
                    break;
            }

            itemList.Add(item);
        }
    }

    /// <summary>
    /// 根据ID找到物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item GetItemById(int id)
    {
        foreach (Item item in itemList)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    // 控制提示面板的位置跟随鼠标
    private void Update()
    {
        if (isToolTipShow)
        {
            // 相对于Canvas的位置
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toolTip.SetPosition(position + toolTipPositionOffset);
        }
    }

    public void ShowToolTip(string content)
    {
        isToolTipShow = true;
        toolTip.Show(content);
    }

    public void HideToolTip()
    {
        isToolTipShow = false;
        toolTip.Hide();
    }

}
