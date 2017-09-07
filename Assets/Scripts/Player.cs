using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    #region Basic Property
    private int basicStrength = 10;
    private int basicIntelligence = 10;
    private int basicAgility = 10;
    private int basicStamina = 10;
    private int basicDamage = 10;

    public int BasicStrength
    {
        get { return this.basicStrength; }
    }
    public int BasicIntelligence
    {
        get { return this.basicIntelligence; }
    }
    public int BasicAgility
    {
        get { return this.basicAgility; }
    }
    public int BasicStamina
    {
        get { return this.basicStamina; }
    }
    public int BasicDamage
    {
        get { return this.basicDamage; }
    }
    #endregion

    private int coinAmount = 100; // 金币
    public int CoinAmount
    {
        get { return coinAmount; }
        set
        {
            coinAmount = value;
            coinText.text = coinAmount.ToString();
        }
    }

    private Text coinText;

    private void Start()
    {
        coinText = GameObject.Find("Coin").GetComponentInChildren<Text>();
        coinText.text = coinAmount.ToString();
    }

    // Update is called once per frame
    void Update () {
        // G键 随机得到一个物品放到背包里
        if (Input.GetKeyDown(KeyCode.G))
        {
            int id = Random.Range(1, 18); // 含小不含大
            Knapsack.Instance.StoreItem(id);
        }

        // T键 背包
        if (Input.GetKeyDown(KeyCode.T))
        {
            Knapsack.Instance.DisplaySwitch();
        }

        // Y键 箱子
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Chest.Instance.DisplaySwitch();
        }

        // U键 角色面板（装备栏）
        if (Input.GetKeyDown(KeyCode.U))
        {
            Character.Instance.DisplaySwitch();
        }

        // I键 商店
        if (Input.GetKeyDown(KeyCode.I))
        {
            Vendor.Instance.DisplaySwitch();
        }

        // O键 锻造面板
        if (Input.GetKeyDown(KeyCode.O))
        {
            Forge.Instance.DisplaySwitch();
        }

    }

    /// <summary>
    /// 消费金币
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>是否付费成功</returns>
    public bool ConsumeCoin(int amount)
    {
        if (coinAmount >= amount)
        {
            // 更新数据
            coinAmount -= amount;
            // 更新UI
            coinText.text = coinAmount.ToString();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获得金币
    /// </summary>
    /// <param name="amount"></param>
    public void EarnCoin(int amount)
    {
        // 更新数据
        coinAmount += amount;
        // 更新UI
        coinText.text = coinAmount.ToString();
    }

}
