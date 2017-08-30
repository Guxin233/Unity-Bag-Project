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

    private void Start()
    {
        ParseItemJson();
    }

    private List<Item> itemList = new List<Item>();
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
                    // todo
                    break;
                case Item.ItemType.Weapon:
                    // todo
                    break;
                case Item.ItemType.Material:
                    // todo
                    break;
                default:
                    break;
            }

            Debug.Log("item.id = " + item.ID + " , consumable.hp = " + ((Consumable)item).HP);
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
}
