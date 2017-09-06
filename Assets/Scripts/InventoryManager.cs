using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class InventoryManager : MonoBehaviour {

    #region 单例模式
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
    #endregion

    #region ToolTip
    private ToolTip toolTip;
    private bool isToolTipShow = false;
    private Canvas canvas;
    private Vector2 toolTipPositionOffset = new Vector2(10, -10);
    #endregion

    #region PickedItem
    private bool isPickedItem = false;
    /// <summary>
    /// 是否鼠标选中了任一物品，是否正在拾取物品
    /// </summary>
    public bool IsPickedItem
    {
        get { return isPickedItem; }
        set { isPickedItem = value; }
    }

    private ItemUI pickedItem;  // 鼠标选中的物品
    public ItemUI PickedItem
    {
        get { return pickedItem;  }
    }
    #endregion

    private void Awake()
    {
        ParseItemJson(); // 由于Vendor的Start中需要ParseItemJson先执行，所以该方法不能写在Start中
    }

    private void Start()
    {
        toolTip = GameObject.FindObjectOfType<ToolTip>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        pickedItem.Hide();
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


    private void Update()
    {
        // 拾取的物品跟随鼠标
        if (IsPickedItem)
        {
            // 相对于Canvas的位置
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            PickedItem.SetLocalPosition(position);
        }
        else if (isToolTipShow) // 控制提示面板的位置跟随鼠标
        {
            // 相对于Canvas的位置
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toolTip.SetPosition(position + toolTipPositionOffset);
        }

        // 丢弃物品的条件：
        // 手上有物品 + 按下鼠标左键 + 鼠标位置不在任何EventSystem object身上（即不在UI身上）
        if (IsPickedItem && Input.GetMouseButton(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false)
        {
            IsPickedItem = false;
            PickedItem.Hide();
        }
    }

    public void ShowToolTip(string content)
    {
        if (IsPickedItem)
        {
            return;
        }
        isToolTipShow = true;
        toolTip.Show(content);
    }

    public void HideToolTip()
    {
        isToolTipShow = false;
        toolTip.Hide();
    }

    /// <summary>
    /// 捡起背包一个格子中的指定数量的物品放到鼠标上。
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount">指定个数，是部分或全部数量</param>
    public void PickUpItem(Item item, int amount)
    {
        PickedItem.SetItem(item, amount);
        PickedItem.Show();

        IsPickedItem = true;
        this.toolTip.Hide();
    }

    /// <summary>
    /// 手上拾取的物品数量-N
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveItem(int amount = 1)
    {
        PickedItem.ReduceAmount(amount);
        if (PickedItem.Amount <= 0) // 手上已经没有物品了
        {
            IsPickedItem = false;
            PickedItem.Hide();
        }
    }


    // 保存所有的面板信息
    public void SaveInventory()
    {
        Knapsack.Instance.SaveInventory();
        Chest.Instance.SaveInventory();
        Character.Instance.SaveInventory();
        //Vendor.Instance.SaveInventory();
        Forge.Instance.SaveInventory();
    }

    // 加载所有的面板信息
    public void LoadInventory()
    {
        Knapsack.Instance.LoadInventory();
        Chest.Instance.LoadInventory();
        Character.Instance.LoadInventory();
        //Vendor.Instance.LoadInventory();
        Forge.Instance.LoadInventory();
    }


}
