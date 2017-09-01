using UnityEngine;
using System.Collections;

public class Chest : Inventory {

    private static Chest instance;
    public static Chest Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("ChestPanel").GetComponent<Chest>();
            }
            return instance;
        }
    }

}
