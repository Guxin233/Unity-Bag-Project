using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 锻造系统
/// </summary>
public class Forge : Inventory {

    private List<Formula> formulaList;

    public override void Start()
    {
        base.Start();
        ParseFormulaJson();
    }

    private void ParseFormulaJson()
    {
        formulaList = new List<Formula>();
        TextAsset textAsset = Resources.Load<TextAsset>("Formula");
        string formulaJson = textAsset.text; // 锻造配方的Json数据

        JSONObject j = new JSONObject(formulaJson);
        foreach (JSONObject temp in j.list)
        {
            int item1ID = (int)temp["item1ID"].n;
            int item1Amount = (int)temp["item1Amount"].n;
            int item2ID = (int)temp["item2ID"].n;
            int item2Amount = (int)temp["item2Amount"].n;
            int resID = (int)temp["ResID"].n;

            Formula formula = new Formula(item1ID, item1Amount, item2ID, item2Amount, resID);
            formulaList.Add(formula);
        }
    }

    /// <summary>
    /// 锻造道具
    /// </summary>
    public void ForgeItem()
    {
        // 当前锻造槽中的物品的ID
        List<int> ownedMaterialIDList = new List<int>();
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI currentItemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                for (int i = 0; i < currentItemUI.Amount; i++)
                {
                    ownedMaterialIDList.Add(currentItemUI.Item.ID); // 当前格子里物品数量为N，则添加N个该ID到列表中
                }
            }
        }

        // 判断以当前的素材组合，能匹配到哪种合成公式
        foreach (Formula formula in formulaList)
        {
            if (formula.Match(ownedMaterialIDList))
            {
                // 背包中出现合成的新物品
                Knapsack.Instance.StoreItem(formula.ResID);
                // 素材从锻造槽中移除
                foreach (int id in formula.NeedIdList)
                {
                    foreach (Slot slot in slotList)
                    {
                        if (slot.transform.childCount > 0)
                        {
                            ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                            if (itemUI.Item.ID == id && itemUI.Amount > 0) // 因为不能保证界面上素材的摆放顺序，所以还需要遍历一次
                            {
                                itemUI.ReduceAmount();
                                if (itemUI.Amount <= 0)
                                {
                                    DestroyImmediate(itemUI.gameObject);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

    }

}
