using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        // 按G键随机得到一个物品，放到背包里
        if (Input.GetKeyDown(KeyCode.G))
        {
            int id = Random.Range(1, 3); // 含小不含大
            Knapsack.Instance.StoreItem(id);
        }
	}
}
