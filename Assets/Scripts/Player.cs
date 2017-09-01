using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        // G键随机得到一个物品，放到背包里
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
    }
}
