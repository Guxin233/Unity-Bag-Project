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

    private float targetScale = 1.0f;
    private Vector3 animScale = new Vector3(1.4f, 1.4f, 1.4f); // 缩放动画，缩放的大小
    public float smoothing = 4.0f; // 缩放动画的执行速度

    private void Update()
    {
        if (transform.localScale.x != targetScale)
        {
            // 缩放动画
            float scale = Mathf.Lerp(transform.localScale.x, targetScale, Time.deltaTime * smoothing);
            transform.localScale = new Vector3(scale, scale, scale);
            if (Mathf.Abs(transform.localScale.x - targetScale) < 0.02f)
            {
                transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            }
        }
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
        // 放大动画
        transform.localScale = animScale;
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
        // 放大动画
        transform.localScale = animScale;
    }

    public void SetAmount(int amount)
    {
        // 更新数据
        this.Amount = amount;
        // 更新UI
        if (Item.Capacity > 1)
            AmountText.text = Amount.ToString();
        else
            AmountText.text = "";
        // 放大动画
        transform.localScale = animScale;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 相对于Canvas下的位置
    /// </summary>
    /// <param name="position"></param>
    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

}
