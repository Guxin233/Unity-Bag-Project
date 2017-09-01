using UnityEngine;
using System.Collections;

public class Character : Inventory{

    private static Character instance;
    public static Character Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("CharacterPanel").GetComponent<Character>();
            }
            return instance;
        }
    }


    /// <summary>
    /// 穿上装备
    /// </summary>
    /// <param name="item"></param>
    public void PutOn(Item item)
    {

    }

    /// <summary>
    /// 卸下装备
    /// </summary>
    public void PutOff()
    {

    }

}
