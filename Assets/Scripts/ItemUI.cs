using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    public Item Item { get; set; }
    public int Amount { get; set; }

    private Image itemImage;
    private Image ItemImage
    {
        get
        {
            if (itemImage == null)
            {
                itemImage = GetComponent<Image>();
            }
            return itemImage;
        }
    }

    private Text amountText;
    private Text AmountText
    {
        get
        {
            if (amountText == null)
            {
                amountText = GetComponentInChildren<Text>();
            }
            return amountText;
        }
    }

    void Start()
    {
    }

    // Item实例化之后调用SetItem时，还没来得及调用Start，所以itemImage和amountText要用属性访问器来初始化，不能在Start中初始化
    public void SetItem(Item item , int amount = 1)
    {
        // 更新数据
        this.Item = item;
        this.Amount = amount;
        // 更新UI
        ItemImage.sprite = Resources.Load<Sprite>(item.Sprite); // /Sprites/Items/hp
        if (Item.Capacity > 1)
            AmountText.text = amount.ToString();
        else
            AmountText.text = "";
    }

    public void AddAmount(int amount = 1)
    {
        // 更新数据
        this.Amount += amount;
        // 更新UI
        if (Item.Capacity > 1)
            AmountText.text = Amount.ToString();
        else
            AmountText.text = "";
    }
}
