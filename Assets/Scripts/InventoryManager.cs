using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
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

    private List<Item> itemList;
    /// <summary>
    /// 解析物品Json
    /// </summary>
    public void ParseItemJson()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Items");
        string itemsJson = textAsset.text;

        List<Item> itemList = JsonConvert.DeserializeObject<List<Item>>(itemsJson);
       
    }

}
