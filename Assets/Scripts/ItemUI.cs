using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    public Item Item { get; set; }
    public int Amount { get; set; }

    private Image itemImage;
    private Text amountText;

    private void Start()
    {
        itemImage = GetComponent<Image>();
        amountText = GetComponentInChildren<Text>();
    }

    public void SetItem(Item item , int amount = 1)
    {
        this.Item = item;
        this.Amount = amount;
    }

    public void AddAmount(int amount = 1)
    {
        this.Amount += amount;
    }
}
