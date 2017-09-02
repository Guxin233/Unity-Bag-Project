﻿using UnityEngine;
using System.Collections;

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

    }
}
