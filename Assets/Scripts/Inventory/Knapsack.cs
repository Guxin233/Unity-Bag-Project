using UnityEngine;
using System.Collections;

public class Knapsack : Inventory {

    private static Knapsack instance;
    public static Knapsack Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("KnapsackPanel").GetComponent<Knapsack>();
            }
            return instance;
        }
    }

}
