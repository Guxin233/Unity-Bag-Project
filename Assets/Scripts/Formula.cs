using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 锻造系统的合成配方
/// </summary>
public class Formula {
    // 两个配方
    public int Item1ID { get; private set; }
    public int Item1Amount { get; private set; }
    public int Item2ID { get; private set; }
    public int Item2Amount { get; private set; }

    // 锻造出来的物品
    public int ResID { get; private set; }

    private List<int> needIdList = new List<int>(); // 合成所需要的物品的ID
    public List<int> NeedIdList
    {
        get { return needIdList; }
    }

    public Formula(int item1ID, int item1Amount, int item2ID, int item2Amount, int resID)
    {
        this.Item1ID = item1ID;
        this.Item1Amount = item1Amount;
        this.Item2ID = item2ID;
        this.Item2Amount = item2Amount;
        this.ResID = resID;

        for (int i = 0; i < Item1Amount; i++)
        {
            needIdList.Add(Item1ID);
        }
        for (int i = 0; i < Item2Amount; i++)
        {
            needIdList.Add(Item2ID);
        }
    }

    /// <summary>
    /// 当前锻造槽中的物品组合，是否有匹配的合成配方
    /// </summary>
    /// <param name="idList">提供的物品的ID</param>
    public bool Match(List<int> idList) // 提供的物品的ID
    {
        List<int> tempIdList = new List<int>(idList);
        foreach (int id in needIdList)
        {
            bool isSuccess = tempIdList.Remove(id);
            if (!isSuccess)
            {
                return false;
            }
        }
        return true;
    }

}
